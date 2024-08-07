using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

class Program
{
	static void Main(string[] args)
	{
		if (args.Length == 0)
		{
			Console.WriteLine("Please drag a PNG file onto the program.");
			return;
		}

		string inputFilePath = args[0];
		if (!File.Exists(inputFilePath) || Path.GetExtension(inputFilePath).ToLower() != ".png")
		{
			Console.WriteLine("Please provide a valid PNG file.");
			return;
		}

		try
		{
			// Load the image
			using (System.Drawing.Image image = System.Drawing.Image.FromFile(inputFilePath))
			{
				// Create a new bitmap with the same size as the original
				using (Bitmap bitmap = new Bitmap(image.Width, image.Height))
				{
					// Iterate through each pixel to reduce the opacity
					for (int y = 0; y < image.Height; y++)
					{
						for (int x = 0; x < image.Width; x++)
						{
							Color originalColor = ((Bitmap)image).GetPixel(x, y);
							Color newColor = Color.FromArgb((int)(originalColor.A * 0.5), originalColor.R, originalColor.G, originalColor.B); // this is basically the opacity level, so 50% is the "dummy" value p3r uses. However in some cases 0.25 (75% opacity) is needed for things like personas or some npcs oddly.
							bitmap.SetPixel(x, y, newColor);
						}
					}

					// Generate the output file path with _composite suffix
					string outputFilePath = Path.Combine(Path.GetDirectoryName(inputFilePath),
						Path.GetFileNameWithoutExtension(inputFilePath) + "_composite.png");

					// Save the new image
					bitmap.Save(outputFilePath, ImageFormat.Png);

					Console.WriteLine($"The image has been saved to {outputFilePath}");
				}
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine($"An error occurred: {ex.Message}");
		}
	}
}
