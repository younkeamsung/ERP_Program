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
    public partial class Production : Form
    {
        MySqlConnection connection = new MySqlConnection("Server=localhost;Database=ERP_project;Uid=root;Pwd=root;");

        public Production()
        {
            InitializeComponent();
        }

        /*
         * 조회조건 (기간, 상품) 에 맞는 주문 정보를 datagridview 에 로딩
         * TABLE : production
         * SQL : SELECT
         */
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string insertQuery = @"SELECT A.prodcode 생산번호, A.itemcode 생산품코드, B.name 생산품명, A.qty 생산수량, A.date 생산일자 
                                    FROM production A, item B 
                                    WHERE A.itemcode = B.itemcode";

                insertQuery += @" AND A.date >= '" + dtpFrom.Value.ToString("yyyy-MM-dd")
                             + "' AND A.date <= '" + dtpTo.Value.ToString("yyyy-MM-dd") + "'";

                if (txtItemId.Text != "")
                {
                    insertQuery += " AND A.itemcode = '" + txtItemId.Text + "'";
                }
                insertQuery += ";";

                MySqlCommand command = new MySqlCommand(insertQuery, connection);
                connection.Open();

                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                DataTable prodData = new DataTable();
                adapter.Fill(prodData);
                dgvProduction.DataSource = prodData;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            connection.Close();
        }

        /*
         * 입력된 생산번호, 상품코드, 상품명, 생산수량, 생산일자 로 생산 데이터 한 행을 생성한다
         * 생산번호가 이미 있는 데이터와 중복되면 안된다.
         * SQL : INSERT
         */
        private void btnCreate_Click(object sender, EventArgs e)
        {
            string prodCode = txtProdCode.Text;
            string itemCode = txtItemIdDetail.Text;
            string date = dtpProdDate.Value.ToString("yyyy-MM-dd");
            string qty = txtQty.Text;

            string insertQuery = @"INSERT INTO production VALUES ('" + prodCode + "', '" + itemCode + "', " + qty + ", '" + date + "');";
            MySqlCommand command = new MySqlCommand(insertQuery, connection);
            connection.Open();

            try
            {
                if (command.ExecuteNonQuery() != 0)
                {
                    MessageBox.Show("생산정보가 생성되었습니다. 조회버튼을 눌러 생산내역을 업데이트 해주세요.", "생성성공", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("생산정보가 생성되지 않았습니다. 중복된 생산번호가 있는지 확인하세요", "생성실패", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            connection.Close();
        }

        /*
         * Data Grid View 에서 한 행을 클릭하면 해당 행의 생산코드, 상품코드, 상품명, 생산수량, 생산일자 를
         * 생산상세 그룹에 표시한다
         */
        private void dgvProduction_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtProdCode.Text = dgvProduction.Rows[e.RowIndex].Cells[0].Value.ToString();
            txtItemIdDetail.Text = dgvProduction.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtItemNameDetail.Text = dgvProduction.Rows[e.RowIndex].Cells[2].Value.ToString();
            txtQty.Text = dgvProduction.Rows[e.RowIndex].Cells[3].Value.ToString();
            dtpProdDate.Value = DateTime.ParseExact(dgvProduction.Rows[e.RowIndex].Cells[4].Value.ToString(), "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
        }

        /*
         * 조회조건에서 상품 정보를 search
         * Item 윈도우폼 클래스 호출
         * 해당 윈도우폼에서 선택한 상품정보를 가져와서 텍스트박스에 display
         */
        private void btnItemSearch_Click(object sender, EventArgs e)
        {
            Item itemForm = new Item();
            itemForm.ShowDialog();
            if (itemForm.itemId != "")
            {
                txtItemId.Text = itemForm.itemId;
                txtItemName.Text = itemForm.itemName;
            }
        }

        /*
         * 데이터그리드뷰에서 선택한 생산코드의 상품, 수량, 일자 데이터를 업데이트한다.
         * SQL : UPDATE
         */
        private void btnModify_Click(object sender, EventArgs e)
        {
            string prodCode = txtProdCode.Text;
            string itemCode = txtItemIdDetail.Text;
            string date = dtpProdDate.Value.ToString("yyyy-MM-dd");
            string qty = txtQty.Text;

            string insertQuery = @"UPDATE production SET itemcode = '" + itemCode + "', date = '" +
                                date + "', qty = " + qty + " WHERE prodcode = '" + prodCode + "';";
            MySqlCommand command = new MySqlCommand(insertQuery, connection);
            connection.Open();

            try
            {
                if (command.ExecuteNonQuery() != 0)
                {
                    MessageBox.Show("생산정보가 수정되었습니다. 조회버튼을 눌러 생산내역을 업데이트 해주세요.", "수정성공", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("생산정보가 수정되지 않았습니다. 주문번호가 맞는지 확인하세요", "수정실패", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            connection.Close();
        }

        /*
         * 데이터그리드뷰에서 선택한 생산코드의 데이터 행을 삭제한다
         * SQL : DELETE
         */
        private void btnDelete_Click(object sender, EventArgs e)
        {
            string prodCode = txtProdCode.Text;

            string insertQuery = @"DELETE FROM production WHERE prodcode = '" + prodCode + "';";
            MySqlCommand command = new MySqlCommand(insertQuery, connection);
            connection.Open();

            try
            {
                if (command.ExecuteNonQuery() != 0)
                {
                    MessageBox.Show("생산정보가 삭제되었습니다. 조회버튼을 눌러 생산내역을 업데이트 해주세요.", "삭제성공", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("생산정보가 삭제되지 않았습니다. 주문번호가 맞는지 확인하세요", "삭제실패", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            connection.Close();
        }

        /*
         * 생산상세의 모든 텍스트박스 내용을 초기화한다
         */
        private void btnInit_Click(object sender, EventArgs e)
        {
            txtProdCode.Text = "";
            txtItemIdDetail.Text = "";
            txtItemNameDetail.Text = "";
            dtpProdDate.Value = DateTime.Now;
            txtQty.Text = "";
        }

        /*
         * 생산상세에서 상품 조회를 위한 서브 윈도우 폼을 오픈한다
         * 소스코드 : Item.cs
         */
        private void btnItemDetail_Click(object sender, EventArgs e)
        {
            Item itemForm = new Item();
            itemForm.ShowDialog();
            if (itemForm.itemId != "")
            {
                txtItemIdDetail.Text = itemForm.itemId;
                txtItemNameDetail.Text = itemForm.itemName;
            }
        }
    }
}
