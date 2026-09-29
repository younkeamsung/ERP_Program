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
    public partial class Item : Form
    {
        MySqlConnection connection = new MySqlConnection("Server=localhost;Database=ERP_project;Uid=root;Pwd=root;");
        public string itemId { get; set; }
        public string itemName { get; set; }
        public Item()
        {
            InitializeComponent();
        }

        /*
         * 제품정보 조회
         * TABLE : item
         * SQL : SELECT
         */
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string insertQuery = @"SELECT itemcode 제품코드, 
                                            name 제품명, 
                                            unit 판매단위, 
                                            price 가격 
                                        FROM item;";
                MySqlCommand command = new MySqlCommand(insertQuery, connection);
                connection.Open();

                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                DataTable itemData = new DataTable();
                adapter.Fill(itemData);
                dgvItem.DataSource = itemData;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            connection.Close();
        }

        /*
         * 데이터그리드뷰에서 선택한 제품정보 저장, 호출한 윈도우로 RETURN
         */
        private void btnSelect_Click(object sender, EventArgs e)
        {
            if (txtId.Text == "")
            {
                MessageBox.Show("제품을 선택하고 버튼을 누르세요", "제품선택필요", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            itemId = txtId.Text;
            itemName = txtName.Text;
            this.Close();
        }

        /*
         * 데이터그리드뷰에서 선택한 제품정보를 필드에 표시
         */
        private void dgvItem_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtId.Text = dgvItem.Rows[e.RowIndex].Cells[0].Value.ToString();
            txtName.Text = dgvItem.Rows[e.RowIndex].Cells[1].Value.ToString();
        }
    }
}
