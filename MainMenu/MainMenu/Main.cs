using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Main;
namespace MainMenu
{

    public class Main : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont font1;
        UserInterface UI;
        Dictionary<int, int[]> openDoors = new Dictionary<int, int[]>();
        Dictionary<int, int[]> allDoors = new Dictionary<int, int[]>();
        Trivia t = new Trivia();
        TriviaReadandWrite trw;
        HighScore sc = new HighScore();
        List<HighScore> highScoreObject;
        Cave c = new Cave();
        Map m = new Map();
        Player p = new Player();
        //variables
        String reason = "";
        String name = "";
        public static String hint = "";
        String secret = "";
        String triviaHint = "";
        String triviaReason = "";
        int index = 0;//variable for pausing the game 3 seconds
        int maps = 1;
        int currentQuestion = 0;
        int oldroom = 0;
        static int numbQuestions = 1;
        int numbQuestionsCorrect = 0;
        String response = "";
        bool trivia = true;
        bool responded = false;
        bool b = true;
        bool win = false;
        //keys for checking input
        Keys[] keysToCheck = new Keys[] { 
        Keys.A, Keys.B, Keys.C, Keys.D, Keys.E,
        Keys.F, Keys.G, Keys.H, Keys.I, Keys.J,
        Keys.K, Keys.L, Keys.M, Keys.N, Keys.O,
        Keys.P, Keys.Q, Keys.R, Keys.S, Keys.T,
        Keys.U, Keys.V, Keys.W, Keys.X, Keys.Y,
        Keys.Z, Keys.Right, Keys.Left, 
        Keys.Back, Keys.Space };

        Keys[] triviaResponse = new Keys[] {
        Keys.A,Keys.B,Keys.C,Keys.D,
        Keys.Space};


        //keyboard states for name input
        KeyboardState currentKeyboardState;
        KeyboardState lastKeyboardState;

        enum GameState
        {
            MainMenu,
            Select,
            HighScores,
            Playing,
            Trivia,
            Dead,
            Win
        }
        GameState currentGameState = GameState.MainMenu;
        //buttons
        cButton btnPlay;
        cButton start;
        cButton btnScores;
        cButton exit;
        cButton menu;
        cButton buyArrows;
        cButton getHint;
        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            allDoors = m.LoadDoors();
            sc.readFile();
            highScoreObject = sc.HighScores();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            IsMouseVisible = true;
            font1 = Content.Load<SpriteFont>("Basic");
            UI = new UserInterface(spriteBatch, graphics, Content.Load<Texture2D>("right"),
                Content.Load<Texture2D>("left"), Content.Load<Texture2D>("platform"),
                Content.Load<Texture2D>("right"), Content.Load<Texture2D>("menubar"));

            //buttons
            graphics.ApplyChanges();
            btnPlay = new cButton(Content.Load<Texture2D>("game"), graphics.GraphicsDevice);
            start = new cButton(Content.Load<Texture2D>("game"), graphics.GraphicsDevice);
            btnScores = new cButton(Content.Load<Texture2D>("scores"), graphics.GraphicsDevice);
            exit = new cButton(Content.Load<Texture2D>("exit"), graphics.GraphicsDevice);
            menu = new cButton(Content.Load<Texture2D>("menu"), graphics.GraphicsDevice);
            getHint = new cButton(Content.Load<Texture2D>("hint"), graphics.GraphicsDevice);
            buyArrows = new cButton(Content.Load<Texture2D>("arrow"), graphics.GraphicsDevice);
            btnPlay.setPosition(new Vector2(350, 300));
            btnScores.setPosition(new Vector2(350, 330));
            exit.setPosition(new Vector2(350, 390));
            menu.setPosition(new Vector2(350, 360));
            start.setPosition(new Vector2(350, 350));
            getHint.setPosition(new Vector2(GraphicsDevice.Viewport.Width * 7 / 8, 0));
            buyArrows.setPosition(new Vector2(GraphicsDevice.Viewport.Width *7 / 8, 20));
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            //if (p.Gold < 0)
            //{
            //    currentGameState = GameState.Dead;
            //}
            MouseState mouse = Mouse.GetState();
            switch (currentGameState)
            {
                case GameState.MainMenu:
                    if (btnPlay.isClicked == true) currentGameState = GameState.Select;
                    if (btnScores.isClicked == true) currentGameState = GameState.HighScores;
                    if (exit.isClicked == true) this.Exit();
                    btnPlay.Update(mouse);
                    btnScores.Update(mouse);
                    exit.Update(mouse);
                    break;
                case GameState.Playing:
                    UI.UpdateUI();
                    getHint.Update(mouse);
                    buyArrows.Update(mouse);
                    currentQuestion = 0;
                    response = "";
                    reason = "";
                    responded = false;
                    b = true;//for high scores
                    if (getHint.isClicked == true)
                    {
                        numbQuestions = 3;
                        reason = "Hint";
                        currentGameState = GameState.Trivia;
                        getHint.isClicked = false;
			            p.Turns++;
                    }
                    if (buyArrows.isClicked == true)
                    {
                        numbQuestions = 3;
                        reason = "Arrows";
                        currentGameState = GameState.Trivia;
                        buyArrows.isClicked = false;
			            p.Turns++;
                    }
                    if (m.player == m.wumpus)
                    {
                        reason = "Wumpus!";
                        numbQuestions = 5;
                        currentGameState = GameState.Trivia;
                    }
                    if (m.player == m.pit1 || m.player == m.pit2)
                    {
                        reason = "Pit";
                        numbQuestions = 3;
                        currentGameState = GameState.Trivia;
                    }
                    if (m.player == m.sbat1 || m.player == m.sbat1)
                    {
                        triviaHint = "Bat Attack!";
                        index++;
                        if (index > 180)
                        {
                           
                            m.player = m.Rooms[25];
                            m.sbat1 = m.Rooms[22];
                            m.sbat2 = m.Rooms[20];
                            index = 0;
                        }
                    }

                    int newroom = m.player;
                    if (oldroom != newroom)
                    {
                        UpdateHint();
                        trw = t.getQuestion();
                        triviaHint = trw.answerHint;
                        p.Turns++;
                        p.Gold++;
                    }
                    oldroom = newroom;
                    break;
                case GameState.HighScores:
                    exit.Update(mouse);
                    menu.Update(mouse);
                    if (menu.isClicked == true)
                    {
                        currentGameState = GameState.MainMenu;
                        if (win) sc.writeFile(name, maps, p.getScore(), p.Turns, p.Gold, p.Arrows);
                    }
                    if (exit.isClicked == true)
                    {
                        if (win) sc.writeFile(name, maps, p.getScore(), p.Turns,p.Gold,p.Arrows); 
                        this.Exit();
                    }
                    if (b)
                    {
                        if (win)
                        {
                            sc.addHighScore(name, maps, p.getScore());
                        }
                        highScoreObject = sc.HighScores();
                        b = false;
                    }
                    break;
                case GameState.Trivia:
                    //start.Update(mouse);
                    currentKeyboardState = Keyboard.GetState();
                    foreach (Keys key in triviaResponse)
                    {
                        if (CheckKey(key))
                        {
                            KeysToCheck(key);
                            break;
                        }
                    }
                    lastKeyboardState = currentKeyboardState;
                    break;
                case GameState.Select:
                    if (start.isClicked == true)
                    {
                        c.ReadMap(maps);
                        openDoors = c.GetRooms();
                        UI.getOpenDoors(openDoors);
                        currentGameState = GameState.Playing;
                    }
                    start.Update(mouse);
                    currentKeyboardState = Keyboard.GetState();
                    foreach (Keys key in keysToCheck)
                    {
                        if (CheckKey(key))
                        {
                            AddKeyToText(key);
                            break;
                        }
                    }
                    lastKeyboardState = currentKeyboardState;
                    break;
                case GameState.Dead:
                    win = false;
                    break;
                case GameState.Win:
                    win = true;
                    break;

            }
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Pink);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            //spriteBatch.Draw(Content.Load<Texture2D>("ocean"), new Rectangle(0, 0, GraphicsDevice.Viewport.Width
            //, GraphicsDevice.Viewport.Width), Color.White);
            switch (currentGameState)
            {
                case GameState.MainMenu:

                    spriteBatch.DrawString(font1, "Hunt the Wumpus!", new Vector2(300, 10), Color.Red);
                    btnPlay.Draw(spriteBatch);
                    btnScores.Draw(spriteBatch);
                    exit.Draw(spriteBatch);
                    break;
                case GameState.Playing:
                    UI.Draw();
                    spriteBatch.DrawString(font1, m.player.ToString(), new Vector2(330,
                        GraphicsDevice.Viewport.Height - 60), Color.Red);
                    spriteBatch.DrawString(font1, hint, new Vector2(GraphicsDevice.Viewport.Width - 222, GraphicsDevice.Viewport.Height - 73),
                        Color.Black);
                    spriteBatch.DrawString(font1, p.Gold.ToString(), new Vector2(200, GraphicsDevice.Viewport.Height - 73), Color.Black);
                    spriteBatch.DrawString(font1, p.Arrows.ToString(), new Vector2(200, GraphicsDevice.Viewport.Height - 40), Color.Black);
                    spriteBatch.DrawString(font1, p.getScore().ToString(), new Vector2(480, 420), Color.Black);
                    spriteBatch.DrawString(font1, "Turn: " + p.Turns, new Vector2(0, 0), Color.Black);
                    spriteBatch.DrawString(font1, triviaHint, new Vector2(0, 30), Color.Black);
                    spriteBatch.DrawString(font1, secret, new Vector2(250, 250), Color.Black);
                    buyArrows.Draw(spriteBatch);
                    getHint.Draw(spriteBatch);
                    break;
                case GameState.HighScores:
                    for(int x = 0; x < 10; x++)
                    {
                        HighScore highScore = highScoreObject[x];
                        spriteBatch.DrawString(font1, highScore.ToString(), new Vector2(250,30*x + 30), Color.Black);
                    }
                    exit.Draw(spriteBatch);
                    menu.Draw(spriteBatch);
                    break;
                case GameState.Trivia:
                    while (trivia)
                    {
                        trw = t.getQuestion();
                        trivia = false;
                    }
                    spriteBatch.DrawString(font1, reason, new Vector2(0, 0), Color.Black);
                    spriteBatch.DrawString(font1, triviaReason, new Vector2(0, 0), Color.Blue);
                    spriteBatch.DrawString(font1, trw.Question, new Vector2(120, 0), Color.Gray);
                    spriteBatch.DrawString(font1, trw.Choice1, new Vector2(250, 50), Color.Gray);
                    spriteBatch.DrawString(font1, trw.Choice2, new Vector2(250, 100), Color.Gray);
                    spriteBatch.DrawString(font1, trw.Choice3, new Vector2(250, 150), Color.Gray);
                    spriteBatch.DrawString(font1, trw.Choice4, new Vector2(250, 200), Color.Gray);
                    spriteBatch.DrawString(font1, response, new Vector2(250, 250), Color.Green);
                    break;
                case GameState.Select:
                    spriteBatch.DrawString(font1, "Enter Name: " + name, new Vector2(300, 10), Color.Red);
                    spriteBatch.DrawString(font1, "Map: " + maps, new Vector2(300, 50), Color.Red);
                    start.Draw(spriteBatch);
                    break;
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void KeysToCheck(Keys key)
        {
            switch (key)
            {
                case Keys.A:
                    while (!responded)
                    {
                        if (trw.Answer == 1)
                        {
                            response = "Correct!";
                            numbQuestionsCorrect++;
                            currentQuestion++;
                            responded = true;
                        }
                        else
                        {
                            response = "Incorrect";
                            currentQuestion++;
                            responded = true;
                        }
                    }
                    break;
                case Keys.B:
                    while (!responded)
                    {
                        if (trw.Answer == 2)
                        {
                            response = "Correct";
                            numbQuestionsCorrect++;
                            currentQuestion++;
                            responded = true;
                        }
                        else
                        {
                            response = "Incorrect";
                            currentQuestion++;
                            responded = true;
                        }
                    }
                    break;
                case Keys.C:
                    while (!responded)
                    {
                        if (trw.Answer == 3)
                        {
                            response = "Correct";
                            numbQuestionsCorrect++;
                            currentQuestion++;
                            responded = true;
                        }
                        else
                        {
                            response = "Incorrect";
                            currentQuestion++;
                            responded = true;
                        }
                    }
                    break;
                case Keys.D:
                    while (!responded)
                    {
                        if (trw.Answer == 4)
                        {
                            response = "Correct";
                            numbQuestionsCorrect++;
                            currentQuestion++;
                            responded = true;
                        }
                        else
                        {
                            response = "Incorrect";
                            currentQuestion++;
                            responded = true;
                        }
                    }
                    break;
                case Keys.Space:
                    if (!response.Equals("") && currentQuestion < numbQuestions)
                    {
                        trivia = true;
                        response = "";
                        responded = false;
                    }
                    else if (currentQuestion == numbQuestions)
                    {
                        switch (reason)
                        {
                            case "Wumpus!":
                                if (numbQuestionsCorrect < 3) currentGameState = GameState.Dead;
                                else
                                {
                                    int[] possibleRooms = allDoors[m.player];
                                    for (int x = 0; x < possibleRooms.Length; x++)
                                    {
                                        if (possibleRooms[x] != m.pit1 && possibleRooms[x] != m.pit2 && possibleRooms[x] != m.sbat1 
                                            && possibleRooms[x] != m.sbat2 && possibleRooms[x] != m.player)
                                        {
                                            m.wumpus = possibleRooms[x];
                                            break;
                                        }
                                    }
                                }
                                break;
                            case "Arrows":
                                if (numbQuestionsCorrect > 1)
                                {
                                    p.Arrows++;
                                }
                                currentGameState = GameState.Playing;
                                break;
                            case "Hint":
                                if (numbQuestionsCorrect > 1) 
                                {
                                    hint = m.getsecret();
                                }
                                currentGameState = GameState.Playing;
                                break;
                            case "Pit":
                                if (numbQuestionsCorrect < 2) currentGameState = GameState.Dead;
                                else
                                {
                                    m.player = m.startingRoom;
                                }
                                break;
                        }
                        reason = "";
                    }

                    break;
            }
        }
        private void AddKeyToText(Keys key)
        {
            string newChar = "";
            if (name.Length >= 20 && key != Keys.Back)
                return;
            switch (key)
            {
                case Keys.A:
                    newChar += "a";
                    break;
                case Keys.B:
                    newChar += "b";
                    break;
                case Keys.C:
                    newChar += "c";
                    break;
                case Keys.D:
                    newChar += "d";
                    break;
                case Keys.E:
                    newChar += "e";
                    break;
                case Keys.F:
                    newChar += "f";
                    break;
                case Keys.G:
                    newChar += "g";
                    break;
                case Keys.H:
                    newChar += "h";
                    break;
                case Keys.I:
                    newChar += "i";
                    break;
                case Keys.J:
                    newChar += "j";
                    break;
                case Keys.K:
                    newChar += "k";
                    break;
                case Keys.L:
                    newChar += "l";
                    break;
                case Keys.M:
                    newChar += "m";
                    break;
                case Keys.N:
                    newChar += "n";
                    break;
                case Keys.O:
                    newChar += "o";
                    break;
                case Keys.P:
                    newChar += "p";
                    break;
                case Keys.Q:
                    newChar += "q";
                    break;
                case Keys.R:
                    newChar += "r";
                    break;
                case Keys.S:
                    newChar += "s";
                    break;
                case Keys.T:
                    newChar += "t";
                    break;
                case Keys.U:
                    newChar += "u";
                    break;
                case Keys.V:
                    newChar += "v";
                    break;
                case Keys.W:
                    newChar += "w";
                    break;
                case Keys.X:
                    newChar += "x";
                    break;
                case Keys.Y:
                    newChar += "y";
                    break;
                case Keys.Z:
                    newChar += "z";
                    break;
                case Keys.Right:
                    if (maps > 0 && maps < 5) maps++;
                    break;
                case Keys.Left:
                    if (maps > 1 && maps < 6) maps--;
                    break;
                case Keys.Space:
                    newChar += " ";
                    break;
                case Keys.Back:
                    if (name.Length != 0)
                        name = name.Remove(name.Length - 1);
                    return;
            }
            name += newChar;
        }
        public void UpdateHint()
        {
            int[] rooms = allDoors[m.player];
            if (rooms.Contains(m.wumpus)) hint += "I smell a Wumpus!" + System.Environment.NewLine;
            if (rooms.Contains(m.sbat1) || rooms.Contains(m.sbat2)) hint += "Bats Nearby" + System.Environment.NewLine;
            if (rooms.Contains(m.pit1) || rooms.Contains(m.pit2)) hint += "I feel a draft" + System.Environment.NewLine;
        }

        private bool CheckKey(Keys theKey)
        {
            return lastKeyboardState.IsKeyDown(theKey) && currentKeyboardState.IsKeyUp(theKey);
        }
    }
}


