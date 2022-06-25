using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProductsCalculator
{
    public partial class ManageProducts : Form
    {

        //path of data base
        string path = "products.db";
        string cs = @"URI=file:" + Application.StartupPath + "\\data_table.db"; //database creat debug folder
        int product_id = 0;
 
        SQLiteConnection con =  new SQLiteConnection("Data Source=products.db");

        SQLiteCommand cmd;
        SQLiteDataReader dr;
        DataTable ds = new DataTable();
   
        private const string ConnectionString = "Data Source=products.db";
        private readonly List<Product> _products;




        public ManageProducts()
        {
            InitializeComponent();

            _products = new List<Product>();
            ds.Columns.Add("Id", typeof(int));
            ds.Columns.Add("Name", typeof(string));
            ds.Columns.Add("Unit", typeof(string));
            ds.Columns.Add("Store", typeof(string));
            ds.Columns.Add("Price", typeof(float));
        }

       

        private void btnAdd_Click(object sender, EventArgs e)
        {
          


                const string query = "INSERT INTO products(Name,Unit, Store,Price) VALUES(@name,@unit, @store,@price)";

                //here we are setting the parameter values that will be actually 
                //replaced in the query in Execute method
                var args = new Dictionary<string, object>
        {
            {"@name",tbName.Text},
            {"@unit",tbUnit.Text},
            {"@store",  tbMagasin.Text},
                      {"@price",  tbPrice.Text}
        };

                int result = ExecuteWrite(query, args);


            //  dataGridView1.Rows.Clear();
            ds.Rows.Clear();
            dataGridView1.DataSource = ds;



            con.Close();
            data_show();

      //      dataGridView1.Rows.Insert(0, tbName.Text, tbMagasin.Text, tbPrice.Text);


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


              /*  Console.WriteLine(dr["id"].ToString());
                Console.WriteLine(dr["name"]);
                Console.WriteLine(dr["store"]);
                Console.WriteLine(dr["price"].ToString()); */
                //       Console.WriteLine( dr.GetInt32(0).ToString());
                //     Console.WriteLine(dr.GetString(1));

               // dataGridView1.Rows.Insert(0, dr["id"], dr["name"], dr["store"], dr["price"]);



              
                ds.Rows.Add(new object[] { dr["id"], dr["name"], dr["unit"], dr["store"], dr["price"] });


                dataGridView1.DataSource = ds;



            }

       //     con.Close();

        }


       
          private Product GetUserById(int id)
           {
               var query = "SELECT * FROM Products WHERE Id = @id";

               var args = new Dictionary<string, object>
       {
           {"@id", id}
       };

               DataTable dt = ExecuteRead(query, args);

               if (dt == null || dt.Rows.Count == 0)
               {
                   return null;
               }

               var prod = new Product
              (Convert.ToInt32(dt.Rows[0]["Id"]),
                   Convert.ToString(dt.Rows[0]["Name"]),
                   Convert.ToString(dt.Rows[0]["Unit"]),
                    Convert.ToString(dt.Rows[0]["Store"]), Convert.ToInt32(dt.Rows[4]["Price"]));

               return prod;
           }
           


        private DataTable ExecuteRead(string query,Dictionary<string, object> args)
        {
            if (string.IsNullOrEmpty(query.Trim()))
                return null;

            using (var con = new SQLiteConnection("Data Source=products.db"))
            {
                con.Open();
                using (var cmd = new SQLiteCommand(query, con))
                {
                    foreach (KeyValuePair<string, object> entry in args)
                    {
                        cmd.Parameters.AddWithValue(entry.Key, entry.Value);
                    }

                    var da = new SQLiteDataAdapter(cmd);

                    var dt = new DataTable();
                    da.Fill(dt);

                    da.Dispose();
                    return dt;
                }
            }
        }




        private int ExecuteWrite(string query, Dictionary<string, object> args)
        {
            int numberOfRowsAffected;

            //setup the connection to the database
            using (var con = new SQLiteConnection("Data Source=products.db"))
            {
                con.Open();

                //open a new command
                using (var cmd = new SQLiteCommand(query, con))
                {
                    //set the arguments given in the query
                    foreach (var pair in args)
                    {
                        cmd.Parameters.AddWithValue(pair.Key, pair.Value);
                    }

                    //execute the query and get the number of row affected
                    numberOfRowsAffected = cmd.ExecuteNonQuery();
                    
                }

                return numberOfRowsAffected;
            }
        }


        //create database and table
        private void Create_db()
        {
            if (!System.IO.File.Exists(path))
            {
                SQLiteConnection.CreateFile(path);
                using (var sqlite = new SQLiteConnection(@"Data Source=" + path))
                {
                    sqlite.Open();
                    string sql = "create table products(id INTEGER NOT NULL UNIQUE ,name varchar(50) NOT NULL,unit varchar(50) NOT NULL,store varchar(50) NOT NULL , price real ot null , primary key (id autoincrement) )";



                    SQLiteCommand command = new SQLiteCommand(sql, sqlite);
                    command.ExecuteNonQuery();
                }
            }
            else
            {
                Console.WriteLine("Database cannot create");
                return;
            }
        }




     


        private void btnDelete_Click(object sender, EventArgs e)
        {

         //  var con = new SQLiteConnection(path);
          //con.Open();

            var cmd = new SQLiteCommand(con);

           try
            {
            cmd.CommandText = "DELETE FROM products where id =@Id";
            cmd.Prepare();
                cmd.Parameters.AddWithValue("@Id", product_id);

                cmd.ExecuteNonQuery();

            //    dataGridView1.Rows.Clear();
            ds.Rows.Clear();
            dataGridView1.DataSource = ds;


           
            con.Close();
            data_show();
        }
            catch (Exception)
            {
                Console.WriteLine("cannot delete data");
                return;
            }

        }

        private void ManageProducts_Load(object sender, EventArgs e)
        {

            //Create_db();
            data_show();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Console.WriteLine("row celected cell ");

            if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
            {
                product_id = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["id"].FormattedValue.ToString());
                dataGridView1.CurrentRow.Selected = true;
                tbName.Text = dataGridView1.Rows[e.RowIndex].Cells["name"].FormattedValue.ToString();
                tbUnit.Text = dataGridView1.Rows[e.RowIndex].Cells["unit"].FormattedValue.ToString();
                tbMagasin.Text = dataGridView1.Rows[e.RowIndex].Cells["store"].FormattedValue.ToString();
                tbPrice.Text = dataGridView1.Rows[e.RowIndex].Cells["price"].FormattedValue.ToString();
                Console.WriteLine("id" + product_id);
            }



        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {


            //con.Open();

            var cmd = new SQLiteCommand(con);

          try
            {
                cmd.CommandText = "UPDATE products Set name =@Name,store =@Store,price =@Price where id =@Id";
                cmd.Prepare();
            cmd.Parameters.AddWithValue("@Id", product_id);
            cmd.Parameters.AddWithValue("@Name", tbName.Text);
                cmd.Parameters.AddWithValue("@Unit", tbUnit.Text);
                cmd.Parameters.AddWithValue("@Store", tbMagasin.Text);
                cmd.Parameters.AddWithValue("@Price", tbPrice.Text);

                cmd.ExecuteNonQuery();
               

         
                ds.Rows.Clear();
                dataGridView1.DataSource = ds;


                con.Close();
                data_show();

            }
            catch (Exception)
            {
                Console.WriteLine("cannot update data");
                return;
            }



        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
         
            //   ( dataGridView1.DataSource as DataTable).DefaultView.RowFilter=string.Format("Field = {1}", tbSearch.Text);

           // DataView dv;
        //   dv = new DataView(ds.Tables[0], "name = 'test' ", "type Desc", DataViewRowState.CurrentRows);

            ds.DefaultView.RowFilter = string.Format("[{0}] LIKE '%{1}%'", "name", tbSearch.Text);
            dataGridView1.DataSource = ds;
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            var mainWindow = new MainWindow();



            this.Hide();
            mainWindow.ShowDialog();
            this.Close();

        }

        private void tableLayoutPanel3_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
