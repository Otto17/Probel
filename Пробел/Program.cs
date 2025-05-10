// Copyright (c) 2025 Otto
// Лицензия: MIT (см. LICENSE)

namespace Пробел
{
    internal static class Program
    {
        // Уникальный GUID для определения копии запущенной программы
        private static readonly Mutex AppMutex = new(true, "{8167CE29-B10C-4BFD-97E3-9876C9743F38}");

        // Основная точка входа в приложение
        [STAThread]
        static void Main()
        {
            if (!AppMutex.WaitOne(TimeSpan.Zero, true))
            {
                // Блокировка запуска программы и выводим сообщение
                MessageBox.Show("Программа уже запущена в трее",
                              "Пробел",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Information);
                return;
            }

            try
            {
                // Настройка конфигурацию приложения
                ApplicationConfiguration.Initialize();
                Application.Run(new FormSpace());
            }
            finally
            {
                AppMutex.ReleaseMutex();    // Разблокируем мьютекс
                AppMutex.Close();           // Освобождаем системные ресурсы
            }
        }
    }
}
