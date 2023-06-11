using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.CleanArchitecture.Domain.Entities
{
   public  class MenuItem
    {
		private int menuId;
		private string menuCode;
		private string menuname;
		private string menuCaption;
		private string controllerName;
		private string controllerAction;
		private string target;
		private string customeStyle;
		private string ourCode;
		private string reportPath;
        private int parentId;
		private bool isRoot;
		private bool ispinned;
		private bool isChanged;
		private long usrObjKy;
        private int childKy;
        private string menuicon= "info-circle";
		private string pin_card_color;
		private IList<MenuItem> subMenus;
		private IList<MenuItem> tiles;


		public int MenuId { get => menuId; set => menuId = value; }
		public string? MenuCode { get => menuCode; set => menuCode = value; }
		public string? Menuname { get => menuname; set => menuname = value; }
		public string? MenuCaption { get => menuCaption; set => menuCaption = value; }
		public string? ControllerName { get => controllerName; set => controllerName = value; }
		public string? ControllerAction { get => controllerAction; set => controllerAction = value; }
		public string? Target { get => target; set => target = value; }
		public string? CustomeStyle { get => customeStyle; set => customeStyle = value; }
		public string? OurCode { get => ourCode; set => ourCode = value; }
		public string? OurCode2 { get; set; }

		public IList<MenuItem> SubMenus { get => subMenus; set => subMenus = value; }
		public IList<MenuItem> Tiles { get => tiles; set => tiles = value; }

		public bool IsRoot { get => isRoot; set => isRoot = value; }
		public int ParentId { get => parentId; set => parentId = value; }
		public string? MenuIcon { get; set; }
		public bool Ispinned { get => ispinned; set => ispinned = value; }
		public bool IsChanged { get => isChanged; set => isChanged = value; }
		public long UserObjectKey { get => usrObjKy; set => usrObjKy = value; }
        public int ChildKey { get => childKy; set => childKy = value; }
        public string? PinCardColor { get => pin_card_color; set => pin_card_color = value; }
        public string? ReportPath { get => reportPath; set => reportPath = value; }
        public MenuItem()
		{
			SubMenus = new List<MenuItem>();
		}

		public override string ToString()
		{
			//return MenuId.ToString() + "-" + Menuname;
			return MenuCaption;
		}


		public string GetPathURL()
        {
			if (!string.IsNullOrWhiteSpace(controllerAction) && !string.IsNullOrWhiteSpace(controllerName))
			{
				return (this.controllerName + "/" + this.controllerAction).ToLower()+"?ElementKey="+this.MenuId;

            }
				return "";
        }

    }

    public class MenuSearchRequest
    {
        public int IsPinned { get; set; }
        public int IsBlLite { get; set; }
        public string? Text { get; set; }
        public long ObjectKey { get; set; }

    }
}
