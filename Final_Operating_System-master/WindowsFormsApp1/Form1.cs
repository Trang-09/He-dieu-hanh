using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        int index = 0;
        public Form1()
        {
            InitializeComponent();
            // Kết nối sự kiện CheckedListBox
            checkedListBox1.SelectedIndexChanged += checklistBox_SelectedIndexChanged;

            // Kết nối sự kiện nút "Send"
            Send.Click += sendButton_Click;

            textBox1.KeyDown += TextBox1_KeyDown;
            textBox2.KeyDown += TextBox2_KeyDown;
        }

        

        private void checklistBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Lấy index của item vừa chọn
            if (checkedListBox1.SelectedIndex != -1)
                index = checkedListBox1.SelectedIndex;
            //bỏ chọn các lựa chọn khác
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                checkedListBox1.SetItemChecked(i, false);
            }
            // Chọn lại item hiện tại đang được chọn
            if (checkedListBox1.SelectedIndex != -1)
            {
                checkedListBox1.SetItemChecked(checkedListBox1.SelectedIndex, true);
                label3.Visible = false;
                textBox2.Visible = false;
            }
            // Hiển thị TextBox và DataGridView khi có item được chọn
            if (checkedListBox1.CheckedItems.Count > 0)
            {
                label1.Visible = true;
                textBox1.Visible = true;
                dataGridView1.Visible = false;
                Send.Visible = false;
            }
            else
            {
                label1.Visible = false;
                textBox1.Visible = false;
                dataGridView1.Visible = false;
                Send.Visible = false;
            }


        }
        private void TextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) // Kiểm tra phím Enter
            {
                e.SuppressKeyPress = true; // Ngăn không cho TextBox xử lý Enter mặc định
                if (index == 3)
                {
                    label3.Visible = true;
                    textBox2.Visible = true;
                }
                else
                {
                    inputTextBox_TextChanged();
                    label3.Visible=false;
                    textBox2.Visible = false;
                }
            }
        }
        private void TextBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) 
            {
                e.SuppressKeyPress = true;
                inputTextBox_TextChanged();
            }
        }

        private void inputTextBox_TextChanged()
        {
            if (int.TryParse(textBox1.Text, out int n) && n > 0)
            {
                // Hiển thị DataGridView và tạo bảng
                dataGridView1.Visible = true;
                dataGridView1.Rows.Clear();
                dataGridView1.Columns.Clear();

                // Tạo cột
                dataGridView1.Columns.Add("ArrivalTime", "Arrival Time");
                dataGridView1.Columns.Add("TimeUseCPU", "Time use CPU");

                // Tạo n dòng
                for (int i = 0; i <= n-1; i++)
                {
                    dataGridView1.Rows.Add(i.ToString(), "");
                }

                Send.Visible = true;
            }
            else
            {
                dataGridView1.Visible = false;
                Send.Visible = false;
            }
        }

        private void sendButton_Click(object sender, EventArgs e)
        {
            if (checkedListBox1.CheckedItems.Count > 0)
            {
                List<Process_Class> processes = new List<Process_Class>();

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells[0].Value != null && row.Cells[1].Value != null)
                    {
                        processes.Add(new Process_Class
                        {
                            ArrivalTime = int.Parse(row.Cells[0].Value.ToString()),
                            BurstTime = int.Parse(row.Cells[1].Value.ToString())
                        });
                    }
                }

                if (processes.Count > 0)
                {
                    switch (index)
                    {
                        case 0: //FCFS
                            ResultForm_FCFS resultForm_FCFS = new ResultForm_FCFS(processes);
                            resultForm_FCFS.Show();
                            break;
                        case 1: // SJF
                            ResultForm_SJF resultForm_SJF = new ResultForm_SJF(processes);
                            resultForm_SJF.Show();
                            break;
                        case 2: //SRTN
                            ResultForm_SRTN resultForm_SRTN = new ResultForm_SRTN(processes);
                            resultForm_SRTN.Show();
                            break;
                        case 3:  //Round Robin
                            try
                            {
                                int.TryParse(textBox2.Text, out int n);
                                ResultForm_RoundRobin resultForm_RoundRobin = new ResultForm_RoundRobin(processes, n);
                                resultForm_RoundRobin.Show();
                            }
                            catch(Exception x)
                            { 
                                Console.WriteLine(x.ToString());
                            }
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    MessageBox.Show("Vui lòng nhập dữ liệu hợp lệ!");
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn thuật toán!");
            }
        }
    }

}
