using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Dewey.Dms.FileService.Hbase.Operations;
using Dewey.Dms.FileService.Hbase.Views;

namespace Dewey.Dms.FileService.Hbase.Service
{
  
   public class HiveDatabaseService:IDatabaseService
    {



        private string ConnectionString = null;

        public HiveDatabaseService(string ConnectionString)
        {
            this.ConnectionString = ConnectionString;
        }


        private static string FormatDateTimeToTimestamp(DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd HH:mm:ss");
        }

#pragma warning disable 1998
        public async Task DoUserOperations(UserOperations operations)
#pragma warning restore 1998
        {
            using (var con = new OdbcConnection(ConnectionString))
            {
                con.Open();
                using (var com = con.CreateCommand())
                {
                    com.CommandText = $@"insert into table dewey_users
                        (
                        key,
                        login,
                        name,
                        surname,
                        is_add ,
                        is_change,
                        is_delete,
                        operation_date,
                        orderby,
                        password
                       ) 
                       values(
                        '{operations.Key}',
                        '{operations.Login}',
                        '{operations.Name}',
                        '{operations.SurName}',
                         {operations.IsAdd},
                         {operations.IsChange},
                         {operations.IsDelete},
                         '{FormatDateTimeToTimestamp(operations.OperationDate)}',
                         {operations.OrderBy},
                         '{operations.Password}'
                      )";
                    com.ExecuteNonQuery();
                    //return true;
                }
            }
        }

#pragma warning disable 1998
        public async Task<User> GetUser(string key)
#pragma warning restore 1998
        {
            using (var con = new OdbcConnection(ConnectionString))
            {
                con.Open();
                using (var com = con.CreateCommand())
                {
                    com.CommandText = $@"select 
                        key , 
                        login ,
                        name , 
                        surname , 
                        is_add ,
                        is_change ,
                        is_delete , 
                        operation_date,
                        orderby,
                        password
                        from dewey_users where key like '{key}|%'";
                    OdbcDataReader reader = com.ExecuteReader();
                    //reader.Read();
                    List<UserHistory> userHistory = new List<UserHistory>();
                    while (reader.Read())
                        userHistory.Add(new UserHistory(reader));

                    return User.CreateUser(userHistory);
                }
            }
        }



#pragma warning disable 1998
        public async Task<IEnumerable<User>> GetUsers(bool? isDelete = null)
#pragma warning restore 1998
        {

            using (var con = new OdbcConnection(ConnectionString))
            {
                con.Open();
                using (var com = con.CreateCommand())
                {
                    com.CommandText = $@"select 
                        key , 
                        login ,
                        name , 
                        surname , 
                        is_add ,
                        is_change ,
                        is_delete , 
                        operation_date,
                        orderby ,
                        password
                        from dewey_users ";

                    /*if (isDelete.HasValue)
                        com.CommandText += " where is_delete = '" + isDelete + "'";
                    */
                    OdbcDataReader reader = com.ExecuteReader();
                    List<UserHistory> userHistory = new List<UserHistory>();
                    while (reader.Read())
                        userHistory.Add(new UserHistory(reader));

                    IEnumerable<User> users =
                        userHistory
                            .Where(a=>a!=null)
                            .GroupBy(a => a.UserKey)
                            .Select(a => User.CreateUser(a.ToList()));

                    if (isDelete.HasValue)
                        users = users.Where(a => a.IsDeleted == isDelete);

                    return users.ToList();
                }
            }
        }


#pragma warning disable 1998
        public async Task DoFileOperations(FileOperations operations)
#pragma warning restore 1998
        {
            using (var con = new OdbcConnection(ConnectionString))
            {
                
                
                con.Open();
                using (var com = con.CreateCommand())
                {
                    string parentColumn = string.IsNullOrEmpty(operations.Parent) ? string.Empty : "parent,";
                    string parentValue = string.IsNullOrEmpty(operations.Parent) ? string.Empty : $"'{operations.Parent}',";
                    
                    com.CommandText = $@"insert into table dewey_files (
                    key ,
                    filename ,
                    extension ,
                    {parentColumn}
                    is_add ,
                    is_change ,
                    is_clone ,
                    is_delete ,
                    operation_date ,
                    orderby)
                    values (
                    '{operations.Key}',
                    '{operations.FileName}',
                    '{operations.Extension}',
                    {parentValue}
                    {operations.IsAdd},
                    {operations.IsChange},
                    {operations.IsClone},
                    {operations.IsDelete},
                    '{FormatDateTimeToTimestamp(operations.OperationDate)}',
                    {operations.OrderBy} )";
                    com.ExecuteNonQuery();    
                    //return true;
                }
            }

        }

#pragma warning disable 1998
        public async Task<File> GetFile(string key)
#pragma warning restore 1998
        {
            using (var con = new OdbcConnection(ConnectionString))
            {
                con.Open();
                using (var com = con.CreateCommand())
                {    
                    com.CommandText = $@"select 
                    key ,
                    filename ,
                    extension ,
                    parent,
                    is_add ,
                    is_change ,
                    is_clone ,
                    is_delete ,
                    operation_date ,
                    orderby
                    from
                    dewey_files where key like '{key}|%'";
                    OdbcDataReader reader = com.ExecuteReader();
                    //reader.Read();
                    List<FileHistory> fileHistory = new List<FileHistory>();
                    while(reader.Read())
                        fileHistory.Add(new FileHistory(reader));
                    return File.CreateFile(fileHistory);


                }
            }
        }

#pragma warning disable 1998
        public async Task<IEnumerable<File>> GetUserFile(string userKey, bool? isDelete = null)
#pragma warning restore 1998
        {
            using (var con = new OdbcConnection(ConnectionString))
            {
                con.Open();
                using (var com = con.CreateCommand())
                {    
                    com.CommandText = $@"select 
                    key ,
                    filename ,
                    extension ,
                    parent,
                    is_add ,
                    is_change ,
                    is_clone ,
                    is_delete ,
                    operation_date ,
                    orderby
                    from
                    dewey_files where key like '{userKey}|%'";
                    OdbcDataReader reader = com.ExecuteReader();
                    //reader.Read();
                    List<FileHistory> fileHistory = new List<FileHistory>();
                    while(reader.Read())
                        fileHistory.Add(new FileHistory(reader));
                    
                    
                    IEnumerable<File> files =
                        fileHistory
                            .Where(a=>a!=null)
                            .GroupBy(a => a.KeyUserFile)
                            .Select(a => File.CreateFile(a.ToList()));
                    
                    if (isDelete.HasValue)
                        files = files.Where(a => a.IsDeleted == isDelete);
                    
                    return files;
                }
            }
        }
    }
}