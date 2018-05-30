using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Bluetooth;
using Android.Content;
using Java.Util;
using System.Threading.Tasks;

namespace BTBulldozer.Droid
{
    [Activity(Label = "BTBulldozer", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        MainPage mainPage;
        BluetoothSocket socket;

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);

            var app = new App();
            mainPage = app.MainPage as MainPage;

            mainPage.OnConnect += async (sender, e, address) =>
            {
                await Connect(address);
            };

            mainPage.OnDisconnect += async (sender, e) =>
            {
                await Disconnect();
            };

            mainPage.OnForward += async (sender, e) =>
            {
                if (! socket.IsConnected)
                {
                    return;
                }

                await socket.OutputStream.WriteAsync(Command.FORWARD, 0, 1);
            };

            mainPage.OnBack += async (sender, e) =>
            {
                if (! socket.IsConnected)
                {
                    return;
                }

                await socket.OutputStream.WriteAsync(Command.BACK, 0, 1);
            };

            mainPage.OnRight += async (sender, e) =>
            {
                if (! socket.IsConnected)
                {
                    return;
                }

                await socket.OutputStream.WriteAsync(Command.RIGHT, 0, 1);
            };

            mainPage.OnLeft += async (sender, e) =>
            {
                if (! socket.IsConnected)
                {
                    return;
                }

                await  socket.OutputStream.WriteAsync(Command.LEFT, 0, 1);
            };

            mainPage.OnStop += async (sender, e) =>
            {
                if (! socket.IsConnected)
                {
                    return;
                }

                await socket.OutputStream.WriteAsync(Command.STOP, 0, 1);
            };

            mainPage.OnUp += async (sender, e) =>
            {
                if (! socket.IsConnected)
                {
                    return;
                }

                await socket.OutputStream.WriteAsync(Command.UP, 0, 1);
            };

            mainPage.OnDown += async (sender, e) =>
            {
                if (! socket.IsConnected)
                {
                    return;
                }

                await socket.OutputStream.WriteAsync(Command.DOWN, 0, 1);
            };

            mainPage.OnKeep += async (sender, e) =>
            {
                if (! socket.IsConnected)
                {
                    return;
                }

                await socket.OutputStream.WriteAsync(Command.KEEP, 0, 1);
            };


            LoadApplication(app);
        }

        async Task Connect(string address)
        {
            // TODO: Add error handling

            var adapter = BluetoothAdapter.DefaultAdapter;
            if (!adapter.IsEnabled)
            {
                var intent = new Intent(BluetoothAdapter.ActionRequestEnable);
                StartActivityForResult(intent, 1);
            }

            var device = adapter.GetRemoteDevice(address.ToUpper());

            socket = device.CreateRfcommSocketToServiceRecord(UUID.FromString(MainPage.SPP_UUID));
            await socket.ConnectAsync();

            ChangeStatus();
        }

        async Task Disconnect()
        {
            socket?.Close();
            ChangeStatus();
            await Task.FromResult(0);
        }

        void ChangeStatus()
        {
            if (socket.IsConnected)
            {
                mainPage.CurrentStatus = MainPage.Status.CONNECTED;
            }
            else
            {
                mainPage.CurrentStatus = MainPage.Status.DISCONNECTED;
            }
        }
    }
}

