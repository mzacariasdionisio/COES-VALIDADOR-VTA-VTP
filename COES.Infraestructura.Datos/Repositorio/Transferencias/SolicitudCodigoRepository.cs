using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Transferencias;
using COES.Infraestructura.Datos.Helper.Transferencias;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Infraestructura.Datos.Repositorio.Transferencias
{
    public class SolicitudCodigoRepository : RepositoryBase, ISolicitudCodigoRepository
    {
        public SolicitudCodigoRepository(string strConn) : base(strConn)
        {
        }

        SolicitudCodigoHelper helper = new SolicitudCodigoHelper();

        public int Save(SolicitudCodigoDTO entity)
        {
            int id = GetCodigoGenerado();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);
            dbProvider.AddInParameter(command, helper.Solicodireticodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.EmprCodi);
            dbProvider.AddInParameter(command, helper.Barrcodi, DbType.Int32, entity.BarrCodi);
            dbProvider.AddInParameter(command, helper.Usuacodi, DbType.String, entity.UsuaCodi);
            dbProvider.AddInParameter(command, helper.Tipocontcodi, DbType.Int32, entity.TipoContCodi);
            dbProvider.AddInParameter(command, helper.Tipousuacodi, DbType.Int32, entity.TipoUsuaCodi);
            dbProvider.AddInParameter(command, helper.Clicodi, DbType.Int32, entity.CliCodi);
            dbProvider.AddInParameter(command, helper.Solicodireticodigo, DbType.String, entity.SoliCodiRetiCodigo);
            dbProvider.AddInParameter(command, helper.Solicodiretifecharegistro, DbType.DateTime, entity.SoliCodiRetiFechaRegistro);
            dbProvider.AddInParameter(command, helper.Solicodiretidescripcion, DbType.String, entity.SoliCodiRetiDescripcion);
            dbProvider.AddInParameter(command, helper.Solicodiretidetalleamplio, DbType.String, entity.SoliCodiRetiDetalleAmplio);
            dbProvider.AddInParameter(command, helper.Solicodiretifechainicio, DbType.DateTime, entity.SoliCodiRetiFechaInicio);
            dbProvider.AddInParameter(command, helper.Solicodiretifechafin, DbType.DateTime, entity.SoliCodiRetiFechaFin);
            dbProvider.AddInParameter(command, helper.Solicodiretiobservacion, DbType.String, entity.SoliCodiRetiObservacion);
            dbProvider.AddInParameter(command, helper.Solicodiretiestado, DbType.String, entity.SoliCodiRetiEstado);
            dbProvider.AddInParameter(command, helper.Solicodiretifecins, DbType.DateTime, DateTime.Now.Date);

            int res = dbProvider.ExecuteNonQuery(command);

            return (res > 0) ? id : 0;
        }

        public void SaveSolicitudPeriodo(SolicitudCodigoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSaveSolicitudPeriodo);
            dbProvider.AddInParameter(command, helper.PeridcCodi, DbType.Int32, entity.PeridcCodi);
            dbProvider.AddInParameter(command, helper.Solicodireticodi, DbType.Int32, entity.SoliCodiRetiCodi);
            dbProvider.AddInParameter(command, helper.CodcnpeUsuarioRegi, DbType.String, entity.UsuaCodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.EmprCodi);
            dbProvider.AddInParameter(command, helper.TrnpcTipoPotencia, DbType.Int32, entity.TrnpcTipoPotencia);
            dbProvider.ExecuteNonQuery(command);

        }
        public void SaveSolicitudPeriodoVTP(SolicitudCodigoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSaveSolicitudPeriodoVTP);
            dbProvider.AddInParameter(command, helper.PeridcCodi, DbType.Int32, entity.PeridcCodi);
            dbProvider.AddInParameter(command, helper.Emprnomb, DbType.String, entity.EmprNombre);
            dbProvider.AddInParameter(command, helper.Codretgencodi, DbType.Int32, entity.Codretgencodi);
            dbProvider.AddInParameter(command, helper.CodcnpeUsuarioRegi, DbType.String, entity.UsuaCodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.EmprCodi);

            dbProvider.ExecuteNonQuery(command);

        }

        public void Update(SolicitudCodigoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.EmprCodi);
            dbProvider.AddInParameter(command, helper.Barrcodi, DbType.Int32, entity.BarrCodi);
            dbProvider.AddInParameter(command, helper.Coesusername, DbType.String, entity.CoesUserName);
            dbProvider.AddInParameter(command, helper.Tipocontcodi, DbType.Int32, entity.TipoContCodi);
            dbProvider.AddInParameter(command, helper.Tipousuacodi, DbType.Int32, entity.TipoUsuaCodi);
            dbProvider.AddInParameter(command, helper.Clicodi, DbType.Int32, entity.CliCodi);
            dbProvider.AddInParameter(command, helper.Solicodireticodigo, DbType.String, entity.SoliCodiRetiCodigo);
            dbProvider.AddInParameter(command, helper.Solicodiretifecharegistro, DbType.DateTime, entity.SoliCodiRetiFechaRegistro);
            dbProvider.AddInParameter(command, helper.Solicodiretidescripcion, DbType.String, entity.SoliCodiRetiDescripcion);
            dbProvider.AddInParameter(command, helper.Solicodiretidetalleamplio, DbType.String, entity.SoliCodiRetiDetalleAmplio);
            dbProvider.AddInParameter(command, helper.Solicodiretifechainicio, DbType.DateTime, entity.SoliCodiRetiFechaInicio);
            dbProvider.AddInParameter(command, helper.Solicodiretifechafin, DbType.DateTime, entity.SoliCodiRetiFechaFin);
            dbProvider.AddInParameter(command, helper.Solicodiretiobservacion, DbType.String, entity.SoliCodiRetiObservacion);
            dbProvider.AddInParameter(command, helper.Solicodiretiestado, DbType.String, entity.SoliCodiRetiEstado);
            dbProvider.AddInParameter(command, helper.Solicodiretifecact, DbType.DateTime, DateTime.Now.Date);
            dbProvider.AddInParameter(command, helper.Solicodiretifechasolbaja, DbType.DateTime, entity.SoliCodiretiFechaSolBaja);
            dbProvider.AddInParameter(command, helper.Solicodiretifechadebaja, DbType.DateTime, entity.SoliCodiRetiFechaBaja);

            dbProvider.AddInParameter(command, helper.Solicodireticodi, DbType.Int32, entity.SoliCodiRetiCodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void UpdateTipPotCodConsolidadoPeriodo(SolicitudCodigoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdateTipPotCodConsolidadoPeriodo);

            dbProvider.AddInParameter(command, helper.TrnpcTipoPotencia, DbType.Int32, entity.TrnpcTipoPotencia);
            dbProvider.AddInParameter(command, helper.CoresoCodi, DbType.Int32, entity.CoresoCodi);
            dbProvider.AddInParameter(command, helper.PeridcCodi, DbType.Int32, entity.PeridcCodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void UpdateTipPotCodCodigoRetiro(SolicitudCodigoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdateTipPotCodConsolidadoPeriodo);

            dbProvider.AddInParameter(command, helper.TrnpcTipoPotencia, DbType.Int32, entity.TrnpcTipoPotencia);
            dbProvider.AddInParameter(command, helper.CoresoCodi, DbType.Int32, entity.CoresoCodi);
            dbProvider.AddInParameter(command, helper.PeridcCodi, DbType.Int32, entity.PeridcCodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int id)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Solicodiretifecact, DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, helper.Solicodireticodi, DbType.Int32, id);

            dbProvider.ExecuteNonQuery(command);
        }

        public SolicitudCodigoDTO GetById(int id)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Solicodireticodi, DbType.Int32, id);
            SolicitudCodigoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SolicitudCodigoDTO> List(string estado)
        {
            List<SolicitudCodigoDTO> entitys = new List<SolicitudCodigoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);
            dbProvider.AddInParameter(command, helper.Solicodiretiestado, DbType.String, estado);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<SolicitudCodigoDTO> GetByCriteriaExtranet(string nombreEmp, string tipoUsu, string tipoCont, string barrTran, string cliNomb, DateTime? fechaIni, DateTime? fechaFin, string soliCodiRetiObservacion, string estado)
        {
            List<SolicitudCodigoDTO> entitys = new List<SolicitudCodigoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteriaExtranet);

            dbProvider.AddInParameter(command, helper.Emprnomb, DbType.String, nombreEmp);
            dbProvider.AddInParameter(command, helper.Emprnomb, DbType.String, nombreEmp);
            dbProvider.AddInParameter(command, helper.Tipousuanombre, DbType.String, tipoUsu);
            dbProvider.AddInParameter(command, helper.Tipousuanombre, DbType.String, tipoUsu);
            dbProvider.AddInParameter(command, helper.Tipocontnombre, DbType.String, tipoCont);
            dbProvider.AddInParameter(command, helper.Tipocontnombre, DbType.String, tipoCont);
            dbProvider.AddInParameter(command, helper.Barrnombbarrtran, DbType.String, barrTran);
            dbProvider.AddInParameter(command, helper.Barrnombbarrtran, DbType.String, barrTran);
            dbProvider.AddInParameter(command, helper.Clinombre, DbType.String, cliNomb);
            dbProvider.AddInParameter(command, helper.Clinombre, DbType.String, cliNomb);
            dbProvider.AddInParameter(command, helper.Solicodiretifechainicio, DbType.DateTime, fechaIni);
            dbProvider.AddInParameter(command, helper.Solicodiretifechainicio, DbType.DateTime, fechaIni);
            dbProvider.AddInParameter(command, helper.Solicodiretifechafin, DbType.DateTime, fechaFin);
            dbProvider.AddInParameter(command, helper.Solicodiretifechafin, DbType.DateTime, fechaFin);
            dbProvider.AddInParameter(command, helper.Solicodiretiobservacion, DbType.String, soliCodiRetiObservacion);
            dbProvider.AddInParameter(command, helper.Solicodiretiobservacion, DbType.String, soliCodiRetiObservacion);
            dbProvider.AddInParameter(command, helper.Solicodiretiestado, DbType.String, estado);
            dbProvider.AddInParameter(command, helper.Solicodiretiestado, DbType.String, estado);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<SolicitudCodigoDTO> ListarCodigoRetiro(string nombreEmp, string tipoUsu, string tipoCont, string barrTran, string cliNomb, DateTime? fechaIni, DateTime? fechaFin, string soliCodiRetiObservacion, string estado)
        {
            List<SolicitudCodigoDTO> entitys = new List<SolicitudCodigoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListarCodigoRetiro);

            dbProvider.AddInParameter(command, helper.Emprnomb, DbType.String, nombreEmp);
            dbProvider.AddInParameter(command, helper.Emprnomb, DbType.String, nombreEmp);
            dbProvider.AddInParameter(command, helper.Tipousuanombre, DbType.String, tipoUsu);
            dbProvider.AddInParameter(command, helper.Tipousuanombre, DbType.String, tipoUsu);
            dbProvider.AddInParameter(command, helper.Tipocontnombre, DbType.String, tipoCont);
            dbProvider.AddInParameter(command, helper.Tipocontnombre, DbType.String, tipoCont);
            dbProvider.AddInParameter(command, helper.Barrnombbarrtran, DbType.String, barrTran);
            dbProvider.AddInParameter(command, helper.Barrnombbarrtran, DbType.String, barrTran);
            dbProvider.AddInParameter(command, helper.Clinombre, DbType.String, cliNomb);
            dbProvider.AddInParameter(command, helper.Clinombre, DbType.String, cliNomb);
            dbProvider.AddInParameter(command, helper.Solicodiretifechainicio, DbType.DateTime, fechaIni);
            dbProvider.AddInParameter(command, helper.Solicodiretifechainicio, DbType.DateTime, fechaIni);
            dbProvider.AddInParameter(command, helper.Solicodiretifechafin, DbType.DateTime, fechaFin);
            dbProvider.AddInParameter(command, helper.Solicodiretifechafin, DbType.DateTime, fechaFin);
            dbProvider.AddInParameter(command, helper.Solicodiretiobservacion, DbType.String, soliCodiRetiObservacion);
            dbProvider.AddInParameter(command, helper.Solicodiretiobservacion, DbType.String, soliCodiRetiObservacion);
            dbProvider.AddInParameter(command, helper.Solicodiretiestado, DbType.String, estado);
            dbProvider.AddInParameter(command, helper.Solicodiretiestado, DbType.String, estado);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<SolicitudCodigoDTO> ListarCodigoRetiroPaginado(string nombreEmp, string tipoUsu, string tipoCont, string barrTran, string cliNomb, DateTime? fechaIni, DateTime? fechaFin, string soliCodiRetiObservacion, string estado, int? pericodi, int NroPagina, int PageSize)
        {
            List<SolicitudCodigoDTO> entitys = new List<SolicitudCodigoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListarPaginado);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Emprnomb, DbType.String, nombreEmp);
            dbProvider.AddInParameter(command, helper.Emprnomb, DbType.String, nombreEmp);
            dbProvider.AddInParameter(command, helper.Tipousuanombre, DbType.String, tipoUsu);
            dbProvider.AddInParameter(command, helper.Tipousuanombre, DbType.String, tipoUsu);
            dbProvider.AddInParameter(command, helper.Tipocontnombre, DbType.String, tipoCont);
            dbProvider.AddInParameter(command, helper.Tipocontnombre, DbType.String, tipoCont);
            dbProvider.AddInParameter(command, helper.Barrnombbarrtran, DbType.String, barrTran);
            dbProvider.AddInParameter(command, helper.Barrnombbarrtran, DbType.String, barrTran);
            dbProvider.AddInParameter(command, helper.Clinombre, DbType.String, cliNomb);
            dbProvider.AddInParameter(command, helper.Clinombre, DbType.String, cliNomb);
            /*dbProvider.AddInParameter(command, helper.Solicodiretifechainicio, DbType.DateTime, fechaIni);
            dbProvider.AddInParameter(command, helper.Solicodiretifechainicio, DbType.DateTime, fechaIni);
            dbProvider.AddInParameter(command, helper.Solicodiretifechafin, DbType.DateTime, fechaFin);
            dbProvider.AddInParameter(command, helper.Solicodiretifechafin, DbType.DateTime, fechaFin);*/
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Solicodiretiobservacion, DbType.String, soliCodiRetiObservacion);
            dbProvider.AddInParameter(command, helper.Solicodiretiobservacion, DbType.String, soliCodiRetiObservacion);
            dbProvider.AddInParameter(command, helper.Solicodiretiestado, DbType.String, estado);
            dbProvider.AddInParameter(command, helper.Solicodiretiestado, DbType.String, estado);
            dbProvider.AddInParameter(command, helper.Nropagina, DbType.Int32, NroPagina);
            dbProvider.AddInParameter(command, helper.Pagesize, DbType.Int32, PageSize);
            dbProvider.AddInParameter(command, helper.Nropagina, DbType.Int32, NroPagina);
            dbProvider.AddInParameter(command, helper.Pagesize, DbType.Int32, PageSize);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }
        public List<SolicitudCodigoDTO> ListarExportacionCodigoRetiro(string nombreEmp, string tipoUsu, string tipoCont, string barrTran, string cliNomb, DateTime? fechaIni, DateTime? fechaFin, string soliCodiRetiObservacion, string estado, int? pericodi, int NroPagina, int PageSize)
        {
            List<SolicitudCodigoDTO> entitys = new List<SolicitudCodigoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListarExportacionCodigoRetiro);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Emprnomb, DbType.String, nombreEmp);
            dbProvider.AddInParameter(command, helper.Emprnomb, DbType.String, nombreEmp);
            dbProvider.AddInParameter(command, helper.Tipousuanombre, DbType.String, tipoUsu);
            dbProvider.AddInParameter(command, helper.Tipousuanombre, DbType.String, tipoUsu);
            dbProvider.AddInParameter(command, helper.Tipocontnombre, DbType.String, tipoCont);
            dbProvider.AddInParameter(command, helper.Tipocontnombre, DbType.String, tipoCont);
            dbProvider.AddInParameter(command, helper.Barrnombbarrtran, DbType.String, barrTran);
            dbProvider.AddInParameter(command, helper.Barrnombbarrtran, DbType.String, barrTran);
            dbProvider.AddInParameter(command, helper.Clinombre, DbType.String, cliNomb);
            dbProvider.AddInParameter(command, helper.Clinombre, DbType.String, cliNomb);
            /*dbProvider.AddInParameter(command, helper.Solicodiretifechainicio, DbType.DateTime, fechaIni);
            dbProvider.AddInParameter(command, helper.Solicodiretifechainicio, DbType.DateTime, fechaIni);
            dbProvider.AddInParameter(command, helper.Solicodiretifechafin, DbType.DateTime, fechaFin);
            dbProvider.AddInParameter(command, helper.Solicodiretifechafin, DbType.DateTime, fechaFin);*/

            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, pericodi);

            dbProvider.AddInParameter(command, helper.Solicodiretiobservacion, DbType.String, soliCodiRetiObservacion);
            dbProvider.AddInParameter(command, helper.Solicodiretiobservacion, DbType.String, soliCodiRetiObservacion);
            dbProvider.AddInParameter(command, helper.Solicodiretiestado, DbType.String, estado);
            dbProvider.AddInParameter(command, helper.Solicodiretiestado, DbType.String, estado);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Nropagina, DbType.Int32, NroPagina);
            dbProvider.AddInParameter(command, helper.Pagesize, DbType.Int32, PageSize);
            dbProvider.AddInParameter(command, helper.Nropagina, DbType.Int32, NroPagina);
            dbProvider.AddInParameter(command, helper.Pagesize, DbType.Int32, PageSize);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<SolicitudCodigoDTO> GetByCriteria(string nombreEmp, string tipoUsu, string tipoCont, string barrTran, string cliNomb, DateTime? fechaIni, DateTime? fechaFin, string soliCodiRetiObservacion, string estado, string codRetiro, int nroPagina, int pageSize)
        {
            List<SolicitudCodigoDTO> entitys = new List<SolicitudCodigoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);

            dbProvider.AddInParameter(command, helper.Emprnomb, DbType.String, nombreEmp);
            dbProvider.AddInParameter(command, helper.Emprnomb, DbType.String, nombreEmp);
            dbProvider.AddInParameter(command, helper.Tipousuanombre, DbType.String, tipoUsu);
            dbProvider.AddInParameter(command, helper.Tipousuanombre, DbType.String, tipoUsu);
            dbProvider.AddInParameter(command, helper.Tipocontnombre, DbType.String, tipoCont);
            dbProvider.AddInParameter(command, helper.Tipocontnombre, DbType.String, tipoCont);
            dbProvider.AddInParameter(command, helper.Barrnombbarrtran, DbType.String, barrTran);
            dbProvider.AddInParameter(command, helper.Barrnombbarrtran, DbType.String, barrTran);
            dbProvider.AddInParameter(command, helper.Clinombre, DbType.String, cliNomb);
            dbProvider.AddInParameter(command, helper.Clinombre, DbType.String, cliNomb);
            dbProvider.AddInParameter(command, helper.Solicodiretifechainicio, DbType.DateTime, fechaIni);
            dbProvider.AddInParameter(command, helper.Solicodiretifechainicio, DbType.DateTime, fechaIni);
            dbProvider.AddInParameter(command, helper.Solicodiretifechafin, DbType.DateTime, fechaFin);
            dbProvider.AddInParameter(command, helper.Solicodiretifechafin, DbType.DateTime, fechaFin);
            dbProvider.AddInParameter(command, helper.Solicodiretiobservacion, DbType.String, soliCodiRetiObservacion);
            dbProvider.AddInParameter(command, helper.Solicodiretiobservacion, DbType.String, soliCodiRetiObservacion);
            dbProvider.AddInParameter(command, helper.Solicodiretiestado, DbType.String, estado);
            dbProvider.AddInParameter(command, helper.Solicodiretiestado, DbType.String, estado);
            dbProvider.AddInParameter(command, helper.Solicodireticodigo, DbType.String, codRetiro);
            dbProvider.AddInParameter(command, helper.Solicodireticodigo, DbType.String, codRetiro);
            dbProvider.AddInParameter(command, helper.Nropagina, DbType.Int32, nroPagina);
            dbProvider.AddInParameter(command, helper.Pagesize, DbType.Int32, pageSize);
            dbProvider.AddInParameter(command, helper.Nropagina, DbType.Int32, nroPagina);
            dbProvider.AddInParameter(command, helper.Pagesize, DbType.Int32, pageSize);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public int ObtenerNroRegistros(string nombreEmp, string tipoUsu, string tipoCont, string barrTran, string cliNomb, DateTime? fechaIni, DateTime? fechaFin, string soliCodiRetiObservacion, string estado, int? pericodi)
        {
            int NroRegistros = 0;
            List<SolicitudCodigoDTO> entitys = new List<SolicitudCodigoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlTotalRecords);

            dbProvider.AddInParameter(command, helper.Emprnomb, DbType.String, nombreEmp);
            dbProvider.AddInParameter(command, helper.Emprnomb, DbType.String, nombreEmp);
            dbProvider.AddInParameter(command, helper.Tipousuanombre, DbType.String, tipoUsu);
            dbProvider.AddInParameter(command, helper.Tipousuanombre, DbType.String, tipoUsu);
            dbProvider.AddInParameter(command, helper.Tipocontnombre, DbType.String, tipoCont);
            dbProvider.AddInParameter(command, helper.Tipocontnombre, DbType.String, tipoCont);
            dbProvider.AddInParameter(command, helper.Barrnombbarrtran, DbType.String, barrTran);
            dbProvider.AddInParameter(command, helper.Barrnombbarrtran, DbType.String, barrTran);
            dbProvider.AddInParameter(command, helper.Clinombre, DbType.String, cliNomb);
            dbProvider.AddInParameter(command, helper.Clinombre, DbType.String, cliNomb);
            dbProvider.AddInParameter(command, helper.Solicodiretifechainicio, DbType.DateTime, fechaIni);
            dbProvider.AddInParameter(command, helper.Solicodiretifechainicio, DbType.DateTime, fechaIni);
            dbProvider.AddInParameter(command, helper.Solicodiretifechafin, DbType.DateTime, fechaFin);
            dbProvider.AddInParameter(command, helper.Solicodiretifechafin, DbType.DateTime, fechaFin);
            dbProvider.AddInParameter(command, helper.Solicodiretiobservacion, DbType.String, soliCodiRetiObservacion);
            dbProvider.AddInParameter(command, helper.Solicodiretiobservacion, DbType.String, soliCodiRetiObservacion);
            dbProvider.AddInParameter(command, helper.Solicodiretiestado, DbType.String, estado);
            dbProvider.AddInParameter(command, helper.Solicodiretiestado, DbType.String, estado);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, pericodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    NroRegistros = Convert.ToInt32(dr["NroRegistros"].ToString().Trim());
                }
            }

            return NroRegistros;
        }

        public int GetCodigoGenerado()
        {
            int newId = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlCodigoGenerado);
            newId = Int32.Parse(dbProvider.ExecuteScalar(command).ToString());

            return newId;
        }

        public SolicitudCodigoDTO GetByCodigoRetiCodigo(System.String sTRetCodigo)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerPorCodigoRetiroCodigo);

            dbProvider.AddInParameter(command, helper.Tretcodigo, DbType.String, sTRetCodigo);

            SolicitudCodigoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = new SolicitudCodigoDTO();

                    int iTrettabla = dr.GetOrdinal(this.helper.Trettabla);
                    if (!dr.IsDBNull(iTrettabla)) entity.TretTabla = dr.GetString(iTrettabla);

                    int iTretcoresocoresccodi = dr.GetOrdinal(this.helper.Tretcoresocoresccodi);
                    if (!dr.IsDBNull(iTretcoresocoresccodi)) entity.SoliCodiRetiCodi = dr.GetInt32(iTretcoresocoresccodi);

                    int iTretcodigo = dr.GetOrdinal(this.helper.Tretcodigo);
                    if (!dr.IsDBNull(iTretcodigo)) entity.SoliCodiRetiCodigo = dr.GetString(iTretcodigo);

                    int iBarrcodi = dr.GetOrdinal(this.helper.Barrcodi);
                    if (!dr.IsDBNull(iBarrcodi)) entity.BarrCodi = dr.GetInt32(iBarrcodi);

                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.EmprCodi = dr.GetInt32(iEmprcodi);

                    int iClicodi = dr.GetOrdinal(this.helper.Clicodi);
                    if (!dr.IsDBNull(iClicodi)) entity.CliCodi = dr.GetInt32(iClicodi);

                }
            }

            return entity;
        }

        public SolicitudCodigoDTO GetCodigoRetiroByCodigo(string sCoReSoCodigo)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetCodigoRetiroByCodigo);

            dbProvider.AddInParameter(command, helper.Solicodireticodigo, DbType.String, sCoReSoCodigo);

            SolicitudCodigoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = new SolicitudCodigoDTO();

                    int iSolicodireticodi = dr.GetOrdinal(this.helper.Solicodireticodi);
                    if (!dr.IsDBNull(iSolicodireticodi)) entity.SoliCodiRetiCodi = dr.GetInt32(iSolicodireticodi);

                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.EmprCodi = dr.GetInt32(iEmprcodi);

                    int iBarrcodi = dr.GetOrdinal(this.helper.Barrcodi);
                    if (!dr.IsDBNull(iBarrcodi)) entity.BarrCodi = dr.GetInt32(iBarrcodi);

                    int iUsuacodi = dr.GetOrdinal(this.helper.Usuacodi);
                    if (!dr.IsDBNull(iUsuacodi)) entity.UsuaCodi = dr.GetString(iUsuacodi);

                    int iTipocontcodi = dr.GetOrdinal(this.helper.Tipocontcodi);
                    if (!dr.IsDBNull(iTipocontcodi)) entity.TipoContCodi = dr.GetInt32(iTipocontcodi);

                    int iTipousuacodi = dr.GetOrdinal(this.helper.Tipousuacodi);
                    if (!dr.IsDBNull(iTipousuacodi)) entity.TipoUsuaCodi = dr.GetInt32(iTipousuacodi);

                    int iClicodi = dr.GetOrdinal(this.helper.Clicodi);
                    if (!dr.IsDBNull(iClicodi)) entity.CliCodi = dr.GetInt32(iClicodi);

                    int iSolicodireticodiGO = dr.GetOrdinal(this.helper.Solicodireticodigo);
                    if (!dr.IsDBNull(iSolicodireticodiGO)) entity.SoliCodiRetiCodigo = dr.GetString(iSolicodireticodiGO);

                    int iSolicodiretifecharegistro = dr.GetOrdinal(this.helper.Solicodiretifecharegistro);
                    if (!dr.IsDBNull(iSolicodiretifecharegistro)) entity.SoliCodiRetiFechaRegistro = dr.GetDateTime(iSolicodiretifecharegistro);

                    int iSolicodiretidescripcion = dr.GetOrdinal(this.helper.Solicodiretidescripcion);
                    if (!dr.IsDBNull(iSolicodiretidescripcion)) entity.SoliCodiRetiDescripcion = dr.GetString(iSolicodiretidescripcion);

                    int iSolicodiretidetalleamplio = dr.GetOrdinal(this.helper.Solicodiretidetalleamplio);
                    if (!dr.IsDBNull(iSolicodiretidetalleamplio)) entity.SoliCodiRetiDetalleAmplio = dr.GetString(iSolicodiretidetalleamplio);

                    int iSolicodiretifechainicio = dr.GetOrdinal(this.helper.Solicodiretifechainicio);
                    if (!dr.IsDBNull(iSolicodiretifechainicio)) entity.SoliCodiRetiFechaInicio = dr.GetDateTime(iSolicodiretifechainicio);

                    int iSolicodiretifechafin = dr.GetOrdinal(this.helper.Solicodiretifechafin);
                    if (!dr.IsDBNull(iSolicodiretifechafin)) entity.SoliCodiRetiFechaFin = dr.GetDateTime(iSolicodiretifechafin);

                    int iSolicodiretiobservacion = dr.GetOrdinal(this.helper.Solicodiretiobservacion);
                    if (!dr.IsDBNull(iSolicodiretiobservacion)) entity.SoliCodiRetiObservacion = dr.GetString(iSolicodiretiobservacion);

                    int iSolicodiretiestado = dr.GetOrdinal(this.helper.Solicodiretiestado);
                    if (!dr.IsDBNull(iSolicodiretiestado)) entity.SoliCodiRetiEstado = dr.GetString(iSolicodiretiestado);

                    int iCoesusername = dr.GetOrdinal(this.helper.Coesusername);
                    if (!dr.IsDBNull(iCoesusername)) entity.CoesUserName = dr.GetString(iCoesusername);

                    int iSolicodiretifecins = dr.GetOrdinal(this.helper.Solicodiretifecins);
                    if (!dr.IsDBNull(iSolicodiretifecins)) entity.SoliCodiRetiFecIns = dr.GetDateTime(iSolicodiretifecins);

                    int iSolicodiretifecact = dr.GetOrdinal(this.helper.Solicodiretifecact);
                    if (!dr.IsDBNull(iSolicodiretifecact)) entity.SoliCodiRetiFecAct = dr.GetDateTime(iSolicodiretifecact);

                    int iSolicodiretifechasolbaja = dr.GetOrdinal(this.helper.Solicodiretifechasolbaja);
                    if (!dr.IsDBNull(iSolicodiretifechasolbaja)) entity.SoliCodiretiFechaSolBaja = dr.GetDateTime(iSolicodiretifechasolbaja);

                    int iSolicodiretifechadebaja = dr.GetOrdinal(this.helper.Solicodiretifechadebaja);
                    if (!dr.IsDBNull(iSolicodiretifechadebaja)) entity.SoliCodiRetiFechaBaja = dr.GetDateTime(iSolicodiretifechadebaja);

                }
            }
            return entity;
        }

        public SolicitudCodigoDTO CodigoRetiroVigenteByPeriodo(int iPericodi, System.String sCodigo)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlCodigoRetiroVigenteByPeriodo);
            dbProvider.AddInParameter(command, helper.Tretcodigo, DbType.String, sCodigo);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, iPericodi);
            SolicitudCodigoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = new SolicitudCodigoDTO();

                    int iTrettabla = dr.GetOrdinal(this.helper.Trettabla);
                    if (!dr.IsDBNull(iTrettabla)) entity.TretTabla = dr.GetString(iTrettabla);

                    int iTretcoresocoresccodi = dr.GetOrdinal(this.helper.Tretcoresocoresccodi);
                    if (!dr.IsDBNull(iTretcoresocoresccodi)) entity.SoliCodiRetiCodi = dr.GetInt32(iTretcoresocoresccodi);

                    int iTretcodigo = dr.GetOrdinal(this.helper.Tretcodigo);
                    if (!dr.IsDBNull(iTretcodigo)) entity.SoliCodiRetiCodigo = dr.GetString(iTretcodigo);

                    int iBarrcodi = dr.GetOrdinal(this.helper.Barrcodi);
                    if (!dr.IsDBNull(iBarrcodi)) entity.BarrCodi = dr.GetInt32(iBarrcodi);

                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.EmprCodi = dr.GetInt32(iEmprcodi);

                    int iClicodi = dr.GetOrdinal(this.helper.Clicodi);
                    if (!dr.IsDBNull(iClicodi)) entity.CliCodi = dr.GetInt32(iClicodi);

                }
            }

            return entity;
        }

        public int SolicitarBajar(SolicitudCodigoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSolicitarBajar);

            dbProvider.AddInParameter(command, helper.Solicodiretiestado, DbType.String, entity.SoliCodiRetiEstado);
            dbProvider.AddInParameter(command, helper.Solicodiretiusumodificacion, DbType.String, entity.SoliCodiRetiUsuRegistro);
            dbProvider.AddInParameter(command, helper.Solicodiretifecmodificacion, DbType.Date, DateTime.Now);
            dbProvider.AddInParameter(command, helper.Solicodireticodi, DbType.Int32, entity.SoliCodiRetiCodi);
            return dbProvider.ExecuteNonQuery(command);
        }
        public int UpdateObservacion(SolicitudCodigoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdateObservacion);
            dbProvider.AddInParameter(command, "CORESODESCRIPCION", DbType.String, entity.SoliCodiRetiDescripcion?.TrimStart());
            dbProvider.AddInParameter(command, helper.Solicodireticodi, DbType.Int32, entity.SoliCodiRetiCodi);
            return dbProvider.ExecuteNonQuery(command);
        }
        // ASSETEC 2019-11
        public List<SolicitudCodigoDTO> ImportarPotenciasContratadas(int pericodi, int idEmpresa)
        {
            List<SolicitudCodigoDTO> entitys = new List<SolicitudCodigoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlImportarCodigosRetiroSolicitud);

            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, idEmpresa);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, pericodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SolicitudCodigoDTO entity = new SolicitudCodigoDTO();

                    // CORESOCODI
                    int iSolicodireticodi = dr.GetOrdinal(this.helper.Solicodireticodi);
                    if (!dr.IsDBNull(iSolicodireticodi)) entity.SoliCodiRetiCodi = dr.GetInt32(iSolicodireticodi);

                    // CORESOCODIGO
                    int iSolicodireticodiGO = dr.GetOrdinal(this.helper.Solicodireticodigo);
                    if (!dr.IsDBNull(iSolicodireticodiGO)) entity.SoliCodiRetiCodigo = dr.GetString(iSolicodireticodiGO);

                    // GENEMPRCODI
                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.EmprCodi = dr.GetInt32(iEmprcodi);

                    // CLiEmprcodi
                    int iClicodi = dr.GetOrdinal(this.helper.Clicodi);
                    if (!dr.IsDBNull(iClicodi)) entity.CliCodi = dr.GetInt32(iClicodi);

                    // TIPCONCODI
                    int iTipocontcodi = dr.GetOrdinal(this.helper.Tipocontcodi);
                    if (!dr.IsDBNull(iTipocontcodi)) entity.TipoContCodi = dr.GetInt32(iTipocontcodi);

                    // TIPUSUCODI
                    int iTipousuacodi = dr.GetOrdinal(this.helper.Tipousuacodi);
                    if (!dr.IsDBNull(iTipousuacodi)) entity.TipoUsuaCodi = dr.GetInt32(iTipousuacodi);

                    // BARRCODI
                    int iBarrcodi = dr.GetOrdinal(this.helper.Barrcodi);
                    if (!dr.IsDBNull(iBarrcodi)) entity.BarrCodi = dr.GetInt32(iBarrcodi);

                    // CORESOFECHAINICIO
                    int iSOLICODIRETIFECHAINICIO = dr.GetOrdinal(this.helper.Solicodiretifechainicio);
                    if (!dr.IsDBNull(iSOLICODIRETIFECHAINICIO)) entity.SoliCodiRetiFechaInicio = dr.GetDateTime(iSOLICODIRETIFECHAINICIO);

                    // CORESOFECHAFIN
                    int iSOLICODIRETIFECHAFIN = dr.GetOrdinal(this.helper.Solicodiretifechafin);
                    if (!dr.IsDBNull(iSOLICODIRETIFECHAFIN)) entity.SoliCodiRetiFechaFin = dr.GetDateTime(iSOLICODIRETIFECHAFIN);

                    // EMPRNOMB (EMPRNOMB)
                    int iEMPRNOMB = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEMPRNOMB)) entity.EmprNombre = dr.GetString(iEMPRNOMB);

                    // CLINOMBRE
                    int iCLINOMBRE = dr.GetOrdinal(this.helper.Clinombre);
                    if (!dr.IsDBNull(iCLINOMBRE)) entity.CliNombre = dr.GetString(iCLINOMBRE);

                    // BARRBARRATRANSFERENCIA
                    int iBARRNOMBBARRTRAN = dr.GetOrdinal(this.helper.Barrnombbarrtran);
                    if (!dr.IsDBNull(iBARRNOMBBARRTRAN)) entity.BarrNombBarrTran = dr.GetString(iBARRNOMBBARRTRAN);

                    // TIPCONNOMBRE
                    int iTIPOCONTNOMBRE = dr.GetOrdinal(this.helper.Tipocontnombre);
                    if (!dr.IsDBNull(iTIPOCONTNOMBRE)) entity.TipoContNombre = dr.GetString(iTIPOCONTNOMBRE);

                    // // TIPUSUNOMBRE
                    int iTIPOUSUANOMBRE = dr.GetOrdinal(this.helper.Tipousuanombre);
                    if (!dr.IsDBNull(iTIPOUSUANOMBRE)) entity.TipoUsuaNombre = dr.GetString(iTIPOUSUANOMBRE);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
