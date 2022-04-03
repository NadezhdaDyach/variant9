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

namespace Variant9
{
    public partial class FormAgents : Form
    {
        DataBase dataBase = new DataBase();
        List<Agent> agents = new List<Agent>();
        Dictionary<int, string> productSaleCount = new Dictionary<int, string>();
        Dictionary<int, string> agentType = new Dictionary<int, string>();
        string sort = " ORDER BY Title Asc";
        public FormAgents()
        {
            InitializeComponent();
        }

        private void FormAgents_Load(object sender, EventArgs e)
        {
            dataBase.openConnection();
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            SelectProductSaleCount();
            SelectAgentType();
            InitComboBox2();
            SelectAgents(sort);
            InitDataGridView1();
            //Filter();
        }

        private void InitComboBox2()
        {
            foreach(var type in agentType)
            {
                comboBox2.Items.Add(type.Value);
            }
        }

        public void InitDataGridView1()
        {
            dataGridView1.Rows.Clear();
            int i = 0;
            foreach (Agent agent in agents) {
                if (textBox1.Text.Trim() == "" || (agent.Title.ToUpper()).Contains(textBox1.Text.Trim().ToUpper()) || agent.Email.ToUpper().Contains(textBox1.Text.Trim().ToUpper()) || agent.Phone.ToUpper().Contains(textBox1.Text.Trim().ToUpper())) 
                {
                    dataGridView1.Rows.Add();
                    dataGridView1[0, i].Value = agent.Title;
                    try
                    {
                        dataGridView1[1, i].Value = productSaleCount[agent.Id];
                    }
                    catch (Exception)
                    {

                    }
                    dataGridView1[3, i].Value = agent.Phone;
                    dataGridView1[4, i].Value = agent.Email;
                    try
                    {
                        dataGridView1[5, i].Value = agentType[agent.AgentTypeId];
                    }
                    catch (Exception)
                    {

                    }
                    dataGridView1[6, i].Value = agent.Priority;
                    dataGridView1[7, i].Value = agent.Id;
                    i++;
                }
            }
        }

        public void SelectAgents(string sort)
        {
            agents.Clear();
            string query = "SELECT * FROM [Agent]"+sort;
            SqlCommand command = new SqlCommand(query, dataBase.getConnection());
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                dataGridView1.Rows.Add();
                Agent agent = new Agent(Convert.ToInt32(reader[0]),reader[1].ToString(), Convert.ToInt32(reader[2]), reader[3].ToString(), reader[4].ToString(), reader[5].ToString(), reader[6].ToString(), reader[7].ToString(), reader[8].ToString(), reader[9].ToString(), Convert.ToInt32(reader[10]));
                agents.Add(agent);
            }
            reader.Close();

        }

        private void SelectProductSaleCount()
        {
            productSaleCount.Clear();
            string query = "Select AgentID,Count(ProductID) as [Count] From ProductSale WHERE SaleDate between \'2022-01-01 00:00:00\'  AND  \'2023-01-01 00:00:00\' GROUP BY AgentID";
            SqlCommand command = new SqlCommand(query, dataBase.getConnection());
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                productSaleCount.Add(Convert.ToInt32(reader[0]),reader[1].ToString());
            }
            reader.Close();
        }

        private void SelectAgentType()
        {
            agentType.Clear();
            string query = "Select * FROM [AgentType]";
            SqlCommand command = new SqlCommand(query, dataBase.getConnection());
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                agentType.Add(Convert.ToInt32(reader[0]), reader[1].ToString());
            }
            reader.Close();
        }

        private void FormAgents_FormClosed(object sender, FormClosedEventArgs e)
        {
            dataBase.closeConnection();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            InitDataGridView1();
            comboBox2.SelectedIndex = 0;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Sort(comboBox1.SelectedItem);
            comboBox2.SelectedIndex = 0;
        }

        private void Sort(object selectedItem)
        {
            if (selectedItem.Equals("По возрастанию: наименование"))
            {
                sort = " ORDER BY Title Asc";
                SelectAgents(sort);
                InitDataGridView1();
            }
            else if (selectedItem.Equals("По убыванию: наименование"))
            {
                sort = " ORDER BY Title DESC";
                SelectAgents(sort);
                InitDataGridView1();
            }
            else if (selectedItem.Equals("По возрастанию: приоритет"))
            {
                sort = " ORDER BY Priority ASC";
                SelectAgents(sort);
                InitDataGridView1();
            }
            else if (selectedItem.Equals("По убыванию: приоритет"))
            {
                sort = " ORDER BY Priority DESC";
                SelectAgents(sort);
                InitDataGridView1();
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            InitDataGridView1();
            Filter();
        }
        public void Filter()
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (comboBox2.SelectedIndex==0)
                    break;
                if (!dataGridView1[5, i].Value.ToString().Equals(comboBox2.SelectedItem.ToString()))
                {
                    dataGridView1.Rows.RemoveAt(i);
                    i--;
                }

            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            FormAddEditAgent f = new FormAddEditAgent(this,Convert.ToInt32(dataGridView1[7,e.RowIndex].Value),sort);
            f.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FormAddEditAgent f = new FormAddEditAgent(this,true,sort);
            f.Show();
            this.Hide();
        }
    }
}
