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
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.oM.Adapter;
using BH.oM.Base;

namespace BH.oM.Adapters.AGS
{
    [Description("A result class containing strings, scores and indexes from the fuzzy matching.")]
    public class FuzzyStringResult : BHoMObject, IFuzzyResult, IImmutable
    {
        /***************************************************/
        /****            Public Properties              ****/
        /***************************************************/

        [Description("A list of strings resulting from the fuzzy matching algorithm.")]
        public virtual List<string> Results { get; }

        [Description("A list of scores resulting from the fuzzy matching algorithm.")]
        public virtual List<int> Scores { get; }

        [Description("A list of indexes resulting from the fuzzy matching algorithm.")]
        public virtual List<int> Indexes { get; }

        /***************************************************/
        /****            Constructor                    ****/
        /***************************************************/

        public FuzzyStringResult(List<string> results, List<int> scores, List<int> indexes)
        {
            Results = results;
            Scores = scores;
            Indexes = indexes;
        }

        /***************************************************/
    }
}




