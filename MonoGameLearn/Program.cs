using System;

namespace MonoGameLearn
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new Learn())
                game.Run();
        }
    }
}
