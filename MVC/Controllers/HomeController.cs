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
        for(int i = 0; i < getFiles.Length; i++)
        {
            filesNames.Add(new FileData(){No = i + 1, Name = Path.GetFileName(getFiles[i])});
        }
        return View(filesNames);
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
