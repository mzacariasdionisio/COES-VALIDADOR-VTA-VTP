using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla VCE_LOG_CARGA_DET
    /// </summary>
    public class VceLogCargaDetHelper : HelperBase
    {
        public VceLogCargaDetHelper(): base(Consultas.VceLogCargaDetSql)
        {
        }

        public VceLogCargaDetDTO Create(IDataReader dr)
        {
            VceLogCargaDetDTO entity = new VceLogCargaDetDTO();

            int iCrlcdnroregistros = dr.GetOrdinal(this.Crlcdnroregistros);
            if (!dr.IsDBNull(iCrlcdnroregistros)) entity.Crlcdnroregistros = Convert.ToInt32(dr.GetValue(iCrlcdnroregistros));

            int iCrlcdusuimport = dr.GetOrdinal(this.Crlcdusuimport);
            if (!dr.IsDBNull(iCrlcdusuimport)) entity.Crlcdusuimport = dr.GetString(iCrlcdusuimport);

            int iCrlcdhoraimport = dr.GetOrdinal(this.Crlcdhoraimport);
            if (!dr.IsDBNull(iCrlcdhoraimport)) entity.Crlcdhoraimport = dr.GetDateTime(iCrlcdhoraimport);

            int iCrlcccodi = dr.GetOrdinal(this.Crlcccodi);
            if (!dr.IsDBNull(iCrlcccodi)) entity.Crlcccodi = Convert.ToInt32(dr.GetValue(iCrlcccodi));

            return entity;
        }


        #region Mapeo de Campos

        public string Crlcdnroregistros = "CRLCDNROREGISTROS";
        public string Crlcdusuimport = "CRLCDUSUIMPORT";
        public string Crlcdhoraimport = "CRLCDHORAIMPORT";
        public string Crlcccodi = "CRLCCCODI";

        //Adicionales
        public string Crlccorden = "CRLCCORDEN";
        public string Crlccentidad = "CRLCCENTIDAD";
        public string Crlccnombtabla = "CRLCCNOMBTABLA";
        public string Pecacodi = "PECACODI";
        public string Fecultactualizacion = "FECULTACTUALIZACION";

        #endregion

        public string SqlListDetalle
        {
            get { return base.GetSqlXml("ListDetalle"); }
        }

        public string SqlSaveDetalle
        {
            get { return base.GetSqlXml("SaveDetalle"); }
        }

        public string SqlDeleteDetPeriodo
        {
            get { return base.GetSqlXml("DeleteDetPeri"); }
        }
    }
}
