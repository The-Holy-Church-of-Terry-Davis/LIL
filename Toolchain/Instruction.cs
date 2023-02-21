namespace LIL.Toolchain;

public class InstructionCollection : Dictionary<string, Instruction>
{
    public Dictionary<string, Instruction> instructions = new();

    public new IEnumerator<KeyValuePair<string, Instruction>> GetEnumerator()
    {
        return instructions.GetEnumerator();
    }
}

public class Instruction
{
    public InstructionType tp { get; set; }
    public object[]? args { get; set; }
}

public class InstructionOutput
{

}

public enum InstructionType
{
    STACK,
    VAR,
    SET,
    SYSCALL
}