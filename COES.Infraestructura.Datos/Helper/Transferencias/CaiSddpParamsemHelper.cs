using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CAI_SDDP_PARAMSEM
    /// </summary>
    public class CaiSddpParamsemHelper : HelperBase
    {

        public CaiSddpParamsemHelper(): base(Consultas.CaiSddpParamsemSql)
        {
        }

        public CaiSddpParamsemDTO Create(IDataReader dr)
        {
            CaiSddpParamsemDTO entity = new CaiSddpParamsemDTO();

            int iSddppscodi = dr.GetOrdinal(this.Sddppscodi);
            if (!dr.IsDBNull(iSddppscodi)) entity.Sddppscodi = Convert.ToInt32(dr.GetValue(iSddppscodi));

            int iCaiajcodi = dr.GetOrdinal(this.Caiajcodi);
            if (!dr.IsDBNull(iCaiajcodi)) entity.Caiajcodi = Convert.ToInt32(dr.GetValue(iCaiajcodi));

            int iSddppsnumsem = dr.GetOrdinal(this.Sddppsnumsem);
            if (!dr.IsDBNull(iSddppsnumsem)) entity.Sddppsnumsem = Convert.ToInt32(dr.GetValue(iSddppsnumsem));

            int iSddppsdiaini = dr.GetOrdinal(this.Sddppsdiaini);
            if (!dr.IsDBNull(iSddppsdiaini)) entity.Sddppsdiaini = dr.GetDateTime(iSddppsdiaini);

            int iSddppsdiafin = dr.GetOrdinal(this.Sddppsdiafin);
            if (!dr.IsDBNull(iSddppsdiafin)) entity.Sddppsdiafin = dr.GetDateTime(iSddppsdiafin);

            int iSddppsusucreacionn = dr.GetOrdinal(this.Sddppsusucreacion);
            if (!dr.IsDBNull(iSddppsusucreacionn)) entity.Sddppsusucreacion = dr.GetString(iSddppsusucreacionn);

            int iSddppsfeccreacion = dr.GetOrdinal(this.Sddppsfeccreacion);
            if (!dr.IsDBNull(iSddppsfeccreacion)) entity.Sddppsfeccreacion = dr.GetDateTime(iSddppsfeccreacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Sddppscodi = "SDDPPSCODI";
        public string Caiajcodi = "CAIAJCODI";
        public string Sddppsnumsem = "SDDPPSNUMSEM";
        public string Sddppsdiaini = "SDDPPSDIAINI";
        public string Sddppsdiafin = "SDDPPSDIAFIN";
        public string Sddppsusucreacion = "SDDPPSUSUCREACION";
        public string Sddppsfeccreacion = "SDDPPSFECCREACION";

        #endregion

        public string SqlCodigoGenerado
        {
            get { return base.GetSqlXml("GetMaxId"); }
        }

    }
}
