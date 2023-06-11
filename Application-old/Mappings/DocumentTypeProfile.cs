using AutoMapper;
using BlueLotus360.Com.Application.Features.DocumentTypes.Commands.AddEdit;
using BlueLotus360.Com.Application.Features.DocumentTypes.Queries.GetAll;
using BlueLotus360.Com.Application.Features.DocumentTypes.Queries.GetById;
using BlueLotus360.Com.Domain.Entities.Misc;

namespace BlueLotus360.Com.Application.Mappings
{
    public class DocumentTypeProfile : Profile
    {
        public DocumentTypeProfile()
        {
            CreateMap<AddEditDocumentTypeCommand, DocumentType>().ReverseMap();
            CreateMap<GetDocumentTypeByIdResponse, DocumentType>().ReverseMap();
            CreateMap<GetAllDocumentTypesResponse, DocumentType>().ReverseMap();
        }
    }
}