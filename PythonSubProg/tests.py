from tabledifffun import *
from tablemerge import *
from tablesort import *

#--------------------------------- ТЕСТ ПЕРВОЙ ФУНКЦИИ --------------------------------
print("-" * 45 + "TableDiffunTest" + "-" * 45)
print("                                                       ")
prev_table = [
    ['A', 1, "Name"],
    ['B', 1, "Age"],
    ['C', 1, "Work"],
    ['D', 1, "Color"],
    ['A', 2, "Andrey"],
    ['B', 2, 18.5],
    ['C', 2, 'manager'],
    ['D', 2, 'red'],
    ['A', 3, "Petya"],
    ['B', 3, 18],
    ['C', 3, 'worker'],
    ['D', 3, 'yellow'],
    ['A', 4, "Sasha"],
    ['B', 4, 19],
    ['C', 4, 'director'],
    ['D', 4, 'purple'],
    ['A', 5, "Pasha"],
    ['B', 5, 20],
    ['C', 5, 'agent'],
    ['D', 5, 'green']]

curr_table = [
    ['A', 1, "Name"],
    ['B', 1, "Age"],
    ['C', 1, "Work"],
    ['A', 3, "Petya"],
    ['B', 3, 18],
    ['C', 3, 'worker'],
    ['A', 4, "Sasha"],
    ['B', 4, 18.5],
    ['C', 4, 'director'],
    ['A', 5, "Pasha"],
    ['B', 5, 20],
    ['C', 5, 'unkown agent'],
    ['A', 6, "Gosha"],
    ['B', 6, 1],
    ['C', 6, 'boss']]          

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
    ['A', 1, "Name"],
    ['B', 1, "Age"],
    ['C', 1, "Work"],
    ['A', 2, "Andrey"],
    ['B', 2, 19],
    ['C', 2, 'manager'],
    ['A', 3, "Petya"],
    ['B', 3, 18],
    ['C', 3, 'worker'],
    ['A', 4, "Sasha"],
    ['B', 4, 19],
    ['C', 4, 'director']]

table2 = [
    ['A', 1, "Name"],
    ['B', 1, "Color"],
    ['C', 1, "Age"],
    ['A', 2, "Gosha"],
    ['B', 2, "Red"],
    ['C', 2, 9],
    ['A', 3, "Vova"],
    ['B', 3, "Black"],
    ['C', 3, 50]]     

new_table = merge_tables(table1, table2)
if new_table != None:
    print("Merge table:")
    for t in new_table:
        print(t)
else:
    print("Tables cannot be merged")

#--------------------------------- ТЕСТ ТРЕТЬЕЙ ФУНКЦИИ --------------------------------
print("-" * 45 + "TableSortTest" + "-" * 45)
print("                                                       ")
table = [
    ['A', 1, "Name"],
    ['B', 1, "Age"],
    ['C', 1, "Work"],
    ['A', 2, "Andrey"],
    ['B', 2, 18.5],
    ['C', 2, 'manager'],
    ['A', 3, "Petya"],
    ['B', 3, 18],
    ['C', 3, 'student'],
    ['A', 4, "Sasha"],
    ['B', 4, 19],
    ['C', 4, 'director'],
    ['A', 5, "Vitya"],
    ['B', 5, None],
    ['C', 5, 'worker'],
    ['A', 6, "Pasha"],
    ['B', 6, 20],
    ['C', 6, 'agent'],
    ['A', 7, "Gosha"],
    ['B', 7, 7],
    ['C', 7, 'doctor'],
    ['A', 8, "Vova"],
    ['B', 8, 50],
    ['C', 8, 'boss'],
    ['A', 9, "Kostya",],
    ['B', 9, 1],
    ['C', 9, None]]
    
new_table1_vozrastanie = sort_table(table, 'Age', mode = True)
if new_table1_vozrastanie != None:
    print("Sorted age_vozrastanie table:")
    for t in new_table1_vozrastanie:
        print(t)
print("-" * 45)
new_table1_ybivanie = sort_table(table, 'Age', mode = False)
if new_table1_ybivanie != None:
    print("Sorted age_ybivanie table:")
    for t in new_table1_ybivanie:
        print(t)
print("-" * 45)
new_table2_vozrastanie = sort_table(table, 'Work', mode = True)
if new_table2_vozrastanie != None:
    print("Sorted work_vozrastanie table:")
    for t in new_table2_vozrastanie:
        print(t)
print("-" * 45)
new_table2_ybivanie = sort_table(table, 'Work', mode = False)
if new_table2_ybivanie != None:
    print("Sorted work_ybivanie table:")
    for t in new_table2_ybivanie:
        print(t)