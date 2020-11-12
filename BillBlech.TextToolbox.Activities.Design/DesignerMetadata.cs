using BillBlech.TextToolbox.Activities.Design.Designers;
using BillBlech.TextToolbox.Activities.Design.Properties;
using System.Activities.Presentation.Metadata;
using System.ComponentModel;
using System.ComponentModel.Design;

namespace BillBlech.TextToolbox.Activities.Design
{
    public class DesignerMetadata : IRegisterMetadata
    {
        public void Register()
        {
            var builder = new AttributeTableBuilder();
            builder.ValidateTable();

            var categoryAttribute = new CategoryAttribute($"{Resources.Category}");

            builder.AddCustomAttributes(typeof(SplitTextNewLines), categoryAttribute);
            builder.AddCustomAttributes(typeof(SplitTextNewLines), new DesignerAttribute(typeof(SplitTextNewLinesDesigner)));
            builder.AddCustomAttributes(typeof(SplitTextNewLines), new HelpKeywordAttribute(""));

            builder.AddCustomAttributes(typeof(TextApplicationScope), categoryAttribute);
            builder.AddCustomAttributes(typeof(TextApplicationScope), new DesignerAttribute(typeof(TextApplicationScopeDesigner)));
            builder.AddCustomAttributes(typeof(TextApplicationScope), new HelpKeywordAttribute(""));

            builder.AddCustomAttributes(typeof(CountWordsInText), categoryAttribute);
            builder.AddCustomAttributes(typeof(CountWordsInText), new DesignerAttribute(typeof(CountWordsInTextDesigner)));
            builder.AddCustomAttributes(typeof(CountWordsInText), new HelpKeywordAttribute(""));

            builder.AddCustomAttributes(typeof(RemoveEmptyRows), categoryAttribute);
            builder.AddCustomAttributes(typeof(RemoveEmptyRows), new DesignerAttribute(typeof(RemoveEmptyRowsDesigner)));
            builder.AddCustomAttributes(typeof(RemoveEmptyRows), new HelpKeywordAttribute(""));

            builder.AddCustomAttributes(typeof(SplitTextRows), categoryAttribute);
            builder.AddCustomAttributes(typeof(SplitTextRows), new DesignerAttribute(typeof(SplitTextRowsDesigner)));
            builder.AddCustomAttributes(typeof(SplitTextRows), new HelpKeywordAttribute(""));

            builder.AddCustomAttributes(typeof(ExtractAllLinesBelowAnchorText), categoryAttribute);
            builder.AddCustomAttributes(typeof(ExtractAllLinesBelowAnchorText), new DesignerAttribute(typeof(ExtractAllLinesBelowAnchorTextDesigner)));
            builder.AddCustomAttributes(typeof(ExtractAllLinesBelowAnchorText), new HelpKeywordAttribute(""));

            builder.AddCustomAttributes(typeof(ExtractTextBelowAnchorWords), categoryAttribute);
            builder.AddCustomAttributes(typeof(ExtractTextBelowAnchorWords), new DesignerAttribute(typeof(ExtractTextBelowAnchorWordsDesigner)));
            builder.AddCustomAttributes(typeof(ExtractTextBelowAnchorWords), new HelpKeywordAttribute(""));

            builder.AddCustomAttributes(typeof(ExtractTextBetweenTwoAnchorWords), categoryAttribute);
            builder.AddCustomAttributes(typeof(ExtractTextBetweenTwoAnchorWords), new DesignerAttribute(typeof(ExtractTextBetweenTwoAnchorWordsDesigner)));
            builder.AddCustomAttributes(typeof(ExtractTextBetweenTwoAnchorWords), new HelpKeywordAttribute(""));

            builder.AddCustomAttributes(typeof(ExtractTextAboveAnchorWords), categoryAttribute);
            builder.AddCustomAttributes(typeof(ExtractTextAboveAnchorWords), new DesignerAttribute(typeof(ExtractTextAboveAnchorWordsDesigner)));
            builder.AddCustomAttributes(typeof(ExtractTextAboveAnchorWords), new HelpKeywordAttribute(""));

            builder.AddCustomAttributes(typeof(ExtractTextUntilBlankLine), categoryAttribute);
            builder.AddCustomAttributes(typeof(ExtractTextUntilBlankLine), new DesignerAttribute(typeof(ExtractTextUntilBlankLineDesigner)));
            builder.AddCustomAttributes(typeof(ExtractTextUntilBlankLine), new HelpKeywordAttribute(""));

            builder.AddCustomAttributes(typeof(RemoveWords), categoryAttribute);
            builder.AddCustomAttributes(typeof(RemoveWords), new DesignerAttribute(typeof(RemoveWordsDesigner)));
            builder.AddCustomAttributes(typeof(RemoveWords), new HelpKeywordAttribute(""));

            builder.AddCustomAttributes(typeof(EncryptValue), categoryAttribute);
            builder.AddCustomAttributes(typeof(EncryptValue), new DesignerAttribute(typeof(EncryptValueDesigner)));
            builder.AddCustomAttributes(typeof(EncryptValue), new HelpKeywordAttribute(""));

            builder.AddCustomAttributes(typeof(DecryptValue), categoryAttribute);
            builder.AddCustomAttributes(typeof(DecryptValue), new DesignerAttribute(typeof(DecryptValueDesigner)));
            builder.AddCustomAttributes(typeof(DecryptValue), new HelpKeywordAttribute(""));

            builder.AddCustomAttributes(typeof(SplitTextByBlankLines), categoryAttribute);
            builder.AddCustomAttributes(typeof(SplitTextByBlankLines), new DesignerAttribute(typeof(SplitTextByBlankLinesDesigner)));
            builder.AddCustomAttributes(typeof(SplitTextByBlankLines), new HelpKeywordAttribute(""));

            builder.AddCustomAttributes(typeof(ReplaceWords), categoryAttribute);
            builder.AddCustomAttributes(typeof(ReplaceWords), new DesignerAttribute(typeof(ReplaceWordsDesigner)));
            builder.AddCustomAttributes(typeof(ReplaceWords), new HelpKeywordAttribute(""));

            builder.AddCustomAttributes(typeof(ExtractAllCharactersUntilWhiteSpace), categoryAttribute);
            builder.AddCustomAttributes(typeof(ExtractAllCharactersUntilWhiteSpace), new DesignerAttribute(typeof(ExtractAllCharactersUntilWhiteSpaceDesigner)));
            builder.AddCustomAttributes(typeof(ExtractAllCharactersUntilWhiteSpace), new HelpKeywordAttribute(""));

            builder.AddCustomAttributes(typeof(FindArrayItems), categoryAttribute);
            builder.AddCustomAttributes(typeof(FindArrayItems), new DesignerAttribute(typeof(FindArrayItemsDesigner)));
            builder.AddCustomAttributes(typeof(FindArrayItems), new HelpKeywordAttribute(""));

            builder.AddCustomAttributes(typeof(MatchItemInArray), categoryAttribute);
            builder.AddCustomAttributes(typeof(MatchItemInArray), new DesignerAttribute(typeof(MatchItemInArrayDesigner)));
            builder.AddCustomAttributes(typeof(MatchItemInArray), new HelpKeywordAttribute(""));

            builder.AddCustomAttributes(typeof(ExtractAllCharactersUntilNextLetter), categoryAttribute);
            builder.AddCustomAttributes(typeof(ExtractAllCharactersUntilNextLetter), new DesignerAttribute(typeof(ExtractAllCharactersUntilNextLetterDesigner)));
            builder.AddCustomAttributes(typeof(ExtractAllCharactersUntilNextLetter), new HelpKeywordAttribute(""));

            builder.AddCustomAttributes(typeof(ReadTextFileEncondig), categoryAttribute);
            builder.AddCustomAttributes(typeof(ReadTextFileEncondig), new DesignerAttribute(typeof(ReadTextFileEncondigDesigner)));
            builder.AddCustomAttributes(typeof(ReadTextFileEncondig), new HelpKeywordAttribute(""));

            builder.AddCustomAttributes(typeof(DetectLanguage), categoryAttribute);
            builder.AddCustomAttributes(typeof(DetectLanguage), new DesignerAttribute(typeof(DetectLanguageDesigner)));
            builder.AddCustomAttributes(typeof(DetectLanguage), new HelpKeywordAttribute(""));

            builder.AddCustomAttributes(typeof(DetectSentiment), categoryAttribute);
            builder.AddCustomAttributes(typeof(DetectSentiment), new DesignerAttribute(typeof(DetectSentimentDesigner)));
            builder.AddCustomAttributes(typeof(DetectSentiment), new HelpKeywordAttribute(""));


            MetadataStore.AddAttributeTable(builder.CreateTable());
        }
    }
}
