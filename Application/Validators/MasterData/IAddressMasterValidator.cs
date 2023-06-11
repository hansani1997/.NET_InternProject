using BlueLotus360.CleanArchitecture.Application.Validators.MessageSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.CleanArchitecture.Application.Validators.MasterData
{
    public interface IAddressMasterValidator
    {
        UserMessageManager ValidationMessages { get; set; }
        bool IsValidAddress();

    }
}
