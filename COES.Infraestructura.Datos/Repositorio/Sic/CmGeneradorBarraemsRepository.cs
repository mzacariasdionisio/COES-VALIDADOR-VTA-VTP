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
    /// Clase de acceso a datos de la tabla CM_GENERADOR_BARRAEMS
    /// </summary>
    public class CmGeneradorBarraemsRepository: RepositoryBase, ICmGeneradorBarraemsRepository
    {
        public CmGeneradorBarraemsRepository(string strConn): base(strConn)
        {
        }

        CmGeneradorBarraemsHelper helper = new CmGeneradorBarraemsHelper();

        public int Save(CmGeneradorBarraemsDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Genbarcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Relacioncodi, DbType.Int32, entity.Relacioncodi);
            dbProvider.AddInParameter(command, helper.Cnfbarcodi, DbType.Int32, entity.Cnfbarcodi);
            dbProvider.AddInParameter(command, helper.Genbarusucreacion, DbType.String, entity.Genbarusucreacion);
            dbProvider.AddInParameter(command, helper.Genbarfeccreacion, DbType.DateTime, entity.Genbarfeccreacion);
            dbProvider.AddInParameter(command, helper.Genbarusumodificacion, DbType.String, entity.Genbarusumodificacion);
            dbProvider.AddInParameter(command, helper.Genbarfecmodificacion, DbType.DateTime, entity.Genbarfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CmGeneradorBarraemsDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Relacioncodi, DbType.Int32, entity.Relacioncodi);
            dbProvider.AddInParameter(command, helper.Cnfbarcodi, DbType.Int32, entity.Cnfbarcodi);
            dbProvider.AddInParameter(command, helper.Genbarusucreacion, DbType.String, entity.Genbarusucreacion);
            dbProvider.AddInParameter(command, helper.Genbarfeccreacion, DbType.DateTime, entity.Genbarfeccreacion);
            dbProvider.AddInParameter(command, helper.Genbarusumodificacion, DbType.String, entity.Genbarusumodificacion);
            dbProvider.AddInParameter(command, helper.Genbarfecmodificacion, DbType.DateTime, entity.Genbarfecmodificacion);
            dbProvider.AddInParameter(command, helper.Genbarcodi, DbType.Int32, entity.Genbarcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int genbarcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Genbarcodi, DbType.Int32, genbarcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CmGeneradorBarraemsDTO GetById(int genbarcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Genbarcodi, DbType.Int32, genbarcodi);
            CmGeneradorBarraemsDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CmGeneradorBarraemsDTO> List()
        {
            List<CmGeneradorBarraemsDTO> entitys = new List<CmGeneradorBarraemsDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CmGeneradorBarraemsDTO entity = helper.Create(dr);

                    int iCnfbarnombre = dr.GetOrdinal(helper.Cnfbarnombre);
                    if (!dr.IsDBNull(iCnfbarnombre)) entity.Cnfbarnombre = dr.GetString(iCnfbarnombre);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<CmGeneradorBarraemsDTO> GetByCriteria(int relacionCodi)
        {
            List<CmGeneradorBarraemsDTO> entitys = new List<CmGeneradorBarraemsDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, helper.Relacioncodi, DbType.Int32, relacionCodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CmGeneradorBarraemsDTO entity = helper.Create(dr);

                    int iCnfbarnombre = dr.GetOrdinal(helper.Cnfbarnombre);
                    if (!dr.IsDBNull(iCnfbarnombre)) entity.Cnfbarnombre = dr.GetString(iCnfbarnombre);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
