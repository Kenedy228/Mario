using System;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.Common;
using System.Drawing;
using OpenTK.Windowing.Common.Input;
using Mario;

namespace FlappyBird
{
    class Program
    {
        static void Main()
        {

            GameWindowSettings gSettings = new GameWindowSettings();
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