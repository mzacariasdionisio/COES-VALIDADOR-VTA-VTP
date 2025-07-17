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
    public class EveAnalisisEventoRepository : RepositoryBase, IEveAnalisisEventoRepository
    {
        public EveAnalisisEventoRepository(string strConn) : base(strConn)
        {
        }
        EveAnalisisEventoHelper helper = new EveAnalisisEventoHelper();
        public void Delete(int evencodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);
            dbProvider.AddInParameter(command, helper.Evencodi, DbType.Int32, evencodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public List<EveAnalisisEventoDTO> List(int evencodi)
        {
            List<EveAnalisisEventoDTO> entitys = new List<EveAnalisisEventoDTO>();
            String query = String.Format(helper.SqlList, evencodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EveAnalisisEventoDTO entity = helper.Create(dr);
                    int iEvetipnumdescripcion = dr.GetOrdinal(helper.Evetipnumdescripcion);
                    if (!dr.IsDBNull(iEvetipnumdescripcion)) entity.EVETIPNUMDESCRIPCION = dr.GetString(iEvetipnumdescripcion);

                    int iEveanaPosicion = dr.GetOrdinal(helper.EveanaPosicion);
                    if (!dr.IsDBNull(iEveanaPosicion)) entity.EVEANAPOSICION = dr.GetInt32(iEveanaPosicion);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public int Save(EveAnalisisEventoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Eveanaevecodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Evencodi, DbType.Int32, entity.EVENCODI);
            dbProvider.AddInParameter(command, helper.Eveanaevedescnumeral, DbType.String, entity.EVEANAEVEDESCNUMERAL);
            dbProvider.AddInParameter(command, helper.Eveanaevedescfigura, DbType.String, entity.EVEANAEVEDESCFIGURA);
            dbProvider.AddInParameter(command, helper.Eveanaeverutafigura, DbType.String, entity.EVEANAEVERUTAFIGURA);
            dbProvider.AddInParameter(command, helper.Evenumcodi, DbType.Int32, entity.EVENUMCODI);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.LASTDATE);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.LASTUSER);
            dbProvider.AddInParameter(command, helper.EveanaHora, DbType.String, entity.EVEANAHORA);
            dbProvider.AddInParameter(command, helper.Eveanatipo, DbType.Int32, entity.EVEANATIPO);
            dbProvider.AddInParameter(command, helper.EveanaPosicion, DbType.Int32, entity.EVEANAPOSICION);
            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(EveAnalisisEventoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);       
            dbProvider.AddInParameter(command, helper.Eveanaeverutafigura, DbType.String, entity.EVEANAEVERUTAFIGURA);
            dbProvider.AddInParameter(command, helper.EveanaPosicion, DbType.Int32, entity.EVEANAPOSICION);
            dbProvider.AddInParameter(command, helper.Eveanaevecodi, DbType.Int32, entity.EVEANAEVECODI);

            dbProvider.ExecuteNonQuery(command);

        }

        public EveAnalisisEventoDTO GetById(int eveanaevecodi)
        {
            String query = String.Format(helper.SqlGetById, eveanaevecodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            EveAnalisisEventoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public void UpdateDescripcion(EveAnalisisEventoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdateDescripcion);
            dbProvider.AddInParameter(command, helper.Eveanaevedescfigura, DbType.String, entity.EVEANAEVEDESCFIGURA);
            dbProvider.AddInParameter(command, helper.Eveanaevedescnumeral, DbType.String, entity.EVEANAEVEDESCNUMERAL);
            dbProvider.AddInParameter(command, helper.Evenumcodi, DbType.Int32, entity.EVENUMCODI);
            dbProvider.AddInParameter(command, helper.EveanaPosicion, DbType.Int32, entity.EVEANAPOSICION);
            dbProvider.AddInParameter(command, helper.Eveanaevecodi, DbType.Int32, entity.EVEANAEVECODI);

            dbProvider.ExecuteNonQuery(command);
        }

        public bool ValidarTipoNumeralxAnalisisEvento(int evetipnumcodi)
        {
            bool respuesta = false;
            int result;
            string query = string.Format(helper.SqlValidarTipoNumeralxAnalisisEvento, evetipnumcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            result = dbProvider.ExecuteNonQuery(command);
            if (result > 0)
                respuesta = true;

            return respuesta;
        }

        public void DeleteAnalisis(int eveanaevecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteAnalisis);
            dbProvider.AddInParameter(command, helper.Eveanaevecodi, DbType.Int32, eveanaevecodi);
            dbProvider.ExecuteNonQuery(command);
        }
    }
}
