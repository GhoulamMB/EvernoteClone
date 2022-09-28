using Azure.Storage.Blobs;
using EvernoteClone.ViewModel;
using EvernoteClone.ViewModel.Helpers;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Media;

namespace EvernoteClone.View
{
    /// <summary>
    /// Interaction logic for NoteWindow.xaml
    /// </summary>
    public partial class NotesWindow : Window
    {
        NotesVM? NotesVM;
        public NotesWindow()
        {

            InitializeComponent();

            NotesVM = Resources["NVM"] as NotesVM;

            NotesVM!.SelectedNoteChanged += NotesVM_SelectedNoteChanged;

            var fontFamilies = Fonts.SystemFontFamilies.OrderBy(f => f.Source);
            List<double> fontSizes = new List<double>() { 8, 9, 10, 12, 14, 16, 20, 30, 60, 72 };
            fontFamilyComboBox.ItemsSource = fontFamilies;
            fontSizeComboBox.ItemsSource = fontSizes;
        }

        protected override async void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            if (string.IsNullOrEmpty(App.UserID))
            {
                LoginWindow loginWindow = new();
                loginWindow.ShowDialog();
                await NotesVM!.GetNoteBooks();
            }
        }

        private async void NotesVM_SelectedNoteChanged(object? sender, EventArgs e)
        {
            contentRichTextContent.Document.Blocks.Clear();
            if(NotesVM!.SelectedNote != null)
            {
                if (!string.IsNullOrEmpty(NotesVM!.SelectedNote.FileLocation))
                {
                    string downloadPath = $"{NotesVM.SelectedNote.Id}.rtf";
                    await new BlobClient(new Uri(NotesVM.SelectedNote.FileLocation)).DownloadToAsync(downloadPath);
                    using (FileStream fileStream = new FileStream(downloadPath, FileMode.Open))
                    {
                        var contents = new TextRange(contentRichTextContent.Document.ContentStart, contentRichTextContent.Document.ContentEnd);
                        contents.Load(fileStream, DataFormats.Rtf);
                    }
                }
            }
        }

        private void contentRichTextContent_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            int ammountofchars = (
                new TextRange(
                    contentRichTextContent.Document.ContentStart, 
                    contentRichTextContent.Document.ContentEnd)
                ).Text.Length;

            statusTextBlock.Text = $"Document Lenght : {ammountofchars} characters";
        }

        private async void SpeechButton_Click(object sender, RoutedEventArgs e)
        {
            string region = "eastus";
            string key1 = "bff4ec7bcfe344ccb8dc64e9c791c426";
            //string key2 = "1f01fa6809444715baba2838f8e160ea";
            //string url = "https://eastus.api.cognitive.microsoft.com/sts/v1.0/issuetoken";

            var speechConf = SpeechConfig.FromSubscription(key1, region);
            using (var audioConf = AudioConfig.FromDefaultMicrophoneInput())
            {
                using(var speechRec = new SpeechRecognizer(speechConf,audioConf))
                {
                    var result = await speechRec.RecognizeOnceAsync();
                    contentRichTextContent.Document.Blocks.Add(
                        new Paragraph(
                            new Run(
                                result.Text)));
                }
            }
        }

        private void BoldButton_Click(object sender, RoutedEventArgs e)
        {
            ToggleButton? toggle = sender as ToggleButton;
            bool isChecked = toggle!.IsChecked ?? false;
            if (isChecked)
            {
                contentRichTextContent.Selection.ApplyPropertyValue(Inline.FontWeightProperty, FontWeights.Bold);
            }
            else
            {
                contentRichTextContent.Selection.ApplyPropertyValue(Inline.FontWeightProperty, FontWeights.Normal);
            }
        }

        private void contentRichTextContent_SelectionChanged(object sender, RoutedEventArgs e)
        {
            var selectedWeight = contentRichTextContent.Selection.GetPropertyValue(FontWeightProperty);
            BoldButton.IsChecked = (selectedWeight != DependencyProperty.UnsetValue) &&
                selectedWeight.Equals(FontWeights.Bold);

            var selectedStyle = contentRichTextContent.Selection.GetPropertyValue(FontStyleProperty);
            ItalicButton.IsChecked = (selectedStyle != DependencyProperty.UnsetValue) &&
                selectedStyle.Equals(FontStyles.Italic);

            var selectedDecoration = contentRichTextContent.Selection.GetPropertyValue(Inline.TextDecorationsProperty);
            UnderlineButton.IsChecked = (selectedDecoration != DependencyProperty.UnsetValue) &&
                selectedDecoration.Equals(TextDecorations.Underline);

            fontFamilyComboBox.SelectedItem = contentRichTextContent.Selection.GetPropertyValue(Inline.FontFamilyProperty);
            fontSizeComboBox.Text = contentRichTextContent.Selection.GetPropertyValue(Inline.FontSizeProperty).ToString();
        }

        private void ItalicButton_Click(object sender, RoutedEventArgs e)
        {
            bool isButtonEnabled = (sender as ToggleButton)!.IsChecked ?? false;
            if (isButtonEnabled)
            {
                contentRichTextContent.Selection.ApplyPropertyValue(Inline.FontStyleProperty, FontStyles.Italic);
            }
            else
            {
                contentRichTextContent.Selection.ApplyPropertyValue(Inline.FontStyleProperty, FontStyles.Normal);
            }
        }

        private void UnderlineButton_Click(object sender, RoutedEventArgs e)
        {
            bool isButtonEnabled = (sender as ToggleButton)!.IsChecked ?? false;
            if (isButtonEnabled)
            {
                contentRichTextContent.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, TextDecorations.Underline);
            }
            else
            {
                TextDecorationCollection textDecorations;
                (contentRichTextContent.Selection.GetPropertyValue(Inline.TextDecorationsProperty) as TextDecorationCollection).TryRemove(TextDecorations.Underline,out textDecorations);
                contentRichTextContent.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, textDecorations);
            }
        }

        private void fontFamilyComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (fontFamilyComboBox.SelectedItem != null)
            {
                contentRichTextContent.Selection.ApplyPropertyValue(Inline.FontFamilyProperty, fontFamilyComboBox.SelectedItem);
            }
        }

        private void fontSizeComboBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            contentRichTextContent.Selection.ApplyPropertyValue(Inline.FontSizeProperty, fontSizeComboBox.SelectedItem);
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string fileName = $"{NotesVM!.SelectedNote.Id}.rtf";
            string rtfFile = Path.Combine(Environment.CurrentDirectory, fileName);

            using (FileStream fileStream = new(rtfFile, FileMode.Create))
            {
                TextRange content = new(contentRichTextContent.Document.ContentStart, contentRichTextContent.Document.ContentEnd);
                content.Save(fileStream, DataFormats.Rtf);
            }
            NotesVM.SelectedNote.FileLocation = await UpdateFile(rtfFile,fileName);
            await DBHelper.Update(NotesVM.SelectedNote);
        }

        private async Task<string> UpdateFile(string rtfFile,string fileName)
        {
            string connectionURI = "DefaultEndpointsProtocol=https;AccountName=evernotestorage0683;AccountKey=zkba2ddslNBu5vumIkisO/96z++SkU/3YGm6vsfVdT0pp9jwSXpkM6JD/i7E71zVWTTR8tzaVDO2+AStf6zfmw==;EndpointSuffix=core.windows.net";
            string containerName = "notes";

            var container = new BlobContainerClient(connectionURI, containerName);
            var blob = container.GetBlobClient(fileName);
            await blob.UploadAsync(rtfFile,overwrite:true);
            return $"https://evernotestorage0683.blob.core.windows.net/notes/{fileName}";
        }
    }
}
