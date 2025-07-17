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
    /// Clase de acceso a datos de la tabla RNT_REG_PUNTO_ENTREGA
    /// </summary>
    public class RntRegpuntoentregaRepository : RepositoryBase, IRntRegPuntoEntregaRepository
    {
        public RntRegpuntoentregaRepository(string strConn)
            : base(strConn)
        {
        }

        RntRegPuntoEntregaHelper helper = new RntRegPuntoEntregaHelper();

        /// <summary>
        /// Registrar Punto de Entrega
        /// </summary>
        public int Save(RntRegPuntoEntregaDTO entity, int codEnvio, IDbConnection conn, DbTransaction tran, int corrId)
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
            param.ParameterName = helper.Rpegrupoenvio; param.Value = entity.RpeGrupoEnvio; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.EnvioCodi; param.Value = codEnvio; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.RpeCodi; param.Value = id; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Rpeempresageneradora; param.Value = entity.RpeEmpresaGeneradora; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Rpecliente; param.Value = entity.RpeCliente; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Areacodi; param.Value = entity.AreaCodi; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Periodocodi; param.Value = entity.PeriodoCodi; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Rpeniveltension; param.Value = entity.RpeNivelTension; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Tipointcodi; param.Value = entity.TipoIntCodi; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Rpefechainicio; param.Value = entity.RpeFechaInicio; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Rpefechafin; param.Value = entity.RpeFechaFin; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Rpecompensacion; param.Value = entity.RpeCompensacion; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Rpecausainterrupcion; param.Value = entity.RpeCausaInterrupcion; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Rpetramfuermayor; param.Value = entity.RpeTramFuerMayor; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Rpeestado; param.Value = entity.RpeEstado; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Rpeusuariocreacion; param.Value = entity.RpeUsuarioCreacion; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.RpeNivelTensionDesc; param.Value = entity.RpeNivelTensionDesc; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.RpeTipoIntDesc; param.Value = entity.RpeTipoIntDesc; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.RpeNi; param.Value = entity.RpeNi; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.RpeKi; param.Value = entity.RpeKi; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.RpeEnergSem; param.Value = entity.RpeEnergSem; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.RpePrgFechaInicio; param.Value = entity.RpePrgFechaInicio; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.RpePrgFechaFin; param.Value = entity.RpePrgFechaFin; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.RpeEiE; param.Value = entity.RpeEiE; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Barrcodi; param.Value = entity.Barrcodi; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.RpeIncremento; param.Value = entity.RpeIncremento; command2.Parameters.Add(param);

            command2.ExecuteNonQuery();

            return id;
        }

        /// <summary>
        /// Actualizar Punto de Entrega
        /// </summary>
        public int Update(RntRegPuntoEntregaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Rpeempresageneradora, DbType.Int32, entity.RpeEmpresaGeneradora);
            dbProvider.AddInParameter(command, helper.Rpecliente, DbType.Int32, entity.RpeCliente);
            dbProvider.AddInParameter(command, helper.Areacodi, DbType.Int32, entity.AreaCodi);
            dbProvider.AddInParameter(command, helper.Periodocodi, DbType.Int32, entity.PeriodoCodi);
            dbProvider.AddInParameter(command, helper.Rpeniveltension, DbType.Int32, entity.RpeNivelTension);
            dbProvider.AddInParameter(command, helper.Tipointcodi, DbType.Int32, entity.TipoIntCodi);
            dbProvider.AddInParameter(command, helper.Rpefechainicio, DbType.DateTime, entity.RpeFechaInicio);
            dbProvider.AddInParameter(command, helper.Rpefechafin, DbType.DateTime, entity.RpeFechaFin);
            dbProvider.AddInParameter(command, helper.Rpecompensacion, DbType.Decimal, entity.RpeCompensacion);
            dbProvider.AddInParameter(command, helper.Rpecausainterrupcion, DbType.String, entity.RpeCausaInterrupcion);
            dbProvider.AddInParameter(command, helper.Rpetramfuermayor, DbType.String, entity.RpeTramFuerMayor);
            dbProvider.AddInParameter(command, helper.Rpeestado, DbType.Int32, entity.RpeEstado);
            dbProvider.AddInParameter(command, helper.Rpeusuarioupdate, DbType.String, entity.RpeUsuarioUpdate);
            dbProvider.AddInParameter(command, helper.Rpefechaupdate, DbType.DateTime, entity.RpeFechaUpdate);
            dbProvider.AddInParameter(command, helper.RpeCodi, DbType.Int32, entity.RegPuntoEntCodi);

            dbProvider.ExecuteNonQuery(command);

            return entity.RegPuntoEntCodi;
        }

        public int UpdatePE(int empresaGeneradora, int periodoCodi, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command2 = (DbCommand)conn.CreateCommand();

            command2.CommandText = helper.SqlUpdate;
            command2.Transaction = tran;
            command2.Connection = (DbConnection)conn;

            IDbDataParameter param = command2.CreateParameter();
            param = command2.CreateParameter(); param.ParameterName = helper.Rpeestado; param.Value = 2; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Rpeempresageneradora; param.Value = empresaGeneradora; command2.Parameters.Add(param);
            param = command2.CreateParameter(); param.ParameterName = helper.Periodocodi; param.Value = periodoCodi; command2.Parameters.Add(param);

            int id = command2.ExecuteNonQuery();

            return id;
        }
        /// <summary>
        /// Eliminar Punto de Entrega
        /// En realidad no se elimina, sino se cambia el Estado (no activo) para que no se visualice.
        /// </summary>

        public void Delete(int regpuntoentcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);
            dbProvider.AddInParameter(command, helper.RpeCodi, DbType.Int32, regpuntoentcodi);
            dbProvider.ExecuteNonQuery(command);
        }

        /// <summary>
        /// Listar Punto de Entrega uno por uno
        /// </summary>
        public RntRegPuntoEntregaDTO GetById(int regpuntoentcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.RpeCodi, DbType.Int32, regpuntoentcodi);
            RntRegPuntoEntregaDTO entity = null;

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
        /// Listar Punto de Entrega por parametros: Empresa Generadora, Periodo, Cliente, Punto de Entrega, Nivel de Tension
        /// </summary>
        public List<RntRegPuntoEntregaDTO> List(int? EmpresaGeneradora, int Periodo, int Cliente, int PEntrega)
        {
            List<RntRegPuntoEntregaDTO> entitys = new List<RntRegPuntoEntregaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);
            dbProvider.AddInParameter(command, helper.Rpeempresageneradora, DbType.Int32, EmpresaGeneradora);
            dbProvider.AddInParameter(command, helper.Rpeempresageneradora, DbType.Int32, EmpresaGeneradora);
            dbProvider.AddInParameter(command, helper.Periodocodi, DbType.Int32, Periodo);
            dbProvider.AddInParameter(command, helper.Periodocodi, DbType.Int32, Periodo);
            dbProvider.AddInParameter(command, helper.Rpecliente, DbType.Int32, Cliente);
            dbProvider.AddInParameter(command, helper.Rpecliente, DbType.Int32, Cliente);
            dbProvider.AddInParameter(command, helper.Barrcodi, DbType.Int32, PEntrega);
            dbProvider.AddInParameter(command, helper.Barrcodi, DbType.Int32, PEntrega);

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
        /// Listar Punto de Entrega por parametros: Empresa Generadora, Periodo, Cliente, Punto de Entrega, Nivel de Tension
        /// </summary>
        public List<RntRegPuntoEntregaDTO> ListPaginado(int? EmpresaGeneradora, int Periodo, int Cliente, int PEntrega, int NroPaginado, int PageSize)
        {
            List<RntRegPuntoEntregaDTO> entitys = new List<RntRegPuntoEntregaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListPaginado);
            dbProvider.AddInParameter(command, helper.Rpeempresageneradora, DbType.Int32, EmpresaGeneradora);
            dbProvider.AddInParameter(command, helper.Rpeempresageneradora, DbType.Int32, EmpresaGeneradora);
            dbProvider.AddInParameter(command, helper.Periodocodi, DbType.Int32, Periodo);
            dbProvider.AddInParameter(command, helper.Periodocodi, DbType.Int32, Periodo);
            dbProvider.AddInParameter(command, helper.Rpecliente, DbType.Int32, Cliente);
            dbProvider.AddInParameter(command, helper.Rpecliente, DbType.Int32, Cliente);
            dbProvider.AddInParameter(command, helper.Barrcodi, DbType.Int32, PEntrega);
            dbProvider.AddInParameter(command, helper.Barrcodi, DbType.Int32, PEntrega);
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
        /// Listar Punto de Entrega por parametros: Empresa Generadora, Periodo, Cliente, Punto de Entrega, Nivel de Tension
        /// </summary>
        public List<RntRegPuntoEntregaDTO> ListReporte(int? EmpresaGeneradora, int Periodo, int Cliente, int PEntrega)
        {
            List<RntRegPuntoEntregaDTO> entitys = new List<RntRegPuntoEntregaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListReporte);
            dbProvider.AddInParameter(command, helper.Rpeempresageneradora, DbType.Int32, EmpresaGeneradora);
            dbProvider.AddInParameter(command, helper.Rpeempresageneradora, DbType.Int32, EmpresaGeneradora);
            dbProvider.AddInParameter(command, helper.Periodocodi, DbType.Int32, Periodo);
            dbProvider.AddInParameter(command, helper.Periodocodi, DbType.Int32, Periodo);
            dbProvider.AddInParameter(command, helper.Rpecliente, DbType.Int32, Cliente);
            dbProvider.AddInParameter(command, helper.Rpecliente, DbType.Int32, Cliente);
            dbProvider.AddInParameter(command, helper.Barrcodi, DbType.Int32, PEntrega);
            dbProvider.AddInParameter(command, helper.Barrcodi, DbType.Int32, PEntrega);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<RntRegPuntoEntregaDTO> ListReporteCarga(int? EmpresaGeneradora, int Periodo, int PuntoEntrega, int CodigoEnvio)
        {
            List<RntRegPuntoEntregaDTO> entitys = new List<RntRegPuntoEntregaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListReporteCarga);
            dbProvider.AddInParameter(command, helper.Rpeempresageneradora, DbType.Int32, EmpresaGeneradora);
            dbProvider.AddInParameter(command, helper.Rpeempresageneradora, DbType.Int32, EmpresaGeneradora);
            dbProvider.AddInParameter(command, helper.Periodocodi, DbType.Int32, Periodo);
            dbProvider.AddInParameter(command, helper.Periodocodi, DbType.Int32, Periodo);
            dbProvider.AddInParameter(command, helper.Areacodi, DbType.Int32, PuntoEntrega);
            dbProvider.AddInParameter(command, helper.Areacodi, DbType.Int32, PuntoEntrega);
            dbProvider.AddInParameter(command, helper.EnvioCodi, DbType.Int32, CodigoEnvio);
            dbProvider.AddInParameter(command, helper.EnvioCodi, DbType.Int32, CodigoEnvio);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateCarga(dr));
                }
            }

            return entitys;
        }


        public List<RntRegPuntoEntregaDTO> ListReporteGrilla(int codEnvio)
        {
            List<RntRegPuntoEntregaDTO> entitys = new List<RntRegPuntoEntregaDTO>();
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
        /// Listar Punto de Entrega por parametros: Empresa Generadora, Periodo, Cliente, Punto de Entrega, Nivel de Tension
        /// </summary>
        public List<RntRegPuntoEntregaDTO> ListReportePaginado(int? EmpresaGeneradora, int Periodo, int Cliente, int PEntrega, int NroPaginado, int PageSize)
        {
            List<RntRegPuntoEntregaDTO> entitys = new List<RntRegPuntoEntregaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListReportePaginado);
            dbProvider.AddInParameter(command, helper.Rpeempresageneradora, DbType.Int32, EmpresaGeneradora);
            dbProvider.AddInParameter(command, helper.Rpeempresageneradora, DbType.Int32, EmpresaGeneradora);
            dbProvider.AddInParameter(command, helper.Periodocodi, DbType.Int32, Periodo);
            dbProvider.AddInParameter(command, helper.Periodocodi, DbType.Int32, Periodo);
            dbProvider.AddInParameter(command, helper.Rpecliente, DbType.Int32, Cliente);
            dbProvider.AddInParameter(command, helper.Rpecliente, DbType.Int32, Cliente);
            dbProvider.AddInParameter(command, helper.Barrcodi, DbType.Int32, PEntrega);
            dbProvider.AddInParameter(command, helper.Barrcodi, DbType.Int32, PEntrega);
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
        /// Listar Punto de Entrega por parametros: Empresa Generadora, Periodo, Cliente, Punto de Entrega, Nivel de Tension, Fecha
        /// Usado para auditoria
        /// </summary>

        public List<RntRegPuntoEntregaDTO> GetByCriteria(string user, int? EmpresaGeneradora, int Periodo, int Cliente, int PEntrega, int Ntension, DateTime desde)
        {
            List<RntRegPuntoEntregaDTO> entitys = new List<RntRegPuntoEntregaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, helper.Rpeusuariocreacion, DbType.String, user);
            dbProvider.AddInParameter(command, helper.Rpeusuariocreacion, DbType.String, user);
            dbProvider.AddInParameter(command, helper.Rpeempresageneradora, DbType.Int32, EmpresaGeneradora);
            dbProvider.AddInParameter(command, helper.Rpeempresageneradora, DbType.Int32, EmpresaGeneradora);
            dbProvider.AddInParameter(command, helper.Periodocodi, DbType.Int32, Periodo);
            dbProvider.AddInParameter(command, helper.Periodocodi, DbType.Int32, Periodo);
            dbProvider.AddInParameter(command, helper.Rpecliente, DbType.Int32, Cliente);
            dbProvider.AddInParameter(command, helper.Rpecliente, DbType.Int32, Cliente);
            dbProvider.AddInParameter(command, helper.Areacodi, DbType.Int32, PEntrega);
            dbProvider.AddInParameter(command, helper.Areacodi, DbType.Int32, PEntrega);
            dbProvider.AddInParameter(command, helper.Rpeniveltension, DbType.Int32, Ntension);
            dbProvider.AddInParameter(command, helper.Rpeniveltension, DbType.Int32, Ntension);
            dbProvider.AddInParameter(command, helper.Rpefechainicio, DbType.DateTime, desde);
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
        /// Listar Punto de Entrega por parametros: Empresa Generadora, Periodo, Cliente, Punto de Entrega, Nivel de Tension, Fecha
        /// Usado para auditoria
        /// </summary>
        public List<RntRegPuntoEntregaDTO> GetByCriteriaPaginado(string user, int? EmpresaGeneradora, int Periodo, int Cliente, int PEntrega, int Ntension, DateTime desde, int NroPaginado, int PageSize)
        {
            List<RntRegPuntoEntregaDTO> entitys = new List<RntRegPuntoEntregaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteriaPaginado);
            dbProvider.AddInParameter(command, helper.Rpeusuariocreacion, DbType.String, user);
            dbProvider.AddInParameter(command, helper.Rpeusuariocreacion, DbType.String, user);
            dbProvider.AddInParameter(command, helper.Rpeempresageneradora, DbType.Int32, EmpresaGeneradora);
            dbProvider.AddInParameter(command, helper.Rpeempresageneradora, DbType.Int32, EmpresaGeneradora);
            dbProvider.AddInParameter(command, helper.Periodocodi, DbType.Int32, Periodo);
            dbProvider.AddInParameter(command, helper.Periodocodi, DbType.Int32, Periodo);
            dbProvider.AddInParameter(command, helper.Rpecliente, DbType.Int32, Cliente);
            dbProvider.AddInParameter(command, helper.Rpecliente, DbType.Int32, Cliente);
            dbProvider.AddInParameter(command, helper.Areacodi, DbType.Int32, PEntrega);
            dbProvider.AddInParameter(command, helper.Areacodi, DbType.Int32, PEntrega);
            dbProvider.AddInParameter(command, helper.Rpeniveltension, DbType.Int32, Ntension);
            dbProvider.AddInParameter(command, helper.Rpeniveltension, DbType.Int32, Ntension);
            dbProvider.AddInParameter(command, helper.Rpefechainicio, DbType.DateTime, desde);
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

        public List<RntRegPuntoEntregaDTO> ListAuditoriaPuntoEntrega(int Audittablacodi, int Tauditcodi)
        {
            List<RntRegPuntoEntregaDTO> entitys = new List<RntRegPuntoEntregaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListAuditoriaPuntoEntrega);
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

        public RntRegPuntoEntregaDTO ListBarras(string barrnombre)
        {
            RntRegPuntoEntregaDTO entity = new RntRegPuntoEntregaDTO();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListBarras);
            dbProvider.AddInParameter(command, helper.BarrNombre, DbType.String, barrnombre);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.CreateBarr(dr);
                }
            }

            return entity;
        }

        public List<RntRegPuntoEntregaDTO> ListAllBarras()
        {
            List<RntRegPuntoEntregaDTO> entitys = new List<RntRegPuntoEntregaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListAllBarras);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateBarr(dr));
                }
            }

            return entitys;
        }

        /// <summary>
        /// Obtener Codigo Generado
        /// </summary>

        public int GetMaxId()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            int id = 1;

            object result = dbProvider.ExecuteScalar(command);
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public List<RntRegPuntoEntregaDTO> ListAllPuntoEntrega()
        {
            List<RntRegPuntoEntregaDTO> entitys = new List<RntRegPuntoEntregaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListAllPuntoEntrega);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateBarr(dr));
                }
            }

            return entitys;
        }

        public List<RntRegPuntoEntregaDTO> ListAllClientePE()
        {
            List<RntRegPuntoEntregaDTO> entitys = new List<RntRegPuntoEntregaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListAllClientePE);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateClientPE(dr));
                }
            }

            return entitys;
        }

        public List<RntRegPuntoEntregaDTO> ListChangeClientePE(int idCliente)
        {
            List<RntRegPuntoEntregaDTO> entitys = new List<RntRegPuntoEntregaDTO>();
            string query = String.Format(helper.SqlListChangeClientePE, idCliente);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateBarr(dr));
                }
            }

            return entitys;
        }
    }
}
