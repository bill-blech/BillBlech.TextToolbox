using BillBlech.TextToolbox.Activities.Activities;
using BillBlech.TextToolbox.Activities.Properties;
using System;
using System.Activities;
using System.Collections.ObjectModel;
using System.Data;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.Localization;
using UiPath.Shared.Activities.Utilities;

namespace BillBlech.TextToolbox.Activities
{
    [LocalizedDisplayName(nameof(Resources.ExtractTextBetweenTwoAnchorWords_DisplayName))]
    [LocalizedDescription(nameof(Resources.ExtractTextBetweenTwoAnchorWords_Description))]
    public class ExtractTextBetweenTwoAnchorWords : ContinuableAsyncCodeActivity
    {
        #region Properties

        /// <summary>
        /// If set, continue executing the remaining activities even if the current activity has failed.
        /// </summary>
        [LocalizedCategory(nameof(Resources.Common_Category))]
        [LocalizedDisplayName(nameof(Resources.ContinueOnError_DisplayName))]
        [LocalizedDescription(nameof(Resources.ContinueOnError_Description))]
        public override InArgument<bool> ContinueOnError { get; set; }

        [LocalizedDisplayName(nameof(Resources.ExtractTextBetweenTwoAnchorWords_BegWords_DisplayName))]
        [LocalizedDescription(nameof(Resources.ExtractTextBetweenTwoAnchorWords_BegWords_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        //public InArgument<String[]> BegWords { get; set; }
        public InArgument<Collection<string>> BegWords { get; set; }

        [LocalizedDisplayName(nameof(Resources.ExtractTextBetweenTwoAnchorWords_EndWords_DisplayName))]
        [LocalizedDescription(nameof(Resources.ExtractTextBetweenTwoAnchorWords_EndWords_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        //public InArgument<String[]> EndWords { get; set; }
        public InArgument<Collection<string>> EndWords { get; set; }

        [LocalizedDisplayName(nameof(Resources.ExtractTextBetweenTwoAnchorWords_RegexParameter_DisplayName))]
        [LocalizedDescription(nameof(Resources.ExtractTextBetweenTwoAnchorWords_RegexParameter_Description))]
        [LocalizedCategory(nameof(Resources.Options_Category))]
        //public EnumRegexParameter RegexParameter { get; set; }
        public InArgument<String> RegexParameter { get; set; }

        [LocalizedDisplayName(nameof(Resources.ExtractTextBetweenTwoAnchorWords_DisplayLog_DisplayName))]
        [LocalizedDescription(nameof(Resources.ExtractTextBetweenTwoAnchorWords_DisplayLog_Description))]
        [LocalizedCategory(nameof(Resources.Options_Category))]
        public bool DisplayLog { get; set; }

        [LocalizedDisplayName(nameof(Resources.ExtractTextBetweenTwoAnchorWords_DisplatyRegex_DisplayName))]
        [LocalizedDescription(nameof(Resources.ExtractTextBetweenTwoAnchorWords_DisplatyRegex_Description))]
        [LocalizedCategory(nameof(Resources.Options_Category))]
        public bool DisplatyRegex { get; set; }

        [LocalizedDisplayName(nameof(Resources.ExtractTextBetweenTwoAnchorWords_Results_DisplayName))]
        [LocalizedDescription(nameof(Resources.ExtractTextBetweenTwoAnchorWords_Results_Description))]
        [LocalizedCategory(nameof(Resources.Output_Category))]
        public OutArgument<String[]> Results { get; set; }


        // ////////////////////////////////////////////////////////////////////
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


        public enum EnumRegexParameter
        {
            Null,
            SameLine,
            DifferentLines

        }

        #endregion


        #region Constructors

        public ExtractTextBetweenTwoAnchorWords()
        {
            Constraints.Add(ActivityConstraints.HasParentType<ExtractTextBetweenTwoAnchorWords, TextApplicationScope>(string.Format(Resources.ValidationScope_Error, Resources.TextApplicationScope_DisplayName)));
        }

        #endregion


        #region Protected Methods

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {

            if (BegWords == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(BegWords)));
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
            var begWordsCol = BegWords.Get(context);
            var endWordsCol = EndWords.Get(context);
            var regexParameterText = RegexParameter.Get(context);
            bool displayLog = DisplayLog;
            bool displayRegex = DisplatyRegex;

            //Convert Collection to Array
            string[] begWords = Utils.ConvertCollectionToArray(begWordsCol);

            string[] endWords = null;
            if (endWordsCol != null)
            {
                endWords = Utils.ConvertCollectionToArray(endWordsCol);
            }
   
            //Output Data Row
            bool bUpdateDataRow = BUpdateDataRow;
            var myDataRow = MyDataRow.Get(context);
            var myDataRowColumn = MyDataRowColumn.Get(context);
            var myIndex = MyIndex.Get(context);

            string OutputString = null;

            ///////////////////////////
            // Add execution logic HERE
            string[] OutputResults = CallExtractions.CallExtractTextBetweenTwoAnchorWords(inputText, begWords, endWords, regexParameterText, displayLog, displayRegex);


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
            return (ctx) =>
            {
                Results.Set(ctx, OutputResults);
            };
        }

        #endregion
    }
}

