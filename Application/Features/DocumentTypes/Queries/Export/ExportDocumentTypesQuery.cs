using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BlueLotus360.Com.Application.Extensions;
using BlueLotus360.Com.Application.Interfaces.Repositories;
using BlueLotus360.Com.Application.Interfaces.Services;
using BlueLotus360.Com.Application.Specifications.Misc;
using BlueLotus360.Com.Domain.Entities.Misc;
using BlueLotus360.Com.Shared.Wrapper;
using MediatR;

using Microsoft.Extensions.Localization;

namespace BlueLotus360.Com.Application.Features.DocumentTypes.Queries.Export
{
    public class ExportDocumentTypesQuery : IRequest<Result<string>>
    {
        public string SearchString { get; set; }

        public ExportDocumentTypesQuery(string searchString = "")
        {
            SearchString = searchString;
        }
    }

    internal class ExportDocumentTypesQueryHandler : IRequestHandler<ExportDocumentTypesQuery, Result<string>>
    {
        private readonly IExcelService _excelService;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<ExportDocumentTypesQueryHandler> _localizer;

        public ExportDocumentTypesQueryHandler(IExcelService excelService
            , IUnitOfWork<int> unitOfWork
            , IStringLocalizer<ExportDocumentTypesQueryHandler> localizer)
        {
            _excelService = excelService;
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<string>> Handle(ExportDocumentTypesQuery request, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            return null;
        }
    }
}