using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
namespace Ofentse.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IWebHostEnvironment _environment;

        public IndexModel(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public List<MemoryFile> MemoryFiles { get; set; } = new();

        [BindProperty]
        public IFormFileCollection UploadedFiles { get; set; }

        public void OnGet()
        {
            // Load existing files from the "Uploads" directory
            var uploadsPath = Path.Combine(_environment.WebRootPath, "uploads");
            if (Directory.Exists(uploadsPath))
            {
                var files = Directory.GetFiles(uploadsPath);
                MemoryFiles = files.Select(file => new MemoryFile
                {
                    FileName = Path.GetFileName(file),
                    FileUrl = $"/uploads/{Path.GetFileName(file)}",
                    FileType = GetFileType(file)
                }).ToList();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (UploadedFiles != null && UploadedFiles.Any())
            {
                var uploadsPath = Path.Combine(_environment.WebRootPath, "uploads");

                // Ensure the "uploads" directory exists
                if (!Directory.Exists(uploadsPath))
                {
                    Directory.CreateDirectory(uploadsPath);
                }

                // Save uploaded files
                foreach (var file in UploadedFiles)
                {
                    var filePath = Path.Combine(uploadsPath, file.FileName);
                    using var fileStream = new FileStream(filePath, FileMode.Create);
                    await file.CopyToAsync(fileStream);
                }
            }

            return RedirectToPage();
        }

        private string GetFileType(string filePath)
        {
            var extension = Path.GetExtension(filePath).ToLower();
            return extension switch
            {
                ".jpg" or ".jpeg" or ".png" or ".gif" => "image/" + extension.Trim('.'),
                ".mp4" or ".mov" => "video/" + extension.Trim('.'),
                ".mp3" or ".wav" => "audio/" + extension.Trim('.'),
                _ => "text/plain"
            };
        }
    }

    public class MemoryFile
    {
        public string FileName { get; set; }
        public string FileUrl { get; set; }
        public string FileType { get; set; }
    }
}
