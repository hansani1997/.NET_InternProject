using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL10.CleanArchitecture.Domain.Entities.Report
{
    public class ReportFilterFields
    {
        public long objectKey;
        public long parentKey;
        public string objectName = "";
        public string objectCaption = "";
        public string toolTip = "";
        public string userObjectKey = "";
        public int patent2Key;
        public string defaultValue;
        public int isFreeze;
        public int isEnable;
        public string alignment = "";
        public int defaultKey;
        public int isVisible;
        public int width;
        public string format = "";
        public string aggregates = "";
        public string footerTemplate = "";
        public string onjectType = "";
        public string uRLController = "";
        public string uRLAction = "";
        public string expr1 = "";
        public string ourCd = "";
        public string ourCd2 = "";
        public string so = "";
        public string nextEntObjName = "";
        public string entryKeyAction = "";
        public string template = "";
        public long expr2;
        public int isFirstObject;
        public int isMust;
        private int parent2Key;
        private string objectType;
        private string defaultAccessPath = "";
        private string onClickAction = "";

        public long ObjectKey { get => objectKey; set => objectKey = value; }
        public long ParentKey { get => parentKey; set => parentKey = value; }
        public string ObjectName { get => objectName; set => objectName = value; }
        public string ObjectCaption { get => objectCaption; set => objectCaption = value; }
        public string ToolTip { get => toolTip; set => toolTip = value; }
        public string UserObjectKey { get => userObjectKey; set => userObjectKey = value; }
        public int Patent2Key { get => patent2Key; set => patent2Key = value; }
        public string DefaultValue { get => defaultValue; set => defaultValue = value; }
        public int IsFreeze { get => isFreeze; set => isFreeze = value; }
        public int IsEnable { get => isEnable; set => isEnable = value; }
        public string Alignment { get => alignment; set => alignment = value; }
        public int DefaultKey { get => defaultKey; set => defaultKey = value; }
        public int IsVisible { get => isVisible; set => isVisible = value; }
        public int Width { get => width; set => width = value; }
        public string Format { get => format; set => format = value; }
        public string Aggregates { get => aggregates; set => aggregates = value; }
        public string FooterTemplate { get => footerTemplate; set => footerTemplate = value; }
        public string OnjectType { get => onjectType; set => onjectType = value; }
        public string URLController { get => uRLController; set => uRLController = value; }
        public string URLAction { get => uRLAction; set => uRLAction = value; }
        public string Expr1 { get => expr1; set => expr1 = value; }
        public string OurCd { get => ourCd; set => ourCd = value; }
        public string OurCd2 { get => ourCd2; set => ourCd2 = value; }
        public string So { get => so; set => so = value; }
        public string NextEntObjName { get => nextEntObjName; set => nextEntObjName = value; }
        public string EntryKeyAction { get => entryKeyAction; set => entryKeyAction = value; }
        public string Template { get => template; set => template = value; }
        public long Expr2 { get => expr2; set => expr2 = value; }
        public int IsFirstObject { get => isFirstObject; set => isFirstObject = value; }
        public int IsMust { get => isMust; set => isMust = value; }
        public int Parent2Key { get => parent2Key; set => parent2Key = value; }
        public string ObjectType { get => objectType; set => objectType = value; }
        public string DefaultAccessPath { get => defaultAccessPath; set => defaultAccessPath = value; }
        public string OnClickAction { get => onClickAction; set => onClickAction = value; }
    }
}
