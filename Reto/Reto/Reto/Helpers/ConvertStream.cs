namespace Reto.Helpers
{
    public static class ConvertStream
    {
        public static byte[] ConvertirStreamAByteArray(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));
            using (MemoryStream memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }


        public static string GetImageFolderPath()
        {
            string folderPath = Path.Combine(FileSystem.AppDataDirectory, "Images");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            return folderPath;
        }

        public static async Task<string> SaveImageFromBytesAsync(byte[] imageBytes, string imageName)
        {
            try
            {
                string folderPath = GetImageFolderPath();
                string filePath = Path.Combine(folderPath, imageName);
                await File.WriteAllBytesAsync(filePath, imageBytes);
                return filePath;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
    }
}
