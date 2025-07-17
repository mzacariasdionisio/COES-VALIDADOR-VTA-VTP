using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla VCE_LOG_CARGA_CAB
    /// </summary>
    public class VceLogCargaCabHelper : HelperBase
    {
        public VceLogCargaCabHelper(): base(Consultas.VceLogCargaCabSql)
        {
        }

        public VceLogCargaCabDTO Create(IDataReader dr)
        {
            VceLogCargaCabDTO entity = new VceLogCargaCabDTO();

            int iCrlccorden = dr.GetOrdinal(this.Crlccorden);
            if (!dr.IsDBNull(iCrlccorden)) entity.Crlccorden = Convert.ToInt32(dr.GetValue(iCrlccorden));

            int iCrlccentidad = dr.GetOrdinal(this.Crlccentidad);
            if (!dr.IsDBNull(iCrlccentidad)) entity.Crlccentidad = dr.GetString(iCrlccentidad);

            int iCrlccnombtabla = dr.GetOrdinal(this.Crlccnombtabla);
            if (!dr.IsDBNull(iCrlccnombtabla)) entity.Crlccnombtabla = dr.GetString(iCrlccnombtabla);

            int iPecacodi = dr.GetOrdinal(this.Pecacodi);
            if (!dr.IsDBNull(iPecacodi)) entity.PecaCodi = Convert.ToInt32(dr.GetValue(iPecacodi));

            int iCrlcccodi = dr.GetOrdinal(this.Crlcccodi);
            if (!dr.IsDBNull(iCrlcccodi)) entity.Crlcccodi = Convert.ToInt32(dr.GetValue(iCrlcccodi));

            return entity;
        }


        #region Mapeo de Campos

        public string Crlccorden = "CRLCCORDEN";
        public string Crlccentidad = "CRLCCENTIDAD";
        public string Crlccnombtabla = "CRLCCNOMBTABLA";
        public string Pecacodi = "PECACODI";
        public string Crlcccodi = "CRLCCCODI";

        #endregion

        public string SqlInit
        {
            get { return base.GetSqlXml("Init"); }
        }
        public string SqlGetMinIdByVersion
        {
            get { return base.GetSqlXml("GetMinIdByVersion"); }
        }
        public string SqlDeleteCabPeriodo
        {
            get { return base.GetSqlXml("DeleteCabPeriodo"); }
        }
        
    }
}
