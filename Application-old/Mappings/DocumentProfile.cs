using AutoMapper;
using BlueLotus360.Com.Application.Features.Documents.Commands.AddEdit;
using BlueLotus360.Com.Application.Features.Documents.Queries.GetById;
using BlueLotus360.Com.Domain.Entities.Misc;

namespace BlueLotus360.Com.Application.Mappings
{
    public class DocumentProfile : Profile
    {
        public DocumentProfile()
        {
            CreateMap<AddEditDocumentCommand, Document>().ReverseMap();
            CreateMap<GetDocumentByIdResponse, Document>().ReverseMap();
        }
    }
}