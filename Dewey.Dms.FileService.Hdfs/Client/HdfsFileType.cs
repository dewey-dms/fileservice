using System;
using System.IO;
using System.Net.NetworkInformation;

namespace Dewey.Dms.FileService.Hdfs.Client
{
    public enum HdfsFileType
    {
        DIRECTORY,
        FILE,
        SYMLINK
    }

    public static class HdfsFileTypeExtension
    {
        
        public static HdfsFileType HdfsFileType(this string fileType)
        {
            switch (fileType)
            {
               case "DIRECTORY": return Client.HdfsFileType.DIRECTORY;
               case "FILE": return Client.HdfsFileType.FILE;
               case "SYMLINK": return Client.HdfsFileType.SYMLINK;
            }
            
            throw new Exception($"Uknown value {fileType}");
        }
    }
}