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
    /// Clase de acceso a datos de la tabla PFR_ENTIDAD
    /// </summary>
    public class PfrEntidadRepository: RepositoryBase, IPfrEntidadRepository
    {
        public PfrEntidadRepository(string strConn): base(strConn)
        {
        }

        PfrEntidadHelper helper = new PfrEntidadHelper();

        public int Save(PfrEntidadDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Pfrentcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Pfrentnomb, DbType.String, entity.Pfrentnomb);
            dbProvider.AddInParameter(command, helper.Pfrentid, DbType.String, entity.Pfrentid);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Pfrentcodibarragams, DbType.Int32, entity.Pfrentcodibarragams);
            dbProvider.AddInParameter(command, helper.Barrcodi, DbType.Int32, entity.Barrcodi);
            dbProvider.AddInParameter(command, helper.Equipadre, DbType.Int32, entity.Equipadre);
            dbProvider.AddInParameter(command, helper.Pfrcatcodi, DbType.Int32, entity.Pfrcatcodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Pfrentfeccreacion, DbType.DateTime, entity.Pfrentfeccreacion);
            dbProvider.AddInParameter(command, helper.Pfrentcodibarragams2, DbType.Int32, entity.Pfrentcodibarragams2);
            dbProvider.AddInParameter(command, helper.Pfrentestado, DbType.Int32, entity.Pfrentestado);
            dbProvider.AddInParameter(command, helper.Pfrentfecmodificacion, DbType.DateTime, entity.Pfrentfecmodificacion);
            dbProvider.AddInParameter(command, helper.Pfrentusucreacion, DbType.String, entity.Pfrentusucreacion);
            dbProvider.AddInParameter(command, helper.Pfrentusumodificacion, DbType.String, entity.Pfrentusumodificacion);
            dbProvider.AddInParameter(command, helper.Pfrentficticio, DbType.Int32, entity.Pfrentficticio);
            dbProvider.AddInParameter(command, helper.Pfrentunidadnomb, DbType.String, entity.Pfrentunidadnomb);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(PfrEntidadDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Pfrentnomb, DbType.String, entity.Pfrentnomb);
            dbProvider.AddInParameter(command, helper.Pfrentid, DbType.String, entity.Pfrentid);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Pfrentcodibarragams, DbType.Int32, entity.Pfrentcodibarragams);
            dbProvider.AddInParameter(command, helper.Barrcodi, DbType.Int32, entity.Barrcodi);
            dbProvider.AddInParameter(command, helper.Equipadre, DbType.Int32, entity.Equipadre);
            dbProvider.AddInParameter(command, helper.Pfrcatcodi, DbType.Int32, entity.Pfrcatcodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Pfrentfeccreacion, DbType.DateTime, entity.Pfrentfeccreacion);
            dbProvider.AddInParameter(command, helper.Pfrentcodibarragams2, DbType.Int32, entity.Pfrentcodibarragams2);
            dbProvider.AddInParameter(command, helper.Pfrentestado, DbType.Int32, entity.Pfrentestado);
            dbProvider.AddInParameter(command, helper.Pfrentfecmodificacion, DbType.DateTime, entity.Pfrentfecmodificacion);
            dbProvider.AddInParameter(command, helper.Pfrentusucreacion, DbType.String, entity.Pfrentusucreacion);
            dbProvider.AddInParameter(command, helper.Pfrentusumodificacion, DbType.String, entity.Pfrentusumodificacion);
            dbProvider.AddInParameter(command, helper.Pfrentficticio, DbType.Int32, entity.Pfrentficticio);
            dbProvider.AddInParameter(command, helper.Pfrentunidadnomb, DbType.String, entity.Pfrentunidadnomb);

            dbProvider.AddInParameter(command, helper.Pfrentcodi, DbType.Int32, entity.Pfrentcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int pfrentcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Pfrentcodi, DbType.Int32, pfrentcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public PfrEntidadDTO GetById(int pfrentcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Pfrentcodi, DbType.Int32, pfrentcodi);
            PfrEntidadDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<PfrEntidadDTO> List()
        {
            List<PfrEntidadDTO> entitys = new List<PfrEntidadDTO>();
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

        public List<PfrEntidadDTO> GetByCriteria(int pfrcatcodi, string pfrentcodi, int pfrentestado)
        {
            List<PfrEntidadDTO> entitys = new List<PfrEntidadDTO>();
            var sqlQuery = string.Format(helper.SqlGetByCriteria, pfrcatcodi, pfrentcodi, pfrentestado);
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);
                    int iIdbarra1desc = dr.GetOrdinal(helper.Idbarra1desc);
                    if (!dr.IsDBNull(iIdbarra1desc)) entity.Idbarra1desc = dr.GetString(iIdbarra1desc);

                    int iIdbarra2desc = dr.GetOrdinal(helper.Idbarra2desc);
                    if (!dr.IsDBNull(iIdbarra2desc)) entity.Idbarra2desc = dr.GetString(iIdbarra2desc);       
                    
                    int iIdbarra2 = dr.GetOrdinal(helper.Idbarra2);
                    if (!dr.IsDBNull(iIdbarra2)) entity.Idbarra2 = dr.GetString(iIdbarra2); 
                    
                    int iIdbarra1 = dr.GetOrdinal(helper.Idbarra1);
                    if (!dr.IsDBNull(iIdbarra1)) entity.Idbarra1 = dr.GetString(iIdbarra1);
                    
                    int iBarrnombre = dr.GetOrdinal(helper.Barrnombre);
                    if (!dr.IsDBNull(iBarrnombre)) entity.Barrnombre = dr.GetString(iBarrnombre);  
                    
                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);       
                    
                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
