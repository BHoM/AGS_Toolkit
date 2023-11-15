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
using FuzzySharp.SimilarityRatio.Scorer.Composite;
using FuzzySharp.SimilarityRatio.Scorer.StrategySensitive;

namespace BH.Engine.Adapters.AGS
{
    public static partial class Compute
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        [Description("Extracts the value with the highest score when comparing the query to the choices. The method uses the weighted ratio and full process.")]
        [Input("query", "The string to carry out the fuzzy matching on.")]
        [Input("choices", "A list of strings to compare the query against.")]
        [MultiOutput(1, "v", "The string with the highest score from the choices.")]
        [MultiOutput(2, "s", "The highest ratio between the query and choices.")]
        [MultiOutput(3, "i", "The index of the highest score from the choices.")]
        public static Output<string, int, int> ExtractOne(string query, IEnumerable<string> choices)
        {
            ExtractedResult<string> result = Process.ExtractOne(query, choices.ToArray());
            return new Output<string, int, int>()
            {
                Item1 = result.Value,
                Item2 = result.Score,
                Item3 = result.Index
            };
        }

        /***************************************************/

        [Description("Extracts the BHoMObject with the highest score when comparing the query to the choices. The method uses the weighted ratio and full process.")]
        [Input("query", "The string to carry out the fuzzy matching on.")]
        [Input("choices", "A list of BHoMObjects to compare the query against.")]
        [Input("propertyName", "The propertyName to compare the query against - the property must be a string.")]
        [MultiOutput(1, "v", "The string with the highest score from the choices.")]
        [MultiOutput(2, "s", "The highest ratio between the query and choices.")]
        [MultiOutput(3, "i", "The index of the highest score from the choices.")]
        public static Output<BHoMObject, int, int> ExtractOne(string query, List<BHoMObject> objects, string propertyName, 
            Scorers scorer = Scorers.DefaultRatioScorer)
        {
            IRatioScorer scorerMethod = ScorerCache.Get<DefaultRatioScorer>();
            if (scorer != Scorers.DefaultRatioScorer)
                scorerMethod = GetScorer(scorer);

            IEnumerable<string> choices = objects.Select(x => x.PropertyValue(propertyName).ToString());

            ExtractedResult<string> result = Process.ExtractOne(query, choices, s =>s,scorerMethod);
            return new Output<BHoMObject, int, int>()
            {
                Item1 = objects[result.Index],
                Item2 = result.Score,
                Item3 = result.Index
            };
        }

        /***************************************************/

    }
}

