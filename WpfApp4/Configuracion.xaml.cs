using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using static WpfApp4.ConfiguracionRutas;


namespace WpfApp4
{
    public partial class Configuracion : Window
    {
        public Configuracion()
        {
            InitializeComponent();

            TextoRutaPiezas.Text = Local.RutaPiezas;
            TextoRutaUrgentes.Text = Local.RutaUrgentes;

        }

        private void EnterDirectorioUrgentes(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                AbrirDirectorioUrgentesBoton.Focus();
        }

        public void AbrirDirectorioPiezasClick(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFolderDialog
            {
                Title = "selecciona la ruta",
                InitialDirectory = ConfiguracionRutas.Local.RutaPiezas ?? "C:\\"
            };
            if (dialog.ShowDialog() == true)
            {
                string RutaSeleccionada = dialog.FolderName;
                ConfiguracionRutas.Local.RutaPiezas = RutaSeleccionada;
                TextoRutaPiezas.Text = RutaSeleccionada;
            }
        }

        private void EnterDirectorioPiezas(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)

            {
                AbrirDirectorioPiezasBoton.Focus();
            }
        }

        private void BotonGuardarDirectoriosClick(object sender, RoutedEventArgs e)
        {
            Local.RutaPiezas = TextoRutaPiezas.Text;
            Local.RutaUrgentes = TextoRutaUrgentes.Text;

            GuardarConfiguracion();
            this.Close();
        }

        private void AbrirDirecorioPiezasUrgentesBoton(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFolderDialog
            {
                Title = "selecciona la ruta",
                InitialDirectory = ConfiguracionRutas.Local.RutaUrgentes ?? "C:\\"
            };
            if (dialog.ShowDialog() == true)
            {
                string RutaSeleccionada = dialog.FolderName;
                ConfiguracionRutas.Local.RutaUrgentes = RutaSeleccionada;
                TextoRutaUrgentes.Text = RutaSeleccionada;
            }
        }
    }
}

