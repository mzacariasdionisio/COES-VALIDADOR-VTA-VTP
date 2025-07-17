using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla TRN_MIGRACION
    /// </summary>
    public class TrnMigracionDTO : EntityBase
    {
        public int Trnmigcodi { get; set; } 
        public int Migracodi { get; set; } 
        public int Emprcodiorigen { get; set; } 
        public int Emprcodidestino { get; set; }
        public string Trnmigdescripcion { get; set; }
        public string Trnmigsql { get; set; }
        public string Trnmigestado { get; set; }
        public string Trnmigusucreacion { get; set; } 
        public DateTime Trnmigfeccreacion { get; set; } 
        public string Trnmigusumodificacion { get; set; } 
        public DateTime Trnmigfecmodificacion { get; set; } 
    }
}
