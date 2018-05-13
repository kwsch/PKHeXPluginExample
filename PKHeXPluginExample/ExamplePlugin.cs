using System.Windows.Forms;
using PKHeX.Core;

namespace PKHeXPluginExample
{
    public class ExamplePlugin : IPlugin
    {
        public string Name => nameof(ExamplePlugin);
        public ISaveFileProvider SaveFileEditor { get; private set; }
        public IPKMView PKMEditor { get; private set; }

        public void Initialize(params object[] args)
        {
            if (args == null)
                return;
            if (args.Length > 0)
                SaveFileEditor = (ISaveFileProvider)args[0];
            if (args.Length > 1)
                PKMEditor = (IPKMView)args[1];
            if (args.Length > 2)
                LoadMenuStrip((ToolStrip)args[2]);
        }

        private static void LoadMenuStrip(ToolStrip menuStrip)
        {
            var items = menuStrip.Items;
            var tools = items.Find("Menu_Tools", false)[0] as ToolStripDropDownItem;
            AddPluginControl(tools);
        }

        private static void AddPluginControl(ToolStripDropDownItem tools)
        {
            var ctrl = new ToolStripMenuItem("ExamplePlugin");
            ctrl.Click += (s, e) => MessageBox.Show("Hello!");
            tools.DropDownItems.Add(ctrl);
        }

        public void NotifySaveLoaded()
        {
            MessageBox.Show("Plugin notified that a Save File was just loaded.");
        }
    }
}
