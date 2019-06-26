using System;
using System.Drawing;
using OpenTK.Graphics.OpenGL;

namespace TankOpenGL
{
    internal static class ShapeDrawer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="top"></param>
        /// <param name="height"></param>
        /// <param name="bottom"></param>
        public static void DrawClippedPyramid(double top, double height, double bottom)
        {
            GL.Begin(PrimitiveType.Quads);
            GL.Color3(Color.Red);
            GL.Vertex3(top, height, top);
            GL.Vertex3(-top, height, top);
            GL.Vertex3(-bottom, -height, bottom);
            GL.Vertex3(bottom, -height, bottom);

            GL.Color3(Color.LawnGreen);
            GL.Vertex3(top, height, -top);
            GL.Vertex3(top, height, top);
            GL.Vertex3(bottom, -height, bottom);
            GL.Vertex3(bottom, -height, -bottom);

            GL.Color3(Color.Violet);
            GL.Vertex3(top, height, -top);
            GL.Vertex3(-top, height, -top);
            GL.Vertex3(-bottom, -height, -bottom);
            GL.Vertex3(bottom, -height, -bottom);

            GL.Color3(Color.Blue);
            GL.Vertex3(-top, height, top);
            GL.Vertex3(-top, height, -top);
            GL.Vertex3(-bottom, -height, -bottom);
            GL.Vertex3(-bottom, -height, bottom);
            GL.End();

            GL.Begin(PrimitiveType.Quads);
            GL.Color3(Color.PaleVioletRed);
            GL.Vertex3(-bottom, -height, bottom);
            GL.Vertex3(-bottom, -height, -bottom);
            GL.Vertex3(bottom, -height, -bottom);
            GL.Vertex3(bottom, -height, bottom);

            GL.Vertex3(-top, height, top);
            GL.Vertex3(-top, height, -top);
            GL.Vertex3(top, height, -top);
            GL.Vertex3(top, height, top);
            GL.End();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sideSize"></param>
        public static void DrawPyramide(double sideSize)
        {
            GL.Begin(PrimitiveType.Triangles);
            GL.Vertex3(0.0, sideSize, 0.0);
            GL.Vertex3(-sideSize, -sideSize, sideSize);
            GL.Vertex3(sideSize, -sideSize, sideSize);

            GL.Vertex3(0.0, sideSize, 0.0);
            GL.Vertex3(sideSize, -sideSize, sideSize);
            GL.Vertex3(sideSize, -sideSize, -sideSize);

            GL.Vertex3(0.0, sideSize, 0.0);
            GL.Vertex3(sideSize, -sideSize, -sideSize);
            GL.Vertex3(-sideSize, -sideSize, -sideSize);

            GL.Vertex3(0.0, sideSize, 0.0);
            GL.Vertex3(-sideSize, -sideSize, -sideSize);
            GL.Vertex3(-sideSize, -sideSize, sideSize);
            GL.End();

            GL.Begin(PrimitiveType.Quads);
            GL.Vertex3(-sideSize, -sideSize, sideSize);
            GL.Vertex3(-sideSize, -sideSize, -sideSize);
            GL.Vertex3(sideSize, -sideSize, -sideSize);
            GL.Vertex3(sideSize, -sideSize, sideSize);
            GL.End();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="radius"></param>
        /// <param name="height"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="stepSize"></param>
        /// <param name="isFilled"></param>
        public static void DrawCylinder(double radius, double height, double x, double y, double stepSize, bool isFilled)
        {
            var angle=0.0;
            if (isFilled)
            {
                GL.Begin(PrimitiveType.QuadStrip);
                CalculateCylinder(radius, height, ref x, ref y, stepSize, ref angle);
                GL.End();

                // рисует круг на вершине                
                DrawCircle3D(radius, height, x, y, stepSize);
                DrawCircle3D(radius, 0.0, x, y, stepSize);
            }
            else
            {
                GL.Begin(PrimitiveType.Lines);
                CalculateCylinder(radius, height, ref x, ref y, stepSize, ref angle);
                GL.End();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="radius"></param>
        /// <param name="height"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="stepSize"></param>
        public static void DrawCircle3D(double radius, double height, double x, double y, double stepSize)
        {
            GL.Begin(PrimitiveType.Polygon);
            var angle = 0.0;
            while (angle < 2 * Math.PI)
            {
                x = radius * Math.Cos(angle);
                y = radius * Math.Sin(angle);
                GL.TexCoord2(x / radius * 0.5 + 0.5, y / radius * 0.5 + 0.5);
                GL.Vertex3(x, y, height);                
                angle += stepSize;
            }
            GL.Vertex3(radius, 0.0, height);
            GL.End();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="radius"></param>
        /// <param name="height"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="stepSize"></param>
        /// <param name="angle"></param>
        private static void CalculateCylinder(double radius, double height, ref double x, ref double y, double stepSize, ref double angle)
        {
            while (angle < 2 * Math.PI)
            {
                GL.Normal3(Math.Cos(angle), 1.0, Math.Sin(angle));
                x = radius * Math.Cos(angle);
                y = radius * Math.Sin(angle);
                GL.TexCoord2(angle / (Math.PI), 1.0);
                GL.Vertex3(x, y, height);
                GL.TexCoord2(angle / (Math.PI), 0.0);
                GL.Vertex3(x, y, 0.0);
                angle += stepSize;
            }
            GL.Vertex3(radius, 0.0, height);
            GL.Vertex3(radius, 0.0, 0.0);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="wX"></param>
        /// <param name="wY"></param>
        /// <param name="wZ"></param>
        /// <param name="isFilled"></param>
        public static void DrawParallelepiped(double x, double y, double z, double wX, double wY, double wZ, bool isFilled)
        {
            var widthX = wX + x;
            var widthY = wY + y;
            var widthZ = wZ + z;

            if (isFilled)
            {
                GL.Begin(PrimitiveType.Quads);
                CalculateParallelepiped(x, y, z, widthX, widthY, widthZ);
                GL.End();
            }
            else
            {
                GL.Begin(PrimitiveType.LineStrip);
                CalculateParallelepiped(x, y, z, widthX, widthY, widthZ);
                GL.End();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="widthX"></param>
        /// <param name="widthY"></param>
        /// <param name="widthZ"></param>
        private static void CalculateParallelepiped(double x, double y, double z, double widthX, double widthY, double widthZ)
        {
            // задняя часть
            GL.Normal3(0.0, 0.0, -1.0);
            GL.TexCoord2(1.0, 1.0);
            GL.Vertex3(x, y, z);
            GL.TexCoord2(0.0, 1.0);
            GL.Vertex3(widthX, y, z);
            GL.TexCoord2(0.0, 0.0);
            GL.Vertex3(widthX, widthY, z);
            GL.TexCoord2(1.0, 0.0);
            GL.Vertex3(x, widthY, z);

            // передняя часть
            GL.Normal3(0.0, 0.0, 1.0);
            GL.TexCoord2(0.0, 0.0);
            GL.Vertex3(x, y, widthZ);
            GL.TexCoord2(1.0, 0.0);
            GL.Vertex3(widthX, y, widthZ);
            GL.TexCoord2(1.0, 1.0);
            GL.Vertex3(widthX, widthY, widthZ);
            GL.TexCoord2(0.0, 1.0);
            GL.Vertex3(x, widthY, widthZ);

            // левая часть
            GL.Normal3(-1.0, 0.0, 0.0);
            GL.TexCoord2(0.0, 1.0);
            GL.Vertex3(x, y, z);
            GL.TexCoord2(1.0, 1.0);
            GL.Vertex3(x, y, widthZ);
            GL.TexCoord2(1.0, 0.0);
            GL.Vertex3(x, widthY, widthZ);
            GL.TexCoord2(0.0, 0.0);
            GL.Vertex3(x, widthY, z);

            // правая часть
            GL.Normal3(1.0, 0.0, 0.0);
            GL.TexCoord2(1.0, 0.0);
            GL.Vertex3(widthX, y, z);
            GL.TexCoord2(0.0, 0.0);
            GL.Vertex3(widthX, y, widthZ);
            GL.TexCoord2(0.0, 1.0);
            GL.Vertex3(widthX, widthY, widthZ);
            GL.TexCoord2(1.0, 1.0);
            GL.Vertex3(widthX, widthY, z);

            // нижняя часть
            GL.Normal3(0.0, -1.0, 0.0);
            GL.TexCoord2(1.0, 1.0);
            GL.Vertex3(x, y, z);
            GL.TexCoord2(0.0, 1.0);
            GL.Vertex3(x, y, widthZ);
            GL.TexCoord2(0.0, 0.0);
            GL.Vertex3(widthX, y, widthZ);
            GL.TexCoord2(1.0, 0.0);
            GL.Vertex3(widthX, y, z);

            // верхняя часть
            GL.Normal3(0.0, 1.0, 0.0);
            GL.TexCoord2(0.0, 0.0);
            GL.Vertex3(x, widthY, z);
            GL.TexCoord2(1.0, 0.0);
            GL.Vertex3(x, widthY, widthZ);
            GL.TexCoord2(1.0, 1.0);
            GL.Vertex3(widthX, widthY, widthZ);
            GL.TexCoord2(0.0, 1.0);
            GL.Vertex3(widthX, widthY, z);
        }

        /// <summary>
        /// рисует сферу, выполняет формирование координат нормалей и текстуры
        /// </summary>
        /// <param name="radius"> радиус сферы </param>
        /// <param name="xPolygons"> число полигонов (сегментов) по на оси x </param>
        /// <param name="yPolygons"> число полигонов (сегментов) по на оси y </param>
        /// <param name="xCoord"> координата центра по оси x </param>
        /// <param name="yCoord"> координата центра по оси y </param>
        /// <param name="zCoord"> координата центра по оси z </param>
        /// <param name="isFilled"> true - рисует сферу закрашенной; 
        ///                         false - рисует меридианы сферы </param>
        public static void DrawSphere(double radius, int xPolygons, int yPolygons, 
                                      double xCoord, double yCoord, double zCoord, bool isFilled)
        {
            int ix, iy;
            double x, y, z;
            for (iy = 0; iy < yPolygons; ++iy)
            {
                if (isFilled)
                {
                    GL.Begin(PrimitiveType.QuadStrip);
                    for (ix = 0; ix <= xPolygons; ++ix)
                        CalculateSphere(radius, xPolygons, yPolygons, xCoord, yCoord, zCoord, 
                                        ix, iy, out x, out y, out z);
                    GL.End();
                }
                else
                {
                    GL.Begin(PrimitiveType.LineLoop);
                    for (ix = 0; ix <= xPolygons; ++ix)
                        CalculateSphere(radius, xPolygons, yPolygons, xCoord, yCoord, zCoord, 
                                        ix, iy, out x, out y, out z);
                    GL.End();
                }
            }
        }

        /// <summary>
        /// производит вычисления координат для рисования сферы
        /// примечание: нормаль направлена от центра
        /// </summary>
        /// <param name="radius"> радиус сферы </param>
        /// <param name="xPolygons"> число полигонов (сегментов) по на оси x </param>
        /// <param name="yPolygons"> число полигонов (сегментов) по на оси y </param>
        /// <param name="xCoord"> координата центра по оси x </param>
        /// <param name="yCoord"> координата центра по оси y </param>
        /// <param name="zCoord"> координата центра по оси z </param>
        /// <param name="ix"> счётчик для вычислений на оси x </param>
        /// <param name="iy"> счётчик для вычислений на оси y </param>
        /// <param name="x"> координата x </param>
        /// <param name="y"> координата y </param>
        /// <param name="z"> координата z </param>
        private static void CalculateSphere(double radius, int xPolygons, int yPolygons, 
                                            double xCoord, double yCoord, double zCoord, 
                                            int ix, int iy, out double x, out double y, out double z)
        {
            x = radius * Math.Sin(iy * Math.PI / yPolygons) * Math.Cos(2 * ix * Math.PI / xPolygons)+ xCoord;
            y = radius * Math.Sin(iy * Math.PI / yPolygons) * Math.Sin(2 * ix * Math.PI / xPolygons)+ yCoord;
            z = radius * Math.Cos(iy * Math.PI / yPolygons) + zCoord;
            GL.Normal3(x, y, z);
            GL.TexCoord2(ix / (double)xPolygons, iy / (double)yPolygons);
            GL.Vertex3(x, y, z);

            x = radius * Math.Sin((iy + 1) * Math.PI / yPolygons) * Math.Cos(2 * ix * Math.PI / xPolygons)+ xCoord;
            y = radius * Math.Sin((iy + 1) * Math.PI / yPolygons) * Math.Sin(2 * ix * Math.PI / xPolygons)+ yCoord;
            z = radius * Math.Cos((iy + 1) * Math.PI / yPolygons)+ zCoord;
            GL.Normal3(x, y, z);
            GL.TexCoord2(ix / (double)xPolygons, (iy+1.0) / (double)yPolygons);
            GL.Vertex3(x, y, z);
        }

        /// <summary>
        /// рисует правильный многоугольник, используя количество сторон, радиус,
        /// координаты центра, параметры заливки
        /// </summary>
        /// <param name="sides"> количество сторон многоугольника </param>
        /// <param name="radius"> радиус многоугольника </param>
        /// <param name="xCoord"> координата центра по оси x </param>
        /// <param name="yCoord"> координата центра по оси y </param>
        /// <param name="isFilled"> true - рисует многоугольник закрашенным; 
        ///                         false - рисует границы многоугольника </param>
        public static void DrawPolygon(int sides, double radius, double xCoord, double yCoord, bool isFilled)
        {
            if (isFilled)
            {
                GL.Begin(PrimitiveType.Polygon);
                var step = (2 * Math.PI) / (double)(sides);
                for (var i = 0; i < sides; ++i)
                    GL.Vertex2(radius * Math.Cos(i * step) + xCoord, radius * Math.Sin(i * step) + yCoord);
                GL.End();
            }
            else
            {
                GL.Begin(PrimitiveType.LineLoop);
                var step = (2 * Math.PI) / (double)(sides);
                for (var i = 0; i < sides; ++i)
                    GL.Vertex2(radius * Math.Cos(i * step) + xCoord, radius * Math.Sin(i * step) + yCoord);
                GL.End();
            }
        }


        /// <summary>
        /// рисует куб, используя координаты центра, параметры заливки
        /// </summary>
        /// <param name="x"> координата по оси x </param>
        /// <param name="y"> координата по оси y </param>
        /// <param name="z"> координата по оси z </param>
        /// <param name="isFilled"> true - рисует куб закрашенным; 
        ///                         false - рисует рёбра куба </param>
        public static void DrawCube(double x, double y, double z, bool isFilled)
        {
            GL.Normal3(0.0,0.0,1.0);
            DrawSquare3dFB(-x, -y, -z, isFilled);   // передняя часть
            GL.Normal3(0.0, 0.0, -1.0);
            DrawSquare3dFB(x, -y, z, isFilled);     // задняя часть

            GL.Normal3(1.0, 0.0, 0.0);
            DrawSquare3dRL(x, -y, -z, isFilled);    // правая часть
            GL.Normal3(-1.0, 0.0, 0.0);
            DrawSquare3dRL(-x, -y, z, isFilled);    // левая часть

            GL.Normal3(0.0, 1.0, 0.0);
            DrawSquare3dTB(x, y, z, isFilled);      // верхняя часть
            GL.Normal3(0.0, -1.0, 0.0);
            DrawSquare3dTB(x, -y, -z, isFilled);    // нижняя часть
        }

        /// <summary>
        /// рисует переднюю (при -x, -y, -z) 
        /// и заднюю (при -x, -y, z) 
        /// стороны куба в ситеме координат x,y,z
        /// </summary>
        /// <param name="x"> координата по оси x </param>
        /// <param name="y"> координата по оси y </param>
        /// <param name="z"> координата по оси z </param>
        /// <param name="isFilled"> true - рисует сторону куба закрашенной; 
        ///                         false - рисует рёбра стороны куба </param>
        public static void DrawSquare3dFB(double x, double y, double z, bool isFilled)
        {
            if (isFilled)
            {
                GL.Begin(PrimitiveType.Quads);
                GL.TexCoord2(1.0, 1.0);
                GL.Vertex3(x, y, z);
                GL.TexCoord2(1.0, 0.0);
                GL.Vertex3(x, -y, z);
                GL.TexCoord2(0.0, 0.0);
                GL.Vertex3(-x, -y, z);
                GL.TexCoord2(0.0, 1.0);
                GL.Vertex3(-x, y, z);
                GL.End();
            }
            else
            {
                GL.Begin(PrimitiveType.LineLoop);
                GL.Vertex3(x, y, z);
                GL.Vertex3(x, -y, z);
                GL.Vertex3(-x, -y, z);
                GL.Vertex3(-x, y, z);
                GL.End();
            }
        }

        /// <summary>
        /// рисует правую (при x, -y, -z) 
        /// и левую (при -x, -y, z) 
        /// стороны куба в ситеме координат x,y,z
        /// </summary>
        /// <param name="x"> координата по оси x </param>
        /// <param name="y"> координата по оси y </param>
        /// <param name="z"> координата по оси z </param>
        /// <param name="isFilled"> true - рисует сторону куба закрашенной; 
        ///                         false - рисует рёбра стороны куба </param>
        public static void DrawSquare3dRL(double x, double y, double z, bool isFilled)
        {
            if (isFilled)
            {
                GL.Begin(PrimitiveType.Quads);
                
                GL.TexCoord2(0.0, 1.0);
                GL.Vertex3(x, y, z);
                GL.TexCoord2(0.0, 0.0);
                GL.Vertex3(x, -y, z);
                GL.TexCoord2(1.0, 0.0);
                GL.Vertex3(x, -y, -z);
                GL.TexCoord2(1.0, 1.0);
                GL.Vertex3(x, y, -z);
                GL.End();
            }
            else
            {
                GL.Begin(PrimitiveType.LineLoop);
                GL.Vertex3(x, y, z);
                GL.Vertex3(x, -y, z);
                GL.Vertex3(x, -y, -z);
                GL.Vertex3(x, y, -z);
                GL.End();
            }
        }

        /// <summary>
        /// рисует верхнюю (при x, y, z) 
        /// и нижнюю (при x, -y, -z) 
        /// стороны куба в ситеме координат x,y,z
        /// </summary>
        /// <param name="x"> координата по оси x </param>
        /// <param name="y"> координата по оси y </param>
        /// <param name="z"> координата по оси z </param>
        /// <param name="isFilled"> true - рисует сторону куба закрашенной; 
        ///                         false - рисует рёбра стороны куба </param>
        public static void DrawSquare3dTB(double x, double y, double z, bool isFilled)
        {
            if (isFilled)
            {
                GL.Begin(PrimitiveType.Quads);
                GL.TexCoord2(1.0, 1.0);
                GL.Vertex3(x, y, z);
                GL.TexCoord2(0.0, 1.0);
                GL.Vertex3(x, y, -z);
                GL.TexCoord2(0.0, 0.0);
                GL.Vertex3(-x, y, -z);
                GL.TexCoord2(1.0, 0.0);
                GL.Vertex3(-x, y, z);
                GL.End();
            }
            else
            {
                GL.Begin(PrimitiveType.LineLoop);
                GL.Vertex3(x, y, z);
                GL.Vertex3(x, y, -z);
                GL.Vertex3(-x, y, -z);
                GL.Vertex3(-x, y, z);
                GL.End();
            }
        }

        /// <summary>
        /// функция рисования квадрата, используя координаты x,y левого нижнего угла квадрата, 
        /// координату точки по диагонали (правый верхний угол), наличие заливки
        /// </summary>
        /// <param name="startPointX"> координата по оси x левого нижнего угла квадрата </param>
        /// <param name="startPointY"> координата по оси y левого нижнего угла квадрата </param>
        /// <param name="diagonalPoint"> координата правого верхнего угла (неважно, по оси x или y,
        ///                              это ж квадрат, алё, она просто по диагонали расположена) </param>
        /// <param name="isFilled"> true - рисует квадрат закрашенным; false - рисует рёбра квадрата </param>
        public static void DrawSquare(double startPointX, double startPointY, double diagonalPoint, bool isFilled)
        {
            if (isFilled)
            {
                GL.Begin(PrimitiveType.Quads);
                GL.Vertex2(startPointX, startPointY);
                GL.Vertex2(startPointX, diagonalPoint);
                GL.Vertex2(diagonalPoint, diagonalPoint);
                GL.Vertex2(diagonalPoint, startPointY);
                GL.End();
            }
            else
            {
                GL.Begin(PrimitiveType.LineLoop);
                GL.Vertex2(startPointX, startPointY);
                GL.Vertex2(startPointX, diagonalPoint);
                GL.Vertex2(diagonalPoint, diagonalPoint);
                GL.Vertex2(diagonalPoint, startPointY);
                GL.End();
            }
        }

        /// <summary>
        /// функция рисования линии, используя радиус круга, координаты x,y центра круга, наличие заливки
        /// </summary>
        /// <param name="radius"> радиус круга </param>
        /// <param name="xCoord"> координата центра по оси x </param>
        /// <param name="yCoord"> координата центра по оси y </param>
        /// <param name="isFilled"> true - рисует круг закрашенным; false - рисует окружность </param>
        public static void DrawCircle(double radius, double xCoord, double yCoord, bool isFilled)
        {
            if (isFilled)
            {
                GL.Begin(PrimitiveType.TriangleFan);
                for (var angle = 0; angle < 360; angle++)
                    GL.Vertex2(xCoord + Math.Sin(angle) * radius, yCoord + Math.Cos(angle) * radius);
                GL.End();
            }
            else
            {
                GL.Begin(PrimitiveType.LineLoop);
                for (var angle = 0; angle < 360; angle++)
                {
                    var theta = 2.0 * Math.PI * angle / 360;
                    GL.Vertex2(radius * Math.Cos(theta) + xCoord, radius * Math.Sin(theta) + yCoord);
                }
                GL.End();
            }
        }

        /// <summary>
        /// функция рисования линии, используя координаты x,y начала и конца линии
        /// </summary>
        /// <param name="fromPointX"> координата по оси x, откуда начинается рисование линии </param>
        /// <param name="fromPointY"> координата по оси y, откуда начинается рисование линии </param>
        /// <param name="toPointX"> координата по оси x, где заканчивается рисование линии </param>
        /// <param name="toPointY"> координата по оси y, где заканчивается рисование линии </param>
        public static void DrawLine(double fromPointX, double fromPointY, double toPointX, double toPointY)
        {
            GL.Begin(PrimitiveType.Lines);
            GL.Vertex2(fromPointX, fromPointY);
            GL.Vertex2(toPointX, toPointY);
            GL.End();
        }

        /// <summary>
        /// функция рисования линии, используя координаты x,y,z начала и конца линии
        /// </summary>
        /// <param name="fromPointX"> координата по оси x, откуда начинается рисование линии</param>
        /// <param name="fromPointY">координата по оси y, откуда начинается рисование линии</param>
        /// <param name="fromPointZ">координата по оси z, откуда начинается рисование линии</param>
        /// <param name="toPointX">координата по оси x, где заканчивается рисование линии</param>
        /// <param name="toPointY">координата по оси y, где заканчивается рисование линии</param>
        /// <param name="toPointZ">координата по оси z, где заканчивается рисование линии</param>
        public static void DrawLine(double fromPointX, double fromPointY, double fromPointZ, double toPointX, double toPointY, double toPointZ)
        {
            GL.Begin(PrimitiveType.Lines);
            GL.Vertex3(fromPointX, fromPointY, fromPointZ);
            GL.Vertex3(toPointX, toPointY, toPointZ);
            GL.End();
        }
    }
}
