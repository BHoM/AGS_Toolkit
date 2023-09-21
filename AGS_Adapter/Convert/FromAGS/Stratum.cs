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

using BH.oM.Adapters.AGS;
using BH.oM.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.Engine.Geometry;
using BH.oM.Geometry;
using BH.oM.Ground;

namespace BH.Adapter.AGS
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/
        public static Stratum FromStratum(string text, Dictionary<string, int> headings, string blankGeology)
        {
            string id = GetValue(text, "LOCA_ID", headings);
            string top = GetValue(text, "GEOL_TOP", headings);
            string bottom = GetValue(text, "GEOL_BASE", headings);

            if (top == "" || bottom == "")
            {
                if (id == "")
                    Engine.Base.Compute.RecordWarning($"The top (GEOL_TOP) and bottom (GEOL_BASE) value for a strata is not valid as well as the id (LOCA_ID) and has been skipped.");
                else
                    Engine.Base.Compute.RecordWarning($"The top (GEOL_TOP) or bottom (GEOL_BASE) value for {id} is invalid and has been skipped.");
                return null;
            }

            string observedGeology = GetValue(text, "GEOL_GEOL", headings);
            if (observedGeology == "")
                observedGeology = blankGeology;

            string interpretedGeology = GetValue(text, "GEOL_GEO2", headings);
            string legend = GetValue(text, "GEOL_LEG", headings);
            if(legend == "")
                Engine.Base.Compute.RecordWarning($"No legend code provided for {id}.");
            string description = GetValue(text, "GEOL_DESC", headings);


            string strataRef = GetValue(text, "GEOL_STAT", headings);
            string lexiconCode = GetValue(text, "GEOL_BGS", headings);
            string references = GetValue(text, "FILE_FSET", headings);
            string remarks = GetValue(text, "GEOL_REM", headings);

            StratumReference reference = new StratumReference()
            {
                Remarks = remarks,
                LexiconCode = lexiconCode,
                Files = references
            };

            Stratum strata = new Stratum()
            {
                Id = id,
                Top = double.Parse(top),
                Bottom = double.Parse(bottom),
                Legend = legend,
                LogDescription = description,
                ObservedGeology = observedGeology,
                InterpretedGeology = interpretedGeology,
                Properties = new List<IStratumProperty>() { reference }
            };

            strata.Name = strataRef;

            return strata;

        }

        /***************************************************/

    }
}

