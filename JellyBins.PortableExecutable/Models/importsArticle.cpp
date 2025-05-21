//
// Taken from https://kaimi.io/2011/09/pe-format-import/
// As Reference of importing functions bound
// My little brain gives up to follow Microsoft documentation
// directly "by words"
//

//Необходимые стандартные заголовки
#include <iostream>
#include <fstream>
#include <iomanip>
#include <string>
#include <sstream>
//Заголовки, определяющие класс pe32 и pe64, а также исключения
#include "pe3264.h"
#include "pe_exception.h"

int main(int argc, const char* argv[])
{
	if(argc != 2)
	{
		std::cout << "Usage: sectons.exe pe_file" << std::endl;
		return 0;
	}

	//Открываем PE-файл на чтение
	std::ifstream pefile;
	pefile.open(argv[1], std::ios::in | std::ios::binary);
	if(!pefile.is_open())
	{
		std::cout << "Can't open file" << std::endl;
		return 0;
	}

	try
	{
		//Создаем объект класса PE32
		pe32 executable(pefile);
		//Если у него есть импорты
		if(executable.has_imports())
		{
			//Получаем указатель на массив таблиц IMAGE_IMPORT_DESCRIPTOR
			const IMAGE_IMPORT_DESCRIPTOR* import_descriptor_array = reinterpret_cast<const IMAGE_IMPORT_DESCRIPTOR*>(executable.section_data_from_rva(executable.directory_rva(IMAGE_DIRECTORY_ENTRY_IMPORT)));

			//И перебираем их до тех пор, пока не достигнем нулевого элемента
			while(import_descriptor_array->Characteristics)
			{
				//Выведем таймстамп и имя библиотеки
				std::cout << "DLL Name: " << executable.section_data_from_rva(import_descriptor_array->Name) << std::endl;
				std::cout << "Import TimeDateStamp: " << import_descriptor_array->TimeDateStamp << std::endl;

				//Получим указатель на таблицу адресов,
				//которую должен заполнить загрузчик
				const DWORD* import_address_table = reinterpret_cast<const DWORD*>(executable.section_data_from_rva(import_descriptor_array->FirstThunk));

				//И указатель на лукап-таблицу, которая содержит
				//имена импортируемых функций.
				//Стоит обратить внимание на то, что некоторые линкеры
				//допускают ошибку и оставляют этот указатель нулевым.
				//Это, в принципе, валидный exe-файл, но в случае
				//необходимости после загрузки файла  уже не удастся восстановить имена
				//импортируемых функций, так как единственная существующая
				//в данном случае таблица адресов, являющаяся одновременно и лукап-таблицей,
				//будет исковеркана загрузчиком
				const DWORD* import_lookup_table = import_descriptor_array->OriginalFirstThunk == 0
					? import_address_table
					: reinterpret_cast<const DWORD*>(
						executable.section_data_from_rva(import_descriptor_array->OriginalFirstThunk)
						);

				//Для информации
				DWORD address_table = import_descriptor_array->FirstThunk;

				//Переменные для хранения имени импортируемой функции и ее порядкового номера в таблице
				//экспортируемых функций DLL (hint)
				//Следует обратить внимание на то, что хинт и ординал - это не одно и то же
				//Ординал - это некий номер, соответствующий функции, и по этому номеру импорт также может производиться
				//Подробнее об этом я напишу, когда доберусь до описания экспорта
				std::string name;
				WORD hint;

				std::cout << std::endl << " hint | name/ordinal                |  address" << std::endl;

				//Перебор импортируемых функций
				if(import_lookup_table != 0 && import_address_table != 0)
				{
					while(true)
					{
						//Тут стоило бы добавить дополнительные проверки, т.к. указатель для кривого exe
						//может оказаться невалидным, но этот пример демонстрационный
						//и не стремится быть идеально правильным
						DWORD address = *import_address_table++;

						//Если мы достигли конца списка импортируемых функций, то переходим
						//к следующей библиотеке
						if(!address)
							break;

						DWORD lookup = *import_lookup_table++;

						//Макрос из WinNT.h, говорит о том, что функция импортируется по ординалу
						if(IMAGE_SNAP_BY_ORDINAL32(lookup))
						{
							//Если это так, то выведем вместо имени функции ее ординал
							std::stringstream stream;
							stream << "#" << IMAGE_ORDINAL32(lookup);
							name = stream.str();
							hint = 0;
						}
						else
						{
							//В противном случае выведем ее имя
							name = executable.section_data_from_rva(lookup + 2);
							hint = *reinterpret_cast<const WORD*>(executable.section_data_from_rva(lookup));
						}

						//Выводим информацию об импортируемой функции
						std::cout << std::dec << "[" << std::setfill('0') << std::setw(4) << hint << "]"
							<< " " << std::left << std::setfill(' ') << std::setw(30) << name
							<< ":0x" << std::hex << std::right << std::setfill('0') << std::setw(8) << address_table
							<< std::endl;

						address_table += 4;
					}
				}

				std::cout << "==========" << std::endl << std::endl;

				//Переходим к следующей библиотеке
				import_descriptor_array++;
			}
		}
	}
	catch(const pe_exception& e)
	{
		//Если вдруг произошла ошибка
		std::cout << "Exception: " << e.what() << std::endl;
	}

	return 0;
}