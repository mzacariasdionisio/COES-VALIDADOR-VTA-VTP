using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla RNT_REG_PUNTO_ENTREGA
    /// </summary>
    public class RntRegPuntoEntregaHelper : HelperBase
    {
        public RntRegPuntoEntregaHelper()
            : base(Consultas.RntRegPuntoEntregaSql)
        {
        }

        public RntRegPuntoEntregaDTO Create(IDataReader dr)
        {
            RntRegPuntoEntregaDTO entity = new RntRegPuntoEntregaDTO();

            int iRpeniveltension = dr.GetOrdinal(this.Rpeniveltension);
            if (!dr.IsDBNull(iRpeniveltension)) entity.RpeNivelTension = Convert.ToInt32(dr.GetValue(iRpeniveltension));

            int iRpefechainicio = dr.GetOrdinal(this.Rpefechainicio);
            if (!dr.IsDBNull(iRpefechainicio)) entity.RpeFechaInicio = Convert.ToDateTime(dr.GetValue(iRpefechainicio));

            int iRpefechafin = dr.GetOrdinal(this.Rpefechafin);
            if (!dr.IsDBNull(iRpefechafin)) entity.RpeFechaFin = Convert.ToDateTime(dr.GetValue(iRpefechafin));

            int iRpecompensacion = dr.GetOrdinal(this.Rpecompensacion);
            if (!dr.IsDBNull(iRpecompensacion)) entity.RpeCompensacion = Convert.ToDouble(dr.GetValue(iRpecompensacion));

            int iRpecausainterrupcion = dr.GetOrdinal(this.Rpecausainterrupcion);
            if (!dr.IsDBNull(iRpecausainterrupcion)) entity.RpeCausaInterrupcion = dr.GetString(iRpecausainterrupcion);

            int iRpetramfuermayor = dr.GetOrdinal(this.Rpetramfuermayor);
            if (!dr.IsDBNull(iRpetramfuermayor)) entity.RpeTramFuerMayor = dr.GetString(iRpetramfuermayor);

            int iRpeestado = dr.GetOrdinal(this.Rpeestado);
            if (!dr.IsDBNull(iRpeestado)) entity.RpeEstado = Convert.ToInt32(dr.GetValue(iRpeestado));

            int iRpeusuariocreacion = dr.GetOrdinal(this.Rpeusuariocreacion);
            if (!dr.IsDBNull(iRpeusuariocreacion)) entity.RpeUsuarioCreacion = dr.GetString(iRpeusuariocreacion);

            int iRpefechacreacion = dr.GetOrdinal(this.Rpefechacreacion);
            if (!dr.IsDBNull(iRpefechacreacion)) entity.RpeFechaCreacion = dr.GetDateTime(iRpefechacreacion);

            int iRpeusuarioupdate = dr.GetOrdinal(this.Rpeusuarioupdate);
            if (!dr.IsDBNull(iRpeusuarioupdate)) entity.RpeUsuarioUpdate = dr.GetString(iRpeusuarioupdate);

            int iRpefechaupdate = dr.GetOrdinal(this.Rpefechaupdate);
            if (!dr.IsDBNull(iRpefechaupdate)) entity.RpeFechaUpdate = dr.GetDateTime(iRpefechaupdate);

            int iRegpuntoentcodi = dr.GetOrdinal(this.RpeCodi);
            if (!dr.IsDBNull(iRegpuntoentcodi)) entity.RegPuntoEntCodi = Convert.ToInt32(dr.GetValue(iRegpuntoentcodi));

            int iAreacodi = dr.GetOrdinal(this.Areacodi);
            if (!dr.IsDBNull(iAreacodi)) entity.AreaCodi = Convert.ToInt32(dr.GetValue(iAreacodi));

            int iPeriodocodi = dr.GetOrdinal(this.Periodocodi);
            if (!dr.IsDBNull(iPeriodocodi)) entity.PeriodoCodi = Convert.ToInt32(dr.GetValue(iPeriodocodi));

            int iTipointcodi = dr.GetOrdinal(this.Tipointcodi);
            if (!dr.IsDBNull(iTipointcodi)) entity.TipoIntCodi = Convert.ToInt32(dr.GetValue(iTipointcodi));

            int iRpeempresageneradora = dr.GetOrdinal(this.Rpeempresageneradora);
            if (!dr.IsDBNull(iRpeempresageneradora)) entity.RpeEmpresaGeneradora = Convert.ToInt32(dr.GetValue(iRpeempresageneradora));

            int iRpecliente = dr.GetOrdinal(this.Rpecliente);
            if (!dr.IsDBNull(iRpecliente)) entity.RpeCliente = Convert.ToInt32(dr.GetValue(iRpecliente));

            //referencias inner join
            int iTipintnombre = dr.GetOrdinal(this.Tipintnombre);
            if (!dr.IsDBNull(iTipintnombre)) entity.TipIntNombre = dr.GetString(iTipintnombre);

            int iAreaNomb = dr.GetOrdinal(this.AreaNomb);
            if (!dr.IsDBNull(iAreaNomb)) entity.AreaCodiNombre = dr.GetString(iAreaNomb);

            int iRpeemprgennombre = dr.GetOrdinal(this.Rpeemprgennombre);
            if (!dr.IsDBNull(iRpeemprgennombre)) entity.RpeEmpresaGeneradoraNombre = dr.GetString(iRpeemprgennombre);

            int iRpeclientenombre = dr.GetOrdinal(this.Rpeclientenombre);
            if (!dr.IsDBNull(iRpeclientenombre)) entity.RpeClienteNombre = dr.GetString(iRpeclientenombre);

            int iRpeEnergSem = dr.GetOrdinal(this.RpeEnergSem);
            if (!dr.IsDBNull(iRpeEnergSem)) entity.RpeEnergSem = Convert.ToDouble(dr.GetValue(iRpeEnergSem));

            int iRpeNi = dr.GetOrdinal(this.RpeNi);
            if (!dr.IsDBNull(iRpeNi)) entity.RpeNi = Convert.ToDouble(dr.GetValue(iRpeNi));

            int iRpeKi = dr.GetOrdinal(this.RpeKi);
            if (!dr.IsDBNull(iRpeKi)) entity.RpeKi = Convert.ToDouble(dr.GetValue(iRpeKi));

            int iRpePrgFechaInicio = dr.GetOrdinal(this.RpePrgFechaInicio);
            if (!dr.IsDBNull(iRpePrgFechaInicio)) entity.RpePrgFechaInicio = Convert.ToDateTime(dr.GetValue(iRpePrgFechaInicio));

            int iRpePrgFechaFin = dr.GetOrdinal(this.RpePrgFechaFin);
            if (!dr.IsDBNull(iRpePrgFechaFin)) entity.RpePrgFechaFin = Convert.ToDateTime(dr.GetValue(iRpePrgFechaFin));

            int iRpeEiE = dr.GetOrdinal(this.RpeEiE);
            if (!dr.IsDBNull(iRpeEiE)) entity.RpeEiE = Convert.ToDouble(dr.GetValue(iRpeEiE));

            int iBarrcodi = dr.GetOrdinal(this.Barrcodi);
            if (!dr.IsDBNull(iBarrcodi)) entity.Barrcodi = Convert.ToInt32(dr.GetValue(iBarrcodi));

            int iEnvioCodi = dr.GetOrdinal(this.EnvioCodi);
            if (!dr.IsDBNull(iEnvioCodi)) entity.EnvioCodi = Convert.ToInt32(dr.GetValue(iEnvioCodi));

            int iRpeGrupoEnvio = dr.GetOrdinal(this.Rpegrupoenvio);
            if (!dr.IsDBNull(iRpeGrupoEnvio)) entity.RpeGrupoEnvio = Convert.ToInt32(dr.GetValue(iRpeGrupoEnvio));

            int iBarrnombre = dr.GetOrdinal(this.BarrNombre);
            if (!dr.IsDBNull(iBarrnombre)) entity.BarrNombre = dr.GetString(iBarrnombre);

            int iRpeNiveltensionDesc = dr.GetOrdinal(this.RpeNivelTensionDesc);
            if (!dr.IsDBNull(iRpeNiveltensionDesc)) entity.RpeNivelTensionDesc = dr.GetString(iRpeNiveltensionDesc);

            int iRpeIncremento = dr.GetOrdinal(this.RpeIncremento);
            if (!dr.IsDBNull(iRpeIncremento)) entity.RpeIncremento = dr.GetString(iRpeIncremento);

            return entity;
        }

        public RntRegPuntoEntregaDTO CreateCarga(IDataReader dr)
        {
            RntRegPuntoEntregaDTO entity = new RntRegPuntoEntregaDTO();

            int iRpeusuariocreacion = dr.GetOrdinal(this.Rpeusuariocreacion);
            if (!dr.IsDBNull(iRpeusuariocreacion)) entity.RpeUsuarioCreacion = dr.GetString(iRpeusuariocreacion);

            int iRpefechacreacion = dr.GetOrdinal(this.Rpefechacreacion);
            if (!dr.IsDBNull(iRpefechacreacion)) entity.RpeFechaCreacion = dr.GetDateTime(iRpefechacreacion);

            int iAreacodi = dr.GetOrdinal(this.Areacodi);
            if (!dr.IsDBNull(iAreacodi)) entity.AreaCodi = Convert.ToInt32(dr.GetValue(iAreacodi));

            int iPeriodocodi = dr.GetOrdinal(this.Periodocodi);
            if (!dr.IsDBNull(iPeriodocodi)) entity.PeriodoCodi = Convert.ToInt32(dr.GetValue(iPeriodocodi));

            int iRpeempresageneradora = dr.GetOrdinal(this.Rpeempresageneradora);
            if (!dr.IsDBNull(iRpeempresageneradora)) entity.RpeEmpresaGeneradora = Convert.ToInt32(dr.GetValue(iRpeempresageneradora));

            int iRpeemprgennombre = dr.GetOrdinal(this.Rpeemprgennombre);
            if (!dr.IsDBNull(iRpeemprgennombre)) entity.RpeEmpresaGeneradoraNombre = dr.GetString(iRpeemprgennombre);

            int iEnvioCodi = dr.GetOrdinal(this.EnvioCodi);
            if (!dr.IsDBNull(iEnvioCodi)) entity.EnvioCodi = Convert.ToInt32(dr.GetValue(iEnvioCodi));

            return entity;
        }

        public RntRegPuntoEntregaDTO CreateBarr(IDataReader dr)
        {
            RntRegPuntoEntregaDTO entity = new RntRegPuntoEntregaDTO();


            int iBarrNombre = dr.GetOrdinal(this.BarrNombre);
            if (!dr.IsDBNull(iBarrNombre)) entity.BarrNombre = dr.GetString(iBarrNombre);

            int iBarrcodi = dr.GetOrdinal(this.Barrcodi);
            if (!dr.IsDBNull(iBarrcodi)) entity.Barrcodi = Convert.ToInt32(dr.GetValue(iBarrcodi));


            return entity;
        }

        public RntRegPuntoEntregaDTO CreateClientPE(IDataReader dr)
        {

            RntRegPuntoEntregaDTO entity = new RntRegPuntoEntregaDTO();

            int iRpeCliente = dr.GetOrdinal(this.Rpecliente);
            if (!dr.IsDBNull(iRpeCliente)) entity.RpeCliente = Convert.ToInt32(dr.GetValue(iRpeCliente));

            int iRpeClienteNombre = dr.GetOrdinal(this.Rpeclientenombre);
            if (!dr.IsDBNull(iRpeClienteNombre)) entity.RpeClienteNombre = dr.GetString(iRpeClienteNombre);

            return entity;
        }

        #region Mapeo de Campos


        public string Rpeniveltension = "RPENIVELTENSION";
        public string Rpefechainicio = "RPEFECHAINICIO";
        public string Rpefechafin = "RPEFECHAFIN";
        public string Rpecompensacion = "RPECOMPENSACION";
        public string Rpecausainterrupcion = "RPECAUSAINTERRUPCION";
        public string Rpetramfuermayor = "RPETRAMFUERMAYOR";
        public string Rpeestado = "RPEESTADO";
        public string Rpeusuariocreacion = "RPEUSUARIOCREACION";
        public string Rpefechacreacion = "RPEFECHACREACION";
        public string Rpeusuarioupdate = "RPEUSUARIOUPDATE";
        public string Rpefechaupdate = "RPEFECHAUPDATE";
        public string RpeCodi = "RPECODI";
        public string Areacodi = "AREACODI";
        public string Periodocodi = "PERDCODI";
        public string Tipointcodi = "TIPINTCODI";
        public string Rpeempresageneradora = "RPEEMPGEN";
        public string Rpecliente = "RPECLIENTE";

        public string Rpegrupoenvio = "RPEGRUPOENVIO";
        public string RpeNivelTensionDesc = "RPENIVELTENSIONDESC";
        public string RpeTipoIntDesc = "RPETIPOINTDESC";
        public string RpeEnergSem = "RPEENERGSEM";
        public string RpeNi = "RPENI";
        public string RpeKi = "RPEKI";
        public string RpePrgFechaInicio = "RPEPRGFECHAINICIO";
        public string RpePrgFechaFin = "RPEPRGFECHAFIN";
        public string RpeEiE = "RPEEIE";
        public string EnvioCodi = "ENVIOCODI";
        public string Barrcodi = "BARRCODI";
        public string BarrNombre = "BARRNOMBRE";

        public string RpeIncremento = "RPEINCREMENTO";

        //campos de referencia iner join
        public string Tipintnombre = "TIPOINTNOMBRE";
        public string AreaNomb = "AREANOMB";
        public string Rpeemprgennombre = "RPEEMPRGENNOMBRE";
        public string Rpeclientenombre = "RPECLIENTENOMBRE";


        #endregion
        public string SqlGetMaxId
        {
            get { return base.GetSqlXml("GetMaxId"); }
        }

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
        public string SqlListReporteCarga
        {
            get { return base.GetSqlXml("ListReporteCarga"); }
        }
        public string SqlListReporteGrilla
        {
            get { return base.GetSqlXml("ListReporteGrilla"); }
        }
        public string SqlGetByCriteriaPaginado
        {
            get { return base.GetSqlXml("GetByCriteriaPaginado"); }
        }
        public string SqlListAuditoriaPuntoEntrega
        {
            get { return base.GetSqlXml("ListAuditoriaPuntoEntrega"); }
        }
        public string SqlListBarras
        {
            get { return base.GetSqlXml("ListBarras"); }
        }
        public string SqlListAllBarras
        {
            get { return base.GetSqlXml("ListAllBarras"); }
        }
        public string SqlListAllPuntoEntrega
        {
            get { return base.GetSqlXml("ListAllPuntoEntrega"); }
        }
        public string SqlListAllClientePE
        {
            get { return base.GetSqlXml("ListAllClientePE"); }
        }
        public string SqlListChangeClientePE
        {
            get { return base.GetSqlXml("ListChangeClientePE"); }
        }

    }
}
