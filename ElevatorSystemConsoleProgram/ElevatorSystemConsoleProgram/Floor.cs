using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace ElevatorSystemConsoleProgram
{
    /// <summary>
    /// Class Representing a Collection of a particular Elevator State.
    /// Each Floor can have many Elevators
    /// </summary>
    public class Floor
    {
        public ArrayList floor;         //Represents the layout of the floor, which can contain elevators, shafts and walls
        public int floorNumber;

        public Floor()
        {
            floor = new ArrayList();
        }

        /// <summary>
        /// Overloaded Constructor that iterates through a floor string and creates corresponding Elevator objects
        /// </summary>
        /// <param name="flr">pass in a string representing the floor</param>
        /// <param name="floorNumber">indicates the floor number</param>
        /// <param name="floorState">indicates the state or T Value that the floor belongs to in the Elevator System</param>
        public Floor(string flr, int floorNumber, int floorState)
        {
            floor = new ArrayList();

            if (flr != null)
            {
                foreach (char c in flr)
                {
                        Elevator elevator = new Elevator(c, floorNumber,floorState);
                        this.floor.Add(elevator);
                }
            }
        }

        /// <summary>
        /// Call this method to change the T value of all Elevators on the current floor
        /// </summary>
        /// <param name="newFloorState">Indicates the new T value</param>
        public void SetAllElevatorStates(int newFloorState)
        {
            foreach (Elevator elev in floor)
            {
                elev.floorState = newFloorState;
            }
        }

        /// <summary>
        /// Call this method to change the floor number of all Elevators on the current floor
        /// </summary>
        /// <param name="floorNumber"></param>
        public void SetAllElevatorFloor(int floorNumber)
        {
            foreach (Elevator elev in floor)
            {
                elev.elevatorFloorNumber = floorNumber;
            }
        }

        /// <summary>
        /// Call this method to see if an Elevator belongs to the current floor
        /// </summary>
        /// <param name="elevatorChar">Char value specifing the elevator to check</param>
        /// <returns>Return an Elevator object. If the elevator isn't on the floor, return null</returns>
        public Elevator IsElevatorOnFloor(char elevatorChar)
        {
            Elevator elevator = null;
            foreach (Elevator elev in floor)
            {
                if (elev.elevatorName == elevatorChar)
                {
                    return (elev);
                }
            }

            return elevator;
        }

        /// <summary>
        /// Call this method to retrieve a specified Elevator from the floor
        /// </summary>
        /// <param name="c">Char value specifing an elevator to retrieve</param>
        /// <returns></returns>
        public Elevator GetElevator(char c)
        {
            Elevator elevator = IsElevatorOnFloor(c);

            if (IsElevatorOnFloor(c) != null)
            {
                return elevator;
            }

            return null;

        }

        /// <summary>
        /// Call this method to get a Collection of all Elevators on the Floor
        /// </summary>
        /// <returns>Return a Collection of all Elevators on the floor</returns>
        public ArrayList GetAllElevatorsOnFloor()
        {
            ArrayList allElevatorsOnFloor = new ArrayList();

            foreach (Elevator elevator in this.floor)
            {
                int currentCharAsciiCode = elevator.elevatorName;

                //If the Elevator object is a valid Elevator (A-Z), then add it to the collection
                if (currentCharAsciiCode >= 65 && currentCharAsciiCode <= 90)
                {
                    allElevatorsOnFloor.Add(elevator);
                }
            }

            return allElevatorsOnFloor;
        }
    }
}
