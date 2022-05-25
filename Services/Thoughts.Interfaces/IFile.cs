

using System.ComponentModel.DataAnnotations;

using Thoughts.Interfaces.Base.Entities;

namespace Thoughts.Interfaces;

/// <summary>
/// Файл
/// </summary>
/// <typeparam name="TKye"></typeparam>
public interface IFile<TKye> : INamedEntity<TKye>
{
    /// <summary>Описание файла (при необходимости)</summary>
    public string? FileDescription { get; set; }

    /// <summary>Файл преобразованный в массив байт</summary>
    [Required]
    public byte[] FileBody { get; set; }

    /// <summary>MD-5 Hash файла</summary>
    [MaxLength(16)]
    public byte[] FileHash { get; set; }

}
