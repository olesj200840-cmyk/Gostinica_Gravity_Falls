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
    /// Логика взаимодействия для EditRoomWindow.xaml
    /// </summary>
    public partial class EditRoomWindow : Window
    {
        public Room _roomToEdit;
        private bool _isDirty = false;
        public EditRoomWindow(Room roomToEdit = null)
        {
            InitializeComponent();
            if (roomToEdit != null)
            {
                // Режим РЕДАКТИРОВАНИЯ существующего номера
                _roomToEdit = roomToEdit;

                tbName.Text = _roomToEdit.Name;
                tbCapacity.Text = _roomToEdit.Capacity.ToString();
                tbPrice.Text = _roomToEdit.PricePerNight.ToString();

                // Слушаем изменения в полях ввода
                tbName.TextChanged += (s, e) => _isDirty = true;
                tbCapacity.TextChanged += (s, e) => _isDirty = true;
                tbPrice.TextChanged += (s, e) => _isDirty = true;
            }
            else
            {
                // Режим ДОБАВЛЕНИЯ нового номера (создаем пустой объект)
                _roomToEdit = new Room { Status = 0 }; // По умолчанию свободен
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            // Сохраняем данные из полей в объект комнаты
            if (!int.TryParse(tbCapacity.Text, out int capacity))
            {
                MessageBox.Show("Вместимость должна быть целым числом.");
                return;
            }

            if (!decimal.TryParse(tbPrice.Text, out decimal price))
            {
                MessageBox.Show("Цена должна быть числом.");
                return;
            }

            _roomToEdit.Name = tbName.Text;
            _roomToEdit.Capacity = capacity;
            _roomToEdit.PricePerNight = price;

            DialogResult = true; // Успешное закрытие окна с результатом "ОК"
            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (_isDirty) // Если данные менялись...
            {
                var result = MessageBox.Show("Точно хотите выйти без сохранения?", "Подтверждение", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    DialogResult = false; // Закрываем с отменой
                    this.Close();
                }
            }
            else // Если данные не менялись...
            {
                DialogResult = false;
                this.Close();
            }
        }
    }
}
