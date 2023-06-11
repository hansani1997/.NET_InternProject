using BL10.CleanArchitecture.Domain.Entities;
using BlueLotus.Com.Domain.Entity;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.CleanArchitecture.Domain.Entities.HR
{
    public class HR
    {
        
    }

    public class LoggedUsers
    {
        public int UsrKy { get; set; } = 1;
        public string UsrId { get; set; } = "";
    }

    public class UserDetails
    {
        public string AdrID { get; set; } = "";
        public string AdrNm { get; set; } = "";
        public int AdrKy { get; set; } = 1;
        public DateTime DOB { get; set; }
        public string AdrFullNm { get; set; } = "";
    }

    public class MultiAtnAnlysis 
    {
        public long EmpKy { get; set; } = 1;
        public string FDt { get; set; } = "";
        public string TDt { get; set; } = "";
        public byte Chk { get; set; }
        public int PrjKy { get; set; } = 1;
        public int TaskKy { get; set; } = 1;
        public long Objky { get; set; } = 1;
        public MultiAtnAnlysis()
        {
            FDt = DateTime.Now.ToString("yyyy/MM/dd");
            TDt = DateTime.Now.ToString("yyyy/MM/dd");
            Chk = 0;
            PrjKy = 1;
            TaskKy = 1;
        }
    }

    public class MultiAtnAnlysis_Response 
    {
        public int MultiAtnDetKy { get; set; } = 1;
        public int AtnDetKy { get; set; } = 1;
        public DateTime Date { get; set; } = DateTime.Now;
        public DateTime? InDtm { get; set; }
        public DateTime? OutDtm { get; set; }
        public double INLatitude { get; set; }
        public double INLongitude { get; set; }
        public double OutLatitude { get; set; }
        public double OutLongitude { get; set; }
        public CodeBaseResponse Location { get; set; } = new CodeBaseResponse();
        public double TTlMint { get; set; }

        //swisstek
        public AddressResponse Address { get; set; } = new AddressResponse();
        public CodeBaseResponse AddressCategory3 { get; set; } = new CodeBaseResponse();
        public CodeBaseResponse WorkReason { get; set; } = new CodeBaseResponse();
        public bool IsDayStartRecord { get; set; }
        public bool IsDayEndRecord { get; set; }
        public MultiAtnAnlysis_Response()
        {
           Location= new CodeBaseResponse();
           AddressCategory3 = new CodeBaseResponse();
           WorkReason = new CodeBaseResponse();
           Address = new AddressResponse();
        }
         
        public TimeSpan GetWorkHours()
        {
            if (InDtm != null && OutDtm != null)
            {
                return OutDtm.Value.Subtract(InDtm.Value);
                
            }
            else
            {
                return TimeSpan.Zero;
            }
        }

        public bool HasPutOut()
        {
            if (IsDayStartRecord)
            {
                return true;
            }
            else if (IsDayEndRecord)
            {
                return true;
            }
            else if (OutDtm != null)
            {
                return true;
            }
            return false;
        }

    }
    
    public class InRequest 
    {
        public long EmpKy { get; set; } = 1;
        public string Date { get; set; } = DateTime.Now.ToString("yyyy/MM/dd");
    }

    public class InShift
    {
        public int ShiftKy { get; set; } = 1;
        public int DedMinute { get; set; }
        public int isHoliday { get; set; }
    }

    public class ManualAttendence:InRequest
    {
        public int ShiftKy { get; set; } = 1;
        public double Latitude { get; set; }
        public double  Longitude { get; set; }
        public CodeBaseResponse Location { get; set; }
        public int MultiAtnDetKy { get; set; } = 1;
        public int AtnDetKy { get; set; } = 1;
        public int IsHoliday { get; set; }
        public int IsIn { get; set; }
        public int IsOut { get; set; }
        public int IsOtherTyp { get; set; }
        public int IsoutWithoutIn { get; set; }
        public UpdateAttendence selectedAttendence { get; set; }

        //swiistek
        public AddressResponse Address { get; set; } = new AddressResponse();
        public CodeBaseResponse AddressCategory3 { get; set; } = new CodeBaseResponse();
        public CodeBaseResponse WorkReason { get; set; } = new CodeBaseResponse();

        public ManualAttendence()
        {
            Latitude = 0.00;
            Longitude = 0.00;
            MultiAtnDetKy = 1;
            IsIn = 0; 
            IsOut = 0;
            IsoutWithoutIn = 0;
            ShiftKy = 1;
            IsHoliday = 0;
            selectedAttendence = new UpdateAttendence();
            Location=new CodeBaseResponse();
            AddressCategory3 = new CodeBaseResponse();
            WorkReason = new CodeBaseResponse();
            Address = new AddressResponse();
        }

    }
    
    public class UpdateAttendence
    {
        public DateTime? InDtm { get; set; }
        public DateTime? OutDtm { get; set; }
        public int MultiAtnDetKy { get; set; } = 1;
        public int AtnDetKy { get; set; } = 1;
        public byte IsManual { get; set; }
        public byte IsOutManual { get; set; }

        public UpdateAttendence()
        {
            IsManual = 0;
            IsOutManual = 0;
        }
    }

    public class AddManualAdt
    {
        public long EmpKy { get; set; } = 1;
        public int ShiftKy { get; set; } = 1;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public CodeBaseResponse Location { get; set; } = new();
        public int MultiAtnDetKy { get; set; } = 1;
        public int IsHoliday { get; set; }
        public DateTime? InDtm { get; set; }
        public DateTime? OutDtm { get; set; }
        public DateTime? AtnDt { get; set; }
        public AddManualAdt()
        {
            AtnDt = DateTime.Now;
            InDtm = null;
            OutDtm = null;
        }

    }

    public class TaskwiseAttendance
    {
        public TaskwiseAttendance()
        {
            Project = new ProjectResponse();
            Task = new TaskResponse();
            TotalTime=TimeSpan.Zero;
        }
        public int TrnKy { get; set; }
        public int ItmTrnKy { get; set; }
        public ProjectResponse Project { get; set; }
        public TaskResponse Task { get; set; }
       
        public TimeSpan? TotalTime { get; set; }
        public DateTime SelectedDate { get; set; } 
        public string Date { get; set; } = "";
        public int ObjectKey { get;set; }
        public int AccKy { get; set; }
        public int TrnTypKy { get; set; }
        public int EmpKy { get; set; }
        public int AnlTyp2Ky { get; set; }
        public double TotalMinute { get; set; }
        public string Des { get; set; } = "";
    
    }

    
}
//((DateTime)OutDtm).CompareTo((DateTime)InDtm)>=0)