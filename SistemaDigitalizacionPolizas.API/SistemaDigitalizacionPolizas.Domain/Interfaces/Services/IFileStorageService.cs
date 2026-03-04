using Amazon.S3.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Infrastructure.Persistence.Services.UploatFile
{
    public interface IFileStorageService
    {
        Task<string> UploadAsync(
            Stream fileStream,
            string fileName,
            string contentType,
            string folder);

        Task DeleteAsync(string filePath);

        string GetPresignedUrl(string filePath, int minutes =60);
        Task<GetObjectResponse> GetFileAsync(string filePath);
    }
}
