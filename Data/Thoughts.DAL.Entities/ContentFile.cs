using System.ComponentModel.DataAnnotations;

using Microsoft.EntityFrameworkCore;

using Thoughts.DAL.Entities.Base;

namespace Thoughts.DAL.Entities;

/// <summary>Класс для хранения Файла</summary>
[Index(nameof(FileHash), IsUnique = true, Name = "NameIndex")]
public class ContentFile : NamedEntity
{
    /// <summary>Описание файла (при необходимости)</summary>
    public string? FileDescription { get; set; }

    /// <summary>Файл преобразованный в массив байт</summary>
    [Required]
    public byte[] FileBody { get; set; } = null!;

    /// <summary>MD-5 Hash файла</summary>
    [MaxLength(16)]
    public byte[] FileHash { get; set; } = null!;
}

