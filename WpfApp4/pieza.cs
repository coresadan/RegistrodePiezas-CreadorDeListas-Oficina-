using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;

namespace WpfApp4
{
    [Table("RegistroDePiezas")]
    public class Pieza : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        private string _nombre;
        [Column("Nombre")]
        public string nombre
        {
            get => _nombre;
            set
            {
                if (_nombre != value)
                {
                    _nombre = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _color;
        [Column("Color")]
        public string color
        {
            get => _color;
            set
            {
                if (_color != value)
                {
                    _color = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _semana;
        [Column("Semana")]
        public string Semana
        {
            get => _semana;
            set
            {
                if (_semana != value)
                {
                    _semana = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _largo;
        [Column("Largo")]
        public string largo
        {
            get => _largo;
            set
            {
                if (_largo != value)
                {
                    _largo = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _ancho;
        [Column("Ancho")]
        public string ancho
        {
            get => _ancho;
            set
            {
                if (_ancho != value)
                {
                    _ancho = value;
                    OnPropertyChanged();
                }
            }
        }
        // Suma de Piezas con las mismas características en la base de datos
        public int Cantidad { get; set; }

        [NotMapped]
        private int _cantidadPiezas;
        [NotMapped]
        public int cantidadPiezas
        {
            get => _cantidadPiezas;
            set
            {
                if (_cantidadPiezas != value)
                {
                    _cantidadPiezas = value;
                    OnPropertyChanged();
                }
            }
        }
        private bool _piezaurgente;
        [Column("Piezaurgente")]
        public bool piezaurgente
        {
            get => _piezaurgente;
            set
            {
                if (_piezaurgente != value)
                {
                    _piezaurgente = value;
                    Estado = value ? "Urgente" : "Pendiente";
                    OnPropertyChanged();
                }
            }
        }

        private string _estado = "Pendiente";
        public string Estado 
        {
            get => _estado;
            set
            {
                if (_estado != value)
                {
                    _estado = value;
                    OnPropertyChanged();
                }
            }
        }

        [Key]
        [Column("Id")]
        public int Id { get; set; }


        public Pieza Clonar()
        {
            string json = JsonSerializer.Serialize(this);
            return JsonSerializer.Deserialize<Pieza>(json);
        }
        public Pieza() { }
    }
}