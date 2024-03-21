using System;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace Calc
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            foreach (UIElement el in MainRoot.Children)
            {
                if (el is Button)
                {
                    ((Button)el).Click += Button_Click;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string[] calcOps = new string[] { "+", "-", "*", "/" };

            if (calcOutput.Text == "∞")
                calcOutput.Text = String.Empty;

            string str = (string)((Button)e.OriginalSource).Content;
            switch (str)
            {
                case string addChar when new Regex(@"[0-9]").IsMatch(addChar):
                    calcOutput.Text += str;
                    break;
                case string addChar when new Regex(@"[+|\-|*|/]").IsMatch(addChar):
                    if (calcOutput.Text.Length > 0)
                    {
                        if (calcOps.Contains(calcOutput.Text[calcOutput.Text.Length - 1].ToString()))
                            calcOutput.Text = calcOutput.Text.Remove(calcOutput.Text.Length - 1);
                        calcOutput.Text += str;
                    }
                    else if (str == "-")
                    {
                        calcOutput.Text += str;
                    };
                    break;
                case "=":
                    if (calcOps.Contains(calcOutput.Text[calcOutput.Text.Length - 1].ToString()))
                        calcOutput.Text = calcOutput.Text.Remove(calcOutput.Text.Length - 1);

                    string res = new DataTable().Compute(calcOutput.Text, null).ToString();
                    if (double.TryParse(res, out double result))
                    {
                        if (double.IsNaN(result) || double.IsInfinity(result))
                            calcOutput.Text = "0";
                        else
                            calcOutput.Text = res;
                    }
                    else
                    {
                        calcOutput.Text = "∞";
                    }
                    calcOutput.Text = res;
                    break;
                case "AC":
                    calcOutput.Text = String.Empty;
                    break;
                default:
                    throw new Exception("Undefined button");
            }
        }
    }
}
