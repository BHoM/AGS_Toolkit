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

        private List<ContaminantSample> ReadContaminantSamples(List<string> ids = null)
        {
            List<string> sectionText = GetSectionText("ERES");
            List<string> unit = new List<string>();
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
                    else if (group.Contains("\"UNIT\""))
                        unit.AddRange(group.Replace("\"", "").Split(','));
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
            Dictionary<string, string> units = new Dictionary<string, string>();

            List<string> parameterHeadings = new List<string>()
            {
                "LOCA_ID","SAMP_TOP","SAMP_REF","SAMP_TYPE","SAMP_ID","SPEC_REF","SPEC_DPTH","ERES_CODE","ERES_METH","ERES_MATX","ERES_RTYP","ERES_TESN","ERES_NAME",
                "ERES_TNAM","ERES_RVAL","ERES_RTCD","ERES_RRES","ERES_DETF","ERES_ORG","ERES_RDLM","ERES_MDLM","ERES_QLM",
                "ERES_DUNI","ERES_TICP","ERES_TICT","ERES_RDAT","ERES_SGRP","SPEC_PREP","SPEC_DESC","ERES_DTIM","ERES_TEST","ERES_TORD","ERES_LOCN","ERES_BAS","ERES_DIL",
                "ERES_LMTH","ERES_LDTM","ERES_IREF","ERES_SIZE","ERES_PERP","ERES_REM","ERES_LAB","ERES_CRED","TEST_STAT","FILE_FSET"
            };

            foreach (string parameterHeading in parameterHeadings)
            {
                int index = GetHeadingIndex(parameterHeading, split);
                headingIndexes.Add(parameterHeading, index);
                units.Add(parameterHeading, unit[index]);
            }

            List<ContaminantSample> contaminantSamples = new List<ContaminantSample>();

            for (int i = dataIndex; i < sectionText.Count; i++)
            {
                ContaminantSample contaminantSample = Convert.FromContaminantSample(sectionText[i], headingIndexes, units);
                if (contaminantSample != null)
                    contaminantSamples.Add(contaminantSample);
            }

            return contaminantSamples;
        }

        /***************************************************/

    }
}

