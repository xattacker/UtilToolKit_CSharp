using System;
using System.Collections.Generic;
using System.Text;

namespace Xattacker.Binary
{
    public interface IBinarySerializer
    {
        byte[] ToBinary();

        bool FromBinary(byte[] content);
    }
}
