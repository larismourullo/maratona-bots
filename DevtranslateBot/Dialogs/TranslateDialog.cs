using System;
using System.Threading.Tasks;
using DevtranslateBot.Services;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;

namespace DevtranslateBot.Dialogs
{
  [Serializable]
  [LuisModel("LuisModelId", "LuisSubscriptionKey")]
  public class TranslateDialog : LuisDialog<object>
  {
    [LuisIntent("None")]
    public async Task None(IDialogContext context, LuisResult result)
    {
      await context.PostAsync($"Me desculpe, não consegui entender a seguinte frase: {result.Query}");
    }

    [LuisIntent("Bot")]
    public async Task Bot(IDialogContext context, LuisResult result)
    {
      await context.PostAsync("Eu sou um bot ou robô, como você preferir. Irei ajuda-lo com suas traduções.");
    }

    [LuisIntent("Greeting")]
    public async Task Greeting(IDialogContext context, LuisResult result)
    {
      var dataTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")).TimeOfDay;
      string greeting;

      if (dataTime < TimeSpan.FromHours(12))
      {
        greeting = "Bom Dia";
      }
      else if (dataTime < TimeSpan.FromHours(18))
      {
        greeting = "Boa Tarde";
      }
      else
      {
        greeting = "Boa Noite";
      }

      await context.PostAsync($"{greeting}! O que deseja?");
    }

    [LuisIntent("Translate")]
    public async Task Translate(IDialogContext context, LuisResult result)
    {
      await context.PostAsync("Basta informar o texto que irei traduzi-lo para você.");
      context.Wait(EnglishTranslate);
    }

    private async Task EnglishTranslate(IDialogContext context, IAwaitable<IMessageActivity> value)
    {
      var message = await value;
      await context.PostAsync(await new LanguageService().EnglishTranslate(message.Text));
    }
  }
}