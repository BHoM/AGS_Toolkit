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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.Engine.Base;
using BH.oM.Ground;

namespace BH.Adapter.AGS
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/
        public static InSituVane FromInSituVane(Dictionary<string, string> data, Dictionary<string, string> units)
        {
            string id = GetString(data, "LOCA_ID");

            double top = GetDouble(data, units, "IVAN_DPTH");

            if (double.IsNaN(top))
            {
                if (id == "")
                    Compute.RecordWarning($"The top (IVAN_DPTH) value for {id} is invalid and has been skipped.");
            }

            double result = GetDouble(data, units, "IVAN_IVAN");

            List<ITestProperties> testProperties = new List<ITestProperties>();

            // InSituVane Reference
            string details = GetString(data, "IVAN_REM");
            string weather = GetString(data, "IVAN_ENV");
            DateTime testDate = GetDateTime(data, units, "IVAN_DATE");
            string stratumReference = GetString(data, "GEOL_STAT");
            string files = GetString(data, "FILE_FSET");

            InSituVaneReferenceProperties InSituVaneReferenceProperties = new InSituVaneReferenceProperties() 
            { 
                Details = details, 
                Weather = weather, Date = testDate, 
                StratumReference = stratumReference, 
                FileReference = files 
            };

            if (InSituVaneReferenceProperties != null)
                testProperties.Add(InSituVaneReferenceProperties);

            // InSituVane Test Properties
            string testReference = GetString(data, "IVAN_TESN");
            string type = GetString(data, "IVAN_TYPE");
            string tester = GetString(data, "IVAN_CONT"); 
            string testMethod = GetString(data, "IVAN_METH");
            string accreditingBody = GetString(data, "IVAN_CRED");
            string testStatus = GetString(data, "TEST_STAT");

            InSituVaneTestProperties InSituVaneTestProperties = new InSituVaneTestProperties()
            {
                Reference = testReference,
                Type = type,
                Method = testMethod,
                Tester = tester,
                AccreditingBody = accreditingBody,
                Status = testStatus
            };
            
            if (InSituVaneTestProperties != null)
                testProperties.Add(InSituVaneTestProperties);

            // InSituVane Result Properties
            double vaneResidualResult = GetDouble(data, units, "IVAN_IVAR");


            InSituVaneResultProperties inSituVaneResultProperties = new InSituVaneResultProperties()
            {
                VaneResidualResult = vaneResidualResult
            };
            if (inSituVaneResultProperties != null)
                testProperties.Add(inSituVaneResultProperties);

            InSituVane inSituVane = new InSituVane
            {
                Id = id,
                Top = top,
                Result=result,
                Properties = testProperties,
            };

            return inSituVane;

        }

        /***************************************************/

    }
}



