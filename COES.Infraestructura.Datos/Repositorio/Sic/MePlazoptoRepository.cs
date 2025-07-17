using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Infraestructura.Datos.Helper.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla ME_PLAZOPTO
    /// </summary>
    public class MePlazoptoRepository : RepositoryBase, IMePlazoptoRepository
    {
        public MePlazoptoRepository(string strConn) : base(strConn)
        {
        }

        MePlazoptoHelper helper = new MePlazoptoHelper();

        public int Save(MePlazoptoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Plzptocodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Plzptodiafinplazo, DbType.Int32, entity.Plzptodiafinplazo);
            dbProvider.AddInParameter(command, helper.Plzptominfinplazo, DbType.Int32, entity.Plzptominfinplazo);
            dbProvider.AddInParameter(command, helper.Plzptofecvigencia, DbType.DateTime, entity.Plzptofechavigencia);
            dbProvider.AddInParameter(command, helper.Plzptofeccreacion, DbType.DateTime, entity.Plzptofecharegistro);
            dbProvider.AddInParameter(command, helper.Plzptominfila, DbType.Int32, entity.Plzptominfila);
            dbProvider.AddInParameter(command, helper.Formatcodi, DbType.Int32, entity.Formatcodi);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.Tipoinfocodi, DbType.Int32, entity.Tipoinfocodi);
            dbProvider.AddInParameter(command, helper.Plzptousucreacion, DbType.String, entity.Plzptousuarioregistro);
            dbProvider.AddInParameter(command, helper.Plzptofecmodificacion, DbType.DateTime, entity.Plzptofechamodificacion);
            dbProvider.AddInParameter(command, helper.Plzptousumodificacion, DbType.String, entity.Plzptousuariomodificacion);



            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(MePlazoptoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);


            dbProvider.AddInParameter(command, helper.Plzptodiafinplazo, DbType.Int32, entity.Plzptodiafinplazo);
            dbProvider.AddInParameter(command, helper.Plzptominfinplazo, DbType.Int32, entity.Plzptominfinplazo);
            dbProvider.AddInParameter(command, helper.Plzptominfila, DbType.Int32, entity.Plzptominfila);
            dbProvider.AddInParameter(command, helper.Formatcodi, DbType.Int32, entity.Formatcodi);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.Tipoinfocodi, DbType.Int32, entity.Tipoinfocodi);
            dbProvider.AddInParameter(command, helper.Plzptofecmodificacion, DbType.DateTime, entity.Plzptofechamodificacion);
            dbProvider.AddInParameter(command, helper.Plzptousumodificacion, DbType.String, entity.Plzptousuariomodificacion);
            dbProvider.AddInParameter(command, helper.Plzptocodi, DbType.Int32, entity.Plzptocodi);
            dbProvider.ExecuteNonQuery(command);
        }


        public void Delete(int plzptocodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Plzptocodi, DbType.Int32, plzptocodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public MePlazoptoDTO GetById(int plzptocodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Plzptocodi, DbType.Int32, plzptocodi);
            MePlazoptoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<MePlazoptoDTO> List()
        {
            List<MePlazoptoDTO> entitys = new List<MePlazoptoDTO>();
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

        public List<MePlazoptoDTO> GetByCriteria()
        {
            List<MePlazoptoDTO> entitys = new List<MePlazoptoDTO>();
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
