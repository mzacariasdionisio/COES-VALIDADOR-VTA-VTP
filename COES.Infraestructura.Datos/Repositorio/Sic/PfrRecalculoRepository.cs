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
    /// Clase de acceso a datos de la tabla PFR_RECALCULO
    /// </summary>
    public class PfrRecalculoRepository: RepositoryBase, IPfrRecalculoRepository
    {
        public PfrRecalculoRepository(string strConn): base(strConn)
        {
        }

        PfrRecalculoHelper helper = new PfrRecalculoHelper();

        public int Save(PfrRecalculoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);
            
            command = dbProvider.GetSqlStringCommand(helper.SqlSave);            

            dbProvider.AddInParameter(command, helper.Pfrreccodi, DbType.Int32, id);
            
            dbProvider.AddInParameter(command, helper.Pfrpercodi, DbType.Int32, entity.Pfrpercodi);
            dbProvider.AddInParameter(command, helper.Pfrrecnombre, DbType.String, entity.Pfrrecnombre);
            dbProvider.AddInParameter(command, helper.Pfrrecdescripcion, DbType.String, entity.Pfrrecdescripcion);
            dbProvider.AddInParameter(command, helper.Pfrrecinforme, DbType.String, entity.Pfrrecinforme);
            dbProvider.AddInParameter(command, helper.Pfrrectipo, DbType.String, entity.Pfrrectipo);
            dbProvider.AddInParameter(command, helper.Pfrrecfechalimite, DbType.DateTime, entity.Pfrrecfechalimite);
            dbProvider.AddInParameter(command, helper.Pfrrecusucreacion, DbType.String, entity.Pfrrecusucreacion);
            dbProvider.AddInParameter(command, helper.Pfrrecfeccreacion, DbType.DateTime, entity.Pfrrecfeccreacion);
            dbProvider.AddInParameter(command, helper.Pfrrecusumodificacion, DbType.String, entity.Pfrrecusumodificacion);
            dbProvider.AddInParameter(command, helper.Pfrrecfecmodificacion, DbType.DateTime, entity.Pfrrecfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(PfrRecalculoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            
            dbProvider.AddInParameter(command, helper.Pfrpercodi, DbType.Int32, entity.Pfrpercodi);            
            dbProvider.AddInParameter(command, helper.Pfrrecnombre, DbType.String, entity.Pfrrecnombre);
            dbProvider.AddInParameter(command, helper.Pfrrecdescripcion, DbType.String, entity.Pfrrecdescripcion);
            dbProvider.AddInParameter(command, helper.Pfrrecinforme, DbType.String, entity.Pfrrecinforme);
            dbProvider.AddInParameter(command, helper.Pfrrectipo, DbType.String, entity.Pfrrectipo);
            dbProvider.AddInParameter(command, helper.Pfrrecfechalimite, DbType.DateTime, entity.Pfrrecfechalimite);
            dbProvider.AddInParameter(command, helper.Pfrrecusucreacion, DbType.String, entity.Pfrrecusucreacion);
            dbProvider.AddInParameter(command, helper.Pfrrecfeccreacion, DbType.DateTime, entity.Pfrrecfeccreacion);
            dbProvider.AddInParameter(command, helper.Pfrrecusumodificacion, DbType.String, entity.Pfrrecusumodificacion);
            dbProvider.AddInParameter(command, helper.Pfrrecfecmodificacion, DbType.DateTime, entity.Pfrrecfecmodificacion);
            dbProvider.AddInParameter(command, helper.Pfrreccodi, DbType.Int32, entity.Pfrreccodi);

            

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int pfrreccodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Pfrreccodi, DbType.Int32, pfrreccodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public PfrRecalculoDTO GetById(int pfrreccodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Pfrreccodi, DbType.Int32, pfrreccodi);
            PfrRecalculoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<PfrRecalculoDTO> List()
        {
            List<PfrRecalculoDTO> entitys = new List<PfrRecalculoDTO>();
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

        public List<PfrRecalculoDTO> GetByCriteria(int pfrpercodi)
        {
            List<PfrRecalculoDTO> entitys = new List<PfrRecalculoDTO>();

            string query = string.Format(helper.SqlGetByCriteria, pfrpercodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

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
