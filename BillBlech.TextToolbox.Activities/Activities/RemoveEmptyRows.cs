using BillBlech.TextToolbox.Activities.Activities;
using BillBlech.TextToolbox.Activities.Properties;
using System;
using System.Activities;
using System.Threading;
using System.Threading.Tasks;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.Localization;

namespace BillBlech.TextToolbox.Activities
{
    [LocalizedDisplayName(nameof(Resources.RemoveEmptyRows_DisplayName))]
    [LocalizedDescription(nameof(Resources.RemoveEmptyRows_Description))]
    public class RemoveEmptyRows : ContinuableAsyncCodeActivity
    {
        #region Properties

        /// <summary>
        /// If set, continue executing the remaining activities even if the current activity has failed.
        /// </summary>
        [LocalizedCategory(nameof(Resources.Common_Category))]
        [LocalizedDisplayName(nameof(Resources.ContinueOnError_DisplayName))]
        [LocalizedDescription(nameof(Resources.ContinueOnError_Description))]
        public override InArgument<bool> ContinueOnError { get; set; }

        [LocalizedDisplayName(nameof(Resources.RemoveEmptyRows_InputTextRow_DisplayName))]
        [LocalizedDescription(nameof(Resources.RemoveEmptyRows_InputTextRow_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string> InputTextRow { get; set; }

        [LocalizedDisplayName(nameof(Resources.ReadTextFileEncondig_Encoding_DisplayName))]
        [LocalizedDescription(nameof(Resources.ReadTextFileEncondig_Encoding_Description))]
        [LocalizedCategory(nameof(Resources.Options_Category))]
        public InArgument<string> Encoding { get; set; }

        [LocalizedDisplayName(nameof(Resources.RemoveEmptyRows_AdjustedTextRow_DisplayName))]
        [LocalizedDescription(nameof(Resources.RemoveEmptyRows_AdjustedTextRow_Description))]
        [LocalizedCategory(nameof(Resources.Output_Category))]
        public OutArgument<string> AdjustedTextRow { get; set; }

        [LocalizedDisplayName(nameof(Resources.IDText_DisplayName))]
        [LocalizedDescription(nameof(Resources.IDText_Description))]
        [LocalizedCategory(nameof(Resources.Common_Category))]
        public InArgument<string> IDText { get; set; }

        [LocalizedDisplayName(nameof(Resources.TextApplicationScope_FilePathPreview_DisplayName))]
        [LocalizedDescription(nameof(Resources.TextApplicationScope_FilePathPreview_Description))]
        [LocalizedCategory(nameof(Resources.Common_Category))]
        public InArgument<string> FilePathPreview { get; set; }

        #endregion


        #region Constructors

        public RemoveEmptyRows()
        {
        }

        #endregion


        #region Protected Methods

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            if (InputTextRow == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(InputTextRow)));
            if (Encoding == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(Encoding)));
            if (AdjustedTextRow == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(AdjustedTextRow)));

            base.CacheMetadata(metadata);
        }

        protected override async Task<Action<AsyncCodeActivityContext>> ExecuteAsync(AsyncCodeActivityContext context, CancellationToken cancellationToken)
        {
            // Inputs
            var inputTextRow = InputTextRow.Get(context);

            ///////////////////////////
            // Add execution logic HERE
            string OutputAdjustedTextRow = Utils.TextRemoveEmptyRows(inputTextRow);
            ///////////////////////////

            // Outputs
            return (ctx) =>
            {
                AdjustedTextRow.Set(ctx, OutputAdjustedTextRow);
            };
        }

        #endregion
    }
}

