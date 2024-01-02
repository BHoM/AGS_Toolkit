/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2024, the respective contributors. All rights reserved.
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.oM.Ground;

namespace BH.Adapter.AGS
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/
        public static Stratum FromStratum(Dictionary<string, string> data, Dictionary<string, string> units, string blankGeology)
        {
            string id = GetString(data, "LOCA_ID");

            double top = GetDouble(data, units, "GEOL_TOP");
            double bottom = GetDouble(data, units, "GEOL_BASE");

            if (double.IsNaN(top) || double.IsNaN(bottom))
            {
                if (id == "")
                    Engine.Base.Compute.RecordWarning($"The top (GEOL_TOP) and bottom (GEOL_BASE) value for a strata is not valid as well as the id (LOCA_ID) and has been skipped.");
                else
                    Engine.Base.Compute.RecordWarning($"The top (GEOL_TOP) or bottom (GEOL_BASE) value for {id} is invalid and has been skipped.");
                return null;
            }

            string observedGeology = GetString(data, "GEOL_GEOL");
            if (observedGeology == "")
                observedGeology = blankGeology;

            string interpretedGeology = GetString(data, "GEOL_GEO2");
            string legend = GetString(data, "GEOL_LEG");
            if (legend == "")
                Engine.Base.Compute.RecordWarning($"No legend code provided for {id}.");
            string description = GetString(data, "GEOL_DESC");

            List<IStratumProperty> stratumProperties = new List<IStratumProperty>();

            string strataRef = GetString(data, "GEOL_STAT");
            string lexiconCode = GetString(data, "GEOL_STAT");
            string references = GetString(data, "FILE_FSET");
            string remarks = GetString(data, "GEOL_REM");

            StratumReference reference = new StratumReference() { Remarks = remarks, LexiconCode = lexiconCode, Name = strataRef, Files = references };
            if (reference != null)
                stratumProperties.Add(reference);

            Stratum strata = Engine.Ground.Create.Stratum(id, top, bottom, description, legend, observedGeology, interpretedGeology, "", blankGeology, stratumProperties);

            strata.Name = strataRef;

            return strata;

        }

        /***************************************************/

    }
}


