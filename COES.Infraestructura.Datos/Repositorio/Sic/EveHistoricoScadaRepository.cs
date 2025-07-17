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
    public class EveHistoricoScadaRepository : RepositoryBase, IEveHistoricoScadaRepository
    {
        public EveHistoricoScadaRepository(string strConn) : base(strConn)
        {
        }
        EveHistoricoScadaHelper helper = new EveHistoricoScadaHelper();

        public void Delete(EveHistoricoScadaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);
            dbProvider.AddInParameter(command, helper.Evencodi, DbType.Int32, entity.EVENCODI);
            dbProvider.AddInParameter(command, helper.Evehistscdazonacodi, DbType.Int32, entity.EVEHISTSCDAZONACODI);
            dbProvider.AddInParameter(command, helper.Evehistscdacanalcodi, DbType.Int32, entity.EVEHISTSCDACANALCODI);
            dbProvider.ExecuteNonQuery(command);
        }

        public List<EveHistoricoScadaDTO> List(int evencodi)
        {
            List<EveHistoricoScadaDTO> entitys = new List<EveHistoricoScadaDTO>();
            String query = String.Format(helper.SqlList, evencodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EveHistoricoScadaDTO entity = helper.Create(dr);
                    int iZonaabrev = dr.GetOrdinal(helper.Zonaabrev);
                    if (!dr.IsDBNull(iZonaabrev)) entity.ZONAABREV = dr.GetString(iZonaabrev);
                    int iCanalnomb = dr.GetOrdinal(helper.Canalnomb);
                    if (!dr.IsDBNull(iCanalnomb)) entity.CANALNOMB = dr.GetString(iCanalnomb);

                    entitys.Add(entity);                  
                }
            }
            return entitys;
        }

        public int Save(EveHistoricoScadaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Evehistscdacodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Evencodi, DbType.Int32, entity.EVENCODI);
            dbProvider.AddInParameter(command, helper.Evehistscdazonacodi, DbType.Int32, entity.EVEHISTSCDAZONACODI);
            dbProvider.AddInParameter(command, helper.Evehistscdacanalcodi, DbType.Int32, entity.EVEHISTSCDACANALCODI);
            dbProvider.AddInParameter(command, helper.Evehistscdacodiequipo, DbType.String, entity.EVEHISTSCDACODIEQUIPO);
            dbProvider.AddInParameter(command, helper.Evehistscdafechdesconexion, DbType.String, entity.strEVEHISTSCDAFECHDESCONEXION);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.LASTDATE);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.LASTUSER);
            dbProvider.AddInParameter(command, helper.EVEHISTSCDAZONADESC, DbType.String, entity.ZONAABREV);
            dbProvider.AddInParameter(command, helper.EVEHISTSCDACANALDESC, DbType.String, entity.CANALNOMB);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public EveHistoricoScadaDTO GetById(int evehistscdacodi)
        {
            String query = String.Format(helper.SqlGetById, evehistscdacodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            EveHistoricoScadaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);                   
                }
            }

            return entity;
        }

        public void DeleteAll(int evencodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteAll);
            dbProvider.AddInParameter(command, helper.Evencodi, DbType.Int32, evencodi);
            dbProvider.ExecuteNonQuery(command);
        }
    }
}
