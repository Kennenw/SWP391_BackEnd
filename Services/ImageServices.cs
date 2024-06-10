using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ImageServices
    {
        private readonly string _imageDirectory;
        public ImageServices()
        {
            _imageDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Repositories", "Image");
            if (!Directory.Exists(_imageDirectory))
            {
                Directory.CreateDirectory(_imageDirectory);
            }
        }

        public Task<byte[]> LoadImageAsync(string imagePath)
        {
            var filePath = Path.Combine(_imageDirectory, imagePath);
            if (!File.Exists(filePath))
                throw new FileNotFoundException("Image not found.");

            return File.ReadAllBytesAsync(filePath);
        }

        public async Task<string> SaveImage(IFormFile image)
        {
            if (image == null || image.Length == 0)
                throw new ArgumentException("Invalid image.");

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
            var filePath = Path.Combine(_imageDirectory, fileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }
            return fileName;
        }
    }
}
