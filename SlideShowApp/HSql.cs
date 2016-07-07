using System.Text;
using SharpHsql;

namespace SlideShowApp
{
    public class HSql
    {
        private Database db;
        private Channel ch;

        public void OpenDB(string name)
        {
            db = new Database(name);
            ch = db.Connect("sa", "");
        }

        public int ExecuteNonQuery(string sql)
        {
            if (ch == null)
                return -1;
            Result rs = db.Execute(sql, ch);
            if (rs.Error != null)
                return -1;
            if (rs.Root == null)
                return 0;
            return rs.UpdateCount;
        }

        public string ExecuteQuery(string sql)
        {
            if (ch == null)
                return null;
            Result rs = db.Execute(sql, ch);
            if (rs.Error != null)
                return rs.Error;
            if (rs.Root == null)
                return rs.UpdateCount.ToString();

            StringBuilder sb = new StringBuilder();
            Record r = rs.Root;
            int column_count = rs.ColumnCount;
            for (int x = 0; x < column_count; x++)
            {
                sb.Append(rs.Label[x]);
                sb.Append('\t');
            }
            sb.AppendLine();
            while (r != null)
            {
                for (int x = 0; x < column_count; x++)
                {
                    sb.Append(r.Data[x]);
                    sb.Append('\t');
                }
                sb.AppendLine();
                r = r.Next;
            }
            sb.AppendLine();
            return sb.ToString();
        }

        public void CloseDB()
        {
            if (ch != null && db != null && !db.IsShutdown)
            {
                db.Execute("SHUTDOWN", ch);
            }

            if (ch != null)
            {
                if (!ch.IsClosed)
                {
                    ch.Disconnect();
                    ch.Dispose();
                }
                ch = null;
            }

            if (db != null)
            {
                db.Dispose();
                db = null;
            }
        }
    }
}
