using BlueLotus360.CleanArchitecture.Application.Validators.MessageSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL10.CleanArchitecture.Application.Validators.FileUpload
{
    public interface IUploadValidator
    {
        UserMessageManager UserMessages { get; set; }
        bool CanFileUpload();
    }
}
