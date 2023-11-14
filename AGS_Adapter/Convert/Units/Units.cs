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

using BH.oM.Geometry;
using BH.Engine.Base;
using BH.Engine.Units;
using System.Collections.Generic;
using System.Linq;

namespace BH.Adapter.AGS
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static double Units(double value, string heading, Dictionary<string,string> units)
        {
            if (double.IsNaN(value))
                return value;

            string unit = "";
            units.TryGetValue(heading, out unit);

            if(heading != "")
            {
                switch (unit)
                {
                    case "m":
                    case "M":
                        return value;
                    case "CM":
                    case "cm":
                        return value.FromCentimetre();
                    case "MM":
                    case "mm":
                        return value.FromMillimetre();
                    case "ft":
                    case "FT":
                        return value.FromFoot();
                    case "in":
                    case "IN":
                        return value.FromInch();
                    case "%":
                    default:
                        Compute.RecordWarning($"Unit {unit} not recognised, no unit conversion has occured for {heading}.");
                        return value;
                }
            }
            else
            {
                Compute.RecordWarning("No unit detected, Therefore no unit conversion will occur.");
                return value;
            }
        }

        /***************************************************/

    }
}



