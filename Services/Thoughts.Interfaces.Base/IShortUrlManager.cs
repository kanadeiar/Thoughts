using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thoughts.Interfaces.Base
{
    public interface IShortUrlManager
    {
        /// <summary>
        /// Получить оригинальный Url по псевдониму
        /// </summary>
        /// <param name="Alias">Псевдоним ссылки</param>
        /// <returns>Оригинальный Url</returns>
        Task<Uri?> GetUrlAsync(string Alias, CancellationToken Cancel = default);

        /// <summary>
        /// Получить оригинальный Url по идентификатору
        /// </summary>
        /// <param name="Id">Идентификатор короткой ссылки</param>
        /// <returns>Оригинальный Url</returns>
        Task<Uri?> GetUrlByIdAsync(int Id, CancellationToken Cancel = default);

        /// <summary>
        /// Добавить короткую ссылку
        /// </summary>
        /// <param name="Url">Добавляемый Url</param>
        /// <returns>Псевдоним ссылки</returns>
        Task<string> AddUrlAsync(string Url, CancellationToken Cancel = default);

        /// <summary>
        /// Удалить короткую ссылку по идентификатору
        /// </summary>
        /// <param name="Id">Идентификатор короткой ссылки</param>
        /// <returns>Результат удаления</returns>
        Task<bool> DeleteUrlAsync(int Id, CancellationToken Cancel = default);

        /// <summary>
        /// Обновить короткую ссылку
        /// </summary>
        /// <param name="Id">Идентификатор короткой ссылки</param>
        /// <param name="Url">Новый Url</param>
        /// <returns>Результат обновления</returns>
        Task<bool> UpdateUrlAsync(int Id, string Url, CancellationToken Cancel = default);
    }
}
