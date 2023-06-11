using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL10.CleanArchitecture.Domain.DTO
{
    public class BaseServerFilterInfo
    {
        public string? Sort { get; set; } = "";
        public int ObjKy { get; set; } = 1;
        public string? SortColumn
        {
            get
            {

                if (Sort == null || Sort.Trim().Length == 0)
                {
                    return "";
                }
                string[] arr = Sort.Split('-');
                if (arr.Length == 2)
                {
                    return arr[0];
                }
                return "";

            }
        }
        public string? SortDirection
        {
            get
            {

                if (Sort == null || Sort.Trim().Length == 0)
                {
                    return "";
                }
                string[] arr = Sort.Split('-');
                if (arr.Length == 2)
                {
                    return arr[1];
                }
                return "";

            }
        }
        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public string? Filter { get; set; } = "IsActive~eq~'--bltrue--'";

        public string? RequestingURL { get; set; } = "";
    }
}
