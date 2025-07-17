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
    /// Clase de acceso a datos de la tabla VCR_VERSIONDSRNS
    /// </summary>
    public class VcrVersiondsrnsRepository: RepositoryBase, IVcrVersiondsrnsRepository
    {
        public VcrVersiondsrnsRepository(string strConn): base(strConn)
        {
        }

        VcrVersiondsrnsHelper helper = new VcrVersiondsrnsHelper();

        public int Save(VcrVersiondsrnsDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Vcrdsrcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, entity.Pericodi);
            dbProvider.AddInParameter(command, helper.Vcrdsrnombre, DbType.String, entity.Vcrdsrnombre);
            dbProvider.AddInParameter(command, helper.Vcrdsrestado, DbType.String, entity.Vcrdsrestado);
            dbProvider.AddInParameter(command, helper.Vcrdsrusucreacion, DbType.String, entity.Vcrdsrusucreacion);
            dbProvider.AddInParameter(command, helper.Vcrdsrfeccreacion, DbType.DateTime, entity.Vcrdsrfeccreacion);
            dbProvider.AddInParameter(command, helper.Vcrdsrusumodificacion, DbType.String, entity.Vcrdsrusumodificacion);
            dbProvider.AddInParameter(command, helper.Vcrdsrfecmodificacion, DbType.DateTime, entity.Vcrdsrfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(VcrVersiondsrnsDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, entity.Pericodi);
            dbProvider.AddInParameter(command, helper.Vcrdsrnombre, DbType.String, entity.Vcrdsrnombre);
            dbProvider.AddInParameter(command, helper.Vcrdsrestado, DbType.String, entity.Vcrdsrestado);
            dbProvider.AddInParameter(command, helper.Vcrdsrusucreacion, DbType.String, entity.Vcrdsrusucreacion);
            dbProvider.AddInParameter(command, helper.Vcrdsrfeccreacion, DbType.DateTime, entity.Vcrdsrfeccreacion);
            dbProvider.AddInParameter(command, helper.Vcrdsrusumodificacion, DbType.String, entity.Vcrdsrusumodificacion);
            dbProvider.AddInParameter(command, helper.Vcrdsrfecmodificacion, DbType.DateTime, entity.Vcrdsrfecmodificacion);
            dbProvider.AddInParameter(command, helper.Vcrdsrcodi, DbType.Int32, entity.Vcrdsrcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int vcrdsrcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Vcrdsrcodi, DbType.Int32, vcrdsrcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public VcrVersiondsrnsDTO GetById(int vcrdsrcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Vcrdsrcodi, DbType.Int32, vcrdsrcodi);
            VcrVersiondsrnsDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public VcrVersiondsrnsDTO GetByIdView(int vcrdsrcodi, int pericodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByIdView);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Vcrdsrcodi, DbType.Int32, vcrdsrcodi);
            
            VcrVersiondsrnsDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iPerinombre = dr.GetOrdinal(this.helper.Perinombre);
                    if (!dr.IsDBNull(iPerinombre)) entity.Perinombre = dr.GetString(iPerinombre);
                }
            }
            return entity;
        }

        public VcrVersiondsrnsDTO GetByIdEdit(int vcrdsrcodi, int pericodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByIdEdit);
            dbProvider.AddInParameter(command, helper.Vcrdsrcodi, DbType.Int32, vcrdsrcodi);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            VcrVersiondsrnsDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public VcrVersiondsrnsDTO GetByIdPeriodo(int pericodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByIdPeriodo);

            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            VcrVersiondsrnsDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<VcrVersiondsrnsDTO> List()
        {
            List<VcrVersiondsrnsDTO> entitys = new List<VcrVersiondsrnsDTO>();
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

        public List<VcrVersiondsrnsDTO> ListIndex()
        {
            List<VcrVersiondsrnsDTO> entitys = new List<VcrVersiondsrnsDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListIndex);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VcrVersiondsrnsDTO entity = helper.Create(dr);

                    int iPerinombre = dr.GetOrdinal(this.helper.Perinombre);
                    if (!dr.IsDBNull(iPerinombre)) entity.Perinombre = dr.GetString(iPerinombre);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<VcrVersiondsrnsDTO> ListVersion(int id = 0)
        {
            List<VcrVersiondsrnsDTO> entitys = new List<VcrVersiondsrnsDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListById);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, id);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VcrVersiondsrnsDTO entity = helper.Create(dr);
                    int iVcrdsrnombre = dr.GetOrdinal(this.helper.Vcrdsrnombre);
                    if (!dr.IsDBNull(iVcrdsrnombre)) entity.Vcrdsrnombre = dr.GetString(iVcrdsrnombre);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<VcrVersiondsrnsDTO> GetByCriteria()
        {
            List<VcrVersiondsrnsDTO> entitys = new List<VcrVersiondsrnsDTO>();
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
