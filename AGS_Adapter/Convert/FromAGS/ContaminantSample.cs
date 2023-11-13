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
        public static ContaminantSample FromContaminantSample(string text, Dictionary<string, int> headings, Dictionary<string, string> units)
        { 
            string id = GetValue<string>(text, "LOCA_ID", headings,units);
            double top = Convert.Units(GetValue<double>(text, "SAMP_TOP", headings,units), "SAMP_TOP", units);

            if (double.IsNaN(top))
                top = Convert.Units(GetValue<double>(text, "SPEC_DPTH", headings,units), "SPEC_DPTH", units);

            if (double.IsNaN(top))
            {
                if (id == "")
                    Engine.Base.Compute.RecordWarning($"The top (SAMP_TOP/SPEC_DPTH) value for the contaminant sample is not valid as well as the id (LOCA_ID) and has been skipped.");
                else
                    Engine.Base.Compute.RecordWarning($"The top (SAMP_TOP/SPEC_DPTH) value for {id} is invalid and has been skipped.");
                return null;
            }

            // ContaminantSample
            string chemical = GetValue<string>(text, "ERES_CODE", headings,units);
            string name = GetValue<string>(text, "ERES_NAME", headings,units);
            string type = GetValue<string>(text, "SAMP_TYPE", headings,units);
            double result = Convert.Units(GetValue<double>(text, "ERES_RVAL", headings,units), "ERES_RVAL", units);

            List<IContaminantProperty> contaminantProperties = new List<IContaminantProperty>();

            // Contaminant Reference
            string reference = GetValue<string>(text, "SAMP_REF", headings,units);
            string specId = GetValue<string>(text, "SAMP_ID", headings,units);
            DateTime receiptDate = GetValue<DateTime>(text, "ERES_RDAT", headings,units);
            string batchCode = GetValue<string>(text, "ERES_SGRP", headings,units);
            string files = GetValue<string>(text, "FILE_FSET", headings,units);

            ContaminantReference contaminantReference = Engine.Ground.Create.ContaminantReference(reference, specId, receiptDate, batchCode, files);
            if (contaminantReference != null)
                contaminantProperties.Add(contaminantReference);

            // Test Properties
            string testName = GetValue<string>(text, "ERES_TEST", headings,units);
            string labTestName = GetValue<string>(text, "ERES_TNAM", headings,units);
            string testReference = GetValue<string>(text, "ERES_TESN", headings,units);
            string runType = GetValue<string>(text, "ERES_RTYP", headings,units);
            string matrix = GetValue<string>(text, "ERES_MATX", headings,units);
            string method = GetValue<string>(text, "ERES_METH", headings,units);
            DateTime analysisDate = GetValue<DateTime>(text, "ERES_DTIM", headings,units);

            string description = GetValue<string>(text, "SPEC_DESC", headings,units);
            string remarks = GetValue<string>(text, "ERES_REM", headings,units);
            string testStatus = GetValue<string>(text, "TEST_STAT", headings,units);

            TestProperties testProperties = Engine.Ground.Create.TestProperties(testName, labTestName, testReference,runType, matrix, method, analysisDate, description,
                remarks, testStatus);
            if (testProperties != null)
                contaminantProperties.Add(testProperties);

            // Anaysis Properties
            string totalOrDissolved = GetValue<string>(text, "ERES_TORD", headings,units);
            string accreditingBody= GetValue<string>(text, "ERES_CRED", headings,units);
            string labName = GetValue<string>(text, "ERES_LAB", headings,units);
            double percentageRemoved = Convert.Units(GetValue<double>(text, "ERES_PERP", headings,units), "ERES_PERP", units);
            double sizeRemoved = Convert.Units(GetValue<double>(text, "ERES_SIZE", headings,units), "ERES_SIZE", units);
            string instrumentReference = GetValue<string>(text, "ERES_IREF", headings,units);
            DateTime leachateDate = GetValue<DateTime>(text, "ERES_LDTM", headings,units);
            string leachateMethod = GetValue<string>(text, "ERES_LMTH", headings,units);
            int dilutionFactor = GetValue<int>(text, "ERES_DIL", headings,units);
            string basis = GetValue<string>(text, "ERES_BAS", headings,units);
            string location = GetValue<string>(text, "ERES_LOCN", headings,units);

            AnalysisProperties analysisProperties = Engine.Ground.Create.AnalysisProperties(totalOrDissolved, accreditingBody, labName, percentageRemoved, 
                sizeRemoved, instrumentReference, leachateDate, leachateMethod, dilutionFactor, basis, location);
            if (analysisProperties != null)
                contaminantProperties.Add(analysisProperties);

            // Result Properties
            string resultType = GetValue<string>(text, "ERES_RTCD", headings,units);
            bool reportable = GetValue<bool>(text, "ERES_RRES", headings,units);
            bool detectFlag = GetValue<bool>(text, "ERES_DETF", headings,units);
            bool organic = GetValue<bool>(text, "ERES_ORG", headings,units);

            ResultProperties resultProperties = Engine.Ground.Create.ResultProperties(organic, reportable, detectFlag, type);
            if (resultProperties != null)
                contaminantProperties.Add(resultProperties);

            // Detection Properties
            double detectionLimit = Convert.Units(GetValue<double>(text, "ERES_RDLM", headings,units), "ERES_RDLM", units);
            double methodDetectionLimit = Convert.Units(GetValue<double>(text, "ERES_MDLM", headings,units), "ERES_MDLM", units);
            double quantificationLimit = Convert.Units(GetValue<double>(text, "ERES_QLM", headings,units), "ERES_QLM", units);
            double ticProbability = Convert.Units(GetValue<double>(text, "ERES_TICP", headings,units), "ERES_TICP", units);
            double ticRetention = Convert.Units(GetValue<double>(text, "ERES_TICT", headings,units), "ERES_TICT", units);

            DetectionProperties detectionProperties = Engine.Ground.Create.DetectionProperties(detectionLimit,methodDetectionLimit, quantificationLimit, ticProbability, ticRetention);
            if (detectionProperties != null)
                contaminantProperties.Add(detectionProperties);

            ContaminantSample contaminantSample = Engine.Ground.Create.ContaminantSample(id, top, chemical, name, result, type, contaminantProperties);

            return contaminantSample;

        }

        /***************************************************/

    }
}

