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
    /// Clase de acceso a datos de la tabla CM_MODELO_COMPONENTE
    /// </summary>
    public class CmModeloComponenteRepository : RepositoryBase, ICmModeloComponenteRepository
    {
        public CmModeloComponenteRepository(string strConn) : base(strConn)
        {
        }

        CmModeloComponenteHelper helper = new CmModeloComponenteHelper();

        public int Save(CmModeloComponenteDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Modcomcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Modembcodi, DbType.Int32, entity.Modembcodi);
            dbProvider.AddInParameter(command, helper.Modcomtipo, DbType.String, entity.Modcomtipo);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Modcomtviaje, DbType.Decimal, entity.Modcomtviaje);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CmModeloComponenteDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Modcomcodi, DbType.Int32, entity.Modcomcodi);
            dbProvider.AddInParameter(command, helper.Modembcodi, DbType.Int32, entity.Modembcodi);
            dbProvider.AddInParameter(command, helper.Modcomtipo, DbType.String, entity.Modcomtipo);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Modcomtviaje, DbType.Decimal, entity.Modcomtviaje);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int modcomcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Modcomcodi, DbType.Int32, modcomcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CmModeloComponenteDTO GetById(int modcomcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Modcomcodi, DbType.Int32, modcomcodi);
            CmModeloComponenteDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CmModeloComponenteDTO> List()
        {
            List<CmModeloComponenteDTO> entitys = new List<CmModeloComponenteDTO>();
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

        public List<CmModeloComponenteDTO> GetByCriteria(string modembcodi, string modcomcodis)
        {
            List<CmModeloComponenteDTO> entitys = new List<CmModeloComponenteDTO>();

            string sql = string.Format(helper.SqlGetByCriteria, modembcodi, modcomcodis);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iRecurcodi = dr.GetOrdinal(helper.Recurcodi);
                    if (!dr.IsDBNull(iRecurcodi)) entity.Recurcodi = Convert.ToInt32(dr.GetValue(iRecurcodi));

                    int iRecurnombre = dr.GetOrdinal(helper.Recurnombre);
                    if (!dr.IsDBNull(iRecurnombre)) entity.Recurnombre = dr.GetString(iRecurnombre);

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
