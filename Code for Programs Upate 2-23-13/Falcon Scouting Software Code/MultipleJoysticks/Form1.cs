
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SlimDX;
using SlimDX.DirectInput;
using System.Globalization;
using System.IO;

namespace FalconScoutingSoftware
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            GetSticks();
            Sticks = GetSticks();
            stick1 = stick;
            timer1.Enabled = true;
        }

        DirectInput Input = new DirectInput();
        SlimDX.DirectInput.Joystick stick;
        Joystick[] Sticks;
        Joystick stick1;

        //Arrays that hold the values for the made and attempt numbers of the frisbee scoring
        // 1 point frisbees
        int[] displayLowFrisbeesMade = { 0, 0, 0, 0, 0, 0 };
        int[] lowFrisbeesMade = { 0, 0, 0, 0, 0, 0 };
        int[] displayLowFrisbeesAtt = { 0, 0, 0, 0, 0, 0 };
        int[] lowFrisbeesAtt = { 0, 0, 0, 0, 0, 0 };
        int[] autoDisplayLowFrisbeesMade = { 0, 0, 0, 0, 0, 0 };
        int[] autoLowFrisbeesMade = { 0, 0, 0, 0, 0, 0 };
        int[] autoDisplayLowFrisbeesAtt = { 0, 0, 0, 0, 0, 0 };
        int[] autoLowFrisbeesAtt = { 0, 0, 0, 0, 0, 0 };

        // 2 point frisbees
        int[] displayMidFrisbeesMade = { 0, 0, 0, 0, 0, 0 };
        int[] midFrisbeesMade = { 0, 0, 0, 0, 0, 0 };
        int[] displayMidFrisbeesAtt = { 0, 0, 0, 0, 0, 0 };
        int[] midFrisbeesAtt = { 0, 0, 0, 0, 0, 0 };
        int[] autoDisplayMidFrisbeesMade = { 0, 0, 0, 0, 0, 0 };
        int[] autoMidFrisbeesMade = { 0, 0, 0, 0, 0, 0 };
        int[] autoDisplayMidFrisbeesAtt = { 0, 0, 0, 0, 0, 0 };
        int[] autoMidFrisbeesAtt = { 0, 0, 0, 0, 0, 0 };

        // 3 point frisbees
        int[] displayHighFrisbeesMade = { 0, 0, 0, 0, 0, 0 };
        int[] highFrisbeesMade = { 0, 0, 0, 0, 0, 0 };
        int[] displayHighFrisbeesAtt = { 0, 0, 0, 0, 0, 0 };
        int[] highFrisbeesAtt = { 0, 0, 0, 0, 0, 0 };
        int[] autoDisplayHighFrisbeesMade = { 0, 0, 0, 0, 0, 0 };
        int[] autoHighFrisbeesMade = { 0, 0, 0, 0, 0, 0 };
        int[] autoDisplayHighFrisbeesAtt = { 0, 0, 0, 0, 0, 0 };
        int[] autoHighFrisbeesAtt = { 0, 0, 0, 0, 0, 0 };

        // Pyramid Frisbees
        int[] displayPyramidFrisbeesMade = { 0, 0, 0, 0, 0, 0 };
        int[] pyramidFrisbeesMade = { 0, 0, 0, 0, 0, 0 };
        int[] displayPyramidFrisbeesAtt = { 0, 0, 0, 0, 0, 0 };
        int[] pyramidFrisbeesAtt = { 0, 0, 0, 0, 0, 0 };

        // Robot Climbing
        int[] climb = { 0, 0, 0, 0, 0, 0 };
        int[] robotClimb = { 0, 0, 0, 0, 0, 0 };

        // Total Points
        int[] teleOpTotalPoints = { 0, 0, 0, 0, 0, 0 };
        int[] autoTotalPoints = { 0, 0, 0, 0, 0, 0 };

        // Defense Ratings
        int[] defenseRating = { 0, 0, 0, 0, 0, 0 };
        int[] displayDefenseRating = { 0, 0, 0, 0, 0, 0 };

        // Declaration of auto-filled team numbers.
        static int autoTeams = 0;
        int[] AutoTeamNo1;
        int[] AutoTeamNo2;
        int[] AutoTeamNo3;
        int[] AutoTeamNo4;
        int[] AutoTeamNo5;
        int[] AutoTeamNo6;

        //Thumstick variables. These are not used but this can tell the value of the joysticks.
        int yValue = 0;
        int xValue = 0;
        int zValue = 0;
        int rotationZValue = 0;

        //This variable fixes a glitch on the code. When a button is pressed in a gamepad it counts by 128. So to fix this we divide the number by 128 so it counts by one.
        int hundred = 128;

        //Keeps track of the match.
        static int match = 1;


        String fileName = "";
        String[] teamsNotePad;
        String x = ",";

        private void Form1_Load(object sender, EventArgs e)
        {
            Joystick[] joystick = GetSticks();
        }

        public Joystick[] GetSticks()
        {

            List<SlimDX.DirectInput.Joystick> sticks = new List<SlimDX.DirectInput.Joystick>(); // Creates the list of joysticks connected to the computer via USB.
            foreach (DeviceInstance device in Input.GetDevices(DeviceClass.GameController, DeviceEnumerationFlags.AttachedOnly))
            {
                // Creates a joystick for each game device in USB Ports
                try
                {
                    stick = new SlimDX.DirectInput.Joystick(Input, device.InstanceGuid);
                    stick.Acquire();

                    // Gets the joysticks properties and sets the range for them.
                    foreach (DeviceObjectInstance deviceObject in stick.GetObjects())
                    {
                        if ((deviceObject.ObjectType & ObjectDeviceType.Axis) != 0)
                            stick.GetObjectPropertiesById((int)deviceObject.ObjectType).SetRange(-100, 100);
                    }

                    // Adds how ever many joysticks are connected to the computer into the sticks list.
                    sticks.Add(stick);
                }
                catch (DirectInputException)
                {
                }
            }
            return sticks.ToArray();
        }

        //Creates the StickHandlingLogic Method which takes all the joysticks in the sticks List and puts them into a timer.
        public void timer1_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < Sticks.Length; i++)
            {
                StickHandlingLogic(Sticks[i], i);
            }
        }

        public void AutoTeamNumbers()
        {
           
        }
        //Defines each joystick.
        void StickHandlingLogic(Joystick stick, int id)
        {
            // Creates an object from the class JoystickState.
            JoystickState state = new JoystickState();

            state = stick.GetCurrentState(); //Gets the state of the joystick

            yValue = -state.Y;
            xValue = state.X;
            zValue = state.Z;
            rotationZValue = -state.RotationZ;

            bool[] buttons = state.GetButtons(); // Stores the number of each button on the gamepad into the bool[] butons.

            String strText = "";
            String strText2 = "";
            String strText3 = "";
            String strText4 = "";
            String strText5 = "";
            String strText6 = "";

            //for loop that gives each gamepad a number starting from 0 and stores it into id.
            for (int x = 0; x < buttons.Length; x++)
            {

                //Code for the first gamepad.
                if (id == 0)
                {
                    // Pyramid Goals
                    lblTeleOpPyramidMade.Text = displayPyramidFrisbeesMade[0].ToString();
                    lblTeleOpPyramidAtt.Text = displayPyramidFrisbeesAtt[0].ToString();

                    // High Goals
                    lblTeleOpHighMade.Text = displayHighFrisbeesMade[0].ToString();
                    lblTeleOpHighAtt.Text = displayHighFrisbeesAtt[0].ToString();
                    lblAutoHighMade.Text = autoDisplayHighFrisbeesMade[0].ToString();
                    lblAutoHighAtt.Text = autoDisplayHighFrisbeesAtt[0].ToString();

                    // Mid Goals
                    lblTeleOpMidMade.Text = displayMidFrisbeesMade[0].ToString();
                    lblTeleOpMidAtt.Text = displayMidFrisbeesAtt[0].ToString();
                    lblAutoMidMade.Text = autoDisplayMidFrisbeesMade[0].ToString();
                    lblAutoMidAtt.Text = autoDisplayMidFrisbeesAtt[0].ToString();

                    // Low Goals
                    lblTeleOpLowMade.Text = displayLowFrisbeesMade[0].ToString();
                    lblTeleOpLowAtt.Text = displayLowFrisbeesAtt[0].ToString();
                    lblAutoLowMade.Text = autoDisplayLowFrisbeesMade[0].ToString();
                    lblAutoLowAtt.Text = autoDisplayLowFrisbeesAtt[0].ToString();

                    // Robot Climb
                    lblRobotClimb.Text = robotClimb[0].ToString();

                    lblTeleOpTotalPoints.Text = teleOpTotalPoints[0].ToString();
                    lblAutoTotalPoints.Text = autoTotalPoints[0].ToString();

                    //Defense Rating
                    lblDefense.Text = displayDefenseRating[0].ToString();

                    if (buttons[9] && !buttons[0] && !buttons[1] && !buttons[2] && !buttons[3] && !buttons[4] && !buttons[5] && !buttons[6] && !buttons[7] && !buttons[8] && !buttons[10] && !buttons[11] &&!buttons[12])
                    {
                        lblAuto.Visible = false; 
                        lblTeleOp.Visible = true;
                        lblAutoHighAtt.Visible = false;
                        lblTeleOpHighAtt.Visible = true;
                        lblAutoHighMade.Visible = false;
                        lblTeleOpHighMade.Visible = true;
                        lblAutoMidAtt.Visible = false;
                        lblTeleOpMidAtt.Visible = true;
                        lblAutoMidMade.Visible = false;
                        lblTeleOpMidMade.Visible = true;
                        lblAutoLowAtt.Visible = false;
                        lblTeleOpLowAtt.Visible = true;
                        lblAutoLowMade.Visible = false;
                        lblTeleOpLowMade.Visible = true;
                        lblTeleOpPyramidAtt.Visible = true;
                        lblTeleOpPyramidMade.Visible = true;
                        lblRobotClimb.Visible = true;
                    }

                    if (buttons[8] && !buttons[0] && !buttons[1] && !buttons[2] && !buttons[3] && !buttons[4] && !buttons[5] && !buttons[6] && !buttons[7] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                    {
                        lblAuto.Visible = true;
                        lblTeleOp.Visible = false;
                        lblAutoHighAtt.Visible = true;
                        lblTeleOpHighAtt.Visible = false;
                        lblAutoHighMade.Visible = true;
                        lblTeleOpHighMade.Visible = false;
                        lblAutoMidAtt.Visible = true;
                        lblTeleOpMidAtt.Visible = false;
                        lblAutoMidMade.Visible = true;
                        lblTeleOpMidMade.Visible = false;
                        lblAutoLowAtt.Visible = true;
                        lblTeleOpLowAtt.Visible = false;
                        lblAutoLowMade.Visible = true;
                        lblTeleOpLowMade.Visible = false;
                        lblTeleOpPyramidAtt.Visible = false;
                        lblTeleOpPyramidMade.Visible = false;
                        lblRobotClimb.Visible = false;
                    }



                    //The following code obtains and tells which button is each game pad is being pressed.
                    if (buttons[x])
                    {
                        strText += x.ToString("00 ", CultureInfo.CurrentCulture);
                        lbldisplayButtons1.Text = strText;
                    }

                    if (lblTeleOp.Visible == true)
                    {


                        int hold = 0;

                        if (buttons[10] && !buttons[0] && !buttons[1] && !buttons[2] && !buttons[3] && !buttons[4] && !buttons[5] && !buttons[6] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[11] && !buttons[12] )
                        {
                            hold = hold + 1;
                            if (hold == 1)
                            {
                                defenseRating[0]++;
                                displayDefenseRating[0] = defenseRating[0] / hundred;
                                if (displayDefenseRating[0] > 10)
                                {
                                    defenseRating[0] = 0;
                                    displayDefenseRating[0] = 0;
                                }
                            }
                          
                        }    

                        else if (buttons[11] && !buttons[0] && !buttons[1] && !buttons[2] && !buttons[3] && !buttons[4] && !buttons[5] && !buttons[6] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[12])
                        {
                            defenseRating[0]--;
                            displayDefenseRating[0] = defenseRating[0] / hundred;
                            if (displayDefenseRating[0] <= 0)
                            {
                                defenseRating[0] = 0;
                            }
                        }
                        
                        // This is the code that counts how many pyramid goals were made or attempted
                        if (buttons[5] && buttons[1] && !buttons[0] && !buttons[2] && !buttons[3] && !buttons[4] && !buttons[6] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            pyramidFrisbeesMade[0]--;
                            displayPyramidFrisbeesMade[0] = pyramidFrisbeesMade[0] / hundred;
                            if (pyramidFrisbeesMade[0] <= 0)
                            {
                                pyramidFrisbeesMade[0] = 0;
                            }
                        }

                        else if (buttons[5] && buttons[3] && !buttons[0] && !buttons[2] && !buttons[1] && !buttons[4] && !buttons[6] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            pyramidFrisbeesMade[0]++;
                            displayPyramidFrisbeesMade[0] = pyramidFrisbeesMade[0] / hundred;
                        }

                        else if (buttons[5] && buttons[0] && !buttons[1] && !buttons[2] && !buttons[3] && !buttons[4] && !buttons[6] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            pyramidFrisbeesAtt[0]--;
                            displayPyramidFrisbeesAtt[0] = pyramidFrisbeesAtt[0] / hundred;

                            if (pyramidFrisbeesAtt[0] <= 0)
                            {
                                pyramidFrisbeesAtt[0] = 0;
                            }
                        }

                        else if (buttons[5] && buttons[2] && !buttons[1] && !buttons[0] && !buttons[3] && !buttons[4] && !buttons[6] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            pyramidFrisbeesAtt[0]++;
                            displayPyramidFrisbeesAtt[0] = pyramidFrisbeesAtt[0] / hundred;
                        }

                        // This code counts how many High goals were made and attempted
                        if (buttons[4] && buttons[1] && !buttons[0] && !buttons[2] && !buttons[3] && !buttons[5] && !buttons[6] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            highFrisbeesMade[0]--;
                            displayHighFrisbeesMade[0] = highFrisbeesMade[0] / hundred;

                            if (highFrisbeesMade[0] <= 0)
                            {
                                highFrisbeesMade[0] = 0;
                            }
                        }
                        else if (buttons[4] && buttons[3] && !buttons[1] && !buttons[2] && !buttons[0] && !buttons[5] && !buttons[6] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            highFrisbeesMade[0]++;
                            displayHighFrisbeesMade[0] = highFrisbeesMade[0] / hundred;
                        }

                        else if (buttons[4] && buttons[0] && !buttons[1] && !buttons[2] && !buttons[3] && !buttons[5] && !buttons[6] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            highFrisbeesAtt[0]--;
                            displayHighFrisbeesAtt[0] = highFrisbeesAtt[0] / hundred;

                            if (highFrisbeesAtt[0] <= 0)
                            {
                                highFrisbeesAtt[0] = 0;
                            }
                        }

                        else if (buttons[4] && buttons[2] && !buttons[1] && !buttons[3] && !buttons[0] && !buttons[5] && !buttons[6] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            highFrisbeesAtt[0]++;
                            displayHighFrisbeesAtt[0] = highFrisbeesAtt[0] / hundred;
                        }

                        // This code counts how many Mid goals were made and attempted
                        if (buttons[7] && buttons[1] && !buttons[2] && !buttons[3] && !buttons[4] && !buttons[5] && !buttons[6] && !buttons[0] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            midFrisbeesMade[0]--;
                            displayMidFrisbeesMade[0] = midFrisbeesMade[0] / hundred;

                            if (midFrisbeesMade[0] <= 0)
                            {
                                midFrisbeesMade[0] = 0;
                            }
                        }

                        else if (buttons[7] && buttons[3] && !buttons[1] && !buttons[2] && !buttons[0] && !buttons[5] && !buttons[6] && !buttons[4] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            midFrisbeesMade[0]++;
                            displayMidFrisbeesMade[0] = midFrisbeesMade[0] / hundred;
                        }

                        else if (buttons[7] && buttons[0] && !buttons[1] && !buttons[3] && !buttons[2] && !buttons[5] && !buttons[6] && !buttons[4] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            midFrisbeesAtt[0]--;
                            displayMidFrisbeesAtt[0] = midFrisbeesAtt[0] / hundred;

                            if (midFrisbeesAtt[0] <= 0)
                            {
                                midFrisbeesAtt[0] = 0;
                            }
                        }

                        else if (buttons[7] && buttons[2] && !buttons[1] && !buttons[3] && !buttons[0] && !buttons[5] && !buttons[6] && !buttons[4] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            midFrisbeesAtt[0]++;
                            displayMidFrisbeesAtt[0] = midFrisbeesAtt[0] / hundred;
                        }

                        // This code counts how many low goals were made and attempted
                        if (buttons[6] && buttons[1] && !buttons[2] && !buttons[3] && !buttons[0] && !buttons[5] && !buttons[4] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            lowFrisbeesMade[0]--;
                            displayLowFrisbeesMade[0] = lowFrisbeesMade[0] / hundred;
                            if (lowFrisbeesMade[0] <= 0)
                            {
                                lowFrisbeesMade[0] = 0;
                            }
                        }

                        else if (buttons[6] && buttons[3] && !buttons[1] && !buttons[2] && !buttons[0] && !buttons[5] && !buttons[4] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            lowFrisbeesMade[0]++;
                            displayLowFrisbeesMade[0] = lowFrisbeesMade[0] / hundred;
                        }

                        else if (buttons[6] && buttons[0] && !buttons[1] && !buttons[3] && !buttons[2] && !buttons[5] && !buttons[4] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            lowFrisbeesAtt[0]--;
                            displayLowFrisbeesAtt[0] = lowFrisbeesAtt[0] / hundred;

                            if (lowFrisbeesAtt[0] <= 0)
                            {
                                lowFrisbeesAtt[0] = 0;
                            }
                        }

                        else if (buttons[6] && buttons[2] && !buttons[1] && !buttons[3] && !buttons[0] && !buttons[5] && !buttons[4] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            lowFrisbeesAtt[0]++;
                            displayLowFrisbeesAtt[0] = lowFrisbeesAtt[0] / hundred;
                        }

                        // Code for the robot climb
                        if (buttons[12] && !buttons[0] && !buttons[1] && !buttons[2] && !buttons[3] && !buttons[4] && !buttons[5] && !buttons[6] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11])
                        {
                            climb[0]++;
                            robotClimb[0] = climb[0] / hundred;

                            if (robotClimb[0] > 3)
                            {
                                robotClimb[0] = 0;
                                climb[0] = 0;
                            }

                            robotClimb[0] = robotClimb[0] * 10;
                        }

                        int teleOpPyramid = displayPyramidFrisbeesMade[0] * 5;
                        int teleOpHigh = displayHighFrisbeesMade[0] * 3;
                        int teleOpMid = displayMidFrisbeesMade[0] * 2;
                        int teleOpLow = displayLowFrisbeesMade[0] * 1;

                        teleOpTotalPoints[0] = teleOpPyramid + teleOpHigh + teleOpMid + teleOpLow + robotClimb[0];

                    }

                    if (lblAuto.Visible == true)
                    {
                        // This code counts how many High goals were made and attempted
                        if (buttons[4] && buttons[1] && !buttons[0] && !buttons[2] && !buttons[3] && !buttons[5] && !buttons[6] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            autoHighFrisbeesMade[0]--;
                            autoDisplayHighFrisbeesMade[0] = autoHighFrisbeesMade[0] / hundred;

                            if (autoHighFrisbeesMade[0] <= 0)
                            {
                                autoHighFrisbeesMade[0] = 0;
                            }
                        }
                        else if (buttons[4] && buttons[3] && !buttons[0] && !buttons[2] && !buttons[1] && !buttons[5] && !buttons[6] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            autoHighFrisbeesMade[0]++;
                            autoDisplayHighFrisbeesMade[0] = autoHighFrisbeesMade[0] / hundred;
                        }

                        else if (buttons[4] && buttons[0] && !buttons[1] && !buttons[2] && !buttons[3] && !buttons[5] && !buttons[6] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            autoHighFrisbeesAtt[0]--;
                            autoDisplayHighFrisbeesAtt[0] = autoHighFrisbeesAtt[0] / hundred;

                            if (autoHighFrisbeesAtt[0] <= 0)
                            {
                                autoHighFrisbeesAtt[0] = 0;
                            }
                        }

                        else if (buttons[4] && buttons[2] && !buttons[0] && !buttons[1] && !buttons[3] && !buttons[5] && !buttons[6] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            autoHighFrisbeesAtt[0]++;
                            autoDisplayHighFrisbeesAtt[0] = autoHighFrisbeesAtt[0] / hundred;
                        }

                        // This code counts how many Mid goals were made and attempted
                        if (buttons[7] && buttons[1] && !buttons[0] && !buttons[2] && !buttons[3] && !buttons[5] && !buttons[6] && !buttons[4] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            autoMidFrisbeesMade[0]--;
                            autoDisplayMidFrisbeesMade[0] = autoMidFrisbeesMade[0] / hundred;

                            if (autoMidFrisbeesMade[0] <= 0)
                            {
                                autoMidFrisbeesMade[0] = 0;
                            }
                        }

                        else if (buttons[7] && buttons[3] && !buttons[0] && !buttons[2] && !buttons[4] && !buttons[5] && !buttons[6] && !buttons[1] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            autoMidFrisbeesMade[0]++;
                            autoDisplayMidFrisbeesMade[0] = autoMidFrisbeesMade[0] / hundred;
                        }

                        else if (buttons[7] && buttons[0] && !buttons[1] && !buttons[2] && !buttons[3] && !buttons[5] && !buttons[6] && !buttons[4] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            autoMidFrisbeesAtt[0]--;
                            autoDisplayMidFrisbeesAtt[0] = autoMidFrisbeesAtt[0] / hundred;

                            if (autoMidFrisbeesAtt[0] <= 0)
                            {
                                autoMidFrisbeesAtt[0] = 0;
                            }
                        }

                        else if (buttons[7] && buttons[2] && !buttons[0] && !buttons[1] && !buttons[3] && !buttons[5] && !buttons[6] && !buttons[4] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            autoMidFrisbeesAtt[0]++;
                            autoDisplayMidFrisbeesAtt[0] = autoMidFrisbeesAtt[0] / hundred;
                        }

                        // This code counts how many low goals were made and attempted
                        if (buttons[6] && buttons[1] && !buttons[0] && !buttons[2] && !buttons[3] && !buttons[5] && !buttons[4] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            autoLowFrisbeesMade[0]--;
                            autoDisplayLowFrisbeesMade[0] = autoLowFrisbeesMade[0] / hundred;

                            if (autoLowFrisbeesMade[0] <= 0)
                            {
                                autoLowFrisbeesMade[0] = 0;
                            }
                        }

                        else if (buttons[6] && buttons[3] && !buttons[0] && !buttons[2] && !buttons[1] && !buttons[5] && !buttons[4] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            autoLowFrisbeesMade[0]++;
                            autoDisplayLowFrisbeesMade[0] = autoLowFrisbeesMade[0] / hundred;
                        }

                        else if (buttons[6] && buttons[0] && !buttons[1] && !buttons[2] && !buttons[3] && !buttons[5] && !buttons[4] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            autoLowFrisbeesAtt[0]--;
                            autoDisplayLowFrisbeesAtt[0] = autoLowFrisbeesAtt[0] / hundred;

                            if (autoLowFrisbeesAtt[0] <= 0)
                            {
                                autoLowFrisbeesAtt[0] = 0;
                            }
                        }

                        else if (buttons[6] && buttons[2] && !buttons[0] && !buttons[1] && !buttons[3] && !buttons[5] && !buttons[4] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            autoLowFrisbeesAtt[0]++;
                            autoDisplayLowFrisbeesAtt[0] = autoLowFrisbeesAtt[0] / hundred;
                        }

                        int autoHigh = autoDisplayHighFrisbeesMade[0] * 6;
                        int autoMid = autoDisplayMidFrisbeesMade[0] * 4;
                        int autoLow = autoDisplayLowFrisbeesMade[0] * 2;

                        autoTotalPoints[0] = autoHigh + autoMid + autoLow;

                    }

                    int total = teleOpTotalPoints[0] + autoTotalPoints[0];

                    lblTotalPoints.Text = total.ToString();
                }

                if (id == 1)
                {
                    // Pyramid Goals
                    lblTeleOpPyramidMade6.Text = displayPyramidFrisbeesMade[5].ToString();
                    lblTeleOpPyramidAtt6.Text = displayPyramidFrisbeesAtt[5].ToString();

                    // High Goals
                    lblTeleOpHighMade6.Text = displayHighFrisbeesMade[5].ToString();
                    lblTeleOpHighAtt6.Text = displayHighFrisbeesAtt[5].ToString();
                    lblAutoHighMade6.Text = autoDisplayHighFrisbeesMade[5].ToString();
                    lblAutoHighAtt6.Text = autoDisplayHighFrisbeesAtt[5].ToString();

                    // Mid Goals
                    lblTeleOpMidMade6.Text = displayMidFrisbeesMade[5].ToString();
                    lblTeleOpMidAtt6.Text = displayMidFrisbeesAtt[5].ToString();
                    lblAutoMidMade6.Text = autoDisplayMidFrisbeesMade[5].ToString();
                    lblAutoMidAtt6.Text = autoDisplayMidFrisbeesAtt[5].ToString();

                    // Low Goals
                    lblTeleOpLowMade6.Text = displayLowFrisbeesMade[5].ToString();
                    lblTeleOpLowAtt6.Text = displayLowFrisbeesAtt[5].ToString();
                    lblAutoLowMade6.Text = autoDisplayLowFrisbeesMade[5].ToString();
                    lblAutoLowAtt6.Text = autoDisplayLowFrisbeesAtt[5].ToString();

                    // Robot Climb
                    lblRobotClimb6.Text = robotClimb[5].ToString();

                    lblTeleOpTotalPoints6.Text = teleOpTotalPoints[5].ToString();
                    lblAutoTotalPoints6.Text = autoTotalPoints[5].ToString();

                    //Defense Rating
                    lblDefense6.Text = displayDefenseRating[5].ToString();

                    if (buttons[9] && !buttons[0] && !buttons[1] && !buttons[2] && !buttons[3] && !buttons[4] && !buttons[5] && !buttons[6] && !buttons[7] && !buttons[8] && !buttons[10] && !buttons[11] && !buttons[12])
                    {
                        lblAuto6.Visible = false;
                        lblTeleOp6.Visible = true;
                        lblAutoHighAtt6.Visible = false;
                        lblTeleOpHighAtt6.Visible = true;
                        lblAutoHighMade6.Visible = false;
                        lblTeleOpHighMade6.Visible = true;
                        lblAutoMidAtt6.Visible = false;
                        lblTeleOpMidAtt6.Visible = true;
                        lblAutoMidMade6.Visible = false;
                        lblTeleOpMidMade6.Visible = true;
                        lblAutoLowAtt6.Visible = false;
                        lblTeleOpLowAtt6.Visible = true;
                        lblAutoLowMade6.Visible = false;
                        lblTeleOpLowMade6.Visible = true;
                        lblTeleOpPyramidAtt6.Visible = true;
                        lblTeleOpPyramidMade6.Visible = true;
                        lblRobotClimb6.Visible = true;
                    }

                    if (buttons[8] && !buttons[0] && !buttons[1] && !buttons[2] && !buttons[3] && !buttons[4] && !buttons[5] && !buttons[6] && !buttons[7] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                    {
                        lblAuto6.Visible = true;
                        lblTeleOp6.Visible = false;
                        lblAutoHighAtt6.Visible = true;
                        lblTeleOpHighAtt6.Visible = false;
                        lblAutoHighMade6.Visible = true;
                        lblTeleOpHighMade6.Visible = false;
                        lblAutoMidAtt6.Visible = true;
                        lblTeleOpMidAtt6.Visible = false;
                        lblAutoMidMade6.Visible = true;
                        lblTeleOpMidMade6.Visible = false;
                        lblAutoLowAtt6.Visible = true;
                        lblTeleOpLowAtt6.Visible = false;
                        lblAutoLowMade6.Visible = true;
                        lblTeleOpLowMade6.Visible = false;
                        lblTeleOpPyramidAtt6.Visible = false;
                        lblTeleOpPyramidMade6.Visible = false;
                        lblRobotClimb6.Visible = false;
                    }



                    //The following code obtains and tells which button is each game pad is being pressed.
                    if (buttons[x])
                    {
                        strText6 += x.ToString("00 ", CultureInfo.CurrentCulture);
                        lbldisplayButtons6.Text = strText6;
                    }

                    if (lblTeleOp6.Visible == true)
                    {

                        if (buttons[10] && !buttons[0] && !buttons[1] && !buttons[2] && !buttons[3] && !buttons[4] && !buttons[5] && !buttons[6] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[11] && !buttons[12])
                        {
                            defenseRating[5]++;
                            displayDefenseRating[5] = defenseRating[5] / hundred;
                            if (displayDefenseRating[5] > 10)
                            {
                                defenseRating[5] = 0;
                                displayDefenseRating[5] = 0;
                            }
                        }

                        else if (buttons[11] && !buttons[0] && !buttons[1] && !buttons[2] && !buttons[3] && !buttons[4] && !buttons[5] && !buttons[6] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[12])
                        {
                            defenseRating[5]--;
                            displayDefenseRating[5] = defenseRating[5] / hundred;
                            if (displayDefenseRating[5] <= 0)
                            {
                                defenseRating[5] = 0;
                            }
                        }
                        // This is the code that counts how many pyramid goals were made or attempted
                        if (buttons[5] && buttons[1] && !buttons[0] && !buttons[2] && !buttons[3] && !buttons[4] && !buttons[6] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            pyramidFrisbeesMade[5]--;
                            displayPyramidFrisbeesMade[5] = pyramidFrisbeesMade[5] / hundred;

                            if (pyramidFrisbeesMade[5] <= 0)
                            {
                                pyramidFrisbeesMade[5] = 0;
                            }
                        }

                        else if (buttons[5] && buttons[3] && !buttons[0] && !buttons[2] && !buttons[1] && !buttons[4] && !buttons[6] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            pyramidFrisbeesMade[5]++;
                            displayPyramidFrisbeesMade[5] = pyramidFrisbeesMade[5] / hundred;
                        }

                        else if (buttons[5] && buttons[0] && !buttons[1] && !buttons[2] && !buttons[3] && !buttons[4] && !buttons[6] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            pyramidFrisbeesAtt[5]--;
                            displayPyramidFrisbeesAtt[5] = pyramidFrisbeesAtt[5] / hundred;

                            if (pyramidFrisbeesAtt[5] <= 0)
                            {
                                pyramidFrisbeesAtt[5] = 0;
                            }
                        }

                        else if (buttons[5] && buttons[2] && !buttons[1] && !buttons[0] && !buttons[3] && !buttons[4] && !buttons[6] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            pyramidFrisbeesAtt[5]++;
                            displayPyramidFrisbeesAtt[5] = pyramidFrisbeesAtt[5] / hundred;
                        }

                        // This code counts how many High goals were made and attempted
                        if (buttons[4] && buttons[1] && !buttons[0] && !buttons[2] && !buttons[3] && !buttons[5] && !buttons[6] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            highFrisbeesMade[5]--;
                            displayHighFrisbeesMade[5] = highFrisbeesMade[5] / hundred;

                            if (highFrisbeesMade[5] <= 0)
                            {
                                highFrisbeesMade[5] = 0;
                            }
                        }
                        else if (buttons[4] && buttons[3] && !buttons[1] && !buttons[2] && !buttons[0] && !buttons[5] && !buttons[6] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            highFrisbeesMade[5]++;
                            displayHighFrisbeesMade[5] = highFrisbeesMade[5] / hundred;
                        }

                        else if (buttons[4] && buttons[0] && !buttons[1] && !buttons[2] && !buttons[3] && !buttons[5] && !buttons[6] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            highFrisbeesAtt[5]--;
                            displayHighFrisbeesAtt[5] = highFrisbeesAtt[5] / hundred;

                            if (highFrisbeesAtt[5] <= 0)
                            {
                                highFrisbeesAtt[5] = 0;
                            }
                        }

                        else if (buttons[4] && buttons[2] && !buttons[1] && !buttons[3] && !buttons[0] && !buttons[5] && !buttons[6] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            highFrisbeesAtt[5]++;
                            displayHighFrisbeesAtt[5] = highFrisbeesAtt[5] / hundred;
                        }

                        // This code counts how many Mid goals were made and attempted
                        if (buttons[7] && buttons[1] && !buttons[2] && !buttons[3] && !buttons[4] && !buttons[5] && !buttons[6] && !buttons[0] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            midFrisbeesMade[5]--;
                            displayMidFrisbeesMade[5] = midFrisbeesMade[5] / hundred;

                            if (midFrisbeesMade[5] <= 0)
                            {
                                midFrisbeesMade[5] = 0;
                            }
                        }

                        else if (buttons[7] && buttons[3] && !buttons[1] && !buttons[2] && !buttons[0] && !buttons[5] && !buttons[6] && !buttons[4] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            midFrisbeesMade[5]++;
                            displayMidFrisbeesMade[5] = midFrisbeesMade[5] / hundred;
                        }

                        else if (buttons[7] && buttons[0] && !buttons[1] && !buttons[3] && !buttons[2] && !buttons[5] && !buttons[6] && !buttons[4] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            midFrisbeesAtt[5]--;
                            displayMidFrisbeesAtt[5] = midFrisbeesAtt[5] / hundred;

                            if (midFrisbeesAtt[5] <= 0)
                            {
                                midFrisbeesAtt[5] = 0;
                            }
                        }

                        else if (buttons[7] && buttons[2] && !buttons[1] && !buttons[3] && !buttons[0] && !buttons[5] && !buttons[6] && !buttons[4] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            midFrisbeesAtt[5]++;
                            displayMidFrisbeesAtt[5] = midFrisbeesAtt[5] / hundred;
                        }

                        // This code counts how many low goals were made and attempted
                        if (buttons[6] && buttons[1] && !buttons[2] && !buttons[3] && !buttons[0] && !buttons[5] && !buttons[4] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            lowFrisbeesMade[5]--;
                            displayLowFrisbeesMade[5] = lowFrisbeesMade[5] / hundred;

                            if (lowFrisbeesMade[5] <= 0)
                            {
                                lowFrisbeesMade[5] = 0;
                            }
                        }

                        else if (buttons[6] && buttons[3] && !buttons[1] && !buttons[2] && !buttons[0] && !buttons[5] && !buttons[4] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            lowFrisbeesMade[5]++;
                            displayLowFrisbeesMade[5] = lowFrisbeesMade[5] / hundred;
                        }

                        else if (buttons[6] && buttons[0] && !buttons[1] && !buttons[3] && !buttons[2] && !buttons[5] && !buttons[4] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            lowFrisbeesAtt[5]--;
                            displayLowFrisbeesAtt[5] = lowFrisbeesAtt[5] / hundred;

                            if (lowFrisbeesAtt[5] <= 0)
                            {
                                lowFrisbeesAtt[5] = 0;
                            }
                        }

                        else if (buttons[6] && buttons[2] && !buttons[1] && !buttons[3] && !buttons[0] && !buttons[5] && !buttons[4] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            lowFrisbeesAtt[5]++;
                            displayLowFrisbeesAtt[5] = lowFrisbeesAtt[5] / hundred;
                        }

                        // Code for the robot climb
                        if (buttons[12] && !buttons[0] && !buttons[1] && !buttons[2] && !buttons[3] && !buttons[4] && !buttons[5] && !buttons[6] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11])
                        {
                            climb[5]++;
                            robotClimb[5] = climb[5] / hundred;

                            if (robotClimb[5] > 3)
                            {
                                robotClimb[5] = 0;
                                climb[5] = 0;
                            }

                            robotClimb[5] = robotClimb[5] * 10;
                        }

                        int teleOpPyramid6 = displayPyramidFrisbeesMade[5] * 5;
                        int teleOpHigh6 = displayHighFrisbeesMade[5] * 3;
                        int teleOpMid6 = displayMidFrisbeesMade[5] * 2;
                        int teleOpLow6 = displayLowFrisbeesMade[5] * 1;

                        teleOpTotalPoints[5] = teleOpPyramid6 + teleOpHigh6 + teleOpMid6 + teleOpLow6 + robotClimb[5];

                    }

                    if (lblAuto6.Visible == true)
                    {
                        // This code counts how many High goals were made and attempted
                        if (buttons[4] && buttons[1] && !buttons[0] && !buttons[2] && !buttons[3] && !buttons[5] && !buttons[6] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            autoHighFrisbeesMade[5]--;
                            autoDisplayHighFrisbeesMade[5] = autoHighFrisbeesMade[5] / hundred;

                            if (autoHighFrisbeesMade[5] <= 0)
                            {
                                autoHighFrisbeesMade[5] = 0;
                            }
                        }
                        else if (buttons[4] && buttons[3] && !buttons[0] && !buttons[2] && !buttons[1] && !buttons[5] && !buttons[6] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            autoHighFrisbeesMade[5]++;
                            autoDisplayHighFrisbeesMade[5] = autoHighFrisbeesMade[5] / hundred;
                        }

                        else if (buttons[4] && buttons[0] && !buttons[1] && !buttons[2] && !buttons[3] && !buttons[5] && !buttons[6] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            autoHighFrisbeesAtt[5]--;
                            autoDisplayHighFrisbeesAtt[5] = autoHighFrisbeesAtt[5] / hundred;

                            if (autoHighFrisbeesAtt[5] <= 0)
                            {
                                autoHighFrisbeesAtt[5] = 0;
                            }
                        }

                        else if (buttons[4] && buttons[2] && !buttons[0] && !buttons[1] && !buttons[3] && !buttons[5] && !buttons[6] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            autoHighFrisbeesAtt[5]++;
                            autoDisplayHighFrisbeesAtt[5] = autoHighFrisbeesAtt[5] / hundred;
                        }

                        // This code counts how many Mid goals were made and attempted
                        if (buttons[7] && buttons[1] && !buttons[0] && !buttons[2] && !buttons[3] && !buttons[5] && !buttons[6] && !buttons[4] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            autoMidFrisbeesMade[5]--;
                            autoDisplayMidFrisbeesMade[5] = autoMidFrisbeesMade[5] / hundred;

                            if (autoMidFrisbeesMade[5] <= 0)
                            {
                                autoMidFrisbeesMade[5] = 0;
                            }
                        }

                        else if (buttons[7] && buttons[3] && !buttons[0] && !buttons[2] && !buttons[4] && !buttons[5] && !buttons[6] && !buttons[1] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            autoMidFrisbeesMade[5]++;
                            autoDisplayMidFrisbeesMade[5] = autoMidFrisbeesMade[5] / hundred;
                        }

                        else if (buttons[7] && buttons[0] && !buttons[1] && !buttons[2] && !buttons[3] && !buttons[5] && !buttons[6] && !buttons[4] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            autoMidFrisbeesAtt[5]--;
                            autoDisplayMidFrisbeesAtt[5] = autoMidFrisbeesAtt[5] / hundred;

                            if (autoMidFrisbeesAtt[5] <= 0)
                            {
                                autoMidFrisbeesAtt[5] = 0;
                            }
                        }

                        else if (buttons[7] && buttons[2] && !buttons[0] && !buttons[1] && !buttons[3] && !buttons[5] && !buttons[6] && !buttons[4] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            autoMidFrisbeesAtt[5]++;
                            autoDisplayMidFrisbeesAtt[5] = autoMidFrisbeesAtt[5] / hundred;
                        }

                        // This code counts how many low goals were made and attempted
                        if (buttons[6] && buttons[1] && !buttons[0] && !buttons[2] && !buttons[3] && !buttons[5] && !buttons[4] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            autoLowFrisbeesMade[5]--;
                            autoDisplayLowFrisbeesMade[5] = autoLowFrisbeesMade[5] / hundred;

                            if (autoLowFrisbeesMade[5] <= 0)
                            {
                                autoLowFrisbeesMade[5] = 0;
                            }
                        }

                        else if (buttons[6] && buttons[3] && !buttons[0] && !buttons[2] && !buttons[1] && !buttons[5] && !buttons[4] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            autoLowFrisbeesMade[5]++;
                            autoDisplayLowFrisbeesMade[5] = autoLowFrisbeesMade[5] / hundred;
                        }

                        else if (buttons[6] && buttons[0] && !buttons[1] && !buttons[2] && !buttons[3] && !buttons[5] && !buttons[4] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            autoLowFrisbeesAtt[5]--;
                            autoDisplayLowFrisbeesAtt[5] = autoLowFrisbeesAtt[5] / hundred;

                            if (autoLowFrisbeesAtt[5] <= 0)
                            {
                                autoLowFrisbeesAtt[5] = 0;
                            }
                        }

                        else if (buttons[6] && buttons[2] && !buttons[0] && !buttons[1] && !buttons[3] && !buttons[5] && !buttons[4] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            autoLowFrisbeesAtt[5]++;
                            autoDisplayLowFrisbeesAtt[5] = autoLowFrisbeesAtt[5] / hundred;
                        }

                        int autoHigh6 = autoDisplayHighFrisbeesMade[5] * 6;
                        int autoMid6 = autoDisplayMidFrisbeesMade[5] * 4;
                        int autoLow6 = autoDisplayLowFrisbeesMade[5] * 2;

                        autoTotalPoints[5] = autoHigh6 + autoMid6 + autoLow6;

                    }

                    int total6 = teleOpTotalPoints[5] + autoTotalPoints[5];

                    lblTotalPoints6.Text = total6.ToString();
                }

                if (id == 2)
                {
                    // Pyramid Goals
                    lblTeleOpPyramidMade5.Text = displayPyramidFrisbeesMade[4].ToString();
                    lblTeleOpPyramidAtt5.Text = displayPyramidFrisbeesAtt[4].ToString();

                    // High Goals
                    lblTeleOpHighMade5.Text = displayHighFrisbeesMade[4].ToString();
                    lblTeleOpHighAtt5.Text = displayHighFrisbeesAtt[4].ToString();
                    lblAutoHighMade5.Text = autoDisplayHighFrisbeesMade[4].ToString();
                    lblAutoHighAtt5.Text = autoDisplayHighFrisbeesAtt[4].ToString();

                    // Mid Goals
                    lblTeleOpMidMade5.Text = displayMidFrisbeesMade[4].ToString();
                    lblTeleOpMidAtt5.Text = displayMidFrisbeesAtt[4].ToString();
                    lblAutoMidMade5.Text = autoDisplayMidFrisbeesMade[4].ToString();
                    lblAutoMidAtt5.Text = autoDisplayMidFrisbeesAtt[4].ToString();

                    // Low Goals
                    lblTeleOpLowMade5.Text = displayLowFrisbeesMade[4].ToString();
                    lblTeleOpLowAtt5.Text = displayLowFrisbeesAtt[4].ToString();
                    lblAutoLowMade5.Text = autoDisplayLowFrisbeesMade[4].ToString();
                    lblAutoLowAtt5.Text = autoDisplayLowFrisbeesAtt[4].ToString();

                    // Robot Climb
                    lblRobotClimb5.Text = robotClimb[4].ToString();

                    lblTeleOpTotalPoints5.Text = teleOpTotalPoints[4].ToString();
                    lblAutoTotalPoints5.Text = autoTotalPoints[4].ToString();

                    //Defense Rating
                    lblDefense5.Text = displayDefenseRating[4].ToString();

                    if (buttons[9] && !buttons[0] && !buttons[1] && !buttons[2] && !buttons[3] && !buttons[4] && !buttons[5] && !buttons[6] && !buttons[7] && !buttons[8] && !buttons[10] && !buttons[11] && !buttons[12])
                    {
                        lblAuto5.Visible = false;
                        lblTeleOp5.Visible = true;
                        lblAutoHighAtt5.Visible = false;
                        lblTeleOpHighAtt5.Visible = true;
                        lblAutoHighMade5.Visible = false;
                        lblTeleOpHighMade5.Visible = true;
                        lblAutoMidAtt5.Visible = false;
                        lblTeleOpMidAtt5.Visible = true;
                        lblAutoMidMade5.Visible = false;
                        lblTeleOpMidMade5.Visible = true;
                        lblAutoLowAtt5.Visible = false;
                        lblTeleOpLowAtt5.Visible = true;
                        lblAutoLowMade5.Visible = false;
                        lblTeleOpLowMade5.Visible = true;
                        lblTeleOpPyramidAtt5.Visible = true;
                        lblTeleOpPyramidMade5.Visible = true;
                        lblRobotClimb5.Visible = true;
                    }

                    if (buttons[8] && !buttons[0] && !buttons[1] && !buttons[2] && !buttons[3] && !buttons[4] && !buttons[5] && !buttons[6] && !buttons[7] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                    {
                        lblAuto5.Visible = true;
                        lblTeleOp5.Visible = false;
                        lblAutoHighAtt5.Visible = true;
                        lblTeleOpHighAtt5.Visible = false;
                        lblAutoHighMade5.Visible = true;
                        lblTeleOpHighMade5.Visible = false;
                        lblAutoMidAtt5.Visible = true;
                        lblTeleOpMidAtt5.Visible = false;
                        lblAutoMidMade5.Visible = true;
                        lblTeleOpMidMade5.Visible = false;
                        lblAutoLowAtt5.Visible = true;
                        lblTeleOpLowAtt5.Visible = false;
                        lblAutoLowMade5.Visible = true;
                        lblTeleOpLowMade5.Visible = false;
                        lblTeleOpPyramidAtt5.Visible = false;
                        lblTeleOpPyramidMade5.Visible = false;
                        lblRobotClimb5.Visible = false;
                    }

                    //The following code obtains and tells which button is each game pad is being pressed.
                    if (buttons[x])
                    {
                        strText5 += x.ToString("00 ", CultureInfo.CurrentCulture);
                        lbldisplayButtons5.Text = strText5;
                    }

                    if (lblTeleOp5.Visible == true)
                    {

                        if (buttons[10] && !buttons[0] && !buttons[1] && !buttons[2] && !buttons[3] && !buttons[4] && !buttons[5] && !buttons[6] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[11] && !buttons[12])
                        {
                            defenseRating[4]++;
                            displayDefenseRating[4] = defenseRating[4] / hundred;
                            if (displayDefenseRating[4] > 10)
                            {
                                defenseRating[4] = 0;
                                displayDefenseRating[4] = 0;
                            }
                        }

                        else if (buttons[11] && !buttons[0] && !buttons[1] && !buttons[2] && !buttons[3] && !buttons[4] && !buttons[5] && !buttons[6] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[12])
                        {
                            defenseRating[4]--;
                            displayDefenseRating[4] = defenseRating[4] / hundred;
                            if (displayDefenseRating[4] <= 0)
                            {
                                defenseRating[4] = 0;
                            }
                        }
                        // This is the code that counts how many pyramid goals were made or attempted
                        if (buttons[5] && buttons[1] && !buttons[0] && !buttons[2] && !buttons[3] && !buttons[4] && !buttons[6] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            pyramidFrisbeesMade[4]--;
                            displayPyramidFrisbeesMade[4] = pyramidFrisbeesMade[4] / hundred;

                            if (pyramidFrisbeesMade[4] <= 0)
                            {
                                pyramidFrisbeesMade[4] = 0;
                            }
                        }

                        else if (buttons[5] && buttons[3] && !buttons[0] && !buttons[2] && !buttons[1] && !buttons[4] && !buttons[6] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            pyramidFrisbeesMade[4]++;
                            displayPyramidFrisbeesMade[4] = pyramidFrisbeesMade[4] / hundred;
                        }

                        else if (buttons[5] && buttons[0] && !buttons[1] && !buttons[2] && !buttons[3] && !buttons[4] && !buttons[6] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            pyramidFrisbeesAtt[4]--;
                            displayPyramidFrisbeesAtt[4] = pyramidFrisbeesAtt[4] / hundred;

                            if (pyramidFrisbeesAtt[4] <= 0)
                            {
                                pyramidFrisbeesAtt[4] = 0;
                            }
                        }

                        else if (buttons[5] && buttons[2] && !buttons[1] && !buttons[0] && !buttons[3] && !buttons[4] && !buttons[6] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            pyramidFrisbeesAtt[4]++;
                            displayPyramidFrisbeesAtt[4] = pyramidFrisbeesAtt[4] / hundred;
                        }

                        // This code counts how many High goals were made and attempted
                        if (buttons[4] && buttons[1] && !buttons[0] && !buttons[2] && !buttons[3] && !buttons[5] && !buttons[6] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            highFrisbeesMade[4]--;
                            displayHighFrisbeesMade[4] = highFrisbeesMade[4] / hundred;

                            if (highFrisbeesMade[4] <= 0)
                            {
                                highFrisbeesMade[4] = 0;
                            }
                        }
                        else if (buttons[4] && buttons[3] && !buttons[1] && !buttons[2] && !buttons[0] && !buttons[5] && !buttons[6] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            highFrisbeesMade[4]++;
                            displayHighFrisbeesMade[4] = highFrisbeesMade[4] / hundred;
                        }

                        else if (buttons[4] && buttons[0] && !buttons[1] && !buttons[2] && !buttons[3] && !buttons[5] && !buttons[6] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            highFrisbeesAtt[4]--;
                            displayHighFrisbeesAtt[4] = highFrisbeesAtt[4] / hundred;

                            if (highFrisbeesAtt[4] <= 0)
                            {
                                highFrisbeesAtt[4] = 0;
                            }
                        }

                        else if (buttons[4] && buttons[2] && !buttons[1] && !buttons[3] && !buttons[0] && !buttons[5] && !buttons[6] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            highFrisbeesAtt[4]++;
                            displayHighFrisbeesAtt[4] = highFrisbeesAtt[4] / hundred;
                        }


                        // This code counts how many Mid goals were made and attempted
                        if (buttons[7] && buttons[1] && !buttons[2] && !buttons[3] && !buttons[4] && !buttons[5] && !buttons[6] && !buttons[0] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            midFrisbeesMade[4]--;
                            displayMidFrisbeesMade[4] = midFrisbeesMade[4] / hundred;

                            if (midFrisbeesMade[4] <= 0)
                            {
                                midFrisbeesMade[4] = 0;
                            }
                        }

                        else if (buttons[7] && buttons[3] && !buttons[1] && !buttons[2] && !buttons[0] && !buttons[5] && !buttons[6] && !buttons[4] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            midFrisbeesMade[4]++;
                            displayMidFrisbeesMade[4] = midFrisbeesMade[4] / hundred;
                        }

                        else if (buttons[7] && buttons[0] && !buttons[1] && !buttons[3] && !buttons[2] && !buttons[5] && !buttons[6] && !buttons[4] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            midFrisbeesAtt[4]--;
                            displayMidFrisbeesAtt[4] = midFrisbeesAtt[4] / hundred;

                            if (midFrisbeesAtt[4] <= 0)
                            {
                                midFrisbeesAtt[4] = 0;
                            }
                        }

                        else if (buttons[7] && buttons[2] && !buttons[1] && !buttons[3] && !buttons[0] && !buttons[5] && !buttons[6] && !buttons[4] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            midFrisbeesAtt[4]++;
                            displayMidFrisbeesAtt[4] = midFrisbeesAtt[4] / hundred;
                        }

                        // This code counts how many low goals were made and attempted
                        if (buttons[6] && buttons[1] && !buttons[2] && !buttons[3] && !buttons[0] && !buttons[5] && !buttons[4] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            lowFrisbeesMade[4]--;
                            displayLowFrisbeesMade[4] = lowFrisbeesMade[4] / hundred;

                            if (lowFrisbeesMade[4] <= 0)
                            {
                                lowFrisbeesMade[4] = 0;
                            }
                        }

                        else if (buttons[6] && buttons[3] && !buttons[1] && !buttons[2] && !buttons[0] && !buttons[5] && !buttons[4] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            lowFrisbeesMade[4]++;
                            displayLowFrisbeesMade[4] = lowFrisbeesMade[4] / hundred;
                        }

                        else if (buttons[6] && buttons[0] && !buttons[1] && !buttons[3] && !buttons[2] && !buttons[5] && !buttons[4] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            lowFrisbeesAtt[4]--;
                            displayLowFrisbeesAtt[4] = lowFrisbeesAtt[4] / hundred;

                            if (lowFrisbeesAtt[4] <= 0)
                            {
                                lowFrisbeesAtt[4] = 0;
                            }
                        }

                        else if (buttons[6] && buttons[2] && !buttons[1] && !buttons[3] && !buttons[0] && !buttons[5] && !buttons[4] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            lowFrisbeesAtt[4]++;
                            displayLowFrisbeesAtt[4] = lowFrisbeesAtt[4] / hundred;
                        }

                        // Code for the robot climb
                        if (buttons[12] && !buttons[0] && !buttons[1] && !buttons[2] && !buttons[3] && !buttons[4] && !buttons[5] && !buttons[6] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11])
                        {
                            climb[4]++;
                            robotClimb[4] = climb[4] / hundred;

                            if (robotClimb[4] > 3)
                            {
                                robotClimb[4] = 0;
                                climb[4] = 0;
                            }

                            robotClimb[4] = robotClimb[4] * 10;
                        }

                        int teleOpPyramid5 = displayPyramidFrisbeesMade[4] * 5;
                        int teleOpHigh5 = displayHighFrisbeesMade[4] * 3;
                        int teleOpMid5 = displayMidFrisbeesMade[4] * 2;
                        int teleOpLow5 = displayLowFrisbeesMade[4] * 1;

                        teleOpTotalPoints[4] = teleOpPyramid5 + teleOpHigh5 + teleOpMid5 + teleOpLow5 + robotClimb[4];

                    }

                    if (lblAuto5.Visible == true)
                    {
                        // This code counts how many High goals were made and attempted
                        if (buttons[4] && buttons[1] && !buttons[0] && !buttons[2] && !buttons[3] && !buttons[5] && !buttons[6] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            autoHighFrisbeesMade[4]--;
                            autoDisplayHighFrisbeesMade[4] = autoHighFrisbeesMade[4] / hundred;

                            if (autoHighFrisbeesMade[4] <= 0)
                            {
                                autoHighFrisbeesMade[4] = 0;
                            }
                        }
                        else if (buttons[4] && buttons[3] && !buttons[0] && !buttons[2] && !buttons[1] && !buttons[5] && !buttons[6] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            autoHighFrisbeesMade[4]++;
                            autoDisplayHighFrisbeesMade[4] = autoHighFrisbeesMade[4] / hundred;
                        }

                        else if (buttons[4] && buttons[0] && !buttons[1] && !buttons[2] && !buttons[3] && !buttons[5] && !buttons[6] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            autoHighFrisbeesAtt[4]--;
                            autoDisplayHighFrisbeesAtt[4] = autoHighFrisbeesAtt[4] / hundred;

                            if (autoHighFrisbeesAtt[4] <= 0)
                            {
                                autoHighFrisbeesAtt[4] = 0;
                            }
                        }

                        else if (buttons[4] && buttons[2] && !buttons[0] && !buttons[1] && !buttons[3] && !buttons[5] && !buttons[6] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            autoHighFrisbeesAtt[4]++;
                            autoDisplayHighFrisbeesAtt[4] = autoHighFrisbeesAtt[4] / hundred;
                        }

                        // This code counts how many Mid goals were made and attempted
                        if (buttons[7] && buttons[1] && !buttons[0] && !buttons[2] && !buttons[3] && !buttons[5] && !buttons[6] && !buttons[4] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            autoMidFrisbeesMade[4]--;
                            autoDisplayMidFrisbeesMade[4] = autoMidFrisbeesMade[4] / hundred;

                            if (autoMidFrisbeesMade[4] <= 0)
                            {
                                autoMidFrisbeesMade[4] = 0;
                            }
                        }

                        else if (buttons[7] && buttons[3] && !buttons[0] && !buttons[2] && !buttons[4] && !buttons[5] && !buttons[6] && !buttons[1] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            autoMidFrisbeesMade[4]++;
                            autoDisplayMidFrisbeesMade[4] = autoMidFrisbeesMade[4] / hundred;
                        }

                        else if (buttons[7] && buttons[0] && !buttons[1] && !buttons[2] && !buttons[3] && !buttons[5] && !buttons[6] && !buttons[4] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            autoMidFrisbeesAtt[4]--;
                            autoDisplayMidFrisbeesAtt[4] = autoMidFrisbeesAtt[4] / hundred;

                            if (autoMidFrisbeesAtt[4] <= 0)
                            {
                                autoMidFrisbeesAtt[4] = 0;
                            }
                        }

                        else if (buttons[7] && buttons[2] && !buttons[0] && !buttons[1] && !buttons[3] && !buttons[5] && !buttons[6] && !buttons[4] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            autoMidFrisbeesAtt[4]++;
                            autoDisplayMidFrisbeesAtt[4] = autoMidFrisbeesAtt[4] / hundred;
                        }

                        // This code counts how many low goals were made and attempted
                        if (buttons[6] && buttons[1] && !buttons[0] && !buttons[2] && !buttons[3] && !buttons[5] && !buttons[4] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            autoLowFrisbeesMade[4]--;
                            autoDisplayLowFrisbeesMade[4] = autoLowFrisbeesMade[4] / hundred;

                            if (autoLowFrisbeesMade[4] <= 0)
                            {
                                autoLowFrisbeesMade[4] = 0;
                            }
                        }

                        else if (buttons[6] && buttons[3] && !buttons[0] && !buttons[2] && !buttons[1] && !buttons[5] && !buttons[4] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            autoLowFrisbeesMade[4]++;
                            autoDisplayLowFrisbeesMade[4] = autoLowFrisbeesMade[4] / hundred;
                        }

                        else if (buttons[6] && buttons[0] && !buttons[1] && !buttons[2] && !buttons[3] && !buttons[5] && !buttons[4] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            autoLowFrisbeesAtt[4]--;
                            autoDisplayLowFrisbeesAtt[4] = autoLowFrisbeesAtt[4] / hundred;

                            if (autoLowFrisbeesAtt[4] <= 0)
                            {
                                autoLowFrisbeesAtt[4] = 0;
                            }
                        }

                        else if (buttons[6] && buttons[2] && !buttons[0] && !buttons[1] && !buttons[3] && !buttons[5] && !buttons[4] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            autoLowFrisbeesAtt[4]++;
                            autoDisplayLowFrisbeesAtt[4] = autoLowFrisbeesAtt[4] / hundred;
                        }

                        int autoHigh5 = autoDisplayHighFrisbeesMade[4] * 6;
                        int autoMid5 = autoDisplayMidFrisbeesMade[4] * 4;
                        int autoLow5 = autoDisplayLowFrisbeesMade[4] * 2;

                        autoTotalPoints[4] = autoHigh5 + autoMid5 + autoLow5;

                    }

                    int total5 = teleOpTotalPoints[4] + autoTotalPoints[4];

                    lblTotalPoints5.Text = total5.ToString();
                }

                if (id == 3)
                {
                    // Pyramid Goals
                    lblTeleOpPyramidMade3.Text = displayPyramidFrisbeesMade[2].ToString();
                    lblTeleOpPyramidAtt3.Text = displayPyramidFrisbeesAtt[2].ToString();

                    // High Goals
                    lblTeleOpHighMade3.Text = displayHighFrisbeesMade[2].ToString();
                    lblTeleOpHighAtt3.Text = displayHighFrisbeesAtt[2].ToString();
                    lblAutoHighMade3.Text = autoDisplayHighFrisbeesMade[2].ToString();
                    lblAutoHighAtt3.Text = autoDisplayHighFrisbeesAtt[2].ToString();

                    // Mid Goals
                    lblTeleOpMidMade3.Text = displayMidFrisbeesMade[2].ToString();
                    lblTeleOpMidAtt3.Text = displayMidFrisbeesAtt[2].ToString();
                    lblAutoMidMade3.Text = autoDisplayMidFrisbeesMade[2].ToString();
                    lblAutoMidAtt3.Text = autoDisplayMidFrisbeesAtt[2].ToString();

                    // Low Goals
                    lblTeleOpLowMade3.Text = displayLowFrisbeesMade[2].ToString();
                    lblTeleOpLowAtt3.Text = displayLowFrisbeesAtt[2].ToString();
                    lblAutoLowMade3.Text = autoDisplayLowFrisbeesMade[2].ToString();
                    lblAutoLowAtt3.Text = autoDisplayLowFrisbeesAtt[2].ToString();

                    // Robot Climb
                    lblRobotClimb3.Text = robotClimb[2].ToString();

                    lblTeleOpTotalPoints3.Text = teleOpTotalPoints[2].ToString();
                    lblAutoTotalPoints3.Text = autoTotalPoints[2].ToString();

                    //Defense Rating
                    lblDefense3.Text = displayDefenseRating[2].ToString();

                    if (buttons[9] && !buttons[0] && !buttons[1] && !buttons[2] && !buttons[3] && !buttons[4] && !buttons[5] && !buttons[6] && !buttons[7] && !buttons[8] && !buttons[10] && !buttons[11] && !buttons[12])
                    {
                        lblAuto3.Visible = false;
                        lblTeleOp3.Visible = true;
                        lblAutoHighAtt3.Visible = false;
                        lblTeleOpHighAtt3.Visible = true;
                        lblAutoHighMade3.Visible = false;
                        lblTeleOpHighMade3.Visible = true;
                        lblAutoMidAtt3.Visible = false;
                        lblTeleOpMidAtt3.Visible = true;
                        lblAutoMidMade3.Visible = false;
                        lblTeleOpMidMade3.Visible = true;
                        lblAutoLowAtt3.Visible = false;
                        lblTeleOpLowAtt3.Visible = true;
                        lblAutoLowMade3.Visible = false;
                        lblTeleOpLowMade3.Visible = true;
                        lblTeleOpPyramidAtt3.Visible = true;
                        lblTeleOpPyramidMade3.Visible = true;
                        lblRobotClimb3.Visible = true;
                    }

                    if (buttons[8] && !buttons[0] && !buttons[1] && !buttons[2] && !buttons[3] && !buttons[4] && !buttons[5] && !buttons[6] && !buttons[7] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                    {
                        lblAuto3.Visible = true;
                        lblTeleOp3.Visible = false;
                        lblAutoHighAtt3.Visible = true;
                        lblTeleOpHighAtt3.Visible = false;
                        lblAutoHighMade3.Visible = true;
                        lblTeleOpHighMade3.Visible = false;
                        lblAutoMidAtt3.Visible = true;
                        lblTeleOpMidAtt3.Visible = false;
                        lblAutoMidMade3.Visible = true;
                        lblTeleOpMidMade3.Visible = false;
                        lblAutoLowAtt3.Visible = true;
                        lblTeleOpLowAtt3.Visible = false;
                        lblAutoLowMade3.Visible = true;
                        lblTeleOpLowMade3.Visible = false;
                        lblTeleOpPyramidAtt3.Visible = false;
                        lblTeleOpPyramidMade3.Visible = false;
                        lblRobotClimb3.Visible = false;
                    }



                    //The following code obtains and tells which button is each game pad is being pressed.
                    if (buttons[x])
                    {
                        strText3 += x.ToString("00 ", CultureInfo.CurrentCulture);
                        lbldisplayButtons3.Text = strText3;
                    }

                    if (lblTeleOp3.Visible == true)
                    {

                        if (buttons[10] && !buttons[0] && !buttons[1] && !buttons[2] && !buttons[3] && !buttons[4] && !buttons[5] && !buttons[6] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[11] && !buttons[12])
                        {
                            defenseRating[2]++;
                            displayDefenseRating[2] = defenseRating[2] / hundred;
                            if (displayDefenseRating[2] > 10)
                            {
                                defenseRating[2] = 0;
                                displayDefenseRating[2] = 0;
                            }
                        }

                        else if (buttons[11] && !buttons[0] && !buttons[1] && !buttons[2] && !buttons[3] && !buttons[4] && !buttons[5] && !buttons[6] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[12])
                        {
                            defenseRating[2]--;
                            displayDefenseRating[2] = defenseRating[2] / hundred;
                            if (displayDefenseRating[2] <= 0)
                            {
                                defenseRating[2] = 0;
                            }
                        }
                        // This is the code that counts how many pyramid goals were made or attempted
                        if (buttons[5] && buttons[1] && !buttons[0] && !buttons[2] && !buttons[3] && !buttons[4] && !buttons[6] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            pyramidFrisbeesMade[2]--;
                            displayPyramidFrisbeesMade[2] = pyramidFrisbeesMade[2] / hundred;

                            if (pyramidFrisbeesMade[2] <= 0)
                            {
                                pyramidFrisbeesMade[2] = 0;
                            }
                        }

                        else if (buttons[5] && buttons[3] && !buttons[0] && !buttons[2] && !buttons[1] && !buttons[4] && !buttons[6] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            pyramidFrisbeesMade[2]++;
                            displayPyramidFrisbeesMade[2] = pyramidFrisbeesMade[2] / hundred;
                        }

                        else if (buttons[5] && buttons[0] && !buttons[1] && !buttons[2] && !buttons[3] && !buttons[4] && !buttons[6] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            pyramidFrisbeesAtt[2]--;
                            displayPyramidFrisbeesAtt[2] = pyramidFrisbeesAtt[2] / hundred;

                            if (pyramidFrisbeesAtt[2] <= 0)
                            {
                                pyramidFrisbeesAtt[2] = 0;
                            }
                        }

                        else if (buttons[5] && buttons[2] && !buttons[1] && !buttons[0] && !buttons[3] && !buttons[4] && !buttons[6] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            pyramidFrisbeesAtt[2]++;
                            displayPyramidFrisbeesAtt[2] = pyramidFrisbeesAtt[2] / hundred;
                        }

                        // This code counts how many High goals were made and attempted
                        if (buttons[4] && buttons[1] && !buttons[0] && !buttons[2] && !buttons[3] && !buttons[5] && !buttons[6] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            highFrisbeesMade[2]--;
                            displayHighFrisbeesMade[2] = highFrisbeesMade[2] / hundred;

                            if (highFrisbeesMade[2] <= 0)
                            {
                                highFrisbeesMade[2] = 0;
                            }
                        }
                        else if (buttons[4] && buttons[3] && !buttons[1] && !buttons[2] && !buttons[0] && !buttons[5] && !buttons[6] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            highFrisbeesMade[2]++;
                            displayHighFrisbeesMade[2] = highFrisbeesMade[2] / hundred;
                        }

                        else if (buttons[4] && buttons[0] && !buttons[1] && !buttons[2] && !buttons[3] && !buttons[5] && !buttons[6] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            highFrisbeesAtt[2]--;
                            displayHighFrisbeesAtt[2] = highFrisbeesAtt[2] / hundred;

                            if (highFrisbeesAtt[2] <= 0)
                            {
                                highFrisbeesAtt[2] = 0;
                            }
                        }

                        else if (buttons[4] && buttons[2] && !buttons[1] && !buttons[3] && !buttons[0] && !buttons[5] && !buttons[6] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            highFrisbeesAtt[2]++;
                            displayHighFrisbeesAtt[2] = highFrisbeesAtt[2] / hundred;
                        }

                        // This code counts how many Mid goals were made and attempted
                        if (buttons[7] && buttons[1] && !buttons[2] && !buttons[3] && !buttons[4] && !buttons[5] && !buttons[6] && !buttons[0] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            midFrisbeesMade[2]--;
                            displayMidFrisbeesMade[2] = midFrisbeesMade[2] / hundred;

                            if (midFrisbeesMade[2] <= 0)
                            {
                                midFrisbeesMade[2] = 0;
                            }
                        }

                        else if (buttons[7] && buttons[3] && !buttons[1] && !buttons[2] && !buttons[0] && !buttons[5] && !buttons[6] && !buttons[4] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            midFrisbeesMade[2]++;
                            displayMidFrisbeesMade[2] = midFrisbeesMade[2] / hundred;
                        }

                        else if (buttons[7] && buttons[0] && !buttons[1] && !buttons[3] && !buttons[2] && !buttons[5] && !buttons[6] && !buttons[4] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            midFrisbeesAtt[2]--;
                            displayMidFrisbeesAtt[2] = midFrisbeesAtt[2] / hundred;

                            if (midFrisbeesAtt[2] <= 0)
                            {
                                midFrisbeesAtt[2] = 0;
                            }

                        }

                        else if (buttons[7] && buttons[2] && !buttons[1] && !buttons[3] && !buttons[0] && !buttons[5] && !buttons[6] && !buttons[4] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            midFrisbeesAtt[2]++;
                            displayMidFrisbeesAtt[2] = midFrisbeesAtt[2] / hundred;
                        }

                        // This code counts how many low goals were made and attempted
                        if (buttons[6] && buttons[1] && !buttons[2] && !buttons[3] && !buttons[0] && !buttons[5] && !buttons[4] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            lowFrisbeesMade[2]--;
                            displayLowFrisbeesMade[2] = lowFrisbeesMade[2] / hundred;

                            if (lowFrisbeesMade[2] <= 0)
                            {
                                lowFrisbeesMade[2] = 0;
                            }
                        }

                        else if (buttons[6] && buttons[3] && !buttons[1] && !buttons[2] && !buttons[0] && !buttons[5] && !buttons[4] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            lowFrisbeesMade[2]++;
                            displayLowFrisbeesMade[2] = lowFrisbeesMade[2] / hundred;
                        }

                        else if (buttons[6] && buttons[0] && !buttons[1] && !buttons[3] && !buttons[2] && !buttons[5] && !buttons[4] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            lowFrisbeesAtt[2]--;
                            displayLowFrisbeesAtt[2] = lowFrisbeesAtt[2] / hundred;

                            if (lowFrisbeesAtt[2] <= 0)
                            {
                                lowFrisbeesAtt[2] = 0;
                            }
                        }

                        else if (buttons[6] && buttons[2] && !buttons[1] && !buttons[3] && !buttons[0] && !buttons[5] && !buttons[4] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            lowFrisbeesAtt[2]++;
                            displayLowFrisbeesAtt[2] = lowFrisbeesAtt[2] / hundred;
                        }

                        // Code for the robot climb
                        if (buttons[12] && !buttons[0] && !buttons[1] && !buttons[2] && !buttons[3] && !buttons[4] && !buttons[5] && !buttons[6] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11])
                        {
                            climb[2]++;
                            robotClimb[2] = climb[2] / hundred;

                            if (robotClimb[2] > 3)
                            {
                                robotClimb[2] = 0;
                                climb[2] = 0;
                            }

                            robotClimb[2] = robotClimb[2] * 10;
                        }

                        int teleOpPyramid3 = displayPyramidFrisbeesMade[2] * 5;
                        int teleOpHigh3 = displayHighFrisbeesMade[2] * 3;
                        int teleOpMid3 = displayMidFrisbeesMade[2] * 2;
                        int teleOpLow3 = displayLowFrisbeesMade[2] * 1;

                        teleOpTotalPoints[2] = teleOpPyramid3 + teleOpHigh3 + teleOpMid3 + teleOpLow3 + robotClimb[2];

                    }

                    if (lblAuto3.Visible == true)
                    {
                        // This code counts how many High goals were made and attempted
                        if (buttons[4] && buttons[1] && !buttons[0] && !buttons[2] && !buttons[3] && !buttons[5] && !buttons[6] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            autoHighFrisbeesMade[2]--;
                            autoDisplayHighFrisbeesMade[2] = autoHighFrisbeesMade[2] / hundred;

                            if (autoHighFrisbeesMade[2] <= 0)
                            {
                                autoHighFrisbeesMade[2] = 0;
                            }
                        }
                        else if (buttons[4] && buttons[3] && !buttons[0] && !buttons[2] && !buttons[1] && !buttons[5] && !buttons[6] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            autoHighFrisbeesMade[2]++;
                            autoDisplayHighFrisbeesMade[2] = autoHighFrisbeesMade[2] / hundred;
                        }

                        else if (buttons[4] && buttons[0] && !buttons[1] && !buttons[2] && !buttons[3] && !buttons[5] && !buttons[6] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            autoHighFrisbeesAtt[2]--;
                            autoDisplayHighFrisbeesAtt[2] = autoHighFrisbeesAtt[2] / hundred;

                            if (autoHighFrisbeesAtt[2] <= 0)
                            {
                                autoHighFrisbeesAtt[2] = 0;
                            }
                        }

                        else if (buttons[4] && buttons[2] && !buttons[0] && !buttons[1] && !buttons[3] && !buttons[5] && !buttons[6] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            autoHighFrisbeesAtt[2]++;
                            autoDisplayHighFrisbeesAtt[2] = autoHighFrisbeesAtt[2] / hundred;
                        }

                        // This code counts how many Mid goals were made and attempted
                        if (buttons[7] && buttons[1] && !buttons[0] && !buttons[2] && !buttons[3] && !buttons[5] && !buttons[6] && !buttons[4] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            autoMidFrisbeesMade[2]--;
                            autoDisplayMidFrisbeesMade[2] = autoMidFrisbeesMade[2] / hundred;

                            if (autoMidFrisbeesMade[2] <= 0)
                            {
                                autoMidFrisbeesMade[2] = 0;
                            }
                        }

                        else if (buttons[7] && buttons[3] && !buttons[0] && !buttons[2] && !buttons[4] && !buttons[5] && !buttons[6] && !buttons[1] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            autoMidFrisbeesMade[2]++;
                            autoDisplayMidFrisbeesMade[2] = autoMidFrisbeesMade[2] / hundred;
                        }

                        else if (buttons[7] && buttons[0] && !buttons[1] && !buttons[2] && !buttons[3] && !buttons[5] && !buttons[6] && !buttons[4] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            autoMidFrisbeesAtt[2]--;
                            autoDisplayMidFrisbeesAtt[2] = autoMidFrisbeesAtt[2] / hundred;

                            if (autoMidFrisbeesAtt[2] <= 0)
                            {
                                autoMidFrisbeesAtt[2] = 0;
                            }
                        }

                        else if (buttons[7] && buttons[2] && !buttons[0] && !buttons[1] && !buttons[3] && !buttons[5] && !buttons[6] && !buttons[4] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            autoMidFrisbeesAtt[2]++;
                            autoDisplayMidFrisbeesAtt[2] = autoMidFrisbeesAtt[2] / hundred;
                        }

                        // This code counts how many low goals were made and attempted
                        if (buttons[6] && buttons[1] && !buttons[0] && !buttons[2] && !buttons[3] && !buttons[5] && !buttons[4] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            autoLowFrisbeesMade[2]--;
                            autoDisplayLowFrisbeesMade[2] = autoLowFrisbeesMade[2] / hundred;

                            if (autoLowFrisbeesMade[2] <= 0)
                            {
                                autoLowFrisbeesMade[2] = 0;
                            }
                        }

                        else if (buttons[6] && buttons[3] && !buttons[0] && !buttons[2] && !buttons[1] && !buttons[5] && !buttons[4] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            autoLowFrisbeesMade[2]++;
                            autoDisplayLowFrisbeesMade[2] = autoLowFrisbeesMade[2] / hundred;
                        }

                        else if (buttons[6] && buttons[0] && !buttons[1] && !buttons[2] && !buttons[3] && !buttons[5] && !buttons[4] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            autoLowFrisbeesAtt[2]--;
                            autoDisplayLowFrisbeesAtt[2] = autoLowFrisbeesAtt[2] / hundred;

                            if (autoLowFrisbeesAtt[2] <= 0)
                            {
                                autoLowFrisbeesAtt[2] = 0;
                            }
                        }

                        else if (buttons[6] && buttons[2] && !buttons[0] && !buttons[1] && !buttons[3] && !buttons[5] && !buttons[4] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            autoLowFrisbeesAtt[2]++;
                            autoDisplayLowFrisbeesAtt[2] = autoLowFrisbeesAtt[2] / hundred;
                        }

                        int autoHigh3 = autoDisplayHighFrisbeesMade[2] * 6;
                        int autoMid3 = autoDisplayMidFrisbeesMade[2] * 4;
                        int autoLow3 = autoDisplayLowFrisbeesMade[2] * 2;

                        autoTotalPoints[2] = autoHigh3 + autoMid3 + autoLow3;

                    }

                    int total3 = teleOpTotalPoints[2] + autoTotalPoints[2];

                    lblTotalPoints3.Text = total3.ToString();
                }

                if (id == 4)
                {
                    // Pyramid Goals
                    lblTeleOpPyramidMade2.Text = displayPyramidFrisbeesMade[1].ToString();
                    lblTeleOpPyramidAtt2.Text = displayPyramidFrisbeesAtt[1].ToString();

                    // High Goals
                    lblTeleOpHighMade2.Text = displayHighFrisbeesMade[1].ToString();
                    lblTeleOpHighAtt2.Text = displayHighFrisbeesAtt[1].ToString();
                    lblAutoHighMade2.Text = autoDisplayHighFrisbeesMade[1].ToString();
                    lblAutoHighAtt2.Text = autoDisplayHighFrisbeesAtt[1].ToString();

                    // Mid Goals
                    lblTeleOpMidMade2.Text = displayMidFrisbeesMade[1].ToString();
                    lblTeleOpMidAtt2.Text = displayMidFrisbeesAtt[1].ToString();
                    lblAutoMidMade2.Text = autoDisplayMidFrisbeesMade[1].ToString();
                    lblAutoMidAtt2.Text = autoDisplayMidFrisbeesAtt[1].ToString();

                    // Low Goals
                    lblTeleOpLowMade2.Text = displayLowFrisbeesMade[1].ToString();
                    lblTeleOpLowAtt2.Text = displayLowFrisbeesAtt[1].ToString();
                    lblAutoLowMade2.Text = autoDisplayLowFrisbeesMade[1].ToString();
                    lblAutoLowAtt2.Text = autoDisplayLowFrisbeesAtt[1].ToString();

                    // Robot Climb
                    lblRobotClimb2.Text = robotClimb[1].ToString();

                    lblTeleOpTotalPoints2.Text = teleOpTotalPoints[1].ToString();
                    lblAutoTotalPoints2.Text = autoTotalPoints[1].ToString();

                    //Defense Rating
                    lblDefense2.Text = displayDefenseRating[1].ToString();

                    if (buttons[9] && !buttons[0] && !buttons[1] && !buttons[2] && !buttons[3] && !buttons[4] && !buttons[5] && !buttons[6] && !buttons[7] && !buttons[8] && !buttons[10] && !buttons[11] && !buttons[12])
                    {
                        lblAuto2.Visible = false;
                        lblTeleOp2.Visible = true;
                        lblAutoHighAtt2.Visible = false;
                        lblTeleOpHighAtt2.Visible = true;
                        lblAutoHighMade2.Visible = false;
                        lblTeleOpHighMade2.Visible = true;
                        lblAutoMidAtt2.Visible = false;
                        lblTeleOpMidAtt2.Visible = true;
                        lblAutoMidMade2.Visible = false;
                        lblTeleOpMidMade2.Visible = true;
                        lblAutoLowAtt2.Visible = false;
                        lblTeleOpLowAtt2.Visible = true;
                        lblAutoLowMade2.Visible = false;
                        lblTeleOpLowMade2.Visible = true;
                        lblTeleOpPyramidAtt2.Visible = true;
                        lblTeleOpPyramidMade2.Visible = true;
                        lblRobotClimb2.Visible = true;
                    }

                    if (buttons[8] && !buttons[0] && !buttons[1] && !buttons[2] && !buttons[3] && !buttons[4] && !buttons[5] && !buttons[6] && !buttons[7] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                    {
                        lblAuto2.Visible = true;
                        lblTeleOp2.Visible = false;
                        lblAutoHighAtt2.Visible = true;
                        lblTeleOpHighAtt2.Visible = false;
                        lblAutoHighMade2.Visible = true;
                        lblTeleOpHighMade2.Visible = false;
                        lblAutoMidAtt2.Visible = true;
                        lblTeleOpMidAtt2.Visible = false;
                        lblAutoMidMade2.Visible = true;
                        lblTeleOpMidMade2.Visible = false;
                        lblAutoLowAtt2.Visible = true;
                        lblTeleOpLowAtt2.Visible = false;
                        lblAutoLowMade2.Visible = true;
                        lblTeleOpLowMade2.Visible = false;
                        lblTeleOpPyramidAtt2.Visible = false;
                        lblTeleOpPyramidMade2.Visible = false;
                        lblRobotClimb2.Visible = false;
                    }



                    //The following code obtains and tells which button is each game pad is being pressed.
                    if (buttons[x])
                    {
                        strText2 += x.ToString("00 ", CultureInfo.CurrentCulture);
                        lbldisplayButtons2.Text = strText2;
                    }

                    if (lblTeleOp2.Visible == true)
                    {
                        if (buttons[10] && !buttons[0] && !buttons[1] && !buttons[2] && !buttons[3] && !buttons[4] && !buttons[5] && !buttons[6] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[11] && !buttons[12])
                        {
                            defenseRating[1]++;
                            displayDefenseRating[1] = defenseRating[1] / hundred;
                            if (displayDefenseRating[1] > 10)
                            {
                                defenseRating[1] = 0;
                                displayDefenseRating[1] = 0;
                            }
                        }

                        else if (buttons[11] && !buttons[0] && !buttons[1] && !buttons[2] && !buttons[3] && !buttons[4] && !buttons[5] && !buttons[6] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[12])
                        {
                            defenseRating[1]--;
                            displayDefenseRating[1] = defenseRating[1] / hundred;
                            if (displayDefenseRating[1] <= 0)
                            {
                                defenseRating[1] = 0;
                            }
                        }


                        // This is the code that counts how many pyramid goals were made or attempted
                        // This is the code that counts how many pyramid goals were made or attempted
                        if (buttons[5] && buttons[1] && !buttons[0] && !buttons[2] && !buttons[3] && !buttons[4] && !buttons[6] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            pyramidFrisbeesMade[1]--;
                            displayPyramidFrisbeesMade[1] = pyramidFrisbeesMade[1] / hundred;

                            if (pyramidFrisbeesMade[1] <= 0)
                            {
                                pyramidFrisbeesMade[1] = 0;
                            }
                        }

                        else if (buttons[5] && buttons[3] && !buttons[0] && !buttons[2] && !buttons[1] && !buttons[4] && !buttons[6] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            pyramidFrisbeesMade[1]++;
                            displayPyramidFrisbeesMade[1] = pyramidFrisbeesMade[1] / hundred;
                        }

                        else if (buttons[5] && buttons[0] && !buttons[1] && !buttons[2] && !buttons[3] && !buttons[4] && !buttons[6] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            pyramidFrisbeesAtt[1]--;
                            displayPyramidFrisbeesAtt[1] = pyramidFrisbeesAtt[1] / hundred;

                            if (pyramidFrisbeesAtt[1] <= 0)
                            {
                                pyramidFrisbeesAtt[1] = 0;
                            }
                        }

                        else if (buttons[5] && buttons[2] && !buttons[1] && !buttons[0] && !buttons[3] && !buttons[4] && !buttons[6] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            pyramidFrisbeesAtt[1]++;
                            displayPyramidFrisbeesAtt[1] = pyramidFrisbeesAtt[1] / hundred;
                        }

                        // This code counts how many High goals were made and attempted
                        if (buttons[4] && buttons[1] && !buttons[0] && !buttons[2] && !buttons[3] && !buttons[5] && !buttons[6] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            highFrisbeesMade[1]--;
                            displayHighFrisbeesMade[1] = highFrisbeesMade[1] / hundred;

                            if (highFrisbeesMade[1] <= 0)
                            {
                                highFrisbeesMade[1] = 0;
                            }
                        }
                        else if (buttons[4] && buttons[3] && !buttons[1] && !buttons[2] && !buttons[0] && !buttons[5] && !buttons[6] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            highFrisbeesMade[1]++;
                            displayHighFrisbeesMade[1] = highFrisbeesMade[1] / hundred;
                        }

                        else if (buttons[4] && buttons[0] && !buttons[1] && !buttons[2] && !buttons[3] && !buttons[5] && !buttons[6] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            highFrisbeesAtt[1]--;
                            displayHighFrisbeesAtt[1] = highFrisbeesAtt[1] / hundred;

                            if (highFrisbeesAtt[1] <= 0)
                            {
                                highFrisbeesAtt[1] = 0;
                            }
                        }

                        else if (buttons[4] && buttons[2] && !buttons[1] && !buttons[3] && !buttons[0] && !buttons[5] && !buttons[6] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            highFrisbeesAtt[1]++;
                            displayHighFrisbeesAtt[1] = highFrisbeesAtt[1] / hundred;
                        }

                        // This code counts how many Mid goals were made and attempted
                        if (buttons[7] && buttons[1] && !buttons[2] && !buttons[3] && !buttons[4] && !buttons[5] && !buttons[6] && !buttons[0] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            midFrisbeesMade[1]--;
                            displayMidFrisbeesMade[1] = midFrisbeesMade[1] / hundred;

                            if (midFrisbeesMade[1] <= 0)
                            {
                                midFrisbeesMade[1] = 0;
                            }
                        }

                        else if (buttons[7] && buttons[3] && !buttons[1] && !buttons[2] && !buttons[0] && !buttons[5] && !buttons[6] && !buttons[4] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            midFrisbeesMade[1]++;
                            displayMidFrisbeesMade[1] = midFrisbeesMade[1] / hundred;
                        }

                        else if (buttons[7] && buttons[0] && !buttons[1] && !buttons[3] && !buttons[2] && !buttons[5] && !buttons[6] && !buttons[4] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            midFrisbeesAtt[1]--;
                            displayMidFrisbeesAtt[1] = midFrisbeesAtt[1] / hundred;

                            if (midFrisbeesAtt[1] <= 0)
                            {
                                midFrisbeesAtt[1] = 0;
                            }
                        }

                        else if (buttons[7] && buttons[2] && !buttons[1] && !buttons[3] && !buttons[0] && !buttons[5] && !buttons[6] && !buttons[4] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            midFrisbeesAtt[1]++;
                            displayMidFrisbeesAtt[1] = midFrisbeesAtt[1] / hundred;
                        }

                        // This code counts how many low goals were made and attempted
                        if (buttons[6] && buttons[1] && !buttons[2] && !buttons[3] && !buttons[0] && !buttons[5] && !buttons[4] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            lowFrisbeesMade[1]--;
                            displayLowFrisbeesMade[1] = lowFrisbeesMade[1] / hundred;

                            if (lowFrisbeesMade[1] <= 0)
                            {
                                lowFrisbeesMade[1] = 0;
                            }
                        }

                        else if (buttons[6] && buttons[3] && !buttons[1] && !buttons[2] && !buttons[0] && !buttons[5] && !buttons[4] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            lowFrisbeesMade[1]++;
                            displayLowFrisbeesMade[1] = lowFrisbeesMade[1] / hundred;
                        }

                        else if (buttons[6] && buttons[0] && !buttons[1] && !buttons[3] && !buttons[2] && !buttons[5] && !buttons[4] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            lowFrisbeesAtt[1]--;
                            displayLowFrisbeesAtt[1] = lowFrisbeesAtt[1] / hundred;

                            if (lowFrisbeesAtt[1] <= 0)
                            {
                                lowFrisbeesAtt[1] = 0;
                            }
                        }

                        else if (buttons[6] && buttons[2] && !buttons[1] && !buttons[3] && !buttons[0] && !buttons[5] && !buttons[4] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            lowFrisbeesAtt[1]++;
                            displayLowFrisbeesAtt[1] = lowFrisbeesAtt[1] / hundred;
                        }

                        // Code for the robot climb
                        if (buttons[12] && !buttons[0] && !buttons[1] && !buttons[2] && !buttons[3] && !buttons[4] && !buttons[5] && !buttons[6] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11])
                        {
                            climb[1]++;
                            robotClimb[1] = climb[1] / hundred;

                            if (robotClimb[1] > 3)
                            {
                                robotClimb[1] = 0;
                                climb[1] = 0;
                            }

                            robotClimb[1] = robotClimb[1] * 10;
                        }

                        int teleOpPyramid2 = displayPyramidFrisbeesMade[1] * 5;
                        int teleOpHigh2 = displayHighFrisbeesMade[1] * 3;
                        int teleOpMid2 = displayMidFrisbeesMade[1] * 2;
                        int teleOpLow2 = displayLowFrisbeesMade[1] * 1;

                        teleOpTotalPoints[1] = teleOpPyramid2 + teleOpHigh2 + teleOpMid2 + teleOpLow2 + robotClimb[1];

                    }

                    if (lblAuto2.Visible == true)
                    {
                        // This code counts how many High goals were made and attempted
                        if (buttons[4] && buttons[1] && !buttons[0] && !buttons[2] && !buttons[3] && !buttons[5] && !buttons[6] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            autoHighFrisbeesMade[1]--;
                            autoDisplayHighFrisbeesMade[1] = autoHighFrisbeesMade[1] / hundred;

                            if (autoHighFrisbeesMade[1] <= 0)
                            {
                                autoHighFrisbeesMade[1] = 0;
                            }
                        }
                        else if (buttons[4] && buttons[3] && !buttons[0] && !buttons[2] && !buttons[1] && !buttons[5] && !buttons[6] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            autoHighFrisbeesMade[1]++;
                            autoDisplayHighFrisbeesMade[1] = autoHighFrisbeesMade[1] / hundred;
                        }

                        else if (buttons[4] && buttons[0] && !buttons[1] && !buttons[2] && !buttons[3] && !buttons[5] && !buttons[6] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            autoHighFrisbeesAtt[1]--;
                            autoDisplayHighFrisbeesAtt[1] = autoHighFrisbeesAtt[1] / hundred;

                            if (autoHighFrisbeesAtt[1] <= 0)
                            {
                                autoHighFrisbeesAtt[1] = 0;
                            }
                        }

                        else if (buttons[4] && buttons[2] && !buttons[0] && !buttons[1] && !buttons[3] && !buttons[5] && !buttons[6] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            autoHighFrisbeesAtt[1]++;
                            autoDisplayHighFrisbeesAtt[1] = autoHighFrisbeesAtt[1] / hundred;
                        }

                        // This code counts how many Mid goals were made and attempted
                        if (buttons[7] && buttons[1] && !buttons[0] && !buttons[2] && !buttons[3] && !buttons[5] && !buttons[6] && !buttons[4] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            autoMidFrisbeesMade[1]--;
                            autoDisplayMidFrisbeesMade[1] = autoMidFrisbeesMade[1] / hundred;

                            if (autoMidFrisbeesMade[1] <= 0)
                            {
                                autoMidFrisbeesMade[1] = 0;
                            }
                        }

                        else if (buttons[7] && buttons[3] && !buttons[0] && !buttons[2] && !buttons[4] && !buttons[5] && !buttons[6] && !buttons[1] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            autoMidFrisbeesMade[1]++;
                            autoDisplayMidFrisbeesMade[1] = autoMidFrisbeesMade[1] / hundred;
                        }

                        else if (buttons[7] && buttons[0] && !buttons[1] && !buttons[2] && !buttons[3] && !buttons[5] && !buttons[6] && !buttons[4] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            autoMidFrisbeesAtt[1]--;
                            autoDisplayMidFrisbeesAtt[1] = autoMidFrisbeesAtt[1] / hundred;

                            if (autoMidFrisbeesAtt[1] <= 0)
                            {
                                autoMidFrisbeesAtt[1] = 0;
                            }
                        }

                        else if (buttons[7] && buttons[2] && !buttons[0] && !buttons[1] && !buttons[3] && !buttons[5] && !buttons[6] && !buttons[4] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            autoMidFrisbeesAtt[1]++;
                            autoDisplayMidFrisbeesAtt[1] = autoMidFrisbeesAtt[1] / hundred;
                        }

                        // This code counts how many low goals were made and attempted
                        if (buttons[6] && buttons[1] && !buttons[0] && !buttons[2] && !buttons[3] && !buttons[5] && !buttons[4] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            autoLowFrisbeesMade[1]--;
                            autoDisplayLowFrisbeesMade[1] = autoLowFrisbeesMade[1] / hundred;

                            if (autoLowFrisbeesMade[1] <= 0)
                            {
                                autoLowFrisbeesMade[1] = 0;
                            }
                        }

                        else if (buttons[6] && buttons[3] && !buttons[0] && !buttons[2] && !buttons[1] && !buttons[5] && !buttons[4] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            autoLowFrisbeesMade[1]++;
                            autoDisplayLowFrisbeesMade[1] = autoLowFrisbeesMade[1] / hundred;
                        }

                        else if (buttons[6] && buttons[0] && !buttons[1] && !buttons[2] && !buttons[3] && !buttons[5] && !buttons[4] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            autoLowFrisbeesAtt[1]--;
                            autoDisplayLowFrisbeesAtt[1] = autoLowFrisbeesAtt[1] / hundred;

                            if (autoLowFrisbeesAtt[1] <= 0)
                            {
                                autoLowFrisbeesAtt[1] = 0;
                            }
                        }

                        else if (buttons[6] && buttons[2] && !buttons[0] && !buttons[1] && !buttons[3] && !buttons[5] && !buttons[4] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            autoLowFrisbeesAtt[1]++;
                            autoDisplayLowFrisbeesAtt[1] = autoLowFrisbeesAtt[1] / hundred;
                        }

                        int autoHigh2 = autoDisplayHighFrisbeesMade[1] * 6;
                        int autoMid2 = autoDisplayMidFrisbeesMade[1] * 4;
                        int autoLow2 = autoDisplayLowFrisbeesMade[1] * 2;

                        autoTotalPoints[1] = autoHigh2 + autoMid2 + autoLow2;

                    }

                    int total2 = teleOpTotalPoints[1] + autoTotalPoints[1];

                    lblTotalPoints2.Text = total2.ToString();
                }

                if (id == 5)
                {
                    // Pyramid Goals
                    lblTeleOpPyramidMade4.Text = displayPyramidFrisbeesMade[3].ToString();
                    lblTeleOpPyramidAtt4.Text = displayPyramidFrisbeesAtt[3].ToString();

                    // High Goals
                    lblTeleOpHighMade4.Text = displayHighFrisbeesMade[3].ToString();
                    lblTeleOpHighAtt4.Text = displayHighFrisbeesAtt[3].ToString();
                    lblAutoHighMade4.Text = autoDisplayHighFrisbeesMade[3].ToString();
                    lblAutoHighAtt4.Text = autoDisplayHighFrisbeesAtt[3].ToString();

                    // Mid Goals
                    lblTeleOpMidMade4.Text = displayMidFrisbeesMade[3].ToString();
                    lblTeleOpMidAtt4.Text = displayMidFrisbeesAtt[3].ToString();
                    lblAutoMidMade4.Text = autoDisplayMidFrisbeesMade[3].ToString();
                    lblAutoMidAtt4.Text = autoDisplayMidFrisbeesAtt[3].ToString();

                    // Low Goals
                    lblTeleOpLowMade4.Text = displayLowFrisbeesMade[3].ToString();
                    lblTeleOpLowAtt4.Text = displayLowFrisbeesAtt[3].ToString();
                    lblAutoLowMade4.Text = autoDisplayLowFrisbeesMade[3].ToString();
                    lblAutoLowAtt4.Text = autoDisplayLowFrisbeesAtt[3].ToString();

                    // Robot Climb
                    lblRobotClimb4.Text = robotClimb[3].ToString();

                    lblTeleOpTotalPoints4.Text = teleOpTotalPoints[3].ToString();
                    lblAutoTotalPoints4.Text = autoTotalPoints[3].ToString();

                    //Defense Rating
                    lblDefense4.Text = displayDefenseRating[3].ToString();

                    

                    if (buttons[9] && !buttons[0] && !buttons[1] && !buttons[2] && !buttons[3] && !buttons[4] && !buttons[5] && !buttons[6] && !buttons[7] && !buttons[8] && !buttons[10] && !buttons[11] && !buttons[12])
                    {
                        lblAuto4.Visible = false;
                        lblTeleOp4.Visible = true;
                        lblAutoHighAtt4.Visible = false;
                        lblTeleOpHighAtt4.Visible = true;
                        lblAutoHighMade4.Visible = false;
                        lblTeleOpHighMade4.Visible = true;
                        lblAutoMidAtt4.Visible = false;
                        lblTeleOpMidAtt4.Visible = true;
                        lblAutoMidMade4.Visible = false;
                        lblTeleOpMidMade4.Visible = true;
                        lblAutoLowAtt4.Visible = false;
                        lblTeleOpLowAtt4.Visible = true;
                        lblAutoLowMade4.Visible = false;
                        lblTeleOpLowMade4.Visible = true;
                        lblTeleOpPyramidAtt4.Visible = true;
                        lblTeleOpPyramidMade4.Visible = true;
                        lblRobotClimb4.Visible = true;
                    }

                    if (buttons[8] && !buttons[0] && !buttons[1] && !buttons[2] && !buttons[3] && !buttons[4] && !buttons[5] && !buttons[6] && !buttons[7] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                    {
                        lblAuto4.Visible = true;
                        lblTeleOp4.Visible = false;
                        lblAutoHighAtt4.Visible = true;
                        lblTeleOpHighAtt4.Visible = false;
                        lblAutoHighMade4.Visible = true;
                        lblTeleOpHighMade4.Visible = false;
                        lblAutoMidAtt4.Visible = true;
                        lblTeleOpMidAtt4.Visible = false;
                        lblAutoMidMade4.Visible = true;
                        lblTeleOpMidMade4.Visible = false;
                        lblAutoLowAtt4.Visible = true;
                        lblTeleOpLowAtt4.Visible = false;
                        lblAutoLowMade4.Visible = true;
                        lblTeleOpLowMade4.Visible = false;
                        lblTeleOpPyramidAtt4.Visible = false;
                        lblTeleOpPyramidMade4.Visible = false;
                        lblRobotClimb4.Visible = false;
                    }



                    //The following code obtains and tells which button is each game pad is being pressed.
                    if (buttons[x])
                    {
                        strText4 += x.ToString("00 ", CultureInfo.CurrentCulture);
                        lbldisplayButtons4.Text = strText4;
                    }

                    if (lblTeleOp4.Visible == true)
                    {

                        if (buttons[10] && !buttons[0] && !buttons[1] && !buttons[2] && !buttons[3] && !buttons[4] && !buttons[5] && !buttons[6] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[11] && !buttons[12])
                        {
                            defenseRating[3]++;
                            displayDefenseRating[3] = defenseRating[3] / hundred;
                            if (displayDefenseRating[3] > 10)
                            {
                                defenseRating[3] = 0;
                                displayDefenseRating[3] = 0;
                            }
                        }

                        else if (buttons[11] && !buttons[0] && !buttons[1] && !buttons[2] && !buttons[3] && !buttons[4] && !buttons[5] && !buttons[6] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[12])
                        {
                            defenseRating[3]--;
                            displayDefenseRating[3] = defenseRating[1] / hundred;
                            if (displayDefenseRating[3] <= 0)
                            {
                                defenseRating[3] = 0;
                            }
                        }
                        // This is the code that counts how many pyramid goals were made or attempted
                        if (buttons[5] && buttons[1] && !buttons[0] && !buttons[2] && !buttons[3] && !buttons[4] && !buttons[6] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            pyramidFrisbeesMade[3]--;
                            displayPyramidFrisbeesMade[3] = pyramidFrisbeesMade[3] / hundred;

                            if (pyramidFrisbeesMade[3] <= 0)
                            {
                                pyramidFrisbeesMade[3] = 0;
                            }
                        }

                        else if (buttons[5] && buttons[3] && !buttons[0] && !buttons[2] && !buttons[1] && !buttons[4] && !buttons[6] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            pyramidFrisbeesMade[3]++;
                            displayPyramidFrisbeesMade[3] = pyramidFrisbeesMade[3] / hundred;
                        }

                        else if (buttons[5] && buttons[0] && !buttons[1] && !buttons[2] && !buttons[3] && !buttons[4] && !buttons[6] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            pyramidFrisbeesAtt[3]--;
                            displayPyramidFrisbeesAtt[3] = pyramidFrisbeesAtt[3] / hundred;

                            if (pyramidFrisbeesAtt[3] <= 0)
                            {
                                pyramidFrisbeesAtt[3] = 0;
                            }
                        }

                        else if (buttons[5] && buttons[2] && !buttons[1] && !buttons[0] && !buttons[3] && !buttons[4] && !buttons[6] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            pyramidFrisbeesAtt[3]++;
                            displayPyramidFrisbeesAtt[3] = pyramidFrisbeesAtt[3] / hundred;
                        }

                        // This code counts how many High goals were made and attempted
                        if (buttons[4] && buttons[1] && !buttons[0] && !buttons[2] && !buttons[3] && !buttons[5] && !buttons[6] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            highFrisbeesMade[3]--;
                            displayHighFrisbeesMade[3] = highFrisbeesMade[3] / hundred;

                            if (highFrisbeesMade[3] <= 0)
                            {
                                highFrisbeesMade[3] = 0;
                            }
                        }
                        else if (buttons[4] && buttons[3] && !buttons[1] && !buttons[2] && !buttons[0] && !buttons[5] && !buttons[6] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            highFrisbeesMade[3]++;
                            displayHighFrisbeesMade[3] = highFrisbeesMade[3] / hundred;
                        }

                        else if (buttons[4] && buttons[0] && !buttons[1] && !buttons[2] && !buttons[3] && !buttons[5] && !buttons[6] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            highFrisbeesAtt[3]--;
                            displayHighFrisbeesAtt[3] = highFrisbeesAtt[3] / hundred;

                            if (highFrisbeesAtt[3] <= 0)
                            {
                                highFrisbeesAtt[3] = 0;
                            }
                        }

                        else if (buttons[4] && buttons[2] && !buttons[1] && !buttons[3] && !buttons[0] && !buttons[5] && !buttons[6] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            highFrisbeesAtt[3]++;
                            displayHighFrisbeesAtt[3] = highFrisbeesAtt[3] / hundred;
                        }

                        // This code counts how many Mid goals were made and attempted
                        if (buttons[7] && buttons[1] && !buttons[2] && !buttons[3] && !buttons[4] && !buttons[5] && !buttons[6] && !buttons[0] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            midFrisbeesMade[3]--;
                            displayMidFrisbeesMade[3] = midFrisbeesMade[3] / hundred;

                            if (midFrisbeesMade[3] <= 0)
                            {
                                midFrisbeesMade[3] = 0;
                            }
                        }

                        else if (buttons[7] && buttons[3] && !buttons[1] && !buttons[2] && !buttons[0] && !buttons[5] && !buttons[6] && !buttons[4] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            midFrisbeesMade[3]++;
                            displayMidFrisbeesMade[3] = midFrisbeesMade[3] / hundred;
                        }

                        else if (buttons[7] && buttons[0] && !buttons[1] && !buttons[3] && !buttons[2] && !buttons[5] && !buttons[6] && !buttons[4] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            midFrisbeesAtt[3]--;
                            displayMidFrisbeesAtt[3] = midFrisbeesAtt[3] / hundred;
                            if (midFrisbeesAtt[3] <= 0)
                            {
                                midFrisbeesAtt[3] = 0;
                            }
                        }

                        else if (buttons[7] && buttons[2] && !buttons[1] && !buttons[3] && !buttons[0] && !buttons[5] && !buttons[6] && !buttons[4] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            midFrisbeesAtt[3]++;
                            displayMidFrisbeesAtt[3] = midFrisbeesAtt[3] / hundred;
                        }

                        // This code counts how many low goals were made and attempted
                        if (buttons[6] && buttons[1] && !buttons[2] && !buttons[3] && !buttons[0] && !buttons[5] && !buttons[4] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            lowFrisbeesMade[3]--;
                            displayLowFrisbeesMade[3] = lowFrisbeesMade[3] / hundred;

                            if (lowFrisbeesMade[3] <= 0)
                            {
                                lowFrisbeesMade[3] = 0;
                            }
                        }

                        else if (buttons[6] && buttons[3] && !buttons[1] && !buttons[2] && !buttons[0] && !buttons[5] && !buttons[4] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            lowFrisbeesMade[3]++;
                            displayLowFrisbeesMade[3] = lowFrisbeesMade[3] / hundred;
                        }

                        else if (buttons[6] && buttons[0] && !buttons[1] && !buttons[3] && !buttons[2] && !buttons[5] && !buttons[4] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            lowFrisbeesAtt[3]--;
                            displayLowFrisbeesAtt[3] = lowFrisbeesAtt[3] / hundred;

                            if (lowFrisbeesAtt[3] <= 0)
                            {
                                lowFrisbeesAtt[3] = 0;
                            }
                        }

                        else if (buttons[6] && buttons[2] && !buttons[1] && !buttons[3] && !buttons[0] && !buttons[5] && !buttons[4] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            lowFrisbeesAtt[3]++;
                            displayLowFrisbeesAtt[3] = lowFrisbeesAtt[3] / hundred;
                        }

                        // Code for the robot climb
                        if (buttons[12] && !buttons[0] && !buttons[1] && !buttons[2] && !buttons[3] && !buttons[4] && !buttons[5] && !buttons[6] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11])
                        {
                            climb[3]++;
                            robotClimb[3] = climb[3] / hundred;

                            if (robotClimb[3] > 3)
                            {
                                robotClimb[3] = 0;
                                climb[3] = 0;
                            }

                            robotClimb[3] = robotClimb[3] * 10;
                        }

                        int teleOpPyramid4 = displayPyramidFrisbeesMade[3] * 5;
                        int teleOpHigh4 = displayHighFrisbeesMade[3] * 3;
                        int teleOpMid4 = displayMidFrisbeesMade[3] * 2;
                        int teleOpLow4 = displayLowFrisbeesMade[3] * 1;

                        teleOpTotalPoints[3] = teleOpPyramid4 + teleOpHigh4 + teleOpMid4 + teleOpLow4 + robotClimb[3];

                    }

                    if (lblAuto4.Visible == true)
                    {
                        // This code counts how many High goals were made and attempted
                        if (buttons[4] && buttons[1] && !buttons[0] && !buttons[2] && !buttons[3] && !buttons[5] && !buttons[6] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            autoHighFrisbeesMade[3]--;
                            autoDisplayHighFrisbeesMade[3] = autoHighFrisbeesMade[3] / hundred;

                            if (autoHighFrisbeesMade[3] <= 0)
                            {
                                autoHighFrisbeesMade[3] = 0;
                            }
                        }
                        else if (buttons[4] && buttons[3] && !buttons[0] && !buttons[2] && !buttons[1] && !buttons[5] && !buttons[6] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            autoHighFrisbeesMade[3]++;
                            autoDisplayHighFrisbeesMade[3] = autoHighFrisbeesMade[3] / hundred;
                        }

                        else if (buttons[4] && buttons[0] && !buttons[1] && !buttons[2] && !buttons[3] && !buttons[5] && !buttons[6] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            autoHighFrisbeesAtt[3]--;
                            autoDisplayHighFrisbeesAtt[3] = autoHighFrisbeesAtt[3] / hundred;

                            if (autoHighFrisbeesAtt[3] <= 0)
                            {
                                autoHighFrisbeesAtt[3] = 0;
                            }
                        }

                        else if (buttons[4] && buttons[2] && !buttons[0] && !buttons[1] && !buttons[3] && !buttons[5] && !buttons[6] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            autoHighFrisbeesAtt[3]++;
                            autoDisplayHighFrisbeesAtt[3] = autoHighFrisbeesAtt[3] / hundred;
                        }

                        // This code counts how many Mid goals were made and attempted
                        if (buttons[7] && buttons[1] && !buttons[0] && !buttons[2] && !buttons[3] && !buttons[5] && !buttons[6] && !buttons[4] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            autoMidFrisbeesMade[3]--;
                            autoDisplayMidFrisbeesMade[3] = autoMidFrisbeesMade[3] / hundred;

                            if (autoMidFrisbeesMade[3] <= 0)
                            {
                                autoMidFrisbeesMade[3] = 0;
                            }
                        }

                        else if (buttons[7] && buttons[3] && !buttons[0] && !buttons[2] && !buttons[4] && !buttons[5] && !buttons[6] && !buttons[1] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            autoMidFrisbeesMade[3]++;
                            autoDisplayMidFrisbeesMade[3] = autoMidFrisbeesMade[3] / hundred;
                        }

                        else if (buttons[7] && buttons[0] && !buttons[1] && !buttons[2] && !buttons[3] && !buttons[5] && !buttons[6] && !buttons[4] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            autoMidFrisbeesAtt[3]--;
                            autoDisplayMidFrisbeesAtt[3] = autoMidFrisbeesAtt[3] / hundred;

                            if (autoMidFrisbeesAtt[3] <= 0)
                            {
                                autoMidFrisbeesAtt[3] = 0;
                            }
                        }

                        else if (buttons[7] && buttons[2] && !buttons[0] && !buttons[1] && !buttons[3] && !buttons[5] && !buttons[6] && !buttons[4] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            autoMidFrisbeesAtt[3]++;
                            autoDisplayMidFrisbeesAtt[3] = autoMidFrisbeesAtt[3] / hundred;
                        }

                        // This code counts how many low goals were made and attempted
                        if (buttons[6] && buttons[1] && !buttons[0] && !buttons[2] && !buttons[3] && !buttons[5] && !buttons[4] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            autoLowFrisbeesMade[3]--;
                            autoDisplayLowFrisbeesMade[3] = autoLowFrisbeesMade[3] / hundred;

                            if (autoLowFrisbeesMade[3] <= 0)
                            {
                                autoLowFrisbeesMade[3] = 0;
                            }

                        }

                        else if (buttons[6] && buttons[3] && !buttons[0] && !buttons[2] && !buttons[1] && !buttons[5] && !buttons[4] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            autoLowFrisbeesMade[3]++;
                            autoDisplayLowFrisbeesMade[3] = autoLowFrisbeesMade[3] / hundred;
                        }

                        else if (buttons[6] && buttons[0] && !buttons[1] && !buttons[2] && !buttons[3] && !buttons[5] && !buttons[4] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            autoLowFrisbeesAtt[3]--;
                            autoDisplayLowFrisbeesAtt[3] = autoLowFrisbeesAtt[3] / hundred;

                            if (autoLowFrisbeesAtt[3] <= 0)
                            {
                                autoLowFrisbeesAtt[3] = 0;
                            }
                        }

                        else if (buttons[6] && buttons[2] && !buttons[0] && !buttons[1] && !buttons[3] && !buttons[5] && !buttons[4] && !buttons[7] && !buttons[8] && !buttons[9] && !buttons[10] && !buttons[11] && !buttons[12])
                        {
                            autoLowFrisbeesAtt[3]++;
                            autoDisplayLowFrisbeesAtt[3] = autoLowFrisbeesAtt[3] / hundred;
                        }

                        int autoHigh4 = autoDisplayHighFrisbeesMade[3] * 6;
                        int autoMid4 = autoDisplayMidFrisbeesMade[3] * 4;
                        int autoLow4 = autoDisplayLowFrisbeesMade[3] * 2;

                        autoTotalPoints[3] = autoHigh4 + autoMid4 + autoLow4;

                    }

                    int total4 = teleOpTotalPoints[3] + autoTotalPoints[3];

                    lblTotalPoints4.Text = total4.ToString(); 
                }
            }
        }
            
        

        private void button1_Click_1(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "txt files (*.txt)|*.txt";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                using (Stream s = File.Open(saveFileDialog1.FileName, FileMode.CreateNew))
                using (StreamWriter sw = new StreamWriter(s))
                {
                   MessageBox.Show(saveFileDialog1.FileName);
                   fileName = saveFileDialog1.FileName;
                    // Code to write the stream goes here.
                    sw.WriteLine(lblAutoTeamNo1.Text + x + lblAutoHighMade.Text + x + lblAutoHighAtt.Text + x + lblAutoMidMade.Text + x + lblAutoMidAtt.Text + x + lblAutoLowMade.Text + x + lblAutoLowAtt.Text + x + lblAutoTotalPoints.Text + x + lblTeleOpPyramidMade.Text + x + lblTeleOpPyramidAtt.Text + x + lblTeleOpHighMade.Text + x + lblTeleOpHighAtt.Text + x + lblTeleOpMidMade.Text + x + lblTeleOpMidAtt.Text + x + lblTeleOpLowMade.Text + x + lblTeleOpLowAtt.Text + x + lblRobotClimb.Text + x + lblTeleOpTotalPoints.Text + x + lblTotalPoints.Text + x + lblDefense.Text);
                    sw.WriteLine(lblAutoTeamNo2.Text + x + lblAutoHighMade2.Text + x + lblAutoHighAtt2.Text + x + lblAutoMidMade2.Text + x + lblAutoMidAtt2.Text + x + lblAutoLowMade2.Text + x + lblAutoLowAtt2.Text + x + lblAutoTotalPoints2.Text + x + lblTeleOpPyramidMade2.Text + x + lblTeleOpPyramidAtt2.Text + x + lblTeleOpHighMade2.Text + x + lblTeleOpHighAtt2.Text + x + lblTeleOpMidMade2.Text + x + lblTeleOpMidAtt2.Text + x + lblTeleOpLowMade2.Text + x + lblTeleOpLowAtt2.Text + x + lblRobotClimb2.Text + x + lblTeleOpTotalPoints2.Text + x + lblTotalPoints2.Text + x + lblDefense2.Text);
                    sw.WriteLine(lblAutoTeamNo3.Text + x + lblAutoHighMade3.Text + x + lblAutoHighAtt3.Text + x + lblAutoMidMade3.Text + x + lblAutoMidAtt3.Text + x + lblAutoLowMade3.Text + x + lblAutoLowAtt3.Text + x + lblAutoTotalPoints3.Text + x + lblTeleOpPyramidMade3.Text + x + lblTeleOpPyramidAtt3.Text + x + lblTeleOpHighMade3.Text + x + lblTeleOpHighAtt3.Text + x + lblTeleOpMidMade3.Text + x + lblTeleOpMidAtt3.Text + x + lblTeleOpLowMade3.Text + x + lblTeleOpLowAtt3.Text + x + lblRobotClimb3.Text + x + lblTeleOpTotalPoints3.Text + x + lblTotalPoints3.Text + x + lblDefense3.Text);
                    sw.WriteLine(lblAutoTeamNo4.Text + x + lblAutoHighMade4.Text + x + lblAutoHighAtt4.Text + x + lblAutoMidMade4.Text + x + lblAutoMidAtt4.Text + x + lblAutoLowMade4.Text + x + lblAutoLowAtt4.Text + x + lblAutoTotalPoints4.Text + x + lblTeleOpPyramidMade4.Text + x + lblTeleOpPyramidAtt4.Text + x + lblTeleOpHighMade4.Text + x + lblTeleOpHighAtt4.Text + x + lblTeleOpMidMade4.Text + x + lblTeleOpMidAtt4.Text + x + lblTeleOpLowMade4.Text + x + lblTeleOpLowAtt4.Text + x + lblRobotClimb4.Text + x + lblTeleOpTotalPoints4.Text + x + lblTotalPoints4.Text + x + lblDefense4.Text);
                    sw.WriteLine(lblAutoTeamNo5.Text + x + lblAutoHighMade5.Text + x + lblAutoHighAtt5.Text + x + lblAutoMidMade5.Text + x + lblAutoMidAtt5.Text + x + lblAutoLowMade5.Text + x + lblAutoLowAtt5.Text + x + lblAutoTotalPoints5.Text + x + lblTeleOpPyramidMade5.Text + x + lblTeleOpPyramidAtt5.Text + x + lblTeleOpHighMade5.Text + x + lblTeleOpHighAtt5.Text + x + lblTeleOpMidMade5.Text + x + lblTeleOpMidAtt5.Text + x + lblTeleOpLowMade5.Text + x + lblTeleOpLowAtt5.Text + x + lblRobotClimb5.Text + x + lblTeleOpTotalPoints5.Text + x + lblTotalPoints5.Text + x + lblDefense5.Text);
                    sw.WriteLine(lblAutoTeamNo6.Text + x + lblAutoHighMade6.Text + x + lblAutoHighAtt6.Text + x + lblAutoMidMade6.Text + x + lblAutoMidAtt6.Text + x + lblAutoLowMade6.Text + x + lblAutoLowAtt6.Text + x + lblAutoTotalPoints6.Text + x + lblTeleOpPyramidMade6.Text + x + lblTeleOpPyramidAtt6.Text + x + lblTeleOpHighMade6.Text + x + lblTeleOpHighAtt6.Text + x + lblTeleOpMidMade6.Text + x + lblTeleOpMidAtt6.Text + x + lblTeleOpLowMade6.Text + x + lblTeleOpLowAtt6.Text + x + lblRobotClimb6.Text + x + lblTeleOpTotalPoints6.Text + x + lblTotalPoints6.Text + x + lblDefense6.Text);
                    sw.Close();
                }
            }
            
            //Increases match Number
            match++;
            lblmatch.Text = match.ToString();

            //Updates the team # automatically when a new match starts.
            lblAutoTeamNo1.Text = AutoTeamNo1[match - 1].ToString();
            lblAutoTeamNo2.Text = AutoTeamNo2[match - 1].ToString();
            lblAutoTeamNo3.Text = AutoTeamNo3[match - 1].ToString();
            lblAutoTeamNo4.Text = AutoTeamNo4[match - 1].ToString();
            lblAutoTeamNo5.Text = AutoTeamNo5[match - 1].ToString();
            lblAutoTeamNo6.Text = AutoTeamNo6[match - 1].ToString();

            for (int f = 0; f < 6; f++)
            {
                displayLowFrisbeesMade[f] = 0;
                lowFrisbeesMade[f] = 0;
                lowFrisbeesAtt[f] = 0;
                autoDisplayLowFrisbeesMade[f] = 0;
                autoLowFrisbeesMade[f] = 0;
                autoDisplayLowFrisbeesAtt[f] = 0;
                autoLowFrisbeesAtt[f] = 0;
                displayMidFrisbeesMade[f] = 0;
                midFrisbeesMade[f] = 0;
                displayMidFrisbeesAtt[f] = 0;
                midFrisbeesAtt[f] = 0;
                autoDisplayMidFrisbeesMade[f] = 0;
                autoMidFrisbeesMade[f] = 0;
                autoDisplayMidFrisbeesAtt[f] = 0;
                autoMidFrisbeesAtt[f] = 0;
                displayHighFrisbeesMade[f] = 0;
                highFrisbeesMade[f] = 0;
                displayHighFrisbeesAtt[f] = 0;
                highFrisbeesAtt[f] = 0;
                autoDisplayHighFrisbeesMade[f] = 0;
                autoHighFrisbeesMade[f] = 0;
                autoDisplayHighFrisbeesAtt[f] = 0;
                autoHighFrisbeesAtt[f] = 0;
                displayPyramidFrisbeesMade[f] = 0;
                pyramidFrisbeesMade[f] = 0;
                displayPyramidFrisbeesAtt[f] = 0;
                pyramidFrisbeesAtt[f] = 0;
                climb[f] = 0;
                robotClimb[f] = 0;
                teleOpTotalPoints[f] = 0;
                autoTotalPoints[f] = 0;
                defenseRating[f] = 0;
                displayDefenseRating[f] = 0;
                lblAuto.Visible = true;
                lblAuto2.Visible = true;
                lblAuto3.Visible = true;
                lblAuto4.Visible = true;
                lblAuto5.Visible = true;
                lblAuto6.Visible = true;
                lblTeleOp.Visible = false;
                lblTeleOp2.Visible = false;
                lblTeleOp3.Visible = false;
                lblTeleOp4.Visible = false;
                lblTeleOp5.Visible = false;
                lblTeleOp6.Visible = false;
            }
        }
            
        //The following code updates the time and the date.
        private void tmrtime_Tick(object sender, EventArgs e)
        {
            lblTime.Text = DateTime.Now.ToString("hh:mm:ss tt");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            String myFileName = fileName;
            StreamWriter sww = File.AppendText(myFileName);
            sww.WriteLine(lblAutoTeamNo1.Text + x + lblAutoHighMade.Text + x + lblAutoHighAtt.Text + x + lblAutoMidMade.Text + x + lblAutoMidAtt.Text + x + lblAutoLowMade.Text + x + lblAutoLowAtt.Text + x + lblAutoTotalPoints.Text + x + lblTeleOpPyramidMade.Text + x + lblTeleOpPyramidAtt.Text + x + lblTeleOpHighMade.Text + x + lblTeleOpHighAtt.Text + x + lblTeleOpMidMade.Text + x + lblTeleOpMidAtt.Text + x + lblTeleOpLowMade.Text + x + lblTeleOpLowAtt.Text + x + lblRobotClimb.Text + x + lblTeleOpTotalPoints.Text + x + lblTotalPoints.Text);
            sww.WriteLine(lblAutoTeamNo2.Text + x + lblAutoHighMade2.Text + x + lblAutoHighAtt2.Text + x + lblAutoMidMade2.Text + x + lblAutoMidAtt2.Text + x + lblAutoLowMade2.Text + x + lblAutoLowAtt2.Text + x + lblAutoTotalPoints2.Text + x + lblTeleOpPyramidMade2.Text + x + lblTeleOpPyramidAtt2.Text + x + lblTeleOpHighMade2.Text + x + lblTeleOpHighAtt2.Text + x + lblTeleOpMidMade2.Text + x + lblTeleOpMidAtt2.Text + x + lblTeleOpLowMade2.Text + x + lblTeleOpLowAtt2.Text + x + lblRobotClimb2.Text + x + lblTeleOpTotalPoints2.Text + x + lblTotalPoints2.Text);
            sww.WriteLine(lblAutoTeamNo3.Text + x + lblAutoHighMade3.Text + x + lblAutoHighAtt3.Text + x + lblAutoMidMade3.Text + x + lblAutoMidAtt3.Text + x + lblAutoLowMade3.Text + x + lblAutoLowAtt3.Text + x + lblAutoTotalPoints3.Text + x + lblTeleOpPyramidMade3.Text + x + lblTeleOpPyramidAtt3.Text + x + lblTeleOpHighMade3.Text + x + lblTeleOpHighAtt3.Text + x + lblTeleOpMidMade3.Text + x + lblTeleOpMidAtt3.Text + x + lblTeleOpLowMade3.Text + x + lblTeleOpLowAtt3.Text + x + lblRobotClimb3.Text + x + lblTeleOpTotalPoints3.Text + x + lblTotalPoints3.Text);
            sww.WriteLine(lblAutoTeamNo4.Text + x + lblAutoHighMade4.Text + x + lblAutoHighAtt4.Text + x + lblAutoMidMade4.Text + x + lblAutoMidAtt4.Text + x + lblAutoLowMade4.Text + x + lblAutoLowAtt4.Text + x + lblAutoTotalPoints4.Text + x + lblTeleOpPyramidMade4.Text + x + lblTeleOpPyramidAtt4.Text + x + lblTeleOpHighMade4.Text + x + lblTeleOpHighAtt4.Text + x + lblTeleOpMidMade4.Text + x + lblTeleOpMidAtt4.Text + x + lblTeleOpLowMade4.Text + x + lblTeleOpLowAtt4.Text + x + lblRobotClimb4.Text + x + lblTeleOpTotalPoints4.Text + x + lblTotalPoints4.Text);
            sww.WriteLine(lblAutoTeamNo5.Text + x + lblAutoHighMade5.Text + x + lblAutoHighAtt5.Text + x + lblAutoMidMade5.Text + x + lblAutoMidAtt5.Text + x + lblAutoLowMade5.Text + x + lblAutoLowAtt5.Text + x + lblAutoTotalPoints5.Text + x + lblTeleOpPyramidMade5.Text + x + lblTeleOpPyramidAtt5.Text + x + lblTeleOpHighMade5.Text + x + lblTeleOpHighAtt5.Text + x + lblTeleOpMidMade5.Text + x + lblTeleOpMidAtt5.Text + x + lblTeleOpLowMade5.Text + x + lblTeleOpLowAtt5.Text + x + lblRobotClimb5.Text + x + lblTeleOpTotalPoints5.Text + x + lblTotalPoints5.Text);
            sww.WriteLine(lblAutoTeamNo6.Text + x + lblAutoHighMade6.Text + x + lblAutoHighAtt6.Text + x + lblAutoMidMade6.Text + x + lblAutoMidAtt6.Text + x + lblAutoLowMade6.Text + x + lblAutoLowAtt6.Text + x + lblAutoTotalPoints6.Text + x + lblTeleOpPyramidMade6.Text + x + lblTeleOpPyramidAtt6.Text + x + lblTeleOpHighMade6.Text + x + lblTeleOpHighAtt6.Text + x + lblTeleOpMidMade6.Text + x + lblTeleOpMidAtt6.Text + x + lblTeleOpLowMade6.Text + x + lblTeleOpLowAtt6.Text + x + lblRobotClimb6.Text + x + lblTeleOpTotalPoints6.Text + x + lblTotalPoints6.Text);
            sww.Close();
            //Increases match Number
            match++;
            lblmatch.Text = match.ToString();

            //Updates the team # automatically when a new match starts.
            lblAutoTeamNo1.Text = AutoTeamNo1[match - 1].ToString();
            lblAutoTeamNo2.Text = AutoTeamNo2[match - 1].ToString();
            lblAutoTeamNo3.Text = AutoTeamNo3[match - 1].ToString();
            lblAutoTeamNo4.Text = AutoTeamNo4[match - 1].ToString();
            lblAutoTeamNo5.Text = AutoTeamNo5[match - 1].ToString();
            lblAutoTeamNo6.Text = AutoTeamNo6[match - 1].ToString();

            for (int f = 0; f < 6; f++)
            {
                displayLowFrisbeesMade[f] = 0;
                lowFrisbeesMade[f] = 0;
                lowFrisbeesAtt[f] = 0;
                autoDisplayLowFrisbeesMade[f] = 0;
                autoLowFrisbeesMade[f] = 0;
                autoDisplayLowFrisbeesAtt[f] = 0;
                autoLowFrisbeesAtt[f] = 0;
                displayMidFrisbeesMade[f] = 0;
                midFrisbeesMade[f] = 0;
                displayMidFrisbeesAtt[f] = 0;
                midFrisbeesAtt[f] = 0;
                autoDisplayMidFrisbeesMade[f] = 0;
                autoMidFrisbeesMade[f] = 0;
                autoDisplayMidFrisbeesAtt[f] = 0;
                autoMidFrisbeesAtt[f] = 0;
                displayHighFrisbeesMade[f] = 0;
                highFrisbeesMade[f] = 0;
                displayHighFrisbeesAtt[f] = 0;
                highFrisbeesAtt[f] = 0;
                autoDisplayHighFrisbeesMade[f] = 0;
                autoHighFrisbeesMade[f] = 0;
                autoDisplayHighFrisbeesAtt[f] = 0;
                autoHighFrisbeesAtt[f] = 0;
                displayPyramidFrisbeesMade[f] = 0;
                pyramidFrisbeesMade[f] = 0;
                displayPyramidFrisbeesAtt[f] = 0;
                pyramidFrisbeesAtt[f] = 0;
                climb[f] = 0;
                robotClimb[f] = 0;
                teleOpTotalPoints[f] = 0;
                autoTotalPoints[f] = 0;
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                System.IO.StreamReader sr = new
                   System.IO.StreamReader(openFileDialog1.FileName);
                String test = sr.ReadToEnd();
                String[] newTeams = test.Split(',');
                int countAgain = newTeams.Length;
                teamsNotePad = new string[countAgain];
                newTeams.CopyTo(teamsNotePad, 0);
                autoTeams = teamsNotePad.Length;
                int teamsDivide = teamsNotePad.Length / 6;

                DialogResult dialogResult = MessageBox.Show(teamsDivide.ToString() + "\n Is this the correct number of matches?" ,"Adding Teams", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    sr.Close();

                    AutoTeamNo1 = new int[autoTeams];
                    AutoTeamNo2 = new int[autoTeams];
                    AutoTeamNo3 = new int[autoTeams];
                    AutoTeamNo4 = new int[autoTeams];
                    AutoTeamNo5 = new int[autoTeams];
                    AutoTeamNo6 = new int[autoTeams];
                }
                else if (dialogResult == DialogResult.No)
                {
                    MessageBox.Show("Check your text file with the schedule to make sure it is right!");
                    System.Environment.Exit(0);
                }
            }

            int count = 0;
            for (int j = 0; j < teamsNotePad.Length / 6; j++)
            {
                AutoTeamNo1[j] = Convert.ToInt32(teamsNotePad[count]);
                count++;
                AutoTeamNo2[j] = Convert.ToInt32(teamsNotePad[count]);
                count++;
                AutoTeamNo3[j] = Convert.ToInt32(teamsNotePad[count]);
                count++;
                AutoTeamNo4[j] = Convert.ToInt32(teamsNotePad[count]);
                count++;
                AutoTeamNo5[j] = Convert.ToInt32(teamsNotePad[count]);
                count++;
                AutoTeamNo6[j] = Convert.ToInt32(teamsNotePad[count]);
                count++;
            }

            lblAutoTeamNo1.Text = AutoTeamNo1[0].ToString();
            lblAutoTeamNo2.Text = AutoTeamNo2[0].ToString();
            lblAutoTeamNo3.Text = AutoTeamNo3[0].ToString();
            lblAutoTeamNo4.Text = AutoTeamNo4[0].ToString();
            lblAutoTeamNo5.Text = AutoTeamNo5[0].ToString();
            lblAutoTeamNo6.Text = AutoTeamNo6[0].ToString();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            lblEvent.Text = textBox1.Text;
            button4.Visible = false;
            btnSkip.Visible = true;
            textBox1.Clear();
        }

        private void btnSkip_Click(object sender, EventArgs e)
        {
            int skip = Convert.ToInt32(textBox1.Text);
            OpenFileDialog open = new OpenFileDialog();
            if (open.ShowDialog() == DialogResult.OK)
            {
                fileName = open.FileName;
            }
            match = skip;
            lblmatch.Text = match.ToString();
            lblAutoTeamNo1.Text = AutoTeamNo1[match - 1].ToString();
            lblAutoTeamNo2.Text = AutoTeamNo2[match - 1].ToString();
            lblAutoTeamNo3.Text = AutoTeamNo3[match - 1].ToString();
            lblAutoTeamNo4.Text = AutoTeamNo4[match - 1].ToString();
            lblAutoTeamNo5.Text = AutoTeamNo5[match - 1].ToString();
            lblAutoTeamNo6.Text = AutoTeamNo6[match - 1].ToString();
        }

        private void btnScouter1_Click(object sender, EventArgs e)
        {
            lblScouter1.Text = textBoxScout1.Text;
            textBoxScout1.Visible = false;
            btnScouter1.Visible = false;
            lblScouter1.Visible = true;
        }

        private void btnScouter2_Click(object sender, EventArgs e)
        {
            lblScouter2.Text = textBoxScout2.Text;
            textBoxScout2.Visible = false;
            btnScouter2.Visible = false;
            lblScouter2.Visible = true;
        }

        private void btnScouter3_Click(object sender, EventArgs e)
        {
            lblScouter3.Text = textBoxScout3.Text;
            textBoxScout3.Visible = false;
            btnScouter3.Visible = false;
            lblScouter3.Visible = true;
        }

        private void btnScouter4_Click(object sender, EventArgs e)
        {
            lblScouter4.Text = textBoxScout4.Text;
            textBoxScout4.Visible = false;
            btnScouter4.Visible = false;
            lblScouter4.Visible = true;
        }

        private void btnScouter5_Click(object sender, EventArgs e)
        {
            lblScouter5.Text = textBoxScout5.Text;
            textBoxScout5.Visible = false;
            btnScouter5.Visible = false;
            lblScouter5.Visible = true;
        }

        private void btnScouter6_Click(object sender, EventArgs e)
        {
            lblScouter6.Text = textBoxScout6.Text;
            textBoxScout6.Visible = false;
            btnScouter6.Visible = false;
            lblScouter6.Visible = true;
        }

       
    }
}