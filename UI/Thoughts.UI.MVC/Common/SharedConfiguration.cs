using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thoughts.UI.MVC.Common
{
    public class SharedConfiguration
    {
        #region FileStorage
        public long FileSizeLimit { get; set; }
        public string TargetFilePath { get; set; }
        public string[]? PermittedExtensionsForUploadedFile { get; set; }

        #endregion
        public SharedConfiguration(long fileSizeLimit, string targetFilePath, string[]? permittedExtensionsForUploadedFile)
        {
            FileSizeLimit = fileSizeLimit;
            TargetFilePath = targetFilePath;
            PermittedExtensionsForUploadedFile = permittedExtensionsForUploadedFile;
        }
    }
}
