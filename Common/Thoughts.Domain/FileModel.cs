

using System.ComponentModel.DataAnnotations;

using Thoughts.Domain.Base;

namespace Thoughts.Domain;

/// <summary>
/// Класс для хранения Файла
/// </summary>
public class FileModel : NamedEntityModel
{
    /// <summary>Описание файла (при необходимости)</summary>
    public string? FileDescription { get; set; } = null!;

    /// <summary>Файл преобразованный в массив байт</summary>
    [Required]
    public byte[] FileBody { get; set; } = null!;

    /// <summary>MD-5 Hash файла</summary>
    [MaxLength(16)]
    public byte[] FileHash { get; set; } = null!;

    public FileModel() { }

    public FileModel(string fileName, string? fileDescription, byte[] fileBody, byte[] fileHash)
    {
        Name = fileName;
        FileDescription = fileDescription;
        FileBody = fileBody;
        FileHash = fileHash;
    }
}

