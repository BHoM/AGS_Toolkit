/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2025, the respective contributors. All rights reserved.
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
using System;
using BH.oM.Quantities.Attributes;
using System.Text.RegularExpressions;

namespace BH.Adapter.AGS
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static Type Quantity(string unit, string key = "")
        {

            switch (Regex.Replace(unit, @"\s+", "").ToLower())
            {
                // Length
                case "m":
                case "cm":
                case "mm":
                case "ft":
                case "in":
                    return typeof(Length);
                // Density
                case "ug/l":
                case "μg/l":
                case "mg/l":
                case "g/l":
                    return typeof(Density);
                //MassFraction
                case "mg/kg":
                case "ug/kg":
                case "μg/kg":
                case "g/kg":
                case "kg/kg":
                case "mass%":
                    return typeof(MassFraction);
                // Molality
                case "moles/g":
                case "mole/g":
                case "mol/g":
                case "mole/kg":
                case "moles/kg":
                case "mol/kg":
                case "mmol/kg":
                    return typeof(Molality);
                //Volume
                case "l":
                    return typeof(Volume);
                //Mass 
                case "kg":
                    return typeof(Mass);
                // Time
                case "s":
                    return typeof(Time);
                // Temperature
                case "degc":
                    return typeof(Temperature);
                // Electric Conductivity
                case "s/m":
                case "s/cm":
                case "us/cm":
                case "ms/cm":
                    return typeof(ElectricConductivity);
                // Dimensionless
                case "%":
                case "%w/w":
                case "% w/w":
                case "":
                case "-":
                case "--":
                case "---":
                case "ph":
                case "phunit":
                case "phunits":
                case "no":
                case "no units":
                case "none":
                    return null;
                default:
                    Compute.RecordWarning($"Unit \"{unit}\" not recognised, no quantity has been assigned.");
                    return null;

            }
        }

        /***************************************************/

    }
}

