using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AVITools;
using System.IO;
using System.Collections;

namespace AviRecorder
{
    public partial class frmRecorder : Form
    {

        private AviWriterCompress aviRec;
        private List<Image> aviBits;
        private ScreenShotDemo.ScreenCapture sc;
        private Rectangle rec = Screen.PrimaryScreen.Bounds;
        private int sizeImage = 4;
        private int Counter = 20;

        public frmRecorder()
        {
            InitializeComponent();
            aviRec = new AviWriterCompress();
            aviBits = new List<Image>();
            sc = new ScreenShotDemo.ScreenCapture();
            numScreenSize.Value = sizeImage;
        }

        private void cmdStart_Click(object sender, EventArgs e)
        {
            if (txtAviFile.Text != string.Empty)
            {
                if (cmdStart.Text == "Start")
                    StartVid();
                else
                    StopVid();
            }
        }

        private void StartVid()
        {
            cmdStart.Text = "Stop";
            txtAviFile.Enabled = false;
            cmdSave.Enabled = false;
            File.Create(txtAviFile.Text).Close();
            aviRec.Open(txtAviFile.Text,10);
            CaptureScreenTime.Enabled = true;
        }

        private void StopVid()
        {
            cmdStart.Text = "Start";
            CaptureScreenTime.Enabled = false;
            
            aviRec.Close();
            
            txtAviFile.Enabled = true;
            cmdSave.Enabled = true;
        }

        private void CaptureScreenTime_Tick(object sender, EventArgs e)
        {
            //aviBits.Add(sc.CaptureScreen().GetThumbnailImage(rec.Width / sizeImage, rec.Height / sizeImage, null, IntPtr.Zero));
            CaptureScreenTime.Enabled = false;
            aviRec.AddFrame((Bitmap)sc.CaptureScreen().GetThumbnailImage(rec.Width / sizeImage, rec.Height / sizeImage, null, IntPtr.Zero));
            CaptureScreenTime.Enabled = true;
            //if (aviBits.Count <= Counter)
            //{
            //    addFrames();
            //}
        }

        private void addFrames()
        {
            CaptureScreenTime.Enabled = false;
            foreach (Image img in aviBits)
            {
                aviRec.AddFrame((Bitmap)img);
                img.Dispose();
            }
            aviBits.Clear();
            
            //System.GC.ReRegisterForFinalize(aviBits);
            aviBits = null;
            aviBits = new List<Image>();
            CaptureScreenTime.Enabled = true;
        }

        private void cmdSave_Click(object sender, EventArgs e)
        {
            if (createAviDlg.ShowDialog() == DialogResult.OK)
            {
                txtAviFile.Text = createAviDlg.FileName;
            }
        }

        private void numScreenSize_ValueChanged(object sender, EventArgs e)
        {
            sizeImage = Convert.ToInt32(numScreenSize.Value);
        }
    }
}
