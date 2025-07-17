using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla IN_PROGRAMACION
    /// </summary>
    public class InProgramacionHelper : HelperBase
    {
        public InProgramacionHelper() : base(Consultas.InProgramacionSql)
        {

        }

        public InProgramacionDTO CreateProgramacion(IDataReader dr)
        {
            InProgramacionDTO entity = new InProgramacionDTO();

            int iProgrcodi = dr.GetOrdinal(this.Progrcodi);
            if (!dr.IsDBNull(iProgrcodi)) entity.Progrcodi = Convert.ToInt32(dr.GetValue(iProgrcodi));

            int iEvenclasecodi = dr.GetOrdinal(this.Evenclasecodi);
            if (!dr.IsDBNull(iEvenclasecodi)) entity.Evenclasecodi = Convert.ToInt32(dr.GetValue(iEvenclasecodi));

            int iProgrnomb = dr.GetOrdinal(this.Progrnomb);
            if (!dr.IsDBNull(iProgrnomb)) entity.Progrnomb = dr.GetString(iProgrnomb);

            int iPrograbrev = dr.GetOrdinal(this.Prograbrev);
            if (!dr.IsDBNull(iPrograbrev)) entity.Prograbrev = dr.GetString(iPrograbrev);

            int iProgrfechaini = dr.GetOrdinal(this.Progrfechaini);
            if (!dr.IsDBNull(iProgrfechaini)) entity.Progrfechaini = dr.GetDateTime(iProgrfechaini);

            int iProgrfechafin = dr.GetOrdinal(this.Progrfechafin);
            if (!dr.IsDBNull(iProgrfechafin)) entity.Progrfechafin = dr.GetDateTime(iProgrfechafin);

            int iProgrversion = dr.GetOrdinal(this.Progrversion);
            if (!dr.IsDBNull(iProgrversion)) entity.Progrversion = Convert.ToInt32(dr.GetValue(iProgrversion));

            int iProgrsololectura = dr.GetOrdinal(this.Progrsololectura);
            if (!dr.IsDBNull(iProgrsololectura)) entity.Progrsololectura = Convert.ToInt32(dr.GetValue(iProgrsololectura));

            int iProgrfechalim = dr.GetOrdinal(this.Progrfechalim);
            if (!dr.IsDBNull(iProgrfechalim)) entity.Progrfechalim = dr.GetDateTime(iProgrfechalim);

            int iProgrusucreacion = dr.GetOrdinal(this.Progrusucreacion);
            if (!dr.IsDBNull(iProgrusucreacion)) entity.Progrusucreacion = dr.GetString(iProgrusucreacion);

            int iProgrfeccreacion = dr.GetOrdinal(this.Progrfeccreacion);
            if (!dr.IsDBNull(iProgrfeccreacion)) entity.Progrfeccreacion = dr.GetDateTime(iProgrfeccreacion);

            int iProgrusuaprob = dr.GetOrdinal(this.Progrusuaprob);
            if (!dr.IsDBNull(iProgrusuaprob)) entity.Progrusuaprob = dr.GetString(iProgrusuaprob);

            int iProgrfecaprob = dr.GetOrdinal(this.Progrfecaprob);
            if (!dr.IsDBNull(iProgrfecaprob)) entity.Progrfecaprob = dr.GetDateTime(iProgrfecaprob);

            int iProgresaprobadorev = dr.GetOrdinal(this.Progresaprobadorev);
            if (!dr.IsDBNull(iProgresaprobadorev)) entity.Progresaprobadorev = Convert.ToInt32(dr.GetValue(iProgresaprobadorev));

            int iProgrmaxfecreversion = dr.GetOrdinal(this.Progrmaxfecreversion);
            if (!dr.IsDBNull(iProgrmaxfecreversion)) entity.Progrmaxfecreversion = dr.GetDateTime(iProgrmaxfecreversion);

            int iProgrusuhabrev = dr.GetOrdinal(this.Progrusuhabrev);
            if (!dr.IsDBNull(iProgrusuhabrev)) entity.Progrusuhabrev = dr.GetString(iProgrusuhabrev);

            int iProgrfechabrev = dr.GetOrdinal(this.Progrfechabrev);
            if (!dr.IsDBNull(iProgrfechabrev)) entity.Progrfechabrev = dr.GetDateTime(iProgrfechabrev);

            int iProgrusuaprobrev = dr.GetOrdinal(this.Progrusuaprobrev);
            if (!dr.IsDBNull(iProgrusuaprobrev)) entity.Progrusuaprobrev = dr.GetString(iProgrusuaprobrev);

            int iProgrfecaprobrev = dr.GetOrdinal(this.Progrfecaprobrev);
            if (!dr.IsDBNull(iProgrfecaprobrev)) entity.Progrfecaprobrev = dr.GetDateTime(iProgrfecaprobrev);

            return entity;
        }

        #region Mapeo de Campos Programación
        public string Progrcodi = "PROGRCODI";
        public string Evenclasecodi = "EVENCLASECODI";
        public string Progrnomb = "PROGRNOMB";
        public string Prograbrev = "PROGRABREV";
        public string Progrfechaini = "PROGRFECHAINI";
        public string Progrfechafin = "PROGRFECHAFIN";
        public string Progrversion = "PROGRVERSION";
        public string Progrsololectura = "PROGRSOLOLECTURA";
        public string Progrfechalim = "PROGRFECHALIM";
        public string Progrusucreacion = "PROGRUSUCREACION";
        public string Progrfeccreacion = "PROGRFECCREACION";
        public string Progrusuaprob = "PROGRUSUAPROB";
        public string Progrfecaprob = "PROGRFECAPROB";
        public string Progresaprobadorev = "PROGRESAPROBADOREV";
        public string Progrmaxfecreversion = "PROGRMAXFECREVERSION";
        public string Progrusuhabrev = "PROGRUSUHABREV";
        public string Progrfechabrev = "PROGRFECHABREV";
        public string Progrusuaprobrev = "PROGRUSUAPROBREV";
        public string Progrfecaprobrev = "PROGRFECAPROBREV";

        #endregion

        public string Evenclasedesc = "EVENCLASEDESC";
        public string TotalRegistro = "TOTALREGISTRO";
        public string TotalRevertidos = "TOTALREVERTIDOS";

        public string SqlHacerSoloLecturaProgramacion
        {
            get { return GetSqlXml("HacerSoloLecturaProgramacion"); }
        }

        public string SqlGetProgramacionesByIdTipoProgramacion
        {
            get { return GetSqlXml("GetProgramacionesByIdTipoProgramacion"); }
        }

        public string SqlObtenerIdProgramacionXFecIniYTipoPro
        {
            get { return base.GetSqlXml("ObtenerIdProgramacionXFecIniYTipoPro"); }
        }

        public string SqlObtenerProgramacionesPorId
        {
            get { return base.GetSqlXml("ObtenerProgramacionesPorId"); }
        }

        public string SqlActualizarAprobadoReversion
        {
            get { return GetSqlXml("ActualizarAprobadoReversion"); }
        }

        public string SqlHabilitarReversion
        {
            get { return GetSqlXml("HabilitarReversion"); }
        }

    }
}
