using MonoKle.Logging;
using MonoKle.Scripting;
using System;
using System.Windows.Forms;

namespace ScriptTester
{
    public partial class Form1 : Form
    {
        private ScriptEnvironment scriptEnvironment = new ScriptEnvironment();
        private int counter = 0;

        public Form1()
        {
            InitializeComponent();

            this.listBox1.DisplayMember = "Name";
            this.executeButton.Enabled = false;
            this.compileSelectedButton.Enabled = false;

            Logger.Global.LogAddedEvent += Global_LogAddedEvent;
        }

        private void Global_LogAddedEvent(object sender, LogAddedEventArgs e) => this.outputBox.Text = e.Log + "\r\n" + this.outputBox.Text;

        private void newButton_Click(object sender, EventArgs e)
        {
            var script = new Script("Script" + counter++, new StaticScriptSource(""));
            if (scriptEnvironment.Add(script))
            {
                this.listBox1.Items.Add(script);
            }
            else
            {
                Logger.Global.Log("Could not add script.");
            }
        }

        private void compileAllButton_Click(object sender, EventArgs e)
        {
            int res = scriptEnvironment.CompileAll();
            Logger.Global.Log("Compiler: " + res + " scripts compiled");
        }

        private void executeButton_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                var s = (Script)listBox1.SelectedItem;
                var result = s.Execute();
                if (result.Success)
                {
                    if (s.InternalScript.ReturnsValue)
                    {
                        Logger.Global.Log("Execution: " + result.Result);
                    }
                }
                else
                {
                    Logger.Global.Log("Execution failed: " + result.Message);
                }
            }
        }

        private void compileSelectedButton_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                var s = (Script)listBox1.SelectedItem;
                if (scriptEnvironment.Compile(s.Name))
                {
                    if (s.CanExecute)
                    {
                        Logger.Global.Log("Compiler: Script compiled.");
                    }
                    else
                    {
                        Logger.Global.Log("Compiler: Script compiled successfully.");
                        foreach (var err in s.Errors)
                        {
                            Logger.Global.Log(err.ToString(), err.IsWarning ? LogLevel.Warning : LogLevel.Error);

                        }
                    }
                }
                else
                {
                    Logger.Global.Log("Compiler: Script already exists!");
                }
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool selected = this.listBox1.SelectedItem != null;
            this.executeButton.Enabled = selected;
            this.compileSelectedButton.Enabled = selected;
            this.codeBox.Enabled = selected;

            if (selected)
            {
                var s = (Script)listBox1.SelectedItem;
                this.codeBox.Text = s.Source.Code;
            }
            else
            {
                this.codeBox.Text = "";
            }
        }

        private void codeBox_TextChanged(object sender, EventArgs e)
        {
            bool selected = this.listBox1.SelectedItem != null;
            if (selected)
            {
                var s = (Script)listBox1.SelectedItem;
                var ss = s.Source as StaticScriptSource;

                if (this.codeBox.Text != ss.Code)
                {
                    ss.Code = this.codeBox.Text;
                }
            }
        }
    }
}
