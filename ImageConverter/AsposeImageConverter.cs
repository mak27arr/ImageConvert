using Aspose.Imaging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;

namespace ImageConvert.ImageConverter
{
    public class AsposeImageConverter : IImageConverter
    {
        /// <summary>
        /// Save support file format
        /// </summary>
        private static ConcurrentDictionary<string,Type> suportFormat = new ConcurrentDictionary<string, Type>();
        /// <summary>
        /// Convert file
        /// </summary>
        /// <param name="file">Input file with param required: File,FileInForma,FileOutFormat</param>
        /// <returns>
        /// If Error file.RezultCode == -1
        /// If Sucsses file.RezultCode == 0
        /// FileInForma and FileOutFormat change places
        /// </returns>
        public FileConvert ConvertFile(FileConvert file)
        {
            //cheak if all set
            file = CheakFile(file);
            if (file.RezultCode != 0)
                return file;
            // conver file
            try
            {
                using (MemoryStream ms = new MemoryStream(file.File))
                {
                    using (var img = Aspose.Imaging.Image.Load(ms))
                    {
                        var file_out_format_option = GetOutPutFormatOption(file.FileOut_extension);
                        //to be honest then this check is unnecessary
                        if (file_out_format_option != null)
                        {
                            img.Save(ms, file_out_format_option);
                        }
                        else
                        {
                            file.RezultCode = -1;
                            file.RezultMsg = "Error: cant convert";
                            return file;
                        }
                    }
                }
                var in_file_format = file.FileIn_extension;
                file.FileIn_extension = file.FileOut_extension;
                file.FileOut_extension = in_file_format;
            }
            catch(Exception ex)
            {
                file.RezultCode = -1;
                file.RezultMsg = "Error: ops :-(";
                return file;
            }
            return file;
        }
        /// <summary>
        /// Return all suportable input file format
        /// </summary>
        /// <returns>
        /// List with name like jpg ....
        /// </returns>
        public List<string> GetSupportInFormat()
        {
            if (suportFormat.Count == 0)
                GetTypesInNamespace();
            return suportFormat.Keys.ToList();
        }
        /// <summary>
        /// Return all suportable output file format
        /// </summary>
        /// <returns>
        /// List with name like jpg ....
        /// </returns>
        public List<string> GetSupportOutFormat()
        {
            if (suportFormat.Count == 0)
                GetTypesInNamespace();
            return suportFormat.Keys.ToList();
        }
        /// <summary>
        /// Return object of ImageOptions for output format
        /// </summary>
        /// <param name="name">format name string</param>
        /// <returns>instanse file format</returns>
        private ImageOptionsBase GetOutPutFormatOption(string name)
        {
            if (suportFormat.Count == 0)
                GetTypesInNamespace();
            if (suportFormat.ContainsKey(name)) {
                Type file_format_type;
                suportFormat.TryGetValue(name, out file_format_type);
                return Activator.CreateInstance(file_format_type) as ImageOptionsBase;
            }
            return null;
        }
        /// <summary>
        /// Cheak if all feald set
        /// </summary>
        /// <param name="file"> input file</param>
        /// <returns>
        /// If not set FileConvert.RezultCode == -1 
        /// If good FileConvert.RezultCode == 0
        /// </returns>
        private FileConvert CheakFile(FileConvert file)
        {
            if (file == null)
            {
                file = new FileConvert();
                file.RezultCode = -1;
                file.RezultMsg = "Empty";
                return file;
            }
            if (file.File == null && file.File.Length == 0)
            {
                file = new FileConvert();
                file.RezultCode = -1;
                file.RezultMsg = "Cant convert empty file";
                return file;
            }
            if (!CheakFileFormat(file))
            {
                file = new FileConvert();
                file.RezultCode = -1;
                file.RezultMsg = "Not support file format";
                return file;
            }
            file.RezultCode = 0;
            return file;
        }
        /// <summary>
        /// Cheak if can conver file
        /// Rezult: true if can
        /// </summary>
        private bool CheakFileFormat(FileConvert file)
        {
            //cheak if set in and out file format
            if (file.FileIn_extension == string.Empty || file.FileOut_extension == string.Empty)
                return false;
            //if nead load suport format
            if (suportFormat.Count == 0)
                GetTypesInNamespace();
            //cheak if can convert
            if (suportFormat.ContainsKey(file.FileIn_extension) && suportFormat.ContainsKey(file.FileOut_extension))
                return true;
            else
                return false;
        }
        /// <summary>
        /// Loading all suportable type for convert from Aspose
        /// Rezult: save in suportFormat 
        /// Format like jpg, bng .... 
        /// </summary>
        private void GetTypesInNamespace()
        {
            string nameSpace = "Aspose.Imaging.ImageOptions";
            var assembly_ref_name = Assembly.GetExecutingAssembly().GetReferencedAssemblies().Where(x => x.Name.Contains("Aspose.Imaging")).FirstOrDefault();
            if (assembly_ref_name == null)
                return;
            var assembly_ref = Assembly.Load(assembly_ref_name);
            var _suport_types = assembly_ref.GetTypes()
                      .Where(t => String.Equals(t.Namespace, nameSpace, StringComparison.Ordinal) && t.BaseType == typeof(ImageOptionsBase))
                      .ToArray();
            foreach(var _suport_type in _suport_types)
            {
                suportFormat.TryAdd(_suport_type.Name.Replace("Options", ""),_suport_type);
            }
        }
    }
}
