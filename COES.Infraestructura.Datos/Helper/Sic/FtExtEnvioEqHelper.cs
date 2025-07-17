using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla FT_EXT_ENVIO_EQ
    /// </summary>
    public class FtExtEnvioEqHelper : HelperBase
    {
        public FtExtEnvioEqHelper() : base(Consultas.FtExtEnvioEqSql)
        {
        }

        public FtExtEnvioEqDTO Create(IDataReader dr)
        {
            FtExtEnvioEqDTO entity = new FtExtEnvioEqDTO();

            int iFteeqcodi = dr.GetOrdinal(this.Fteeqcodi);
            if (!dr.IsDBNull(iFteeqcodi)) entity.Fteeqcodi = Convert.ToInt32(dr.GetValue(iFteeqcodi));

            int iFteeqestado = dr.GetOrdinal(this.Fteeqestado);
            if (!dr.IsDBNull(iFteeqestado)) entity.Fteeqestado = dr.GetString(iFteeqestado);

            int iFtevercodi = dr.GetOrdinal(this.Ftevercodi);
            if (!dr.IsDBNull(iFtevercodi)) entity.Ftevercodi = Convert.ToInt32(dr.GetValue(iFtevercodi));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            int iFtfmtcodi = dr.GetOrdinal(this.Ftfmtcodi);
            if (!dr.IsDBNull(iFtfmtcodi)) entity.Ftfmtcodi = Convert.ToInt32(dr.GetValue(iFtfmtcodi));

            int iFteeqcodiorigen = dr.GetOrdinal(this.Fteeqcodiorigen);
            if (!dr.IsDBNull(iFteeqcodiorigen)) entity.Fteeqcodiorigen = Convert.ToInt32(dr.GetValue(iFteeqcodiorigen));

            int iFteeqflagaprob = dr.GetOrdinal(this.Fteeqflagaprob);
            if (!dr.IsDBNull(iFteeqflagaprob)) entity.Fteeqflagaprob = dr.GetString(iFteeqflagaprob);

            int iFteeqflagespecial = dr.GetOrdinal(this.Fteeqflagespecial);
            if (!dr.IsDBNull(iFteeqflagespecial)) entity.Fteeqflagespecial = Convert.ToInt32(dr.GetValue(iFteeqflagespecial));

            return entity;
        }

        #region Mapeo de Campos

        public string Fteeqcodi = "FTEEQCODI";
        public string Fteeqestado = "FTEEQESTADO";
        public string Ftevercodi = "FTEVERCODI";
        public string Equicodi = "EQUICODI";
        public string Grupocodi = "GRUPOCODI";
        public string Ftfmtcodi = "FTFMTCODI";
        public string Fteeqcodiorigen = "FTEEQCODIORIGEN";
        public string Fteeqflagaprob = "FTEEQFLAGAPROB";
        public string Fteeqflagespecial = "FTEEQFLAGESPECIAL";

        public string Nombreelemento = "NOMBREELEMENTO";
        public string Idelemento = "IDELEMENTO";
        public string Areaelemento = "AREAELEMENTO";
        public string Ftenvcodi = "FTENVCODI";
        public string Nombempresaelemento = "NOMBEMPRESAELEMENTO";
        public string Idempresaelemento = "IDEMPRESAELEMENTO";
        public string Tipoelemento = "TIPOELEMENTO";
        public string Famcodi = "FAMCODI";
        public string Catecodi = "CATECODI";
        public string Ftedatflagmodificado = "FTEDATFLAGMODIFICADO";
        public string Ftitcodi = "FTITCODI";
        public string Ftetcodi = "FTETCODI";
        public string Equipadre = "EQUIPADRE";

        #endregion

        public string SqlListarPorEnvios
        {
            get { return GetSqlXml("ListarPorEnvios"); }
        }

        public string SqlListarPorIds
        {
            get { return GetSqlXml("ListarPorIds"); }
        }

        public string SqlGetByVersionYModificacion
        {
            get { return GetSqlXml("GetByVersionYModificacion"); }
        }

        public string SqlUpdateEstado
        {
            get { return GetSqlXml("UpdateEstado"); }
        }

        public string SqlGetTotalXFormatoExtranet
        {
            get { return GetSqlXml("GetTotalXFormatoExtranet"); }
        }
    }
}
