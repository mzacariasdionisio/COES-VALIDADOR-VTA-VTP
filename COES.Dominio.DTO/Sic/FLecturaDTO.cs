using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla F_LECTURA
    /// </summary>
    [Serializable]
    public partial class FLecturaDTO : EntityBase
    {
        public DateTime Fechahora { get; set; } 
        public int Gpscodi { get; set; } 
        public decimal? Vsf { get; set; } 
        public decimal? Maximo { get; set; } 
        public decimal? Minimo { get; set; } 
        public decimal? Voltaje { get; set; } 
        public int? Num { get; set; } 
        public decimal? Desv { get; set; } 
        public decimal? H0 { get; set; } 
        public decimal? H1 { get; set; } 
        public decimal? H2 { get; set; } 
        public decimal? H3 { get; set; } 
        public decimal? H4 { get; set; } 
        public decimal? H5 { get; set; } 
        public decimal? H6 { get; set; } 
        public decimal? H7 { get; set; } 
        public decimal? H8 { get; set; } 
        public decimal? H9 { get; set; } 
        public decimal? H10 { get; set; } 
        public decimal? H11 { get; set; } 
        public decimal? H12 { get; set; } 
        public decimal? H13 { get; set; } 
        public decimal? H14 { get; set; } 
        public decimal? H15 { get; set; } 
        public decimal? H16 { get; set; } 
        public decimal? H17 { get; set; } 
        public decimal? H18 { get; set; } 
        public decimal? H19 { get; set; } 
        public decimal? H20 { get; set; } 
        public decimal? H21 { get; set; } 
        public decimal? H22 { get; set; } 
        public decimal? H23 { get; set; } 
        public decimal? H24 { get; set; } 
        public decimal? H25 { get; set; } 
        public decimal? H26 { get; set; } 
        public decimal? H27 { get; set; } 
        public decimal? H28 { get; set; } 
        public decimal? H29 { get; set; } 
        public decimal? H30 { get; set; } 
        public decimal? H31 { get; set; } 
        public decimal? H32 { get; set; } 
        public decimal? H33 { get; set; } 
        public decimal? H34 { get; set; } 
        public decimal? H35 { get; set; } 
        public decimal? H36 { get; set; } 
        public decimal? H37 { get; set; } 
        public decimal? H38 { get; set; } 
        public decimal? H39 { get; set; } 
        public decimal? H40 { get; set; } 
        public decimal? H41 { get; set; } 
        public decimal? H42 { get; set; } 
        public decimal? H43 { get; set; } 
        public decimal? H44 { get; set; } 
        public decimal? H45 { get; set; } 
        public decimal? H46 { get; set; } 
        public decimal? H47 { get; set; } 
        public decimal? H48 { get; set; } 
        public decimal? H49 { get; set; } 
        public decimal? H50 { get; set; } 
        public decimal? H51 { get; set; } 
        public decimal? H52 { get; set; } 
        public decimal? H53 { get; set; } 
        public decimal? H54 { get; set; } 
        public decimal? H55 { get; set; } 
        public decimal? H56 { get; set; } 
        public decimal? H57 { get; set; } 
        public decimal? H58 { get; set; } 
        public decimal? H59 { get; set; } 
        public decimal? Devsec { get; set; } 
        
        public decimal? Meditotal { get; set; }
    }

    public partial class FLecturaDTO
    {
        public string TextoRangoIni { get; set; }
        public string TextoRangoFin { get; set; }
        public string TextoUmbral { get; set; }
        public string TextoMin { get; set; }
        public string TextoMed { get; set; }
        public string TextoMax { get; set; }

        public decimal Linf;
        public decimal Lsup;
        public decimal XValor;
        public decimal Minima_Cont;
        public decimal Media_Cont;
        public decimal Maxima_Cont;
        public decimal Minima_Porc;
        public decimal Media_Porc;
        public decimal Maxima_Porc;
        public List<decimal> ListaMaxima { get; set; }
        public List<decimal> ListaMinima { get; set; }
        public List<decimal> ListaMedia { get; set; }

        public decimal Umbral;
        public decimal Frecuencia;
    }
}
