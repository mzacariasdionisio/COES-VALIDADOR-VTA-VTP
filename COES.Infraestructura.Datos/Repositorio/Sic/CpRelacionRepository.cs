using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using COES.Dominio.Interfaces.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Infraestructura.Datos.Helper.Sic;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla CP_GRUPORECURSO
    /// </summary>
    public class CpRelacionRepository : RepositoryBase, ICpRelacionRepository
    {
        public CpRelacionRepository(string strConn)
            : base(strConn)
        {
        }

        CpRelacionHelper helper = new CpRelacionHelper();

        public void Save(CpRelacionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Recurcodi1, DbType.Int32, entity.Recurcodi1);
            dbProvider.AddInParameter(command, helper.Recurcodi2, DbType.Int32, entity.Recurcodi2);
            dbProvider.AddInParameter(command, helper.Cptrelcodi, DbType.Int32, entity.Cptrelcodi);
            dbProvider.AddInParameter(command, helper.Topcodi, DbType.Int32, entity.Topcodi);
            dbProvider.AddInParameter(command, helper.Cprelval1, DbType.Decimal, entity.Cprelval1);
            dbProvider.AddInParameter(command, helper.Cprelusucreacion, DbType.String, entity.Cprelusucreacion);
            dbProvider.AddInParameter(command, helper.Cprelfeccreacion, DbType.DateTime, entity.Cprelfeccreacion);
            dbProvider.AddInParameter(command, helper.Cprelusumodificacion, DbType.String, entity.Cprelusumodificacion);
            dbProvider.AddInParameter(command, helper.Cprelfecmodificacion, DbType.DateTime, entity.Cprelfecmodificacion);
            dbProvider.AddInParameter(command, helper.Cprelval2, DbType.Decimal, entity.Cprelval2);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(CpRelacionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Cprelval1, DbType.Decimal, entity.Cprelval1);
            dbProvider.AddInParameter(command, helper.Cprelusucreacion, DbType.String, entity.Cprelusucreacion);
            dbProvider.AddInParameter(command, helper.Cprelfeccreacion, DbType.DateTime, entity.Cprelfeccreacion);
            dbProvider.AddInParameter(command, helper.Cprelusumodificacion, DbType.String, entity.Cprelusumodificacion);
            dbProvider.AddInParameter(command, helper.Cprelfecmodificacion, DbType.DateTime, entity.Cprelfecmodificacion);
            dbProvider.AddInParameter(command, helper.Cprelval2, DbType.Decimal, entity.Cprelval2);
            dbProvider.AddInParameter(command, helper.Recurcodi1, DbType.Int32, entity.Recurcodi1);
            dbProvider.AddInParameter(command, helper.Recurcodi2, DbType.Int32, entity.Recurcodi2);
            dbProvider.AddInParameter(command, helper.Cptrelcodi, DbType.Int32, entity.Cptrelcodi);
            dbProvider.AddInParameter(command, helper.Topcodi, DbType.Int32, entity.Topcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int recurcodi1, int recurcodi2, int topcodi,int cptrelcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Recurcodi1, DbType.Int32, recurcodi1);
            dbProvider.AddInParameter(command, helper.Recurcodi2, DbType.Int32, recurcodi2);
            dbProvider.AddInParameter(command, helper.Cptrelcodi, DbType.Int32, cptrelcodi);
            dbProvider.AddInParameter(command, helper.Topcodi, DbType.Int32, topcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteAll(int recurcodi, int topcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteAll);

            dbProvider.AddInParameter(command, helper.Recurcodi1, DbType.Int32, recurcodi);
            dbProvider.AddInParameter(command, helper.Topcodi, DbType.Int32, topcodi);

            dbProvider.ExecuteNonQuery(command);            
        }

        public void DeleteAllTipo(int recurcodi, int cptrelcodi, int topcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteAllTipo);

            dbProvider.AddInParameter(command, helper.Recurcodi1, DbType.Int32, recurcodi);
            dbProvider.AddInParameter(command, helper.Cptrelcodi, DbType.Int32, cptrelcodi);
            dbProvider.AddInParameter(command, helper.Topcodi, DbType.Int32, topcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteEscenario(int topcodi)
        {
            string sqldelete = string.Format(helper.SqlDeleteEscenario, topcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sqldelete);
            
            dbProvider.ExecuteNonQuery(command);            
        }

        public CpRelacionDTO GetById(int recurcodi1, int recurcodi2, int topcodi, int cptrelcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Recurcodi1, DbType.Int32, recurcodi1);
            dbProvider.AddInParameter(command, helper.Recurcodi2, DbType.Int32, recurcodi2);
            dbProvider.AddInParameter(command, helper.Cptrelcodi, DbType.Int32, cptrelcodi);
            dbProvider.AddInParameter(command, helper.Topcodi, DbType.Int32, topcodi);
            CpRelacionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CpRelacionDTO> List(int topcodi, string scptrelcodi)
        {
            string sqlQuery = string.Format(helper.SqlList, topcodi, scptrelcodi);
            List<CpRelacionDTO> entitys = new List<CpRelacionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            
            CpRelacionDTO entity = new CpRelacionDTO();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);
                    int iCatcodi = dr.GetOrdinal(helper.Catcodi);
                    if (!dr.IsDBNull(iCatcodi)) entity.Catcodi2 = Convert.ToInt32(dr.GetValue(iCatcodi));
                    int iRecurconsideragams = dr.GetOrdinal(helper.Recurconsideragams);
                    if (!dr.IsDBNull(iRecurconsideragams)) entity.Recurconsideragams = Convert.ToInt32(dr.GetValue(iRecurconsideragams));
                    int iCatcodi1 = dr.GetOrdinal(helper.Catcodi1);
                    if (!dr.IsDBNull(iCatcodi1)) entity.Catcodi1 = Convert.ToInt32(dr.GetValue(iCatcodi1));
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<CpRelacionDTO> GetByCriteria(int recurcodi, string cptrelcodi,int topcodi)
        {
            List<CpRelacionDTO> entitys = new List<CpRelacionDTO>();
            string sqlQuery = string.Format(helper.SqlGetByCriteria, recurcodi, cptrelcodi, topcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            CpRelacionDTO entity;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);
                    int iRecurnombre = dr.GetOrdinal(helper.Recurnombre);
                    if (!dr.IsDBNull(iRecurnombre)) entity.Recurnombre = dr.GetString(iRecurnombre);
                    int iCatcodi = dr.GetOrdinal(helper.Catcodi);
                    if (!dr.IsDBNull(iCatcodi)) entity.Catcodi2 = Convert.ToInt32(dr.GetValue(iCatcodi));
                    entitys.Add(entity);

                }
            }

            return entitys;
        }

        public List<CpRelacionDTO> ObtenerDependencias(int pRecurso, short pTopologia)
        {
            List<CpRelacionDTO> entitys = new List<CpRelacionDTO>();
            string query = string.Format(helper.SqlObtenerDependencias, pRecurso, pTopologia);
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

  
        public void CrearCopia(int topcodi1, int topcodi2)
        {
            string query = string.Format(helper.SqlCrearCopia, topcodi1, topcodi2);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            dbProvider.ExecuteNonQuery(command);
        }

    }
}
