using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;
using System.Windows.Forms;

namespace ElevatorSystemConsoleProgram
{
    public class ElevatorProgram
    {   
        //Global Variables representing Command Line Arguments
        public static string InvokeProgramCommand = String.Empty;       
        public static string ElevatorSystemFilePath = String.Empty;
        public static char StartingElevator;                            
        public static int FinalFloor = 0;
        public static int FinalTime = 0;
        public static int CurrentState = 0;

        //Main Entry Point of the Elevator System Console Application
        static void Main(string[] args)
        {
            try
            {
                ArrayList validActionStringsList = new ArrayList();

                ParseCommandLineArgs(args);

                //The Command to Invoke the Program must be called InvokeProgram
                if (InvokeProgramCommand.Equals("InvokeProgram"))
                {
                    /*Create an object encasulating elevator states representative of the
                    *correctly formatted Input File*/
                    ElevatorState elevatorStateCollection = new ElevatorState(ElevatorSystemFilePath);

                    //Get a list of valid action strings
                    validActionStringsList = elevatorStateCollection.FindValidActionStrings(StartingElevator, FinalFloor, FinalTime);

                    /*If there were valid action strings that were found print them out
                     * Otherwise, throw an exception since no input matched the Command Line arguments*/
                    if (validActionStringsList != null)
                    {
                        BuildingUtilityClass.PrintValidActionStrings(validActionStringsList, FinalTime);
                    }
                    else
                    {
                        throw new Exception("No Solution");
                    }
                }
                else
                {
                    throw new Exception("No Solution");
                }
            }
            catch (Exception ex)
            {
                //If there is no solution, print out "No Solution" to stderr or Console.Error and nothing to Console.WriteLine
                Console.Error.WriteLine("No Solution");
            }
        }

        /// <summary>
        /// Call this static Method to parse the CommandLine Arguments
        /// </summary>
        /// <param name="args">These args are to be passed in via CommandLine</param>
        public static void ParseCommandLineArgs(string[] args)
        {
            try
            {
                string finalDestination = String.Empty;     //temporary variable used to parse FinalFloor and FinalTime

                InvokeProgramCommand = args[0];
                ElevatorSystemFilePath = args[1];
                StartingElevator = Char.Parse(args[2].ToString());
                finalDestination = args[3];

                string[] finalDestinationParts = finalDestination.Split('-');

                FinalFloor = Int32.Parse(finalDestinationParts[0].ToString());
                FinalTime = Int32.Parse(finalDestinationParts[1].ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
