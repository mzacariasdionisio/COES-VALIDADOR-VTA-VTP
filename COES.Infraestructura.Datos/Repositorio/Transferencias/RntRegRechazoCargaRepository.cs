using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Transferencias;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Transferencias;

namespace COES.Infraestructura.Datos.Repositorio.Transferencias
{
    /// <summary>
    /// Clase de acceso a datos de la tabla RNT_REG_RECHAZO_CARGA
    /// </summary>
    public class RntRegrechazocargaRepository : RepositoryBase, IRntRegRechazoCargaRepository
    {
        public RntRegrechazocargaRepository(string strConn)
            : base(strConn)
        {
        }

        RntRegRechazoCargaHelper helper = new RntRegRechazoCargaHelper();

        /// <summary>
        /// Registrar Rechazo de Carga
        /// </summary>
        public int Save(RntRegRechazoCargaDTO entity, int codEnvio, IDbConnection conn, DbTransaction tran, int corrId)
        {
            int id = 1;

            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlGetMaxId;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            if (corrId != 0)
                id = corrId + 1;
            else
            {
                object result = command.ExecuteNonQuery();
                if (result != null) id = Convert.ToInt32(result);
            }

            DbCommand command2 = (DbCommand)conn.CreateCommand();

            command2.CommandText = helper.SqlSave;
            command2.Transaction = tran;
            command2.Connection = (DbConnection)conn;

            IDbDataParameter param = command2.CreateParameter();
            param.ParameterName = helper.RrcGrupoEnvio;
            param.Value = entity.RrcGrupoEnvio;
            command2.Parameters.Add(param);

            param = command2.CreateParameter(); param.ParameterName = helper.EnvioCodi; param.Value = codEnvio; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Regrechazocargacodi; param.Value = id; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Rrcempresageneradora; param.Value = entity.RrcEmpresaGeneradora; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.RrcCliente; param.Value = entity.RrcCliente; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Areacodi; param.Value = entity.AreaCodi; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Periodocodi; param.Value = entity.PeriodoCodi; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Rrcniveltension; param.Value = entity.RrcNivelTension; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Evencodi; param.Value = entity.EvenCodi; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Rrcfechainicio; param.Value = entity.RrcFechaInicio; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Rrcfechafin; param.Value = entity.RrcFechaFin; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Rrcsubestaciondstrb; param.Value = entity.RrcSubestacionDstrb; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Rrcniveltensionsed; param.Value = entity.RrcNivelTensionSed; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Rrccodialimentador; param.Value = entity.RrcCodiAlimentador; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Rrcenergiaens; param.Value = entity.RrcEnergiaEns; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Rrcnrcf; param.Value = entity.RrcNrcf; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Rrcef; param.Value = entity.RrcEf; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Rrccompensacion; param.Value = entity.RrcCompensacion; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Rrcestado; param.Value = entity.RrcEstado; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Rrcusuariocreacion; param.Value = entity.RrcUsuarioCreacion; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Rrcpk; param.Value = entity.RrcPk; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Rrccompensable; param.Value = entity.RrcCompensable; command2.Parameters.Add(param);//compensable
            param = command2.CreateParameter(); param.ParameterName = helper.Rrcensfk; param.Value = entity.RrcEnsFk; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Rrcevencodidesc; param.Value = entity.RrcEvenCodiDesc; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.BarrCodi; param.Value = entity.Barrcodi; command2.Parameters.Add(param);

            command2.ExecuteNonQuery();

            return id;

        }

        /// <summary>
        /// Actualizar rechazo de Carga
        /// </summary>
        public int Update(RntRegRechazoCargaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Rrcempresageneradora, DbType.Int32, entity.RrcEmpresaGeneradora);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.EmprCodi);
            dbProvider.AddInParameter(command, helper.Areacodi, DbType.Int32, entity.AreaCodi);
            dbProvider.AddInParameter(command, helper.Periodocodi, DbType.Int32, entity.PeriodoCodi);
            dbProvider.AddInParameter(command, helper.Rrcniveltension, DbType.Int32, entity.RrcNivelTension);
            dbProvider.AddInParameter(command, helper.Evencodi, DbType.Int32, entity.EvenCodi);
            dbProvider.AddInParameter(command, helper.Rrcfechainicio, DbType.DateTime, entity.RrcFechaInicio);
            dbProvider.AddInParameter(command, helper.Rrcfechafin, DbType.DateTime, entity.RrcFechaFin);
            dbProvider.AddInParameter(command, helper.Rrcsubestaciondstrb, DbType.String, entity.RrcSubestacionDstrb);
            dbProvider.AddInParameter(command, helper.Rrcniveltensionsed, DbType.Decimal, entity.RrcNivelTensionSed);
            dbProvider.AddInParameter(command, helper.Rrccodialimentador, DbType.String, entity.RrcCodiAlimentador);
            dbProvider.AddInParameter(command, helper.Rrcenergiaens, DbType.Decimal, entity.RrcEnergiaEns);
            dbProvider.AddInParameter(command, helper.Rrcnrcf, DbType.Int32, entity.RrcNrcf);
            dbProvider.AddInParameter(command, helper.Rrcef, DbType.Decimal, entity.RrcEf);
            dbProvider.AddInParameter(command, helper.Rrccompensacion, DbType.Decimal, entity.RrcCompensacion);
            dbProvider.AddInParameter(command, helper.Rrcestado, DbType.Int32, entity.RrcEstado);
            dbProvider.AddInParameter(command, helper.Rrcusuarioupdate, DbType.String, entity.RrcUsuarioUpdate);
            dbProvider.AddInParameter(command, helper.Rrcfechaupdate, DbType.DateTime, entity.RrcFechaUpdate);
            dbProvider.AddInParameter(command, helper.Regrechazocargacodi, DbType.Int32, entity.RegRechazoCargaCodi);

            dbProvider.ExecuteNonQuery(command);

            return entity.RegRechazoCargaCodi;
        }

        public int UpdateRC(int empresaGeneradora, int periodoCodi, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command2 = (DbCommand)conn.CreateCommand();

            command2.CommandText = helper.SqlUpdate;
            command2.Transaction = tran;
            command2.Connection = (DbConnection)conn;

            IDbDataParameter param = command2.CreateParameter();
            param = command2.CreateParameter(); param.ParameterName = helper.Rrcestado; param.Value = 2; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Rrcempresageneradora; param.Value = empresaGeneradora; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Periodocodi; param.Value = periodoCodi; command2.Parameters.Add(param);

            int id = command2.ExecuteNonQuery();

            return id;
        }

        /// <summary>
        /// Actualizar rechazo de Carga
        /// </summary>
        public int UpdateNRCF(RntRegRechazoCargaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdateNRCF);

            dbProvider.AddInParameter(command, helper.Rrcnrcf, DbType.Int32, entity.RrcNrcf);
            if (entity.RrcUsuarioUpdate == null)
            {
                dbProvider.AddInParameter(command, helper.Rrcusuarioupdate, DbType.String, entity.RrcUsuarioCreacion);
            }
            else
            {
                dbProvider.AddInParameter(command, helper.Rrcusuarioupdate, DbType.String, entity.RrcUsuarioUpdate);
            }
            if (entity.RrcFechaUpdate == null)
            {
                dbProvider.AddInParameter(command, helper.Rrcfechaupdate, DbType.DateTime, entity.RrcFechaCreacion);
            }
            else
            {
                dbProvider.AddInParameter(command, helper.Rrcfechaupdate, DbType.DateTime, entity.RrcFechaUpdate);
            }
            dbProvider.AddInParameter(command, helper.Rrcempresageneradora, DbType.Int32, entity.RrcEmpresaGeneradora);
            dbProvider.AddInParameter(command, helper.Periodocodi, DbType.Int32, entity.PeriodoCodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.EmprCodi);
            dbProvider.AddInParameter(command, helper.Areacodi, DbType.Int32, entity.AreaCodi);
            dbProvider.AddInParameter(command, helper.Rrcniveltension, DbType.Int32, entity.RrcNivelTension);
            dbProvider.AddInParameter(command, helper.Rrcniveltensionsed, DbType.String, entity.RrcNivelTensionSed);
            dbProvider.AddInParameter(command, helper.Rrcsubestaciondstrb, DbType.String, entity.RrcSubestacionDstrb);
            dbProvider.AddInParameter(command, helper.Rrccodialimentador, DbType.String, entity.RrcCodiAlimentador);

            dbProvider.ExecuteNonQuery(command);

            return 1;
        }

        /// <summary>
        /// Actualizar rechazo de Carga
        /// </summary>
        public int UpdateEF(RntRegRechazoCargaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdateEF);

            dbProvider.AddInParameter(command, helper.Rrcef, DbType.Decimal, entity.RrcEf);
            dbProvider.AddInParameter(command, helper.Rrccompensacion, DbType.Decimal, entity.RrcCompensacion);
            if (entity.RrcUsuarioUpdate == null)
            {
                dbProvider.AddInParameter(command, helper.Rrcusuarioupdate, DbType.String, entity.RrcUsuarioCreacion);
            }
            else
            {
                dbProvider.AddInParameter(command, helper.Rrcusuarioupdate, DbType.String, entity.RrcUsuarioUpdate);
            }
            if (entity.RrcFechaUpdate == null)
            {
                dbProvider.AddInParameter(command, helper.Rrcfechaupdate, DbType.DateTime, entity.RrcFechaCreacion);
            }
            else
            {
                dbProvider.AddInParameter(command, helper.Rrcfechaupdate, DbType.DateTime, entity.RrcFechaUpdate);
            }
            dbProvider.AddInParameter(command, helper.Regrechazocargacodi, DbType.Int32, entity.RegRechazoCargaCodi);

            dbProvider.ExecuteNonQuery(command);

            return 1;
        }

        /// <summary>
        /// Eliminar rechazo de Carga
        /// - Realmente no se elimina el registro de la Base de datos, sino se cambia de Estado para que no se pueda visualizar
        /// </summary>
        public void Delete(int regrechazocargacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);
            dbProvider.AddInParameter(command, helper.Regrechazocargacodi, DbType.Int32, regrechazocargacodi);
            dbProvider.ExecuteNonQuery(command);
        }

        /// <summary>
        /// Obtener registro de Rechazo de Carga - uno por uno
        /// </summary>
        public RntRegRechazoCargaDTO GetById(int regrechazocargacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);
            dbProvider.AddInParameter(command, helper.Regrechazocargacodi, DbType.Int32, regrechazocargacodi);
            RntRegRechazoCargaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        /// <summary>
        /// Obtener listado de Rechazo de Carga por criterios de EmpresaGeneradora, Periodo, Cliente, Punto Entrega, Nivel de Tension
        /// Solo obtiene activos
        /// </summary>
        public List<RntRegRechazoCargaDTO> List(int? EmpresaGeneradora, int Periodo, int Cliente, int PEntrega)
        {
            List<RntRegRechazoCargaDTO> entitys = new List<RntRegRechazoCargaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);
            dbProvider.AddInParameter(command, helper.Rrcempresageneradora, DbType.Int32, EmpresaGeneradora);
            dbProvider.AddInParameter(command, helper.Rrcempresageneradora, DbType.Int32, EmpresaGeneradora);
            dbProvider.AddInParameter(command, helper.Periodocodi, DbType.Int32, Periodo);
            dbProvider.AddInParameter(command, helper.Periodocodi, DbType.Int32, Periodo);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, Cliente);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, Cliente);
            dbProvider.AddInParameter(command, helper.RrcGrupoEnvio, DbType.Int32, PEntrega);
            dbProvider.AddInParameter(command, helper.RrcGrupoEnvio, DbType.Int32, PEntrega);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        /// <summary>
        /// Obtener listado de Rechazo de Carga por criterios de EmpresaGeneradora, Periodo, Cliente, Punto Entrega, Nivel de Tension
        /// Solo obtiene activos
        /// </summary>
        public List<RntRegRechazoCargaDTO> ListPaginado(int? EmpresaGeneradora, int Periodo, int Cliente, int PEntrega, int NroPaginado, int PageSize)
        {
            List<RntRegRechazoCargaDTO> entitys = new List<RntRegRechazoCargaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListPaginado);
            dbProvider.AddInParameter(command, helper.Rrcempresageneradora, DbType.Int32, EmpresaGeneradora);
            dbProvider.AddInParameter(command, helper.Rrcempresageneradora, DbType.Int32, EmpresaGeneradora);
            dbProvider.AddInParameter(command, helper.Periodocodi, DbType.Int32, Periodo);
            dbProvider.AddInParameter(command, helper.Periodocodi, DbType.Int32, Periodo);
            dbProvider.AddInParameter(command, helper.RrcCliente, DbType.Int32, Cliente);
            dbProvider.AddInParameter(command, helper.RrcCliente, DbType.Int32, Cliente);
            dbProvider.AddInParameter(command, helper.RrcGrupoEnvio, DbType.Int32, PEntrega);
            dbProvider.AddInParameter(command, helper.RrcGrupoEnvio, DbType.Int32, PEntrega);
            dbProvider.AddInParameter(command, helper.PageNumber, DbType.Int32, NroPaginado);
            dbProvider.AddInParameter(command, helper.PageSize, DbType.Int32, PageSize);
            dbProvider.AddInParameter(command, helper.PageNumber, DbType.Int32, NroPaginado);
            dbProvider.AddInParameter(command, helper.PageSize, DbType.Int32, PageSize);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        /// <summary>
        /// Obtener listado de Rechazo de Carga por criterios de EmpresaGeneradora, Periodo, Cliente, Punto Entrega, Nivel de Tension
        /// Solo obtiene activos
        /// </summary>
        public List<RntRegRechazoCargaDTO> ListReporte(int? EmpresaGeneradora, int Periodo, int Cliente, int PEntrega)
        {
            List<RntRegRechazoCargaDTO> entitys = new List<RntRegRechazoCargaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListReporte);
            dbProvider.AddInParameter(command, helper.Rrcempresageneradora, DbType.Int32, EmpresaGeneradora);
            dbProvider.AddInParameter(command, helper.Rrcempresageneradora, DbType.Int32, EmpresaGeneradora);
            dbProvider.AddInParameter(command, helper.Periodocodi, DbType.Int32, Periodo);
            dbProvider.AddInParameter(command, helper.Periodocodi, DbType.Int32, Periodo);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, Cliente);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, Cliente);
            dbProvider.AddInParameter(command, helper.BarrCodi, DbType.Int32, PEntrega);
            dbProvider.AddInParameter(command, helper.BarrCodi, DbType.Int32, PEntrega);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<RntRegRechazoCargaDTO> ListReporteGrilla(int codEnvio)
        {
            List<RntRegRechazoCargaDTO> entitys = new List<RntRegRechazoCargaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListReporteGrilla);
            dbProvider.AddInParameter(command, helper.EnvioCodi, DbType.Int32, codEnvio);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        /// <summary>
        /// Obtener listado de Rechazo de Carga por criterios de EmpresaGeneradora, Periodo, Cliente, Punto Entrega, Nivel de Tension
        /// Solo obtiene activos
        /// </summary>
        public List<RntRegRechazoCargaDTO> ListReportePaginado(int? EmpresaGeneradora, int Periodo, int Cliente, int PEntrega, int NroPaginado, int PageSize)
        {
            List<RntRegRechazoCargaDTO> entitys = new List<RntRegRechazoCargaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListReportePaginado);
            dbProvider.AddInParameter(command, helper.Rrcempresageneradora, DbType.Int32, EmpresaGeneradora);
            dbProvider.AddInParameter(command, helper.Rrcempresageneradora, DbType.Int32, EmpresaGeneradora);
            dbProvider.AddInParameter(command, helper.Periodocodi, DbType.Int32, Periodo);
            dbProvider.AddInParameter(command, helper.Periodocodi, DbType.Int32, Periodo);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, Cliente);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, Cliente);
            dbProvider.AddInParameter(command, helper.BarrCodi, DbType.Int32, PEntrega);
            dbProvider.AddInParameter(command, helper.BarrCodi, DbType.Int32, PEntrega);
            dbProvider.AddInParameter(command, helper.PageNumber, DbType.Int32, NroPaginado);
            dbProvider.AddInParameter(command, helper.PageSize, DbType.Int32, PageSize);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        /// <summary>
        /// Obtener listado de Rechazo de Carga por criterios para Auditoria - EmpresaGeneradora, Periodo, Cliente, Punto Entrega, Nivel de Tension, Fecha
        /// Obtiene activos y no activos (eliminados)
        /// </summary>
        public List<RntRegRechazoCargaDTO> GetByCriteria(string user, int? EmpresaGeneradora, int Periodo, int Cliente, int PEntrega, int Ntension, DateTime desde)
        {
            List<RntRegRechazoCargaDTO> entitys = new List<RntRegRechazoCargaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, helper.Rrcusuariocreacion, DbType.String, user);
            dbProvider.AddInParameter(command, helper.Rrcusuariocreacion, DbType.String, user);
            dbProvider.AddInParameter(command, helper.Rrcempresageneradora, DbType.Int32, EmpresaGeneradora);
            dbProvider.AddInParameter(command, helper.Rrcempresageneradora, DbType.Int32, EmpresaGeneradora);
            dbProvider.AddInParameter(command, helper.Periodocodi, DbType.Int32, Periodo);
            dbProvider.AddInParameter(command, helper.Periodocodi, DbType.Int32, Periodo);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, Cliente);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, Cliente);
            dbProvider.AddInParameter(command, helper.Areacodi, DbType.Int32, PEntrega);
            dbProvider.AddInParameter(command, helper.Areacodi, DbType.Int32, PEntrega);
            dbProvider.AddInParameter(command, helper.Rrcniveltension, DbType.Int32, Ntension);
            dbProvider.AddInParameter(command, helper.Rrcniveltension, DbType.Int32, Ntension);
            dbProvider.AddInParameter(command, helper.Rrcfechainicio, DbType.DateTime, desde);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        /// <summary>
        /// Obtener listado de Rechazo de Carga por criterios para Auditoria - EmpresaGeneradora, Periodo, Cliente, Punto Entrega, Nivel de Tension, Fecha
        /// Obtiene activos y no activos (eliminados)
        /// </summary>
        public List<RntRegRechazoCargaDTO> GetByCriteriaPaginado(string user, int? EmpresaGeneradora, int Periodo, int Cliente, int PEntrega, int Ntension, DateTime desde, int NroPaginado, int PageSize)
        {
            List<RntRegRechazoCargaDTO> entitys = new List<RntRegRechazoCargaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteriaPaginado);
            dbProvider.AddInParameter(command, helper.Rrcusuariocreacion, DbType.String, user);
            dbProvider.AddInParameter(command, helper.Rrcusuariocreacion, DbType.String, user);
            dbProvider.AddInParameter(command, helper.Rrcempresageneradora, DbType.Int32, EmpresaGeneradora);
            dbProvider.AddInParameter(command, helper.Rrcempresageneradora, DbType.Int32, EmpresaGeneradora);
            dbProvider.AddInParameter(command, helper.Periodocodi, DbType.Int32, Periodo);
            dbProvider.AddInParameter(command, helper.Periodocodi, DbType.Int32, Periodo);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, Cliente);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, Cliente);
            dbProvider.AddInParameter(command, helper.Areacodi, DbType.Int32, PEntrega);
            dbProvider.AddInParameter(command, helper.Areacodi, DbType.Int32, PEntrega);
            dbProvider.AddInParameter(command, helper.Rrcniveltension, DbType.Int32, Ntension);
            dbProvider.AddInParameter(command, helper.Rrcniveltension, DbType.Int32, Ntension);
            dbProvider.AddInParameter(command, helper.Rrcfechainicio, DbType.DateTime, desde);
            dbProvider.AddInParameter(command, helper.PageNumber, DbType.Int32, NroPaginado);
            dbProvider.AddInParameter(command, helper.PageSize, DbType.Int32, PageSize);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<RntRegRechazoCargaDTO> ListAuditoriaRechazoCarga(int Audittablacodi, int Tauditcodi)
        {
            List<RntRegRechazoCargaDTO> entitys = new List<RntRegRechazoCargaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListAuditoriaRechazoCarga);
            dbProvider.AddInParameter(command, "AUDITTABLACODI", DbType.Int32, Audittablacodi);
            dbProvider.AddInParameter(command, "TAUDITCODI", DbType.Int32, Tauditcodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public int GetMaxId()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            int id = 1;

            object result = dbProvider.ExecuteScalar(command);
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public List<RntRegRechazoCargaDTO> ListAllRechazoCarga()
        {
            List<RntRegRechazoCargaDTO> entitys = new List<RntRegRechazoCargaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListAllRechazoCarga);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateList(dr));
                }
            }

            return entitys;
        }

        public List<RntRegRechazoCargaDTO> ListAllClienteRC()
        {
            List<RntRegRechazoCargaDTO> entitys = new List<RntRegRechazoCargaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListAllClienteRC);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateClientRC(dr));
                }
            }

            return entitys;
        }

        public List<RntRegRechazoCargaDTO> ListChangeClienteRC(int idCliente)
        {
            List<RntRegRechazoCargaDTO> entitys = new List<RntRegRechazoCargaDTO>();
            string query = String.Format(helper.SqlListChangeClienteRC, idCliente);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateList(dr));
                }
            }

            return entitys;
        }

    }
}
