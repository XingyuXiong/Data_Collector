using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Collector
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Info object stroing necessary information to show in table for imported text - contains ID, Content, Done
        private ObservableCollection<Info> info;
        // List of all fields' names of Info class
        private List<string> lst_Field_Names;
        // Path to save txt and wav files,which is the directory of current working project
        //private string SavePath = Environment.CurrentDirectory;
        private string SavePath =System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        //Path to save compression package
        private string CompressPath = "";
        private Info CurrentItem;

        public MainWindow()
        {
            // Initialize fields
            info = new ObservableCollection<Info>();
            lst_Field_Names = typeof(Info).GetProperties().Select(f => f.Name).ToList();

            // Create directory for storing audio files
            if (!Directory.Exists(SavePath + @"\audio"))
            {
                //create a directory to save audios
                Directory.CreateDirectory(SavePath + @"\audio");
            }

            // As usual
            InitializeComponent();

            // Some UI element initialization
            for (int i = 0; i < lst_Field_Names.Count; i++)
            {
                // Add columns according to types of fileds in Info class
                grdContent.Columns.Add(new GridViewColumn { Header = lst_Field_Names[i], DisplayMemberBinding = new Binding(lst_Field_Names[i]) });
            }
            Terminate_Button.Visibility = Visibility.Collapsed;
            Terminate_Button.IsEnabled = false;
            Terminate_Button2.Visibility = Visibility.Collapsed;
            Terminate_Button2.IsEnabled = false;
            Search_Type_ComboBox.ItemsSource = lst_Field_Names;
            Search_Type_ComboBox.SelectedIndex = lst_Field_Names.IndexOf("Content");
            AudioPlayer.LoadedBehavior = MediaState.Manual;
            AudioPlayer.Stop();
        }

        /// <summary>
        /// Select desired file and import data from it, then publish data to the table
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Import_Data(object sender, RoutedEventArgs e)
        {
            Import_Button.IsEnabled = false;

            Import_Button.Content = "Start Another";

            // OPen a dialog to select desired file - *.txt only
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Text | *.txt";
            if (dialog.ShowDialog() == true)
            {
                info = Txt_To_Collection(dialog.FileName);
            }

            // Bind Info List as itemsource of the table
            lstContent.ItemsSource = info;

            // Set view for filtering
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lstContent.ItemsSource);

            // Set Filter for filtering on the fly
            view.Filter = UserFilter;

            Update_Progress();

            lstContent.Visibility = Visibility.Visible;

            Import_Button.IsEnabled = true;
        }

        /// <summary>
        /// Export collected data to zip files
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Export_Data(object sender, RoutedEventArgs e)
        {
            Export_Button.IsEnabled = false;

            // Check if any record exists
            bool flag = false;
            foreach (Info line_info in lstContent.Items)
            {
                if (line_info.Done == "Yes")
                    flag = true;
            }

            // If any record exist, pack them
            if (flag)
            {
                System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    CompressPath = dialog.SelectedPath;

                    if (File.Exists(CompressPath + @"\audio.zip"))
                    {
                        File.Delete(CompressPath + @"\audio.zip");
                    }
                    System.IO.Compression.ZipFile.CreateFromDirectory(SavePath + @"\audio", CompressPath + @"\audio.zip");
                    MessageBox.Show("File has been saved to " + CompressPath + @"\audio.zip");
                }
            }
            // If not, show alert
            else
            {
                MessageBox.Show("There is no file to compress!");
            }

            Export_Button.IsEnabled = true;
        }

        /// <summary>
        /// Converting text file's content to a list of Info objects
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private ObservableCollection<Info> Txt_To_Collection(string path)
        {
            // Read selected file
            ObservableCollection<Info> result = new ObservableCollection<Info>();
            System.IO.StreamReader file = new System.IO.StreamReader(@path);
            string line;
            int count = 0;
            while ((line = file.ReadLine()) != null)
            {
                // Increment count
                count++;

                // Initialize Done argument
                string done = "No";
                if (File.Exists(string.Format(SavePath + @"\audio\{0}.wav", count)))
                {
                    done = "Yes";
                }

                // Converting a line of text to one Info object
                result.Add(new Info() { ID = count.ToString(), Content = line, Done = done });
            }

            return result;
        }

        /// <summary>
        /// Filter for textbox searching
        /// </summary>
        /// <param name="line_info"></param>
        /// <returns></returns>
        private bool UserFilter(object line_info)
        {
            if (String.IsNullOrEmpty(TextFilter_TextBox.Text))
            {
                return true;
            }
            else
            {
                return (line_info as Info).GetType().GetProperty(lst_Field_Names[Search_Type_ComboBox.SelectedIndex]).GetGetMethod().Invoke(line_info, null).ToString().IndexOf(TextFilter_TextBox.Text, StringComparison.OrdinalIgnoreCase) >= 0;
            }
        }

        /// <summary>
        /// Call when new text enters textbox, refersh what is shown in the table
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtFilter_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(lstContent.ItemsSource).Refresh();
        }

        /// <summary>
        /// Select all entries
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Select_All(object sender, RoutedEventArgs e)
        {
            lstContent.SelectAll();
        }

        /// <summary>
        /// Delete selected entry(s) from the table
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Delete_Audios(object sender, RoutedEventArgs e)
        {
            if (lstContent.SelectedItems.Count == 0)
            {
                // Message for unselection notification
                MessageBox.Show(string.Format("Please select entries first."));
            }
            else
            {
                // For each recorded entry, delete its record
                foreach (Info item in lstContent.SelectedItems)
                {
                    // Get index of the item
                    int ID = Int32.Parse(item.ID);

                    // Delete audio file
                    if (File.Exists(string.Format(SavePath + @"\audio\{0}.wav", ID)))
                    {
                        File.Delete(string.Format(SavePath + @"\audio\{0}.wav", ID));
                    }

                    info[ID - 1].Done = "No";
                }

                // Refresh binding and view
                lstContent.ItemsSource = info;
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lstContent.ItemsSource);
                view.Filter = UserFilter;

                // Show message
                MessageBox.Show(string.Format("Audios of selected lines have been deleted."));
            }
            Update_Progress();
        }

        /// <summary>
        /// Audio recording frame
        /// </summary>
        /// <param name="lpstrCommand"></param>
        /// <param name="lpstrReturnString"></param>
        /// <param name="uReturnLength"></param>
        /// <param name="hwndCallback"></param>
        /// <returns></returns>
        [DllImport("winmm.dll", EntryPoint = "mciSendString", CharSet = CharSet.Auto)]
        public static extern int mciSendString(
                 string lpstrCommand,
                 string lpstrReturnString,
                 int uReturnLength,
                 int hwndCallback
                );

        /// <summary>
        /// Record audio for one or many selected audio
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartRecord(string Wavname)
        {
            if (File.Exists(SavePath + @"\audio\" + Wavname))
            {
                new FileInfo(SavePath + @"\audio\" + Wavname).Attributes = FileAttributes.Normal;
                File.Delete(SavePath + @"\audio\" + Wavname);
            }
            string currentID = Wavname.Substring(0, (Wavname.Length - 4));
            Current_Content.Text = info[Int32.Parse(currentID) - 1].Content;
            Record_Button.Background = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
            Record_Button.Content = "Stop";
            mciSendString("set wave bitpersample 8", "", 0, 0);

            mciSendString("set wave samplespersec 20000", "", 0, 0);
            mciSendString("set wave channels 2", "", 0, 0);
            mciSendString("set wave format tag pcm", "", 0, 0);
            mciSendString("open new type WAVEAudio alias movie", "", 0, 0);

            mciSendString("record movie", "", 0, 0);
        }
        private void RecordNextAudio(Info Currentitem, Info Nextitem)
        {

            StartRecord(string.Format("{0}.wav", Nextitem.ID));
            //lstContent.SelectedItems.IndexOf()
            CurrentItem = Nextitem;

        }
        private void Record_Audio(object sender, RoutedEventArgs e)
        {
            if (lstContent.SelectedIndex == -1)
            {
                MessageBox.Show("You have not selected anything yet!");
                return;
            }
            Play_Button.IsEnabled = false;
            if (CurrentItem == null)
            {
                CurrentItem = (Info)lstContent.SelectedItems[0];
            }
            string WavName = string.Format("{0}.wav", CurrentItem.ID);
            if (Record_Button.Content.ToString() == "Record")
            {
                Current_Content.Text = CurrentItem.Content;
                Terminate_Button2.Visibility = Visibility.Visible;
                Terminate_Button2.IsEnabled = true;
                StartRecord(WavName);
            }
            else if (Record_Button.Content.ToString() == "Stop")
            {
                Record_Button.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                Record_Button.Content = "Record";
                mciSendString("stop movie", "", 0, 0);
                

                string audio_filename = SavePath + @"\audio\" + WavName;
                mciSendString("save movie " + audio_filename, "", 0, 0);
                mciSendString("close movie", "", 0, 0);

                CurrentItem.Done = "Yes";

                Update_Progress();

                // Bind Info List as itemsource of the table
                lstContent.ItemsSource = info;

                // Set view for filtering
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lstContent.ItemsSource);

                // Set Filter for filtering on the fly
                view.Filter = UserFilter;

                Info NextItem = null;
                if (lstContent.SelectedItems.IndexOf(CurrentItem) + 1 != lstContent.SelectedItems.Count)
                {
                    NextItem = (Info)lstContent.SelectedItems[lstContent.SelectedItems.IndexOf(CurrentItem) + 1];
                    if (CurrentItem != null && NextItem != null)
                        RecordNextAudio(CurrentItem, NextItem);
                }
                else
                {
                    CurrentItem = null;
                    Terminate_Button2.Visibility = Visibility.Collapsed;
                    MessageBox.Show("Record Ended.");
                }
                    
            }
            Play_Button.IsEnabled = true;
        }

        /// <summary>
        /// When audio is finished, refresh play button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AudioPlayer_MediaEnded(object sender, RoutedEventArgs e)
        {
            // Retrive current audio's ID
            string path = this.AudioPlayer.Source.LocalPath;
            string file_Name = System.IO.Path.GetFileName(path);
            string current_id = file_Name.Substring(0, (file_Name.Length - 4));

            string next_id = Next_Playable_ID(current_id);
            string next_audio_filename = ID_To_FileName(next_id);
            AudioPlayer.Source = new Uri(next_audio_filename);
            Current_Content.Text = info[Int32.Parse(next_id) - 1].Content;
        }

        /// <summary>
        /// Play record for selected entry(s)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Play_Audios(object sender, RoutedEventArgs e)
        {
            if (lstContent.SelectedIndex == -1)
            {
                MessageBox.Show("You have not selected anything yet!");
            }
            else if (Play_Button.Content.ToString() == "Play")
            {

                string id = Next_Playable_ID("-1");

                if (id == "-1")
                {
                    MessageBox.Show("None of selected entries has a record.");
                }
                else
                {
                    if (AudioPlayer.Source == null)
                    {
                        string audio_filename = ID_To_FileName(id);
                        AudioPlayer.Source = new Uri(audio_filename);
                    }

                    Record_Button.IsEnabled = false;
                    Terminate_Button.Visibility = Visibility.Visible;
                    Terminate_Button.IsEnabled = true;

                    
                    AudioPlayer.LoadedBehavior = MediaState.Manual;
                    if(AudioPlayer.Source!=null)
                        AudioPlayer.Play();
                    Play_Button.Content = "Pause";
                }
            }
            else
            {
                AudioPlayer.Pause();
                Play_Button.Content = "Play";
            }
        }

        /// <summary>
        /// Stop audioplayer, unload audioplayer source
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Terminate_Play(object sender, RoutedEventArgs e)
        {
            AudioPlayer.Source = null;
            AudioPlayer.Stop();
            Play_Button.Content = "Play";
            Terminate_Button.Visibility = Visibility.Collapsed;
            Record_Button.IsEnabled = true;
        }

        /// <summary>
        /// Update complettion information, shows it in x/y format
        /// </summary>
        private void Update_Progress()
        {
            int count = 0;
            foreach (Info item in info)
            {
                if (item.Done == "Yes")
                {
                    count++;
                }
            }
            Progress.Text = "Progress: " + count.ToString() + "/" + info.Count.ToString();
        }

        /// <summary>
        /// Retrive an ID that corresponds to a playable wav file
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        private string Next_Playable_ID(string ID)
        {
            int length = lstContent.SelectedItems.Count;
            if (ID == "-1")
            {
                for (int i = 0; i < length; i++)
                {
                    Info item = (Info)lstContent.SelectedItems[i];
                    if (ID == "-1")
                    {
                        if (item.Done == "Yes")
                        {
                            return item.ID;
                        }
                    }
                }

                return "-1";
            }
            else
            {
                for (int i = 0; i < length; i++)
                {
                    Info item = (Info)lstContent.SelectedItems[i];
                    if (ID == item.ID)
                    {
                        Info next_item;
                        while (true)
                        {
                            i = (i + 1) % length;
                            next_item = (Info)lstContent.SelectedItems[i];

                            if (next_item.Done == "Yes")
                            {
                                break;
                            }
                        }
                        return next_item.ID;
                    }
                }

                return ID;
            }
        }

        /// <summary>
        /// Format an ID to a wav file name
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        private string ID_To_FileName(string ID)
        {
            string AudioFileString = string.Format(SavePath + @"\audio\{0}.wav", ID);
            return AudioFileString;
        }

        /// <summary>
        /// Rest some variables changed during record process
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Terminate_Record(object sender, RoutedEventArgs e)
        {
            CurrentItem = null;
            Record_Button.Content = "Record";
            Record_Button.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
            Terminate_Button2.Visibility = Visibility.Collapsed;
            Play_Button.IsEnabled = true;
        }

        private void lstContent_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }

    public class Info
    {
        public string ID { get; set; }
        public string Content { get; set; }
        public string Done { get; set; }
    }
}
