using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBot_Timetable.Commands;

public class RegCommand : ICommand
{
    public async Task Run(TelegramBotClient bot, Update update)
    {
        var userid = update.Message.From.Id;
        if (DbCommands.CheckUser(userid))
        {
            await bot.SendTextMessageAsync(update.Message.Chat,
                $"Вы уже зарегистрировались.");
        }
        else
        {
            DbCommands.AddUser(userid, update.Message.Chat.Id);
            await bot.SendTextMessageAsync(update.Message.Chat,
                $"Регистрация успешно завершена.");
        }
    }

    public string GetHelpMsg()
    {
        return "/reg - зарегистрироваться в боте";
    }
}