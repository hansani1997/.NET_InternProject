using BL10.CleanArchitecture.Domain.Entities.Report;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.Com.commonLib.Managers.NavMenuManager
{
    public interface INavMenuManager:IManager
    {
        public Task<IDictionary<string, BlueLotus360.CleanArchitecture.Domain.Entities.MenuItem>> GetNavAndPinnedMenus();

        Task<BLUIElement> GetMenuUIElement(ObjectFormRequest request);
        Task<MenuItem> GetPinnedMenus();
        Task UpdatePinnedMenus(BlueLotus360.CleanArchitecture.Domain.Entities.MenuItem menurequest);
        Task<UserConfigObjectsBlLite> LoadObjectsForUserConfiguration(ObjectFormRequest request);
        Task UpdateObjectsForUserConfiguration(UserConfigObjectsBlLite request);
        Task<IList<MenuItem>> SearchBlLiteMenu(MenuSearchRequest request);

        public Task<MenuItem> GetReportPinnedMenus();
        public Task<IList<ReportModuleItem>> GetReportModuleMenus();
        public Task<IList<ReportSubModule>> GetReportSubModuleMenus(SubModuleRequest PrntKy);
        public Task<IList<ReportFilterFields>> GetReportFilterFields(SubModuleRequest ParenttKey);
    }
}
