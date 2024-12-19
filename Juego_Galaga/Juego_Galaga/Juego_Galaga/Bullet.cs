using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Juego_Galaga.GameObjects
{
    public class Bullet
    {
        private Texture2D textura;
        private Vector2 posicion;
        private float velocidad = 1000f;


        public Rectangle limite => new Rectangle((int)posicion.X, (int)posicion.Y, textura.Width, textura.Height);


        public Bullet(Texture2D texture, Vector2 startPosition)
        {
            textura = texture;
            posicion = startPosition;
        }

        public void Update(GameTime gameTime)
        {
            posicion.Y -= velocidad * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(textura, posicion, Color.White);
        }
    }
}
