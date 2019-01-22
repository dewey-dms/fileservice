using System;
using System.Data.Odbc;
using System.Runtime.InteropServices.ComTypes;
using Dewey.HBase.Stargate.Client.Models;

namespace Dewey.Dms.FileService.Hbase.Views
{
    public class FileHistory
    {
        
        public string LongKey { get; }
        public string Key { get; }
        
        public string KeyFile { get;  }
        public string KeyUser { get;  }
        public string KeyFileUser
        {
            get => $"{KeyFile}|{KeyUser}";
        }
        
        
        public string Parent { get; }
        public string FileName { get; }
        public string Extension { get;  }
        public bool IsDelete { get; set; }
        public bool IsAdd { get; set; }
        public bool IsClone { get; set; }
        public bool IsChange { get; set; }
        public DateTime OperationDate { get; }

        public FileHistory(OdbcDataReader reader)
        {
            LongKey = reader.GetString(0);

            (KeyFile, KeyUser, Key) = GetKeys(LongKey);
            
            FileName = reader.GetString(1);
            Extension = reader.GetString(2);
            if (!reader.IsDBNull(3))
                Parent = reader.GetString(3);
            IsAdd =  reader.GetString(4) == "1";
            IsChange = reader.GetString(5) == "1";
            IsClone = reader.GetString(6) == "1";
            IsDelete = reader.GetString(7) == "1";
            OperationDate = reader.GetDateTime(8);

        }
        
        public FileHistory(string key ,CellSet cellSet)
        {
            LongKey = key;

            (KeyFile, KeyUser, Key) = GetKeys(LongKey);
            FileName =  cellSet.GetString("description", "filename");
            Extension = cellSet.GetString("description", "extension");
            
            Parent = cellSet.GetStringOrNull("params", "parent");

            IsAdd = cellSet.GetBoolean("params", "is_add");
            IsChange = cellSet.GetBoolean("params", "is_change");
            IsClone = cellSet.GetBoolean("params", "is_clone");
            IsDelete = cellSet.GetBoolean("params", "is_delete");
            OperationDate = cellSet.GetDateTime("history", "operation_date");

        }
        
        


        private (string KeyFile, string KeyUser, string Key) GetKeys(string longKey)
        {
            string[] keys = LongKey.Split('|');
            if (keys.Length!=3)
                throw new Exception($"Wrong form of key {LongKey}");
            return (keys[0], keys[1], keys[2]);
        }
        
    }
}