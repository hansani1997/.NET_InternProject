using BL10.CleanArchitecture.Domain.Entities.ProjectManagement;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using bluelotus360.com.razorComponents.Extensions;
using bluelotus360.com.razorComponents.MB.Shared.Components;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bluelotus360.Com.commonLib.Helpers;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Components.Forms;
using BlueLotus360.CleanArchitecture.Domain;
using System.Diagnostics;
using Process = BL10.CleanArchitecture.Domain.Entities.ProjectManagement.Process;
using BL10.CleanArchitecture.Domain.Entities.Document;
using System.ComponentModel;
using Telerik.Blazor.Components.Editor;
using static BlueLotus360.Com.Infrastructure.OrderPlatforms.Ubereats.UberEntities.MenuFromUber;
using BlueLotus360.CleanArchitecture.Domain.Entities.HR;

namespace bluelotus360.com.razorComponents.Pages.ProjectManagement
{
    public partial class KanbanNewTask
    {
        #region parameter

        private BLUIElement formDefinition;
        private BLUIElement TaskDetailsView;
        private BLUIElement TaskDetails;
        private BLUIElement AttachmentDetails;
        private BLUIElement Teams;
        private BLUIElement Comment;
        private BLUIElement SubTasks;
        private BLUIElement KanbanButtonSection;
        private BLUIElement _CheckList;
        private BLUIElement CheckListBtnSec;
        private BLUIElement CheckListDataEntry;
        private IDictionary<string, EventCallback> _interactionLogic;
        private IDictionary<string, IBLUIOperationHelper> _objectHelpers;
        private UIBuilder _refBuilder;
        private ProcessRequest _processRequest;
        private IList<BLUIElement> ListOfTabs;
        private static string DefaultDragClass = "relative rounded-lg border-2 border-dashed pa-4 mt-4 mud-width-full mud-height-full z-10";
        private string DragClass = DefaultDragClass;
        private UserDetails _user;
        private List<string> fileNames = new List<string>();
        [Parameter]
        public string? NavigateElementKey { get; set; }
        [Parameter]
        public string? TaskIdentity { get; set; }
        [Parameter]
        public string? SubTaskIdentity { get; set; }
        [Parameter]
        public Process EditTask { get; set; }
        public Process NewTask { get; set; }
        public int TaskKey { get; set; } = 1;
        public IDictionary<string, EventCallback> InteractionLogics { get; set; }
        public IDictionary<string, IBLUIOperationHelper> ObjectHelpers { get; set; }
        private IReadOnlyList<IBrowserFile> files;
        public FileUpload UploadObject { get; set; }
        private ProcessComponent _resource;
        private ProcessRemark _Comment = new ProcessRemark();
        private bool isChkLstDataEntryView= false;
        private CheckList _chkLst;
        private bool isOpenSubTaskOption = false;
        public int SubTaskKey { get; set; } = 1;
        #endregion

        #region General


        protected override async Task OnInitializedAsync()
        {


            InitializeObjects();

            long elementKey = Convert.ToInt64(NavigateElementKey);
           // _navigationManager.TryGetQueryString<long>("ElementKey", out elementKey);// get element key from url 

            if (elementKey > 10)
            {
                var formrequest = new ObjectFormRequest();

                formrequest.MenuKey = elementKey;
                formDefinition = await _navManger.GetMenuUIElement(formrequest);//get ui elements
            }
            TaskKey = Convert.ToInt32(TaskIdentity);
            var childsHash = formDefinition.Children.ToLookup(elem => elem.ParentKey);
            foreach (var child in formDefinition.Children)
            {
                child.Children = childsHash[child.ElementKey].ToList();
            }
            if (formDefinition != null)
            {
                TaskDetailsView = formDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("TaskTabs")).FirstOrDefault();
                ListOfTabs=TaskKey > 11 ? TaskDetailsView.Children.Where(x => x.ElementType == "Tab").ToList() : TaskDetailsView.Children.Where(x=>x.ElementType=="Tab" && !x.IsFreeze).ToList();
                TaskDetails = ListOfTabs.Where(x => x.ElementID == "TaskDetails").FirstOrDefault();
                AttachmentDetails=ListOfTabs.Where(x => x.ElementID == "Attachments").FirstOrDefault();
                Teams= ListOfTabs.Where(x => x.ElementID == "Teams").FirstOrDefault();
                Comment= ListOfTabs.Where(x => x.ElementID == "Comments").FirstOrDefault();
                SubTasks = ListOfTabs.Where(x => x.ElementID == "SubTask").FirstOrDefault();
                KanbanButtonSection= formDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("TabButtonSec")).FirstOrDefault();
                _CheckList= ListOfTabs.Where(x => x.ElementID == "CheckList").FirstOrDefault();
                if(_CheckList != null)
                {
                    CheckListBtnSec = _CheckList.Children.Where(x => x.ElementID == "ChkLstBtnSec").FirstOrDefault();
                    CheckListDataEntry = _CheckList.Children.Where(x => x.ElementID == "ChkLstDataEntry").FirstOrDefault();
                }
              
                formDefinition.IsDebugMode = true;
            }
            
            await GetExistingTask();

            HookInteractions();



        }

        public async void InitializeObjects()
        {
            _interactionLogic = new Dictionary<string, EventCallback>();
            _objectHelpers = new Dictionary<string, IBLUIOperationHelper>();
            _refBuilder = new UIBuilder();
            _processRequest = new ProcessRequest();
            TaskDetailsView = new BLUIElement();
            ListOfTabs = new List<BLUIElement>();
            TaskDetails = new BLUIElement();
            AttachmentDetails = new BLUIElement();
            EditTask = new Process();
            UploadObject = new FileUpload();
            Teams = new BLUIElement();
            SubTasks = new BLUIElement();
            _user = new UserDetails();
            _resource = new ProcessComponent();
            _CheckList = new BLUIElement();
            CheckListBtnSec= new BLUIElement();
            CheckListDataEntry= new BLUIElement();
            _chkLst = new CheckList();    
        }

        private void UIStateChanged()
        {

            this.StateHasChanged();
        }

        private void HookInteractions()
        {
            InteractionHelper helper = new InteractionHelper(this, formDefinition);//formdefinition has all form objects 
            _interactionLogic = helper.GenerateEventCallbacks();// generate all event callbacks 
            //AppSettings.RefreshTopBar("Geo Attendence");
            appStateService._AppBarName = TaskKey > 11 ? EditTask.TaskId : "Task Creation";
        }
        #endregion

        private void OnInputFileChanged(InputFileChangeEventArgs e)
        {
            ClearDragClass();
            files = e.GetMultipleFiles();
            foreach (var file in files)
            {
                fileNames.Add(file.Name);
            }
        }

        private void SetDragClass()
        {
            DragClass = $"{DefaultDragClass} mud-border-primary";
        }

        private void ClearDragClass()
        {
            DragClass = DefaultDragClass;
        }

        //private async void OnTaskNameChange(UIInterectionArgs<string?> args)
        //{
        //    EditTask.TaskName = args.DataObject;
        //    StateHasChanged();
        //}

        //private async void Det_LeadOnChange(UIInterectionArgs<AddressResponse> args)
        //{
        //    //EditTask.Lead = args.DataObject;
        //    //StateHasChanged();

        //    await Task.CompletedTask;
        //    StateHasChanged();
        //}
        private async void Det_PriorityOnChange(UIInterectionArgs<CodeBaseResponse> args)
        {
            EditTask.Priority = args.DataObject;
            StateHasChanged();
        }
        
        private async void ExpansionPanelClick(BLUIElement item)
        {
            if (item.ElementID == "TaskDetails")
            {
                await GetExistingTask();
            }
            UIStateChanged();
        }

        private async Task<Process> GetExistingTask()
        {
            if (TaskKey > 11)
            {
                ProcessRequest processrequest = new ProcessRequest()
                {
                    TaskKey = TaskKey
                };
                EditTask = await _processManager.GetTaskByTaskKey(processrequest);
                await SetValue("TaskName", EditTask.TaskName);
                DocumentRetrivaltDTO document = new DocumentRetrivaltDTO();
                document.ProcessDetKey = (int)EditTask.TaskKey;
                EditTask.Base64Documents = await _uploadManager.GetAllDocuments(document);
                EditTask.Resources = await GetTeamMembers();
                EditTask.Comments = await GetComments();
                EditTask.SubProcess = await GetSubTasks();
                EditTask.CheckList= await GetCheckList();
            }

            return EditTask;
        }

        private async Task ClearFiles()
        {
            fileNames.Clear();
            ClearDragClass();
            await Task.Delay(100);
        }

        private async Task UploadFiles(int TaskKey)
        {
            try
            {
                foreach (var file in files)
                {
                    await using MemoryStream fs = new MemoryStream();
                    await file.OpenReadStream(maxAllowedSize: 1048576).CopyToAsync(fs);
                    byte[] somBytes = GetBytes(fs);
                    string base64String = Convert.ToBase64String(somBytes, 0, somBytes.Length);
                    UploadObject.Buffer = somBytes;
                    UploadObject.UploadedFile.Size = file.Size;
                    UploadObject.UploadedFile.FileName = file.Name ?? "";
                    UploadObject.UploadedFile.Extension = file.Name.Split(".").Last() ?? "";
                    UploadObject.ProcessDetKey = TaskKey;
                    await _uploadManager.UploadFile(UploadObject);

                }
                _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                _snackBar.Add("File has been  Uploaded Successfully", Severity.Success);

            }
            catch (Exception ex)
            {
                _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                _snackBar.Add("Error Occured", Severity.Error);
            }
            finally
            {
                await ClearFiles();
                DocumentRetrivaltDTO document = new DocumentRetrivaltDTO();
                document.ProcessDetKey = TaskKey;
                EditTask.Base64Documents = await _uploadManager.GetAllDocuments(document);
            }
            UIStateChanged();
        }

        public static byte[] GetBytes(Stream stream)
        {
            var bytes = new byte[stream.Length];
            stream.Seek(0, SeekOrigin.Begin);
            stream.ReadAsync(bytes, 0, bytes.Length);
            stream.Dispose();
            return bytes;
        }

        private async void DownloadFile(Base64Document item)
        {
           
            UIStateChanged();
        }

        private async void DeleteFile(Base64Document item)
        {
            DocumentRetrivaltDTO doc = new DocumentRetrivaltDTO()
            {
                DocumentKey=item.DocumentKey
            };
            bool success= await _uploadManager.DeleteDocument(doc);
            if(success) 
            {
                _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                _snackBar.Add("File has been  Deleted Successfully", Severity.Success);
            }
            else
            {
                _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                _snackBar.Add("Something went wrong", Severity.Error);
            }
            DocumentRetrivaltDTO document = new DocumentRetrivaltDTO();
            document.ProcessDetKey = TaskKey;
            EditTask.Base64Documents = await _uploadManager.GetAllDocuments(document);
            UIStateChanged();
        }
        private async void OnTeamMembersChange(UIInterectionArgs<AddressResponse> args)
        {
            _resource.ComponentAddress = args.DataObject;
            StateHasChanged();
        }
        private async void OnNoOfHoursChange(UIInterectionArgs<decimal> args)
        {
            _resource.TransactionQuantity = args.DataObject;
            StateHasChanged();
        }

        private async void OnTeam_DesChange(UIInterectionArgs<string?> args)
        {
            _resource.Description = args.DataObject;
            StateHasChanged();
        }

        

        private async void AddTeamMembers()
        {
            if(TaskKey > 11)
            {
                _resource.PrcessKey = TaskKey;
                await _processManager.CreateProcessComponent(_resource);
                EditTask.Resources = await GetTeamMembers();
                _resource = new ProcessComponent();
                UIStateChanged();
            }
            else
            {
                if (_resource.ComponentAddress.AddressKey > 11 && _resource.TransactionQuantity > 0)
                {
                    _resource.ProcessComponentKey = 1;
                    EditTask.Resources.Add(_resource);
                    _resource = new ProcessComponent();
                    UIStateChanged();
                }
                else
                {
                    _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                    _snackBar.Add("Please select Team member / Hours", Severity.Warning);
                }
            }
           
            UIStateChanged();
        }

        private async void DeleteTeamMembers(ProcessComponent cmpnt)
        {
            if(cmpnt.ProcessComponentKey > 11)
            {
                ProcessRequest req = new ProcessRequest()
                {
                    TaskKey=TaskKey,
                    PrcsCmpKy= cmpnt.ProcessComponentKey
                };
               bool success= await _processManager.DeleteComponents(req);
                EditTask.Resources = await GetTeamMembers();
                UIStateChanged();
            }
            else
            {
                EditTask.Resources.Remove(cmpnt);
                UIStateChanged();
            }
            
        }

        private async Task<IList<Process>> GetSubTasks()
        {
            ProcessRequest request = new ProcessRequest()
            {
                TaskKey = TaskKey
            };
            IList<Process> SubTask = await _processManager.GetSubTaskByTaskKey(request);
            return SubTask;
        }

        private async Task<IList<ProcessComponent>> GetTeamMembers()
        {
            ProcessRequest request = new ProcessRequest()
            {
                TaskKey= TaskKey
            };
            IList<ProcessComponent> Teams = await _processManager.GetProcessComponents(request);
            return Teams;
        }

        private async Task<IList<ProcessRemark>> GetComments()
        {
            ProcessRequest request = new ProcessRequest()
            {
                TaskKey = TaskKey
            };
            IList<ProcessRemark> Teams = await _processManager.GetProcessRemarksByProcess(request);
            return Teams;
        }

        private async void OnCommentChange(UIInterectionArgs<string?> args)
        {
            _Comment.Remarks = args.DataObject;
            StateHasChanged();
        }

        private async void OnPostClick(UIInterectionArgs<object> args)
        {
            if(_Comment.Remarks != "")
            {
                _Comment.TaskKey= TaskKey;
                await _processManager.SaveRemarks(_Comment);
                EditTask.Comments=await GetComments();
                _Comment = new ProcessRemark();
                _Comment.Remarks = "";
                
            }
            else
            {
                _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                _snackBar.Add("Please leave a comment", Severity.Warning);
            }
            StateHasChanged();
        }

        private async void OnBackClick(UIInterectionArgs<object> args)
        {
            _navigationManager.NavigateTo($"/Process/todomobileview?ElementKey=" + Convert.ToInt64(NavigateElementKey));
        }

        private async void OnSaveClick(UIInterectionArgs<object> args)
        {
            if (SaveRecordValidation(EditTask))
            {
                _user= await _hrManager.GetUserAsync();
                if (EditTask.TaskKey > 11)
                {
                    TaskInsertUpdate task = new TaskInsertUpdate()
                    {
                        PrcsDetKy = EditTask.TaskKey,
                        TaskId = EditTask.TaskId,
                        TaskName = EditTask.TaskName,
                        PrcsTypKy = EditTask.ProcessType.CodeKey,
                        TrnQty = EditTask.TransactionQauntity,
                        TrnPri = EditTask.CostPrice,
                        TrnRate = EditTask.TransactionRate,
                        TrnUnitKy = EditTask.TransactionUnit.UnitKey,
                        LeadAdrKy = EditTask.Lead.AddressKey < 11 ? _user.AdrKy : EditTask.Lead.AddressKey,
                        CurAdrKy = EditTask.CurrentResponsible.AddressKey < 11 ? _user.AdrKy : EditTask.CurrentResponsible.AddressKey,
                        PrjKy = EditTask.ProcessProject.ProjectKey,
                        PrtyKy = EditTask.Priority.CodeKey,
                        LiNo = EditTask.LineNumber,
                        ObjKy = 1,
                        AprStsKy = EditTask.ApproveStatus.CodeKey,
                        ReqDt = EditTask.RequestDate.ToString("yyyy/MM/dd"),
                        PrntKy = EditTask.ParentTaskKey,
                        PrcsDetCat1Ky = EditTask.PorcessCategory.CodeKey,
                        PrjPrcsCat1Ky = EditTask.PorcessCategory.CodeKey,
                        InitalPrgs = EditTask.ProgressPercentage,
                        ScheduleProgress = EditTask.ScheduleDetail.Progress,
                        VersionNo = EditTask.ScheduleDetail.Schedule.VersionNumber == null ? "" : EditTask.ScheduleDetail.Schedule.VersionNumber,
                        YurRef = EditTask.ScheduleDetail.Schedule.YourReference == null ? "" : EditTask.ScheduleDetail.Schedule.YourReference,
                        FromDt = EditTask.ScheduleDetail.StartDate.ToString("yyyy/MM/dd"),
                        ToDt = EditTask.ScheduleDetail.EndDate.ToString("yyyy/MM/dd"),
                        WrkHrs = EditTask.ScheduleDetail.WorkHours,
                        PrcsObjKy = EditTask.PrcsObjKy < 11 ? 1 : EditTask.PrcsObjKy,
                        Des = EditTask.Description,
                        Rate = EditTask.CostPrice,
                        Weight = EditTask.Weight,
                        IsCompleted = EditTask.IsCompleted,
                        IsSprint = EditTask.IsSprint,
                        IsProgram = EditTask.IsProgram,
                        IsCorporate = EditTask.IsCorporate,
                        IsGeneric = EditTask.IsGeneric,
                        IsClients = EditTask.IsClients,
                        IsDeveloper = EditTask.IsDeveloper,
                        IsConsultant = EditTask.IsConsultant,
                        IsStandUp = EditTask.IsStandUp,
                        RecurrenceRule = EditTask.RecurrenceRule,
                        AdrKy = EditTask.AddressKey < 11 ? 1 : EditTask.AddressKey,
                        Remarks = EditTask.Remarks == null || EditTask.Remarks == "" ? "" : EditTask.Remarks,
                        No1 = EditTask.No1,
                        No2 = EditTask.No2,
                        TaskSNm = EditTask.TaskSNm == null ? "" : EditTask.TaskSNm,
                        PrcsDetCat2Ky = EditTask.PrcsDetCat2Ky,
                        Anl2Ky = EditTask.Anl2Ky

                    };


                    bool success = await _processManager.UpdateTask(task);
                    if (success)
                    {
                        _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                        _snackBar.Add("Task has been updated successfully", Severity.Success);
                        //_navigationManager.ToBaseRelativePath();
                        _navigationManager.NavigateTo($"/Process/todomobileview?ElementKey=" + Convert.ToInt64(NavigateElementKey));
                    }
                    else
                    {
                        _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                        _snackBar.Add("Something went wrong", Severity.Error);
                    }
                }
                else
                {
                    TaskInsertUpdate Newtask = new TaskInsertUpdate();
                    ProcessRequest req = new ProcessRequest();
                    req.ProjectKey = EditTask.ProcessProject.ProjectKey;
                    Newtask.PrcsDetKy = 1;
                    Newtask.TaskId = await _processManager.GetNextTaskID(req);
                    Newtask.TaskName = EditTask.TaskName;
                    Newtask.PrcsTypKy = EditTask.ProcessType.CodeKey;
                    Newtask.TrnQty = EditTask.TransactionQauntity;
                    Newtask.TrnPri = EditTask.CostPrice;
                    Newtask.TrnRate = EditTask.TransactionRate;
                    Newtask.TrnUnitKy = EditTask.TransactionUnit.UnitKey;
                    Newtask.LeadAdrKy = EditTask.Lead.AddressKey < 11 ? _user.AdrKy : EditTask.Lead.AddressKey;
                    Newtask.CurAdrKy = EditTask.CurrentResponsible.AddressKey < 11 ? _user.AdrKy : EditTask.CurrentResponsible.AddressKey;
                    Newtask.PrjKy = EditTask.ProcessProject.ProjectKey;
                    Newtask.PrtyKy = EditTask.Priority.CodeKey;
                    Newtask.LiNo = EditTask.LineNumber;
                    Newtask.ObjKy = Convert.ToInt32(NavigateElementKey);
                    Newtask.AprStsKy = EditTask.ApproveStatus.CodeKey;
                    Newtask.ReqDt = EditTask.RequestDate.ToString("yyyy/MM/dd");
                    Newtask.PrntKy = EditTask.ParentTaskKey;
                    Newtask.PrcsDetCat1Ky = EditTask.PorcessCategory.CodeKey;
                    Newtask.PrjPrcsCat1Ky = EditTask.PorcessCategory.CodeKey;
                    Newtask.InitalPrgs = EditTask.ProgressPercentage;
                    Newtask.ScheduleProgress = EditTask.ScheduleDetail.Progress;
                    Newtask.VersionNo = EditTask.ScheduleDetail.Schedule.VersionNumber == null ? "" : EditTask.ScheduleDetail.Schedule.VersionNumber;
                    Newtask.YurRef = EditTask.ScheduleDetail.Schedule.YourReference == null ? "" : EditTask.ScheduleDetail.Schedule.YourReference;
                    Newtask.FromDt = EditTask.ScheduleDetail.StartDate.ToString("yyyy/MM/dd");
                    Newtask.ToDt = EditTask.ScheduleDetail.EndDate.ToString("yyyy/MM/dd");
                    Newtask.WrkHrs = EditTask.ScheduleDetail.WorkHours;
                    Newtask.PrcsObjKy = EditTask.PrcsObjKy < 11 ? 1 : EditTask.PrcsObjKy;
                    Newtask.Des = EditTask.Description;
                    Newtask.Rate = EditTask.CostPrice;
                    Newtask.Weight = EditTask.Weight;
                    Newtask.IsCompleted = EditTask.IsCompleted;
                    Newtask.IsSprint = EditTask.IsSprint;
                    Newtask.IsProgram = EditTask.IsProgram;
                    Newtask.IsCorporate = EditTask.IsCorporate;
                    Newtask.IsGeneric = EditTask.IsGeneric;
                    Newtask.IsClients = EditTask.IsClients;
                    Newtask.IsDeveloper = EditTask.IsDeveloper;
                    Newtask.IsConsultant = EditTask.IsConsultant;
                    Newtask.IsStandUp = EditTask.IsStandUp;
                    Newtask.RecurrenceRule = EditTask.RecurrenceRule;
                    Newtask.AdrKy = EditTask.AddressKey < 11 ? 1 : EditTask.AddressKey;
                    Newtask.Remarks = EditTask.Remarks == null || EditTask.Remarks == "" ? "" : EditTask.Remarks;
                    Newtask.No1 = EditTask.No1;
                    Newtask.No2 = EditTask.No2;
                    Newtask.TaskSNm = EditTask.TaskSNm == null ? "" : EditTask.TaskSNm;
                    Newtask.PrcsDetCat2Ky = EditTask.PrcsDetCat2Ky;
                    Newtask.Anl2Ky = EditTask.Anl2Ky;


                    bool succes = await _processManager.CreateTask(Newtask);
                    if (succes)
                    {
                        _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                        _snackBar.Add("Task has been created successfully", Severity.Success);
                        //_navigationManager.ToBaseRelativePath();
                        _navigationManager.NavigateTo($"/Process/todomobileview?ElementKey=" + Convert.ToInt64(NavigateElementKey));
                    }
                    else
                    {
                        _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                        _snackBar.Add("Something went wrong", Severity.Error);
                    }
                }
            }
            EditTask = new Process();
            StateHasChanged();
        }

        private async Task SetValue(string name, object value)
        {
            IBLUIOperationHelper helper;

            if (_objectHelpers.TryGetValue(name, out helper))
            {
                await helper.SetValue(value);
                UIStateChanged();
                await Task.CompletedTask;
            }
        }

        private void OnSubTaskViewOptionOpen(long TaskKey)
        {
            //Hidepopup();
            //isOpenViewOption = true;
            //TaskIdentity = TaskKey;
            //Count = SubTaskCount;
            UIStateChanged();


        }

        private void OnSubTaskAddClick(long TaskKey)
        {
            
            UIStateChanged();


        }

        private void OnChildSubTaskAddClick(long TaskKey)
        {
            //TaskIdentity = "1";
            //TaskIdentity = TaskKey.ToString();
            //_navigationManager.NavigateTo($"/Process/SubTask/{NavigateElementKey}/{TaskIdentity}");
            SubTaskKey = 1;
            isOpenSubTaskOption = true;
            UIStateChanged();



        }

        private void OnSubTaskEditClick(long TaskKey)
        {
            //Hidepopup();
            //isOpenViewOption = true;
            //TaskIdentity = TaskKey;
            //Count = SubTaskCount;
            UIStateChanged();


        }

        private bool SaveRecordValidation(Process Task)
        {
            if (Task.TaskName == "")
            {
                _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                _snackBar.Add("Please Enter Task", Severity.Warning);
                return false;
            }
            if (Task.ProcessProject.ProjectKey < 11)
            {
                _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                _snackBar.Add("Please select project", Severity.Warning);
                return false;
            }
            else if(Task.ApproveStatus.CodeKey < 11) 
            {
                _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                _snackBar.Add("Please select Status", Severity.Warning);
                return false;
            }
           
            else
            {
                return true;
            }
        }

        private async Task<IList<CheckList>> GetCheckList()
        {
            ProcessRequest request = new ProcessRequest()
            {
                TaskKey = TaskKey
            };
            IList<CheckList> Teams = await _processManager.ToDoChkLst(request);
            return Teams;
        }
        private async void OpenCheckListPopup(UIInterectionArgs<object> args)
        {
            isChkLstDataEntryView = true;
            StateHasChanged();
        }
        private async void AddCheckList(UIInterectionArgs<object> args)
        {

            if (_chkLst.Content != "")
            {
                _chkLst.TaskKey = TaskKey;
                _chkLst.isActive= true;
                _chkLst.Date = DateTime.Now.ToString("yyyy/MM/dd");
                await _processManager.CreateUpdateCheckList(_chkLst);
                EditTask.CheckList = await GetCheckList();
                _chkLst = new CheckList();

            }
            else
            {
                _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                _snackBar.Add("Please enter content", Severity.Warning);
            }
            StateHasChanged();
        }

        private async void CancelChkLst(UIInterectionArgs<object> args)
        {
            isChkLstDataEntryView = false;
            _chkLst = new CheckList();
            StateHasChanged();
        }

        private async void OnCheckListClick(CheckList checkList)
        {
            checkList.isChecked = false;
            _processManager.CreateUpdateCheckList(checkList);
            EditTask.CheckList = await GetCheckList();
            UIStateChanged();
        }

        private async void OnUnCheckListClick(CheckList checkList)
        {
            checkList.isChecked = true;
            _processManager.CreateUpdateCheckList(checkList);
            EditTask.CheckList = await GetCheckList();
            UIStateChanged();
        }

        private async void CheckListDelete(CheckList checkList)
        {
            _processManager.CreateUpdateCheckList(checkList);
            EditTask.CheckList = await GetCheckList();
            UIStateChanged();
        }

        private void Hidepopup()
        {
           isOpenSubTaskOption= false;
            UIStateChanged();
        }

    }
}
