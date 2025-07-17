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
    /// Clase de acceso a datos de la tabla EQ_AREAREL
    /// </summary>
    public class EqArearelRepository: RepositoryBase, IEqArearelRepository
    {
        public EqArearelRepository(string strConn): base(strConn)
        {
        }

        EqArearelHelper helper = new EqArearelHelper();

        public int Save(EqAreaRelDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Arearlcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Areapadre, DbType.Int32, entity.AreaPadre);
            dbProvider.AddInParameter(command, helper.Areacodi, DbType.Int32, entity.AreaCodi);
            dbProvider.AddInParameter(command, helper.Fechadat, DbType.DateTime, entity.FechaDat);
            dbProvider.AddInParameter(command, helper.Lastcodi, DbType.Int32, entity.LastCodi);
            dbProvider.AddInParameter(command, helper.Arearlusumodificacion, DbType.String, entity.Arearlusumodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(EqAreaRelDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Arearlcodi, DbType.Int32, entity.AreaRlCodi);
            dbProvider.AddInParameter(command, helper.Areapadre, DbType.Int32, entity.AreaPadre);
            dbProvider.AddInParameter(command, helper.Areacodi, DbType.Int32, entity.AreaCodi);
            dbProvider.AddInParameter(command, helper.Fechadat, DbType.DateTime, entity.FechaDat);
            dbProvider.AddInParameter(command, helper.Lastcodi, DbType.Int32, entity.LastCodi);
            dbProvider.AddInParameter(command, helper.Arearlusumodificacion, DbType.String, entity.Arearlusumodificacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int arearlcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Arearlcodi, DbType.Int32, arearlcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete_UpdateAuditoria(int arearlcodi, string arearlusumodificacion)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete_UpdateAuditoria);
            
            dbProvider.AddInParameter(command, helper.Arearlusumodificacion, DbType.String, arearlusumodificacion);
            dbProvider.AddInParameter(command, helper.Arearlcodi, DbType.Int32, arearlcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public EqAreaRelDTO GetById(int arearlcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Arearlcodi, DbType.Int32, arearlcodi);
            EqAreaRelDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<EqAreaRelDTO> List()
        {
            List<EqAreaRelDTO> entitys = new List<EqAreaRelDTO>();
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

        public List<EqAreaRelDTO> GetByCriteria()
        {
            List<EqAreaRelDTO> entitys = new List<EqAreaRelDTO>();
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

        #region ZONAS
        /// <summary>
        /// Listado de todas las Areas que pertenecen a una determinada zona
        /// </summary>
        /// <returns></returns>
        public List<EqAreaRelDTO> ListarAreasxAreapadre(int areacodi)   
        {
            List<EqAreaRelDTO> entitys = new List<EqAreaRelDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListarAreasxAreapadre);

            dbProvider.AddInParameter(command, helper.Areapadre, DbType.Int32, areacodi); 

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqAreaRelDTO entity = new EqAreaRelDTO();
                    entity = helper.Create(dr);
                    entity.AREAABREV = dr.GetString(dr.GetOrdinal("AREAABREV"));
                    entity.AREANOMB = dr.GetString(dr.GetOrdinal("AREANOMB"));
                    entity.TAREANOMB = dr.GetString(dr.GetOrdinal("TAREANOMB"));
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        /// <summary>
        /// Identificación de un área por medio de la zona a la que pertenece(areapadre) y su codigo de área(areacodi)
        /// </summary>
        /// <returns></returns>
        public EqAreaRelDTO GetxAreapadrexAreacodi(int areapadre, int areacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetxAreapadrexAreacodi);
            dbProvider.AddInParameter(command, helper.Areapadre, DbType.Int32, areapadre);
            dbProvider.AddInParameter(command, helper.Areacodi, DbType.Int32, areacodi);
            EqAreaRelDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }
        #endregion

    }
}
