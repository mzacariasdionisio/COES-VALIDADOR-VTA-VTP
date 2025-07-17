using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Infraestructura.Datos.Helper.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla AF_INTERRUP_SUMINISTRO
    /// </summary>
    public class AfInterrupSuministroRepository : RepositoryBase, IAfInterrupSuministroRepository
    {
        public AfInterrupSuministroRepository(string strConn) : base(strConn)
        {
        }

        AfInterrupSuministroHelper helper = new AfInterrupSuministroHelper();

        public int Save(AfInterrupSuministroDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Intsumcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Enviocodi, DbType.Int32, entity.Enviocodi);
            dbProvider.AddInParameter(command, helper.Intsummwred, DbType.Decimal, entity.Intsummwred);
            dbProvider.AddInParameter(command, helper.Intsummwfin, DbType.Decimal, entity.Intsummwfin);
            dbProvider.AddInParameter(command, helper.Intsumsuministro, DbType.String, entity.Intsumsuministro);
            dbProvider.AddInParameter(command, helper.Intsumobs, DbType.String, entity.Intsumobs);
            dbProvider.AddInParameter(command, helper.Intsumnumetapa, DbType.Int32, entity.Intsumnumetapa);
            dbProvider.AddInParameter(command, helper.Intsumduracion, DbType.Decimal, entity.Intsumduracion);
            dbProvider.AddInParameter(command, helper.Intsumfuncion, DbType.String, entity.Intsumfuncion);
            dbProvider.AddInParameter(command, helper.Intsumfechainterrfin, DbType.DateTime, entity.Intsumfechainterrfin);
            dbProvider.AddInParameter(command, helper.Intsumfechainterrini, DbType.DateTime, entity.Intsumfechainterrini);
            dbProvider.AddInParameter(command, helper.Intsummw, DbType.Decimal, entity.Intsummw);
            dbProvider.AddInParameter(command, helper.Intsumsubestacion, DbType.String, entity.Intsumsubestacion);
            dbProvider.AddInParameter(command, helper.Intsumempresa, DbType.String, entity.Intsumempresa);
            dbProvider.AddInParameter(command, helper.Intsumzona, DbType.String, entity.Intsumzona);
            dbProvider.AddInParameter(command, helper.Afecodi, DbType.Int32, entity.Afecodi);
            dbProvider.AddInParameter(command, helper.Intsumfeccreacion, DbType.DateTime, entity.Intsumfeccreacion);
            dbProvider.AddInParameter(command, helper.Intsumusucreacion, DbType.String, entity.Intsumusucreacion);
            dbProvider.AddInParameter(command, helper.Intsumestado, DbType.Int32, entity.Intsumestado);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(AfInterrupSuministroDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Enviocodi, DbType.Int32, entity.Enviocodi);
            dbProvider.AddInParameter(command, helper.Intsummwred, DbType.Decimal, entity.Intsummwred);
            dbProvider.AddInParameter(command, helper.Intsummwfin, DbType.Decimal, entity.Intsummwfin);
            dbProvider.AddInParameter(command, helper.Intsumsuministro, DbType.String, entity.Intsumsuministro);
            dbProvider.AddInParameter(command, helper.Intsumobs, DbType.String, entity.Intsumobs);
            dbProvider.AddInParameter(command, helper.Intsumnumetapa, DbType.Int32, entity.Intsumnumetapa);
            dbProvider.AddInParameter(command, helper.Intsumduracion, DbType.Decimal, entity.Intsumduracion);
            dbProvider.AddInParameter(command, helper.Intsumfuncion, DbType.String, entity.Intsumfuncion);
            dbProvider.AddInParameter(command, helper.Intsumfechainterrfin, DbType.DateTime, entity.Intsumfechainterrfin);
            dbProvider.AddInParameter(command, helper.Intsumfechainterrini, DbType.DateTime, entity.Intsumfechainterrini);
            dbProvider.AddInParameter(command, helper.Intsummw, DbType.Decimal, entity.Intsummw);
            dbProvider.AddInParameter(command, helper.Intsumsubestacion, DbType.String, entity.Intsumsubestacion);
            dbProvider.AddInParameter(command, helper.Intsumempresa, DbType.String, entity.Intsumempresa);
            dbProvider.AddInParameter(command, helper.Intsumzona, DbType.String, entity.Intsumzona);
            dbProvider.AddInParameter(command, helper.Intsumcodi, DbType.Int32, entity.Intsumcodi);
            dbProvider.AddInParameter(command, helper.Afecodi, DbType.Int32, entity.Afecodi);
            dbProvider.AddInParameter(command, helper.Intsumfeccreacion, DbType.DateTime, entity.Intsumfeccreacion);
            dbProvider.AddInParameter(command, helper.Intsumusucreacion, DbType.String, entity.Intsumusucreacion);
            dbProvider.AddInParameter(command, helper.Intsumestado, DbType.Int32, entity.Intsumestado);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int intsumcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Intsumcodi, DbType.Int32, intsumcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public AfInterrupSuministroDTO GetById(int intsumcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Intsumcodi, DbType.Int32, intsumcodi);
            AfInterrupSuministroDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<AfInterrupSuministroDTO> List()
        {
            List<AfInterrupSuministroDTO> entitys = new List<AfInterrupSuministroDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<AfInterrupSuministroDTO> GetByCriteria(int afecodi, int emprcodi, int fdatcodi, int enviocodi)
        {
            List<AfInterrupSuministroDTO> entitys = new List<AfInterrupSuministroDTO>();
            string query = string.Format(helper.SqlGetByCriteria, afecodi, emprcodi, fdatcodi, enviocodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public int Save(AfInterrupSuministroDTO entity, IDbConnection connection, IDbTransaction transaction)
        {
            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = (DbTransaction)transaction;
                dbCommand.Connection = (DbConnection)connection;
                dbCommand.CommandText = helper.SqlGetMaxId;

                object result = dbCommand.ExecuteScalar();
                int id = result == null ? 1 : Convert.ToInt32(result);

                dbCommand.CommandText = helper.SqlSave;

                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Enviocodi, DbType.Int32, entity.Enviocodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Intsummwred, DbType.Decimal, entity.Intsummwred));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Intsummwfin, DbType.Decimal, entity.Intsummwfin));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Intsumsuministro, DbType.String, entity.Intsumsuministro));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Intsumobs, DbType.String, entity.Intsumobs));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Intsumnumetapa, DbType.Int32, entity.Intsumnumetapa));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Intsumduracion, DbType.Decimal, entity.Intsumduracion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Intsumfuncion, DbType.String, entity.Intsumfuncion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Intsumfechainterrfin, DbType.DateTime, entity.Intsumfechainterrfin));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Intsumfechainterrini, DbType.DateTime, entity.Intsumfechainterrini));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Intsummw, DbType.Decimal, entity.Intsummw));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Intsumsubestacion, DbType.String, entity.Intsumsubestacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Intsumempresa, DbType.String, entity.Intsumempresa));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Intsumzona, DbType.String, entity.Intsumzona));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Intsumcodi, DbType.Int32, id));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Afecodi, DbType.Int32, entity.Afecodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Intsumfeccreacion, DbType.DateTime, entity.Intsumfeccreacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Intsumusucreacion, DbType.String, entity.Intsumusucreacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Intsumestado, DbType.Int32, entity.Intsumestado));

                dbCommand.ExecuteNonQuery();
                return id;

            }
        }

        public void UpdateAEstadoBajaXEmpresa(int afecodi, int emprcodi, int fdatcodi, IDbConnection connection, IDbTransaction transaction)
        {
            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = (DbTransaction)transaction;
                dbCommand.Connection = (DbConnection)connection;

                var query = string.Format(helper.SqlUpdateAEstadoBaja, afecodi, emprcodi, fdatcodi);
                dbCommand.CommandText = query;

                dbCommand.ExecuteNonQuery();
            }
        }

        public List<AfInterrupSuministroDTO> ObtenerUltimoEnvio(int afecodi, int emprcodi, int fdatcodi)
        {
            List<AfInterrupSuministroDTO> entitys = new List<AfInterrupSuministroDTO>();

            var query = string.Format(helper.SqlObtenerUltimoEnvio, afecodi, emprcodi, fdatcodi);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<AfInterrupSuministroDTO> ListarReporteExtranetCTAF(int afecodi, int emprcodi, int fdatcodi)
        {
            List<AfInterrupSuministroDTO> entitys = new List<AfInterrupSuministroDTO>();

            var query = string.Format(helper.SqlListarReporteExtranetCTAF, afecodi, emprcodi, fdatcodi);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iFdatcodi = dr.GetOrdinal(helper.Fdatcodi);
                    if (!dr.IsDBNull(iFdatcodi)) entity.Fdatcodi = Convert.ToInt32(dr.GetValue(iFdatcodi));

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);


                    int iEvencodi = dr.GetOrdinal(helper.Evencodi);
                    if (!dr.IsDBNull(iEvencodi)) entity.EVENCODI = dr.GetInt32(iEvencodi);

                    int iEvenini = dr.GetOrdinal(helper.Evenini);
                    if (!dr.IsDBNull(iEvenini)) entity.EVENINI = dr.GetDateTime(iEvenini);

                    int iEracmfsuministrador = dr.GetOrdinal(helper.Eracmfsuministrador);
                    if (!dr.IsDBNull(iEracmfsuministrador)) entity.EmpresaSuministradora = dr.GetString(iEracmfsuministrador);

                    int iAfemprnomb = dr.GetOrdinal(helper.Afemprnomb);
                    if (!dr.IsDBNull(iAfemprnomb)) entity.Afemprnomb = dr.GetString(iAfemprnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }


        public List<AfInterrupSuministroDTO> ListarReporteInterrupcionesCTAF( string anio, string correlativo, int emprcodi, int fdatcodi)
        {
            List<AfInterrupSuministroDTO> entitys = new List<AfInterrupSuministroDTO>();

            var query = string.Format(helper.SqlListarReporteInterrupcionesCTAF, anio, correlativo, emprcodi, fdatcodi);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);


                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iFdatcodi = dr.GetOrdinal(helper.Fdatcodi);
                    if (!dr.IsDBNull(iFdatcodi)) entity.Fdatcodi = Convert.ToInt32(dr.GetValue(iFdatcodi));

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEvencodi= dr.GetOrdinal(helper.Evencodi);
                    if (!dr.IsDBNull(iEvencodi)) entity.EVENCODI = dr.GetInt32(iEvencodi);

                    int iEvenini = dr.GetOrdinal(helper.Evenini);
                    if (!dr.IsDBNull(iEvenini)) entity.EVENINI = dr.GetDateTime(iEvenini);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
