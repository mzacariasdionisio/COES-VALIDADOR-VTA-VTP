using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla VCR_RESERVASIGN
    /// </summary>
    public class VcrReservasignHelper : HelperBase
    {
        public VcrReservasignHelper(): base(Consultas.VcrReservasignSql)
        {
        }

        public VcrReservasignDTO Create(IDataReader dr)
        {
            VcrReservasignDTO entity = new VcrReservasignDTO();

            int iVcrasgcodi = dr.GetOrdinal(this.Vcrasgcodi);
            if (!dr.IsDBNull(iVcrasgcodi)) entity.Vcrasgcodi = Convert.ToInt32(dr.GetValue(iVcrasgcodi));

            int iVcrecacodi = dr.GetOrdinal(this.Vcrecacodi);
            if (!dr.IsDBNull(iVcrecacodi)) entity.Vcrecacodi = Convert.ToInt32(dr.GetValue(iVcrecacodi));

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            int iGruponomb = dr.GetOrdinal(this.Gruponomb);
            if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

            int iVcrasgfecha = dr.GetOrdinal(this.Vcrasgfecha);
            if (!dr.IsDBNull(iVcrasgfecha)) entity.Vcrasgfecha = dr.GetDateTime(iVcrasgfecha);

            int iVcrasghorinicio = dr.GetOrdinal(this.Vcrasghorinicio);
            if (!dr.IsDBNull(iVcrasghorinicio)) entity.Vcrasghorinicio = dr.GetDateTime(iVcrasghorinicio);

            int iVcrasghorfinal = dr.GetOrdinal(this.Vcrasghorfinal);
            if (!dr.IsDBNull(iVcrasghorfinal)) entity.Vcrasghorfinal = dr.GetDateTime(iVcrasghorfinal);

            int iVcrasgreservasign = dr.GetOrdinal(this.Vcrasgreservasign);
            if (!dr.IsDBNull(iVcrasgreservasign)) entity.Vcrasgreservasign = dr.GetDecimal(iVcrasgreservasign);

            int iVcrasgtipo = dr.GetOrdinal(this.Vcrasgtipo);
            if (!dr.IsDBNull(iVcrasgtipo)) entity.Vcrasgtipo = dr.GetString(iVcrasgtipo);

            int iVcrasgusucreacion = dr.GetOrdinal(this.Vcrasgusucreacion);
            if (!dr.IsDBNull(iVcrasgusucreacion)) entity.Vcrasgusucreacion = dr.GetString(iVcrasgusucreacion);

            int iVcrasgfeccreacion = dr.GetOrdinal(this.Vcrasgfeccreacion);
            if (!dr.IsDBNull(iVcrasgfeccreacion)) entity.Vcrasgfeccreacion = dr.GetDateTime(iVcrasgfeccreacion);

            int iVcrasgreservasignb = dr.GetOrdinal(this.Vcrasgreservasignb);
            if (!dr.IsDBNull(iVcrasgreservasignb)) entity.Vcrasgreservasignb = dr.GetDecimal(iVcrasgreservasignb);

            return entity;
        }


        #region Mapeo de Campos

        public string Vcrasgcodi = "VCRASGCODI";
        public string Vcrecacodi = "VCRECACODI";
        public string Grupocodi = "GRUPOCODI";
        public string Gruponomb = "GRUPONOMB";
        public string Vcrasgfecha = "VCRASGFECHA";
        public string Vcrasghorinicio = "VCRASGHORINICIO";
        public string Vcrasghorfinal = "VCRASGHORFINAL";
        public string Vcrasgreservasign = "VCRASGRESERVASIGN";
        public string Vcrasgtipo = "VCRASGTIPO";
        public string Vcrasgusucreacion = "VCRASGUSUCREACION";
        public string Vcrasgfeccreacion = "VCRASGFECCREACION";
        public string Vcrasgreservasignb = "VCRASGRESERVASIGNB";

        #endregion

        //Metodos de la tabla
        public string SqlGetByCriteriaURSDia
        {
            get { return base.GetSqlXml("GetByCriteriaURSDia"); }
        }

        public string SqlGetByCriteriaDia
        {
            get { return base.GetSqlXml("GetByCriteriaDia"); }
        }
    }
}
