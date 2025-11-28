import sys, os
script_dir = os.path.dirname(os.path.abspath(__file__))
sys.path.insert(0, script_dir)
from tabledifffun import *
from XlsArrayer import *
from tablemerge import *

Table = ''
filename = ''

def Xlsout(): # мне было проще переписать это в def чем дважды писать одно и то же, но да я насрал глобальными переменными
    global Table
    global filename
    Table = XlsArrayerOut(filename)
    return

def main():
    ##!!!!
    ## В print() всегда возвращаем flag+'$'+(ответ), если на вход был флаг (а он должен быть ВСЕГДА!)
    ##!!!!
    Merged = ''
    global filename
    while True:
        line = sys.stdin.readline().strip() # чтение строки
        splitedline = line.split("$")
        flag = "none"
        if len(splitedline) == 2:
            flag = splitedline[0]
            line = splitedline[1]
        if flag=='SetFILE': 
            filename = line
        if line == "MERGE!": # def merge_tables(table1, table2):
            err_list = '' 
            if Table == '': err_list += 'NO CURRENT TABLE'
            elif SavedTable == '': err_list += 'NO SAVED TABLE'
            else: 
                print('merging of saved and current tables')
                Merged = merge_tables(Table, SavedTable)
                print('Merge ended', flush=True)
            

            if err_list != '': print('merge is not allowed, there is no:', *err_list, flag+'$'+filename, flush=True)
        # if line == "SAVE!" and :
        #     Xlsout()
        #     SavedTable = Table
        #     print ("saved", SavedTable, flag+'$'+filename, flush=True)
        if line == "COMPARE!":
            if SavedTable == '':
                print("no saved previous result(nothing to compare)",flag+'$'+filename, flush=True)
            else:
                print(tables_differences(SavedTable, XlsArrayerOut(filename), False), flush=True) #настроен на печать вывода 
        elif flag=='Debug' and line == "CHECK!":
            i = 0
            n = 1
            for i in range(1, 10000):
                if i%3==0:
                    n//=3
                else:
                    n*=2
            print (flag+'$'+filename, n, flush=True)
        else:
            print(flag+'$'+"uncknown command", flush=True)
        Table = ''
if __name__ == "__main__":
    main()