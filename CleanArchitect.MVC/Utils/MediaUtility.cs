using CleanArchitect.Common.Contants;

namespace CleanArchitect.MVC.Utils
{
    public static class MediaUtility
    {
        public static async Task<string?> SaveImage(IFormFile file)
        {
            string? storedFileName = null;
            #region Xử lý ảnh
            if (file != null && file.Length > 0)
            {
                string[] validImages = { ".jpg", ".jpeg", ".png", ".webp" };
                var fileName = file.FileName;
                var extension = Path.GetExtension(file.FileName).ToLower();
                if (validImages.Contains(extension))
                {
                    if (file.Length > 5242880) return storedFileName;
                    storedFileName = Guid.NewGuid().ToString() + extension;
                    var filePath = Path.Combine("wwwroot", AppConstants.ImageFolderPath, storedFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                }
            }
            #endregion
            return storedFileName;
        }
    }
}
