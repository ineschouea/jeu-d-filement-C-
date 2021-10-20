using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using WMPLib;

namespace MiniProjet_Chouea_Ines
{
    public partial class Form1 : Form
    {
        WindowsMediaPlayer music = new WindowsMediaPlayer();
        bool goLeft, goRight, jumping, isGameOver;

        int jumpSpeed;
        int force;
        int score = 0;
        int playerSpeed = 7;

        int horizontalSpeed = 5;
        int verticalSpeed = 3;

        int enemyOneSpeed = 2;
        int enemyTwoSpeed = 4;

        DateTime  startTime, endTime;

        System.Timers.Timer t;
        int h, m, s;

        public Form1()
        {
            InitializeComponent();
            music.URL = "music.mp3";
        }
        public String conString = "Data Source=DESKTOP-GD54TOK;Initial Catalog=bestTime;Integrated Security=True";
        private void Form1_Load(object sender, EventArgs e)
        {
            music.controls.play();

            t = new System.Timers.Timer();
            t.Interval = 1000;
            t.Elapsed += onTimeEvent;

        }

        private void onTimeEvent(object sender, System.Timers.ElapsedEventArgs e)
        {
            Invoke(new Action(() => {
                s += 1;
                if (s == 60)
                {
                    s = 0;
                    m += 1;
                }

                if (m == 60)
                {
                    m = 0;
                    h += 1;
                }
                chrono.Text = string.Format("{0}:{1}:{2}", h.ToString().PadLeft(2, '0'), m.ToString().PadLeft(2, '0'), s.ToString().PadLeft(2, '0'));
            }));
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox11_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox11_Click_1(object sender, EventArgs e)
        {

        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(conString);
            con.Open();

            SqlCommand cmd = new SqlCommand("update best set name=@p1, time=@p2 where id=1", con);
            cmd.Parameters.Add("p1", winner.Text);
            cmd.Parameters.Add("p2", timeBox.Text);
            cmd.ExecuteNonQuery();

            //MessageBox.Show("Saved");

           

            con.Close();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void close(object sender, FormClosingEventArgs e)
        {
            t.Stop();
            Application.DoEvents();

        }

        private void MainGameTimerEvent(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(conString);

            txtScore.Text = "Score: " + score + Environment.NewLine + "Collect all the coins";
            startTime = DateTime.Now;
            t.Start();

            player.Top += jumpSpeed;

            if (goLeft == true)
            {
                player.Left -= playerSpeed;
            }
            if (goRight == true)
            {
                player.Left += playerSpeed;
            }

            if (jumping == true && force < 0)
            {
                jumping = false;
            }

            if (jumping == true)
            {
                jumpSpeed = -8;
                force -= 1;
            }
            else
            {
                jumpSpeed = 10;
            }

            if (score == 17)
            {
                txtScore.Text = "Score: " + score;
                door.Image = Properties.Resources.door_open;


            }

            foreach (Control x in this.Controls)
            {
                if (x is PictureBox)
                {

                    if ((string)x.Tag == "platform")
                    {
                        if (player.Bounds.IntersectsWith(x.Bounds))
                        {
                            force = 8;
                            player.Top = x.Top - player.Height;

                            //moving player with the same speed of platform to not fall down
                            if ((string)x.Name == "horizintalplatform" && goLeft == false || (string)x.Name == "horizintalplatform" && goRight == false)
                            {
                                player.Left -= horizontalSpeed;
                            }

                        }
                         x.BringToFront();
                    }

                    if ((string)x.Tag == "coin")
                    {
                        if (player.Bounds.IntersectsWith(x.Bounds) && x.Visible == true)
                        {
                            x.Visible = false;
                            score++;
                        }
                    }

                    if ((string)x.Tag == "ennemy")
                    {
                        if (player.Bounds.IntersectsWith(x.Bounds))
                        {
                            gametimer.Stop();
                            isGameOver = true;
                            txtScore.Text = "Score: " + score + Environment.NewLine + "You lose! To play again, press enter";
                            
                            
                            }
                    }

                }

            }

            //Moving platforms

            horizintalplatform.Left -= horizontalSpeed;

            if (horizintalplatform.Left < 0 || horizintalplatform.Left + horizintalplatform.Width > this.ClientSize.Width)
            {
                horizontalSpeed = -horizontalSpeed;
            }

            verticalplatform.Top += verticalSpeed;

            if (verticalplatform.Top < 166 || verticalplatform.Top > 496)
            {
                verticalSpeed = -verticalSpeed;
            }

            //Moving ennemies
            ennemy1.Left -= enemyOneSpeed;

            if (ennemy1.Left < pictureBox3.Left || ennemy1.Left + ennemy1.Width > pictureBox3.Left + pictureBox3.Width)
            {
                enemyOneSpeed = -enemyOneSpeed;
            }

            ennemy2.Left += enemyTwoSpeed;

            if (ennemy2.Left < pictureBox7.Left || ennemy2.Left + ennemy2.Width > pictureBox7.Left + pictureBox7.Width)
            {
                enemyTwoSpeed = -enemyTwoSpeed;
            }

            //Falling from platforms
            if (player.Top + player.Height > this.ClientSize.Height + 50)
            {
                gametimer.Stop();
                isGameOver = true;
                txtScore.Text = "Score: " + score + Environment.NewLine + "You lose! To play again, press enter";
            }

            if (player.Bounds.IntersectsWith(door.Bounds) && score == 17)
            {
                gametimer.Stop();
                isGameOver = true;
                txtScore.Text = "Score: " + score + Environment.NewLine + "Mission completed successfully !";

                //played time
                //////
                endTime = DateTime.Now;
                TimeSpan time = endTime - startTime;
                t.Stop();
                con.Open();

                SqlCommand cmd = new SqlCommand("Select name, time from best", con);
                SqlDataReader data = cmd.ExecuteReader();

                String name = "Ines";
                String bestTime = "0";

                while (data.Read())
                {
                    name = data.GetValue(0).ToString();
                    bestTime = data.GetValue(1).ToString();
                }

                con.Close();
                if (Double.Parse(time.TotalMilliseconds.ToString()) > Double.Parse(bestTime))

                    MessageBox.Show("You have spent " + time.TotalMilliseconds.ToString() + " milliseconds " + Environment.NewLine + name + " is faster than you (" + bestTime + ")");
                else
                {
                    MessageBox.Show("You have spent " + time.TotalMilliseconds.ToString() + " milliseconds " + Environment.NewLine + "You are the best! type your name");
                    winner.Visible = true;
                    button1.Visible = true;
                    timeBox.Text = time.TotalMilliseconds.ToString();
                }
            }
           


        }



        private void KesyIsDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                goLeft = true;
            }
            if (e.KeyCode == Keys.Right)
            {
                goRight = true;
            }
            if (e.KeyCode == Keys.Space && jumping == false)
            {
                jumping = true;
            }
            if (e.KeyCode == Keys.Q)
            {
                Application.Exit();
            }

        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                goLeft = false;
            }
            if (e.KeyCode == Keys.Right)
            {
                goRight = false;
            }
            if (jumping == true)
            {
                jumping = false;
            }

            if (e.KeyCode == Keys.Enter && isGameOver == true)
            {
                RestartGame();
            }

        }

        private void RestartGame()
        {
            jumping = false;
            goLeft = false;
            goRight = false;
            isGameOver = false;
            score = 0;

            txtScore.Text = "Score: " + score;

            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && x.Visible == false)
                {
                    x.Visible = true;
                }
            }

            player.Left = 158;
            player.Top = 449;

            ennemy1.Left = 358;
            ennemy2.Left = 483;

            horizintalplatform.Left = 320;
            verticalplatform.Top = 313;

            door.Image = Properties.Resources.door_closed;

            gametimer.Start();
 
        }

    }
}
