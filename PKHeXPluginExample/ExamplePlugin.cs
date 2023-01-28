using System;
using System.Windows.Forms;
using PKHeX.Core;

namespace PKHeXPluginExample
{
    public class ExamplePlugin : IPlugin
    {
        public string Name => nameof(ExamplePlugin);
        public int Priority => 1; // Loading order, lowest is first.

        // Initialized on plugin load
        public ISaveFileProvider SaveFileEditor { get; private set; } = null!;
        public IPKMView PKMEditor { get; private set; } = null!;

        public void Initialize(params object[] args)
        {
            Console.WriteLine($"Loading {Name}...");
            SaveFileEditor = (ISaveFileProvider)Array.Find(args, z => z is ISaveFileProvider)!;
            PKMEditor = (IPKMView)Array.Find(args, z => z is IPKMView)!;
            var menu = (ToolStrip)Array.Find(args, z => z is ToolStrip)!;
            LoadMenuStrip(menu);
        }

        private void LoadMenuStrip(ToolStrip menuStrip)
        {
            var items = menuStrip.Items;
            if (!(items.Find("Menu_Tools", false)[0] is ToolStripDropDownItem tools))
                throw new ArgumentException(nameof(menuStrip));
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
            var c4 = new ToolStripMenuItem($"{Name} modify current SaveFile");
            c4.Click += (s, e) => ModifySaveFile();
            ctrl.DropDownItems.Add(c2);
            ctrl.DropDownItems.Add(c3);
            ctrl.DropDownItems.Add(c4);
            Console.WriteLine($"{Name} added menu items.");
        }

        private void ModifySaveFile()
        {
            var sav = SaveFileEditor.SAV;
            sav.ModifyBoxes(ModifyPKM);
            SaveFileEditor.ReloadSlots();
        }

        public static void ModifyPKM(PKM pkm)
        {
            // Make everything Bulbasaur!
            pkm.Species = (int)Species.Bulbasaur;
            pkm.Move1 = 1; // pound
            pkm.Move1_PP = 40;
            CommonEdits.SetShiny(pkm);
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
