using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Assignment_1
{
    public partial class Form1 : Form
    {

        //Making several variables which will be used by functions. These include several arrays
        int atX = 10;
        int atY = 20;
        bool[,] bombs = new bool[21, 21];
        bool[,] score = new bool[21, 21];
        bool[,] rocks = new bool[21, 21];
        int scoreCount = 0;
        bool dead = true;
        int countDown;

        public Form1()
        {
            InitializeComponent();
        }

        //This function takes the x and y values given to it, determines which label in the grid the x and y refer
        //to and stores that data.
        private Label getLabel(int x, int y)
        {
            //Maths to figure out which label x and y point to
            int k = (y - 1) * 20 + x;
            //Storing that data to a string in 's'
            string s = "label" + k.ToString();

            //cycling through each element on the form
            foreach (Control c in panel1.Controls)
            {
                //Checking if it's a label
                if (c.GetType() == typeof(System.Windows.Forms.Label))
                {
                    //Checking to see if it's name matches the string stored in 's'
                    if (c.Name == s)
                    {
                        //Returning that label
                        return (Label)c;
                    }
                }
            }
            return null;
        }

        //This function is used to move the sprite and change the background colour of a square you move onto
        private void drawsprite(int x, int y)
        {
            //Find which label is being moved into by using the getLabel function
            Label lbl = getLabel(x, y);
            //Change it's colour and image
            lbl.BackColor = Color.White;
            lbl.Image = Properties.Resources.w;
        }

        //This function is used to remove the sprite image from a square being moved off of
        private void wipesprite(int x, int y)
        {
            //Find which label is being moved off of using getLabel
            Label lbl = getLabel(x, y);
            //Remove the image from it
            lbl.Image = null;
        }

        //This functionis used to generate rocks. The player is unable to go onto squares which contain rocks
        private void placeRocks(int target)
        {
            //Create a random number generator
            Random r = new Random();

            //Set up variables
            int x;
            int y;
            int k;

            //Make k equal to target. Target is the number of rocks to generate
            k = target;

            //Clear the current rocks list
            Array.Clear(rocks, 0, bombs.Length);

            //Start loop
            do
            {
                //Make x and y the next value
                x = r.Next(1, 21);
                y = r.Next(1, 21);
                //Check to see if that value is not true (not a rock)
                if (!rocks[x, y])
                {
                    //Make sure this isn't the start location
                    if (x != 10 || y != 20)
                    {
                        //Make this value true (make it a rock)
                        rocks[x, y] = true;
                        //Lower 'k' by 1
                        k--;
                        drawRocks(x, y);
                    }
                }
                //Loop until k is 0 or below
            } while (k > 0);
        }

        //This function is used to change the appearance of the rocks
        private void drawRocks(int x, int y)
        {
            //Find which label is being moved into by using the getLabel function
            Label lbl = getLabel(x, y);
            //Change it's colour and image
            lbl.BackColor = Color.Gray;
        }

        //This function generates the bombs
        private void placebombs(int target)
        {
            //Create a random number generator
            Random r = new Random();

            //Set up variables
            int x = 0;
            int y = 0;
            int k;

            //Make k equal to target. Target is the number of bombs to generate
            k = target;

            //Clear the current mines list
            Array.Clear(bombs, 0, bombs.Length);

            //Start loop
            do
            {
                //Make x and y the next value
                x = r.Next(1, 21);
                y = r.Next(1, 21);
                //Check to see if that value is not true (not a bomb)
                if (!bombs[x, y])
                {
                    //Make sure this isn't the start location
                    if (x != 10 || y != 20)
                    {

                        if (!rocks[x, y])
                        {
                            //Make sure this value true (make it a bomb)
                            bombs[x, y] = true;
                            //Lower 'k' by 1
                            k--;
                        }
                    }

                }
                //Loop until k is 0 or below
            } while (k > 0);
        }

        //This function is used to count the bombs in adjacent squares to the player
        private void countbombs(int x, int y)
        {
            //Set up variables
            int count = 0;
            int newx;
            int newy;

            //Set newx to 1 less than x
            newx = x - 1;
            //Check that newx is positive
            if (newx > -1)
            {
                //Check to see if the position pointed at is a bomb
                if (bombs[newx, y])
                {
                    //increase 'count' by 1
                    count++;
                }
            }

            //Set newx to 1 more than x
            newx = x + 1;
            //Check that x is less than 20
            if (newx < 21)
            {
                //Check to see if the position pointed at is a bomb
                if (bombs[newx, y])
                {
                    //increase 'count' by 1
                    count++;
                }
            }

            //Set newy to 1 less than y
            newy = y - 1;
            //Check that newy is positive
            if (newy > -1)
            {
                //Check to see if the position pointed at is a bomb
                if (bombs[x, newy])
                {
                    //increase 'count' by 1
                    count++;
                }
            }

            //Set newy to 1 more than x
            newy = y + 1;
            //Check that newy is less than 20
            if (newy < 21)
            {
                //Check to see if the position pointed at is a bomb
                if (bombs[x, newy])
                {
                    //increase 'count' by 1
                    count++;
                }
            }

            //Checking whether there is only 1 bomb next to the player
            if (count == 1)
            {
                //Write the number of bombs adjacent to the player into label401
                label401.Text = count.ToString() + " bomb adjacent to you.";
            }
            else
            {
                //Write the number of bombs adjacent to the player into label401
                label401.Text = count.ToString() + " bombs adjacent to you.";
            }


        }

        //This function is used to show the player where the bombs were after they lose
        private void showbombs()
        {
            //This whole block of code is used to loop through every label and change their background colour
            //The colour it changes to depends on whether they were a bomb or not.
            Label lbl;
            for (int y = 1; y < 21; y++)
            {
                for (int x = 1; x < 21; x++)
                {
                    lbl = getLabel(x, y);
                    if (bombs[x, y])
                    {
                        lbl.BackColor = Color.Yellow;
                    }
                    else
                    {
                        lbl.BackColor = Color.DarkGray;
                    }
                }

            }
        }

        private void endGame()
        {
            //Making the background of the form red, disabling buttons and making 'dead' true
            this.BackColor = Color.Red;
            btnLeft.Enabled = false;
            btnRight.Enabled = false;
            btnUp.Enabled = false;
            btnDown.Enabled = false;
            dead = true;
            timer1.Enabled = false;
            //Running showbombs to reveal the bombs
            showbombs();
        }

        //This function is used to check whether the square the player is moving onto is a bomb
        private void chkbomb(int x, int y)
        {
            //Checking to see if the square is a bomb
            if (bombs[x, y])
            {
                //Run endGame
                endGame();
                MessageBox.Show("Your score was " + scoreCount + ".", "You hit a mine!");
            }
            //if the square was not a bomb
            else
            {
                //Run countBombs using the current x and y values
                countbombs(x, y);
            }
        }

        private void chkRock(int x, int y)
        {

        }

        //This function is used to keep count of the score. It's called every time the player moves
        private void chkScore(int x, int y)
        {
            //Checking the square the player's moving into isn't a bomb
            if (!bombs[x, y])
            {
                //Making sure the square the player's moving onto is false (hasn't been landed on yet)
                if (!score[x, y])
                {
                    //Make this value true (it's been landed on) and add 1 to scoreCount
                    score[x, y] = true;
                    scoreCount++;
                    //Writing the current score into a label. 1 is taken off it to account for the free starting square.
                    lblScore.Text = "Your score is " + (scoreCount - 1).ToString();
                }
            }
        }

        //This is used to reset the score
        private void resetScore()
        {
            //Making scoreCount equal 0 and clearing the score array
            scoreCount = 0;
            Array.Clear(score, 0, score.Length);
        }

        //Used to make the game look as it does before the player dies
        private void resetColour()
        {
            //Looping through all the labels changing them back to default colours
            Label lbl;
            for (int y = 1; y < 21; y++)
            {
                for (int x = 1; x < 21; x++)
                {
                    lbl = getLabel(x, y);
                    lbl.BackColor = Color.SkyBlue;
                }
            }
            //Changing the form background colour
            this.BackColor = Form1.DefaultBackColor;
        }

        //This function mvoes the player up one square
        private void moveUp()
        {
            //Make sure atY is above 1
            if (atY > 1)
            {
                //Making sure the next square isn't a rock
                if (!rocks[atX, atY - 1])
                {
                    //Delete sprite from current location
                    wipesprite(atX, atY);
                    //Move up one row
                    atY--;
                    //Draw sprite at new location
                    drawsprite(atX, atY);
                    //Run the chkScore funtion
                    chkScore(atX, atY);
                }

            }
        }

        //This function moves the player down one square
        private void moveDown()
        {
            //Make sure atY is below 20
            if (atY < 20)
            {
                //Making sure the next square isn't a rock
                if (!rocks[atX, atY + 1])
                {
                    //Delete sprite from current location
                    wipesprite(atX, atY);
                    //Move down one row
                    atY++;
                    //Draw sprite at new location
                    drawsprite(atX, atY);
                    //Run the chkScore funtion
                    chkScore(atX, atY);
                }
            }
        }

        //This function moves the player left one square
        private void moveLeft()
        {
            //Make sure atX is above 1
            if (atX > 1)
            {
                //Making sure the next square isn't a rock
                if (!rocks[atX - 1, atY])
                {
                    //Delete sprite from current location
                    wipesprite(atX, atY);
                    //Move left one row
                    atX--;
                    //Draw sprite at new location
                    drawsprite(atX, atY);
                    //Run the chkScore funtion
                    chkScore(atX, atY);
                }
            }
        }

        //This function moves the player right one square
        private void moveRight()
        {
            //Make sure atX is below 20
            if (atX < 20)
            {
                //Making sure the next square isn't a rock
                if (!rocks[atX + 1, atY])
                {
                    //Delete sprite from current location
                    wipesprite(atX, atY);
                    //Move right one row
                    atX++;
                    //Draw sprite at new location
                    drawsprite(atX, atY);
                    //Run the chkScore funtion
                    chkScore(atX, atY);
                }
            }
        }

        //This function is used to make the player move when an arrow key is pressed.
        //This allows the player to either click or use the keyboard to play
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            //Running a switch-case on the key which was pressed
            switch (keyData)
            {
                //If up was pressed
                case Keys.Up:
                    //If dead is false
                    if (dead == false)
                    {
                        //Call moveUp
                        moveUp();
                        //Check for bombs
                        chkbomb(atX, atY);
                    }
                    break;

                //If down was pressed
                case Keys.Down:
                    //If dead is false
                    if (dead == false)
                    {
                        //Call moveDown
                        moveDown();
                        //Check for bombs
                        chkbomb(atX, atY);
                    }
                    break;

                //If left was pressed
                case Keys.Left:
                    //If dead is false
                    if (dead == false)
                    {
                        //Call moveLeft
                        moveLeft();
                        //Check for bombs
                        chkbomb(atX, atY);
                    }
                    break;

                //If right was pressed
                case Keys.Right:
                    //If dead is false
                    if (dead == false)
                    {
                        //Call moveRight
                        moveRight();
                        //Check for bombs
                        chkbomb(atX, atY);
                    }
                    break;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        //Button to move up
        private void btnUp_Click(object sender, EventArgs e)
        {
            //Call moveUp
            moveUp();
            //Check for bombs
            chkbomb(atX, atY);
        }

        //Button to move down
        private void btnDown_Click(object sender, EventArgs e)
        {
            //Call moveDown
            moveDown();
            //Check for bombs
            chkbomb(atX, atY);
        }

        //Button to move left
        private void btnLeft_Click(object sender, EventArgs e)
        {
            //Call moveLeft
            moveLeft();
            //Check for bombs
            chkbomb(atX, atY);
        }

        //Button to move right
        private void btnRight_Click(object sender, EventArgs e)
        {
            //Call moveRight
            moveRight();
            //Check for bombs
            chkbomb(atX, atY);
        }

        //Countdown timer which ends the game when it hit 0
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (countDown > 0)
            {
                //Lowering the countdown
                countDown--;
                //Writing the remaining time into a label
                lblCountDown.Text = countDown.ToString() + " Seconds left";
            }
            else
            {
                endGame();
                MessageBox.Show("Your score was " + scoreCount + ".", "Time's up!");

            }
        }

        //Button which starts the game
        private void btnStart_Click(object sender, EventArgs e)
        {
            //Declaring variables used to store values later on
            int[] math = new int[50];
            int n;
            int bombs;
            int rocks;
            int timer;

            //These lines are used to check whether the values written in the textboxes are integers
            //The booleans will be returned true if it's able to parse them successfully
            bool isNumeric = int.TryParse(txtBombCount.Text, out n);
            bool isNumeric2 = int.TryParse(txtRockCount.Text, out n);
            bool isNumeric3 = int.TryParse(txtTimer.Text, out n);

            //Checking that the isNumeric values are true
            if (isNumeric && isNumeric2 && isNumeric3)
            {
                //Storing the textbox values as the variables declared earlier
                bombs = int.Parse(txtBombCount.Text);
                rocks = int.Parse(txtRockCount.Text);
                timer = int.Parse(txtTimer.Text);

                //Making sure the value entered into the box isn't over the max number of bombs
                if (bombs < (400 - rocks))
                {
                    //Resetting several variables back to their default values to restart the game
                    wipesprite(atX, atY);
                    atX = 10;
                    atY = 20;
                    scoreCount = 0;
                    this.KeyPreview = true;
                    dead = false;

                    //Reset the colours of the labels and background
                    resetScore();
                    resetColour();

                    //Place the sprite at its startup location
                    drawsprite(atX, atY);

                    //Enable movement buttons 
                    btnLeft.Enabled = true;
                    btnRight.Enabled = true;
                    btnUp.Enabled = true;
                    btnDown.Enabled = true;

                    //Resetting the countdown timer
                    timer1.Enabled = true;
                    //Starting value of countDown is based on value entered by the player into the textbox
                    countDown = timer;
                    lblCountDown.Text = countDown.ToString() + " Seconds left";

                    //Place and check for bombs then call chkScore the reset the text in the score label
                    placeRocks(rocks);
                    placebombs(bombs);
                    chkbomb(atX, atY);
                    chkScore(atX, atY);
                }
            }
        }
    }
}