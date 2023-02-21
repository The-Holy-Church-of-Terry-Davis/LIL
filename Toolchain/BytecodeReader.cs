using LIL.Toolchain.Types;

namespace LIL.Toolchain;

public ref struct BytecodeReader
{
    public Span<byte> b { get; set; }
    public int pos { get; set; }
    public BytecodeContext ctx { get; set; }

    public Instruction ReadInstruction()
    {
        Instruction ret = new();
        
        switch(ctx)
        {
            case BytecodeContext.NORMAL:
            {
                switch(b[pos])
                {
                    case 0x1:
                    {
                        ret.tp = InstructionType.VAR;
                        
                        int size = (int)b[pos + 1];

                        ProgramDataType type = ByteTools.GetType(ref b[pos + 2]);

                        Span<byte> str = b.Slice(pos + 3).Slice(ByteTools.FindStringEnd(pos + 3, b)[1]);
                        string name = ByteTools.ReadString(ref str);

                        int valstart = ByteTools.FindStringEnd(pos + 3, b)[1] + 1;
                        Span<byte> pass = b.Slice(valstart);
                        Span<byte> second = pass.Slice(ByteTools.FindValueEnd(valstart, ref pass)[1]);
                        object val = ByteTools.ReadValue(ref second, type);

                        ret.args = new object[] { size, type, name, val };
                        
                        pos = pos + 4;

                        break;
                    }

                    case 0x2:
                    {
                        ret.tp = InstructionType.STACK;

                        

                        break;
                    }
                }

                break;
            }
        }

        return ret;
    }
}

public class ByteTools
{
    public static ProgramDataType GetType(ref byte b)
    {
        switch(b)
        {
            case 0xA1:
            {
                return ProgramDataType.CHAR;
            }

            case 0xA2:
            {
                return ProgramDataType.STRING;
            }

            case 0xA3:
            {
                return ProgramDataType.INT;
            }

            case 0xA4:
            {
                return ProgramDataType.BOOL;
            }
        }

        return ProgramDataType.BYTE;
    }

    public static int[] FindValueEnd(int indicator1, ref Span<byte> b, byte tolook = 0x1F) //Value indicators are byte: 0x1F
    {
        int[] ret = new int[2] { indicator1, 0 };

        for(int i = indicator1; i < b.Length; i++)
        {
            if(b[i] == tolook)
            {
                ret[1] = i;
                break;
            }
        }

        if(ret[1] < indicator1)
        {
            ret[1] = b.Length - 1;
        }

        return ret;
    }

    public static int[] FindStringEnd(int indicator1, Span<byte> b, byte tolook = 0xEF)  //String indicators are byte: 0xEF
    {
        int[] ret = new int[2] { indicator1, 0 };

        for(int i = indicator1; i < b.Length; i++)
        {
            if(b[i] == tolook)
            {
                ret[1] = i;
                break;
            }
        }

        if(ret[1] < indicator1)
        {
            ret[1] = b.Length - 1;
        }

        return ret;
    }

    public static string ReadString(ref Span<byte> b)
    {
        string ret = "";

        foreach(byte bt in b)
        {
            ret = ret + (byte)bt;
        }

        return ret;
    }

    public static object ReadValue(ref Span<byte> b, ProgramDataType tp)
    {
        switch(tp)
        {
            case ProgramDataType.CHAR:
            {
                return (char)b[0];
            }

            case ProgramDataType.INT:
            {
                return (int)b[0];
            }

            case ProgramDataType.STRING:
            {
                //look for byte 0xEE as string indicator
                switch(b[0])
                {
                    case 0xEE:
                    {
                        int[] indicators = FindStringEnd(b[0], b, 0xEE);
                        Span<byte> strbts = b.Slice(1).Slice(indicators[1] - 1);

                        return ReadString(ref strbts);
                    }
                }

                return "";
            }

            case ProgramDataType.BOOL:
            {
                //You will need to make your own byte indicators for a boolean value (0xF0 false, 0xF1 true)

                switch(b[0])
                {
                    case 0xF1:
                    {
                        return true;
                    }
                    
                    case 0xF0:
                    {
                        return false;
                    }

                }

                return false;
            }
        }

        return new object();
    }
}

public enum BytecodeContext
{
    NORMAL,
    INIT_VAR,
    INIT_METHOD,
    SYSCALL
}