using System;

namespace TankOpenGL
{
    internal static class Program
    {
        /// <summary>
        /// точка входа в приложение
        /// </summary>
        [STAThread]
        public static void Main()
        {
            Render.RenderInNewWindow();
        }
    }
}