namespace Dewey.Dms.FileService.Hdfs.Client
{
    public class HdfsFileStatus
    {
        public string Name { get;}
        public string Owner { get; }
        public HdfsFileType FileType { get;}
        
        public string Permission { get; }

        public HdfsFileStatus(string Name, string Owner, HdfsFileType FileType , string Permission
        )
        {
            this.Name = Name;
            this.Owner = Owner;
            this.FileType = FileType;
            this.Permission = Permission;
        }
        

    }
}