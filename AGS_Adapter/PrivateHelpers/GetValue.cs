/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2023, the respective contributors. All rights reserved.
 *
 * Each contributor holds copyright over their respective contributions.
 * The project versioning (Git) records all such contribution source information.
 *                                           
 *                                                                              
 * The BHoM is free software: you can redistribute it and/or modify         
 * it under the terms of the GNU Lesser General Public License as published by  
 * the Free Software Foundation, either version 3.0 of the License, or          
 * (at your option) any later version.                                          
 *                                                                              
 * The BHoM is distributed in the hope that it will be useful,              
 * but WITHOUT ANY WARRANTY; without even the implied warranty of               
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the                 
 * GNU Lesser General Public License for more details.                          
 *                                                                            
 * You should have received a copy of the GNU Lesser General Public License     
 * along with this code. If not, see <https://www.gnu.org/licenses/lgpl-3.0.html>.      
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BH.Adapter.AGS
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private static dynamic GetValue<T>(string text, string heading, Dictionary<string, int> headings, Dictionary<string, string> units)
        {
            if (text == "")
                return "";

            int index = -1;
            string value = "";

            // Split using "," because commas can be used in the remarks
            if (headings.TryGetValue(heading, out index))
            {
                if (index == -1)
                {
                    string section = heading.Split('_')[0];
                    Engine.Base.Compute.RecordError($"The heading {heading} was not present in the section {section}");
                }
                else
                {
                    value = text.Split(new string[] { "\",\"" }, StringSplitOptions.None)[index].Replace("\"", "").Trim();

                }    
            }
            else
            {
                string section = heading.Split('_')[0];
                Engine.Base.Compute.RecordError($"The heading {heading} was not present in the section {section}");
            }

            if (typeof(T) == typeof(string))
            {
                return value;
            }
            else if (typeof(T) == typeof(double))
            {
                double number;
                if (!double.TryParse(value, out number))
                    number = double.NaN;
                return number;
            }
            else if(typeof(T) == typeof(DateTime))
            {
                DateTime date;
                if (!DateTime.TryParse(value, out date))
                    date = default(DateTime);
                return date;
            }
            else if(typeof(T) == typeof(bool))
            {
                bool boolean = ParseYNString(value);
                return boolean;
            }
            else
            {
                return value as object;
            }
        }

        /***************************************************/

    }
}



