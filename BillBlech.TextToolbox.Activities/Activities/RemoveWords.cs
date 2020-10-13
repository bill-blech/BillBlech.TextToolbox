using BillBlech.TextToolbox.Activities.Activities;
using BillBlech.TextToolbox.Activities.Properties;
using System;
using System.Activities;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.Localization;
using UiPath.Shared.Activities.Utilities;
using static BillBlech.TextToolbox.Activities.Activities.Utils;

namespace BillBlech.TextToolbox.Activities
{
    [LocalizedDisplayName(nameof(Resources.RemoveWords_DisplayName))]
    [LocalizedDescription(nameof(Resources.RemoveWords_Description))]
    public class RemoveWords : ContinuableAsyncCodeActivity
    {
        #region Properties

        // <summary>
        // If set, continue executing the remaining activities even if the current activity has failed.
        /// </summary>
        [LocalizedCategory(nameof(Resources.Common_Category))]
        [LocalizedDisplayName(nameof(Resources.ContinueOnError_DisplayName))]
        [LocalizedDescription(nameof(Resources.ContinueOnError_Description))]
        public override InArgument<bool> ContinueOnError { get; set; }

        [LocalizedDisplayName(nameof(Resources.RemoveWords_Words_DisplayName))]
        [LocalizedDescription(nameof(Resources.RemoveWords_Words_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        //public InArgument<String[]> Words { get; set; }
        public InArgument<Collection<string>> Words { get; set; }

        [LocalizedDisplayName(nameof(Resources.RemoveWords_Occurrences_DisplayName))]
        [LocalizedDescription(nameof(Resources.RemoveWords_Occurrences_Description))]
        [LocalizedCategory(nameof(Resources.Options_Category))]
        //public EnumTextOccurrence Occurrences { get; set; }
        public InArgument<string> Occurrences { get; set; }

        [LocalizedDisplayName(nameof(Resources.RemoveWords_OccurrenceNumber_DisplayName))]
        [LocalizedDescription(nameof(Resources.RemoveWords_OccurrenceNumber_Description))]
        [LocalizedCategory(nameof(Resources.Options_Category))]
        public InArgument<int> OccurrenceNumber { get; set; }

        [LocalizedDisplayName(nameof(Resources.RemoveWords_AdjustText_DisplayName))]
        [LocalizedDescription(nameof(Resources.RemoveWords_AdjustText_Description))]
        [LocalizedCategory(nameof(Resources.Output_Category))]
        public OutArgument<string> AdjustText { get; set; }

        [LocalizedDisplayName(nameof(Resources.RemoveWords_DisplayLog_DisplayName))]
        [LocalizedDescription(nameof(Resources.RemoveWords_DisplayLog_Description))]
        [LocalizedCategory(nameof(Resources.Options_Category))]
        public bool DisplayLog { get; set; }

        [LocalizedDisplayName(nameof(Resources.IDText_DisplayName))]
        [LocalizedDescription(nameof(Resources.IDText_Description))]
        [LocalizedCategory(nameof(Resources.Common_Category))]
        public InArgument<string> IDText { get; set; }

        #endregion


        #region Constructors

        public RemoveWords()
        {
            Constraints.Add(ActivityConstraints.HasParentType<RemoveWords, TextApplicationScope>(string.Format(Resources.ValidationScope_Error, Resources.TextApplicationScope_DisplayName)));
        }

        #endregion


        #region Protected Methods

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            if (Words == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(Words)));
            if (AdjustText == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(AdjustText)));

            base.CacheMetadata(metadata);
        }

        protected override async Task<Action<AsyncCodeActivityContext>> ExecuteAsync(AsyncCodeActivityContext context, CancellationToken cancellationToken)
        {

            // Object Container: Use objectContainer.Get<T>() to retrieve objects from the scope
            var objectContainer = context.GetFromContext<IObjectContainer>(TextApplicationScope.ParentContainerPropertyTag);

            // Inputs
            string inputText = objectContainer.Get<string>();

            // Inputs
            var wordsCol = Words.Get(context);
            var occurrencesText = Occurrences.Get(context);
            var occurrenceNumber = OccurrenceNumber.Get(context);
            var displayLog = DisplayLog;

            //Convert Collection to Array
            string[] words = Utils.ConvertCollectionToArray(wordsCol);

            ///////////////////////////
            // Add execution logic HERE
            string OuputString = Utils.RemoveWordsFromText(inputText, words, occurrencesText, occurrenceNumber, displayLog);
            ///////////////////////////

            // Outputs
            return (ctx) =>
            {
                AdjustText.Set(ctx, OuputString);
            };

        }
        #endregion
    }
}

