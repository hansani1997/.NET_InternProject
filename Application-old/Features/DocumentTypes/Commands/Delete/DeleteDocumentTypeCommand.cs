﻿using System.Threading;
using System.Threading.Tasks;
using BlueLotus360.Com.Application.Interfaces.Repositories;
using BlueLotus360.Com.Domain.Entities.Misc;
using BlueLotus360.Com.Shared.Constants.Application;
using BlueLotus360.Com.Shared.Wrapper;
using MediatR;
using Microsoft.Extensions.Localization;

namespace BlueLotus360.Com.Application.Features.DocumentTypes.Commands.Delete
{
    public class DeleteDocumentTypeCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    internal class DeleteDocumentTypeCommandHandler : IRequestHandler<DeleteDocumentTypeCommand, Result<int>>
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly IStringLocalizer<DeleteDocumentTypeCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public DeleteDocumentTypeCommandHandler(IUnitOfWork<int> unitOfWork, IDocumentRepository documentRepository, IStringLocalizer<DeleteDocumentTypeCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _documentRepository = documentRepository;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteDocumentTypeCommand command, CancellationToken cancellationToken)
        {
            var isDocumentTypeUsed = await _documentRepository.IsDocumentTypeUsed(command.Id);
            if (!isDocumentTypeUsed)
            {
                var documentType = await _unitOfWork.Repository<DocumentType>().GetByIdAsync(command.Id);
                if (documentType != null)
                {
                    await _unitOfWork.Repository<DocumentType>().DeleteAsync(documentType);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllDocumentTypesCacheKey);
                    return await Result<int>.SuccessAsync(documentType.Id, _localizer["Document Type Deleted"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Document Type Not Found!"]);
                }
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Deletion Not Allowed"]);
            }
        }
    }
}