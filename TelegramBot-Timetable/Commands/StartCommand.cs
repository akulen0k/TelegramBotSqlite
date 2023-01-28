using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBot_Timetable.Commands;

public class StartCommand : ICommand
{
    public async Task Run(TelegramBotClient bot, Update update)
    {
        await bot.SendTextMessageAsync(update.Message.Chat,
            $"Привет, {update.Message.From.Username}.\n" +
            $"Это бот личного расписания, используй\n" +
            $"/help для просмотра всех доспупных команд.");
    }

    public string GetHelpMsg()
    {
        return @"/start - начать работу с TimetableBot";
    }
}