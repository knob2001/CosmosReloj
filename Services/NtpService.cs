using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace CosmosReloj.Services
{
    public static class NtpService
    {
        public static async Task<DateTime> GetNetworkTimeAsync(string ntpServer = "pool.ntp.org")
        {
            const int ntpPort = 123;

            var addresses = await Dns.GetHostAddressesAsync(ntpServer);
            if (addresses.Length == 0)
                throw new Exception("No se pudo resolver el servidor NTP");

            var ntpData = new byte[48];
            ntpData[0] = 0x1B;

            using var socket = new Socket(SocketType.Dgram, ProtocolType.Udp);

            if (await TaskCuandoAny(socket.ConnectAsync(addresses[0], ntpPort), 4000))
                throw new Exception("Tiempo de espera al conectar con servidor NTP");

            if (await TaskCuandoAny(socket.SendAsync(new ArraySegment<byte>(ntpData), SocketFlags.None), 4000))
                throw new Exception("Tiempo de espera al enviar petición NTP");

            var buffer = new byte[48];
            var receiveTask = socket.ReceiveAsync(new ArraySegment<byte>(buffer), SocketFlags.None);
            if (await TaskCuandoAny(receiveTask, 4000))
                throw new Exception("Tiempo de espera al recibir respuesta NTP");

            int received = await receiveTask;
            if (received < 48)
                throw new Exception("Respuesta NTP incompleta");

            ulong intPart = (ulong)buffer[40] << 24 | (ulong)buffer[41] << 16 | (ulong)buffer[42] << 8 | buffer[43];
            ulong fractPart = (ulong)buffer[44] << 24 | (ulong)buffer[45] << 16 | (ulong)buffer[46] << 8 | buffer[47];

            var milliseconds = (intPart * 1000) + (fractPart * 1000 / 0x100000000L);
            var ntpEpoch = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            return ntpEpoch.AddMilliseconds((long)milliseconds).ToLocalTime();
        }

        private static async Task<bool> TaskCuandoAny(Task tarea, int timeoutMs)
        {
            if (await Task.WhenAny(tarea, Task.Delay(timeoutMs)) == Task.Delay(timeoutMs))
                return true;
            await tarea; // propagar excepción si la hubo
            return false;
        }
    }
}
