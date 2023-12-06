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
            string id = data["LOCA_ID"];
            double top = Convert.Units(GetDouble(data["SAMP_TOP"]), units["SAMP_TOP"]);

            if (double.IsNaN(top))
                top = Convert.Units(GetDouble(data["SPEC_DPTH"]), units["SPEC_DPTH"]);

            if (double.IsNaN(top))
            {
                if (id == "")
                    Engine.Base.Compute.RecordWarning($"The top (SAMP_TOP/SPEC_DPTH) value for the contaminant sample is not valid as well as the id (LOCA_ID) and has been skipped.");
                else
                    Engine.Base.Compute.RecordWarning($"The top (SAMP_TOP/SPEC_DPTH) value for {id} is invalid and has been skipped.");
                return null;
            }

            // ContaminantSample
            string chemical = data["ERES_CODE"];
            string name = data["ERES_NAME"];
            string type = data["SAMP_TYPE"];
            string rvalUnit = data["ERES_RUNI"];

            //Replace the ERES_RVAL unit value, as this is provided in the ERES_RUNI column (not the UNITS heading)
            units["ERES_RVAL"] = rvalUnit;
            double result = Convert.Units(GetDouble(data["ERES_RVAL"]), units["ERES_RVAL"]);

            List<IContaminantProperty> contaminantProperties = new List<IContaminantProperty>();

            // Contaminant Reference
            string reference = data["SAMP_REF"];
            string specId = data["SAMP_ID"];
            DateTime receiptDate = GetDateTime(data["ERES_RDAT"], units["ERES_RDAT"]);
            string batchCode = data["ERES_SGRP"];
            string files = data["FILE_FSET"];

            ContaminantReference contaminantReference = new ContaminantReference() { Reference = reference, Id = specId, ReceiptDate = receiptDate, BatchCode = batchCode, Files = files };
            if (contaminantReference != null)
                contaminantProperties.Add(contaminantReference);

            // Test Properties
            string testName = data["ERES_TEST"];
            string labTestName = data["ERES_TNAM"];
            string testReference = data["ERES_TESN"];
            string runType = data["ERES_RTYP"];
            string matrix = data["ERES_MATX"];
            string method = data["ERES_METH"];
            DateTime analysisDate = GetDateTime(data["ERES_DTIM"], units["ERES_DTIM"]);

            string description = data["SPEC_DESC"];
            string remarks = data["ERES_REM"];
            string testStatus = data["TEST_STAT"];

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
            string totalOrDissolved = data["ERES_TORD"];
            string accreditingBody = data["ERES_CRED"];
            string labName = data["ERES_LAB"];
            double percentageRemoved = Convert.Units(GetDouble(data["ERES_PERP"]), units["ERES_PERP"]);
            double sizeRemoved = Convert.Units(GetDouble(data["ERES_SIZE"]), units["ERES_SIZE"]);
            string instrumentReference = data["ERES_IREF"];
            DateTime leachateDate = GetDateTime(data["ERES_LDTM"], units["ERES_LDTM"]);
            string leachateMethod = data["ERES_LMTH"];
            int dilutionFactor = GetInt(data["ERES_DIL"]);
            string basis = data["ERES_BAS"];
            string location = data["ERES_LOCN"];

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
            string resultType = data["ERES_RTCD"];
            bool reportable = GetBool(data["ERES_RRES"]);
            bool detectFlag = GetBool(data["ERES_DETF"]);
            bool organic = GetBool(data["ERES_ORG"]);

            ResultProperties resultProperties = new ResultProperties() { Organic = organic, Reportable = reportable, DetectFlag = detectFlag, Type = resultType };
            if (resultProperties != null)
                contaminantProperties.Add(resultProperties);

            // Detection Properties
            double detectionLimit = Convert.Units(GetDouble(data["ERES_RDLM"]), units["ERES_RDLM"]);
            double methodDetectionLimit = Convert.Units(GetDouble(data["ERES_MDLM"]), units["ERES_MDLM"]);
            double quantificationLimit = Convert.Units(GetDouble(data["ERES_QLM"]), units["ERES_QLM"]);
            double ticProbability = Convert.Units(GetDouble(data["ERES_TICP"]), units["ERES_TICP"]);
            double ticRetention = Convert.Units(GetDouble(data["ERES_TICT"]), units["ERES_TICT"]);

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

