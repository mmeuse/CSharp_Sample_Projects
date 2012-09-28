using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;
using System.Windows.Forms;
namespace ElevatorSystemConsoleProgram
{
    /// <summary>
    /// Utility Class containing static methods that are generic to the Elevator System Program
    /// </summary>
    class BuildingUtilityClass
    {
        /// <summary>
        /// Call this static method to determine if the action string is valid
        /// </summary>
        /// <param name="validActionString">Specifies the Action String to Evaluate</param>
        /// <param name="finalState">Specifies the Final T Value</param>
        /// <returns>Return a boolean indicating if the action string is valid</returns>
        public static bool IsValidActionString(string validActionString, int finalTime)
        {
            bool isValidResult = false;

            try
            {
                if (validActionString != String.Empty && validActionString != null)
                {
                    /*The action string is valid if it has T-1 Actions in it and that
                    *last elevator @ T-1 will arrive @ State T*/
                    if (validActionString.Length == finalTime - 1)
                    {
                        isValidResult = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return isValidResult;
        }

        /// <summary>
        /// Used to Reverse a String
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ReverseString(string s)
        {
            char[] arr = s.ToCharArray();
            Array.Reverse(arr);
            return new string(arr);
        }

        /// <summary>
        /// Prints out all action strings in the ValidActionStringsList Collection
        /// </summary>
        /// <param name="validActionStringsList">Specifies a Collection of valid action strings</param>
        /// <param name="finalState">Specifies the Final Time or State in the Elevator System</param>
        public static void PrintValidActionStrings(ArrayList validActionStringsList,int finalState)
        {
            if (validActionStringsList.Count == 0)
            {
                throw new Exception("No Solution");
            }

            foreach (string actionString in validActionStringsList)
            {
                //Check if the Action String is valid before printing
                if (BuildingUtilityClass.IsValidActionString(actionString, finalState))
                {
                    Console.WriteLine(actionString);
                }
                else
                {
                    Console.Error.WriteLine("No Solution");
                }
            }
        }

        /// <summary>
        /// Prints out a single action string
        /// </summary>
        /// <param name="actionString">Action String to print out</param>
        /// <param name="finalState">Specifies the Final Time or State in the Elevator System</param>
        public static void PrintValidActionString(string actionString, int finalState)
        {
            //Check if the Action String is valid before printing
            if (BuildingUtilityClass.IsValidActionString(actionString, finalState))
            {
                Console.WriteLine(actionString);
            }
            else
            {
                Console.Error.WriteLine("No Solution");
            }
        }
    }
}
