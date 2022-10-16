using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using Thoughts.DAL.Entities;
using Thoughts.Tools.Extensions;

namespace Thoughts.UI.MVC.Controllers;

public class FileController : Controller
{
    private readonly IFileManager _fileManager;

    private readonly string _targetFilePath;

    public FileController(IFileManager fileManager, SharedConfiguration sharedConfiguration)
    {
        _fileManager = fileManager;
        _targetFilePath = sharedConfiguration.TargetFilePath;
    }
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    [DisableRequestSizeLimit]
    //[ValidateAntiForgeryToken]
    public async Task<IActionResult> Upload()
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

        // This sample try to get the first file from request and save it
        // Make changes according to your needs in actual use
        while (section != null)
        {
            var hasContentDispositionHeader = ContentDispositionHeaderValue.TryParse(
                section.ContentDisposition,
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
                var saveToPath                    = Path.Combine(_targetFilePath, trustedFileNameForFileStorage);
                Directory.CreateDirectory(Path.GetDirectoryName(saveToPath)!);


                await using (var targetStream = System.IO.File.Create(saveToPath))
                {
                    await section.Body.CopyToAsync(targetStream);
                }

                var fi = new FileInfo(saveToPath);
                
                await using (var fs = fi.OpenRead())
                {
                    var  sha1       = await fs.GetSha1Async();
                    var fileExists = await _fileManager.Exists(sha1);

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

                return RedirectToAction("Index", "File");
            }

            section = await reader.ReadNextSectionAsync();
        }

        // If the code runs to this location, it means that no files have been saved
        return BadRequest("No files data in the request.");
    }
}
