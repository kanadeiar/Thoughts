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
using Thoughts.Tools.Extensions;
using Thoughts.UI.MVC.Common;
using Thoughts.UI.MVC.Filters;
using Thoughts.UI.MVC.Utilities;

using static System.Net.WebRequestMethods;

namespace Thoughts.UI.MVC.Controllers
{
    [Route("fileapi/")]
    public class FileApiController : Controller
    {

        private readonly long _fileSizeLimit;

        private readonly string _targetFilePath;

        private readonly string[] _permittedExtensions;

        private static readonly FormOptions _defaultFormOptions = new FormOptions();

        private readonly IFileManager _fileManager;

        public FileApiController(SharedConfiguration sharedConfiguration, IFileManager fileManager)
        {
            _fileManager = fileManager;
            _fileSizeLimit = sharedConfiguration.FileSizeLimit;

            // To save physical files to a path provided by configuration:
            _targetFilePath = sharedConfiguration.TargetFilePath;

            // To save physical files to the temporary files folder, use:
            //_targetFilePath = Path.GetTempPath();
            _permittedExtensions = sharedConfiguration.PermittedExtensionsForUploadedFile;
        }


        /// <summary>
        /// Загрузка файла с проверкой файла на разрешенные расширения файла
        /// Максимальный размер файла ограничен размером байтового массива
        /// </summary>
        /// <returns></returns>
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
                            Sha1 = await streamedFileContent.GetSha1Async(),
                            Md5 = await streamedFileContent.GetMd5Async(),
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

            return Created(nameof(FileApiController), null);
        }



        /// <summary>
        /// Загрузка файла без ограничений по размеру
        /// Файл не проверяется на разрешенные расширения
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [DisableRequestSizeLimit]
        [Route("uploadlarge")]
        public async Task<IActionResult> UploadLargeFile()
        {
            var request = HttpContext.Request;

            // validation of Content-Type
            // 1. first, it must be a form-data request
            // 2. a boundary should be found in the Content-Type
            if (!request.HasFormContentType ||
                !MediaTypeHeaderValue.TryParse(request.ContentType, out var mediaTypeHeader) ||
                string.IsNullOrEmpty(mediaTypeHeader.Boundary.Value))
            {
                return new UnsupportedMediaTypeResult();
            }

            var reader = new MultipartReader(mediaTypeHeader.Boundary.Value, request.Body);
            var section = await reader.ReadNextSectionAsync();

            var saveToPath = "";
            var fileExists = false;
            // This sample try to get the first file from request and save it
            // Make changes according to your needs in actual use
            while (section != null)
            {
                var hasContentDispositionHeader = ContentDispositionHeaderValue.TryParse(section.ContentDisposition,
                    out var contentDisposition);

                if (hasContentDispositionHeader && contentDisposition.DispositionType.Equals("form-data") &&
                    !string.IsNullOrEmpty(contentDisposition.FileName.Value))
                {
                    // Don't trust any file name, file extension, and file data from the request unless you trust them completely
                    // Otherwise, it is very likely to cause problems such as virus uploading, disk filling, etc
                    // In short, it is necessary to restrict and verify the upload
                    // Here, we just use the temporary folder and a random file name

                    // Get the temporary folder, and combine a random file name with it
                    var trustedFileNameForFileStorage = Path.GetRandomFileName();
                    saveToPath = Path.Combine(_targetFilePath, trustedFileNameForFileStorage);


                    await using (var targetStream = System.IO.File.Create(saveToPath))
                    {
                        await section.Body.CopyToAsync(targetStream);
                    }

                    var fi = new FileInfo(saveToPath);
                    await using (var fs = fi.OpenRead())
                    {
                        var sha1 = await fs.GetSha1Async();
                        fileExists = await _fileManager.Exists(sha1);

                        var file = new UploadedFile
                        {
                            Sha1 = sha1,
                            Md5 = await fi.FullName.GetMd5Async(),
                            NameForDisplay = contentDisposition.FileName.Value,
                            FileNameForFileStorage = trustedFileNameForFileStorage,
                            Url = "",
                            Created = DateTimeOffset.Now,
                            Updated = null,
                            Meta = "",
                            Access = 0,
                            Flags = 0,
                            MimeType = section.ContentType ?? "",
                            Path = _targetFilePath,
                            Size = fi.Length,
                            Stream = fs,
                            Active = true
                        };

                        if (fileExists)
                        {
                            await _fileManager.AddOrUpdate(file);
                            //TODO Delete exists file
                        }

                        await _fileManager.AddOrUpdate(file);
                    }

                    return Ok();
                }

                section = await reader.ReadNextSectionAsync();
            }

            // If the code runs to this location, it means that no files have been saved
            return BadRequest("No files data in the request.");
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
            if (result == null || !result.Active) return NotFound();
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
            if (result == null) return NotFound();

            await _fileManager.Delete(file);
            return Ok();
        }

        [HttpGet]
        [Route("softdelete")]
        public async Task<IActionResult> SoftDelete(string file)
        {
            var result = await _fileManager.SoftDelete(file);
            return Json(new { result });
        }

        [HttpGet]
        [Route("activatefile")]
        public async Task<IActionResult> ActivateFile(string file)
        {
            var result = await _fileManager.ActivateFile(file);
            return Json(new { result });
        }

        [HttpGet]
        [Route("getallfileinfo")]
        public async Task<IActionResult> GetAllFileInfo()
        {
            var uploadedFiles = await _fileManager.GetAllFilesInfo();
            var result = uploadedFiles
                .Select(item => 
                    new
                    {
                        Hash = item.Sha1,
                        Counter = item.Counter,
                        Name = item.NameForDisplay,
                        Size = item.Size,
                        Created = item.Created,
                        Active = item.Active,
                    });
            return Json(result);
        }
    }
}
