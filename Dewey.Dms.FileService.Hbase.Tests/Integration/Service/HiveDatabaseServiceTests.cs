using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Dewey.Dms.FileService.Hbase.Service;

namespace Dewey.Dms.FileService.Hive.Tests.Integration
{
    [TestClass]
    public class HiveDatabaseServiceTests:DatabaseServiceBaseTests
    {

        private static string connectionString = "Driver=Hive;Host=localhost;Port=10000";


        public HiveDatabaseServiceTests() : base(new HiveDatabaseService(connectionString)){}
        
        
        [TestMethod]
        public new void GetUsers_RunAddUser_AndResultListUser()
        {
            base.GetUsers_RunAddUser_AndResultListUser();
        }

         
        [TestMethod]
        public new void AddUser_RunAddUser_AndResultNewKey()
        {
           base.AddUser_RunAddUser_AndResultNewKey();
        }
        
        
        
        
        [TestMethod]
        public new void GetUser_RunAddUser_AndResultUser()
        {
            base.GetUser_RunAddUser_AndResultUser();

        }
        
        [TestMethod]
        public new void DeleteUser_RunAddUserAndDeleteUser_AndResultDeletedUser()
        {
            base.DeleteUser_RunAddUserAndDeleteUser_AndResultDeletedUser();


        }

        [TestMethod]
        public new void ChangeUser_RunAddUserAndChangeUser_AndResultChangedUser()
        {
            base.ChangeUser_RunAddUserAndChangeUser_AndResultChangedUser();

        }
        
        
        [TestMethod]
        public new void AddFile_RunAddUserAndAddFile_AndResultNewKey()
        {
             base.AddFile_RunAddUserAndAddFile_AndResultNewKey();
        }
        
        
        [TestMethod]
        public new void GetFile_RunAddUserAndAddFile_AndResultListFile()
        {
            
          base.GetFile_RunAddUserAndAddFile_AndResultListFile();
        }
        
        [TestMethod]
        public new void ChangeFile_RunAddUserAndAddFileAndChangeFile_AndResultChangedFile()
        {
            base.ChangeFile_RunAddUserAndAddFileAndChangeFile_AndResultChangedFile();
        }
        
        
        
        [TestMethod]
        public new void DeleteFile_RunAddUserAndAddFileAndDeleteFile_AndResultDeleteFile()
        {
            base.DeleteFile_RunAddUserAndAddFileAndDeleteFile_AndResultDeleteFile();
        }


        [TestMethod] 
        public new void CloneFile_RunAddUserAndAddFileAndCloneFile_AndResultDeleteFile()
        {
            base.CloneFile_RunAddUserAndAddFileAndCloneFile_AndResultDeleteFile();
        }

        [TestMethod] 
        public new void GetFileUser_RunAddUserAndAddFile_AndResultListFile()
        {
            base.GetFileUser_RunAddUserAndAddFile_AndResultListFile();
        }
        
    }
}