using System;

namespace Dewey.Dms.FileService.Api.Models.View
{
    public class FileRest
    {
        public string FileName { get;  }
        public string Extension { get;  }
        public bool IsCloned;
        public bool IsDeleted;
        public string Key { get;  }
        public string OwnerKey { get; }

        public FileRest(Dewey.Dms.FileService.Hbase.Views.File file)
        {
            this.FileName = file.FileName;
            this.Extension = file.Extension;
            this.Key = file.Key;
            this.OwnerKey = file.KeyUser;
            this.IsDeleted = file.IsDeleted;
            this.IsCloned = file.IsCloned;
        }
    }
}