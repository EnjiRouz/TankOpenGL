// https://tanks.gg/tank/bt-2/model?vm=visual (БТ-5. В игре называется БТ-2, но в топовой комплектации, выглядит как БТ-5.) 

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace TankOpenGL
{
    internal static class Tank
    {
        // каждая функция рисует свою часть танка
        // для рисования танка используются методы класса ShapeDrawer
        public static void DrawTank(double turretRotationAngle)
        {
            GL.PushMatrix();

            //GL.Rotate(90.0, Vector3d.UnitY);  // вид спереди
            //GL.Rotate(-90.0, Vector3d.UnitY); // вид сзади
            //GL.Rotate(-90, Vector3d.UnitX);   // вид сверху            

            DrawTurret(turretRotationAngle);
            DrawTankBase();
            DrawWheels();

            GL.PopMatrix();
        }

        private static void DrawWheels()
        {
            GL.BindTexture(TextureTarget.Texture2D, Render.TextureId[1]);
            DrawWheelsOnOneSide(-0.026);
            DrawWheelsOnOneSide(0.165);
            GL.BindTexture(TextureTarget.Texture2D, Render.TextureId[2]);
            DrawTracks();
        }

        private static void DrawTracks()
        {
            DrawTracksOnOneSide(-0.04);
            DrawTracksOnOneSide(0.152);
        }

        private static void DrawTankBase()
        {
            // корпус
            GL.BindTexture(TextureTarget.Texture2D, Render.TextureId[0]);
            ShapeDrawer.DrawParallelepiped(0.0, 0.0, 0.0, 0.3175, 0.1134, 0.17, true);

            // нос??
            GL.PushMatrix();
            GL.Rotate(90.0, Vector3d.UnitX);
            GL.Rotate(-90, Vector3d.UnitZ);
            GL.Translate(-0.085, 0.358, -0.05);
            DrawNose(0.01, 0.04, 0.1700 / 2, 0.13 / 2);
            GL.PopMatrix();

            // ящик?
            GL.PushMatrix();
            GL.Rotate(-90.0, Vector3d.UnitY);
            GL.Rotate(5.0, Vector3d.UnitX);
            GL.Translate(0.085, 0.048, -0.32);
            DrawBox(0.01, 0.025, 0.1326 / 3);
            GL.PopMatrix();

            // нос??
            GL.PushMatrix();
            GL.Translate(0.38,0.015,0.0);
            ShapeDrawer.DrawCylinder(0.0275 / 2, 0.1550, 0.0, 0.0, 0.1, true);
            GL.PopMatrix();

            // бочка            
            GL.PushMatrix();
            GL.Translate(-0.015, 0.075, 0.02);
            GL.BindTexture(TextureTarget.Texture2D, Render.TextureId[4]);
            ShapeDrawer.DrawCylinder(0.0275 / 2, 0.1326, 0.0, 0.0, 0.1, true);
            GL.BindTexture(TextureTarget.Texture2D, Render.TextureId[5]);
            ShapeDrawer.DrawCircle3D(0.0275 / 2, 0.1327, 0.0, 0.0, 0.1);
            ShapeDrawer.DrawCircle3D(0.0275 / 2, -0.0001, 0.0, 0.0, 0.1);
            GL.PopMatrix();

            // задняя часть
            GL.BindTexture(TextureTarget.Texture2D, Render.TextureId[3]);
            ShapeDrawer.DrawParallelepiped(0.005, 0.112, 0.02, 0.06, 0.015, 0.1326, true);
        }

        private static void DrawTurret(double angle)
        {
            GL.BindTexture(TextureTarget.Texture2D, Render.TextureId[0]);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.PushMatrix();
            GL.Translate(0.2, 0.08, -0.1764/10+0.1);
            GL.Rotate(angle, Vector3d.UnitY);
            GL.Translate(-(0.2), -(0.08), -(-0.1764/10+0.1));
            DrawTurretBase();
            DrawGun();
            GL.PopMatrix();
        }

        private static void DrawWheelsOnOneSide(double z)
        {
            GL.PushMatrix();
            GL.Translate(0.04 - 0.1, 0.0 + 0.02, z);
            ShapeDrawer.DrawCylinder(0.0540 / 2, 0.026, 0.0, 0.0, 0.1, true);
            GL.PopMatrix();

            for (int i = 0; i < 2; i++)
            {
                GL.PushMatrix();
                GL.Translate(0.0 + i * 0.09, 0.0 + 0.01, z);
                ShapeDrawer.DrawCylinder(0.0815 / 2, 0.026, 0.0, 0.0, 0.1, true);
                GL.PopMatrix();
            }

            for (int i = 2; i < 4; i++)
            {
                GL.PushMatrix();
                GL.Translate(0.0 + (i+0.7) * 0.09, 0.0 + 0.01, z);
                ShapeDrawer.DrawCylinder(0.0815 / 2, 0.026, 0.0, 0.0, 0.1, true);
                GL.PopMatrix();
            }

            GL.PushMatrix();
            GL.Translate(0.035 + 0.09 * 4.0, 0.0 + 0.0275, z);
            ShapeDrawer.DrawCylinder(0.0275 / 2, 0.026, 0.0, 0.0, 0.1, true);
            GL.PopMatrix();
        }

        private static void DrawBox(double top, double height, double bottom)
        {
            GL.PushMatrix();
            GL.Rotate(-135.0, Vector3d.UnitX);
            GL.Rotate(-270.0, Vector3d.UnitZ);

            GL.Begin(PrimitiveType.Quads);
            GL.Vertex3(bottom, height, top);
            GL.Vertex3(-bottom, height, top);
            GL.Vertex3(-bottom, -height, top);
            GL.Vertex3(bottom, -height, top);

            GL.Normal3(-1.0, 0.0, 0.0);
            GL.TexCoord2(0.0, 0.0);
            GL.Vertex3(bottom, height, top);
            GL.TexCoord2(1.0, 0.0);
            GL.Vertex3(0.0, height, 5 * top);
            GL.TexCoord2(0.0, 1.0);
            GL.Vertex3(0.0, -height, 5 * top);
            GL.TexCoord2(1.0, 1.0);
            GL.Vertex3(bottom, -height, top);
            
            GL.Normal3(1.0, 0.0, 0.0);
            GL.TexCoord2(1.0, 1.0);
            GL.Vertex3(-bottom, height, top);
            GL.TexCoord2(1.0, 0.0);
            GL.Vertex3(0.0, height, 5 * top);
            GL.TexCoord2(0.0, 0.0);
            GL.Vertex3(0.0, -height, 5 * top);
            GL.TexCoord2(0.0, 1.0);
            GL.Vertex3(-bottom, -height, top);
            GL.End();

            GL.Begin(PrimitiveType.Triangles);
            GL.Normal3(-1.0, 0.0, 0.0);
            GL.TexCoord2(1.0, 1.0);
            GL.Vertex3(bottom, height, top);
            GL.TexCoord2(0.0, 1.0);
            GL.Vertex3(0.0, height, 5 * top);
            GL.TexCoord2(0.0, 0.0);
            GL.Vertex3(-bottom, height, top);

            GL.Normal3(1.0, 0.0, 0.0);
            GL.TexCoord2(1.0, 1.0);
            GL.Vertex3(bottom, -height, top);
            GL.TexCoord2(0.0, 1.0);
            GL.Vertex3(0.0, -height, 5 * top);
            GL.TexCoord2(0.0, 0.0);
            GL.Vertex3(-bottom, -height, top);
            GL.End();

            GL.PopMatrix();
        }

        private static void DrawNose(double top, double height, double bottom, double side)
        {
            GL.Begin(PrimitiveType.Quads);
            GL.Normal3(0.0, -1.0, 0.0);
            GL.TexCoord2(1.0, 1.0);
            GL.Vertex3(top + top / 2, height / 1.2, 5 * top);
            GL.TexCoord2(1.0, 0.0);
            GL.Vertex3(-top - top / 2, height / 1.2, 5 * top);
            GL.TexCoord2(0.0, 0.0);
            GL.Vertex3(-bottom, -height, 5 * top);
            GL.TexCoord2(0.0, 1.0);
            GL.Vertex3(bottom, -height, 5 * top);
            
            GL.Normal3(-1.0, 0.0, 0.0);
            GL.TexCoord2(0.0, 0.0);
            GL.Vertex3(top, height, 2 * top);
            GL.TexCoord2(1.0, 0.0);
            GL.Vertex3(top + top / 2, height / 1.2, 5 * top);
            GL.TexCoord2(0.0, 1.0);
            GL.Vertex3(bottom, -height, 5 * top);
            GL.TexCoord2(1.0, 1.0);
            GL.Vertex3(bottom, -height, -side);

            GL.Normal3(0.0, 1.0, 0.0);
            GL.TexCoord2(1.0, 1.0);
            GL.Vertex3(top, height, 2 * top);
            GL.TexCoord2(1.0, 0.0);
            GL.Vertex3(-top, height, 2 * top);
            GL.TexCoord2(0.0, 0.0);
            GL.Vertex3(-bottom, -height, -side);
            GL.TexCoord2(0.0, 1.0);
            GL.Vertex3(bottom, -height, -side);

            GL.Normal3(1.0, 0.0, 0.0);
            GL.TexCoord2(0.0, 0.0);
            GL.Vertex3(-top - top / 2, height / 1.2, 5 * top);
            GL.TexCoord2(1.0, 0.0);
            GL.Vertex3(-top, height, 2 * top);
            GL.TexCoord2(0.0, 1.0);
            GL.Vertex3(-bottom, -height, -side);
            GL.TexCoord2(1.0, 1.0);
            GL.Vertex3(-bottom, -height, 5 * top);
            GL.End();

            GL.Begin(PrimitiveType.Quads);
            GL.Normal3(0.0, 0.0, 1.0);
            GL.TexCoord2(1.0, 1.0);
            GL.Vertex3(-top - top / 2, height / 1.2, 5 * top);
            GL.TexCoord2(1.0, 0.0);
            GL.Vertex3(-top, height, 2 * top);
            GL.TexCoord2(0.0, 0.0);
            GL.Vertex3(top, height, 2 * top);
            GL.TexCoord2(0.0, 1.0);
            GL.Vertex3(top + top / 2, height / 1.2, 5 * top);
            GL.End();
        }

        private static void DrawTracksOnOneSide(double z)
        {
            // верх и низ гусенниц
            for (int i= 0;i< 0.5024/0.0255; i++)
                ShapeDrawer.DrawParallelepiped(-0.075+i*0.025, 0.05, z, 0.02, 0.01, 0.052, true);
            for (int i = 0; i < 0.4 / 0.025; i++)
                ShapeDrawer.DrawParallelepiped(-0.05 + (i * 0.025), -0.032, z, 0.02, 0.01, 0.052, true);
            
            // угол справа
            GL.PushMatrix();
            GL.Translate(0.3570, -0.035, (z + 0.1));
            GL.Rotate(35, Vector3d.UnitZ);
            GL.Translate(-0.3570, -0.035, -(z + 0.1));
            for (int i = 0; i < 0.075 / 0.025; i++)
                ShapeDrawer.DrawParallelepiped(0.3570 + (i * 0.025), 0.04, z, 0.02, 0.01, 0.052, true);          
            GL.PopMatrix();

            GL.PushMatrix();
            GL.Translate(0.3570 + 0.055, 0.005, (z + 0.1));
            GL.Rotate(65, Vector3d.UnitZ);
            GL.Translate(-0.3570 - 0.055, -0.005, -(z + 0.1));
            for (int i = 0; i < 0.05 / 0.025; i++)
                ShapeDrawer.DrawParallelepiped(0.3570 + 0.055 + (i * 0.025), 0.005, z, 0.02, 0.01, 0.052, true);
            GL.PopMatrix();
            
            // угол слева
            GL.PushMatrix();
            GL.Translate(-0.05, -0.035, (z + 0.1));
            GL.Rotate(-35, Vector3d.UnitZ);
            GL.Translate(0.05, -0.035, -(z + 0.1));
            for (int i = 0; i < 0.07 / 0.025; i++)
                ShapeDrawer.DrawParallelepiped(-0.05 - (i * 0.025), 0.04, z, -0.02, 0.01, 0.052, true);
            GL.PopMatrix();
            
            GL.PushMatrix();
            GL.Translate(-0.05 - 0.05, 0.015, (z + 0.1));
            GL.Rotate(-120, Vector3d.UnitZ);
            GL.Translate(-(-0.05 - 0.05), -0.015, -(z + 0.1));
            for (int i = 0; i < 0.05 / 0.025; i++)
                ShapeDrawer.DrawParallelepiped(-0.05 - 0.05 - (i * 0.025), 0.015, z, -0.02, 0.01, 0.052, true);
            GL.PopMatrix();
        }

        private static void DrawTurretBase()
        {
            GL.BindTexture(TextureTarget.Texture2D, Render.TextureId[0]);
            // основание башни
            GL.PushMatrix();
            GL.Rotate(90.0, Vector3d.UnitX);
            GL.Translate(0.2, 0.08, -0.1764);
            ShapeDrawer.DrawCylinder(0.1326 / 2, 0.0638, 0.0, 0.0, 0.1, true);
            GL.PopMatrix();

            // крупление башни к орудию
            GL.PushMatrix();
            GL.Translate(0.26, 0.15, 0.05);
            ShapeDrawer.DrawCylinder(0.019, 0.1326 / 2, 0.0, 0.0, 0.1, true);
            GL.PopMatrix();

            // задняя часть башни
            ShapeDrawer.DrawParallelepiped(0.1, 0.135, 0.06, 0.049, 0.029, 0.1326 / 3, true);

            // труба на башне
            GL.PushMatrix();
            GL.Rotate(90.0, Vector3d.UnitX);
            GL.Translate(0.21, 0.115, -0.185);
            ShapeDrawer.DrawCylinder(0.0075, 0.01, 0.0, 0.0, 0.1, true);
            GL.PopMatrix();
        }

        private static void DrawGun()
        {
            // орудие
            GL.PushMatrix();
            GL.Rotate(0.0, Vector3d.UnitX);
            GL.Rotate(90.0, Vector3d.UnitY);
            GL.Translate(-0.085, 0.15, 0.26);
            ShapeDrawer.DrawCylinder(0.0045, 0.185, 0.0, 0.0, 0.1, true);
            GL.PopMatrix();

            // начало орудия
            GL.PushMatrix();
            GL.Rotate(0.0, Vector3d.UnitX);
            GL.Rotate(90.0, Vector3d.UnitY);
            GL.Translate(-0.085, 0.15, 0.26);
            ShapeDrawer.DrawCylinder(0.008, 0.035, 0.0, 0.0, 0.1, true);
            GL.PopMatrix();

            // конец орудия
            GL.PushMatrix();
            GL.Rotate(0.0, Vector3d.UnitX);
            GL.Rotate(90.0, Vector3d.UnitY);
            GL.Translate(-0.085, 0.15, 0.44);
            ShapeDrawer.DrawCylinder(0.005, 0.01, 0.0, 0.0, 0.1, true);
            GL.PopMatrix();
        }
    }
}
