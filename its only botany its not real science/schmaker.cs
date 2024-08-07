/*using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;

class schmaker
{
	static Bitmap ApplyLookupTable(Bitmap image, byte[][] lut)
	{
		Bitmap result = new Bitmap(image.Width, image.Height);
		BitmapData srcData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
		BitmapData dstData = result.LockBits(new Rectangle(0, 0, result.Width, result.Height), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);

		int stride = srcData.Stride;
		IntPtr srcScan0 = srcData.Scan0;
		IntPtr dstScan0 = dstData.Scan0;

		unsafe
		{
			byte* pSrc = (byte*)(void*)srcScan0;
			byte* pDst = (byte*)(void*)dstScan0;

			for (int y = 0; y < image.Height; y++)
			{
				for (int x = 0; x < image.Width; x++)
				{
					int idx = (y * stride) + (x * 3);
					pDst[idx + 2] = lut[0][pSrc[idx + 2]];
					pDst[idx + 1] = lut[1][pSrc[idx + 1]];
					pDst[idx + 0] = lut[2][pSrc[idx + 0]];
				}
			}
		}

		image.UnlockBits(srcData);
		result.UnlockBits(dstData);

		return result;
	}

	static Bitmap LoadEmbeddedLUT()
	{
		Assembly assembly = Assembly.GetExecutingAssembly();
		using (Stream stream = assembly.GetManifestResourceStream("P3RE_ShC.P3RE_ShC_LUT.png"))
		{
			if (stream == null)
				throw new Exception("Could not find embedded LUT image resource.");

			return new Bitmap(stream);
		}
	}

	static void Main(string[] args)
	{
		if (args.Length < 1)
		{
			Console.WriteLine("Usage: <input_image1> <input_image2> ... <input_imageN>");
			return;
		}

		// Load lookup table from an embedded image
		Bitmap lutImage;
		try
		{
			lutImage = LoadEmbeddedLUT();
		}
		catch (Exception ex)
		{
			Console.WriteLine("Could not load embedded LUT image.");
			Console.WriteLine(ex.Message);
			return;
		}

		byte[][] lut = new byte[3][];
		for (int channel = 0; channel < 3; channel++)
		{
			lut[channel] = new byte[256];
			for (int i = 0; i < 256; i++)
			{
				lut[channel][i] = lutImage.GetPixel(i, 0).R;
			}
		}

		foreach (string inputImagePath in args)
		{
			Bitmap inputImage;
			try
			{
				inputImage = new Bitmap(inputImagePath);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Could not read input image: {inputImagePath}");
				Console.WriteLine(ex.Message);
				continue;
			}

			// Resize input image to half the size
			Bitmap resizedImage = new Bitmap(inputImage, new Size(inputImage.Width / 2, inputImage.Height / 2));
			Bitmap processedImage = ApplyLookupTable(resizedImage, lut);

			// Save the processed image with a _ShC suffix
			string baseName = Path.GetFileNameWithoutExtension(inputImagePath);
			string ext = Path.GetExtension(inputImagePath);
			string outputImagePath = Path.Combine(Path.GetDirectoryName(inputImagePath), baseName + "_ShC" + ext);

			try
			{
				processedImage.Save(outputImagePath);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Could not save output image: {outputImagePath}");
				Console.WriteLine(ex.Message);
			}
		}
	}
}
*/