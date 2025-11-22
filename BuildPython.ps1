docker build -t exelbdconverter-win .
$containerId = docker create exelbdconverter-win
docker cp "$containerId:C:\app\PythonSubprog\main.exe" .\main.exe
docker rm $containerId
Write-Host "exctracted to somewhere on your PC..."