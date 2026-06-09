using System;
using System.Collections.Generic;
using System.Text;

namespace Gostinica
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; } // Например, "Люкс 101"
        public int CategoryId { get; set; } // Ссылка на категорию
        public int Capacity { get; set; } // Количество мест
        public decimal PricePerNight { get; set; } // Цена за ночь

        // Свойство для отображения статуса в виде строки
        public string StatusDisplay
        {
            get
            {
                return Status == 0 ? "Свободен" : Status == 1 ? "Забронирован" : "Занят";
            }
        }

        // Внутренний статус: 0 - Свободен, 1 - Забронирован, 2 - Занят
        public int Status { get; set; }
    }
}
