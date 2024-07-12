/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2024, the respective contributors. All rights reserved.
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
using BH.oM.Base.Attributes;

namespace BH.oM.Adapters.AGS
{
    [Description("Different approaches for the AGS_Toolkit to handle blank geology.")]
    public enum BlankGeologyStrategy
    {
        [Description("Replace any entries of blank geology with a specificed string.")]
        Replace = 0,
        [Description("Replace any entries of blank geology using the description attribute (GEOL_DESC) with words in CAPS.")]
        Description = 1,
        [Description("Replace any entries of blank geology using the legend (GEOL_LEG).")]
        Legend = 2,
        [Description("Replace any entries of blank geology using the lexicon code (GEOL_BGS).")]
        Lexicon = 3,
    }
}





