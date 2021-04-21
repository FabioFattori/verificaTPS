using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.IO;

namespace verifica_21_4_21
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int lunghezzaStringa;
        List<string> caratteriPossibili;
        Random r;
        int i ;
        public MainWindow()
        {
            InitializeComponent();
            r = new Random();
            caratteriPossibili = new List<string>();
            btn_estrai.Visibility = Visibility.Hidden;
           
        }

        public void LeggiFile()
        {
            using(StreamReader sr=new StreamReader("file.txt"))
            {
                while (sr.Peek() != -1)
                {
                    caratteriPossibili.Add(sr.ReadLine());
                }
            }
        }

        private async void Sorteggio()
        {
            await Task.Run(() =>
            {
                int sorteggiato;
                i = 0;
                while (i<lunghezzaStringa)
                {
                    sorteggiato = r.Next(1, caratteriPossibili.Count);
                    Thread.Sleep(550);
                    this.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        lbl_letteraCheCambia.Content = caratteriPossibili[sorteggiato];
                    }));
                    
                }
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    btn_estrai.IsEnabled = false;
                }));
                
            });
        }

        private void btn_estrai_Click(object sender, RoutedEventArgs e)
        {
            lbl_stringaCheVieneComposta.Content += (string)lbl_letteraCheCambia.Content;
            i++;
        }

        
        private async void btn_start_Click(object sender, RoutedEventArgs e)
        {
            btn_start.IsEnabled = false;
            LeggiFile();
            
            lunghezzaStringa = Convert.ToInt32(txt_lunghezzaParola.Text);

            Sorteggio();
            btn_estrai.Visibility = Visibility.Visible;
            

            await Task.Run(() =>
            {
                while (true)
                {
                    AggiungiInLista();
                }
            });
                
        }

        private async void txt_lunghezzaParola_TextChanged(object sender, TextChangedEventArgs e)
        {
            await Task.Run(() =>
            {
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    try
                    {
                        lunghezzaStringa = Convert.ToInt32(txt_lunghezzaParola.Text);
                    }
                    catch
                    {
                        lunghezzaStringa = 1;
                    }
                }));
                
            });

            
        }

        public void AggiungiInLista()
        {
            if (i == lunghezzaStringa)
            {
                
                i = 0;
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    
                    lst_listaDiParole.Items.Add(lbl_stringaCheVieneComposta.Content);
                    lbl_stringaCheVieneComposta.Content = "";
                    
                }));
                
            }
        }
    }
}
