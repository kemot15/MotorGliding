using MotorGliding.Models.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MotorGliding.Services.Interfaces
{
    public interface IImageService
    {
        /// <summary>
        /// Zapisuje obraz na serwerze
        /// </summary>
        /// <param name="image">Obraz do zapisania</param>
        /// <param name="folder">Podfolder do zapisu</param>
        /// <param name="main">Ustawia czy obraz jest głównym dla danego wydarzenia</param>
        /// <returns></returns>
        Task<Image> AddImageAsync(Image image, string folder, bool main = false);
        Task<bool> SaveImageAsync(Image image);
        Task<bool> DeleteImageAsync(Image image, string folder);
        Task<Image> GetAsync(int id);
        Task<bool> RemoveImageAsync(Image image);
        Task<List<Image>> GetGalleryAsync(int id, string category);
        Task<Image> GetMainAsync(int id, string category);
        Task<bool> UpdateImageAsync(Image image);
        /// <summary>
        /// Pobranie listy obrazow dla galerii
        /// </summary>
        /// <param name="active">lista wszystkich/aktywnych</param>
        /// <returns></returns>
        Task<IList<Image>> GetGalleryAsync(bool active);
        /// <summary>
        /// Zmiana statusu wyswietlania na glownej stronie
        /// </summary>
        /// <param name="id">Id obrazu do zmiany stanu</param>
        /// <returns></returns>
        Task<bool> ActiveChange(int id);
    }
}
