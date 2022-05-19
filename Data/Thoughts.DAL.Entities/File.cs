

using System.ComponentModel.DataAnnotations;

using Microsoft.EntityFrameworkCore;

using Thoughts.DAL.Entities.Base;

namespace Thoughts.DAL.Entities;

/// <summary>
/// Класс для хранения Файла
/// </summary>

[Index(nameof(FileName), IsUnique = true, Name = "NameIndex")]
public class File : Entity
{
    /// <summary>Имя файла включая расширение</summary>
    [Required]
    public string FileName { get; set; } = null!;

    /// <summary>Описание файла (при необходимости)</summary>
    public string? FileDescription { get; set; } = null!;

    /// <summary>Файл преобразованный в массив байт</summary>
    [Required]
    public byte[] FileBody { get; set; } = null!;

    public File() { }

    public File(string fileName, string? fileDescription, byte[] fileBody)
    {
        FileName = fileName;
        FileDescription = fileDescription;
        FileBody = fileBody;
    }
}

