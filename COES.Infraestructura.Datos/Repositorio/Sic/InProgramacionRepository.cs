using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Sic;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla IN_PROGRAMACION
    /// </summary>
    public class InProgramacionRepository : RepositoryBase, IInProgramacionRepository
    {
        public InProgramacionRepository(string strConn) : base(strConn)
        {
        }

        InProgramacionHelper helper = new InProgramacionHelper();

        //-----------------------------------------------------------------------------------------------------------------------
        // ASSETEC.SGH - 01/10/2017: FUNCIONES PERSONALIZADAS PARA TIPOS DE PROGRAMACIONES
        //-----------------------------------------------------------------------------------------------------------------------       
        public int Save(InProgramacionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Progrcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Evenclasecodi, DbType.Int32, entity.Evenclasecodi);
            dbProvider.AddInParameter(command, helper.Progrnomb, DbType.String, entity.Progrnomb);
            dbProvider.AddInParameter(command, helper.Prograbrev, DbType.String, entity.Prograbrev);
            dbProvider.AddInParameter(command, helper.Progrfechaini, DbType.DateTime, entity.Progrfechaini);
            dbProvider.AddInParameter(command, helper.Progrfechafin, DbType.DateTime, entity.Progrfechafin);
            dbProvider.AddInParameter(command, helper.Progrversion, DbType.Int32, entity.Progrversion);
            dbProvider.AddInParameter(command, helper.Progrsololectura, DbType.Int32, entity.Progrsololectura);
            dbProvider.AddInParameter(command, helper.Progrfechalim, DbType.DateTime, entity.Progrfechalim);
            dbProvider.AddInParameter(command, helper.Progrusucreacion, DbType.String, entity.Progrusucreacion);
            dbProvider.AddInParameter(command, helper.Progrfeccreacion, DbType.DateTime, entity.Progrfeccreacion);
            dbProvider.AddInParameter(command, helper.Progrusuaprob, DbType.String, entity.Progrusuaprob);
            dbProvider.AddInParameter(command, helper.Progrfecaprob, DbType.DateTime, entity.Progrfecaprob);
            dbProvider.AddInParameter(command, helper.Progresaprobadorev, DbType.String, entity.Progresaprobadorev);
            dbProvider.AddInParameter(command, helper.Progrmaxfecreversion, DbType.DateTime, entity.Progrmaxfecreversion);
            dbProvider.AddInParameter(command, helper.Progrusuhabrev, DbType.String, entity.Progrusuhabrev);
            dbProvider.AddInParameter(command, helper.Progrfechabrev, DbType.DateTime, entity.Progrfechabrev);
            dbProvider.AddInParameter(command, helper.Progrusuaprobrev, DbType.String, entity.Progrusuaprobrev);
            dbProvider.AddInParameter(command, helper.Progrfecaprobrev, DbType.DateTime, entity.Progrfecaprobrev);

            dbProvider.ExecuteNonQuery(command);

            return id;
        }

        /// <summary>
        /// Actualizar campos de la programacion
        /// </summary>
        /// <param name="entity"></param>
        public void Update(InProgramacionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Progrcodi, DbType.Int32, entity.Progrcodi);
            dbProvider.AddInParameter(command, helper.Evenclasecodi, DbType.Int32, entity.Evenclasecodi);
            dbProvider.AddInParameter(command, helper.Progrnomb, DbType.String, entity.Progrnomb);
            dbProvider.AddInParameter(command, helper.Prograbrev, DbType.String, entity.Prograbrev);
            dbProvider.AddInParameter(command, helper.Progrfechaini, DbType.DateTime, entity.Progrfechaini);
            dbProvider.AddInParameter(command, helper.Progrfechafin, DbType.DateTime, entity.Progrfechafin);
            dbProvider.AddInParameter(command, helper.Progrversion, DbType.Int32, entity.Progrversion);
            dbProvider.AddInParameter(command, helper.Progrsololectura, DbType.Int32, entity.Progrsololectura);
            dbProvider.AddInParameter(command, helper.Progrfechalim, DbType.DateTime, entity.Progrfechalim);
            dbProvider.AddInParameter(command, helper.Progrusucreacion, DbType.String, entity.Progrusucreacion);
            dbProvider.AddInParameter(command, helper.Progrfeccreacion, DbType.DateTime, entity.Progrfeccreacion);
            dbProvider.AddInParameter(command, helper.Progrusuaprob, DbType.String, entity.Progrusuaprob);
            dbProvider.AddInParameter(command, helper.Progrfecaprob, DbType.DateTime, entity.Progrfecaprob);
            dbProvider.AddInParameter(command, helper.Progresaprobadorev, DbType.String, entity.Progresaprobadorev);
            dbProvider.AddInParameter(command, helper.Progrmaxfecreversion, DbType.DateTime, entity.Progrmaxfecreversion);
            dbProvider.AddInParameter(command, helper.Progrusuhabrev, DbType.String, entity.Progrusuhabrev);
            dbProvider.AddInParameter(command, helper.Progrfechabrev, DbType.DateTime, entity.Progrfechabrev);
            dbProvider.AddInParameter(command, helper.Progrusuaprobrev, DbType.String, entity.Progrusuaprobrev);
            dbProvider.AddInParameter(command, helper.Progrfecaprobrev, DbType.DateTime, entity.Progrfecaprobrev);

            dbProvider.ExecuteNonQuery(command);
        }

        /// Función que actualiza a solo lectura
        /// </summary>
        /// <param name="progCodigo">id de la programacion</param>        
        /// <param name="conn">Objeto de tipo IDbConnection</param>
        /// <param name="tran">Objeto de tipo DbTransaction</param>
        public void ActualizarSoloLectura(int progCodigo, int flagLectura, string usuario, DateTime fechaAprob, IDbConnection conn, DbTransaction tran)
        {
            DbCommand commandTbProgramacionSL = (DbCommand)conn.CreateCommand();
            commandTbProgramacionSL.CommandText = helper.SqlHacerSoloLecturaProgramacion;
            commandTbProgramacionSL.Transaction = tran;
            commandTbProgramacionSL.Connection = (DbConnection)conn;

            commandTbProgramacionSL.Parameters.Add(dbProvider.CreateParameter(commandTbProgramacionSL, helper.Progrsololectura, DbType.Int32, flagLectura));
            commandTbProgramacionSL.Parameters.Add(dbProvider.CreateParameter(commandTbProgramacionSL, helper.Progrusuaprob, DbType.String, usuario));
            commandTbProgramacionSL.Parameters.Add(dbProvider.CreateParameter(commandTbProgramacionSL, helper.Progrfecaprob, DbType.DateTime, fechaAprob));

            commandTbProgramacionSL.Parameters.Add(dbProvider.CreateParameter(commandTbProgramacionSL, helper.Progrcodi, DbType.Int32, progCodigo));

            commandTbProgramacionSL.ExecuteNonQuery();
        }


        /// <summary>
        /// Permite listar todos las programaciones
        /// (Utilizar para llenar la grilla de Programaciones)
        /// </summary>        
        /// <param name="IdTipoProgramacion">IdTipoProgramacion</param>
        /// <param name="IdProgramacion">IdProgramacion</param>
        /// <returns>lista de programaciones</returns>
        public List<InProgramacionDTO> ListarProgramaciones(int idTipoProgramacion, string idsProgramacion)
        {
            List<InProgramacionDTO> entitys = new List<InProgramacionDTO>();

            string cadenaParametros = string.Format(helper.SqlGetProgramacionesByIdTipoProgramacion, idTipoProgramacion, idsProgramacion);
            DbCommand command = dbProvider.GetSqlStringCommand(cadenaParametros);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.CreateProgramacion(dr);

                    int iEvenclasedesc = dr.GetOrdinal(helper.Evenclasedesc);
                    if (!dr.IsDBNull(iEvenclasedesc)) entity.Evenclasedesc = dr.GetString(iEvenclasedesc);

                    int iTotalRegistro = dr.GetOrdinal(helper.TotalRegistro);
                    if (!dr.IsDBNull(iTotalRegistro)) entity.TotalRegistro = Convert.ToInt32(dr.GetValue(iTotalRegistro));

                    int iTotalRevertidos = dr.GetOrdinal(helper.TotalRevertidos);
                    if (!dr.IsDBNull(iTotalRevertidos)) entity.TotalRevertidos = Convert.ToInt32(dr.GetValue(iTotalRevertidos));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<InProgramacionDTO> ListProgramacionById(string progrcodis)
        {
            List<InProgramacionDTO> entitys = new List<InProgramacionDTO>();

            string sql = string.Format(helper.SqlObtenerProgramacionesPorId, progrcodis);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.CreateProgramacion(dr);

                    int iEvenclasedesc = dr.GetOrdinal(helper.Evenclasedesc);
                    if (!dr.IsDBNull(iEvenclasedesc)) entity.Evenclasedesc = dr.GetString(iEvenclasedesc);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }


        public InProgramacionDTO ObtenerProgramacionesPorId(int idProgramacion)
        {
            InProgramacionDTO entity = null;

            string sql = string.Format(helper.SqlObtenerProgramacionesPorId, idProgramacion);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.CreateProgramacion(dr);

                    int iEvenclasedesc = dr.GetOrdinal(helper.Evenclasedesc);
                    if (!dr.IsDBNull(iEvenclasedesc)) entity.Evenclasedesc = dr.GetString(iEvenclasedesc);
                }
            }

            return entity;
        }

        /// <summary>
        /// Permite obtener el Id de la programación ABIERTA propuesta ya existente en la BD
        /// </summary>
        /// <param name="dFecInicio">dFecInicio</param>
        /// <param name="IdTipoProgramacion">IdTipoProgramacion</param>
        /// <returns>valor entero</returns>
        public InProgramacionDTO ObtenerProgramacionesPorFechaYTipo(DateTime fecInicio, int idTipoProgramacion)
        {
            InProgramacionDTO entity = null;

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerIdProgramacionXFecIniYTipoPro);

            dbProvider.AddInParameter(command, helper.Progrfechaini, DbType.String, fecInicio.ToString(ConstantesBase.FormatoFecha));
            dbProvider.AddInParameter(command, helper.Evenclasecodi, DbType.Int32, idTipoProgramacion);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.CreateProgramacion(dr);

                    int iEvenclasedesc = dr.GetOrdinal(helper.Evenclasedesc);
                    if (!dr.IsDBNull(iEvenclasedesc)) entity.Evenclasedesc = dr.GetString(iEvenclasedesc);

                }
            }

            return entity;
        }

        public void ActualizarAprobadoReversion(int progCodigo, int flagAprobadoRevertido, string usuario, DateTime fechaAprob, IDbConnection conn, DbTransaction tran)
        {
            DbCommand commandTbProgramacionSL = (DbCommand)conn.CreateCommand();
            commandTbProgramacionSL.CommandText = helper.SqlActualizarAprobadoReversion;
            commandTbProgramacionSL.Transaction = tran;
            commandTbProgramacionSL.Connection = (DbConnection)conn;

            commandTbProgramacionSL.Parameters.Add(dbProvider.CreateParameter(commandTbProgramacionSL, helper.Progresaprobadorev, DbType.Int32, flagAprobadoRevertido));
            commandTbProgramacionSL.Parameters.Add(dbProvider.CreateParameter(commandTbProgramacionSL, helper.Progrusuaprobrev, DbType.String, usuario));
            commandTbProgramacionSL.Parameters.Add(dbProvider.CreateParameter(commandTbProgramacionSL, helper.Progrfecaprobrev, DbType.DateTime, fechaAprob));

            commandTbProgramacionSL.Parameters.Add(dbProvider.CreateParameter(commandTbProgramacionSL, helper.Progrcodi, DbType.Int32, progCodigo));

            commandTbProgramacionSL.ExecuteNonQuery();
        }

        public void HabilitarReversion(int progCodigo, DateTime fechaMaxEnReversion, string usuario, DateTime fechaHabilitaReversion, IDbConnection conn, DbTransaction tran)
        {
            DbCommand commandTbProgramacionSL = (DbCommand)conn.CreateCommand();
            commandTbProgramacionSL.CommandText = helper.SqlHabilitarReversion;
            commandTbProgramacionSL.Transaction = tran;
            commandTbProgramacionSL.Connection = (DbConnection)conn;

            commandTbProgramacionSL.Parameters.Add(dbProvider.CreateParameter(commandTbProgramacionSL, helper.Progrusuhabrev, DbType.String, usuario));
            commandTbProgramacionSL.Parameters.Add(dbProvider.CreateParameter(commandTbProgramacionSL, helper.Progrfechabrev, DbType.DateTime, fechaHabilitaReversion));
            commandTbProgramacionSL.Parameters.Add(dbProvider.CreateParameter(commandTbProgramacionSL, helper.Progrmaxfecreversion, DbType.DateTime, fechaMaxEnReversion));

            commandTbProgramacionSL.Parameters.Add(dbProvider.CreateParameter(commandTbProgramacionSL, helper.Progrcodi, DbType.Int32, progCodigo));

            commandTbProgramacionSL.ExecuteNonQuery();
        }
    }
}
