using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using MainMenu;
namespace Main
{
    class UserInterface
    {
        Map m = new Map();
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D playerRight;
        Texture2D playerLeft;
        Texture2D platform;
        Texture2D door;
        Texture2D infoBar;
        SpriteFont font;
        Rectangle infoRect;
        Rectangle playerRect;
        Rectangle door1;
        Rectangle door2;
        Rectangle door3;
        Rectangle groundRect;
        Rectangle sign1;
        Rectangle sign2;
        Rectangle sign3;
        Rectangle alertWumpus;
        Rectangle alertBat;
        Rectangle alertPit;
        Rectangle score;
        Rectangle time;
        Rectangle arrow;
        Rectangle gold;
        Rectangle currentRoom;

        Dictionary<int, int[]> openDoors = new Dictionary<int, int[]>();

        //get method for current room int currentRoom = ;
        int jumpCounter = 25;
        bool facingLeft = true;
        bool falling = false;
        bool jumping = false;
        int fallCounter = 0;
        int turbo = 1;
        float movement = 0;
        bool onGround = true;

        public void getOpenDoors(Dictionary<int, int[]> openDoor)
        {
            openDoors = openDoor;
        }
        public UserInterface(SpriteBatch b, GraphicsDeviceManager gm, Texture2D right, Texture2D left, Texture2D p, Texture2D d, Texture2D i)
        {
            spriteBatch = b;
            graphics = gm;
            playerRight = right;
            playerLeft = left;
            platform = p;
            door = d;
            infoBar = i;
            playerRect = new Rectangle(spriteBatch.GraphicsDevice.Viewport.Width / 2 - 15,
                spriteBatch.GraphicsDevice.Viewport.Height - 75,
                20, 40);
            groundRect = new Rectangle(0, spriteBatch.GraphicsDevice.Viewport.Height - 15, spriteBatch.GraphicsDevice.Viewport.Width, 15);
            infoRect = new Rectangle(0, graphics.GraphicsDevice.Viewport.Height - 100, graphics.GraphicsDevice.Viewport.Width, 100);
            groundRect = new Rectangle(0, infoRect.Y - 15, graphics.GraphicsDevice.Viewport.Width, 15);
            playerRect = new Rectangle(0, groundRect.Y - 40, 20, 40);
            door1 = new Rectangle(50, groundRect.Y - 50, 30, 50);
            door2 = new Rectangle(graphics.GraphicsDevice.Viewport.Width / 2 - door2.Width / 2, groundRect.Y - door1.Height, 30, 50);
            door3 = new Rectangle(graphics.GraphicsDevice.Viewport.Width - 50 - door3.Width, groundRect.Y - door1.Height, 30, 50);
        }

        public void UpdateUI()
        {

            //openDoors.Add(1, new int[]{0,2,3,30,0,0});
            int[] doors = openDoors[m.player];
            // TODO: Add your update logic here
            GamePadState gp1 = GamePad.GetState(PlayerIndex.One);
            //player moves left
            //if (gp1.ThumbSticks.Left.X >= -1 && gp1.ThumbSticks.Left.X < 0)
			if (Keyboard.GetState().IsKeyDown(Keys.A))
            { movement = gp1.ThumbSticks.Left.X; moveLeft(movement); }

            //player moves right
            else if (Keyboard.GetState().IsKeyDown(Keys.C))

            { movement = gp1.ThumbSticks.Left.X; moveRight(movement);  }

            //check for sprinting
            if (gp1.Triggers.Left > 0)sprint();
            //if not, then turbo goes back to 0
            else { turbo = 1; }

            if (playerRect.Intersects(door1) && gp1.IsButtonDown(Buttons.A))
            {
                m.player = doors[1];
            }
            else if (playerRect.Intersects(door2) && gp1.IsButtonDown(Buttons.A))
            {
                m.player = doors[2];
            }
            else if (playerRect.Intersects(door3) && gp1.IsButtonDown(Buttons.A))
            {
                m.player = doors[3];
            }
            
            //player boundaries on either side.
            if (playerRect.X <= 0)
            {
                playerRect.X = 0;
            }
            else if (playerRect.X >= graphics.GraphicsDevice.Viewport.Width - 20)
            {
                playerRect.X = graphics.GraphicsDevice.Viewport.Width - playerRect.Width;
            }
            
            else
            {
                falling = true; 
            }
        }
        public void moveLeft(float moveL)
        {

            playerRect.X -= (int)Math.Pow(moveL - .75, 2) + turbo; facingLeft = true;
        }
        public void moveRight(float moveR)
        {

            playerRect.X += (int)Math.Pow(moveR + .75, 2) + turbo; facingLeft = false;
        }
        public void sprint()
        {
            GamePadState gp1 = GamePad.GetState(PlayerIndex.One);
            turbo = (int)Math.Pow(gp1.Triggers.Left + 1, 2);
        }
        public void Draw()
        {
            spriteBatch.Draw(platform, groundRect, Color.White);
            spriteBatch.Draw(infoBar, infoRect, Color.White);
            spriteBatch.Draw(door, door1, Color.White);
            spriteBatch.Draw(door, door2, Color.White);
            spriteBatch.Draw(door, door3, Color.White);



            if (facingLeft == true)
            {
                spriteBatch.Draw(playerLeft, playerRect, Color.White);
            }
            else if (facingLeft == false)
            {
                spriteBatch.Draw(playerRight, playerRect, Color.White);
            }
        }
    }

}
