MNJGKHТак, пошаговый план: 

1) скачай DockerDesktop и зарегайся там 

2) тут лежит файл .tar (не лежит, слишком тяжелый для гита, попытаюсь скинуть отдельно) вам то он и нужен это image питона. 
   зараньте это в командной строке проект при скачанном докере и он сам все должен подгрузить.
   docker load -i exelbdconverter.tar

3) Чтобы заранить контейнер, в папке проекта (ExelBdConverter) заранить это:
   docker run -d --name exelbdconverter-container exelbdconverter

НАПОМИНАНИЕ: 

ДА Я НАПИСАЛ МИНИ ГАЙД ДЛЯ УПРОЩЕНИЯ РАБОТЫ, НО не стесняйтесь спрашивать и писать мне. 
вероятно ничерта не получится и придется созваниватся в дс. 

Upd. чтобы получить Bid файл с фулл прогой на питоне надо заранить в фолдере с image который вы получили

это три команды, НО надо будет зарегаться на докере и подключиться к общему хабу там, чтобы когда я обновлял
image до новой версии вы могли его трогать. в теории и это можно автоматизировать, но я еще не придумал как...

docker create --name tempcontainer exelbdconverter
docker cp tempcontainer:/Ducker/PythonSubProg/main.bin ./main.bin
docker rm tempcontainer

!!!! заПуллить Докер: docker pull stopich1/exelbdconverter:latest !!!!
