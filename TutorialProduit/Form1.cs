using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;


namespace TutorialProduit
{


    

    public partial class Form1 : Form
    {


        SqlConnection connexion;
        SqlCommand insertCommand;
        SqlCommand selectCommand;
        public Form1()
        {
            InitializeComponent();
            connexion = new SqlConnection("Server=COLOSSUS;Database=ProduitDB;User Id=sa;Password = zilla; ");
            connexion.StateChange += new StateChangeEventHandler(Connexion_StateChange);


            insertCommand = new SqlCommand();
            insertCommand.Connection = connexion;
            insertCommand.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));
            insertCommand.Parameters.Add(new SqlParameter("@nom", SqlDbType.VarChar));
            insertCommand.Parameters.Add(new SqlParameter("@prix", SqlDbType.Decimal));
            insertCommand.CommandText = "insert into PRODUIT values (@id,@nom,@prix)";

            selectCommand = new SqlCommand("Select IdProduit, Libelle, Prix from PRODUIT",connexion);
                       
           

        }

        private void Connexion_StateChange(object sender, StateChangeEventArgs e)
        {
            label1.Text = "state: " + connexion.State.ToString();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (connexion.State == ConnectionState.Closed)
                {
                    connexion.Open();
                   
                }
                else
                    if (connexion.State == ConnectionState.Open)
                {
                    connexion.Close();
                   
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERREUR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            try
            {
                connexion.Open();
                insertCommand.Parameters["@id"].Value = int.Parse(textBoxId.Text);
                insertCommand.Parameters["@nom"].Value = textBoxNom.Text;
                insertCommand.Parameters["@prix"].Value = Double.Parse(textBoxPrix.Text);

                int i = insertCommand.ExecuteNonQuery();
                MessageBox.Show(i + " enregestrements insérés", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                connexion.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERREUR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            dataGridProduit.Rows.Clear();
            connexion.Open();

            SqlDataReader reader = selectCommand.ExecuteReader();
            while (reader.Read())
            {
                
                object[] ligne = { reader.GetInt32(0), reader[1].ToString(), decimal.Parse(reader[2].ToString())};

                dataGridProduit.Rows.Add(ligne);
            }

            connexion.Close();


        }
    }
}
