using System;

namespace GsmsLibrary
{
    public class GsmsUtils
    {
        public static string CreateGuiId()
        {
            Guid guid = Guid.NewGuid();
            return guid.ToString();
        }
    }
}
