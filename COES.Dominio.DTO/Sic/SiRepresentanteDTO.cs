using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SI_REPRESENTANTE
    /// </summary>
    public class SiRepresentanteDTO : EntityBase
    {
        public int Rptecodi { get; set; }
        public string Rptetipo { get; set; }
        public string Rptetipodesc { get; set; }

        public string Rptetiprepresentantelegal { get; set; }
        public string Rptebaja { get; set; }
        public string Rpteinicial { get; set; }        
        public string Rptetipdocidentidad { get; set; }
        public string Rptedocidentidad { get; set; }
        public string Rptedocidentidadadj { get; set; }
        public string Rptedocidentidadadjfilename { get; set; }
        public string Rptenombres { get; set; }
        public string Rpteapellidos { get; set; }
        public string Rptevigenciapoder { get; set; }
        public string Rptevigenciapoderfilename { get; set; }
        public string Rptecargoempresa { get; set; }
        public string Rptetelefono { get; set; }
        public string Rptetelfmovil { get; set; }
        public string Rptecorreoelectronico { get; set; }
        public DateTime? Rptefeccreacion { get; set; }
        public string Rpteusucreacion { get; set; }
        public int? Emprcodi { get; set; }
        public string Rpteusumodificacion { get; set; }
        public DateTime? Rptefecmodificacion { get; set; }
        public DateTime? Rptefechavigenciapoder { get; set; }

        public string SoloLectura { get; set; }
        public string Accion { get; set; }
        public int Id { get; set; }

        public string RpteObservacion { get; set; }
        public int Dervcodi { get; set; }

        public string Emprnomb { get; set; }
        public string Emprruc { get; set; }
        public string Rpteindnotic { get; set; }

    }
}
