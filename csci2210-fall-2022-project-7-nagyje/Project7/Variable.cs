using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/////////////////////////////////////////
// 
// Author:  Joe Nagy, nagyje@etsu.edu
// Course CSCI 2210-001 - Data Structures
// Assignment: Project 7 - Calculator
//
/////////////////////////////////////////
namespace Project_7
{
    /// <summary>
    /// The Variable class contains a double and a String data
    /// field, which store the info required for use in the calculator.
    /// There is one auxiliary method: GetInfo()
    /// </summary>
    public class Variable
    {
        public double value { get; set; }
        public String name { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="Value">
        /// saved value of variable
        /// </param>
        /// <param name="Name">
        /// user's chosen call String for the variable
        /// </param>
        public Variable(double Value, string Name)
        {
            value = Value;
            name = Name;
        }

        /// <summary>
        /// Grabs the value and name of the variable, formats them,
        /// and returns them.
        /// </summary>
        /// <returns>
        /// String - "name:value"
        /// </returns>
        public String GetInfo()
        {
            String info =  name + ":" + value.ToString();
            return info;
        }
    }
}
