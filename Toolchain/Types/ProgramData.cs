namespace LIL.Toolchain.Types;

public class ProgramData : Data
{
    public ProgramDataType datatype { get; set; }
}

public enum ProgramDataType
{
    CHAR,
    INT,
    STRING,
    BOOL,
    BYTE,
    EMPTY
}