using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PdfUploadApp.Pages
{
    public class UploadModel : PageModel
    {
        [BindProperty]
        public string UploadedFilePath { get; set; }

        public List<FileUploadInfo> UploadedFiles { get; set; } = new List<FileUploadInfo>();
        public string ErrorMessage { get; set; } // Property to hold error messages

        public void OnGet()
        {
            // Load previously uploaded files from a persistent store or in-memory list
            UploadedFiles = LoadUploadedFiles();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var file = Request.Form.Files[0];

            if (file != null && file.Length > 0)
            {
                // Check if the file is a PDF
                if (Path.GetExtension(file.FileName).ToLower() != ".pdf")
                {
                    ErrorMessage = "Only PDF files are allowed.";
                    return Page();
                }

                var uploadsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                Directory.CreateDirectory(uploadsFolderPath);

                // Remove all previous files in the uploads folder
                var existingFiles = Directory.GetFiles(uploadsFolderPath);
                foreach (var existingFile in existingFiles)
                {
                    System.IO.File.Delete(existingFile);
                }

                var filePath = Path.Combine(uploadsFolderPath, file.FileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

               // UploadedFilePath = $"/uploads/{file.FileName}";//for local
                UploadedFilePath = $"/fileupload/uploads/{file.FileName}";//for server

                // Store file info with upload date
                UploadedFiles.Clear(); // Clear previous file info
                UploadedFiles.Add(new FileUploadInfo
                {
                    FileName = file.FileName,
                    UploadDate = DateTime.Now
                });
            }

            return Page();
        }

        private List<FileUploadInfo> LoadUploadedFiles()
        {
            // This method should retrieve the list of uploaded files from a persistent store
            return new List<FileUploadInfo>();
        }
    }

    public class FileUploadInfo
    {
        public string FileName { get; set; }
        public DateTime UploadDate { get; set; }
    }
}