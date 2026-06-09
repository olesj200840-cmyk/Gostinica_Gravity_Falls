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
            // Загрузка начальных данных (если еще не загружены)
            if (DataManager.GetRooms().Count == 0)
                DataManager.SeedData();

            // Привязка источника данных к ListBox
            listRooms.ItemsSource = DataManager.GetRooms();
        }

        private void exit_client_vxod(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Add_click(object sender, RoutedEventArgs e)
        {
            // --- ДОБАВИТЬ НОВЫЙ НОМЕР ---
            var editWnd = new EditRoomWindow(null); // Передаем null для нового номера

            editWnd.Owner = this;
            editWnd.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            bool? result = editWnd.ShowDialog();

            if (result == true) // Если нажали "Сохранить"
            {
                // Добавляем новую комнату из окна в твой DataManager.
                DataManager.AddRoom(editWnd._roomToEdit);
                listRooms.Items.Refresh(); // Обновляем список на экране админа.
            }

        }

        private void redactor_click(object sender, RoutedEventArgs e)
        {
            // --- РЕДАКТИРОВАТЬ ВЫБРАННЫЙ НОМЕР ---
            var selectedRoom = listRooms.SelectedItem as Room; // Получаем выбранный номер

            if (selectedRoom != null)
            {
                var editWnd = new EditRoomWindow(selectedRoom); // Передаем выбранный номер

                editWnd.Owner = this;
                editWnd.WindowStartupLocation = WindowStartupLocation.CenterOwner;

                editWnd.ShowDialog(); // Показываем окно

                // Обновляем список, чтобы увидеть изменения (например, новое имя).
                listRooms.Items.Refresh();
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите номер из списка для редактирования.");
            }
        }
        
    }
}
