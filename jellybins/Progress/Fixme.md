# Вернись и исправь работу

| Что                                                           | Где         | Почему                                       |
|---------------------------------------------------------------|-------------|----------------------------------------------|
| ```JbConfigReader.Read()```                                   | Middleware  | Чтение файла настроек программы              |
| ```JbAnalyser.Get()```                                        | Middleware  |                                              |
| ```JbPeInformationBlock```                                    | Modeling    | Необходимо знать названия и смещения секций  |
| ```JbClrInformation```                                        | Modeling    | Необходимо подробнее знать среду выполнения  |
| Поддержку чтения и анализа двоичных Unix файлов               | Modeling    | Нет поддержки ELF формата                    |
| ```ExecutableAnalyser.Get()``` ```ExecutableAnalyser.Set()``` | View        | В анализе не учитывается архитектура ЦП      |
| ```LinearExecutableInformation.VersionFlag```                 | Modeling    | Версия Windows для VDD 6342.3445 вместо 4.0  |





