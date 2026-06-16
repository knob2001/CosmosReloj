using System.ComponentModel;

namespace CosmosReloj
{
    partial class MainWindowVM : INotifyPropertyChanged
    {
        private string _nombreVentana;
        public string NombreVentana
        {
            get => _nombreVentana;
            set { if (_nombreVentana != value) { _nombreVentana = value; OnPropertyChanged(); } }
        }

        private string _ventanaBordeColor;
        public string VentanaBordeColor
        {
            get => _ventanaBordeColor;
            set { if (_ventanaBordeColor != value) { _ventanaBordeColor = value; OnPropertyChanged(); } }
        }

        private string _mensajeEstado = "Listo.";
        public string MensajeEstado
        {
            get => _mensajeEstado;
            set { _mensajeEstado = value; OnPropertyChanged(); }
        }

        private string _mensajeEstadoColor = "#8B949E";
        public string MensajeEstadoColor
        {
            get => _mensajeEstadoColor;
            set { _mensajeEstadoColor = value; OnPropertyChanged(); }
        }

        private string _horaActual;
        public string HoraActual
        {
            get => _horaActual;
            set { _horaActual = value; OnPropertyChanged(); }
        }

        private string _fechaActual;
        public string FechaActual
        {
            get => _fechaActual;
            set { _fechaActual = value; OnPropertyChanged(); }
        }

        private string _cuentaAtrasHora;
        public string CuentaAtrasHora
        {
            get => _cuentaAtrasHora;
            set { _cuentaAtrasHora = value; OnPropertyChanged(); }
        }

        private string _cuentaAtrasMediaHora;
        public string CuentaAtrasMediaHora
        {
            get => _cuentaAtrasMediaHora;
            set { _cuentaAtrasMediaHora = value; OnPropertyChanged(); }
        }

        private string _horaObjetivoTexto;
        public string HoraObjetivoTexto
        {
            get => _horaObjetivoTexto;
            set { _horaObjetivoTexto = value; OnPropertyChanged(); }
        }

        private string _mediaHoraObjetivoTexto;
        public string MediaHoraObjetivoTexto
        {
            get => _mediaHoraObjetivoTexto;
            set { _mediaHoraObjetivoTexto = value; OnPropertyChanged(); }
        }

        private bool _actualizarEnabled = true;
        public bool ActualizarEnabled
        {
            get => _actualizarEnabled;
            set { _actualizarEnabled = value; OnPropertyChanged(); }
        }

        private string _horaActualColor = "#C9D1D9";
        public string HoraActualColor
        {
            get => _horaActualColor;
            set { _horaActualColor = value; OnPropertyChanged(); }
        }
    }
}
