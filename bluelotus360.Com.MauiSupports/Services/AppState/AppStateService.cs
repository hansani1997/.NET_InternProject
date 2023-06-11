using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.Com.MauiSupports.Services.AppState
{
    public class AppStateService
    {
        private bool _isLoaded;
        private bool _isGridLoaded;
        private string _appbarName = "Home";

        public event Action? LoadStateChanged;
        public event Action? GridLoadStateChanged;

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

        public bool IsGridLoaded
        {
            get => _isGridLoaded;
            set
            {
                if (_isGridLoaded != value)
                {
                    _isGridLoaded = value;
                    GridLoadStateChanged?.Invoke();
                }
            }
        }

        public string _AppBarName
        {
            get => _appbarName;
            set
            {
                _appbarName = value;
                LoadStateChanged?.Invoke();
            }
        }
    }
}
