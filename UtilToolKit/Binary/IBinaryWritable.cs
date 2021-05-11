
namespace Xattacker.Utility.Binary
{
    public interface IBinaryWritable <T>
    {
        T WriteBinary(byte[] data, int offset, int length);

        T WriteShort(short value);
        T WriteInteger(int value);
        T WriteLong(long value);
        T WriteDouble(double value);
        T WriteString(string value);
    }
}
