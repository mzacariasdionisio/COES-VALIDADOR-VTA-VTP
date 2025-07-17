using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla PR_CONGESTION
    /// </summary>
    [Serializable]
    public class PrCongestionDTO : EntityBase
    {
        public int Congescodi { get; set; }
        public DateTime? Congesfecinicio { get; set; }
        public DateTime? Congesfecfin { get; set; }
        public int? Configcodi { get; set; }
        public int? Grulincodi { get; set; }
        public string Indtipo { get; set; }
        public string Equinomb { get; set; }
        public string Emprnomb {  get; set; }
        public string Grulinnombre { get; set; }
        public string Lastuser { get; set; }
        public DateTime? Lastdate { get; set; }
        public string Nodobarra1 { get; set; }
        public string Nodobarra2 { get; set; }
        public string Nodobarra3 { get; set; }
        public int Equicodi { get; set; }
        public string Fuente { get; set; }
        public int Famcodi { get; set; }
        public string NombLinea { get; set; }
        public double Flujo { get; set; }
        public List<PrCongestionitemDTO> ListaItems { get; set; }
        public string Congesmotivo { get; set; }
        public int? Iccodi;
        public string Areanomb { get; set; }
        public string Famabrev { get; set; }
        public double Horatonumber { get; set; }
        public int Orden { get; set; }
        public List<string> ListaGrupoDespacho { get; set; }
        public string Areaoperativa { get; set; }

        public string Nombretna1 { get; set; }
        public string Nombretna2 { get; set; }
        public string Nombretna3 { get; set; }
        public string Nombretna { get; set; }

        #region SIOSEIN2
        public int Areacodi { get; set; }
        #endregion


        #region Regiones_seguridad

        public int? Regsegcodi { get; set; }
        public decimal Regsegvalorm { get; set; }
        public string Regsegdirec { get; set; }
        public decimal Pmax { get; set; }
        public decimal ParamB { get; set; }
        public string Regsegnombre { get; set; }

        #region CambioRS29122020
        public decimal GenT { get; set; }
        public decimal FlujoT { get; set; }

        #endregion

        #endregion

        #region Mejoras CMgN

        public string NombreResultado { get; set; }

        #endregion

        #region CMgCP_PR07
        public string Grulintipo { get; set; }
        public int? Hopcodi { get; set; }
        #endregion
    }

    public class PrCongestionitemDTO
    {
        public string Nombarra1 { get; set; }
        public string Nombarra2 { get; set; }
        public string NombLinea { get; set; }
        public int Codigo { get; set; }
        public string Nombretna { get; set; }

        #region Regiones_seguridad

        public int TipoEquipo { get; set; }

        #endregion
    }



}
