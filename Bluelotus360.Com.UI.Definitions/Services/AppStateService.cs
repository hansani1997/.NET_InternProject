using BlueLotus360.Com.UI.Definitions.MB.Shared;
using System;

namespace BlueLotus360.Com.UI.Definitions.Services
{
    public class AppStateService
    {
        private bool _isLoaded;
        private string _appbarName= "Home";

        public event Action? LoadStateChanged;

        public bool IsLoaded
        {
            get => _isLoaded;
            set
            {
                if (_isLoaded != value)
                {
                    _isLoaded = value;
                    LoadStateChanged?.Invoke();
                }
            }
        }

        public  string _AppBarName 
        { 
            get => _appbarName;
            set {
                _appbarName = value;
                LoadStateChanged?.Invoke();
            }
        } 

        
    }
}
