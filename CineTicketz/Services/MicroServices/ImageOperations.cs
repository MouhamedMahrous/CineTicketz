using CineTicketz.Settings;

namespace CineTicketz.Services.MicroServices
{
    public class ImageOperations
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _imagesPath;
        public ImageOperations(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            _imagesPath = $"{_webHostEnvironment.WebRootPath}{FileSettings.ImagesPath}";
        }

        public async Task<string> Create(IFormFile file)
        {
            var coverName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

            var path = Path.Combine(_imagesPath, coverName);

            using var stream = File.Create(path);
            await file.CopyToAsync(stream);

            return coverName;
        }

        public void Delete(string imageName) 
        {
            var cover = Path.Combine(_imagesPath, imageName);
            File.Delete(cover);
        }
    }
}
