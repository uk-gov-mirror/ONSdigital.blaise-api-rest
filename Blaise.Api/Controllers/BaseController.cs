using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Results;
using Blaise.Api.Filters;

namespace Blaise.Api.Controllers
{
    [ExceptionFilter]
    public class BaseController : ApiController
    {
        internal StatusCodeResult NoContent()
        {
            return StatusCode(HttpStatusCode.NoContent);
        }

        internal IHttpActionResult DownloadFile(string filePath)
        {
            try
            {
                var responseMsg = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ByteArrayContent(File.ReadAllBytes(filePath))
                };

                responseMsg.Content.Headers.ContentDisposition =  new ContentDispositionHeaderValue("attachment") {FileName = Path.GetFileName(filePath)};
                responseMsg.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            
                return ResponseMessage(responseMsg);
            }
            finally
            {
                CleanUpTempFiles(filePath);
            }
        }

        private void CleanUpTempFiles(string filePath)
        {
            File.Delete(filePath);

            var path = Path.GetDirectoryName(filePath);

            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            DeleteTemporaryGuidFolders(path);
        }

        private void DeleteTemporaryGuidFolders(string path)
        {
            var rootDir = Directory.GetDirectoryRoot(path);
            
            if (string.IsNullOrEmpty(rootDir))
            {
                return;
            }

            var subDirectories = Directory.GetDirectories(rootDir);
            foreach (var subDirectory in subDirectories)
            {
                var pathIsGuid = Guid.TryParse(new DirectoryInfo(subDirectory).Name, out _);

                if (!pathIsGuid) continue;

                Directory.Delete(subDirectory, true);
            }
        }
    }
}
