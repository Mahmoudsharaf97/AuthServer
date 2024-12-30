using Auth_Application.Interface;
using Auth_Application.Models;
using Auth_Core.Helper;
using Newtonsoft.Json;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;

namespace Auth_Application.Services.Captch
{
	internal class CaptchService : ICaptchService
	{
		public async Task<string> GenerateBase64Captcha(string captchaValue)
		{
			int height = 50;
			int width = 100;
			Random random = new Random();
			HatchStyle[] hatchStyleArray = new HatchStyle[40]
			{
		HatchStyle.BackwardDiagonal,
		HatchStyle.Cross,
		HatchStyle.DashedDownwardDiagonal,
		HatchStyle.DashedHorizontal,
		HatchStyle.DashedUpwardDiagonal,
		HatchStyle.DashedVertical,
		HatchStyle.DiagonalBrick,
		HatchStyle.DiagonalCross,
		HatchStyle.Divot,
		HatchStyle.DottedDiamond,
		HatchStyle.DottedGrid,
		HatchStyle.ForwardDiagonal,
		HatchStyle.Horizontal,
		HatchStyle.HorizontalBrick,
		HatchStyle.LargeCheckerBoard,
		HatchStyle.LargeConfetti,
		HatchStyle.Cross,
		HatchStyle.LightDownwardDiagonal,
		HatchStyle.LightHorizontal,
		HatchStyle.LightUpwardDiagonal,
		HatchStyle.LightVertical,
		HatchStyle.Cross,
		HatchStyle.Horizontal,
		HatchStyle.NarrowHorizontal,
		HatchStyle.NarrowVertical,
		HatchStyle.OutlinedDiamond,
		HatchStyle.Plaid,
		HatchStyle.Shingle,
		HatchStyle.SmallCheckerBoard,
		HatchStyle.SmallConfetti,
		HatchStyle.SmallGrid,
		HatchStyle.SolidDiamond,
		HatchStyle.Sphere,
		HatchStyle.Trellis,
		HatchStyle.Vertical,
		HatchStyle.Wave,
		HatchStyle.Weave,
		HatchStyle.WideDownwardDiagonal,
		HatchStyle.WideUpwardDiagonal,
		HatchStyle.ZigZag
			};
			MemoryStream memoryStream = new MemoryStream();

			string str = captchaValue;
			Bitmap bitmap = new Bitmap(width, height);
			Graphics graphics = Graphics.FromImage((Image)bitmap);
			graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
			graphics.SmoothingMode = SmoothingMode.AntiAlias;
			RectangleF rect = new RectangleF(0, 0, width, height);
			Brush brush = (Brush)new HatchBrush(hatchStyleArray[random.Next(checked(hatchStyleArray.Length - 1))], Color.FromArgb(random.Next(100, (int)byte.MaxValue), random.Next(100, (int)byte.MaxValue), random.Next(100, (int)byte.MaxValue)), Color.White);
			graphics.FillRectangle(brush, rect);
			for (int i = 0; i < str.Length; i++)
			{
				int charWidth = bitmap.Width / str.Length;
				graphics.DrawString(str.Substring(i, 1),
					new Font("Tahoma", GetRandomNumber(30, 35), GetRandomFontStyle(), GraphicsUnit.Pixel),
					GetRandomBrush(),
					GetRandomNumber(-3, 3) + (charWidth * i),
					GetRandomNumber(2, 10));

				//graphics.ResetTransform();
			}

			bitmap.Save((Stream)memoryStream, ImageFormat.Png);
			return Convert.ToBase64String(memoryStream.ToArray());
		}
		public bool ValidateCaptchaToken(string captchToken, string captchInput, string Key)
		{
			var encryptedCaptcha = AESEncryptionUtilities.DecryptString(captchToken, Key);
			var captchaToken = JsonConvert.DeserializeObject<CapchaResponse>(encryptedCaptcha);
			if ((captchaToken.ExpiredInSeconds.CompareTo(DateTime.Now.AddSeconds(-600)) < 0) ||
				(!captchaToken.Token.Equals(captchInput, StringComparison.Ordinal)))
				return false;
			else
				return true;
		}

		#region helper
		private static Random random;
		private static int GetRandomNumber(int min, int max)
		{
			random = random ?? new Random((int)DateTime.Now.Ticks);
			lock (random) // synchronize
			{
				return random.Next(min, max);
			}
		}
		private Brush GetRandomBrush()
		{

			Dictionary<int, Brush> brushes = new Dictionary<int, Brush>();
			brushes.Add(1, Brushes.Black);
			brushes.Add(2, Brushes.Blue);
			brushes.Add(3, Brushes.Gray);
			brushes.Add(4, Brushes.Brown);
			brushes.Add(5, Brushes.Chocolate);
			brushes.Add(6, Brushes.Indigo);
			brushes.Add(7, Brushes.BlueViolet);
			return brushes[GetRandomNumber(1, 7)];
		}
		private FontStyle GetRandomFontStyle()
		{
			Dictionary<int, FontStyle> fontStyles = new Dictionary<int, FontStyle>();
			fontStyles.Add(0, FontStyle.Bold);
			fontStyles.Add(1, FontStyle.Italic);
			fontStyles.Add(2, FontStyle.Regular);
			fontStyles.Add(3, FontStyle.Underline);
			fontStyles.Add(4, FontStyle.Bold | FontStyle.Italic);
			fontStyles.Add(5, FontStyle.Italic | FontStyle.Underline);
			fontStyles.Add(6, FontStyle.Italic | FontStyle.Underline | FontStyle.Bold);
			fontStyles.Add(7, FontStyle.Bold | FontStyle.Regular);
			fontStyles.Add(8, FontStyle.Underline | FontStyle.Bold);
			return fontStyles[GetRandomNumber(1, 8)];
		}
		#endregion
	}
}
