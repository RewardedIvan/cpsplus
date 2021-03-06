using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.IO;


namespace cps_
{
    public partial class Form1 : Form
    {
        long[] cps = new long[2];
        long[] cpsr = new long[2];

        public Form1()
        {
            InitializeComponent();
        }

        void SaveThemeToPG()
        {
            try
            {
                Settings1.Default.RecordLeftF = label2.ForeColor;
                Settings1.Default.RecordRightF = label3.ForeColor;
                Settings1.Default.cpsF = label1.ForeColor;
                Settings1.Default.RecordLeftB = label2.BackColor;
                Settings1.Default.RecordRightB = label3.BackColor;
                Settings1.Default.cpsB = label1.BackColor;
                Settings1.Default.RecordLeftFO = label2.Font;
                Settings1.Default.RecordRightFO = label3.Font;
                Settings1.Default.cpsFO = label1.Font;
                Settings1.Default.bg = this.BackColor;
                Settings1.Default.Save();
            }
            catch(Exception err)
            {
                MessageBox.Show("An unexpected error occurred, " + err, "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void LoadThemeFromPG()
        {
            try
            {
                label2.ForeColor = Settings1.Default.RecordLeftF;
                label3.ForeColor = Settings1.Default.RecordRightF;
                label1.ForeColor = Settings1.Default.cpsF;
                label2.BackColor = Settings1.Default.RecordLeftB;
                label3.BackColor = Settings1.Default.RecordRightB;
                label1.BackColor = Settings1.Default.cpsB;
                label2.Font = Settings1.Default.RecordLeftFO;
                label3.Font = Settings1.Default.RecordRightFO;
                label1.Font = Settings1.Default.cpsFO;
                this.BackColor = Settings1.Default.bg;
            }
            catch(Exception err)
            {
                MessageBox.Show("An unexpected error occurred, " + err, "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void ChangeValue(Label Obj, int type)
        {
            //shortcut for changing colors
            try
            {
                if (type == 1)
                {
                    ColorDialog UColor = new ColorDialog();
                    if (UColor.ShowDialog() != DialogResult.Cancel)
                    {
                        Obj.ForeColor = UColor.Color;
                    }
                }
                else if (type == 2)
                {
                    ColorDialog UColor = new ColorDialog();
                    if (UColor.ShowDialog() != DialogResult.Cancel)
                    {
                        Obj.BackColor = UColor.Color;
                    }
                }
                else if (type == 3)
                {
                    FontDialog UFont = new FontDialog();
                    if (UFont.ShowDialog() != DialogResult.Cancel)
                    {
                        Obj.Font = UFont.Font;
                    }
                }
            }
            catch(Exception err)
            {
                MessageBox.Show("An unexpected error occurred, " + err, "error", MessageBoxButtons.OK, MessageBoxIcon.Error);   
            }
        }

        private void Form1_Click(object sender, EventArgs e)
        {
            
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            //click handler
            try
            {
                if (e.Button == MouseButtons.Left)
                {
                    timer1.Start();
                    cps[0]++;
                    if (Settings1.Default.Display == 0)
                        label1.Text = $"{cps[0]} l {cps[1]}";
                }
                else if (e.Button == MouseButtons.Right)
                {
                    timer1.Start();
                    cps[1]++;
                    if (Settings1.Default.Display == 0)
                        label1.Text = $"{cps[0]} l {cps[1]}";
                }
            }
            catch(Exception err)
            {
                MessageBox.Show("An unexpected error occurred, " + err, "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //cps display
            try
            {
                if (cps[0] >= cpsr[0]) { Settings1.Default.Record0 = cps[0]; Settings1.Default.Save(); cpsr[0] = cps[0]; label2.Text = $"Record: {cpsr[0]}"; }
                if (cps[1] >= cpsr[1]) { Settings1.Default.Record1 = cps[1]; Settings1.Default.Save(); cpsr[1] = cps[1]; label3.Text = $"Record: {cpsr[1]}"; }
                if (Settings1.Default.Display == 1)
                    label1.Text = $"{cps[0]} l {cps[1]}";
                cps[0] = 0;
                cps[1] = 0;
                timer1.Stop();
            }
            catch(Exception err)
            {
                MessageBox.Show("An unexpected error occurred, " + err, "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                //record loader
                cpsr[0] = Settings1.Default.Record0;
                label2.Text = $"Record: {cpsr[0]}";
                cpsr[1] = Settings1.Default.Record1;
                label3.Text = $"Record: {cpsr[1]}";
                //theme loader
                LoadThemeFromPG();
                //settings loader
                //auto save setting loader
                if (Settings1.Default.AutoSave == 0)
                {
                    autoSaveToolStripMenuItem.Text = "Auto Save - OFF";
                    autoSaveToolStripMenuItem.ToolTipText = "Doesnt automaticly save after opening a theme from a file; Does give a warning.";
                }
                else if (Settings1.Default.AutoSave == 1)
                {
                    autoSaveToolStripMenuItem.Text = "Auto Save - ON";
                    autoSaveToolStripMenuItem.ToolTipText = "Automaticly saves after opening a theme from a file; Doesnt give a warning.";
                }
                else if (Settings1.Default.AutoSave == 2)
                {
                    autoSaveToolStripMenuItem.Text = "Auto Save - SILENT ON";
                    autoSaveToolStripMenuItem.ToolTipText = "Automaticly saves after opening a theme from a file; Also doesnt give a warning.";
                }
                else if (Settings1.Default.AutoSave == 3)
                {
                    autoSaveToolStripMenuItem.Text = "Auto Save - SILENT OFF";
                    autoSaveToolStripMenuItem.ToolTipText = "Doesnt automaticly save after opening a theme from a file; Also doesnt give a warning.";
                }
                //display setting loader
                if (Settings1.Default.Display == 0)
                {
                    displayNowToolStripMenuItem.Text = "Display Now";
                    displayNowToolStripMenuItem.ToolTipText = "Displays cps after every click.";
                }
                else if (Settings1.Default.Display == 1)
                {
                    displayNowToolStripMenuItem.Text = "Display After";
                    displayNowToolStripMenuItem.ToolTipText = "Displays cps after you haved clicked for a sec.";
                }
            }
            catch(Exception err)
            {
                MessageBox.Show("An unexpected error occurred, " + err, "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void label3_MouseClick(object sender, MouseEventArgs e)
        {
            
        }

        private void changeFontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeValue(label3, 3);
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            
        }

        private void changeForegroundToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeValue(label3, 1);
        }

        private void changeBackgroundToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeValue(label3, 2);
        }

        private void Form1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                //bg changer cuz the function takes labels
                if (e.Button == MouseButtons.Middle)
                {
                    ColorDialog UColor = new ColorDialog();
                    if (UColor.ShowDialog() != DialogResult.Cancel)
                    {
                        this.BackColor = UColor.Color;
                    }
                }
            }
            catch(Exception err)
            {
                MessageBox.Show("An unexpected error occurred, " + err, "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ChangeValue(label2, 3);
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            ChangeValue(label2, 1);
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            ChangeValue(label2, 2);
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            ChangeValue(label1, 3);
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            ChangeValue(label1, 1);
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            ChangeValue(label1, 2);
        }

        private void saveToFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //theme json saver
            try
            {
                saveFileDialog1.Filter = "json file|*.json|text file|*.txt|all|*.*";
                saveFileDialog1.Title = "Save theme";
                saveFileDialog1.FileName = "";
                if (saveFileDialog1.ShowDialog() != DialogResult.Cancel)
                {
                    using (StreamWriter w = new StreamWriter(saveFileDialog1.FileName))
                    {
                        w.WriteLine("{\n" + $"  \"note\": \"format: Alpha, Red, Green, Blue\",");
                        w.WriteLine($"  \"RecordRightB\": \"{label2.BackColor.A}, {label2.BackColor.R}, {label2.BackColor.G}, {label2.BackColor.B}\",");
                        w.WriteLine($"  \"RecordLeftB\": \"{label3.BackColor.A}, {label3.BackColor.R}, {label3.BackColor.G}, {label3.BackColor.B}\",");
                        w.WriteLine($"  \"CPSB\": \"{label1.BackColor.A}, {label1.BackColor.R}, {label1.BackColor.G}, {label1.BackColor.B}\",");
                        w.WriteLine($"  \"RecordRightF\": \"{label2.ForeColor.A}, {label2.ForeColor.R}, {label2.ForeColor.G}, {label2.ForeColor.B}\",");
                        w.WriteLine($"  \"RecordLeftF\": \"{label3.ForeColor.A}, {label3.ForeColor.R}, {label3.ForeColor.G}, {label3.ForeColor.B}\",");
                        w.WriteLine($"  \"CPSF\": \"{label1.ForeColor.A}, {label1.ForeColor.R}, {label1.ForeColor.G}, {label1.ForeColor.B}\",");
                        w.WriteLine($"  \"bg\": \"{this.BackColor.A}, {this.BackColor.R}, {this.BackColor.G}, {this.BackColor.B}\"" + "\n}");
                        w.Close();
                    }
                }
            }
            catch(Exception err)
            {
                MessageBox.Show("An unexpected error occurred, " + err, "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void saveToProgramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //theme saver
            SaveThemeToPG();
        }

        private void openThemeFromFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //theme file loader
            try
            {
                openFileDialog1.Filter = "json file|*.json|text file|*.txt|all|*.*";
                openFileDialog1.Title = "Open theme";
                openFileDialog1.FileName = "";
                if (openFileDialog1.ShowDialog() != DialogResult.Cancel)
                {
                    using (StreamReader r = new StreamReader(openFileDialog1.FileName))
                    {
                        dynamic JsFile = JsonConvert.DeserializeObject(r.ReadToEnd());
                        Color B1 = label2.ForeColor;
                        Color B2 = label3.ForeColor;
                        Color B3 = label1.ForeColor;
                        Color B4 = label2.BackColor;
                        Color B5 = label3.BackColor;
                        Color B6 = label1.BackColor;
                        Color B7 = this.BackColor;
                        label2.ForeColor = JsFile["RecordLeftF"];
                        label3.ForeColor = JsFile["RecordRightF"];
                        label1.ForeColor = JsFile["CPSF"];
                        label2.BackColor = JsFile["RecordLeftB"];
                        label3.BackColor = JsFile["RecordRightB"];
                        label1.BackColor = JsFile["CPSB"];
                        this.BackColor = JsFile["bg"];
                        if (Settings1.Default.AutoSave == 0)
                            MessageBox.Show("note: theme is not automaticly saved to program! go into settings to save it or enable auto save", "note", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        else if (Settings1.Default.AutoSave == 1)
                        {
                            SaveThemeToPG();
                            if (MessageBox.Show("Theme has been automaticly saved to the program! Do you want to revert?", "Theme saved", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                Settings1.Default.RecordLeftF = B1;
                                Settings1.Default.RecordRightF = B2;
                                Settings1.Default.cpsF = B3;
                                Settings1.Default.RecordLeftB = B4;
                                Settings1.Default.RecordRightB = B5;
                                Settings1.Default.cpsB = B6;
                                Settings1.Default.bg = B7;
                                Settings1.Default.Save();
                                LoadThemeFromPG();
                            }
                        }
                        else if (Settings1.Default.AutoSave == 2)
                            SaveThemeToPG();
                    }
                }
            }
            catch(Exception err)
            {
                MessageBox.Show("An unexpected error occurred, " + err, "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void autoSaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (Settings1.Default.AutoSave == 0)
                {
                    Settings1.Default.AutoSave = 1;
                    Settings1.Default.Save();
                    autoSaveToolStripMenuItem.Text = "Auto Save - ON";
                    autoSaveToolStripMenuItem.ToolTipText = "Automaticly saves after opening a theme from a file; Doesnt give a warning.";
                }
                else if (Settings1.Default.AutoSave == 1)
                {
                    Settings1.Default.AutoSave = 2;
                    Settings1.Default.Save();
                    autoSaveToolStripMenuItem.Text = "Auto Save - SILENT ON";
                    autoSaveToolStripMenuItem.ToolTipText = "Automaticly saves after opening a theme from a file; Also doesnt give a warning.";
                }
                else if (Settings1.Default.AutoSave == 2)
                {
                    Settings1.Default.AutoSave = 3;
                    Settings1.Default.Save();
                    autoSaveToolStripMenuItem.Text = "Auto Save - SILENT OFF";
                    autoSaveToolStripMenuItem.ToolTipText = "Doesnt automaticly save after opening a theme from a file; Also doesnt give a warning.";
                }
                else if (Settings1.Default.AutoSave == 3)
                {
                    Settings1.Default.AutoSave = 0;
                    Settings1.Default.Save();
                    autoSaveToolStripMenuItem.Text = "Auto Save - OFF";
                    autoSaveToolStripMenuItem.ToolTipText = "Doesnt automaticly save after opening a theme from a file; Does give a warning.";
                }
            }
            catch(Exception err)
            {
                MessageBox.Show("An unexpected error occurred, " + err, "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void displayNowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (Settings1.Default.Display == 0)
                {
                    Settings1.Default.Display = 1;
                    Settings1.Default.Save();
                    displayNowToolStripMenuItem.Text = "Display After";
                    displayNowToolStripMenuItem.ToolTipText = "Displays cps after you haved clicked for a sec.";
                }
                else if (Settings1.Default.Display == 1)
                {
                    Settings1.Default.Display = 0;
                    Settings1.Default.Save();
                    displayNowToolStripMenuItem.Text = "Display Now";
                    displayNowToolStripMenuItem.ToolTipText = "Displays cps after every click.";
                }
            }
            catch(Exception err)
            {
                MessageBox.Show("An unexpected error occurred, " + err, "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
