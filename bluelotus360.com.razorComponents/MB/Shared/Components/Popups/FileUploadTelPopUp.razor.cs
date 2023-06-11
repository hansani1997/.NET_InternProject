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
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using Microsoft.JSInterop;
using System.Net.Http.Headers;

namespace bluelotus360.com.razorComponents.MB.Shared.Components.Popups
{
    public partial class FileUploadTelPopUp
    {
        [Parameter] public FileUpload UploadObject { get; set; }
        [Parameter] public bool WindowIsVisible { get; set; }
        [Parameter] public string PopupTitle { get; set; } = "";
        [Parameter] public EventCallback OnCloseButtonClick { get; set; }
        [Parameter] public string UploadFileType { get; set; } = "";
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
        private string Description { get; set; } = "";
        List<string> AcceptedExtensionTypes => UploadFileType switch
        {
            "Images" => new List<string>() { "jpg", "jpeg", "png" },
            "Document" => new List<string>() { "xlsx", "xls", "docx", "pptx", "pdf", "pptx", "txt" },
            "" => new List<string>() { "jpg", "jpeg", "png", "xlsx", "xls", "docx", "pptx", "pdf", "pptx", "txt" },


        };

        private DialogOptions dialogOptions = new() {  };
        private bool IsUploadedDocumentShown, isDocumentLoading;
        IList<Base64Document> uploadedFiles = new List<Base64Document>(); /*{ new Base64Document() { Filename="sample.pdf",FileSize=10}, new Base64Document() { Filename = "sample1.pdf", FileSize = 10 }, new Base64Document() { Filename = "sample2.pdf", FileSize = 20 } }*/
        private string _selectedBase64string = "", _selectedFileName = "";
        protected override async Task OnInitializedAsync()
        {
            IsUploadedDocumentShown = false;
            isDocumentLoading = false;
            Validator = new UploadValidator(new FileUploadValidation());
        }
        protected override async Task OnParametersSetAsync()
        {
           
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
                    UploadObject.UploadedFile.FileName = file.Name??"";
                    UploadObject.UploadedFile.Extension = string.Join(".", "", file.Name.Split(".").Last() ?? "");
                    UploadObject.Description = this.Description;

                    await _uploadManager.UploadFile(UploadObject);

                }

                _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                _snackBar.Add("File has been  Uploaded Successfully", Severity.Success);
                if (UploaddSuccess.HasDelegate)
                {
                    await UploaddSuccess.InvokeAsync();
                }
                
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
        private void OnFilePreview(Base64Document base64Document)
        {
            _selectedBase64string = base64Document.Base64Source;
            _selectedFileName = base64Document.Filename;
            StateHasChanged();
        }
        public async void ShowUploadedFileOnOff()
        {
            _selectedBase64string = "";
            _selectedFileName = "";
            IsUploadedDocumentShown = !IsUploadedDocumentShown;
            isDocumentLoading = true;
            if (IsUploadedDocumentShown)
            {
                uploadedFiles = await _uploadManager.getBase64Documents(UploadObject);
            }
            isDocumentLoading = false;

            StateHasChanged();
        }
        public async void OnFileDownload(Base64Document base64Document)
        {
            var bytes = Convert.FromBase64String(base64Document.Base64Source);
            var contentDisposition = new ContentDispositionHeaderValue("attachment");
            if (!string.IsNullOrEmpty(base64Document.Filename))
            {
                contentDisposition.FileName = base64Document.Filename;
            }

            var contentType = GetContentType(base64Document.Filename);
            var response = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(bytes)
            };
            response.Content.Headers.ContentType = new MediaTypeHeaderValue(contentType);
            response.Content.Headers.ContentDisposition = contentDisposition;

            await _jsRuntime.InvokeVoidAsync("BlazorDownloadFile", base64Document.Filename, Convert.ToBase64String(bytes), contentType);
        }

        private string GetContentType(string filename)
        {
            var extension = Path.GetExtension(filename);
            switch (extension.ToLower())
            {
                case ".jpg":
                case ".jpeg":
                    return "image/jpeg";
                case ".png":
                    return "image/png";
                case ".xlsx":
                    return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                case ".pdf":
                    return "application/pdf";
                case ".docx":
                    return "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                case ".pptx":
                    return "application/vnd.openxmlformats-officedocument.presentationml.presentation";
                default:
                    return "application/octet-stream";
            }
        }
    }

    public enum SizeUnits
    {
        Byte, KB, MB, GB, TB, PB, EB, ZB, YB
    }


}
