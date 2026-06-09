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
            DateTime checkInDate = dpCheckIn.SelectedDate.Value;
            DateTime checkOutDate = dpCheckOut.SelectedDate.Value;

            // 2. Проверяем, что даты логичны
            if (checkOutDate <= checkInDate)
            {
                MessageBox.Show("Дата выезда должна быть позже даты заезда.");
                return;
            }

            // 3. ПРОВЕРКА НА НАЛИЧИЕ ДРУГОЙ БРОНИ (ГЛАВНОЕ ИЗМЕНЕНИЕ)
            bool isAvailable = DataManager.IsRoomAvailableForDates(_selectedRoom.Id, checkInDate, checkOutDate);

            if (!isAvailable)
            {
                MessageBox.Show("Извините, этот номер уже забронирован на выбранные даты. Пожалуйста, выберите другой номер или другие даты.");
                return; // Выходим из метода, ничего не создаем
            }
            // 4. Если номер свободен, создаем гостя и бронь
            var newGuest = new Gost
            {
                FullName = tbFullName.Text,
                PhoneNumber = tbPhoneNumber.Text
            };
            DataManager.AddGuest(newGuest);

            var newBooking = new Booking
            {
                RoomId = _selectedRoom.Id,
                GuestId = newGuest.Id,
                CheckInDate = checkInDate,
                CheckOutDate = checkOutDate,
                Status = "Ожидание" // Или "Забронировано вами"
            };

            // Добавление брони теперь всегда будет успешным, так как мы проверили доступность выше
            DataManager.AddBooking(newBooking);

            // Обновляем статус номера в самом объекте комнаты
            var roomToUpdate = DataManager.GetRooms().FirstOrDefault(r => r.Id == _selectedRoom.Id);
            if (roomToUpdate != null)
            {
                roomToUpdate.Status = 1; // Статус "Забронирован"
            }

            MessageBox.Show("Бронь успешно создана!");
            this.DialogResult = true;
            this.Close();
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
