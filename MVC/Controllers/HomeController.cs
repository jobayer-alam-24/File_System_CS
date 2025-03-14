using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;

namespace MVC.Controllers;

public class HomeController : Controller
{
    private readonly IWebHostEnvironment _environment;
    public HomeController(IWebHostEnvironment environment)
    {
        _environment = environment;
    }
    public IActionResult Index()
    {
        //Directory.GetFiles() - returns an array of files from specified path (Folder).
        string[] getFiles = Directory.GetFiles(Path.Combine(_environment.WebRootPath, "Files"));
        List<FileData> filesNames = new List<FileData>();
        for (int i = 0; i < getFiles.Length; i++)
        {
            filesNames.Add(new FileData() { No = i + 1, Name = Path.GetFileName(getFiles[i]) });
        }
        return View(filesNames);
    }
    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create([FromForm] IFormFile file)
    {
        if (file != null && file.Length > 0)
        {
            string path = Path.Combine(_environment.WebRootPath, "Files/", file.FileName);
            string extension = Path.GetExtension(path);
            if (extension.Equals(".pdf") || extension.Equals(".docx") || extension.Equals(".pptx") || extension.Equals(".xlsx"))
            {
                if (!System.IO.File.Exists(path))
                {
                    using (var stream = new FileStream(path, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.Read))
                    {
                        await file.CopyToAsync(stream);
                    }
                }
                else
                {
                    using (var stream = new FileStream(path, FileMode.Truncate, FileAccess.ReadWrite, FileShare.Read))
                    {
                        await file.CopyToAsync(stream);
                    }
                }
            }
            else
            {
                ViewBag.typeError = "File Extension must be: .pdf, .docx, .pptx, .xlsx";
                return View();
            }
        }
        else
        {
            ViewBag.notUploaded = "Please! Upload a file.";
            return View();
        }
        TempData["success"] = "FILE Uploaded Successfully!";
        return RedirectToAction(nameof(Index));
    }
    public async Task<FileResult> Download(string name)
    {
        string path = Path.Combine(_environment.WebRootPath, "Files/", name);
        byte[] contents = await System.IO.File.ReadAllBytesAsync(path);
        return File(contents, "application/octet-stream", name);
    }
    public IActionResult Details(string name)
    {
        string path = Path.Combine(_environment.WebRootPath, "Files/", name);
        FileInfo info = new FileInfo(path);
        return View(info);
    }
    public IActionResult Delete(string name)
    {
        string path = Path.Combine(_environment.WebRootPath, "Files/", name);
        if(System.IO.File.Exists(path))
        {
            System.IO.File.Delete(path);
            TempData["delete_messege"] = $"{name} is Deleted!";
        }
        return  RedirectToAction(nameof(Index));
    }

    public async Task<FileResult> Open(string name)
    {
        string path = Path.Combine(_environment.WebRootPath, "Files/", name);
        byte[] contents = await System.IO.File.ReadAllBytesAsync(path);
        Response.Headers["Content-Disposition"] = "inline; filename=" + name; 

        return File(contents, "application/pdf");
    }
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
