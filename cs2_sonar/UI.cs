using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace cs2_sonar
{
    public class UI
    {
        public static void UIModule()
        {
            while (true)
            {
                Console.WriteLine($"Buzzer: F1 --> STATUS: {(ThreadToggle.buzzerEnabled ? "ON" : "OFF")}");
                Console.WriteLine($"Sonar: F2 --> STATUS: {(ThreadToggle.sonarEnabled ? "ON" : "OFF")}");

                Thread.Sleep(1000);
                Console.Clear();
            }
        }
    }
}
