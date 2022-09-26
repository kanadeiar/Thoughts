using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using Thoughts.DAL.Entities;

namespace Thoughts.DAL
{
    public class FileStorageDb : DbContext
    {
        public DbSet<UploadedFile> Files { get; set; }

        public FileStorageDb(DbContextOptions<FileStorageDb> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
