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

namespace MadCalc
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Calculator _calc;

        public MainWindow()
        {
            InitializeComponent();
            _calc = new Calculator();
            LoadAddIns();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SetDefault();
            foreach (Button button in this.FindVisualChildren<Button>())
            {
                button.Click += Button_Click;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string operation = ((Button)sender).Content.ToString();
            if (operation == "CE")
            {
                SetDefault();
            }
            else
            {
                try
                {
                      txtDisplay.Text = _calc.Calculate(operation, txtDisplay.Text.Trim()).ToString();
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            
        }

        private void SetDefault()
        {
            txtDisplay.Text = "0";
            txtDisplay.SelectAll();
            txtDisplay.Focus();
        }

        private void LoadAddIns()
        {
            foreach(string key in _calc.AddIns)
            {
                Button button = new Button() { Content = key };
                AddButton(button);
            }
        }

        private void AddButton(Button button)
        {
            ButtonsPanel.Children.Add(button);
            button.Style = (Style)this.Resources["buttonStyle"];
        }
    }
}
