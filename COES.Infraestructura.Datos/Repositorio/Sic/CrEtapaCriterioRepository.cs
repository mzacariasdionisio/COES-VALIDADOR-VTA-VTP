using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Infraestructura.Datos.Helper.Sic;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    public class CrEtapaCriterioRepository : RepositoryBase, ICrEtapaCriterioRepository
    {
        public CrEtapaCriterioRepository(string strConn) : base(strConn)
        {
        }
        CrEtapaCriterioHelper helper = new CrEtapaCriterioHelper();
        public int Save(CrEtapaCriterioDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Cretapacricodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Cretapacodi, DbType.Int32, entity.CRETAPACODI);
            dbProvider.AddInParameter(command, helper.Crcriteriocodi, DbType.Int32, entity.CRCRITERIOCODI);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.LASTDATE);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.LASTUSER);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public List<CrEtapaCriterioDTO> ListaCriteriosEtapa(int cretapacricodi)
        {
            List<CrEtapaCriterioDTO> entitys = new List<CrEtapaCriterioDTO>();

            string query = string.Format(helper.ListaCriteriosEtapa, cretapacricodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CrEtapaCriterioDTO entity = helper.Create(dr);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public void Delete(int cretapacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);
            dbProvider.AddInParameter(command, helper.Cretapacodi, DbType.Int32, cretapacodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public List<CrEtapaCriterioDTO> ListaCriteriosEvento(int cretapacodi)
        {
            List<CrEtapaCriterioDTO> entitys = new List<CrEtapaCriterioDTO>();

            string query = string.Format(helper.SqlListaCriteriosEvento, cretapacodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CrEtapaCriterioDTO entity = helper.Create(dr);
                    int iCredescripcion = dr.GetOrdinal(helper.Credescripcion);
                    if (!dr.IsDBNull(iCredescripcion)) entity.CREDESCRIPCION = dr.GetString(iCredescripcion);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        
    }
}
