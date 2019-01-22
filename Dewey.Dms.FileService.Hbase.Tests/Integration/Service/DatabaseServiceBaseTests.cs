using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Dewey.Dms.FileService.Hbase.Operations;
using Dewey.Dms.FileService.Hbase.Service;
using Dewey.Dms.FileService.Hbase.Views;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dewey.Dms.FileService.Hive.Tests.Integration
{
    //[TestClass]
    public class DatabaseServiceBaseTests
    {
        protected IDatabaseService sut;

        protected int? SleepBetweenOperation = null;
        //private static string connectionString = "Driver=Hive;Host=localhost;Port=10000";


       
        
        public DatabaseServiceBaseTests(IDatabaseService sut )
        {
            this.sut = sut;
            //this.SleepBetweenOperation = sleepBetweenOperation;
        }
        
        
       // [TestMethod]
        public void GetUsers_RunAddUser_AndResultListUser()
        {
            
            // Assign
            string login = "Login";
            string name = "Name";
            string surName = "SurName";
            //HiveDatabaseService databaseService = new HiveDatabaseService(connectionString);
            AddUserOperations addOperations = AddUserOperations.CreateUserOperations(login, name, surName);
            bool resultAdd = sut.DoUserOperations(addOperations);
            
            // Act
            List<User> users =  sut.GetUsers();
            
            // Assert
            Assert.IsTrue(resultAdd);
            Assert.IsTrue(users.Count>0);
            User user = users.Where(a => a.Key == addOperations.KeyUser).SingleOrDefault();
            Assert.IsNotNull(user);
            Assert.AreEqual(login,user.Login);
            Assert.AreEqual(name,user.Name);
            Assert.AreEqual(surName,user.SurName);
            Assert.AreEqual(addOperations.Key,user.LongKey);
            Assert.IsFalse(user.IsDeleted);

        }

         
       // [TestMethod]
        public void AddUser_RunAddUser_AndResultNewKey()
        {
            
            //Assign
            
            
            
            string login = "Login";
            string name = "Name";
            string surName = "SurName";
            AddUserOperations operations = AddUserOperations.CreateUserOperations(login, name, surName);
            
            //HiveDatabaseService databaseService = new HiveDatabaseService(connectionString);
            
            //Act
            bool result = sut.DoUserOperations(operations);
            
            //Assert
            Assert.IsTrue(result);
            
        }
        
        
        
        
       // [TestMethod]
        public void GetUser_RunAddUser_AndResultUser()
        {
            //Assign
            string login = "Login";
            string name = "Name";
            string surName = "SurName";
           // HiveDatabaseService databaseService = new  HiveDatabaseService(connectionString);
            AddUserOperations addOperations = AddUserOperations.CreateUserOperations(login, name, surName);
            bool resultAdd = sut.DoUserOperations(addOperations);
            
            
            //Act
            User user = sut.GetUser(addOperations.KeyUser);
            
            //Assert
            Assert.IsTrue(resultAdd);
            Assert.AreEqual(login, user.Login);
            Assert.AreEqual(name,user.Name);
            Assert.AreEqual(surName,user.SurName);
            Assert.AreEqual(addOperations.Key,user.LongKey);
            Assert.IsFalse(user.IsDeleted);
           
         }
        
      //  [TestMethod]
        public void DeleteUser_RunAddUserAndDeleteUser_AndResultDeletedUser()
        {
            //Assign
            string login = "Login";
            string name = "Name";
            string surName = "SurName";
           // HiveDatabaseService databaseService = new  HiveDatabaseService(connectionString);
            AddUserOperations addOperations = AddUserOperations.CreateUserOperations(login, name, surName);
            bool resultAdd = sut.DoUserOperations(addOperations);
            User user = sut.GetUser(addOperations.KeyUser);

            DeleteUserOperations deleteOperations = DeleteUserOperations.CreateUserOperations(user);
            
            
            //Act
            bool resultDelete = sut.DoUserOperations(deleteOperations);
            
            //Assert
            User userRead = sut.GetUser(addOperations.KeyUser);
            Assert.IsTrue(resultAdd);
            Assert.IsTrue(resultDelete);
           
            Assert.AreEqual(login, userRead.Login);
            Assert.AreEqual(name,userRead.Name);
            Assert.AreEqual(surName,userRead.SurName);
            Assert.AreEqual(deleteOperations.Key,userRead.LongKey);
            Assert.IsTrue(userRead.IsDeleted);
           
           
        }

       // [TestMethod]
        public void ChangeUser_RunAddUserAndChangeUser_AndResultChangedUser()
        {
            //Assert
            string login = "Login";
            string name = "Name";
            string surName = "SurName";
            //HiveDatabaseService databaseService = new  HiveDatabaseService(connectionString);
            AddUserOperations addOperations = AddUserOperations.CreateUserOperations(login, name, surName);
            bool resultAdd = sut.DoUserOperations(addOperations);
            User user = sut.GetUser(addOperations.KeyUser);
            
            
            string newLogin = "Login2";
            string newName = "Name2";
            string newSurName = "SurName2";
            ChangeUserOperations changeOperations = ChangeUserOperations.CreateUserOperations(user)
                .ChangeName(newName)
                .ChangeLogin(newLogin)
                .ChangeSurName(newSurName);
            
            //Act
            bool resultChange = sut.DoUserOperations(changeOperations);
            
            //Assert
            User userRead = sut.GetUser(addOperations.KeyUser);
            Assert.AreEqual(newLogin, userRead.Login);
            Assert.AreEqual(newName,userRead.Name);
            Assert.AreEqual(newSurName,userRead.SurName);
            Assert.AreEqual(changeOperations.Key,userRead.LongKey);
            Assert.IsFalse(user.IsDeleted);
           
        }
        
        
       // [TestMethod]
        public void AddFile_RunAddUserAndAddFile_AndResultNewKey()
        {
            
            //Assign
            
            
            
            string login = "Login";
            string name = "Name";
            string surName = "SurName";
            
            AddUserOperations addUserOperations = AddUserOperations.CreateUserOperations(login, name, surName);
            //HiveDatabaseService databaseService = new  HiveDatabaseService(connectionString);
            bool resultAddUser = sut.DoUserOperations(addUserOperations);
            User user = sut.GetUser(addUserOperations.KeyUser);


            string fileName = "filename";
            string extension = "extension";
            AddFileOperations addFileOperations = AddFileOperations.CreateFileOperations(user, fileName, extension);
            
            //Act
            bool resultAddFile = sut.DoFileOperations(addFileOperations);
            
            //Assert
            Assert.IsTrue(resultAddUser);
            Assert.IsNotNull(user);
            Assert.IsTrue(resultAddFile);
            
        }
        
        
       // [TestMethod]
        public void GetFile_RunAddUserAndAddFile_AndResultListFile()
        {
            
            //Assign
            
            
            
            string login = "Login";
            string name = "Name";
            string surName = "SurName";
            
            AddUserOperations addUserOperations = AddUserOperations.CreateUserOperations(login, name, surName);
            //HiveDatabaseService databaseService = new  HiveDatabaseService(connectionString);
            bool resultAddUser = sut.DoUserOperations(addUserOperations);
            User user = sut.GetUser(addUserOperations.KeyUser);


            string fileName = "filename";
            string extension = "extension";
            AddFileOperations addFileOperations = AddFileOperations.CreateFileOperations(user, fileName, extension);
            //Act
            bool resultAddFile = sut.DoFileOperations(addFileOperations);
            
            //Assert
            File file = sut.GetFile(addFileOperations.KeyFileUser);
            
            
            
            //Assert
            Assert.IsTrue(resultAddUser);
            Assert.IsNotNull(user);
            Assert.IsTrue(resultAddFile);
            Assert.IsNotNull(file);
            Assert.AreEqual(fileName, file.FileName);
            Assert.AreEqual(extension,file.Extension);
            Assert.AreEqual(addFileOperations.KeyUser,file.KeyUser);
            Assert.AreEqual(addFileOperations.KeyFileUser,  file.Key);
            Assert.AreEqual(addFileOperations.Key, file.LongKey);
            Assert.IsFalse(file.IsDeleted);

        }
        
      //  [TestMethod]
        public void ChangeFile_RunAddUserAndAddFileAndChangeFile_AndResultChangedFile()
        {
            string login = "Login";
            string name = "Name";
            string surName = "SurName";
            
            AddUserOperations addUserOperations = AddUserOperations.CreateUserOperations(login, name, surName);
           // HiveDatabaseService databaseService = new  HiveDatabaseService(connectionString);
            bool resultAddUser = sut.DoUserOperations(addUserOperations);
            User user = sut.GetUser(addUserOperations.KeyUser);


            string fileName = "filename";
            string extension = "extension";
            AddFileOperations addFileOperations = AddFileOperations.CreateFileOperations(user, fileName, extension);
            bool resultAddFile = sut.DoFileOperations(addFileOperations);
           
            File file = sut.GetFile(addFileOperations.KeyFileUser);
            string newFileName = "filename2";
            string newExtension = "extension2";
            ChangeFileOperations changeFileOperations = ChangeFileOperations.CreateFileOperations(file)
                .ChangeFileName(newFileName).ChangeExtension(newExtension);

            //Act
            bool resultChangeFile = sut.DoFileOperations(changeFileOperations);

            // Assert
            File fileRead = sut.GetFile(changeFileOperations.KeyFileUser);
            Assert.IsTrue(resultAddUser);
            Assert.IsTrue(resultAddFile);
            Assert.IsTrue(resultChangeFile);
            
            Assert.AreEqual(newFileName, fileRead.FileName);
            Assert.AreEqual(newExtension,fileRead.Extension);
           
            Assert.AreEqual(changeFileOperations.KeyUser,fileRead.KeyUser);
            Assert.AreEqual(changeFileOperations.KeyFileUser,  fileRead.Key);
            Assert.AreEqual(changeFileOperations.Key, fileRead.LongKey);
            Assert.IsFalse(fileRead.IsDeleted);
        }
        
        
        
       // [TestMethod]
        public void DeleteFile_RunAddUserAndAddFileAndDeleteFile_AndResultDeleteFile()
        {
            string login = "Login";
            string name = "Name";
            string surName = "SurName";
            
            AddUserOperations addUserOperations = AddUserOperations.CreateUserOperations(login, name, surName);
            //HiveDatabaseService databaseService = new  HiveDatabaseService(connectionString);
            bool resultAddUser = sut.DoUserOperations(addUserOperations);
            User user = sut.GetUser(addUserOperations.KeyUser);


            string fileName = "filename";
            string extension = "extension";
            AddFileOperations addFileOperations = AddFileOperations.CreateFileOperations(user, fileName, extension);
            bool resultAddFile = sut.DoFileOperations(addFileOperations);
           
            File file = sut.GetFile(addFileOperations.KeyFileUser);
           
            DeleteFileOperations deleteFileOperations = DeleteFileOperations.CreateFileOperations(file);
                

            //Act
            bool resultDeleteFile = sut.DoFileOperations(deleteFileOperations);

            // Assert
            File fileRead = sut.GetFile(deleteFileOperations.KeyFileUser);
            Assert.IsTrue(resultAddUser);
            Assert.IsTrue(resultAddFile);
            Assert.IsTrue(resultDeleteFile);
            
            Assert.AreEqual(fileName, fileRead.FileName);
            Assert.AreEqual(extension,fileRead.Extension);
           
            Assert.AreEqual(deleteFileOperations.KeyUser,fileRead.KeyUser);
            Assert.AreEqual(deleteFileOperations.KeyFileUser,  fileRead.Key);
            Assert.AreEqual(deleteFileOperations.Key, fileRead.LongKey);
            Assert.IsTrue(fileRead.IsDeleted);
        }


       // [TestMethod]
        public void CloneFile_RunAddUserAndAddFileAndCloneFile_AndResultDeleteFile()
        {
            string login = "Login";
            string name = "Name";
            string surName = "SurName";
            
            AddUserOperations addUserOperations = AddUserOperations.CreateUserOperations(login, name, surName);
            //HiveDatabaseService databaseService = new  HiveDatabaseService(connectionString);
            bool resultAddUser = sut.DoUserOperations(addUserOperations);
            User user = sut.GetUser(addUserOperations.KeyUser);


            AddUserOperations addUserOperationsOther = AddUserOperations.CreateUserOperations(login, name, surName);
          
            bool resultAddUserOther = sut.DoUserOperations(addUserOperationsOther);
            User userOther = sut.GetUser(addUserOperationsOther.KeyUser);
            
            
            string fileName = "filename";
            string extension = "extension";
            AddFileOperations addFileOperations = AddFileOperations.CreateFileOperations(user, fileName, extension);
            bool resultAddFile = sut.DoFileOperations(addFileOperations);
           
            File file = sut.GetFile(addFileOperations.KeyFileUser);

            CloneFileOperations cloneFileOperations = CloneFileOperations.CreateFileOperations(file, userOther);
                

            //Act
            bool resultCloneFile = sut.DoFileOperations(cloneFileOperations);

            // Assert
            File fileRead = sut.GetFile(cloneFileOperations.KeyFileUser);
            Assert.IsTrue(resultAddUser);
            
            
            Assert.IsTrue(resultAddFile);
            Assert.IsTrue(resultAddUserOther);
            Assert.IsTrue(resultCloneFile);
            
            Assert.AreEqual(fileName, fileRead.FileName);
            Assert.AreEqual(extension,fileRead.Extension);
           
            Assert.AreEqual(cloneFileOperations.KeyUser,fileRead.KeyUser);
            Assert.AreEqual(cloneFileOperations.KeyFileUser,  fileRead.Key);
            Assert.AreEqual(cloneFileOperations.Key, fileRead.LongKey);
            Assert.IsFalse(fileRead.IsDeleted);
            Assert.AreEqual(addFileOperations.Key,  fileRead.Parent);
        }
    }
}