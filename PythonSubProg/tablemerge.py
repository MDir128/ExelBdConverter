#функция объединения двух таблиц на основе общих данных
def merge_tables(table1, table2):
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
    if flag == False:
        print("There is no shared data")
        return None

    #функция для продолжения нумерации ячеек 
    def id_cont(cell_id):
        #если ячкйка пуста — начинаем с первой ячейки A
        if cell_id == None:
            return 'A'

        symbols = list(cell_id) #для двузначных ID (AA, BB, CC)
        i = len(symbols) - 1
        while i >= 0:
            #если последнее ID не Z, то можно просто брать следующую букву
            if symbols[i] != 'Z':
                symbols[i] = chr(ord(symbols[i]) + 1)
                return ''.join(symbols)
            #если последнее ID не Z, следующее — AA
            else:
                symbols[i] = 'A'
                i -= 1
        return 'A' + ''.join(symbols) 
    
    #функция для переиндексации ячеек в таблице
    def reindex_id(table, start_cell_id):
        if table == None:
            return []
        reindex_table = []
        curr_cell_id = start_cell_id #назначение текущей ячейки

        for cell in table:
            new_cell = [curr_cell_id] + cell[1:] #заменяется буква у ячейки с сохранением всеъ остальных данных
            reindex_table.append(new_cell)
            curr_cell_id = id_cont(curr_cell_id) #получение следующего ID следующей ячейки
        return reindex_table

    #функция для нахождения последней ячейки (по букве)
    def get_maxid(table):
        if table == None:
            return None
        all_id = [cell[0] for cell in table]
        return max(all_id)

    #обработка исключений
    if table1 == None and table2 == None:
        print("Both tables are None")
        return None
    if table1 != None and table2 == None:
        print("Table #2 is None. Return table #1")
        return table1
    if table1 == None and table2 != None:
        print("Table #1 is None. Return table #2")
        return table2


    last_table1_id = get_maxid(table1) #выделение последней ячейки первой таблицы
    start_cell_id = id_cont(last_table1_id)
    reindex_table2 = reindex_id(table2, start_cell_id) #новая переиндексированная вторая таблица

    merged_table = table1 + reindex_table2
    return merged_table


#тест и красивый вывод    
'''
table1 = [
    ['A', 1, "Sasha", 'student'],
    ['B', 1, "Andrey", 'manager'],
    ['E', 1, "Petya", 'worker']]

table2 = [
    ['A', 1, "Pasha", 'student']]          

new_table = merge_tables(table1, table2)
if new_table != None:
    print("Merge table:")
    for t in new_table:
        print(t)
else:
    print("Tables cannot be merged")
'''