using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyPlanner.Framework
{
    class CheckList
    {
        private List<string> CheckListItems;

        public CheckList()
        {
            string filename = Path.Combine("Mods", "DailyPlanner", "Plans", "Checklist.txt");
            this.CheckListItems = new List<string>(File.ReadAllLines(filename));
            this.CheckListItems.Remove("");
            this.CheckListItems.Remove(" ");
        }

        public List<string> GetCheckListItems()
        {
            return this.CheckListItems;
        }

        public void CompleteTask(string label)
        {
            this.CheckListItems.Remove(label);
            string filename = Path.Combine("Mods", "DailyPlanner", "Plans", "Checklist.txt");
            File.WriteAllLines(filename, this.CheckListItems);
        }
    }
}
