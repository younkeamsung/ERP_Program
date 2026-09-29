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
    public partial class Main : Form
    {
        MySqlConnection connection = new MySqlConnection("Server=localhost;Database=ERP_project;Uid=root;Pwd=root;");

        public Main()
        {
            InitializeComponent();
        }

        private void 마스터데이터ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        /*
         * 주문관리 윈도우폼을 오픈한다
         * 소스코드 : Sales.cs
         */
        private void 주문관리ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Sales salesForm = new Sales();
            salesForm.ShowDialog();
        }
        /*
         * 생산관리 윈도우폼을 오픈한다
         * 소스코드 : Production.cs
         */
        private void 생산관리ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Production prodForm = new Production();
            prodForm.ShowDialog();
        }
        /*
         * 입출고관리 윈도우폼을 오픈한다
         * 소스코드 : Inout.cs
         */
        private void 입출고관리ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Inout inoutForm = new Inout();
            inoutForm.ShowDialog();

        }
        /*
         * 마스터데이터의 서브 메뉴, 계정관리 윈도우폼을 오픈한다
         * 소스코드 : Account.cs
         */
        private void 계정관리ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Account account = new Account();
            account.ShowDialog();
        }

        /*
         * 마스터데이터의 서브 메뉴, 직원관리 윈도우폼을 오픈한다
         * 소스코드 : Employee.cs
         */
        private void 직원관리ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Employee employee = new Employee();
            employee.ShowDialog();
        }
        
        /*
         * 마스터데이터의 서브 메뉴, 고객관리 윈도우폼을 오픈한다
         * 소스코드 : Custmaster.cs
         */
        private void 고객관리ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Custmaster custmaster = new Custmaster();
            custmaster.ShowDialog();
        }

        /*
         * 마스터데이터의 서브 메뉴, 상품관리 윈도우폼을 오픈한다
         * 소스코드 : Itemmaster.cs
         */
        private void 제품관리ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Itemmaster itemmaster = new Itemmaster();
            itemmaster.ShowDialog();
        }

        /*
         * 메인 윈도우를 종료한다.
         */
        private void 종료ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /*
         * 마스터데이터의 서브 메뉴, 부서관리 윈도우폼을 오픈한다
         * 소스코드 : Deptmaster.cs
         */
        private void 부서관리ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Deptmaster deptmaster = new Deptmaster();
            deptmaster.ShowDialog();
        }

        /*
         * 메인 윈도우가 생성된 후에 다음의 차트를 생성한다
         * 월별매출현황
         * 월별생산현황
         * 월별재고현황
         * Top5 고객 매출현황
         */
        private void Main_Load(object sender, EventArgs e)
        {
            MonthlySalesChartLoad();
            MonthlyProductionChartLoad();
            MonthlyStockChartLoad();
            Top5ClientChartLoad();
        }

        /*
         * 월별매출현황 차트를 생성한다
         * MySql Table : sales
         * 월 = LEFT(일자, 7)
         */
        private void MonthlySalesChartLoad()
        {
            try
            {
                string insertQuery = "SELECT LEFT(date, 7) month, SUM(amt) msales FROM sales GROUP BY LEFT(date, 7);";
                MySqlCommand command = new MySqlCommand(insertQuery, connection);
                connection.Open();

                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string month = reader.GetString(0);
                    int mSales = reader.GetInt32(1);

                    chtMonthlySales.Series[0].Points.AddXY(month, mSales);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            connection.Close();
        }

        /*
         * 월별생산현황 차트를 생성한다
         * MySql Table : production
         * 월 = LEFT(일자, 7)
         */
        private void MonthlyProductionChartLoad()
        {
            try
            {
                string insertQuery = "SELECT LEFT(date,7) month, SUM(qty) mproduction FROM production GROUP BY LEFT(date,7);";
                MySqlCommand command = new MySqlCommand(insertQuery, connection);
                connection.Open();

                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                DataTable customerData = new DataTable();
                adapter.Fill(customerData);
                chtMonthlyProduction.DataSource = customerData;
                chtMonthlyProduction.Series[0].XValueMember = "month";
                chtMonthlyProduction.Series[0].YValueMembers = "mproduction";
                chtMonthlyProduction.Series[0].IsValueShownAsLabel = true;
                chtMonthlyProduction.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
                chtMonthlyProduction.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
                chtMonthlyProduction.ChartAreas[0].AxisX.LabelStyle.Angle = 45;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            connection.Close();
        }

        /*
         * 상품별 월생산현황을 보기 위해서 상품조회 서브 윈도우 폼을 호출한다
         */
        private void btnItem_Click(object sender, EventArgs e)
        {
            Item itemForm = new Item();
            itemForm.ShowDialog();
            if (itemForm.itemId != "")
            {
                txtId.Text = itemForm.itemId;
                txtName.Text = itemForm.itemName;
            }
        }

        /*
         * 선택된 상품을 기준으로 월별생산현황 차트를 리프레쉬한다
         */
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                string insertQuery = "SELECT LEFT(date,7) month, SUM(qty) mproduction FROM production ";
                if (txtId.Text != "") 
                {
                    insertQuery += "WHERE itemcode = '" + txtId.Text + "'";
                }
                insertQuery += " GROUP BY LEFT(date,7)";
                MySqlCommand command = new MySqlCommand(insertQuery, connection);
                connection.Open();

                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                DataTable customerData = new DataTable();
                adapter.Fill(customerData);
                chtMonthlyProduction.DataSource = customerData;
                chtMonthlyProduction.Series[0].XValueMember = "month";
                chtMonthlyProduction.Series[0].YValueMembers = "mproduction";
                chtMonthlyProduction.Series[0].IsValueShownAsLabel = true;
                chtMonthlyProduction.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
                chtMonthlyProduction.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
                chtMonthlyProduction.ChartAreas[0].AxisX.LabelStyle.Angle = 45;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            connection.Close();
        }

        /*
         * 월별재고현황 차트를 생성한다
         * 재고 = sum(수량), 입고 = +수량 / 출고 = -수량
         * MySql Table : inandout
         */
        private void MonthlyStockChartLoad()
        {
            try
            {
                string insertQuery = "SELECT LEFT(date, 7) month, SUM(qty) mstock FROM inandout GROUP BY LEFT(date, 7);";
                MySqlCommand command = new MySqlCommand(insertQuery, connection);
                connection.Open();

                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                DataTable customerData = new DataTable();
                adapter.Fill(customerData);
                chtMonthlyStock.DataSource = customerData;
                chtMonthlyStock.Series[0].XValueMember = "month";
                chtMonthlyStock.Series[0].YValueMembers = "mstock";
                chtMonthlyStock.Series[0].IsValueShownAsLabel = true;
                chtMonthlyStock.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
                chtMonthlyStock.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
                chtMonthlyStock.ChartAreas[0].AxisX.LabelStyle.Angle = 45;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            connection.Close();
        }

        /*
         * 상품별 월재고현황을 보기 위해서 상품조회 서브 윈도우 폼을 호출한다
         */
        private void btnItemStock_Click(object sender, EventArgs e)
        {
            Item itemForm = new Item();
            itemForm.ShowDialog();
            if (itemForm.itemId != "")
            {
                txtIdStock.Text = itemForm.itemId;
                txtNameStock.Text = itemForm.itemName;
            }
        }

        /*
         * 선택된 상품을 기준으로 월별재고현황 차트를 리프레쉬한다
         */
        private void btnRefreshStock_Click(object sender, EventArgs e)
        {
            try
            {
                string insertQuery = "SELECT LEFT(date,7) month, SUM(qty) mstock FROM inandout ";
                if (txtIdStock.Text != "")
                {
                    insertQuery += "WHERE itemcode = '" + txtIdStock.Text + "'";
                }
                insertQuery += " GROUP BY LEFT(date,7)";
                MySqlCommand command = new MySqlCommand(insertQuery, connection);
                connection.Open();

                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                DataTable customerData = new DataTable();
                adapter.Fill(customerData);
                chtMonthlyStock.DataSource = customerData;
                chtMonthlyStock.Series[0].XValueMember = "month";
                chtMonthlyStock.Series[0].YValueMembers = "mstock";
                chtMonthlyStock.Series[0].IsValueShownAsLabel = true;
                chtMonthlyStock.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
                chtMonthlyStock.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
                chtMonthlyStock.ChartAreas[0].AxisX.LabelStyle.Angle = 45;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            connection.Close();
        }

        /*
         * 전체 매출이 가장 많은 5명의 고객과 고객별 매출 차트를 생성한다.
         * MySql Table : sales
         */
        private void Top5ClientChartLoad()
        {
            try
            {
                string insertQuery = "SELECT customercode, SUM(amt) total_amt FROM sales GROUP BY customercode ORDER BY total_amt DESC LIMIT 5;";
                MySqlCommand command = new MySqlCommand(insertQuery, connection);
                connection.Open();

                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                DataTable customerData = new DataTable();
                adapter.Fill(customerData);
                chtTop5Client.DataSource = customerData;
                chtTop5Client.Series[0].XValueMember = "customercode";
                chtTop5Client.Series[0].YValueMembers = "total_amt";
                chtTop5Client.Series[0].IsValueShownAsLabel = true;
                chtTop5Client.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
                chtTop5Client.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
                chtTop5Client.ChartAreas[0].AxisX.LabelStyle.Angle = 45;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            connection.Close();
        }
    }
}
