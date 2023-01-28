using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBot_Timetable.Commands;

public interface ICommand
{
    public Task Run(TelegramBotClient bot, Update update);
    public string GetHelpMsg();
}