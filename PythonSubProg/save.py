import os
import pandas as pd

Data = {}

df = pd.DataFrame(Data)
df.to_excel('save.xlsx', index=False, engine='xlsxwriter')

#просто заготовка, я слегка в отчаянии