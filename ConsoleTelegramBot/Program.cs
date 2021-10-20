using Microsoft.Extensions.Configuration;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace ConsoleTelegramBot
{
    class Program
    {
        static IConfigurationRoot Configuration;
        static ITelegramBotClient botClient;

        static async Task Main(string[] args)
        {
            Configuration = new ConfigurationBuilder().SetBasePath(ConfigurationPath.Combine(AppContext.BaseDirectory))
                .AddJsonFile("appsettings.json", optional: false).Build();
            string token = Configuration.GetSection("BotConfiguration")?.GetSection("BotToken")?.Value ?? throw new ArgumentNullException("Failed to get a token from a configuration.");

            botClient = new TelegramBotClient(token);
            var info = await botClient.GetMeAsync();
            Console.WriteLine($"Telegram bot {info.FirstName} is started.");

            botClient.OnMessage += Bot_OnMessage;
            botClient.StartReceiving();

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();

            botClient.StopReceiving();
        }

        static async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            var input = e.Message.Text;
            if (input != null)
            {
                var regexYes = new Regex(@"(^((да)|(Да)|(ДА))(\?*|\.*|!*)$)|(((\s|,)да\?+)$)");
                if (regexYes.IsMatch(input))
                {
                    await botClient.SendTextMessageAsync(
                        chatId: e.Message.Chat.Id,
                        replyToMessageId: e.Message.MessageId,
                        text: "пизда");
                }

                var regexNo = new Regex(@"(^((нет)|(Нет)|(НЕТ))(\.*|!*)$)");
                if (regexNo.IsMatch(input))
                {
                    await botClient.SendTextMessageAsync(
                        chatId: e.Message.Chat.Id,
                        replyToMessageId: e.Message.MessageId,
                        text: "пидора ответ");
                }
            }
        }
    }
}
