using System;
using Dewey.Dms.FileService.Hbase.Views;

namespace Dewey.Dms.FileService.Hbase.Operations
{
    public class UserOperations
    {
        public string KeyUser { get;  protected set; }
        public string KeyUserVersion { get; protected set; }

        public string Key
        {
            get => $"{KeyUser}|{KeyUserVersion}";
        }
        
        public string Login { get; protected set; }
        public string Name { get; protected set; }
        public string SurName { get; protected set; }
        public bool IsDelete { get; protected set; }
        public bool IsChange { get; protected set; }
        public bool IsAdd { get; protected set; }
        public DateTime OperationDate { get; protected set; }

       
    }

    public class AddUserOperations:UserOperations
    {
       
        private AddUserOperations(){}
        
        
        public static AddUserOperations CreateUserOperations(string login, string name, string surName)
        {
            AddUserOperations add = new AddUserOperations();
            add.KeyUser = Guid.NewGuid().ToString();
            add.KeyUserVersion = Guid.NewGuid().ToString();
            add.Login = login;
            add.Name = name;
            add.SurName = surName;
            add.IsAdd = true;
            add.IsDelete = false;
            add.IsChange = false;
            add.OperationDate = DateTime.Now;
            return add;
        }
    }
    
    public class ChangeUserOperations:UserOperations
    {
        
        private ChangeUserOperations(){}
        public static ChangeUserOperations CreateUserOperations(User user)
        {
            if (user.IsDeleted)
               throw new Exception($"Can't change deleted user {user.Key}");
            
            
            ChangeUserOperations change = new ChangeUserOperations();
            change.KeyUser = user.Key;
            change.KeyUserVersion = Guid.NewGuid().ToString();
            
            change.Login = user.Login;
            change.Name = user.Name;
            change.SurName = user.SurName;
            change.IsAdd = false;
            change.IsDelete = false;
            change.IsChange = true;
            change.OperationDate = DateTime.Now;
            return change;
        }

        public ChangeUserOperations ChangeLogin(string login)
        {
            this.Login = login;
            return this;
        }
        public ChangeUserOperations ChangeName(string name)
        {
            this.Name = name;
            return this;
        }
        
        public ChangeUserOperations ChangeSurName(string surName)
        {
            this.SurName = surName;
            return this;
        }
        
    }

    public class DeleteUserOperations : UserOperations
    {
        private DeleteUserOperations(){}
        
        public static DeleteUserOperations CreateUserOperations(User user)
        {
            if (user.IsDeleted)
                throw new Exception($"Can't delete deleted user {user.Key}");
            
            DeleteUserOperations delete = new  DeleteUserOperations();
            delete.KeyUser = user.Key;
            delete.KeyUserVersion = Guid.NewGuid().ToString();
            
            delete.Login = user.Login;
            delete.Name = user.Name;
            delete.SurName = user.SurName;
            delete.IsAdd = false;
            delete.IsDelete = true;
            delete.IsChange = false;
            delete.OperationDate = DateTime.Now;
            return delete;
        }
    }











}