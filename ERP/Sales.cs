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

namespace ERP
{
    public partial class Sales : Form
    {
        MySqlConnection connection = new MySqlConnection("Server=localhost;Database=ERP_project;Uid=root;Pwd=root;");

        public Sales()
        {
            InitializeComponent();
        }

        /*
         * 조회조건 (기간, 고객, 상품) 에 맞는 주문 정보를 datagridview 에 로딩
         * SQL : SELECT
         */
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string insertQuery = @"SELECT A.salescode 주문번호, 
                                            A.customercode 고객번호, B.name 고객명, 
                                            A.itemcode 상품번호, 
                                            C.name 상품명, 
                                            A.date 주문일자, 
                                            A.qty 주문수량, 
                                            A.amt 주문금액 
                                        FROM sales A, customer B, item C 
                                      WHERE A.customercode = B.customercode 
                                        AND A.itemcode = C.itemcode";

                insertQuery += @" AND date >= '" + dtpFrom.Value.ToString("yyyy-MM-dd")
                             + "' AND date <= '" + dtpTo.Value.ToString("yyyy-MM-dd") + "'";

                if(txtCustomerId.Text != ""){
                    insertQuery += " AND A.customercode = '" + txtCustomerId.Text + "'";
                }
                if (txtItemId.Text != "")
                {
                    insertQuery += " AND A.itemcode = '" + txtItemId.Text + "'";
                }

                insertQuery += ";";

                MySqlCommand command = new MySqlCommand(insertQuery, connection);
                connection.Open();

                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                DataTable salesData = new DataTable();
                adapter.Fill(salesData);
                dataGridView1.DataSource = salesData;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            connection.Close();
        }

        /*
         * 조회조건에서 고객 정보를 SEARCH
         * Customer 윈도우폼 클래스를 호출
         * Customer 윈도우에서 선택한 고객 정보를 가져와서 해당 텍스트 박스에 display
         */

        private void btnCustomerSearch_Click(object sender, EventArgs e)
        {
            Customer customerForm = new Customer();
            customerForm.ShowDialog();
            if(customerForm.customerId != "")
            {
                txtCustomerId.Text = customerForm.customerId;
                txtCustomerName.Text = customerForm.customerName;
            }
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
         * 주문상세 정보 (주문번호, 고객, 상품, 일자, 수량, 금액)을 모두 입력받은 후에
         * sales 테이블에 해당 데이터를 추가
         * SQL: INSERT
         */
        private void btnCreate_Click(object sender, EventArgs e)
        {
            string salesCode = txtSalesCode.Text;
            string customerCode = txtCustIdDetail.Text;
            string itemCode = txtItemIdDetail.Text;
            string date = dtpSalesDate.Value.ToString("yyyy-MM-dd");
            string qty = txtQty.Text;
            string amt = txtAmt.Text;

            string insertQuery = @"INSERT INTO sales VALUES ('" + salesCode + "', '" + customerCode + "', '" +
                                 itemCode + "', '" + date + "', " + qty + ", " + amt + ");";
            MySqlCommand command = new MySqlCommand(insertQuery, connection);
            connection.Open();

            try
            {
                if(command.ExecuteNonQuery() != 0)
                {
                    MessageBox.Show("주문이 생성되었습니다. 조회버튼을 눌러 주문내역을 업데이트 해주세요." , "주문성공", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("주문이 생성되지 않았습니다. 중복된 주문번호가 있는지 확인하세요", "주문실패", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            connection.Close();
        }

        /*
         * datagridwiew 에서 선택한 주문 정보의 상세를 수정한 후 (주문번호는 수정하면 안됨)
         * 해당 주문건 (주문번호로 식별되는 sales 테이블의 데이터행) 의 정보를 업데이트
         * SQL : UPDATE
         */
        private void btnModify_Click(object sender, EventArgs e)
        {
            string salesCode = txtSalesCode.Text;
            string customerCode = txtCustIdDetail.Text;
            string itemCode = txtItemIdDetail.Text;
            string date = dtpSalesDate.Value.ToString("yyyy-MM-dd");
            string qty = txtQty.Text;
            string amt = txtAmt.Text;

            string insertQuery = @"UPDATE sales SET customercode = '" + customerCode + "', itemcode = '" + itemCode + "', date = '" +
                                date + "', qty = " + qty + ", amt = " + amt + " WHERE salescode = '" + salesCode + "'";
            MySqlCommand command = new MySqlCommand(insertQuery, connection);
            connection.Open();

            try
            {
                if (command.ExecuteNonQuery() != 0)
                {
                    MessageBox.Show("주문이 수정되었습니다. 조회버튼을 눌러 주문내역을 업데이트 해주세요.", "주문수정성공", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("주문이 수정되지 않았습니다. 주문번호가 맞는지 확인하세요", "주문수정실패", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            connection.Close();
        }

        /*
         * 데이터그리드뷰에서 선택한 주문 정보가 주문상세에 display 되면
         * 해당 주문번호의 주문상세 건을 sales 테이블에서 삭제
         * SQL : DELETE
         */
        private void btnDelete_Click(object sender, EventArgs e)
        {
            string salesCode = txtSalesCode.Text;

            string insertQuery = @"DELETE FROM sales WHERE salescode = '" + salesCode + "';";
            MySqlCommand command = new MySqlCommand(insertQuery, connection);
            connection.Open();

            try
            {
                if (command.ExecuteNonQuery() != 0)
                {
                    MessageBox.Show("주문이 삭제되었습니다. 조회버튼을 눌러 주문내역을 업데이트 해주세요.", "주문삭제성공", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("주문이 삭제되지 않았습니다. 주문번호가 맞는지 확인하세요", "주문삭제실패", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            connection.Close();
        }

        /*
         * 주문 상세내역을 입력받기 위해서 모든 상세 필드를 초기화
         */
        private void btnInit_Click(object sender, EventArgs e)
        {
            txtSalesCode.Text = "";
            txtCustIdDetail.Text = "";
            txtCustNameDetail.Text = "";
            txtItemIdDetail.Text = "";
            txtItemNameDetail.Text = "";
            dtpSalesDate.Value = DateTime.Now;
            txtQty.Text = "";
            txtAmt.Text = "";
        }

        /*
         * 주문 상세에서 고객 정보를 가져오는 버튼
         * btnCustomerSearch_Click 매서드와 동일하다 (display 하는 텍스트박스만 다름)
         */
        private void btnCustomerDetail_Click(object sender, EventArgs e)
        {
            Customer customerForm = new Customer();
            customerForm.ShowDialog();
            if (customerForm.customerId != "")
            {
                txtCustIdDetail.Text = customerForm.customerId;
                txtCustNameDetail.Text = customerForm.customerName;
            }
        }

        /*
         * 주문 상세에서 상품 정보를 가져오는 버튼
         * btnItemSearch_Click 매서드와 동일하다 (display 하는 텍스트박스만 다름)
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
         * 데이터 그리드뷰의 한 행을 클릭하면 해당 행의 주문정보를 주문 상세에 display
         */
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtSalesCode.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            txtCustIdDetail.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtCustNameDetail.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            txtItemIdDetail.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            txtItemNameDetail.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
            dtpSalesDate.Value = DateTime.ParseExact(dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString(), "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            txtQty.Text = dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString();
            txtAmt.Text = dataGridView1.Rows[e.RowIndex].Cells[7].Value.ToString();
        }
    }
}
