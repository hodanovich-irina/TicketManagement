using System;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace TicketManagement.EventAPI.ImportThirdPartyEvent
{
    /// <summary>
    /// Class for work with images.
    /// </summary>
    public static class ImageExtension
    {
        /// <summary>
        /// Convert image and save in wwwroot/images.
        /// </summary>
        /// <param name="imgName">name of image to save.</param>
        /// <param name="hostEnvironment">web host enviroment for choose path.</param>
        /// <param name="configuration">configuration to get some data from application.</param>
        /// <returns>short path of image.</returns>
        public static string UploadSampleImage(this string imgName, IWebHostEnvironment hostEnvironment, IConfiguration configuration)
        {
            var base64str = imgName.Substring(imgName.IndexOf(',') + 1);
            var bytes = Convert.FromBase64String(base64str);
            var path = Path.Combine(hostEnvironment.WebRootPath, configuration["ImgFolder"]);
            var shortPath = Path.Combine("\\", configuration["ImgFolder"]);
            var returnPath = configuration["ImgHostPath"];
            var flag = true;
            while (flag)
            {
                var newImgName = RandomString() + ".png";
                var newPath = Path.Combine(path, newImgName);
                var newShortPath = Path.Combine(shortPath, newImgName);
                if (!File.Exists(newPath))
                {
                    flag = false;
                    path = newPath;
                    shortPath = newShortPath;
                    returnPath += newShortPath;
                }
            }

            File.WriteAllBytes(path, bytes);

            return returnPath;
        }

        private static string RandomString()
        {
            var random = new Random(Environment.TickCount);

            var chars = "0123456789abcdefghijklmnopqrstuvwxyz";
            StringBuilder builder = new StringBuilder(5);

            for (int i = 0; i < 5; ++i)
            {
                builder.Append(chars[random.Next(chars.Length)]);
            }

            return builder.ToString();
        }
    }
}
