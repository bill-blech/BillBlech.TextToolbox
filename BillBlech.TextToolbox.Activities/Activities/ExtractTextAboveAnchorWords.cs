using System;
using System.Activities;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using BillBlech.TextToolbox.Activities.Activities;
using BillBlech.TextToolbox.Activities.Properties;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.Localization;
using UiPath.Shared.Activities.Utilities;

namespace BillBlech.TextToolbox.Activities
{
    [LocalizedDisplayName(nameof(Resources.ExtractTextAboveAnchorWords_DisplayName))]
    [LocalizedDescription(nameof(Resources.ExtractTextAboveAnchorWords_Description))]
    public class ExtractTextAboveAnchorWords : ContinuableAsyncCodeActivity
    {
        #region Properties

        /// <summary>
        /// If set, continue executing the remaining activities even if the current activity has failed.
        /// </summary>
        [LocalizedCategory(nameof(Resources.Common_Category))]
        [LocalizedDisplayName(nameof(Resources.ContinueOnError_DisplayName))]
        [LocalizedDescription(nameof(Resources.ContinueOnError_Description))]
        public override InArgument<bool> ContinueOnError { get; set; }

        [LocalizedDisplayName(nameof(Resources.ExtractTextAboveAnchorWords_AnchorWords_DisplayName))]
        [LocalizedDescription(nameof(Resources.ExtractTextAboveAnchorWords_AnchorWords_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<String[]> AnchorWords { get; set; }

        [LocalizedDisplayName(nameof(Resources.ExtractTextAboveAnchorWords_Results_DisplayName))]
        [LocalizedDescription(nameof(Resources.ExtractTextAboveAnchorWords_Results_Description))]
        [LocalizedCategory(nameof(Resources.Output_Category))]
        public OutArgument<String[]> Results { get; set; }

        [LocalizedDisplayName(nameof(Resources.ExtractTextAboveAnchorWords_DisplayLog_DisplayName))]
        [LocalizedDescription(nameof(Resources.ExtractTextAboveAnchorWords_DisplayLog_Description))]
        [LocalizedCategory(nameof(Resources.Options_Category))]
        public bool DisplayLog { get; set; }

        [LocalizedDisplayName(nameof(Resources.ExtractTextAboveAnchorWords_DisplayRegex_DisplayName))]
        [LocalizedDescription(nameof(Resources.ExtractTextAboveAnchorWords_DisplayRegex_Description))]
        [LocalizedCategory(nameof(Resources.Options_Category))]
        public bool DisplayRegex { get; set; }

        [LocalizedDisplayName(nameof(Resources.ExtractTextAboveAnchorWords_LinesNumber_DisplayName))]
        [LocalizedDescription(nameof(Resources.ExtractTextAboveAnchorWords_LinesNumber_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<int> LinesNumber { get; set; }

        [LocalizedDisplayName(nameof(Resources.ExtractTextAboveAnchorWords_NumberLines_DisplayName))]
        [LocalizedDescription(nameof(Resources.ExtractTextAboveAnchorWords_NumberLines_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<int> NumberLines { get; set; }

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

        #endregion


        #region Constructors

        public ExtractTextAboveAnchorWords()
        {
            Constraints.Add(ActivityConstraints.HasParentType<ExtractTextAboveAnchorWords, TextApplicationScope>(string.Format(Resources.ValidationScope_Error, Resources.TextApplicationScope_DisplayName)));
        }

        #endregion


        #region Protected Methods

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            if (AnchorWords == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(AnchorWords)));
            if (LinesNumber == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(LinesNumber)));
            if (NumberLines == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(NumberLines)));
            if (Results == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(Results)));

            base.CacheMetadata(metadata);
        }

        protected override async Task<Action<AsyncCodeActivityContext>> ExecuteAsync(AsyncCodeActivityContext context, CancellationToken cancellationToken)
        {

            // Object Container: Use objectContainer.Get<T>() to retrieve objects from the scope
            var objectContainer = context.GetFromContext<IObjectContainer>(TextApplicationScope.ParentContainerPropertyTag);

            // Inputs
            string inputText = objectContainer.Get<string>();

            // Inputs
            var anchorWords = AnchorWords.Get(context);
            var displayLog = DisplayLog;
            var displayRegex = DisplayRegex;
            var linesNumber = LinesNumber.Get(context);
            var numberLines = NumberLines.Get(context);

            string[] OutputResults = null;

            //Output Data Row
            bool bUpdateDataRow = BUpdateDataRow;
            var myDataRow = MyDataRow.Get(context);
            var myDataRowColumn = MyDataRowColumn.Get(context);
            var myIndex = MyIndex.Get(context);

            string OutputString = null;

            ///////////////////////////
            // Add execution logic HERE

            //Loop through the Words
            foreach (string Word in anchorWords)
            {
                //Loop through last words
                OutputResults = Utils.ExtractTextLinePreviousAnchorText(inputText, Word, linesNumber, numberLines, displayLog, displayRegex);

                //Exit the Loop in case result is found
                if (OutputResults.Length > 0)
                {
                    //Exit the Loop
                    goto ExitLoop;
                }
            }

            ExitLoop:

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

