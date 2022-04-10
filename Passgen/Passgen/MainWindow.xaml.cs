using System;
using System.Collections.Generic;
using System.IO;
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
using Newtonsoft.Json;

namespace Passgen
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public class Account {
            public List<string> List { get; set; }
        }

        public void Jsonlist() {
            Account account = JsonConvert.DeserializeObject<Account>(System.IO.File.ReadAllText("Typelist.json"));
            for (int i = 0; i < account.List.Count; i++) {
                accountBox.Items.Add(account.List[i]);
            }
        }

        private static Random random = new Random();
        public static string RandomString(int length) {
            const string chars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz0123456789!@$?_#$%*";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }


        public void styleChange(string type, dynamic element, string foreground, string background, string text) {
            BrushConverter bc = new BrushConverter();
            switch (type)
            {
                case "box":
                    element.Background = (Brush)bc.ConvertFrom(background);
                    break;
                case "label":
                    element.Foreground = (Brush)bc.ConvertFrom(foreground);
                    element.Content = text;
                    break;
                default:
                    MessageBox.Show("3");
                    return;
            }
        }

        public async Task<string> barActions(int number) {
            string[] lookup = {
                "Error",
                "Successful",
                "Ready for use"
            };
            BrushConverter bc = new BrushConverter();
            switch (number)
            {
                case 0:
                    styleChange("box", infoBar, "None", "#F95454", "");
                    styleChange("label", infoLabel, "#FFFFFF", "None", lookup[number].ToUpper());
                    await Task.Delay(3000);
                    styleChange("box", infoBar, "None", "#FF1B1B20", "");
                    styleChange("label", infoLabel, "#FFAFB2C8", "None", lookup[2].ToUpper());
                    break;
                case 1:
                    var generatedPassword = accountBox.SelectedItem + "~" + RandomString(25);

                    styleChange("box", infoBar, "None", "#5ED450", "");
                    styleChange("label", infoLabel, "#FFFFFF", "None", lookup[number].ToUpper());

                    passwordGenBox.Text = generatedPassword;

                    await Task.Delay(3000);
                    styleChange("box", infoBar, "None", "#FF1B1B20", "");
                    styleChange("label", infoLabel, "#FFAFB2C8", "None", lookup[2].ToUpper());
                    break;
                default:
                    break;
            }
            return lookup[number];
        }

        public void test()
        {
            BrushConverter bc = new BrushConverter();
            infoBar.Background = (Brush)bc.ConvertFrom("#FF4D4D");
        }

        private async void onGenerateButtonClick(object sender, RoutedEventArgs e)
        {
            var returnvariable = false ? "" : accountBox.SelectedItem;
            if (accountBox.SelectedItem == "" || accountBox.SelectedItem == null) {
                await barActions(0);
            } else {
                await barActions(1);
            }
        }

        private void onBoxSelection(object sender, SelectionChangedEventArgs e) {

        }

        public MainWindow() {
            InitializeComponent();
            Jsonlist();
        }
    }
}
