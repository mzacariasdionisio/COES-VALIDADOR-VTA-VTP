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
   public class IngresoRetiroSCHelper: HelperBase
    {
        /// <summary>
        /// Clase que contiene el mapeo de la tabla trn_ing_retirosc
        /// </summary>
        public IngresoRetiroSCHelper() : base(Consultas.IngresoRetiroSCSql)
        {
        }
       public IngresoRetiroSCDTO Create(IDataReader dr)
        {
            IngresoRetiroSCDTO entity = new IngresoRetiroSCDTO();

            int iPeriCodi = dr.GetOrdinal(this.PeriCodi);
            if (!dr.IsDBNull(iPeriCodi)) entity.PeriCodi = dr.GetInt32(iPeriCodi);

            int iIngrscCodi = dr.GetOrdinal(this.Ingrsccodi);
            if (!dr.IsDBNull(iIngrscCodi)) entity.IngrscCodi = dr.GetInt32(iIngrscCodi);

            int iIngrscVers = dr.GetOrdinal(this.Ingrscversion);
               if (!dr.IsDBNull(iIngrscVers)) entity.IngrscVersion = dr.GetInt32(iIngrscVers);

            int iEmprCodi = dr.GetOrdinal(this.EmprCodi);
            if (!dr.IsDBNull(iEmprCodi)) entity.EmprCodi = dr.GetInt32(iEmprCodi);

            int iIngrscImporte = dr.GetOrdinal(this.Ingrscimporte);
            if (!dr.IsDBNull(iIngrscImporte)) entity.IngrscImporte = dr.GetDecimal(iIngrscImporte);

            int iIngrscImporteVtp = dr.GetOrdinal(this.Ingrscimportevtp);
            if (!dr.IsDBNull(iIngrscImporteVtp)) entity.IngrscImporteVtp = dr.GetDecimal(iIngrscImporteVtp);

            int iIngrscUsername = dr.GetOrdinal(this.Ingrscusername);
            if (!dr.IsDBNull(iIngrscUsername)) entity.IngrscUserName = dr.GetString(iIngrscUsername);

            int iIngrscFecins = dr.GetOrdinal(this.Ingrscfecins);
            if (!dr.IsDBNull(iIngrscFecins)) entity.IngrscFecIns = dr.GetDateTime(iIngrscFecins);

            int iIngrscFecact = dr.GetOrdinal(this.Ingrscfecact);
            if (!dr.IsDBNull(iIngrscFecact)) entity.IngrscFecAct = dr.GetDateTime(iIngrscFecact);

            return entity;
        }

        #region Mapeo de Campos

        public string Ingrsccodi = "INGRSCCODI";
        public string PeriCodi = "PERICODI";
        public string EmprCodi = "EMPRCODI";
        public string Ingrscversion = "INGRSCVERSION";
        public string Ingrscimporte = "INGRSCIMPORTE";
        public string Ingrscimportevtp = "INGRSCIMPORTEVTP";
        public string Ingrscusername = "INGRSCUSERNAME";
        public string Ingrscfecins = "INGRSCFECINS";
        public string Ingrscfecact = "INGRSCFECACT";
        public string EmprNombre = "NOMBEMPRESA";
        public string PeriNombre = "NOMBPERIODO";
        public string Total = "TOTAL";      

        #endregion

        public string SqlCodigoGenerado
        {
            get { return base.GetSqlXml("GetMaxId"); }
        }

        public string SqlGetByCodigo
        {
            get { return base.GetSqlXml("GetByCodigo"); }
        }

        public string SqlListByPeriodoVersion
        {
            get { return base.GetSqlXml("ListByPeriodoVersion"); }
        }

        public string SqlGetByPeriodoVersionEmpresa
        {
            get { return base.GetSqlXml("GetByPeriodoVersionEmpresa"); }
        }
    }
}
