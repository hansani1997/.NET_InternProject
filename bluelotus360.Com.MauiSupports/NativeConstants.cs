using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.Com.MauiSupports
{
    public class NativeConstants
    {
        public const string DatabaseName = "bl360hybriderpdb.db3";

        public const SQLite.SQLiteOpenFlags flags = SQLite.SQLiteOpenFlags.ReadWrite | SQLite.SQLiteOpenFlags.Create | SQLite.SQLiteOpenFlags.SharedCache;

        public static string DatabasePath = Path.Combine(FileSystem.AppDataDirectory, DatabaseName);
    }
}
