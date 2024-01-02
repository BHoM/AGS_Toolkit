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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.oM.Ground;

namespace BH.Adapter.AGS
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/
        public static ContaminantSample FromContaminantSample(Dictionary<string, string> data, Dictionary<string, string> units)
        {
            string id = GetString(data, "LOCA_ID");
            double top = GetDouble(data, units, "SAMP_TOP");

            if (double.IsNaN(top))
                top = GetDouble(data, units, "SPEC_DPTH");

            if (double.IsNaN(top))
            {
                if (id == "")
                    Engine.Base.Compute.RecordWarning($"The top (SAMP_TOP/SPEC_DPTH) value for the contaminant sample is not valid as well as the id (LOCA_ID) and has been skipped.");
                else
                    Engine.Base.Compute.RecordWarning($"The top (SAMP_TOP/SPEC_DPTH) value for {id} is invalid and has been skipped.");
                return null;
            }

            // ContaminantSample
            string chemical = GetString(data, "ERES_CODE");
            string name = GetString(data, "ERES_NAME");
            string type = GetString(data, "SAMP_TYPE");
            string rvalUnit = GetString(data, "ERES_RUNI");

            //Replace the ERES_RVAL unit value, as this is provided in the ERES_RUNI column (not the UNITS heading)
            units["ERES_RVAL"] = rvalUnit;
            double result = GetDouble(data, units, "ERES_RVAL");

            List<IContaminantProperty> contaminantProperties = new List<IContaminantProperty>();

            // Contaminant Reference
            string reference = GetString(data, "SAMP_REF");
            string specId = GetString(data, "SAMP_ID");
            DateTime receiptDate = GetDateTime(data, units, "ERES_RDAT");
            string batchCode = GetString(data, "ERES_SGRP");
            string files = GetString(data, "FILE_FSET");

            ContaminantReference contaminantReference = new ContaminantReference() { Reference = reference, Id = specId, ReceiptDate = receiptDate, BatchCode = batchCode, Files = files };
            if (contaminantReference != null)
                contaminantProperties.Add(contaminantReference);

            // Test Properties
            string testName = GetString(data, "ERES_TEST");
            string labTestName = GetString(data, "ERES_TNAM");
            string testReference = GetString(data, "ERES_TESN");
            string runType = GetString(data, "ERES_RTYP");
            string matrix = GetString(data, "ERES_MATX");
            string method = GetString(data, "ERES_METH");
            DateTime analysisDate = GetDateTime(data, units, "ERES_DTIM");

            string description = GetString(data, "SPEC_DESC");
            string remarks = GetString(data, "ERES_REM");
            string testStatus = GetString(data, "TEST_STAT");

            TestProperties testProperties = new TestProperties()
            {
                Name = testName,
                LabTestName = labTestName,
                Reference = testReference,
                RunType = runType,
                TestMatrix = matrix,
                Method = method,
                AnalysisDate = analysisDate,
                Description = description,
                Remarks = remarks,
                TestStatus = testStatus
            };
            if (testProperties != null)
                contaminantProperties.Add(testProperties);

            // Anaysis Properties
            string totalOrDissolved = GetString(data, "ERES_TORD");
            string accreditingBody = GetString(data, "ERES_CRED");
            string labName = GetString(data, "ERES_LAB");
            double percentageRemoved = GetDouble(data, units, "ERES_PERP");
            double sizeRemoved = GetDouble(data, units, "ERES_SIZE");
            string instrumentReference = GetString(data, "ERES_IREF");
            DateTime leachateDate = GetDateTime(data, units, "ERES_LDTM");
            string leachateMethod = GetString(data, "ERES_LMTH");
            int dilutionFactor = GetInt(data, "ERES_DIL");
            string basis = GetString(data, "ERES_BAS");
            string location = GetString(data, "ERES_LOCN");

            AnalysisProperties analysisProperties = new AnalysisProperties()
            {
                TotalOrDissolved = totalOrDissolved,
                AccreditingBody = accreditingBody,
                LabName = labName,
                PercentageRemoved = percentageRemoved,
                SizeRemoved = sizeRemoved,
                InstrumentReference = instrumentReference,
                LeachateDate = leachateDate,
                LeachateMethod = leachateMethod,
                DilutionFactor = dilutionFactor,
                Basis = basis,
                Location = location
            };
            if (analysisProperties != null)
                contaminantProperties.Add(analysisProperties);

            // Result Properties
            string resultType = GetString(data, "ERES_RTCD");
            bool reportable = GetBool(data, "ERES_RRES");
            bool detectFlag = GetBool(data, "ERES_DETF");
            bool organic = GetBool(data, "ERES_ORG");

            ResultProperties resultProperties = new ResultProperties() { Organic = organic, Reportable = reportable, DetectFlag = detectFlag, Type = resultType };
            if (resultProperties != null)
                contaminantProperties.Add(resultProperties);

            // Detection Properties
            double detectionLimit = GetDouble(data, units, "ERES_RDLM");
            double methodDetectionLimit = GetDouble(data, units, "ERES_MDLM");
            double quantificationLimit = GetDouble(data, units, "ERES_QLM");
            double ticProbability = GetDouble(data, units, "ERES_TICP");
            double ticRetention = GetDouble(data, units, "ERES_TICT");

            DetectionProperties detectionProperties = new DetectionProperties()
            {
                DetectionLimit = detectionLimit,
                MethodDetectionLimit = methodDetectionLimit,
                QuantificationLimit = quantificationLimit,
                TICProbability = ticProbability,
                TICRetention = ticRetention
            };
            if (detectionProperties != null)
                contaminantProperties.Add(detectionProperties);

            ContaminantSample contaminantSample = Engine.Ground.Create.ContaminantSample(id, top, chemical, name, result, type, contaminantProperties);

            return contaminantSample;

        }

        /***************************************************/

    }
}


