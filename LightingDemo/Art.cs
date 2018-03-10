using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace LightningDemo
{
	static class Art
	{
		public static Texture2D LightningSegment, HalfCircle, Pixel;

		public static void Load(ContentManager content)
		{
			LightningSegment = content.Load<Texture2D>("Images/Lightning Segment");
			HalfCircle = content.Load<Texture2D>("Images/Half Circle");
			Pixel = content.Load<Texture2D>("Images/Pixel");
		}
	}
}
