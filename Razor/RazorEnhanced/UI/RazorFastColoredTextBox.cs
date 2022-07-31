using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Assistant
{
    public partial class RazorFastColoredTextBox : FastColoredTextBoxNS.FastColoredTextBox
    {
        public RazorFastColoredTextBox()
        {
            InitializeComponent();
        }
        protected override FastColoredTextBoxNS.TextSource CreateTextSource()
        {
            return new RazorTextSource(this);
        }
    }


    public class RazorTextSource : FastColoredTextBoxNS.TextSource
    {

        public RazorTextSource(RazorFastColoredTextBox currentTB)
            : base(currentTB)
        {
        }

        public override FastColoredTextBoxNS.Line CreateLine()
        {
            if (lines.Count > 0)
            {
                if (lines[0].Text.ToLower().StartsWith("//uos") || lines[0].Text.ToLower().StartsWith("//"))
                    {
                    if (CurrentTB.Language != FastColoredTextBoxNS.Language.Uos)
                    {
                        CurrentTB.Language = FastColoredTextBoxNS.Language.Uos;
                        CurrentTB.AutoIndentExistingLines = true;
                    }
                }
                else if (CurrentTB.Language == FastColoredTextBoxNS.Language.Uos)
                {
                    CurrentTB.Language = FastColoredTextBoxNS.Language.Python;
                }
            }

            int lineID = GenerateUniqueLineId();
            return new FastColoredTextBoxNS.Line(lineID);
        }

    }
    
}
