using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Juego_Galaga.GameObjects;
using Juego_Galaga.Managers;


namespace Juego_Galaga
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graficos;
        private SpriteBatch imagenes;
        private Player1 jugador;
        private EnemyManager aparecerEnemigos;
        private List<Bullet> balas;
        private Texture2D texturaJugador;
        private Texture2D texturaEnemigo;
        private Texture2D texturaBala;
        private Texture2D texturaCorazones;
        private Texture2D texturaFondo;
        private bool poderDisparar = true;
        private bool juegoIniciado = false;
        private bool juegoCompleto = false;
        private int puntaje;
        private SpriteFont fondoPuntaje;

        public Game1()
        {
            graficos = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            puntaje = 0;
            graficos.PreferredBackBufferWidth = 1920;
            graficos.PreferredBackBufferHeight = 1080;
            graficos.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            imagenes = new SpriteBatch(GraphicsDevice);
            texturaJugador = Content.Load<Texture2D>("player");
            texturaEnemigo = Content.Load<Texture2D>("enemy");
            texturaBala = Content.Load<Texture2D>("bullet");
            texturaCorazones = Content.Load<Texture2D>("heart");
            texturaFondo = Content.Load<Texture2D>("fondo");
            fondoPuntaje = Content.Load<SpriteFont>("ScoreFont");
            jugador = new Player1(texturaJugador, new Vector2(400, 900));
            aparecerEnemigos = new EnemyManager(texturaEnemigo);
            balas = new List<Bullet>();

        }

        protected override void Update(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();

            if (!juegoIniciado)
            {
                if (keyboardState.IsKeyDown(Keys.Space))
                {
                    juegoIniciado = true;
                    
                }
                return;
            }

            if (juegoCompleto || puntaje == 50)
            {
                if (keyboardState.IsKeyDown(Keys.Space))
                {
                    juegoIniciado = false;
                    juegoCompleto = true;
                    Reiniciar();           
                }
                return;
            }

            if (jugador != null)
            {
                float tiempo = (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (keyboardState.IsKeyDown(Keys.Escape))
                    Exit();

                if (keyboardState.IsKeyDown(Keys.Space) && poderDisparar)
                {
                    balas.Add(new Bullet(texturaBala, new Vector2(jugador.Posicion.X + 32, jugador.Posicion.Y)));
                    poderDisparar = false;
                }

                if (keyboardState.IsKeyUp(Keys.Space))
                {
                    poderDisparar = true;
                }

                jugador.Update(gameTime);

                aparecerEnemigos.Update(gameTime, jugador);

                foreach (var bala in balas)
                    bala.Update(gameTime);
                    balas.RemoveAll(bullet => bullet.limite.Bottom < 0);

                Colisiones();

                if (jugador.Lives == 0)
                {
                    jugador = null;
                    juegoCompleto = true;
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {              
            imagenes.Begin();
            imagenes.Draw(texturaFondo, new Rectangle(0, 0, 1920, 1080), Color.White);

            if (!juegoIniciado)
            {
                string startMessage = "Presione [ESPACIO] para comenzar";
                Vector2 messageSize = fondoPuntaje.MeasureString(startMessage);
                Vector2 messagePosition = new Vector2(
                    (graficos.PreferredBackBufferWidth - messageSize.X) / 2,
                    (graficos.PreferredBackBufferHeight - messageSize.Y) / 2);
                imagenes.DrawString(fondoPuntaje, startMessage, messagePosition, Color.White);
            }
            else if (puntaje == 50)
            {
                string winMessage = "Victoria. Has logrado salvar al mundo de los aliens.\nPresione [ESPACIO] para reiniciar.";
                Vector2 winMessageSize = fondoPuntaje.MeasureString(winMessage);
                Vector2 winMessagePosition = new Vector2(
                    (graficos.PreferredBackBufferWidth - winMessageSize.X) / 2,
                    (graficos.PreferredBackBufferHeight - winMessageSize.Y) / 2);
                imagenes.DrawString(fondoPuntaje, winMessage, winMessagePosition, Color.White);
            }
            else if (juegoCompleto && jugador == null)
            {
                string loseMessage = "Fin del juego. Los aliens destruyeron el mundo.\nPresione [ESPACIO] para reiniciar.";
                Vector2 loseMessageSize = fondoPuntaje.MeasureString(loseMessage);
                Vector2 loseMessagePosition = new Vector2(
                    (graficos.PreferredBackBufferWidth - loseMessageSize.X) / 2,
                    (graficos.PreferredBackBufferHeight - loseMessageSize.Y) / 2);
                imagenes.DrawString(fondoPuntaje, loseMessage, loseMessagePosition, Color.White);
            }
            else
            {
                jugador?.Draw(imagenes);
                aparecerEnemigos.Draw(imagenes);

                foreach (var bala in balas)
                    bala.Draw(imagenes);

                float tamañoCorazones = 0.25f;
                int espacioCorazones = 4;
                int anchoCorazones = (int)(texturaCorazones.Width * tamañoCorazones);
                int altoCorazones = (int)(texturaCorazones.Height * tamañoCorazones);

                for (int i = 0; i < jugador?.Lives; i++)
                {
                    int x = 10 + i * (anchoCorazones + espacioCorazones);
                    int y = graficos.PreferredBackBufferHeight - altoCorazones - 10;
                    imagenes.Draw(texturaCorazones, new Vector2(x, y), null, Color.White, 0f, Vector2.Zero, tamañoCorazones, SpriteEffects.None, 0f);
                }

                imagenes.DrawString(fondoPuntaje, $"Puntaje: {puntaje}", new Vector2(20, 20), Color.White);
            }

            imagenes.End();
            base.Draw(gameTime);
        }

        private void Reiniciar()
        {
            puntaje = 0;
            juegoCompleto = false;

            jugador = new Player1(texturaJugador, new Vector2(400, 900));
            balas = new List<Bullet>();

            aparecerEnemigos = new EnemyManager(texturaEnemigo);

            poderDisparar = true;
        }

        private void Colisiones()
        {
            var enemigosRemovidos = new List<Enemy>();
            var balasRemovidas = new List<Bullet>();

            foreach (var bala in balas)
            {
                foreach (var enemigo in aparecerEnemigos.Enemigos)
                {
                    if (bala.limite.Intersects(enemigo.limite))
                    {
                        balasRemovidas.Add(bala);
                        enemigosRemovidos.Add(enemigo);
                        puntaje += 10;
                    }
                }
            }

            foreach (var enemigo in aparecerEnemigos.Enemigos)
            {
                if (jugador.limite.Intersects(enemigo.limite))
                {
                    jugador.TakeDamage();
                    enemigosRemovidos.Add(enemigo);
                }
            }

            foreach (var bala in balasRemovidas)
                balas.Remove(bala);

            foreach (var enemigo in enemigosRemovidos)
                aparecerEnemigos.Enemigos.Remove(enemigo);
        }
    }
}
