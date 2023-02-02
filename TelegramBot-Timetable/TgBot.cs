using System.Text;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBot_Timetable.Commands;


namespace TelegramBot_Timetable;

public class TgBot
{
    private TelegramBotClient _bot;
    
    private readonly Dictionary<string, ICommand> _commands = new Dictionary<string, ICommand>() {
        {"/start", new StartCommand()},
        {"/reg", new RegCommand()},
        {"/add", new AddCommand()},
        {"/show", new ShowCommand()},
        {"/del" , new DelCommand()}
    };

    public TgBot(string token)
    {
        _bot = new TelegramBotClient(token);
    }

    public void Start()
    {
        Console.WriteLine("Запущен бот " + _bot.GetMeAsync().Result.FirstName);

        var cts = new CancellationTokenSource();
        var cancellationToken = cts.Token;
        var receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = { }, // receive all update types
        };
        
        _bot.StartReceiving(
            HandleUpdateAsync,
            HandleErrorAsync,
            receiverOptions,
            cancellationToken
        );

        SendToAllUsers();
        
        Console.ReadLine();
    }
    
    private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        Console.BackgroundColor = ConsoleColor.Green;
        Console.WriteLine("DEBUG");
        Console.ResetColor();
        Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(update));

        if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
        {
            var command = update.Message.Text.Split(" ")[0].ToLower();

            if (_commands.ContainsKey(command))
            {
                await _commands[command].Run(_bot, update);
            }
            else if (command == "/help")
            {
                var sb = new StringBuilder("Список всех команд:\n");
                foreach (var v in _commands)
                {
                    sb.Append(v.Value.GetHelpMsg() + "\n");
                }
                sb.Append("/help - получить список всех команд");
                await _bot.SendTextMessageAsync(update.Message.Chat, sb.ToString());
            }
            else
            {
                await _bot.SendTextMessageAsync(update.Message.Chat,
                    $"Не удалось найти комманду {command}. " +
                    $"Используйте /help для просмотра всех доспупных команд.");
            }
        }
    }

    private async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        Console.BackgroundColor = ConsoleColor.Red;
        Console.WriteLine("ERROR");
        Console.ResetColor();
        Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
    }

    private async Task SendToAllUsers()
    {
        while (true)
        {
            if (DateTime.Today.ToUniversalTime().Hour == 7)
            {
                var users = DbCommands.GetUsers();
                foreach (var v in users)
                {
                    Console.WriteLine(v.userId);
                    _commands["/show"].Run(_bot, new Update()
                    {
                        Message = new Message()
                        {
                            From = new Telegram.Bot.Types.User()
                            {
                                Id = v.userId
                            },
                            Chat = new Chat()
                            {
                                Id = v.chatId
                            }
                        }
                    });
                }
                await Task.Delay(1000 * 60 * 65);
            }
            else
            {
                await Task.Delay(1000 * 60);
            }
        }
    }
}