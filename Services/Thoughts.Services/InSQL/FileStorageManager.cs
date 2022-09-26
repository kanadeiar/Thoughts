using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using Thoughts.DAL;
using Thoughts.DAL.Entities;
using Thoughts.Interfaces;

namespace Thoughts.Services.InSQL
{
    public class FileStorageManager : IFileManager
    {
        private readonly FileStorageDb _context;

        public FileStorageManager(FileStorageDb context)
        {
            _context = context;
        }

        public async Task<UploadedFile?> Get(string sha1)
        {
            var file = await _context.Files.FindAsync(sha1);

            return file;
        }

        public async Task Delete(string sha1)
        {
            var file = await _context.Files.FindAsync(sha1);
            if (file != null)
            {
                if (file.Counter <= 0)
                {
                    _context.Files.Remove(file);
                    File.Delete(Path.Combine(file.Path + file.FileNameForFileStorage));
                }
                else
                {
                    file.Counter--;
                    _context.Files.Update(file);
                }
                await _context.SaveChangesAsync();
            }
        }

        public async Task<string> AddOrUpdate(UploadedFile fileModel)
        {
            if (fileModel != null)
            {
                fileModel.Sha1 = await ComputedSha1Checksum(fileModel.ByteArray);
                var file = await _context.Files.FindAsync(fileModel.Sha1);
                if (file != null)
                {
                    file.Counter++;
                    file.Updated = DateTimeOffset.Now;
                    _context.Files.Update(file);
                }
                else
                {
                    fileModel.Md5 = await ComputeMd5Checksum(fileModel.ByteArray);
                    fileModel.Counter++;
                    await _context.Files.AddAsync(fileModel);
                }
                await _context.SaveChangesAsync();
            }

            return fileModel.Sha1;
        }

        public async Task<bool> Exists(string sha1)
        {
            var file = await _context.Files.FindAsync(sha1);

            return file != null && FileExistsInStorage(file);
        }        

        public async Task<bool> Exists(byte[] buffer)
        {
            return await Exists(await ComputedSha1Checksum(buffer));
        }

        public async Task<string> ComputedSha1Checksum(byte[] buffer)
        {
            using var sha = SHA1.Create();
            var checksum = sha.ComputeHash(buffer);
            var sendCheckSum = BitConverter.ToString(checksum)
                .Replace("-", string.Empty);

            return sendCheckSum;
        }

        private async Task<string> ComputedSha1Checksum(string path)
        {
            var fs = File.OpenRead(path);
            var fileData = new byte[fs.Length];
            fs.Read(fileData, 0, fileData.Length);
            return await ComputedSha1Checksum(fileData);
        }

        private static string ComputeMd5Checksum(string path)
        {
            var fs = File.OpenRead(path);
            MD5 md5 = new MD5CryptoServiceProvider();
            var fileData = new byte[fs.Length];
            fs.Read(fileData, 0, (int)fs.Length);
            var checkSum = md5.ComputeHash(fileData);
            return BitConverter.ToString(checkSum).Replace("-", String.Empty);
        }

        public async Task<string> ComputeMd5Checksum(byte[] buffer)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            var checkSum = md5.ComputeHash(buffer);
            return BitConverter.ToString(checkSum).Replace("-", String.Empty);
        }

        private bool FileExistsInStorage(UploadedFile file)
        {
            var find = false;
            if (file == null) return find;

            var dir = new DirectoryInfo(file.Path);
            return dir.GetFiles().Any(f => ComputeMd5Checksum(f.FullName) == file.Md5);
        }

    }
}
