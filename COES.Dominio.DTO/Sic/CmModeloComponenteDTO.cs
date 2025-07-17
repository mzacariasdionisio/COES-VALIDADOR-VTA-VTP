using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CM_MODELO_COMPONENTE
    /// </summary>
    public partial class CmModeloComponenteDTO : EntityBase
    {
        public int Modcomcodi { get; set; }
        public int Modembcodi { get; set; }
        public string Modcomtipo { get; set; }
        public int? Equicodi { get; set; }
        public decimal? Modcomtviaje { get; set; }
    }

    public partial class CmModeloComponenteDTO
    {
        public List<CmModeloAgrupacionDTO> ListaAgrupacion { get; set; }
        public int Recurcodi { get; set; }
        public string Recurnombre { get; set; }
        public string Equinomb { get; set; }
        public decimal? Rendimiento { get; set; }

        public string UnidadMedida { get; set; }
        public string UnidadMedidaAbrev { get; set; }

        public bool EsCompNoVisible { get; set; }
    }
}
