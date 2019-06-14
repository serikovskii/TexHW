using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MyTimer = System.Timers.Timer;

namespace TextHW
{
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            MyTimer timer = new MyTimer(10000);
            timer.Elapsed += TimerElapsed;
            timer.Start();
        }

        public void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            Thread thread = new Thread(() =>
            {
                TextRange doc = new TextRange(note.Document.ContentStart, note.Document.ContentEnd);
                using (FileStream fileStream = File.Create("fileReserve.txt"))
                {
                    doc.Save(fileStream, DataFormats.Text);
                }
            });
            thread.Start();
        }
        private void ItalicButton(object sender, RoutedEventArgs e)
        {
            EditingCommands.ToggleItalic.Execute(null, note);
        }

        private void BoldButton(object sender, RoutedEventArgs e)
        {
            EditingCommands.ToggleBold.Execute(null, note);
        }

        private void SaveButton(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text file (*.txt)|*.txt| RTF Files (*.rtf)|*.rtf | All files (*.*)|*.*";
            if (saveFileDialog.ShowDialog() == true)
            {
                TextRange doc = new TextRange(note.Document.ContentStart, note.Document.ContentEnd);
                using (FileStream fileStream = File.Create(saveFileDialog.FileName))
                {
                    doc.Save(fileStream, DataFormats.Text);
                }
            }
        }

        private void OpenButton(object sender, RoutedEventArgs e)
        {
            OpenFileDialog OP = new OpenFileDialog();
            OP.FileName = "";
            OP.Filter = "TXT File (*.txt)|*.txt |RTF Files (*.rtf)|*.rtf | All files (*.*)|*.*";
            OP.Title = "Открыть документ";
            if (OP.ShowDialog() == true)
            {
                var sr = new StreamReader(OP.FileName, Encoding.Default);
                string text = sr.ReadToEnd();
                var document = new FlowDocument();
                var paragraph = new Paragraph();
                paragraph.Inlines.Add(text);
                document.Blocks.Add(paragraph);
                note.Document = document;
            }
        }
    }
}
