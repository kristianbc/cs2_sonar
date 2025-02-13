using System.Numerics;

namespace cs2_sonar
{
    public class Program
    {
        static void Main()
        {
            Base cheatbase = new Base();

            if(!cheatbase.Initialize())
            {
                return;
            }

            Thread sonarmodule = new Thread(() => Sonar.SonarModule(cheatbase));
            Thread buzzermodule = new Thread(() => Buzzer.BuzzerModule(cheatbase));
            //Thread uimodule = new Thread(() => UI.UIModule(sonarmodule, buzzermodule));

            sonarmodule.Start();
            buzzermodule.Start();
            //uimodule.Start();

        }

       /* public static bool ReturnThreadState(Thread thread)
        {
            if (thread.ThreadState == ThreadState.Running)
            {
                return true;
            }
            else
            {
                return false;
            }
        }*/
    }
}
