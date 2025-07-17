using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla RER_FAC_PER_MED_DET
    /// </summary>
    public class RerFacPerMedDetHelper : HelperBase
    {
        public RerFacPerMedDetHelper() : base(Consultas.RerFacPerMedDetSql)
        {
        }

        public RerFacPerMedDetDTO Create(IDataReader dr)
        {
            RerFacPerMedDetDTO entity = new RerFacPerMedDetDTO();
            SetCreate(dr, entity);

            return entity;
        }

        public RerFacPerMedDetDTO CreateByRangeDate(IDataReader dr)
        {
            RerFacPerMedDetDTO entity = new RerFacPerMedDetDTO();
            SetCreate(dr, entity);

            int iRerfpmdesde = dr.GetOrdinal(this.Rerfpmdesde);
            if (!dr.IsDBNull(iRerfpmdesde)) entity.Rerfpmdesde = dr.GetDateTime(iRerfpmdesde);

            int iRerfpmhasta = dr.GetOrdinal(this.Rerfpmhasta);
            if (!dr.IsDBNull(iRerfpmhasta)) entity.Rerfpmhasta = dr.GetDateTime(iRerfpmhasta);

            return entity;
        }

        private void SetCreate(IDataReader dr, RerFacPerMedDetDTO entity)
        {
            int iRerfpdcodi = dr.GetOrdinal(this.Rerfpdcodi);
            if (!dr.IsDBNull(iRerfpdcodi)) entity.Rerfpdcodi = Convert.ToInt32(dr.GetValue(iRerfpdcodi));

            int iRerfpmcodi = dr.GetOrdinal(this.Rerfpmcodi);
            if (!dr.IsDBNull(iRerfpmcodi)) entity.Rerfpmcodi = Convert.ToInt32(dr.GetValue(iRerfpmcodi));

            int iCodentcodi = dr.GetOrdinal(this.Codentcodi);
            if (!dr.IsDBNull(iCodentcodi)) entity.Codentcodi = Convert.ToInt32(dr.GetValue(iCodentcodi));

            int iBarrcodi = dr.GetOrdinal(this.Barrcodi);
            if (!dr.IsDBNull(iBarrcodi)) entity.Barrcodi = Convert.ToInt32(dr.GetValue(iBarrcodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iRerfpdfactperdida = dr.GetOrdinal(this.Rerfpdfactperdida);
            if (!dr.IsDBNull(iRerfpdfactperdida)) entity.Rerfpdfactperdida = dr.GetDecimal(iRerfpdfactperdida);

            int iRerfpdusucreacion = dr.GetOrdinal(this.Rerfpdusucreacion);
            if (!dr.IsDBNull(iRerfpdusucreacion)) entity.Rerfpdusucreacion = dr.GetString(iRerfpdusucreacion);

            int iRerfpdfeccreacion = dr.GetOrdinal(this.Rerfpdfeccreacion);
            if (!dr.IsDBNull(iRerfpdfeccreacion)) entity.Rerfpdfeccreacion = dr.GetDateTime(iRerfpdfeccreacion);

            int iRerfpdusumodificacion = dr.GetOrdinal(this.Rerfpdusumodificacion);
            if (!dr.IsDBNull(iRerfpdusumodificacion)) entity.Rerfpdusumodificacion = dr.GetString(iRerfpdusumodificacion);

            int iRerfpdfecmodificacion = dr.GetOrdinal(this.Rerfpdfecmodificacion);
            if (!dr.IsDBNull(iRerfpdfecmodificacion)) entity.Rerfpdfecmodificacion = dr.GetDateTime(iRerfpdfecmodificacion);
        }

        #region Mapeo de Campos

        public string Rerfpdcodi = "RERFPDCODI";
        public string Rerfpmcodi = "RERFPMCODI";
        public string Codentcodi = "CODENTCODI";
        public string Barrcodi = "BARRCODI";
        public string Emprcodi = "EMPRCODI";
        public string Equicodi = "EQUICODI";
        public string Rerfpdfactperdida = "RERFPDFACTPERDIDA";
        public string Rerfpdusucreacion = "RERFPDUSUCREACION";
        public string Rerfpdfeccreacion = "RERFPDFECCREACION";
        public string Rerfpdusumodificacion = "RERFPDUSUMODIFICACION";
        public string Rerfpdfecmodificacion = "RERFPDFECMODIFICACION";

        //Additional
        public string Codentcodigo = "CODENTCODIGO";
        public string Barrnombre = "BARRBARRATRANSFERENCIA";
        public string Empresanombre = "EMPRNOMB";
        public string Equiponombre = "EQUINOMB";
        public string Rerfpmdesde = "RERFPMDESDE";
        public string Rerfpmhasta = "RERFPMHASTA";
        #endregion

        public string SqlListByFPM
        {
            get { return base.GetSqlXml("ListByFPM"); }
        }

        public string SqlGetByRangeDate
        {
            get { return base.GetSqlXml("GetByRangeDate"); }
        }

    }
}