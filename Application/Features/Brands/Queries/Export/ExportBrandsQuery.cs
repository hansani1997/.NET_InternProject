using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BlueLotus360.Com.Application.Extensions;
using BlueLotus360.Com.Application.Interfaces.Repositories;
using BlueLotus360.Com.Application.Interfaces.Services;
using BlueLotus360.Com.Application.Specifications.Catalog;
using BlueLotus360.Com.Domain.Entities.Catalog;
using BlueLotus360.Com.Shared.Wrapper;
using MediatR;

using Microsoft.Extensions.Localization;

namespace BlueLotus360.Com.Application.Features.Brands.Queries.Export
{
    public class ExportBrandsQuery : IRequest<Result<string>>
    {
        public string SearchString { get; set; }

        public ExportBrandsQuery(string searchString = "")
        {
            SearchString = searchString;
        }
    }

    internal class ExportBrandsQueryHandler : IRequestHandler<ExportBrandsQuery, Result<string>>
    {
        private readonly IExcelService _excelService;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<ExportBrandsQueryHandler> _localizer;

        public ExportBrandsQueryHandler(IExcelService excelService
            , IUnitOfWork<int> unitOfWork
            , IStringLocalizer<ExportBrandsQueryHandler> localizer)
        {
            _excelService = excelService;
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<string>> Handle(ExportBrandsQuery request, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            return null;
        }
    }
}
