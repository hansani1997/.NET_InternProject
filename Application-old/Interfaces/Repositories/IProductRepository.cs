using System.Threading.Tasks;

namespace BlueLotus360.Com.Application.Interfaces.Repositories
{
    public interface IProductRepository
    {
        Task<bool> IsBrandUsed(int brandId);
    }
}