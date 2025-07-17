using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Infraestructura.Datos.Helper.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla EVE_GPSAISLADO
    /// </summary>
    public class EveGpsaisladoRepository : RepositoryBase, IEveGpsaisladoRepository
    {
        public EveGpsaisladoRepository(string strConn) : base(strConn)
        {
        }

        EveGpsaisladoHelper helper = new EveGpsaisladoHelper();

        public int Save(EveGpsaisladoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Gpsaisfeccreacion, DbType.DateTime, entity.Gpsaisfeccreacion);
            dbProvider.AddInParameter(command, helper.Gpsaisusucreacion, DbType.String, entity.Gpsaisusucreacion);
            dbProvider.AddInParameter(command, helper.Gpsaisprincipal, DbType.Int32, entity.Gpsaisprincipal);
            dbProvider.AddInParameter(command, helper.Gpscodi, DbType.Int32, entity.Gpscodi);
            dbProvider.AddInParameter(command, helper.Iccodi, DbType.Int32, entity.Iccodi);
            dbProvider.AddInParameter(command, helper.Gpsaiscodi, DbType.Int32, id);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(EveGpsaisladoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Gpsaisfeccreacion, DbType.DateTime, entity.Gpsaisfeccreacion);
            dbProvider.AddInParameter(command, helper.Gpsaisusucreacion, DbType.String, entity.Gpsaisusucreacion);
            dbProvider.AddInParameter(command, helper.Gpsaisprincipal, DbType.Int32, entity.Gpsaisprincipal);
            dbProvider.AddInParameter(command, helper.Gpscodi, DbType.Int32, entity.Gpscodi);
            dbProvider.AddInParameter(command, helper.Iccodi, DbType.Int32, entity.Iccodi);
            dbProvider.AddInParameter(command, helper.Gpsaiscodi, DbType.Int32, entity.Gpsaiscodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int gpsaiscodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Gpsaiscodi, DbType.Int32, gpsaiscodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteByIccodi(int iccodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteByIccodi);

            dbProvider.AddInParameter(command, helper.Iccodi, DbType.Int32, iccodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public EveGpsaisladoDTO GetById(int gpsaiscodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Gpsaiscodi, DbType.Int32, gpsaiscodi);
            EveGpsaisladoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<EveGpsaisladoDTO> List()
        {
            List<EveGpsaisladoDTO> entitys = new List<EveGpsaisladoDTO>();
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

        public List<EveGpsaisladoDTO> GetByCriteria(int iccodi)
        {
            List<EveGpsaisladoDTO> entitys = new List<EveGpsaisladoDTO>();
            string query = string.Format(helper.SqlGetByCriteria, iccodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EveGpsaisladoDTO entity = helper.Create(dr);

                    int iNombre = dr.GetOrdinal(this.helper.Gpsnombre);
                    if (!dr.IsDBNull(iNombre)) entity.Gpsnombre = dr.GetString(iNombre);

                    int iGpsosinerg = dr.GetOrdinal(this.helper.Gpsosinerg);
                    if (!dr.IsDBNull(iGpsosinerg)) entity.Gpsosinerg = dr.GetString(iGpsosinerg);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
