using BillBlech.TextToolbox.Activities.Activities;
using BillBlech.TextToolbox.Activities.Properties;
using System;
using System.Activities;
using System.Threading;
using System.Threading.Tasks;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.Localization;
using UiPath.Shared.Activities.Utilities;
using static BillBlech.TextToolbox.Activities.Activities.Utils;

namespace BillBlech.TextToolbox.Activities
{
    [LocalizedDisplayName(nameof(Resources.ReplaceWords_DisplayName))]
    [LocalizedDescription(nameof(Resources.ReplaceWords_Description))]
    public class ReplaceWords : ContinuableAsyncCodeActivity
    {
        #region Properties

        /// <summary>
        /// If set, continue executing the remaining activities even if the current activity has failed.
        /// </summary>
        [LocalizedCategory(nameof(Resources.Common_Category))]
        [LocalizedDisplayName(nameof(Resources.ContinueOnError_DisplayName))]
        [LocalizedDescription(nameof(Resources.ContinueOnError_Description))]
        public override InArgument<bool> ContinueOnError { get; set; }

        [LocalizedDisplayName(nameof(Resources.ReplaceWords_SearchWord_DisplayName))]
        [LocalizedDescription(nameof(Resources.ReplaceWords_SearchWord_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string[]> SearchWord { get; set; }

        [LocalizedDisplayName(nameof(Resources.ReplaceWords_ReplacedWord_DisplayName))]
        [LocalizedDescription(nameof(Resources.ReplaceWords_ReplacedWord_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string> ReplacedWord { get; set; }

        [LocalizedDisplayName(nameof(Resources.ReplaceWords_TextOccurrance_DisplayName))]
        [LocalizedDescription(nameof(Resources.ReplaceWords_TextOccurrance_Description))]
        [LocalizedCategory(nameof(Resources.Options_Category))]
        //public EnumTextOccurrence TextOccurrance { get; set; }
        public InArgument<string> TextOccurrance { get; set; }

        [LocalizedDisplayName(nameof(Resources.ReplaceWords_IndexOccurence_DisplayName))]
        [LocalizedDescription(nameof(Resources.ReplaceWords_IndexOccurence_Description))]
        [LocalizedCategory(nameof(Resources.Options_Category))]
        public InArgument<int> IndexOccurence { get; set; }

        [LocalizedDisplayName(nameof(Resources.ReplaceWords_DisplayLog_DisplayName))]
        [LocalizedDescription(nameof(Resources.ReplaceWords_DisplayLog_Description))]
        [LocalizedCategory(nameof(Resources.Options_Category))]
        public bool DisplayLog { get; set; }

        [LocalizedDisplayName(nameof(Resources.ReplaceWords_AdjustedText_DisplayName))]
        [LocalizedDescription(nameof(Resources.ReplaceWords_AdjustedText_Description))]
        [LocalizedCategory(nameof(Resources.Output_Category))]
        public OutArgument<string> AdjustedText { get; set; }

        [LocalizedDisplayName(nameof(Resources.IDText_DisplayName))]
        [LocalizedDescription(nameof(Resources.IDText_Description))]
        [LocalizedCategory(nameof(Resources.Common_Category))]
        public InArgument<string> IDText { get; set; }

        #endregion


        #region Constructors

        public ReplaceWords()
        {
            Constraints.Add(ActivityConstraints.HasParentType<ReplaceWords, TextApplicationScope>(string.Format(Resources.ValidationScope_Error, Resources.TextApplicationScope_DisplayName)));
        }

        #endregion


        #region Protected Methods

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            if (SearchWord == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(SearchWord)));
            if (ReplacedWord == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(ReplacedWord)));
            if (AdjustedText == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(AdjustedText)));


            base.CacheMetadata(metadata);
        }

        protected override async Task<Action<AsyncCodeActivityContext>> ExecuteAsync(AsyncCodeActivityContext context, CancellationToken cancellationToken)
        {


            // Object Container: Use objectContainer.Get<T>() to retrieve objects from the scope
            var objectContainer = context.GetFromContext<IObjectContainer>(TextApplicationScope.ParentContainerPropertyTag);

            // Inputs
            string inputText = objectContainer.Get<string>();

            // Inputs
            var searchWords = SearchWord.Get(context);
            var replacedWord = ReplacedWord.Get(context);
            var textOccurrance = TextOccurrance.Get(context);
            var indexOccurence = IndexOccurence.Get(context);
            var displayLog = DisplayLog;

            ///////////////////////////
            // Add execution logic HERE
            //Replace word from text
            string OutputString = Utils.ReplaceWordsFromText(inputText, searchWords, replacedWord, textOccurrance, indexOccurence, displayLog);
            ///////////////////////////

            // Outputs
            return (ctx) =>
            {
                AdjustedText.Set(ctx, OutputString);
            };
        }

        #endregion
    }
}

