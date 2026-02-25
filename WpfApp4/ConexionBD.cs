using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WpfApp4;

namespace Registro_de_Piezas
{
    class ConexionBD : DbContext
    {
       public DbSet<Pieza> RegistroDePiezas { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string directorio = @"C:\pruebas";
            string nombreArchivo = "BDPiezas.s3db";
            string rutaFisica = System.IO.Path.Combine(directorio, nombreArchivo);

            if (!System.IO.Directory.Exists(directorio))
            {
                System.IO.Directory.CreateDirectory(directorio);
            }
            optionsBuilder.UseSqlite($"Data Source={rutaFisica}");
        }
    }
}
