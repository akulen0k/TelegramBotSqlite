using System;


namespace TelegramBot_Timetable
{
    class Program
    {
        public static void Main(string[] args)
        {
            var tgBot = new TgBot(GetToken());
            tgBot.Start();
            // 1) /start - приверствие с пользователем по имени
            // 2) /reg регистрация в базе данных
            // 3) /add Название Описание
            // 5) /done id
            // 7) /show
        }

        private static string GetToken()
        {
            if (!File.Exists("../../../token.txt"))
                throw new FileNotFoundException("The file token.txt doesn't exist in the current directory");
                    
            using (var sr = new StreamReader("../../../token.txt"))
            {
                var token = sr.ReadLine() ?? throw new Exception("The token was not found");
                return token;
            }
        }
    }
}