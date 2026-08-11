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

        public static Triaxial FromTriaxial(Dictionary<string, string> data, Dictionary<string, string> units)
        {
            string id = GetString(data, "LOCA_ID");

            if (id == "")
                Compute.RecordWarning("No valid id found for the Triaxial.");

            double top = GetDouble(data, units, "SAMP_TOP");
            double specimenDepth = GetDouble(data, units, "SPEC_DPTH");
            string sampleId = GetString(data, "SAMP_ID");
            string specimenReference = GetString(data, "SPEC_REF");
            double undrainedShearStrength = GetDouble(data, units, "TRIT_CU");
            string failureMode = GetString(data, "TRIT_MODE");

            List<ITestProperties> testProperties = new List<ITestProperties>();

            // Triaxial Reference Properties
            string sampleReference = GetString(data, "SAMP_REF");
            string sampleType = GetString(data, "SAMP_TYPE");
            string remarks = GetString(data, "TRIT_REM");
            string file = GetString(data, "FILE_FSET");

            TriaxialReferenceProperties triaxialReferenceProperties = new TriaxialReferenceProperties()
            {
                Top = top,
                Reference = sampleReference,
                SampleType = sampleType,
                Remarks = remarks,
                FileReference = file
            };

            if (triaxialReferenceProperties != null)
                testProperties.Add(triaxialReferenceProperties);

            // Triaxial Test Properties
            double sampleDiameter = GetDouble(data, units, "TRIT_SDIA");
            double sampleLength = GetDouble(data, units, "TRIT_SLEN");
            string triaxialReference = GetString(data, "TRIT_TESN");

            TriaxialTestProperties triaxialTestProperties = new TriaxialTestProperties()
            {
                SampleDiameter = sampleDiameter,
                SampleLength = sampleLength,
                TriaxialReference = triaxialReference
            };

            if (triaxialTestProperties != null)
                testProperties.Add(triaxialTestProperties);

            // Triaxial Result Properties
            double initialWaterMoistureContent = GetDouble(data, units, "TRIT_IMC");
            double finalWaterMoistureContent = GetDouble(data, units, "TRIT_FMC");
            double totalCellPressure = GetDouble(data, units, "TRIT_CELL");
            double deviatorStress = GetDouble(data, units, "TRIT_DEVF");
            double bulkDensity = GetDouble(data, units, "TRIT_BDEN");
            double dryDensity = GetDouble(data, units, "TRIT_DDEN");
            double axialStrain = GetDouble(data, units, "TRIT_STRN");
            string shearRate = GetString(data, "TRIT_RATE");
            double failureZoneWaterContent = GetDouble(data, units, "TRIT_FZWC");

            TriaxialResultProperties triaxialResultProperties = new TriaxialResultProperties()
            {
                InitialWaterMoistureContent = initialWaterMoistureContent,
                FinalWaterMoistureContent = finalWaterMoistureContent,
                TotalCellPressure = totalCellPressure,
                DeviatorStress = deviatorStress,
                BulkDensity = bulkDensity,
                DryDensity = dryDensity,
                AxialStrain = axialStrain,
                ShearRate = shearRate,
                FailureZoneWaterContent = failureZoneWaterContent
            };

            if (triaxialResultProperties != null)
                testProperties.Add(triaxialResultProperties);

            Triaxial triaxial = new Triaxial()
            {
                Id = id,
                Top = top,
                SpecimenDepth = specimenDepth,
                SampleId = sampleId,
                SpecimenReference = specimenReference,
                UndrainedShearStrength = undrainedShearStrength,
                FailureMode = failureMode,
                Properties = testProperties
            };

            return triaxial;
        }

        /***************************************************/

    }
}




