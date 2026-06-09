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

            // 1. Берем все свободные комнаты
            var freeRooms = DataManager.GetRooms().Where(r => r.Status == 0).ToList();

            // 2. Для каждой комнаты создаем анонимный объект с нужными нам свойствами
            var roomsForDisplay = freeRooms.Select(room => new
            {
                // Берем свойства напрямую из объекта Room
                room.Name,
                room.StatusDisplay,

                // Находим категорию по ID и берем её название
                // Если категории нет, показываем "Неизвестно"
                CategoryName = DataManager._categories.FirstOrDefault(c => c.Id == room.CategoryId)?.Name ?? "Неизвестно"
            }).ToList();

            // 3. Привязываем к ListBox наш новый список анонимных объектов
            listRooms.ItemsSource = roomsForDisplay;

        }

        

        private void exit_client_vxod(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ListRooms_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedItem = listRooms.SelectedItem;

            if (selectedItem != null)
            {
                // Нам нужно получить исходный объект Room, чтобы работать с ним дальше.
                // Для этого сначала получаем имя из выбранного объекта.
                var selectedName = selectedItem.GetType().GetProperty("Name").GetValue(selectedItem).ToString();

                // Ищем исходный объект Room в главном списке по этому имени
                var originalRoom = DataManager.GetRooms().FirstOrDefault(r => r.Name == selectedName);

                if (originalRoom != null && originalRoom.Status == 0)
                {
                    var add_Booking = new Add_Booking(originalRoom);
                    add_Booking.Owner = this;
                    add_Booking.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                    bool? result = add_Booking.ShowDialog();

                    if (result == true)
                    {
                        // При успешном бронировании пересоздаем список для отображения
                        // (т.к. статус комнаты изменился и она должна исчезнуть из списка)
                        var roomsForDisplay = DataManager.GetRooms().Where(r => r.Status == 0).Select(room => new
                        {
                            room.Name,
                            room.StatusDisplay,
                            CategoryName = DataManager._categories.FirstOrDefault(c => c.Id == room.CategoryId)?.Name ?? "Неизвестно"
                        }).ToList();

                        listRooms.ItemsSource = roomsForDisplay;
                    }
                }
            }
        }
    }
}
