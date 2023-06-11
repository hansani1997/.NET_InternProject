using BL10.CleanArchitecture.Domain.Entities.Document;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.Com.commonLib.Managers.UploadManager
{
    public interface IUploadManager:IManager
    {
        Task UploadFile(FileUpload uploadReq);
        Task<IList<Base64Document>> getBase64Documents(DocumentRetrivaltDTO document);
        Task<IList<Base64Document>> GetAllDocuments(DocumentRetrivaltDTO document);
        Task<bool> DeleteDocument(DocumentRetrivaltDTO document);
        Task<IList<Base64Document>> getBase64DocumentsV2(DocumentRetrivaltDTO document);
    }
}
