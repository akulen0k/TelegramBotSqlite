using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBot_Timetable.Commands;

public class DelCommand : ICommand
{
    public async Task Run(TelegramBotClient bot, Update update)
    {
        var cmd = update.Message.Text.Split(" ").ToList().Select(x => x.Trim()).ToArray();
        long uid = update.Message.From.Id;
        
        if (!DbCommands.CheckUser(uid))
        {
            await bot.SendTextMessageAsync(update.Message.Chat,
                $"Вы не зарегистрированы. Используйте\n/reg для регистрации.");
            return;
        }
        
        if (cmd.Length < 2)
        {
            await bot.SendTextMessageAsync(update.Message.Chat,
                $"Не удалось найти номер удаляемой задачи.");
            return;
        }
        
        uid = DbCommands.GetUser(uid);
        
        int taskId;
        if (!int.TryParse(cmd[1], out taskId))
        {
            await bot.SendTextMessageAsync(update.Message.Chat,
                $"Удаление производится только по номеру задачи.");
            return;
        }
        
        var allTasks = DbCommands.GetTasks((int)uid);
        if (taskId > allTasks.Length || taskId < 1)
        {
            await bot.SendTextMessageAsync(update.Message.Chat,
                $"Номер задачи должен быть от 1 до {allTasks.Length}.");
            return;
        }
        
        DbCommands.ModifyTask(allTasks[taskId - 1], 2);
        await bot.SendTextMessageAsync(update.Message.Chat,
            $"Задача {taskId} успешно удалена.");
    }

    public string GetHelpMsg()
    {
        return $"/del - удалить задачу";
    }
}