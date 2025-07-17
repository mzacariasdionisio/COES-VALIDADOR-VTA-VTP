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
    /// Clase de acceso a datos de la tabla FT_FICTECITEM_NOTA
    /// </summary>
    public class FtFictecItemNotaRepository : RepositoryBase, IFtFictecItemNotaRepository
    {
        public FtFictecItemNotaRepository(string strConn)
            : base(strConn)
        {
        }

        FtFictecItemNotaHelper helper = new FtFictecItemNotaHelper();

        public int Save(FtFictecItemNotaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Ftitntcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Ftitcodi, DbType.Int32, entity.Ftitcodi);
            dbProvider.AddInParameter(command, helper.Ftnotacodi, DbType.Int32, entity.Ftnotacodi);
            dbProvider.AddInParameter(command, helper.Ftitntfecha, DbType.DateTime, entity.Ftitntfecha);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(FtFictecItemNotaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Ftitcodi, DbType.Int32, entity.Ftitcodi);
            dbProvider.AddInParameter(command, helper.Ftnotacodi, DbType.Int32, entity.Ftnotacodi);
            dbProvider.AddInParameter(command, helper.Ftitntfecha, DbType.DateTime, entity.Ftitntfecha);

            dbProvider.AddInParameter(command, helper.Ftitntcodi, DbType.Int32, entity.Ftitntcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int ftitntcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Ftitntcodi, DbType.Int32, ftitntcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteByFtitcodi(int ftitcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteByFtitcodi);

            dbProvider.AddInParameter(command, helper.Ftitcodi, DbType.Int32, ftitcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public FtFictecItemNotaDTO GetById(int ftitntcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Ftitntcodi, DbType.Int32, ftitntcodi);
            FtFictecItemNotaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<FtFictecItemNotaDTO> List()
        {
            List<FtFictecItemNotaDTO> entitys = new List<FtFictecItemNotaDTO>();
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

        public List<FtFictecItemNotaDTO> GetByCriteria()
        {
            List<FtFictecItemNotaDTO> entitys = new List<FtFictecItemNotaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<FtFictecItemNotaDTO> ListByFteqcodi(int fteqcodi)
        {
            List<FtFictecItemNotaDTO> entitys = new List<FtFictecItemNotaDTO>();

            string query = string.Format(helper.SqlListByFteqcodi, fteqcodi);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    FtFictecItemNotaDTO entity = helper.Create(dr);

                    int iFtnotanum = dr.GetOrdinal(this.helper.Ftnotanum);
                    if (!dr.IsDBNull(iFtnotanum)) entity.Ftnotanum = Convert.ToInt32(dr.GetValue(iFtnotanum));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
