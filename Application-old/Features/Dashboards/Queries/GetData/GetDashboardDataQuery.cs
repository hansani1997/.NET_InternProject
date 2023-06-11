using BlueLotus360.Com.Application.Interfaces.Repositories;
using BlueLotus360.Com.Application.Interfaces.Services.Identity;
using BlueLotus360.Com.Domain.Entities.Catalog;
using BlueLotus360.Com.Shared.Wrapper;
using MediatR;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BlueLotus360.Com.Domain.Entities.ExtendedAttributes;
using BlueLotus360.Com.Domain.Entities.Misc;
using Microsoft.Extensions.Localization;

namespace BlueLotus360.Com.Application.Features.Dashboards.Queries.GetData
{
    public class GetDashboardDataQuery : IRequest<Result<DashboardDataResponse>>
    {

    }

    internal class GetDashboardDataQueryHandler : IRequestHandler<GetDashboardDataQuery, Result<DashboardDataResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly IStringLocalizer<GetDashboardDataQueryHandler> _localizer;

        public GetDashboardDataQueryHandler(IUnitOfWork<int> unitOfWork, IUserService userService, IRoleService roleService, IStringLocalizer<GetDashboardDataQueryHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _userService = userService;
            _roleService = roleService;
            _localizer = localizer;
        }

        public async Task<Result<DashboardDataResponse>> Handle(GetDashboardDataQuery query, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            return null;
        }
    }
}