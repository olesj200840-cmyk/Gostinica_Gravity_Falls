using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Gostinica
{
    /// <summary>
    /// Логика взаимодействия для Admin_vxod.xaml
    /// </summary>
    public partial class Admin_vxod : Window
    {
        private ClientMain _clientMain;
        private const string Olesya = "asd123zxc";
        private const string Veronika = "pyps";
        public Admin_vxod()
        {
            InitializeComponent();
        }

        private void Pasword_Admin(object sender, RoutedEventArgs e)
        {
            // 1. Проверка пароля
            if (PaswordAdmin.Password == Olesya || PaswordAdmin.Password == Veronika)
            {
                CloseAccept();
                MessageTextBlockAdmin.Text = "Добро пожаловать в Gravity Falls Admins";
            }
            else
            {
                // Если пароль неверный — показываем 
                MessageText.Text = "Неверный пароль! Вы точно админ?";
                MessageText.Visibility = Visibility.Visible;
            }
        }
        // 2. метод для скрытия элементов
        private void HideAllInterfaceElements()
        {
            PaswordAdmin.Visibility = Visibility.Collapsed;
            Button_vxodAdmin.Visibility = Visibility.Collapsed;
            vod_posword.Visibility = Visibility.Collapsed;
            Privet.Visibility = Visibility.Collapsed;
            MessageText.Visibility= Visibility.Collapsed;

            // показываем текстовое сообщение
            MessageTextBlockAdmin.Visibility = Visibility.Visible;
        }
        // 3. Метод для плавного перехода между окнами
        private async void CloseAccept()
        {
            
            // Скрываем все элементы интерфейса
            HideAllInterfaceElements();
            Admin_Gostinica admin_gostinica = new Admin_Gostinica();

            // Ждем 1 секунду (1000 миллисекунд), не блокируя UI-поток
            await Task.Delay(1000);
            
            this.Close();
            MainWindow.Instance?.Close(); // '?' проверяет на null перед вызовом Close()

            admin_gostinica.Show();
        }

        private void exit_admin_vxod(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
