using System;
using System.Activities;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Data;
using BillBlech.TextToolbox.Activities.Activities;
using BillBlech.TextToolbox.Activities.Properties;
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
        public InArgument<String[]> BegWords { get; set; }

        [LocalizedDisplayName(nameof(Resources.ExtractTextBetweenTwoAnchorWords_EndWords_DisplayName))]
        [LocalizedDescription(nameof(Resources.ExtractTextBetweenTwoAnchorWords_EndWords_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<String[]> EndWords { get; set; }

        [LocalizedDisplayName(nameof(Resources.ExtractTextBetweenTwoAnchorWords_RegexParameter_DisplayName))]
        [LocalizedDescription(nameof(Resources.ExtractTextBetweenTwoAnchorWords_RegexParameter_Description))]
        [LocalizedCategory(nameof(Resources.Options_Category))]
        public EnumRegexParameter RegexParameter { get; set; }

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


        public enum EnumRegexParameter
        {
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
            var begWords = BegWords.Get(context);
            var endWords = EndWords.Get(context);
            var regexParameter = RegexParameter;
            bool displayLog = DisplayLog;
            bool displayRegex = DisplatyRegex;

            string[] OutputResults = null;

            //Output Data Row
            bool bUpdateDataRow = BUpdateDataRow;
            var myDataRow = MyDataRow.Get(context);
            var myDataRowColumn = MyDataRowColumn.Get(context);
            var myIndex = MyIndex.Get(context);

            string OutputString = null;
            
            //Start the Variable
            RegexOptions regexOptionsoptions = RegexOptions.None;

            //Check the Variable
            switch (regexParameter)
            {
                //Same Line
                case EnumRegexParameter.SameLine:
                    regexOptionsoptions = RegexOptions.Multiline;
                    break;
                //Different Lines
                case EnumRegexParameter.DifferentLines:
                    regexOptionsoptions = RegexOptions.Singleline;
                    break;

            }

            ///////////////////////////
            // Add execution logic HERE

            #region BegWords & End Words
            if (begWords != null && endWords != null)
            {
                //Loop through the Beggining Words
                foreach (string begWord in begWords)
                {
                    //Loop through Ending Words
                    foreach (string endWord in endWords)
                    {

                        //Search the Text
                        OutputResults = Utils.ExtractTextBetweenTags(inputText, begWord, endWord, regexOptionsoptions, displayLog, displayRegex);

                        //Exit the Loop in case result is found
                        if (OutputResults.Length > 0)
                        {
                            //Exit the Loop
                            goto ExitLoop;
                        }

                    }
                }
            }

            #endregion

            #region BegWords Only
            if (begWords != null && endWords == null)
            {
                //Loop through the Beggining Words
                foreach (string begWord in begWords)
                {
                    //Search the Text
                    OutputResults = Utils.ExtractTextBetweenTags(inputText, begWord, null, regexOptionsoptions, displayLog, displayRegex);

                    //Exit the Loop in case result is found
                    if (OutputResults.Length > 0)
                    {
                        //Exit the Loop
                        goto ExitLoop;
                    }

                }
            }

            #endregion

            #region EndWords Only
            if (begWords == null && endWords != null)
            {
                //Loop through the Beggining Words
                foreach (string endWord in endWords)
                {
                    //Search the Text
                    OutputResults = Utils.ExtractTextBetweenTags(inputText, null, endWord, regexOptionsoptions, displayLog, displayRegex);

                    //Exit the Loop in case result is found
                    if (OutputResults.Length > 0)
                    {
                        //Exit the Loop
                        goto ExitLoop;
                    }

                }
            }

            #endregion

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

