using AutoMapper;
using BlueLotus360.Com.Application.Features.Brands.Commands.AddEdit;
using BlueLotus360.Com.Application.Features.Brands.Queries.GetAll;
using BlueLotus360.Com.Application.Features.Brands.Queries.GetById;
using BlueLotus360.Com.Domain.Entities.Catalog;

namespace BlueLotus360.Com.Application.Mappings
{
    public class BrandProfile : Profile
    {
        public BrandProfile()
        {
            CreateMap<AddEditBrandCommand, Brand>().ReverseMap();
            CreateMap<GetBrandByIdResponse, Brand>().ReverseMap();
            CreateMap<GetAllBrandsResponse, Brand>().ReverseMap();
        }
    }
}