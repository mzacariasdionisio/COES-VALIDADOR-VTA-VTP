using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// 
    /// </summary>
   public class DemandaMercadoLibreHelper: HelperBase
    {
       public DemandaMercadoLibreHelper()
           : base(Consultas.DemandaMercadoLibreSql)
       {
       }
      

       #region Mapeo de Campos

       public string EmprCodi = "EMPRCODI";
       public string EmprRazSocial = "EMPRRAZSOCIAL";
       public string DemandaMes1 = "DEMANDAMES1";
       public string DemandaMes2 = "DEMANDAMES2";
       public string DemandaMes3 = "DEMANDAMES3";
       public string DemandaMes4 = "DEMANDAMES4";
       public string DemandaMes5 = "DEMANDAMES5";
       public string DemandaMes6 = "DEMANDAMES6";
       public string DemandaMes7 = "DEMANDAMES7";
       public string DemandaMes8 = "DEMANDAMES8";
       public string DemandaMes9 = "DEMANDAMES9";
       public string DemandaMes10 = "DEMANDAMES10";
       public string DemandaMes11 = "DEMANDAMES11";
       public string DemandaMes12 = "DEMANDAMES12";
       public string DemandaMaxima = "DEMANDAMAXIMA";
       public string PotenciaMaximaRetirar = "POTENCIAMAXIMARETIRAR";

        #endregion

        public string SqlListaDemandaMercadoLibre
        {
            get { return base.GetSqlXml("ListDemandaMercadoLibre"); }
        }

       
    }
}
