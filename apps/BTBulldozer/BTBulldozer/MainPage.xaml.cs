﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
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

        public enum Status
        {
            INIT,
            INVALID_ADDRESS,
            DISCONNECTED,
            CONNECTED
        };

        Status status;

        public Status CurrentStatus
        {
            get
            {
                return status;
            }

            set
            {
                switch(value)
                {
                    case Status.INIT:
                    case Status.INVALID_ADDRESS:
                        Address.IsEnabled = true;
                        Connect.Text = "Connect";
                        Connect.IsEnabled = false;
                        EnableOperations(false);
                        break;
                    case Status.DISCONNECTED:
                        Address.IsEnabled = true;
                        Connect.Text = "Connect";
                        Connect.IsEnabled = true;
                        EnableOperations(false);
                        break;
                    case Status.CONNECTED:
                        Address.IsEnabled = false;
                        Connect.Text = "Disconnect";
                        Connect.IsEnabled = true;
                        EnableOperations(true);
                        break;
                    default:
                        throw new InvalidOperationException($"Unsupported status: {status}");
                }
                status = value;
            }
        }

        public MainPage()
        {
            InitializeComponent();
            if (Application.Current.Properties.ContainsKey(ADDRESS_KEY))
            {
                Address.Text = Application.Current.Properties[ADDRESS_KEY] as string;
                CurrentStatus = Status.DISCONNECTED;
            }
            else
            {
                CurrentStatus = Status.INIT;
            }
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
                CurrentStatus = Status.INVALID_ADDRESS;
                Address.BackgroundColor = Color.Pink;
                return;
            }

            CurrentStatus = Status.DISCONNECTED;
            Address.BackgroundColor = Color.Default;
            Application.Current.Properties[ADDRESS_KEY] = Address.Text;
        }

        private async void OnConnectClicked(object sender, EventArgs e)
        {
            switch (status)
            {
                case Status.CONNECTED:
                    await OnDisconnect?.Invoke(sender, e);
                    break;
                case Status.DISCONNECTED:
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
