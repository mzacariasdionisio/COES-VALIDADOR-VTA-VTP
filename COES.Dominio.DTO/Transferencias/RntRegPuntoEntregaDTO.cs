using System;
using System.Collections.Generic;
using COES.Base.Core;
using System.Xml.Serialization;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla RNT_REG_PUNTO_ENTREGA
    /// </summary>
    [XmlRoot("RNT_REG_PUNTO_ENTREGA")]
    public class RntRegPuntoEntregaDTO : EntityBase
    {
        public int Rpecodi { get; set; }
        public int RpeGrupoEnvio { get; set; }
        public string Concatenar { get { return TipoIntCodi.ToString() + "-" + RpeFechaInicio.ToString(); } set { } }
        public int RpeNivelTension { get; set; }
        public DateTime RpeFechaInicio { get; set; }
        public DateTime RpeFechaFin { get; set; }
        public Double RpeCompensacion { get; set; }
        public string RpeCausaInterrupcion { get; set; }
        public string RpeTramFuerMayor { get; set; }
        public string RpeIncremento { get; set; }
        public int RpeEstado { get; set; }
        public string RpeUsuarioCreacion { get; set; }
        public DateTime RpeFechaCreacion { get; set; }
        public string RpeUsuarioUpdate { get; set; }
        public DateTime RpeFechaUpdate { get; set; }
        public int RegPuntoEntCodi { get; set; }
        public int AreaCodi { get; set; }
        public int PeriodoCodi { get; set; }
        public string PeriodoDesc { get; set; }
        public int TipoIntCodi { get; set; }
        public int RpeEmpresaGeneradora { get; set; }
        public int RpeCliente { get; set; } //public int? RpeCliente { get; set; } 

        public string RpeNivelTensionDesc { get; set; }

        public string RpeTipoIntDesc { get; set; }

        public Double RpeEnergSem { get; set; }
        public Double RpeNi { get; set; }
        public Double RpeKi { get; set; }
        public DateTime? RpePrgFechaInicio { get; set; }
        public DateTime? RpePrgFechaFin { get; set; }
        public Double RpeEiE { get; set; }
        public int Barrcodi { get; set; }
        public int EnvioCodi { get; set; }
        public string RpeEmpResponsables { get; set; }

        public bool[] arrEsValido { get; set; }

        //INNER JOIN
        public string BarrNombre { get; set; }
        public string TipIntNombre { get; set; }
        public string RpeClienteNombre { get; set; }
        public string RpeEmpresaGeneradoraNombre { get; set; }
        public string AreaCodiNombre { get; set; }
        public string Accion { get; set; }
        [XmlArrayAttribute("Items")]
        public OrderedItem[] OrderedItems;


    }

    public class OrderedItem
    {

        public int OrdRegPuntoEntCodi { get; set; }
        public int OrdAreaCodi { get; set; }
        public int OrdPeriodoCodi { get; set; }
    }



}
