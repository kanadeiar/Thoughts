using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.FileProviders;
using Microsoft.Net.Http.Headers;

using Thoughts.DAL;
using Thoughts.DAL.Entities;
using Thoughts.UI.MVC.Common;
using Thoughts.UI.MVC.Filters;
using Thoughts.UI.MVC.Utilities;

namespace Thoughts.UI.MVC.Controllers
{
    [Route("file/")]
    public class FileController : Controller
    {
        private readonly long _fileSizeLimit;
        private readonly string _targetFilePath;
        private readonly string[] _permittedExtensions;
        private static readonly FormOptions _defaultFormOptions = new FormOptions();
        private readonly IFileManager _fileManager;
        public FileController(SharedConfiguration sharedConfiguration, IFileManager fileManager)
        {
            _fileManager = fileManager;
            _fileSizeLimit = sharedConfiguration.FileSizeLimit;

            // To save physical files to a path provided by configuration:
            _targetFilePath = sharedConfiguration.TargetFilePath;

            // To save physical files to the temporary files folder, use:
            //_targetFilePath = Path.GetTempPath();
            _permittedExtensions = sharedConfiguration.PermittedExtensionsForUploadedFile;
        }

        [HttpPost]
        [Route("upload")]
        [DisableFormValueModelBinding]
        [DisableRequestSizeLimit]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadPhysical()
        {
            if (!MultipartRequestHelper.IsMultipartContentType(Request.ContentType))
            {
                ModelState.AddModelError("File",
                    $"The request couldn't be processed (Error 1).");
                // Log error

                return BadRequest(ModelState);
            }

            var boundary = MultipartRequestHelper.GetBoundary(
                MediaTypeHeaderValue.Parse(Request.ContentType),
                _defaultFormOptions.MultipartBoundaryLengthLimit);
            var reader = new MultipartReader(boundary, HttpContext.Request.Body);
            var section = await reader.ReadNextSectionAsync();

            while (section != null)
            {
                var hasContentDispositionHeader =
                    ContentDispositionHeaderValue.TryParse(
                        section.ContentDisposition, out var contentDisposition);

                if (hasContentDispositionHeader)
                {
                    // This check assumes that there's a file
                    // present without form data. If form data
                    // is present, this method immediately fails
                    // and returns the model error.
                    if (!MultipartRequestHelper
                        .HasFileContentDisposition(contentDisposition))
                    {
                        ModelState.AddModelError("File",
                            $"The request couldn't be processed (Error 2).");
                        // Log error

                        return BadRequest(ModelState);
                    }
                    else
                    {
                        // Don't trust the file name sent by the client. To display
                        // the file name, HTML-encode the value.
                        var trustedFileNameForDisplay = WebUtility.HtmlEncode(
                                contentDisposition.FileName.Value);
                        var trustedFileNameForFileStorage = Path.GetRandomFileName();

                        // **WARNING!**
                        // In the following example, the file is saved without
                        // scanning the file's contents. In most production
                        // scenarios, an anti-virus/anti-malware scanner API
                        // is used on the file before making the file available
                        // for download or for use by other systems. 
                        // For more information, see the topic that accompanies 
                        // this sample.

                        var streamedFileContent = await FileHelpers.ProcessStreamedFile(
                            section, contentDisposition, ModelState,
                            _permittedExtensions, _fileSizeLimit);

                        var file = new UploadedFile
                        {
                            NameForDisplay = trustedFileNameForDisplay,
                            FileNameForFileStorage = trustedFileNameForFileStorage,
                            ByteArray = streamedFileContent,
                            Url = "",
                            Created = DateTimeOffset.Now,
                            Updated = null,
                            Meta = "",
                            Access = 0,
                            Flags = 0,
                            MimeType = section.ContentType ?? "",
                            Path = _targetFilePath,
                            Size = streamedFileContent.Length,
                        };

                        var fileExists = await _fileManager.Exists(streamedFileContent);
                        if (fileExists)
                        {
                            await _fileManager.AddOrUpdate(file);
                            break;
                        }


                        if (!ModelState.IsValid)
                        {
                            return BadRequest(ModelState);
                        }

                        await _fileManager.AddOrUpdate(file);

                        await using (var targetStream = System.IO.File.Create(
                            Path.Combine(_targetFilePath, trustedFileNameForFileStorage)))
                        {
                            await targetStream.WriteAsync(streamedFileContent);

                            //_logger.LogInformation(
                            //    "Uploaded file '{TrustedFileNameForDisplay}' saved to " +
                            //    "'{TargetFilePath}' as {TrustedFileNameForFileStorage}",
                            //    trustedFileNameForDisplay, _targetFilePath,
                            //    trustedFileNameForFileStorage);
                        }
                    }
                }

                // Drain any remaining section body that hasn't been consumed and
                // read the headers for the next section.
                section = await reader.ReadNextSectionAsync();
            }

            return Created(nameof(FileController), null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file">Хеш сумма файла в sha1</param>
        /// <returns></returns>
        [HttpGet]
        [Route("get")]
        public async Task<IActionResult> Get(string file)
        {
            var result = await _fileManager.Get(file);
            if(result == null) return NotFound();
            var path = Path.Combine(result.Path + result.FileNameForFileStorage);
            var byteArray = await System.IO.File.ReadAllBytesAsync(path);

            return File(byteArray, result.MimeType, result.NameForDisplay);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file">Хеш сумма файла в sha1</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> Delete(string file)
        {
            var result = await _fileManager.Get(file);
            if(result == null) return NotFound();

            await _fileManager.Delete(file);
            return Ok();
        }
    }
}
