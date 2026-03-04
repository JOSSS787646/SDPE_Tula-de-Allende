using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Configuration;



namespace SistemaDigitalizacionPolizas.Infrastructure.Persistence.Services.UploatFile
{
    public class WasabiFileStorageService : IFileStorageService
    {
        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName;

        public WasabiFileStorageService(IConfiguration configuration)
        {
            var accessKey = configuration["Wasabi:AccessKey"];
            var secretKey = configuration["Wasabi:SecretKey"];
            var serviceUrl = configuration["Wasabi:ServiceUrl"];
            _bucketName = configuration["Wasabi:BucketName"];

            var config = new AmazonS3Config
            {
                ServiceURL = serviceUrl,
                ForcePathStyle = true
            };

            _s3Client = new AmazonS3Client(accessKey, secretKey, config);
        }

        public async Task<string> UploadAsync(
            Stream fileStream,
            string fileName,
            string contentType,
            string folder)
        {
            var key = $"{folder}/{Guid.NewGuid()}_{fileName}";

            var request = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = key,
                InputStream = fileStream,
                ContentType = contentType
            };

            await _s3Client.PutObjectAsync(request);

            return key;
        }

        public async Task DeleteAsync(string filePath)
        {
            var request = new DeleteObjectRequest
            {
                BucketName = _bucketName,
                Key = filePath
            };

            await _s3Client.DeleteObjectAsync(request);
        }

        public string GetPresignedUrl(string filePath, int minutes = 60)
        {
            var request = new GetPreSignedUrlRequest
            {
                BucketName = _bucketName,
                Key = filePath,
                Expires = DateTime.UtcNow.AddMinutes(minutes)
            };

            return _s3Client.GetPreSignedURL(request);
        }

        public async Task<GetObjectResponse> GetFileAsync(string filePath)
        {
            var request = new GetObjectRequest
            {
                BucketName = _bucketName,
                Key = filePath
            };

            return await _s3Client.GetObjectAsync(request);
        }
    }
}
