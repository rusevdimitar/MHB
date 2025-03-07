using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using MHB.Gadgets.Commands;
using MHB.Gadgets.MhbApiService;

namespace MHB.Gadgets.ActionLogMonitor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        #region Constants

        private const int DEFAULT_TIMER_INTERVAL = 10;
        private const string EXCEPTION_NO_DATA_DEFAULT_MESSAGE = "[No Data]";

        #endregion Constants

        #region Fields

        private BackgroundWorker _worker = new BackgroundWorker();

        private string _userName = string.Empty;
        private string _password = string.Empty;
        private string _apiKey = string.Empty;
        private bool _showDistinctUsers = false;

        private DateTime? _startDate;

        private DispatcherTimer _timer = new DispatcherTimer();

        private List<ActionLog> _logs = new List<ActionLog>();

        private List<ExceptionLog> _exceptions = new List<ExceptionLog>();

        #endregion Fields

        #region Properties

        private ObservableCollection<ActionLog> _actionLogs = new ObservableCollection<ActionLog>();

        public ObservableCollection<ActionLog> ActionLogs
        {
            get
            {
                return this._actionLogs;
            }
            set
            {
                this._actionLogs = value;
            }
        }

        private ObservableCollection<ExceptionLog> _exceptionLogs = new ObservableCollection<ExceptionLog>();

        public ObservableCollection<ExceptionLog> ExceptionLogs
        {
            get
            {
                return this._exceptionLogs;
            }
            set
            {
                this._exceptionLogs = value;
            }
        }

        private ICommand _blockUserCommand;

        public ICommand BlockUserCommand
        {
            get
            {
                return this._blockUserCommand;
            }
            set
            {
                this._blockUserCommand = value;
            }
        }

        public bool ShowDistinctUsers
        {
            get
            {
                return this._showDistinctUsers;
            }
            set
            {
                this._showDistinctUsers = value;
            }
        }

        #endregion Properties

        public MainWindow()
        {
            InitializeComponent();

            this._worker.DoWork += new DoWorkEventHandler(this._worker_DoWork);
            this._worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this._worker_RunWorkerCompleted);

            this._timer.Tick += new EventHandler(this._timer_Tick);
            this._timer.Interval = new TimeSpan(0, 0, this.GetTimerInterval());

            dateStartDate.SelectedDate = DateTime.Today;

            this.DataContext = this;

            this._blockUserCommand = new ActionCommand((id) =>
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    MhbApiServiceClient client = new MhbApiServiceClient();

                    var selectedActionLog = this.ActionLogs.Where(a => a.ID == (int)id).FirstOrDefault();

                    string ip = selectedActionLog.IP;

                    int userID = selectedActionLog.UserID;

                    client.BanIP(this._userName, this._password, this._apiKey, ip, string.Empty, userID);

                    MessageBox.Show(string.Format("IP address: {0} has been added successfully to our black list", ip), "Blacklisted", MessageBoxButton.OK, MessageBoxImage.Information);
                }));
            });
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            this._startDate.Value.AddSeconds(-this.GetTimerInterval());

            if (!this._worker.IsBusy)
                this._worker.RunWorkerAsync();
        }

        private void _worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            progressBarStatus.IsIndeterminate = false;

            comboBoxHistoryActions.ItemsSource = this.ActionLogs.ToList().GroupBy(l => l.Action).Select(a => a.First().Action);
            comboBoxHistoryActions.ItemsSource = this.ActionLogs.ToList().GroupBy(l => l.Action).Select(a => a.First().Action);
            comboBoxUsers.ItemsSource = this.ActionLogs.ToList().GroupBy(l => l.UserEmail).Select(a => a.First().UserEmail);
            comboBoxIP.ItemsSource = this.ActionLogs.ToList().GroupBy(l => l.IP).Select(a => a.First().IP);
            comboBoxHistoryActions.ItemsSource = this.ActionLogs.ToList().GroupBy(l => l.Action).Select(a => a.First().Action);

            this._timer.Start();

            visitorsMap.LoadCities();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                textBoxUserName.Text = ConfigurationManager.AppSettings["UserName"];
                textBoxPassword.Password = ConfigurationManager.AppSettings["Password"];
                textBoxAPIKey.Text = ConfigurationManager.AppSettings["APIKey"];
            }
            catch (Exception ex)
            {
                this.ShowErrorMessageBox(ex, "Grid_Loaded");
            }
        }

        private void ButtonGetActionLogs_Click(object sender, RoutedEventArgs e)
        {
            this._userName = textBoxUserName.Text;
            this._password = textBoxPassword.Password;
            this._apiKey = textBoxAPIKey.Text;

            if (!this._worker.IsBusy)
            {
                progressBarStatus.IsIndeterminate = true;

                this._worker.RunWorkerAsync();
            }
        }

        MhbApiServiceClient _client = new MhbApiServiceClient();

        private void _worker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                ActionLog[] newActionLogs = this._client.GetActionLogs(this._userName, this._password, this._apiKey, this._startDate.HasValue ? this._startDate.Value : DateTime.Today);

                ExceptionLog[] exceptionLogs = this._client.GetExceptionLogs(this._userName, this._password, this._apiKey, this._startDate.HasValue ? this._startDate.Value : DateTime.Today);

                if (newActionLogs.Count() == 0 && exceptionLogs.Count() == 0) return;

                if (newActionLogs.Any())
                    this._startDate = newActionLogs.Max(al => al.LogDate).AddSeconds(1);
                else if (exceptionLogs.Any())
                    this._startDate = exceptionLogs.Max(el => el.LogDate).AddSeconds(1);              

                Dispatcher.Invoke(new Action(() =>
                    {
                        // Action logs
                        ObservableCollection<ActionLog> logs = new ObservableCollection<ActionLog>(this.ActionLogs);

                        this.ActionLogs.Clear();

                        logs.AddRange(newActionLogs.GroupBy(alog => alog.ID).Select(grp => grp.First()));

                        this.ActionLogs.AddRange(logs.OrderByDescending(al => al.LogDate));

                        this._logs = this.ActionLogs.ToList();

                        // Exception logs
                        ObservableCollection<ExceptionLog> exceptions = new ObservableCollection<ExceptionLog>(this.ExceptionLogs);

                        this.ExceptionLogs.Clear();

                        exceptions.AddRange(exceptionLogs);

                        this.ExceptionLogs.AddRange(exceptions.OrderByDescending(ex => ex.LogDate));

                        this._exceptions = this.ExceptionLogs.ToList();
                    }));
            }
            catch (Exception ex)
            {
                this.ShowErrorMessageBox(ex, "_worker_DoWork");
            }
        }

        private void ButtonGetDistinctUsers_Click(object sender, RoutedEventArgs e)
        {
            if (this.ActionLogs != null && this.ActionLogs.Count > 0)
            {
                this._timer.Stop();

                ObservableCollection<ActionLog> logs = new ObservableCollection<ActionLog>(this._actionLogs.ToList().Where(a => a.UserID != 0).OrderByDescending(a => a.Action == LoggerHistoryAction.Login).ThenBy(a => a.LogDate).GroupBy(l => l.UserEmail).Select(log => log.First()));

                this.ActionLogs.Clear();

                this.ActionLogs.AddRange(logs.OrderByDescending(al => al.LogDate));

                this._showDistinctUsers = true;
            }
        }

        private void comboBoxHistoryActions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                this._timer.Stop();

                LoggerHistoryAction action = (LoggerHistoryAction)e.AddedItems[0];

                ObservableCollection<ActionLog> logs = new ObservableCollection<ActionLog>(this._actionLogs.ToList().Where(a => a.Action == action));

                this.ActionLogs.Clear();

                this.ActionLogs.AddRange(logs.OrderByDescending(al => al.LogDate));
            }
        }

        private void comboBoxUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                this._timer.Stop();

                ObservableCollection<ActionLog> logs = new ObservableCollection<ActionLog>(this._actionLogs.ToList().Where(a => a.UserEmail == e.AddedItems[0].ToString()));

                this.ActionLogs.Clear();

                this.ActionLogs.AddRange(logs.OrderByDescending(al => al.LogDate));
            }
        }

        private void ButtonShowAll_Click(object sender, RoutedEventArgs e)
        {
            this._timer.Start();

            // Action logs
            this.ActionLogs.Clear();

            this.ActionLogs.AddRange(this._logs);

            // Exception logs
            this.ExceptionLogs.Clear();

            this.ExceptionLogs.AddRange(this._exceptions);

            this.ResetComboBoxes();

            this._showDistinctUsers = false;
        }

        private void comboBoxIP_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                this._timer.Stop();

                ObservableCollection<ActionLog> logs = new ObservableCollection<ActionLog>(this._actionLogs.ToList().Where(a => a.IP == e.AddedItems[0].ToString()));

                this.ActionLogs.Clear();

                this.ActionLogs.AddRange(logs.OrderByDescending(al => al.LogDate));
            }
        }

        private void ResetComboBoxes()
        {
            foreach (ComboBox cb in Helper.FindVisualChildren<ComboBox>(this))
            {
                cb.SelectedIndex = -1;
            }
        }

        private int GetTimerInterval()
        {
            int interval = DEFAULT_TIMER_INTERVAL;

            try
            {
                interval = int.Parse(ConfigurationManager.AppSettings["TimerInterval"]);
            }
            catch (Exception ex)
            {
                this.ShowErrorMessageBox(ex, "GetTimerInterval", string.Format("Default refresh interval was set to {0} seconds.", DEFAULT_TIMER_INTERVAL));
            }

            return interval;
        }

        private void ShowErrorMessageBox(Exception ex, string methodName, string details = "")
        {
            try
            {
                string exceptionMessage = string.IsNullOrEmpty(ex.Message) ? EXCEPTION_NO_DATA_DEFAULT_MESSAGE : ex.Message;
                string exceptionInnerMessage = ex.InnerException == null || string.IsNullOrEmpty(ex.InnerException.Message) ? EXCEPTION_NO_DATA_DEFAULT_MESSAGE : ex.InnerException.Message;
                string exceptionDetails = string.IsNullOrEmpty(details) ? EXCEPTION_NO_DATA_DEFAULT_MESSAGE : details;

                MessageBox.Show(string.Format("Message:\n{0}\n\nInnner Exception:\n{1}\n\nOther details:\n{2}", exceptionMessage, exceptionInnerMessage, exceptionDetails), string.Format("[{0}] has thrown a {1}", methodName, ex.GetType()), MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception noWay)
            {
                MessageBox.Show(noWay.Message);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        // Create the OnPropertyChanged method to raise the event
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        private void dateStartDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            this._startDate = (sender as DatePicker).SelectedDate;

            this.ActionLogs.Clear();
        }
    }
}