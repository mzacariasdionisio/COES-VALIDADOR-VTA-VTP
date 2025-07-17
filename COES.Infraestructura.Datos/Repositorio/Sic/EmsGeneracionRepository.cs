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
    /// Clase de acceso a datos de la tabla EMS_GENERACION
    /// </summary>
    public class EmsGeneracionRepository: RepositoryBase, IEmsGeneracionRepository
    {
        public EmsGeneracionRepository(string strConn): base(strConn)
        {
        }

        EmsGeneracionHelper helper = new EmsGeneracionHelper();

        public int Save(EmsGeneracionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Emggencodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Emsgenfecha, DbType.DateTime, entity.Emsgenfecha);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Emsgenoperativo, DbType.String, entity.Emsgenoperativo);
            dbProvider.AddInParameter(command, helper.Emsgenvalor, DbType.Decimal, entity.Emsgenvalor);
            dbProvider.AddInParameter(command, helper.Emsgenpotmax, DbType.Decimal, entity.Emsgenpotmax);
            dbProvider.AddInParameter(command, helper.Emsgenusucreacion, DbType.String, entity.Emsgenusucreacion);
            dbProvider.AddInParameter(command, helper.Emsgenfeccreacion, DbType.DateTime, entity.Emsgenfeccreacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(EmsGeneracionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Emsgenfecha, DbType.DateTime, entity.Emsgenfecha);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Emsgenoperativo, DbType.String, entity.Emsgenoperativo);
            dbProvider.AddInParameter(command, helper.Emsgenvalor, DbType.Decimal, entity.Emsgenvalor);
            dbProvider.AddInParameter(command, helper.Emsgenusucreacion, DbType.String, entity.Emsgenusucreacion);
            dbProvider.AddInParameter(command, helper.Emsgenfeccreacion, DbType.DateTime, entity.Emsgenfeccreacion);
            dbProvider.AddInParameter(command, helper.Emggencodi, DbType.Int32, entity.Emggencodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int emggencodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Emggencodi, DbType.Int32, emggencodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public EmsGeneracionDTO GetById(int emggencodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Emggencodi, DbType.Int32, emggencodi);
            EmsGeneracionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<EmsGeneracionDTO> List()
        {
            List<EmsGeneracionDTO> entitys = new List<EmsGeneracionDTO>();
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

        public List<EmsGeneracionDTO> GetByCriteria()
        {
            List<EmsGeneracionDTO> entitys = new List<EmsGeneracionDTO>();
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

        public List<EmsGeneracionDTO> ObtenerDatosSupervisionDemanda(DateTime fecha)
        {
            List<EmsGeneracionDTO> entitys = new List<EmsGeneracionDTO>();
            string sql = string.Format(helper.SqlObtenerSupervisionDemanda, fecha.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EmsGeneracionDTO entity = new EmsGeneracionDTO();

                    //int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    //if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    //int iEmsgenoperativo = dr.GetOrdinal(helper.Emsgenoperativo);
                    //if (!dr.IsDBNull(iEmsgenoperativo)) entity.Emsgenoperativo = dr.GetString(iEmsgenoperativo);

                    int iEmsgenvalor = dr.GetOrdinal(helper.Emsgenvalor);
                    if (!dr.IsDBNull(iEmsgenvalor)) entity.Emsgenvalor = dr.GetDecimal(iEmsgenvalor);

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iIndice = dr.GetOrdinal(helper.Indice);
                    if (!dr.IsDBNull(iIndice)) entity.Indice = Convert.ToInt32(dr.GetValue(iIndice));

                    int iTgenercodi = dr.GetOrdinal(helper.Tgenercodi);
                    if (!dr.IsDBNull(iTgenercodi)) entity.Tgenercodi = Convert.ToInt32(dr.GetValue(iTgenercodi));

                    if (entity.Tgenercodi == 3)
                    {
                        if (entity.Emsgenvalor < 0)
                        {
                            entity.Emsgenvalor = 0;
                        }
                    }

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
