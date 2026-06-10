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
    /// Логика взаимодействия для Admin_Gostinica.xaml
    /// </summary>
    public partial class Admin_Gostinica : Window
    {
        public Admin_Gostinica()
        {
            InitializeComponent();
            if (DataManager.GetRooms().Count == 0)
                DataManager.SeedData();

            RefreshRoomList(); // Используем наш метод для инициализации списка
        }

        private void exit_client_vxod(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Add_click(object sender, RoutedEventArgs e)
        {
            var editWnd = new EditRoomWindow(null); // null для нового номера

            editWnd.Owner = this;
            editWnd.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            bool? result = editWnd.ShowDialog();

            if (result == true)
            {
                // Добавление уже произошло внутри окна (в btnSave_Click)
                // Теперь нужно просто обновить отображение списка
                RefreshRoomList();
            }

        }

        private void redactor_click(object sender, RoutedEventArgs e)
        {
            // 1. Проверяем, выбран ли элемент в списке
            if (listRooms.SelectedItem == null)
            {
                MessageBox.Show("Пожалуйста, выберите номер для редактирования.");
                return;
            }

            // 2. Находим исходный объект Room в DataManager
            Room selectedRoom = GetOriginalRoomFromSelectedItem(listRooms.SelectedItem);
            if (selectedRoom == null)
            {
                MessageBox.Show("Не удалось найти информацию о номере.");
                return;
            }

            // 3. Открываем окно редактирования, передавая найденный объект
            var editWnd = new EditRoomWindow(selectedRoom);
            editWnd.Owner = this;
            editWnd.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            bool? result = editWnd.ShowDialog();

            // 4. Если пользователь нажал "Сохранить" (result == true), обновляем список на экране
            if (result == true)
            {
                RefreshRoomList(); // Обновляем ListBox, чтобы отобразить изменения
            }
        }
        // Этот метод находит настоящий объект Room в DataManager,
        // используя информацию из выбранного в ListBox элемента.
        private Room GetOriginalRoomFromSelectedItem(object selectedItem)
        {
            try
            {
                // Получаем свойство "Name" из анонимного объекта в ListBox
                var selectedName = selectedItem.GetType().GetProperty("Name").GetValue(selectedItem).ToString();

                // Ищем исходный объект Room по этому имени в главном списке
                return DataManager.GetRooms().FirstOrDefault(r => r.Name == selectedName);
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка при получении данных о номере.");
                return null;
            }
        }

        // --- МЕТОД ДЛЯ ОБНОВЛЕНИЯ СПИСКА ---
        // Чтобы не дублировать код обновления ListBox, вынесем его в отдельный метод.
        private void RefreshRoomList()
        {
            var roomsForDisplay = DataManager.GetRooms().Select(room => new
            {
                room.Name,
                room.StatusDisplay,
                CategoryName = DataManager.GetRooms().FirstOrDefault(c => c.Id == room.CategoryId)?.Name ?? "Неизвестно"
            }).ToList();

            listRooms.ItemsSource = roomsForDisplay;
        }
    }
}
