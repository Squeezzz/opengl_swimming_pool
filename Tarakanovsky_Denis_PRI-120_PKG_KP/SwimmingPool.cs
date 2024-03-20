using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tao.FreeGlut;
using Tao.OpenGl;

namespace Tarakanovsky_Denis_PRI_120_PKG_KP
{
    class SwimmingPool
    {
        private float deltaColor;

        //Отрисовка стен
        public void drawWalls()
        {
            Gl.glPushMatrix();
            setColor(0.48f, 0.83f, 0.57f);
            Gl.glBegin(Gl.GL_QUADS);
            Gl.glVertex3d(-100, 5, 200);
            Gl.glTexCoord2f(0, 0);
            Gl.glVertex3d(-100, 5, 0);
            Gl.glTexCoord2f(0, 1);
            Gl.glVertex3d(200, 5, 0);
            Gl.glTexCoord2f(1, 1);
            Gl.glVertex3d(200, 5, 200);
            Gl.glTexCoord2f(1, 0);
            Gl.glEnd();
            Gl.glPopMatrix();

            Gl.glPushMatrix();
            Gl.glTranslated(-90, -100, 0);
            Gl.glRotated(90, 0, 0, 1);
            setColor(0.48f, 0.83f, 0.57f);
            Gl.glBegin(Gl.GL_QUADS);
            Gl.glVertex3d(-100, 5, 200);
            Gl.glTexCoord2f(0, 0);
            Gl.glVertex3d(-100, 5, 0);
            Gl.glTexCoord2f(0, 1);
            Gl.glVertex3d(200, 5, 0);
            Gl.glTexCoord2f(1, 1);
            Gl.glVertex3d(200, 5, 200);
            Gl.glTexCoord2f(1, 0);
            Gl.glEnd();

            Gl.glLineWidth(6f);

            Gl.glColor3f(0.18f, 0.53f, 0.27f);
            Gl.glTranslated(100, 0, 0);
            Gl.glBegin(Gl.GL_LINES);
            Gl.glVertex3d(0, 0, 0);
            Gl.glVertex3d(0, 0, 200);
            Gl.glEnd();
            Gl.glPopMatrix();
        }


        //Отрисовка пола
        public void drawFloor()
        {
            Gl.glPushMatrix();
            for (int i = 0; i < 2; i++)
            {
                Gl.glPushMatrix();
                setColor(0.7f, 0.7f, 0.75f);
                Gl.glTranslated(50, -18 - i * 160, -14);
                Gl.glScalef(10, 1.5f, 1);
                Glut.glutSolidCube(30);
                Gl.glColor3f(0, 0, 0);
                Gl.glLineWidth(2f);
                Glut.glutWireCube(30);
                Gl.glPopMatrix();
            }

            for (int i = 0; i < 2; i++)
            {
                Gl.glPushMatrix();
                setColor(0.7f, 0.7f, 0.75f);
                Gl.glTranslated(-80 + i * 200, -18, -14);
                Gl.glScalef(1.5f + i * 4, 10f, 1);
                Glut.glutSolidCube(30);
                Gl.glColor3f(0, 0, 0);
                Gl.glLineWidth(2f);
                Glut.glutWireCube(30);
                Gl.glPopMatrix();
            }

            Gl.glPopMatrix();
        }

        //Отрисовка бассейна
        public void drawPool()
        {
            //Вода
            Gl.glPushMatrix();
            Gl.glTranslated(0, 10, -10);
            setColor(0, 0.4f, 0.85f);
            Gl.glLineWidth(10f);
            Gl.glBegin(Gl.GL_TRIANGLE_FAN);
            Gl.glVertex3d(-60, -170, 0);
            Gl.glVertex3d(40, -170, 0);
            Gl.glVertex3d(40, -50, 0);
            Gl.glVertex3d(-60, -50, 0);
            Gl.glEnd();
            Gl.glPopMatrix();

            for (int i = 0; i < 2; i++)
            {
                Gl.glPushMatrix();
                Gl.glTranslated(-90 - i * 40, 0, -8);
                Gl.glRotated(90, 1, 0, 0);
                Gl.glColor3f(1, 0, 0);
                Gl.glTranslated(100, 0, 0);
                Gl.glEnable(Gl.GL_LINE_STIPPLE);
                Gl.glLineStipple(1, 366);
                Gl.glBegin(Gl.GL_LINES);
                Gl.glVertex3d(0, 0, 50);
                Gl.glVertex3d(0, 0, 170);
                Gl.glEnd();
                Gl.glDisable(Gl.GL_LINE_STIPPLE);
                Gl.glPopMatrix();
            }

        }

        //Отрисовка знаков (общий метод)
        public void drawSigns(uint frame1, uint frame2, uint sign1,
            uint sign2, uint sign3, bool isSign1, bool isSign2, bool isSign3,
            bool isSign1True, bool isSign2True, double deltaXSign1,
            double deltaXSign2, double deltaXSign3)
        {
            drawFrame(frame1, -20);
            drawFrame(frame2, 70);
            if (isSign1True)
            {
                drawSign(sign1, 0, -52);
            }
            else if (!isSign1) drawSign(sign1, 0, 0); else drawSign(sign1, 40 - deltaXSign1, -52);
            if (isSign2True)
            {
                drawSign(sign2, 81, -52);
            }
            else if (!isSign2) drawSign(sign2, 40, 0); else drawSign(sign2, 40 - deltaXSign2, -52);
            if (!isSign3) drawSign(sign3, 80, 0); else drawSign(sign3, 40 - deltaXSign3, -52);

        }

        //Отрисовка надписей
        private void drawFrame(uint frame, double deltaX)
        {
            Gl.glPushMatrix();
            Gl.glTranslated(deltaX, 21, 110);
            Gl.glRotated(180, 0, 1, 0);
            Gl.glEnable(Gl.GL_TEXTURE_2D);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, frame);
            Gl.glPushMatrix();
            Gl.glBegin(Gl.GL_QUADS);
            Gl.glVertex3d(10, -20, 60);
            Gl.glTexCoord2f(0, 0);
            Gl.glVertex3d(40, -20, 60);
            Gl.glTexCoord2f(0, 1);
            Gl.glVertex3d(40, -20, 20);
            Gl.glTexCoord2f(1, 1);
            Gl.glVertex3d(10, -20, 20);
            Gl.glTexCoord2f(1, 0);
            Gl.glEnd();
            Gl.glPopMatrix();
            Gl.glDisable(Gl.GL_TEXTURE_2D);
            Gl.glPopMatrix();
        }

        //Отрисовка самих знаков (они передвигаются)
        private void drawSign(uint sign, double deltaX, double deltaZ)
        {
            Gl.glPushMatrix();
            Gl.glTranslated(deltaX - 20, 10, 160 + deltaZ);
            Gl.glRotated(180, 0, 1, 0);
            Gl.glEnable(Gl.GL_TEXTURE_2D);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, sign);
            Gl.glPushMatrix();
            Gl.glBegin(Gl.GL_QUADS);
            Gl.glVertex3d(10, -20, 40);
            Gl.glTexCoord2f(0, 0);
            Gl.glVertex3d(30, -20, 40);
            Gl.glTexCoord2f(0, 1);
            Gl.glVertex3d(30, -20, 20);
            Gl.glTexCoord2f(1, 1);
            Gl.glVertex3d(10, -20, 20);
            Gl.glTexCoord2f(1, 0);
            Gl.glEnd();
            Gl.glPopMatrix();
            Gl.glDisable(Gl.GL_TEXTURE_2D);
            Gl.glPopMatrix();
        }


        //Отрисовка спасательного круга
        public void drawLifebuoy(double deltaY)
        {
            Gl.glPushMatrix();
            Gl.glTranslated(-85, -18, 30);
            Gl.glPushMatrix();
            setColor(0.7f, 0.1f, 0.15f);

            Gl.glTranslated(75, -90 + deltaY, -35);

            Glut.glutSolidTorus(3, 10, 10, 20);
            Gl.glColor3f(0, 0, 0);
            Gl.glLineWidth(2f);
            Glut.glutWireTorus(3, 10, 10, 20);
            Gl.glPopMatrix();
            Gl.glPopMatrix();
        }

        //Отрисовка лестницы
        public void drawLadder()
        {
            Gl.glPushMatrix();
            for (int i = 0; i < 2; i++)
            {
                Gl.glPushMatrix();
                Gl.glLineWidth(6f);
                Gl.glColor3f(0, 0, 0);
                Gl.glTranslated(0 + i * 20, -45, -20);
                Gl.glBegin(Gl.GL_LINES);
                Gl.glVertex3d(0, 0, 0);
                Gl.glVertex3d(0, 0, 30);
                Gl.glEnd();
                Gl.glPopMatrix();
            }

            for (int i = 0; i < 3; i++)
            {
                Gl.glPushMatrix();
                Gl.glLineWidth(6f);
                Gl.glColor3f(0, 0, 0);
                Gl.glTranslated(0, -45, 0 - i * 7);
                Gl.glBegin(Gl.GL_LINES);
                Gl.glVertex3d(0, 0, 6);
                Gl.glVertex3d(20, 0, 6);
                Gl.glEnd();
                Gl.glPopMatrix();
            }

            Gl.glPopMatrix();
        }

        //Мяч
        public void drawBall(double deltaY, double deltaZ)
        {
            Gl.glPushMatrix();
            setColor(0.7f, 0.1f, 0.7f);
            Gl.glTranslated(100, -50 - deltaY, 15 + deltaZ);
            Glut.glutSolidSphere(13, 20, 20);
            Gl.glColor3f(0, 0, 0);
            Gl.glLineWidth(2f);
            Glut.glutWireSphere(13, 12, 12);
            Gl.glPopMatrix();
        }

        //Задать цвет
        private void setColor(float R, float G, float B)
        {
            RGB color = new RGB(R - deltaColor, G - deltaColor, B - deltaColor);
            Gl.glColor3f(color.getR(), color.getG(), color.getB());
        }

    }

    class RGB
    {
        private float R;
        private float G;
        private float B;

        public RGB(float R, float G, float B)
        {
            this.R = R;
            this.G = G;
            this.B = B;
        }

        public float getR()
        {
            return R;
        }

        public float getG()
        {
            return G;
        }

        public float getB()
        {
            return B;
        }
    }
}
