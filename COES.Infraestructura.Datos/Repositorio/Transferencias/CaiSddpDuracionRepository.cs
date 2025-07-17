using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Transferencias;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Transferencias;

namespace COES.Infraestructura.Datos.Repositorio.Transferencias
{
    /// <summary>
    /// Clase de acceso a datos de la tabla CAI_SDDP_DURACION
    /// </summary>
    public class CaiSddpDuracionRepository: RepositoryBase, ICaiSddpDuracionRepository
    {
        public CaiSddpDuracionRepository(string strConn): base(strConn)
        {
        }

        CaiSddpDuracionHelper helper = new CaiSddpDuracionHelper();

        public int Save(CaiSddpDuracionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Sddpducodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Caiajcodi, DbType.Int32, entity.Caiajcodi);
            dbProvider.AddInParameter(command, helper.Sddpduetapa, DbType.Int32, entity.Sddpduetapa);
            dbProvider.AddInParameter(command, helper.Sddpduserie, DbType.Int32, entity.Sddpduserie);
            dbProvider.AddInParameter(command, helper.Sddpdubloque, DbType.Int32, entity.Sddpdubloque);
            dbProvider.AddInParameter(command, helper.Sddpduduracion, DbType.Decimal, entity.Sddpduduracion);
            dbProvider.AddInParameter(command, helper.Sddpduusucreacion, DbType.String, entity.Sddpduusucreacion);
            dbProvider.AddInParameter(command, helper.Sddpdufeccreacion, DbType.DateTime, entity.Sddpdufeccreacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CaiSddpDuracionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Caiajcodi, DbType.Int32, entity.Caiajcodi);
            dbProvider.AddInParameter(command, helper.Sddpduetapa, DbType.Int32, entity.Sddpduetapa);
            dbProvider.AddInParameter(command, helper.Sddpduserie, DbType.Int32, entity.Sddpduserie);
            dbProvider.AddInParameter(command, helper.Sddpdubloque, DbType.Int32, entity.Sddpdubloque);
            dbProvider.AddInParameter(command, helper.Sddpduduracion, DbType.Decimal, entity.Sddpduduracion);
            dbProvider.AddInParameter(command, helper.Sddpduusucreacion, DbType.String, entity.Sddpduusucreacion);
            dbProvider.AddInParameter(command, helper.Sddpdufeccreacion, DbType.DateTime, entity.Sddpdufeccreacion);
            dbProvider.AddInParameter(command, helper.Sddpducodi, DbType.Int32, entity.Sddpducodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.ExecuteNonQuery(command);
        }

        public CaiSddpDuracionDTO GetById(int sddpducodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Sddpducodi, DbType.Int32, sddpducodi);
            CaiSddpDuracionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CaiSddpDuracionDTO> List()
        {
            List<CaiSddpDuracionDTO> entitys = new List<CaiSddpDuracionDTO>();
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

        public List<CaiSddpDuracionDTO> GetByCriteria()
        {
            List<CaiSddpDuracionDTO> entitys = new List<CaiSddpDuracionDTO>();
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

        public List<CaiSddpDuracionDTO> ListByEtapa(int sddpduetapa)
        {
            List<CaiSddpDuracionDTO> entitys = new List<CaiSddpDuracionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListPorEtapa);
            dbProvider.AddInParameter(command, helper.Sddpduetapa, DbType.Int32, sddpduetapa);

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
