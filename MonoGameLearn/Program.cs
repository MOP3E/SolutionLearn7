using System;

namespace MonoGameLearn
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (Learn game = new Learn())
                game.Run();
        }
    }
}
