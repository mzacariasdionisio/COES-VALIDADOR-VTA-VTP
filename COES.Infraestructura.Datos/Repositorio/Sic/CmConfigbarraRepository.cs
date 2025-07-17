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
    /// Clase de acceso a datos de la tabla CM_CONFIGBARRA
    /// </summary>
    public class CmConfigbarraRepository: RepositoryBase, ICmConfigbarraRepository
    {
        public CmConfigbarraRepository(string strConn): base(strConn)
        {
        }

        CmConfigbarraHelper helper = new CmConfigbarraHelper();

        public int Save(CmConfigbarraDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Cnfbarcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Cnfbarnodo, DbType.String, entity.Cnfbarnodo);
            dbProvider.AddInParameter(command, helper.Cnfbarnombre, DbType.String, entity.Cnfbarnombre);
            dbProvider.AddInParameter(command, helper.Cnfbarestado, DbType.String, entity.Cnfbarestado);
            dbProvider.AddInParameter(command, helper.Cnfbarindpublicacion, DbType.String, entity.Cnfbarindpublicacion);
            dbProvider.AddInParameter(command, helper.Cnfbarusucreacion, DbType.String, entity.Cnfbarusucreacion);
            dbProvider.AddInParameter(command, helper.Cnfbarfeccreacion, DbType.DateTime, entity.Cnfbarfeccreacion);
            dbProvider.AddInParameter(command, helper.Cnfbarusumodificacion, DbType.String, entity.Cnfbarusumodificacion);
            dbProvider.AddInParameter(command, helper.Cnfbarfecmodificacion, DbType.DateTime, entity.Cnfbarfecmodificacion);
            dbProvider.AddInParameter(command, helper.Cnfbardefecto, DbType.String, entity.Cnfbardefecto);
            dbProvider.AddInParameter(command, helper.Cnfbarnombncp, DbType.String, entity.Cnfbarnombncp);
            dbProvider.AddInParameter(command, helper.Cnfbarnomtna, DbType.String, entity.Cnfbarnomtna);

            #region Mejoras CMgN    
            dbProvider.AddInParameter(command, helper.Canalcodi, DbType.Int32, entity.Canalcodi);
            dbProvider.AddInParameter(command, helper.Recurcodi, DbType.Int32, entity.Recurcodi);
            dbProvider.AddInParameter(command, helper.Topcodi, DbType.Int32, entity.Topcodi);
            #endregion

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CmConfigbarraDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Cnfbarnodo, DbType.String, entity.Cnfbarnodo);
            dbProvider.AddInParameter(command, helper.Cnfbarnombre, DbType.String, entity.Cnfbarnombre);            
            dbProvider.AddInParameter(command, helper.Cnfbarestado, DbType.String, entity.Cnfbarestado);
            dbProvider.AddInParameter(command, helper.Cnfbarindpublicacion, DbType.String, entity.Cnfbarindpublicacion);
            dbProvider.AddInParameter(command, helper.Cnfbarusumodificacion, DbType.String, entity.Cnfbarusumodificacion);
            dbProvider.AddInParameter(command, helper.Cnfbarfecmodificacion, DbType.DateTime, entity.Cnfbarfecmodificacion);
            dbProvider.AddInParameter(command, helper.Cnfbardefecto, DbType.String, entity.Cnfbardefecto);
            dbProvider.AddInParameter(command, helper.Cnfbarnombncp, DbType.String, entity.Cnfbarnombncp);
            dbProvider.AddInParameter(command, helper.Cnfbarnomtna, DbType.String, entity.Cnfbarnomtna);

            #region Mejoras CMgN
            dbProvider.AddInParameter(command, helper.Canalcodi, DbType.Int32, entity.Canalcodi);
            dbProvider.AddInParameter(command, helper.Recurcodi, DbType.Int32, entity.Recurcodi);
            dbProvider.AddInParameter(command, helper.Topcodi, DbType.Int32, entity.Topcodi);
            #endregion

            dbProvider.AddInParameter(command, helper.Cnfbarcodi, DbType.Int32, entity.Cnfbarcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void UpdateCoordenada(CmConfigbarraDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdateCoordenada);
                        
            dbProvider.AddInParameter(command, helper.Cnfbarcoorx, DbType.String, entity.Cnfbarcoorx);
            dbProvider.AddInParameter(command, helper.Cnfbarcoory, DbType.String, entity.Cnfbarcoory);           
            dbProvider.AddInParameter(command, helper.Cnfbarusumodificacion, DbType.String, entity.Cnfbarusumodificacion);
            dbProvider.AddInParameter(command, helper.Cnfbarfecmodificacion, DbType.DateTime, entity.Cnfbarfecmodificacion);
            dbProvider.AddInParameter(command, helper.Cnfbarcodi, DbType.Int32, entity.Cnfbarcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int cnfbarcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Cnfbarcodi, DbType.Int32, cnfbarcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CmConfigbarraDTO GetById(int cnfbarcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Cnfbarcodi, DbType.Int32, cnfbarcodi);
            CmConfigbarraDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CmConfigbarraDTO> List()
        {
            List<CmConfigbarraDTO> entitys = new List<CmConfigbarraDTO>();
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

        public List<CmConfigbarraDTO> GetByCriteria(string estado, string publicacion)
        {
            List<CmConfigbarraDTO> entitys = new List<CmConfigbarraDTO>();

            string sql = string.Format(helper.SqlGetByCriteria, estado, publicacion);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);                

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CmConfigbarraDTO entity = helper.Create(dr);

                    int iRecurnombre = dr.GetOrdinal(helper.Recurnombre);
                    if (!dr.IsDBNull(iRecurnombre)) entity.Recurnombre = dr.GetString(iRecurnombre);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int ValidarRegistro(int recurcodi, int topcodi, int canalcodi, int cnfbarcodi)
        {
            string sql = string.Format(helper.SqlValidarRegistro, recurcodi, topcodi, canalcodi, cnfbarcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            object result = dbProvider.ExecuteScalar(command);

            if (result != null) {
                return Convert.ToInt32(result);
            }

            return 0;
        }

        public List<CmConfigbarraDTO> ObtenerBarrasYupana(int topcodi, int catcodi)
        {
            List<CmConfigbarraDTO> entitys = new List<CmConfigbarraDTO>();
            string sql = string.Format(helper.SqlObtenerBarrasYupana, topcodi, catcodi);

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CmConfigbarraDTO entity = new CmConfigbarraDTO();

                    int iRecurcodi = dr.GetOrdinal(helper.Recurcodi);
                    if (!dr.IsDBNull(iRecurcodi)) entity.Recurcodi = Convert.ToInt32(dr.GetValue(iRecurcodi));

                    int iRecurnombre = dr.GetOrdinal(helper.Recurnombre);
                    if (!dr.IsDBNull(iRecurnombre)) entity.Recurnombre = dr.GetString(iRecurnombre);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int ValidarCodigoScada(int canalcodi)
        {
            string sql = string.Format(helper.SqlValidarCodigoScada, canalcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            object result = dbProvider.ExecuteScalar(command);

            if (result != null)
            {
                return Convert.ToInt32(result);
            }

            return 0;
        }
    }
}
