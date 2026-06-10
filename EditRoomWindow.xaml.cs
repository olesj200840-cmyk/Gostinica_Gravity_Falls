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
            //Заполняем ComboBox списком категорий из DataManager
            cbCategory.ItemsSource = DataManager.GetRooms();

            if (roomToEdit != null)
            {
                
                _roomToEdit = roomToEdit;

                tbName.Text = _roomToEdit.Name;
                tbCapacity.Text = _roomToEdit.Capacity.ToString();
                tbPrice.Text = _roomToEdit.PricePerNight.ToString();

                // Устанавливаем выбранный элемент в ComboBox по CategoryId комнаты
                cbCategory.SelectedValue = _roomToEdit.CategoryId;

                // Слушаем изменения во всех полях ввода
                tbName.TextChanged += (s, e) => _isDirty = true;
                tbCapacity.TextChanged += (s, e) => _isDirty = true;
                tbPrice.TextChanged += (s, e) => _isDirty = true;
                cbCategory.SelectionChanged += (s, e) => _isDirty = true;
            }
            else
            {
                
                _roomToEdit = new Room { }; // По умолчанию свободен

                // Можно установить категорию по умолчанию (например, первую в списке)
                if (DataManager.GetRooms().Any())
                    cbCategory.SelectedIndex = 0;

                // Слушаем изменения во всех полях ввода
                tbName.TextChanged += (s, e) => _isDirty = true;
                tbCapacity.TextChanged += (s, e) => _isDirty = true;
                tbPrice.TextChanged += (s, e) => _isDirty = true;
                cbCategory.SelectionChanged += (s, e) => _isDirty = true;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            //  Базовая проверка: являются ли введенные данные числами?
            if (!int.TryParse(tbCapacity.Text, out int capacity))
            {
                MessageBox.Show("Вместимость должна быть целым числом.");
                tbCapacity.Focus();
                return;
            }

            if (!decimal.TryParse(tbPrice.Text, out decimal price))
            {
                MessageBox.Show("Цена должна быть числом.");
                tbPrice.Focus();
                return;
            }

            //  Проверка на выбранную категорию номера
            if (cbCategory.SelectedItem is not Category selectedCategory)
            {
                MessageBox.Show("Пожалуйста, выберите категорию номера.");
                cbCategory.Focus();
                return;
            }

            //  Проверка на максимальное количество людей (до 3 человек)
            const int MAX1 = 3;
            if (capacity > MAX1 || capacity < 1)
            {
                MessageBox.Show($"Ошибка! Максимальное количество людей в одну комнату не может превышать {MAX1} человек.");
                tbCapacity.SelectAll();
                tbCapacity.Focus();
                return;
            }

            //  Проверка на максимальную стоимость (до 67000)
            const decimal MAX = 67000m;
            if (price > MAX || price < 1)
            {
                MessageBox.Show($"Ошибка! Цена за номер не может превышать {MAX} рублей.");
                tbPrice.SelectAll();
                tbPrice.Focus();
                return;
            }

            // Если все проверки пройдены, сохраняем данные
            _roomToEdit.Name = tbName.Text;
            _roomToEdit.Capacity = capacity;
            _roomToEdit.PricePerNight = price;
            _roomToEdit.CategoryId = selectedCategory.Id;

            DialogResult = true;
            Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (_isDirty)
            {
                var result = MessageBox.Show("Точно хотите выйти без сохранения?", "Подтверждение", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    DialogResult = false; // Закрываем с отменой
                    this.Close();
                }
            }
            else
            {
                DialogResult = false;
                this.Close();
            }
        }
    }
}
