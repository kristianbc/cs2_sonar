using System.Numerics;
using System.Reflection;

namespace cs2_sonar
{
    public class Program
    {
        static void Main()
        {
            Base cheatbase = new Base();
            if (!cheatbase.Initialize())
            {
                return;
            }

            Thread uimodule = new Thread(() => UI.UIModule());
            uimodule.Start();

            new Thread(() => ThreadToggle.ToggleModule(0x70, token => Buzzer.BuzzerModule(cheatbase, token), ref ThreadToggle.buzzerEnabled)).Start();
            new Thread(() => ThreadToggle.ToggleModule(0x71, token => Sonar.SonarModule(cheatbase, token), ref ThreadToggle.sonarEnabled)).Start();
        }
    }
}
