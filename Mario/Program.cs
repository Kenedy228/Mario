using System;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.Common;

namespace Mario
{
    class Program
    {
        static void Main()
        {

            GameWindowSettings gSettings = new GameWindowSettings() 
            {
                UpdateFrequency = 60.0
            };

            NativeWindowSettings nSettings = new NativeWindowSettings()
            {
                Title = "Mario",
                Size = (800, 600),
                Flags = ContextFlags.Default,
                Profile = ContextProfile.Compatability,
            };

            Game game = new Game(gSettings, nSettings);
            game.Run();
        }
    }
}