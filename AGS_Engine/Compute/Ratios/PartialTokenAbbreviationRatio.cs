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
using BH.oM.Base.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using FuzzySharp;

namespace BH.Engine.Adapters.AGS
{
    public static partial class Compute
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        [Description("Similarity ratio that attempts to determine whether one strings tokens are an abbreviation of the other strings tokens. One string must have all its " +
            "characters in order in the other string to even be considered. This makes use of the FuzzySharp library.")]
        [Input("text", "The string to carry out the fuzzy matching on.")]
        [Input("compare", "The string to compare against.")]
        [Output("r", "The ratio of similarity between the two strings.")]
        public static int PartialTokenAbbreviationRatio(string text, string compare)
        {
            return Fuzz.PartialTokenAbbreviationRatio(text, compare);
        }

        /***************************************************/

    }
}

