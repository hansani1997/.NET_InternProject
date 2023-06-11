using BlueLotus360.Com.Application.Interfaces.Repositories;
using BlueLotus360.Com.Domain.Entities.Catalog;
using BlueLotus360.Com.Shared.Wrapper;
using MediatR;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BlueLotus360.Com.Application.Features.Products.Queries.GetProductImage
{
    public class GetProductImageQuery : IRequest<Result<string>>
    {
        public int Id { get; set; }

        public GetProductImageQuery(int productId)
        {
            Id = productId;
        }
    }

    internal class GetProductImageQueryHandler : IRequestHandler<GetProductImageQuery, Result<string>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;

        public GetProductImageQueryHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<string>> Handle(GetProductImageQuery request, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            return null;
        }
    }
}