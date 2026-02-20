using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Windows;

namespace WpfApp4
{
    public class ConfiguracionRutas
    
    {
        public string RutaPiezas { get; set; }
        public string? RutaUrgentes { get; set; }

        int? numero;

        static string RutaConfiguracion = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),"Registro de Piezas","Configuracion.json");
        public static ConfiguracionRutas Local { get; set; } 

        public static void CargarConfiguracion()
        {
            try
            {
                if (System.IO.File.Exists(RutaConfiguracion))
                {
                    var json = System.IO.File.ReadAllText(RutaConfiguracion);
                    Local = JsonSerializer.Deserialize<ConfiguracionRutas>(json);
                }
                else
                {
                    Local = new ConfiguracionRutas();
                }
            }
            catch (Exception)
            {
                Local = new ConfiguracionRutas();
            }
        }
        public static void GuardarConfiguracion()
        {
            if (string.IsNullOrEmpty(Path.GetDirectoryName(Local.RutaPiezas)) || !Directory.Exists(Path.GetDirectoryName(Local.RutaPiezas)))

            {
                MessageBox.Show("No se pudo guardar la configuracion: Escriba un directorio valido.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrEmpty(Path.GetDirectoryName(Local.RutaUrgentes)) || !Directory.Exists(Path.GetDirectoryName(Local.RutaUrgentes)))
            {
                MessageBox.Show("No se pudo guardar la configuracion: Escriba un directorio valido.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                var json = JsonSerializer.Serialize(Local, new JsonSerializerOptions { WriteIndented = true });
                System.IO.File.WriteAllText(RutaConfiguracion, json);
                MessageBox.Show("Configuración guardada correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Local = new ConfiguracionRutas();
            }
        }
    }
}