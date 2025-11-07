#функция для нахождения общих элементов
def common_elements(table1, table2):
    uniq_elems = set() #множество для уникальных элементов
    flag = False
    #проверка на наличие общих элементов в первой таблице
    for cell in table1:
        for i in range(2, len(cell)): #нельзя учитывать имя ячейки
            znach = cell[i]
            if znach != '': #проверка на то, чтобы не было пустых ячеек
                uniq_elems.add(znach)

    #проверка на наличие общих элементов во второй таблице
    for cell in table2:
        for i in range(2, len(cell)): #нельзя учитывать имя ячейки:
            znach = cell[i]
            if znach != '' and znach in uniq_elems:
                flag = True
                break
        break
    #если совпадений не найдено
    return flag


#функция для продолжения нумерации ячеек 
def id_cont(cell_id):
    #если ячкйка пуста — начинаем с первой ячейки A
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
    curr_cell_id = start_cell_id #назначение текущей ячейки
    for cell in table:
        new_cell = [curr_cell_id] + cell[1:] #заменяется буква у ячейки с сохранением всех остальных данных
        reindex_table.append(new_cell)
        curr_cell_id = id_cont(curr_cell_id) #получение следующего id следующей ячейки
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
        print("Both tables are None")
        return None
    if table1 != None and table2 == None:
        print("Table #2 is None. Return table #1")
        return table1
    if table1 == None and table2 != None:
        print("Table #1 is None. Return table #2")
        return table2

#функция объединения двух таблиц на основе общих данных
def merge_tables(table1, table2):
    #проверка на общие элементы
    flag_common = common_elements(table1, table2)
    if flag_common == False:
        print("There is no shared data")
        return None

    #проверка исключений
    exception_result = exception_process(table1, table2)
    if exception_result != None:
        return exception_result

    last_table1_id = get_maxid(table1) #выделение последней ячейки первой таблицы
    start_cell_id = id_cont(last_table1_id)
    reindex_table2 = reindex_id(table2, start_cell_id) #новая переиндексированная вторая таблица

    merged_table = table1 + reindex_table2
    return merged_table
    

    


