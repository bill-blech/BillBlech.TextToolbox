using System.Activities.Presentation.Metadata;
using System.ComponentModel;
using System.ComponentModel.Design;
using BillBlech.TextToolbox.Activities.Design.Designers;
using BillBlech.TextToolbox.Activities.Design.Properties;

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


            MetadataStore.AddAttributeTable(builder.CreateTable());
        }
    }
}
