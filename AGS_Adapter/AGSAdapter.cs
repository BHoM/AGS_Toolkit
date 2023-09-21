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
        public AGSAdapter(string filePath, AGSConfig agsConfig = null, bool active = false)
        {
            if (File.Exists(filePath))
            {
                m_directory = GetDirectoryRoot(filePath);
                string textFiles = m_directory + "\\Text Files";
                Directory.CreateDirectory(textFiles);

                List<string> agsFile = File.ReadAllLines(filePath).ToList();
                List<int> groupHeadings = new List<int>();

                // Determine where the section starts
                for (int i = 0; i < agsFile.Count; i++)
                {
                    string line = agsFile[i];
                    if (!(line.Length < 5))
                    {
                        if (line.Split(',')[0].Contains("\"GROUP\""))
                            groupHeadings.Add(i);
                    }
                }

                // Seperate out the text files
                for (int i = 0; i < groupHeadings.Count - 1; i++)
                {
                    int groupHeading = groupHeadings[i];
                    List<string> section = agsFile.GetRange(groupHeading, groupHeadings[i + 1] - groupHeading -1);
                    string sectionHeading = section[0].Split(',')[1].Replace('"', ' ').Trim();
                    File.WriteAllLines(textFiles + "\\" + sectionHeading + ".txt",section);
                }
            }

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

        private string m_directory;

        /***************************************************/
    }
}

