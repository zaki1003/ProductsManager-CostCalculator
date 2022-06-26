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
using ClosedXML.Excel;


namespace ProductsCalculator
{
    public partial class MainWindow : Form
    {

        string text = "";
        NavigationForm cal;
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

          /*    ds.Columns.Add("Id", typeof(int));
            ds.Columns.Add("Name", typeof(string));
            ds.Columns.Add("Unit", typeof(string));
            ds.Columns.Add("Store", typeof(string));
            ds.Columns.Add("Price", typeof(float));*/



            ds.Columns.Add("Id", typeof(int));
            ds.Columns.Add("Nom", typeof(string));
            ds.Columns.Add("Unité", typeof(string));
            ds.Columns.Add("Magasin", typeof(string));
            ds.Columns.Add("Prix", typeof(float));



            dataGridView1.ReadOnly = true;

            //Add a CheckBox Column to the DataGridView at the first position.
            DataGridViewCheckBoxColumn checkBoxColumn = new DataGridViewCheckBoxColumn();
            checkBoxColumn.HeaderText = "Séléctionner";
            checkBoxColumn.Width = 70;
            checkBoxColumn.Name = "selected";
            
            dataGridView1.Columns.Insert(0, checkBoxColumn);

            //Assign Click event to the DataGridView Cell.
            dataGridView1.CellContentClick += new DataGridViewCellEventHandler(DataGridView_CellClick);


            /*  ds2.Columns.Add("Quantité", typeof(int));
              ds2.Columns.Add("id", typeof(int));
              ds2.Columns.Add("Name", typeof(string));
              ds2.Columns.Add("Unit", typeof(string));
              ds2.Columns.Add("Store", typeof(string));
              ds2.Columns.Add("Price", typeof(float));*/


            ds2.Columns.Add("Quantité", typeof(int));
            ds2.Columns.Add("Id", typeof(int));
            ds2.Columns.Add("Nom", typeof(string));
            ds2.Columns.Add("Unité", typeof(string));
            ds2.Columns.Add("Magasin", typeof(string));
            ds2.Columns.Add("Prix", typeof(float));




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
                    ds2.Rows.Add(new object[] {0, row.Cells["id"].Value,row.Cells["nom"].Value, row.Cells["unité"].Value, row.Cells["magasin"].Value, row.Cells["prix"].Value });
                    


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
               ds.Rows.Add(new object[] { dr["id"], dr["name"], dr["unit"], dr["store"], dr["price"] });
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

                cost += Convert.ToDouble(dataGridView2.Rows[i].Cells[0].Value.ToString()) * Convert.ToDouble(dataGridView2.Rows[i].Cells[5].Value.ToString());


            }
            lbCost.Text= cost.ToString();



        }

     
            private void btnSearch_Click(object sender, EventArgs e)
            {

                ds.DefaultView.RowFilter = string.Format("[{0}] LIKE '%{1}%'", "nom", tbSearch.Text);
                dataGridView1.DataSource = ds;





            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)

            {
                if (text.Contains(dataGridView1.Rows[i].Cells["id"].Value.ToString()))
                    dataGridView1.Rows[i].Cells["selected"].Value = true;
            }





        }
















        private void btnExport_Click(object sender, EventArgs e)
        {


            DataTable dt3 = new DataTable();


            //Adding the Columns.
            foreach (DataGridViewColumn column in dataGridView2.Columns)
            {
                dt3.Columns.Add(column.HeaderText, column.ValueType);
            }

            //Adding the Rows.
            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                dt3.Rows.Add();
                foreach (DataGridViewCell cell in row.Cells)
                {
              //  dt3.Rows[dt3.Rows.Count - 1][cell.ColumnIndex] = cell.Value.ToString();
                }
            }



            using (SaveFileDialog sfd = new SaveFileDialog() { Filter = "Excel Workbook|*.xlsx" })
            {
                if( sfd.ShowDialog()== DialogResult.OK)
                {
                    try
                    {
                        using(XLWorkbook workbook = new XLWorkbook())
                        {


                          var ws =   workbook.Worksheets.Add(ds2, "Produits");
                            
                            ws.Row(1).InsertRowsAbove(3);


                            ws.Cell(1, 1).Value = "Nom du client: ";
                            ws.Cell(1, 2).Value = tbClient.Text;


                            ws.Cell(2, 1).Value = "Date: ";
                            ws.Cell(2, 2).Value = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");


                            ws.Cell(3, 1).Value = "Montant: ";
                            ws.Cell(3, 2).Value = lbCost.Text;


                            ws.Range("A1:B1").Style.Font.FontColor = XLColor.White;
                            ws.Range("A2:B2").Style.Font.FontColor = XLColor.White;
                            ws.Range("A3:B3").Style.Font.FontColor = XLColor.White;
                            ws.Range("A1:B1").Style.Fill.BackgroundColor = XLColor.DarkBlue;
                            ws.Range("A2:B2").Style.Fill.BackgroundColor = XLColor.DarkBlue;
                            ws.Range("A3:B3").Style.Fill.BackgroundColor = XLColor.DarkBlue;




                            workbook.SaveAs(sfd.FileName);
                        }
                        MessageBox.Show("Vous avez réussi à exporter vos données vers un fichier Excel", "Message", MessageBoxButtons.OK,MessageBoxIcon.Information);
                    }catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }

                }


            }






        }
    }
}
