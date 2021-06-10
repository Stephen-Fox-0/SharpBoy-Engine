using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SharpBoyEngine.Screen.Extentions
{
    public static class SpriteBatchExtentions
    {
		public static bool ShadowsEnabled = true;

        public static void DrawLine( this SpriteBatch spriteBatch, Texture2D lineTexture, Vector2 start, Vector2 end, Color color )
		{
			Vector2 edge = end - start;
			// calculate angle to rotate line
			float angle =
				(float)Math.Atan2( edge.Y, edge.X );


			spriteBatch.Draw( lineTexture,
				new Rectangle(
					(int)start.X,
					(int)start.Y,
					(int)edge.Length(),
					1
				),
				null,
				color,
				angle,
				Vector2.Zero,
				SpriteEffects.None,
				0
			);
		}
		public static void DrawTextShadow( this SpriteBatch spriteBatch, SpriteFont font, string text, Vector2 position, Color color, float alpha = 0.3f )
		{
			if (ShadowsEnabled)
			{
				spriteBatch.DrawString( font, text, new Vector2( position.X, position.Y + 1 ), Color.Black * alpha );
				spriteBatch.DrawString( font, text, new Vector2( position.X+1, position.Y + 2 ), Color.Black * (alpha / 2) );
				spriteBatch.DrawString( font, text, new Vector2( position.X, position.Y + 3 ), Color.Black * (alpha / 3) );
			}
			spriteBatch.DrawString( font, text, position, color );
		}
		public static void DrawTextShadow( this SpriteBatch spriteBatch, SpriteFont font, string text, Vector2 position, Color color, Vector2 origin, float alpha = 0.3f, float scale = 1f )
		{
			if (ShadowsEnabled) spriteBatch.DrawString( font, text, new Vector2( position.X + 1, position.Y + 1 ), Color.Black * alpha, 0f, origin, scale, SpriteEffects.None, 0f );
			spriteBatch.DrawString( font, text, position, color, 0f, origin, scale, SpriteEffects.None, 0f );
		}

		public static void DrawTextShadow( this SpriteBatch spriteBatch, SpriteFont font, string text, Vector2 position, Color color, Color shadowColor, Vector2 origin, float alpha = 0.3f, float scale = 1f )
		{
			if (ShadowsEnabled) spriteBatch.DrawString( font, text, new Vector2( position.X + 1, position.Y + 1 ), shadowColor * alpha, 0f, origin, scale, SpriteEffects.None, 0f );
			spriteBatch.DrawString( font, text, position, color, 0f, origin, scale, SpriteEffects.None, 0f );
		}


		public static void DrawTextureShadow( this SpriteBatch spriteBatch, Texture2D texture, Rectangle rectangle, Color color, float alpha = 0.3f )
		{
			if (ShadowsEnabled)
			{
				spriteBatch.Draw( texture, new Rectangle( rectangle.X + 1, rectangle.Y + 1, rectangle.Width, rectangle.Height ), Color.Black * alpha );
				spriteBatch.Draw( texture, new Rectangle( rectangle.X + 2, rectangle.Y + 2, rectangle.Width, rectangle.Height ), Color.Black * (alpha / 3) );
				spriteBatch.Draw( texture, new Rectangle( rectangle.X + 3, rectangle.Y +3, rectangle.Width, rectangle.Height ), Color.Black * (alpha / 5) );
			}
			spriteBatch.Draw( texture, rectangle, color );
		}

		public static void DrawTextureShadow( this SpriteBatch spriteBatch, Texture2D texture, Rectangle rectangle, Color color, float rotation, Vector2 origin, float alpha = 0.3f )
		{
			if(ShadowsEnabled) spriteBatch.Draw( texture, new Rectangle( rectangle.X - 1, rectangle.Y - 1, rectangle.Width, rectangle.Height ), null, Color.Black * alpha, rotation, origin, SpriteEffects.None, 0f );
			spriteBatch.Draw( texture, new Rectangle( rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height ), null, Color.Black * alpha, rotation, origin, SpriteEffects.None, 0f );
		}
		public static void DrawTextureShadow( this SpriteBatch spriteBatch, Texture2D texture, Vector2 position, Color color, float alpha = 0.3f )
		{
			if(ShadowsEnabled) spriteBatch.Draw( texture, new Vector2( position.X + 1, position.Y + 1 ), Color.Black * alpha );
			spriteBatch.Draw( texture, position, color );
		}

		public static void DrawRectangle( this SpriteBatch spriteBatch, Texture2D texture, Rectangle rect, Color color, int thickness = 1)
		{

			for (int i = 0; i < thickness; i++)
			{
				spriteBatch.DrawLine( texture, new Vector2( rect.X + i, rect.Y ), new Vector2( rect.X + i, rect.Y + rect.Height ), color );
				spriteBatch.DrawLine( texture, new Vector2( rect.X + rect.Width + i, rect.Y ), new Vector2( rect.X + rect.Width + i, rect.Y + rect.Height ), color );
				spriteBatch.DrawLine( texture, new Vector2( rect.X, rect.Y + rect.Height + i ), new Vector2( rect.X + rect.Width, rect.Y + rect.Height + i ), color );
				spriteBatch.DrawLine( texture, new Vector2( rect.X, rect.Y - i ), new Vector2( rect.X + rect.Width, rect.Y ), color );
			}
		}
	}
}
