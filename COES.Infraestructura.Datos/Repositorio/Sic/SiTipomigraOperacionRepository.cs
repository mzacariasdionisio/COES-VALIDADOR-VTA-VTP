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
    /// Clase de acceso a datos de la tabla SI_TIPOMIGRAOPERACION
    /// </summary>
    public class SiTipomigraOperacionRepository: RepositoryBase, ISiTipomigraOperacionRepository
    {
        public SiTipomigraOperacionRepository(string strConn): base(strConn)
        {
        }

        SiTipomigraOperacionHelper helper = new SiTipomigraOperacionHelper();

        public int Save(SiTipomigraoperacionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Tmopercodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Tmoperdescripcion, DbType.String, entity.Tmoperdescripcion);
            dbProvider.AddInParameter(command, helper.Tmoperusucreacion, DbType.String, entity.Tmoperusucreacion);
            dbProvider.AddInParameter(command, helper.Tmoperfeccreacion, DbType.DateTime, entity.Tmoperfeccreacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(SiTipomigraoperacionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Tmopercodi, DbType.Int32, entity.Tmopercodi);
            dbProvider.AddInParameter(command, helper.Tmoperdescripcion, DbType.String, entity.Tmoperdescripcion);
            dbProvider.AddInParameter(command, helper.Tmoperusucreacion, DbType.String, entity.Tmoperusucreacion);
            dbProvider.AddInParameter(command, helper.Tmoperfeccreacion, DbType.DateTime, entity.Tmoperfeccreacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int tmopercodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Tmopercodi, DbType.Int32, tmopercodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public SiTipomigraoperacionDTO GetById(int tmopercodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Tmopercodi, DbType.Int32, tmopercodi);
            SiTipomigraoperacionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SiTipomigraoperacionDTO> List()
        {
            List<SiTipomigraoperacionDTO> entitys = new List<SiTipomigraoperacionDTO>();
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

        public List<SiTipomigraoperacionDTO> GetByCriteria()
        {
            List<SiTipomigraoperacionDTO> entitys = new List<SiTipomigraoperacionDTO>();
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
    }
}
