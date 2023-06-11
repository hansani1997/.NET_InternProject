using BlueLotus360.CleanArchitecture.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL10.CleanArchitecture.Domain.Entities.Document
{
    public class Document
    {
    }

    public class FileUpload : DocumentRetrivaltDTO
    {
        public FileType UploadedFile { get; set; } = new FileType();
        public string? Description { get; set; } = "";
        public byte[] Buffer { get; set; } = new byte[] { };
    }
    public class FileType
    {
        public long Size { get; set; }
        public string? FileName { get; set; } = "";
        public string? Extension { get; set; } = "";
        public bool HasAcceptedExtension { get; set; }

    }
    public class FileUploadValidation
    {
        public int MaxAllowedFiles { get; set; }
        public long MaxFileSize { get; set; }
        public int NumberofFilesUploaded { get; set; }
        public FileType? SelectedFile { get; set; } = new FileType();
        public List<string> AcceptedFileExtensions { get; set; } = new List<string>();


    }
}
