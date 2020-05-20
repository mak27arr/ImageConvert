using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageConvert.ImageConverter
{
    interface IImageConverter
    {
        FileConvert ConvertFile(FileConvert file);
        List<string> GetSupportInFormat();
        List<string> GetSupportOutFormat();
    }
}
