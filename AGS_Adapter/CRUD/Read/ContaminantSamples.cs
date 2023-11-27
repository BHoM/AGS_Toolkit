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

using BH.Engine.Base;
using BH.oM.Adapter;
using BH.oM.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.oM.Ground;

namespace BH.Adapter.AGS
{
    public partial class AGSAdapter
    {
        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private List<ContaminantSample> ReadContaminantSamples(List<string> ids = null)
        {
            string groupKey = "ERES";

            if (!m_Data.ContainsKey(groupKey))
            {
                Compute.RecordError($"No data regarding boreholes was found in the file ({groupKey} group).");
                return new List<ContaminantSample>();
            }

            if (!m_Units.ContainsKey(groupKey))
            {
                Compute.RecordError($"No units regarding boreholes was found in the file ({groupKey} group).");
                return new List<ContaminantSample>();
            }

            return m_Data[groupKey].Select(data => Convert.FromContaminantSample(data, m_Units[groupKey])).ToList();
        }

        /***************************************************/

    }
}

