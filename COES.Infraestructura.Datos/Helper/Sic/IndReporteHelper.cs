using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla IND_REPORTE
    /// </summary>
    public class IndReporteHelper : HelperBase
    {
        public IndReporteHelper() : base(Consultas.IndReporteSql)
        {
        }

        public IndReporteDTO Create(IDataReader dr)
        {
            IndReporteDTO entity = new IndReporteDTO();

            int iIrptcodi = dr.GetOrdinal(this.Irptcodi);
            if (!dr.IsDBNull(iIrptcodi)) entity.Irptcodi = Convert.ToInt32(dr.GetValue(iIrptcodi));

            int iIrecacodi = dr.GetOrdinal(this.Irecacodi);
            if (!dr.IsDBNull(iIrecacodi)) entity.Irecacodi = Convert.ToInt32(dr.GetValue(iIrecacodi));

            int iIcuacodi = dr.GetOrdinal(this.Icuacodi);
            if (!dr.IsDBNull(iIcuacodi)) entity.Icuacodi = Convert.ToInt32(dr.GetValue(iIcuacodi));

            int iIrptestado = dr.GetOrdinal(this.Irptestado);
            if (!dr.IsDBNull(iIrptestado)) entity.Irptestado = dr.GetString(iIrptestado);

            int iIrpttipo = dr.GetOrdinal(this.Irpttipo);
            if (!dr.IsDBNull(iIrpttipo)) entity.Irpttipo = dr.GetString(iIrpttipo);

            int iIrpttiempo = dr.GetOrdinal(this.Irpttiempo);
            if (!dr.IsDBNull(iIrpttiempo)) entity.Irpttiempo = dr.GetString(iIrpttiempo);

            int iIrptmedicionorigen = dr.GetOrdinal(this.Irptmedicionorigen);
            if (!dr.IsDBNull(iIrptmedicionorigen)) entity.Irptmedicionorigen = dr.GetString(iIrptmedicionorigen);

            int iIrptnumversion = dr.GetOrdinal(this.Irptnumversion);
            if (!dr.IsDBNull(iIrptnumversion)) entity.Irptnumversion = Convert.ToInt32(dr.GetValue(iIrptnumversion));

            int iIrptesfinal = dr.GetOrdinal(this.Irptesfinal);
            if (!dr.IsDBNull(iIrptesfinal)) entity.Irptesfinal = Convert.ToInt32(dr.GetValue(iIrptesfinal));

            int iIrptreporteold = dr.GetOrdinal(this.Irptreporteold);
            if (!dr.IsDBNull(iIrptreporteold)) entity.Irptreporteold = Convert.ToInt32(dr.GetValue(iIrptreporteold));

            int iIrptusucreacion = dr.GetOrdinal(this.Irptusucreacion);
            if (!dr.IsDBNull(iIrptusucreacion)) entity.Irptusucreacion = dr.GetString(iIrptusucreacion);

            int iIrptfeccreacion = dr.GetOrdinal(this.Irptfeccreacion);
            if (!dr.IsDBNull(iIrptfeccreacion)) entity.Irptfeccreacion = dr.GetDateTime(iIrptfeccreacion);

            int iIrptusumodificacion = dr.GetOrdinal(this.Irptusumodificacion);
            if (!dr.IsDBNull(iIrptusumodificacion)) entity.Irptusumodificacion = dr.GetString(iIrptusumodificacion);

            int iIrptfecmodificacion = dr.GetOrdinal(this.Irptfecmodificacion);
            if (!dr.IsDBNull(iIrptfecmodificacion)) entity.Irptfecmodificacion = dr.GetDateTime(iIrptfecmodificacion);

            return entity;
        }

        #region Mapeo de Campos

        public string Irptcodi = "IRPTCODI";
        public string Irecacodi = "IRECACODI";
        public string Icuacodi = "ICUACODI";
        public string Irptestado = "IRPTESTADO";
        public string Irpttipo = "IRPTTIPO";
        public string Irpttiempo = "IRPTTIEMPO";
        public string Irptmedicionorigen = "IRPTMEDICIONORIGEN";
        public string Irptnumversion = "IRPTNUMVERSION";
        public string Irptesfinal = "IRPTESFINAL";
        public string Irptreporteold = "IRPTREPORTEOLD";
        public string Irptusucreacion = "IRPTUSUCREACION";
        public string Irptfeccreacion = "IRPTFECCREACION";
        public string Irptusumodificacion = "IRPTUSUMODIFICACION";
        public string Irptfecmodificacion = "IRPTFECMODIFICACION";

        public string Iperinombre = "IPERINOMBRE";
        public string Irecanombre = "IRECANOMBRE";
        public string Irecafechaini = "IRECAFECHAINI";
        public string Irecafechafin = "IRECAFECHAFIN";
        public string Irecainforme = "IRECAINFORME";

        #endregion

        public string SqlUpdateAprobar
        {
            get { return base.GetSqlXml("UpdateAprobar"); }
        }

        public string SqlUpdateHistorico
        {
            get { return base.GetSqlXml("UpdateHistorico"); }
        }

        public string SqlReporteParaPFR
        {
            get { return base.GetSqlXml("LstReporteParaPFR"); }
        }
    }
}
