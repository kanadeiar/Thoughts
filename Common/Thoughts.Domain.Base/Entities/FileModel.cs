using System.ComponentModel.DataAnnotations;

namespace Thoughts.Domain.Base.Entities;

/// <summary>Файл</summary>
public class FileModel : NamedEntityModel
{
    /// <summary>Описание файла (при необходимости)</summary>
    public string? Description { get; set; } = null!;

    /// <summary>Файл преобразованный в массив байт</summary>
    [Required]
    public byte[] Content { get; set; } = null!;

    /// <summary>MD-5 Hash файла</summary>
    [MaxLength(16)]
    public byte[] Hash { get; set; } = null!;

    public FileModel() { }

    public FileModel(string FileName, string? Description, byte[] Content, byte[] Hash)
    {
        Name = FileName;
        this.Description = Description;
        this.Content = Content;
        this.Hash = Hash;
    }
}

