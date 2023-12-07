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
using BH.oM.Base;
using BH.oM.Analytical.Results;

namespace BH.oM.Adapters.AGS
{
    [Description("A result class containing objects, scores and indexes from the fuzzy matching.")]
    public class FuzzyResult<T> : IResult
    {
        /***************************************************/
        /****            Public Properties              ****/
        /***************************************************/

        [Description("A list of objects resulting from the fuzzy matching algorithm.")]
        public virtual T Result { get; }

        [Description("A list of scores resulting from the fuzzy matching algorithm.")]
        public virtual int Score { get; }

        [Description("A list of indexes resulting from the fuzzy matching algorithm.")]
        public virtual int Index { get; }

        /***************************************************/
        /****            Constructor                    ****/
        /***************************************************/

        public FuzzyResult(T result, int score, int index)
        {
            Result = result;
            Score = score;
            Index = index;
        }

        public int CompareTo(IResult other)
        {
            // TODO: Add comparison for objects at a later date
            FuzzyResult<T> otherRes = other as FuzzyResult<T>;

            if(otherRes == null)
                return this.GetType().Name.CompareTo(other.GetType().Name);

            int n = this.Score.CompareTo(otherRes.Score);

            if (n == 0)
            {
                int l = this.Index.CompareTo(otherRes.Index);
                if (l == 0)
                {
                    return 0;
                }
                else
                {
                    return l;
                }
            }
            else
            {
                return n;
            }
        }

        /***************************************************/
    }
}