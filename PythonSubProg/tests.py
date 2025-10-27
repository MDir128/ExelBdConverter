from tabledifffun import *
from tablemerge import *

#--------------------------------- ТЕСТ ПЕРВОЙ ФУНКЦИИ --------------------------------
print("-" * 45 + "TableDiffunTest" + "-" * 45)
print("                                                       ")
prev_table = [
    ['A', 1, 100, '=B1+10'],
    ['B', 1, 90, ''],
    ['C', 1, 200, '=A1+1'],
    ['E', 1, 3, '=A1*4']]

curr_table = [
    ['A', 1, 150, '=B1+10'],  #изменилось значение
    ['B', 1, 91, '=A1-50'],   #изменилось значение и формула
    #ячейка ['C', 1, 200, '=A1+1'] удалена
    ['D', 1, 50, '=A1+B1'],   #новая ячейка
    #ячейка ['E', 1, 3, '=A1*4'] удалена
    ['F', 3, 50, '=A1+B1'],   #новая ячейка
    ['G', 2, 1, '']]          #новая ячейка

print("**** JSON-data ****")
diff1 = tables_differences(prev_table, curr_table, switch=False)
print("\n==============================================================\n")
print("**** All data ****")
diff2 = tables_differences(prev_table, curr_table, switch=True)
print("\n==============================================================\n")
#--------------------------------- ТЕСТ ВТОРОЙ ФУНКЦИИ --------------------------------

print("-" * 45 + "TableMergeTest" + "-" * 45)
print("                                                       ")
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