using System;
using System.Activities;
using System.Threading;
using System.Threading.Tasks;
using BillBlech.TextToolbox.Activities.Activities;
using BillBlech.TextToolbox.Activities.Properties;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.Localization;

namespace BillBlech.TextToolbox.Activities
{
    [LocalizedDisplayName(nameof(Resources.DetectSentiment_DisplayName))]
    [LocalizedDescription(nameof(Resources.DetectSentiment_Description))]
    public class DetectSentiment : ContinuableAsyncCodeActivity
    {
        #region Properties

        /// <summary>
        /// If set, continue executing the remaining activities even if the current activity has failed.
        /// </summary>
        [LocalizedCategory(nameof(Resources.Common_Category))]
        [LocalizedDisplayName(nameof(Resources.ContinueOnError_DisplayName))]
        [LocalizedDescription(nameof(Resources.ContinueOnError_Description))]
        public override InArgument<bool> ContinueOnError { get; set; }

        [LocalizedDisplayName(nameof(Resources.DetectSentiment_InputText_DisplayName))]
        [LocalizedDescription(nameof(Resources.DetectSentiment_InputText_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string> InputText { get; set; }

        [LocalizedDisplayName(nameof(Resources.DetectSentiment_SentimentAnalysis_DisplayName))]
        [LocalizedDescription(nameof(Resources.DetectSentiment_SentimentAnalysis_Description))]
        [LocalizedCategory(nameof(Resources.Output_Category))]
        public OutArgument<string> SentimentAnalysis { get; set; }

        #endregion


        #region Constructors

        public DetectSentiment()
        {
        }

        #endregion


        #region Protected Methods

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            if (InputText == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(InputText)));
            if (SentimentAnalysis == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(SentimentAnalysis)));

            base.CacheMetadata(metadata);
        }

        protected override async Task<Action<AsyncCodeActivityContext>> ExecuteAsync(AsyncCodeActivityContext context, CancellationToken cancellationToken)
        {
            // Inputs
            var inputText = InputText.Get(context);
            var sentimentAnalysis = SentimentAnalysis.Get(context);

            ///////////////////////////
            // Add execution logic HERE
            string SentimentResult = textApiSentiment.ReturnTextSentiment(inputText);
            ///////////////////////////

            // Outputs
            return (ctx) =>
            {
                SentimentAnalysis.Set(ctx, SentimentResult);
            };
        }

        #endregion
    }
}

