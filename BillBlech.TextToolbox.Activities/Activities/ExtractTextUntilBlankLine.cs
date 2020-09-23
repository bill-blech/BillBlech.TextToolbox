using BillBlech.TextToolbox.Activities.Activities;
using BillBlech.TextToolbox.Activities.Properties;
using System;
using System.Activities;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.Localization;
using UiPath.Shared.Activities.Utilities;

namespace BillBlech.TextToolbox.Activities
{
    [LocalizedDisplayName(nameof(Resources.ExtractTextUntilBlankLine_DisplayName))]
    [LocalizedDescription(nameof(Resources.ExtractTextUntilBlankLine_Description))]
    public class ExtractTextUntilBlankLine : ContinuableAsyncCodeActivity
    {
        #region Properties

        /// <summary>
        /// If set, continue executing the remaining activities even if the current activity has failed.
        /// </summary>
        [LocalizedCategory(nameof(Resources.Common_Category))]
        [LocalizedDisplayName(nameof(Resources.ContinueOnError_DisplayName))]
        [LocalizedDescription(nameof(Resources.ContinueOnError_Description))]
        public override InArgument<bool> ContinueOnError { get; set; }

        [LocalizedDisplayName(nameof(Resources.ExtractTextUntilBlankLine_AnchorWords_DisplayName))]
        [LocalizedDescription(nameof(Resources.ExtractTextUntilBlankLine_AnchorWords_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<String[]> AnchorWords { get; set; }

        [LocalizedDisplayName(nameof(Resources.ExtractTextUntilBlankLine_AnchorWordsParameter_DisplayName))]
        [LocalizedDescription(nameof(Resources.ExtractTextUntilBlankLine_AnchorWordsParameter_Description))]
        [LocalizedCategory(nameof(Resources.Options_Category))]
        //public EnumAnchorTextParam AnchorWordsParameter { get; set; }
        public InArgument<String> AnchorWordsParameter { get; set; }

        [LocalizedDisplayName(nameof(Resources.ExtractTextUntilBlankLine_Direction_DisplayName))]
        [LocalizedDescription(nameof(Resources.ExtractTextUntilBlankLine_Direction_Description))]
        [LocalizedCategory(nameof(Resources.Options_Category))]
        public EnumDirection Direction { get; set; }

        [LocalizedDisplayName(nameof(Resources.ExtractTextUntilBlankLine_IncludeAnchorWordsRow_DisplayName))]
        [LocalizedDescription(nameof(Resources.ExtractTextUntilBlankLine_IncludeAnchorWordsRow_Description))]
        [LocalizedCategory(nameof(Resources.Options_Category))]
        public bool IncludeAnchorWordsRow { get; set; }

        [LocalizedDisplayName(nameof(Resources.ExtractTextUntilBlankLine_Results_DisplayName))]
        [LocalizedDescription(nameof(Resources.ExtractTextUntilBlankLine_Results_Description))]
        [LocalizedCategory(nameof(Resources.Output_Category))]
        public OutArgument<String[]> Results { get; set; }

        [LocalizedDisplayName(nameof(Resources.ExtractTextUntilBlankLine_DisplayLog_DisplayName))]
        [LocalizedDescription(nameof(Resources.ExtractTextUntilBlankLine_DisplayLog_Description))]
        [LocalizedCategory(nameof(Resources.Options_Category))]
        public bool DisplayLog { get; set; }

        [LocalizedDisplayName(nameof(Resources.ExtractTextUntilBlankLine_DisplayRegex_DisplayName))]
        [LocalizedDescription(nameof(Resources.ExtractTextUntilBlankLine_DisplayRegex_Description))]
        [LocalizedCategory(nameof(Resources.Options_Category))]
        public bool DisplayRegex { get; set; }


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

        public enum EnumAnchorTextParam
        {
            Null,
            Any,
            All,

        }

        public enum EnumDirection
        {
            Above,
            Below,
            Both,

        }

        #endregion


        #region Constructors

        public ExtractTextUntilBlankLine()
        {
            Constraints.Add(ActivityConstraints.HasParentType<ExtractTextUntilBlankLine, TextApplicationScope>(string.Format(Resources.ValidationScope_Error, Resources.TextApplicationScope_DisplayName)));
        }

        #endregion


        #region Protected Methods

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            if (AnchorWords == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(AnchorWords)));
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
            var anchorWordsParameterText = AnchorWordsParameter.Get(context);
            var direction = Direction;
            var includeAnchorWordsRow = IncludeAnchorWordsRow;
            var displayLog = DisplayLog;
            var displayRegex = DisplayRegex;

            //Fill in Variable anchorTextParam
            EnumAnchorTextParam anchorTextParam = EnumAnchorTextParam.Null;

            //Fill in Variable anchorTextParam
            switch (anchorWordsParameterText)
            {
                //Any
                case "Any":
                    anchorTextParam = EnumAnchorTextParam.Any;
                    break;
                //All
                case "All":
                    anchorTextParam = EnumAnchorTextParam.All;
                    break;
            }

            string[] OutputResults = null;

            //Output Data Row
            bool bUpdateDataRow = BUpdateDataRow;
            var myDataRow = MyDataRow.Get(context);
            var myDataRowColumn = MyDataRowColumn.Get(context);
            var myIndex = MyIndex.Get(context);

            string OutputString = null;

            ///////////////////////////
            // Add execution logic HERE

            //Check Direction Variable
            switch (Direction)
            {
                //Below
                #region Direction: Below
                case EnumDirection.Below:

                    //Chech Anchor Words Parameter
                    switch (anchorTextParam)
                    {

                        //Any of the Anchor Words
                        case EnumAnchorTextParam.Any:

                            //Loop through the Words
                            foreach (string Word in anchorWords)
                            {
                                //Extract text Below Line Text Until Blank Line
                                OutputResults = Utils.ExtractAllLinesBelowAnchorTextUntilBlankline(inputText, Word, displayLog, displayRegex, includeAnchorWordsRow);

                                //Exit the Loop in case result is found
                                if (OutputResults.Length > 0)
                                {
                                    //Exit the Loop
                                    goto ExitLoop;
                                }

                            }

                            break;

                        //All of the Anchor Words
                        case EnumAnchorTextParam.All:

                            //Extract text Below Line Array Text Until Blank Line
                            OutputResults = Utils.ExtractAllLinesBelowAnchorArrayofTextUntilBlankline(inputText, anchorWords, displayLog, displayRegex, includeAnchorWordsRow);

                            break;

                    }

                    //End Above Code
                    break;
                #endregion

                //Above
                #region Direction: Above
                case EnumDirection.Above:

                    //Chech Anchor Words Parameter
                    switch (anchorTextParam)
                    {

                        //Any of the Anchor Words
                        case EnumAnchorTextParam.Any:

                            //Loop through the Words
                            foreach (string Word in anchorWords)
                            {
                                //Extract text Above Line Text Until Blank Line
                                OutputResults = Utils.ExtractAllLinesAboveAnchorTextUntilBlankline(inputText, Word, displayLog, displayRegex, includeAnchorWordsRow);

                                //Exit the Loop in case result is found
                                if (OutputResults.Length > 0)
                                {
                                    //Exit the Loop
                                    goto ExitLoop;
                                }
                            }

                            break;


                        //All of the Anchor Words
                        case EnumAnchorTextParam.All:

                            //Extract text Above Line Array Text Until Blank Line
                            OutputResults = Utils.ExtractAllLinesAboveAnchorArrayofTextUntilBlankline(inputText, anchorWords, displayLog, displayRegex, includeAnchorWordsRow);

                            break;

                    }


                    //End Below Code
                    break;

                #endregion

                #region Direction: Both Directions
                case EnumDirection.Both:


                    //Chech Anchor Words Parameter
                    switch (anchorTextParam)
                    {

                        //Any of the Anchor Words
                        case EnumAnchorTextParam.Any:

                            //Loop through the Words
                            foreach (string Word in anchorWords)
                            {
                                //Extract All Lines Both Direction Anchor Text Between Blank Lines
                                OutputResults = Utils.ExtractAllLinesBothDirectionsAnchorTextBetweenBlanklines(inputText, Word, displayLog, displayRegex);
                            }

                            break;

                        //All of the Anchor Words
                        case EnumAnchorTextParam.All:

                            //Extract All Lines Both Direction Anchor Text Array Between Blank Lines
                            OutputResults = Utils.ExtractAllLinesBothDirectionsAnchorArrayofTextBetweenBlanklines(inputText, anchorWords, displayLog, displayRegex);

                            break;
                    }

                    //End Both Directions Code
                    break;

                    #endregion
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
            return (ctx) =>
            {
                Results.Set(ctx, OutputResults);
            };
        }

        #endregion
    }
}

