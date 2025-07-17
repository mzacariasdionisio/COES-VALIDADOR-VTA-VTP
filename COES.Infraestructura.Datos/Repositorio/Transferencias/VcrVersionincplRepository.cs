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
    /// Clase de acceso a datos de la tabla VCR_VERSIONINCPL
    /// </summary>
    public class VcrVersionincplRepository: RepositoryBase, IVcrVersionincplRepository
    {
        public VcrVersionincplRepository(string strConn): base(strConn)
        {
        }

        VcrVersionincplHelper helper = new VcrVersionincplHelper();

        public int Save(VcrVersionincplDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Vcrinccodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, entity.Pericodi);
            dbProvider.AddInParameter(command, helper.Vcrincnombre, DbType.String, entity.Vcrincnombre);
            dbProvider.AddInParameter(command, helper.Vcrincestado, DbType.String, entity.Vcrincestado);
            dbProvider.AddInParameter(command, helper.Vcrincusucreacion, DbType.String, entity.Vcrincusucreacion);
            dbProvider.AddInParameter(command, helper.Vcrincfeccreacion, DbType.DateTime, entity.Vcrincfeccreacion);
            dbProvider.AddInParameter(command, helper.Vcrincusumodificacion, DbType.String, entity.Vcrincusumodificacion);
            dbProvider.AddInParameter(command, helper.Vcrincfecmodificacion, DbType.DateTime, entity.Vcrincfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(VcrVersionincplDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, entity.Pericodi);
            dbProvider.AddInParameter(command, helper.Vcrincnombre, DbType.String, entity.Vcrincnombre);
            dbProvider.AddInParameter(command, helper.Vcrincestado, DbType.String, entity.Vcrincestado);
            dbProvider.AddInParameter(command, helper.Vcrincusucreacion, DbType.String, entity.Vcrincusucreacion);
            dbProvider.AddInParameter(command, helper.Vcrincfeccreacion, DbType.DateTime, entity.Vcrincfeccreacion);
            dbProvider.AddInParameter(command, helper.Vcrincusumodificacion, DbType.String, entity.Vcrincusumodificacion);
            dbProvider.AddInParameter(command, helper.Vcrincfecmodificacion, DbType.DateTime, entity.Vcrincfecmodificacion);
            dbProvider.AddInParameter(command, helper.Vcrinccodi, DbType.Int32, entity.Vcrinccodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int vcrinccodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Vcrinccodi, DbType.Int32, vcrinccodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public VcrVersionincplDTO GetById(int vcrinccodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Vcrinccodi, DbType.Int32, vcrinccodi);
            VcrVersionincplDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public VcrVersionincplDTO GetByIdEdit(int vcrinccodi, int pericodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByIdEdit);

            dbProvider.AddInParameter(command, helper.Vcrinccodi, DbType.Int32, vcrinccodi);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            VcrVersionincplDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public VcrVersionincplDTO GetByIdView(int vcrinccodi, int pericodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByIdView);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.Vcrinccodi, DbType.Int32, vcrinccodi);

            VcrVersionincplDTO entity = null;

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

        public List<VcrVersionincplDTO> List()
        {
            List<VcrVersionincplDTO> entitys = new List<VcrVersionincplDTO>();
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

        public List<VcrVersionincplDTO> ListIncpl(int id = 0)
        {
            List<VcrVersionincplDTO> entitys = new List<VcrVersionincplDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListById);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, id);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VcrVersionincplDTO entity = helper.Create(dr);
                    int iVcrincnombre = dr.GetOrdinal(this.helper.Vcrincnombre);
                    if (!dr.IsDBNull(iVcrincnombre)) entity.Vcrincnombre = dr.GetString(iVcrincnombre);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<VcrVersionincplDTO> ListIncplIndex()
        {
            List<VcrVersionincplDTO> entitys = new List<VcrVersionincplDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListIndex);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VcrVersionincplDTO entity = helper.Create(dr);

                    int iPerinombre = dr.GetOrdinal(this.helper.Perinombre);
                    if (!dr.IsDBNull(iPerinombre)) entity.Perinombre = dr.GetString(iPerinombre);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<VcrVersionincplDTO> GetByCriteria()
        {
            List<VcrVersionincplDTO> entitys = new List<VcrVersionincplDTO>();
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
