using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BlueLotus360.Com.Application.Interfaces.Repositories;
using BlueLotus360.Com.Domain.Contracts;
using BlueLotus360.Com.Shared.Constants.Application;
using BlueLotus360.Com.Shared.Wrapper;
using MediatR;
using Microsoft.Extensions.Localization;

namespace BlueLotus360.Com.Application.Features.ExtendedAttributes.Commands.Delete
{
    internal class DeleteExtendedAttributeCommandLocalization
    {
        // for localization
    }

    public class DeleteExtendedAttributeCommand<TId, TEntityId, TEntity, TExtendedAttribute>
        : IRequest<Result<TId>>
            where TEntity : AuditableEntity<TEntityId>, IEntityWithExtendedAttributes<TExtendedAttribute>, IEntity<TEntityId>
            where TExtendedAttribute : AuditableEntityExtendedAttribute<TId, TEntityId, TEntity>, IEntity<TId>
            where TId : IEquatable<TId>
    {
        public TId Id { get; set; }
    }

    internal class DeleteExtendedAttributeCommandHandler<TId, TEntityId, TEntity, TExtendedAttribute>
        : IRequestHandler<DeleteExtendedAttributeCommand<TId, TEntityId, TEntity, TExtendedAttribute>, Result<TId>>
            where TEntity : AuditableEntity<TEntityId>, IEntityWithExtendedAttributes<TExtendedAttribute>, IEntity<TEntityId>
            where TExtendedAttribute : AuditableEntityExtendedAttribute<TId, TEntityId, TEntity>, IEntity<TId>
            where TId : IEquatable<TId>
    {
        private readonly IStringLocalizer<DeleteExtendedAttributeCommandLocalization> _localizer;
        private readonly IExtendedAttributeUnitOfWork<TId, TEntityId, TEntity> _unitOfWork;

        public DeleteExtendedAttributeCommandHandler(
            IExtendedAttributeUnitOfWork<TId, TEntityId, TEntity> unitOfWork,
            IStringLocalizer<DeleteExtendedAttributeCommandLocalization> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<TId>> Handle(DeleteExtendedAttributeCommand<TId, TEntityId, TEntity, TExtendedAttribute> command, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            return null;
        }
    }
}