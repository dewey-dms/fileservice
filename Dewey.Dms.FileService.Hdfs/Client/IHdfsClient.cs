using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Dewey.Dms.FileService.Hdfs.Client
{
    public interface IHdfsClient
    {
         Task<bool> Delete(
            string path,
            bool? recursive = null);
        Task<bool> MakeDirectory(
            string path,
            string permission = null);

        Task<bool> WriteStream(Stream stream, string path);

      

        Task<bool> ReadStream(
            Stream stream,
            string path
        );

        Task<HdfsFileStatus> GetFileStatus(
            string path);

        Task<IEnumerable<HdfsFileStatus>> ListStatus(
            string path);
    }
}