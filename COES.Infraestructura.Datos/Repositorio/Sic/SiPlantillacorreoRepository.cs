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
    /// Clase de acceso a datos de la tabla SI_PLANTILLACORREO
    /// </summary>
    public class SiPlantillacorreoRepository: RepositoryBase, ISiPlantillacorreoRepository
    {
        public SiPlantillacorreoRepository(string strConn): base(strConn)
        {
        }

        SiPlantillacorreoHelper helper = new SiPlantillacorreoHelper();

        public int Save(SiPlantillacorreoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Plantcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Plantcontenido, DbType.String, entity.Plantcontenido);
            dbProvider.AddInParameter(command, helper.Modcodi, DbType.Int32, entity.Modcodi);
            dbProvider.AddInParameter(command, helper.Tpcorrcodi, DbType.Int32, entity.Tpcorrcodi);
            dbProvider.AddInParameter(command, helper.Plantasunto, DbType.String, entity.Plantasunto);
            dbProvider.AddInParameter(command, helper.Plantnomb, DbType.String, entity.Plantnomb);
            dbProvider.AddInParameter(command, helper.Plantindhtml, DbType.String, entity.Plantindhtml);
            dbProvider.AddInParameter(command, helper.Plantindadjunto, DbType.String, entity.Plantindadjunto);
            dbProvider.AddInParameter(command, helper.Planticorreos, DbType.String, entity.Planticorreos);
            dbProvider.AddInParameter(command, helper.PlanticorreosCc, DbType.String, entity.PlanticorreosCc);
            dbProvider.AddInParameter(command, helper.PlanticorreosBcc, DbType.String, entity.PlanticorreosBcc);
            dbProvider.AddInParameter(command, helper.PlanticorreoFrom, DbType.String, entity.PlanticorreoFrom);
            dbProvider.AddInParameter(command, helper.Plantlinkadjunto, DbType.String, entity.Plantlinkadjunto);
            dbProvider.AddInParameter(command, helper.Plantfeccreacion, DbType.DateTime, entity.Plantfeccreacion);
            dbProvider.AddInParameter(command, helper.Plantfecmodificacion, DbType.DateTime, entity.Plantfecmodificacion);
            dbProvider.AddInParameter(command, helper.Plantusucreacion, DbType.String, entity.Plantusucreacion);
            dbProvider.AddInParameter(command, helper.Plantusumodificacion, DbType.String, entity.Plantusumodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(SiPlantillacorreoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            
            dbProvider.AddInParameter(command, helper.Plantcontenido, DbType.String, entity.Plantcontenido);
            dbProvider.AddInParameter(command, helper.Modcodi, DbType.Int32, entity.Modcodi);
            dbProvider.AddInParameter(command, helper.Tpcorrcodi, DbType.Int32, entity.Tpcorrcodi);
            dbProvider.AddInParameter(command, helper.Plantasunto, DbType.String, entity.Plantasunto);
            dbProvider.AddInParameter(command, helper.Plantnomb, DbType.String, entity.Plantnomb);
            dbProvider.AddInParameter(command, helper.Plantindhtml, DbType.String, entity.Plantindhtml);
            dbProvider.AddInParameter(command, helper.Plantindadjunto, DbType.String, entity.Plantindadjunto);
            dbProvider.AddInParameter(command, helper.Planticorreos, DbType.String, entity.Planticorreos);
            dbProvider.AddInParameter(command, helper.PlanticorreosCc, DbType.String, entity.PlanticorreosCc);
            dbProvider.AddInParameter(command, helper.PlanticorreosBcc, DbType.String, entity.PlanticorreosBcc);
            dbProvider.AddInParameter(command, helper.PlanticorreoFrom, DbType.String, entity.PlanticorreoFrom);
            dbProvider.AddInParameter(command, helper.Plantlinkadjunto, DbType.String, entity.Plantlinkadjunto);
            dbProvider.AddInParameter(command, helper.Plantfeccreacion, DbType.DateTime, entity.Plantfeccreacion);
            dbProvider.AddInParameter(command, helper.Plantfecmodificacion, DbType.DateTime, entity.Plantfecmodificacion);
            dbProvider.AddInParameter(command, helper.Plantusucreacion, DbType.String, entity.Plantusucreacion);
            dbProvider.AddInParameter(command, helper.Plantusumodificacion, DbType.String, entity.Plantusumodificacion);
            dbProvider.AddInParameter(command, helper.Plantcodi, DbType.Int32, entity.Plantcodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public void ActualizarPlantilla(SiPlantillacorreoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlActualizarPlantilla);

            dbProvider.AddInParameter(command, helper.Plantasunto, DbType.String, entity.Plantasunto);
            dbProvider.AddInParameter(command, helper.Plantcontenido, DbType.String, entity.Plantcontenido);           
            dbProvider.AddInParameter(command, helper.Plantcodi, DbType.Int32, entity.Plantcodi);

            dbProvider.ExecuteNonQuery(command);
        }
             

        public void Delete(int plantcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Plantcodi, DbType.Int32, plantcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public SiPlantillacorreoDTO GetById(int plantcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Plantcodi, DbType.Int32, plantcodi);
            SiPlantillacorreoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SiPlantillacorreoDTO> List()
        {
            List<SiPlantillacorreoDTO> entitys = new List<SiPlantillacorreoDTO>();
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

        public List<SiPlantillacorreoDTO> GetByCriteria()
        {
            List<SiPlantillacorreoDTO> entitys = new List<SiPlantillacorreoDTO>();
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

        public SiPlantillacorreoDTO ObtenerPlantillaPorModulo(int idTipoPlantilla, int idModulo)
        {
            SiPlantillacorreoDTO entity = null;
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerPlantillaPorModulo);

            dbProvider.AddInParameter(command, helper.Tpcorrcodi, DbType.Int32, idTipoPlantilla);
            dbProvider.AddInParameter(command, helper.Modcodi, DbType.Int32, idModulo);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SiPlantillacorreoDTO> ListarPlantillas(string plantillaCodis)
        {
            List<SiPlantillacorreoDTO> entitys = new List<SiPlantillacorreoDTO>();
            string query = string.Format(helper.SqlListarPlantillas, plantillaCodis);
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
