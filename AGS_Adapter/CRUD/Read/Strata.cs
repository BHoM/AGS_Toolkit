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
        private List<Stratum> ReadStrata(List<string> ids = null)
        {
            List<string> sectionText = GetSectionText("GEOL");
            string heading = "";
            int dataIndex = -1;

            // Determine where the section starts
            for (int i = 0; i < sectionText.Count; i++)
            {
                string line = sectionText[i];
                if (!(line.Length < 5))
                {
                    string group = line.Split(',')[0];
                    if (group.Contains("\"HEADING\""))
                    {
                        heading = sectionText[i].Replace("\"", "");
                    }
                    else if (group.Contains("\"DATA\""))
                    {
                        dataIndex = i;
                    }

                    if (dataIndex != -1 && heading != "")
                        break;
                }
            }

            if (heading == "")
            {
                Engine.Base.Compute.RecordError("The HEADING header is not present in the text file.");
                return null;
            }
            if (dataIndex == -1)
            {
                Engine.Base.Compute.RecordError("The DATA header is not present in the text file.");
                return null;
            }

            List<string> split = heading.Split(',').ToList();

            Dictionary<string, int> headingIndexes = new Dictionary<string, int>();

            List<string> parameterHeadings = new List<string>()
            {
                "GEOL_TOP", "GEOL_BASE", "GEOL_DESC", "GEOL_LEG","GEOL_GEOL", "GEOL_GEO2", "GEOL_GEO3",
                "GEOL_STAT", "GEOL_BGS", "GEOL_FSET", "GEOL_REF"
            };

            foreach (string parameterHeading in parameterHeadings)
                headingIndexes.Add(parameterHeading, GetHeadingIndex(parameterHeading, split));

            List<Stratum> strata = new List<Stratum>();

            for (int i = dataIndex; i < sectionText.Count; i++)
            {
                strata.Add(Convert.FromStratum(sectionText[i], headingIndexes));
            }

            return strata;
        }

        /***************************************************/

    }
}

