using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Juego_Galaga.GameObjects
{
    public class Player2
    {
        private Texture2D textura;
        private Vector2 posicion;
        private float velocidad = 1000f;
        private int vidas;

        public Rectangle Bounds => new Rectangle((int)posicion.X, (int)posicion.Y, textura.Width, textura.Height);

        public Vector2 Posicion => posicion;
        public int Lives => vidas;


        public Player2(Texture2D texture, Vector2 startPosition)
        {
            textura = texture;
            posicion = startPosition;
            vidas = 4;
        }


        public void TakeDamage(int amount = 1)
        {
            vidas -= amount;
            if (vidas < 0)
            {
                vidas = 0;
            }
        }
        public void Update(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;


            if (keyboardState.IsKeyDown(Keys.A))
                posicion.X -= velocidad * deltaTime;
            if (keyboardState.IsKeyDown(Keys.D))
                posicion.X += velocidad * deltaTime;


            posicion.X = MathHelper.Clamp(posicion.X, 0, 1920 - textura.Width);
            posicion.Y = MathHelper.Clamp(posicion.Y, 0, 1080 - textura.Height);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(textura, posicion, Color.White);
        }
    }
}
