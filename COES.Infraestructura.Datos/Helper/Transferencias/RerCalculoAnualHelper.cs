using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla RER_CALCULO_ANUAL
    /// </summary>
    public class RerCalculoAnualHelper : HelperBase
    {
        #region Mapeo de Campos
        public string Rercacodi = "RERCACODI";
        public string Emprcodi = "EMPRCODI";
        public string Equicodi = "EQUICODI";
        public string Reravcodi = "RERAVCODI";
        public string Rercaippi = "RERCAIPPI";
        public string Rercaippo = "RERCAIPPO";
        public string Rercataradjbase = "RERCATARADJBASE";
        public string Rercafaccorreccion = "RERCAFACCORRECCION";
        public string Rercafacactanterior = "RERCAFACACTANTERIOR";
        public string Rercafacactualizacion = "RERCAFACACTUALIZACION";
        public string Rercataradj = "RERCATARADJ";
        public string Rercacomment = "RERCACOMMENT";
        public string Rercausucreacion = "RERCAUSUCREACION";
        public string Rercafeccreacion = "RERCAFECCREACION";

        //Additional
        public string Emprnomb = "EMPRNOMB";
        public string Equinomb = "EQUINOMB";
        public string Reravaniotarif = "RERAVANIOTARIF";
        public string Reravversion = "RERAVVERSION";
        #endregion

        public RerCalculoAnualHelper() : base(Consultas.RerCalculoAnualSql)
        {
        }

        public RerCalculoAnualDTO Create(IDataReader dr)
        {
            RerCalculoAnualDTO entity = new RerCalculoAnualDTO();

            int iRercacodi = dr.GetOrdinal(this.Rercacodi);
            if (!dr.IsDBNull(iRercacodi)) entity.Rercacodi = Convert.ToInt32(dr.GetValue(iRercacodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iReravcodi = dr.GetOrdinal(this.Reravcodi);
            if (!dr.IsDBNull(iReravcodi)) entity.Reravcodi = Convert.ToInt32(dr.GetValue(iReravcodi));

            int iRercaippi = dr.GetOrdinal(this.Rercaippi);
            if (!dr.IsDBNull(iRercaippi)) entity.Rercaippi = dr.GetDecimal(iRercaippi);

            int iRercaippo = dr.GetOrdinal(this.Rercaippo);
            if (!dr.IsDBNull(iRercaippo)) entity.Rercaippo = dr.GetDecimal(iRercaippo);

            int iRercataradjbase = dr.GetOrdinal(this.Rercataradjbase);
            if (!dr.IsDBNull(iRercataradjbase)) entity.Rercataradjbase = dr.GetDecimal(iRercataradjbase);

            int iRercafaccorreccion = dr.GetOrdinal(this.Rercafaccorreccion);
            if (!dr.IsDBNull(iRercafaccorreccion)) entity.Rercafaccorreccion = dr.GetDecimal(iRercafaccorreccion);

            int iRercafacactanterior = dr.GetOrdinal(this.Rercafacactanterior);
            if (!dr.IsDBNull(iRercafacactanterior)) entity.Rercafacactanterior = dr.GetDecimal(iRercafacactanterior);

            int iRercafacactualizacion = dr.GetOrdinal(this.Rercafacactualizacion);
            if (!dr.IsDBNull(iRercafacactualizacion)) entity.Rercafacactualizacion = dr.GetDecimal(iRercafacactualizacion);

            int iRercataradj = dr.GetOrdinal(this.Rercataradj);
            if (!dr.IsDBNull(iRercataradj)) entity.Rercataradj = dr.GetDecimal(iRercataradj);

            int iRercacomment = dr.GetOrdinal(this.Rercacomment);
            if (!dr.IsDBNull(iRercacomment)) entity.Rercacomment = dr.GetString(iRercacomment);

            int iRercausucreacion = dr.GetOrdinal(this.Rercausucreacion);
            if (!dr.IsDBNull(iRercausucreacion)) entity.Rercausucreacion = dr.GetString(iRercausucreacion);

            int iRercafeccreacion = dr.GetOrdinal(this.Rercafeccreacion);
            if (!dr.IsDBNull(iRercafeccreacion)) entity.Rercafeccreacion = dr.GetDateTime(iRercafeccreacion);

            return entity;
        }

        public RerCalculoAnualDTO CreateByAnnioAndVersion(IDataReader dr)
        {
            RerCalculoAnualDTO entity = Create(dr);

            int iEmprnomb = dr.GetOrdinal(this.Emprnomb);
            if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

            int iEquinomb = dr.GetOrdinal(this.Equinomb);
            if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

            int iReravaniotarif = dr.GetOrdinal(this.Reravaniotarif);
            if (!dr.IsDBNull(iReravaniotarif)) entity.Reravaniotarif = Convert.ToInt32(dr.GetValue(iReravaniotarif));

            int iReravversion = dr.GetOrdinal(this.Reravversion);
            if (!dr.IsDBNull(iReravversion)) entity.Reravversion = dr.GetString(iReravversion);

            return entity;
        }

        public string SqlDeleteByAnioVersion
        {
            get { return base.GetSqlXml("DeleteByAnioVersion"); }
        }

        public string SqlGetByAnioAndVersion
        {
            get { return base.GetSqlXml("GetByAnioAndVersion"); }
        }
        
    }
}

