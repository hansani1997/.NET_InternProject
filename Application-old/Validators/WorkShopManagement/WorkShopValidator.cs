//using BL10.CleanArchitecture.Domain.Entities.WorkShopManagement;
//using BlueLotus360.CleanArchitecture.Application.Validators.MessageSystem;
//using BlueLotus360.CleanArchitecture.Domain.Entities.Transaction;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace BL10.CleanArchitecture.Application.Validators.WorkShopManagement
//{
//    public class WorkShopValidator : IWorkShopValidator
//    {
//        private WorkOrder _work_order;
//        public UserMessageManager UserMessages { get; set; }
//        public WorkShopValidator(WorkOrder workOrd)
//        {
//            this._work_order = workOrd;
//            UserMessages = new UserMessageManager();
//        }

//        public bool CanCreateWorkOrder()
//        {
//            UserMessages.UserMessages.Clear();

//            if (_work_order.OrderCategory1!=null && _work_order.OrderCategory1.CodeKey<10)
//            {
//                UserMessages.AddErrorMessage("Please Select Work Order Category !");
//            }
//            if (_work_order.OrderCategory2 != null && _work_order.OrderCategory2.CodeKey <10)
//            {
//                UserMessages.AddErrorMessage("Please Select Work Order Type !");
//            }
//            if (_work_order.SelectedVehicle.PreviousMilage > _work_order.MeterReading)
//            {
//                UserMessages.AddErrorMessage($"Current Milage can't be less than {_work_order.SelectedVehicle.PreviousMilage}!");
//            }

//            return UserMessages.UserMessages.Count == 0;
//        }

//        public bool CanAddToGridItem() 
//        {
//            UserMessages.UserMessages.Clear();

//            if (_work_order.SelectedOrderItem != null)
//            {
//                if (_work_order.SelectedOrderItem.IsMaterialItem)
//                {
//                    if (_work_order.SelectedOrderItem.TransactionItem.ItemKey < 11)
//                    {
//                        UserMessages.AddErrorMessage("Please select a material before add !");
//                    }
//                    if (_work_order.SelectedOrderItem.TransactionUnit.UnitKey < 11)
//                    {
//                        UserMessages.AddErrorMessage("Please select a material unit before add !");
//                    }
//                    if (_work_order.SelectedOrderItem.TransactionRate <= 0)
//                    {
//                        UserMessages.AddErrorMessage("Please enter rate!");
//                    }
//                    if (_work_order.SelectedOrderItem.TransactionQuantity <= 0)
//                    {
//                        UserMessages.AddErrorMessage("Please enter quantity!");
//                    }
//                    if (_work_order.SelectedOrderItem.TransactionQuantity > _work_order.SelectedOrderItem.AvailableStock)
//                    {
//                        UserMessages.AddErrorMessage("Can't add morer than !"+ _work_order.SelectedOrderItem.AvailableStock);
//                    }

//                    if (_work_order.OrderCategory1.Code.Equals("Good Will Warranty"))
//                    {
//                        if (_work_order.SelectedOrderItem.BaringCompany.AccountKey < 11)
//                        {
//                            UserMessages.AddErrorMessage("Please enter Carmart account!");
//                        }
//                        if (_work_order.SelectedOrderItem.BaringPrinciple.AccountKey < 11)
//                        {
//                            UserMessages.AddErrorMessage("Please enter Principle account!");
//                        }
//                    }
//                }
//                else if (_work_order.SelectedOrderItem.IsServiceItem)
//                {

//                    if (_work_order.SelectedOrderItem.TransactionItem.ItemKey < 11)
//                    {
//                        UserMessages.AddErrorMessage("Please select a service before add !");
//                    }
//                    if (_work_order.SelectedOrderItem.TransactionUnit.UnitKey < 11)
//                    {
//                        UserMessages.AddErrorMessage("Please select a service unit before add !");
//                    }
//                    if (_work_order.SelectedOrderItem.TransactionRate <= 0)
//                    {
//                        UserMessages.AddErrorMessage("Please enter rate!");
//                    }
//                    if (_work_order.SelectedOrderItem.TransactionQuantity == 0)
//                    {
//                        UserMessages.AddErrorMessage("Please enter time!");
//                    }

//                    if (_work_order.OrderCategory1.Code.Equals("Good Will Warranty"))
//                    {
//                        if (_work_order.SelectedOrderItem.BaringCompany.AccountKey < 11)
//                        {
//                            UserMessages.AddErrorMessage("Please enter Carmart account!");
//                        }
//                        if (_work_order.SelectedOrderItem.BaringPrinciple.AccountKey < 11)
//                        {
//                            UserMessages.AddErrorMessage("Please enter Principle account!");
//                        }
//                    }
//                }
//                else if (_work_order.SelectedOrderItem.IsNoteItem)
//                {
//                    if (_work_order.SelectedOrderItem.TransactionItem.ItemKey < 11)
//                    {
//                        UserMessages.AddErrorMessage("Please select a note before add !");
//                    }
//                    if (string.IsNullOrEmpty(_work_order.SelectedOrderItem.Description))
//                    {
//                        UserMessages.AddErrorMessage("Please add a description before add !");
//                    }
//                }
//                else { }
//            }
           

//            return UserMessages.UserMessages.Count == 0;
//        }

//        public bool CanSaveWorkOrder()
//        {
//            UserMessages.UserMessages.Clear();

//            if (_work_order.WorkOrderMaterials.Count()==0)
//            {
//                UserMessages.AddErrorMessage("Please add at least one item before save !");
//            }
//            if (_work_order.OrderStatus != null && _work_order.OrderStatus.Code.Equals("Closed") )
//            {
//                UserMessages.AddErrorMessage("This work order is already closed!");
//            }
            

//            return UserMessages.UserMessages.Count == 0;
//        }
//    }
//}
