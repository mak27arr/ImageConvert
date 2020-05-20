using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ImageConvert.ImageConverter;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ImageConvert.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageConvertController : ControllerBase
    {
        [HttpGet(""), Route("GetInSuportFormat")]
        public ActionResult<IEnumerable<object>> GetInSuportFormat()
        {
            var typesss = Assembly.GetExecutingAssembly().GetTypes();
            AsposeImageConverter converter = new AsposeImageConverter();
            return converter.GetSupportInFormat();
        }
        [HttpGet(""), Route("GetOutSuportFormat")]
        public ActionResult<IEnumerable<object>> GetOutSuportFormat()
        {
            AsposeImageConverter converter = new AsposeImageConverter();
            return converter.GetSupportOutFormat();
        }
        [HttpPut(""), Route("ConverFile")]
        public ActionResult<object> ConverFile()
        {
            string jsonString;
            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                jsonString = reader.ReadToEndAsync().Result;
            }
            FileConvert rezalt = new FileConvert();
            if (jsonString != String.Empty)
            {
                try
                {
                    FileConvert input_file = JsonSerializer.Deserialize<FileConvert>(jsonString);
                    AsposeImageConverter converter = new AsposeImageConverter();
                }
                catch (JsonException jex)
                {
                    rezalt.RezultCode = -1;
                    rezalt.RezultMsg = "Error: json string ";
                }
                catch (Exception jex)
                {
                    rezalt.RezultCode = -1;
                    rezalt.RezultMsg = "Error: ops  :-( ";
                }
            }
            else
            {
                rezalt.RezultCode = -1;
                rezalt.RezultMsg = "Error: empty body";
            }
            return rezalt;
        }
    }
}