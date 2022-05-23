

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

    public FileModel() { }

    public FileModel(string fileName, string? fileDescription, byte[] fileBody)
    {
        FileName = fileName;
        FileDescription = fileDescription;
        FileBody = fileBody;
    }
}

