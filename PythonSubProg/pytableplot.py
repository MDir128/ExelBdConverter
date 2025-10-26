import sys, os
script_dir = os.path.dirname(os.path.abspath(__file__))
sys.path.insert(0, os.path.join(script_dir))

from tabledifffun import *
from XlsArrayer import *
def main():
    filename = ""
    while True:
        line = sys.stdin.readline().strip() # чтение строки
        if line.startswith("FILE="): 
            filename = line[5:]
        elif line == "SAVE!":
            SavedTable = ''
            SavedTable = XlsArrayerOut(filename)
            print ("saved", SavedTable, flush=True)
        elif line == "COMPARE!":
            if SavedTable == '':
                print("no saved previous result(nothing to compare)", flush=True)
            else:
                print(tables_differences(SavedTable, XlsArrayerOut(filename), False), flush=True) #настроен на печать вывода 
        elif line == "CHECK!":
            print (filename, flush=True)
        else:
            print("uncknown command", flush=True)
if __name__ == "__main__":
    main()