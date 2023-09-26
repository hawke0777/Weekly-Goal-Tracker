
////Designed and coded with the use of chatgpt with some modifications
//Programer Andre Collins
//http://amcollinsresume.experserv.com/


using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;



namespace Goal
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            SetDaysToCheckBox();
        }
        private void SetDaysToCheckBox()
        {
            // Create an array of day names.
            string[] dayNames = { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };

            // Loop through the first 7 checkboxes on your form.
            for (int i = 0; i < 7; i++)
            {
                // Assuming you have CheckBox controls named checkBox1, checkBox2, ... checkBox7.
                string checkBoxName = "checkBox" + (i + 1);
                string dayText = dayNames[i];

                // Find the checkbox control by name.
                CheckBox checkBox = Controls.Find(checkBoxName, true)[0] as CheckBox;

                // Set the text of the checkbox.
                if (checkBox != null)
                {
                    checkBox.Text = dayText + " Goal";
                }
            }
        }
        private List<WeeklyGoal> goalsList = new List<WeeklyGoal>();
        private class WeeklyGoal
        {
            public string Text { get; set; }
            public bool[] CheckBoxes { get; set; } = new bool[7];
        }



        private void button1_Click(object sender, EventArgs e)
        {
            // Get text from textBox1
            string goalText = textBox1.Text;

            // Get the state of checkBox1 to checkBox7
            bool[] checkBoxStates = { checkBox1.Checked, checkBox2.Checked, checkBox3.Checked,
                                      checkBox4.Checked, checkBox5.Checked, checkBox6.Checked,
                                      checkBox7.Checked };

            // Create a WeeklyGoal object and add it to the list
            WeeklyGoal goal = new WeeklyGoal
            {
                Text = goalText,
                CheckBoxes = checkBoxStates
            };
            goalsList.Add(goal);

            // Clear textBox1 and checkboxes
            textBox1.Clear();
            ClearCheckBoxes();

            // Display saved information in textBox3
            DisplaySavedGoals();
        }
        private void ClearCheckBoxes()
        {
            // Clear the checkboxes
            checkBox1.Checked = false;
            checkBox2.Checked = false;
            checkBox3.Checked = false;
            checkBox4.Checked = false;
            checkBox5.Checked = false;
            checkBox6.Checked = false;
            checkBox7.Checked = false;
        }

        private void DisplaySavedGoals()
        {
            // Display saved information in textBox3

            foreach (var goal in goalsList)
            {
                textBox3.AppendText($"Goal: {goal.Text}\r\n");
                textBox3.AppendText("Days: ");
                for (int i = 0; i < goal.CheckBoxes.Length; i++)
                {
                    if (goal.CheckBoxes[i])
                    {
                        textBox3.AppendText($"{(DayOfWeek)i} ");
                    }
                }
                textBox3.AppendText("\r\n\r\n");
            }
        }



        private void openCsv()
        {
            // Show an OpenFileDialog to choose the CSV file to open
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "CSV files (*.csv)|*.csv",
                Title = "Open CSV File"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;

                try
                {
                    using (StreamReader reader = new StreamReader(filePath))
                    {
                        // Read the contents of the CSV file
                        string csvContent = reader.ReadToEnd();

                        // Set the contents of textBox3 with the CSV content
                        textBox3.Text = csvContent;
                    }

                    MessageBox.Show("CSV file opened and displayed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void openGoalsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox3.Clear();
            openCsv();
        }
        private void saveCSV()
        {
            // Show a SaveFileDialog to choose the location to save the CSV file
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "CSV files (*.csv)|*.csv",
                Title = "Save CSV File",
                FileName = "goals.csv"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = saveFileDialog.FileName;

                try
                {
                    using (StreamWriter writer = new StreamWriter(filePath))
                    {
                        // Write the contents of textBox3 to the CSV file
                        writer.Write(textBox3.Text);
                    }

                    MessageBox.Show("Data saved to CSV file successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void saveGoalsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveCSV();
        }

        private void sendReminderEmail()
        {
            string recipient = textBox2.Text;
            string subject = "Goals for the week";
            string body = textBox3.Text;

            OutlookEmailSender emailSender = new OutlookEmailSender();
            emailSender.SendEmail(recipient, subject, body);
        }
        private void button3_Click(object sender, EventArgs e)
        {
            sendReminderEmail();
        }

    }
}


