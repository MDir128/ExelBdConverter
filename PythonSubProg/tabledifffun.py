import json
from collections import OrderedDict #для сохранения порядка вставки элементов

#основная функция
def tables_differences(prev_table, curr_table, switch):

    if prev_table is None:
        prev_table = []
    if curr_table is None:
        curr_table = []

    #создаются словари, которые будут заполняться ячейками таблицу
    prev_dict = dict()
    curr_dict = dict()
    for cell in prev_table:
        if len(cell) > 2:
            key = f"{cell[0]}{cell[1]}"
            value = cell[2]
            prev_dict[key] = value
    for cell in curr_table:
        if len(cell) > 2:
            key = f"{cell[0]}{cell[1]}"
            value = cell[2]
            curr_dict[key] = value

    #выделяются ключи — номера ячеек и вносятся в множества, так как номер ячейки в таблице - уникальный объект
    prevdict_keys = set(prev_dict.keys())
    currdict_keys = set(curr_dict.keys())

    #словарь с изменениями 
    changed_info = dict()
    for changed_key in sorted(currdict_keys & prevdict_keys, key=key_sort): #ищем по объединению    
        if prev_dict[changed_key] != curr_dict[changed_key]:
            changed_info[changed_key] = {"previous version": f"value: {prev_dict[changed_key]}", "current version": f"value: {curr_dict[changed_key]}"}
            #записываются как старое значение, так и новое, чтобы наглядно были видны изменения

    #словарь с новыми добавленными ячейками
    new_info = dict()
    for newlyadded_key in sorted(currdict_keys - prevdict_keys, key=key_sort): 
        new_info[newlyadded_key] = {newlyadded_key: f"value: {curr_dict[newlyadded_key]}"}
   
    #словарь с удалёнными ячейками
    deleted_info = dict()
    for deleted_key in sorted(prevdict_keys - currdict_keys, key=key_sort): 
        deleted_info[deleted_key] = {deleted_key: f"value: {prev_dict[deleted_key]}"} 

    #создаётся словарь-структура, в котором будут содержаться все измененения
    differences = {"changed information": changed_info, "newly added information": new_info, "deleted information": deleted_info}
    ''' первый элемент — изменённая информация в ячейках,
        второй элемент — новые добавленные ячейки
        третий элемент — удалённые ячейки '''    

    if switch == True:
        print("                                                       ")
        print("---- NUMBER OF CHANGES ----")
        print(f"Changes found: {len(changed_info)}")
        print(f"Newly added found: {len(new_info)}")
        print(f"Deleted found: {len(deleted_info)}")

    print(json.dumps(differences, indent=2))

    #преобразуем наш словарь в JSON-строку
    return json.dumps(differences, indent=2)
    
#функция для дальнейшей сортировке при выводе изменений
def key_sort(key):
        for letter_id, num_id in enumerate(key):
            if num_id.isdigit():
                letter = key[:letter_id]
                num = key[letter_id:]
                return (letter, int(num))
        return key