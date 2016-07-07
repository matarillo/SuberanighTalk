using System.IO.IsolatedStorage;
using System.IO;

namespace Helper
{
    public class ISFileInfo
    {
        private IsolatedStorageFile f;
        public string FileName { get; set; }
        public ISFileInfo(string filename)
        {
            this.FileName = filename;
            this.f = IsolatedStorageFile.GetUserStoreForSite();
        }

        public bool Exists
        {
            get
            {
                return f.FileExists(this.FileName);
            }
        }

        public long Length
        {
            get
            {
                using (IsolatedStorageFileStream fs = f.OpenFile(this.FileName, FileMode.Open, FileAccess.Read))
                {
                    return fs.Length;
                }
            }
        }


        public void Delete()
        {
            if (!f.FileExists(this.FileName))
            {
                return;
            }
            f.DeleteFile(this.FileName);
        }

        public void MoveTo(string file)
        {
            if (!f.FileExists(this.FileName))
            {
                return;
            }
            MemoryStream ms = new MemoryStream();
            using (IsolatedStorageFileStream fs = f.OpenFile(this.FileName, FileMode.Open, FileAccess.Read))
            {
                byte[] buffer = new byte[128 * 1024];
                int count = 0;
                while ((count = fs.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, count);
                }
            }
            f.DeleteFile(this.FileName);
            using (IsolatedStorageFileStream fs = f.OpenFile(file, FileMode.Create, FileAccess.Write))
            {
                ms.WriteTo(fs);
            }
        }
    }

    /*

        public class ISFileInfo
        {
            public string FileName { get; set; }
            public ISFileInfo(string filename)
            {
                this.FileName = filename;
            }

            public bool Exists
            {
                get
                {
                    using (IsolatedStorageFile f = IsolatedStorageFile.GetUserStoreForSite())
                    {
                        return f.FileExists(this.FileName);
                    }
                }
            }

            public long Length
            {
                get
                {
                    using (IsolatedStorageFile f = IsolatedStorageFile.GetUserStoreForSite())
                    {
                        using (IsolatedStorageFileStream fs = f.OpenFile(this.FileName, FileMode.Open, FileAccess.Read))
                        {
                            return fs.Length;
                        }
                    }
                }
            }


            public void Delete()
            {
                using (IsolatedStorageFile f = IsolatedStorageFile.GetUserStoreForSite())
                {
                    if (!f.FileExists(this.FileName))
                    {
                        return;
                    }
                    f.DeleteFile(this.FileName);
                }
            }

            public void MoveTo(string file)
            {
                using (IsolatedStorageFile f = IsolatedStorageFile.GetUserStoreForSite())
                {
                    if (!f.FileExists(this.FileName))
                    {
                        return;
                    }
                    MemoryStream ms = new MemoryStream();
                    using (IsolatedStorageFileStream fs = f.OpenFile(this.FileName, FileMode.Open, FileAccess.Read))
                    {
                        byte[] buffer = new byte[128 * 1024];
                        int count = 0;
                        while ((count = fs.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            ms.Write(buffer, 0, count);
                        }
                    }
                    f.DeleteFile(this.FileName);
                    using (IsolatedStorageFileStream fs = f.OpenFile(file, FileMode.Create, FileAccess.Write))
                    {
                        ms.WriteTo(fs);
                    }
                }
            }
        }

     */
}
