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
    /// Clase de acceso a datos de la tabla EVE_OBSERVACION
    /// </summary>
    public class EveObservacionRepository: RepositoryBase, IEveObservacionRepository
    {
        public EveObservacionRepository(string strConn): base(strConn)
        {
        }

        EveObservacionHelper helper = new EveObservacionHelper();

        public int Save(EveObservacionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Obscodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Subcausacodi, DbType.Int32, entity.Subcausacodi);
            dbProvider.AddInParameter(command, helper.Obsfecha, DbType.DateTime, entity.Obsfecha);
            dbProvider.AddInParameter(command, helper.Obsdescrip, DbType.String, entity.Obsdescrip);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Evenclasecodi, DbType.Int32, entity.Evenclasecodi);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(EveObservacionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Obscodi, DbType.Int32, entity.Obscodi);
            dbProvider.AddInParameter(command, helper.Subcausacodi, DbType.Int32, entity.Subcausacodi);
            dbProvider.AddInParameter(command, helper.Obsfecha, DbType.DateTime, entity.Obsfecha);
            dbProvider.AddInParameter(command, helper.Obsdescrip, DbType.String, entity.Obsdescrip);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Evenclasecodi, DbType.Int32, entity.Evenclasecodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int obscodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Obscodi, DbType.Int32, obscodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public EveObservacionDTO GetById(int obscodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Obscodi, DbType.Int32, obscodi);
            EveObservacionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<EveObservacionDTO> List()
        {
            List<EveObservacionDTO> entitys = new List<EveObservacionDTO>();
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

        public List<EveObservacionDTO> GetByCriteria(DateTime fecIni, string subcausacodi, string evenclasecodi)
        {
            List<EveObservacionDTO> entitys = new List<EveObservacionDTO>();
            string query = string.Format(helper.SqlGetByCriteria, fecIni.ToString(ConstantesBase.FormatoFecha), subcausacodi, evenclasecodi);
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
