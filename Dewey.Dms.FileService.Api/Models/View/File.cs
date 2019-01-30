using System;

namespace Dewey.Dms.FileService.Api.Models.View
{
    public class File
    {
        public string FileName { get;  }
        public string Extension { get;  }
        public DateTime OperationTime { get;  }
        public bool IsCloned;
        public bool IsDeleted;
        public string Key { get;  }
        public string OwnerKey { get; }

        public File(Dewey.Dms.FileService.Hbase.Views.File file)
        {
            this.FileName = FileName;
            this.Extension = Extension;
            this.Key = file.Key;
            this.OwnerKey = file.KeyUser;
            this.IsDeleted = file.IsDeleted;
            this.IsCloned = file.IsCloned;
        }
    }
}