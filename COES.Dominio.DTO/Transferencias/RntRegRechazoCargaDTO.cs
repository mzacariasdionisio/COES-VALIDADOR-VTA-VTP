using System;
using System.Collections.Generic;
using COES.Base.Core;
using System.Xml.Serialization;

namespace COES.Dominio.DTO.Transferencias
{
    /// <summary>
    /// Clase que mapea la tabla RNT_REG_RECHAZO_CARGA
    /// </summary>
    [XmlRoot("RNT_REG_RECHAZO_CARGA")]
    public class RntRegRechazoCargaDTO : EntityBase
    {
        public int RrcGrupoEnvio { get; set; }
        public int RrcNivelTension { get; set; }
        public DateTime RrcFechaInicio { get; set; }
        public DateTime RrcFechaFin { get; set; }
        public string RrcSubestacionDstrb { get; set; }
        public decimal RrcNivelTensionSed { get; set; }
        public string RrcCodiAlimentador { get; set; }
        public decimal RrcEnergiaEns { get; set; }
        public int RrcNrcf { get; set; }
        public decimal RrcEf { get; set; }
        public decimal RrcCompensacion { get; set; }
        public int RrcCliente { get; set; }
        public int RrcEstado { get; set; }
        public string RrcUsuarioCreacion { get; set; }
        public DateTime RrcFechaCreacion { get; set; }
        public string RrcUsuarioUpdate { get; set; }
        public DateTime RrcFechaUpdate { get; set; }
        public int RegRechazoCargaCodi { get; set; }
        public int AreaCodi { get; set; }
        public int PeriodoCodi { get; set; }
        public int EvenCodi { get; set; }
        public int RrcEmpresaGeneradora { get; set; }
        public int EmprCodi { get; set; }
        public bool[] arrEsValido { get; set; }
        public int Barrcodi { get; set; }
        public int Enviocodi { get; set; }
        public string BarrNombre { get; set; }
        public decimal RrcPk { get; set; }
        public string RrcCompensable { get; set; }
        public decimal RrcEnsFk { get; set; }
        public string RrcEvenCodiDesc { get; set; }
        //INNER JOIN
        public string RrcClienteNombre { get; set; }
        public string RrcEmpresaGeneradoraNombre { get; set; }
        public string AreaCodiNombre { get; set; }
        public string Accion { get; set; }
    }
}
