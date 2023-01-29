using System.Text;
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
        var name = cmd[1];
        
        var sb = new StringBuilder("");
        for (int i = 2; i < cmd.Length; ++i)
            sb.Append(cmd[i]);
            sb.Append(" ");
        
        var desc = sb.ToString();
        if (desc == "")
            desc = null;

        if (name.Length > 250 || desc.Length > 250)
        {
            await bot.SendTextMessageAsync(update.Message.Chat,
                $"Название и описание задачи не должны быть длинее 250 символов.");
            return;
        }
        
        var curTask = new TimeTask(-1, (int)uid, 1, name, desc);
        DbCommands.AddTask(curTask);
        Console.WriteLine($"Task added {curTask}");
        
        await bot.SendTextMessageAsync(update.Message.Chat,
            $"Задача успешно добавлена в список.");
    }

    public string GetHelpMsg()
    {
        return "/add название описание - добавляет в расписание новую задачу";
    }
}