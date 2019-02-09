using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dewey.Dms.FileService.Hbase.Operations;
using Dewey.Dms.FileService.Hbase.Views;
using Dewey.HBase.Stargate.Client.Api;
using Dewey.HBase.Stargate.Client.Models;

namespace Dewey.Dms.FileService.Hbase.Service
{
    public class RestDatabaseService:IDatabaseService
    {
        public string BaseUrl { get; }
      
        public IStargate StargateApi;

        public RestDatabaseService(string BaseUrl )
        {
            this.BaseUrl = BaseUrl;
            this.StargateApi = Stargate.Create(BaseUrl);
          
        }

      
        
        public async Task DoUserOperations(UserOperations operations)
        {
            
            CellSet cellSet = 
                HbaseRowFactory.CreateFactory("dewey_users", operations.Key)
                .AddColumnStringValue("description", "login", operations.Login)
                .AddColumnStringValue("description", "password", operations.Password)
                .AddColumnStringValue("description", "name", operations.Name)
                .AddColumnStringValue("description", "surname", operations.SurName)
                .AddColumnBooleanValue("params", "is_add", operations.IsAdd)
                .AddColumnBooleanValue("params", "is_change", operations.IsChange)
                .AddColumnBooleanValue("params", "is_delete", operations.IsDelete)
                .AddColumnDateTimeValue("history", "operation_date", operations.OperationDate)
                .AddColumnLongValue("history", "orderby", operations.OrderBy)
                .MakeCellSet();
            
             //StargateApi.WriteValue(operations.Name,"dewey_users",operations.Key, "description","name");
           
             await StargateApi.WriteCellsAsync(cellSet);
            
             
        }

        public async Task<User> GetUser(string key)
        
        {

            List<UserHistory> userHistory =  new List<UserHistory>();
            
            //throw new System.NotImplementedException();
            var scannerOptions = new ScannerOptions
            {
                TableName = "dewey_users",
                Filter = new PrefixFilter(key)
            };

            using(IScanner scanner = await StargateApi.CreateScannerAsync(scannerOptions))
            {
           //     int count = scanner.Count();
           foreach (CellSet result in scanner)
           {
               userHistory.AddRange(
                    result.GroupBy(a => a.Identifier.Row).Select(a =>
                    new UserHistory(a.Key, new CellSet(a))
                ));
           }
          }
            return User.CreateUser(userHistory);
        }

        public async Task<IEnumerable<User>> GetUsers(bool? isDelete = null)
        {
            List<UserHistory> userHistory = new List<UserHistory>();

            //throw new System.NotImplementedException();
            var scannerOptions = new ScannerOptions
            {
                TableName = "dewey_users",
                Filter = new PrefixFilter(string.Empty)
            };

            using (IScanner scanner = await StargateApi.CreateScannerAsync(scannerOptions))
            {
                //     int count = scanner.Count();
                foreach (CellSet result in scanner)
                {
                    userHistory.AddRange(
                        result.GroupBy(a => a.Identifier.Row).Select(a =>
                            new UserHistory(a.Key, new CellSet(a))
                        ));
                }
            }

            IEnumerable<User> users =
                userHistory
                    .GroupBy(a => a.UserKey)
                    .Select(a => User.CreateUser(a.ToList()));

            if (isDelete.HasValue)
                users = users.Where(a => a.IsDeleted == isDelete);

            return users;

        }

        public async Task DoFileOperations(FileOperations operations)
        {
            HbaseRowFactory factory =
                HbaseRowFactory.CreateFactory("dewey_files", operations.Key)
                    .AddColumnStringValue("description", "filename", operations.FileName)
                    .AddColumnStringValue("description", "extension", operations.Extension)
                    .AddColumnBooleanValue("params", "is_add", operations.IsAdd)
                    .AddColumnBooleanValue("params", "is_change", operations.IsChange)
                    .AddColumnBooleanValue("params", "is_delete", operations.IsDelete)
                    .AddColumnBooleanValue("params", "is_clone", operations.IsClone)
                    .AddColumnDateTimeValue("history", "operation_date", operations.OperationDate)
                    .AddColumnLongValue("history", "orderby", operations.OrderBy);

            if (!string.IsNullOrEmpty(operations.Parent))
                factory = factory.AddColumnStringValue("params", "parent", operations.Parent);

            CellSet cellSet = factory.MakeCellSet();
           
            await StargateApi.WriteCellsAsync(cellSet);
            
            //return true;

        }

        public async Task<File> GetFile(string key)
        {
            List<FileHistory> fileHistory =  new List<FileHistory>();
            
            //throw new System.NotImplementedException();
            var scannerOptions = new ScannerOptions
            {
                TableName = "dewey_files",
                Filter = new PrefixFilter(key)
            };

            using(IScanner scanner = await StargateApi.CreateScannerAsync(scannerOptions))
            {
                //     int count = scanner.Count();
                foreach (CellSet result in scanner)
                {
                    fileHistory.AddRange(
                        result.GroupBy(a => a.Identifier.Row).Select(a =>
                            new FileHistory(a.Key, new CellSet(a))
                        ));
                }
            }

            return File.CreateFile(fileHistory);
        }

        public async Task<IEnumerable<File>> GetUserFile(string userKey, bool? isDelete = null)
        {
            List<FileHistory> fileHistory =  new List<FileHistory>();
            
            //throw new System.NotImplementedException();
            var scannerOptions = new ScannerOptions
            {
                TableName = "dewey_files",
                Filter = new PrefixFilter(userKey),
             };

            using(IScanner scanner = await StargateApi.CreateScannerAsync(scannerOptions))
            {
                //     int count = scanner.Count();
                foreach (CellSet result in scanner)
                {
                    fileHistory.AddRange(
                        result.GroupBy(a => a.Identifier.Row).Select(a =>
                            new FileHistory(a.Key, new CellSet(a))
                        ));
                }
                IEnumerable<File> files =
                    fileHistory
                        .GroupBy(a => a.KeyUserFile)
                        .Select(a => File.CreateFile(a.ToList()));
                    
                if (isDelete.HasValue)
                    files = files.Where(a => a.IsDeleted == isDelete);
                    
                return files;
            }

        }
    }
}