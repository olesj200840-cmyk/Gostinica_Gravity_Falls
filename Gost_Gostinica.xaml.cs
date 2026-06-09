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
    /// Логика взаимодействия для Gost_Gostinica.xaml
    /// </summary>
    public partial class Gost_Gostinica : Window
    {
        public Gost_Gostinica()
        {
            InitializeComponent();
            // Заполняем базу тестовыми данными при запуске
            DataManager.SeedData();

            // Привязываем к ListBox только свободные номера (Статус == 0)
            // Используем ToList(), чтобы зафиксировать выборку на момент открытия окна.
            listRooms.ItemsSource = DataManager.GetRooms().Where(r => r.Status == 0).ToList();

        }

        

        private void exit_client_vxod(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ListRooms_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // selectedRoom теперь существует в этом контексте, так как мы берем его из listRooms.SelectedItem
            var selectedRoom = listRooms.SelectedItem as Room;

            if (selectedRoom != null && selectedRoom.Status == 0) // Проверяем, что номер свободен
            {
                // Открываем окно бронирования, передавая выбранный номер
                var addBookingWnd = new Add_Booking(selectedRoom);
                addBookingWnd.Owner = this;
                addBookingWnd.WindowStartupLocation = WindowStartupLocation.CenterOwner;

                bool? result = addBookingWnd.ShowDialog(); // Ждем закрытия окна

                // Если бронь создана успешно...
                if (result == true)
                {
                    // Обновляем список номеров в главном окне.
                    listRooms.ItemsSource = DataManager.GetRooms().Where(r => r.Status == 0).ToList();
                    listRooms.Items.Refresh();
                }
            }
        }
    }
}
