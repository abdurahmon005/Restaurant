using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Aplication.Services.Interface
{
    public interface IFileStorageService
    {
        // Faylni Minio'ga yuklash
        Task<string> UploadFileAsync(string bucketName, string objectName, Stream data, string contentType);

        // Faylni Minio'dan yuklab olish
        Task<Stream> DownloadFileAsync(string bucketName, string objectName);

        // Faylning mavjudligini tekshirish
        Task<bool> FileExistsAsync(string bucketName, string objectName);

        // Faylni Minio'dan o'chirish
        Task<bool> RemoveFileAsync(string bucketName, string objectName);

        // Bucket mavjudligini tekshirish
        Task<bool> BucketExistsAsync(string bucketName);

        // Yangi bucket yaratish
        Task CreateBucketAsync(string bucketName);
    }
}
