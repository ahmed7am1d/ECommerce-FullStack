using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TBU.eshop.web.Models.Entities
{
    public class FileUpload
    {
        public string RootPath { get; set; }
        public string FolderName { get; set; }
        public string ContentType { get; set; }
        public long MaxLengthFile { get; set; }


        public FileUpload(string rootPath, string folderName, string contentType, long maxLengthFile = 2_000_000)
        {
            this.RootPath = rootPath;
            this.FolderName = folderName;
            this.ContentType = contentType;
            this.MaxLengthFile = maxLengthFile;
        }

        public bool CheckFileContent(IFormFile iFormFile)
        {
            return iFormFile != null && iFormFile.ContentType.ToLower().Contains(ContentType);
        }

        public bool CheckFileLength(IFormFile iFormFile)
        {
            return iFormFile.Length > 0 && iFormFile.Length < MaxLengthFile;
        }

        public async Task<string> FileUploadAsync(IFormFile iFormFile)
        {
            string filePathOutput = String.Empty;
            if (CheckFileContent(iFormFile) && CheckFileLength(iFormFile))
            {
                var fileName = Path.GetFileNameWithoutExtension(iFormFile.FileName);
                var fileExtension = Path.GetExtension(iFormFile.FileName);
                //var fileNameGenerated = Path.GetRandomFileName();

                var fileRelative = Path.Combine(FolderName, fileName + fileExtension);
                var filePath = Path.Combine(RootPath, fileRelative);

                Directory.CreateDirectory(Path.Combine(RootPath, FolderName));
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await iFormFile.CopyToAsync(stream);
                }

                filePathOutput = $"/{fileRelative}";
            }

            return filePathOutput;
        }
    }
}
