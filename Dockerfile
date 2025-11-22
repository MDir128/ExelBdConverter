FROM mcr.microsoft.com/windows/python:3.9
RUN pip install nuitka
COPY PythonSubprog\ C:\app\PythonSubprog\
WORKDIR C:\app\PythonSubprog
RUN nuitka --standalone --onefile main.py
CMD ["main.exe"]