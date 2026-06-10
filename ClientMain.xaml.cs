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
    /// Логика взаимодействия для ClientMain.xaml
    /// </summary>
    public partial class ClientMain : Window
    {
        public static ClientMain _clientMain;
        public ClientMain()
        {
            InitializeComponent();
            _clientMain=this;
        }

        private void Pasword_client(object sender, RoutedEventArgs e)
        {
            // 1. Проверка пароля
            if (Accept_button.IsChecked == true)
            {
                CloseAccept();
                MessageTextBlock.Text = "Добро пожаловать в Gravity Folls";
            }
            else
            {
                // Если капча неверный — показываем ошибку
                Closenet();
                MessageTextBlock.Text = "Неверно!";
            }
        }
        // метод для скрытия элементов
        private void HideAllInterfaceElements()
        {
            net_button1.Visibility = Visibility.Collapsed;
            net_button2.Visibility = Visibility.Collapsed;
            net_button3.Visibility = Visibility.Collapsed;
            Accept_button.Visibility = Visibility.Collapsed;
            horosho.Visibility = Visibility.Collapsed;
            Privet.Visibility = Visibility.Collapsed;

            // показываем текстовое сообщение
            MessageTextBlock.Visibility = Visibility.Visible;
        }
        // Метод для плавного перехода между окнами
        private async void CloseAccept()
        {
            
            // Скрываем все элементы интерфейса
            HideAllInterfaceElements();
            // Создаем и показываем новое окно
            Gost_Gostinica gost_gostinica = new Gost_Gostinica();

            // Ждем 1 секунду (1000 миллисекунд), не блокируя UI-поток
            await Task.Delay(1000);

            // Безопасно закрываем текущее окно и главное окно
            Admin_vxod._admin_vxod?.Close();
            this.Close();
            MainWindow.Instance?.Close(); // '?' проверяет на null перед вызовом Close()

            gost_gostinica.Show();
        }
        private async void Closenet()
        {
            HideAllInterfaceElements();
            await Task.Delay(1000);
            Admin_vxod._admin_vxod?.Close();
            this.Close();
            MainWindow.Instance?.Close(); 
        }
        private void exit_client_vxod(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
