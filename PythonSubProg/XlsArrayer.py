import os
import openpyxl
import pandas as pd

NAME = "XlsArrayer"
def XlsArrayerOut(cheker):
    if not os.path.exists(cheker):
        print("file err", cheker)
    else:
        xlsx = pd.read_excel(cheker, dtype=str)#извелчение
        data = xlsx.fillna('') # эта строчка делает пустые клетки пустыми строками     
    Data = []
    for i, tablet in xlsx.iterrows():  # проходим по строкам
        info = []
        for a, value in enumerate(tablet):
            letter = chr(ord('A') + a)
            number = str(i + 1)
            if tablet.data_type == 'f': # Да я дважды извелкаю файлы, тут вообще всё неправильно написано. ничего страшного
                formula = tablet.value
            else:
                formula = set()  # если формулы нет, пустое множество
            info.append([letter, number, value, formula])
        Data.append(info)

    return(Data)