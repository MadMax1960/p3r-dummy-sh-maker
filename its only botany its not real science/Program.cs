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

		Console.WriteLine("Enter the desired opacity level:");
		Console.WriteLine("1: 0% (Fully Transparent)");
		Console.WriteLine("2: 25%");
		Console.WriteLine("3: 50%");
		Console.WriteLine("4: 75%");
		string input = Console.ReadLine();
		double opacity = input switch
		{
			"1" => 0.0,
			"2" => 0.25,
			"3" => 0.5,
			"4" => 0.75,
			_ => 0.5 // Default to 50% if invalid input
		};

		try
		{
			// Load the image
			using (System.Drawing.Image image = System.Drawing.Image.FromFile(inputFilePath))
			{
				// Create a new bitmap with the same size as the original
				using (Bitmap bitmap = new Bitmap(image.Width, image.Height))
				{
					// Iterate through each pixel to adjust the opacity
					for (int y = 0; y < image.Height; y++)
					{
						for (int x = 0; x < image.Width; x++)
						{
							Color originalColor = ((Bitmap)image).GetPixel(x, y);
							Color newColor = Color.FromArgb((int)(originalColor.A * opacity), originalColor.R, originalColor.G, originalColor.B);
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
