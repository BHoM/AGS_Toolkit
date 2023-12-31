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
using BH.oM.Quantities.Attributes;

namespace BH.Adapter.AGS
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static Borehole FromBorehole(Dictionary<string, string> data, Dictionary<string, string> units, List<Stratum> strata,
            List<ContaminantSample> contaminantSamples)
        {
            string id = GetString(data, "LOCA_ID");

            if (id == "")
                Engine.Base.Compute.RecordWarning("No valid id found for the Borehole.");

            double eastingTop = GetDouble(data, units, "LOCA_NATE");
            double northingTop = GetDouble(data, units, "LOCA_NATN");
            double topLevel = GetDouble(data, units, "LOCA_GL");
            double eastingBot = GetDouble(data, units, "LOCA_ETRV");
            double northingBot = GetDouble(data, units, "LOCA_NTRV");
            double botDepth = GetDouble(data, units, "LOCA_FDEP");

            double eastingTopLocal = double.NaN;
            double northingTopLocal = double.NaN;

            if (double.IsNaN(eastingTop) || double.IsNaN(northingTop))
            {
                eastingTopLocal = GetDouble(data, units, "LOCA_LOCX");
                northingTopLocal = GetDouble(data, units, "LOCA_LOCY");
                if (double.IsNaN(eastingTopLocal) && double.IsNaN(northingTopLocal))
                {
                    Engine.Base.Compute.RecordWarning($"No valid coordinates are found for the top of the borehole {id}.");
                }
            }
            else if (double.IsNaN(eastingBot) || double.IsNaN(northingBot))
            {
                double eastingBotLocal = GetDouble(data, units, "LOCA_XTRL");
                double northingBotLocal = GetDouble(data, units, "LOCA_YTRL");
                if (double.IsNaN(eastingBotLocal) && double.IsNaN(northingBotLocal))
                {
                    Engine.Base.Compute.RecordWarning("No valid coordinates are found for the bottom of the borehole. Therefore, the borehole is assumed straight.");
                    eastingBot = double.IsNaN(eastingTop) ? eastingTopLocal : eastingTop;
                    northingBot = double.IsNaN(northingTop) ? northingTopLocal : northingTop;
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
            string type = GetString(data, "LOCA_TYPE");
            string status = GetString(data, "LOCA_STAT");
            string remarks = GetString(data, "LOCA_REM");
            string purpose = GetString(data, "LOCA_PURP");
            string termination = GetString(data, "LOCA_TERM");

            Methodology methodology = new Methodology() { Type = type, Status = status, Remarks = remarks, Purpose = purpose, Termination = termination };
            if (methodology != null)
                boreholeProperties.Add(methodology);

            // Location
            string method = GetString(data, "LOCA_LOCM");
            string subDivision = GetString(data, "LOCA_LOCA");
            string phase = GetString(data, "LOCA_CLST");
            string alignment = GetString(data, "LOCA_ALID");
            double offset = GetDouble(data, units, "LOCA_OFFS");
            string chainage = GetString(data, "LOCA_CNGE");
            string algorithim = GetString(data, "LOCA_TRAN");

            Location location = new Location() { Method = method, SubDivision = subDivision, Phase = phase, Alignment = alignment, Offset = offset, Chainage = chainage, Algorithm = algorithim };
            if (location != null)
                boreholeProperties.Add(location);

            // BoreholeReference
            DateTime startDate = GetDateTime(data, units, "LOCA_STAR");
            DateTime endDate = GetDateTime(data, units, "LOCA_ENDD");

            string file = GetString(data, "FILE_FSET");
            string originalId = GetString(data, "LOCA_ORID");
            string originalReference = GetString(data, "LOCA_ORJO");
            string originalCompany = GetString(data, "LOCA_ORCO");

            BoreholeReference boreholeReference = new BoreholeReference()
            {
                StartDate = startDate,
                EndDate = endDate,
                File = file,
                OriginalId = originalId,
                OriginalReference = originalReference,
                OriginalCompany = originalCompany
            };

            if (boreholeReference != null)
                boreholeProperties.Add(boreholeReference);

            Borehole borehole = Engine.Ground.Create.Borehole(id, top, bottom, null, boreholeProperties, boreholeStrata, boreholeContaminants);

            return borehole;

        }

        /***************************************************/

    }
}


