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
    public partial class Deptmaster : Form
    {
        MySqlConnection connection = new MySqlConnection("Server=localhost;Database=ERP_project;Uid=root;Pwd=root;");
        public Deptmaster()
        {
            InitializeComponent();
        }

        /*
         * 부서현황그룹에 있는 데이터그리드뷰의 데이터를 조회하는매서드
         * SQL : SELECT
         */
        private void ReSetGridView()
        {
            try
            {
                string insertQuery = @"SELECT dept_no 부서번호, name 부서명 FROM department WHERE 1;";

                MySqlCommand command = new MySqlCommand(insertQuery, connection);

                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                DataTable accountData = new DataTable();
                adapter.Fill(accountData);
                dataGridView1.DataSource = accountData;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /*
         * 부서정보를 조회
         * TABLE : department
         * SQL : SELECT
         * CLASS : ReSetGridView()
         */
        private void btnSearch_Click(object sender, EventArgs e)
        {
            connection.Open();
            ReSetGridView();
            connection.Close();
        }

        /*
         * 부서정보 그룹의 모든 필드를 초기화
         */
        private void btnInit_Click(object sender, EventArgs e)
        {
            txtId.Text = "";
            txtName.Text = "";
        }

        /*
         * 입력된 부서정보 그룹의 필드 데이터를 이용하여 부서 데이터 한 행을 생성 후 성공하면
         * 데이터그리드뷰의 데이터를 다시 출력해준다
         * 부서코드는 중복되면안됨
         * SQL : INSERT
         */
        private void btnCreate_Click(object sender, EventArgs e)
        {
            string Id = txtId.Text;
            string Name = txtName.Text;

            string insertQuery = @"INSERT INTO department VALUES ('" + Id + "', '" + Name + "');";
            MySqlCommand command = new MySqlCommand(insertQuery, connection);
            connection.Open();

            try
            {
                if (command.ExecuteNonQuery() != 0)
                {
                    ReSetGridView();
                    MessageBox.Show("부서정보가 생성되었습니다. 조회버튼을 눌러 부서현황을 업데이트 해주세요.", "생성성공", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("부서정보가 생성되지 않았습니다. 중복된 부서번호가 있는지 확인하세요", "생성실패", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            connection.Close();
        }

        /*
         * 데이터그리드뷰에서 선택된 부서정보를 업데이트(부셔명)후 업데이트에 성공되면
         * 데이터그리드뷰의 데이터를 다시 출력해준다
         * SQL : UPDATE
         */
        private void btnModify_Click(object sender, EventArgs e)
        {
            string Id = txtId.Text;
            string Name = txtName.Text;

            string insertQuery = @"UPDATE department SET name = '" + Name + "' WHERE dept_no = '" + Id + "';";
            MySqlCommand command = new MySqlCommand(insertQuery, connection);
            connection.Open();

            try
            {
                if (command.ExecuteNonQuery() != 0)
                {
                    ReSetGridView();
                    MessageBox.Show("부서명이 수정되었습니다. 조회버튼을 눌러 부서현황을 업데이트 해주세요.", "수정성공", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("부서명이 수정되지 않았습니다. 부서번호가 맞는지 확인하세요", "수정실패", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            connection.Close();
        }

        /*
         * 데이터그리드뷰에서 선택된 부서정보를 삭제후 업데이트에 성공되면
         * 데이터그리드뷰의 데이터를 다시 출력해준다
         * SQL : DELETE
         */
        private void btnDelete_Click(object sender, EventArgs e)
        {
            string Id = txtId.Text;

            string insertQuery = @"DELETE FROM department WHERE dept_no = '" + Id + "';";
            MySqlCommand command = new MySqlCommand(insertQuery, connection);
            connection.Open();

            try
            {
                if (command.ExecuteNonQuery() != 0)
                {
                    ReSetGridView();
                    MessageBox.Show("부서정보가 삭제되었습니다. 조회버튼을 눌러 부서현황을 업데이트 해주세요.", "삭제성공", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("부서정보가 삭제되지 않았습니다. 부서번호가 맞는지 확인하세요", "삭제실패", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            connection.Close();
        }

        /*
         * 데이터그리드뷰에서 선택한 행의 부서정보를 부서정보 그룹의 필드에 표시
         */
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtId.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            txtName.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
        }
    }
}
