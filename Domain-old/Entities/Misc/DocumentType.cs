using BlueLotus360.Com.Domain.Contracts;

namespace BlueLotus360.Com.Domain.Entities.Misc
{
    public class DocumentType : AuditableEntity<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}