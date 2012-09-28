using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElevatorSystemConsoleProgram
{
    /// <summary>
    /// Class representing a single elevator object.
    /// A single elevator object can be an Elevator (A-Z),wall (x) or shaft(.)
    /// </summary>
    public class Elevator
    {
        public char elevatorName;
        public int elevatorFloorNumber;
        public int floorState;

        public Elevator()
        {
             elevatorName = ' ';
             elevatorFloorNumber = 0;
             floorState = 1;
        }

        /// <summary>
        /// Overloaded Constructor
        /// </summary>
        /// <param name="elevatorName">Specifies the elevator name</param>
        /// <param name="elevatorFloorNumber">Specifies the floor in which the elevator belongs</param>
        /// <param name="floorState">Specifies the T value of the current Elevator</param>
        public Elevator(char elevatorName, int elevatorFloorNumber, int floorState)
        {
            this.elevatorName = elevatorName;
            this.elevatorFloorNumber = elevatorFloorNumber;
            this.floorState = floorState;
        }
    }
}
