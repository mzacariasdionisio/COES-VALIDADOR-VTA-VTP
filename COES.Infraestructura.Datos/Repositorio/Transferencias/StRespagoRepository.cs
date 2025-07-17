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
    /// Clase de acceso a datos de la tabla ST_RESPAGO
    /// </summary>
    public class StRespagoRepository : RepositoryBase, IStRespagoRepository
    {
        public StRespagoRepository(string strConn)
            : base(strConn)
        {
        }

        StRespagoHelper helper = new StRespagoHelper();

        public int Save(StRespagoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Respagcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Strecacodi, DbType.Int32, entity.Strecacodi);
            //dbProvider.AddInParameter(command, helper.Sistrncodi, DbType.Int32, entity.Sistrncodi);
            dbProvider.AddInParameter(command, helper.Stcntgcodi, DbType.Int32, entity.Stcntgcodi);
            dbProvider.AddInParameter(command, helper.Respagusucreacion, DbType.String, entity.Respagusucreacion);
            dbProvider.AddInParameter(command, helper.Respagfeccreacion, DbType.DateTime, entity.Respagfeccreacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(StRespagoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Strecacodi, DbType.Int32, entity.Strecacodi);
            //dbProvider.AddInParameter(command, helper.Sistrncodi, DbType.Int32, entity.Sistrncodi);
            dbProvider.AddInParameter(command, helper.Stcntgcodi, DbType.Int32, entity.Stcntgcodi);
            dbProvider.AddInParameter(command, helper.Respagusucreacion, DbType.String, entity.Respagusucreacion);
            dbProvider.AddInParameter(command, helper.Respagfeccreacion, DbType.DateTime, entity.Respagfeccreacion);
            dbProvider.AddInParameter(command, helper.Respagcodi, DbType.Int32, entity.Respagcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int strecacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Strecacodi, DbType.Int32, strecacodi);

            dbProvider.ExecuteNonQuery(command);


        }

        public StRespagoDTO GetById(int respagcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Respagcodi, DbType.Int32, respagcodi);
            StRespagoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<StRespagoDTO> GetByCodElem(int strecacodi, int stcompcodi)
        {
            List<StRespagoDTO> entitys = new List<StRespagoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCodElem);
            dbProvider.AddInParameter(command, helper.Strecacodi, DbType.Int32, strecacodi);
            dbProvider.AddInParameter(command, helper.Stcompcodi, DbType.Int32, stcompcodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    StRespagoDTO entity = helper.Create(dr);

                    //int iStcompcodelemento = dr.GetOrdinal(this.helper.Stcompcodelemento);
                    //if (!dr.IsDBNull(iStcompcodelemento)) entity.Stcompcodelemento = dr.GetString(iStcompcodelemento);

                    int iEquinomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<StRespagoDTO> List()
        {
            List<StRespagoDTO> entitys = new List<StRespagoDTO>();
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

        public List<StRespagoDTO> GetByCriteria(int recacodi)
        {
            List<StRespagoDTO> entitys = new List<StRespagoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, helper.Strecacodi, DbType.Int32, recacodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    StRespagoDTO entity = helper.Create(dr);

                    int iEquinomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    //int iSisNombre = dr.GetOrdinal(this.helper.Sistrnnombre);
                    //if (!dr.IsDBNull(iSisNombre)) entity.Sistrnnombre = dr.GetString(iSisNombre);

                    entitys.Add(entity);
                }

            }

            return entitys;
        }

        public List<StRespagoDTO> ListByStRespagoVersion(int strecacodi)
        {
            List<StRespagoDTO> entitys = new List<StRespagoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListByStRespagoVersion);
            dbProvider.AddInParameter(command, helper.Strecacodi, DbType.Int32, strecacodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    StRespagoDTO entity = helper.Create(dr);
                    entitys.Add(entity);
                }

            }
            return entitys;
        }

    }
}
