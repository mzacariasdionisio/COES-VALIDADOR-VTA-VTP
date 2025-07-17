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
    /// Clase de acceso a datos de la tabla VCE_LOG_CARGA_CAB
    /// </summary>
    public class VceLogCargaCabRepository: RepositoryBase, IVceLogCargaCabRepository
    {
        public VceLogCargaCabRepository(string strConn): base(strConn)
        {
        }

        VceLogCargaCabHelper helper = new VceLogCargaCabHelper();

        public int Save(VceLogCargaCabDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Crlcccodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Crlccorden, DbType.Int32, entity.Crlccorden);
            dbProvider.AddInParameter(command, helper.Crlccentidad, DbType.String, entity.Crlccentidad);
            dbProvider.AddInParameter(command, helper.Crlccnombtabla, DbType.String, entity.Crlccnombtabla);
            dbProvider.AddInParameter(command, helper.Pecacodi, DbType.Int32, entity.PecaCodi);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(VceLogCargaCabDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Crlccorden, DbType.Int32, entity.Crlccorden);
            dbProvider.AddInParameter(command, helper.Crlccentidad, DbType.String, entity.Crlccentidad);
            dbProvider.AddInParameter(command, helper.Crlccnombtabla, DbType.String, entity.Crlccnombtabla);
            dbProvider.AddInParameter(command, helper.Pecacodi, DbType.Int32, entity.PecaCodi);
            dbProvider.AddInParameter(command, helper.Crlcccodi, DbType.Int32, entity.Crlcccodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int crlcccodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Crlcccodi, DbType.Int32, crlcccodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteCabPeriodo(int pecacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteCabPeriodo);

            dbProvider.AddInParameter(command, helper.Crlcccodi, DbType.Int32, pecacodi);

            dbProvider.ExecuteNonQuery(command);
        }


        public VceLogCargaCabDTO GetById(int crlcccodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Crlcccodi, DbType.Int32, crlcccodi);
            VceLogCargaCabDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<VceLogCargaCabDTO> List()
        {
            List<VceLogCargaCabDTO> entitys = new List<VceLogCargaCabDTO>();
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

        public List<VceLogCargaCabDTO> GetByCriteria()
        {
            List<VceLogCargaCabDTO> entitys = new List<VceLogCargaCabDTO>();
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

        //- conpensaciones.JDEL - Inicio 03/01/2017: Cambio para atender el requerimiento.

        public void Init(int pecacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            string queryString = string.Format(helper.SqlInit, pecacodi, id, id + 1, id + 2, id + 3);
            command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.ExecuteNonQuery(command);
        }

        //- JDEL Fin
        public int GetMinIdByVersion(int pecacodi, string nombTabla)
        {
            int newId = 0;
            string queryString = string.Format(helper.SqlGetMinIdByVersion, pecacodi, nombTabla);

            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            newId = Int32.Parse(dbProvider.ExecuteScalar(command).ToString());

            return newId;
        }
        
    }
}
