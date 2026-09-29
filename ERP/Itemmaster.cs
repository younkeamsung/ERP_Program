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
    public partial class Itemmaster : Form
    {
        MySqlConnection connection = new MySqlConnection("Server=localhost;Database=ERP_project;Uid=root;Pwd=root;");

        public Itemmaster()
        {
            InitializeComponent();
        }

        /*
         * 상품정보를 조회
         * 상품코드가 입력된 경우 해당 상품코드의 상품정보를 조회
         * 상품명이 입력된 경우 입력된 문자열을 상품명에 포함하는 상품정보를 조회(LIKE%)
         * TABLE : item
         * SQL : SELECT
         */
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string insertQuery = @"SELECT itemcode 상품코드, name 상품명, unit 판매단위, price 가격 FROM item WHERE 1";

                if (txtId.Text != "")
                {
                    insertQuery += " AND itemcode = '" + txtId.Text + "'";
                }

                if (txtName.Text != "")
                {
                    insertQuery += " AND name LIKE '%" + txtName.Text + "%'";
                }
                insertQuery += ";";

                MySqlCommand command = new MySqlCommand(insertQuery, connection);
                connection.Open();

                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                DataTable inoutData = new DataTable();
                adapter.Fill(inoutData);
                dataGridView1.DataSource = inoutData;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            connection.Close();
        }

        /*
         * 상품정보의 모든 필드를 초기화
         */
        private void btnInit_Click(object sender, EventArgs e)
        {
            txtInfoId.Text = "";
            txtInfoName.Text = "";
            txtUnit.Text = "";
            txtPrice.Text = "";
        }

        /*
         * 입력된 상품정보의 모든 필드를 이용하여 상품 데이터 한 행을 생성
         * 상품코드는 기존에 있는 데이터의 상품코드와 중복되면 안됨
         * SQL : INSERT
         */
        private void btnCreate_Click(object sender, EventArgs e)
        {
            string infoId = txtInfoId.Text;
            string infoName = txtInfoName.Text;
            string unit = txtUnit.Text;
            string price = txtPrice.Text;

            string insertQuery = @"INSERT INTO item VALUES ('" + infoId + "', '" + infoName + "', '" + unit + "', " + price + ");";
            MySqlCommand command = new MySqlCommand(insertQuery, connection);
            connection.Open();

            try
            {
                if (command.ExecuteNonQuery() != 0)
                {
                    MessageBox.Show("상품정보가 생성되었습니다. 조회버튼을 눌러 상품현황을 업데이트 해주세요.", "생성성공", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("상품정보가 생성되지 않았습니다. 중복된 상품코드가 있는지 확인하세요", "생성실패", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            connection.Close();
        }

        /*
         * 데이터그리드뷰에서 선택된 상품정보의 상품명, 판매단위, 단가 정보를 업데이트
         * SQL : UPDATE
         */
        private void btnModify_Click(object sender, EventArgs e)
        {
            string infoId = txtInfoId.Text;
            string infoName = txtInfoName.Text;
            string unit = txtUnit.Text;
            string price = txtPrice.Text;

            string insertQuery = @"UPDATE item SET name = '" + infoName + "', unit = '" + unit + "', price = " + price + ""
                                + " WHERE itemcode = '" + infoId + "';";
            MySqlCommand command = new MySqlCommand(insertQuery, connection);
            connection.Open();

            try
            {
                if (command.ExecuteNonQuery() != 0)
                {
                    MessageBox.Show("상품정보가 수정되었습니다. 조회버튼을 눌러 상품현황을 업데이트 해주세요.", "수정성공", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("상품정보가 수정되지 않았습니다. 상품코드가 맞는지 확인하세요", "수정실패", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            connection.Close();
        }

        /*
         * 데이터그리드뷰에서 선택한 상품정보를 삭제
         * SQL : DELETE
         */
        private void btnDelete_Click(object sender, EventArgs e)
        {
            string infoId = txtInfoId.Text;

            string insertQuery = @"DELETE FROM item WHERE itemcode = '" + infoId + "';";
            MySqlCommand command = new MySqlCommand(insertQuery, connection);
            connection.Open();

            try
            {
                if (command.ExecuteNonQuery() != 0)
                {
                    MessageBox.Show("상품정보가 삭제되었습니다. 조회버튼을 눌러 상품현황을 업데이트 해주세요.", "삭제성공", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("상품정보가 삭제되지 않았습니다. 상품코드이 맞는지 확인하세요", "삭제실패", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            connection.Close();
        }

        /*
         * 데이터그리드뷰에서 선택한 제품정보를 제품정보 그룹의 필드에 표시
         */
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtInfoId.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            txtInfoName.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtUnit.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            txtPrice.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
        }
    }
}
