# Немного о COM


### Перевод ответа Ответ со StackOverFlow
(Перевел на русский)

COM файлы это здорово, потому что вы можете магическим
образом можете перевести его в текст. У EXE файлов
есть MZ заголовок, а у COM - его нет.
Поскольку вы можете генерировать код динамически 
в x86 архитектуре, вы можете генерировать 
двоичные коды операций, используя коды операций в печатаемых
символах.

(Readme.COM)
```
P5CQ5sPP[X5iK4iH4]P_1?CC5IQ5CBP_1?SX4v4pPZ5iH5i@okey
```

В этом примере коды операций начальной загрузки 
генерируют прерывание DOS INT 21H
для отображения строки, оканчивающейся на '$'.

Некоторые старые компиляторы могут поддерживать
некоторые опции .COM-файл, но поскольку это 16-разрядный исполняемый
файл, они не поддерживаются уже много лет.

### Ответ со StackOverFlow

COM files is great, because you can convert 
executable magically to text files. 
EXE files does always have a MZ header 
while COM doesn't. As you can generate code dynamically in x86 arch, 
you can generate binary opcodes using opcodes 
in printable chars.
Here is an example, program README.COM:

```
P5CQ5sPP[X5iK4iH4]P_1?CC5IQ5CBP_1?SX4v4pPZ5iH5i@okey
Text2COM example by 谢继雷 (Lenik).
$
```
In this example, the bootstrap opcodes generate 
a DOS INT 21H interrupt to display a string 
terminated by '$'.

Some old time compilers may have some options to support 
.COM file, but since it's 16-bit executable, 
they haven't been supported for many years.




### Подробнее
https://en.wikipedia.org/wiki/COM_file#MS-DOS_binary_format