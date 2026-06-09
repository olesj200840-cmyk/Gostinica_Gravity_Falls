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
    /// Логика взаимодействия для Add_Booking.xaml
    /// </summary>
    public partial class Add_Booking : Window
    {
        // Сохраняем номер и гостя как поля класса
        private Room _selectedRoom;
        
        public Add_Booking(Room selectedRoom)
        {
            InitializeComponent();
            _selectedRoom = selectedRoom;
            this.Title = $"Бронирование номера: {_selectedRoom.Name}";
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            // 1. Проверяем заполненность всех полей
            if (string.IsNullOrWhiteSpace(tbFullName.Text) ||
                string.IsNullOrWhiteSpace(tbPhoneNumber.Text) ||
                dpCheckIn.SelectedDate == null ||
                dpCheckOut.SelectedDate == null)
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
                return;
            }

            // 2. Создаем НОВОГО гостя с введенными данными
            var newGuest = new Gost
            {
                FullName = tbFullName.Text,
                PhoneNumber = tbPhoneNumber.Text
            };

            // 3. Добавляем гостя в общий список через DataManager
            DataManager.AddGuest(newGuest);

            // 4. Создаем бронь, используя ID только что добавленного гостя
            var newBooking = new Booking
            {
                RoomId = _selectedRoom.Id,
                GuestId = newGuest.Id, // Используем ID нового гостя
                CheckInDate = dpCheckIn.SelectedDate.Value,
                CheckOutDate = dpCheckOut.SelectedDate.Value,
                Status = "Ожидание"
            };
            // 5. Пытаемся добавить бронь
            bool success = DataManager.AddBooking(newBooking);

            if (success)
            {
                MessageBox.Show("Бронь успешно создана!");
                this.DialogResult = true; // Успешное закрытие
                this.Close();
            }
            else
            {
                MessageBox.Show("Ошибка: Дата выезда должна быть позже даты заезда.");
            }
        }
        
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Точно хотите выйти без сохранения?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                DialogResult = false; // Закрываем окно с результатом "Отмена"
                this.Close();
            }
        }
    }
}
