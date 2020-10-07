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
    [LocalizedDisplayName(nameof(Resources.ReadTextFileEncondig_DisplayName))]
    [LocalizedDescription(nameof(Resources.ReadTextFileEncondig_Description))]
    public class ReadTextFileEncondig : ContinuableAsyncCodeActivity
    {
        #region Properties

        /// <summary>
        /// If set, continue executing the remaining activities even if the current activity has failed.
        /// </summary>
        [LocalizedCategory(nameof(Resources.Common_Category))]
        [LocalizedDisplayName(nameof(Resources.ContinueOnError_DisplayName))]
        [LocalizedDescription(nameof(Resources.ContinueOnError_Description))]
        public override InArgument<bool> ContinueOnError { get; set; }

        [LocalizedDisplayName(nameof(Resources.ReadTextFileEncondig_FileName_DisplayName))]
        [LocalizedDescription(nameof(Resources.ReadTextFileEncondig_FileName_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string> FileName { get; set; }

        [LocalizedDisplayName(nameof(Resources.ReadTextFileEncondig_Encoding_DisplayName))]
        [LocalizedDescription(nameof(Resources.ReadTextFileEncondig_Encoding_Description))]
        [LocalizedCategory(nameof(Resources.Options_Category))]
        public InArgument<string> Encoding { get; set; }

        [LocalizedDisplayName(nameof(Resources.ReadTextFileEncondig_Content_DisplayName))]
        [LocalizedDescription(nameof(Resources.ReadTextFileEncondig_Content_Description))]
        [LocalizedCategory(nameof(Resources.Output_Category))]
        public OutArgument<string> Content { get; set; }

        [LocalizedDisplayName(nameof(Resources.ReadTextFileEncondig_DisplayLog_DisplayName))]
        [LocalizedDescription(nameof(Resources.ReadTextFileEncondig_DisplayLog_Description))]
        [LocalizedCategory(nameof(Resources.Options_Category))]
        public bool DisplayLog { get; set; }

        #endregion


        #region Constructors

        public ReadTextFileEncondig()
        {
        }

        #endregion


        #region Protected Methods

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            if (FileName == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(FileName)));
            if (Encoding == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(Encoding)));
            if (Content == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(Content)));

            base.CacheMetadata(metadata);
        }

        protected override async Task<Action<AsyncCodeActivityContext>> ExecuteAsync(AsyncCodeActivityContext context, CancellationToken cancellationToken)
        {
            // Inputs
            var fileName = FileName.Get(context);
            var Strencoding = Encoding.Get(context);
            var displayLog = DisplayLog;

            ///////////////////////////
            // Add execution logic HERE
            String OutputString = Utils.ReadTextFileEncoding(fileName, Strencoding);
            ///////////////////////////

            // Outputs
            return (ctx) => {
                Content.Set(ctx, OutputString);
            };
        }

        #endregion
    }
}

