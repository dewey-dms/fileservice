using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Dewey.Dms.FileService.Hdfs.Client;
using Dewey.Dms.FileService.Hdfs.Client.Web;
using System.Linq;

namespace Dewey.Dms.FileService.Hdfs
{
    public class HdfsOperation
    {

        private string GetRootName => "dewey";
        private string GetRootPath => $"/{GetRootName}";

        private string GetHomeDirectoryName => UserGuid.ToString();
        private string GetHomeDirectoryPath => $"{GetRootPath}/{GetRootName}";

        private string GetFilePath(Guid fileGuid) => $"{GetHomeDirectoryPath}/{fileGuid.ToString()}";
        
        public IHdfsClient HdfsClient { get; }
        public Guid UserGuid { get; }
        
        public HdfsOperation(IHdfsClient HdfsClient , Guid UserGuid)
        {
            this.HdfsClient = HdfsClient;
            this.UserGuid = UserGuid;
        }

        private void AddDeweyRoot()
        {
            bool isRoot = HdfsClient.ListStatus("/").Result.SingleOrDefault(a => a.Name == GetRootName) != null;
            if (isRoot)
            {
                bool result = HdfsClient.MakeDirectory(GetRootPath).Result;
                if (!result)
                    throw new Exception($"Problem add  root {GetRootPath}");
                
            }

            
        }

        private void AddHomeDirectory()
        {
            bool isHomeDirectory = HdfsClient.ListStatus(path: GetRootPath).Result.SingleOrDefault(a=>a.Name == GetHomeDirectoryName)!=null;
            if (isHomeDirectory)
            {
                bool result = HdfsClient.MakeDirectory(GetHomeDirectoryPath).Result;
                if (!result)
                    throw new Exception($"Problem add home directory {GetHomeDirectoryPath}");
            }
            
        }

        public void MakeHomeDirectory()
        {
            AddDeweyRoot();
            AddHomeDirectory();
        }

        public bool CreateFile(Guid fileGuid, Stream stream)
        {
           
            MakeHomeDirectory();
            return HdfsClient.WriteStream(path: GetFilePath(fileGuid) , stream: stream).Result;
        }
        
        public bool ReadFile(Guid fileGuid, Stream stream)
        {
            MakeHomeDirectory();
            return HdfsClient.ReadStream(path: GetFilePath(fileGuid) , stream: stream).Result;
        }
        
        
        
       
    }
}