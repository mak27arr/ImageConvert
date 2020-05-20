using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageConvert.ImageConverter
{
    /// <summary>
    /// For file transfere
    /// </summary>
    public class FileConvert
    {
        /// <summary>
        /// Realt converting
        /// </summary>
        public short RezultCode { get; set; }
        /// <summary>
        /// Message for user
        /// </summary>
        public string RezultMsg { get; set; }
        /// <summary>
        /// File name
        /// </summary>
        public string File_name { get; set; }
        /// <summary>
        /// File format
        /// </summary>
        public string FileIn_extension { get; set; }
        /// <summary>
        /// Output file format
        /// </summary>
        public string FileOut_extension { get; set; }
        /// <summary>
        /// File in binary
        /// </summary>
        public byte[] File { get; set; }
    }
}
