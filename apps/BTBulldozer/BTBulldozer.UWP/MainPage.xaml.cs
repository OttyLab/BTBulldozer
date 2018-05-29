using System;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.Rfcomm;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;

namespace BTBulldozer.UWP
{
    public sealed partial class MainPage
    {
        BTBulldozer.MainPage mainPage;
        StreamSocket socket;

        public MainPage()
        {
            this.InitializeComponent();

            var app = new BTBulldozer.App();
            mainPage  = app.MainPage as BTBulldozer.MainPage;

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
                if (socket is null)
                {
                    return;
                }

                var writer = new DataWriter(socket.OutputStream);
                writer.WriteBytes(Command.FORWARD);
                await writer.StoreAsync();
            };

            mainPage.OnBack += async (sender, e) =>
            {
                if (socket is null)
                {
                    return;
                }

                var writer = new DataWriter(socket.OutputStream);
                writer.WriteBytes(Command.BACK);
                await writer.StoreAsync();
            };

            mainPage.OnRight += async (sender, e) =>
            {
                if (socket is null)
                {
                    return;
                }

                var writer = new DataWriter(socket.OutputStream);
                writer.WriteBytes(Command.RIGHT);
                await writer.StoreAsync();
            };

            mainPage.OnLeft += async (sender, e) =>
            {
                if (socket is null)
                {
                    return;
                }

                var writer = new DataWriter(socket.OutputStream);
                writer.WriteBytes(Command.LEFT);
                await writer.StoreAsync();
            };

            mainPage.OnStop += async (sender, e) =>
            {
                if (socket is null)
                {
                    return;
                }

                var writer = new DataWriter(socket.OutputStream);
                writer.WriteBytes(Command.STOP);
                await writer.StoreAsync();
            };

            mainPage.OnUp += async (sender, e) =>
            {
                if (socket is null)
                {
                    return;
                }

                var writer = new DataWriter(socket.OutputStream);
                writer.WriteBytes(Command.UP);
                await writer.StoreAsync();
            };

            mainPage.OnDown += async (sender, e) =>
            {
                if (socket is null)
                {
                    return;
                }

                var writer = new DataWriter(socket.OutputStream);
                writer.WriteBytes(Command.DOWN);
                await writer.StoreAsync();
            };

            mainPage.OnKeep += async (sender, e) =>
            {
                if (socket is null)
                {
                    return;
                }

                var writer = new DataWriter(socket.OutputStream);
                writer.WriteBytes(Command.KEEP);
                await writer.StoreAsync();
            };

            LoadApplication(app);
        }

        async Task Connect(string address)
        {
            // TODO: Add error handling
            var rawAddress = Convert.ToUInt64(address.Replace(":", ""), 16);
            var device = await BluetoothDevice.FromBluetoothAddressAsync(rawAddress);
            var service = await device.GetRfcommServicesForIdAsync(RfcommServiceId.FromUuid(Guid.Parse(BTBulldozer.MainPage.SPP_UUID)));
            var spp = service.Services[0];
            socket = new StreamSocket();
            await socket.ConnectAsync(spp.ConnectionHostName, spp.ConnectionServiceName);

            ChangeStatus();
        }

        async Task Disconnect()
        {
            socket?.Dispose();
            socket = null;
            ChangeStatus();
            await Task.FromResult(0);
        }

        void ChangeStatus()
        {
            if (socket is null)
            {
                mainPage.ChangeStatus(BTBulldozer.MainPage.Status.DISCONNECTED);
            }
            else
            {
                mainPage.ChangeStatus(BTBulldozer.MainPage.Status.CONNECTED);
            }
        }
    }
}
