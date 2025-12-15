def sort_table(table, column_name, mode = True): #True � �� �����������, False � �� ��������
	sorted_table = []

	if not table:
		return table

	headers_dict = dict() #������� ��� ���������� � �� ��������� ��������
	headers_cells = [] #������ ��� ������ ����������
	for cell in table:
		if cell[1] == 1: #��� ���������
			headers_cells.append(cell)
			if len(cell) > 2 and cell[2] != None:
				headers_dict[cell[2]] = cell[0] #������� ��������� ��������������� � �������� �������� ��� ��������� ������

	if column_name not in headers_dict:
		raise ValueError("Table doesn't have a column with given name")
	header_id = headers_dict[column_name] 
	table_strings = dict()
	for cell in table:
		string_number = cell[1]
		if string_number == 1:
			continue #������ ���������� ��� ���� ����������
		if string_number not in table_strings:
			table_strings[string_number] = {"string number": string_number, "string cells": [], "sort value": None} #string_number � ����, ������� ������ � ��������
		table_strings[string_number]["string cells"].append(cell) #���� ������ ��������� �������, �� � ������� �������� � ����� � ������ ���� � �����
		if cell[0] == header_id and len(cell) > 2 and cell[2] != None:
			table_strings[string_number]["sort value"] = cell[2]

	#������� ��� ��������� ������ ����� ������ ��� ����������: ����� �����, ���������� �����, ������, None
	#��� ��������� ���������, ��� ��������� ������ ����� ������������
	def sort_case(string): #���� ���� � �������� �� ������� table_strings
		string_number, string_datas = string
		sort_value = string_datas["sort value"]
		if sort_value is None:
			if mode == True:
				return (0, 0) #����� ������ � ����� ������ (���� ���� ������� ����)
			else:
				return (-2, 0) #����� ������ � ����� �����

		if isinstance(sort_value, (int, float)):
			num = float(sort_value)
			if mode == True:
				return (1, num)
			else:
				return (-1, num)
		try:
			if '.' in str(sort_value):
				num = float(sort_value) 
			else:
				num = int(sort_value)
			if mode == True:
				return (1, num)
			else:
				return (1, -num)
		except (TypeError, ValueError):
			if mode == True:
				return (2, str(sort_value).strip().lower())
			else:
				return (2, str(sort_value).strip().lower())

	if mode == True:
		strings_sort = sorted(table_strings.items(), key=sort_case)
	else:
		strings_sort = sorted(table_strings.items(),key=sort_case, reverse=True)

	#���������� ����� ��������������� �������
	sorted_table.extend(headers_cells)
	string_newnumber = 2
	for string_number, string_datas in strings_sort: #��������� ������ ����
		for cell in string_datas["string cells"]: #��������� ������ ������ � ������ ����
			new_cell = cell.copy()
			new_cell[1] = string_newnumber
			sorted_table.append(new_cell)
		string_newnumber += 1 #��� ��� ������ �������������� ���� ��������� ������

	return sorted_table