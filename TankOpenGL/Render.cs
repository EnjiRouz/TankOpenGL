using System;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using PixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;

namespace TankOpenGL
{
    internal static class Render
    {
        private static double _turretRotationAngle;
        private static double _perspective = 1.0;
        public static int[] TextureId;
        

        /// <summary>
        /// создание нового окна, рендеринг в нем, проверка нажатия горячих клавиш
        /// частота обновления - 60 кадров
        /// </summary>
        public static void RenderInNewWindow()
        {
            using (var window = new GameWindow(800, 800, 
                    new GraphicsMode(32, 24, 8, 1), "TankOpenGL ^_^"))
            {
                Load(window);
                Resize(window);
                Update(window);
                DrawScene(window);
                window.Run(60.0);
            }
        }

        /// <summary>
        /// настройки загрузки текстур, звука и прочего для диалогового окна
        /// </summary>
        /// <param name="window"> диалоговое окно, где происходит рисование сцены </param>
        private static void Load(GameWindow window)
        {
            window.Load += (sender, e) =>
            {
                window.VSync = VSyncMode.On;
                GL.DepthMask(true);

                // включение z-buffer
                GL.Enable(EnableCap.DepthTest);
                GL.DepthFunc(DepthFunction.Less);

                // освещение
                GL.Enable(EnableCap.Lighting);
                GL.Light(LightName.Light0, LightParameter.Ambient, new[] { 1.0f, 1.0f, 1.0f });
                GL.Light(LightName.Light0, LightParameter.Diffuse, new[] { 1.0f, 1.0f, 1.0f });
                GL.Light(LightName.Light0, LightParameter.Position, new[] { 1.0f, 0.0f, 0.0f });
                GL.Enable(EnableCap.Light0);

                GL.Light(LightName.Light1, LightParameter.Ambient, new[] { 1.0f, 1.0f, 1.0f });
                GL.Light(LightName.Light1, LightParameter.Diffuse, new[] { 1.0f, 1.0f, 1.0f });
                GL.Light(LightName.Light1, LightParameter.Position, new[] { 0.0f, 0.0f, 1.0f });
                GL.Enable(EnableCap.Light1);

                GL.Light(LightName.Light2, LightParameter.Ambient, new[] { 1.0f, 1.0f, 1.0f });
                GL.Light(LightName.Light2, LightParameter.Diffuse, new[] { 1.0f, 1.0f, 1.0f });
                GL.Light(LightName.Light2, LightParameter.Position, new[] { 0.0f, 1.0f, 0.0f });
                GL.Enable(EnableCap.Light2);


                //настройка загрузки и отображения текстур
                GL.Enable(EnableCap.Texture2D);
                TextureId = new int[6];

                GL.GenTextures(6, TextureId);                
                GenerateNewTexture(0, @"..\..\resourses\textures\body_1.bmp");
                GenerateNewTexture(1, @"..\..\resourses\textures\wheel_1.bmp");
                GenerateNewTexture(2, @"..\..\resourses\textures\track_1.bmp");
                GenerateNewTexture(3, @"..\..\resourses\textures\back.bmp");
                GenerateNewTexture(4, @"..\..\resourses\textures\barrel.bmp");
                GenerateNewTexture(5, @"..\..\resourses\textures\barrel_1.bmp");
            };
        }



        /// <summary>
        /// настройка изменения размера окна (приводит к неверному отображению картинки, как фиксить - пока хз)
        /// </summary>
        /// <param name="window"> диалоговое окно, где происходит рисование сцены </param>
        private static void Resize(INativeWindow window)
        {
            window.Resize += (sender, e) =>
            {
                GL.Viewport(0, 0, window.Width, window.Height);
            };
        }

        /// <summary>
        /// позвляет настроить события, происходящие при смене кадра, например, 
        /// управление клавишами (для поворота фигуры, закрытия окна и т.д.)
        /// </summary>
        /// <param name="window"> диалоговое окно, где происходит рисование сцены </param>
        private static void Update(GameWindow window)
        {
            window.UpdateFrame += (sender, e) =>
            {
                if (window.Keyboard[Key.Escape])
                {
                    window.Exit();
                }

                if (window.Keyboard[Key.Right])
                {
                    if (Math.Abs(_turretRotationAngle - 360.0) < 0.001)
                        _turretRotationAngle = 0.0;
                    else
                        _turretRotationAngle += 5.0;
                }

                if (window.Keyboard[Key.Left])
                {
                    if (Math.Abs(_turretRotationAngle - (-360.0)) < 0.001)
                        _turretRotationAngle = 0.0;
                    else
                        _turretRotationAngle -= 5.0;
                }

                if (window.Keyboard[Key.Up])
                {
                    _perspective += 0.1;
                }

                if (window.Keyboard[Key.Down])
                {
                    _perspective -= 0.1;
                }
            };
        }

        /// <summary>
        /// здесь происходит рендеринг графики 
        /// </summary>
        /// <param name="window"> диалоговое окно, где происходит рисование сцены </param>
        private static void DrawScene(GameWindow window)
        {
            window.RenderFrame += (sender, e) =>
            {
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
                GL.Color3(Color.White);
                GL.MatrixMode(MatrixMode.Projection);
                GL.LoadIdentity();
                GL.Scale(_perspective, _perspective, _perspective);

                // фигуры
                Tank.DrawTank(_turretRotationAngle);                

                // поворот фигуры
                GL.MatrixMode(MatrixMode.Modelview);
                GL.Rotate(1.0, 1.0, 0.0, 0.0);
                GL.Rotate(1.0, 0.0, 1.0, 0.0);
                window.SwapBuffers();
            };
        }

        private static BitmapData LoadImage(string filePath)
        {
            var bmp = new Bitmap(filePath);
            var rectangle = new Rectangle(0, 0, bmp.Width, bmp.Height);
            var bmpData = bmp.LockBits(rectangle,ImageLockMode.ReadOnly,
                                                                    System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            bmp.UnlockBits(bmpData);
            return bmpData;
        }

        private static void GenerateNewTexture(int id, string filePath)
        {
            GL.BindTexture(TextureTarget.Texture2D, TextureId[id]);
            var textureData = LoadImage(filePath);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb,
                          textureData.Width, textureData.Height, 0,
                          PixelFormat.Bgr, PixelType.UnsignedByte,
                          textureData.Scan0);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        }
    }
}