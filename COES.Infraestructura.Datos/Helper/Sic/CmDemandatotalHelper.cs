using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CM_DEMANDATOTAL
    /// </summary>
    public class CmDemandatotalHelper : HelperBase
    {
        public CmDemandatotalHelper() : base(Consultas.CmDemandatotalSql)
        {
        }

        public CmDemandatotalDTO Create(IDataReader dr)
        {
            CmDemandatotalDTO entity = new CmDemandatotalDTO();

            int iDemacodi = dr.GetOrdinal(this.Demacodi);
            if (!dr.IsDBNull(iDemacodi)) entity.Demacodi = Convert.ToInt32(dr.GetValue(iDemacodi));

            int iDemafecha = dr.GetOrdinal(this.Demafecha);
            if (!dr.IsDBNull(iDemafecha)) entity.Demafecha = dr.GetDateTime(iDemafecha);

            int iDematermica = dr.GetOrdinal(this.Dematermica);
            if (!dr.IsDBNull(iDematermica)) entity.Dematermica = dr.GetDecimal(iDematermica);

            int iDemaintervalo = dr.GetOrdinal(this.Demaintervalo);
            if (!dr.IsDBNull(iDemaintervalo)) entity.Dematermica = Convert.ToInt32(dr.GetValue(iDemaintervalo));

            int iDemahidraulica = dr.GetOrdinal(this.Demahidraulica);
            if (!dr.IsDBNull(iDemahidraulica)) entity.Demahidraulica = dr.GetDecimal(iDemahidraulica);

            int iDematotal = dr.GetOrdinal(this.Dematotal);
            if (!dr.IsDBNull(iDematotal)) entity.Dematotal = dr.GetDecimal(iDematotal);

            int iDemasucreacion = dr.GetOrdinal(this.Demasucreacion);
            if (!dr.IsDBNull(iDemasucreacion)) entity.Demasucreacion = dr.GetString(iDemasucreacion);

            int iDemafeccreacion = dr.GetOrdinal(this.Demafeccreacion);
            if (!dr.IsDBNull(iDemafeccreacion)) entity.Demafeccreacion = dr.GetDateTime(iDemafeccreacion);

            int iDemausumodificacion = dr.GetOrdinal(this.Demausumodificacion);
            if (!dr.IsDBNull(iDemausumodificacion)) entity.Demausumodificacion = dr.GetString(iDemausumodificacion);

            int iDemafecmodificacion = dr.GetOrdinal(this.Demafecmodificacion);
            if (!dr.IsDBNull(iDemafecmodificacion)) entity.Demafecmodificacion = dr.GetDateTime(iDemafecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Demacodi = "DEMACODI";
        public string Demafecha = "DEMAFECHA";
        public string Demaintervalo = "DEMAINTERVALO";
        public string Dematermica = "DEMATERMICA";
        public string Demahidraulica = "DEMAHIDRAULICA";
        public string Dematotal = "DEMATOTAL";
        public string Demasucreacion = "DEMASUCREACION";
        public string Demafeccreacion = "DEMAFECCREACION";
        public string Demausumodificacion = "DEMAUSUMODIFICACION";
        public string Demafecmodificacion = "DEMAFECMODIFICACION";

        #endregion


        public string SqlDeleteByCriteria
        {
            get { return GetSqlXml("DeleteByCriteria"); }
        }

        public string SqlGetByDate
        {
            get { return GetSqlXml("GetByDate"); }
        }

        #region FIT - Aplicativo VTD

        public string SqlGetDemandaTotal
        {
            get { return GetSqlXml("GetDemandaTotal"); }
        }

        #endregion

    }
}
