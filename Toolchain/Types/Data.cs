namespace LIL.Toolchain.Types;

public class Data
{
    public object[] data { get; set; } = new object[1];

    public byte[] AsByteArray()
    {
        byte[] ret = new byte[data.Length - 1];

        for(int i = 0; i < data.Length - 1; i++)
        {
            ret[i] = (byte)data[i];
        }

        return ret;
    }
}