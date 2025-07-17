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
    /// Clase de acceso a datos de la tabla SI_TIPOGENERACION
    /// </summary>
    public class SiTipogeneracionRepository: RepositoryBase, ISiTipogeneracionRepository
    {
        public SiTipogeneracionRepository(string strConn): base(strConn)
        {
        }

        SiTipogeneracionHelper helper = new SiTipogeneracionHelper();

        public int Save(SiTipogeneracionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Tgenercodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Tgenerabrev, DbType.String, entity.Tgenerabrev);
            dbProvider.AddInParameter(command, helper.Tgenernomb, DbType.String, entity.Tgenernomb);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(SiTipogeneracionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Tgenercodi, DbType.Int32, entity.Tgenercodi);
            dbProvider.AddInParameter(command, helper.Tgenerabrev, DbType.String, entity.Tgenerabrev);
            dbProvider.AddInParameter(command, helper.Tgenernomb, DbType.String, entity.Tgenernomb);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int tgenercodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Tgenercodi, DbType.Int32, tgenercodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public SiTipogeneracionDTO GetById(int tgenercodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Tgenercodi, DbType.Int32, tgenercodi);
            SiTipogeneracionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SiTipogeneracionDTO> List()
        {
            List<SiTipogeneracionDTO> entitys = new List<SiTipogeneracionDTO>();
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

        public List<SiTipogeneracionDTO> GetByCriteria()
        {
            List<SiTipogeneracionDTO> entitys = new List<SiTipogeneracionDTO>();
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

        #region PR5
        public List<SiTipogeneracionDTO> TipoGeneracionxCentral(string equicodi)
        {
            List<SiTipogeneracionDTO> entitys = new List<SiTipogeneracionDTO>();
            string query = string.Format(helper.SqlTipoGeneracionxCentral, equicodi);
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
        #endregion
    }
}
