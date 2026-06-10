using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Gostinica
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ClientMain clientMain;
        private Admin_vxod admin;
        public static MainWindow Instance;
        public MainWindow()
        {
            InitializeComponent();
            Instance=this;
        }

        private void client_click(object sender, RoutedEventArgs e)
        {
            // Проверяем, не создано ли окно ИЛИ не загружено ли оно (закрыто)
            if (clientMain == null || !clientMain.IsLoaded)
            {
                clientMain = new ClientMain();
                clientMain.Closed += (s, args) => clientMain = null;
                clientMain.Show();
            }
            else
            {
                // Если окно уже открыто, просто активируем его (выводим на передний план)
                clientMain.Activate();
            }
        }

        private void admin_click(object sender, RoutedEventArgs e)
        {
            // Проверяем, не создано ли окно ИЛИ не загружено ли оно (закрыто)
            if (admin == null || !admin.IsLoaded)
            {
                admin = new Admin_vxod();
                admin.Closed += (s, args) => admin = null;
                admin.Show();
            }
            else
            {
                // Если окно уже открыто, просто активируем его (выводим на передний план)
                admin.Activate();
            }
        }
    }
}
  