using BL10.CleanArchitecture.Domain.Entities.Document;
using BlueLotus.Com.Domain.Entity;
using bluelotus360.com.razorComponents.Extensions;
using bluelotus360.com.razorComponents.StateManagement;
using BlueLotus360.CleanArchitecture.Domain;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using bluelotus360.Com.commonLib.Managers.ItemProfileMobile;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace bluelotus360.com.razorComponents.MB.Shared.Components.RigidComponents
{
    public partial class ImageBox
    {

        [Parameter] public Item Item { get; set; }=new Item();  
        double ScreenWidth;
        WindowSize ws = new WindowSize();
        BLBreakpoint breakpointMargin = new BLBreakpoint();

        private bool arrows = true;
        private Transition transition = Transition.Slide;

        private FileUpload uploadObject;
        private bool ImagePopupShown = false;
        private Item lineitem;
        private Item item = new();
        

        //private int currentIndex = 0;

        List<string> images = new List<string> { "Item-Profile-1.jpg", "Item-Profile-2.jpg", "Item-Profile-3.jpg" };


        ObjectPosition ImagePosition = ObjectPosition.Center;

        protected override async Task OnInitializedAsync()
        {

            ws = await JS.InvokeAsync<WindowSize>("getWindowSize");

            if (ws.Width < 600)
            {
                breakpointMargin.BreakpointMargin = MudBlazor.Breakpoint.Xs;
            }
            else if (600 <= ws.Width && ws.Width < 960)
            {
                breakpointMargin.BreakpointMargin = MudBlazor.Breakpoint.Sm;
            }
            else if (960 <= ws.Width && ws.Width < 1280)
            {
                breakpointMargin.BreakpointMargin = MudBlazor.Breakpoint.Md;
            }
            else if (1280 <= ws.Width && ws.Width < 1920)
            {
                breakpointMargin.BreakpointMargin = MudBlazor.Breakpoint.Lg;
            }
            else if (1920 <= ws.Width && ws.Width < 2560)
            {
                breakpointMargin.BreakpointMargin = MudBlazor.Breakpoint.Xl;
            }

        }


        void SetImagePosition(ObjectPosition value)
        {
            ImagePosition = value;
        }

        //IList<IBrowserFile> files = new List<IBrowserFile>();

        //private void UploadFiles(IBrowserFile file)
        //{
        //    files.Add(file);
        //    //TODO upload the files to the server
        //}


        #region Image Upload

        IList<Item> files = new List<Item>();

        private async void ShowImageUploadPopUp( )
        {

            uploadObject = new FileUpload();
            uploadObject.ItemKey = (int)Item.ItemKey;

            ImagePopupShown = true;
            StateHasChanged();

        }

        private async void HideAllPopups()
        {
            ImagePopupShown = false;
            StateHasChanged();
        }

        private async void UploadSuccess()
        {
            ItemOpenRequest request = new ItemOpenRequest();
            request.ItemKey = Item.ItemKey;


            DocumentRetrivaltDTO document = new DocumentRetrivaltDTO();
            document.ItemKey = (int)Item.ItemKey;
            Item.Base64Documents = await _uploadManager.getBase64DocumentsV2(document);

            //await LoadItem(request);
            StateHasChanged();
        }

        #endregion


        //private void NextImage()
        //{
        //    currentIndex = (currentIndex + 1) % items.Count;
        //}

        //private void PreviousImage()
        //{
        //    currentIndex = currentIndex == 0 ? items.Count - 1 : currentIndex - 1;
        //}
    }
}
