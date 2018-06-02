using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BTBulldozer
{
    public partial class MainPage : ContentPage
    {
        public const string SPP_UUID = "00001101-0000-1000-8000-00805F9B34FB";
        public const string ADDRESS_KEY = "Address";

        public event Func<object, EventArgs, string, Task> OnConnect;
        public event Func<object, EventArgs, Task> OnDisconnect;
        public event Func<object, EventArgs, Task> OnForward;
        public event Func<object, EventArgs, Task> OnBack;
        public event Func<object, EventArgs, Task> OnRight;
        public event Func<object, EventArgs, Task> OnLeft;
        public event Func<object, EventArgs, Task> OnStop;
        public event Func<object, EventArgs, Task> OnUp;
        public event Func<object, EventArgs, Task> OnDown;
        public event Func<object, EventArgs, Task> OnKeep;

        public enum State
        {
            INIT,
            INVALID_ADDRESS,
            DISCONNECTED,
            CONNECTED
        }

        public struct Status
        {
            public int Voltage { get; set; }
        }

        State state;

        public State CurrentState
        {
            get
            {
                return state;
            }

            set
            {
                switch(value)
                {
                    case State.INIT:
                    case State.INVALID_ADDRESS:
                        Address.IsEnabled = true;
                        Connect.Text = "Connect";
                        Connect.IsEnabled = false;
                        EnableOperations(false);
                        break;
                    case State.DISCONNECTED:
                        Address.IsEnabled = true;
                        Connect.Text = "Connect";
                        Connect.IsEnabled = true;
                        EnableOperations(false);
                        break;
                    case State.CONNECTED:
                        Address.IsEnabled = false;
                        Connect.Text = "Disconnect";
                        Connect.IsEnabled = true;
                        EnableOperations(true);
                        break;
                    default:
                        throw new InvalidOperationException($"Unsupported state: {state}");
                }
                state = value;
            }
        }

        public MainPage()
        {
            InitializeComponent();
            if (Application.Current.Properties.ContainsKey(ADDRESS_KEY))
            {
                Address.Text = Application.Current.Properties[ADDRESS_KEY] as string;
                CurrentState = State.DISCONNECTED;
            }
            else
            {
                CurrentState = State.INIT;
            }
        }

        public void OnStatusUpdate(Status status)
        {
            var voltage = 3.3 * (status.Voltage / (double) Int16.MaxValue);
            StatusLabel.Text = $"Voltage: {voltage} V";
        }

        private void EnableOperations(bool isEnabled)
        {
            Forward.IsEnabled = isEnabled;
            Back.IsEnabled = isEnabled;
            Right.IsEnabled = isEnabled;
            Left.IsEnabled = isEnabled;
            Stop.IsEnabled = isEnabled;
            Up.IsEnabled = isEnabled;
            Down.IsEnabled = isEnabled;
            Keep.IsEnabled = isEnabled;
        }

        private void OnAddressChanged(object sender, EventArgs e)
        {
            if (!Regex.IsMatch(Address.Text, "^([0-9a-fA-F]{2}[:]){5}[0-9a-fA-F]{2}"))
            {
                CurrentState = State.INVALID_ADDRESS;
                Address.BackgroundColor = Color.Pink;
                return;
            }

            CurrentState = State.DISCONNECTED;
            Address.BackgroundColor = Color.Default;
            Application.Current.Properties[ADDRESS_KEY] = Address.Text;
        }

        private async void OnConnectClicked(object sender, EventArgs e)
        {
            switch (state)
            {
                case State.CONNECTED:
                    await OnDisconnect?.Invoke(sender, e);
                    break;
                case State.DISCONNECTED:
                    await OnConnect?.Invoke(sender, e, Address.Text);
                    break;
            }
        }

        private async void OnForwardClicked(object sender, EventArgs e)
        {
            await OnForward?.Invoke(sender, e);
        }

        private async void OnBackClicked(object sender, EventArgs e)
        {
            await OnBack?.Invoke(sender, e);
        }

        private async void OnRightClicked(object sender, EventArgs e)
        {
            await OnRight?.Invoke(sender, e);
        }

        private async void OnLeftClicked(object sender, EventArgs e)
        {
            await OnLeft?.Invoke(sender, e);
        }

        private async void OnStopClicked(object sender, EventArgs e)
        {
            await OnStop?.Invoke(sender, e);
        }

        private async void OnUpClicked(object sender, EventArgs e)
        {
            await OnUp?.Invoke(sender, e);
        }

        private async void OnDownClicked(object sender, EventArgs e)
        {
            await OnDown?.Invoke(sender, e);
        }

        private async void OnKeepClicked(object sender, EventArgs e)
        {
            await OnKeep?.Invoke(sender, e);
        }
    }
}
