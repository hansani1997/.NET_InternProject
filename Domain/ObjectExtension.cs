using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BlueLotus360.CleanArchitecture.Domain
{

    public class PropertyConversionResponse<T>
    {
        public bool IsConversionSuccess { get; set; }
        public T Value { get; set; }

        public string Message { get; set; }
    }
    public static class ObjectExtension
    {

        public static T GetPropertyValue<T>(this object value)
        {
            T convertedValue = default(T);

            return convertedValue;
        }

        public static PropertyConversionResponse<T> GetPropObject<T>(this object obj, string name)
        {
            PropertyConversionResponse<T> conversionResponse = new PropertyConversionResponse<T>();
            conversionResponse.IsConversionSuccess = false;
            if (name == null || name.Trim().Length < 2)
            {
                conversionResponse.Message = "Default Path is Null Or Empty";
                return conversionResponse;

            }
            foreach (String part in name.Split('.'))
            {
                if (obj == null) { return null; }

                Type type = obj.GetType();
                PropertyInfo info = type.GetProperty(part);
                if (info == null)
                {
                    conversionResponse.Message = "No Valid Object found for the Path " + name;
                    return conversionResponse;
                }

                obj = info.GetValue(obj, null);
            }


            if (obj.Is<T>())
            {
                try
                {
                    conversionResponse.Value = (T)obj;
                    conversionResponse.IsConversionSuccess = true;

                }

                catch (Exception exp)
                {
                    conversionResponse.IsConversionSuccess = false;
                }
            }
            else
            {
                conversionResponse.Message = "Path " + name + " Is Not Convertible to " + typeof(T).Name;
            }
            return conversionResponse;


        }

        public static bool Is<T>(this object input)
        {
            try
            {
                if (input == null)
                {
                    return false;
                }
                if (typeof(T).IsGenericType && (typeof(T).GetGenericTypeDefinition() == typeof(List<>) || typeof(T).GetGenericTypeDefinition() == typeof(IList<>)))
                {
                    Type itemType = typeof(T).GetGenericArguments()[0];
                    Type t = input.GetType().GetGenericArguments()[0];
                    return input.GetType().IsGenericType && (
                        input.GetType().GetGenericTypeDefinition() == typeof(List<>) ||
                        input.GetType().GetGenericTypeDefinition() == typeof(IList<>) ||
                        input.GetType().GetGenericTypeDefinition() == typeof(IEnumerable<>) ||
                        input.GetType().GetGenericTypeDefinition() == typeof(ICollection<>)
                    );
                }
                return (typeof(T) == input.GetType()) || input.IsNumericType();
            }
            catch (Exception ex)
            {
                return false;
            }


        }

        public static T GetPropertyOject<T>(this object value)
        {
            T convertedValue = default(T);

            return convertedValue;
        }

        public static string ConvertDecimalToScientificNotation(this object val)
        {
            string deci_not = "";
            if (val!=null)
            {
                deci_not = Convert.ToDecimal(val).ToString();
            }
             
            return !string.IsNullOrEmpty(deci_not) ? string.Format("{0:E2}", deci_not):"0";
        }

        public static bool IsNumericType(this object o)
        {

            switch (Type.GetTypeCode(o.GetType()))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                default:
                    return false;
            }
        }

        public static Type GetObjectType(this object obj, string name)
        {
            if (name == null || name.Trim().Length < 2)
            {


                return typeof(void);


            }
            foreach (String part in name.Split('.'))
            {
                if (obj == null) { return typeof(void); }

                Type type = obj.GetType();
                PropertyInfo info = type.GetProperty(part);
                if (info == null)
                {

                    return typeof(void);
                }

                obj = info.GetValue(obj, null);
            }


            if (obj == null)
            {
                return null;
            }
            return obj.GetType();



        }

        public static PropertyConversionResponse<IList<T>> GetPropListObject<T>(this object obj, string name)
        {
            PropertyConversionResponse<IList<T>> conversionResponse = new PropertyConversionResponse<IList<T>>();
            conversionResponse.IsConversionSuccess = false;
            if (name == null || name.Trim().Length < 2)
            {
                conversionResponse.Message = "Default Path is Null Or Empty";
                return conversionResponse;
            }
            string[] parts = name.Split('.');
            foreach (String part in parts)
            {
                if (obj == null) { return null; }

                Type type = obj.GetType();
                PropertyInfo info = type.GetProperty(part);
                if (info == null)
                {
                    conversionResponse.Message = "No Valid Object found for the Path " + name;
                    return conversionResponse;
                }
                if (info.PropertyType.IsGenericType && info.PropertyType.GetGenericTypeDefinition() == typeof(IList<>))
                {
                    IList list = info.GetValue(obj, null) as IList;
                    if (list != null && list.Count > 0)
                    {
                        Type itemType = info.PropertyType.GetGenericArguments()[0];
                        IList<object> convertedList = new List<object>();
                        foreach (object item in list)
                        {
                            //object convertedItem = GetPropObject<object>(item, part).Value;
                            convertedList.Add(item);
                        }
                        obj = Activator.CreateInstance(typeof(List<object>), convertedList.ToList());
                    }
                    else
                    {
                        conversionResponse.Message = "List is null or empty for the Path " + name;
                        return conversionResponse;
                    }
                }
                else
                {
                    obj = info.GetValue(obj, null);
                }


            }
            if (obj.Is<IList<T>>())
            {
                try
                {
                    conversionResponse.Value = ((IList)obj).Cast<T>().ToList();
                    conversionResponse.IsConversionSuccess = true;
                }
                catch (Exception exp)
                {
                    conversionResponse.IsConversionSuccess = false;
                }
            }
            else
            {
                conversionResponse.Message = "Path " + name + " Is Not Convertible to " + typeof(T).Name;
            }
            return conversionResponse;
        }

        public static void SetValueByObjectPath(this object obj, string DefaultAccessPath, object value)
        {
            if (DefaultAccessPath == null || value == null) { return; }

            string[] accessArray=DefaultAccessPath.Split('.');
             

            if(accessArray.Length== 1)
            {
                PropertyInfo prop = obj.GetType().GetProperty(accessArray[0], BindingFlags.Public | BindingFlags.Instance);// get all public instances properties of this obj type
                if (prop != null && prop.CanWrite)//CanWrite - Gets a value indicating whether the property can be written to.
                {
                    prop.SetValue(obj, value, null);
                    
                    //When overridden in a derived class, sets the property value for the given object to the given value.
                    
                }
                return;
            }
            object current=obj;
            for(int i = 0; i < accessArray.Length; i++)
            {
                if (i == accessArray.Length - 1)
                {
                    PropertyInfo prop = current.GetType().GetProperty(accessArray[i], BindingFlags.Public | BindingFlags.Instance);
                    if (prop != null && prop.CanWrite)//CanWrite - Gets a value indicating whether the property can be written to.
                    {
                        prop.SetValue(current, value, null);

                        //When overridden in a derived class, sets the property value for the given object to the given value.

                    }

                }
                else
                {
                    PropertyInfo prop = current.GetType().GetProperty(accessArray[i], BindingFlags.Public | BindingFlags.Instance);
                    if(prop != null && prop.CanRead){
                        current = prop.GetValue(current, null);

                    }
                    else
                    {
                        return;
                    }
                }
            }

            

        }

        public static void SetListValueByObjectPath<T>(this object obj, string DefaultAccessPath, object value)
        {
            if (DefaultAccessPath == null || value == null) { return; }

            string[] accessArray = DefaultAccessPath.Split('.');

            //if (accessArray.Length == 1)
            //{
            //    if (obj is T)
            //    {
            //        PropertyInfo prop = obj.GetType().GetProperty(accessArray[0], BindingFlags.Public | BindingFlags.Instance);
            //        if (prop != null && prop.CanWrite)
            //        {
            //            prop.SetValue(obj, value, null);
            //        }
            //    }
            //    else if (obj is IList<T>)
            //    {
            //        foreach (T item in (IList<T>)obj)
            //        {
            //            PropertyInfo prop = item.GetType().GetProperty(accessArray[0], BindingFlags.Public | BindingFlags.Instance);
            //            if (prop != null && prop.CanWrite)
            //            {
            //                prop.SetValue(item, value, null);
            //            }
            //        }
            //    }
            //    return;
            //}

            object current = obj;
            for (int i = 0; i < accessArray.Length; i++)
            {
                if (i == accessArray.Length - 1)
                {
                    PropertyInfo prop = current.GetType().GetProperty(accessArray[i], BindingFlags.Public | BindingFlags.Instance);
                    if (prop != null && prop.CanWrite)
                    {
                        prop.SetValue(current, value, null);
                    }
                }
                else
                {
                    PropertyInfo prop = current.GetType().GetProperty(accessArray[i], BindingFlags.Public | BindingFlags.Instance);
                    if (prop != null && prop.CanRead)
                    {
                        current = prop.GetValue(current, null);
                    }
                    else
                    {
                        return;
                    }
                }
            }
        }
        public static string GetGridProperty(this Object obj, string name)
        {
            string property = "";
            if (name == null || name.Trim().Length < 2)
            {
                property = String.Empty;

            }

            Type type = obj.GetType();

            foreach (string part in name.Split('.'))
            {
                if (obj == null) { property = String.Empty; }


                PropertyInfo info = type.GetProperty(part);
                property = info?.Name;

                type = info?.PropertyType;
                obj = info?.GetValue(obj, null);
            }
            return property;
        }

        public static string GetGridPropertyValue(this Object obj, string name)
        {
            string column = "";
            if (name == null || name.Trim().Length < 2)
            {
                column = String.Empty;
            }
            else
            {
                Type type = obj.GetType();

                foreach (string part in name.Split('.'))
                {
                    if (obj == null) { column = String.Empty; }


                    PropertyInfo info = type.GetProperty(part);
                    object val = info?.GetValue(obj, null);

                    if (val != null)
                    {
                        if (info == null)
                        {
                            column = String.Empty;
                        }
                        else if (val.IsNumericType())
                        {
                            column = decimal.Parse(val.ToString()).ToString("N2");
                        }
                        else
                        {
                            column = val.ToString();
                        }
                    }



                    type = info?.PropertyType;
                    obj = info?.GetValue(obj, null);
                }
            }
            
            return column;
        }

        public static bool HasNullProperty(this Object obj)
        {
            PropertyInfo[] properties =obj.GetType().GetProperties();
            foreach (PropertyInfo prop in properties)
            {
                if (prop.GetValue(obj)==null)
                {
                    return true;
                }
                
            }
            return false;
        }
    }

    public static class StringExt
    {
        public static string? Truncate(this string? value, int maxLength, string truncationSuffix = "…")
        {
            return value?.Length > maxLength
                ? value.Substring(0, maxLength) + truncationSuffix
                : value;
        }
    }

    public static class DateExtensions
    {
       public static bool  IsDefaultDate(this DateTime value)
        {
            return value.Year == 1901 && value.Month == 1 && value.Day == 1;
        }
    }

    public static class ByteExtensions
    {
        public static double ToMB(this long byteValue)
        {
            return Math.Round((double)byteValue / 1048576, 2);
        }
    }
}
