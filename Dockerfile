FROM python
WORKDIR /Ducker
COPY . .
#RUN apt-get update && apt-get install -y patchelf
#RUN pip install nuitka
#WORKDIR /Ducker/PythonSubProg
#RUN nuitka --standalone --onefile main.py
