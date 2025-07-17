using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Sic;
using System.Data;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    public class DpoTransformadorRepository : RepositoryBase, IDpoTransformadorRepository
    {
        public DpoTransformadorRepository(string strConn) : base(strConn)
        {
        }

        DpoTransformadorHelper helper = new DpoTransformadorHelper();

        public void Save(DpoTransformadorDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Dpotnfcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Dpotnfcodiexcel, DbType.String, entity.Dpotnfcodiexcel);
            dbProvider.AddInParameter(command, helper.Dposubnombre, DbType.String, entity.Dposubnombre);
            dbProvider.AddInParameter(command, helper.Emprnomb, DbType.String, entity.Emprnomb);
            dbProvider.AddInParameter(command, helper.Dpotnfusucreacion, DbType.String, entity.Dpotnfusucreacion);
            dbProvider.AddInParameter(command, helper.Dpotnffeccreacion, DbType.DateTime, entity.Dpotnffeccreacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(DpoTransformadorDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Dpotnfcodi, DbType.String, entity.Dpotnfcodi);
            dbProvider.AddInParameter(command, helper.Dposubnombre, DbType.String, entity.Dposubnombre);
            dbProvider.AddInParameter(command, helper.Emprnomb, DbType.String, entity.Emprnomb);
            dbProvider.AddInParameter(command, helper.Dpotnfusucreacion, DbType.String, entity.Dpotnfusucreacion);
            dbProvider.AddInParameter(command, helper.Dpotnffeccreacion, DbType.DateTime, entity.Dpotnffeccreacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(string codi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Dpotnfcodi, DbType.String, codi);

            dbProvider.ExecuteNonQuery(command);
        }

        public List<DpoTransformadorDTO> List()
        {
            List<DpoTransformadorDTO> entitys = new List<DpoTransformadorDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    DpoTransformadorDTO entity = new DpoTransformadorDTO();

                    int iDpotnfcodi = dr.GetOrdinal(helper.Dpotnfcodi);
                    if (!dr.IsDBNull(iDpotnfcodi)) entity.Dpotnfcodi = Convert.ToInt32(dr.GetValue(iDpotnfcodi));

                    int iDpotnfcodiexcel = dr.GetOrdinal(helper.Dpotnfcodiexcel);
                    if (!dr.IsDBNull(iDpotnfcodiexcel)) entity.Dpotnfcodiexcel = dr.GetString(iDpotnfcodiexcel);                   

                    int iDposubnombre = dr.GetOrdinal(helper.Dposubnombre);
                    if (!dr.IsDBNull(iDposubnombre)) entity.Dposubnombre = dr.GetString(iDposubnombre);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iDpotnfusucreacion = dr.GetOrdinal(helper.Dpotnfusucreacion);
                    if (!dr.IsDBNull(iDpotnfusucreacion)) entity.Dpotnfusucreacion = dr.GetString(iDpotnfusucreacion);

                    int iDpotnffeccreacion = dr.GetOrdinal(helper.Dpotnffeccreacion);
                    if (!dr.IsDBNull(iDpotnffeccreacion)) entity.Dpotnffeccreacion = dr.GetDateTime(iDpotnffeccreacion);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<DpoTransformadorDTO> ListTransformadorBySubestacion(int codigo)
        {
            List<DpoTransformadorDTO> entitys = new List<DpoTransformadorDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListTransformadorBySubestacion);
            dbProvider.AddInParameter(command, helper.Dposubcodi, DbType.Int32, codigo);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    DpoTransformadorDTO entity = new DpoTransformadorDTO();

                    int iDpotnfcodi = dr.GetOrdinal(helper.Dpotnfcodi);
                    if (!dr.IsDBNull(iDpotnfcodi)) entity.Dpotnfcodi = Convert.ToInt32(dr.GetValue(iDpotnfcodi));

                    int iDpotnfcodiexcel = dr.GetOrdinal(helper.Dpotnfcodiexcel);
                    if (!dr.IsDBNull(iDpotnfcodiexcel)) entity.Dpotnfcodiexcel = dr.GetString(iDpotnfcodiexcel);

                    int iDposubcodiexcel = dr.GetOrdinal(helper.Dposubcodiexcel);
                    if (!dr.IsDBNull(iDposubcodiexcel)) entity.Dposubcodiexcel = dr.GetString(iDposubcodiexcel);

                    int iDposubnombre = dr.GetOrdinal(helper.Dposubnombre);
                    if (!dr.IsDBNull(iDposubnombre)) entity.Dposubnombre = dr.GetString(iDposubnombre);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public DpoTransformadorDTO GetById(int codi)
        {
            DpoTransformadorDTO entity = new DpoTransformadorDTO();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);
            dbProvider.AddInParameter(command, helper.Dpotnfcodi, DbType.Int32, codi);
            //string query = string.Format(helper.SqlGetById, codi);

            //DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<DpoTransformadorDTO> ListTransformadorByList(string codigo)
        {
            List<DpoTransformadorDTO> entitys = new List<DpoTransformadorDTO>();
            string sqlQuery = string.Format(helper.SqlListTransformadorByList, codigo);
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    DpoTransformadorDTO entity = new DpoTransformadorDTO();

                    int iDpotnfcodi = dr.GetOrdinal(helper.Dpotnfcodi);
                    if (!dr.IsDBNull(iDpotnfcodi)) entity.Dpotnfcodi = Convert.ToInt32(dr.GetValue(iDpotnfcodi));

                    int iDpotnfcodiexcel = dr.GetOrdinal(helper.Dpotnfcodiexcel);
                    if (!dr.IsDBNull(iDpotnfcodiexcel)) entity.Dpotnfcodiexcel = dr.GetString(iDpotnfcodiexcel);

                    int iDposubnombre = dr.GetOrdinal(helper.Dposubnombre);
                    if (!dr.IsDBNull(iDposubnombre)) entity.Dposubnombre = dr.GetString(iDposubnombre);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iDpotnfusucreacion = dr.GetOrdinal(helper.Dpotnfusucreacion);
                    if (!dr.IsDBNull(iDpotnfusucreacion)) entity.Dpotnfusucreacion = dr.GetString(iDpotnfusucreacion);

                    int iDpotnffeccreacion = dr.GetOrdinal(helper.Dpotnffeccreacion);
                    if (!dr.IsDBNull(iDpotnffeccreacion)) entity.Dpotnffeccreacion = dr.GetDateTime(iDpotnffeccreacion);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<DpoTransformadorDTO> ListTransformadorByListExcel(string codigo)
        {
            List<DpoTransformadorDTO> entitys = new List<DpoTransformadorDTO>();
            string sqlQuery = string.Format(helper.SqlListTransformadorByListExcel, codigo);
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    DpoTransformadorDTO entity = new DpoTransformadorDTO();

                    int iDpotnfcodi = dr.GetOrdinal(helper.Dpotnfcodi);
                    if (!dr.IsDBNull(iDpotnfcodi)) entity.Dpotnfcodi = Convert.ToInt32(dr.GetValue(iDpotnfcodi));

                    int iDpotnfcodiexcel = dr.GetOrdinal(helper.Dpotnfcodiexcel);
                    if (!dr.IsDBNull(iDpotnfcodiexcel)) entity.Dpotnfcodiexcel = dr.GetString(iDpotnfcodiexcel);

                    int iDposubnombre = dr.GetOrdinal(helper.Dposubnombre);
                    if (!dr.IsDBNull(iDposubnombre)) entity.Dposubnombre = dr.GetString(iDposubnombre);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iDpotnfusucreacion = dr.GetOrdinal(helper.Dpotnfusucreacion);
                    if (!dr.IsDBNull(iDpotnfusucreacion)) entity.Dpotnfusucreacion = dr.GetString(iDpotnfusucreacion);

                    int iDpotnffeccreacion = dr.GetOrdinal(helper.Dpotnffeccreacion);
                    if (!dr.IsDBNull(iDpotnffeccreacion)) entity.Dpotnffeccreacion = dr.GetDateTime(iDpotnffeccreacion);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public void UpdateTransformadoresSirpit(string inicio, string fin)
        {
            string query = string.Format(helper.SqlUpdateTransformadoresSirpit, inicio, fin);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            dbProvider.ExecuteNonQuery(command);
        }
    }
}
