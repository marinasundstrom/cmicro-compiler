using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CSharp.Compiler;

namespace Mini_CSharpGUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public Parser parser;
        public Scanner scanner;

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.SaveFile("temp.dat", RichTextBoxStreamType.PlainText);


            System.IO.StreamReader reader = new System.IO.StreamReader("temp.dat");

            scanner = new Scanner(reader, Scanner.Keywords, false);
            parser = new Parser(scanner.Scan(), false);

            reader.Close();

            if (parser.Errors.Count > 0)
            {
                richTextBox2.Clear();

                foreach (var error in parser.Errors)
                    richTextBox2.Text += (error.ToString() + "\n");
            }
            else
            {
                CodeXMLGenerator xmlgen = new CodeXMLGenerator((CSharp.Compiler.Ast.Program)parser.Result);
                xmlgen.GenerateAndSave("temp2.dat");

                richTextBox2.LoadFile("temp2.dat", RichTextBoxStreamType.PlainText);
            }
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (richTextBox1.SelectedText != null)
            {
                Clipboard.SetText(richTextBox1.SelectedText);
                richTextBox1.SelectedText = "";
            }
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (richTextBox1.SelectedText != null)
            {
                Clipboard.SetText(richTextBox1.SelectedText);
            }
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (richTextBox1.SelectedText != null)
            {
                richTextBox1.SelectedText = Clipboard.GetText();
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Title = "Open Source file";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.LoadFile(openFileDialog1.FileName, RichTextBoxStreamType.PlainText);
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Title = "Save Source As";
            saveFileDialog1.Filter = "All formats|*.*";

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.SaveFile(saveFileDialog1.FileName, RichTextBoxStreamType.PlainText);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Title = "Save Xml As";
            saveFileDialog1.Filter = "Xml|*.xml";

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                richTextBox2.SaveFile(saveFileDialog1.FileName, RichTextBoxStreamType.PlainText);
            }
        }
    }
}
