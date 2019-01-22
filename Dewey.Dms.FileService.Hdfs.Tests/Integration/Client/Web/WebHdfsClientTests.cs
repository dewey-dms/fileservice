using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using Dewey.Dms.FileService.Hdfs.Client;
using Dewey.Dms.FileService.Hdfs.Client.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dewey.Dms.FileService.Hdfs.Tests.Integration.Client.Web
{
    [TestClass]
    public class WebHdfsClientTests
    {
        public static Stream GenerateStreamFromString(string s)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(s));
        }

        public static string ReadStream(Stream stream)
        {
            stream.Position = 0;
            StreamReader reader = new StreamReader( stream );
            
            string text = reader.ReadToEnd();
            return text;
        }
        
        
        public static WebHdfsClient GetWebHdfsClient()
        {
            string baseApi = "http://localhost:50070/webhdfs/v1";
            return new WebHdfsClient(BaseApi: baseApi ,  ExpectContinue: true, Timeout: null);
        }

        
        [TestMethod]
        public void WriteStream_AddRandomFileWithRandomContent_ResultOk()
        {
            
          
            //Assign
            WebHdfsClient client = GetWebHdfsClient();
            String fileNamePath = $"/dewey/tests/{Guid.NewGuid()}";
            string text = $"{Guid.NewGuid()}";
            Stream textStream = GenerateStreamFromString(text);
            
            //Act

            bool result = client.WriteStream(textStream, fileNamePath).Result;

            //Assert
            Assert.IsTrue(result);
        }
        
        [TestMethod]
        public void ReadStream_AddRandomFileWithRandomContent_ResultTheSameContent()
        {
            
          
            //Assign
            WebHdfsClient client = GetWebHdfsClient();
            String fileNamePath = $"/dewey/tests/{Guid.NewGuid()}";
            string text = $"{Guid.NewGuid()}";
            
            Stream textStream = GenerateStreamFromString(text);
            bool resultWrite = client.WriteStream(textStream, fileNamePath).Result;

            MemoryStream streamToRead = new MemoryStream(256);
            streamToRead.Position = 0;
            //Act
            bool resultRead = client.ReadStream(streamToRead, fileNamePath).Result;
            
            
            //Assert
            string textRead = ReadStream(streamToRead);
            
            Assert.IsTrue(resultWrite);
            Assert.IsTrue(resultRead);
            Assert.AreEqual(text,textRead);
        }


        [TestMethod]
        public void GetFileStatus_AddRandomFileWithRandomContent_ResultFileStatus()
        {
            //Assign
            WebHdfsClient client = GetWebHdfsClient();
            string fileName = Guid.NewGuid().ToString();
            String fileNamePath = $"/dewey/tests/{fileName}";
            string text = $"{Guid.NewGuid()}";
            
            Stream textStream = GenerateStreamFromString(text);
            bool resultWrite = client.WriteStream(textStream, fileNamePath).Result;
            //Act
            HdfsFileStatus fileStatus = client.GetFileStatus(fileNamePath).Result;

            //Assert
            Assert.IsTrue(resultWrite);
            Assert.AreEqual(HdfsFileType.FILE, fileStatus.FileType);
        }
        
        
        [TestMethod]
        public void ListStatus_AddRandomFileWithRandomContent_ResultListStatus()
        {
            //Assign
            WebHdfsClient client = GetWebHdfsClient();
            string fileName = Guid.NewGuid().ToString();
            string rootPath = "/dewey/tests";
            String fileNamePath = $"{rootPath}/{fileName}";
            string text = $"{Guid.NewGuid()}";
            
            Stream textStream = GenerateStreamFromString(text);
            bool resultWrite = client.WriteStream(textStream, fileNamePath).Result;
            //Act
            IEnumerable<HdfsFileStatus> fileStatus = client.ListStatus(rootPath).Result;
            
            //Assert
            Assert.IsTrue(resultWrite);
            HdfsFileStatus findStatus =
                fileStatus.SingleOrDefault(a => a.FileType == HdfsFileType.FILE && a.Name == fileName);
            
            Assert.IsNotNull(findStatus);
            
        }
        
        
        
        [TestMethod]
        public void MakeDirectory_MakerRandomDirectory_ResultListStatusDirectory()
        {
            //Assign
            WebHdfsClient client = GetWebHdfsClient();
            string directoryName = Guid.NewGuid().ToString();
            string rootPath = "/dewey/tests";
            String directoryNamePath = $"{rootPath}/{directoryName}";
           
            
           //Act
            bool resultMakeDirectory = client.MakeDirectory(directoryNamePath).Result;
            
            //Assert
            Assert.IsTrue(resultMakeDirectory);
            IEnumerable<HdfsFileStatus> fileStatus = client.ListStatus(rootPath).Result;
            
            HdfsFileStatus findStatus =
                fileStatus.SingleOrDefault(a => a.FileType == HdfsFileType.DIRECTORY && a.Name == directoryName);
            
            Assert.IsNotNull(findStatus);
            
        }
        
        [TestMethod]
        public void Delete_AddRandomFile_ResultDeleteFile()
        {
            //Assign
            WebHdfsClient client = GetWebHdfsClient();
            string fileName = Guid.NewGuid().ToString();
            string rootPath = "/dewey/tests";
            String fileNamePath = $"{rootPath}/{fileName}";
            string text = $"{Guid.NewGuid()}";
            Stream textStream = GenerateStreamFromString(text);
            bool resultWrite = client.WriteStream(textStream, fileNamePath).Result;
            
            //Act
            bool resultDelete = client.Delete(fileNamePath).Result;
            
            //Assert
            Assert.IsTrue(resultWrite);
            Assert.IsTrue(resultDelete);
            
        }
           
        
    }
}