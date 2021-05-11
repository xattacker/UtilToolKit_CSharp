using System;
using System.Collections.Generic;
using System.Text;

namespace Xattacker.Utility
{
    public class GUID
    { 
        /// <summary>
        /// to hide the constructor
        /// </summary>
        private GUID()
        { 
        }

        /// <summary>
        /// generate a new format: xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx string
        /// </summary>
        public static string GenerateGUID()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
