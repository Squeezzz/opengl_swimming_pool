using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tao.DevIl;
using Tao.FreeGlut;
using Tao.OpenGl;

namespace Tarakanovsky_Denis_PRI_120_PKG_KP
{
    public partial class Form1 : Form
    {
        //Управление положением пользователя
        double angle = 3, angleX = -96, angleY = 0, angleZ = -30;
        double sizeX = 1, sizeY = 1, sizeZ = 1;
        double translateX = -9, translateY = -60, translateZ = -10;
        double cameraSpeed;
        float global_time = 0;


        //Текстуры (знаки на стене)
        string sign1 = "sign9.png";
        string sign2 = "sign4.png";
        string sign3 = "sign7.png";
        string frame1Texture = "sign6.png";
        string frame2 = "sign8.png";
        uint sign1Texture, sign2Texture, sign3Texture, frame1, frame2Texture;
        int imageId;
        bool isSign1Taken = false, isSign2Taken = false, isSign3Taken = false;
        bool isTrueSign1, isTrueSign2;
        double deltaXSign1 = 0, deltaXSign2 = 0, deltaXSign3 = 0;
        bool isWin = false, isLose = false;

        //Перемещение круга и мяча
        double deltaYLifebuoy = 0, deltaZBall = 0, deltaYBall = 0, translateZBall = 0;
        bool isLifebouyDown = true, isBallThrown = false, isZBallUp = false;



        // экземпляра класса Explosion
        private readonly Explosion boom = new Explosion(1, 10, 1, 300, 1000);

        //Проигрывание аудио при выигрыше в игре со знаками
        public WMPLib.WindowsMediaPlayer WMP = new WMPLib.WindowsMediaPlayer();



        SwimmingPool swimmingPool = new SwimmingPool();

        public void Draw()
        {
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
            Gl.glClearColor(255, 255, 255, 1);
            Gl.glLoadIdentity();
            Gl.glPushMatrix();
            Gl.glRotated(angleX, 1, 0, 0);
            Gl.glRotated(angleY, 0, 1, 0);
            Gl.glRotated(angleZ, 0, 0, 1);
            Gl.glTranslated(translateX, translateY, translateZ);
            Gl.glScaled(sizeX, sizeY, sizeZ);
            boom.Calculate(global_time);
            drawCommonScene();
            drawFractal();

            Gl.glPopMatrix();
            Gl.glFlush();
            AnT.Invalidate();
        }

        public void drawFractal()
        {
            Gl.glPushMatrix();

            Gl.glTranslated(-90, -70, 90);

            drawSpiral(15, 5);
            Gl.glEnd();
            Gl.glPopMatrix();
        }

        public void drawSpiral(int depth, float size)
        {
            if (depth <= 0 && size <= 0.005) return;
            Gl.glPushMatrix();
            Gl.glRotated(90, 0, 1, 0);
            Gl.glColor3f(0.7f, 0.1f, 0.15f);
            Glut.glutSolidTorus(1, 3, 32, 32);
            Gl.glColor3f(0, 0,0);
            Glut.glutWireTorus(1, 3, 32, 12);
            
            Gl.glPopMatrix();
            Gl.glTranslated(0, -5.5 * size, 0);
            Gl.glRotated(40, 1, 0, 0);
            depth--;
            size /= 1.1f;
            drawSpiral(depth, size);
        }
    

    private void drawCommonScene()
        {
            swimmingPool.drawWalls();
            swimmingPool.drawFloor();
            swimmingPool.drawPool();
            swimmingPool.drawSigns(frame1, frame2Texture, sign1Texture, sign2Texture, sign3Texture,
                isSign1Taken, isSign2Taken, isSign3Taken, isTrueSign1, isTrueSign2,
                deltaXSign1, deltaXSign2, deltaXSign3);
            swimmingPool.drawLadder();
            swimmingPool.drawLifebuoy(deltaYLifebuoy);

            if (isLifebouyDown)
            {
                if (deltaYLifebuoy >= 30) isLifebouyDown = false;
                else deltaYLifebuoy += 1;
            }
            else
            {
                if (deltaYLifebuoy <= -30) isLifebouyDown = true;
                else deltaYLifebuoy -= 1;
            }

            swimmingPool.drawBall(deltaYBall, deltaZBall);
            if (isBallThrown)
            {
                if (translateZBall > 1)
                {
                    deltaYBall += 1;
                    if (isZBallUp)
                    {
                        if (deltaZBall >= translateZBall)
                        {
                            isZBallUp = false;
                        }
                        else deltaZBall += 3;
                        
                    } else
                    {
                        if (deltaZBall < 0)
                        {
                            isZBallUp = true;
                            translateZBall -= 5;
                        }
                        else deltaZBall -= 4;
                    }
                } else
                {
                    deltaYBall = 0;
                    deltaZBall = 0;
                    translateZBall = 0;
                    isBallThrown = false;
                    isZBallUp = false;
                }
                

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 1)
            {
                isSign1Taken = false;
                isSign2Taken = false;
                isSign3Taken = false;
                isTrueSign1 = false;
                isTrueSign2 = false;
                isWin = false;
                isLose = false;
                deltaXSign1 = 0;
                deltaXSign2 = 0;
                deltaXSign3 = 0;
                label6.Visible = true;
                initSecondPosition();
            } else if (comboBox1.SelectedIndex == 3)
            {
                isBallThrown = true;
                translateZBall = deltaZBall -15;
            }
           
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    angle = 3; angleX = -50; angleY = 0; angleZ = 0;
                    sizeX = 1; sizeY = 1; sizeZ = 1;
                    translateX = 0; translateY = 200; translateZ = -100;
                    initFirstPosition();
                    break;
                case 1:
                    translateX = 0; translateY = 120; translateZ = -90;
                    angle = 3; angleX = -90; angleY = 0; angleZ = 0;
                    initSecondPosition();
                    break;
                case 2:
                    translateX = -30; translateY = 80; translateZ = -35;
                    angle = 3; angleX = -90; angleY = 0; angleZ = -90;
                    initThirdPosition();
                    break;
                case 3:
                    translateX = -120; translateY = 180; translateZ = -100;
                    angle = 3; angleX = -60; angleY = 0; angleZ = -30;
                    initFourthPosition();
                    break;
            }
            AnT.Focus();
        }

        private void initFirstPosition()
        {
            label7.Visible = false;
            label8.Visible = false;
            label9.Visible = false;
            label10.Visible = false;
            label11.Visible = false;
            label12.Visible = false;
            button1.Visible = false;
            label6.Text = "Мм, бассейн";
        }

        private void initSecondPosition()
        {
            button1.Visible = true;
            button1.Text = "Заново";
            if (isLose) label6.Text = "Ничего, попробуй ещё :)";
            else if (isWin) label6.Text = "Ура!!!!";
            else label6.Text = "Вперёд!";
            label7.Visible = true;
            label8.Visible = true;
            label9.Visible = true;
            label10.Visible = true;
            label11.Visible = true;
            label12.Visible = true;
        }

        private void initThirdPosition()
        {
            label7.Visible = false;
            label8.Visible = false;
            label9.Visible = false;
            label10.Visible = false;
            label11.Visible = false;
            label12.Visible = false;
            button1.Visible = false;
            label6.Text = "Фрактал хорош";
        }

        private void initFourthPosition()
        {
            label7.Visible = false;
            label8.Visible = false;
            label9.Visible = false;
            label10.Visible = false;
            label11.Visible = false;
            label12.Visible = false;
            label6.Text = "IK - поднять/опустить мяч";
            button1.Visible = true;
            button1.Text = "Бросить мяч";
        }

        private void AnT_MouseClick(object sender, MouseEventArgs e)
        {
            if (comboBox1.SelectedIndex == 1 && (!isSign1Taken && !isSign2Taken && !isSign3Taken) && !isLose && !isWin)
            {
                if (e.X >= 115 && e.X <= 175 && e.Y >= 60 && e.Y <= 120)
                {
                    isSign1Taken = true;
                }

                if (e.X >= 235 && e.X <= 287 && e.Y >= 60 && e.Y <= 120)
                {
                    isSign2Taken = true;
                }

                if (e.X >= 345 && e.X <= 400 && e.Y >= 60 && e.Y <= 120)
                {
                    isSign3Taken = true;
                }
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            AnT.Focus();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown1.Value > 0)
                cameraSpeed = (double)numericUpDown1.Value;
            AnT.Focus();
        }



        public Form1()
        {
            InitializeComponent();
            AnT.InitializeContexts();
        }

        private void RenderTimer_Tick(object sender, EventArgs e)
        {
            global_time += (float)RenderTimer.Interval / 1000;
            Draw();
        }

        private void выйтиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void справкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form aboutForm = new AboutForm();
            aboutForm.ShowDialog();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // инициализация openGL (glut)
            Glut.glutInit();
            Glut.glutInitDisplayMode(Glut.GLUT_RGB | Glut.GLUT_DOUBLE | Glut.GLUT_DEPTH);

            Il.ilInit();
            Il.ilEnable(Il.IL_ORIGIN_SET);

            // цвет очистки окна
            Gl.glClearColor(255, 255, 255, 1);

            // настройка порта просмотра
            Gl.glViewport(0, 0, AnT.Width, AnT.Height);

            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glLoadIdentity();
            Glu.gluPerspective(60, (float)AnT.Width / (float)AnT.Height, 0.1, 900);
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();
            Gl.glEnable(Gl.GL_DEPTH_TEST);

            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            cameraSpeed = 5;

            isSign1Taken = false;
            isSign2Taken = false;
            isSign3Taken = false;

            sign1Texture = genImage(sign1);
            sign2Texture = genImage(sign2);
            sign3Texture = genImage(sign3);
            frame1 = genImage(frame1Texture);
            frame2Texture = genImage(frame2);
            RenderTimer.Start();
        }

        private void AnT_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    translateY -= cameraSpeed;
                    break;
                case Keys.S:
                    translateY += cameraSpeed;
                    break;
                case Keys.A:
                    translateX += cameraSpeed;
                    break;
                case Keys.D:
                    translateX -= cameraSpeed;
                    break;
                case Keys.R:
                    switch (comboBox2.SelectedIndex)
                    {
                        case 0:
                            angleX += angle;

                            break;
                        case 1:
                            angleY += angle;

                            break;
                        case 2:
                            angleZ += angle;

                            break;
                        default:
                            break;
                    }
                    break;
                case Keys.E:
                    switch (comboBox2.SelectedIndex)
                    {
                        case 0:
                            angleX -= angle;
                            break;
                        case 1:
                            angleY -= angle;
                            break;
                        case 2:
                            angleZ -= angle;
                            break;
                        default:
                            break;
                    }
                    break;
                case Keys.K:
                    if (comboBox1.SelectedIndex == 1 && !isLose && !isWin)
                    {
                        if (isSign1Taken) deltaXSign1 += 1;
                        else if (isSign2Taken) deltaXSign2 += 1;
                        else if (isSign3Taken) deltaXSign3 += 1;
                        checkMovementOfSign();
                    }
                    else if (comboBox1.SelectedIndex == 3 && !isBallThrown && deltaZBall >= 0)
                    {
                        deltaZBall -= 1;
                    }
                    
                    break;
                case Keys.L:
                    if (comboBox1.SelectedIndex == 1 && !isLose && !isWin)
                    {
                        if (isSign1Taken) deltaXSign1 -= 1;
                        else if (isSign2Taken) deltaXSign2 -= 1;
                        else if (isSign3Taken) deltaXSign3 -= 1;
                        checkMovementOfSign();
                    }
                    break;
                case Keys.I:
                    if (comboBox1.SelectedIndex == 3 && !isBallThrown && deltaZBall <= 50)
                    {
                        deltaZBall += 1;
                    }
                    break;

            }

        }

        private void checkMovementOfSign()
        {
            if (isSign1Taken)
            {
                if (deltaXSign1 <= -40) isLose = true;
                else if (deltaXSign1 >= 40)
                {
                    isTrueSign1 = true;
                    isSign1Taken = false;
                    label6.Text = "Правильно!";
                }
            }
            else if (isSign2Taken)
            {
                if (deltaXSign2 <= -40)
                {
                    isTrueSign2 = true;
                    isSign2Taken = false;
                    label6.Text = "Правильно!";
                }
                else if (deltaXSign2 >= 40) isLose = true;
            }
            else if (isSign3Taken)
            {
                if (deltaXSign3 <= -40) isLose = true;
                else if (deltaXSign3 >= 40) isLose = true;
            }
            if (isTrueSign1 && isTrueSign2)
            {
                isWin = true;
                label6.Text = "Победа!";
                boom.SetNewPosition(-80, -20, 30);
                boom.SetNewPower(100);
                boom.Boooom(global_time);
                WMP.URL = @"sound.mp3";
                WMP.controls.play();

            }
            else
            if (isLose)
            {
                label6.Text = "Это фиаско :(";
            }
        }

        private uint genImage(string image)
        {
            uint sign = 0;
            Il.ilGenImages(1, out imageId);
            Il.ilBindImage(imageId);
            if (Il.ilLoadImage(image))
            {
                int width = Il.ilGetInteger(Il.IL_IMAGE_WIDTH);
                int height = Il.ilGetInteger(Il.IL_IMAGE_HEIGHT);
                int bitspp = Il.ilGetInteger(Il.IL_IMAGE_BITS_PER_PIXEL);
                switch (bitspp)
                {
                    case 24:
                        sign = MakeGlTexture(Gl.GL_RGB, Il.ilGetData(), width, height);
                        break;
                    case 32:
                        sign = MakeGlTexture(Gl.GL_RGBA, Il.ilGetData(), width, height);
                        break;
                }
            }
            Il.ilDeleteImages(1, ref imageId);
            return sign;
        }

        private static uint MakeGlTexture(int Format, IntPtr pixels, int w, int h)
        {
            uint texObject;
            Gl.glGenTextures(1, out texObject);
            Gl.glPixelStorei(Gl.GL_UNPACK_ALIGNMENT, 1);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, texObject);
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_S, Gl.GL_REPEAT);
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_T, Gl.GL_REPEAT);
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_LINEAR);
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_LINEAR);
            Gl.glTexEnvf(Gl.GL_TEXTURE_ENV, Gl.GL_TEXTURE_ENV_MODE, Gl.GL_REPLACE);
            switch (Format)
            {

                case Gl.GL_RGB:
                    Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, Gl.GL_RGB, w, h, 0, Gl.GL_RGB, Gl.GL_UNSIGNED_BYTE, pixels);
                    break;

                case Gl.GL_RGBA:
                    Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, Gl.GL_RGBA, w, h, 0, Gl.GL_RGBA, Gl.GL_UNSIGNED_BYTE, pixels);
                    break;

            }
            return texObject;
        }

    }
}
