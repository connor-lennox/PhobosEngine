using System;

namespace PhobosEngine
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new PhobosEngine())
                game.Run();
        }
    }
}
