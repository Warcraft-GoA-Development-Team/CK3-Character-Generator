using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace CK3_Character_Generator
{
    class CharacterGenerator
    {
        const string maleGender = "Male";
        const string femaleGender = "Female";
        const string equalGender = "Equal";
        static string Tab = "\t";
        static string NewLine = Environment.NewLine;

        int firstID;

        List<string> maleNames;
        List<string> femaleNames;

        string dynastyID;

        string religion;
        string culture;

        int averageLifespan;
        int firstYear;
        int lastYear;

        string genderLaw;
        string raceTrait;

        int currentID = 0;

        public CharacterGenerator(int _firstID, string maleNamesString, string femaleNamesString, string _dynastyID, string _religion, string _culture, int _averageLifespan, int _firstYear, int _lastYear, string _genderLaw, string _raceTrait)
        {
            firstID = _firstID;
            currentID = firstID;

            maleNames = Regex.Split(maleNamesString, @"\s+").ToList();
            femaleNames = Regex.Split(femaleNamesString, @"\s+").ToList();

            dynastyID = _dynastyID;

            religion = _religion;
            culture = _culture;

            averageLifespan = _averageLifespan;
            firstYear = _firstYear;
            lastYear = _lastYear;

            genderLaw = _genderLaw;
            raceTrait = _raceTrait;
        }

        public void GenerateCharacters(TextBox charOutput, TextBox titleOutput)
        {
            Character prevCharacter = null;
            Character firstCharacter = null;
            int year = firstYear;

            charOutput.AppendText($"#dynasty = {dynastyID}" + NewLine);
            do
            {
                if (prevCharacter != null)
                {
                    if (prevCharacter.deathDate.Year > 20)
                        year = prevCharacter.deathDate.Year - 17;
                    else
                        year = prevCharacter.deathDate.Year - 1;
                }

                Character character = GenerateCharacter(year);

                //If it's not first character, add child to prev character
                if (prevCharacter != null)
                    prevCharacter.AddChild(character);
                //Saves first character
                if (firstCharacter == null)
                    firstCharacter = character;

                prevCharacter = character;

                //Writes character history
                //ID and opens
                charOutput.AppendText(character.id + " = { #Generated" + NewLine);
                //Name
                charOutput.AppendText(Tab + $"name = {character.name}" + NewLine);
                //Sex
                if (character.female) charOutput.AppendText(Tab + "female = yes" + NewLine);
                //Dynasty
                charOutput.AppendText(Tab + $"dynasty = {character.dynasty}" + NewLine);
                //Religion
                charOutput.AppendText(Tab + $"religion = {character.religion}" + NewLine);
                //Culture
                charOutput.AppendText(Tab + $"culture = {character.culture}" + NewLine);
                //Father
                if (character.father != null) charOutput.AppendText(Tab + $"father = {character.father.id}" + NewLine);
                //Mother
                if (character.mother != null) charOutput.AppendText(Tab + $"mother = {character.mother.id}" + NewLine);
                //Birthdate
                charOutput.AppendText(Tab + $"{character.PrintBirthDate}" + " = { birth = yes ");
                // If you have race trait...
                if (raceTrait != "") charOutput.AppendText($"trait = {raceTrait} ");
                charOutput.AppendText("}" + NewLine);
                //Deathdate
                charOutput.AppendText(Tab + $"{character.PrintDeathDate}" + " = { death = yes }" + NewLine);
                //Closes
                charOutput.AppendText("}" + NewLine);
            }
            while (prevCharacter.deathDate.Year < lastYear);

            charOutput.AppendText(NewLine);

            //Writes title history
            WriteTitleHistory(firstCharacter, titleOutput);
        }
        public Character GenerateCharacter(int year)
        {
            System.Threading.Thread.Sleep(5);
            Random random = new Random();

            //Decides sex
            bool female;
            switch (genderLaw)
            {
                case femaleGender:
                    female = true;
                    break;
                case equalGender:
                    if(random.Next(100) > 50)
                        female = false;
                    else
                        female = true;
                    break;
                default:
                    female = false;
                    break;
            }

            // Picks name
            List<string> names = new List<string>();
            if (female)
                names = femaleNames;
            else
                names = maleNames;
            string name = names[random.Next(names.Count)];

            // Random birthdate
            DateTime birthDate = new DateTime(year, random.Next(1, 12), random.Next(1, 29));

            // Random deathdate
            int lifespan = (int)(averageLifespan + (averageLifespan * random.Next(-10, 10) * 0.03));
            DateTime deathDate = new DateTime(year + lifespan, random.Next(1, 12), random.Next(1, 29));

            Character character = new Character($"{culture}{currentID}", name, female, dynastyID, religion, culture, birthDate, deathDate);

            currentID++;

            return character;
        }

        public void WriteTitleHistory(Character firstHolder, TextBox titleOutput)
        {
            Character prevCharacter = firstHolder;

            // Writes title history for first
            titleOutput.AppendText(firstHolder.PrintBirthDate + " = { holder = " + firstHolder.id + " }" + $" #{firstHolder.name}[{firstHolder.dynasty}]" + NewLine);

            while(TryFindHeir(prevCharacter, out Character heir))
            {
                // Writes title history for next heir
                titleOutput.AppendText(prevCharacter.PrintDeathDate + " = { holder = " + heir.id + " }" + $" #{heir.name}[{heir.dynasty}]" + NewLine);
                prevCharacter = heir;
            }

            titleOutput.AppendText(NewLine);
        }
        public bool TryFindHeir(Character target, out Character heir)
        {
            heir = null;
            for (int i = 0; target.children.Count > i; i++)
            {
                if (target.children[i].deathDate > target.deathDate)
                {
                    heir = target.children[i];
                    return true;
                }
            }
            return false;
        }
    }
}
