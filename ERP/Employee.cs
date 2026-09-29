using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace ERP
{
    public partial class Employee : Form
    {
        MySqlConnection connection = new MySqlConnection("Server=localhost;Database=ERP_project;Uid=root;Pwd=root;");

        public Employee()
        {
            InitializeComponent();
        }

        /*
         * 모든 직원정보를 조회 
         * 사번이 입력된 경우는 해당 사번의 직원정보를 조회
         * 이름이 입력된 경우는 입력된 문자열이 이름에 포함된 직원정보를 조회(SQL : LIKE %)
         */
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string insertQuery = @"select A.emp_no 사번, A.name 이름, A.id 로그인ID, A.addr 주소, A.phone 전화번호, A.birth 생년월일,
                                              A.email 이메일, A.enter_date 입사일, A.dept_no 부서번호, B.name 부서명, A.quit_date 퇴사일 
                                        from employee A, department B where A.dept_no = B.dept_no";

                if (txtId.Text != "")
                {
                    insertQuery += " AND A.emp_no = '" + txtId.Text + "'";
                }

                if(txtName.Text != "")
                {
                    insertQuery += " AND A.name LIKE '%" + txtName.Text + "%'";
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
         * 직원정보를 추가하기 전에 입력한 로그인계정이 이미 존재하는지 체크
         * TABLE : user
         */
        private void btnDuplicate_Click(object sender, EventArgs e)
        {
            string loginId = txtLoginId.Text;


            if (loginId == "")
            {
                MessageBox.Show("사용할 로그인 계정을 입력하세요", "로그인 계정 입력", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string insertQuery = @"SELECT COUNT(*) cnt FROM user WHERE id = '" + loginId + "';";
            MySqlCommand command = new MySqlCommand(insertQuery, connection);
            connection.Open();

            MySqlDataReader reader = command.ExecuteReader();

            bool isDuplicated = true;
            while (reader.Read())
            {
                if (reader["cnt"].ToString() == "0")
                {
                    isDuplicated = false;
                }
            }

            if (isDuplicated)
            {
                MessageBox.Show("이미 사용중인 로그인 계정입니다.", "중복", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                MessageBox.Show("사용가능한 로그인 계정입니다.", "성공", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            connection.Close();
        }

        /*
         * 부서정보를 조회하기 위해서 부서 서브윈도우폼을 오픈
         * 소스코드 : Department.cs
         */
        private void btnDept_Click(object sender, EventArgs e)
        {
            Department department = new Department();
            department.ShowDialog();
            if (department.deptNo != "")
            {
                txtDeptNo.Text = department.deptNo;
                txtDeptName.Text = department.deptName;
            }
        }

        /*
         * 직원정보에 있는 모든 필드를 초기화
         */
        private void btnInit_Click(object sender, EventArgs e)
        {
            txtInfoId.Text = "";
            txtInfoName.Text = "";
            txtLoginId.Text = "";
            txtAddr.Text = "";
            txtPhone.Text = "";
            dtpBirth.Value = DateTime.Now;
            txtEmail.Text = "";
            dtpEnter.Value = DateTime.Now;
            txtDeptNo.Text = "";
            txtDeptName.Text = "";
            txtQuit.Text = "";
        }

        /*
         * 직원정보에 있는 필드를 입력받은 후 직원 데이터 한 행을 생성
         * SQL : INSERT
         */
        private void btnCreate_Click(object sender, EventArgs e)
        {
            string infoId = txtInfoId.Text;
            string infoName = txtInfoName.Text;
            string loginId = txtLoginId.Text;
            string addr = txtAddr.Text;
            string phone = txtPhone.Text;
            string birth = dtpBirth.Value.ToString("yyyy-MM-dd");
            string email = txtEmail.Text;
            string enter = dtpEnter.Value.ToString("yyyy-MM-dd");
            string deptNo = txtDeptNo.Text;
            string quit = txtQuit.Text;

            string insertQuery = @"INSERT INTO employee VALUES ('" + infoId + "', '" + infoName + "', '" + loginId + "', '" + addr + "', '"
                                    + phone + "', '" + birth + "', '" + email + "', '" + enter + "', '" + deptNo + "', '" + quit + "');"; 
            MySqlCommand command = new MySqlCommand(insertQuery, connection);
            connection.Open();

            try
            {
                if (command.ExecuteNonQuery() != 0)
                {
                    MessageBox.Show("직원정보가 생성되었습니다. 조회버튼을 눌러 직원현황을 업데이트 해주세요.", "생성성공", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("직원정보가 생성되지 않았습니다. 중복된 사번이 있는지 확인하세요", "생성실패", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            connection.Close();
        }

        /*
         * 데이터그리드뷰에서 선택한 사번으로 사번 이외의 정보를 업데이트
         * SQL : UPDATE
         */
        private void btnModify_Click(object sender, EventArgs e)
        {
            string infoId = txtInfoId.Text;
            string infoName = txtInfoName.Text;
            string loginId = txtLoginId.Text;
            string addr = txtAddr.Text;
            string phone = txtPhone.Text;
            string birth = dtpBirth.Value.ToString("yyyy-MM-dd");
            string email = txtEmail.Text;
            string enter = dtpEnter.Value.ToString("yyyy-MM-dd");
            string deptNo = txtDeptNo.Text;
            string quit = txtQuit.Text;

            string insertQuery = @"UPDATE employee SET name = '" + infoName + "', id = '" + loginId  + "', addr = '" + addr 
                                + "', phone = '" + phone + "', birth = '" + birth + "', email = '" + email + "', enter_date = '"
                                + enter + "', dept_no = '" + deptNo + "', quit_date = '" + quit + "'"
                                + " WHERE emp_no = '" + infoId + "';";
            MySqlCommand command = new MySqlCommand(insertQuery, connection);
            connection.Open();

            try
            {
                if (command.ExecuteNonQuery() != 0)
                {
                    MessageBox.Show("직원정보가 수정되었습니다. 조회버튼을 눌러 직원내역을 업데이트 해주세요.", "수정성공", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("직원정보가 수정되지 않았습니다. 사번이 맞는지 확인하세요", "수정실패", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            connection.Close();
        }

        /*
         * 데이터그리드뷰에서 선택한 사번의 직원정보를 테이블에서 삭제
         * SQL : DELETE
         */
        private void btnDelete_Click(object sender, EventArgs e)
        {
            string infoId = txtInfoId.Text;

            string insertQuery = @"DELETE FROM employee WHERE emp_no = '" + infoId + "';";
            MySqlCommand command = new MySqlCommand(insertQuery, connection);
            connection.Open();

            try
            {
                if (command.ExecuteNonQuery() != 0)
                {
                    MessageBox.Show("직원정보가 삭제되었습니다. 조회버튼을 눌러 직원현황을 업데이트 해주세요.", "삭제성공", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("직원정보가 삭제되지 않았습니다. 사번이 맞는지 확인하세요", "삭제실패", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            connection.Close();
        }

        /*
         * 데이터그리드뷰에 조회된 데이터 중 클릭한 데이터 행의 정보를 직원정보 그룹의 필드에 표시
         */ 
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtInfoId.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            txtInfoName.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtLoginId.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            txtAddr.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            txtPhone.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
            dtpBirth.Value = DateTime.ParseExact(dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString(), "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            txtEmail.Text = dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString();
            dtpEnter.Value = DateTime.ParseExact(dataGridView1.Rows[e.RowIndex].Cells[7].Value.ToString(), "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            txtDeptNo.Text = dataGridView1.Rows[e.RowIndex].Cells[8].Value.ToString();
            txtDeptName.Text = dataGridView1.Rows[e.RowIndex].Cells[9].Value.ToString();
            txtQuit.Text = dataGridView1.Rows[e.RowIndex].Cells[10].Value.ToString();
        }
    }
}
