
namespace bluelotus360.Com.commonLib.Routes
{
    public static class TokenEndpoints
    {
        public static string Get = "api/identity/token";
        public static string Refresh = "api/identity/token/refresh";

        public static string AuthenticateURL = BaseEndpoint.BaseURL + "Authentication/Authenticate";
        public static string CompanyListingEndPoint = BaseEndpoint.BaseURL + "Authentication/GetUserCompanies";
        public static string CompanyDetailsEndpoint = BaseEndpoint.BaseURL + "Company/GetCompanyInformation";
        public static string CompanySelectedEndPoint = BaseEndpoint.BaseURL + "Authentication/UpdateSelectedCompany";
        public static string OrderSaveEndpoint = BaseEndpoint.BaseURL + "Order/CreateGenericOrder";
        public static string FindOrder = BaseEndpoint.BaseURL + "Order/FindOrders";
        public static string LoadOrderEndPoint = BaseEndpoint.BaseURL + "Order/OpenOrder";
        public static string LoadOrderEndPointFromQuotation = BaseEndpoint.BaseURL + "Order/OpenQuotation";
        public static string OpenQuotationAsSalesOrderEndPoint = BaseEndpoint.BaseURL + "Order/LoadOrder";
        public static string FindFromQuotation = BaseEndpoint.BaseURL + "Order/GetFromQuotation";
        public static string ItemRateEndPoint = BaseEndpoint.BaseURL + "Item/GetItemRateEx";
        public static string GetItemByItemCodeEndPoint = BaseEndpoint.BaseURL + "Item/GetItemByItemCode";
        public static string openReportEndPoint = BaseEndpoint.BaseURL + "Report/OpenReports";
        public static string HtmlToPdfReportEndPoint = BaseEndpoint.BaseURL + "Report/HtmlToPdf";
        public static string CompanyReportInformationEndPoint = BaseEndpoint.BaseURL + "Company/GetCompanyInformation";
        public static string OrderEditEndpoint = BaseEndpoint.BaseURL + "Order/UpdateGenericOrder";
        public static string TransactionSaveEndpoint = BaseEndpoint.BaseURL + "Transaction/saveTransaction"; 
        public static string LoadOrderApprovalDetails = BaseEndpoint.BaseURL + "Order/LoadOrderApprovals";
        public static string UpdateOrderApproval = BaseEndpoint.BaseURL + "Order/InsertUpdateApprovals";
        public static string CheckAddeditPermissionForAddEditOrdTrnEndPoint = BaseEndpoint.BaseURL + "Order/CheckAddeditPermissionForAddEditOrdTrn";
        public static string StockAsAtEndpoint { get; set; } = BaseEndpoint.BaseURL + "Transaction/GetStockAsAtByLocation";
        public static string AllStockAsAtEndpoint { get; set; } = BaseEndpoint.BaseURL + "Transaction/GetAllStockAsAtByLocation";
       

        public static string FindTransaction = BaseEndpoint.BaseURL + "Transaction/FindTransaction";
        public static string GetPriceListEndPoint = BaseEndpoint.BaseURL + "Transaction/PriceList";
        public static string GetAccountMapping = BaseEndpoint.BaseURL + "Account/GetAccMappingByPaymentType";
        public static string OpenTransaction = BaseEndpoint.BaseURL + "Transaction/OpenTransaction";
        public static string ReadFromTransaction = BaseEndpoint.BaseURL + "Transaction/ReadFromTransaction";
        public static string SaveAccountRecieptURL = BaseEndpoint.BaseURL + "Transaction/SaveAccountReciept";
        public static string ItemImageRequest = BaseEndpoint.BaseURL + "Product/GetItemImage";
        public static string CashDenominationRead = BaseEndpoint.BaseURL + "Transaction/GetDenomination";
        public static string SaveDenominationEndpint = BaseEndpoint.BaseURL + "Transaction/SaveDenomination";
        public static string LoadTransactionApprovalDetails = BaseEndpoint.BaseURL + "Transaction/LoadTransactionApprovals";
        public static string UpdateTransactionApproval = BaseEndpoint.BaseURL + "Transaction/InsertUpdateApprovals";
        public static string CheckTransactionPermissionEndpoint = BaseEndpoint.BaseURL + "Transaction/getOrderTrnPermission";
        public static string GetItemByItemCodeEndpoint = BaseEndpoint.BaseURL + "Item/getItemByItemCode";

        public static string GetVehicleDetails = BaseEndpoint.BaseURL + "Booking/GetVehicleDetails";
        public static string GetBookingItmDetails = BaseEndpoint.BaseURL + "Booking/GetBookingItmDetails";
        //public static string GetBookingCusAdrDetails = BaseEndpoint.BaseURL + "Booking/GetBookingCusAdrDetails";
        public static string GetInsertUpdateBooking = BaseEndpoint.BaseURL + "Booking/InsertUpdateBooking";
        public static string TabDetails = BaseEndpoint.BaseURL + "Booking/GetTabDetails";
        public static string CreateNewServiceType = BaseEndpoint.BaseURL + "Booking/CreateServiceType";
        public static string InsertServiceType = BaseEndpoint.BaseURL + "Booking/InsertServiceType";

        public static string GetBookedCustomerDetails = BaseEndpoint.BaseURL + "BookingModule/GetBookedCustomerDetails";
		public static string GetBookingList = BaseEndpoint.BaseURL + "BookingModule/GetBookingList";

		public static string GetProfileList = BaseEndpoint.BaseURL + "MasterData/GetProjectProfileList";
        public static string UpdateProfile = BaseEndpoint.BaseURL + "MasterData/UpdateProfile";
        public static string Insertprofile = BaseEndpoint.BaseURL + "MasterData/InsertProfile";
        public static string UpdateItem = BaseEndpoint.BaseURL + "MasterData/UpdateItemList";
        public static string InsertItem = BaseEndpoint.BaseURL + "MasterData/InsertItemList";
        public static string GetItemProfileSelectList = BaseEndpoint.BaseURL + "MasterData/GetItemList";

        //Account profile
        public static string GetAccountProfileList = BaseEndpoint.BaseURL + "MasterData/GetProfileList";
        public static string InsertAccountProfileItem = BaseEndpoint.BaseURL + "MasterData/InsertAccountRecord";
        public static string UpdateAccountProfile = BaseEndpoint.BaseURL + "MasterData/UpdateAccountRecord";
        //public static string UpdateAccountProfile = BaseEndpoint.BaseURL + "Account/UpdateAccount";

        public static string GetLocationViseStocks = BaseEndpoint.BaseURL + "Chart/GetStockList";
        public static string GetSalesHeaderDetails = BaseEndpoint.BaseURL + "Chart/GetSalesDetails";
        public static string GetSalesByLocationEndPoint = BaseEndpoint.BaseURL + "Chart/GetLocationWiseSales";
        public static string GetSalesByLocationRepEndPoint = BaseEndpoint.BaseURL + "Chart/GetLocationWiseSalesRep";
        public static string GetActualVsBudgetedIncomeEndPoint = BaseEndpoint.BaseURL + "Chart/GetActualVsBudgetedIncomeDetails";
        public static string GPft_NetPft_MarginEndPoint = BaseEndpoint.BaseURL + "Chart/Get_Gprf_Netprf_Details";
        public static string Get_Debtors_Age_Analysis_EndPoint = BaseEndpoint.BaseURL + "Chart/Get_Debtors_Ages";
        public static string Get_Creditors_Age_Analysis_EndPoint = BaseEndpoint.BaseURL + "Chart/Get_Creditors_Ages";
        public static string Get_Debtors_Age_Analysis_Overdue_EndPoint = BaseEndpoint.BaseURL + "Chart/Get_Debtors_Ages_Overdue";
        public static string Get_Creditors_Age_Analysis_Overdue_EndPoint = BaseEndpoint.BaseURL + "Chart/Get_Creditors_Ages_Overdue";
        public static string GPft_NetPft_DT_EndPoint = BaseEndpoint.BaseURL + "Chart/Get_Gprf_Netprf_Details_DT";
        public static string Get_Combined_Finance_Chart_EndPoint= BaseEndpoint.BaseURL + "Chart/GetCombinedFinanceChart";    
        public static string Get_Debtors_DT_EndPoint = BaseEndpoint.BaseURL + "Chart/Get_Debtors_Ages_DT";
        public static string Get_Creditors_DT_EndPoint = BaseEndpoint.BaseURL + "Chart/Get_Creditors_Ages_DT";
        public static string Get_Debtors_Overdue_DT_EndPoint = BaseEndpoint.BaseURL + "Chart/Get_Debtors_Ages_Overdue_DT";
        public static string Get_Creditors_Overdue_DT_EndPoint = BaseEndpoint.BaseURL + "Chart/Get_Creditors_Ages_Overdue_DT";
        public static string Get_Debtor_DT_Transaction_Details_EndPoint = BaseEndpoint.BaseURL + "Chart/Get_Debtors_Ages_DT_Transaction_Details";
        public static string Get_User_Details_EndPoint = BaseEndpoint.BaseURL + "HR/GetEmployeeDetails";
        public static string Get_Address_DetailsByLogin_EndPoint = BaseEndpoint.BaseURL + "HR/GetAdressDetailsByLogin";
        public static string Get_Existing_Record_EndPoint = BaseEndpoint.BaseURL + "HR/MultiAtnAnlysis";
        public static string Get_Existing_Record_EndPointV2 = BaseEndpoint.BaseURL + "HR/MultiAtnAnlysisV2";
        public static string Put_In_EndPoint = BaseEndpoint.BaseURL + "HR/InOut";
        public static string Get_Shift_EndPoint = BaseEndpoint.BaseURL + "HR/GetShiftDetails";
        public static string Update_EndPoint = BaseEndpoint.BaseURL + "HR/UpdateRecord";
        public static string Put_Manual_Attendence_EndPoint = BaseEndpoint.BaseURL + "HR/AddManualAttendenceInOut";
        public static string User_Permission_EndPoint = BaseEndpoint.BaseURL + "HR/GetUserPermissionUnderCompany";

        public static string Get_Already_Applied_Leave_EndPoint = BaseEndpoint.BaseURL + "HR/GetAlreadyAppliedLeaves";
        public static string Get_LeaveTrnTypeDetails_EndPoint = BaseEndpoint.BaseURL + "HR/RetriveLeaveTrnTypeDetails";
        public static string Get_Short_Leave_Key_EndPoint = BaseEndpoint.BaseURL + "HR/RetriveShortLeaveKey";
        public static string Get_Leave_Type_By_Company_EndPoint = BaseEndpoint.BaseURL + "HR/RetriveLeaveTypeByCompany";
        public static string Get_Leave_Summary_EndPoint = BaseEndpoint.BaseURL + "HR/LoadLeaveSummary";
        public static string Get_MultiApproval_EndPoint = BaseEndpoint.BaseURL + "HR/CheckMultiApproval";
        public static string Apply_Leave_EndPoint = BaseEndpoint.BaseURL + "HR/ApplyLeave";
        public static string Retrive_Reporting_Person_EndPoint = BaseEndpoint.BaseURL + "HR/RetriveReportingPerson";
        public static string SelectLeaveCheck_EndPoint = BaseEndpoint.BaseURL + "HR/SelectLeaveCheck";
        public static string Delete_Leave_EndPoint = BaseEndpoint.BaseURL + "HR/DeleteLeave";
        public static string Leave_Filter_EndPoint = BaseEndpoint.BaseURL + "HR/FilterLeaves";
        public static string Change_Leave_Status_EndPoint = BaseEndpoint.BaseURL + "HR/LevTrnAprInsert";
        public static string Get_Employee_Details_EndPoint = BaseEndpoint.BaseURL + "HR/GetEmployeeProfile";
        public static string Generate_Payslip_EndPoint = BaseEndpoint.BaseURL + "HR/GeneratePaySlip";
        public static string PaySlip_EndPoint = BaseEndpoint.BaseURL + "HR/PrintPaySlip";
        public static string CreateNewAddressURL = BaseEndpoint.BaseURL + "Address/CreateNewAddress";
        public static string UserInfoReadURL = BaseEndpoint.BaseURL + "Authentication/GetCompanyInformation";
        public static string ItemImageReadURL = BaseEndpoint.BaseURL + "Item/GetItemImage";
        public static string Save_Trn_Hdr_From_Location_EndPoint = BaseEndpoint.BaseURL + "ItemTransfer/TrnHeaderSaveFromLocation";
        public static string Save_Trn_Hdr_To_Location_EndPoint = BaseEndpoint.BaseURL + "ItemTransfer/TrnHeaderSaveToLocation";
        public static string Get_Data_EndPoint = BaseEndpoint.BaseURL + "ItemTransfer/GetScannedData";
        public static string Get_Invoice_Data_EndPoint = BaseEndpoint.BaseURL + "ItemTransfer/GetScannedInvoiceData";
        public static string Save_Line_EndPoint = BaseEndpoint.BaseURL + "ItemTransfer/LineItemSave";
        public static string Find_ItmTrn_EndPoint = BaseEndpoint.BaseURL + "ItemTransfer/FindItemTransfers";
        public static string Refresh_Header_EndPoint = BaseEndpoint.BaseURL + "ItemTransfer/ItemTransferSelect";
        public static string Update_Header_For_Out_EndPoint = BaseEndpoint.BaseURL + "ItemTransfer/TrnHeaderUpdateOut";
        public static string Update_Header_For_In_EndPoint = BaseEndpoint.BaseURL + "ItemTransfer/TrnHeaderUpdateIn";
        public static string Update_Line_For_Out_EndPoint = BaseEndpoint.BaseURL + "ItemTransfer/LineItemUpdateForOut";
        public static string Update_Line_For_In_EndPoint = BaseEndpoint.BaseURL + "ItemTransfer/LineItemUpdateForIn";
        public static string FIFO_Posting_EndPoint = BaseEndpoint.BaseURL + "ItemTransfer/FIFO";
        public static string Refresh_ItemTransfer_Invoice_EndPoint = BaseEndpoint.BaseURL + "ItemTransfer/ItemTransferSelectForInvoice";
        public static string ItemTrnasactionSerialsURL = BaseEndpoint.BaseURL + "Transaction/GetSerialNumbersForTransactionLineItem";
        public static string ItemTransferValidationEndpoint = BaseEndpoint.BaseURL + "ItemTransfer/LineItemValidationBySerNo";
        public static string InvoiceItemTransferValidationEndpoint = BaseEndpoint.BaseURL + "ItemTransfer/InvoiceValidationBySerNo";
        public static string GetInvoiceItemsSerialNoList = BaseEndpoint.BaseURL + "ItemTransfer/RetriviewSerialNoList";
        public static string CreateItemTransfer_EndPoint = BaseEndpoint.BaseURL + "ItemTransfer/CreateItemTransfer";
        public static string Update_ItmTransfer_EndPoint = BaseEndpoint.BaseURL + "ItemTransfer/UpdateItemTransfer";
        public static string TransferMultiAprLock_EndPoint = BaseEndpoint.BaseURL + "ItemTransfer/TransferMultiAprLock";
        public static string ItemTrnasactionSerials = BaseEndpoint.BaseURL + "Transaction/GetSerialNumbersForTransactionLineItem";
        public static string FileUploadEndPoint = BaseEndpoint.BaseURL + "Document/UploadFile";
        public static string GetBase64DocumentsEndPoint = BaseEndpoint.BaseURL + "Document/GetBase64Doc";
        public static string GetBase64DocumentsFullExEndPoint = BaseEndpoint.BaseURL + "Document/GetBase64DocFullEx";
        public static string CreateProjectEndPoint = BaseEndpoint.BaseURL + "Project/projectHeaderInsert";
        public static string UpdateProjectEndPoint = BaseEndpoint.BaseURL + "Project/projectHeaderUpdate";
        public static string SelectProjectHeaderEndPoint = BaseEndpoint.BaseURL + "Project/projectHeaderSelect";
        public static string CatItemReadURL = BaseEndpoint.BaseURL + "Item/ReadProducts";
        public static string ItemTransferPreSavingValidationEndpoint = BaseEndpoint.BaseURL + "ItemTransfer/TransferPreSaving";
        public static string ItmTrnContraAprGridSelectWebEndPoint = BaseEndpoint.BaseURL + "ItemTransfer/ItmTrnContraAprGridSelectWeb";
        public static string ItmTrnContraAprInsertWebEndPoint = BaseEndpoint.BaseURL + "ItemTransfer/ItmTrnContraAprInsertWeb";
        public static string ItmTrnSerAprGridSelectWebEndPoint = BaseEndpoint.BaseURL + "ItemTransfer/ItmTrnSerAprGridSelectWeb";
        public static string ItmTrnSerAprGridInsertWebEndPoint = BaseEndpoint.BaseURL + "ItemTransfer/ApproveSerNoInsert";

        //journal lite
        public static string GetJournalDetails_EndPoint = BaseEndpoint.BaseURL + "Payment/RetrieveJournalDetail";
        public static string SelectAccTrnSingleEntries_EndPoint = BaseEndpoint.BaseURL + "Payment/SelectAccTrnSingleEntryDetail";
        public static string InsertSingleAccTrn_EndPoint = BaseEndpoint.BaseURL + "Payment/InsertUpdateSingleEntryDetail";
        public static string DeleteSingleEntry_EndPoint = BaseEndpoint.BaseURL + "Payment/DeleteSingleEntry";

        //lnd

        public static string InvoiceBySerial = BaseEndpoint.BaseURL + "Transaction/InvoiceFromSerianNumber";
        public static string TotalPayedRequestURL = BaseEndpoint.BaseURL + "Transaction/GetTransactionTotalPayed";
        public static string SaveItemSerialURL = BaseEndpoint.BaseURL + "Transaction/SaveItemSerialNumber";
        public static string CodeBaseDetailRequest = BaseEndpoint.BaseURL + "CodeBase/GetCodeDetail";
        public static string SaveAccountRecieptURLEx = BaseEndpoint.BaseURL + "Transaction/SaveAccountRecieptEx";
        public static string SaveAccountRecieptURLExLND = BaseEndpoint.BaseURL + "Transaction/SaveAccountRecieptExLND";
        public static string SaveHeaderSerialURL = BaseEndpoint.BaseURL + "Transaction/saveHeaderSeralNumberLaund";

        //carmart
        public static string SearchVehicleDetailsEndpoint = BaseEndpoint.BaseURL + "WorkShopManagement/SearchVehicle";
        public static string GetJobHistoryDetailsEndpoint = BaseEndpoint.BaseURL + "WorkShopManagement/getJobHistories";
        
        public static string SelectCarMartOngoingProjectDetails = BaseEndpoint.BaseURL + "WorkShopManagement/getCarProgessingProjectDetails";
        public static string SaveWorkOrderEndPoint = BaseEndpoint.BaseURL + "WorkShopManagement/createWorkOrder";
        public static string WorkOrderToPostingEndpoint = BaseEndpoint.BaseURL + "WorkShopManagement/orderToOrderPosting";
        public static string UpdateWorkOrderEndpoint = BaseEndpoint.BaseURL + "WorkShopManagement/updateWorkOrder";       
        public static string OpenWorkorderEndPoint = BaseEndpoint.BaseURL + "WorkShopManagement/openWorkOrder";
        public static string GetRecentBookingDetailsEndPoint = BaseEndpoint.BaseURL + "WorkShopManagement/getRecentBookingDetails";
        public static string SaveWorkOrderTransactionEndpoint=BaseEndpoint.BaseURL + "WorkShopManagement/saveTransactionOfWorkOrder";
        public static string OpenWorkOrderTransactionEndpoint = BaseEndpoint.BaseURL + "WorkShopManagement/openTransactionOfWorkOrder";
        public static string GetAddressByUsrKyEndPoint = BaseEndpoint.BaseURL + "Address/GetAddressByUsrKy";
        public static string WorkShopValidationEndpoint = BaseEndpoint.BaseURL + "WorkShopManagement/validationWorkOrderEndPoint";
        public static string GetAvailableStockEndpoint = BaseEndpoint.BaseURL + "Item/getAvailableStock";
        public static string UpdateApproveStateOfTrnEndPoint = BaseEndpoint.BaseURL + "Transaction/changeTransactionApproveState";
        public static string CheckPrintPermissionEndPoint= BaseEndpoint.BaseURL + "Transaction/checkPrintPermission";
        public static string GetNextApproveStatusEndPoint = BaseEndpoint.BaseURL + "Transaction/getTrnNextApproveStatus";
        public static string GetAddressByAdrKyEndPoint = BaseEndpoint.BaseURL + "Address/getAddressByAdrKy";
        public static string GetAllWorkOrdersEndPoint = BaseEndpoint.BaseURL + "WorkShopManagement/findWorkOrders";
        //public static string GetVehicleDetailsByregNoEndpoint = BaseEndpoint.BaseURL + "WorkShopManagement/getVehicleByRegNo";
        public static string Get_AuditTrail_Details = BaseEndpoint.BaseURL + "Chart/GetAuditTrailDetails";
        public static string Get_AuditTrail_Enterd_Details = BaseEndpoint.BaseURL + "Chart/GetAuditTrailEnterdDetails";
        public static string Get_AuditTrail_Updated_Details = BaseEndpoint.BaseURL + "Chart/GetAuditTrailUpdatedDetails";

        //carmart insurence
        public static string SaveIRNWorkOrderEndPoint = BaseEndpoint.BaseURL + "WorkShopManagement/createIRNWorkOrder";
        public static string EditIRNWorkOrderEndPoint = BaseEndpoint.BaseURL + "WorkShopManagement/updateIRNWorkOrder";
        public static string GetIRNDetailsEndPoint = BaseEndpoint.BaseURL + "WorkShopManagement/getIRNByStatus";
        public static string OrderMultiTransactionPostEndPoint = BaseEndpoint.BaseURL + "WorkShopManagement/orderDetailsAccountWiseMultiTransactionPost";
        public static string GetOrderMultiTransactionEndPoint = BaseEndpoint.BaseURL + "WorkShopManagement/getOrderMultiTransaction";

		//carmart--Add new Customer
		public static string CreateCustomer = BaseEndpoint.BaseURL + "Address/createCustomer";
        public static string CreateCustomerValidation = BaseEndpoint.BaseURL + "Address/customerValidation";
        public static string GetVehicleDetailsByregNoEndpoint = BaseEndpoint.BaseURL + "WorkShopManagement/getVehicleByRegNo";



        //OrderHub
        public static string GetOrderStatus = BaseEndpoint.BaseURL + "Order/GetOrderStatus";
        public static string PartnerOrderCount = BaseEndpoint.BaseURL + "Order/GetPartnerCountByStatus";
        public static string GetAllPartnerOrders = BaseEndpoint.BaseURL + "Order/GetAllPartnerOrders";
        public static string GetAPIInfo = BaseEndpoint.BaseURL + "Order/GetAPIDetails";
        public static string GetAPIEndPoints = BaseEndpoint.BaseURL + "Order/GetOrderEndPoints";
        public static string GetLastSyncTime = BaseEndpoint.BaseURL + "Order/GetLastOrderSyncTime";
        public static string GetOrderStatusByPartnerStatus = BaseEndpoint.BaseURL + "Order/GetOrderStatusByPartnerStatus";
        public static string SavePartnerOrder = BaseEndpoint.BaseURL + "Order/GetOrdersFromOrderPlatforms";
        public static string GetItemsByItemCode = BaseEndpoint.BaseURL + "Order/GetItemsByItemCode";
        public static string CheckAdvanceAnalysisAvailability = BaseEndpoint.BaseURL + "Address/CheckAdvanceAnalysisAvailability";
        public static string CreateAdvanceAnalysis = BaseEndpoint.BaseURL + "Address/CreateAdvanceAnalysis";
        public static string GetPartnerOrdersByOrderKy = BaseEndpoint.BaseURL + "Order/GetPartnerOrdersByOrderKy";
        public static string InsertLastOrderSync = BaseEndpoint.BaseURL + "Order/InsertLastOrderSync";
        public static string GenerateProvisionURL = BaseEndpoint.BaseURL + "Order/GenerateProvisionURL";
        public static string GetUberToken = BaseEndpoint.BaseURL + "Order/GenerateUberToken";
        public static string GetUberEndPoints = BaseEndpoint.BaseURL + "Order/GetUberEndPoints";
        public static string GetCodesByConditionCode = BaseEndpoint.BaseURL + "CodeBase/ReadCodeByConditionCode";
        public static string GetApiDetailsByMerchantID = BaseEndpoint.BaseURL + "Order/GetAPIDetailsByMerchantID";
        public static string UberProvision_InsertUpdate = BaseEndpoint.BaseURL + "Order/UberProvision_InsertUpdate";
        public static string GetOrderMenuConfiguration = BaseEndpoint.BaseURL + "Order/GetOrderMenuConfiguration";
        public static string OrderMenuConfiguration_InsertUpdate = BaseEndpoint.BaseURL + "Order/OrderMenuConfiguration_InsertUpdate";
        public static string OrderHubStatus_UpdateWeb = BaseEndpoint.BaseURL + "Order/OrderHubStatus_UpdateWeb";
        public static string GetAllOrderMenuItems = BaseEndpoint.BaseURL + "Order/GetAllOrderMenuItems";
        public static string GetNextOrderHubStatusByStatusKey = BaseEndpoint.BaseURL + "Order/GetNextOrderHubStatusByStatusKey";
        public static string GetCodesByConditionCodeAndOurCode = BaseEndpoint.BaseURL + "CodeBase/GetCodeByOurCodeAndConditionCode";
        public static string GetOrderHubBU = BaseEndpoint.BaseURL + "Order/GetOrderHubBU";
        public static string GetAvailablePickmeOrders = BaseEndpoint.BaseURL + "Order/GetAvailablePickmeOrders";
        public static string APIResponseDet_InsertWeb = BaseEndpoint.BaseURL + "Order/APIResponseDet_InsertWeb";
        public static string GetPickMeOrderByOrderID = BaseEndpoint.BaseURL + "Order/GetPickMeOrderByOrderID";
        public static string UberMenu_DiscontinueWeb = BaseEndpoint.BaseURL + "Order/UberMenu_DiscontinueWeb";
        public static string GetOrderHubItemRateByItemKy = BaseEndpoint.BaseURL + "Order/GetOrderHubItemRateByItemKy";
        public static string UnmappedSKUUpdate = BaseEndpoint.BaseURL + "Order/UnmappedSKUUpdate";
        public static string OrderItem_DeleteWeb = BaseEndpoint.BaseURL + "Order/OrderItem_DeleteWeb";
        public static string GetAvailableQtyByItem = BaseEndpoint.BaseURL + "Order/GetAvailableQtyByItem";
        public static string GetMissingUberOber = BaseEndpoint.BaseURL + "Order/GetMissingUberOber";
        public static string GetRecentlyAddedPickmeOrders = BaseEndpoint.BaseURL + "Order/GetRecentlyAddedPickmeOrders";
        public static string OrderplatformOrder_Findweb = BaseEndpoint.BaseURL + "Order/OrderplatformOrder_Findweb";
        public static string GetPickmeOrdersByDuration = BaseEndpoint.BaseURL + "Order/GetPickmeOrdersByDuration";

        public static string TaskwiseAttendance = BaseEndpoint.BaseURL + "HR/TaskwiseAttendance_SelectWeb";
        public static string GetAllProjects = BaseEndpoint.BaseURL + "Project/GetAllProjects";
        public static string GetTasks = BaseEndpoint.BaseURL + "Project/GetTasks";
        public static string SaveTaskwiseAttendance = BaseEndpoint.BaseURL + "HR/SaveTaskwiseAttendance";
        
        //ToDo
        public static string GetKanbanBoard = BaseEndpoint.BaseURL + "Process/GetKanbanBoard";
        public static string GetListOfTasks = BaseEndpoint.BaseURL + "Process/GetListOfTasks";
        public static string GetListOfTasksCount = BaseEndpoint.BaseURL + "Process/GetListOfTasksCount";
        public static string GetTaskByTaskKey = BaseEndpoint.BaseURL + "Process/GetTaskByTaskKey";
        public static string GetAllDocuments = BaseEndpoint.BaseURL + "Document/GetAllDocuments";
        public static string GetProcessComponents = BaseEndpoint.BaseURL + "Process/GetProcessComponents";
        public static string DeleteDocument = BaseEndpoint.BaseURL + "Document/DeleteDocument";
        public static string DeleteComponents = BaseEndpoint.BaseURL + "Process/DeleteComponent";
        public static string GetProcessRemarksByProcess = BaseEndpoint.BaseURL + "Process/GetProcessRemarksByProcess";
        public static string SaveRemarks = BaseEndpoint.BaseURL + "Process/SaveRemarks";
        public static string UpdateTask = BaseEndpoint.BaseURL + "Process/UpdateTask";
        public static string CreateProcessComponent = BaseEndpoint.BaseURL + "Process/CreateProcessComponent";
        public static string GetSubTaskByTaskKey = BaseEndpoint.BaseURL + "Process/GetSubTaskByTaskKey";
        public static string CreateTask = BaseEndpoint.BaseURL + "Process/CreateTask";
        public static string GetNextTaskID = BaseEndpoint.BaseURL + "Process/GetNextTaskID";
        public static string GetTodo_ListviewSelectWeb = BaseEndpoint.BaseURL + "Process/GetTodo_ListviewSelectWeb";
        public static string ToDoChkLst = BaseEndpoint.BaseURL + "Process/ToDoCheckList";
        public static string CreateUpdateChkLst = BaseEndpoint.BaseURL + "Process/CreateUpdateCheckList";

        //HRAdminDashboard
        public static string GetLocationWiseHeadCountEndPoint= BaseEndpoint.BaseURL + "HR/getHRAdminDashboardMaleFemaleCountResponse";
        public static string GetAttendanceSummaryEndPoint = BaseEndpoint.BaseURL + "HR/GetAttendanceChartSummaryDetails";
        public static string GetTaskWiseAttendanceEndPoint = BaseEndpoint.BaseURL + "HR/GetTaskWiseAttendanceDetailsResponse";
        public static string GetHRAdminDashboardCardDetailsEndPoint = BaseEndpoint.BaseURL + "HR/GetHRAdminDashboardCardDetails";

        //ItemProfile version 3.0
        public static string GetItemProfileSelectListV3 = BaseEndpoint.BaseURL + "MasterData/GetItemListV3";
        public static string InsertItemV3  = BaseEndpoint.BaseURL + "MasterData/InsertItemListV3";
        public static string SelectSingleItem = BaseEndpoint.BaseURL + "MasterData/GetSingleItem";
        public static string GetMultiUnitsGridDetails = BaseEndpoint.BaseURL + "MasterData/GetMultiUnitsGridDetails";
        public static string MultiUnitsUpdateEndPoint = BaseEndpoint.BaseURL + "MasterData/UpdateMultiUnits";
        public static string MultiUnitsInsertEndPoint = BaseEndpoint.BaseURL + "MasterData/InsertMultiUnit";
        public static string UpdateItemV3 = BaseEndpoint.BaseURL + "MasterData/UpdateItemV3";
        public static string GenerateCombinations = BaseEndpoint.BaseURL + "MasterData/GenerateCombinations";
        public static string GetCombinationItems = BaseEndpoint.BaseURL + "MasterData/GetCombinationItems";
        public static string GenerateCombinationComponent = BaseEndpoint.BaseURL + "MasterData/GenerateComponent";
        public static string UpdateItemComponent = BaseEndpoint.BaseURL + "MasterData/UpdateItemComponent";
        public static string GetSingleItemComponent = BaseEndpoint.BaseURL + "MasterData/GetSingleItemComponent";
        public static string DeleteItemComponent = BaseEndpoint.BaseURL + "MasterData/DeleteItemComponent";
        public static string GetItemComponentListEndPoint = BaseEndpoint.BaseURL + "MasterData/GetItemComponents";

        //theme-preset
        public static string GetBLThemePresetsEndPoint = BaseEndpoint.BaseURL + "Theme/GetBLThemePresets";


        //Account Profile Version 2
        public static string InsertAccountProfile = BaseEndpoint.BaseURL + "MasterData/InsertAccountProfile";
        public static string SelectAccountRecord = BaseEndpoint.BaseURL + "MasterData/GetAccountProfileSingleRecord";
        public static string UpdateAccountProfileRecord = BaseEndpoint.BaseURL + "MasterData/UpdateAccountProfileRecord";


        #region Endpoints for Sales and Productivity Dashboard

        public static string SalesAndProductivityDashboard_GetSummaryDetails = BaseEndpoint.BaseURL + "SalesAndProductivity/GetSummaryDetails";
        public static string SalesAndProductivityDashboard_GetRouteDetails = BaseEndpoint.BaseURL + "SalesAndProductivity/GetRouteDetails";
        public static string SalesAndProductivityDashboard_GetShopDetailsForRoute = BaseEndpoint.BaseURL + "SalesAndProductivity/GetShopDetailsForRoute";
        public static string SalesAndProductivityDashboard_GetPrimaryChartDetails = BaseEndpoint.BaseURL + "SalesAndProductivity/GetPrimaryChartDetails";
        public static string SalesAndProductivityDashboard_GetItemCategoryDetails = BaseEndpoint.BaseURL + "SalesAndProductivity/GetItemCategoryDetails";

        #endregion
    }
}