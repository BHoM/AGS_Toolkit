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

using BH.oM.Base;
using BH.oM.Adapters.AGS;
using BH.oM.Base.Attributes;
using System;
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

        [Description("Carries out a fuzzy match of the two strings provided using the scorer specified.")]
        [Input("text", "The string to carry out the fuzzy matching on.")]
        [Input("compare", "The string to compare against.")]
        [Input("scorer", "The method to use to score the strings when compared.")]
        [Output("r", "The ratio of similarity between the two strings.")]
        public static int FuzzyMatching(string text, string compare, Scorer scorer)
        {
            switch (scorer)
            {
                case Scorer.DefaultRatioScorer:
                default:
                    return SimpleRatio(text, compare);
                case Scorer.PartialRatioScorer:
                    return PartialRatio(text, compare);
                case Scorer.TokenSetScorer:
                    return TokenSetRatio(text, compare);
                case Scorer.PartialTokenSetScorer:
                    return PartialRatio(text, compare);
                case Scorer.TokenSortScorer:
                    return TokenSortRatio(text, compare);
                case Scorer.PartialTokenSortScorer:
                    return PartialTokenSortRatio(text, compare);
                case Scorer.TokenAbbreviationScorer:
                    return TokenAbbreviationRatio(text, compare);
                case Scorer.PartialTokenAbbreviationScorer:
                    return PartialTokenAbbreviationRatio(text, compare);
                case Scorer.WeightedRatioScorer:
                    return WeightedRatio(text, compare);

            }
        }

        /***************************************************/

    }
}

