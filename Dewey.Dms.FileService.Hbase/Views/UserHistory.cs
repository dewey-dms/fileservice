using System;
using System.Data.Odbc;
using System.Xml;
using Dewey.HBase.Stargate.Client.Models;

namespace Dewey.Dms.FileService.Hbase.Views
{
    public class UserHistory
    {
        public string UserKey { get=>   LongKey.Substring(0, LongKey.IndexOf("|")); }
        public string LongKey { get; }
        public string Key { get=> LongKey.Substring(LongKey.IndexOf("|")+1);}
        public string Login { get; }
        
        public string Password { get; }
        public string Name { get;  }
        public string SurName { get; }
        public bool IsAdd { get; }
        public bool IsChange { get; }
        public bool IsDelete { get; }
        public DateTime OperationDate { get; }

        public long OrderBy { get; }
        
        public UserHistory(OdbcDataReader reader)
        {
            LongKey  = reader.GetString(0);
            this.Login = reader.GetString(1);
            this.Name = reader.GetString(2);
            this.SurName = reader.GetString(3);
            this.IsAdd = reader.GetString(4) == "1";
            this.IsChange = reader.GetString(5) == "1";
            this.IsDelete = reader.GetString(6) == "1";
            this.OperationDate = reader.GetDateTime(7);
            OrderBy = reader.GetInt64(8);
            Password = reader.GetString(9);

        }

        
        public UserHistory(string rowKey , CellSet cellSet)
        {

           LongKey = rowKey;
           this.Login = cellSet.GetString("description", "login");
           this.Password = cellSet.GetString("description", "password");
           this.Name =  cellSet.GetString("description", "name");
           this.SurName =  cellSet.GetString("description", "surname");
           this.IsAdd =  cellSet.GetBoolean("params", "is_add");
           this.IsChange =  cellSet.GetBoolean("params", "is_change");
           this.IsDelete =  cellSet.GetBoolean("params", "is_delete");
           this.OperationDate = cellSet.GetDateTime("history", "operation_date");
           OrderBy = cellSet.GetLong("history", "orderby");
          
        }
        
    }
}