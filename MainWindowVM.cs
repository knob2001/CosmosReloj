using System;
using System.ComponentModel;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.Input;
using CosmosReloj.Services;

namespace CosmosReloj
{
    public partial class MainWindowVM : INotifyPropertyChanged
    {
        private DispatcherTimer _timer;
        private DispatcherTimer _timerMensaje;
        private TimeSpan _ntpOffset = TimeSpan.Zero;

        public MainWindowVM()
        {
            NombreVentana       = "COSMOS RELOJ";
            VentanaBordeColor   = "#21262D";
            MensajeEstado       = "Cosmos Reloj";
            MensajeEstadoColor  = "#8B949E";
            ActualizarHoraCommand = new RelayCommand(async () => await SincronizarHoraNtp());

            ActualizarHora();

            _timer = new DispatcherTimer(DispatcherPriority.Normal)
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _timer.Tick += (s, e) => ActualizarHora();
            _timer.Start();
        }

        private void LimpiarMensaje()
        {
            MensajeEstado = "Cosmos Reloj";
            MensajeEstadoColor = "#8B949E";
        }

        private void ProgramarLimpiarMensaje()
        {
            _timerMensaje?.Stop();
            _timerMensaje = new DispatcherTimer(DispatcherPriority.Normal)
            {
                Interval = TimeSpan.FromSeconds(8)
            };
            _timerMensaje.Tick += (s, e) =>
            {
                _timerMensaje.Stop();
                LimpiarMensaje();
            };
            _timerMensaje.Start();
        }

        private bool _parpadeoRojo;
        private DateTime Now => DateTime.Now + _ntpOffset;

        private void ActualizarHora()
        {
            var now = Now;
            HoraActual  = now.ToString("HH:mm:ss");
            FechaActual = now.ToString("dddd, dd MMMM yyyy");

            var nextHour = now.Minute == 0 && now.Second == 0
                ? now.AddHours(1)
                : new DateTime(now.Year, now.Month, now.Day, now.Hour, 0, 0).AddHours(1);
            var diffHora = nextHour - now;
            CuentaAtrasHora = $"{(int)diffHora.TotalHours:D2}:{diffHora.Minutes:D2}:{diffHora.Seconds:D2}";
            HoraObjetivoTexto = nextHour.ToString("HH:mm");

            var nextHalf = now.Minute < 30
                ? new DateTime(now.Year, now.Month, now.Day, now.Hour, 30, 0)
                : new DateTime(now.Year, now.Month, now.Day, now.Hour, 0, 0).AddHours(1);
            var diffMedia = nextHalf - now;
            CuentaAtrasMediaHora = $"{(int)diffMedia.TotalHours:D2}:{diffMedia.Minutes:D2}:{diffMedia.Seconds:D2}";
            MediaHoraObjetivoTexto = nextHalf.ToString("HH:mm");

            var menor = diffHora.TotalSeconds < diffMedia.TotalSeconds ? diffHora : diffMedia;
            if (menor.TotalSeconds <= 10)
            {
                _parpadeoRojo = !_parpadeoRojo;
                HoraActualColor = _parpadeoRojo ? "#F85149" : "#C9D1D9";
            }
            else
            {
                HoraActualColor = "#C9D1D9";
            }
        }

        private async System.Threading.Tasks.Task SincronizarHoraNtp()
        {
            MensajeEstado = "Sincronizando hora con servidor NTP…";
            MensajeEstadoColor = "#58A6FF";

            try
            {
                var ntpTime = await NtpService.GetNetworkTimeAsync();
                _ntpOffset = ntpTime - DateTime.Now;

                MensajeEstado = "Reloj Sincronizado con Servidor Online";
                MensajeEstadoColor = "#3FB950";
                ActualizarHora();
            }
            catch
            {
                _ntpOffset = TimeSpan.Zero;
                MensajeEstado = "Servidor de Hora online no disponible. Seguimos con la del sistema";
                MensajeEstadoColor = "#F85149";
            }
            finally
            {
                ProgramarLimpiarMensaje();
            }
        }
    }
}
