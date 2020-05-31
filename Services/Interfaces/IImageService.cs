using MotorGliding.Models.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MotorGliding.Services.Interfaces
{
    public interface IImageService
    {
        Task<Image> AddImageAsync(Image image, string folder, bool main = false);
        Task<bool> SaveImageAsync(Image image);
        Task<bool> DeleteImageAsync(Image image, string folder);
        Task<Image> GetAsync(int id);
        Task<bool> RemoveImageAsync(Image image);
        Task<List<Image>> GetGalleryAsync(int id, string category);
        Task<Image> GetMainAsync(int id, string category);
        Task<bool> UpdateImageAsync(Image image);
    }
}
