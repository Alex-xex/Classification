using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Classification
{
    public partial class Form2 : Form
    {
        public Form1 form { get; set; }
        public IEnumerable<string> onlylines { get; set; }
        public Form2(Form1 f, IEnumerable<string> onlylines)
        {
            InitializeComponent();
            comboBox1.Items.Add(",");          
            comboBox1.Items.Add(";");
            comboBox1.Items.Add(":");
            comboBox1.Items.Add("|");
            comboBox1.Items.Add("/");
            comboBox1.SelectedIndex = 0;
            form = f;
            textBox1.ScrollBars = ScrollBars.Both;
            this.onlylines = onlylines;
            List<string> list = onlylines.ToList();
            string alltext = "";
            foreach(string line in list)
            {
                alltext = alltext + line + "\r\n";
            }
            textBox1.Text = alltext;
            textBox1.ReadOnly = true;
        }


        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            form.WriteDG(onlylines, comboBox1.SelectedItem.ToString().ToCharArray()[0]);
            form.Show();
            this.Close();

        }
    }
}
