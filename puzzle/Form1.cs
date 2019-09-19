using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class frmPuzzleGame : Form
    {
        int inNullSliceIndex, inmoves = 0;
        List<Bitmap> lstOriginalPictureList = new List<Bitmap>();
        System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();

        public frmPuzzleGame()
        {
            InitializeComponent();
            lstOriginalPictureList.AddRange(new Bitmap[] { Properties.Resources._1, Properties.Resources._2, Properties.Resources._3, Properties.Resources._4, Properties.Resources._5, Properties.Resources._6, Properties.Resources._7, Properties.Resources._8, Properties.Resources._9, Properties.Resources._null });
            lblMovesMade.Text += inmoves;
            lblTimeElapsed.Text = "00:00:00";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Shuffle();
        }

        void Shuffle()
        {
            do
            {
                int j;
                List<int> Indexes = new List<int>(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 9 });//8 is not present - since it is the last slice.
                Random r = new Random();
                for (int i = 0; i < 9; i++)
                {
                    Indexes.Remove((j = Indexes[r.Next(0, Indexes.Count)]));
                    ((PictureBox)gbPuzzleBox.Controls[i]).Image = lstOriginalPictureList[j];
                    if (j == 9) inNullSliceIndex = i;//store empty picture box index
                }
            } while (CheckWin());
        }

        private void btnShuffle_Click(object sender, EventArgs e)
        {
            DialogResult YesOrNo = new DialogResult();     
            if (lblTimeElapsed.Text != "00:00:00")
            {
                YesOrNo = MessageBox.Show("Yeniden Başlatmak İster misin ?", "Tavşan Puzzle", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            }
            if (YesOrNo == DialogResult.Yes || lblTimeElapsed.Text == "00:00:00")
            {
                Shuffle();
                timer.Reset();
                lblTimeElapsed.Text = "00:00:00";
                inmoves = 0;
                lblMovesMade.Text = "Hamle Sayısı : 0";
            }
        }

        private void AskPermissionBeforeQuite(object sender, FormClosingEventArgs e)
        {
            DialogResult YesOrNO = MessageBox.Show("Çıkmak için emin misiniz?", "Tavşan Puzzle", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (sender as Button != btnQuit && YesOrNO == DialogResult.No) e.Cancel = true;
            if (sender as Button == btnQuit && YesOrNO == DialogResult.Yes) Environment.Exit(0);
        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
            AskPermissionBeforeQuite(sender, e as FormClosingEventArgs);
        }

        private void SwitchPictureBox(object sender, EventArgs e)
        {
            if (lblTimeElapsed.Text == "00:00:00")
                timer.Start();
            int inPictureBoxIndex = gbPuzzleBox.Controls.IndexOf(sender as Control);
            if (inNullSliceIndex != inPictureBoxIndex)
            {
                List<int> FourBrothers = new List<int>(new int[] { ((inPictureBoxIndex % 3 == 0) ? -1 : inPictureBoxIndex - 1), inPictureBoxIndex - 3, (inPictureBoxIndex % 3 == 2) ? -1 : inPictureBoxIndex + 1, inPictureBoxIndex + 3 });
                if (FourBrothers.Contains(inNullSliceIndex))
                {
                    ((PictureBox)gbPuzzleBox.Controls[inNullSliceIndex]).Image = ((PictureBox)gbPuzzleBox.Controls[inPictureBoxIndex]).Image;
                    ((PictureBox)gbPuzzleBox.Controls[inPictureBoxIndex]).Image = lstOriginalPictureList[9];
                    inNullSliceIndex = inPictureBoxIndex;
                    lblMovesMade.Text = "Hamle Sayısı  : " + (++inmoves);
                    if (CheckWin())
                    {
                        timer.Stop();
                        (gbPuzzleBox.Controls[8] as PictureBox).Image = lstOriginalPictureList[8];
                        MessageBox.Show("Tebrikler...\nAdamsın\nGeçen Süre : " + timer.Elapsed.ToString().Remove(8) + "\nToplam Hamle Sayısı : " + inmoves, "Tavşan Puzzle");
                        inmoves = 0;
                        lblMovesMade.Text = "Hamle Sayısı: 0";
                        lblTimeElapsed.Text = "00:00:00";
                        timer.Reset();
                        Shuffle();
                    }
                }
            }
        }

        bool CheckWin()
        {
            int i;
            for (i = 0; i < 8; i++)
            {
                if ((gbPuzzleBox.Controls[i] as PictureBox).Image != lstOriginalPictureList[i]) break;
            }
            if (i == 8) return true;
            else return false;
        }

        private void UpdateTimeElapsed(object sender, EventArgs e)
        {
            if (timer.Elapsed.ToString() != "00:00:00")
                lblTimeElapsed.Text = timer.Elapsed.ToString().Remove(8);
            if (timer.Elapsed.ToString() == "00:00:00")
                btnPause.Enabled = false;
            else
                btnPause.Enabled = true;
            if (timer.Elapsed.Minutes.ToString() == "2")
            {
                timer.Reset();
                lblMovesMade.Text = "Hamle Sayısı : 0";
                lblTimeElapsed.Text = "00:00:00";
                inmoves = 0;
                btnPause.Enabled = false;
                MessageBox.Show("Süre Bitti!\nTekrar Deneyiniz", "Tavşan Puzzle");
                Shuffle();
            }
        }

        private void lblMovesMade_Click(object sender, EventArgs e)
        {

        }

        private void gbOriginal_Enter(object sender, EventArgs e)
        {

        }

        private void gbPuzzleBox_Enter(object sender, EventArgs e)
        {

        }

        private void PauseOrResume(object sender, EventArgs e)
        {
            if (btnPause.Text == "Duraklat")
            {
                timer.Stop();
                gbPuzzleBox.Visible = false;
                btnPause.Text = "Devam Et";
            }
            else
            {
                timer.Start();
                gbPuzzleBox.Visible = true;
                btnPause.Text = "Duraklat";
            }
        }
    }
}
