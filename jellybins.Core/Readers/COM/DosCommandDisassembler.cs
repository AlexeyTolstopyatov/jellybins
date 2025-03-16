namespace jellybins.Core.Readers.COM;

public class Instruction 
{
    public string? Mnemonic { get; set; }
    public byte[]? Operands { get; set; }
}

public class DosCommandDisassembler
{
    public List<Instruction> Instructions { get; private set; } = new List<Instruction>();
    /// <summary>
    /// Reads and translates .CODE to Assembler Instructions
    /// array. It will be needed for API calls determination.
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    public (string CPU, string OS) DetermineSystemCalls(byte[] code)
    {
        int i = 0;
        (string tCPU, string tOS) tresult = new();
        
        while (i < code.Length)
        {
            byte opcode = code[i];

            if (opcode == 0xCD) // i8080 / i8086
            {
                if ((i + 1) < code.Length && code[i + 1] == 0x21)
                {
                    // INT 0x21
                    Instructions.Add(new Instruction
                    {
                        Mnemonic = "INT 0x21",
                        Operands = new byte[] { 0x21 }
                    });
                    i += 2;
                    tresult = ("Intel 8086", "Microsoft DOS");
                }
                else if ((i + 2) < code.Length && code[i + 1] == 0x05 && code[i + 2] == 0x00)
                {
                    // CALL 0x0005
                    Instructions.Add(new Instruction
                    {
                        Mnemonic = "CALL 0x0005",
                        Operands = new byte[] { 0x05, 0x00 }
                    });
                    i += 3;
                    tresult = ("Intel 8080", "CP/M");
                }
                else
                {
                    Instructions.Add(new Instruction
                    {
                        Mnemonic = $"CALL 0x{code[i + 1]:x}",
                        Operands = new[] { code[i + 1] }
                    });
                    i += 2;
                    tresult = ("Intel 8080", "CP/M");
                }
            }
            else
            {
                // Другие инструкции
                i += 1;
            }
            
        }
        return tresult;
    }
}