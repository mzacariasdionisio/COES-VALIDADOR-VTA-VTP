using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
	/// <summary>
	/// Clase que mapea la tabla DOC_TIPO
	/// </summary>
	public class DocTipoDTO : EntityBase
	{
		public int Tipcodi { get; set; }
        public String Tipnombre { get; set; }
        public String Tipdesc { get; set; }
        public String Tipselec { get; set; }
        public int Tipplazo { get; set; }
        public String Tipdiacal { get; set; }


    }
}
