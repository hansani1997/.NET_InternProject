using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BlueLotus360.Com.Application.Extensions;
using BlueLotus360.Com.Application.Interfaces.Repositories;
using BlueLotus360.Com.Application.Interfaces.Services;
using BlueLotus360.Com.Application.Specifications.ExtendedAttribute;
using BlueLotus360.Com.Domain.Contracts;
using BlueLotus360.Com.Domain.Enums;
using BlueLotus360.Com.Shared.Wrapper;
using MediatR;

using Microsoft.Extensions.Localization;

namespace BlueLotus360.Com.Application.Features.ExtendedAttributes.Queries.Export
{
    internal class ExportExtendedAttributesQueryLocalization
    {
        // for localization
    }

    public class ExportExtendedAttributesQuery<TId, TEntityId, TEntity, TExtendedAttribute>
        : IRequest<Result<string>>
            where TEntity : AuditableEntity<TEntityId>, IEntityWithExtendedAttributes<TExtendedAttribute>, IEntity<TEntityId>
            where TExtendedAttribute : AuditableEntityExtendedAttribute<TId, TEntityId, TEntity>, IEntity<TId>
            where TId : IEquatable<TId>
    {
        public string SearchString { get; set; }
        public TEntityId EntityId { get; set; }
        public bool IncludeEntity { get; set; }
        public bool OnlyCurrentGroup { get; set; }
        public string CurrentGroup { get; set; }

        public ExportExtendedAttributesQuery(string searchString = "", TEntityId entityId = default, bool includeEntity = false, bool onlyCurrentGroup = false, string currentGroup = "")
        {
            SearchString = searchString;
            EntityId = entityId;
            IncludeEntity = includeEntity;
            OnlyCurrentGroup = onlyCurrentGroup;
            CurrentGroup = currentGroup;
        }
    }

    internal class ExportExtendedAttributesQueryHandler<TId, TEntityId, TEntity, TExtendedAttribute>
        : IRequestHandler<ExportExtendedAttributesQuery<TId, TEntityId, TEntity, TExtendedAttribute>, Result<string>>
            where TEntity : AuditableEntity<TEntityId>, IEntityWithExtendedAttributes<TExtendedAttribute>, IEntity<TEntityId>
            where TExtendedAttribute : AuditableEntityExtendedAttribute<TId, TEntityId, TEntity>, IEntity<TId>
            where TId : IEquatable<TId>
    {
        private readonly IExcelService _excelService;
        private readonly IUnitOfWork<TId> _unitOfWork;
        private readonly IStringLocalizer<ExportExtendedAttributesQueryLocalization> _localizer;

        public ExportExtendedAttributesQueryHandler(IExcelService excelService
            , IUnitOfWork<TId> unitOfWork
            , IStringLocalizer<ExportExtendedAttributesQueryLocalization> localizer)
        {
            _excelService = excelService;
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<string>> Handle(ExportExtendedAttributesQuery<TId, TEntityId, TEntity, TExtendedAttribute> request, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            return null;
        }
    }
}