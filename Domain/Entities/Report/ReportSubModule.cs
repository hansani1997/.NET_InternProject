using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL10.CleanArchitecture.Domain.Entities.Report
{
    public class ReportSubModule
    {

        private int objKy;
        private int prntKy;
        private string objNm = "";
        private string objCaptn = "";
        private string reportPath = "";

        public int ObjKy { get => objKy; set => objKy = value; }
        public int PrntKy { get => prntKy; set => prntKy = value; }
        public string? ObjNm { get => objNm; set => objNm = value; }
        public string? ObjCaptn { get => objCaptn; set => objCaptn = value; }
        public string? ReportPath { get => reportPath; set => reportPath = value; }

    }
}
