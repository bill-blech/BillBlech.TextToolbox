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
    [LocalizedDisplayName(nameof(Resources.SplitTextRows_DisplayName))]
    [LocalizedDescription(nameof(Resources.SplitTextRows_Description))]
    public class SplitTextRows : ContinuableAsyncCodeActivity
    {
        #region Properties

        /// <summary>
        /// If set, continue executing the remaining activities even if the current activity has failed.
        /// </summary>
        [LocalizedCategory(nameof(Resources.Common_Category))]
        [LocalizedDisplayName(nameof(Resources.ContinueOnError_DisplayName))]
        [LocalizedDescription(nameof(Resources.ContinueOnError_Description))]
        public override InArgument<bool> ContinueOnError { get; set; }

        [LocalizedDisplayName(nameof(Resources.SplitTextRows_InputTextRow_DisplayName))]
        [LocalizedDescription(nameof(Resources.SplitTextRows_InputTextRow_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string> InputTextRow { get; set; }

        [LocalizedDisplayName(nameof(Resources.SplitTextRows_NullLimit_DisplayName))]
        [LocalizedDescription(nameof(Resources.SplitTextRows_NullLimit_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<int> NullLimit { get; set; }

        [LocalizedDisplayName("Suppress Null Values")]
        [LocalizedDescription("Remove Empty words from extracted text")]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public bool bSuppressNulls { get; set; }

        [LocalizedDisplayName(nameof(Resources.SplitTextRows_ExtractedText_DisplayName))]
        [LocalizedDescription(nameof(Resources.SplitTextRows_ExtractedText_Description))]
        [LocalizedCategory(nameof(Resources.Output_Category))]
        public OutArgument<String[]> ExtractedText { get; set; }

        #endregion


        #region Constructors

        public SplitTextRows()
        {
        }

        #endregion


        #region Protected Methods

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            if (InputTextRow == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(InputTextRow)));
            if (NullLimit == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(NullLimit)));
            if (ExtractedText == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, "Extracted Words"));

            base.CacheMetadata(metadata);
        }

        protected override async Task<Action<AsyncCodeActivityContext>> ExecuteAsync(AsyncCodeActivityContext context, CancellationToken cancellationToken)
        {
            // Inputs
            var inputTextRow = InputTextRow.Get(context);
            var nullLimit = NullLimit.Get(context);
            var bsuppressNulls = bSuppressNulls;

            ///////////////////////////
            // Add execution logic HERE

            //Fill in Array of Words
            string[] ArrayWordsExtractedText = Utils.SplitTextBigSpaces(inputTextRow, nullLimit, bsuppressNulls);
            ///////////////////////////

            // Outputs
            return (ctx) => {
                ExtractedText.Set(ctx, ArrayWordsExtractedText);
            };
        }

        #endregion
    }
}

