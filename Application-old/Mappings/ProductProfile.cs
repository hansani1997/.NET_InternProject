using AutoMapper;
using BlueLotus360.Com.Application.Features.Products.Commands.AddEdit;
using BlueLotus360.Com.Domain.Entities.Catalog;

namespace BlueLotus360.Com.Application.Mappings
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<AddEditProductCommand, Product>().ReverseMap();
        }
    }
}