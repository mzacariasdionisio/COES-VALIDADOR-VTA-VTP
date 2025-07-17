using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CM_MODELO_EMBALSE
    /// </summary>
    public partial class CmModeloEmbalseDTO : EntityBase
    {
        public int Modembcodi { get; set; }
        public int Topcodi { get; set; }
        public int Recurcodi { get; set; }
        public int? Ptomedicodi { get; set; }
        public string Modembindyupana { get; set; }
        public DateTime Modembfecvigencia { get; set; }

        public string Modembestado { get; set; }
        public string Modembusucreacion { get; set; }
        public DateTime? Modembfeccreacion { get; set; }
        public string Modembusumodificacion { get; set; }
        public DateTime? Modembfecmodificacion { get; set; }
    }

    public partial class CmModeloEmbalseDTO
    {
        public string Recurnombre { get; set; }
        public string Ptomedidesc { get; set; }
        public string Ptomedielenomb { get; set; }
        public string Ptomedibarranomb { get; set; }
        public string CentralTurbinate { get; set; }

        public string ModembfecvigenciaDesc { get; set; }
        public string ModembindyupanaDesc { get; set; }
        public string UltimaModificacionUsuarioDesc { get; set; }
        public string UltimaModificacionFechaDesc { get; set; }

        public List<CmModeloComponenteDTO> ListaComponente { get; set; }
    }
}
