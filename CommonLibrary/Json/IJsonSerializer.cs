using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xattacker.Utility.Json
{
    interface IJsonSerializer
    {
        string ToJson();

        bool FromJson(string json);
    }
}
