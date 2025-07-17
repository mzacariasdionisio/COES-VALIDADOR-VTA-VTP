using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CM_COSTOMARGINALPROG
    /// </summary>
    public class CmCostomarginalprogHelper : HelperBase
    {
        public CmCostomarginalprogHelper(): base(Consultas.CmCostomarginalprogSql)
        {
        }

        public CmCostomarginalprogDTO Create(IDataReader dr)
        {
            CmCostomarginalprogDTO entity = new CmCostomarginalprogDTO();

            int iCmarprcodi = dr.GetOrdinal(this.Cmarprcodi);
            if (!dr.IsDBNull(iCmarprcodi)) entity.Cmarprcodi = Convert.ToInt32(dr.GetValue(iCmarprcodi));

            int iCnfbarcodi = dr.GetOrdinal(this.Cnfbarcodi);
            if (!dr.IsDBNull(iCnfbarcodi)) entity.Cnfbarcodi = Convert.ToInt32(dr.GetValue(iCnfbarcodi));

            int iCmarprtotal = dr.GetOrdinal(this.Cmarprtotal);
            if (!dr.IsDBNull(iCmarprtotal)) entity.Cmarprtotal = dr.GetDecimal(iCmarprtotal);

            int iCmarprfecha = dr.GetOrdinal(this.Cmarprfecha);
            if (!dr.IsDBNull(iCmarprfecha)) entity.Cmarprfecha = dr.GetDateTime(iCmarprfecha);

            int iCmarprlastuser = dr.GetOrdinal(this.Cmarprlastuser);
            if (!dr.IsDBNull(iCmarprlastuser)) entity.Cmarprlastuser = dr.GetString(iCmarprlastuser);

            int iCmarprlastdate = dr.GetOrdinal(this.Cmarprlastdate);
            if (!dr.IsDBNull(iCmarprlastdate)) entity.Cmarprlastdate = dr.GetDateTime(iCmarprlastdate);

            return entity;
        }


        #region Mapeo de Campos

        public string Cmarprcodi = "CMARPRCODI";
        public string Cnfbarcodi = "CNFBARCODI";
        public string Cmarprtotal = "CMARPRTOTAL";
        public string Cmarprfecha = "CMARPRFECHA";
        public string Cmarprlastuser = "CMARPRLASTUSER";
        public string Cmarprlastdate = "CMARPRLASTDATE";
        #region IMME
        public string NombreTabla = "Cm_Costomarginalprog";
        #endregion
        #region MonitoreoMME
        public string Grupocodi = "GRUPOCODI";
        #endregion

        #region SIOSEIN - CAMPOS
        public string Osinergcodi = "OSINERGCODI";
        public string Cnfbarnombre = "CNFBARNOMBRE";
        #endregion

        #region IMME

        public string Fechadia = "FECHADIA";
        public string Totalregdia = "TOTALREGDIA";

        #endregion

        #endregion

        #region MonitoreoMME

        public string SqlListCostMarProg
        {
            get { return GetSqlXml("ListCostoMarginalProgPeriodo"); }
        }

        #endregion 

        #region SIOSEIN

        public string SqlGetByBarratranferencia
        {
            get { return GetSqlXml("GetByBarratranferencia"); }
        }

        #endregion

        #region IMME

        public string SqlDeleteDias
        {
            get { return GetSqlXml("DeleteDias"); }
        }

        public string SqlListaTotalXDia
        {
            get { return GetSqlXml("ListaTotalXDia"); }
        }
        
        #endregion

    }
}
