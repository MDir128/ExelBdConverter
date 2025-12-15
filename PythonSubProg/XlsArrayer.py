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
            # Преобразуем индекс в букву
            letter = ''
            col_idx = iletter + 1
            while col_idx > 0:
                col_idx -= 1
                letter = chr(65 + (col_idx % 26)) + letter
                col_idx //= 26
            
            number = inumber + 1  # int, а не str
            
            # Создаем ячейку в формате, который ожидает tablemerge.py
            cell = [letter, number, value]
            array.append(cell)
    
    return array


def array_to_df(arr):
    if not arr:
        return pd.DataFrame()
    
    # Находим максимальные размеры
    maxRow = 0
    maxCol = 0
    
    # Сначала находим все номера строк и столбцов
    for cell in arr:
        try:
            rowNum = int(cell[1])
            colNum = int(cell[0])
            if rowNum > maxRow:
                maxRow = rowNum
            if colNum > maxCol:
                maxCol = colNum
        except (ValueError, IndexError):
            continue
    
    # Создаем матрицу данных
    data_matrix = {}
    
    # Заполняем матрицу
    for cell in arr:
        try:
            rowNum = int(cell[1])
            colNum = int(cell[0])
            value = cell[2] if cell[2] is not None else ""
            
            if rowNum not in data_matrix:
                data_matrix[rowNum] = {}
            data_matrix[rowNum][colNum] = value
        except (ValueError, IndexError):
            continue
    
    # Определяем заголовки из первой строки
    headers = []
    
    # Если есть первая строка
    if 1 in data_matrix:
        # Проходим по всем колонкам от 0 до maxCol
        for col in range(maxCol + 1):
            if col in data_matrix[1]:
                header_value = str(data_matrix[1][col])
                # Если заголовок пустой, создаем стандартный
                if header_value and header_value.strip():
                    headers.append(header_value)
                else:
                    headers.append(f"Column_{col}")
            else:
                headers.append(f"Column_{col}")
    else:
        # Если нет первой строки, создаем стандартные заголовки
        headers = [f"Column_{col}" for col in range(maxCol + 1)]
    
    # Собираем данные (начиная со второй строки)
    data = []
    for row in range(2, maxRow + 1):
        row_data = []
        if row in data_matrix:
            for col in range(maxCol + 1):
                if col in data_matrix[row]:
                    row_data.append(data_matrix[row][col])
                else:
                    row_data.append("")
        else:
            # Если строки нет в данных, заполняем пустыми значениями
            row_data = ["" for _ in range(maxCol + 1)]
        data.append(row_data)
    
    # Создаем DataFrame
    df = pd.DataFrame(data, columns=headers)
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