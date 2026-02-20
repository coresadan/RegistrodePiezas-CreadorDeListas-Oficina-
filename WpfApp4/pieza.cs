using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;

namespace WpfApp4
{
    public class Pieza : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        private string _nombre;
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
        private string _largo;
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

        private int _cantidadPiezas;
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
        public bool piezaurgente
        {
            get => _piezaurgente;
            set
            {
                if (_piezaurgente != value)
                {
                    _piezaurgente = value;
                    OnPropertyChanged();
                }
            }
        }
        public Pieza Clonar()
        {
            string json = JsonSerializer.Serialize(this);
            return JsonSerializer.Deserialize<Pieza>(json);
        }
        public Pieza() { }
    }
}