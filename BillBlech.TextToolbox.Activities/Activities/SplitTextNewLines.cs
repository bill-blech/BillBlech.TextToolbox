using BillBlech.TextToolbox.Activities.Activities;
using BillBlech.TextToolbox.Activities.Properties;
using System;
using System.Activities;
using System.Threading;
using System.Threading.Tasks;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.Localization;
using UiPath.Shared.Activities.Utilities;

namespace BillBlech.TextToolbox.Activities
{
    [LocalizedDisplayName(nameof(Resources.SplitTextNewLines_DisplayName))]
    [LocalizedDescription(nameof(Resources.SplitTextNewLines_Description))]
    public class SplitTextNewLines : ContinuableAsyncCodeActivity
    {
        #region Properties

        /// <summary>
        /// If set, continue executing the remaining activities even if the current activity has failed.
        /// </summary>
        [LocalizedCategory(nameof(Resources.Common_Category))]
        [LocalizedDisplayName(nameof(Resources.ContinueOnError_DisplayName))]
        [LocalizedDescription(nameof(Resources.ContinueOnError_Description))]
        public override InArgument<bool> ContinueOnError { get; set; }

        [LocalizedDisplayName(nameof(Resources.SplitTextNewLines_TextLines_DisplayName))]
        [LocalizedDescription(nameof(Resources.SplitTextNewLines_TextLines_Description))]
        [LocalizedCategory(nameof(Resources.Output_Category))]
        public OutArgument<string[]> TextLines { get; set; }

        #endregion


        #region Constructors

        public SplitTextNewLines()
        {
            Constraints.Add(ActivityConstraints.HasParentType<SplitTextNewLines, TextApplicationScope>(string.Format(Resources.ValidationScope_Error, Resources.TextApplicationScope_DisplayName)));
        }

        #endregion


        #region Protected Methods

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {

            if (TextLines == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(TextLines)));

            base.CacheMetadata(metadata);
        }

        protected override async Task<Action<AsyncCodeActivityContext>> ExecuteAsync(AsyncCodeActivityContext context, CancellationToken cancellationToken)
        {
            // Object Container: Use objectContainer.Get<T>() to retrieve objects from the scope
            var objectContainer = context.GetFromContext<IObjectContainer>(TextApplicationScope.ParentContainerPropertyTag);

            // Inputs
            string inputText = objectContainer.Get<string>();

            ///////////////////////////
            // Add execution logic HERE
            string[] OutLinesArray = Utils.SplitTextNewLine(inputText);
            ///////////////////////////

            // Outputs
            return (ctx) =>
            {
                TextLines.Set(ctx, OutLinesArray);
            };
        }

        #endregion
    }
}

