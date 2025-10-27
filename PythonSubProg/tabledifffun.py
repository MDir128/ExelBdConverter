NAME = "tabledifffun"

import json

from numpy import diff

#основная функция
def tables_differences(prev_table, curr_table, switch):
    
    #преобразовываем списки в словари
    prev_dict = {f"{el[0]}{el[1]}": (el[2], el[3]) for el in prev_table}
    curr_dict = {f"{el[0]}{el[1]}": (el[2], el[3]) for el in curr_table}
        
    #выделяем ключи — номера ячеек и вносим их в множества, так как номер ячейки в таблице - уникальный объект
    prevdict_keys = set(prev_dict.keys())
    currdict_keys = set(curr_dict.keys())

    #создаём словарь-структуру, в котором будут содержаться измененения
    differences = {"changed information": {}, "newly added information": {}, "deleted information": {}}
    ''' первый элемент — изменённая информация в ячейках,
        второй элемент — новые добавленные ячейки
        третий элемент — удалённые ячейки '''

    #вносим в словарь с изменениями изменения ячеек
    for changed_keys in (currdict_keys & prevdict_keys): #ищем по объединению    
        curr_value, curr_formula = curr_dict[changed_keys]
        prev_value, prev_formula = prev_dict[changed_keys]
        if (curr_value != prev_value) or (curr_formula != prev_formula):
            differences["changed information"][changed_keys] = {
                'current version': f"(value: {curr_value}, formula: {curr_formula})",
                'previous version': f"(value: {prev_value}, formula: {prev_formula})"}
    #записываем как старое значение, так и новое, чтобы нагляно показать изменение

    #вносим в словарь с изменениями новые добавленные ячейки
    for newlyadded_keys in (currdict_keys - prevdict_keys): 
        value, formula = curr_dict[newlyadded_keys]
        differences["newly added information"][newlyadded_keys] = f"(value: {value}, formula: {formula})"
   
    #вносим в словарь с изменениями старые удалённые ячейки
    for deleted_keys in (prevdict_keys - currdict_keys): 
        value, formula = prev_dict[deleted_keys]
        differences["deleted information"][deleted_keys] = f"(value: {value}, formula: {formula})"    

    if switch == True:
        print("                                                       ")
        print("---- NUMBER OF CHANGES ----")
        print(f"Changes found: {len(differences['changed information'])}")
        print(f"Newly added found: {len(differences['newly added information'])}")
        print(f"Deleted found: {len(differences['deleted information'])}")

    print(json.dumps(differences, indent=2))

    #преобразуем наш словарь в JSON-строку
    return json.dumps(differences, indent=2)
    

