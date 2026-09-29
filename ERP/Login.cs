using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace ERP
{
    public partial class Login : Form
    {
        MySqlConnection connection = new MySqlConnection("Server=localhost;Database=ERP_project;Uid=root;Pwd=root;");
        public Login()
        {
            InitializeComponent();
        }
        /*
         *  입력된 아이디와 비밀번호로 user 테이블에서 동일한 id, password 를 갖는 행을 SELECT
         *  true = 로그인 메세지박스를 띄우고 메인 윈도우 oper(password 는 암호화 된 password 와 비교)
         *  flalse = id 나 password 가 맞지 않다고 메세지 박스에 출력
         */

        private void btnLogin_Click(object sender, EventArgs e)
        {
            bool isLogin = false;

            // 입력된 password 의 암호화된 문자열 생성
            Encryption encryption = new Encryption();
            string encriptedPw = Encryption.EncryptString(txtPwd.Text);

            try
            {
                string insertQuery = "SELECT id, password FROM user WHERE id = '" + txtId.Text + "';";
                MySqlCommand command = new MySqlCommand(insertQuery, connection);
                connection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                

                while (reader.Read())
                {
                    if (reader["password"].ToString() == encriptedPw)
                    {
                        isLogin = true;
                    }
                }
                if (isLogin)
                {
                    MessageBox.Show(txtId.Text + "님, 환영합니다.", "로그인 성공", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Main mainForm = new Main();
                    mainForm.ShowDialog();
                }
                else
                {
                    MessageBox.Show("아이디 혹은 비밀번호가 일치하지 않습니다.","로그인 실패", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
            }
            catch(Exception ex) 
            {
                MessageBox.Show(ex.Message);
            }

            connection.Close();
        }

        /*
         * 비밀번호 텍스트박스에 입력된 비밀번호를 ● 로 표시할지, 입력된 문자열 그대로 표시할지 선택
         */
        private void cbShowPwd_CheckedChanged(object sender, EventArgs e)
        {
            if (cbShowPwd.Checked)
            {
                txtPwd.UseSystemPasswordChar = false;
            }
            else
            {
                txtPwd.UseSystemPasswordChar = true;
            }
        }
    }
}
