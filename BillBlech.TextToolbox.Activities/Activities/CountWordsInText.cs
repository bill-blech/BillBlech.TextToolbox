using System;
using System.Activities;
using System.Threading;
using System.Threading.Tasks;
using BillBlech.TextToolbox.Activities.Activities;
using BillBlech.TextToolbox.Activities.Properties;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.Localization;
using UiPath.Shared.Activities.Utilities;

namespace BillBlech.TextToolbox.Activities
{
    [LocalizedDisplayName(nameof(Resources.CountWordsInText_DisplayName))]
    [LocalizedDescription(nameof(Resources.CountWordsInText_Description))]
    public class CountWordsInText : ContinuableAsyncCodeActivity
    {
        #region Properties

        /// <summary>
        /// If set, continue executing the remaining activities even if the current activity has failed.
        /// </summary>
        [LocalizedCategory(nameof(Resources.Common_Category))]
        [LocalizedDisplayName(nameof(Resources.ContinueOnError_DisplayName))]
        [LocalizedDescription(nameof(Resources.ContinueOnError_Description))]
        public override InArgument<bool> ContinueOnError { get; set; }

        [LocalizedDisplayName(nameof(Resources.CountWordsInText_SearchWords_DisplayName))]
        [LocalizedDescription(nameof(Resources.CountWordsInText_SearchWords_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<String[]> SearchWords { get; set; }

        [LocalizedDisplayName(nameof(Resources.CountWordsInText_Percentage_DisplayName))]
        [LocalizedDescription(nameof(Resources.CountWordsInText_Percentage_Description))]
        [LocalizedCategory(nameof(Resources.Output_Category))]
        public OutArgument<double> Percentage { get; set; }

        [LocalizedDisplayName(nameof(Resources.ExtractTextBetweenTwoAnchorWords_DisplayLog_DisplayName))]
        [LocalizedDescription(nameof(Resources.ExtractTextBetweenTwoAnchorWords_DisplayLog_Description))]
        [LocalizedCategory(nameof(Resources.Options_Category))]
        public bool DisplayLog { get; set; }

        #endregion


        #region Constructors

        public CountWordsInText()
        {
            Constraints.Add(ActivityConstraints.HasParentType<CountWordsInText, TextApplicationScope>(string.Format(Resources.ValidationScope_Error, Resources.TextApplicationScope_DisplayName)));
        }

        #endregion


        #region Protected Methods

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            if (SearchWords == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(SearchWords)));
            if (Percentage == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(Percentage)));

            base.CacheMetadata(metadata);
        }

        protected override async Task<Action<AsyncCodeActivityContext>> ExecuteAsync(AsyncCodeActivityContext context, CancellationToken cancellationToken)
        {

            // Object Container: Use objectContainer.Get<T>() to retrieve objects from the scope
            var objectContainer = context.GetFromContext<IObjectContainer>(TextApplicationScope.ParentContainerPropertyTag);

            // Inputs
            string inputText = objectContainer.Get<string>();

            // Inputs
            var searchWords = SearchWords.Get(context);
            var displayLog = DisplayLog;

            ///////////////////////////
            // Add execution logic HERE

            //Find Words in String
            double PercResults = Utils.FindWordsInString(inputText, searchWords, displayLog);
            ///////////////////////////

            // Outputs
            return (ctx) => {
                Percentage.Set(ctx, PercResults);
            };
        }

        #endregion
    }
}

