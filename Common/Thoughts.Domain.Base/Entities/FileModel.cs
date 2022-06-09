using System.ComponentModel.DataAnnotations;

namespace Thoughts.Domain.Base.Entities;

/// <summary>Файл</summary>
public class FileModel : NamedEntityModel
{
    /// <summary>Описание файла (при необходимости)</summary>
    public string? Description { get; set; }

    /// <summary>Файл преобразованный в массив байт</summary>
    [Required]
    public byte[] Content { get; set; } = null!;

    /// <summary>MD-5 Hash файла</summary>
    [MaxLength(16)]
    public byte[] Hash { get; set; } = null!;
}

