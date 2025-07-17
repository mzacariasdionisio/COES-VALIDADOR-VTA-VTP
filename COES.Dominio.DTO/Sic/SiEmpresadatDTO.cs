using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SI_EMPRESADAT
    /// </summary>
    [Serializable]
    public class SiEmpresadatDTO : EntityBase
    {
        public DateTime Empdatfecha { get; set; }
        public int Consiscodi { get; set; }
        public int Emprcodi { get; set; }
        public string Empdatusucreacion { get; set; }
        public DateTime? Empdatfeccreacion { get; set; }
        public string Empdatusumodificacion { get; set; }
        public DateTime? Empdatfecmodificacion { get; set; }
        public int Empdatdeleted { get; set; }
        public string Empdatvalor { get; set; }

        public string Emprabrev { get; set; }
        public string Emprnomb { get; set; }
        public string Emprestado { get; set; }
        public string EmprestadoDesc { get; set; }
        public DateTime EmprestadoFecha { get; set; }
        public string EmprestadoFechaDesc { get; set; }

        public string EmpdatfeccreacionDesc { get; set; }
        public string EmpdatfecmodificacionDesc { get; set; }
        public string UltimaModificacionUsuarioDesc { get; set; }
        public string UltimaModificacionFechaDesc { get; set; }

        public int Tipoemprcodi { get; set; }
    }
}
