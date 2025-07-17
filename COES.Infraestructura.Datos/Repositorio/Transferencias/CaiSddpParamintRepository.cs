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
    /// Clase de acceso a datos de la tabla CAI_SDDP_PARAMINT
    /// </summary>
    /// 
    public class CaiSddpParamintRepository : RepositoryBase, ICaiSddpParamintRepository
    {

        public CaiSddpParamintRepository(string strConn)
            : base(strConn)
        {
        }

        CaiSddpParamintHelper helper = new CaiSddpParamintHelper();

        public int Save(CaiSddpParamintDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Sddppicodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Caiajcodi, DbType.Int32, entity.Caiajcodi);
            dbProvider.AddInParameter(command, helper.Sddppiintervalo, DbType.DateTime, entity.Sddppiintervalo);
            dbProvider.AddInParameter(command, helper.Sddppilaboral, DbType.Int32, entity.Sddppilaboral);
            dbProvider.AddInParameter(command, helper.Sddppibloque, DbType.Int32, entity.Sddppibloque);
            dbProvider.AddInParameter(command, helper.Sddppiusucreacion, DbType.String, entity.Sddppiusucreacion);
            dbProvider.AddInParameter(command, helper.Sddppifeccreacion, DbType.DateTime, entity.Sddppifeccreacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CaiSddpParamintDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Caiajcodi, DbType.Int32, entity.Caiajcodi);
            dbProvider.AddInParameter(command, helper.Sddppiintervalo, DbType.Int32, entity.Sddppiintervalo);
            dbProvider.AddInParameter(command, helper.Sddppilaboral, DbType.Int32, entity.Sddppilaboral);
            dbProvider.AddInParameter(command, helper.Sddppibloque, DbType.Int32, entity.Sddppibloque);
            dbProvider.AddInParameter(command, helper.Sddppiusucreacion, DbType.String, entity.Sddppiusucreacion);
            dbProvider.AddInParameter(command, helper.Sddppifeccreacion, DbType.DateTime, entity.Sddppifeccreacion);
            dbProvider.AddInParameter(command, helper.Sddppicodi, DbType.Int32, entity.Sddppicodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.ExecuteNonQuery(command);
        }

        public CaiSddpParamintDTO GetById(int Sddppicodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Sddppicodi, DbType.Int32, Sddppicodi);
            CaiSddpParamintDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CaiSddpParamintDTO> List(int Sddppicodi)
        {
            List<CaiSddpParamintDTO> entitys = new List<CaiSddpParamintDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);
            dbProvider.AddInParameter(command, helper.Caiajcodi, DbType.Int32, Sddppicodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<CaiSddpParamintDTO> GetByCriteria(int caiajcodi)
        {
            List<CaiSddpParamintDTO> entitys = new List<CaiSddpParamintDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);

            dbProvider.AddInParameter(command, helper.Caiajcodi, DbType.Int32, caiajcodi);

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
