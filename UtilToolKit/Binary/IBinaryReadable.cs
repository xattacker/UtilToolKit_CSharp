
namespace Xattacker.Utility.Binary
{
    public interface IBinaryReadable
    {
        bool Available { get; }

        byte[] ReadBinary(int length);
        short ReadShort();
        int ReadInteger();
        long ReadLong();
        double ReadDouble();
        string ReadString();
    }
}
