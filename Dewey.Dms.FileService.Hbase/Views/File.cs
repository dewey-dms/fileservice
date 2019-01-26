using System;
using System.Collections.Generic;
using System.Linq;

namespace Dewey.Dms.FileService.Hbase.Views
{
    public class File
    {
        public string LongKey
        {
            get => $"{Key}|{FileLastHistory.Key}";

        }

        public string Key
        {
            get => $"{KeyFile}|{KeyUser}";

        }

        public string KeyFile
        {
            get => FileLastHistory.KeyFile;


        }

        public string KeyUser
        {
            get => FileLastHistory.KeyUser;
        }



        public string Parent
        {
            get => History.Where(a=>a.IsClone).SingleOrDefault()?.Parent;
            
        }

        public string FileName
        {
            get => FileLastHistory.FileName;
        }
        public string Extension
        {
            get => FileLastHistory.Extension;
        }
       
        public bool IsDeleted
        {
            get => FileLastHistory.IsDelete;

        }

        public List<FileHistory> History { get; } 
        private FileHistory FileLastHistory { get; }


        public static File CreateFile  ( List<FileHistory> history)
        {

            if (history != null && history.Count > 0)
            {
                
                if (history.Select(a=>a.KeyFileUser).Distinct().Count()>1)
                    throw new Exception($"History not the same file");

                string fileKey = history.First().KeyFileUser;
                
                if (history.Count(a => a.IsClone) > 1)
                    throw new Exception($"To many clone operation in file {fileKey}");
                
                if (history.Count(a => a.IsAdd) > 1)
                    throw new Exception($"To many add operation in file {fileKey}");
                
                if (history.Count(a => a.IsDelete) > 1)
                    throw new Exception($"To many delete operation in file {fileKey}");
                
                return new File(history);
            }

            return null;
        }
        
        
        
        
        
        private File(List<FileHistory> history)
        {
            History = history;
            FileLastHistory = history.Where(a => a.OrderBy == history.Max(b => b.OrderBy)).Single();
        }
       
    }
}