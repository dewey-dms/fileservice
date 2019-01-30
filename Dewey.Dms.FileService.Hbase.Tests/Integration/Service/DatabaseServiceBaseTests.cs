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
            string password = "Password";
            //HiveDatabaseService databaseService = new HiveDatabaseService(connectionString);
            AddUserOperations addOperations = AddUserOperations.CreateUserOperations(login, password,name, surName);
            sut.DoUserOperations(addOperations).Wait();
            
            // Act
            List<User> users =  sut.GetUsers().Result.ToList();
            
            // Assert
            //Assert.IsTrue(resultAdd);
            Assert.IsTrue(users.Count>0);
            User user = users.Where(a => a.Key == addOperations.KeyUser).SingleOrDefault();
            Assert.IsNotNull(user);
            Assert.AreEqual(login,user.Login);
            Assert.AreEqual(name,user.Name);
            Assert.AreEqual(surName,user.SurName);
            Assert.AreEqual(addOperations.Key,user.LongKey);
            Assert.AreEqual(password,user.Password);
            Assert.IsFalse(user.IsDeleted);

        }

         
       // [TestMethod]
        public void AddUser_RunAddUser_AndResultNewKey()
        {
            
            //Assign
            
            
            
            string login = "Login";
            string name = "Name";
            string surName = "SurName";
            string password = "Password";
            AddUserOperations operations = AddUserOperations.CreateUserOperations(login, password, name, surName);
            
            //HiveDatabaseService databaseService = new HiveDatabaseService(connectionString);
            
            //Act
            sut.DoUserOperations(operations).Wait();;
            
            //Assert
            //Assert.IsTrue(result);
            
        }
        
        
        
        
       // [TestMethod]
        public void GetUser_RunAddUser_AndResultUser()
        {
            //Assign
            string login = "Login";
            string name = "Name";
            string surName = "SurName";
            string password = "Password";
           // HiveDatabaseService databaseService = new  HiveDatabaseService(connectionString);
            AddUserOperations addOperations = AddUserOperations.CreateUserOperations(login, password, name, surName);
            sut.DoUserOperations(addOperations).Wait();;
            
            
            //Act
            User user = sut.GetUser(addOperations.KeyUser).Result;
            
            //Assert
            
            Assert.AreEqual(login, user.Login);
            Assert.AreEqual(name,user.Name);
            Assert.AreEqual(surName,user.SurName);
            Assert.AreEqual(addOperations.Key,user.LongKey);
            Assert.AreEqual(password,user.Password);
            Assert.IsFalse(user.IsDeleted);
           
         }
        
      //  [TestMethod]
        public void DeleteUser_RunAddUserAndDeleteUser_AndResultDeletedUser()
        {
            //Assign
            string login = "Login";
            string name = "Name";
            string surName = "SurName";
            string password = "Password";
           // HiveDatabaseService databaseService = new  HiveDatabaseService(connectionString);
            AddUserOperations addOperations = AddUserOperations.CreateUserOperations(login, password, name, surName);
            sut.DoUserOperations(addOperations).Wait();;
            User user = sut.GetUser(addOperations.KeyUser).Result;

            DeleteUserOperations deleteOperations = DeleteUserOperations.CreateUserOperations(user);
            
            
            //Act
             sut.DoUserOperations(deleteOperations);
            
            //Assert
            User userRead = sut.GetUser(addOperations.KeyUser).Result;
           
           
            Assert.AreEqual(login, userRead.Login);
            Assert.AreEqual(name,userRead.Name);
            Assert.AreEqual(surName,userRead.SurName);
            Assert.AreEqual(deleteOperations.Key,userRead.LongKey);
            Assert.AreEqual(password,user.Password);
            Assert.IsTrue(userRead.IsDeleted);
           
           
        }

       // [TestMethod]
        public void ChangeUser_RunAddUserAndChangeUser_AndResultChangedUser()
        {
            //Assert
            string login = "Login";
            string name = "Name";
            string surName = "SurName";
            string password = "Password";
            //HiveDatabaseService databaseService = new  HiveDatabaseService(connectionString);
            AddUserOperations addOperations = AddUserOperations.CreateUserOperations(login, password,name, surName);
          sut.DoUserOperations(addOperations).Wait();;
            User user = sut.GetUser(addOperations.KeyUser).Result;
            
            
            string newLogin = "Login2";
            string newPassword = "Password2";
            string newName = "Name2";
            string newSurName = "SurName2";
            
            ChangeUserOperations changeOperations = ChangeUserOperations.CreateUserOperations(user)
                .ChangeName(newName)
                .ChangeLogin(newLogin)
                .ChangePassword(newPassword)
                .ChangeSurName(newSurName);
            
            //Act
           sut.DoUserOperations(changeOperations).Wait();;
            
            //Assert
            User userRead = sut.GetUser(addOperations.KeyUser).Result;
            Assert.AreEqual(newLogin, userRead.Login);
            Assert.AreEqual(newName,userRead.Name);
            Assert.AreEqual(newSurName,userRead.SurName);
            Assert.AreEqual(changeOperations.Key,userRead.LongKey);
            Assert.AreEqual(password,user.Password);
            Assert.IsFalse(user.IsDeleted);
           
        }
        
        
       // [TestMethod]
        public void AddFile_RunAddUserAndAddFile_AndResultNewKey()
        {
            
            //Assign
            
            
            
            string login = "Login";
            string name = "Name";
            string surName = "SurName";
            string password = "Password";
            
            AddUserOperations addUserOperations = AddUserOperations.CreateUserOperations(login, password,name, surName);
            //HiveDatabaseService databaseService = new  HiveDatabaseService(connectionString);
            sut.DoUserOperations(addUserOperations).Wait();;
            User user = sut.GetUser(addUserOperations.KeyUser).Result;


            string fileName = "filename";
            string extension = "extension";
            AddFileOperations addFileOperations = AddFileOperations.CreateFileOperations(user, fileName, extension);
            
            //Act
            sut.DoFileOperations(addFileOperations).Wait();;
            
            //Assert
            Assert.IsNotNull(user);
           
            
        }
        
        
       // [TestMethod]
        public void GetFile_RunAddUserAndAddFile_AndResultListFile()
        {
            
            //Assign
            
            
            
            string login = "Login";
            string name = "Name";
            string surName = "SurName";
            string password = "Password";
            
            AddUserOperations addUserOperations = AddUserOperations.CreateUserOperations(login, password, name, surName);
            //HiveDatabaseService databaseService = new  HiveDatabaseService(connectionString);
            sut.DoUserOperations(addUserOperations).Wait();;
            User user = sut.GetUser(addUserOperations.KeyUser).Result;


            string fileName = "filename";
            string extension = "extension";
            AddFileOperations addFileOperations = AddFileOperations.CreateFileOperations(user, fileName, extension);
            //Act
            sut.DoFileOperations(addFileOperations).Wait();;
            
            //Assert
            File file = sut.GetFile(addFileOperations.KeyUserFile).Result;
            
            
            
            //Assert
            Assert.IsNotNull(user);
            Assert.IsNotNull(file);
            Assert.AreEqual(fileName, file.FileName);
            Assert.AreEqual(extension,file.Extension);
            Assert.AreEqual(addFileOperations.KeyUser,file.KeyUser);
            Assert.AreEqual(addFileOperations.KeyUserFile,  file.Key);
            Assert.AreEqual(addFileOperations.Key, file.LongKey);
            Assert.IsFalse(file.IsDeleted);
            Assert.IsFalse(file.IsCloned);

        }
        
      //  [TestMethod]
        public void ChangeFile_RunAddUserAndAddFileAndChangeFile_AndResultChangedFile()
        {
            string login = "Login";
            string name = "Name";
            string surName = "SurName";
            string password = "Password";
            
            AddUserOperations addUserOperations = AddUserOperations.CreateUserOperations(login, password,name, surName);
           // HiveDatabaseService databaseService = new  HiveDatabaseService(connectionString);
            sut.DoUserOperations(addUserOperations).Wait();;
            User user = sut.GetUser(addUserOperations.KeyUser).Result;


            string fileName = "filename";
            string extension = "extension";
            AddFileOperations addFileOperations = AddFileOperations.CreateFileOperations(user, fileName, extension);
            sut.DoFileOperations(addFileOperations).Wait();;
           
            File file = sut.GetFile(addFileOperations.KeyUserFile).Result;
            string newFileName = "filename2";
            string newExtension = "extension2";
            ChangeFileOperations changeFileOperations = ChangeFileOperations.CreateFileOperations(file)
                .ChangeFileName(newFileName).ChangeExtension(newExtension);

            //Act
            sut.DoFileOperations(changeFileOperations).Wait();;

            // Assert
            File fileRead = sut.GetFile(changeFileOperations.KeyUserFile).Result;
           
            Assert.AreEqual(newFileName, fileRead.FileName);
            Assert.AreEqual(newExtension,fileRead.Extension);
           
            Assert.AreEqual(changeFileOperations.KeyUser,fileRead.KeyUser);
            Assert.AreEqual(changeFileOperations.KeyUserFile,  fileRead.Key);
            Assert.AreEqual(changeFileOperations.Key, fileRead.LongKey);
            Assert.IsFalse(fileRead.IsDeleted);
            Assert.IsFalse(fileRead.IsCloned);
        }
        
        
        
       // [TestMethod]
        public void DeleteFile_RunAddUserAndAddFileAndDeleteFile_AndResultDeleteFile()
        {
            string login = "Login";
            string name = "Name";
            string surName = "SurName";
            string password = "Password";
            
            AddUserOperations addUserOperations = AddUserOperations.CreateUserOperations(login, password,name, surName);
            //HiveDatabaseService databaseService = new  HiveDatabaseService(connectionString);
            sut.DoUserOperations(addUserOperations).Wait();;
            User user = sut.GetUser(addUserOperations.KeyUser).Result;


            string fileName = "filename";
            string extension = "extension";
            AddFileOperations addFileOperations = AddFileOperations.CreateFileOperations(user, fileName, extension);
             sut.DoFileOperations(addFileOperations).Wait();;
           
            File file = sut.GetFile(addFileOperations.KeyUserFile).Result;
           
            DeleteFileOperations deleteFileOperations = DeleteFileOperations.CreateFileOperations(file);
                

            //Act
           sut.DoFileOperations(deleteFileOperations).Wait();

            // Assert
            File fileRead = sut.GetFile(deleteFileOperations.KeyUserFile).Result;
           
            
            Assert.AreEqual(fileName, fileRead.FileName);
            Assert.AreEqual(extension,fileRead.Extension);
           
            Assert.AreEqual(deleteFileOperations.KeyUser,fileRead.KeyUser);
            Assert.AreEqual(deleteFileOperations.KeyUserFile,  fileRead.Key);
            Assert.AreEqual(deleteFileOperations.Key, fileRead.LongKey);
            Assert.IsTrue(fileRead.IsDeleted);
            Assert.IsFalse(fileRead.IsCloned);
        }


       // [TestMethod]
        public void CloneFile_RunAddUserAndAddFileAndCloneFile_AndResultDeleteFile()
        {
            string login = "Login";
            string name = "Name";
            string surName = "SurName";
            string password = "Password";
            
            AddUserOperations addUserOperations = AddUserOperations.CreateUserOperations(login, password, name, surName);
            //HiveDatabaseService databaseService = new  HiveDatabaseService(connectionString);
            sut.DoUserOperations(addUserOperations).Wait();;
            User user = sut.GetUser(addUserOperations.KeyUser).Result;


            AddUserOperations addUserOperationsOther = AddUserOperations.CreateUserOperations(login, password , name, surName);
          
            sut.DoUserOperations(addUserOperationsOther).Wait();;
            User userOther = sut.GetUser(addUserOperationsOther.KeyUser).Result;
            
            
            string fileName = "filename";
            string extension = "extension";
            AddFileOperations addFileOperations = AddFileOperations.CreateFileOperations(user, fileName, extension);
             sut.DoFileOperations(addFileOperations).Wait();
           
            File file = sut.GetFile(addFileOperations.KeyUserFile).Result;

            CloneFileOperations cloneFileOperations = CloneFileOperations.CreateFileOperations(file, userOther);
                

            //Act
           sut.DoFileOperations(cloneFileOperations).Wait();

            // Assert
            File fileRead = sut.GetFile(cloneFileOperations.KeyUserFile).Result;
        
            
            Assert.AreEqual(fileName, fileRead.FileName);
            Assert.AreEqual(extension,fileRead.Extension);
           
            Assert.AreEqual(cloneFileOperations.KeyUser,fileRead.KeyUser);
            Assert.AreEqual(cloneFileOperations.KeyUserFile,  fileRead.Key);
            Assert.AreEqual(cloneFileOperations.Key, fileRead.LongKey);
            Assert.IsFalse(fileRead.IsDeleted);
            Assert.IsTrue(fileRead.IsCloned);
            Assert.AreEqual(addFileOperations.Key,  fileRead.Parent);
        }
        
        
        
        public void GetFileUser_RunAddUserAndAddFile_AndResultListFile()
        {
            
            //Assign
            
            
            
            string login = "Login";
            string name = "Name";
            string surName = "SurName";
            string password = "Password";
            
            AddUserOperations addUserOperations = AddUserOperations.CreateUserOperations(login, password, name, surName);
            //HiveDatabaseService databaseService = new  HiveDatabaseService(connectionString);
            sut.DoUserOperations(addUserOperations).Wait();;
            User user = sut.GetUser(addUserOperations.KeyUser).Result;


            string fileName = "filename";
            string extension = "extension";
            AddFileOperations addFileOperations = AddFileOperations.CreateFileOperations(user, fileName, extension);
            //Act
            sut.DoFileOperations(addFileOperations).Wait();;
            
            //Assert
            IEnumerable<File> files = sut.GetUserFile(addFileOperations.KeyUser).Result;
            
            
            
            //Assert
            Assert.IsNotNull(user);
            Assert.IsNotNull(files);
            File file = files.SingleOrDefault();

            Assert.IsNotNull(file);
            Assert.AreEqual(fileName, file.FileName);
            Assert.AreEqual(extension,file.Extension);
            Assert.AreEqual(addFileOperations.KeyUser,file.KeyUser);
            Assert.AreEqual(addFileOperations.KeyUserFile,  file.Key);
            Assert.AreEqual(addFileOperations.Key, file.LongKey);
            Assert.IsFalse(file.IsDeleted);
            Assert.IsFalse(file.IsCloned);

        }
        
    }
}