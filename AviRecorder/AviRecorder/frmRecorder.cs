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
        /// <summary>
        /// The AVI Writer class
        /// </summary>
        private AviWriterCompress aviRec;
        
        /// <summary>
        /// Holds and array of bitmap images
        /// This was use to speed up the video recording process
        /// </summary>
        private List<Image> aviBits;

        /// <summary>
        /// Used for capturing the main screen
        /// </summary>
        private ScreenShotDemo.ScreenCapture sc;

        /// <summary>
        /// Rectangle object to hold the screenboundries
        /// </summary>
        private Rectangle rec = Screen.PrimaryScreen.Bounds;

        /// <summary>
        /// Used for dividing against the screen size to reduce the image data size in bytes
        /// </summary>
        private int sizeImage = 4;

        /// <summary>
        /// Counts the amount of frames to be stored before pushing them to the avi stream
        /// </summary>
        private int Counter = 20;

        /// <summary>
        /// Contructor for making the frmRecorder GUI
        /// </summary>
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

        /// <summary>
        /// Function which starts the video recording
        /// </summary>
        private void StartVid()
        {
            cmdStart.Text = "Stop";
            txtAviFile.Enabled = false;
            cmdSave.Enabled = false;
            File.Create(txtAviFile.Text).Close();
            aviRec.Open(txtAviFile.Text,10);
            CaptureScreenTime.Enabled = true;
        }

        /// <summary>
        /// Function which stops the video recording
        /// </summary>
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

        /// <summary>
        /// Add the frames from the aviBits array
        /// </summary>
        private void addFrames()
        {
            CaptureScreenTime.Enabled = false;
            foreach (Image img in aviBits)
            {
                aviRec.AddFrame((Bitmap)img);
                img.Dispose();
            }
            aviBits.Clear();
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
