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
    /// Clase de acceso a datos de la tabla CCC_VCOM
    /// </summary>
    public class CccVcomRepository : RepositoryBase, ICccVcomRepository
    {
        public CccVcomRepository(string strConn) : base(strConn)
        {
        }

        CccVcomHelper helper = new CccVcomHelper();

        public int GetMaxId()
        {

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            int id = 1;

            object result = dbProvider.ExecuteScalar(command);
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public int Save(CccVcomDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Vcomcodi, DbType.Int32, id));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cccvercodi, DbType.Int32, entity.Cccvercodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Fenergcodi, DbType.Int32, entity.Fenergcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Equipadre, DbType.Int32, entity.Equipadre));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Vcomvalor, DbType.Decimal, entity.Vcomvalor));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Vcomcodigomop, DbType.String, entity.Vcomcodigomop));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Vcomcodigotcomb, DbType.String, entity.Vcomcodigotcomb));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Tinfcoes, DbType.Int32, entity.Tinfcoes));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Tinfosi, DbType.Int32, entity.Tinfosi));

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CccVcomDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Vcomcodi, DbType.Int32, entity.Vcomcodi);
            dbProvider.AddInParameter(command, helper.Cccvercodi, DbType.Int32, entity.Cccvercodi);
            dbProvider.AddInParameter(command, helper.Fenergcodi, DbType.Int32, entity.Fenergcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Equipadre, DbType.Int32, entity.Equipadre);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Vcomvalor, DbType.Decimal, entity.Vcomvalor);
            dbProvider.AddInParameter(command, helper.Vcomcodigomop, DbType.String, entity.Vcomcodigomop);
            dbProvider.AddInParameter(command, helper.Vcomcodigotcomb, DbType.String, entity.Vcomcodigotcomb);
            dbProvider.AddInParameter(command, helper.Tinfcoes, DbType.Int32, entity.Tinfcoes);
            dbProvider.AddInParameter(command, helper.Tinfosi, DbType.Int32, entity.Tinfosi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int vcomcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Vcomcodi, DbType.Int32, vcomcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CccVcomDTO GetById(int vcomcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Vcomcodi, DbType.Int32, vcomcodi);
            CccVcomDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CccVcomDTO> List()
        {
            List<CccVcomDTO> entitys = new List<CccVcomDTO>();
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

        public List<CccVcomDTO> GetByCriteria(int cccvercodi)
        {
            List<CccVcomDTO> entitys = new List<CccVcomDTO>();

            string sql = string.Format(helper.SqlGetByCriteria, cccvercodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iEmprabrev = dr.GetOrdinal(this.helper.Emprabrev);
                    if (!dr.IsDBNull(iEmprabrev)) entity.Emprabrev = dr.GetString(iEmprabrev);

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iCentral = dr.GetOrdinal(this.helper.Central);
                    if (!dr.IsDBNull(iCentral)) entity.Central = dr.GetString(iCentral);

                    int iEquinomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iFenergnomb = dr.GetOrdinal(this.helper.Fenergnomb);
                    if (!dr.IsDBNull(iFenergnomb)) entity.Fenergnomb = dr.GetString(iFenergnomb);

                    int iGruponomb = dr.GetOrdinal(this.helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    int iTinfosiabrev = dr.GetOrdinal(helper.Tinfosiabrev);
                    if (!dr.IsDBNull(iTinfosiabrev)) entity.Tinfosiabrev = dr.GetString(iTinfosiabrev);

                    int iTinfcoesabrev = dr.GetOrdinal(helper.Tinfcoesabrev);
                    if (!dr.IsDBNull(iTinfcoesabrev)) entity.Tinfcoesabrev = dr.GetString(iTinfcoesabrev);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
