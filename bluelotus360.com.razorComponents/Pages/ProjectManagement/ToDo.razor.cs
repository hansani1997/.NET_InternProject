using BL10.CleanArchitecture.Domain.Entities;
using BlueLotus360.CleanArchitecture.Application.Validators.MessageSystem;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using bluelotus360.com.razorComponents.Extensions;
using bluelotus360.com.razorComponents.MB.Shared.Components;
using bluelotus360.com.razorComponents.Pages.Orderhub.Components;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bluelotus360.Com.commonLib.Helpers;
using static MudBlazor.CategoryTypes;
using System.Reflection.Metadata;
using System.Threading;
using BL10.CleanArchitecture.Domain.Entities.ProjectManagement;
using Telerik.Blazor.Components.Stepper.Interfaces;
using BlueLotus360.CleanArchitecture.Domain.Entities.HR;
using System.Timers;

namespace bluelotus360.com.razorComponents.Pages.ProjectManagement
{
    public partial class ToDo
    {
        #region parameter

        private BLUIElement formDefinition;
        private BLUIElement _headerSection;
        private BLUIElement _headerSectionExtended;
        private BLUIElement _headerSection2;
        private BLUIElement TaskDetailsView;
        private BLUIElement ToDoButtonSec;
        private IDictionary<string, EventCallback> _interactionLogic;
        private IDictionary<string, IBLUIOperationHelper> _objectHelpers;
        private UIBuilder _refBuilder;
        private IList<CodeBaseResponseExtended> ListOfBoards;
        private IList<CodeBaseResponseExtended> ListOfBoardsWithCount;
        private IList<Process> ListOfTasks;
        private bool isExpandToggle = false;
        private ProcessRequest _processRequest;
        private MudDropContainer<Process> _dropContainer;
        private List<Process> _tasks = new();
        private bool isSwitchOn = false;
        private bool isExpandable = false;
        private bool isOpenNewTask = false;
        private long NavigateElementKey=0;
        private long TaskIdentity = 0;
        private bool isOpenViewOption = false;
        private bool isExpandSubTask = false;
        private IList<Process> ListOfSubTasks;
        private int Count = 0;
        private long SubTaskIdentity = 0;
        private bool isKanbanview = false;
        private bool Listview = false;
        private BLUIElement Tableview;
        private BLTable<Process> _blTb;
        private UserDetails _user;
        private IList<Process> ListOfTableTask;
        //private bool isPlayClicked = false;
        //private bool isStopClicked = false;
        const string DefaultTime = "00:00:00";
        //private string elapsedTime= DefaultTime;
        //System.Timers.Timer timer = new System.Timers.Timer(1000);
        //DateTime TimerStartTime = DateTime.Now;
        #endregion

        #region General
        protected override async Task OnInitializedAsync()
        {


            InitializeObjects();

            long elementKey = 1;
            string url= _navigationManager.Uri.ToString();
            _navigationManager.TryGetQueryString<long>("ElementKey", out elementKey);// get element key from url 

            if (elementKey > 10)
            {
                var formrequest = new ObjectFormRequest();
                NavigateElementKey=elementKey;
                formrequest.MenuKey = elementKey;
                formDefinition = await _navManger.GetMenuUIElement(formrequest);//get ui elements
            }
            //var childsHash = formDefinition.Children.ToLookup(elem => elem.ParentKey);
            //foreach (var child in formDefinition.Children)
            //{
            //    child.Children = childsHash[child.ElementKey].ToList();
            //}

            if (formDefinition != null)
            {
                HookInteractions();
                BreakComponent();
                _headerSection = formDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("HeaderSection")).FirstOrDefault();
                _headerSectionExtended= formDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("ExtendedHeaderSection")).FirstOrDefault();
                _headerSection2 = formDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("HeadeSection2")).FirstOrDefault();
                TaskDetailsView= formDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("TaskTabs")).FirstOrDefault();
                Tableview = formDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("TableView")).FirstOrDefault();
                ToDoButtonSec = formDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("ToDoButton")).FirstOrDefault();
                formDefinition.IsDebugMode = true;
            }

           
            HideToggleSection();
            await GetKanbanBoard();

            GetListview();

        }

        public async void InitializeObjects()
        {
            _interactionLogic = new Dictionary<string, EventCallback>();
            _objectHelpers = new Dictionary<string, IBLUIOperationHelper>();
            ListOfBoards = new List<CodeBaseResponseExtended>();
            _headerSection = new BLUIElement();
            _refBuilder = new UIBuilder();
            _headerSectionExtended = new BLUIElement();
            _headerSection2= new BLUIElement();
            _processRequest = new ProcessRequest();
            ListOfBoardsWithCount=new List<CodeBaseResponseExtended>();
            _dropContainer = new MudDropContainer<Process>();
            ListOfTasks=new List<Process>();  
            TaskDetailsView=new BLUIElement();
            ListOfSubTasks= new List<Process>();
            isKanbanview = true;
            Tableview = new BLUIElement();
            _user = new UserDetails();
            ListOfTableTask = new List<Process>();
            ToDoButtonSec=new BLUIElement();
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
            appStateService._AppBarName = "To Do";
        }



        #endregion

        #region UI Events

        private void OnToggeleExpandClick()
        {
            if (!isExpandable)
            {
                ShowToggleSection();
            }
            else
            {
                HideToggleSection();
            }
            
            
        }

        private void OnKanbanviewClick()
        {
            Hidepopup();
            Listview = false;
            isKanbanview =true;
                UIStateChanged();
        }
        private async void OnListviewClick()
        {
            //appStateService.IsLoaded= true;
            Hidepopup();
            isKanbanview = false;
            Listview = true;
            GetListview();
            //appStateService.IsLoaded = false;
            UIStateChanged();
        }

        private void OnViewOptionOpen(long TaskKey,int SubTaskCount)
        {
            Hidepopup();
            isOpenViewOption = true;
            TaskIdentity = TaskKey;
            Count = SubTaskCount;
            UIStateChanged();


        }

    

        private void OnProjectChange(UIInterectionArgs<ProjectResponse> args)
        {
            _processRequest.ProjectKey=args.DataObject != null ? Convert.ToInt32(args.DataObject.ProjectKey) :1;
            UIStateChanged();
        }

        private async void OnPriorityChange(UIInterectionArgs<CodeBaseResponse> args)
        {
            _processRequest.PrtyKy = args.DataObject != null ? Convert.ToInt32(args.DataObject.CodeKey):1;
            //_processRequest.Priority = args.DataObject;
            await GetListOfTasksCount();
            UIStateChanged();
        }

        private async void OnLeadChange(UIInterectionArgs<AddressResponse> args)
        {
            _processRequest.LeadAdrKy = args.DataObject != null ? Convert.ToInt32(args.DataObject.AddressKey) : 1;
            await GetListOfTasksCount();
            UIStateChanged();
        }

        private async void OnCurrentActionChange(UIInterectionArgs<AddressResponse> args)
        {
            _processRequest.CurAdrKy = args.DataObject != null ? Convert.ToInt32(args.DataObject.AddressKey) : 1;
            await GetListOfTasksCount();
            UIStateChanged();
        }

        private void OnIsStandupChange(UIInterectionArgs<bool> args)
        {
           
            _processRequest.IsStandup = args.DataObject;
            UIStateChanged();
        }
        private void OnIsDevChange(UIInterectionArgs<bool> args)
        {
            _processRequest.IsDeveloper = args.DataObject;
            UIStateChanged();
        }
        private void OnIsSprintChange(UIInterectionArgs<bool> args)
        {
            _processRequest.IsSprint = args.DataObject;
            UIStateChanged();
        }
        private void OnIsConsultantChange(UIInterectionArgs<bool> args)
        {
            _processRequest.IsConsultant = args.DataObject;
            UIStateChanged();
        }
        private void OnIsCEOChange(UIInterectionArgs<bool> args)
        {
            _processRequest.IsStandup = args.DataObject;
            UIStateChanged();
        }
        private void OnIsCoporateChange(UIInterectionArgs<bool> args)
        {
            _processRequest.IsCorporate = args.DataObject;
            UIStateChanged();
        }

        private void OnIsClientsChange(UIInterectionArgs<bool> args)
        {
            _processRequest.IsClients = args.DataObject;
            UIStateChanged();
        }

        //private void OnFromDateChange(UIInterectionArgs<DateTime?> args)
        //{
        //    _processRequest.FromDate = args.DataObject.Value;
        //    UIStateChanged();
        //}

        //private void OnToDateChange(UIInterectionArgs<DateTime?> args)
        //{
        //    _processRequest.ToDate = args.DataObject.Value;
        //    UIStateChanged();
        //}

        private async void OnSearchChange(UIInterectionArgs<string> args)
        {
            _processRequest.SearchQuery = args.DataObject;
            await GetListOfTasksCount();
            UIStateChanged();
        }
        private async void ExpansionPanelClick(CodeBaseResponseExtended item)
        {
            await GetListOfTasks(item.CodeKey);
            UIStateChanged();
        }

        private async void OpenNewTaskPopup(UIInterectionArgs<object> args)
        {
            TaskIdentity = 1;
            SubTaskIdentity=1;
            _navigationManager.NavigateTo($"/Process/NewTask/{NavigateElementKey}/{TaskIdentity}/{SubTaskIdentity}");
            UIStateChanged();
        }

        #endregion


        #region Functions

        public async Task GetKanbanBoard()
        {
            ProcessRequest code = new ProcessRequest()
            {
                ConditionCode="AprSts"
            };
            ListOfBoards = await _processManager.GetKanbanBoard(code);
            UIStateChanged();
        }

        public async Task GetListOfTasks(int CodeKey)
        {
            _processRequest.AprStsKy = CodeKey;
            //_processRequest.CurAdrKy = 634911;
            ListOfTasks = await _processManager.GetListOfTasks(_processRequest);
             _dropContainer.Refresh();
            UIStateChanged();
        }

        public async Task GetListOfTasksCount()
        {
            ListOfBoardsWithCount = await _processManager.GetListOfTasksCount(_processRequest);
            foreach (CodeBaseResponseExtended item in ListOfBoards)
            {
                CodeBaseResponseExtended recs = ListOfBoardsWithCount.Where(x => x.CodeKey == item.CodeKey).FirstOrDefault();
                if (recs != null)
                {
                    int Count = recs.Count;
                    item.CdExtraInformation1 = item.CodeName + " (" + Count + ")";
                }
                else
                {
                    item.CdExtraInformation1 = item.CodeName + " (0)";
                }
                
            }
            UIStateChanged();
        }

        public void HideToggleSection()
        {
            isExpandToggle = false;
            isExpandable = false;
            UIStateChanged();
        }

        public void ShowToggleSection()
        {
            isExpandToggle = true;
            isExpandable = true;
            UIStateChanged();
        }

        private void ItemUpdated(MudItemDropInfo<Process> dropItem)
        {
            dropItem.Item.ApproveStatus.CodeName = dropItem.DropzoneIdentifier;
        }

        
        private void Hidepopup()
        {
            isOpenNewTask=false;
            isOpenViewOption=false;
        }


        private void BreakComponent()
        {
            if (formDefinition != null)
            {
                var childsHash = formDefinition.Children.ToLookup(elem => elem.ParentKey);
                foreach (var child in formDefinition.Children)
                {
                    child.Children = childsHash[child.ElementKey].ToList();
                }
                BLUIElement form = formDefinition.Children.Where(x => x.ElementKey == formDefinition.ElementKey).FirstOrDefault();
                if (form != null)
                {
                    formDefinition = form;



                }
            }
        }

        private async void OnTableEditClick(UIInterectionArgs<object> args)
        {
            Process editdata = args.DataObject as Process;
            TaskIdentity = editdata.TaskKey;
            SubTaskIdentity = 1;
            _navigationManager.NavigateTo($"/Process/NewTask/{NavigateElementKey}/{TaskIdentity}/{SubTaskIdentity}");
           
            StateHasChanged();
        }

        public async void GetListview()
        {
            _user = await _hrManager.GetUserAsync();
            _processRequest.CurAdrKy = _user.AdrKy;
           // _processRequest.LeadAdrKy = _user.AdrKy;
            ListOfTasks = await _processManager.GetTodo_ListviewSelectWeb(_processRequest);
        }

        //private async void PlayButtonClick(long TaskKey)
        //{
        //    Hidepopup();
        //    isPlayClicked= true;
        //    StartTimer();
        //    UIStateChanged();
        //}

        //private async void StopButtonClick(long TaskKey)
        //{
        //    Hidepopup();
        //    isPlayClicked = false;
        //    StopTimer();
        //    UIStateChanged();
        //}

        //private void OnTimeEvent(object Obj, ElapsedEventArgs e)
        //{
        //    DateTime CurrentTimerTime = e.SignalTime;
        //    elapsedTime = $"{CurrentTimerTime.Subtract(TimerStartTime)}".Substring(0, 8);
        //    this.InvokeAsync(StateHasChanged);
        //}
        //public void Dispose()
        //    => this.timer.Elapsed -= this.OnTimeEvent;
        //private void StartTimer()
        //{
        //    TimerStartTime=DateTime.Now;
        //    timer = new System.Timers.Timer(1000);
        //    timer.Elapsed += OnTimeEvent;
        //    timer.AutoReset= true;
        //    timer.Enabled=true;
        //}

        //private void StopTimer()
        //{
        //    timer.Enabled = false;
        //    elapsedTime = DefaultTime;
        //}

        private void PlayButtonClick(long TaskKey)
        {
            var task = ListOfTasks.First(t => t.TaskKey == TaskKey);

            if (!task.IsPlaying)
            {
                Hidepopup();
                task.IsPlaying = true;
                task.TimerStartTime = DateTime.Now;
                StartTimer(task);
                UIStateChanged();
            }
        }

        private void StopButtonClick(long TaskKey)
        {
            var task = ListOfTasks.First(t => t.TaskKey == TaskKey);

            if (task.IsPlaying)
            {
                Hidepopup();
                task.IsPlaying = false;
                StopTimer(task);
                UIStateChanged();
            }
        }

        private void StartTimer(Process task)
        {
            task.Timer = new System.Timers.Timer(1000);
            task.Timer.Elapsed += (sender, e) => OnTimeEvent(sender, e, task);
            task.Timer.AutoReset = true;
            task.Timer.Enabled = true;
        }

        private void StopTimer(Process task)
        {
            task.Timer.Enabled = false;
            task.ElapsedTime = DefaultTime;
        }

        private void OnTimeEvent(object Obj, ElapsedEventArgs e, Process task)
        {
            DateTime CurrentTimerTime = e.SignalTime;
            task.ElapsedTime = $"{CurrentTimerTime.Subtract(task.TimerStartTime)}".Substring(0, 8);
            this.InvokeAsync(StateHasChanged);
        }

        public void Dispose()
        {
            foreach (var task in ListOfTasks)
            {
                task.Timer.Elapsed -= (sender, e) => OnTimeEvent(sender, e, task);
                task.Timer.Dispose();
            }
        }
        #endregion
    }
}
