using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBot_Timetable.Commands;

public class AddCommand : ICommand
{
    public async Task Run(TelegramBotClient bot, Update update)
    {
        var cmd = update.Message.Text.Split(" ").ToList().Select(x => x.Trim()).ToArray();
        var uid = update.Message.From.Id;
        
        if (!DbCommands.CheckUser(uid))
        {
            await bot.SendTextMessageAsync(update.Message.Chat,
                $"Вы не зарегистрированы. Используйте\n/reg для регистрации.");
            return;
        }
        
        if (cmd.Length < 2)
        {
            await bot.SendTextMessageAsync(update.Message.Chat,
                $"Задача должна иметь название.");
            return;
        }
        
        uid = DbCommands.GetUser(uid);
        var curTask = new TimeTask(-1, (int)uid, 1, cmd[1], (cmd.Length < 3) ? null : cmd[2]);
        DbCommands.AddTask(curTask);
        
        Console.WriteLine($"Task added {curTask}");
    }

    public string GetHelpMsg()
    {
        return "/add название описание - добавляет в расписание новую задачу";
    }
}