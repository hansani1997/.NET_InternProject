using BlueLotus360.Com.Domain.Entities.ExtendedAttributes;
using BlueLotus360.Com.Domain.Entities.Misc;
using Microsoft.Extensions.Localization;

namespace BlueLotus360.Com.Application.Validators.Features.ExtendedAttributes.Commands.AddEdit
{
    public class AddEditDocumentExtendedAttributeCommandValidator : AddEditExtendedAttributeCommandValidator<int, int, Document, DocumentExtendedAttribute>
    {
        public AddEditDocumentExtendedAttributeCommandValidator(IStringLocalizer<AddEditExtendedAttributeCommandValidatorLocalization> localizer) : base(localizer)
        {
            // you can override the validation rules here
        }
    }
}