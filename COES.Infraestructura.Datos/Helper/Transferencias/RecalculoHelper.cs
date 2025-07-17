using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla trn_recalculo
    /// </summary>
   public class RecalculoHelper: HelperBase
    {
       public RecalculoHelper() : base(Consultas.RecalculoSql)
       {
       }
       
       public RecalculoDTO Create(IDataReader dr)
       {
            RecalculoDTO entity = new RecalculoDTO();

            int iRecacodi = dr.GetOrdinal(this.Recacodi);
            if (!dr.IsDBNull(iRecacodi)) entity.RecaCodi = dr.GetInt32(iRecacodi);

            int iRecapericodi = dr.GetOrdinal(this.Recapericodi);
            if (!dr.IsDBNull(iRecapericodi)) entity.RecaPeriCodi = dr.GetInt32(iRecapericodi);

            int iRecafechavalorizacion = dr.GetOrdinal(this.Recafechavalorizacion);
            if (!dr.IsDBNull(iRecafechavalorizacion)) entity.RecaFechaValorizacion = dr.GetDateTime(iRecafechavalorizacion);

            int iRecafechalimite = dr.GetOrdinal(this.Recafechalimite);
            if (!dr.IsDBNull(iRecafechalimite)) entity.RecaFechaLimite = dr.GetDateTime(iRecafechalimite);

            int iRecahoralimite = dr.GetOrdinal(this.Recahoralimite);
            if (!dr.IsDBNull(iRecahoralimite)) entity.RecaHoraLimite = dr.GetString(iRecahoralimite);

            int iRecafechaobservacion = dr.GetOrdinal(this.Recafechaobservacion);
            if (!dr.IsDBNull(iRecafechaobservacion)) entity.RecaFechaObservacion = dr.GetDateTime(iRecafechaobservacion);

            int iRecaestado = dr.GetOrdinal(this.Recaestado);
            if (!dr.IsDBNull(iRecaestado)) entity.RecaEstado = dr.GetString(iRecaestado);

            int iRecanombre = dr.GetOrdinal(this.Recanombre);
            if (!dr.IsDBNull(iRecanombre)) entity.RecaNombre = dr.GetString(iRecanombre);

            int iRecadescripcion = dr.GetOrdinal(this.Recadescripcion);
            if (!dr.IsDBNull(iRecadescripcion)) entity.RecaDescripcion = dr.GetString(iRecadescripcion);

            int iRecanroinforme = dr.GetOrdinal(this.Recanroinforme);
            if (!dr.IsDBNull(iRecanroinforme)) entity.RecaNroInforme = dr.GetString(iRecanroinforme);

            int iRecamasinfo = dr.GetOrdinal(this.Recamasinfo);
            if (!dr.IsDBNull(iRecamasinfo)) entity.RecaMasInfo = dr.GetString(iRecamasinfo);

            int iRecausername = dr.GetOrdinal(this.Recausername);
            if (!dr.IsDBNull(iRecausername)) entity.RecaUserName = dr.GetString(iRecausername);

            int iRecafecins = dr.GetOrdinal(this.Recafecins);
            if (!dr.IsDBNull(iRecafecins)) entity.RecaFecIns = dr.GetDateTime(iRecafecins);

            int iRecafecact = dr.GetOrdinal(this.Recafecact);
            if (!dr.IsDBNull(iRecafecact)) entity.RecaFecAct = dr.GetDateTime(iRecafecact);

            int iPeriCodiDestino = dr.GetOrdinal(this.PeriCodiDestino);
            if (!dr.IsDBNull(iPeriCodiDestino)) entity.PeriCodiDestino = dr.GetInt32(iPeriCodiDestino);

            int iRecacuadro1 = dr.GetOrdinal(this.Recacuadro1);
            if (!dr.IsDBNull(iRecacuadro1)) entity.RecaCuadro1 = dr.GetString(iRecacuadro1);

            int iRecacuadro2 = dr.GetOrdinal(this.Recacuadro2);
            if (!dr.IsDBNull(iRecacuadro2)) entity.RecaCuadro2 = dr.GetString(iRecacuadro2);

            int iRecanota2 = dr.GetOrdinal(this.Recanota2);
            if (!dr.IsDBNull(iRecanota2)) entity.RecaNota2 = dr.GetString(iRecanota2);

            int iRecacuadro3 = dr.GetOrdinal(this.Recacuadro3);
            if (!dr.IsDBNull(iRecacuadro3)) entity.RecaCuadro3 = dr.GetString(iRecacuadro3);

            int iRecacuadro4 = dr.GetOrdinal(this.Recacuadro4);
            if (!dr.IsDBNull(iRecacuadro4)) entity.RecaCuadro4 = dr.GetString(iRecacuadro4);

            int iRecacuadro5 = dr.GetOrdinal(this.Recacuadro5);
            if (!dr.IsDBNull(iRecacuadro5)) entity.RecaCuadro5 = dr.GetString(iRecacuadro5);

            return entity;
        }

        #region Mapeo de Campos

        public string Recacodi = "RECACODI";
        public string Recapericodi = "PERICODI";
        public string Recafechavalorizacion = "RECAFECHAVALORIZACION";
        public string Recafechalimite = "RECAFECHALIMITE";
        public string Recahoralimite = "RECAHORALIMITE";
        public string Recafechaobservacion = "RECAFECHAOBSERVACION";
        public string Recaestado = "RECAESTADO";
        public string Recanombre = "RECANOMBRE";
        public string Recadescripcion = "RECADESCRIPCION";
        public string Recanroinforme = "RECANROINFORME";
        public string Recamasinfo = "RECAMASINFO";
        public string Recausername = "RECAUSERNAME";
        public string Recafecins = "RECAFECINS";
        public string Recafecact = "RECAFECACT";
        public string PeriCodiDestino = "PERICODIDESTINO";
        public string PeriNombre = "PERINOMBRE";
        public string Recacuadro1 = "RECACUADRO1";
        public string Recacuadro2 = "RECACUADRO2";
        public string Recanota2 = "RECANOTA2";
        public string Recacuadro3 = "RECACUADRO3";
        public string Recacuadro4 = "RECACUADRO4";
        public string Recacuadro5 = "RECACUADRO5";
        public string Pericodi = "PERICODI";

        public string Perianio = "PERIANIO";                        // PrimasRER.2023
        public string Perimes = "PERIMES";                          // PrimasRER.2023
        public string PeriNombreDestino = "PERINOMBREDESTINO";      // PrimasRER.2023
        #endregion

        public string SqlCodigoGenerado
        {
            get { return base.GetSqlXml("GetMaxId"); }
        }

        public string SqlGetUltimaVersion
        {
            get { return base.GetSqlXml("GetUltimaVersion"); }
        }
        #region PrimasRER.2023
        public string SqlListByAnioMes
        {
            get { return base.GetSqlXml("ListByAnioMes"); }
        }
        public string SqlListVTEAByAnioMes
        {
            get { return base.GetSqlXml("ListVTEAByAnioMes"); }
        }
        #endregion
        public string SqlListEstadoPublicarCerrado
        {
            get { return base.GetSqlXml("ListEstadoPublicarCerrado"); }
        }

        public string SqlObtenerVersionDTR
        {
            get { return base.GetSqlXml("ObtenerVersionDTR"); }
        }

        public string SqlListRecalculosTrnCodigoEnviado
        {
            get { return base.GetSqlXml("ListRecalculosTrnCodigoEnviado"); }
        }

        //ASSETEC 202108 - TIEE
        public string SqlListMaxRecalculoByPeriodo
        {
            get { return base.GetSqlXml("ListMaxRecalculoByPeriodo"); }
        }

        public string SqlMigrarSaldosVTEA
        {
            get { return base.GetSqlXml("MigrarSaldosVTEA"); }
        }

        public string SqlMigrarCalculoVTEA
        {
            get { return base.GetSqlXml("MigrarCalculoVTEA"); }
        }
    }
}
