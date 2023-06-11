using BlueLotus360.Com.Shared.Settings;
using System.Threading.Tasks;
using BlueLotus360.Com.Shared.Wrapper;

namespace BlueLotus360.Com.Shared.Managers
{
    public interface IPreferenceManager
    {
        Task SetPreference(IPreference preference);

        Task<IPreference> GetPreference();

        Task<IResult> ChangeLanguageAsync(string languageCode);
    }
}