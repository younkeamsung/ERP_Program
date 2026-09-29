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
    public partial class Custmaster : Form
    {
        MySqlConnection connection = new MySqlConnection("Server=localhost;Database=ERP_project;Uid=root;Pwd=root;");

        public Custmaster()
        {
            InitializeComponent();
        }

        /*
         * 데이터그리드뷰에서 선택된 데이터 행의 정보를 고객정보 그룹의 필드에 표시
         */
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtInfoId.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            txtInfoName.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtPhone.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            txtAddr.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
        }

        /*
         * 고객 정보를 조회
         * 고객코드가 조회조건으로 입력된 경우 해당 고객코드의 고객정보를 조회
         * 고객명이 입력된 경우 입력된 문자열이 고객명에 포함된 모든 고객정보를 조회
         * TABLE : customer
         * SQL : SELECT
         */
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string insertQuery = @"select customercode 고객코드, name 이름, phone 전화번호, addr 주소 from customer where 1";

                if (txtId.Text != "")
                {
                    insertQuery += " AND customercode = '" + txtId.Text + "'";
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
         * 고객정보 그룹의 모든 필드를 초기화
         */
        private void btnInit_Click(object sender, EventArgs e)
        {
            txtInfoId.Text = "";
            txtInfoName.Text = "";
            txtAddr.Text = "";
            txtPhone.Text = "";
        }

        /*
         * 고객정보 그룹의 필드에 입력된 데이터로 고객정보를 생성
         * 고객코드는 중복되면 안됨
         * SQL : INSERT
         */
        private void btnCreate_Click(object sender, EventArgs e)
        {
            string infoId = txtInfoId.Text;
            string infoName = txtInfoName.Text;
            string addr = txtAddr.Text;
            string phone = txtPhone.Text;

            string insertQuery = @"INSERT INTO customer VALUES ('" + infoId + "', '" + infoName + "', '" + phone + "', '" + addr + "');";
            MySqlCommand command = new MySqlCommand(insertQuery, connection);
            connection.Open();

            try
            {
                if (command.ExecuteNonQuery() != 0)
                {
                    MessageBox.Show("고객정보가 생성되었습니다. 조회버튼을 눌러 고객현황을 업데이트 해주세요.", "생성성공", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("고객정보가 생성되지 않았습니다. 중복된 고객코드가 있는지 확인하세요", "생성실패", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            connection.Close();
        }
        
        /*
         * 데이터 그리드뷰에서 선택한 고객정보를 입력된 필드 내용으로 업데이트
         * SQL : UPDATE
         */
        private void btnModify_Click(object sender, EventArgs e)
        {
            string infoId = txtInfoId.Text;
            string infoName = txtInfoName.Text;
            string addr = txtAddr.Text;
            string phone = txtPhone.Text;

            string insertQuery = @"UPDATE customer SET name = '" + infoName + "', addr = '" + addr + "', phone = '" + phone + "'"
                                + " WHERE customercode = '" + infoId + "';";
            MySqlCommand command = new MySqlCommand(insertQuery, connection);
            connection.Open();

            try
            {
                if (command.ExecuteNonQuery() != 0)
                {
                    MessageBox.Show("고객정보가 수정되었습니다. 조회버튼을 눌러 고객현황을 업데이트 해주세요.", "수정성공", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("고객정보가 수정되지 않았습니다. 고객코드가 맞는지 확인하세요", "수정실패", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            connection.Close();
        }

        /*
         * 데이터그리드뷰에서 선택된 고객정보를 삭제
         * SQL : DELETE
         */
        private void btnDelete_Click(object sender, EventArgs e)
        {
            string infoId = txtInfoId.Text;

            string insertQuery = @"DELETE FROM customer WHERE customercode = '" + infoId + "';";
            MySqlCommand command = new MySqlCommand(insertQuery, connection);
            connection.Open();

            try
            {
                if (command.ExecuteNonQuery() != 0)
                {
                    MessageBox.Show("고객정보가 삭제되었습니다. 조회버튼을 눌러 고객현황을 업데이트 해주세요.", "삭제성공", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("고객정보가 삭제되지 않았습니다. 고객코드이 맞는지 확인하세요", "삭제실패", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            connection.Close();
        }
    }
}
