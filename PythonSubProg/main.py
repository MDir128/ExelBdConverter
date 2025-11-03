import sys, os
script_dir = os.path.dirname(os.path.abspath(__file__))
sys.path.insert(0, script_dir)
from tabledifffun import *
from XlsArrayer import *
def main():
    ##!!!!
    ## В print() всегда возвращаем flag+'$'+(ответ), если на вход был флаг (а он должен быть ВСЕГДА!)
    ##!!!!
    filename = ""
    while True:
        line = sys.stdin.readline().strip() # чтение строки
        splitedline = line.split("$")
        flag = "none"
        if len(splitedline) == 2:
            flag = splitedline[0]
            line = splitedline[1]
        if flag=='SetFILE': 
            filename = line
        ##elif line == "SAVE!":
        ##    SavedTable = ''
        ##    SavedTable = XlsArrayerOut(filename)
        ##    print ("saved", SavedTable, flush=True)
        ##elif line == "COMPARE!":
        ##    if SavedTable == '':
        ##        print("no saved previous result(nothing to compare)", flush=True)
        ##    else:
        ##        print(tables_differences(SavedTable, XlsArrayerOut(filename), False), flush=True) #настроен на печать вывода 
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
if __name__ == "__main__":
    main()