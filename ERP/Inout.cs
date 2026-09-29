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
    public partial class Inout : Form
    {
        MySqlConnection connection = new MySqlConnection("Server=localhost;Database=ERP_project;Uid=root;Pwd=root;");

        public Inout()
        {
            InitializeComponent();
        }

        /*
         * 입출고 데이터를 조회한다.
         * 입출고 기간과, 상품이 조회조건으로 입력되면 해당 조건에 맞는 입출고 데이터를 조회한다
         * TABLE : inandout
         * SQL : SELECT
         */
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string insertQuery = @"SELECT A.inoutcode 입출고번호, A.itemcode 상품코드, B.name 상품명, A.qty 입출고수량, A.date 입출고일자 
                                    FROM inandout A, item B 
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
                DataTable inoutData = new DataTable();
                adapter.Fill(inoutData);
                dgvInout.DataSource = inoutData;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            connection.Close();
        }

        /*
         * 입출고번호, 상품, 수량, 입출고일자를 입력받아 입출고 데이터 한 행을 생성한다
         * SQL : INSERT
         */
        private void btnCreate_Click(object sender, EventArgs e)
        {
            string InoutCode = txtCode.Text;
            string itemCode = txtItemIdDetail.Text;
            string date = dtpDate.Value.ToString("yyyy-MM-dd");
            string qty = txtQty.Text;

            string insertQuery = @"INSERT INTO inandout VALUES ('" + InoutCode + "', '" + itemCode + "', " + qty + ", '" + date + "');";
            MySqlCommand command = new MySqlCommand(insertQuery, connection);
            connection.Open();

            try
            {
                if (command.ExecuteNonQuery() != 0)
                {
                    MessageBox.Show("입출고정보가 생성되었습니다. 조회버튼을 눌러 입출고내역을 업데이트 해주세요.", "생성성공", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("입출고정보가 생성되지 않았습니다. 중복된 입출고번호가 있는지 확인하세요", "생성실패", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            connection.Close();
        }

        /*
         * 데이터그리드뷰에서 선택한 입출고번호의 상품, 수량, 입출고일자를 업데이트한다
         * SQL : UPDATE 
         */
        private void btnModify_Click(object sender, EventArgs e)
        {
            string InoutCode = txtCode.Text;
            string itemCode = txtItemIdDetail.Text;
            string date = dtpDate.Value.ToString("yyyy-MM-dd");
            string qty = txtQty.Text;

            string insertQuery = @"UPDATE inandout SET itemcode = '" + itemCode + "', date = '" +
                                date + "', qty = " + qty + " WHERE InoutCode = '" + InoutCode + "';";
            MySqlCommand command = new MySqlCommand(insertQuery, connection);
            connection.Open();

            try
            {
                if (command.ExecuteNonQuery() != 0)
                {
                    MessageBox.Show("입출고정보가 수정되었습니다. 조회버튼을 눌러 입출고내역을 업데이트 해주세요.", "수정성공", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("입출고정보가 수정되지 않았습니다. 입출고번호가 맞는지 확인하세요", "수정실패", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            connection.Close();
        }

        /*
         * 데이터그리드뷰에서 선택한 입출고번호의 데이터 한 행을 삭제한다
         * SQL : DELETE
         */
        private void btnDelete_Click(object sender, EventArgs e)
        {
            string inoutCode = txtCode.Text;

            string insertQuery = @"DELETE FROM inandout WHERE InoutCode = '" + inoutCode + "';";
            MySqlCommand command = new MySqlCommand(insertQuery, connection);
            connection.Open();

            try
            {
                if (command.ExecuteNonQuery() != 0)
                {
                    MessageBox.Show("입출고정보가 삭제되었습니다. 조회버튼을 눌러 입출고내역을 업데이트 해주세요.", "삭제성공", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("입출고정보가 삭제되지 않았습니다. 입출고번호가 맞는지 확인하세요", "삭제실패", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            connection.Close();
        }

        /*
         * 입출고번호, 상품, 수량, 입출일자 텍스트박스를 초기화한다
         */
        private void btnInit_Click(object sender, EventArgs e)
        {
            txtCode.Text = "";
            txtItemIdDetail.Text = "";
            txtItemNameDetail.Text = "";
            dtpDate.Value = DateTime.Now;
            txtQty.Text = "";
        }
        /*
         * 입출고조회 그룹의 상품을 조회하기 위한 서브윈도우폼을 오픈한다
         * 소스코드 : Item.cs
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
         * 입출고상세 그룹의 상품을 조회하기 위한 서브윈도우폼을 오픈한다
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

        /*
         * 데이터그리드뷰에 조회된 데이터 중 선택된 행의 입출고번호, 상품, 수량, 입출고일자 정보를
         * 입출고상세 그룹에 표시한다.
         */
        private void dgvInout_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtCode.Text = dgvInout.Rows[e.RowIndex].Cells[0].Value.ToString();
            txtItemIdDetail.Text = dgvInout.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtItemNameDetail.Text = dgvInout.Rows[e.RowIndex].Cells[2].Value.ToString();
            txtQty.Text = dgvInout.Rows[e.RowIndex].Cells[3].Value.ToString();
            dtpDate.Value = DateTime.ParseExact(dgvInout.Rows[e.RowIndex].Cells[4].Value.ToString(), "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}
