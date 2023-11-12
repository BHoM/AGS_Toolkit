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

        public static Borehole FromBorehole(string text, Dictionary<string,int> headings, Dictionary<string, string> units, IEnumerable<Stratum> strata, 
            IEnumerable<ContaminantSample> contaminantSamples)
        {
            string id = GetValue<string>(text, "LOCA_ID", headings,units);
            double eastingTop = GetValue<double>(text, "LOCA_NATE", headings,units);
            double northingTop = GetValue<double>(text, "LOCA_NATN", headings,units);
            double topLevel = GetValue<double>(text, "LOCA_GL", headings,units);
            double eastingBot = GetValue<double>(text, "LOCA_ETRV", headings,units);
            double northingBot = GetValue<double>(text, "LOCA_NTRV", headings,units);
            double botDepth = GetValue<double>(text, "LOCA_FDEP", headings,units);

            double eastingTopLocal;
            double northingTopLocal;

            if(double.IsNaN(eastingTop) || double.IsNaN(northingTop))
            {
                eastingTopLocal = GetValue<double>(text, "LOCA_LOCX", headings,units);
                northingTopLocal = GetValue<double>(text, "LOCA_LOCY", headings,units);
                if (double.IsNaN(eastingTopLocal)  && double.IsNaN(northingTopLocal))
                {
                    Engine.Base.Compute.RecordError("No valid coordinates are found for the top of the borehole.");
                    return null;
                }
                else if (double.IsNaN(eastingBot) || double.IsNaN(northingBot))
                {
                    double eastingBotLocal = GetValue<double>(text, "LOCA_XTRL", headings, units);
                    double northingBotLocal = GetValue<double>(text, "LOCA_YTRL", headings, units);
                    if (double.IsNaN(eastingBotLocal) && double.IsNaN(northingBotLocal))
                    {
                        Engine.Base.Compute.RecordWarning("No valid coordinates are found for the bottom of the borehole. Therefore, the borehole is assumed straight.");
                        eastingBot = double.IsNaN(eastingTop) ? eastingTopLocal : eastingTop;
                        northingBot = double.IsNaN(northingTop) ? northingTopLocal : northingTop;
                    }
                }
            }

            Point top = new Point()
            {
                X = eastingTop,
                Y = northingTop,
                Z = topLevel
            };

            Point bottom = new Point()
            {
                X = eastingBot,
                Y = northingBot,
                Z = top.Z - botDepth
            };

            List<Stratum> boreholeStrata = strata.Where(x => x.Id == id).ToList();
            List<ContaminantSample> boreholeContaminants = contaminantSamples.Where(x => x.Id == id).ToList();

            List<IBoreholeProperty> boreholeProperties = new List<IBoreholeProperty>();

            // Methodology
            string type = GetValue<string>(text, "LOCA_TYPE", headings,units);
            string status = GetValue<string>(text, "LOCA_STAT", headings,units);
            string remarks = GetValue<string>(text, "LOCA_REM", headings,units);
            string purpose = GetValue<string>(text, "LOCA_PURP", headings,units);
            string termination = GetValue<string>(text, "LOCA_TERM", headings,units);

            Methodology methodology = Engine.Ground.Create.Methodology(type, status, remarks, purpose, termination);
            if (methodology != null)
                boreholeProperties.Add(methodology);

            // Location
            string method = GetValue<string>(text, "LOCA_LOCM", headings,units);
            string subDivision = GetValue<string>(text, "LOCA_LOCA", headings,units);
            string phase = GetValue<string>(text, "LOCA_CLST", headings,units);
            string alignment = GetValue<string>(text, "LOCA_ALID", headings,units);
            double offset = GetValue<double>(text, "LOCA_OFFS", headings,units);
            string chainage = GetValue<string>(text, "LOCA_CNGE", headings,units);
            string algorithim = GetValue<string>(text, "LOCA_TRAN", headings,units);

            Location location = Engine.Ground.Create.Location(method, subDivision, phase, alignment, offset, chainage, algorithim);
            if (location != null)
                boreholeProperties.Add(location);

            // BoreholeReference
            DateTime startDate = GetValue<DateTime>(text, "LOCA_STAR", headings,units);
            DateTime endDate = GetValue<DateTime>(text, "LOCA_ENDD", headings,units);

            string file = GetValue<string>(text, "FILE_FSET", headings,units);
            string originalId = GetValue<string>(text, "LOCA_ORID", headings,units);
            string originalReference = GetValue<string>(text, "LOCA_ORJO", headings,units);
            string originalCompany = GetValue<string>(text, "LOCA_ORCO", headings,units);

            BoreholeReference boreholeReference = Engine.Ground.Create.BoreholeReference(startDate, endDate, file, originalId, originalReference, originalCompany);
            if (boreholeReference != null)
                boreholeProperties.Add(boreholeReference);

            Borehole borehole = Engine.Ground.Create.Borehole(id, top, bottom, null, boreholeProperties, boreholeStrata, boreholeContaminants);

            return borehole;

        }

        /***************************************************/

    }
}

