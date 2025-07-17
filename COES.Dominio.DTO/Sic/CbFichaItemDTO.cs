using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CB_FICHA_ITEM
    /// </summary>
    public partial class CbFichaItemDTO : EntityBase, ICloneable
    {
        public int Cbftitcodi { get; set; }
        public int Cbftcodi { get; set; }
        public int? Ccombcodi { get; set; }

        public string Cbftitesseccion { get; set; }
        public string Cbftitnombre { get; set; }
        public string Cbftitnumeral { get; set; }
        public string Cbftitoperacion { get; set; }
        public string Cbftitformula { get; set; }
        public string Cbftitinstructivo { get; set; }
        public string Cbftittipodato { get; set; }
        public string Cbftitabrev { get; set; }
        public string Cbftitconfidencial { get; set; }
        public string Cbftittipocelda { get; set; }
        public string Cbftitceldatipo1 { get; set; }
        public string Cbftitceldatipo2 { get; set; }
        public string Cbftitceldatipo3 { get; set; }
        public string Cbftitceldatipo4 { get; set; }
        public int? Cbftitcnp0 { get; set; }
        public int? Cbftitcnp1 { get; set; }
        public int? Cbftitcnp2 { get; set; }
        public int? Cbftitcnp3 { get; set; }
        public int? Cbftitcnp4 { get; set; }
        public int? Cbftitcnp5 { get; set; }
        public int? Cbftitcnp6 { get; set; }

        public int Cbftitactivo { get; set; }
        public string Cbftitusucreacion { get; set; }
        public DateTime? Cbftitfeccreacion { get; set; }
        public string Cbftitusumodificacion { get; set; }
        public DateTime? Cbftitfecmodificacion { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    public partial class CbFichaItemDTO
    {
        public bool EsSeccion { get; set; }
        public string Ccombnombre { get; set; }
        public string Ccombunidad { get; set; }
        public List<CbFichaItemDTO> ListaItemXSeccion { get; set; }
        public List<CbFichaItemDTO> ListaItemCabXSeccion { get; set; }

        public bool TieneDato { get; set; }
        public int PosRow { get; set; }
        public int PosCol { get; set; }

        public string TipoOpcionSeccion { get; set; }
        public int NumColSeccion { get; set; }
        public string MesUltimoCentralNuevaSeccion { get; set; }

        public int NumColSeccionMesAnt { get; set; }
        public int CbcentcodiMesUltimoComb { get; set; }
        public int CbcentcodiMesAnteriorComb { get; set; }

        public string NombreSeccion { get; set; }
        public string NumeralSeccion { get; set; }
        public bool TieneSetearValorDefaultSeccion { get; set; }
        public bool EsColEstado { get; set; }
        public bool EsSeccionSoloLectura { get; set; }
        public bool TieneCheckConf { get; set; }
        public bool TieneCheckConfEditable { get; set; }

        public bool OmitirCalculoFormula { get; set; }

        public int ConcepcodiDesplegable { get; set; }
        public bool TieneListaDesplegable { get; set; }
        public List<string> ListaOpcionDesplegable { get; set; }
        public int[] ListaOpcionNumcolDesplegableMesActual { get; set; }
        public int[] ListaOpcionNumcolDesplegableMesAnterior { get; set; }
        public int NumColNoaplica { get; set; }

        public CbObsDTO Obs { get; set; }
    }
}
