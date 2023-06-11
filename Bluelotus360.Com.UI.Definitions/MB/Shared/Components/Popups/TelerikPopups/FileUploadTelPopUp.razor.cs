using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL10.CleanArchitecture.Domain.Entities.Document;
using BL10.CleanArchitecture.Application.Validators.FileUpload;

namespace BlueLotus360.Com.UI.Definitions.MB.Shared.Components.Popups.TelerikPopups
{
    public partial class FileUploadTelPopUp
    {
        [Parameter] public FileUpload UploadObject { get; set; }
        [Parameter] public bool WindowIsVisible { get; set; }
        [Parameter] public string PopupTitle { get; set; }
        [Parameter] public EventCallback OnCloseButtonClick { get; set; }
        [Parameter] public string UploadFileType { get; set; } = "Images";
        [Parameter] public EventCallback UploaddSuccess { get; set; }

        bool HideMinMax { get; set; } = false;

        private bool Clearing = false;
        private static string DefaultDragClass = "relative rounded-lg border-2 border-dashed pa-4 mt-4 mud-width-full mud-height-full";
        private string DragClass = DefaultDragClass;
        private IList<FileType> files = new List<FileType>();
        private int MaxNumberofFiles = 1;
        long maxFileSize = 1024 * 10;

        private IUploadValidator Validator;
        private FileUploadValidation _uploadValidate;
        private bool shouldRender;
        private IReadOnlyList<IBrowserFile> entries;
        private bool IsUploading { get; set; }
        private string Description { get; set; }
        List<string> AcceptedExtensionTypes => UploadFileType switch
        {
            "Images" => new List<string>() { "jpg", "jpeg", "png" },
            "" => new List<string>() { "jpg", "jpeg", "png" },

        };



        protected override async Task OnParametersSetAsync()
        {
            Validator = new UploadValidator(new FileUploadValidation());
            await base.OnParametersSetAsync();
        }

        private async void OnCloseClick()
        {
            if (OnCloseButtonClick.HasDelegate)
            {
                await Clear();
                await OnCloseButtonClick.InvokeAsync();
            }
        }

        private void OnInputFileChanged(InputFileChangeEventArgs e)
        {
            if (Validator != null) { Validator.UserMessages.UserMessages.Clear(); }

            ClearDragClass();
            entries = e.GetMultipleFiles();

            _uploadValidate = new FileUploadValidation()
            {
                MaxAllowedFiles = 1,
                MaxFileSize = 10,
                NumberofFilesUploaded = files.Count(),
                AcceptedFileExtensions = AcceptedExtensionTypes,
                SelectedFile = new FileType() { Size = this.ToSize(e.File.Size, SizeUnits.MB), FileName = e.File.Name, Extension = e.File.Name.Split(".").Last() }

            };
            Validator = new UploadValidator(_uploadValidate);

            if (Validator.CanFileUpload())
            {
                foreach (var file in entries)
                {
                    files.Add(new FileType() { FileName = file.Name });
                }
            }


        }

        private async Task Clear()
        {
            Clearing = true;
            files.Clear();
            if (Validator != null) { Validator.UserMessages.UserMessages.Clear(); }
            ClearDragClass();
            await Task.Delay(100);
            Clearing = false;
        }
        private async void Upload()
        {
            //Upload the files here
            IsUploading = true;
            try
            {
                foreach (var file in entries)
                {


                    await using MemoryStream fs = new MemoryStream();
                    await file.OpenReadStream(maxAllowedSize: 1048576).CopyToAsync(fs);
                    byte[] somBytes = GetBytes(fs);
                    string base64String = Convert.ToBase64String(somBytes, 0, somBytes.Length);
                    UploadObject.Buffer = somBytes;
                    UploadObject.UploadedFile.Size = file.Size;
                    UploadObject.UploadedFile.FileName = file.Name;
                    UploadObject.UploadedFile.Extension = file.Name.Split(".").Last();
                    UploadObject.Description = this.Description;

                    await _uploadManager.UploadFile(UploadObject);

                }

                _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                _snackBar.Add("File has been  Uploaded Successfully", Severity.Success);
                await UploaddSuccess.InvokeAsync();
            }
            catch (Exception ex)
            {
                _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                _snackBar.Add("Error occured", Severity.Error);
            }
            finally
            {
                IsUploading = false;
                await Clear();
                await OnCloseButtonClick.InvokeAsync();
            }

        }

        public static byte[] GetBytes(Stream stream)
        {
            var bytes = new byte[stream.Length];
            stream.Seek(0, SeekOrigin.Begin);
            stream.ReadAsync(bytes, 0, bytes.Length);
            stream.Dispose();
            return bytes;
        }
        private void SetDragClass()
        {
            DragClass = $"{DefaultDragClass} mud-border-primary";
        }

        private void ClearDragClass()
        {
            DragClass = DefaultDragClass;
        }

        public long ToSize(Int64 value, SizeUnits unit)
        {
            return Convert.ToInt64(value / (double)Math.Pow(1024, (Int64)unit));
        }


    }

    public enum SizeUnits
    {
        Byte, KB, MB, GB, TB, PB, EB, ZB, YB
    }


}
