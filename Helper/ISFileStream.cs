using System.IO;
using System.IO.IsolatedStorage;

namespace Helper
{
    public class ISFileStream
    {
        private static IsolatedStorageFile f;
        private static object lockObject = new object();

        public static FileStream Get(string name, FileMode mode, FileAccess access)
        {
            return Get(name, mode, access, FileShare.Read);
        }

        public static FileStream Get(string name, FileMode mode, FileAccess access, FileShare share)
        {
            lock (lockObject)
            {
                if (f == null)
                    f = IsolatedStorageFile.GetUserStoreForSite();
            }
            return f.OpenFile(name, mode, access, share);
        }
    }
}
