using BlueLotus360.CleanArchitecture.Domain.Entities;
using System.Threading.Tasks;

namespace BlueLotus360.Com.UI.Definitions.Extensions
{
    public interface IBLUIOperationHelper
    {
        void ResetToInitialValue();

        void UpdateVisibility(bool IsVisible);

        void ToggleEditable(bool IsEditable);

        Task Refresh();

        Task SetValue(object value);
        Task FocusComponentAsync();

        BLUIElement LinkedUIObject { get; }




    }


    public interface IBLServerDependentComponent
    {
        Task FetchData(bool useLocalstorage = false);

        Task SetDataSource(object DataSource);
       
    }


}
