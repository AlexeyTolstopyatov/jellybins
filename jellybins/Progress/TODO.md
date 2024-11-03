# Логика в jellybins.Processing

Прийдется действовать по принципу разделяемой ответственности
Кто-то должен вернуть из ```byte[]``` структуру
Кто-то должен вернуть из структуры Таблицу свойств ```GeneralPropertiesTable```
Кто-то должен вернуть на основе таблицы -- страницу свойств.

### Конкретика.
Если в ход идут unix-like двоичные файлы, необходимо вернуть только таблицу свойств
Если в ход идут двоичные файлы "старого формата" (DOS/Windows/OS-2) + NET компоненты
Вернуть таблицу ```GeneralPropertiesTable``` + вернуть специфические свойства
(параметры заголовка)

### Microsoft/IBM подобные заголовки
Надо разобраться с разделяемостью свойств
Существует ```GeneralPropertiesTable```
```CSharp
class GeneralPropertiesTable
{
	public string Architecture { get; private set; }
	public string OperatingSystem { get; private set; }
	public string OsVersion { get; private set; }
	public string CpuFlags { get; private set; }
}
```

### Поле дополнительных флагов процессора
Скомпоновать это с остальными частями окна.

