using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.IO;

namespace AVITools {

	/// <summary>Extract bitmaps from AVI files</summary>
	public class AviReader{

		//position of the first frame, count of frames in the stream
		private int firstFrame = 0, countFrames = 0;
		//pointers
		private int aviFile = 0;
		private int getFrameObject;
		private IntPtr aviStream;
		//stream and header info
		private Avi.AVISTREAMINFO streamInfo;

		public int CountFrames{
			get{ return countFrames; }
		}

		public UInt32 FrameRate{
			get{ return streamInfo.dwRate / streamInfo.dwScale; }
		}

		public Size BitmapSize{
			get{ return new Size( (int)streamInfo.rcFrame.right, (int)streamInfo.rcFrame.bottom); }
		}

		/// <summary>Opens an AVI file and creates a GetFrame object</summary>
		/// <param name="fileName">Name of the AVI file</param>
		public void Open(string fileName) {
			//Intitialize AVI library
			Avi.AVIFileInit();

			//Open the file
			int result = Avi.AVIFileOpen(
				ref aviFile, fileName,
				Avi.OF_SHARE_DENY_WRITE, 0);

			if(result != 0){ throw new Exception("Exception in AVIFileOpen: "+result.ToString()); }

			//Get the video stream
			result = Avi.AVIFileGetStream(
				aviFile,
				out aviStream,
				Avi.StreamtypeVIDEO, 0);

			if(result != 0){ throw new Exception("Exception in AVIFileGetStream: "+result.ToString()); }

			firstFrame = Avi.AVIStreamStart(aviStream.ToInt32());
			countFrames = Avi.AVIStreamLength(aviStream.ToInt32());
			
			streamInfo = new Avi.AVISTREAMINFO();
			result = Avi.AVIStreamInfo(aviStream.ToInt32(), ref streamInfo, Marshal.SizeOf(streamInfo));

			if(result != 0){ throw new Exception("Exception in AVIStreamInfo: "+result.ToString()); }
			
			//Open frames

			Avi.BITMAPINFOHEADER bih = new Avi.BITMAPINFOHEADER();
			bih.biBitCount = 24;
			bih.biClrImportant = 0;
			bih.biClrUsed = 0;
			bih.biCompression = 0; //BI_RGB;
			bih.biHeight = (Int32)streamInfo.rcFrame.bottom;
			bih.biWidth = (Int32)streamInfo.rcFrame.right;
			bih.biPlanes = 1;
			bih.biSize = (UInt32)Marshal.SizeOf(bih);
			bih.biXPelsPerMeter = 0;
			bih.biYPelsPerMeter = 0;

			getFrameObject = Avi.AVIStreamGetFrameOpen(aviStream, ref bih); //force function to return 24bit DIBS
			//getFrameObject = Avi.AVIStreamGetFrameOpen(aviStream, 0); //return any bitmaps
			if(getFrameObject == 0){ throw new Exception("Exception in AVIStreamGetFrameOpen!"); }
		}

		/// <summary>Closes all streams, files and libraries</summary>
		public void Close() {
			if(getFrameObject != 0){
				Avi.AVIStreamGetFrameClose(getFrameObject);
				getFrameObject = 0;
			}
			if(aviStream != IntPtr.Zero){
				Avi.AVIStreamRelease(aviStream);
				aviStream = IntPtr.Zero;
			}
			if(aviFile != 0){
				Avi.AVIFileRelease(aviFile);
				aviFile = 0;
			}
			Avi.AVIFileExit();
		}

		/// <summary>Exports a frame into a bitmap file</summary>
		/// <param name="position">Position of the frame</param>
		/// <param name="dstFileName">Name ofthe file to store the bitmap</param>
		public void ExportBitmap(int position, String dstFileName){
			if(position > countFrames){
				throw new Exception("Invalid frame position");
			}

			//Decompress the frame and return a pointer to the DIB
			int pDib = Avi.AVIStreamGetFrame(getFrameObject, firstFrame + position);
			//Copy the bitmap header into a managed struct
			Avi.BITMAPINFOHEADER bih = new Avi.BITMAPINFOHEADER();
			bih = (Avi.BITMAPINFOHEADER)Marshal.PtrToStructure(new IntPtr(pDib), bih.GetType());

			/*if(bih.biBitCount < 24){
				throw new Exception("Not enough colors! DIB color depth is less than 24 bit.");
			}else */
			if(bih.biSizeImage < 1){
				throw new Exception("Exception in AVIStreamGetFrame: Not bitmap decompressed.");
			}

			//Copy the image
			byte[] bitmapData = new byte[bih.biSizeImage];
			int address = pDib + Marshal.SizeOf(bih);
			for(int offset=0; offset<bitmapData.Length; offset++){
				bitmapData[offset] = Marshal.ReadByte(new IntPtr(address));
				address++;
			}

			//Copy bitmap info
			byte[] bitmapInfo = new byte[Marshal.SizeOf(bih)];
			IntPtr ptr;
			ptr = Marshal.AllocHGlobal(bitmapInfo.Length);
			Marshal.StructureToPtr(bih, ptr, false);
			address = ptr.ToInt32();
			for(int offset=0; offset<bitmapInfo.Length; offset++){
				bitmapInfo[offset] = Marshal.ReadByte(new IntPtr(address));
				address++;
			}

			//Create file header
			Avi.BITMAPFILEHEADER bfh = new Avi.BITMAPFILEHEADER();
			bfh.bfType = Avi.BMP_MAGIC_COOKIE;
			bfh.bfSize = (Int32)(55 + bih.biSizeImage); //size of file as written to disk
			bfh.bfReserved1 = 0;
			bfh.bfReserved2 = 0;
			bfh.bfOffBits = Marshal.SizeOf(bih) + Marshal.SizeOf(bfh);
		
			//Create or overwrite the destination file
			FileStream fs = new FileStream(dstFileName, System.IO.FileMode.Create);
			BinaryWriter bw = new BinaryWriter(fs);

			//Write header
			bw.Write(bfh.bfType);
			bw.Write(bfh.bfSize);
			bw.Write(bfh.bfReserved1);
			bw.Write(bfh.bfReserved2);
			bw.Write(bfh.bfOffBits);
			//Write bitmap info
			bw.Write(bitmapInfo);
			//Write bitmap data
			bw.Write(bitmapData);
			bw.Close();
			fs.Close();
		}
	}
}
