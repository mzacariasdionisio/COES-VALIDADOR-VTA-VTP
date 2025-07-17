using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SI_PLANTILLACORREO
    /// </summary>
    public partial class SiPlantillacorreoDTO : EntityBase
    {
        public int Plantcodi { get; set; }
        public string Plantcontenido { get; set; }
        public int? Modcodi { get; set; }
        public int? Tpcorrcodi { get; set; }
        public string Plantasunto { get; set; }
        public string Plantnomb { get; set; }
        public string Plantindhtml { get; set; }
        public string Plantindadjunto { get; set; }
        public string Planticorreos { get; set; }
        public string PlanticorreosCc { get; set; }
        public string PlanticorreosBcc { get; set; }
        public string PlanticorreoFrom { get; set; }
        public string Plantlinkadjunto { get; set; }
        public DateTime? Plantfeccreacion { get; set; }
        public DateTime? Plantfecmodificacion { get; set; }
        public string Plantusucreacion { get; set; }
        public string Plantusumodificacion { get; set; }
    }

    public partial class SiPlantillacorreoDTO
    {
        public string PlantfeccreacionDesc { get; set; }
        public string PlantfecmodificacionDesc { get; set; }
        public string UltimaModificacionUsuarioDesc { get; set; }
        public string UltimaModificacionFechaDesc { get; set; }

        public int Progrcodi { get; set; }
        public int Evenclasecodi { get; set; }
        public string Correlativos { get; set; }

        public int Prcscodi { get; set; }

        #region region CCGAS.PR31

        public string EstadoRecordatorio { get; set; }
        public string Hora { get; set; }
        public string RespondeAAgente { get; set; }
        public string ParametroDiaHora { get; set; }
        #endregion

        public int CarpetaFiles { get; set; }
        public List<InArchivoDTO> ListaArchivo { get; set; }
    }
}
