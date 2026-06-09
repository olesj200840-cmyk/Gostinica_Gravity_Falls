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
            // 1. Заполняем ComboBox списком категорий из DataManager
            cbCategory.ItemsSource = DataManager._categories;

            if (roomToEdit != null)
            {
                // --- РЕЖИМ РЕДАКТИРОВАНИЯ ---
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
                // --- РЕЖИМ ДОБАВЛЕНИЯ ---
                _roomToEdit = new Room { Status = 0 }; // По умолчанию свободен

                // Можно установить категорию по умолчанию (например, первую в списке)
                if (DataManager._categories.Any())
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
            // Валидация всех полей
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

            // Проверяем, выбрана ли категория в ComboBox
            if (cbCategory.SelectedItem == null)
            {
                MessageBox.Show("Пожалуйста, выберите категорию номера.");
                return;
            }

            // Сохраняем данные из полей в объект комнаты
            _roomToEdit.Name = tbName.Text;
            _roomToEdit.Capacity = capacity;
            _roomToEdit.PricePerNight = price;

            // Сохраняем ID выбранной категории.
            // SelectedValuePath="Id" позволяет нам напрямую обращаться к Id.
            _roomToEdit.CategoryId = (int)cbCategory.SelectedValue;

            DialogResult = true; // Успешное закрытие окна с результатом "ОК"
            this.Close();
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
