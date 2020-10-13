using System;
using System.Activities;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using BillBlech.TextToolbox.Activities.Activities;
using BillBlech.TextToolbox.Activities.Properties;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.Localization;

namespace BillBlech.TextToolbox.Activities
{
    [LocalizedDisplayName(nameof(Resources.MatchItemInArray_DisplayName))]
    [LocalizedDescription(nameof(Resources.MatchItemInArray_Description))]
    public class MatchItemInArray : ContinuableAsyncCodeActivity
    {
        #region Properties

        /// <summary>
        /// If set, continue executing the remaining activities even if the current activity has failed.
        /// </summary>
        [LocalizedCategory(nameof(Resources.Common_Category))]
        [LocalizedDisplayName(nameof(Resources.ContinueOnError_DisplayName))]
        [LocalizedDescription(nameof(Resources.ContinueOnError_Description))]
        public override InArgument<bool> ContinueOnError { get; set; }

        [LocalizedDisplayName(nameof(Resources.MatchItemInArray_InputArray_DisplayName))]
        [LocalizedDescription(nameof(Resources.MatchItemInArray_InputArray_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<String[]> InputArray { get; set; }

        [LocalizedDisplayName(nameof(Resources.MatchItemInArray_SearchWord_DisplayName))]
        [LocalizedDescription(nameof(Resources.MatchItemInArray_SearchWord_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        //public InArgument<string[]> SearchWord { get; set; }
        public InArgument<Collection<string>> SearchWord { get; set; }

        [LocalizedDisplayName(nameof(Resources.FindArrayItems_DisplayLog_DisplayName))]
        [LocalizedDescription(nameof(Resources.FindArrayItems_DisplayLog_Description))]
        [LocalizedCategory(nameof(Resources.Options_Category))]
        public bool DisplayLog { get; set; }

        [LocalizedDisplayName(nameof(Resources.MatchItemInArray_IsFound_DisplayName))]
        [LocalizedDescription(nameof(Resources.MatchItemInArray_IsFound_Description))]
        [LocalizedCategory(nameof(Resources.Output_Category))]
        public OutArgument<bool> IsFound { get; set; }

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

        public MatchItemInArray()
        {
        }

        #endregion


        #region Protected Methods

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            if (InputArray == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(InputArray)));
            if (SearchWord == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(SearchWord)));
            if (IsFound == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(IsFound)));

            base.CacheMetadata(metadata);
        }

        protected override async Task<Action<AsyncCodeActivityContext>> ExecuteAsync(AsyncCodeActivityContext context, CancellationToken cancellationToken)
        {
            // Inputs
            var inputArray = InputArray.Get(context);
            var searchWordCol = SearchWord.Get(context);
            var displayLog = DisplayLog;

            //Convert Collection to Array
            string[] searchWord = Utils.ConvertCollectionToArray(searchWordCol);
            ///////////////////////////
            // Add execution logic HERE
            bool bIsFound = Utils.MatchItemInArrayOfStrings(inputArray, searchWord,displayLog);
            ///////////////////////////

            // Outputs
            return (ctx) => {
                IsFound.Set(ctx, bIsFound);
            };
        }

        #endregion
    }
}

