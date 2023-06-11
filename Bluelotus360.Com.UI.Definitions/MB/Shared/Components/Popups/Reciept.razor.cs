﻿using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.CleanArchitecture.Domain.Entities.Transaction;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using BlueLotus360.Com.UI.Definitions.Extensions;

namespace BlueLotus360.Com.UI.Definitions.MB.Shared.Components.Popups
{
    public partial class Reciept
    {
        [Parameter]
        public IDictionary<string, EventCallback> InteractionLogics { get; set; }


        [Parameter]
        public IDictionary<string, IBLUIOperationHelper> ObjectHelpers { get; set; }

        [Parameter]
        public BLUIElement RecieptUIDefinitions { get; set; }


        [Parameter]
        public BLTransaction Transaction { get; set; }


        [Parameter]
        public EventCallback OnCloseClick { get; set; }
        private IList<RecieptDetailResponse> currentTrasnactionReps = new List<RecieptDetailResponse>();

        private object DataObject = new object();

        private IList<AccoutRecieptPayment> payments = new List<AccoutRecieptPayment>();

        private decimal BalanceAmount = 0;

        private bool HasPaymentEntries = false;

        protected override void OnInitialized()
        {

            base.OnInitialized();
        }




        protected override async Task OnParametersSetAsync()
        {

            await base.OnParametersSetAsync();
        }


        public async Task ReadRecieptsDetails()
        {
            currentTrasnactionReps = new List<RecieptDetailResponse>();

            if (Transaction == null || Transaction.TransactionKey < 11)
            {
                return;
            }
            URLDefinitions url = new URLDefinitions();
            url.URL = RecieptUIDefinitions.UrlController + "/" + RecieptUIDefinitions.UrlAction;
            RecieptDetailRequest request = new RecieptDetailRequest();
            request.ElementKey = Transaction.ElementKey;
            request.TransactionKey = Transaction.TransactionKey;
            currentTrasnactionReps = await _transactionManager.GetRecieptDetailResponses(request, url);
            if (currentTrasnactionReps != null && currentTrasnactionReps.Count > 0)
            {
                BalanceAmount = currentTrasnactionReps[0].BalanceAmount;

            }

            await ReadPayableInformations();
            HasPaymentEntries = payments.Count > 0;
            StateHasChanged();
        }


        public async Task ReadPayableInformations()
        {
            if (Transaction.Location.CodeKey > 10 && Transaction.TransactionKey > 10)
            {
                AccPaymentMappingRequest request = new AccPaymentMappingRequest();
                request.ELementKey = Transaction.ElementKey;
                request.Location = Transaction.Location;
                request.LoadAll = true;
                request.PayementTerm = new CodeBaseResponse();
                payments = new List<AccoutRecieptPayment>();
                IList<AccPaymentMappingResponse> responses = await _comboManager.GetPayementAccountMapping(request);

                if (responses != null && responses.Count > 0)
                {
                    foreach (AccPaymentMappingResponse item in responses)
                    {
                        AccoutRecieptPayment payment = new AccoutRecieptPayment();
                        payment.AccountTransactionKey = currentTrasnactionReps[0].DebitAccountTransactioKey;
                        payment.DebitAccountKey = item.Account.AccountKey;
                        payment.PaymentTerm = item.PayementMode;
                        payments.Add(payment);
                    }


                }
                await Task.CompletedTask;
                StateHasChanged();
            }
        }

        public async void OnCloseButtonClick()
        {
            if (OnCloseClick.HasDelegate)
            {
                await OnCloseClick.InvokeAsync();
            }
        }

        public async void OnSaveButtonClick()
        {
            if (BalanceAmount == 0 || BalanceAmount < payments.Sum(x => x.Amount))
            {
                bool? result = await _dialogService.ShowMessageBox(
                   "Warning",
                   $"Total Cannot exceed Balance Amount"
                        );
                return;
            }

            foreach (AccoutRecieptPayment item in payments)
            {
                if (item.Amount > 0)
                {
                    await _transactionManager.SaveAccountRecieptPayement(item);
                }
            }
            bool? resultx = await _dialogService.ShowMessageBox(
                 "Success",
                 $"Reciept Saved Successfully"
                      );
            await ReadRecieptsDetails();
            StateHasChanged();
            await Task.CompletedTask;
        }
    }
}
