import os
import shutil
import pandas

from pybuilder.core import use_plugin, init


# ЭТОТ КУСОК КОДА РАБОТАЕТ ИСКЛЮЧИТЕЛЬНО У САНИ
# если запустит кто то другой есть шанс поломать папки, так что не надо 

use_plugin("python.core")

name = "PythonSubProg"
version = "1.0.0"

# Список библиотек, которые нужно встроить
DEPENDENCIES = ["pandas", "openpyxl"]

@init
def copy_binaries(project):
   
    libs_dir = os.path.join(os.path.dirname(__file__), "libs")

    for lib_name in DEPENDENCIES:
        try:
            # Импортируем библиотеку, чтобы найти путь
            lib = __import__(lib_name)
            src_dir = os.path.dirname(lib.__file__)
            dst_dir = os.path.join(libs_dir, lib_name)
            os.makedirs(dst_dir, exist_ok=True)

            for f in os.listdir(src_dir):
                src_file = os.path.join(src_dir, f)
                dst_file = os.path.join(dst_dir, f)

                if os.path.isfile(src_file) and (f.endswith(".pyd") or f.endswith(".py")):
                    shutil.copy2(src_file, dst_file)
                elif os.path.isdir(src_file):
                    # рекурсивно копируем папку, если нужно
                    dst_subdir = os.path.join(dst_dir, f)
                    if not os.path.exists(dst_subdir):
                        shutil.copytree(src_file, dst_subdir)
        except ImportError:
            print(f"[WARN] Библиотека {lib_name} не найдена, пропускаем.")

    print("Копирование бинарников завершено.")