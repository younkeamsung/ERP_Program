using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.Tsp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace ERP
{
    public partial class Account : Form
    {
        MySqlConnection connection = new MySqlConnection("Server=localhost;Database=ERP_project;Uid=root;Pwd=root;");




        public Account()
        {
            InitializeComponent();
        }

        /*
         * 계정정보의 모든 필드를 초기화
         */
        private void btnInit_Click(object sender, EventArgs e)
        {
            txtId.Text = "";
            txtPwd.Text = "";
        }

        /*
         * 계정정보를 생성 (아이디, 비밀번호)
         * 비밀번호는 테이블에 저장전 암호화
         * SQL : INSERT
         */
        private void btnCreate_Click(object sender, EventArgs e)
        {
            string Id = txtId.Text;
            string Pwd = txtPwd.Text;
            Encryption encryption = new Encryption();
            string encriptedPw = Encryption.EncryptString(Pwd);

            string insertQuery = @"INSERT INTO user VALUES ('" + Id + "', '" + encriptedPw + "');";
            MySqlCommand command = new MySqlCommand(insertQuery, connection);
            connection.Open();

            try
            {
                if (command.ExecuteNonQuery() != 0)
                {
                    MessageBox.Show("계정정보가 생성되었습니다. 조회버튼을 눌러 계정현황을 업데이트 해주세요.", "생성성공", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("계정정보가 생성되지 않았습니다. 중복된 아이디가 있는지 확인하세요", "생성실패", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            connection.Close();
        }
        /*
         * 데이터그리드뷰에서 선택한 계정정보를(패스워드) 업데이트
         * SQL : UPDATE
         */
        private void btnModify_Click(object sender, EventArgs e)
        {
            string Id = txtId.Text;
            string Pwd = txtPwd.Text;
            Encryption encryption = new Encryption();
            string encriptedPw = Encryption.EncryptString(Pwd);

            string insertQuery = @"UPDATE user SET password = '" + encriptedPw + "' WHERE id = '" + Id + "';";
            MySqlCommand command = new MySqlCommand(insertQuery, connection);
            connection.Open();

            try
            {
                if (command.ExecuteNonQuery() != 0)
                {
                    MessageBox.Show("비밀번호가 수정되었습니다. 조회버튼을 눌러 계정현황을 업데이트 해주세요.", "수정성공", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("비밀번호가 수정되지 않았습니다. 아이디가 맞는지 확인하세요", "수정실패", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            connection.Close();
        }

        /*
         * 데이터그리드뷰에서 선택한 계정정보를 삭제
         * SQL : DELETE
         */
        private void btnDelete_Click(object sender, EventArgs e)
        {
            string Id = txtId.Text;
            string Pwd = txtPwd.Text;
            Encryption encryption = new Encryption();
            string encriptedPw = Encryption.EncryptString(Pwd);

            string insertQuery = @"DELETE FROM user WHERE id = '" + Id + "' AND password = '" + encriptedPw + "';";
            MySqlCommand command = new MySqlCommand(insertQuery, connection);
            connection.Open();

            try
            {
                if (command.ExecuteNonQuery() != 0)
                {
                    MessageBox.Show("계정정보가 삭제되었습니다. 조회버튼을 눌러 계정현황을 업데이트 해주세요.", "삭제성공", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("계정정보가 삭제되지 않았습니다. 아이디와 비밀번호를 확인해주세요", "삭제실패", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            connection.Close();
        }

        /*
         * 사용자 계정 정보를 조회
         * TABLE : user
         * SQL : SELECT
         */
        private void btnSearch_Click(object sender, EventArgs e)
        {
            
            try
            {
                string insertQuery = @"SELECT id 로그인아이디, password 패스워드 FROM user WHERE 1;";

                MySqlCommand command = new MySqlCommand(insertQuery, connection);
                connection.Open();

                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                DataTable accountData = new DataTable();
                adapter.Fill(accountData);
                dataGridView1.DataSource = accountData;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            connection.Close();
            
        }

        /*
         * 데이터그리드뷰에서 선택한 계정정보를 계정정보 그룹의 필드에 표시(로그인계정, 패스워드)
         * 암호화된 패스워드는 복호화해서 보여줌
         */
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtId.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            string Pwd = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            Encryption encryption = new Encryption();
            string encriptedPw = Encryption.DecryptString(Pwd, Encryption.Key, Encryption.IV);
            txtPwd.Text = encriptedPw;
        }
    }
}
