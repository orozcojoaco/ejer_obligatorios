using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Juego_Galaga
{
    public class Enemy
    {
        private Texture2D textura;
        private Vector2 posicion;
        private float velocidad = 100f;

        public Rectangle limite => new Rectangle((int)posicion.X, (int)posicion.Y, textura.Width, textura.Height);

        public Enemy(Texture2D texture, Vector2 startPosition)
        {
            textura = texture;
            posicion = startPosition;
        }

        public void Update(GameTime gameTime)
        {
            posicion.Y += velocidad * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (posicion.Y > 1920)
                posicion.Y = -1080;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(textura, posicion, Color.White);
        }
    }
}
