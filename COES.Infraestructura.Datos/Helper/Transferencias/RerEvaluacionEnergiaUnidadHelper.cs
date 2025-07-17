using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla RER_EVALUACION_ENERGIAUNIDAD
    /// </summary>
    public class RerEvaluacionEnergiaUnidadHelper : HelperBase
    {
        #region Mapeo de columnas

        //table
        public string Rereeucodi = "REREEUCODI";
        public string Reresecodi = "RERESECODI";
        public string Rerevacodi = "REREVACODI";
        public string Rereucodi = "REREUCODI";
        public string Rersedcodi = "RERSEDCODI";
        public string Equicodi = "EQUICODI";
        public string Rereeuenergiaunidad = "REREEUENERGIAUNIDAD";
        public string Rereeutotenergia = "REREEUTOTENERGIA";
        public string Rereeuusucreacionext = "REREEUUSUCREACIONEXT";
        public string Rereeufeccreacionext = "REREEUFECCREACIONEXT";
        public string Rereeuusucreacion = "REREEUUSUCREACION";
        public string Rereeufeccreacion = "REREEUFECCREACION";

        //Additional
        public string Equinomb = "EQUINOMB";
        #endregion

        public RerEvaluacionEnergiaUnidadHelper() : base(Consultas.RerEvaluacionEnergiaUnidadSql)
        {
        }

        public RerEvaluacionEnergiaUnidadDTO CreateById(IDataReader dr)
        {
            RerEvaluacionEnergiaUnidadDTO entity = new RerEvaluacionEnergiaUnidadDTO();
            SetCreate(dr, entity);

            int iEquinomb = dr.GetOrdinal(this.Equinomb);
            if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

            return entity;
        }

        public RerEvaluacionEnergiaUnidadDTO CreateByList(IDataReader dr)
        {
            RerEvaluacionEnergiaUnidadDTO entity = new RerEvaluacionEnergiaUnidadDTO();
            SetCreate(dr, entity);

            return entity;
        }

        public RerEvaluacionEnergiaUnidadDTO CreateByCriteria(IDataReader dr)
        {
            RerEvaluacionEnergiaUnidadDTO entity = new RerEvaluacionEnergiaUnidadDTO();
            SetCreate(dr, entity);

            int iEquinomb = dr.GetOrdinal(this.Equinomb);
            if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

            return entity;
        }

        private void SetCreate(IDataReader dr, RerEvaluacionEnergiaUnidadDTO entity)
        {
            int iRereeucodi = dr.GetOrdinal(this.Rereeucodi);
            if (!dr.IsDBNull(iRereeucodi)) entity.Rereeucodi = Convert.ToInt32(dr.GetValue(iRereeucodi));

            int iReresecodi = dr.GetOrdinal(this.Reresecodi);
            if (!dr.IsDBNull(iReresecodi)) entity.Reresecodi = Convert.ToInt32(dr.GetValue(iReresecodi));

            int iRerevacodi = dr.GetOrdinal(this.Rerevacodi);
            if (!dr.IsDBNull(iRerevacodi)) entity.Rerevacodi = Convert.ToInt32(dr.GetValue(iRerevacodi));

            int iRereucodi = dr.GetOrdinal(this.Rereucodi);
            if (!dr.IsDBNull(iRereucodi)) entity.Rereucodi = Convert.ToInt32(dr.GetValue(iRereucodi));

            int iRersedcodi = dr.GetOrdinal(this.Rersedcodi);
            if (!dr.IsDBNull(iRersedcodi)) entity.Rersedcodi = Convert.ToInt32(dr.GetValue(iRersedcodi));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iRereeuenergiaunidad = dr.GetOrdinal(this.Rereeuenergiaunidad);
            if (!dr.IsDBNull(iRereeuenergiaunidad)) entity.Rereeuenergiaunidad = dr.GetString(iRereeuenergiaunidad);

            int iRereeuusucreacionext = dr.GetOrdinal(this.Rereeuusucreacionext);
            if (!dr.IsDBNull(iRereeuusucreacionext)) entity.Rereeuusucreacionext = dr.GetString(iRereeuusucreacionext);

            int iRereeufeccreacionext = dr.GetOrdinal(this.Rereeufeccreacionext);
            if (!dr.IsDBNull(iRereeufeccreacionext)) entity.Rereeufeccreacionext = dr.GetDateTime(iRereeufeccreacionext);

            int iRereeuusucreacion = dr.GetOrdinal(this.Rereeuusucreacion);
            if (!dr.IsDBNull(iRereeuusucreacion)) entity.Rereeuusucreacion = dr.GetString(iRereeuusucreacion);

            int iRereeufeccreacion = dr.GetOrdinal(this.Rereeufeccreacion);
            if (!dr.IsDBNull(iRereeufeccreacion)) entity.Rereeufeccreacion = dr.GetDateTime(iRereeufeccreacion);
        }

    }
}