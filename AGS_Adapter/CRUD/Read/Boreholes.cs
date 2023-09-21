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
        private List<Borehole> ReadBoreholes(List<string> ids = null)
        {
            List<string> sectionText = GetSectionText("LOCA");
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

            // Find index for different parameters
            headingIndexes.Add("LOCA_ID", GetHeadingIndex("LOCA_ID", split));
            headingIndexes.Add("LOCA_NATE", GetHeadingIndex("LOCA_NATE", split));
            headingIndexes.Add("LOCA_NATN", GetHeadingIndex("LOCA_NATN", split));
            headingIndexes.Add("LOCA_GL", GetHeadingIndex("LOCA_GL", split));
            headingIndexes.Add("LOCA_ETRV", GetHeadingIndex("LOCA_ETRV", split));
            headingIndexes.Add("LOCA_NTRV", GetHeadingIndex("LOCA_NTRV", split));
            headingIndexes.Add("LOCA_FDEP", GetHeadingIndex("LOCA_FDEP", split));

            // If the National Grid coordinates are not given, try getting the local setting out - top
            headingIndexes.Add("LOCA_LOCX", GetHeadingIndex("LOCA_LOCX", split));
            headingIndexes.Add("LOCA_LOCY", GetHeadingIndex("LOCA_LOCY", split));
            headingIndexes.Add("LOCA_LOCZ", GetHeadingIndex("LOCA_LOCZ", split));

            // If the National Grid coordinates are not given, try getting the local setting out - bottom
            headingIndexes.Add("LOCA_XTRL", GetHeadingIndex("LOCA_XTRL", split));
            headingIndexes.Add("LOCA_YTRL", GetHeadingIndex("LOCA_YTRL", split));
            headingIndexes.Add("LOCA_ZTRL", GetHeadingIndex("LOCA_ZTRL", split));

            List<Borehole> boreholes = new List<Borehole>();

            for (int i = dataIndex; i < sectionText.Count; i++)
            {
                Borehole borehole = Convert.FromBorehole(sectionText[i], headingIndexes);
                if (borehole != null)
                    boreholes.Add(borehole);
            }

            return boreholes;
        }

        /***************************************************/

    }
}

