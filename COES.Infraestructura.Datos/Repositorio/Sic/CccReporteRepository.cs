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
    /// Clase de acceso a datos de la tabla CCC_REPORTE
    /// </summary>
    public class CccReporteRepository : RepositoryBase, ICccReporteRepository
    {
        public CccReporteRepository(string strConn) : base(strConn)
        {
        }

        CccReporteHelper helper = new CccReporteHelper();

        public int GetMaxId()
        {

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            int id = 1;

            object result = dbProvider.ExecuteScalar(command);
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public int Save(CccReporteDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cccrptcodi, DbType.Int32, id));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cccvercodi, DbType.Int32, entity.Cccvercodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Equipadre, DbType.Int32, entity.Equipadre));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Mogrupocodi, DbType.Int32, entity.Mogrupocodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Tipoinfocodi, DbType.Int32, entity.Tipoinfocodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cccrptvalorreal, DbType.Decimal, entity.Cccrptvalorreal));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cccrptvalorteorico, DbType.Decimal, entity.Cccrptvalorteorico));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cccrptvariacion, DbType.Decimal, entity.Cccrptvariacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Cccrptflagtienecurva, DbType.Int32, entity.Cccrptflagtienecurva));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Fenergcodi, DbType.Int32, entity.Fenergcodi));

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CccReporteDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Cccrptcodi, DbType.Int32, entity.Cccrptcodi);
            dbProvider.AddInParameter(command, helper.Cccvercodi, DbType.Int32, entity.Cccvercodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Equipadre, DbType.Int32, entity.Equipadre);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Cccrptvalorreal, DbType.Decimal, entity.Cccrptvalorreal);
            dbProvider.AddInParameter(command, helper.Cccrptvalorteorico, DbType.Decimal, entity.Cccrptvalorteorico);
            dbProvider.AddInParameter(command, helper.Cccrptvariacion, DbType.Decimal, entity.Cccrptvariacion);
            dbProvider.AddInParameter(command, helper.Fenergcodi, DbType.Int32, entity.Fenergcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int cccrptcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Cccrptcodi, DbType.Int32, cccrptcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CccReporteDTO GetById(int cccrptcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Cccrptcodi, DbType.Int32, cccrptcodi);
            CccReporteDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CccReporteDTO> List()
        {
            List<CccReporteDTO> entitys = new List<CccReporteDTO>();
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

        public List<CccReporteDTO> GetByCriteria(string cccvercodi)
        {
            List<CccReporteDTO> entitys = new List<CccReporteDTO>();

            string sql = string.Format(helper.SqlGetByCriteria, cccvercodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iCccverfecha = dr.GetOrdinal(helper.Cccverfecha);
                    if (!dr.IsDBNull(iCccverfecha)) entity.Cccverfecha = dr.GetDateTime(iCccverfecha);

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

                    int iTipoinfoabrev = dr.GetOrdinal(this.helper.Tipoinfoabrev);
                    if (!dr.IsDBNull(iTipoinfoabrev)) entity.Tipoinfoabrev = dr.GetString(iTipoinfoabrev);

                    int iMogruponomb = dr.GetOrdinal(this.helper.Mogruponomb);
                    if (!dr.IsDBNull(iMogruponomb)) entity.Mogruponomb = dr.GetString(iMogruponomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

    }
}
