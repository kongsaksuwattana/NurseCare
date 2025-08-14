using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentFTP;
namespace NurseCare.DataAccess
{
    internal class FileFtp
    {
        const string ftpServer = "thesolutions.tech";
        const string ftpUser = "admin";
        const string ftpPassword = "26May@252213";

        // Upload the photo to the server via FTP using FluentFTP
        public static async Task UploadFileAsync(byte[] photo, string fileName,string folder)
        {
            var token = CancellationToken.None;
            if (photo == null)
            {
                Console.WriteLine("No photo to upload.");
                return;
            }
            try
            {
                using (var ftp = new AsyncFtpClient(ftpServer, ftpUser, ftpPassword))
                {
                    await ftp.Connect(token);

                    // upload a file and ensure the FTP directory is created on the server
                    await ftp.UploadBytes(photo, $"/Nursecare/{folder}/{fileName}", FtpRemoteExists.Overwrite, true, token: token);

                    await ftp.Disconnect();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error uploading file: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Error", $"Failed to upload file: {ex.Message}", "OK");
            }
        }

        // Upload the photo to the server via HTTP POST
        public async Task UploadPhotoAsync(FileResult photo)
        {
            if (photo == null)
                return;

            using var stream = await photo.OpenReadAsync();
            using var content = new MultipartFormDataContent();
            //content.Add(new StreamContent(stream), "file", photo.FileName);

            using var client = new HttpClient();
            var response = await client.PostAsync("https://yourserver.com/api/upload", content);

            if (response.IsSuccessStatusCode)
                Console.WriteLine("Upload successful");
            else
                Console.WriteLine($"Upload failed: {response.StatusCode}");
        }

        public async Task SavePhotoToPicturesFolder(byte[] photoBytes, string fileName)
        {
            try
            {
                // Get the photo stream
                //using var stream = await photo.OpenReadAsync();
                //using var memoryStream = new MemoryStream();
                //await stream.CopyToAsync(memoryStream);
                //byte[] photoBytes = memoryStream.ToArray();

                // Get the Pictures folder path
                string picturesPath = string.Empty;

#if ANDROID
                picturesPath = Android.OS.Environment.GetExternalStoragePublicDirectory(
                    Android.OS.Environment.DirectoryPictures).AbsolutePath;
#elif IOS
        picturesPath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
#elif WINDOWS
        picturesPath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
#endif

                // Ensure the folder exists
                Directory.CreateDirectory(picturesPath);

                // Save the photo
                string fullPath = Path.Combine(picturesPath, fileName);
                File.WriteAllBytes(fullPath, photoBytes);

                await Application.Current.MainPage.DisplayAlert("Success", $"Photo saved to: {fullPath}", "OK");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Failed to save photo: {ex.Message}", "OK");
            }
        }
    }
}
