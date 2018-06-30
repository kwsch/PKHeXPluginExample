using System;
using System.Windows.Forms;
using PKHeX.Core;

namespace PKHeXPluginExample
{
    public class ExamplePlugin : IPlugin
    {
        public string Name => nameof(ExamplePlugin);
        public int Priority => 1; // Loading order, lowest is first.
        public ISaveFileProvider SaveFileEditor { get; private set; }
        public IPKMView PKMEditor { get; private set; }

        public void Initialize(params object[] args)
        {
            Console.WriteLine($"Loading {Name}...");
            if (args == null)
                return;
            SaveFileEditor = (ISaveFileProvider)Array.Find(args, z => z is ISaveFileProvider);
            PKMEditor = (IPKMView)Array.Find(args, z => z is IPKMView);
            var menu = (ToolStrip)Array.Find(args, z => z is ToolStrip);
            LoadMenuStrip(menu);
        }

        private void LoadMenuStrip(ToolStrip menuStrip)
        {
            var items = menuStrip.Items;
            var tools = items.Find("Menu_Tools", false)[0] as ToolStripDropDownItem;
            AddPluginControl(tools);
        }

        private void AddPluginControl(ToolStripDropDownItem tools)
        {
            var ctrl = new ToolStripMenuItem(Name);
            tools.DropDownItems.Add(ctrl);

            var c2 = new ToolStripMenuItem($"{Name} sub form");
            c2.Click += (s, e) => new Form().ShowDialog();
            var c3 = new ToolStripMenuItem($"{Name} show message");
            c3.Click += (s, e) => MessageBox.Show("Hello!");
            var c4 = new ToolStripMenuItem($"{Name} modify savefile");
            c4.Click += (s, e) => ModifySaveFile();
            ctrl.DropDownItems.Add(c2);
            ctrl.DropDownItems.Add(c3);
            ctrl.DropDownItems.Add(c4);
            Console.WriteLine($"{Name} added menu items.");
        }

        private void ModifySaveFile()
        {
            // Make everything Bulbasaur!
            SaveFileEditor.SAV.ModifyBoxes(pk => pk.Species = 1);
            SaveFileEditor.ReloadSlots();
        }

        public void NotifySaveLoaded()
        {
            Console.WriteLine($"{Name} was notified that a Save File was just loaded.");
        }
        public bool TryLoadFile(string filePath)
        {
            Console.WriteLine($"{Name} was provided with the file path, but chose to do nothing with it.");
            return false; // no action taken
        }
    }
}
