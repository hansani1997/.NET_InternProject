using BL10.CleanArchitecture.Domain.Entities.ProjectManagement;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities.HR;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using bluelotus360.Com.commonLib.Helpers;
using bluelotus360.com.razorComponents.Extensions;
using bluelotus360.com.razorComponents.MB.Shared.Components;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.com.razorComponents.Pages.ProjectManagement
{
    public partial class ToDoDesktop
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
        private long NavigateElementKey = 0;
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
            string url = _navigationManager.Uri.ToString();
            _navigationManager.TryGetQueryString<long>("ElementKey", out elementKey);// get element key from url 

            if (elementKey > 10)
            {
                var formrequest = new ObjectFormRequest();
                NavigateElementKey = elementKey;
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
                _headerSectionExtended = formDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("ExtendedHeaderSection")).FirstOrDefault();
                _headerSection2 = formDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("HeadeSection2")).FirstOrDefault();
                TaskDetailsView = formDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("TaskTabs")).FirstOrDefault();
                Tableview = formDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("TableView")).FirstOrDefault();
                ToDoButtonSec = formDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("ToDoButton")).FirstOrDefault();
                formDefinition.IsDebugMode = true;
            }


            HideToggleSection();
            await GetKanbanBoard();

           // GetListview();

        }

        public async void InitializeObjects()
        {
            _interactionLogic = new Dictionary<string, EventCallback>();
            _objectHelpers = new Dictionary<string, IBLUIOperationHelper>();
            ListOfBoards = new List<CodeBaseResponseExtended>();
            _headerSection = new BLUIElement();
            _refBuilder = new UIBuilder();
            _headerSectionExtended = new BLUIElement();
            _headerSection2 = new BLUIElement();
            _processRequest = new ProcessRequest();
            ListOfBoardsWithCount = new List<CodeBaseResponseExtended>();
            _dropContainer = new MudDropContainer<Process>();
            ListOfTasks = new List<Process>();
            TaskDetailsView = new BLUIElement();
            ListOfSubTasks = new List<Process>();
            isKanbanview = true;
            Tableview = new BLUIElement();
            _user = new UserDetails();
            ListOfTableTask = new List<Process>();
            ToDoButtonSec= new BLUIElement();
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

        #region Functions
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

        public void HideToggleSection()
        {
            isExpandToggle = false;
            isExpandable = false;
            UIStateChanged();
        }
        public async Task GetKanbanBoard()
        {
            ProcessRequest code = new ProcessRequest()
            {
                ConditionCode = "AprSts"
            };
            ListOfBoards = await _processManager.GetKanbanBoard(code);
            UIStateChanged();
        }

		private void TaskUpdated(MudItemDropInfo<Process> info)
		{
			info.Item.ApproveStatus.CodeName = info.DropzoneIdentifier;
		}

		private void OnProjectChange(UIInterectionArgs<ProjectResponse> args)
		{
			_processRequest.ProjectKey = args.DataObject != null ? Convert.ToInt32(args.DataObject.ProjectKey) : 1;
			UIStateChanged();
		}

		private async void OnPriorityChange(UIInterectionArgs<CodeBaseResponse> args)
		{
			_processRequest.PrtyKy = args.DataObject != null ? Convert.ToInt32(args.DataObject.CodeKey) : 1;
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
            ListOfTasks = await _processManager.GetListOfTasks(_processRequest);
            _dropContainer.Refresh();
            UIStateChanged();
		}

        
        #endregion
    }
}
