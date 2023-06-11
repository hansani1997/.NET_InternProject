using BL10.CleanArchitecture.Domain.DTO.RequestDTO;
using BL10.CleanArchitecture.Domain.Entities.Financial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.Com.commonLib.Managers.PaymentManager
{
    public interface IPaymentManager : IManager
    {
        Task<IList<JournalLiteFindResponseDTO>> FindJournalDetails(JournalFindDTO dto);
        Task<BLJournalLite> SelectAccTrnSingleEntryDetail(JournalLiteFindResponseDTO model);
        Task<BLJournalLite> InsertSingleEntryDetail(BLJournalLite dto);
        Task DeleteSingleEntry(AccTrnSingleEntry dto);
    }
}
