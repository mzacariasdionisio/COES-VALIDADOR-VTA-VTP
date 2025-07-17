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
    /// Clase de acceso a datos de la tabla ST_COMPENSACION
    /// </summary>
    public class StCompensacionRepository: RepositoryBase, IStCompensacionRepository
    {
        public StCompensacionRepository(string strConn): base(strConn)
        {
        }

        StCompensacionHelper helper = new StCompensacionHelper();

        public int Save(StCompensacionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Stcompcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Sistrncodi, DbType.Int32, entity.Sistrncodi);
            dbProvider.AddInParameter(command, helper.Stcompcodelemento, DbType.String, entity.Stcompcodelemento);
            dbProvider.AddInParameter(command, helper.Stcompnomelemento, DbType.String, entity.Stcompnomelemento);
            dbProvider.AddInParameter(command, helper.Stcompimpcompensacion, DbType.Decimal, entity.Stcompimpcompensacion);
            dbProvider.AddInParameter(command, helper.Barrcodi1, DbType.Int32, entity.Barrcodi1);
            dbProvider.AddInParameter(command, helper.Barrcodi2, DbType.Int32, entity.Barrcodi2);
            dbProvider.AddInParameter(command, helper.Sstcmpusucreacion, DbType.String, entity.Sstcmpusucreacion);
            dbProvider.AddInParameter(command, helper.Sstcmpfeccreacion, DbType.DateTime, entity.Sstcmpfeccreacion);
            dbProvider.AddInParameter(command, helper.Sstcmpusumodificacion, DbType.String, entity.Sstcmpusumodificacion);
            dbProvider.AddInParameter(command, helper.Sstcmpfecmodificacion, DbType.DateTime, entity.Sstcmpfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(StCompensacionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Sistrncodi, DbType.Int32, entity.Sistrncodi);
            dbProvider.AddInParameter(command, helper.Stcompcodelemento, DbType.String, entity.Stcompcodelemento);
            dbProvider.AddInParameter(command, helper.Stcompnomelemento, DbType.String, entity.Stcompnomelemento);
            dbProvider.AddInParameter(command, helper.Stcompimpcompensacion, DbType.Decimal, entity.Stcompimpcompensacion);
            dbProvider.AddInParameter(command, helper.Barrcodi1, DbType.Int32, entity.Barrcodi1);
            dbProvider.AddInParameter(command, helper.Barrcodi2, DbType.Int32, entity.Barrcodi2);
            dbProvider.AddInParameter(command, helper.Sstcmpusucreacion, DbType.String, entity.Sstcmpusucreacion);
            dbProvider.AddInParameter(command, helper.Sstcmpfeccreacion, DbType.DateTime, entity.Sstcmpfeccreacion);
            dbProvider.AddInParameter(command, helper.Sstcmpusumodificacion, DbType.String, entity.Sstcmpusumodificacion);
            dbProvider.AddInParameter(command, helper.Sstcmpfecmodificacion, DbType.DateTime, entity.Sstcmpfecmodificacion);
            dbProvider.AddInParameter(command, helper.Stcompcodi, DbType.Int32, entity.Stcompcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int stcompcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Stcompcodi, DbType.Int32, stcompcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteVersion(int strecacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteVersion);

            dbProvider.AddInParameter(command, helper.Strecacodi, DbType.Int32, strecacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public StCompensacionDTO GetById(int stcompcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Stcompcodi, DbType.Int32, stcompcodi);
            StCompensacionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<StCompensacionDTO> List()
        {
            List<StCompensacionDTO> entitys = new List<StCompensacionDTO>();
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

        public List<StCompensacionDTO> GetByCriteria(int strecacodi)
        {
            List<StCompensacionDTO> entitys = new List<StCompensacionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, helper.Strecacodi, DbType.Int32, strecacodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    StCompensacionDTO entity = helper.Create(dr);

                    int iBarrnombre = dr.GetOrdinal(this.helper.Barrnombre);
                    if (!dr.IsDBNull(iBarrnombre)) entity.Barrnombre = dr.GetString(iBarrnombre);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<StCompensacionDTO> GetBySisTrans(int sistrncodi)
        {
            List<StCompensacionDTO> entitys = new List<StCompensacionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.GetBySisTrans);
            dbProvider.AddInParameter(command, helper.Sistrncodi, DbType.Int32, sistrncodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    StCompensacionDTO entity = helper.Create(dr);

                    int iBarrnombre1 = dr.GetOrdinal(this.helper.Barrnombre1);
                    if (!dr.IsDBNull(iBarrnombre1)) entity.Barrnombre1 = dr.GetString(iBarrnombre1);

                    int iBarrnombre2 = dr.GetOrdinal(this.helper.Barrnombre2);
                    if (!dr.IsDBNull(iBarrnombre2)) entity.Barrnombre2 = dr.GetString(iBarrnombre2);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<StCompensacionDTO> ListStCompensacionsPorID(int sistrncodi)
        {
            List<StCompensacionDTO> entitys = new List<StCompensacionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListStCompensacionsPorID);
            dbProvider.AddInParameter(command, helper.Sistrncodi, DbType.Int32, sistrncodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    StCompensacionDTO entity = helper.Create(dr);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
