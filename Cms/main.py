import pandas as pd
import os

cheker = "C:/Users/user/Desktop/Cms/test.xlsx" 

def outputer(cheker):
    if not os.path.exists(cheker):
        print("file err", cheker)
    else:
        xlsx = pd.read_excel(cheker, dtype=str)#извелчение(нужно менять)
        data = xlsx.fillna('') #как я понял эта строчка делает пустые клетки пустыми строками
        
    Data = []
    for i, tablet in xlsx.iterrows():  # проходим по строкам
        info = []
        for a, value in enumerate(tablet):
            letter = chr(ord('A') + a)  # буква столбца A, B, C...
            number = str(i + 1)         # номер строки Excel
            info.append([letter, number, value, 0])  # формула = 0
        Data.append(info)

    print(Data)

for i in (1, 2):
    cheker = "C:/Users/user/Desktop/Cms/test" + str(i) + ".xlsx"
    outputer(cheker)