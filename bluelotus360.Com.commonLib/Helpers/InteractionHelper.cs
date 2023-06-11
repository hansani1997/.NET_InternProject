using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities.Transaction;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.Com.commonLib.Helpers
{
    public class InteractionHelper
    {
        public object BlazorComponent { get; set; }

        public BLUIElement MainUiElement { get; set; }

        public InteractionHelper(object BlazorComponent, BLUIElement element)
        {
            this.BlazorComponent = BlazorComponent;//blazor component
            MainUiElement = element;//  all menu ui elements

        }


        public IDictionary<string, EventCallback> GenerateEventCallbacks()
        {
            IDictionary<string, EventCallback> callbacks = new Dictionary<string, EventCallback>();

            Type t = BlazorComponent.GetType();//get type of the blazor component
            if (MainUiElement != null && MainUiElement.Children != null && MainUiElement.Children.Count() > 0)
            {
                foreach (BLUIElement elem in MainUiElement.Children)// generate onchanage events and onbeforedatafetch events in chidern components of the  blazor component 
                {
                    GenerateOnChangeEvents(callbacks, t, elem); //callback- empty  dictionary with string and callback,t - type of the blazor component , elem - childern component  by childern wise 
                    GenerateOnBeforeDataFecth(callbacks, t, elem);
                    GenerateOnAfterDataFecth(callbacks, t, elem);
                    GenerateOnEnterEvents(callbacks, t, elem);
                    GenerateOnAdornmentClickEvents(callbacks, t, elem);
                    GenerateOnToggledEvents(callbacks, t, elem);

                }
            }


            return callbacks;
        }

        private void GenerateOnChangeEvents(IDictionary<string, EventCallback> callbacks, Type t, BLUIElement elem)
        {
            if (!string.IsNullOrWhiteSpace(elem.OnClickAction))
            {
                EventCallback callbackCh;
                if (callbacks.TryGetValue(elem.OnClickAction, out callbackCh))
                {
                    // This will avoid same key execption and if you have brains guess what it will do... :(
                    return;
                }
                if (elem.ElementType != null && (elem.ElementType.Equals("Cmb") || elem.ElementType.Equals("TelCmb")))
                {
                    if (elem.IsFreeze)
                    {
                        if (elem.ElementID != null && elem.ElementID.Equals("CodeBase"))
                        {
                            MethodInfo targetCaller = t
                                     .GetMethod(elem.OnClickAction, BindingFlags.NonPublic | BindingFlags.Instance);
                            if (targetCaller != null)
                            {
                                Action<UIInterectionArgs<IList<CodeBaseResponse>>> selectBoxAction = (Action<UIInterectionArgs<IList<CodeBaseResponse>>>)
                                    Delegate.CreateDelegate(typeof(Action<UIInterectionArgs<IList<CodeBaseResponse>>>), BlazorComponent, targetCaller);
                                EventCallback callback = new EventCallback(null, selectBoxAction);
                                callbacks.Add(elem.OnClickAction, callback);
                            }


                        }
                        if (elem.ElementID != null && elem.ElementID.Equals("Address"))
                        {
                            MethodInfo targetCaller = t
                                     .GetMethod(elem.OnClickAction, BindingFlags.NonPublic | BindingFlags.Instance);
                            if (targetCaller != null)
                            {
                                Action<UIInterectionArgs<IList<AddressResponse>>> selectBoxAction = (Action<UIInterectionArgs<IList<AddressResponse>>>)
                                    Delegate.CreateDelegate(typeof(Action<UIInterectionArgs<IList<AddressResponse>>>), BlazorComponent, targetCaller);
                                EventCallback callback = new EventCallback(null, selectBoxAction);
                                callbacks.Add(elem.OnClickAction, callback);
                            }


                        }
                    }
                    else
                    {
                        if (elem.ElementID != null && elem.ElementID.Equals("CodeBase"))
                        {
                            MethodInfo targetCaller = t.GetMethod(elem.OnClickAction, BindingFlags.NonPublic | BindingFlags.Instance);//search all non public instances those have elem.onClickAction name in the given blazor component  

                            if (targetCaller != null)
                            {
                                Action<UIInterectionArgs<CodeBaseResponse>> codeBaseAction = (Action<UIInterectionArgs<CodeBaseResponse>>)
                                    Delegate.CreateDelegate(typeof(Action<UIInterectionArgs<CodeBaseResponse>>), BlazorComponent, targetCaller);
                                EventCallback callback = new EventCallback(null, codeBaseAction);
                                callbacks.Add(elem.OnClickAction, callback);
                            }


                        }

                        if (elem.ElementID != null && elem.ElementID.Equals("Address"))
                        {
                            MethodInfo targetCaller = t
                                     .GetMethod(elem.OnClickAction, BindingFlags.NonPublic | BindingFlags.Instance);
                            if (targetCaller != null)
                            {
                                Action<UIInterectionArgs<AddressResponse>> codeBaseAction = (Action<UIInterectionArgs<AddressResponse>>)
                                    Delegate.CreateDelegate(typeof(Action<UIInterectionArgs<AddressResponse>>), BlazorComponent, targetCaller);
                                EventCallback callback = new EventCallback(null, codeBaseAction);
                                callbacks.Add(elem.OnClickAction, callback);
                            }


                        }
                        if (elem.ElementID != null && elem.ElementID.Equals("Item"))
                        {
                            MethodInfo targetCaller = t
                                     .GetMethod(elem.OnClickAction, BindingFlags.NonPublic | BindingFlags.Instance);
                            if (targetCaller != null)
                            {
                                Action<UIInterectionArgs<ItemResponse>> codeBaseAction = (Action<UIInterectionArgs<ItemResponse>>)
                                    Delegate.CreateDelegate(typeof(Action<UIInterectionArgs<ItemResponse>>), BlazorComponent, targetCaller);
                                EventCallback callback = new EventCallback(null, codeBaseAction);
                                callbacks.Add(elem.OnClickAction, callback);
                            }


                        }
                        // edit for units 
                        if (elem.ElementID != null && elem.ElementID.Equals("Unit"))
                        {
                            MethodInfo targetCaller = t.GetMethod(elem.OnClickAction, BindingFlags.NonPublic | BindingFlags.Instance);//search all non public instances those have elem.onClickAction name in the given blazor component  

                            if (targetCaller != null)
                            {
                                Action<UIInterectionArgs<UnitResponse>> codeBaseAction = (Action<UIInterectionArgs<UnitResponse>>)
                                    Delegate.CreateDelegate(typeof(Action<UIInterectionArgs<UnitResponse>>), BlazorComponent, targetCaller);
                                EventCallback callback = new EventCallback(null, codeBaseAction);
                                callbacks.Add(elem.OnClickAction, callback);
                            }


                        }

                        if (elem.ElementID != null && elem.ElementID.Equals("Account"))
                        {
                            MethodInfo targetCaller = t.GetMethod(elem.OnClickAction, BindingFlags.NonPublic | BindingFlags.Instance);//search all non public instances those have elem.onClickAction name in the given blazor component  

                            if (targetCaller != null)
                            {
                                Action<UIInterectionArgs<AccountResponse>> codeBaseAction = (Action<UIInterectionArgs<AccountResponse>>)
                                    Delegate.CreateDelegate(typeof(Action<UIInterectionArgs<AccountResponse>>), BlazorComponent, targetCaller);
                                EventCallback callback = new EventCallback(null, codeBaseAction);
                                callbacks.Add(elem.OnClickAction, callback);
                            }


                        }

                        if (elem.ElementID != null && elem.ElementID.Equals("NextStatusCombo"))
                        {
                            MethodInfo targetCaller = t.GetMethod(elem.OnClickAction, BindingFlags.NonPublic | BindingFlags.Instance);//search all non public instances those have elem.onClickAction name in the given blazor component  

                            if (targetCaller != null)
                            {
                                Action<UIInterectionArgs<CodeBaseResponse>> codeBaseAction = (Action<UIInterectionArgs<CodeBaseResponse>>)
                                    Delegate.CreateDelegate(typeof(Action<UIInterectionArgs<CodeBaseResponse>>), BlazorComponent, targetCaller);
                                EventCallback callback = new EventCallback(null, codeBaseAction);
                                callbacks.Add(elem.OnClickAction, callback);
                            }


                        }

                        if (elem.ElementID != null && elem.ElementID.Equals("ApprovedStatus"))
                        {
                            MethodInfo targetCaller = t.GetMethod(elem.OnClickAction, BindingFlags.NonPublic | BindingFlags.Instance);//search all non public instances those have elem.onClickAction name in the given blazor component  

                            if (targetCaller != null)
                            {
                                Action<UIInterectionArgs<CodeBaseResponse>> codeBaseAction = (Action<UIInterectionArgs<CodeBaseResponse>>)
                                    Delegate.CreateDelegate(typeof(Action<UIInterectionArgs<CodeBaseResponse>>), BlazorComponent, targetCaller);
                                EventCallback callback = new EventCallback(null, codeBaseAction);
                                callbacks.Add(elem.OnClickAction, callback);
                            }


                        }
                        if (elem.ElementID != null && elem.ElementID.Equals("AcceptedName"))
                        {
                            MethodInfo targetCaller = t
                                     .GetMethod(elem.OnClickAction, BindingFlags.NonPublic | BindingFlags.Instance);
                            if (targetCaller != null)
                            {
                                Action<UIInterectionArgs<AddressResponse>> codeBaseAction = (Action<UIInterectionArgs<AddressResponse>>)
                                    Delegate.CreateDelegate(typeof(Action<UIInterectionArgs<AddressResponse>>), BlazorComponent, targetCaller);
                                EventCallback callback = new EventCallback(null, codeBaseAction);
                                callbacks.Add(elem.OnClickAction, callback);
                            }


                        }
                        if (elem.ElementID != null && elem.ElementID.Equals("SerialNumber"))
                        {
                            MethodInfo targetCaller = t
                                     .GetMethod(elem.OnClickAction, BindingFlags.NonPublic | BindingFlags.Instance);
                            if (targetCaller != null)
                            {
                                Action<UIInterectionArgs<ItemSerialNumber>> serialNoAction = (Action<UIInterectionArgs<ItemSerialNumber>>)
                                    Delegate.CreateDelegate(typeof(Action<UIInterectionArgs<ItemSerialNumber>>), BlazorComponent, targetCaller);
                                EventCallback callback = new EventCallback(null, serialNoAction);
                                callbacks.Add(elem.OnClickAction, callback);
                            }


                        }

                        if (elem.ElementID != null && elem.ElementID.Equals("Project"))
                        {
                            MethodInfo targetCaller = t
                                     .GetMethod(elem.OnClickAction, BindingFlags.NonPublic | BindingFlags.Instance);
                            if (targetCaller != null)
                            {
                                Action<UIInterectionArgs<ProjectResponse>> serialNoAction = (Action<UIInterectionArgs<ProjectResponse>>)
                                    Delegate.CreateDelegate(typeof(Action<UIInterectionArgs<ProjectResponse>>), BlazorComponent, targetCaller);
                                EventCallback callback = new EventCallback(null, serialNoAction);
                                callbacks.Add(elem.OnClickAction, callback);
                            }


                        }

                        if (elem.ElementID != null && elem.ElementID.Equals("Task"))
                        {
                            MethodInfo targetCaller = t
                                     .GetMethod(elem.OnClickAction, BindingFlags.NonPublic | BindingFlags.Instance);
                            if (targetCaller != null)
                            {
                                Action<UIInterectionArgs<TaskResponse>> serialNoAction = (Action<UIInterectionArgs<TaskResponse>>)
                                    Delegate.CreateDelegate(typeof(Action<UIInterectionArgs<TaskResponse>>), BlazorComponent, targetCaller);
                                EventCallback callback = new EventCallback(null, serialNoAction);
                                callbacks.Add(elem.OnClickAction, callback);
                            }


                        }
                    }

                    

                }

                if (elem.ElementType != null && (elem.ElementType.Equals("NumericBox") || elem.ElementType.Equals("TelNumericBox")))
                {

                    MethodInfo targetCaller = t
                             .GetMethod(elem.OnClickAction, BindingFlags.NonPublic | BindingFlags.Instance);
                    if (targetCaller != null)
                    {
                        Action<UIInterectionArgs<decimal>> codeBaseAction = (Action<UIInterectionArgs<decimal>>)
                            Delegate.CreateDelegate(typeof(Action<UIInterectionArgs<decimal>>), BlazorComponent, targetCaller);
                        EventCallback callback = new EventCallback(null, codeBaseAction);
                        callbacks.Add(elem.OnClickAction, callback);
                    }
                }

                if (elem.ElementType != null && (elem.ElementType.Equals("TextBox") || elem.ElementType.Equals("TextArea") || elem.ElementType.Equals("TelTextBox") || elem.ElementType.Equals("TelTextArea")))
                {

                    MethodInfo targetCaller = t
                             .GetMethod(elem.OnClickAction, BindingFlags.NonPublic | BindingFlags.Instance);
                    if (targetCaller != null)
                    {
                        Action<UIInterectionArgs<string>> codeBaseAction = (Action<UIInterectionArgs<string>>)
                            Delegate.CreateDelegate(typeof(Action<UIInterectionArgs<string>>), BlazorComponent, targetCaller);
                        EventCallback callback = new EventCallback(null, codeBaseAction);
                        callbacks.Add(elem.OnClickAction, callback);
                    }
                }


                if (elem.ElementType != null && (elem.ElementType.Equals("Button") || elem.ElementType.Equals("TelButton") || elem.ElementType.Equals("ToggleButton") || elem.ElementType.Equals("Chip")))
                {

                    MethodInfo targetCaller = t
                             .GetMethod(elem.OnClickAction, BindingFlags.NonPublic | BindingFlags.Instance);
                    if (targetCaller != null)
                    {
                        Action<UIInterectionArgs<object>> codeBaseAction = (Action<UIInterectionArgs<object>>)
                            Delegate.CreateDelegate(typeof(Action<UIInterectionArgs<object>>), BlazorComponent, targetCaller);
                        EventCallback callback = new EventCallback(null, codeBaseAction);
                        callbacks.Add(elem.OnClickAction, callback);
                    }
                }



                if (elem.ElementType != null && elem.ElementType.Equals("MultiRadio"))
                {

                    MethodInfo targetCaller = t.GetMethod(elem.OnClickAction, BindingFlags.NonPublic | BindingFlags.Instance);

                    if (targetCaller != null)
                    {
                        Action<UIInterectionArgs<int>> radioAction = (Action<UIInterectionArgs<int>>)Delegate.CreateDelegate(typeof(Action<UIInterectionArgs<int>>), BlazorComponent, targetCaller);
                        EventCallback callback = new EventCallback(null, radioAction);
                        callbacks.Add(elem.OnClickAction, callback);
                    }
                }
                if (elem.ElementType != null && elem.ElementType.Equals("CheckBox"))
                {

                    MethodInfo targetCaller = t.GetMethod(elem.OnClickAction, BindingFlags.NonPublic | BindingFlags.Instance);

                    if (targetCaller != null)
                    {
                        Action<UIInterectionArgs<bool>> radioAction = (Action<UIInterectionArgs<bool>>)Delegate.CreateDelegate(typeof(Action<UIInterectionArgs<bool>>), BlazorComponent, targetCaller);
                        EventCallback callback = new EventCallback(null, radioAction);
                        callbacks.Add(elem.OnClickAction, callback);
                    }
                }

                if (elem.ElementType != null && (elem.ElementType.Equals("Switch") || elem.ElementType.Equals("TelSwitch")))
                {

                    MethodInfo targetCaller = t.GetMethod(elem.OnClickAction, BindingFlags.NonPublic | BindingFlags.Instance);

                    if (targetCaller != null)
                    {
                        Action<UIInterectionArgs<bool>> radioAction = (Action<UIInterectionArgs<bool>>)Delegate.CreateDelegate(typeof(Action<UIInterectionArgs<bool>>), BlazorComponent, targetCaller);
                        EventCallback callback = new EventCallback(null, radioAction);
                        callbacks.Add(elem.OnClickAction, callback);
                    }
                }
                if (elem.ElementType != null && (elem.ElementType.Equals("DatePicker") || elem.ElementType.Equals("TelDatePicker") || elem.ElementType.Equals("TelDTimePicker")))
                {

                    MethodInfo targetCaller = t
                             .GetMethod(elem.OnClickAction, BindingFlags.NonPublic | BindingFlags.Instance);
                    if (targetCaller != null)
                    {
                        Action<UIInterectionArgs<DateTime?>> datePic = (Action<UIInterectionArgs<DateTime?>>)
                            Delegate.CreateDelegate(typeof(Action<UIInterectionArgs<DateTime?>>), BlazorComponent, targetCaller);
                        EventCallback callback = new EventCallback(null, datePic);
                        callbacks.Add(elem.OnClickAction, callback);
                    }
                }

                if (elem.ElementType != null && elem.ElementType.Equals("TimePicker"))
                {

                    MethodInfo targetCaller = t
                             .GetMethod(elem.OnClickAction, BindingFlags.NonPublic | BindingFlags.Instance);
                    if (targetCaller != null)
                    {
                        Action<UIInterectionArgs<TimeSpan?>> timePic = (Action<UIInterectionArgs<TimeSpan?>>)
                            Delegate.CreateDelegate(typeof(Action<UIInterectionArgs<TimeSpan?>>), BlazorComponent, targetCaller);
                        EventCallback callback = new EventCallback(null, timePic);
                        callbacks.Add(elem.OnClickAction, callback);
                    }
                }

                if (elem.ElementType != null && elem.ElementType.Equals("Grid"))
                {
                    string delegateName = "OnGridInitilize_" + elem._internalElementName;
                    MethodInfo targetCaller = t
                             .GetMethod(delegateName, BindingFlags.NonPublic | BindingFlags.Instance);
                    if (targetCaller != null)
                    {
                        Action<UIInterectionArgs<object?>> grid = (Action<UIInterectionArgs<object>>)
                            Delegate.CreateDelegate(typeof(Action<UIInterectionArgs<object?>>), BlazorComponent, targetCaller);
                        EventCallback callback = new EventCallback(null, grid);
                        callbacks.Add(delegateName, callback);
                    }
                }

                if (elem.ElementType != null && elem.ElementType.Equals("Card"))
                {

                    MethodInfo targetCaller = t
                             .GetMethod(elem.OnClickAction, BindingFlags.NonPublic | BindingFlags.Instance);
                    if (targetCaller != null)
                    {
                        Action<UIInterectionArgs<object>> codeBaseAction = (Action<UIInterectionArgs<object>>)
                            Delegate.CreateDelegate(typeof(Action<UIInterectionArgs<object>>), BlazorComponent, targetCaller);
                        EventCallback callback = new EventCallback(null, codeBaseAction);
                        callbacks.Add(elem.OnClickAction, callback);
                    }
                }
            }
        }

        private void GenerateOnBeforeDataFecth(IDictionary<string, EventCallback> callbacks, Type t, BLUIElement elem)
        {
            if (!string.IsNullOrWhiteSpace(elem.OnBeforeComboLoad))
            {
                if (elem.ElementType != null && (elem.ElementType.Equals("Cmb") || elem.ElementType.Equals("TelCmb")))
                {
                    MethodInfo targetCaller = t
                             .GetMethod(elem.OnBeforeComboLoad, BindingFlags.NonPublic | BindingFlags.Instance);
                    if (targetCaller != null)
                    {
                        Action<UIInterectionArgs<ComboRequestDTO>> codeBaseAction = (Action<UIInterectionArgs<ComboRequestDTO>>)
                            Delegate.CreateDelegate(typeof(Action<UIInterectionArgs<ComboRequestDTO>>), BlazorComponent, targetCaller);
                        EventCallback callback = new EventCallback(null, codeBaseAction);
                        callbacks.Add(elem.OnBeforeComboLoad, callback);
                    }


                }


            }
        }

        private void GenerateOnEnterEvents(IDictionary<string, EventCallback> callbacks, Type t, BLUIElement elem)
        {
            if (!string.IsNullOrWhiteSpace(elem.EnterKeyAction))
            {
                if (elem.ElementType != null && (elem.ElementType.Equals("TextBox") || elem.ElementType.Equals("TextArea")))
                {
                    MethodInfo targetCaller = t
                             .GetMethod(elem.EnterKeyAction, BindingFlags.NonPublic | BindingFlags.Instance);
                    if (targetCaller != null)
                    {
                        Action<UIInterectionArgs<object>> enterAction = (Action<UIInterectionArgs<object>>)
                            Delegate.CreateDelegate(typeof(Action<UIInterectionArgs<object>>), BlazorComponent, targetCaller);
                        EventCallback callback = new EventCallback(null, enterAction);
                        callbacks.Add(elem.EnterKeyAction, callback);
                    }


                }


            }

            if (!string.IsNullOrWhiteSpace(elem.EnterKeyAction))
            {
                if (elem.ElementType != null && elem.ElementType.Equals("NumericBox"))
                {
                    MethodInfo targetCaller = t
                             .GetMethod(elem.EnterKeyAction, BindingFlags.NonPublic | BindingFlags.Instance);
                    if (targetCaller != null)
                    {
                        Action<UIInterectionArgs<decimal>> enterAction = (Action<UIInterectionArgs<decimal>>)
                            Delegate.CreateDelegate(typeof(Action<UIInterectionArgs<decimal>>), BlazorComponent, targetCaller);
                        EventCallback callback = new EventCallback(null, enterAction);
                        if (!callbacks.ContainsKey(elem.EnterKeyAction))
                        {
                            callbacks.Add(elem.EnterKeyAction, callback);
                        }
                    }


                }


            }
        }

        public void GenerateOnAfterDataFecth(IDictionary<string, EventCallback> callbacks, Type t, BLUIElement elem)
        {
            if (!string.IsNullOrWhiteSpace(elem.OnAfterComboLoad))
            {
                if (elem.ElementType != null && elem.ElementType.Equals("Cmb"))
                {
                    MethodInfo targetCaller = t
                             .GetMethod(elem.OnAfterComboLoad, BindingFlags.NonPublic | BindingFlags.Instance);
                    if (targetCaller != null)
                    {
                        if (elem.ElementID != null && elem.ElementID.Equals("CodeBase"))
                        {
                            Action<UIInterectionArgs<IList<CodeBaseResponse>>> codeBaseAction = (Action<UIInterectionArgs<IList<CodeBaseResponse>>>)
                            Delegate.CreateDelegate(typeof(Action<UIInterectionArgs<IList<CodeBaseResponse>>>), BlazorComponent, targetCaller);
                            EventCallback callback = new EventCallback(null, codeBaseAction);

                            callbacks.Add(elem.OnAfterComboLoad, callback);
                        }

                        if (elem.ElementID != null && elem.ElementID.Equals("Item"))
                        {
                            Action<UIInterectionArgs<IList<ItemResponse>>> codeBaseAction = (Action<UIInterectionArgs<IList<ItemResponse>>>)
                            Delegate.CreateDelegate(typeof(Action<UIInterectionArgs<IList<ItemResponse>>>), BlazorComponent, targetCaller);
                            EventCallback callback = new EventCallback(null, codeBaseAction);
                            callbacks.Add(elem.OnAfterComboLoad, callback);
                        }




                    }


                }


            }
        }

        private void GenerateOnAdornmentClickEvents(IDictionary<string, EventCallback> callbacks, Type t, BLUIElement elem)
        {
            if (elem.ElementType != null && !string.IsNullOrEmpty(elem.OnAdornmentAction))
            {
                MethodInfo targetCaller = t.GetMethod(elem.OnAdornmentAction, BindingFlags.NonPublic | BindingFlags.Instance);

                if (targetCaller != null)
                {
                    Action<UIInterectionArgs<object>> clickAction = (Action<UIInterectionArgs<object>>)
                        Delegate.CreateDelegate(typeof(Action<UIInterectionArgs<object>>), BlazorComponent, targetCaller);
                    EventCallback callback = new EventCallback(null, clickAction);
                    callbacks.Add(elem.OnAdornmentAction, callback);
                }
            }
        }

        private void GenerateOnToggledEvents(IDictionary<string, EventCallback> callbacks, Type t, BLUIElement elem)
        {

            if (elem.ElementType != null && !string.IsNullOrEmpty(elem.OnToggledAction))
            {
                MethodInfo targetCaller = t.GetMethod(elem.OnToggledAction, BindingFlags.NonPublic | BindingFlags.Instance);

                if (targetCaller != null)
                {
                    Action<UIInterectionArgs<object>> toggledAction = (Action<UIInterectionArgs<object>>)
                        Delegate.CreateDelegate(typeof(Action<UIInterectionArgs<object>>), BlazorComponent, targetCaller);
                    EventCallback callback = new EventCallback(null, toggledAction);
                    callbacks.Add(elem.OnToggledAction, callback);
                }
            }
        }
    }
}
