using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SI_AMPLAZOENVIO
    /// </summary>
    public class SiAmplazoenvioHelper : HelperBase
    {
        public SiAmplazoenvioHelper()
            : base(Consultas.SiAmplazoenvioSql)
        {
        }

        public SiAmplazoenvioDTO Create(IDataReader dr)
        {
            SiAmplazoenvioDTO entity = new SiAmplazoenvioDTO();

            int iAmplzcodi = dr.GetOrdinal(this.Amplzcodi);
            if (!dr.IsDBNull(iAmplzcodi)) entity.Amplzcodi = Convert.ToInt32(dr.GetValue(iAmplzcodi));

            int iFdatcodi = dr.GetOrdinal(this.Fdatcodi);
            if (!dr.IsDBNull(iFdatcodi)) entity.Fdatcodi = Convert.ToInt32(dr.GetValue(iFdatcodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iAmplzfecha = dr.GetOrdinal(this.Amplzfecha);
            if (!dr.IsDBNull(iAmplzfecha)) entity.Amplzfecha = dr.GetDateTime(iAmplzfecha);

            int iAmplzfechaperiodo = dr.GetOrdinal(this.Amplzfechaperiodo);
            if (!dr.IsDBNull(iAmplzfechaperiodo)) entity.Amplzfechaperiodo = dr.GetDateTime(iAmplzfechaperiodo);

            int iAmplzusucreacion = dr.GetOrdinal(this.Amplzusucreacion);
            if (!dr.IsDBNull(iAmplzusucreacion)) entity.Amplzusucreacion = dr.GetString(iAmplzusucreacion);

            int iAmplzfeccreacion = dr.GetOrdinal(this.Amplzfeccreacion);
            if (!dr.IsDBNull(iAmplzfeccreacion)) entity.Amplzfeccreacion = dr.GetDateTime(iAmplzfeccreacion);

            int iAmplzusumodificacion = dr.GetOrdinal(this.Amplzusumodificacion);
            if (!dr.IsDBNull(iAmplzusumodificacion)) entity.Amplzusumodificacion = dr.GetString(iAmplzusumodificacion);

            int iAmplzfecmodificacion = dr.GetOrdinal(this.Amplzfecmodificacion);
            if (!dr.IsDBNull(iAmplzfecmodificacion)) entity.Amplzfecmodificacion = dr.GetDateTime(iAmplzfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Amplzcodi = "AMPLZCODI";
        public string Fdatcodi = "FDATCODI";
        public string Emprcodi = "EMPRCODI";
        public string Amplzfecha = "AMPLZFECHA";
        public string Amplzfechaperiodo = "AMPLZFECHAPERIODO";
        public string Amplzusucreacion = "AMPLZUSUCREACION";
        public string Amplzfeccreacion = "AMPLZFECCREACION";
        public string Amplzusumodificacion = "AMPLZUSUMODIFICACION";
        public string Amplzfecmodificacion = "AMPLZFECMODIFICACION";

        public string Fdatnombre = "FDATNOMBRE";
        public string Emprnomb = "Emprnomb";

        #endregion

        #region Consultas

        public string SqlListarAmpliacionMultiple
        {
            get { return base.GetSqlXml("ListarAmpliacionMultiple"); }
        }

        public string SqlGetByIdCriteria
        {
            get { return base.GetSqlXml("GetByIdCriteria"); }
        }

        #endregion
    }
}
