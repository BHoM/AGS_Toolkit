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
        public static ContaminantSample FromContaminantSample(string text, Dictionary<string, int> headings)
        { 
            string id = GetValue(text, "LOCA_ID", headings);
            string top = GetValue(text, "SAMP_TOP", headings);

            if (top == "")
                top = GetValue(text, "SPEC_DPTH", headings);

            if (top == "")
            {
                if (id == "")
                    Engine.Base.Compute.RecordWarning($"The top (SAMP_TOP/SPEC_DPTH) value for the contaminant sample is not valid as well as the id (LOCA_ID) and has been skipped.");
                else
                    Engine.Base.Compute.RecordWarning($"The top (SAMP_TOP/SPEC_DPTH) value for {id} is invalid and has been skipped.");
                return null;
            }

            // ContaminantSample
            string chemical = GetValue(text, "ERES_CODE", headings);
            string name = GetValue(text, "ERES_NAME", headings);
            string result = GetValue(text, "ERES_RVAL", headings);
            string type = GetValue(text, "SAMP_TYPE", headings);

            List<IContaminantProperty> contaminantProperties = new List<IContaminantProperty>();

            // Contaminant Reference
            string reference = GetValue(text, "SAMP_REF", headings);
            string specId = GetValue(text, "SAMP_ID", headings);
            string receiptDate = GetValue(text, "ERES_RDAT", headings);
            DateTime receiptDateValue;
            if (!DateTime.TryParse(receiptDate, out receiptDateValue))
                receiptDateValue = default(DateTime);

            string batchCode = GetValue(text, "ERES_SGRP", headings);
            string files = GetValue(text, "FILE_FSET", headings);

            ContaminantReference contaminantReference = Engine.Ground.Create.ContaminantReference(reference, specId, receiptDateValue, batchCode, files);
            if (contaminantReference != null)
                contaminantProperties.Add(contaminantReference);

            // Test Properties
            string testName = GetValue(text, "ERES_TEST", headings);
            string labTestName = GetValue(text, "ERES_TNAM", headings);
            string testReference = GetValue(text, "ERES_TESN", headings);
            string runType = GetValue(text, "ERES_RTYP", headings);
            string matrix = GetValue(text, "ERES_MATX", headings);
            string method = GetValue(text, "ERES_METH", headings);
            string analysisDate = GetValue(text, "ERES_DTIM", headings);
            DateTime analysisDateValue;
            if (!DateTime.TryParse(analysisDate, out analysisDateValue))
                analysisDateValue = default(DateTime);

            string description = GetValue(text, "SPEC_DESC", headings);
            string remarks = GetValue(text, "ERES_REM", headings);
            string testStatus = GetValue(text, "ERES_STAT", headings);

            TestProperties testProperties = Engine.Ground.Create.TestProperties(testName, labTestName, testReference,runType, matrix, method, analysisDateValue, description,
                remarks, testStatus);
            if (testProperties != null)
                contaminantProperties.Add(testProperties);

            // Anaysis Properties
            string totalOrDissolved = GetValue(text, "ERES_TORD", headings);
            string accreditingBody= GetValue(text, "ERES_CRED", headings);
            string labName = GetValue(text, "ERES_LAB", headings);
            string percentageRemoved = GetValue(text, "ERES_PERP", headings);
            double percentageRemovedValue;
            if (!double.TryParse(percentageRemoved, out percentageRemovedValue))
                percentageRemovedValue = 0;

            string sizeRemoved = GetValue(text, "ERES_SIZE", headings);
            double sizeRemovedValue;
            if (!double.TryParse(sizeRemoved, out sizeRemovedValue))
                sizeRemovedValue = 0;

            string instrumentReference = GetValue(text, "ERES_IREF", headings);
            string leachateDate = GetValue(text, "ERES_LDTM", headings);
            string leachateMethod = GetValue(text, "ERES_LMTH", headings);
            string diluationFactor = GetValue(text, "ERES_DIL", headings);
            int dilutionFactorValue;
            if (!int.TryParse(diluationFactor, out dilutionFactorValue))
                dilutionFactorValue = 0;

            string basis = GetValue(text, "ERES_BAS", headings);
            string location = GetValue(text, "ERES_LOCN", headings);

            AnalysisProperties analysisProperties = Engine.Ground.Create.AnalysisProperties(totalOrDissolved, accreditingBody, labName, percentageRemovedValue, sizeRemovedValue, instrumentReference, DateTime.Parse(leachateDate),
                leachateMethod, dilutionFactorValue, basis, location);
            if (analysisProperties != null)
                contaminantProperties.Add(analysisProperties);

            // Result Properties
            string resultType = GetValue(text, "ERES_RTCD", headings);
            string reportable = GetValue(text, "ERES_RRES", headings);
            string detectFlag = GetValue(text, "ERES_DETF", headings);
            string organic = GetValue(text, "ERES_ORG", headings);

            ResultProperties resultProperties = Engine.Ground.Create.ResultProperties(ParseYNString(organic), ParseYNString(reportable), ParseYNString(detectFlag), type);
            if (resultProperties != null)
                contaminantProperties.Add(resultProperties);

            // Detection Properties
            string detectionLimit = GetValue(text, "ERES_RDLM", headings);
            string methodDetectionLimit = GetValue(text, "ERES_MDLM", headings);
            string quantificationLimit = GetValue(text, "ERES_QLM", headings);
            string ticProbability = GetValue(text, "ERES_TICP", headings);
            string ticRetention = GetValue(text, "ERES_TICT", headings);

            DetectionProperties detectionProperties = Engine.Ground.Create.DetectionProperties(double.Parse(detectionLimit), double.Parse(methodDetectionLimit), double.Parse(quantificationLimit),
                double.Parse(ticProbability), double.Parse(ticRetention));
            if (detectionProperties != null)
                contaminantProperties.Add(detectionProperties);

            ContaminantSample contaminantSample = Engine.Ground.Create.ContaminantSample(id, double.Parse(top), chemical, name, double.Parse(result), type, contaminantProperties);

            return contaminantSample;

        }

        /***************************************************/

    }
}

