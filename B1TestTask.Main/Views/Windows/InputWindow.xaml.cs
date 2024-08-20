using System.Windows;

namespace B1TestTask.Main.Views.Windows
{
    public partial class InputWindow : Window
    {
        public string Result 
        {
            get => inputBlock.Text;
            set { }
        }

        public InputWindow(string messageStr)
        {
            InitializeComponent();
            message.Text = messageStr;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Result = inputBlock.Text;
            this.Close();
        }
    }
}
