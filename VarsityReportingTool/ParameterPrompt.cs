using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VarsityReportingTool {
    public static class ParameterPrompt {
        public static DialogResult PromptForParameterValues(ref OdbcCommand command) {
            OdbcParameterCollection parameterCollection = command.Parameters;
            DialogResult result = DialogResult.Cancel;
            int parameterCount = parameterCollection.Count;

            Form prompt = new Form();
            prompt.Text = "Enter Query Values";

            prompt.Width = 200;
            prompt.Height = 90 + parameterCount * 25;
            prompt.FormBorderStyle = FormBorderStyle.FixedDialog;

            List<TextBox> textBoxes = new List<TextBox>();
            for(int i = 0; i != parameterCount; ++i) {
                Label label = new Label() { Left = 10, Top = 10 + 25 * i, Text = "Label " + i, Width = 50 };
                TextBox textBox = new TextBox() { Left = 10 + label.Width + 1, Top = 8 + 25 * i, Width = 100 };
                textBoxes.Add(textBox);

                prompt.Controls.Add(label);
                prompt.Controls.Add(textBox);
            }

            Button okButton = new Button() { Text = "Ok", Left = 15, Top = prompt.Height - 70, Width = 75 };
            prompt.Controls.Add(okButton);
            prompt.AcceptButton = okButton;
            okButton.Click += (sender, e) => {
                for(int i = 0; i != textBoxes.Count; ++i) {
                    parameterCollection[i].Value = textBoxes[i].Text;
                }

                result = DialogResult.OK;
                prompt.Close();
            };

            Button cancelButton = new Button() { Text = "Cancel", Left = 95, Top = prompt.Height - 70, Width = 75 };
            prompt.Controls.Add(cancelButton);
            prompt.CancelButton = cancelButton;
            cancelButton.Click += (sender, e) => { result = DialogResult.Cancel; };

            prompt.ShowDialog();
            return result;
        }
    }
}
