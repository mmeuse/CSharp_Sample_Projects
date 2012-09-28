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
    /// Class that represents a Collection of Elevator States and methods
    /// that allow Calling class to determine a valid set of actions that 
    /// can be followed according to Elevator System Input Parameters and 
    /// Command Line arguments
    /// </summary>
    class ElevatorState
    {
        private Hashtable ElevatorStates;           //Represents a hashtable or collection of all Elevator States
        private Hashtable ElevatorFloors;           //Represents a hashtable of all floors belonging to each Elevator State
        private int NumberOfFloorsInTheBuilding;    //Indicates the standard number of floors across all states of the same Elevator System
        public ArrayList ActionStringList;          //Collection of valid Action Strings
        private TreeView elevatorTree = new TreeView();

        public ElevatorState()
        {
            ElevatorStates = new Hashtable();
            ElevatorFloors = new Hashtable();
            NumberOfFloorsInTheBuilding = 0;
            ActionStringList = new ArrayList();
        }

        /// <summary>
        /// Overloaded Constructor to parse the Elevator System Input File 
        /// and store the parsed information into a complex set of data structures
        /// </summary>
        /// <param name="filePath">Specifies the filePath of the Elevator System Input File</param>
        public ElevatorState(string filePath)
        {
            ElevatorStates = new Hashtable();
            ElevatorFloors = new Hashtable();
            NumberOfFloorsInTheBuilding = 0;
            ActionStringList = new ArrayList();
            int currentFloor = 1;
            int currentState = 1;

            //Open the Input file
            using (StreamReader sr = new StreamReader(filePath))
            {
                try
                {
                    string floorStr = String.Empty;

                    //Read in lines from the Input file until EOF
                    while (!sr.EndOfStream)
                    {
                        //Get a floor from the Input file
                        floorStr = sr.ReadLine();

                        /*If the floor is not an empty line or the EOF,
                         * get the current floor and instantiate a Floor object with the input string.
                         * After doing so, hash this floor and continue to read in all floors 
                         * until the current State (T) is complete. Each State is seperated by an Empty String
                         * */
                        while (floorStr != String.Empty && floorStr != null)
                        {
                            if (floorStr != null)
                            {
                                ElevatorFloors.Add(currentFloor, new Floor(floorStr, currentFloor, currentState));
                                floorStr = sr.ReadLine();
                                currentFloor++;
                            }
                        }

                        /*If all floors were retrieved for the current state,
                         * add these floors to the ElevatorStates hash table
                         * and read in the next Elevator State from the Input File
                         */
                        if (ElevatorFloors != null && ElevatorFloors.Count > 0)
                        {
                            NumberOfFloorsInTheBuilding = ElevatorFloors.Count;
                            ElevatorStates.Add(currentState, ElevatorFloors);
                            ElevatorFloors = new Hashtable();
                            currentFloor = 1;
                            currentState++;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                ReverseFloors();
            }
        }

        /// <summary>
        /// Call this method to reverse all floors for all States since floors for each state
        /// were parsed from Top Floor to Bottom Floor and assigned a unique Key. In order to create
        /// a Tree Structure, it is expected that floor 1 have index 1. Hence the need for reversal.
        /// </summary>
        private void ReverseFloors()
        {
            for (int i = 1; i <= ElevatorStates.Count; i++)
            {
                Hashtable newFloorForCurrent = new Hashtable();

                //Get the Floor representation for the current state
                Hashtable allFloorsAtCurrentState = (Hashtable)ElevatorStates[i];

                int highestKey = allFloorsAtCurrentState.Count;
                int newKey = 1;

                while (highestKey >= 1)
                {
                    //Get Highest Floor
                    Floor f = (Floor)allFloorsAtCurrentState[highestKey];
                    f.SetAllElevatorStates(i);
                    f.SetAllElevatorFloor(newKey);
                    newFloorForCurrent.Add(newKey, f);
                    highestKey--;
                    newKey++;
                }

                ElevatorStates[i] = newFloorForCurrent;
            }
        }

        /// <summary>
        /// Call this method to find a valid set of actions according to Input File Information and CMD Line Args
        /// </summary>
        /// <param name="startingElevator">Specifies the Elevator to Start</param>
        /// <param name="finalFloor">Specifies the Final Floor to evaluate. Associated with finalState</param>
        /// <param name="finalState">Specifies the Final T value to evaluate</param>
        /// <returns></returns>
        public ArrayList FindValidActionStrings(char startingElevator, int finalFloor, int finalState)
        {
            try
            {
                //Create a tree structure
                CreateElevatorTree(startingElevator, finalState, finalFloor);

                //Search the tree structure for valid action strings
                FindActionString(this.elevatorTree);

                return this.ActionStringList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// In order to find a valid set of actions, we need to create a Tree structure of
        /// all possible paths that the Elevator can take to reach the Final Floor in the specified time
        /// T. It is important to note that these paths must include transfers to other elevators on the
        /// same floor. Once we build the tree structure, it will be easy to perform a Breath First Search
        /// to find a valid set of actions.
        /// Note that the tree is unbalanced and is not binary. Perhaps, it could be enhanced to improve
        /// Performance for Best and Worst Case scenarios.
        /// </summary>
        /// <param name="startingElevator">Specifies the Starting Elevator</param>
        /// <param name="finalState">Specifies Final Time T</param>
        /// <param name="finalFloor">Specifies Final Floor</param>
        private void CreateElevatorTree(char startingElevator, int finalState, int finalFloor)
        {
            /*Create a Tree Structure to represent a directed graph 
            *of all possible paths for the startingElevator until it reaches the Final Floor and Final State (T)*/
            ElevatorProgram.CurrentState = 1;

            //Retrieve the Starting Elevator from T = 1
            Elevator currentElevatorObj = GetElevatorFromCurrentState(startingElevator);
            TreeNode tempNode = new TreeNode();

            if (currentElevatorObj != null)
            {
                tempNode = elevatorTree.Nodes.Add("RootNode");
                tempNode.Tag = null;

                //Pass in a Root Node to GetChildren in order to find a child nodes representing valid actions
                GetChildren(tempNode, currentElevatorObj);
            }
        }

        /// <summary>
        /// Call this method to find a valid set of actions according to 
        /// Starting Elevator,Final Floor and Final State
        /// </summary>
        /// <param name="actionTree">Specifies the Tree Structure to Search</param>
        private void FindActionString(TreeView actionTree)
        {
            foreach (TreeNode n in actionTree.Nodes)
            {
                FindActionPathToFinalFloorAtFinalState(n);
            }
        }

        /// <summary>
        /// Recursive method to build action string
        /// </summary>
        /// <param name="treeNode">Current Node to check in the Tree</param>
        private void FindActionPathToFinalFloorAtFinalState(TreeNode treeNode)
        {
            foreach (TreeNode tn in treeNode.Nodes)
            {
                //Get the current elevator object specified by the current node in the tree
                Elevator currentElevatorNode = (Elevator)tn.Tag;

                //If the current elevator doesnt belongs to the Final Floor at State T,
                //continue to search the tree
                if (!IsCurrentElevatorAFinalNode(currentElevatorNode))
                {
                    FindActionPathToFinalFloorAtFinalState(tn);
                }
                else
                {
                    //If the current elevator does belong to the Final foor at State T,
                    //create an Action string by retrieving all Parent Node values
                    TreeNode currentNode = tn.Parent;
                    string actionString = String.Empty;

                    while (currentNode.Text != "RootNode")
                    {
                        actionString += currentNode.Text;
                        currentNode = currentNode.Parent;
                    }

                    //Reverse the ActionString since it was build from Final Node to Root Node
                    actionString = BuildingUtilityClass.ReverseString(actionString);

                    //Add the action string to an ActionString list of valid actions
                    if (!ActionStringList.Contains(actionString))
                    {
                        if (actionString != String.Empty)
                        {
                            ActionStringList.Add(actionString);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Call this method to see if the current elevator is on the Final Floor at time T
        /// </summary>
        /// <param name="currentElevatorNode"></param>
        /// <returns></returns>
        private bool IsCurrentElevatorAFinalNode(Elevator currentElevatorNode)
        {
            if (currentElevatorNode.floorState == ElevatorProgram.FinalTime &&
                    currentElevatorNode.elevatorFloorNumber == ElevatorProgram.FinalFloor)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Call this method to retrieve and Elevator object from the Current State
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        private Elevator GetElevatorFromCurrentState(char c)
        {
            //Get all Floors at State 1
            Hashtable allFloorsAtCurrentState = (Hashtable)ElevatorStates[ElevatorProgram.CurrentState];
            Elevator startingElevatorObj = null;

            //Parse all Floors for the current state in order to retrive an Elevator object
            foreach (Floor f in allFloorsAtCurrentState.Values)
            {
                startingElevatorObj = f.GetElevator(c);

                if (startingElevatorObj != null)
                {
                    return startingElevatorObj;
                }
            }

            return null;
        }

        /// <summary>
        /// Recursive method to find all possible paths that the Starting Elevator can take until time T.
        /// </summary>
        /// <param name="parentNode"></param>
        /// <param name="currentElevator"></param>
        private void GetChildren(TreeNode parentNode, Elevator currentElevator)
        {
            Elevator nextElevator = null;

            //Get a list of all Elevators in the Current State
            ArrayList allElevatorsOnFloor = FindAllElevatorsForCurrentState(currentElevator);

            try
            {
                if (currentElevator.floorState <= ElevatorProgram.FinalTime)
                {
                    foreach (Elevator elevator in allElevatorsOnFloor)
                    {
                        //Create a tree node object for all Elevators in the Current State
                        TreeNode Node = new TreeNode(elevator.elevatorName.ToString());
                        Node.Tag = elevator;

                        parentNode.Nodes.Add(Node);

                        //Find the next State of the current Elevator and call this method recursively
                        //to find all of its Child Nodes or possible path until time T
                        nextElevator = FindNextElevator(elevator);

                        if (nextElevator != null)
                        {
                            GetChildren(Node, nextElevator);
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// Call this method to obtain a list of all Elevators in the Current State
        /// </summary>
        /// <param name="currentElevator"></param>
        /// <returns></returns>
        private ArrayList FindAllElevatorsForCurrentState(Elevator currentElevator)
        {
            //Get all Floors at State 1
            Hashtable allFloorsAtCurrentState = (Hashtable)ElevatorStates[currentElevator.floorState];
            ArrayList allElevatorsOnFloor = null;
            Floor currentFloor = null;

            foreach (Floor f in allFloorsAtCurrentState.Values)
            {
                //Find out which floor that the current elevator is on
                if (f.IsElevatorOnFloor(currentElevator.elevatorName) != null)
                {
                    currentFloor = f;
                    break;
                }
            }

            if (currentFloor != null)
            {
                //Get a list of all Elevators on the floor
                allElevatorsOnFloor = currentFloor.GetAllElevatorsOnFloor();
            }

            return allElevatorsOnFloor;
        }

        /// <summary>
        /// Call this method to find out where a current elevator will be in the next State
        /// </summary>
        /// <param name="currentElevator"></param>
        /// <returns></returns>
        private Elevator FindNextElevator(Elevator currentElevator)
        {
            int nextStateOfCurrentElevator = currentElevator.floorState + 1;
            Elevator elevator = null;

            if (currentElevator != null && nextStateOfCurrentElevator <= ElevatorProgram.FinalTime)
            {
                //Get a list of all floors at the current state
                Hashtable allFloorsAtCurrentState = (Hashtable)ElevatorStates[nextStateOfCurrentElevator];

                foreach (Floor f in allFloorsAtCurrentState.Values)
                {
                    //Return the Elevator object as soon as you find it in the Current State
                    elevator = f.IsElevatorOnFloor(currentElevator.elevatorName);

                    if (elevator != null)
                    {
                        break;
                    }
                }
            }

            return elevator;
        }
    }
}
