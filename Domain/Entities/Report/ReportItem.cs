using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL10.CleanArchitecture.Domain.Entities.Report
{
    public class ReportItem:ReportBaseItem
    {
        private int menuId;
        private string menuCode = "";
        private string menuname = "";
        private string menuCaption = "";
        private string controllerName = "";
        private string controllerAction = "";
        private string target = "";
        private string customeStyle = "";
        private string ourCode = "";
        private int parentId;
        private bool isRoot;
        private bool ispinned;
        private long usrObjKy;
        public bool IsDebug { get; set; }


        private IList<ReportItem> subMenus;

        


        public int MenuId { get => menuId; set => menuId = value; }
        public string? MenuCode { get => menuCode; set => menuCode = value; }
        public string? MenuName { get => menuname; set => menuname = value; }
        public string? MenuCaption { get => menuCaption; set => menuCaption = value; }
        public string? ControllerName { get => controllerName; set => controllerName = value; }
        public string? ControllerAction { get => controllerAction; set => controllerAction = value; }
        public string? Target { get => target; set => target = value; }
        public string? CustomeStyle { get => customeStyle; set => customeStyle = value; }
        public string? OurCode { get => ourCode; set => ourCode = value; }
        public string? OurCode2 { get; set; }
        public string? MenuIcon { get; set; }
        public IList<ReportItem> SubMenus { get => subMenus; set => subMenus = value; }
        public bool IsRoot { get => isRoot; set => isRoot = value; }
        public int ParentId { get => parentId; set => parentId = value; }
        public bool Ispinned { get => ispinned; set => ispinned = value; }
        public long UserObjectKey { get => usrObjKy; set => usrObjKy = value; }
        public ReportItem()
        {
            SubMenus = new List<ReportItem>();

        }
    }
}
