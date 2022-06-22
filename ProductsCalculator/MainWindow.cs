using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProductsCalculator
{
    public partial class MainWindow : Form
    {

        string text = "";
        Calculator cal;
        Product prod;
        List<Product> products;

        //path of data base
        string path = "products.db";



        SQLiteConnection con = new SQLiteConnection("Data Source=products.db");

        SQLiteCommand cmd;
        SQLiteDataReader dr;
        DataTable ds = new DataTable();
        DataTable ds2 = new DataTable();

        public MainWindow()
        {
            InitializeComponent();

              ds.Columns.Add("Id", typeof(int));
            ds.Columns.Add("Name", typeof(string));
            ds.Columns.Add("Store", typeof(string));
            ds.Columns.Add("Price", typeof(float));

         dataGridView1.ReadOnly = true;

            //Add a CheckBox Column to the DataGridView at the first position.
            DataGridViewCheckBoxColumn checkBoxColumn = new DataGridViewCheckBoxColumn();
            checkBoxColumn.HeaderText = "Séléctionner";
            checkBoxColumn.Width = 70;
            checkBoxColumn.Name = "selected";
            
            dataGridView1.Columns.Insert(0, checkBoxColumn);

            //Assign Click event to the DataGridView Cell.
            dataGridView1.CellContentClick += new DataGridViewCellEventHandler(DataGridView_CellClick);


            ds2.Columns.Add("Quantité", typeof(int));
            ds2.Columns.Add("id", typeof(int));
            ds2.Columns.Add("Name", typeof(string));
            ds2.Columns.Add("Store", typeof(string));
            ds2.Columns.Add("Price", typeof(float));



        }


        private void DataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
            if (e.RowIndex >= 0 && e.ColumnIndex == 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                row.Cells["selected"].Value = !Convert.ToBoolean(row.Cells["selected"].EditedFormattedValue);
                if (Convert.ToBoolean(row.Cells["selected"].Value))
                {
                    text += row.Cells[1].Value.ToString() + ",";
                    ds2.Rows.Add(new object[] {0, row.Cells["id"].Value,row.Cells["name"].Value, row.Cells["store"].Value, row.Cells["price"].Value });
                    


                }
                else
                {
            
                   text=  text.Replace(row.Cells[1].Value.ToString()+',', "");
          
                    for (int i = ds2.Rows.Count - 1; i >= 0; i--)
                    {
                        DataRow dr = ds2.Rows[i];
                        
                        if (dr["id"].ToString() == row.Cells[1].Value.ToString()) dr.Delete();

                    }
                    ds2.AcceptChanges();

                    dataGridView2.DataSource = ds2;

  

                }

                dataGridView2.DataSource = ds2;
               
                Console.WriteLine("text " + text);
            }
        }

        private void btnManage_Click(object sender, EventArgs e)
        {
            var manageProducts = new ManageProducts();



            this.Hide();
            manageProducts.ShowDialog();
            this.Close();


        }

        private void data_show()
        {

            //    SQLiteConnection con = new SQLiteConnection(path);
            con.Open();




            string stm = "SELECT * FROM products";
            var cmd = new SQLiteCommand(stm, con);
            dr = cmd.ExecuteReader();



            while (dr.Read())
            {
               ds.Rows.Add(new object[] { dr["id"], dr["name"], dr["store"], dr["price"] });
               dataGridView1.DataSource = ds;
}
            



        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            data_show();
         
            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++) dataGridView1.Rows[i].Cells["selected"].Value=false;
            for (int i = 0; i < dataGridView2.Rows.Count - 1; i++) dataGridView2.Rows[i].Cells["qte"].Value = 0;



        }

        private void btnCalculator_Click(object sender, EventArgs e)
        {
            /*  cal = new Calculator();
              cal.products = new List<Product>();

              for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
              {
                  if (bool.Parse(dataGridView1.Rows[i].Cells["selected"].Value.ToString()) == true)
                  {
                      prod = new Product(Convert.ToInt32(dataGridView1.Rows[i].Cells["id"].FormattedValue.ToString()),
                          dataGridView1.Rows[i].Cells["name"].Value.ToString(),
                          dataGridView1.Rows[i].Cells["store"].Value.ToString(),
                        Convert.ToDouble(dataGridView1.Rows[i].Cells["price"].FormattedValue.ToString()));
                      cal.products.Add(prod);
                  }
              }
              Console.WriteLine("nombre des elements selectionné " + cal.products.Count);
              */
            double cost = 0.0;

            for (int i = 0; i < dataGridView2.Rows.Count - 1; i++) {

                cost += Convert.ToDouble(dataGridView2.Rows[i].Cells[0].Value.ToString()) * Convert.ToDouble(dataGridView2.Rows[i].Cells[4].Value.ToString());


            }
            lbCost.Text= cost.ToString();



        }

     
            private void btnSearch_Click(object sender, EventArgs e)
            {

                ds.DefaultView.RowFilter = string.Format("[{0}] LIKE '%{1}%'", "name", tbSearch.Text);
                dataGridView1.DataSource = ds;





            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)

            {
                if (text.Contains(dataGridView1.Rows[i].Cells["id"].Value.ToString()))
                    dataGridView1.Rows[i].Cells["selected"].Value = true;
            }





        }
       
    }
}
