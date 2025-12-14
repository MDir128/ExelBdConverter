import os
import pandas as pd

def excel_to_df(file_path, sheet=0):
    if not os.path.exists(file_path):
        print(f"Ошибка: файл не найден - {file_path}")
        return None
    
    df = pd.read_excel(file_path, sheet_name=sheet, dtype=str)
    df = df.fillna('')
    return df

def df_to_array(df):
    if df is None:
        return []
    array = []
    for inumber in range(len(df)):
        for iletter in range(len(df.columns)):

            value = str(df.iat[inumber, iletter])
            letter = str(iletter)
            number = str(inumber + 1)
            formula = "" # maybe in the future but not tooday
            
            cell = [letter, number, value, formula]
            array.append(cell)
    
    return array

def array_to_df(arr):

    if not arr:
        return pd.DataFrame()
    
    maxRow = 0
    maxCol = 0

    for cell in arr:
        rowNum = int(cell[1])
        colNum = int(cell[0])
        if rowNum > maxRow:
            maxRow = rowNum
        if colNum > maxCol:
            maxCol = colNum
    
    headers = {}
    for col in range(maxCol + 1):
        headers[col] = f"Column_{col}"
    
    for cell in arr:
        if cell[1] == "1":
            colNum = int(cell[0])
            headers[colNum] = cell[2] 
    
    data = []
    for row in range(2, maxRow + 1):
        rowData = [""] * (maxCol + 1)
        
        for cell in arr:
            if cell[1] == str(row):
                colNum = int(cell[0])
                if colNum <= maxCol:
                    rowData[colNum] = cell[2]
        
        data.append(rowData)
    
    if headers:
        sortedHeaders = [headers[col] for col in sorted(headers.keys())]
        df = pd.DataFrame(data, columns=sortedHeaders)
    else:
        df = pd.DataFrame(data)
    
    return df

def XlsArrayerOut(file_path):
    df = excel_to_df(file_path)
    if df is not None:
        return df_to_array(df)
    return []

def XlsArrayerIn(array, output_path):
    try:
        df = array_to_df(array)
        df.to_excel(output_path, index=False)
        print(f"Файл сохранен: {output_path}")
        return True
    except Exception as e:
        print(f"Ошибка при сохранении: {e}")
        return False