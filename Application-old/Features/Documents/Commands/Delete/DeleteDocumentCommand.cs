using System.Linq;
using BlueLotus360.Com.Application.Interfaces.Repositories;
using BlueLotus360.Com.Domain.Entities.Misc;
using BlueLotus360.Com.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using BlueLotus360.Com.Shared.Constants.Application;

using Microsoft.Extensions.Localization;

namespace BlueLotus360.Com.Application.Features.Documents.Commands.Delete
{
    public class DeleteDocumentCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    internal class DeleteDocumentCommandHandler : IRequestHandler<DeleteDocumentCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<DeleteDocumentCommandHandler> _localizer;

        public DeleteDocumentCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<DeleteDocumentCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteDocumentCommand command, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            return null;
        }
    }
}