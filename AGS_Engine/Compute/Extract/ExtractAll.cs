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

using BH.Engine.Base;
using BH.oM.Base;
using BH.oM.Adapters.AGS;
using BH.oM.Base.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using FuzzySharp;
using FuzzySharp.Extractor;
using FuzzySharp.SimilarityRatio;
using FuzzySharp.SimilarityRatio.Scorer;
using FuzzySharp.SimilarityRatio.Scorer.StrategySensitive;

namespace BH.Engine.Adapters.AGS
{
    public static partial class Compute
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        [Description("Extracts all values when comparing the query to the choices. The method uses the weighted ratio and full process.")]
        [Input("query", "The string to carry out the fuzzy matching on.")]
        [Input("choices", "A list of strings to compare the query against.")]
        [Input("cutOff", "The cuttoff score (i.e. lower bound) for results to be returned.")]
        [Output("result", "A FuzzyStringResult containing the strings, scores and indexes resulting from the fuzzy matching algorithm.")]
        public static FuzzyStringResult ExtractAll(string query, IEnumerable<string> choices, int cutOff = 0)
        {
            return ExtractAll(query, choices, Scorer.DefaultRatioScorer, cutOff);
        }

        /***************************************************/

        [Description("Extracts the top n values with the highest score when comparing the query to the choices. If the default value on n = 0 is used, all values will be returned unsorted.")]
        [Input("query", "The string to carry out the fuzzy matching on.")]
        [Input("choices", "A list of strings to compare the query against.")]
        [Input("scorer", "The method to use to score the strings when compared.")]
        [Input("cutOff", "The cuttoff score (i.e. lower bound) for results to be returned.")]
        [Output("result", "A FuzzyStringResult containing the strings, scores and indexes resulting from the fuzzy matching algorithm.")]
        public static FuzzyStringResult ExtractAll(string query, IEnumerable<string> choices, Scorer scorer = Scorer.DefaultRatioScorer, int cutOff = 0)
        {
            IRatioScorer scorerMethod = ScorerCache.Get<DefaultRatioScorer>();
            if (scorer != Scorer.DefaultRatioScorer)
                scorerMethod = GetScorer(scorer);

            IEnumerable<ExtractedResult<string>> result = Process.ExtractAll(query, choices.ToArray(), s => s, scorerMethod, cutOff);
            return new FuzzyStringResult
            (
                result.Select(x => x.Value).ToList(), 
                result.Select(x => x.Score).ToList(), 
                result.Select(x => x.Index).ToList()
            );
        }

        /***************************************************/

        [Description("Extracts the top n BHoMObjects with the highest score when comparing the query to the choices. If the default value on n = 0 is used, all values will be returned unsorted.")]
        [Input("query", "The string to carry out the fuzzy matching on.")]
        [Input("objects", "A list of BHoMObjects to compare the query against.")]
        [Input("propertyName", "The propertyName to compare the query against - the property must be a string.")]
        [Input("scorer", "The method to use to score the strings when compared.")]
        [Input("cutOff", "The cuttoff score (i.e. lower bound) for results to be returned.")]
        [Output("result", "A FuzzyObjectResult containing the objects, scores and indexes resulting from the fuzzy matching algorithm.")]
        public static FuzzyObjectResult ExtractAll(string query, List<BHoMObject> objects, string propertyName, Scorer scorer = Scorer.DefaultRatioScorer, int cutOff = 0)
        {
            IRatioScorer scorerMethod = ScorerCache.Get<DefaultRatioScorer>();
            if (scorer != Scorer.DefaultRatioScorer)
                scorerMethod = GetScorer(scorer);

            IEnumerable<string> choices = objects.Select(x => x.PropertyValue(propertyName).ToString());

            IEnumerable<ExtractedResult<string>> result = Process.ExtractAll(query, choices, s => s, scorerMethod, cutOff);

            List<BHoMObject> resultObjects = new List<BHoMObject>();
            foreach (int i in result.Select(x => x.Index))
                resultObjects.Add(objects[i]);

            return new FuzzyObjectResult
            (
                resultObjects,
                result.Select(x => x.Score).ToList(),
                result.Select(x => x.Index).ToList()
            );
        }

        /***************************************************/

    }
}

