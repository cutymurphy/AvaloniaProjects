using Avalonia;
using System;

namespace Task3_2
{
    class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Запуск приложения...");
            try
            {
                Console.WriteLine("Инициализация Avalonia...");
                var appBuilder = BuildAvaloniaApp();
                Console.WriteLine("Запуск Avalonia с классическим десктопным жизненным циклом...");
                appBuilder.StartWithClassicDesktopLifetime(args);
                Console.WriteLine("Приложение успешно запущено.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при запуске приложения: {ex}");
                throw;
            }
        }

        public static AppBuilder BuildAvaloniaApp()
        {
            Console.WriteLine("Настройка AppBuilder...");
            return AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .WithInterFont()
                .LogToTrace();
        }
    }
}