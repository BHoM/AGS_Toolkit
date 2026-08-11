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

        public static SPT FromSPT(Dictionary<string, string> data, Dictionary<string, string> units)
        {
            string id = GetString(data, "LOCA_ID");

            if (id == "")
                Compute.RecordWarning("No valid id found for the SPT.");

            double top = GetDouble(data, units, "ISPT_TOP");
            int numberOfBlows = GetInt(data, "ISPT_NVAL");
            double energyRatio = GetDouble(data, units, "ISPT_ERAT");
            int mainTestDrive = GetInt(data, "ISPT_MAIN");
            double totalPenetration = GetDouble(data, units, "ISPT_NPEN");

            List<ITestProperties> testProperties = new List<ITestProperties>();

            // SPT Reference Properties
            string file = GetString(data, "FILE_FSET");
            string remarks = GetString(data, "ISPT_REM");

            SPTReferenceProperties sptReferenceProperties = new SPTReferenceProperties()
            {
                FileReference = file,
                Remarks = remarks
            };

            if (sptReferenceProperties != null)
                testProperties.Add(sptReferenceProperties);

            // SPT Test Properties
            string hammerNumber = GetString(data, "ISPT_HAM");
            string type = GetString(data, "ISPT_TYPE");
            double waterDepth = GetDouble(data, units, "ISPT_WAT");
            double casingDepth = GetDouble(data, units, "ISPT_CAS");
            bool softRock = GetBool(data, "ISPT_ROCK");
            string weatherConditions = GetString(data, "ISPT_ENV");
            string method = GetString(data, "ISPT_METH");
            string accreditingBody = GetString(data, "ISPT_CRED");
            string status = GetString(data, "TEST_STAT");

            SPTTestProperties sptTestProperties = new SPTTestProperties()
            {
                HammerNumber = hammerNumber,
                Type = type,
                WaterDepth = waterDepth,
                CasingDepth = casingDepth,
                SoftRock = softRock,
                WeatherConditions = weatherConditions,
                Method = method,
                AccreditingBody = accreditingBody,
                Status = status
            };

            if (sptTestProperties != null)
                testProperties.Add(sptTestProperties);

            // SPT Result Properties
            string reportedResult = GetString(data, "ISPT_REP");
            int seatingDriveBlows = GetInt(data, "ISPT_SEAT");
            int mainTestDriveBlows = GetInt(data, "ISPT_MAIN");
            double selfWeightPenetration = GetDouble(data, units, "ISPT_SWP");
            int seatBlows1 = GetInt(data, "ISPT_INC1");
            int seatBlows2 = GetInt(data, "ISPT_INC2");
            int testBlows1 = GetInt(data, "ISPT_INC3");
            int testBlows2 = GetInt(data, "ISPT_INC4");
            int testBlows3 = GetInt(data, "ISPT_INC5");
            int testBlows4 = GetInt(data, "ISPT_INC6");
            double seatPenetration1 = GetDouble(data, units, "ISPT_PEN1");
            double seatPenetration2 = GetDouble(data, units, "ISPT_PEN2");
            double penetrationTest1 = GetDouble(data, units, "ISPT_PEN3");
            double penetrationTest2 = GetDouble(data, units, "ISPT_PEN4");
            double penetrationTest3 = GetDouble(data, units, "ISPT_PEN5");
            double penetrationTest4 = GetDouble(data, units, "ISPT_PEN6");
            double sptN60 = Engine.Ground.Compute.CorrectedNumberOfBlows(numberOfBlows, energyRatio);

            SPTResultProperties sptResultProperties = new SPTResultProperties()
            {
                ReportedResult = reportedResult,
                SeatingDriveBlows = seatingDriveBlows,
                MainTestDriveBlows = mainTestDriveBlows,
                SPTN60 = sptN60,
                TotalPenetration = totalPenetration,
                SelfWeightPenetration = selfWeightPenetration,
                SeatBlows1 = seatBlows1,
                SeatBlows2 = seatBlows2,
                TestBlows1 = testBlows1,
                TestBlows2 = testBlows2,
                TestBlows3 = testBlows3,
                TestBlows4 = testBlows4,
                SeatPenetration1 = seatPenetration1,
                SeatPenetration2 = seatPenetration2,
                PenetrationTest1 = penetrationTest1,
                PenetrationTest2 = penetrationTest2,
                PenetrationTest3 = penetrationTest3,
                PenetrationTest4 = penetrationTest4
            };

            if (sptResultProperties != null)
                testProperties.Add(sptResultProperties);

            SPT spt = new SPT()
            {
                Id = id,
                Top = top,
                NumberofBlows = numberOfBlows,
                EnergyRatio = energyRatio,
                MainTestDrive = mainTestDrive,
                TotalPenetration = totalPenetration,
                Properties = testProperties
            };

            return spt;
        }

        /***************************************************/

    }
}




