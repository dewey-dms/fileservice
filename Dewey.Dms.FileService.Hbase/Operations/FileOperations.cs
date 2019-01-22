using System;
using System.Security.Cryptography;
using Dewey.Dms.FileService.Hbase.Views;

namespace Dewey.Dms.FileService.Hbase.Operations
{
    public class FileOperations
    {
        public string Key
        {
            get => $"{KeyFile}|{KeyUser}|{KeyFileVersion}";
        }
        
        public string KeyFileUser
        {
            get => $"{KeyFile}|{KeyUser}";
        }

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
        }

    public class AddFileOperations:FileOperations
    {
        public static AddFileOperations CreateFileOperations(User user, string fileName, string extension)
        {
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
            return add;
        }
    }

    public class ChangeFileOperations : FileOperations
    {

        public static ChangeFileOperations CreateFileOperations(File file)
        {
            if (file.IsDeleted)
                throw new Exception($"Can't change deleted file {file.Key}");
            
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
            return change;
        }

        public ChangeFileOperations ChangeFileName(string fileName)
        {
            this.FileName = fileName;
            return this;
        }

        public ChangeFileOperations ChangeExtension(string extension)
        {
            this.Extension = extension;
            return this;
        }
    }
    
    
    public class DeleteFileOperations : FileOperations
    {
        
        

        public static DeleteFileOperations CreateFileOperations(File file)
        {
            if (file.IsDeleted)
                throw new Exception($"Can't delete deleted file {file.Key}");
            
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
            return delete;
        }

        
    }
    
    
    public class CloneFileOperations : FileOperations
    {

        public static CloneFileOperations CreateFileOperations(File file , User user)
        {
            if (file.IsDeleted)
                throw new Exception($"Can't clone deleted file {file.Key}");
            
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
            return clone;
        }

        
    }
    
}


