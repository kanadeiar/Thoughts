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
    [Index(nameof(Md5), nameof(Meta), nameof(Sha1), IsUnique = true)]
    public class UploadedFile
    {
        [Key]
        //Первичный ключ
        public string Sha1 { get; set; }

        //Кол-во ссылок на файл в хранилище
        public int Counter { get; set; }

        //Публичное имя установленное пользователем
        public string NameForDisplay { get; set; }


        //Имя файла с безопастным для хранилища расширением,
        public string FileNameForFileStorage { get; set; }
        
        public string Url { get; set; }

        //Путь к файлу
        public string Path { get; set; }

        //Тип файла
        public string MimeType { get; set; }
        
        //Мд5 хэш сумма
        [MaxLength(32)]
        public string Md5 { get; set; }
        
        //Размер файла в байтах
        [Column(TypeName = "bigint")]
        public long Size { get; set; }

        //Дата создания файла
        public DateTimeOffset Created { get; set; }

        //Дата обновления файла
        public DateTimeOffset? Updated { get; set; }

        //Мета информация о файле
        public string Meta { get; set; }
        
        //Доступность файла
        public byte Access { get; set; }
        
        //Флаги
        public byte Flags { get; set; }

        //Если активен отображается
        public bool Active { get; set; }

        [NotMapped]
        public byte[] ByteArray { get; set; }

        [NotMapped]
        public FileStream Stream { get; set; }
    }
}
