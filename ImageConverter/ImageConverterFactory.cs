using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageConvert.ImageConverter
{
    class ImageConverterFactory
    {
        public IImageConverter GetImageConverter(string name)
        {
            switch (name)
            {
                case "Aspose":
                    return new AsposeImageConverter();
                default:
                    return null;

            }
        }
    }
}
