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
        Task<Uri?> GetUrl(string Alias, CancellationToken Cancel = default);

        /// <summary>
        /// Получить оригинальный Url по идентификатору
        /// </summary>
        /// <param name="Id">Идентификатор короткой ссылки</param>
        /// <returns>Оригинальный Url</returns>
        Task<Uri?> GetUrlById(int Id, CancellationToken Cancel = default);

        /// <summary>
        /// Добавить короткую ссылку
        /// </summary>
        /// <param name="Url">Добавляемый Url</param>
        /// <returns>Псевдоним ссылки</returns>
        Task<string> AddUrl(string Url, CancellationToken Cancel = default);

        /// <summary>
        /// Удалить короткую ссылку по идентификатору
        /// </summary>
        /// <param name="Id">Идентификатор короткой ссылки</param>
        /// <returns>Результат удаления</returns>
        Task<bool> DeleteUrl(int Id, CancellationToken Cancel = default);

        /// <summary>
        /// Обновить короткую ссылку
        /// </summary>
        /// <param name="Id">Идентификатор короткой ссылки</param>
        /// <param name="Url">Новый Url</param>
        /// <returns>Результат обновления</returns>
        Task<bool> UpdateUrl(int Id, string Url, CancellationToken Cancel = default);
    }
}
