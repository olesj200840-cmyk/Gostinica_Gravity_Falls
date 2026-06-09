using System;
using System.Collections.Generic;
using System.Text;

namespace Gostinica
{
    class DataManager
    {
        private static List<Room> _rooms = new List<Room>();
        private static List<Booking> _bookings = new List<Booking>();
        private static List<Category> _categories = new List<Category>();
        private static List<Gost> _guests = new List<Gost>();

        private static int _roomIdCounter = 1;
        private static int _bookingIdCounter = 1;
        private static int _guestIdCounter = 1;

        // Получить список всех номеров
        public static List<Room> GetRooms()
        {
            return _rooms;
        }
        // Добавить новый номер. Принимает объект Room.
        public static void AddRoom(Room room)
        {
            room.Id = _roomIdCounter++;
            _rooms.Add(room);
        }
        // Получить список всех броней
        public static List<Booking> GetBookings()
        {
            return _bookings;
        }

        public static bool AddBooking(Booking booking)
        {
            // 1. Валидация дат: Дата выезда должна быть позже даты заезда.
            if (booking.CheckOutDate <= booking.CheckInDate)
            {
                return false; // Ошибка валидации
            }

            booking.Id = _bookingIdCounter++;
            _bookings.Add(booking);

            // 2. Изменение статуса номера на "Забронирован"
            var room = _rooms.FirstOrDefault(r => r.Id == booking.RoomId);
            if (room != null)
            {
                room.Status = 1;
            }
            return true;
        }
        public static void AddGuest(Gost guest)
        {
            guest.Id = _guestIdCounter++;
            _guests.Add(guest);
        }

        public static void AddCategory(Category category)
        {
            category.Id = _roomIdCounter++; // Используем тот же счетчик для простоты или отдельный.
            _categories.Add(category);
        }
        // Метод-пример для заполнения базы данных начальными данными при запуске программы.
        public static void SeedData()
        {
            AddCategory(new Category { Name = "Эконом", Description = "Базовый набор" });
            AddCategory(new Category { Name = "Комфорт", Description = "Кондиционер, Wi-Fi" });

            AddRoom(new Room { Name = "101", CategoryId = 1, Capacity = 2, PricePerNight = 2000, Status = 0 });
            AddRoom(new Room { Name = "202", CategoryId = 2, Capacity = 3, PricePerNight = 4500, Status = 0 });

            AddGuest(new Gost { FullName = "Иванов Иван", PhoneNumber = "+79991112233" });
        }
    }
}
