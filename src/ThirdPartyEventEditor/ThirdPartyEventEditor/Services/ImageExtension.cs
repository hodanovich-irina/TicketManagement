using System;
using System.IO;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Hosting;

namespace ThirdPartyEventEditor.Services
{
    public static class ImageExtension
    {
        public static async Task<string> UploadSampleImage(this string imgName)
        {
            var path = Path.Combine(HostingEnvironment.MapPath(WebConfigurationManager.AppSettings["ImgPath"]), imgName);
            if (!File.Exists(path)) 
            {
                throw new InvalidOperationException("Choose other image");
            }
            using (var memoryStream = new MemoryStream())
            using (var fileStream = new FileStream(path, FileMode.Open))
            {
                await fileStream.CopyToAsync(memoryStream);
                return "data:image/png;base64," + Convert.ToBase64String(memoryStream.ToArray());
            }
        }
    }
}