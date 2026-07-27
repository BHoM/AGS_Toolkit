/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2026, the respective contributors. All rights reserved.
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

using System.Collections.Generic;
using BH.Engine.Base;
using BH.oM.Ground;

namespace BH.Adapter.AGS
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static WaterStrike FromWaterStrike(Dictionary<string, string> data, Dictionary<string, string> units)
        {
            string id = GetString(data, "LOCA_ID");

            if (id == "")
                Compute.RecordWarning("No valid id found for the WaterStrike.");

            double depth = GetDouble(data, units, "WSTG_DPTH");
            double timePostStrike = GetDouble(data, units, "WSTD_NMIN");
            double depthPostStrike = GetDouble(data, units, "WSTD_POST");
            string remarks = GetString(data, "WSTD_REM");
            string file = GetString(data, "FILE_FSET");

            WaterStrike waterStrike = new WaterStrike()
            {
                Id = id,
                Depth = depth,
                TimePostStrike = timePostStrike,
                DepthPostStrike = depthPostStrike,
                Remarks = remarks,
                File = file
            };

            return waterStrike;
        }

        /***************************************************/

    }
}




