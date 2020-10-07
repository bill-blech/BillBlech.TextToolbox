using System;
using System.Activities;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using BillBlech.TextToolbox.Activities.Activities;
using BillBlech.TextToolbox.Activities.Properties;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.Localization;

namespace BillBlech.TextToolbox.Activities
{
    [LocalizedDisplayName(nameof(Resources.FindArrayItems_DisplayName))]
    [LocalizedDescription(nameof(Resources.FindArrayItems_Description))]
    public class FindArrayItems : ContinuableAsyncCodeActivity
    {
        #region Properties

        /// <summary>
        /// If set, continue executing the remaining activities even if the current activity has failed.
        /// </summary>
        [LocalizedCategory(nameof(Resources.Common_Category))]
        [LocalizedDisplayName(nameof(Resources.ContinueOnError_DisplayName))]
        [LocalizedDescription(nameof(Resources.ContinueOnError_Description))]
        public override InArgument<bool> ContinueOnError { get; set; }

        [LocalizedDisplayName(nameof(Resources.FindArrayItems_FilterWords_DisplayName))]
        [LocalizedDescription(nameof(Resources.FindArrayItems_FilterWords_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<String[]> FilterWords { get; set; }

        [LocalizedDisplayName(nameof(Resources.FindArrayItems_DisplayLog_DisplayName))]
        [LocalizedDescription(nameof(Resources.FindArrayItems_DisplayLog_Description))]
        [LocalizedCategory(nameof(Resources.Options_Category))]
        public bool DisplayLog { get; set; }

        [LocalizedDisplayName(nameof(Resources.FindArrayItems_Results_DisplayName))]
        [LocalizedDescription(nameof(Resources.FindArrayItems_Results_Description))]
        [LocalizedCategory(nameof(Resources.Output_Category))]
        public OutArgument<String[]> Results { get; set; }

        [LocalizedDisplayName(nameof(Resources.FindArrayItems_AnchorWordsParameters_DisplayName))]
        [LocalizedDescription(nameof(Resources.FindArrayItems_AnchorWordsParameters_Description))]
        [LocalizedCategory(nameof(Resources.Options_Category))]
        public InArgument<string> AnchorWordsParameters { get; set; }

        [LocalizedDisplayName(nameof(Resources.FindArrayItems_InputArray_DisplayName))]
        [LocalizedDescription(nameof(Resources.FindArrayItems_InputArray_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<String[]> InputArray { get; set; }

        //////////////////////////////////////////////////////////////////////
        //Update Data Row
        // ////////////////////////////////////////////////////////////////////
        [LocalizedDisplayName("Activate")]
        [LocalizedDescription("Update Data Row variable with Extraction Result")]
        [LocalizedCategory("Output Data Row")]
        public bool BUpdateDataRow { get; set; }

        [LocalizedDisplayName("Data Row")]
        [LocalizedDescription("Output Data Row")]
        [LocalizedCategory("Output Data Row")]
        public InOutArgument<DataRow> MyDataRow { get; set; }

        [LocalizedDisplayName("Column Name")]
        [LocalizedDescription("Data Row Column to be updated")]
        [LocalizedCategory("Output Data Row")]
        public InArgument<string> MyDataRowColumn { get; set; }

        [LocalizedDisplayName("Index")]
        [LocalizedDescription("'Results' variable index to be outputed to the DataRow. Use -1 for the last find")]
        [LocalizedCategory("Output Data Row")]
        public InArgument<int> MyIndex { get; set; }

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

        public FindArrayItems()
        {
        }

        #endregion


        #region Protected Methods

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            if (FilterWords == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(FilterWords)));
            if (InputArray == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(InputArray)));
            if (Results == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(Results)));

            base.CacheMetadata(metadata);
        }

        protected override async Task<Action<AsyncCodeActivityContext>> ExecuteAsync(AsyncCodeActivityContext context, CancellationToken cancellationToken)
        {
            // Inputs
            var filterWords = FilterWords.Get(context);
            var displayLog = DisplayLog;
            var anchorWordsText = AnchorWordsParameters.Get(context);
            var inputArray = InputArray.Get(context);

            //Output Data Row
            bool bUpdateDataRow = BUpdateDataRow;
            var myDataRow = MyDataRow.Get(context);
            var myDataRowColumn = MyDataRowColumn.Get(context);
            var myIndex = MyIndex.Get(context);

            string OutputString = null;

            ///////////////////////////
            // Add execution logic HERE
            string[] OutputResults = CallExtractions.CallFindArrayItems(inputArray, filterWords, anchorWordsText, displayLog);

            #region Update Data Row (optional)
            //Check if functionality is Activated
            if (bUpdateDataRow == true)
            {
                //Check it there is an item to the Output Variable
                if (OutputResults.Length > 0)
                {
                    if (myIndex == -1)
                    {

                        //Upper Bound
                        OutputString = OutputResults[OutputResults.Length - 1];
                    }
                    else
                    {
                        OutputString = OutputResults[myIndex];
                    }

                    //Update Data Row
                    Utils.CallUpdateDataRow2(myDataRow, myDataRowColumn, OutputString);
                }

            }
            #endregion
            ///////////////////////////

            // Outputs
            return (ctx) => {
                Results.Set(ctx, OutputResults);
            };
        }

        #endregion
    }
}

