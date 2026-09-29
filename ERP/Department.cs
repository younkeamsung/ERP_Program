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
    public partial class Department : Form
    {
        MySqlConnection connection = new MySqlConnection("Server=localhost;Database=ERP_project;Uid=root;Pwd=root;");
        public string deptNo { get; set; }
        public string deptName { get; set; }
        public Department()
        {
            InitializeComponent();
        }

        /*
         * 부서정보 조회
         * TABLE : department
         * SQL : SELETE
         */
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string insertQuery = @"SELECT dept_no 부서번호, name 부서명 FROM department;";
                MySqlCommand command = new MySqlCommand(insertQuery, connection);
                connection.Open();

                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                DataTable itemData = new DataTable();
                adapter.Fill(itemData);
                dgvDept.DataSource = itemData;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            connection.Close();
        }

        /*
         * 데이터그리드뷰에서 선택한 부서정보를 저장, 호출한 윈도우로 RETURN
         */
        private void btnSelect_Click(object sender, EventArgs e)
        {
            if (txtId.Text == "")
            {
                MessageBox.Show("부서를 선택하고 버튼을 누르세요", "부서선택", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            deptNo = txtId.Text;
            deptName = txtName.Text;
            this.Close();
        }

        /*
         * 데이터그리드뷰에서 선택한 부서정보를 필드에 표시
         */
        private void dgvDept_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtId.Text = dgvDept.Rows[e.RowIndex].Cells[0].Value.ToString();
            txtName.Text = dgvDept.Rows[e.RowIndex].Cells[1].Value.ToString();
        }
    }
}
