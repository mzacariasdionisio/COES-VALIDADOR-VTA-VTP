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
    /// Clase de acceso a datos de la tabla EVE_HORAOPERACION_EQUIPO
    /// </summary>
    public class EveHoraoperacionEquipoRepository: RepositoryBase, IEveHoraoperacionEquipoRepository
    {
        public EveHoraoperacionEquipoRepository(string strConn): base(strConn)
        {
        }

        EveHoraoperacionEquipoHelper helper = new EveHoraoperacionEquipoHelper();

        public int Save(EveHoraoperacionEquipoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Hopequcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Hopcodi, DbType.Int32, entity.Hopcodi);
            dbProvider.AddInParameter(command, helper.Configcodi, DbType.Int32, entity.Configcodi);
            dbProvider.AddInParameter(command, helper.Grulincodi, DbType.Int32, entity.Grulincodi);
            dbProvider.AddInParameter(command, helper.Regsegcodi, DbType.Int32, entity.Regsegcodi);
            dbProvider.AddInParameter(command, helper.Hopequusucreacion, DbType.String, entity.Hopequusucreacion);
            dbProvider.AddInParameter(command, helper.Hopequfeccreacion, DbType.DateTime, entity.Hopequfeccreacion);
            dbProvider.AddInParameter(command, helper.Hopequusumodificacion, DbType.String, entity.Hopequusumodificacion);
            dbProvider.AddInParameter(command, helper.Hopequfecmodificacion, DbType.DateTime, entity.Hopequfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(EveHoraoperacionEquipoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Hopequcodi, DbType.Int32, entity.Hopequcodi);
            dbProvider.AddInParameter(command, helper.Hopcodi, DbType.Int32, entity.Hopcodi);
            dbProvider.AddInParameter(command, helper.Configcodi, DbType.Int32, entity.Configcodi);
            dbProvider.AddInParameter(command, helper.Grulincodi, DbType.Int32, entity.Grulincodi);
            dbProvider.AddInParameter(command, helper.Regsegcodi, DbType.Int32, entity.Regsegcodi);
            dbProvider.AddInParameter(command, helper.Hopequusucreacion, DbType.String, entity.Hopequusucreacion);
            dbProvider.AddInParameter(command, helper.Hopequfeccreacion, DbType.DateTime, entity.Hopequfeccreacion);
            dbProvider.AddInParameter(command, helper.Hopequusumodificacion, DbType.String, entity.Hopequusumodificacion);
            dbProvider.AddInParameter(command, helper.Hopequfecmodificacion, DbType.DateTime, entity.Hopequfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int hopequcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Hopequcodi, DbType.Int32, hopequcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public EveHoraoperacionEquipoDTO GetById(int hopequcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Hopequcodi, DbType.Int32, hopequcodi);
            EveHoraoperacionEquipoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<EveHoraoperacionEquipoDTO> List()
        {
            List<EveHoraoperacionEquipoDTO> entitys = new List<EveHoraoperacionEquipoDTO>();
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

        public List<EveHoraoperacionEquipoDTO> GetByCriteria()
        {
            List<EveHoraoperacionEquipoDTO> entitys = new List<EveHoraoperacionEquipoDTO>();
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

        public List<EveHoraoperacionEquipoDTO> ObtenerEquiposInvolucrados(string lsthopcodis)
        {
            var entitys = new List<EveHoraoperacionEquipoDTO>();
            string query = string.Format(helper.SqlListarEquiposInv, lsthopcodis);
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
