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
using System.Globalization;
using System.IO;
using System.Linq;
using BH.oM.Quantities.Attributes;

namespace BH.Adapter.AGS
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private static T GetValue<T>(string text, string format)
        {
            object value = text;

            if (typeof(T) == typeof(double))
            {
                double number;
                if (!double.TryParse(text, out number))
                    number = double.NaN;

                value = number;
            }
            else if (typeof(T) == typeof(int))
            {
                int number;
                if (!int.TryParse(text, out number))
                    number = 0;

                value = number;
            }
            else if (typeof(T) == typeof(DateTime))
            {
                DateTime date;
                if (format == "")
                {
                    DateTime.TryParse(text, out date);
                    date = default(DateTime);
                }
                else
                {
                    if (!DateTime.TryParseExact(text, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                        date = default(DateTime);
                }

                value = date;
            }
            else if (typeof(T) == typeof(bool))
            {
                value = text == "Y" || text.ToLower() == "yes"; // This seems simpler that calling a method that is 
            }

            return (T)value;
        }

        /***************************************************/

    }
}



