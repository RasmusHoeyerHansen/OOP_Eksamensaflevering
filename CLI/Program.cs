using Controller;
using System;
using System.Collections;
using System.Collections.Generic;

namespace UI
{
    class Program
    {
        static void Main(string[] args)
        {
            StregSystemKerne sys = new StregSystemKerne();
            UI cli = new UI(sys);
            cli.Start();
        }
    }
}
