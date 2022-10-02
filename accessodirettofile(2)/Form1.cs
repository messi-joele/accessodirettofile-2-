using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace accessodirettofile_2_
{
    public partial class File_scraping : Form
    {
        public File_scraping()
        {
            InitializeComponent();
        }
        public string filename = "veneto_verona.csv";
        private void button1_Click(object sender, EventArgs e)
        {
            string cerca = textBox1_ingresso.Text.ToUpper();
            label_nome.Text = ("nome: "+Ricerca(filename, cerca));
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        static string Ricerca(string filename, string nomecercato)
        {
            string line="";
            var f = new FileStream(filename, FileMode.Open, FileAccess.ReadWrite);// accesso al file binario 
            BinaryReader reader = new BinaryReader(f);
            BinaryWriter writer = new BinaryWriter(f);
            int righetot = Convert.ToInt32(f.Length);//byte totali
            int lunghezzariga = 528;
            righetot /= 528; // per ottenere il numero di righe 

            string result = "";

            int lung =Convert.ToInt32(f.Length);
            int i = 0, j = righetot - 1, m, pos = -1;

            do // RICERCA DICOTOMICA
            {
                m = (i + j) / 2;
                f.Seek(m * lunghezzariga, SeekOrigin.Begin);
                line = Encoding.ASCII.GetString(reader.ReadBytes(lunghezzariga));
                result = FromString(line,0);

                if (myCompare(result,nomecercato)==0)
                {
                    pos = m;
                }
                else if (myCompare(result, nomecercato) == -1)
                {
                    i = m + 1;
                }
                else
                    j = m - 1;
               

            } while (i <= j && pos == -1);

            if (pos != -1)
                MessageBox.Show("campo trovato in posizione: " + pos);
            else
            throw new Exception("campo non trovato");
            string fine = FromString(line,7);
            f.Close();
            return fine;
           
        }

        static int myCompare(string stringa1, string stringa2) 
        {
            if (stringa1 == stringa2)//0=sono uguali 1=stringa viene prima -1=stringa viene dopo
                return 0;

            char[] char1 = stringa1.ToCharArray();
            char[] char2 = stringa2.ToCharArray();
            int l = char1.Length;
            if (char2.Length < l)//in l c'è la lunghezza più piccola
                l = char2.Length;

            for (int i = 0; i < l; i++)
            {
                if (char1[i] < char2[i])
                    return -1;
                if (char1[i] > char2[i])
                    return 1;
            }

            return 0;//visto che qui non mi interessa lunghezza allora mi basta che la prima parte si uguale
        }



        public static string FromString(string Stringa, int pos, string sep = ";")//funzione che da una stringa separa i campi e ritorna una stringa
        {
            string[] ris = Stringa.Split(';');
            return ris[pos];
            
        }

        private void File_scraping_Load(object sender, EventArgs e)
        {

        }
    }
}
