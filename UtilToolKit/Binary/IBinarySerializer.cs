
namespace Xattacker.Utility.Binary
{
    public interface IBinarySerializer
    {
        byte[] ToBinary();
        bool FromBinary(byte[] content);
    }
}
