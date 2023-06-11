using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.CleanArchitecture.Domain.Entities
{
    public class EntityBase
    {
        public Guid UUID { get; set; }

        public override bool Equals(object obj)
        {
            try
            {
                EntityBase enity = obj as EntityBase;
                if(enity != null)
                {
                    return enity.UUID.ToString().Equals(UUID.ToString());
                }
                return false;
            }
           catch{

            }
            return false;
        }
    }
}
