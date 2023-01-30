using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace Classification
{
    public partial class Form1 : Form
    {
        List<NotFinisedElement> notFinisedElements = new List<NotFinisedElement>();
        List<Element> elements = new List<Element>();
        ClassElement classElement = new ClassElement(0);
        List<TestElement> testElements = new List<TestElement>();
        Element test = new Element();

        public Form1()
        {
            InitializeComponent();
            openFileDialog1.Filter = "Data files(*.data)|*.data|Text files(*.txt)|*.txt|All files(*.*)|*.*";
            openFileDialog1.Title = "Выберете файл данных";
            dataGridView1.AllowUserToAddRows = false;
            dataGridView2.AllowUserToAddRows = false;          
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void FileAdd_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                notFinisedElements.Clear();
                dataGridView1.Rows.Clear();
                dataGridView1.Columns.Clear();               
                dataGridView2.Columns.Clear();

                string filename = openFileDialog1.FileName; // получаем путь к файлу
                IEnumerable<string> onlylines = File.ReadLines(filename); // считываем файл и сразу разделяем по строкам
                onlylines = onlylines.Where(x => !string.IsNullOrWhiteSpace(x));
                Form2 newForm = new Form2(this, onlylines);
                newForm.Show();
                Hide();
            }
            else
                return;
        }

        private void ChoseClass_Click(object sender, EventArgs e)
        {
            listBox2.Items.Clear();
            classElement = new ClassElement(0);
            if (notFinisedElements.Count != 0)
            {             
                foreach (var line in notFinisedElements) //Заполнение листа классов
                {
                    if (comboBox1.SelectedIndex != -1)
                    {
                        classElement.classlist.Add(line.element[comboBox1.SelectedIndex]);
                        classElement.classnumber = comboBox1.SelectedIndex;
                    }
                    else
                    {
                        MessageBox.Show("Сначала выберете столбец!");
                        break;
                    }
                }
                classElement.classlist.removeDuplicates();
                string[] classarray = classElement.classlist.Distinct().ToArray();
                listBox2.Items.AddRange(classarray);
            }
            else
                MessageBox.Show("Сначала загрузите данные!");
        }

        private void StartAlgorithm_Click(object sender, EventArgs e)
        {
            elements.Clear();
            dataGridView2.Rows.Clear();
            dataGridView2.Columns.Clear();           

            foreach (var list in notFinisedElements)
            {
                Element element = new Element();
                for(int i = 0; i < list.element.Count; i++)
                {
                    if(i != classElement.classnumber)
                    {
                        list.element[i] = list.element[i].Replace(".", ",");
                        element.info.Add(Convert.ToDouble(list.element[i]));
                    }
                    else if(i == classElement.classnumber)
                    {
                        element.classification = list.element[i];
                    }
                }
                elements.Add(element);
            }

            for(int i = 0; i < elements[0].info.Count; i++)
            {
                dataGridView2.Columns.Add($"{i}", $"Столбец номер {i + 1}");
            }
            dataGridView2.Rows.Add();

            
            foreach(var classificator in classElement.classlist)
            {
                testElements.Add(new TestElement(classificator));
            }
        }

        

        public void WriteDG(IEnumerable<string> onlylines, char separator)
        {
            foreach (var line in onlylines)
            {
                List<string> listString = line.Split(new char[] {separator}).OfType<string>().ToList(); // Разделение по запятой и последующее преобразование в формат List<string>
                NotFinisedElement notFinisedElement = new NotFinisedElement();
                notFinisedElement.element = listString;
                notFinisedElements.Add(notFinisedElement);
            }

            for (int i = 0; i < notFinisedElements.Count; i++) //заполнение DataGrid
            {
                for (int j = 0; j < notFinisedElements[i].element.Count; j++)
                {
                    if (i == 0)
                    {
                        dataGridView1.Columns.Add($"{j}", $"Столбец номер {j + 1}");
                        comboBox1.Items.Add($"Столбец номер {j + 1}");
                    }
                    if (j == 0)
                        dataGridView1.Rows.Add();

                    dataGridView1.Rows[i].Cells[j].Value = notFinisedElements[i].element[j];
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        

        private void button2_Click(object sender, EventArgs e)
        {
            for(int i = 0; i < dataGridView2.Rows[0].Cells.Count; i++)
            {
                test.info.Add(Convert.ToDouble(dataGridView2.Rows[0].Cells[i].Value));
            }
                
            foreach (var element in elements)
            {
                for(int i = 0; i < element.info.Count; i++)
                {
                    for(int j = 0; j < testElements.Count; j++)
                    {
                        if(element.classification == testElements[j].classification && element.info[i] == test.info[i])
                        {
                            testElements[j].score++;
                        }                     
                    }
                }
            }

            List<int> help = new List<int>();
            for (int i = 0; i < testElements.Count; i++)
            {
                help.Add(testElements[i].score);
            }

            test.classification = testElements[help.IndexOf(help.Max())].classification;
            textBox2.Text = $"Следующие вычисления показали, что с наибольшей вероятностью тестовый элемент относится к классу {test.classification}";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView2.Columns.Count; i++)
            {
                dataGridView2.Rows[0].Cells[i].Value = DBNull.Value;
            }
            textBox2.Text = "";
            for (int j = 0; j < testElements.Count; j++)
            {             
                testElements[j].score = 0;             
            }
            test.info.Clear();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
