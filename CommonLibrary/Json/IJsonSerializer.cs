using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xattacker.Utility.Json
{
    public interface IJsonSerializer
    {
        string ToJson();

        bool FromJson(string json);
    }
}
