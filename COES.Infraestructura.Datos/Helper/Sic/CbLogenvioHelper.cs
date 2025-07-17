using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CB_LOGENVIO
    /// </summary>
    public class CbLogenvioHelper : HelperBase
    {
        public CbLogenvioHelper() : base(Consultas.CbLogenvioSql)
        {
        }

        public CbLogenvioDTO Create(IDataReader dr)
        {
            CbLogenvioDTO entity = new CbLogenvioDTO();

            int iLogenvcodi = dr.GetOrdinal(this.Logenvcodi);
            if (!dr.IsDBNull(iLogenvcodi)) entity.Logenvcodi = Convert.ToInt32(dr.GetValue(iLogenvcodi));

            int iEstenvcodi = dr.GetOrdinal(this.Estenvcodi);
            if (!dr.IsDBNull(iEstenvcodi)) entity.Estenvcodi = Convert.ToInt32(dr.GetValue(iEstenvcodi));

            int iCbenvcodi = dr.GetOrdinal(this.Cbenvcodi);
            if (!dr.IsDBNull(iCbenvcodi)) entity.Cbenvcodi = Convert.ToInt32(dr.GetValue(iCbenvcodi));

            int iLogenvusucreacion = dr.GetOrdinal(this.Logenvusucreacion);
            if (!dr.IsDBNull(iLogenvusucreacion)) entity.Logenvusucreacion = dr.GetString(iLogenvusucreacion);

            int iLogenvfeccreacion = dr.GetOrdinal(this.Logenvfeccreacion);
            if (!dr.IsDBNull(iLogenvfeccreacion)) entity.Logenvfeccreacion = dr.GetDateTime(iLogenvfeccreacion);

            int iLogenvobservacion = dr.GetOrdinal(this.Logenvobservacion);
            if (!dr.IsDBNull(iLogenvobservacion)) entity.Logenvobservacion = dr.GetString(iLogenvobservacion);

            int iLogenvfecrecepcion= dr.GetOrdinal(this.Logenvfecrecepcion);
            if (!dr.IsDBNull(iLogenvfecrecepcion)) entity.Logenvfecrecepcion = dr.GetDateTime(iLogenvfecrecepcion);

            int iLogenvfeclectura = dr.GetOrdinal(this.Logenvfeclectura);
            if (!dr.IsDBNull(iLogenvfeclectura)) entity.Logenvfeclectura = dr.GetDateTime(iLogenvfeclectura);

            int iLogenvusurecepcion = dr.GetOrdinal(this.Logenvusurecepcion);
            if (!dr.IsDBNull(iLogenvusurecepcion)) entity.Logenvusurecepcion = dr.GetString(iLogenvusurecepcion);

            int iLogenvusulectura = dr.GetOrdinal(this.Logenvusulectura);
            if (!dr.IsDBNull(iLogenvusulectura)) entity.Logenvusulectura = dr.GetString(iLogenvusulectura);

            int iLogenvplazo = dr.GetOrdinal(this.Logenvplazo);
            if (!dr.IsDBNull(iLogenvplazo)) entity.Logenvplazo = Convert.ToInt32(dr.GetValue(iLogenvplazo));

            return entity;
        }


        #region Mapeo de Campos

        public string Logenvcodi = "LOGENVCODI";
        public string Estenvcodi = "ESTENVCODI";
        public string Cbenvcodi = "CBENVCODI";
        public string Logenvusucreacion = "LOGENVUSUCREACION";
        public string Logenvfeccreacion = "LOGENVFECCREACION";
        public string Logenvobservacion = "LOGENVOBSERVACION";
        public string Logenvusurecepcion = "LOGENVUSURECEPCION";
        public string Logenvfecrecepcion = "LOGENVFECRECEPCION";
        public string Logenvusulectura = "LOGENVUSULECTURA";
        public string Logenvfeclectura = "LOGENVFECLECTURA";
        public string Logenvplazo = "LOGENVPLAZO";

        public string Emprnomb = "EMPRNOMB";
        public string Estenvnomb = "ESTENVNOMB";

        #endregion

        public string SqlGetByEnviosYEstado
        {
            get { return base.GetSqlXml("GetByEnviosYEstado"); }
        }

        public string SqlGetLogGaseososPorEmpresaYRango
        {
            get { return base.GetSqlXml("GetLogGaseososPorEmpresaYRango"); }
        }
    }
}
