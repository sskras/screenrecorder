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
            aviRec = null;
            aviRec = new AviWriterCompress();
            aviRec.Open(txtAviFile.Text,(uint)(1000D / (double)CaptureScreenTime.Interval - 13));
            CaptureImage();
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
            cmdSave.Enabled = true;
            txtAviFile.Enabled = true;
        }

        private void CaptureScreenTime_Tick(object sender, EventArgs e)
        {
            CaptureImage();
        }

        private void CaptureImage()
        {
            //Make sure the image gets disposed once added to avi recorder
            Image mainImage = sc.CaptureScreen();
            Image littleImage = mainImage.GetThumbnailImage(rec.Width / sizeImage, rec.Height / sizeImage, null, IntPtr.Zero);
            using (Bitmap screenCaptured = (Bitmap)littleImage)
            {
                aviRec.AddFrame(screenCaptured);
            }
            littleImage.Dispose();
            mainImage.Dispose();
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
