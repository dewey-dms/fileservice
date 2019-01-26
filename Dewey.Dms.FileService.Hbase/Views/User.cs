using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;

namespace Dewey.Dms.FileService.Hbase.Views
{
    public class User
    {

        public string LongKey
        {
            get => $"{Key}|{UserLastHistory.Key}";
        }

        public string Key
        {
            get => UserLastHistory.UserKey;
        }
        public string Login
        {
            get => UserLastHistory.Login;
        }
        
        public string Password
        {
            get => UserLastHistory.Password;
        }

        public string Name
        {
            get => UserLastHistory.Name;
        }
        public string SurName
        {
            get => UserLastHistory.SurName;
        }
        public bool IsDeleted
        {
            get => UserLastHistory.IsDelete;
        }
        public List<UserHistory> History { get; }

        private UserHistory UserLastHistory { get; set; }

        public static User CreateUser  ( List<UserHistory> history)
        {

            if (history != null && history.Count > 0)
            {
                
                if (history.Select(a=>a.UserKey).Distinct().Count()>1)
                    throw new Exception($"History not the same user");

                string userKey = history.First().UserKey;
                
                if (history.Count(a => a.IsAdd) > 1)
                    throw new Exception($"To many add operation in user {userKey}");
                
                if (history.Count(a => a.IsDelete) > 1)
                    throw new Exception($"To many delete operation in user {userKey}");
                
                return new User(history);
            }

            return null;
        }

        private User( List<UserHistory> history)
        {
            this.History = history;
            UserLastHistory = history.Where(a => a.OrderBy == history.Max(b => b.OrderBy)).Single();
         }
    }
}