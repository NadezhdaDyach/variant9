using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using Variant9.Properties;
using System.Drawing.Imaging;

namespace Variant9
{
    public partial class FormAddEditAgent : Form
    {
        int agentId;
        int maxAgentId;
        bool addAgent=false;
        FormAgents formAgents = new FormAgents();
        string logo = "";
        string defaultPicture = "\\agents\\picture.png";
        private Bitmap bmp;
        string pathToFile;
        DataBase dataBase = new DataBase();
        Dictionary<int, string> agentType = new Dictionary<int, string>();
        Dictionary<string, int> agentTypeReverse = new Dictionary<string, int>();
        string formAgentSort;

        public FormAddEditAgent()
        {
            InitializeComponent();
        }

        public FormAddEditAgent(FormAgents f,int id,string sort)
        {
            InitializeComponent();
            formAgents = f;
            agentId=id;
            formAgentSort = sort;
        }

        public FormAddEditAgent(FormAgents f,bool addAgent, string sort)
        {
            InitializeComponent();
            formAgents = f;
            this.addAgent = addAgent;
            formAgentSort = sort;
        }
        public void SelectAgent()
        {
            string query = "SELECT * FROM [Agent] WHERE ID=" + agentId;
            SqlCommand command = new SqlCommand(query, dataBase.getConnection());
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                textBox1.Text = reader[1].ToString();
                comboBox1.SelectedItem = agentType[Convert.ToInt32(reader[2])];
                textBox3.Text = reader[3].ToString();
                textBox4.Text = reader[4].ToString();
                textBox5.Text = reader[5].ToString();
                textBox6.Text = reader[6].ToString();
                textBox7.Text = reader[7].ToString();
                textBox8.Text = reader[8].ToString();
                pathToFile = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources");
                pictureBox1.Image = new Bitmap(pathToFile + defaultPicture);
                if (reader[9].ToString() != "")
                {
                    logo = reader[9].ToString();
                    pictureBox1.Image = new Bitmap(pathToFile + logo);
                }
                textBox10.Text = reader[10].ToString();
            }
            reader.Close();

        }

        private void FormAddEditAgent_Load(object sender, EventArgs e)
        {
            dataBase.openConnection();
            InitComboBox1();
            if (!addAgent)
                SelectAgent();
            else comboBox1.SelectedIndex = 0;
        } 

        private void FormAddEditAgent_FormClosed(object sender, FormClosedEventArgs e)
        {
            dataBase.closeConnection();
            formAgents.Show();
        }

        private void SelectAgentType()
        {
            agentType.Clear();
            agentTypeReverse.Clear();
            string query = "Select * FROM [AgentType]";
            SqlCommand command = new SqlCommand(query, dataBase.getConnection());
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                agentType.Add(Convert.ToInt32(reader[0]), reader[1].ToString());
                agentTypeReverse.Add(reader[1].ToString(), Convert.ToInt32(reader[0]));
            }
            reader.Close();
        }
        private void SelectMaxAgentId()
        {
            agentType.Clear();
            agentTypeReverse.Clear();
            string query = "SELECT MAX(Id) From [Agent]";
            SqlCommand command = new SqlCommand(query, dataBase.getConnection());
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                agentType.Add(Convert.ToInt32(reader[0]), reader[1].ToString());
                agentTypeReverse.Add(reader[1].ToString(), Convert.ToInt32(reader[0]));
                maxAgentId = Convert.ToInt32(reader[0]);
            }
            reader.Close();
        }

        private void InitComboBox1()
        {
            SelectAgentType();
            foreach (var type in agentType)
            {
                comboBox1.Items.Add(type.Value);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
                        if (addAgent) {
                     if (textBox1.Text.Trim() == "")
                     {
                         MessageBox.Show("Поле \"Наименование\" не может быть пустым!");
                         return;
                     }
                     if (textBox4.Text.Trim() == "")
                     {
                         MessageBox.Show("Поле \"ИНН\" не может быть пустым!");
                         return;
                     }
                     if (textBox7.Text.Trim() == "")
                     {
                         MessageBox.Show("Поле \"Телефон\" не может быть пустым!");
                         return;
                     }
                     if (textBox10.Text.Trim() == "")
                     {
                         MessageBox.Show("Поле \"Приоритет\" не может быть пустым!");
                         return;
                     }
                    try
                    {
                        Bitmap bmpSave = (Bitmap)pictureBox1.Image;
                        bmpSave.Save(pathToFile + "\\agents\\agent_" + (++maxAgentId).ToString() + ".png", ImageFormat.Png);
                        logo = "\\agents\\agent_" + agentId.ToString() + ".png";
                    }
                    catch (Exception) { }
                    InsertIntoAgent();
            }
            else
                 {
                     if (textBox1.Text.Trim() == "")
                     {
                         MessageBox.Show("Поле \"Наименование\" не может быть пустым!");
                         return;
                     }
                     if (textBox4.Text.Trim() == "")
                     {
                         MessageBox.Show("Поле \"ИНН\" не может быть пустым!");
                         return;
                     }
                     if (textBox7.Text.Trim() == "")
                     {
                         MessageBox.Show("Поле \"Телефон\" не может быть пустым!");
                         return;
                     }
                     if (textBox10.Text.Trim() == "")
                     {
                         MessageBox.Show("Поле \"Приоритет\" не может быть пустым!");
                         return;
                     }
                    try
                    {
                        Bitmap bmpSave = (Bitmap)pictureBox1.Image;
                        bmpSave.Save(pathToFile + "\\agents\\agent_" + agentId.ToString() + ".png", ImageFormat.Png);
                        logo = "\\agents\\agent_" + agentId.ToString() + ".png";
                    }
                    catch (Exception) { }
                    UpdateAgent();
                    

            }

                 this.Close();
                 formAgents.Show();
                 formAgents.SelectAgents(formAgentSort);
                 formAgents.InitDataGridView1();
                 formAgents.Filter();
             }

             private void InsertIntoAgent()
             {
                 String sqlExpression = "INSERT INTO Agent VALUES (" +
                     "'" + textBox1.Text + "'" +
                     "," + agentTypeReverse[comboBox1.SelectedItem.ToString()] +
                     ",'" + textBox3.Text + "'" +
                     ",'" + textBox4.Text + "'" +
                     ",'" + textBox5.Text + "'" +
                     ",'" + textBox6.Text + "'" +
                     ",'" + textBox7.Text + "'" +
                     ",'" + textBox8.Text + "'" +
                     ",'" + logo + "'" +
                     "," + textBox10.Text + ");"; 
                 SqlCommand command = new SqlCommand(sqlExpression, dataBase.getConnection());
                 command.ExecuteNonQuery();
                 MessageBox.Show("Агент добавлен");
             }

             private void UpdateAgent()
             {

                 String sqlExpression = "UPDATE Agent SET " + 
                     "[Title]='" + textBox1.Text + "'" +
                     ",[AgentTypeID]=" + agentTypeReverse[comboBox1.SelectedItem.ToString()] +
                     ",[Address]='" + textBox3.Text + "'" +
                     ",[INN]='" + textBox4.Text + "'" +
                     ",[KPP]='" + textBox5.Text + "'" +
                     ",[DirectorName]='" + textBox6.Text + "'" +
                     ",[Phone]='" + textBox7.Text + "'" +
                     ",[Email]='" + textBox8.Text + "'" +
                     ",[Logo]='" + logo + "'" +
                     ",[Priority]=" + textBox10.Text + 
                     " WHERE ID=" + agentId;
                 SqlCommand command = new SqlCommand(sqlExpression, dataBase.getConnection());
                 command.ExecuteNonQuery();
                 MessageBox.Show("Изменения сохранены");
             }

             private void button3_Click(object sender, EventArgs e)
             {
                 try
                 {
                     DeleteAgent();
                     MessageBox.Show("Агент удален");
                 }
                 catch (Exception)
                 {

                 }
                 this.Close();
                 formAgents.Show();
                 formAgents.SelectAgents(formAgentSort);
                 formAgents.InitDataGridView1();
                 formAgents.Filter();
             }

             private void DeleteAgent()
             {
                     String sqlExpression = "DELETE FROM [Agent] WHERE ID=" + agentId;
                     SqlCommand command = new SqlCommand(sqlExpression, dataBase.getConnection());
                     command.ExecuteNonQuery();


             }

             private void button2_Click(object sender, EventArgs e)
             {
                 this.Close();
                 formAgents.Show();
             }

             private void buttonDeleteImage_Click(object sender, EventArgs e)
             {
                 string logo = "";
                 pictureBox1.Image = new Bitmap(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources")+defaultPicture);
             }

             private void buttonOpen_Click(object sender, EventArgs e)
             {
                 // Описываем объект класса OpenFileDialog 
                 OpenFileDialog dialog = new OpenFileDialog();
                 // Задаем расширения файлов 
                 dialog.Filter = "Image files (*.BMP, *.JPG, " + "*.GIF, *.PNG)|*.bmp;*.jpg;*.gif;*.png"; 
                 // Вызываем диалог и проверяем выбран ли файл 
                 if (dialog.ShowDialog() == DialogResult.OK)
                 {
                     // Загружаем изображение из выбранного файла 
                     Image image = Image.FromFile(dialog.FileName);
                     int width = image.Width; int height = image.Height;

                     // Создаем и загружаем изображение в формате bmp 
                     bmp = new Bitmap(image, width, height);
                     // Записываем изображение в pictureBox1 
                     pictureBox1.Image = bmp;
                     pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                 }
        }
    }
}
