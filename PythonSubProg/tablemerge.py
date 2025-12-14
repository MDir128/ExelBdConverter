def HatHunter(table):
    Hat = []
    for box in table:
        if box[1] == 1:
            curr_id = box[0]
            for i in range(2, len(box)):
                header_value = box[i]
                if header_value != None:
                    Hat.append([curr_id, header_value]) #исправлена ошибка, где все заголовки получали одинаковую букву в id — "A"
                    curr_id = id_cont(curr_id) #теперь для следующего заголовка будет генерироваться следующая буква, то есть "B"
    return Hat

def HatChecker(hat1, hat2):
    hat1 = HatHunter(hat1)
    hat2 = HatHunter(hat2)
    matches = []
    for i in range(len(hat1)):
        ha=hat1[i]
        a=ha[1]
        for x in range(len(hat2)):
            hb = hat2[x]
            b = hb[1]
            if a == b:
                matches.append([ha[0], hb[0]])
    return matches

#функция объединения двух таблиц на основе общих данных
def merge_tables(table1, table2):
    #проверка на общие элементы
    flag_common = HatChecker(table1, table2)
    if not flag_common:
        raise ValueError("There is no shared data")
        return None

    #проверка исключений
    exception_result = exception_process(table1, table2)
    if exception_result != None:
        return exception_result
    all_headers = select_headers(table1, table2) #сначала нужно получать словарь всех заголовков
    merged_table = create_newtable(table1, table2, all_headers) #потом уже создавать новую таблицу
    return merged_table
   
#функция для выделения всех заголовков в шапке в один словарь
def select_headers(table1, table2):
    headersdict = dict() 
    for table, table_name in [(table1, "Table 1"), (table2, "Table 2")]: #каждая итерация проходится по каждой таблице
        alltable_headers = HatHunter(table) 
        for header_id, header_name in alltable_headers: #теперь каждая итерация по каждому заголовку с его уникальным id
            #если заголовка ещё нет в словаре
            if header_name not in headersdict:
                headersdict[header_name] = {"table1_id": None, "table2_id": None} #пока None, ведь просто создаётся запись — потом None будут заполняться id заголовков с первой таблицы, а потом со второй
            #если такой заголовок уже есть
            if table_name == "Table 1":
                headersdict[header_name]["table1_id"] = header_id
            else:
                headersdict[header_name]["table2_id"] = header_id
    return headersdict #по итогу вернётся словарь, где явно видно, какие заголовки у таблиц общие, а какие нет
        
#функция создания новой мёрджнутой таблицы 
def create_newtable(table1, table2, all_uniq_headers): #принимает на вход словарь заголовков
    new_table = []

    #заполнение массивов уникальных заголовков
    uniq_headers1 = [] 
    uniq_headers2 = []
    common_headers = []
    for header, id in all_uniq_headers.items():
        if id["table1_id"] and id["table2_id"]: #в table1_id и в table2_id хранятся буквенные id
            common_headers.append(header)
        elif id["table1_id"] and not id["table2_id"]:
            uniq_headers1.append(header)
        elif not id["table1_id"] and id["table2_id"]:
            uniq_headers2.append(header)
    all_headers = common_headers + uniq_headers1 + uniq_headers2 #теперь создаётся один общий массив всех заголовков

    #составлене самой шапки
    curr_id = 'A'
    for header in all_headers:
        new_table.append([curr_id, 1, header]) #заполнение новой таблицы шапкой
        curr_id = id_cont(curr_id)

    #заполнение словарей заголовков с их id
    table1_id_header = dict()
    table2_id_header = dict()
    for cell in table1: 
        if cell[1] == 1:
            for i in range(2, len(cell)): #индекс 2, потому что 0 — буквенный id ячейки, а 1 — числовой id ячейки
                header_name = cell[i]
                table1_id_header[header_name] = i
    for cell in table2: 
        if cell[1] == 1:
            for i in range(2, len(cell)):
                header_name = cell[i]
                table2_id_header[header_name] = i

    maxstroka_num_table1 = max(cell[1] for cell in table1) #максимальный номер строки в первой таблице
    maxstroka_num_table2 = max(cell[1] for cell in table2)

    #добавление строк из первой таблицы
    for stroka_num in range(2, maxstroka_num_table1 + 1): #1 итерация для каждой строки (по её номеру)
        #нужно собрать все ячейки строки
        all_cells = []
        for cell in table1: #находим все ячейки первой строки первой таблицы
            if cell[1] == stroka_num:
                all_cells.append(cell)
        string_elements = dict() #словарь для хранения значений строки, то есть заголовок: значение
        for cell in all_cells: #для каждой из ячеек строки
            for i in range(2, len(cell)): #для каждого из значений строки
                for header_name, header_id in table1_id_header.items(): 
                    if header_id == i: #проверка индекса
                        cell_value = cell[i] if i < len(cell) else '' #проверка, чтобы не ввышло из диапазона
                        string_elements[header_name] = cell_value #сохранение в этот словарь
                        break
        #создание ячеек для строк в новой таблице 
        curr_id = 'A'
        for header in all_headers:
            cell_value = string_elements.get(header, '') #получаем значения у всех ключей заголовков в словаре
            new_table.append([curr_id, stroka_num, cell_value]) #теперь всё это формируется в строки и добавляется к уже составленной шапке в новой таблицы
            curr_id = id_cont(curr_id) #генерация id для следующего столбца

    #добавление строк из второй таблицы
    next_stroka_num = maxstroka_num_table1 + 1
    for stroka_num in range(2, maxstroka_num_table2 + 1): #находим все ячейки первой строки второй таблицы
        all_cells = []
        for cell in table2:
            if cell[1] == stroka_num:
                all_cells.append(cell)
        string_elements = dict()
        for cell in all_cells:
            for i in range(2, len(cell)):
                for header_name, header_id in table2_id_header.items():
                    if header_id == i:
                        cell_value = cell[i] if i < len(cell) else '' #проверка, чтобы не ввышло из диапазона
                        string_elements[header_name] = cell_value
                        break
        curr_id = 'A'
        for header in all_headers:
            cell_value = string_elements.get(header, '') 
            new_table.append([curr_id, next_stroka_num, cell_value])
            curr_id = id_cont(curr_id)
        next_stroka_num += 1
    return new_table


#функция для продолжения нумерации ячеек 
def id_cont(cell_id):
    #если ячейка пуста — начинаем с первой ячейки A
    if cell_id == None:
        return 'A'
    '''для определения правильного двузначного и большеидентификатора будет строиться 
    следующая логика: текущий буквенный идентификатор сначала превратится в число, 
    потом это число увеличится и вернётся новый правильный буквенный идентификатор'''
    def id_chislo(cell_id): 
        chislo = 0
        for letter in cell_id: #для каждой буквы в id
            chislo = chislo * 26 + (ord(letter.upper()) - ord('A') + 1) #вычисление номера буквы по латинскому алфавиту
        return chislo

    def chislo_newid(chislo):
        cell_id = ''    
        while chislo > 0:
            ostatok = (chislo - 1) % 26
            chislo = (chislo - 1) // 26
            cell_id = chr(ord('A') + ostatok) + cell_id 
        return cell_id
    curr_chislo = id_chislo(cell_id)
    next_chislo = curr_chislo + 1 #номер следующей ячейки от заданной
    return chislo_newid(next_chislo) #id этой следующей
    

#функция для переиндексации ячеек в таблице
def reindex_id(table, start_cell_id):
    if table == None:
        return []
    reindex_table = [] 
    curr_cell_id = start_cell_id
    for cell in table:
        new_cell = [curr_cell_id] + cell[1:] #заменяется буква у ячейки с сохранением всех остальных данных
        reindex_table.append(new_cell)
        curr_cell_id = id_cont(curr_cell_id)
    return reindex_table


#функция для нахождения последней ячейки (по букве)
def get_maxid(table):
    if table == None:
        return None
    #для правильного определения большего идентификатора
    def id_chislo(id_stroka):
        chislo = 0
        for index, letter in enumerate(reversed(id_stroka)): #перебор по каждому индексу (для веса разряда) и самой букве
            chislo += (ord(letter.upper()) - ord('A') + 1) * (26 ** index) #вычисление номера буквы по латинскому алфавиту и умножение на вес разряда
        return chislo #значение идентификатора (по которому будет сравниваться)
    all_id = [cell[0] for cell in table] #все id ячеек (без чисел)
    maxid_stroka = all_id[0] 
    maxid_chislo = id_chislo(maxid_stroka) 
    for cell_id in all_id[1:]: #начнётся перебор со второго id (так как первый уже обозначен, как максимальный) и будет переобозначаться максимальный id
        cell_value = id_chislo(cell_id)
        if cell_value > maxid_chislo: #переобозначаться максимальный id будет просто в сравнении числовых значений id
            maxid_stroka = cell_id
            maxid_chislo = cell_value
    return maxid_stroka


#функция для обработки исключений
def exception_process(table1, table2):
    if table1 == None and table2 == None:
        raise ValueError("Both tables are None")
        return None
    if table1 != None and table2 == None:
        print("Table #2 is None. Return table #1")
        return table1
    if table1 == None and table2 != None:
        print("Table #1 is None. Return table #2")
        return table2