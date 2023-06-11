using BlueLotus360.CleanArchitecture.Application.Responses.ServerResponse;
using BlueLotus360.CleanArchitecture.Domain.DTO.MasterData;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.Com.commonLib.Managers.Address
{
    public interface IAddressManager : IManager
    {
        Task<AddressCreateServerResponse> CreateNewAddress(AddressMaster record);
        Task<AddressMaster> CreateCustomer(AddressMaster customer);
        Task<AddressMaster> CreateCustomerValidation(AddressMaster customer);
        Task<AddressMaster> CheckAdvanceAnalysisAvailability(AddressMaster customer);
        Task<AddressMaster> CreateAdvanceAnalysis(AddressMaster customer);
        Task<AddressResponse> GetAddressByUserKy();
        //Task<AddressCreateServerResponse> CreateNewAddress(AddressMaster addressMaster);
    }
}
