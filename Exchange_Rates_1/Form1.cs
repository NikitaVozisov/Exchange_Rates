using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;

namespace Exchange_Rates_1
{
    
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
       public  struct Graph
        {
            public string Date;
            public double USD, EUR, BTC;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
             
        }
        string Parser_FromFile(string s)
        {
            string s1=String.Empty;
            for (int i = 0; i < s.Length; i++)
            {
                if (Char.IsDigit(s[i]) || (s[i] == ','))
                { s1 += s[i]; }
                
            }
            return s1;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string url = "https://openexchangerates.org/api/latest.json?app_id=81433f9edaf6434baafc9670e0fd1c5a%20&base=USD";
                using (var webClient = new WebClient())
                {
                    // Getting response by URL
                    string response = webClient.DownloadString(url);

                    //USD
                    string USD = String.Empty;
                    double USD_value = 0;
                    int f = 0;
                    f = response.IndexOf("RUB");
                    for (int i = 0; i < 20; i++)
                    {
                        if (Char.IsDigit(response[i + f]) || (response[i + f] == '.'))
                        { USD += response[i + f]; }

                    }
                    USD = USD.Trim();
                    USD = USD.Replace('.', ',');

                    USD_value = double.Parse(USD);


                    //EURO
                    string EUR = String.Empty;
                    double EUR_value = 0;
                    f = response.IndexOf("EUR");
                    for (int i = 0; i < 20; i++)
                    {
                        if (Char.IsDigit(response[i + f]) || (response[i + f] == '.'))
                        { EUR += response[i + f]; }

                    }
                    EUR = EUR.Trim();
                    EUR = EUR.Replace('.', ',');

                    EUR_value = double.Parse(EUR);
                    EUR_value = USD_value / EUR_value;

                    //Bitcoin
                    string BTC = String.Empty;
                    double BTC_value = 0;
                    f = response.IndexOf("BTC");
                    for (int i = 0; i < 20; i++)
                    {
                        if (Char.IsDigit(response[i + f]) || (response[i + f] == '.'))
                        { BTC += response[i + f]; }

                    }
                    BTC = BTC.Trim();
                    BTC = BTC.Replace('.', ',');

                    BTC_value = double.Parse(BTC);
                    BTC_value = USD_value / BTC_value;

                    textBox2.Text = (String.Format("{0:0.000}", USD_value));
                    textBox3.Text = (String.Format("{0:0.000}", EUR_value));
                    textBox4.Text = (String.Format("{0:0.000}", BTC_value));


                    //Saving Information


                    string path = @"C:\Users\Никита\YandexDisk\Exchange_Rates_1\exchange_rates.txt";
                    using (StreamReader sr = new StreamReader(path, System.Text.Encoding.Default))
                    {

                        String file = string.Empty;
                        String line;
                        while (((line = sr.ReadLine()) != null))
                        {
                            file += line + "\r\n";
                        }
                        sr.Close();
                        DateTime Now = DateTime.Today;
                        int pos = file.IndexOf(Now.Day + "." + Now.Month + "." + Now.Year);
                        if (pos == (-1))
                        {
                            int length = file.Length;
                            file = file.Replace("}", "");
                            string ap = @"""";
                            file += ap + "Date" + ap + ":" + ap + Now.Day + "." + Now.Month + "." + Now.Year + ap + "\r\n";
                            file += "  " + ap + "USD" + ap + ":" + ap + String.Format("{0:0.000}", USD_value) + ap + "\r\n";
                            file += "  " + ap + "EUR" + ap + ":" + ap + String.Format("{0:0.000}", EUR_value) + ap + "\r\n";
                            file += "  " + ap + "BTC" + ap + ":" + ap + String.Format("{0:0.000}", BTC_value) + ap + "\r\n";
                            file += "}";
                            File.WriteAllText(@"C:\Users\Никита\YandexDisk\Exchange_Rates_1\exchange_rates.txt", file, Encoding.Default);
                        }
                    }
                }
            }
            catch (Exception exc) {MessageBox.Show("Possible reason: no internet connection","No internet connection",MessageBoxButtons.RetryCancel); }
            
        }
           
        
        

        private void button2_Click(object sender, EventArgs e)
        {
            //Reading Information
            
            try { 
            string path = @"C:\Users\Никита\YandexDisk\Exchange_Rates_1\exchange_rates.txt";
            using (StreamReader sr = new StreamReader(path, System.Text.Encoding.Default))
            {
                string line;
                while (((line = sr.ReadLine()) != null))
                {
                    int position = 0;
                    position = line.IndexOf(textBox7.Text);
                    if (position > 0)
                    { break; }

                }
                string USD_fromfile, EUR_fromfile, BTC_fromfile;
                USD_fromfile = sr.ReadLine();
                EUR_fromfile = sr.ReadLine();
                BTC_fromfile = sr.ReadLine();
                textBox1.Text = Parser_FromFile(USD_fromfile);
                textBox5.Text = Parser_FromFile(EUR_fromfile);
                textBox6.Text = Parser_FromFile(BTC_fromfile);
                sr.Close();
            }
            }
            catch (Exception exc) { MessageBox.Show("There is no data for this day or Incorrect syntax(Example: 25.10.2016)","Incorrect syntax or not data",MessageBoxButtons.RetryCancel); }

        }

        Graph[] G = new Graph[5];
        private void button3_Click(object sender, EventArgs e)
        {
            DateTime Now = DateTime.Today;
            int i = 0;
            string ap = @"""";
            while (i != 5)
            {
                String NowS = ap+Now.Day + "." + Now.Month + "." + Now.Year+ap;
                try
                {

                    string path = @"C:\Users\Никита\YandexDisk\Exchange_Rates_1\exchange_rates.txt";
                    using (StreamReader sr = new StreamReader(path, System.Text.Encoding.Default))
                    {
                        string line;
                        int minus = 0;
                        while (((line = sr.ReadLine()) != null))
                        {
                            int position = 0;
                            position = line.IndexOf(NowS);
                            if (position > 0)
                            {
                                minus++;
                                break;
                            }
                        }
                        if (minus == 0) { Now = Now.AddDays(-1); minus = 0; }
                        else
                        {
                            string USD_fromfile, EUR_fromfile, BTC_fromfile;
                            USD_fromfile = sr.ReadLine();
                            EUR_fromfile = sr.ReadLine();
                            BTC_fromfile = sr.ReadLine();
                            G[i].Date = NowS;
                            G[i].USD = double.Parse(Parser_FromFile(USD_fromfile));
                            G[i].EUR = double.Parse(Parser_FromFile(EUR_fromfile));
                            G[i].BTC = double.Parse(Parser_FromFile(BTC_fromfile));
                            Now = Now.AddDays(-1);                            
                            minus = 0;
                            i++;
                        }
                        
                        sr.Close(); 
                    }
                }
                catch
                {
                }                
            }

            if (radioButton1.Checked == true) { Draw("USD");}
            if (radioButton2.Checked == true) { Draw("EUR");}
            if (radioButton3.Checked == true) { Draw("BTC");}
            
        }

         void Draw(String Currency){
            Graphics gr = panel1.CreateGraphics();
            gr.Clear(Color.White);
            Pen mypen = new Pen(Color.Black, 2);
            Pen pen = new Pen(Color.Green, 3f); // перо, для отрисовки графика
            Pen gridPen = new Pen(Color.Black, 0.0001f); //перо для отрисовки координатной сетки
            Pen penCO = new Pen(Color.Black, 2f);
            gr.DrawLine(penCO, new Point(0, 0), new Point(745, 0)); //555
            gr.DrawLine(penCO, new Point(0, 0), new Point(0, 250));//165
            gr.DrawLine(penCO, new Point(0, 250), new Point(745, 250));//165
            int x = 0; //начальное значение координаты х. Постороение идет из указанной точки
            int y = 0; // начальное значение координаты у. Пояснение см выше.

            while (x <= 730) //570
            {
                x = x + 15; // шаг линий, параллельных оси ОУ
                y = y + 15; //шаг линий, параллельных оси ОХ
                gr.DrawLine(gridPen, new Point(x, 0), new Point(x, 250)); // рисуем линии, параллельные оси ОУ
                gr.DrawLine(gridPen, new Point(0, y), new Point(745, y)); //рисуем линии, параллельные оси ОХ

            }
            List<Point> p = new List<Point>();
            int x1, y1;
            double max = 0, min = 1000000;
            double scale =0;
            Point ps;
            switch (Currency)
            {
                case "USD":
                    for (int i = 0; i < 5; i++)
                    {
                        if (G[i].USD > max) { max = G[i].USD; }
                        if (G[i].USD < min) { min = G[i].USD; }
                    }
                    max++;
                    min--;
                    scale = 180 / (max - min);
                    for (int n = 4; n > 0; n--)
                    {
                        x1 = 560 - 140 * n;
                        y1 = (int)(scale * (G[n].USD - min));
                        Point poss = new Point(x1, 180 - y1);
                        label8.Text = y1.ToString();
                        p.Add(poss);
                    }
                    x1 = 745;
                    y1 = (int)(scale * (G[0].USD - min));
                    ps = new Point(x1, 180 - y1);
                    p.Add(ps);
                    break;
                case "EUR":
                    for (int i = 0; i < 5; i++)
                    {
                        if (G[i].EUR > max) { max = G[i].EUR; }
                        if (G[i].EUR < min) { min = G[i].EUR; }
                    }
                    max++;
                    min--;
                    scale = 180 / (max - min);
                    for (int n = 4; n > 0; n--)
                    {
                        x1 = 560 - 140 * n;
                        y1 = (int)(scale * (G[n].EUR - min));
                        Point poss = new Point(x1, 180 - y1);
                        label8.Text = y1.ToString();
                        p.Add(poss);
                    }
                    x1 = 745;
                    y1 = (int)(scale * (G[0].EUR - min));
                    ps = new Point(x1, 180 - y1);
                    p.Add(ps);
                    break;
                case "BTC":
                    for (int i = 0; i < 5; i++)
                    {
                        if (G[i].BTC > max) { max = G[i].BTC; }
                        if (G[i].BTC < min) { min = G[i].BTC; }
                    }
                    max++;
                    min--;
                    scale = 180 / (max - min);
                    for (int n = 4; n > 0; n--)
                    {
                        x1 = 560 - 140 * n;
                        y1 = (int)(scale * (G[n].BTC - min));
                        Point poss = new Point(x1, 180 - y1);
                        label8.Text = y1.ToString();
                        p.Add(poss);
                    }
                    x1 = 745;
                    y1 = (int)(scale * (G[0].BTC - min));
                    ps = new Point(x1, 180 - y1);
                    p.Add(ps);
                    break;
            }            
            
            label9.Text = ((int)max).ToString();
            label10.Text = ((int)min).ToString();
            label16.Text = ((int)((max + min) / 2)).ToString();            
            gr.DrawCurve(pen, p.ToArray());
            string ap = @"""";
            for (int i = 0; i < 5; i++)
            { G[i].Date = G[i].Date.Replace(ap, ""); }
            label11.Text = G[4].Date;
            label12.Text = G[3].Date;
            label13.Text = G[2].Date;
            label14.Text = G[1].Date;
            label15.Text = G[0].Date;
        }       
         private void label11_Click(object sender, EventArgs e)
         {

         }
    }
}
