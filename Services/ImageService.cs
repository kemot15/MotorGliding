using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using MotorGliding.Context;
using MotorGliding.Models.Db;
using MotorGliding.Models.Other;
using MotorGliding.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MotorGliding.Services
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly MotorGlidingContext _context;

        public ImageService(IWebHostEnvironment hostEnvironment, MotorGlidingContext context)
        {
            _hostEnvironment = hostEnvironment;
            this._context = context;
        }
        /// <summary>
        /// Zapisuje obraz na serwerze
        /// </summary>
        /// <param name="image">Obraz do zapisania</param>
        /// <param name="folder">Podfolder do zapisu</param>
        /// <param name="main">Ustawia czy obraz jest głównym dla danego wydarzenia</param>
        /// <returns></returns>
        public async Task<Image> AddImageAsync(Image image, string folder, bool main = false)
        {
            string wwwRootPath = _hostEnvironment.WebRootPath;
            string fileName = Path.GetFileNameWithoutExtension(image.ImageFile.FileName);
            string extension = Path.GetExtension(image.ImageFile.FileName);
            image.Name = fileName = fileName + DateTime.Now.ToString("_yymmssfff") + extension;
            string path = Path.Combine($"{wwwRootPath}/{folder}/{fileName}");
            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                await image.ImageFile.CopyToAsync(fileStream);
                image.Default = main;            
            }
            return image;
        }
        /// <summary>
        /// Zapisuje informacje o obrazie w bazie danych
        /// </summary>
        /// <param name="image">Obraz do zapisania</param>
        /// <returns></returns>
        public async Task<bool> SaveImageAsync (Image image)
        {
            await _context.Images.AddAsync(image);
            return await _context.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// Usuwa plik z folderu
        /// </summary>
        /// <param name="image">Obraz do usunięcia</param>
        /// <param name="folder">Dokładna ścieżka</param>
        /// <returns></returns>
        public async Task<bool> DeleteImageAsync(Image image, string folder) 
        {           
            var imagePath = Path.Combine($"{_hostEnvironment.WebRootPath}\\{folder}\\{image.Name}");
            //var fileToDelete = Image.FromFile(imagePath);
            if (File.Exists(imagePath))
            {             
                    File.Delete(imagePath);
              
                return true;
            }
            return false;
        }

        /// <summary>
        /// Usuwa wpis o obrazie z bazy danych
        /// </summary>
        /// <param name="image">Obraz do usuniecia</param>
        /// <returns></returns>
        public async Task<bool> RemoveImageAsync(Image image) //usuwa wpis w bazie
        {
            _context.Images.Remove(image);
            return await _context.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// Pobiera informacje o obrazie z bazy danych
        /// </summary>
        /// <param name="id">Id obrazu do pobrania</param>
        /// <returns>Zwraca obraz</returns>
        public async Task<Image> GetAsync(int id)
        {
            return await _context.Images.FindAsync(id);
        }

        /// <summary>
        /// Zwraca glowny obraz dla zadanej kategorii
        /// </summary>
        /// <param name="id">Id Eventu, pojazdu</param>
        /// <param name="category">Kategoria</param>
        /// <returns></returns>
        public async Task<Image> GetMainAsync(int id, string category)
        {
            return await _context.Images.SingleOrDefaultAsync(i => i.SourceId == id && i.Category == category && i.Default);
        }

        /// <summary>
        /// Pobranie wszystkich obrazów nalezacych do danej kategorii
        /// </summary>
        /// <param name="id">Id eventu lub pojazdu</param>
        /// <param name="category">Kategoria obrazow</param>
        /// <returns></returns>
        public async Task<List<Image>> GetGalleryAsync(int id, string category)
        {
            return await _context.Images.Where(i => i.SourceId == id && i.Category == category).ToListAsync();
        }

        public async Task<bool> UpdateImageAsync (Image image)
        {
            var newImage = await GetAsync(image.Id);
            newImage.Name = image.Name;
            _context.Images.Update(newImage);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IList<Image>> GetGalleryAsync(bool active)
        {
            if (active)
                return await _context.Images.Where(i => i.Category == Folders.gallery.ToString() && i.Active).ToListAsync();
            return await _context.Images.Where(i => i.Category == Folders.gallery.ToString()).ToListAsync();
        }

        public async Task<bool> ActiveChange(int id)
        {
            var image = _context.Images.Find(id);
            if (image == null)
                return false;
            image.Active = !image.Active;
            _context.Update(image);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
