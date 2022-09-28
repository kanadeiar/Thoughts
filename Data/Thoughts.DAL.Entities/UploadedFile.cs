using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using Thoughts.DAL.Entities.Base;

namespace Thoughts.DAL.Entities
{
    [Index(nameof(Md5), nameof(Meta), nameof(Sha1))]
    public class UploadedFile
    {
        [Key]
        public string Sha1 { get; set; }
        public int Counter { get; set; }
        public string NameForDisplay { get; set; }
        public string FileNameForFileStorage { get; set; }
        public string Url { get; set; }
        public string Path { get; set; }
        public string MimeType { get; set; }
        [MaxLength(32)]
        public string Md5 { get; set; }
        [Column(TypeName = "bigint")]
        public int Size { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Updated { get; set; }
        public string Meta { get; set; }
        public byte Access { get; set; }
        public byte Flags { get; set; }

        [NotMapped]
        public byte[] ByteArray { get; set; }
    }
}
