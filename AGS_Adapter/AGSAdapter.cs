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

using BH.Adapter;
using BH.oM.Base.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.oM.Adapters.AGS;

namespace BH.Adapter.AGS
{
    public partial class AGSAdapter : BHoMAdapter
    {
        /***************************************************/
        /**** Constructors                              ****/
        /***************************************************/

        [Description("Adapter for AGS files.")]
        [Output("The created AGS adapter.")]
        public AGSAdapter(string filePath, AGSSettings agsSettings = null, bool active = false)
        {
            if (File.Exists(filePath))
            {
                List<string> lines = File.ReadAllLines(filePath).ToList();

                // Determine where the section starts
                string group = "";
                List<string> headings = new List<string>();
                foreach (string line in lines)
                {
                    List<string> split = line.Split(new string[] { "\",\"" }, StringSplitOptions.None).ToList();
                    if (split.Count < 2)
                        continue;

                    switch (split[0]) // TODO: If there are risks that the file is incorectly formated, we need to add addtional checks (e.g missing section, nb of columns not matching)
                    {
                        case "\"GROUP":
                            group = split[1].Replace("\"", "");
                            m_Data[group] = new List<Dictionary<string, string>>();
                            break;
                        case "\"HEADING":
                            headings = split.Skip(1).Select(x => x.Replace("\"", "")).ToList();
                            break;
                        case "\"UNIT":
                            m_Units[group] = headings.Zip(split.Select(x => x.Replace("\"", "")).Skip(1), (h, u) => new { h, u }).ToDictionary(x => x.h, x => x.u);
                            break;
                        case "\"DATA":
                            m_Data[group].Add(headings.Zip(split.Select(x => x.Replace("\"", "")).Skip(1), (h, u) => new { h, u }).ToDictionary(x => x.h, x => x.u));
                            break;
                        default:   // TYPE is ignored for now as it doeesn't seem to be used anywhere
                            break;

                    }
                }
            }

            m_Settings = agsSettings;
            m_blankGeology = agsSettings.BlankGeology;
        }

        /***************************************************/
        /**** Private helper methods                    ****/
        /***************************************************/
        private string GetDirectoryRoot(string directory)
        {
            List<string> directoryRoot = directory.Split('\\').ToList();
            directoryRoot.RemoveAt(directoryRoot.Count - 1);

            return String.Join("\\", directoryRoot.ToArray());
        }


        /***************************************************/
        /**** Private  Fields                           ****/
        /***************************************************/

        private Dictionary<string, List<Dictionary<string, string>>> m_Data = new Dictionary<string, List<Dictionary<string, string>>>();
        private Dictionary<string, Dictionary<string, string>> m_Units = new Dictionary<string, Dictionary<string, string>>();
        private AGSSettings m_Settings = null;
        private string m_blankGeology;

        /***************************************************/
    }
}

