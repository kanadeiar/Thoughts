

using System.ComponentModel.DataAnnotations;

namespace Thoughts.Interfaces.Base.Entities;

/// <summary>
/// Файл
/// </summary>
/// <typeparam name="TKye"></typeparam>
public interface IFile<TKye>:INamedEntity<TKye>
{
    /// <summary>Описание файла (при необходимости)</summary>
    public string? FileDescription { get; set; }

    /// <summary>Файл преобразованный в массив байт</summary>
    [Required]
    public byte[] FileBody { get; set; }

}
