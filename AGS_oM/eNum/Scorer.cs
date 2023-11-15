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

using System.ComponentModel;

namespace BH.oM.Adapters.AGS
{
    /***************************************************/

    [Description("Defines the scorer method to be used in the fuzzy matching methods (i.e. Extract).")]
    public enum Scorer
    {
        [Description("Calculates a Levenshtein simple ratio between the strings. This indicates a measure of similarity.")]
        DefaultRatioScorer,
        [Description("This ratio uses a heuristic called 'best partial' for when two strings are of noticeably different lengths.")]
        PartialRatioScorer,
        [Description("Splits the strings into tokens and computes intersections and remainders between the tokens of the two strings.A comparison string is then " +
    "built up and is compared using the simple ratio algorithm. Useful for strings where words appear redundantly.")]
        TokenSetScorer,
        [Description("Splits the strings into tokens and computes intersections and remainders between the tokens of the two strings.A comparison string is then " +
            "built up and is compared using the simple ratio algorithm. Useful for strings where words appear redundantly.")]
        PartialTokenSetScorer,
        [Description("Find all alphanumeric tokens in the string and sort those tokens and then take ratio of resulting joined strings.")]
        TokenSortScorer,
        [Description("Find all alphanumeric tokens in the string and sort those tokens and then take ratio of resulting joined strings.")]
        PartialTokenSortScorer,
        [Description("Similarity ratio that attempts to determine whether one strings tokens are an abbreviation of the other strings tokens. One string must have all its " +
            "characters in order in the other string to even be considered.")]
        TokenAbbreviationScorer,
        [Description("Similarity ratio that attempts to determine whether one strings tokens are an abbreviation of the other strings tokens. One string must have all its " +
            "characters in order in the other string to even be considered.")]
        PartialTokenAbbreviationScorer,
        [Description("Calculates a weighted ratio between the different algorithms for best results.")]
        WeightedRatioScorer,
    }

    /***************************************************/
}