using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla RNT_REG_RECHAZO_CARGA
    /// </summary>
    public class RntRegRechazoCargaHelper : HelperBase
    {
        public RntRegRechazoCargaHelper()
            : base(Consultas.RntRegRechazoCargaSql)
        {
        }

        public RntRegRechazoCargaDTO Create(IDataReader dr)
        {
            RntRegRechazoCargaDTO entity = new RntRegRechazoCargaDTO();

            int iRrcgrupoenvio = dr.GetOrdinal(this.RrcGrupoEnvio);
            if (!dr.IsDBNull(iRrcgrupoenvio)) entity.RrcGrupoEnvio = Convert.ToInt32(dr.GetValue(iRrcgrupoenvio));

            int iEnviocodi = dr.GetOrdinal(this.EnvioCodi);
            if (!dr.IsDBNull(iEnviocodi)) entity.Enviocodi = Convert.ToInt32(dr.GetValue(iEnviocodi));

            int iRrcniveltension = dr.GetOrdinal(this.Rrcniveltension);
            if (!dr.IsDBNull(iRrcniveltension)) entity.RrcNivelTension = Convert.ToInt32(dr.GetValue(iRrcniveltension));

            int iRrcfechainicio = dr.GetOrdinal(this.Rrcfechainicio);
            if (!dr.IsDBNull(iRrcfechainicio)) entity.RrcFechaInicio = Convert.ToDateTime(dr.GetValue(iRrcfechainicio));

            int iRrcfechafin = dr.GetOrdinal(this.Rrcfechafin);
            if (!dr.IsDBNull(iRrcfechafin)) entity.RrcFechaFin = Convert.ToDateTime(dr.GetValue(iRrcfechafin));

            int iRrcsubestaciondstrb = dr.GetOrdinal(this.Rrcsubestaciondstrb);
            if (!dr.IsDBNull(iRrcsubestaciondstrb)) entity.RrcSubestacionDstrb = dr.GetString(iRrcsubestaciondstrb);

            int iRrcniveltensionsed = dr.GetOrdinal(this.Rrcniveltensionsed);
            if (!dr.IsDBNull(iRrcniveltensionsed)) entity.RrcNivelTensionSed = Convert.ToDecimal(dr.GetValue(iRrcniveltensionsed));

            int iRrccodialimentador = dr.GetOrdinal(this.Rrccodialimentador);
            if (!dr.IsDBNull(iRrccodialimentador)) entity.RrcCodiAlimentador = dr.GetString(iRrccodialimentador);

            int iRrcenergiaens = dr.GetOrdinal(this.Rrcenergiaens);
            if (!dr.IsDBNull(iRrcenergiaens)) entity.RrcEnergiaEns = Convert.ToDecimal(dr.GetValue(iRrcenergiaens));

            int iRrcnrcf = dr.GetOrdinal(this.Rrcnrcf);
            if (!dr.IsDBNull(iRrcnrcf)) entity.RrcNrcf = Convert.ToInt32(dr.GetValue(iRrcnrcf));

            int iRrcef = dr.GetOrdinal(this.Rrcef);
            if (!dr.IsDBNull(iRrcef)) entity.RrcEf = Convert.ToDecimal(dr.GetValue(iRrcef));

            int iRrccompensacion = dr.GetOrdinal(this.Rrccompensacion);
            if (!dr.IsDBNull(iRrccompensacion)) entity.RrcCompensacion = Convert.ToDecimal(dr.GetValue(iRrccompensacion));

            int iRrcestado = dr.GetOrdinal(this.Rrcestado);
            if (!dr.IsDBNull(iRrcestado)) entity.RrcEstado = Convert.ToInt32(dr.GetValue(iRrcestado));

            int iRrcusuariocreacion = dr.GetOrdinal(this.Rrcusuariocreacion);
            if (!dr.IsDBNull(iRrcusuariocreacion)) entity.RrcUsuarioCreacion = dr.GetString(iRrcusuariocreacion);

            int iRrcfechacreacion = dr.GetOrdinal(this.Rrcfechacreacion);
            if (!dr.IsDBNull(iRrcfechacreacion)) entity.RrcFechaCreacion = dr.GetDateTime(iRrcfechacreacion);

            int iRrcusuarioupdate = dr.GetOrdinal(this.Rrcusuarioupdate);
            if (!dr.IsDBNull(iRrcusuarioupdate)) entity.RrcUsuarioUpdate = dr.GetString(iRrcusuarioupdate);

            int iRrcfechaupdate = dr.GetOrdinal(this.Rrcfechaupdate);
            if (!dr.IsDBNull(iRrcfechaupdate)) entity.RrcFechaUpdate = dr.GetDateTime(iRrcfechaupdate);

            int iRegrechazocargacodi = dr.GetOrdinal(this.Regrechazocargacodi);
            if (!dr.IsDBNull(iRegrechazocargacodi)) entity.RegRechazoCargaCodi = Convert.ToInt32(dr.GetValue(iRegrechazocargacodi));// Convert.ToInt32(dr.GetInt32(iRegrechazocargacodi));

            int iAreacodi = dr.GetOrdinal(this.Areacodi);
            if (!dr.IsDBNull(iAreacodi)) entity.AreaCodi = Convert.ToInt32(dr.GetValue(iAreacodi));

            int iPeriodocodi = dr.GetOrdinal(this.Periodocodi);
            if (!dr.IsDBNull(iPeriodocodi)) entity.PeriodoCodi = Convert.ToInt32(dr.GetValue(iPeriodocodi));

            int iEvencodi = dr.GetOrdinal(this.Evencodi);
            if (!dr.IsDBNull(iEvencodi)) entity.EvenCodi = Convert.ToInt32(dr.GetValue(iEvencodi));

            int iRrcempresageneradora = dr.GetOrdinal(this.Rrcempresageneradora);
            if (!dr.IsDBNull(iRrcempresageneradora)) entity.RrcEmpresaGeneradora = Convert.ToInt32(dr.GetValue(iRrcempresageneradora));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.EmprCodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            //referencias inner join
            int iAreaNomb = dr.GetOrdinal(this.AreaNomb);
            if (!dr.IsDBNull(iAreaNomb)) entity.AreaCodiNombre = dr.GetString(iAreaNomb);

            int iRrcemprgennombre = dr.GetOrdinal(this.Rrcemprgennombre);
            if (!dr.IsDBNull(iRrcemprgennombre)) entity.RrcEmpresaGeneradoraNombre = dr.GetString(iRrcemprgennombre);

            int iRrcclientenombre = dr.GetOrdinal(this.Rrcclientenombre);
            if (!dr.IsDBNull(iRrcclientenombre)) entity.RrcClienteNombre = dr.GetString(iRrcclientenombre);

            int iRrcpk = dr.GetOrdinal(this.Rrcpk);
            if (!dr.IsDBNull(iRrcpk)) entity.RrcPk = Convert.ToDecimal(dr.GetValue(iRrcpk));

            int iRrccompensable = dr.GetOrdinal(this.Rrccompensable);
            if (!dr.IsDBNull(iRrccompensable)) entity.RrcCompensable = dr.GetString(iRrccompensable);

            int iRrcensfk = dr.GetOrdinal(this.Rrcensfk);
            if (!dr.IsDBNull(iRrcensfk)) entity.RrcEnsFk = Convert.ToDecimal(dr.GetValue(iRrcensfk));

            int iRrcevencodidesc = dr.GetOrdinal(this.Rrcevencodidesc);
            if (!dr.IsDBNull(iRrcevencodidesc)) entity.RrcEvenCodiDesc = dr.GetString(iRrcevencodidesc);

            int iBarrnombre = dr.GetOrdinal(this.Barrnombre);
            if (!dr.IsDBNull(iBarrnombre)) entity.BarrNombre = dr.GetString(iBarrnombre);

            int iBarrcodi = dr.GetOrdinal(this.BarrCodi);
            if (!dr.IsDBNull(iBarrcodi)) entity.Barrcodi = Convert.ToInt32(dr.GetValue(iBarrcodi));

            return entity;
        }

        public RntRegRechazoCargaDTO CreateList(IDataReader dr)
        {
            RntRegRechazoCargaDTO entity = new RntRegRechazoCargaDTO();

            int iBarrCodi = dr.GetOrdinal(this.BarrCodi);
            if (!dr.IsDBNull(iBarrCodi)) entity.Barrcodi = Convert.ToInt32(dr.GetValue(iBarrCodi));

            int iBarrnombre = dr.GetOrdinal(this.Barrnombre);
            if (!dr.IsDBNull(iBarrnombre)) entity.BarrNombre = dr.GetString(iBarrnombre);

            return entity;
        }

        public RntRegRechazoCargaDTO CreateClientRC(IDataReader dr)
        {

            RntRegRechazoCargaDTO entity = new RntRegRechazoCargaDTO();

            int iRrcCliente = dr.GetOrdinal(this.RrcCliente);
            if (!dr.IsDBNull(iRrcCliente)) entity.RrcCliente = Convert.ToInt32(dr.GetValue(iRrcCliente));

            int iRrcClienteNombre = dr.GetOrdinal(this.Rrcclientenombre);
            if (!dr.IsDBNull(iRrcClienteNombre)) entity.RrcClienteNombre = dr.GetString(iRrcClienteNombre);

            return entity;
        }


        #region Mapeo de Campos


        public string RrcGrupoEnvio = "RRCGRUPOENVIO";
        public string RrcCliente = "RRCCLIENTE";
        public string EnvioCodi = "ENVIOCODI";
        public string Rrcniveltension = "RRCNIVELTENSION";
        public string Rrcfechainicio = "RRCFECHAINICIO";
        public string Rrcfechafin = "RRCFECHAFIN";
        public string Rrcsubestaciondstrb = "RRCSUBESTACIONDSTRB";
        public string Rrcniveltensionsed = "RRCNIVELTENSIONSED";
        public string Rrccodialimentador = "RRCCODIALIMENTADOR";
        public string Rrcenergiaens = "RRCENERGIAENS";
        public string Rrcnrcf = "RRCNRCF";
        public string Rrcef = "RRCEF";
        public string Rrccompensacion = "RRCCOMPENSACION";
        public string Rrcestado = "RRCESTADO";
        public string Rrcusuariocreacion = "RRCUSUARIOCREACION";
        public string Rrcfechacreacion = "RRCFECHACREACION";
        public string Rrcusuarioupdate = "RRCUSUARIOUPDATE";
        public string Rrcfechaupdate = "RRCFECHAUPDATE";
        public string Regrechazocargacodi = "RRCCODI";
        public string Areacodi = "AREACODI";
        public string Periodocodi = "PERDCODI";
        public string Evencodi = "EVENCODI";
        public string Rrcempresageneradora = "RRCEMPGEN";
        public string Emprcodi = "RRCCLIENTE";

        public string Rrcpk = "RRCPK";
        public string Rrccompensable = "RRCCOMPENSABLE";
        public string Rrcensfk = "RRCENSFK";
        public string Rrcevencodidesc = "RRCEVENCODIDESC";
        //campos de referencia iner join
        public string AreaNomb = "AREANOMB";
        public string Rrcemprgennombre = "RRCEMPRGENNOMBRE";
        public string Rrcclientenombre = "RRCCLIENTENOMBRE";
        public string BarrCodi = "BARRCODI";
        public string Barrnombre = "BARRNOMBRE";

        #endregion
        public string SqlListPaginado
        {
            get { return base.GetSqlXml("ListPaginado"); }
        }
        public string SqlListReportePaginado
        {
            get { return base.GetSqlXml("ListReportePaginado"); }
        }
        public string SqlListReporte
        {
            get { return base.GetSqlXml("ListReporte"); }
        }

        public string SqlListReporteGrilla
        {
            get { return base.GetSqlXml("ListReporteGrilla"); }
        }

        public string SqlGetByCriteriaPaginado
        {
            get { return base.GetSqlXml("GetByCriteriaPaginado"); }
        }
        public string SqlListAuditoriaRechazoCarga
        {
            get { return base.GetSqlXml("ListAuditoriaRechazoCarga"); }
        }
        public string SqlUpdateNRCF
        {
            get { return base.GetSqlXml("UpdateNRCF"); }
        }
        public string SqlUpdateEF
        {
            get { return base.GetSqlXml("UpdateEF"); }
        }
        public string SqlListAllRechazoCarga
        {
            get { return base.GetSqlXml("ListAllRechazoCarga"); }
        }

        public string SqlListAllClienteRC
        {
            get { return base.GetSqlXml("ListAllClienteRC"); }
        }

        public string SqlListChangeClienteRC
        {
            get { return base.GetSqlXml("ListChangeClienteRC"); }
        }

    }
}
