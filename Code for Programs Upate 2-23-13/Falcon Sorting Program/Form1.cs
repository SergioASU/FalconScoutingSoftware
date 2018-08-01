using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;


namespace FalconSortingProgram
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        static int scoring = 18;

        int avg;
        int b;
        int h;
        int l;
        int save1;

        int[] numbers = new int[1];


        int[] allScores = new int[0];
        int colCount = 0;
        int rowCount = 0;
        int x = 0;
        int counting = 0;

        static int preAveragedTeams = 0;
        int preAveragedScores = 0;
        String fileName = "";
        int[] teamsFormat;

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                fileName = openFileDialog1.FileName;
                DialogResult dialogResult = MessageBox.Show(fileName + "\n Is this the correct directory?", "Importing Excel", MessageBoxButtons.YesNo);

                if (dialogResult == DialogResult.Yes)
                {
                    MessageBox.Show("Importing Complete");
                }

                if (dialogResult == DialogResult.No)
                {
                    MessageBox.Show("Check your Excel File and try again");
                }
            }
        }

        public void btnGetData_Click(object sender, EventArgs e)
        {
            Boolean isComputing = true;

            while (isComputing == true)
            {
                label10.Text = "0 %";
                
                label5.Text = "Computing the averages please wait.....";
                int[] matches = new int[listBox1.Items.Count];
                int newMatch = 0;

                Excel.Application xlApp = new Excel.Application();
                Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(fileName);
                Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
                Excel.Range xlRange = xlWorksheet.UsedRange;
                label10.Text = "10 %";
                rowCount = xlRange.Rows.Count;
                colCount = xlRange.Columns.Count;
                int[] teamsBefore = new int[rowCount];
                int all = rowCount * colCount;
                preAveragedTeams = rowCount;
                preAveragedScores = colCount;
                label10.Text = "20 %";

                int[][][] newArray = new int[listBox1.Items.Count][][];
                int matchCount = Convert.ToInt32(txtBoxMatches.Text);
                int z;


                //Filling the notepad arrays
                for (x = 0; x < rowCount; x++)
                {
                    teamsBefore[x] = Convert.ToInt32(xlRange.Cells[x + 1, 1].Value2);
                    label10.Text = "30 %";
                }
                int[][] preAveraged = new int[teamsBefore.Length][];

                for (int gh = 0; gh < teamsBefore.Length; gh++)
                {
                    preAveraged[gh] = new int[18];
                    label10.Text = "40 %";
                }

                for (int g = 0; g < teamsBefore.Length; g++)
                {
                    for (int d = 0; d < scoring; d++)
                    {
                        preAveraged[g][d] = Convert.ToInt32(xlRange.Cells[g + 1, d + 2].Value2);
                    }

                    label10.Text = "50 %";
                } 

               // MessageBox.Show(listBox1.Items[1].ToString());

                //Creating the jagged array for all the data
                for (int x = 0; x < listBox1.Items.Count; x++)
                {
                    newArray[x] = new int[matchCount][];
                    for (int y = 0; y < matchCount; y++)
                    {
                        newArray[x][y] = new int[scoring];
                        label10.Text = "60 %";
                    }
                }

                int row = 1;
                int col = 2;

                //Filling the jagged array with data
                for (int x = 0; x < listBox1.Items.Count; x++)
                {
                    for (int i = 0; i < rowCount; i++)
                    {
                        if (listBox1.Items[x].ToString().Equals(xlRange.Cells[i + 1, 1].Value2.ToString()))
                        {
                            //MessageBox.MessageBox.Show(xlRange.Cells[i + 1, 1].Value2.ToString(), "Row");
                            for (z = 0; z <= 17; z++)
                            {
                                newArray[x][newMatch][z] = Convert.ToInt32(xlRange.Cells[row , col].Value2);
                                label10.Text = "70 %";

                               // MessageBox.Show(newArray[x][newMatch][z].ToString());
                                col++;
                            }
                            matches[x] = matches[x] + 1;
                            newMatch++;
                            col = 2;
                        }

                        row++;
                    }
                    newMatch = 0;
                    row = 1;
                    col = 2;
                }

                //MessageBox.Show(newArray[3][0][3].ToString(), "H");

                double[][] averageArray = new double[listBox1.Items.Count][];

                for (int o = 0; o < listBox1.Items.Count; o++)
                {
                    averageArray[o] = new double[scoring];
                }
               // MessageBox.Show(newArray.Length.ToString());

                // Filling an array with all the numbers
                for (int a = 0; a < listBox1.Items.Count; a++)
                {
                    for (b = 0; b < matches[a]; b++)
                    {
                        for (avg = 0; avg < scoring; avg++)
                        {
                            averageArray[a][avg] = averageArray[a][avg] + newArray[a][b][avg];
                            //MessageBox.Show(averageArray[a][avg].ToString());
                            
                            label10.Text = "80 %";
                        }

                    }
                }
                

                for (int list = 0; list < listBox1.Items.Count; list++)
                {
                    for (int score = 0; score < scoring; score++)
                    {
                        averageArray[list][score] = averageArray[list][score] / matches[list];
                        
                    }
                }

                
                MessageBox.Show("Computing The Averages Is Complete");

                SaveFileDialog saveFileDialog1 = new SaveFileDialog();

                saveFileDialog1.Filter = "txt files (*.txt)|*.txt";
                saveFileDialog1.FilterIndex = 2;
                saveFileDialog1.RestoreDirectory = true;

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    using (Stream s = File.Open(saveFileDialog1.FileName, FileMode.CreateNew))
                    using (StreamWriter sw = new StreamWriter(s))
                    {
                        for (int save = 0; save < listBox1.Items.Count; save++)
                        {
                            for (save1 = 0; save1 < scoring; save1++)
                            {
                                decimal number = 55M;
                                double nextHighest = Math.Round(averageArray[save][save1], 1);
                                sw.Write(nextHighest.ToString() + "," + " " + ",");
                            }
                            sw.WriteLine("");
                        }
                    }
                }

                MessageBox.Show("Succesfully Saved To Notepad");

                for (int fc = 0; fc < listBox1.Items.Count; fc++)
                {
                    for (h = 0; h < matchCount; h++)
                    {
                        for (l = 0; l < scoring; l++)
                        {
                            averageArray[fc][l] = 0;
                        }

                        avg = 0;
                    }

                    b = 0;
                    isComputing = false;
                    label5.Text = "Computing Complete";
                }
            }
        }

        private void btnImportTeams_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                System.IO.StreamReader sr = new
                   System.IO.StreamReader(openFileDialog1.FileName);

                String test = sr.ReadToEnd();
                String[] newTeams = test.Split(',');

                for (int teams = 0; teams < newTeams.Length; teams++)
                {
                    listBox1.Items.Add(newTeams[teams]);
                }

                teamsFormat = new int[listBox1.Items.Count];
                for (int i = 0; i < listBox1.Items.Count; i++)
                {
                    teamsFormat[i] = Convert.ToInt32(listBox1.Items[i]);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            String[] format = { "Team", "AutoHighMade", "Team", "AutoHighAtt", "Team", "AutoMidMade", "Team", "AutoMidAtt", "Team", "AutoLowMade", "Team", "AutoLowAtt", "Team", "AutoTotalPoints", "Team", "PyramidMade", "Team", "PyramidAtt", "Team", "TeleOpHighMade", "Team", "TeleOpHighAtt", "Team", "TeleOpMidMade", "Team", "TeleOpMidAtt", "Team", "TeleOpLowMade", "Team", "TeleOpLowAtt", "Team", "RobotClimb", "Team", "TeleOpTotal", "Team", "TotalOverall" };
            MessageBox.Show("Please choose a black excel file to format");
            OpenFileDialog open = new OpenFileDialog();

            if (open.ShowDialog() == DialogResult.OK)
            {
                fileName = open.FileName;

               
            }

            Excel.Application xlApp2 = new Excel.Application();
            Excel.Workbook xlWorkbook2 = xlApp2.Workbooks.Open(fileName);
            Excel._Worksheet xlWorksheet2 = xlWorkbook2.Sheets[1];
            Excel.Range xlRange2 = xlWorksheet2.UsedRange;

            int rowCount2 = xlRange2.Rows.Count;
            int colCount2 = xlRange2.Columns.Count;

            int count = 1;
            int countingIt = 0;
            label10.Text = countingIt.ToString();

            for(int j = 1; j <= teamsFormat.Length * 18;j++)
            {
                if (countingIt == teamsFormat.Length)
                {
                    count = count + 2;
                    countingIt = 0;
                }
            
                xlApp2.Cells[countingIt + 2, count] = teamsFormat[countingIt].ToString();
                countingIt++;
                
            }

            for (int l = 1; l <= format.Length; l++)
            {
                xlApp2.Cells[1, l] = format[l - 1];
            }
            MessageBox.Show("Complete");
            xlApp2.Visible = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            label9.Text = textBox1.Text;
            textBox1.Visible = false;
            button3.Visible = false;
        }
    }
}