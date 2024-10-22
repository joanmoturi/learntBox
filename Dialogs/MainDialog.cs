﻿using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using LearnBox.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace LearnBox.Dialogs
{
    public class MainDialog : ComponentDialog
    {
        #region Variables
        private readonly BotStateService _botStateService;
        private readonly BotServices _botServices;
        #endregion  


        public MainDialog(BotStateService botStateService, BotServices botServices) : base(nameof(MainDialog))
        {
            _botStateService = botStateService ?? throw new System.ArgumentNullException(nameof(botStateService));
            _botServices = botServices ?? throw new System.ArgumentNullException(nameof(botServices));

            InitializeWaterfallDialog();
        }

        private void InitializeWaterfallDialog()
        {
            // Create Waterfall Steps
            var waterfallSteps = new WaterfallStep[]
            {
                InitialStepAsync,
                FinalStepAsync
            };

            // Add Named Dialogs
            AddDialog(new GreetingDialog($"{nameof(MainDialog)}.greeting", _botStateService));
            AddDialog(new UserStoriesDialog($"{nameof(MainDialog)}UserStories", _botStateService));
           // AddDialog(new BugTypeDialog($"{nameof(MainDialog)}.bugType", _botStateService, _botServices));
            AddDialog(new WaterfallDialog($"{nameof(MainDialog)}.mainFlow", waterfallSteps));

            // Set the starting Dialog
            InitialDialogId = $"{nameof(MainDialog)}.mainFlow";
        }

        private async Task<DialogTurnResult> InitialStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        /* {
             // First, we use the dispatch model to determine which cognitive service (LUIS or QnA) to use.
          //  var recognizerResult = await _botServices.Dispatch.RecognizeAsync(stepContext.Context, cancellationToken);

             // Top intent tell us which cognitive service to use.
           //  var topIntent = recognizerResult.GetTopScoringIntent();

             switch  (
                 
             {
                 case "GreetingIntent":
                     return await stepContext.BeginDialogAsync($"{nameof(MainDialog)}.greeting", null, cancellationToken);
                 case "UserStoriesIntent":
                     return await stepContext.BeginDialogAsync($"{nameof(MainDialog)}.User stories", null, cancellationToken);
                 case "QuesryUserStory":
                     return await stepContext.BeginDialogAsync($"{nameof(MainDialog)}.user story", null, cancellationToken);
                 default:
                     await stepContext.Context.SendActivityAsync(MessageFactory.Text($"I'm sorry I don't know what you mean."), cancellationToken);
                     break;
             }

             return await stepContext.NextAsync(null, cancellationToken);
         }*/

        {
            if (Regex.Match(stepContext.Context.Activity.Text.ToLower(), "hi").Success)
            {
                return await stepContext.BeginDialogAsync($"{nameof(MainDialog)}.greeting", null, cancellationToken);
            }
            else
            {
                return await stepContext.BeginDialogAsync($"{nameof(MainDialog)}.UserStories", null, cancellationToken);
            }
        }

        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            return await stepContext.EndDialogAsync(null, cancellationToken);
        }
    }
}
