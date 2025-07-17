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
    /// Clase de acceso a datos de la tabla EVE_SUBEVENTOS
    /// </summary>
    public class EveSubeventosRepository : RepositoryBase, IEveSubeventosRepository
    {
        public EveSubeventosRepository(string strConn)
            : base(strConn)
        {
        }

        EveSubeventosHelper helper = new EveSubeventosHelper();

        public void Save(EveSubeventosDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Evencodi, DbType.Int32, entity.Evencodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Subevedescrip, DbType.String, entity.Subevedescrip);
            dbProvider.AddInParameter(command, helper.Subevenfin, DbType.DateTime, entity.Subevenfin);
            dbProvider.AddInParameter(command, helper.Subevenini, DbType.DateTime, entity.Subevenini);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(EveSubeventosDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Evencodi, DbType.Int32, entity.Evencodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Subevedescrip, DbType.String, entity.Subevedescrip);
            dbProvider.AddInParameter(command, helper.Subevenfin, DbType.DateTime, entity.Subevenfin);
            dbProvider.AddInParameter(command, helper.Subevenini, DbType.DateTime, entity.Subevenini);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int evencodi, int equicodi, DateTime subevenini)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Evencodi, DbType.Int32, evencodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, equicodi);
            dbProvider.AddInParameter(command, helper.Subevenini, DbType.DateTime, subevenini);

            dbProvider.ExecuteNonQuery(command);
        }

        public EveSubeventosDTO GetById(int evencodi, int equicodi, DateTime subevenini)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Evencodi, DbType.Int32, evencodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, equicodi);
            dbProvider.AddInParameter(command, helper.Subevenini, DbType.DateTime, subevenini);
            EveSubeventosDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<EveSubeventosDTO> List()
        {
            List<EveSubeventosDTO> entitys = new List<EveSubeventosDTO>();
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

        public List<EveSubeventosDTO> GetByCriteria(int idEvento)
        {
            List<EveSubeventosDTO> entitys = new List<EveSubeventosDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, helper.Evencodi, DbType.Int32, idEvento);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EveSubeventosDTO entity = helper.Create(dr);

                    int iEquiAbrev = dr.GetOrdinal(helper.EquiAbrev);
                    if (!dr.IsDBNull(iEquiAbrev)) entity.EquiAbrev = dr.GetString(iEquiAbrev);

                    int iEmprNomb = dr.GetOrdinal(helper.EmprNomb);
                    if (!dr.IsDBNull(iEmprNomb)) entity.EmprNomb = dr.GetString(iEmprNomb);

                    int iAreaNomb = dr.GetOrdinal(helper.AreaNomb);
                    if (!dr.IsDBNull(iAreaNomb)) entity.AreaNomb = dr.GetString(iAreaNomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
