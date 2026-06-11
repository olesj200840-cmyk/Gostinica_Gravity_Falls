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

        
        // Чтобы не дублировать код обновления ListBox, вынесем его в отдельный метод.
        private void RefreshRoomList()
        {
            // Шаг 1: Получаем полный список комнат из менеджера данных.
            var allRooms = DataManager.GetRooms();

            // Шаг 2: Для удобства создадим справочник категорий.
            // Это нужно, чтобы быстро находить имя категории по её Id,
            // не перебирая весь список категорий заново для каждой комнаты.
            var categoryDict = DataManager._categories.ToDictionary(c => c.Id, c => c.Name);

            // Шаг 3: Создаем новый список объектов специально для отображения в UI (ListBox).
            // Мы берем данные из Room и дополняем их названием категории.
            var roomsForDisplay = allRooms.Select(room => new
            {
                // Берем свойства напрямую из объекта комнаты.
                room.Name,

                // Вычисляем статус текстом. Проверяем значение поля Status.
                StatusDisplay = room.Status switch
                {
                    0 => "Свободен",
                    1 => "Забронирован",
                    2 => "Занят",
                    _ => "Неизвестный статус" // На случай, если появится новое число
                },

                // Ищем имя категории в нашем словаре.
                // Если категория с таким Id не найдена, показываем "Неизвестно".
                CategoryName = categoryDict.TryGetValue(room.CategoryId, out string catName) ? catName : "Неизвестно",
                RoomPricePerNight = $"{room.PricePerNight:C} руб/ночь" // :C - формат валюты
            }).ToList(); // Выполняем запрос

            // Шаг 4: Привязываем наш новый сформированный список к элементу управления на форме.
            listRooms.ItemsSource = roomsForDisplay;
        }
    }
}
