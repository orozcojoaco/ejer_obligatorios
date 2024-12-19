using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Juego_Galaga.GameObjects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Juego_Galaga.Managers
{
    public class EnemyManager
    {

        private List<Enemy> enemigos;
        private Texture2D texturaEnemigos;
        private Random random;
        private float tiempoSpawn;
        private float intervaloSpawn = 0.5f;
        private int contadorescapeEnemigo;
        public int ContadorEscapeEnemigo => contadorescapeEnemigo;
        public List<Enemy> Enemigos => enemigos;

        public EnemyManager(Texture2D enemyTexture)
        {
            enemigos = new List<Enemy>();
            texturaEnemigos = enemyTexture;
            random = new Random();
            contadorescapeEnemigo = 0;

        }

        public void Update(GameTime gameTime, Player1 jugador)
        {
            tiempoSpawn += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (tiempoSpawn >= intervaloSpawn)
            {
                SpawnEnemy();
                tiempoSpawn = 0f;
            }

            foreach (var enemigo in enemigos)
            {
                enemigo.Update(gameTime);
            }

            int enemigosEscapados = enemigos.RemoveAll(enemy => enemy.limite.Top > 1080);
            if (enemigosEscapados > 0)
            {
                jugador.TakeDamage(enemigosEscapados);
            }
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var enemigo in enemigos)
            {
                enemigo.Draw(spriteBatch);
            }
        }

        private void SpawnEnemy()
        {

            int xPosition = random.Next(0, 1920 - texturaEnemigos.Width);
            int yPosition = -texturaEnemigos.Height;

            enemigos.Add(new Enemy(texturaEnemigos, new Vector2(xPosition, yPosition)));
        }

    }
}
