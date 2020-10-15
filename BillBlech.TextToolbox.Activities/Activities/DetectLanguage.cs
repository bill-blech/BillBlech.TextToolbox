using System;
using System.Activities;
using System.Threading;
using System.Threading.Tasks;
using BillBlech.TextToolbox.Activities.Activities.Encryption;
using BillBlech.TextToolbox.Activities.Properties;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.Localization;

namespace BillBlech.TextToolbox.Activities
{
    [LocalizedDisplayName(nameof(Resources.DetectLanguage_DisplayName))]
    [LocalizedDescription(nameof(Resources.DetectLanguage_Description))]
    public class DetectLanguage : ContinuableAsyncCodeActivity
    {
        #region Properties

        /// <summary>
        /// If set, continue executing the remaining activities even if the current activity has failed.
        /// </summary>
        [LocalizedCategory(nameof(Resources.Common_Category))]
        [LocalizedDisplayName(nameof(Resources.ContinueOnError_DisplayName))]
        [LocalizedDescription(nameof(Resources.ContinueOnError_Description))]
        public override InArgument<bool> ContinueOnError { get; set; }

        [LocalizedDisplayName(nameof(Resources.DetectLanguage_InputText_DisplayName))]
        [LocalizedDescription(nameof(Resources.DetectLanguage_InputText_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string> InputText { get; set; }

        [LocalizedDisplayName(nameof(Resources.DetectLanguage_ConfigFile_DisplayName))]
        [LocalizedDescription(nameof(Resources.DetectLanguage_ConfigFile_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string> ConfigFile { get; set; }

        [LocalizedDisplayName(nameof(Resources.DetectLanguage_Language_DisplayName))]
        [LocalizedDescription(nameof(Resources.DetectLanguage_Language_Description))]
        [LocalizedCategory(nameof(Resources.Output_Category))]
        public OutArgument<string> Language { get; set; }

        #endregion


        #region Constructors

        public DetectLanguage()
        {
        }

        #endregion


        #region Protected Methods

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            if (InputText == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(InputText)));
            if (ConfigFile == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(ConfigFile)));
            if (Language == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(Language)));

            base.CacheMetadata(metadata);
        }

        protected override async Task<Action<AsyncCodeActivityContext>> ExecuteAsync(AsyncCodeActivityContext context, CancellationToken cancellationToken)
        {
            // Inputs
            var inputText = InputText.Get(context);
            var configFile = ConfigFile.Get(context);

            ///////////////////////////
            // Add execution logic HERE
            string OutputLanguage = LanguageDetection.RunDetectLanguage(inputText, configFile);
            ///////////////////////////

            // Outputs
            return (ctx) => {
                Language.Set(ctx, OutputLanguage);
            };
        }

        #endregion
    }
}

