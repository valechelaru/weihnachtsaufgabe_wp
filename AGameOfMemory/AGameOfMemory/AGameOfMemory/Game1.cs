using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;

namespace AGameOfMemory
{
    /// <summary>
    /// Dies ist der Haupttyp für Ihr Spiel
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        const int WIDTH = 6;
        const int HEIGHT = 4;


        const int frameWidth = 16;
        //const int coverWidth = 64;



        int[] gezogeneKarte = new int[12];
        int cardselected = 0;

        Karte[,] spielfeld = new Karte[HEIGHT, WIDTH];

        Random rnd = new Random();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            /* Übrflüssiger quatsch?
            // Frame-Rate ist standardmäßig 30 fps für das Windows Phone.
            TargetElapsedTime = TimeSpan.FromTicks(333333);

            // Batterielebensdauer bei Sperre verlängern.
            InactiveSleepTime = TimeSpan.FromSeconds(1);
             */
        }

        /// <summary>
        /// Ermöglicht dem Spiel, alle Initialisierungen durchzuführen, die es benötigt, bevor die Ausführung gestartet wird.
        /// Hier können erforderliche Dienste abgefragt und alle nicht mit Grafiken
        /// verbundenen Inhalte geladen werden.  Bei Aufruf von base.Initialize werden alle Komponenten aufgezählt
        /// sowie initialisiert.
        /// </summary>
        protected override void Initialize()
        {
            


            // TODO: Fügen Sie Ihre Initialisierungslogik hier hinzu
            for (int y = 0; y < HEIGHT; y++)
            {
                for (int x = 0; x < WIDTH; x++)
                {
                    bool done = false;

                    do
                    {
                        int tempnum = rnd.Next(0, 12);

                        if (gezogeneKarte[tempnum] < 2)
                        {
                            Karte neueKarte = new Karte();
                            neueKarte.id = tempnum;
                            neueKarte.show = false;
                            gezogeneKarte[tempnum] += 1;
                            spielfeld[y, x] = neueKarte;
                            done = true;
                        }

                    } while (!done);

                }
            }

            base.Initialize();
        }

        // This is a texture we can render.
        Texture2D myTexture;

        // Set the coordinates to draw the sprite at.
        Vector2 spritePosition = Vector2.Zero;

        /// <summary>
        /// LoadContent wird einmal pro Spiel aufgerufen und ist der Platz, wo
        /// Ihr gesamter Content geladen wird.
        /// </summary>
        protected override void LoadContent()
        {
            // Erstellen Sie einen neuen SpriteBatch, der zum Zeichnen von Texturen verwendet werden kann.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            myTexture = Content.Load<Texture2D>("set");
            // TODO: Verwenden Sie this.Content, um Ihren Spiel-Inhalt hier zu laden
        }

        /// <summary>
        /// UnloadContent wird einmal pro Spiel aufgerufen und ist der Ort, wo
        /// Ihr gesamter Content entladen wird.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Entladen Sie jeglichen Nicht-ContentManager-Inhalt hier
        }

        /// <summary>
        /// Ermöglicht dem Spiel die Ausführung der Logik, wie zum Beispiel Aktualisierung der Welt,
        /// Überprüfung auf Kollisionen, Erfassung von Eingaben und Abspielen von Ton.
        /// </summary>
        /// <param name="gameTime">Bietet einen Schnappschuss der Timing-Werte.</param>
        protected override void Update(GameTime gameTime)
        {
            // Ermöglicht ein Beenden des Spiels
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Fügen Sie Ihre Aktualisierungslogik hier hinzu
            
            // Move the sprite around.
            UpdateSprite(gameTime);

            base.Update(gameTime);
        }

        void UpdateSprite(GameTime gameTime)
        {

        }

        /// <summary>
        /// Dies wird aufgerufen, wenn das Spiel selbst zeichnen soll.
        /// </summary>
        /// <param name="gameTime">Bietet einen Schnappschuss der Timing-Werte.</param>
        protected override void Draw(GameTime gameTime)
        {

            // Graphic infos
            int screenWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            int screenHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

            double totalWidth = 0.8 * screenHeight / 6;
            int coverWidth = (int) Math.Floor(totalWidth / 16.0) * 16;

            //setting spielfeld display
            /// ACHTUNG!!! da wir im Querformat sind, muss hier widht und height vertauscht werden!!
            double coverSizeW = 0.8 * screenHeight / 6;
            double coverSizeH = screenWidth / 4;

            double abstandX = (0.8 * screenHeight - 6 * coverWidth) / 7;
            double abstandY = (screenWidth - 4 * coverWidth) / 5;

            int intAbstandX = (int)Math.Round(abstandX);
            int intAbstandY = (int)Math.Round(abstandY);

            GraphicsDevice.Clear(Color.Pink);

            // TODO: Fügen Sie Ihren Zeichnungscode hier hinzu

            // Draw the sprite.
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            for(int x = 0; x < WIDTH; x++)
            {
                for (int y = 0; y < HEIGHT; y++)
                {
                    if (spielfeld[y, x].show == true)
                    {
                        spriteBatch.Draw(myTexture, new Rectangle((int)(coverWidth + intAbstandX) * x + intAbstandX, (int)(coverWidth + intAbstandY) * y + intAbstandY, coverWidth, coverWidth), new Rectangle((int)spielfeld[y, x].id * frameWidth, 0, frameWidth, frameWidth), Color.White);
                    }
                    else
                    {
                        spriteBatch.Draw(myTexture, new Rectangle((int)(coverWidth + intAbstandX) * x + intAbstandX, (int)(coverWidth + intAbstandY) * y + intAbstandY, coverWidth, coverWidth), new Rectangle(12 * frameWidth, 0, frameWidth, frameWidth), Color.White);
                    }
                }
            }
            
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
