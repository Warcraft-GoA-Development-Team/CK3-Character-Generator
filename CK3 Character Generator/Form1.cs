using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CK3_Character_Generator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void generateButton_Click(object sender, EventArgs e)
        {
            generateButton.Enabled = false;

            if (Int32.TryParse(characterIDTextbox.Text, out int firstID))
            {
                if (Int32.TryParse(lifespanTextBox.Text, out int lifespan))
                {
                    if (lifespan > 18)
                    {
                        if (Int32.TryParse(firstYearTextBox.Text, out int firstYear))
                        {
                            if (Int32.TryParse(lastYearTextBox.Text, out int lastYear))
                            {
                                CharacterGenerator generator = new CharacterGenerator(firstID, maleNamesTextBox.Text, femaleNamesTextBox.Text, dynastyIDTextbox.Text, faithTextbox.Text, cultureTextbox.Text, lifespan, firstYear, lastYear, dominantGenderTextBox.Text, raceTextBox.Text);
                                generator.GenerateCharacters(characterTextbox, titleTextBox);
                            }
                            else
                                characterTextbox.Text += "Error! Last Year must be a number!" + Environment.NewLine;
                        }
                        else
                            characterTextbox.Text += "Error! First Year must be a number!" + Environment.NewLine;
                    }
                    else
                    {
                        characterTextbox.Text += "Error! Lifespan must be highter than 18!" + Environment.NewLine;
                    }
                }
                else
                    characterTextbox.Text += "Error! Lifespan must be a number!" + Environment.NewLine;
            }
            else
                characterTextbox.Text += "Error! Character ID must be a number!" + Environment.NewLine;

            generateButton.Enabled = true;
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            clearButton.Enabled = false;

            characterTextbox.Text = "";
            titleTextBox.Text = "";

            clearButton.Enabled = true;
        }
    }
}
