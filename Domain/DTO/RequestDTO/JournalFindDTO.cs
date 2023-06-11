using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL10.CleanArchitecture.Domain.DTO.RequestDTO
{
    public class JournalFindDTO
    {
        public string TransactionNumber { get; set; } = "";
        public AccountResponse Account { get; set; }
        public string Description { get; set; } = "";
        public CodeBaseResponse TransactionType { get; set; }
        public long ObjectKey { get; set; } = 1;
        public JournalFindDTO()
        {
            Account = new AccountResponse();
            Description = string.Empty;
            TransactionType = new CodeBaseResponse();
        }
    }

    public class JournalLiteFindResponseDTO
    {
        public long ObjectKey { get; set; } = 1;
        public int TransactionKey { get; set; } = 1;
        public int TransactionNumber { get; set; }
        public string UserId { get; set; } = "";
        public DateTime InsertDate { get; set; }
        public JournalLiteFindResponseDTO()
        {
            TransactionKey = 1;
            TransactionNumber = 1;
            UserId = string.Empty;
            InsertDate = DateTime.Now;
        }

    }
}
