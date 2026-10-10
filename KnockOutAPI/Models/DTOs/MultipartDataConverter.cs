using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System.Web;

namespace KnockOutAPI.Models.DTOs
{
    public class MultipartDataConverter : MediaTypeFormatter
    {
        public MultipartDataConverter()
        {
            SupportedMediaTypes.Add(new System.Net.Http.Headers.MediaTypeHeaderValue("multipart/form-data"));
        }
        public override bool CanReadType(Type type)
        {
            return type == typeof(GuestRequest);
        }

        public override bool CanWriteType(Type type)
        {
            return false;
        }

        public override async Task<object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger)
        {
            var multipartData = await content.ReadAsMultipartAsync();
            var guestData = new GuestRequest();

            foreach (var contentPart in multipartData.Contents)
            {
                string fieldName = contentPart.Headers.ContentDisposition.Name.Trim('\"');

                if (fieldName == "Guest")
                {
                    var guestContent = await contentPart.ReadAsStringAsync();
                    guestData.Guest = JsonConvert.DeserializeObject<Guest>(guestContent);
                }
                else if (fieldName == "ImageFile")
                {
                    guestData.ImageFile = await contentPart.ReadAsByteArrayAsync();
                    guestData.ImageFileName = contentPart.Headers.ContentDisposition.FileName;
                }
            }
            return guestData;
        }
    }
}