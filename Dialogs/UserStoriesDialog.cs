using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using LearnBox.Models;
using LearnBox.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace LearnBox.Dialogs
{
  


     public class UserStoriesDialog : ComponentDialog
     {

         #region Variables
         private readonly BotStateService _botStateService;
         #endregion  


         public UserStoriesDialog(string dialogId, BotStateService botStateService) : base(dialogId)
         {
             _botStateService = botStateService ?? throw new System.ArgumentNullException(nameof(botStateService));

             InitializeWaterfallDialog();
         }

         private void InitializeWaterfallDialog()
         {
             // Create Waterfall Steps
             var waterfallSteps = new WaterfallStep[]
           
                {
                 UserStoriesStepAsync,
                 ContinueStepAsync,
                 PreviousStoryStepAsync
                // MenuStepAsync

                };

            // Add Named Dialogs
            AddDialog(new WaterfallDialog($"{nameof(UserStoriesDialog)}.mainFlow", waterfallSteps));
            AddDialog(new ChoicePrompt($"{nameof(UserStoriesDialog)}. success story option"));
            AddDialog(new ChoicePrompt($"{nameof(UserStoriesDialog)}.continue"));
            AddDialog(new ChoicePrompt($"{nameof(UserStoriesDialog)}.move to previous step"));
            //AddDialog(new ChoicePrompt($"{nameof(UserStoriesDialog)}.Back to main menu"));

            // Set the starting Dialog
            InitialDialogId = $"{nameof(UserStoriesDialog)}.mainFlow";
        }

        #region Waterfall Steps
        private async Task<DialogTurnResult> UserStoriesStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
         {
             return await stepContext.PromptAsync($"{nameof(UserStoriesDialog)}.success story option",
                 new PromptOptions
                 {
                     Prompt = MessageFactory.Text("would you like to go through some of our successs stories"),
                     Choices = ChoiceFactory.ToChoices(new List<string> { "Yes", "Main Menu", "Go back" }),
                 }, cancellationToken);
         }

         private async Task<DialogTurnResult> ContinueStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
         {
             stepContext.Values["description"] = (string)stepContext.Result;

             return await stepContext.PromptAsync($"{nameof(UserStoriesDialog)}.continue",
                 new PromptOptions
                 {
                     Prompt = MessageFactory.Text("Would you like to the next success story?"),
                     Choices = ChoiceFactory.ToChoices(new List<string> { "Yes", "Main Menu", "Go back" }),
                    
                 }, cancellationToken);
         }

         private async Task<DialogTurnResult> PreviousStoryStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
         {
            // stepContext.Values["callbackTime"] = Convert.ToDateTime(((List<DateTimeResolution>)stepContext.Result).FirstOrDefault().Value);

             return await stepContext.PromptAsync($"{nameof(UserStoriesDialog)}.move to previous step",
                 new PromptOptions
                 {
                     Prompt = MessageFactory.Text("Would you like to go back?"),
                     
                     Choices = ChoiceFactory.ToChoices(new List<string> { "Yes", "Main Menu", "Go back" }),
                 }, cancellationToken);


         }

      




       
         #endregion
/*
         #region Validators
         private Task<bool> CallbackTimeValidatorAsync(PromptValidatorContext<IList<DateTimeResolution>> promptContext, CancellationToken cancellationToken)
         {
             var valid = false;

             if (promptContext.Recognized.Succeeded)
             {
                 var resolution = promptContext.Recognized.Value.First();
                 DateTime selectedDate = Convert.ToDateTime(resolution.Value);
                 TimeSpan start = new TimeSpan(9, 0, 0); //9 o'clock
                 TimeSpan end = new TimeSpan(17, 0, 0); //5 o'clock
                 if ((selectedDate.TimeOfDay >= start) && (selectedDate.TimeOfDay <= end))
                 {
                     valid = true;
                 }
             }
             return Task.FromResult(valid);
         }

         private Task<bool> PhoneNumberValidatorAsync(PromptValidatorContext<string> promptContext, CancellationToken cancellationToken)
         {
             var valid = false;

             if (promptContext.Recognized.Succeeded)
             {
                 valid = Regex.Match(promptContext.Recognized.Value, @"^(\+\d{1,2}\s)?\(?\d{3}\)?[\s.-]?\d{3}[\s.-]?\d{4}$").Success;
             }
             return Task.FromResult(valid);
         }

         #endregion
    */
     } 


}

