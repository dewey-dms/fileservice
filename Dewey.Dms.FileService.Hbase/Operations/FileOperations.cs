using System;
using System.Security.Cryptography;
using Dewey.Dms.FileService.Hbase.Views;
using Dewey.Dms.Core;

namespace Dewey.Dms.FileService.Hbase.Operations
{
    public class FileOperations
    {
        public string Key
        {
            get => $"{KeyUser}|{KeyFile}|{KeyFileVersion}";
        }
        
        public string KeyUserFile
        {
            get => $"{KeyUser}|{KeyFile}";
        }   
            
// $"{KeyFile}|{KeyUser}";
        

        public string KeyUser { get; protected set; }
        public string KeyFile { get; protected set; }
        public string KeyFileVersion { get; protected set; }
        
        public string FileName { get; protected set; }
        public string Extension { get; protected set; }
        public string Parent { get; protected set; }
        public bool IsAdd { get; protected set; }
        public bool IsChange { get; protected set; }
        public bool IsDelete { get; protected set; }
        public bool IsClone { get; protected set; }
        public DateTime OperationDate { get; protected set; }
        
        
        public long OrderBy { get; private set; }

        public void CreateOrderBy()
        {
            OrderBy =  (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds;
        }
        
      }

     
    
    
    public class AddFileOperations:FileOperations
    {
        public static ResultService<AddFileOperations> CreateFileOperations(User user, string fileName, string extension)
        {
            if (string.IsNullOrEmpty(fileName))
                return ResultService<AddFileOperations>.Error($"Filename is empty");
            
            if (string.IsNullOrEmpty(extension))
                return ResultService<AddFileOperations>.Error($"Extension is empty");
            
            AddFileOperations add = new AddFileOperations();
            add.KeyUser = user.Key;
            add.KeyFile = Guid.NewGuid().ToString();
            add.KeyFileVersion = Guid.NewGuid().ToString();
            add.FileName = fileName;
            add.Extension = extension;
            add.Parent = null;
            add.IsAdd = true;
            add.IsChange = false;
            add.IsClone = false;
            add.IsDelete = false;
            add.OperationDate = DateTime.Now;
            add.CreateOrderBy();
            return ResultService<AddFileOperations>.Ok(add);
        }
    }

    public class ChangeFileOperations : FileOperations
    {

        public static ResultService<ChangeFileOperations> CreateFileOperations(File file )
        {
            if (file == null)
                return ResultService<ChangeFileOperations>.Error($"File parametr is null");
            
            if (file.IsDeleted)
                //throw new Exception($"Can't change deleted file {file.Key}");
                return ResultService<ChangeFileOperations>.Error($"Can't change deleted file {file.Key}");
            
            ChangeFileOperations change = new ChangeFileOperations();
            change.KeyUser = file.KeyUser;
            change.KeyFile = file.KeyFile;
            change.KeyFileVersion = Guid.NewGuid().ToString();
            change.FileName = file.FileName;
            change.Extension = file.Extension;
            change.Parent = null;
            change.IsAdd = false;
            change.IsChange = true;
            change.IsClone = false;
            change.IsDelete = false;
            change.OperationDate = DateTime.Now;
            change.CreateOrderBy();
            return ResultService<ChangeFileOperations>.Ok(change);
        }

        public ResultService<ChangeFileOperations> ChangeFileName(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return ResultService<ChangeFileOperations>.Error($"Filename is null");
            this.FileName = fileName;
            
            return ResultService<ChangeFileOperations>.Ok(this);
        }

        public ResultService<ChangeFileOperations> ChangeExtension(string extension)
        {
            if (string.IsNullOrEmpty(extension))
                return ResultService<ChangeFileOperations>.Error($"Extension is null");
            
            this.Extension = extension;
            return ResultService<ChangeFileOperations>.Ok(this);
        }
    }
    
    
    public class DeleteFileOperations : FileOperations
    {
        
        

        public static ResultService<DeleteFileOperations> CreateFileOperations(File file)
        {
            
            if (file == null)
                return ResultService<DeleteFileOperations>.Error($"File parametr is null");
            
            
            if (file.IsDeleted)
                ResultService<DeleteFileOperations>.Error($"Can't delete deleted file {file.Key}");
            
            DeleteFileOperations delete = new DeleteFileOperations();
            delete.KeyUser = file.KeyUser;
            delete.KeyFile = file.KeyFile;
            delete.KeyFileVersion = Guid.NewGuid().ToString();
            delete.FileName = file.FileName;
            delete.Extension = file.Extension;
            delete.Parent = null;
            delete.IsAdd = false;
            delete.IsChange = false;
            delete.IsClone = false;
            delete.IsDelete = true;
            delete.OperationDate = DateTime.Now;
            delete.CreateOrderBy();
            return  ResultService<DeleteFileOperations>.Ok(delete);
        }

        
    }
    
    
    public class CloneFileOperations : FileOperations
    {

        public static ResultService<CloneFileOperations> CreateFileOperations(File file , User user)
        {
            
            if (file == null)
                return ResultService<CloneFileOperations>.Error($"File parametr is null");
            
            if (file.IsDeleted)
                //    throw new Exception($"Can't clone deleted file {file.Key}");
                return ResultService<CloneFileOperations>.Error($"Can't clone deleted file {file.Key}");
            
            CloneFileOperations clone = new CloneFileOperations();
            clone.KeyUser = user.Key;
            clone.KeyFile = Guid.NewGuid().ToString();
            clone.KeyFileVersion = Guid.NewGuid().ToString();
            clone.FileName = file.FileName;
            clone.Extension = file.Extension;
            clone.Parent = file.LongKey;
            clone.IsAdd = false;
            clone.IsChange = false;
            clone.IsClone = true;
            clone.IsDelete = false;
            clone.OperationDate = DateTime.Now;
            clone.CreateOrderBy();
            return ResultService<CloneFileOperations>.Ok(clone);
        }

        
    }
    
}


