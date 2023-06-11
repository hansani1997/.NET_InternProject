using BlueLotus360.CleanArchitecture.Domain.DTO.MasterData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.CleanArchitecture.Application.Responses.ServerResponse
{
    public class AddressCreateServerResponse : BaseServerResponse<AddressMaster>
    {
        public string AddressId { get; set; }
        public bool IsAddressIDAvailable { get; set; }


    }
}
