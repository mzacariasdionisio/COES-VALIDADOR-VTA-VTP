using COES.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla MAN_MANTTO
    /// </summary>
    public class ManManttoDTO : EntityBase
    {
        public int Mancodi { get; set; }
        public int? Equicodi { get; set; }
        public int? Tipoevencodi { get; set; }
        public int? Emprcodireporta { get; set; }
        public DateTime? Evenini { get; set; }
        public DateTime? Evenpreini { get; set; }
        public DateTime? Evenfin { get; set; }
        public DateTime? Evenprefin { get; set; }
        public int? Subcausacodi { get; set; }
        public decimal? Evenmwindisp { get; set; }
        public int? Evenpadre { get; set; }
        public string Evenindispo { get; set; }
        public string Eveninterrup { get; set; }
        public string Eventipoprog { get; set; }
        public string Evendescrip { get; set; }
        public string Evenobsrv { get; set; }
        public string Evenestado { get; set; }
        public string Lastuser { get; set; }
        public DateTime? Lastdate { get; set; }
        public int? Evenprocesado { get; set; }
        public int? Deleted { get; set; }
        public int? Regcodi { get; set; }
        public int? Manttocodi { get; set; }
        public string Isfiles { get; set; }
        public DateTime? Created { get; set; }
    }
}

