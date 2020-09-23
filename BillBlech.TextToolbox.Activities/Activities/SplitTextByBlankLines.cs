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
    [LocalizedDisplayName(nameof(Resources.SplitTextByBlankLines_DisplayName))]
    [LocalizedDescription(nameof(Resources.SplitTextByBlankLines_Description))]
    public class SplitTextByBlankLines : ContinuableAsyncCodeActivity
    {
        #region Properties

        /// <summary>
        /// If set, continue executing the remaining activities even if the current activity has failed.
        /// </summary>
        [LocalizedCategory(nameof(Resources.Common_Category))]
        [LocalizedDisplayName(nameof(Resources.ContinueOnError_DisplayName))]
        [LocalizedDescription(nameof(Resources.ContinueOnError_Description))]
        public override InArgument<bool> ContinueOnError { get; set; }

        [LocalizedDisplayName(nameof(Resources.SplitTextByBlankLines_PiecesOfText_DisplayName))]
        [LocalizedDescription(nameof(Resources.SplitTextByBlankLines_PiecesOfText_Description))]
        [LocalizedCategory(nameof(Resources.Output_Category))]
        public OutArgument<String[]> PiecesOfText { get; set; }

        #endregion


        #region Constructors

        public SplitTextByBlankLines()
        {
            Constraints.Add(ActivityConstraints.HasParentType<SplitTextByBlankLines, TextApplicationScope>(string.Format(Resources.ValidationScope_Error, Resources.TextApplicationScope_DisplayName)));
        }

        #endregion


        #region Protected Methods

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {

            if (PiecesOfText == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(PiecesOfText)));

            base.CacheMetadata(metadata);
        }

        protected override async Task<Action<AsyncCodeActivityContext>> ExecuteAsync(AsyncCodeActivityContext context, CancellationToken cancellationToken)
        {
            // Object Container: Use objectContainer.Get<T>() to retrieve objects from the scope
            var objectContainer = context.GetFromContext<IObjectContainer>(TextApplicationScope.ParentContainerPropertyTag);

            // Inputs
            string inputText = objectContainer.Get<string>();

            // Inputs

            ///////////////////////////
            // Add execution logic HERE
            string[] OutputArray = Utils.SplitTextByBlankLines(inputText);
            ///////////////////////////

            // Outputs
            return (ctx) =>
            {
                PiecesOfText.Set(ctx, OutputArray);
            };
        }

        #endregion
    }
}

