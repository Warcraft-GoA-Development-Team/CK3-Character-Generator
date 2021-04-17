using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CK3_Character_Generator
{
    class Character
    {
        public string id;
        public string name;
        public bool female;
        public string dynasty;
        public string religion;
        public string culture;

        public Character father = null;
        public Character mother = null;
        public List<Character> children = new List<Character>();

        public DateTime birthDate;
        public DateTime deathDate;

        public string PrintBirthDate
        {
            get => $"{birthDate.Year}.{birthDate.Month}.{birthDate.Day}";
        }
        public string PrintDeathDate
        {
            get => $"{deathDate.Year}.{deathDate.Month}.{deathDate.Day}";
        }

        public Character(string _id, string _name, bool _female, string _dynasty, string _religion, string _culture, DateTime _birthDate, DateTime _deathDate)
        {
            id = _id;
            name = _name;
            female = _female;
            dynasty = _dynasty;
            religion = _religion;
            culture = _culture;
            birthDate = _birthDate;
            deathDate = _deathDate;
        }

        public void AddChild(Character _child)
        {
            if (female)
                _child.mother = this;
            else
                _child.father = this;
            children.Add(_child);
        }
    }
}
