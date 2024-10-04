using Microsoft.AspNetCore.Mvc;

public class FilesController : Controller
{
    private readonly BlobService _blobService;
    private readonly EmailService _emailService;

    public FilesController(BlobService blobService, EmailService emailService)
    {
        _blobService = blobService;
        _emailService = emailService;
    }

    public async Task<IActionResult> Index()
    {
        var files = await _blobService.ListFilesAsync();
        return View(files);
    }

    [HttpPost]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        if (file != null)
        {
            await _blobService.UploadFileAsync(file);

            // Send email notification
            await _emailService.SendEmailAsync(
                "recipient@example.com",
                "File Uploaded",
                $"File {file.FileName} has been uploaded.");

            return RedirectToAction("Index");
        }
        return View("Index");
    }

    public async Task<IActionResult> Download(string fileName)
    {
        var stream = await _blobService.DownloadFileAsync(fileName);
        return File(stream, "application/octet-stream", fileName);
    }

    public async Task<IActionResult> Delete(string fileName)
    {
        await _blobService.DeleteFileAsync(fileName);

        // Send email notification
        await _emailService.SendEmailAsync(
            "recipient@example.com",
            "File Deleted",
            $"File {fileName} has been deleted.");

        return RedirectToAction("Index");
    }
}
