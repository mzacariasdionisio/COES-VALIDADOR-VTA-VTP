using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CB_FICHA
    /// </summary>
    public partial class CbFichaDTO : EntityBase, ICloneable
    {
        public int Cbftcodi { get; set; }
        public string Cbftnombre { get; set; }
        public DateTime Cbftfechavigencia { get; set; }

        public int Cbftactivo { get; set; }
        public string Cbftusucreacion { get; set; }
        public DateTime Cbftfeccreacion { get; set; }
        public string Cbftusumodificacion { get; set; }
        public DateTime? Cbftfecmodificacion { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    public partial class CbFichaDTO
    {
        public string CbftfechavigenciaDesc { get; set; }

        public string UltimaModificacionUsuarioDesc { get; set; }
        public string UltimaModificacionFechaDesc { get; set; }

        public string EstadoActual { get; set; }
        public bool EsVigente { get; set; }

        public List<CbFichaItemDTO> ListaSeccion { get; set; }
        public int TotalItem { get; set; }

        public CbFichaItemDTO ItemCostoOsinergmin { get; set; }


    }
}
