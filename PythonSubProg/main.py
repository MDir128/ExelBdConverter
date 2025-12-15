import sys, os
script_dir = os.path.dirname(os.path.abspath(__file__))
sys.path.insert(0, script_dir)
from tabledifffun import *
from XlsArrayer import *
from tablemerge import *

Table = ''
filename = ''


def Xlsout():
    global Table
    global filename
    Table = XlsArrayerOut(filename)
    return

def main():

    ##!!!!
    ## В print() всегда возвращаем flag+'$'+(ответ), если на вход был флаг (а он должен быть ВСЕГДА!)
    ##!!!!

    Table2 = '' 
    global filename
    filename1, filename2 = '', ''
    while True:
        line = sys.stdin.readline().strip() # чтение строки
        splitedline = line.split("$")

        flag = line
        if len(splitedline) == 2:
            flag = splitedline[0]
            line = splitedline[1]

        if flag=='SetFILE1': 
            if line != '':
                filename = line
                filename1 = XlsArrayerOut(filename)
                print(flag+"$"+f'done seting {filename} as filename1')
            else:
                print(flag+'$'+ "no path" )
            
            
        elif flag=='SetFILE2': 
            if line != '':
                filename = line
                filename2 = XlsArrayerOut(filename)
                print(flag+"$"+f'done seting {filename} as filename1')
            else:
                print(flag+'$'+ "no path" )

        elif flag == "MERGE!":
            # line содержит путь для сохранения
            save_path = line
    
            err_list = '' 
            if filename1 == '': 
                err_list += 'NO FILENAME1 (try: "SetFILE1)'
            elif filename2 == '': 
                err_list += 'NO FILENAME2 (try: "SetFILE2)'
    
            if err_list != '': 
                print(flag + '$' + 'merge is not allowed, there is no: ' + err_list, flush=True)
            else: 
                print(flag + '$' + 'merging tables', flush=True)
                filename1 = merge_tables(filename1, filename2)
        
            # Автоматически сохраняем результат
            print(flag + '$' + 'saving merged file...', flush=True)
            XlsArrayerOut(filename1, save_path)
            print(flag + '$' + f'Merge ended and saved to {save_path}', flush=True)
            
        elif flag == "COMPARE!":

            err_list = '' 
            if filename1 == '': err_list += 'NO FILENAME1 (try: "SetFILE1)'
            elif filename2 == '': err_list += 'NO FILENAME2 (try: "SetFILE2)'

            if err_list != '':
                print(flag+ '$' + 'compare is not allowed, there is no:', *err_list, flush=True)
            else:
                print(flag+ '$', tables_differences(filename1, filename2, False), flush=True) #настроен на печать вывода 

        elif flag == "SAVE!":
            file_path = line
            print(flag+'$'+ ' Saving file1 ' )
            XlsArrayerIn(filename1, file_path)

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