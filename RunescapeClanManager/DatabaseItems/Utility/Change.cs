using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using System;

namespace RunescapeClanManager.DatabaseItems.Utility
{
    public static class Change
    {
        public class ChangeResult
        {
            public bool isSucces;
            public string error;
            public object obj;
            public object value;
            public PropertyInfo property;

            public static ChangeResult Succes(object obj, object value, PropertyInfo property)
            {
                return new ChangeResult() { obj = obj, isSucces = true, value = value, property = property };
            }
            public static ChangeResult Error(object obj, object value, PropertyInfo property, string error)
            {
                return new ChangeResult() { obj = obj, isSucces = true, value = value, property = property, error = error };
            }
        }

        /// <summary>
        /// Performs a change in variables on obj
        /// </summary>
        /// <typeparam name="t">Type of object to perform change on</typeparam>
        /// <param name="obj">The object to perform the change on</param>
        /// <param name="changesStr">The list of changes, in the format "NameOfVariable:NewValue NextVariable:Value"</param>
        /// <returns>A string containing information about the changes made, errors or succesfull changes</returns>
        public static List<ChangeResult> PerformChange<t>(t obj, string changesStr)
        {
            List<ChangeResult> returnVal = new List<ChangeResult>();

            List<string> names = new List<string>(), values = new List<string>();
            for (int i = changesStr.IndexOf(':'); i != -1; i = changesStr.IndexOf(':', i+1))
            {
                if (changesStr[i + 1] == '\"')
                {
                    values.Add(changesStr.Substring(i+2, changesStr.IndexOf('"', i + 2) - i-2));
                }
                else
                {
                    values.Add(changesStr.Substring(i+1, changesStr.IndexOf(' ', i) -i-1));
                }
                int nameStart = changesStr.Substring(0, i).LastIndexOf(' ');
                nameStart = (nameStart == -1 && names.Count != 0) ? 0 : nameStart;
                names.Add(changesStr.Substring(nameStart+1, i - nameStart-1));
            }

            PropertyInfo[] properties = typeof(t).GetProperties();
            
            for (int b = 0; b < names.Count(); b++)
            {
                bool foundProperty = false;

                for (int a = 0; a < properties.Count(); a++)
                {
                    if (properties[a].Name.ToLower() == names[b].ToLower())
                    {
                        returnVal.Add(PerformSingleChange(properties[a], values[b], obj));
                        foundProperty = true;
                        break;
                    }
                }
                if (!foundProperty)
                {
                    returnVal.Add(ChangeResult.Error(obj, values[b], null, "Variable not found"));
                }
            }

            return returnVal;
        }

        /// <summary>
        /// Perform a single change
        /// </summary>
        /// <param name="property">Property to change</param>
        /// <param name="value">Value to set the property to</param>
        /// <param name="obj">The object to perform the change on</param>
        /// <returns>A string with either errors or succes messages.</returns>
        private static ChangeResult PerformSingleChange(PropertyInfo property, string value, object obj)
        {
            object val = null;
            if (property.PropertyType == typeof(int))
            {
                int v;
                if (int.TryParse(value, out v))
                {
                    val = v;
                }
            }
            else if (property.PropertyType == typeof(double))
            {
                double v;
                if (double.TryParse(value, out v))
                {
                    val = v;
                }
            }
            else if (property.PropertyType == typeof(float))
            {
                float v;
                if (float.TryParse(value, out v))
                {
                    val = v;
                }
            }
            else if (property.PropertyType == typeof(string))
            {
                val = value;
            }
            else
            {
                Console.WriteLine($"To Change.cs you gotta add the type {value.GetType()}. To test this create a {obj} {property.Name}:\"{value}\n");
                return ChangeResult.Error(obj, value, property, "Type not found, author of this bot have been notified");
            }

            if (val == null)
            {
                return ChangeResult.Error(obj, value, property, "Invalid value");
            }

            property.SetValue(obj, val);
            return ChangeResult.Succes(obj, value, property);
        }
    }
}
