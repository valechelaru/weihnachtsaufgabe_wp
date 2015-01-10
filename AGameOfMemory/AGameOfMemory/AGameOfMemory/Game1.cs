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
        //Graphics Kram, SpielDefinitionen
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        const int WIDTH = 6;
        const int HEIGHT = 4;


        const int frameWidth = 16;
        //const int coverWidth = 64;
        Vector2 origin;


        int[] gezogeneKarte = new int[12];
        int cardselected = 0;
        Vector2 selectedCard1 = new Vector2();
        Vector2 selectedCard2 = new Vector2();
        bool canClick = true;
        int numOfAttempts = 0;

        Karte[,] spielfeld = new Karte[HEIGHT, WIDTH];

        Random rnd = new Random();

        //Textdarstellung
        SpriteBatch spriteBatchText;
        SpriteFont font;
        Color fontColor;
        string text = "Attempts:\n";
        float textPosX;
        float textPosY;

        //timer
        float timer = 0;
        bool gameStarted = false;
        int numOfSolvedPairs = 0;

        //Buttons
        Button restart;
        float button1PosX;
        float button1PosY;
        float button1Width;
        float button1Height;

        Button showHighscore;

        
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
            shuffleCards();

            base.Initialize();
        }

        void shuffleCards()
        {
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
                            spielfeld[y, x].rotationAngle = 0;
                            done = true;
                        }

                    } while (!done);

                }
            }
        }

        void resetDeck()
        {
            for (int y = 0; y < 12; y++)
            {
                gezogeneKarte[y] = 0;
            }
            shuffleCards();
        }

        // This is a texture we can render.
        Texture2D myTexture;
        Texture2D simpleTexture;
        Texture2D restartButtonTexture;

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

            font = Content.Load<SpriteFont>("SpriteFont1");

            simpleTexture = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            Int32[] pixel = { 0xFFFFFF };
            simpleTexture.SetData<Int32>(pixel, 0, simpleTexture.Width * simpleTexture.Height);

            restartButtonTexture = Content.Load<Texture2D>("restartButton");
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
            //timer
            if (gameStarted)
            {
                timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            
            // Move the sprite around.
            UpdateSprite(gameTime);

            base.Update(gameTime);
        }

        void UpdateSprite(GameTime gameTime)
        {
            MouseState ms = Mouse.GetState();

            DisplaySettings displaySettings = getDisplayInfo();
            //detect click on cards
            for (int x = 0; x < WIDTH; x++)
            {
                for (int y = 0; y < HEIGHT; y++)
                {
                    Rectangle tempRec = new Rectangle((int)(displaySettings.coverWidth + displaySettings.intAbstandX) * x + displaySettings.intAbstandX, (int)(displaySettings.coverWidth + displaySettings.intAbstandY) * y + displaySettings.intAbstandY, displaySettings.coverWidth, displaySettings.coverWidth);

                    bool cardClick = tempRec.Intersects(new Rectangle(ms.X, ms.Y, 1, 1));
                    bool canClick = true;
                    if (canClick && cardClick && ms.LeftButton == ButtonState.Pressed && !spielfeld[y,x].show)
                    {
                        cardIsClicked(y, x);
                        canClick = false;
                    }
                    else if (ms.LeftButton == ButtonState.Released)
                    {
                        canClick = true;
                    }
                }
            }
            //detect click on buttons
            Rectangle resetRectangle = new Rectangle((int)button1PosX, (int)button1PosY, (int)button1Width, (int)button1Height);
            bool hitReset = resetRectangle.Intersects(new Rectangle(ms.X, ms.Y, 1, 1));

            if (hitReset)
            {
                resetDeck();
                timer = 0;
                numOfAttempts = 0;
                numOfSolvedPairs = 0;                    
            }

        }

        ///Hier wird die ausgewählte Karte aufgedeckt
        void cardIsClicked(int y, int x)
        {
            if (cardselected == 2)
            {
                cardselected = 0;
                numOfAttempts++;
                spielfeld[(int) selectedCard1.Y, (int) selectedCard1.X].show = false;
                spielfeld[(int) selectedCard2.Y, (int) selectedCard2.X].show = false;
            }

            if (cardselected == 0)
            {
                cardselected++;
                spielfeld[y, x].show = true;
                selectedCard1.X = x;
                selectedCard1.Y = y;
            }

            else if (cardselected == 1)
            {
                cardselected++;
                spielfeld[y, x].show = true;
                selectedCard2.X = x;
                selectedCard2.Y = y;
                if (spielfeld[(int) selectedCard1.Y, (int) selectedCard1.X].id == spielfeld[(int) selectedCard2.Y, (int) selectedCard2.X].id)
                {
                    cardselected = 0;
                    numOfAttempts++;
                    numOfSolvedPairs++;
                    spielfeld[(int) selectedCard1.Y, (int)selectedCard1.X].solved = true;
                    spielfeld[(int)selectedCard2.Y, (int)selectedCard2.X].solved = true;
                }                 
            }
            //for the timer to stop
            if (numOfSolvedPairs < 12)
            {
                gameStarted = true;
            }
            else
            {
                gameStarted = false;
            }
        }

        /// <summary>
        /// Dies wird aufgerufen, wenn das Spiel selbst zeichnen soll.
        /// </summary>
        /// <param name="gameTime">Bietet einen Schnappschuss der Timing-Werte.</param>
        protected override void Draw(GameTime gameTime)
        {
            DisplaySettings displaySettings = getDisplayInfo();

            GraphicsDevice.Clear(Color.DarkOrange);

            // TODO: Fügen Sie Ihren Zeichnungscode hier hinzu

            // Draw the sprite.
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

            // draw background rectangle
            int linePos = Convert.ToInt32(displaySettings.screenHeight * 0.8);
            spriteBatch.Draw(simpleTexture, new Rectangle(0, 0, displaySettings.screenHeight, displaySettings.lineWidth), null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
            spriteBatch.Draw(simpleTexture, new Rectangle(0, 0, displaySettings.lineWidth, displaySettings.screenWidth), null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
            spriteBatch.Draw(simpleTexture, new Rectangle(0, displaySettings.screenWidth - displaySettings.lineWidth, displaySettings.screenHeight, displaySettings.lineWidth), null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
            spriteBatch.Draw(simpleTexture, new Rectangle(displaySettings.screenHeight - displaySettings.lineWidth, 0, displaySettings.lineWidth, displaySettings.screenWidth), null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
            spriteBatch.Draw(simpleTexture, new Rectangle(linePos - displaySettings.lineWidth, 0, displaySettings.lineWidth, displaySettings.screenWidth), null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);

            // Draw Spielfeld
            for(int x = 0; x < WIDTH; x++)
            {
                for (int y = 0; y < HEIGHT; y++)
                {
                    if (spielfeld[y, x].show == true)
                    {
                        if (spielfeld[y, x].solved == true)
                        {
                            spielfeld[y, x].rotationAngle += (float)Math.PI / 30;
                            if (spielfeld[y, x].rotationAngle >= 2 * Math.PI)
                            {
                                spielfeld[y, x].solved = false;
                                spielfeld[y, x].rotationAngle = 0;
                            }
                        }
                        origin.X =  frameWidth / 2;
                        origin.Y =  frameWidth / 2;
                        spriteBatch.Draw(myTexture, new Rectangle((int)(displaySettings.coverWidth + displaySettings.intAbstandX) * x + displaySettings.intAbstandX + displaySettings.coverWidth / 2, (int)(displaySettings.coverWidth + displaySettings.intAbstandY) * y + displaySettings.intAbstandY + displaySettings.coverWidth / 2, displaySettings.coverWidth, displaySettings.coverWidth), new Rectangle((int)spielfeld[y, x].id * frameWidth, 0, frameWidth, frameWidth), Color.White, spielfeld[y, x].rotationAngle, origin, SpriteEffects.None, 0);
                        //spriteBatch.Draw(myTexture, new Rectangle((int)(displaySettings.coverWidth + displaySettings.intAbstandX) * x + displaySettings.intAbstandX, (int)(displaySettings.coverWidth + displaySettings.intAbstandY) * y + displaySettings.intAbstandY, displaySettings.coverWidth, displaySettings.coverWidth), new Rectangle((int)spielfeld[y, x].id * frameWidth, 0, frameWidth, frameWidth), Color.White);
                    }
                    else
                    {
                        spriteBatch.Draw(myTexture, new Rectangle((int)(displaySettings.coverWidth + displaySettings.intAbstandX) * x + displaySettings.intAbstandX, (int)(displaySettings.coverWidth + displaySettings.intAbstandY) * y + displaySettings.intAbstandY, displaySettings.coverWidth, displaySettings.coverWidth), new Rectangle(12 * frameWidth, 0, frameWidth, frameWidth), Color.White);
                    }
                }
            }
            //Draw Text
            textPosX = (float) 0.8 * displaySettings.screenHeight + displaySettings.lineWidth;
            textPosY = displaySettings.intAbstandY;
            Vector2 textPosition = new Vector2(textPosX, textPosY);
            spriteBatch.DrawString(font, text + numOfAttempts.ToString() + "\nTime:\n" + timer.ToString("0.00") + " s", textPosition, Color.Black);

            //Draw Buttons
            button1PosX = (float) (displaySettings.screenHeight * 0.8 + 2 * displaySettings.lineWidth);
            button1PosY = (float) (displaySettings.screenWidth * 0.5);
            button1Width = (float) (displaySettings.screenHeight * 0.2 - 5 * displaySettings.lineWidth);
            button1Height = (float) (button1Width / 2.8);

            spriteBatch.Draw(restartButtonTexture, new Rectangle((int)button1PosX, (int)button1PosY, (int)button1Width, (int)button1Height), new Rectangle(0,0,140,50), Color.White);
            
            spriteBatch.End();

            base.Draw(gameTime);
        }

        /// Hier kommt die Einstellung für die jeweiligen Displays
        DisplaySettings getDisplayInfo()
        {
            DisplaySettings displaySettings = new DisplaySettings();

                // Graphic infos
                displaySettings.screenWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                displaySettings.screenHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

                double totalWidth = 0.8 * displaySettings.screenHeight / 6;
                displaySettings.coverWidth = (int)Math.Floor(totalWidth / 16.0) * 16;

                //setting spielfeld display
                /// ACHTUNG!!! da wir im Querformat sind, muss hier widht und height vertauscht werden!!

                double abstandX = (0.8 * displaySettings.screenHeight - 6 * displaySettings.coverWidth) / 7;
                double abstandY = (displaySettings.screenWidth - 4 * displaySettings.coverWidth) / 5;

                displaySettings.intAbstandX = (int)Math.Round(abstandX);
                displaySettings.intAbstandY = (int)Math.Round(abstandY);

                displaySettings.lineWidth = (int)displaySettings.screenHeight / 200;

            return displaySettings;
        }
    }
}
