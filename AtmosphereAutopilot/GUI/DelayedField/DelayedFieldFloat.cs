﻿/*
Atmosphere Autopilot, plugin for Kerbal Space Program.
Copyright (C) 2015-2016, Baranin Alexander aka Boris-Barboris.
 
Atmosphere Autopilot is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.
Atmosphere Autopilot is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.
You should have received a copy of the GNU General Public License
along with Atmosphere Autopilot.  If not, see <http://www.gnu.org/licenses/>. 
*/

//
// THIS FILE IS AUTO-GENERATED
//

using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace AtmosphereAutopilot
{

    /// <summary>
    /// Class for organizing delayed textfield input. Use DisplayLayout() to integrate into OnGUI code,
    /// use OnUpdate() to account for time.
    /// </summary>
    public class DelayedFieldFloat
    {
        /// <summary>
        /// Underlying float value to represent
        /// </summary>
        protected float val;

        /// <summary>
        /// Time in seconds after input string was changed by user input
        /// </summary>
        protected float time;

        /// <summary>
        /// true when we're counting time
        /// </summary>
        protected bool changed;

        /// <summary>
        /// String that holds input value
        /// </summary>
        public string input_str;

        /// <summary>
        /// String that holds conversion formatting
        /// </summary>
        public string format_str;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="init_value">Initial value</param>
        /// <param name="format"></param>
        public DelayedFieldFloat(
            float init_value
            , string format
        )
        {
            val = init_value;
            time = 0.0f;
            changed = false;
            input_str = val.ToString(format);
            format_str = format;
        }
        
        public static DelayedFieldFloat operator +(DelayedFieldFloat lhs, float rhs) {
            if (rhs == 0) return lhs;

            lhs.ParseImmediately();
            lhs.val += rhs;
            lhs.input_str = lhs.val.ToString(lhs.format_str);
            return lhs;
        }
        public static DelayedFieldFloat operator -(DelayedFieldFloat lhs, float rhs) {
            return lhs + (-rhs);
        }

        public static implicit operator float(DelayedFieldFloat f)
        {
            return f.val;
        }

        public float Value
        {
            get { return val; }
            set
            {
                if (value != val)
                {
                    changed = false;
                    time = 0.0f;
                    input_str = value.ToString(format_str);
                    val = value;
                }
            }
        }

        public void DisplayLayout(GUIStyle style, params GUILayoutOption[] options)
        {
            string new_str = GUILayout.TextField(input_str, style, options);
            if (!input_str.Equals(new_str))
            {
                changed = true;
                time = 0.0f;
            }
            input_str = new_str;
        }

        public void OnUpdate()
        {
            if (changed)
                time += Time.deltaTime;
            if (time >= 2.0f)
            {
                time = 0.0f;
                changed = false;
                {
                    float v;
                    if (float.TryParse(input_str, out v))
                        this.val = v;
                    else
                        this.input_str = this.val.ToString(format_str);
                }
            }
        }

        public override string ToString()
        {
            return val.ToString() + "_" + format_str;
        }

        public string ToString(string format)
        {
            return val.ToString(format) + "_" + format_str;
        }

        public static DelayedFieldFloat Parse(string str)
        {
            int delim_index = str.IndexOf('_');
            if (delim_index < 0)
            {
                return new DelayedFieldFloat(0.0f, "G4");
            }
            else
            {
                string val_str = str.Substring(0, delim_index);
                string format_str = str.Substring(delim_index + 1);
                float new_val = 0.0f;
                float.TryParse(val_str, out new_val);
                return new DelayedFieldFloat(new_val, format_str);
            }
        }

        //Doesn't update the string, do that yourself after
        private void ParseImmediately() {
            if (changed && float.TryParse(input_str, out float v)) {
                changed = false;
                time = 0.0f;
                val = v;
            }
        }

        public void InvertValue() {
            ParseImmediately();
            val = -val;
            input_str = val.ToString(format_str);
        }
    }
}