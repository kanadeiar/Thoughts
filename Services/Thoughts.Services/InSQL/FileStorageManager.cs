using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using Thoughts.DAL;
using Thoughts.DAL.Entities;
using Thoughts.Interfaces;
using Thoughts.Tools.Extensions;

namespace Thoughts.Services.InSQL
{
    public class FileStorageManager : IFileManager
    {
        private readonly FileStorageDb _context;

        public FileStorageManager(FileStorageDb context)
        {
            _context = context;
        }

        public async Task<UploadedFile> Get(string sha1)
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

        public async Task<bool> SoftDelete(string sha1)
        {
            var file = await _context.Files.FindAsync(sha1);
            if (file == null) return false;
            file.Active = false;
            _context.Files.Update(file);
            await _context.SaveChangesAsync();
            return true;

        }

        public async Task<bool> ActivateFile(string sha1)
        {
            var file = await _context.Files.FindAsync(sha1);
            if(file == null) return false;
            file.Active = true;
            _context.Files.Update(file);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<string> AddOrUpdate(UploadedFile fileModel)
        {
            if (fileModel != null)
            {
                var file = await _context.Files.FindAsync(fileModel.Sha1);
                if (file != null)
                {
                    file.Counter++;
                    file.Updated = DateTimeOffset.Now;
                    _context.Files.Update(file);
                }
                else
                {
                    fileModel.Counter++;
                    await _context.Files.AddAsync(fileModel);
                }
                await _context.SaveChangesAsync();
            }

            return fileModel?.Sha1;
        }

        public async Task<bool> Exists(string sha1)
        {
            var file = await _context.Files.FindAsync(sha1);

            return file != null && FileExistsInStorage(file);
        }        

        public async Task<bool> Exists(byte[] buffer)
        {
            return await Exists(await buffer.GetSha1Async());
        }

        public async Task<List<UploadedFile>> GetAllFilesInfo()
        {
            return await _context.Files.ToListAsync();
        } 

        private static bool FileExistsInStorage(UploadedFile file)
        {
            var find = false;
            if (file == null) return find;

            var dir = new DirectoryInfo(file.Path);
            return dir.GetFiles().Any(f => f.FullName.GetMd5Async().Result == file.Md5);
        }

    }
}
