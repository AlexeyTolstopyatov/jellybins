using jellybins.Binary;
using jellybins.Models;
using jellybins.Views;

namespace jellybins.Middleware;
/*
 * JellyBins (C) Толстопятов Алексей А. 2024
 *      Binary Analyser
 * Здесь содержится начало операции сравнения характеристик
 * Вероятно прийдется переименовывать это, как JbAnalyser
 */
public class JbAnalyser
{
    
    public JbFileChars Characteristics { get; private set; } =
        new (JbFileOs.Unknown, JbFileType.Other, 0, 0);

    public static string EqualsToString(JbFileChars analysing, JbFileChars thisPc)
    {
        // Я же понимаю, что GUI часть я только под Windows
        // могу осилить. Походу речи и быть не может, что ОС
        // сравниваемого файла может не быть MS Windows
        if (analysing.Os is JbFileOs.Windows && analysing.MajorVersion <= thisPc.MajorVersion)
            return "Запускается на вашем устройстве";
        
        // Начиная с Windows 2000 (NT 5.0) убрали поддержку подсистемы
        // OS/2 (os2ss) для приложений собранных для 1.1 версии Поэтому наверное
        // будет лучше сразу проверить версию ОС
        //
        // Поддержку NTVDM (NT Virtual DOS Machine) отключили чуть позже,
        // на моей памяти уже в NT 6.0 (Windows Vista) нельзя было
        // запускать приложения собранные под DOS.
        
        // Для всего ценного старья.
        if (analysing is
            {
                Os: not JbFileOs.Unix,
                Type: JbFileType.New or JbFileType.MarkZbykowski or JbFileType.Linear
            } && thisPc.MajorVersion <= 5)
            return "Запускается на вашем устройстве";
            
        // Если все-таки заголовок уже все-таки COFF/PE
        if (analysing is { Os: JbFileOs.Windows, Type: JbFileType.Portable } 
            && thisPc.MajorVersion >= analysing.MajorVersion)
            return "Запускается на вашем устройстве";
        
        return "Не совместимо";
    }
    
    /// <summary>
    /// Получает информацию о исследуемом файле
    /// Возвращает характеристики исследуемого файла.
    /// </summary>
    /// <param name="path"></param>
    /// <param name="hPage"></param>
    /// <returns></returns>
    public static JbAnalyser Get(string path, ref BinaryHeaderPage hPage)
    {
        ushort mzSign = JbSearcher.GetUInt16(path, 0);
        if (JbTypeInformation.DetectType(mzSign) == JbFileType.Other)
        {
            JbCommon.TryParseUnknown(ref hPage, mzSign);
            return new JbAnalyser();
        }

        MzHeader mz = new();
        JbSearcher.Fill(ref path, ref mz);
        JbCommon.TryParseMzHeader(ref hPage, ref mz);

        ushort word = JbSearcher.GetUInt16(path, mz.e_lfanew);

        switch (JbTypeInformation.DetectType(word))
        {
            case JbFileType.Linear:
                LeHeader le = new();
                JbSearcher.Fill(ref path, ref le, (int)mz.e_lfanew);
                JbFileChars lch = 
                    JbCommon.TryParseLeHeader(ref hPage, ref le);
                return new JbAnalyser()
                {
                    Characteristics = lch
                };
            case JbFileType.New:
                NeHeader ne = new();
                JbSearcher.Fill(ref path, ref ne, (int)mz.e_lfanew);
                JbFileChars nch = 
                    JbCommon.TryParseNeHeader(ref hPage, ref ne);
                return new JbAnalyser()
                {
                    Characteristics = nch
                };
            case JbFileType.Portable:
                NtHeader nt = new();
                JbSearcher.Fill(ref path, ref nt, (int)mz.e_lfanew);
                JbFileChars pch =
                    JbCommon.TryParsePortableHeader(ref hPage, ref nt);

                // Если внутри PE файла есть сведения о CIL/CLR Надо узнать где они будут
                // располагаться.
                // Смещение CLR заголовка зависит от разрядности файла, как и положение
                // всех сегментов/структур (стоящих после PE заголовка)
                int offset = nt.WinNtOptional.Magic switch
                {
                    0x10b => 80,
                    0x20b => 200,
                    _ => 0
                };
                if (JbSearcher.GetUInt32(path, offset) != 0)
                {
                    ClrHeader clr = new();
                    JbSearcher.Fill(ref path, ref clr);
                    JbCommon.TryParseCommonLangRuntimeHeader(ref hPage, ref clr);
                }

                return new JbAnalyser()
                {
                    Characteristics = pch
                };
            default:
                return new JbAnalyser()
                {
                    Characteristics = new JbFileChars(
                        JbFileOs.Dos, 
                        JbFileType.MarkZbykowski, 
                        2, 
                        0)
                };
        }
    }

    /// <summary>
    /// Устанавливает файл-образец с которого будут получены
    /// сведения о системе
    /// </summary>
    /// <param name="path"></param>
    /// <param name="bin"></param>
    /// <returns></returns>
    public static JbAnalyser Set(string path, ref BinaryHeaderPage bin)
    {
        MzHeader doshead = new();
        NtHeader winnt = new();
        
        JbSearcher.Fill(ref path, ref doshead);
        ushort word = 
        JbSearcher.GetUInt16(path, doshead.e_lfanew);
        JbSearcher.Fill(ref path, ref winnt, (int)doshead.e_lfanew);
        JbFileChars chars = JbCommon.TryParseReference(ref bin, ref winnt);
        return new JbAnalyser()
        {
            Characteristics = chars
        };
    }
}
