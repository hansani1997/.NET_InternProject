using BL10.CleanArchitecture.Domain.Entities.ProjectProfileMobile;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.Com.commonLib.Managers.ProjectProfileMobile
{
    public interface IProjectProfileMobileManager : IManager
    {
        bool IsExceptionthrown();
        Task<IList<ProjectProfileList>> GetProjectProfileList(ProjectProfileRequest request);

        Task<ProjectProfileList> InsertProjectList(ProjectProfileList request);
        Task<ProjectProfileList> UpdateProjectList(ProjectProfileList request);
        Task<IList<ProjectResponse>> GetAllProjects(ComboRequestDTO request);
        Task<IList<TaskResponse>> GetTasksByProject(ComboRequestDTO request);

    }
}
