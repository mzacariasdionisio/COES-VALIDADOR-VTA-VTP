using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    public class GmmTrienioHelper : HelperBase
    {
        public GmmTrienioHelper()
            : base(Consultas.GmmTrienioSql)
        {

        }

        #region Mapeo de Campos
        public string Tricodi = "TRICODI";
        public string Empgcodi = "EMPGCODI";
        public string Trinuminc = "TRINUMINC";
        public string Trisecuencia = "TRISECUENCIA";
        public string Trifecinicio = "TRIFECINICIO";
        public string Trifeclimite = "TRIFECLIMITE";
        #endregion 
    }
}


	 
          
          
         
          
         