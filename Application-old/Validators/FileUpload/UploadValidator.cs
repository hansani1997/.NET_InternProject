using BL10.CleanArchitecture.Domain.Entities.Document;
using BlueLotus360.CleanArchitecture.Application.Validators.MessageSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL10.CleanArchitecture.Application.Validators.FileUpload
{
    public class UploadValidator:IUploadValidator
    {
        private FileUploadValidation _file;
        public UserMessageManager UserMessages { get; set; }

        public UploadValidator(FileUploadValidation file)
        {
            _file = file;
            UserMessages = new UserMessageManager();
        }
        public bool CanFileUpload()
        {
            UserMessages.UserMessages.Clear();


            if (_file.MaxAllowedFiles < (_file.NumberofFilesUploaded + 1))
            {
                UserMessages.AddErrorMessage($"Can't add more than {_file.MaxAllowedFiles} files");
            }


            if (_file.SelectedFile.Size > _file.MaxFileSize)
            {
                UserMessages.AddErrorMessage($"size of {_file.SelectedFile.FileName} is more than 10MB,please add 10MB or less");
            }
            foreach (string extension in _file.AcceptedFileExtensions)
            {
                if (_file.SelectedFile.Extension.Equals(extension))
                {
                    _file.SelectedFile.HasAcceptedExtension = true;
                    break;
                }
            }
            if (!_file.SelectedFile.HasAcceptedExtension)
            {
                UserMessages.AddErrorMessage($"{_file.SelectedFile.Extension} is not supported file type");

            }




            return UserMessages.UserMessages.Count == 0;

        }
    }
}
