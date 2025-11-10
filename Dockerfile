FROM python
WORKDIR /DockerApp
COPY . .
#RUN pip install --no-cache-dir -r requirements.txt
CMD ["python", "main.py"]