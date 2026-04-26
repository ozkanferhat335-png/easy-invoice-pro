using System;
using System.IO;

namespace EasyInvoicePro.Utils
{
    public static class FileHelper
    {
        public static bool CreateFolder(string folderPath)
        {
            try
            {
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                    System.Diagnostics.Debug.WriteLine($"[FILE] Klasor olusturuldu: {folderPath}");
                }
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ERROR] Klasor olusturulurken hata: {ex.Message}");
                return false;
            }
        }

        public static bool FileExists(string filePath)
        {
            return File.Exists(filePath);
        }

        public static bool DeleteFile(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    System.Diagnostics.Debug.WriteLine($"[FILE] Dosya silindi: {filePath}");
                }
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ERROR] Dosya silinirken hata: {ex.Message}");
                return false;
            }
        }

        public static bool CopyFile(string sourceFile, string destinationFile, bool overwrite = false)
        {
            try
            {
                if (File.Exists(sourceFile))
                {
                    File.Copy(sourceFile, destinationFile, overwrite);
                    System.Diagnostics.Debug.WriteLine($"[FILE] Dosya kopyalandi: {sourceFile} -> {destinationFile}");
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ERROR] Dosya kopyalanirken hata: {ex.Message}");
                return false;
            }
        }

        public static bool MoveFile(string sourceFile, string destinationFile)
        {
            try
            {
                if (File.Exists(sourceFile))
                {
                    File.Move(sourceFile, destinationFile, true);
                    System.Diagnostics.Debug.WriteLine($"[FILE] Dosya tasindi: {sourceFile} -> {destinationFile}");
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ERROR] Dosya tasirken hata: {ex.Message}");
                return false;
            }
        }

        public static long GetFileSize(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    var fileInfo = new FileInfo(filePath);
                    return fileInfo.Length;
                }
                return 0;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ERROR] Dosya boyutu alinirken hata: {ex.Message}");
                return 0;
            }
        }

        public static string FormatFileSize(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            double len = bytes;
            int order = 0;

            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }

            return $"{len:0.##} {sizes[order]}";
        }

        public static double GetFolderSize(string folderPath)
        {
            try
            {
                if (!Directory.Exists(folderPath))
                    return 0;

                var dirInfo = new DirectoryInfo(folderPath);
                long totalSize = 0;

                foreach (var file in dirInfo.GetFiles())
                {
                    totalSize += file.Length;
                }

                foreach (var dir in dirInfo.GetDirectories())
                {
                    totalSize += (long)(GetFolderSize(dir.FullName) * 1024 * 1024);
                }

                return totalSize / (1024.0 * 1024.0);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ERROR] Klasor boyutu hesaplanirken hata: {ex.Message}");
                return 0;
            }
        }
    }
}
