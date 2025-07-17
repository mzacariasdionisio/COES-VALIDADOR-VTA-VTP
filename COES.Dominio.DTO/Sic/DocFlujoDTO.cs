using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
	/// <summary>
	/// Clase que mapea la tabla ME_ENVIO
	/// </summary>
	public class DocFlujoDTO : EntityBase
	{
		public DateTime? Fljfecharecep { get; set; }
		public DateTime? Fljfechaproce { get; set; }
		public int Fljcodi { get; set; }
		public int Emprcodi { get; set; }
		public int Fljremitente { get; set; }
		public DateTime? Fljfechaorig { get; set; }

        public DateTime? Fljfechainicio { get; set; }
        public int Areacodedest { get; set; }
        public int Areacode { get; set; }
        public int Areapadre { get; set; }
        public int Fljdetnivel { get; set; }

        public int Tipcodi { get; set; }
        public String Fljnombre { get; set; }
        public int Fljdiasmaxaten { get; set; }

        public DateTime? Fljfechaterm { get; set; }
        public String Fljnumext { get; set; }
        public String Fljestado { get; set; }
        public int Tatcodi { get; set; }


        public int Fljdetcodi { get; set; }
        public int Fljdetcodiref { get; set; }

        public String Fljcadatencion { get; set; } //07 Nov 2017 STS

        #region MigracionSGOCOES-GrupoB

        public string FljfecharecepDesc { get; set; }
        public string FljfechaorigDesc { get; set; }

        #endregion
    }
}
