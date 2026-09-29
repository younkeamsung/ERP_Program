using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ERP
{
    public partial class Customer : Form
    {
        MySqlConnection connection = new MySqlConnection("Server=localhost;Database=ERP_project;Uid=root;Pwd=root;");
        public string customerId { get; set; }
        public string customerName {  get; set; }
        public Customer()
        {
            InitializeComponent();
        }

        /*
         * 고객정보를 조회
         * TABLE : customer
         * SQL : SELECT
         */
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string insertQuery = @"select customercode 고객번호, 
                                            name 고객명, 
                                            phone 연락처, 
                                            addr 주소 
                                        from customer;";
                MySqlCommand command = new MySqlCommand(insertQuery, connection);
                connection.Open();

                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                DataTable customerData = new DataTable();
                adapter.Fill(customerData);
                dgvCustomer.DataSource = customerData;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            connection.Close();

        }

        /*
         * 데이터그리드뷰에서 선택한 부서정보를 저장한 후 호출한 윈도우로 RETURN
         */
        private void btnSelect_Click(object sender, EventArgs e)
        {
            if(txtId.Text == "")
            {
                MessageBox.Show("고객을 선택하고 버튼을 누르세요", "고객선택필요", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            customerId = txtId.Text;
            customerName = txtName.Text;
            this.Close();
        }

        /*
         * 데이터그리드뷰에서 선택한 고객 정보를 필드에 표시
         */
        private void dgvCustomer_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtId.Text = dgvCustomer.Rows[e.RowIndex].Cells[0].Value.ToString();
            txtName.Text = dgvCustomer.Rows[e.RowIndex].Cells[1].Value.ToString();
        }
    }
}
