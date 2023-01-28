using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBot_Timetable.Commands;

public class ShowCommand : ICommand
{
    public async Task Run(TelegramBotClient bot, Update update)
    {
        var uid = update.Message.From.Id;
        
        if (!DbCommands.CheckUser(uid))
        {
            await bot.SendTextMessageAsync(update.Message.Chat,
                $"Вы не зарегистрированы. Используйте\n/reg для регистрации.");
            return;
        }
        
        uid = DbCommands.GetUser(uid);
        var ls = DbCommands.GetTasks((int)uid);
        var sb = new StringBuilder("");
        
        for (int i = 0; i < ls.Length; ++i)
        {
            sb.Append($"{i + 1}) ");
            sb.Append($"{ls[i].heading} ");
            if (ls[i].description is not null && ls[i].description.Trim() != "")
            {
                sb.Append($"- {ls[i].description}");
            }
            if (i + 1 < ls.Length)
                sb.Append("\n");
        }
        
        await bot.SendTextMessageAsync(update.Message.Chat, sb.ToString());
    }

    public string GetHelpMsg()
    {
        return "/show - показывает все задачи";
    }
}