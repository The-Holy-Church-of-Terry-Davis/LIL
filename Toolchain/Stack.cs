using LIL.Toolchain.Types;

namespace LIL.Toolchain;

public class Stack : List<byte[]>
{
    protected List<Data> bt { get; set; } = new();

    public Stack(List<Data> b)
    {
        bt = b;
    }

    public new IEnumerator<Data> GetEnumerator()
    {
        return bt.GetEnumerator();
    }
}