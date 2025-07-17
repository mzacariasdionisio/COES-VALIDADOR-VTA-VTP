using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla RE_RECHAZO_CARGA
    /// </summary>
    public class ReRechazoCargaHelper : HelperBase
    {
        public ReRechazoCargaHelper() : base(Consultas.ReRechazoCargaSql)
        {
        }

        public ReRechazoCargaDTO Create(IDataReader dr)
        {
            ReRechazoCargaDTO entity = new ReRechazoCargaDTO();

            int iRerccodi = dr.GetOrdinal(this.Rerccodi);
            if (!dr.IsDBNull(iRerccodi)) entity.Rerccodi = Convert.ToInt32(dr.GetValue(iRerccodi));

            int iRepercodi = dr.GetOrdinal(this.Repercodi);
            if (!dr.IsDBNull(iRepercodi)) entity.Repercodi = Convert.ToInt32(dr.GetValue(iRepercodi));

            int iRercpadre = dr.GetOrdinal(this.Rercpadre);
            if (!dr.IsDBNull(iRercpadre)) entity.Rercpadre = Convert.ToInt32(dr.GetValue(iRercpadre));

            int iRercfinal = dr.GetOrdinal(this.Rercfinal);
            if (!dr.IsDBNull(iRercfinal)) entity.Rercfinal = dr.GetString(iRercfinal);

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iRercestado = dr.GetOrdinal(this.Rercestado);
            if (!dr.IsDBNull(iRercestado)) entity.Rercestado = dr.GetString(iRercestado);

            int iRercmotivoanulacion = dr.GetOrdinal(this.Rercmotivoanulacion);
            if (!dr.IsDBNull(iRercmotivoanulacion)) entity.Rercmotivoanulacion = dr.GetString(iRercmotivoanulacion);

            int iRercusueliminacion = dr.GetOrdinal(this.Rercusueliminacion);
            if (!dr.IsDBNull(iRercusueliminacion)) entity.Rercusueliminacion = dr.GetString(iRercusueliminacion);

            int iRercfecanulacion = dr.GetOrdinal(this.Rercfecanulacion);
            if (!dr.IsDBNull(iRercfecanulacion)) entity.Rercfecanulacion = dr.GetDateTime(iRercfecanulacion);

            int iRerccorrelativo = dr.GetOrdinal(this.Rerccorrelativo);
            if (!dr.IsDBNull(iRerccorrelativo)) entity.Rerccorrelativo = Convert.ToInt32(dr.GetValue(iRerccorrelativo));

            int iRerctipcliente = dr.GetOrdinal(this.Rerctipcliente);
            if (!dr.IsDBNull(iRerctipcliente)) entity.Rerctipcliente = dr.GetString(iRerctipcliente);

            int iRerccliente = dr.GetOrdinal(this.Rerccliente);
            if (!dr.IsDBNull(iRerccliente)) entity.Rerccliente = Convert.ToInt32(dr.GetValue(iRerccliente));

            int iRepentcodi = dr.GetOrdinal(this.Repentcodi);
            if (!dr.IsDBNull(iRepentcodi)) entity.Repentcodi = Convert.ToInt32(dr.GetValue(iRepentcodi));

            int iRercptoentrega = dr.GetOrdinal(this.Rercptoentrega);
            if (!dr.IsDBNull(iRercptoentrega)) entity.Rercptoentrega = dr.GetString(iRercptoentrega);

            int iRercalimentadorsed = dr.GetOrdinal(this.Rercalimentadorsed);
            if (!dr.IsDBNull(iRercalimentadorsed)) entity.Rercalimentadorsed = dr.GetString(iRercalimentadorsed);

            int iRercenst = dr.GetOrdinal(this.Rercenst);
            if (!dr.IsDBNull(iRercenst)) entity.Rercenst = dr.GetDecimal(iRercenst);

            int iReevecodi = dr.GetOrdinal(this.Reevecodi);
            if (!dr.IsDBNull(iReevecodi)) entity.Reevecodi = Convert.ToInt32(dr.GetValue(iReevecodi));

            int iRerccomentario = dr.GetOrdinal(this.Rerccomentario);
            if (!dr.IsDBNull(iRerccomentario)) entity.Rerccomentario = dr.GetString(iRerccomentario);

            int iRerctejecinicio = dr.GetOrdinal(this.Rerctejecinicio);
            if (!dr.IsDBNull(iRerctejecinicio)) entity.Rerctejecinicio = dr.GetDateTime(iRerctejecinicio);

            int iRerctejecfin = dr.GetOrdinal(this.Rerctejecfin);
            if (!dr.IsDBNull(iRerctejecfin)) entity.Rerctejecfin = dr.GetDateTime(iRerctejecfin);

            int iRercpk = dr.GetOrdinal(this.Rercpk);
            if (!dr.IsDBNull(iRercpk)) entity.Rercpk = dr.GetDecimal(iRercpk);

            int iRerccompensable = dr.GetOrdinal(this.Rerccompensable);
            if (!dr.IsDBNull(iRerccompensable)) entity.Rerccompensable = dr.GetString(iRerccompensable);

            int iRercens = dr.GetOrdinal(this.Rercens);
            if (!dr.IsDBNull(iRercens)) entity.Rercens = dr.GetDecimal(iRercens);

            int iRercresarcimiento = dr.GetOrdinal(this.Rercresarcimiento);
            if (!dr.IsDBNull(iRercresarcimiento)) entity.Rercresarcimiento = dr.GetDecimal(iRercresarcimiento);

            int iRercusucreacion = dr.GetOrdinal(this.Rercusucreacion);
            if (!dr.IsDBNull(iRercusucreacion)) entity.Rercusucreacion = dr.GetString(iRercusucreacion);

            int iRercfeccreacion = dr.GetOrdinal(this.Rercfeccreacion);
            if (!dr.IsDBNull(iRercfeccreacion)) entity.Rercfeccreacion = dr.GetDateTime(iRercfeccreacion);

            int iRercdisposicion1 = dr.GetOrdinal(this.Rercdisposicion1);
            if (!dr.IsDBNull(iRercdisposicion1)) entity.Rercdisposicion1 = dr.GetString(iRercdisposicion1);

            int iRercdisposicion2 = dr.GetOrdinal(this.Rercdisposicion2);
            if (!dr.IsDBNull(iRercdisposicion2)) entity.Rercdisposicion2 = dr.GetString(iRercdisposicion2);

            int iRercdisposicion3 = dr.GetOrdinal(this.Rercdisposicion3);
            if (!dr.IsDBNull(iRercdisposicion3)) entity.Rercdisposicion3 = dr.GetString(iRercdisposicion3);

            int iRercdisposicion4 = dr.GetOrdinal(this.Rercdisposicion4);
            if (!dr.IsDBNull(iRercdisposicion4)) entity.Rercdisposicion4 = dr.GetString(iRercdisposicion4);

            int iRercdisposicion5 = dr.GetOrdinal(this.Rercdisposicion5);
            if (!dr.IsDBNull(iRercdisposicion5)) entity.Rercdisposicion5 = dr.GetString(iRercdisposicion5);

            int iRercporcentaje1 = dr.GetOrdinal(this.Rercporcentaje1);
            if (!dr.IsDBNull(iRercporcentaje1)) entity.Rercporcentaje1 = dr.GetDecimal(iRercporcentaje1);

            int iRercporcentaje2 = dr.GetOrdinal(this.Rercporcentaje2);
            if (!dr.IsDBNull(iRercporcentaje2)) entity.Rercporcentaje2 = dr.GetDecimal(iRercporcentaje2);

            int iRercporcentaje3 = dr.GetOrdinal(this.Rercporcentaje3);
            if (!dr.IsDBNull(iRercporcentaje3)) entity.Rercporcentaje3 = dr.GetDecimal(iRercporcentaje3);

            int iRercporcentaje4 = dr.GetOrdinal(this.Rercporcentaje4);
            if (!dr.IsDBNull(iRercporcentaje4)) entity.Rercporcentaje4 = dr.GetDecimal(iRercporcentaje4);

            int iRercporcentaje5 = dr.GetOrdinal(this.Rercporcentaje5);
            if (!dr.IsDBNull(iRercporcentaje5)) entity.Rercporcentaje5 = dr.GetDecimal(iRercporcentaje5);

            return entity;
        }


        #region Mapeo de Campos

        public string Rerccodi = "RERCCODI";
        public string Repercodi = "REPERCODI";
        public string Rercpadre = "RERCPADRE";
        public string Rercfinal = "RERCFINAL";
        public string Emprcodi = "EMPRCODI";
        public string Rercestado = "RERCESTADO";
        public string Rercmotivoanulacion = "RERCMOTIVOANULACION";
        public string Rercusueliminacion = "RERCUSUELIMINACION";
        public string Rercfecanulacion = "RERCFECANULACION";
        public string Rerccorrelativo = "RERCCORRELATIVO";
        public string Rerctipcliente = "RERCTIPCLIENTE";
        public string Rerccliente = "RERCCLIENTE";
        public string Repentcodi = "REPENTCODI";
        public string Rercptoentrega = "RERCPTOENTREGA";
        public string Rercalimentadorsed = "RERCALIMENTADORSED";
        public string Rercenst = "RERCENST";
        public string Reevecodi = "REEVECODI";
        public string Rerccomentario = "RERCCOMENTARIO";
        public string Rerctejecinicio = "RERCTEJECINICIO";
        public string Rerctejecfin = "RERCTEJECFIN";
        public string Rercpk = "RERCPK";
        public string Rerccompensable = "RERCCOMPENSABLE";
        public string Rercens = "RERCENS";
        public string Rercresarcimiento = "RERCRESARCIMIENTO";
        public string Rercusucreacion = "RERCUSUCREACION";
        public string Rercfeccreacion = "RERCFECCREACION";

        public string Suministrador = "SUMINISTRADOR";
        public string Cliente = "CLIENTE";
        public string Evento = "EVENTO";

        public string Emprnomb = "EMPRNOMB";
        public string Ptoentrega = "PTOENTREGA";

        public string Rercdisposicion1 = "RERCDISPOSICION1";
        public string Rercdisposicion2 = "RERCDISPOSICION2";
        public string Rercdisposicion3 = "RERCDISPOSICION3";
        public string Rercdisposicion4 = "RERCDISPOSICION4";
        public string Rercdisposicion5 = "RERCDISPOSICION5";
        public string Rercporcentaje1 = "RERCPORCENTAJE1";
        public string Rercporcentaje2 = "RERCPORCENTAJE2";
        public string Rercporcentaje3 = "RERCPORCENTAJE3";
        public string Rercporcentaje4 = "RERCPORCENTAJE4";
        public string Rercporcentaje5 = "RERCPORCENTAJE5";
        public string Rercresponsable1 = "RERCRESPONSABLE1";
        public string Rercresponsable2 = "RERCRESPONSABLE2";
        public string Rercresponsable3 = "RERCRESPONSABLE3";
        public string Rercresponsable4 = "RERCRESPONSABLE4";
        public string Rercresponsable5 = "RERCRESPONSABLE5";

        #endregion

        public string SqlObtenerPorEmpresaPeriodo
        {
            get { return base.GetSqlXml("ObtenerPorEmpresaPeriodo"); }
        }

        public string SqlAnularRechazoCarga
        {
            get { return base.GetSqlXml("AnularRechazoCarga"); }
        }

        public string SqlActualizarPorcentajes
        {
            get { return base.GetSqlXml("ActualizarPorcentajes"); }
        }

        public string SqlObtenerConsolidado
        {
            get { return base.GetSqlXml("ObtenerConsolidado"); }
        }

        public string SqlObtenerTrazabilidad
        {
            get { return base.GetSqlXml("ObtenerTrazabilidad"); }
        }

        public string SqlObtenerNotificacionInterrupcion
        {
            get { return base.GetSqlXml("ObtenerNotificacionInterrupcion"); }
        }

        public string SqlActualizarResarcimiento
        {
            get { return base.GetSqlXml("ActualizarResarcimiento"); }
        }
    }
}
