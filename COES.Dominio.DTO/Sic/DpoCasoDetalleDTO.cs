using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla DPO_CASO_DETALLE
    /// </summary>
    [Serializable]
    public class DpoCasoDetalleDTO : EntityBase
    {
		public int Dpocasdetcodi { get; set; }
		public int Dpocsocodi { get; set; }

		public int Dpodetmafscada { get; set; }
		public DateTime Dpodetmatinicio { get; set; }
		public DateTime Dpodetmatfin { get; set; }
		public int Dpofnccodima { get; set; }
		public string Dposecuencma { get; set; }

        public string Dpotipfuncion { get; set; }

        // ------------------------------------
        public string Pafunr1dtg1 { get; set; }
        public string Pafunr1dtg2 { get; set; }
        public string Pafunr1dtg3 { get; set; }
        public string Pafunr1dtg4 { get; set; }
        public string Pafunr1dtg5 { get; set; }
        public string Pafunr1dtg6 { get; set; }
        public string Pafunr1dtg7 { get; set; }

        //public DateTime Pafunr1deg7 { get; set; }
        //public DateTime Pafunr1hag7 { get; set; }
        public string Pafunr1deg7 { get; set; }
        public string Pafunr1hag7 { get; set; }

        // ------------------------------------
        public string Pafunr2dtg1 { get; set; }
        public string Pafunr2dtg2 { get; set; }
        public string Pafunr2dtg3 { get; set; }
        public string Pafunr2dtg4 { get; set; }
        public string Pafunr2dtg5 { get; set; }
        public string Pafunr2dtg6 { get; set; }
        public string Pafunr2dtg7 { get; set; }

        // ------------------------------------
        public string Pafunf1toram { get; set; }

        // ------------------------------------
        public string Pafunf2factk { get; set; }

        // ------------------------------------
        public string Pafuna1aniof { get; set; }
        public string Pafuna1idfer { get; set; }

        public string Pafuna1dtg1 { get; set; }
        public string Pafuna1dtg2 { get; set; }
        public string Pafuna1dtg3 { get; set; }
        public string Pafuna1dtg4 { get; set; }
        public string Pafuna1dtg5 { get; set; }
        public string Pafuna1dtg6 { get; set; }
        public string Pafuna1dtg7 { get; set; }

        // ------------------------------------
        public string Pafuna2aniof { get; set; }
        public string Pafuna2idfer { get; set; }

        public string Pafuna2dtg1 { get; set; }
        public string Pafuna2dtg2 { get; set; }
        public string Pafuna2dtg3 { get; set; }
        public string Pafuna2dtg4 { get; set; }
        public string Pafuna2dtg5 { get; set; }
        public string Pafuna2dtg6 { get; set; }
        public string Pafuna2dtg7 { get; set; }

        // Adicionales
        public string Dpodesfundm { get; set; }
        public string StrDpodetmatinicio { get; set; }
        public string StrDpodetmatfin { get; set; }
    }

	public class DpoFuncionDataMaestraDTO : EntityBase
	{
		public int Dpocasdetcodi { get; set; }
		public int Dpocsocodi { get; set; }

		public int Dpodetmafscada { get; set; }
		public DateTime Dpodetmatinicio { get; set; }
		public DateTime Dpodetmatfin { get; set; }
		public int Dpofnccodima { get; set; }
		public string Dposecuencma { get; set; }

        // Adicionales
        public string Dpodesfundm { get; set; }
        public string StrDpodetmatinicio { get; set; }
        public string StrDpodetmatfin { get; set; }
    }

	public class DpoFuncionDataProcesarDTO : EntityBase
	{
		public int Dpocasdetcodi { get; set; }
		public int Dpocsocodi { get; set; }

		public int Dpodetprfscada { get; set; } 
		public DateTime Dpodetprinicio { get; set; }
		public DateTime Dpodetprfin { get; set; }
		public int Dpofnccodipr { get; set; }
		public string Dposecuencpr { get; set; }

        // Adicionales
        public string Dpodesfunpr { get; set; }

        public string StrDpodetprinicio { get; set; }
        public string StrDpodetprfin { get; set; }
    }

    public class DpoParametrosR1DTO : EntityBase
    {
        public int Dpocasdetcodi { get; set; }
        public int Dpocsocodi { get; set; }
        public string Pafunr1dtg1 { get; set; }
        public string Pafunr1dtg2 { get; set; }
        public string Pafunr1dtg3 { get; set; }
        public string Pafunr1dtg4 { get; set; }
        public string Pafunr1dtg5 { get; set; }
        public string Pafunr1dtg6 { get; set; }
        public string Pafunr1dtg7 { get; set; }
        public string Pafunr1deg7 { get; set; }
        public string Pafunr1hag7 { get; set; }

    }

    public class DpoParametrosR2DTO : EntityBase
    {
        public int Dpocasdetcodi { get; set; }
        public int Dpocsocodi { get; set; }
        public string Pafunr2dtg1 { get; set; }
        public string Pafunr2dtg2 { get; set; }
        public string Pafunr2dtg3 { get; set; }
        public string Pafunr2dtg4 { get; set; }
        public string Pafunr2dtg5 { get; set; }
        public string Pafunr2dtg6 { get; set; }
        public string Pafunr2dtg7 { get; set; }
    }

    public class DpoParametrosF1DTO : EntityBase
    {
        public int Dpocasdetcodi { get; set; }
        public int Dpocsocodi { get; set; }
        public string Pafunf1toram { get; set; }
    }

    public class DpoParametrosF2DTO : EntityBase
    {
        public int Dpocasdetcodi { get; set; }
        public int Dpocsocodi { get; set; }
        public string Pafunf2factk { get; set; }
    }

    public class DpoParametrosA1DTO : EntityBase
    {
        public int Dpocasdetcodi { get; set; }
        public int Dpocsocodi { get; set; }
        public string Pafuna1aniof { get; set; }
        public string Pafuna1idfer { get; set; }
        public string Pafuna1dtg1 { get; set; }
        public string Pafuna1dtg2 { get; set; }
        public string Pafuna1dtg3 { get; set; }
        public string Pafuna1dtg4 { get; set; }
        public string Pafuna1dtg5 { get; set; }
        public string Pafuna1dtg6 { get; set; }
        public string Pafuna1dtg7 { get; set; }
    }

    public class DpoParametrosA2DTO : EntityBase
    {
        public int Dpocasdetcodi { get; set; }
        public int Dpocsocodi { get; set; }
        public string Pafuna2aniof { get; set; }
        public string Pafuna2idfer { get; set; }
        public string Pafuna2dtg1 { get; set; }
        public string Pafuna2dtg2 { get; set; }
        public string Pafuna2dtg3 { get; set; }
        public string Pafuna2dtg4 { get; set; }
        public string Pafuna2dtg5 { get; set; }
        public string Pafuna2dtg6 { get; set; }
        public string Pafuna2dtg7 { get; set; }
    }

    public class DpoDiasTipicosR1DTO : EntityBase
    {
        public string Pafunr1dtg1 { get; set; }
        public string Pafunr1dtg2 { get; set; }
        public string Pafunr1dtg3 { get; set; }
        public string Pafunr1dtg4 { get; set; }
        public string Pafunr1dtg5 { get; set; }
        public string Pafunr1dtg6 { get; set; }
        public string Pafunr1dtg7 { get; set; }
    }

    public class DpoDiasTipicosR2DTO : EntityBase
    {
        public string Pafunr2dtg1 { get; set; }
        public string Pafunr2dtg2 { get; set; }
        public string Pafunr2dtg3 { get; set; }
        public string Pafunr2dtg4 { get; set; }
        public string Pafunr2dtg5 { get; set; }
        public string Pafunr2dtg6 { get; set; }
        public string Pafunr2dtg7 { get; set; }
    }

    public class DpoDiasTipicosA1DTO : EntityBase
    {
        public string Pafuna1dtg1 { get; set; }
        public string Pafuna1dtg2 { get; set; }
        public string Pafuna1dtg3 { get; set; }
        public string Pafuna1dtg4 { get; set; }
        public string Pafuna1dtg5 { get; set; }
        public string Pafuna1dtg6 { get; set; }
        public string Pafuna1dtg7 { get; set; }
    }

    public class DpoDiasTipicosA2DTO : EntityBase
    {
        public string Pafuna2dtg1 { get; set; }
        public string Pafuna2dtg2 { get; set; }
        public string Pafuna2dtg3 { get; set; }
        public string Pafuna2dtg4 { get; set; }
        public string Pafuna2dtg5 { get; set; }
        public string Pafuna2dtg6 { get; set; }
        public string Pafuna2dtg7 { get; set; }
    }
}
