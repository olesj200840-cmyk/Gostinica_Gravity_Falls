using System;
using System.Collections.Generic;
using System.Text;

namespace Gostinica
{
    class Booking
    {
        public int Id { get; set; }
        public int RoomId { get; set; } // К какому номеру относится бронь
        public int GuestId { get; set; } // Кто забронировал

        // Даты заезда и выезда. Используем DateTime для работы с датами.
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }

        // Статус брони как текст (Подтверждена, Ожидание)
        public string Status { get; set; }
    }
}
