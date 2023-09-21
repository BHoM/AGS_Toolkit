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

        // Add methods for converting to BHoM from the specific software types. 
        // Example:
        public static Borehole FromBorehole(string text, Dictionary<string,int> headings)
        {
            string id = GetValue(text, "LOCA_ID", headings);
            string eastingTop = GetValue(text, "LOCA_NATE", headings);
            string northingTop = GetValue(text, "LOCA_NATN", headings);
            string topLevel = GetValue(text, "LOCA_GL", headings);
            string eastingBot = GetValue(text, "LOCA_ETRV", headings);
            string northingBot = GetValue(text, "LOCA_NTRV", headings);
            string botLevel = GetValue(text, "LOCA_FDEP", headings);

            string eastingTopLocal = "";
            string northingTopLocal = "";

            if(eastingTop == "" || northingTop == "")
            {
                eastingTopLocal = GetValue(text, "LOCA_LOCX", headings);
                northingTopLocal = GetValue(text, "LOCA_LOCY", headings);
                if (eastingTopLocal == "" && northingTopLocal == "")
                {
                    Engine.Base.Compute.RecordError("No valid coordinates are found for the top of the borehole.");
                    return null;
                }
            }

            if (eastingBot == "" || northingBot == "")
            {
                string eastingBotLocal = GetValue(text, "LOCA_XTRL", headings);
                string northingBotLocal = GetValue(text, "LOCA_YTRL", headings);
                if (eastingBotLocal == "" && northingBotLocal == "")
                {
                    Engine.Base.Compute.RecordWarning("No valid coordinates are found for the bottom of the borehole. Therefore, the borehole is assumed straight.");
                    eastingBot = eastingTop == "" ? eastingTopLocal : eastingTop;
                    northingBot = northingTop == "" ? northingTopLocal : northingTop;
                }
            }

            Point top = new Point()
            {
                X = double.Parse(eastingTop),
                Y = double.Parse(northingTop),
                Z = double.Parse(topLevel)
            };

            Point bottom = new Point()
            {
                X = double.Parse(eastingBot),
                Y = double.Parse(northingBot),
                Z = double.Parse(botLevel)
            };

            Borehole borehole = new Borehole()
            {
                Id = id,
                Top = top,
                Bottom = bottom
            };

            return borehole;

        }

        /***************************************************/

    }
}

