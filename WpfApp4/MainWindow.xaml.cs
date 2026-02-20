using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Globalization;
using System.IO;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WpfApp4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {

        public ObservableCollection<Pieza> lista { get; set; } = new ObservableCollection<Pieza>();
        public Pieza NuevaPieza { get; set; } = new Pieza();

        public Pieza PiezaOriginal;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
            this.DataContext = NuevaPieza;
            //texto_ancho.DataContext = PiezaOriginal;
            listapezas.ItemsSource = lista;
            ConfiguracionRutas.CargarConfiguracion();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double nuevoTamano = e.NewSize.Width / 45;

            if (nuevoTamano < 12) nuevoTamano = 12;
            if (nuevoTamano > 60) nuevoTamano = 60;

            if (listapezas != null)
            {
                listapezas.FontSize = nuevoTamano;
            }
        }
        public void CargarLista()
        {
            var dir = ConfiguracionRutas.Local.RutaPiezas;
            var fecha = DateTime.Now.ToString("yyyy_MM_dd");
            var nombrearchivo = $"Registro Piezas_{fecha}.txt";
            var path = System.IO.Path.Combine(dir, nombrearchivo);

            lista.Clear();
            if (File.Exists(path))
            {
                try
                {
                    string texto = File.ReadAllText(path);
                    var datosCargados = JsonSerializer.Deserialize<List<Pieza>>(texto);

                    if (datosCargados != null)
                    {
                        foreach (var p in datosCargados)
                        {
                            lista.Add(p);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Archivo corrupto: {ex.Message}");
                }
            }
            PanelContadorUrgentes();
            PanelContadorTotalPezas();
        }
        public void GuardarLista()
        {
            var dir = ConfiguracionRutas.Local.RutaPiezas;

            if (string.IsNullOrEmpty(dir) || !Directory.Exists(dir))
            {
                MessageBox.Show("Selecciona una ruta de guardado válida en Configuración.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var path = System.IO.Path.Combine(dir, $"Registro Piezas_{DateTime.Now:yyyy_MM_dd}.txt");

            try
            {
                string json = JsonSerializer.Serialize(lista);
                File.WriteAllText(path, json);

                MessageBox.Show("Guardado con éxito.", "OK", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudo guardar: {ex.Message}");
            }
        }
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            botoneditar.Visibility = Visibility.Hidden;
            botonguardarcambios.Visibility = Visibility.Hidden;
            botoncancelar.Visibility = Visibility.Hidden;
            eliminarpeza.Visibility = Visibility.Visible;
            texto_nombre.Focus();
            engadirpeza.IsEnabled = false;
            advertencia.Visibility = Visibility.Hidden;

            CargarLista();
        }

        private void Listapezas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listapezas.SelectedItem != null)
            {
                botoneditar.Visibility = Visibility.Visible;
                engadirpeza.Visibility = Visibility.Hidden;
            }
            else
            {
                botoneditar.Visibility = Visibility.Hidden;
                engadirpeza.Visibility = Visibility.Visible;
            }
        }

        public void BotonEditarClick(object sender, RoutedEventArgs e)
        {
            if (listapezas.SelectedItem is Pieza seleccionada)
            {
                PiezaOriginal = seleccionada;
                this.DataContext = seleccionada.Clonar();
            }

            botoncancelar.Visibility = Visibility.Visible;
            botonguardarcambios.Visibility = Visibility.Visible;
        }
        public void botonborrar(object sender, RoutedEventArgs e)
        {
            if (listapezas.SelectedItem is Pieza p)
            {
                lista.Remove(p);
                PanelContadorUrgentes();
                PanelContadorTotalPezas();
            }
        }

        //  public string compruebanumero(TextBox t, string textoSiError)
        //{
        //     if (!int.TryParse(t.Text, out _))
        //      {
        //          return textoSiError;
        //      }
        //      return "";
        //  }

        // public string compruebatexto(TextBox r, string textoSiError)
        // {
        //     if (!string.IsNullOrEmpty(r.Text))
        //     {
        //         return "";
        //     }
        //     return textoSiError;
        // }

        public bool comprobamedidas(int largo, int ancho)
        {
            if (largo < 100)
            {
                texto_largo.Focus();
                texto_largo.SelectAll();
                return false;
            }
            if (ancho < 100)
            {
                texto_ancho.Focus();
                texto_ancho.SelectAll();
                return false;
            }
            return true;
        }

        //  public void comprobatodososcampos()
        //  {
        //      var errores = "";
        //      errores = compruebatexto(texto_color, "Color ");
        //      errores += compruebanumero(texto_ancho, "Ancho ");
        //      errores += compruebanumero(texto_largo, "Largo ");
        //      errores += compruebatexto(texto_nombre, "Nombre ");

        //      engadirpeza.IsEnabled = errores == "";

        //      if (errores != "")
        //      {
        //          advertencia.Visibility = Visibility.Visible;
        //          advertencia.Text = $"Introduce el {errores} de la pieza";
        //     }
        //      else
        //      {
        //          advertencia.Visibility = Visibility.Hidden;
        //          advertencia.Text = "";
        //      }
        //  }
        //var respuesta = MessageBox.Show("MensajeTexto", "titulo", MessageBoxButton.YesNo, MessageBoxImage.Warning);

        public void botonengadir(object sender, RoutedEventArgs e)
        {
            if (this.DataContext is Pieza piezaEnPantalla)
            {
                if (!int.TryParse(CantidadDePezas.Text, out int cantidad)) cantidad = 1;

                if (comprobamedidas(int.Parse(piezaEnPantalla.largo), int.Parse(piezaEnPantalla.ancho)))
                {
                    for (int i = 0; i < cantidad; i++)
                    {
                        lista.Add(piezaEnPantalla.Clonar());
                    }
                    NuevaPieza = new Pieza();
                    this.DataContext = NuevaPieza;
                    FuncionEngadirPeza();
                }
                else
                {
                    advertencia.Visibility = Visibility.Visible;
                    advertencia.Text = "Esta pieza no cuenta con el tamaño mínimo (100x100cm)";
                }
            }
        }

        //       private void texto_nombre_TextChanged(object sender, TextChangedEventArgs e)
        //        {
        //            comprobatodososcampos();
        //        }
        //        private void texto_ancho_TextChanged(object sender, TextChangedEventArgs e)
        //        {
        //            comprobatodososcampos();
        //        }
        //        private void texto_largo_TextChanged(object sender, TextChangedEventArgs e)
        //        {
        //            comprobatodososcampos();
        //        }
        //        private void texto_color_TextChanged(object sender, TextChangedEventArgs e)
        //        {
        //            comprobatodososcampos();
        //        }
        //        private void nombre_TextChanged(object sender, TextChangedEventArgs e)
        //        {
        //            comprobatodososcampos();
        //        }

        private void pulsarteclaenter(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                botonengadir(sender, e);
                PanelContadorUrgentes();
                PanelContadorTotalPezas();
            }
        }
        public void eliminartodososcampos(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                FuncionEngadirPeza();
            }
        }
        private void enternombre(object sender, KeyEventArgs e)
       {
            if (e.Key == Key.Enter)
            {
                texto_color.Focus();
            }
        }
        private void entercolor(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                texto_largo.Focus();
            }
            if (e.Key == Key.Escape)
            {
                texto_nombre.Focus();
            }
        }
        private void enterlargo(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                texto_ancho.Focus();
            }
            if (e.Key == Key.Escape)
            {
                texto_color.Focus();
            }
        }
        private void enterancho(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                engadirpeza.Focus();
            }

            if (e.Key == Key.Escape)
            {
                texto_largo.Focus();
            }
        }
        private void botonguardar(object sender, RoutedEventArgs e)
        {
            GuardarLista();
        }
        private void eliminartextos(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)

            {
                FuncionEngadirPeza();
            }
        }
        private void ReaparecerEngadirConClick(object sender, MouseButtonEventArgs e)
        {
            botoneditar.Visibility = Visibility.Hidden;
            engadirpeza.Visibility = Visibility.Visible;
        }
        private void BotonCancelarClick(object sender, RoutedEventArgs e)
        {
            PiezaOriginal = null;

            botoneditar.Visibility = Visibility.Hidden;
            engadirpeza.Visibility = Visibility.Visible;
            botoncancelar.Visibility = Visibility.Hidden;
            botonguardarcambios.Visibility = Visibility.Hidden;

            FuncionEngadirPeza();
        }
        private void BotonGuardarCambiosClick(object sender, RoutedEventArgs e)
        {
            //            if
            //               (listapezas.SelectedItem is Pieza p)
            //            {
            //                p.nombre = texto_nombre.Text;
            //                p.color = texto_color.Text;
            //                p.largo = Convert.ToInt32(texto_largo.Text);
            //                p.ancho = Convert.ToInt32(texto_ancho.Text);
            //                p.piezaurgente = urgentecheck.IsChecked ?? false;
            //                listapezas.Items.Refresh();
            //            }

            if (this.DataContext is Pieza borrador && PiezaOriginal != null)
            {
                string datosEnTransito = JsonSerializer.Serialize(borrador);
                var piezaActualizada = JsonSerializer.Deserialize<Pieza>(datosEnTransito);

                int index = lista.IndexOf(PiezaOriginal);
                lista[index] = piezaActualizada;

              //  PiezaOriginal.nombre = borrador.nombre;
              //  PiezaOriginal.color = borrador.color;
              //  PiezaOriginal.largo = borrador.largo;
              //  PiezaOriginal.ancho = borrador.ancho;
              //  PiezaOriginal.piezaurgente = borrador.piezaurgente;

                MessageBox.Show("Cambios aplicados correctamente a la lista.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);

                botoneditar.Visibility = Visibility.Hidden;
                engadirpeza.Visibility = Visibility.Visible;
                botoncancelar.Visibility = Visibility.Hidden;
                botonguardarcambios.Visibility = Visibility.Hidden;

                FuncionEngadirPeza();
            }
        }
        public void FuncionEngadirPeza()
        {
            //            texto_color.Text = "";
            //            texto_largo.Text = "";
            //            texto_ancho.Text = "";
            //            urgentecheck.IsChecked = false;
            //            CantidadDePezas.Text = "";

            this.DataContext = new Pieza();

          //  advertencia.Text = "";
            advertencia.Visibility = Visibility.Hidden;
            texto_nombre.Focus();
            listapezas.SelectedItem = null;
            PanelContadorUrgentes();
            PanelContadorTotalPezas();
        }
        private void BotonNotificarPiezasUrgentesClick(object sender, RoutedEventArgs e)
        {
            var PiezasUrgentes = new List<Pieza>();
            foreach (var item in listapezas.Items)
            {
                if (item is Pieza p && p.piezaurgente)
                {
                    PiezasUrgentes.Add(p);
                }
            }
            var ruta = ConfiguracionRutas.Local.RutaUrgentes;
            if (string.IsNullOrEmpty(ConfiguracionRutas.Local.RutaUrgentes) || !Directory.Exists(ConfiguracionRutas.Local.RutaUrgentes))
            {
                MessageBox.Show("Selecciona una ruta de guardado para las piezas urgentes", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var fecha = DateTime.Now.ToString("yyyy_MM_dd");
            var NombreArchivoUrgentes = $"Piezas_Urgentes_{fecha}.txt";
            var RutaCompletaUrgentes = System.IO.Path.Combine(ruta, NombreArchivoUrgentes);

            try
            {
                string json = JsonSerializer.Serialize(PiezasUrgentes);
                File.WriteAllText(RutaCompletaUrgentes, json);
                var respuesta = MessageBox.Show("La lista de urgentes se ha guardado correctamente", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                var respuesta2 = MessageBox.Show("¿Deseas abrir la carpeta donde se ha guardado el archivo?", "Abrir carpeta", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (respuesta2 == MessageBoxResult.Yes)
                {
                    System.Diagnostics.Process.Start("explorer.exe", ruta);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar la lista de piezas urgentes: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public void PanelContadorUrgentes()
        {
            int urgentes = lista.Count(p => p.piezaurgente);
            ContadorUrgentes.Text = $"Piezas Urgentes: {urgentes}";
        }
        public void PanelContadorTotalPezas()
        {
            ContadorTotalPezas.Text = $"Total Piezas: {lista.Count}";
        }
        private void BotonConfiguracionClick(object sender, RoutedEventArgs e)
        {
            var VentanaConfig = new Configuracion();
            VentanaConfig.Owner = this;
            VentanaConfig.ShowDialog();
        }
        private void BotonImportarListaClick(object sender, RoutedEventArgs e)
        {
            var buscar = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Archivos de texto (*.txt)|*.txt|Todos los archivos (*.*)|*.*",
                Title = "Seleccionar archivo de piezas"
            };
            if (buscar.ShowDialog() != true) return;

            string rutaOrigen = buscar.FileName;
            string dirDestino = ConfiguracionRutas.Local.RutaPiezas;
            string fecha = DateTime.Now.ToString("yyyy_MM_dd");
            string rutaDestino = System.IO.Path.Combine(dirDestino, $"Registro Piezas_{fecha}.txt");
            try
            {
                if (File.Exists(rutaDestino))
                {
                    var resultado = MessageBox.Show(
                        "Ya existe un registro para hoy. ¿Deseas sobrescribirlo?",
                        "Confirmar",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Warning);

                    if (resultado == MessageBoxResult.No) return;
                }

                File.Copy(rutaOrigen, rutaDestino, overwrite: true);

                CargarLista();

                MessageBox.Show("Lista importada correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error crítico al importar: {ex.Message}", "Error de E/S", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ValidarFormulario()
        {
            bool nombreOk = !string.IsNullOrWhiteSpace(texto_nombre.Text);
            bool colorOk = !string.IsNullOrWhiteSpace(texto_color.Text);
            bool largoOk = !string.IsNullOrWhiteSpace(texto_largo.Text);
            bool anchoOk = !string.IsNullOrWhiteSpace(texto_ancho.Text);

            if (engadirpeza != null)
            {
                engadirpeza.IsEnabled = nombreOk && colorOk && largoOk && anchoOk;
            }
        }

        private void texto_nombre_TextChanged(object sender, TextChangedEventArgs e) => ValidarFormulario();
    }
}