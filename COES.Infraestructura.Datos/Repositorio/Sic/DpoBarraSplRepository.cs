using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Sic;
using System.Data;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    public class DpoBarraSplRepository : RepositoryBase, IDpoBarraSplRepository
    {
        public DpoBarraSplRepository(string strConn) : base(strConn)
        {
        }

        DpoBarraSplHelper helper = new DpoBarraSplHelper();

        public void Save(DpoBarraSplDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Barsplcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Gruponomb, DbType.String, entity.Gruponomb);
            dbProvider.AddInParameter(command, helper.Grupoabrev, DbType.String, entity.Grupoabrev);
            dbProvider.AddInParameter(command, helper.Barsplestado, DbType.String, entity.Barsplestado);
            dbProvider.AddInParameter(command, helper.Barsplusucreacion, DbType.String, entity.Barsplusucreacion);
            dbProvider.AddInParameter(command, helper.Barsplfeccreacion, DbType.DateTime, entity.Barsplfeccreacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(DpoBarraSplDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Barsplcodi, DbType.Int32, entity.Barsplcodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Gruponomb, DbType.String, entity.Gruponomb);
            dbProvider.AddInParameter(command, helper.Grupoabrev, DbType.String, entity.Grupoabrev);
            dbProvider.AddInParameter(command, helper.Barsplestado, DbType.String, entity.Barsplestado);
            dbProvider.AddInParameter(command, helper.Barsplusucreacion, DbType.String, entity.Barsplusucreacion);
            dbProvider.AddInParameter(command, helper.Barsplfeccreacion, DbType.DateTime, entity.Barsplfeccreacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int codi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Barsplcodi, DbType.Int32, codi);

            dbProvider.ExecuteNonQuery(command);
        }

        public List<DpoBarraSplDTO> List()
        {
            List<DpoBarraSplDTO> entitys = new List<DpoBarraSplDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    DpoBarraSplDTO entity = new DpoBarraSplDTO();

                    int iBarsplcodi = dr.GetOrdinal(helper.Barsplcodi);
                    if (!dr.IsDBNull(iBarsplcodi)) entity.Barsplcodi = Convert.ToInt32(dr.GetValue(iBarsplcodi));

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    int iGrupoabrev = dr.GetOrdinal(helper.Grupoabrev);
                    if (!dr.IsDBNull(iGrupoabrev)) entity.Grupoabrev = dr.GetString(iGrupoabrev);

                    int iBarsplestado = dr.GetOrdinal(helper.Barsplestado);
                    if (!dr.IsDBNull(iBarsplestado)) entity.Barsplestado = dr.GetString(iBarsplestado);

                    int iBarsplusucreacion = dr.GetOrdinal(helper.Barsplusucreacion);
                    if (!dr.IsDBNull(iBarsplusucreacion)) entity.Barsplusucreacion = dr.GetString(iBarsplusucreacion);

                    int iBarsplfeccreacion = dr.GetOrdinal(helper.Barsplfeccreacion);
                    if (!dr.IsDBNull(iBarsplfeccreacion)) entity.Barsplfeccreacion = dr.GetDateTime(iBarsplfeccreacion);


                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public DpoBarraSplDTO GetById(int codi)
        {
            DpoBarraSplDTO entity = new DpoBarraSplDTO();

            string query = string.Format(helper.SqlGetById, codi);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public void UpdateDpoBarraSPlEstado(string ids, string estado)
        {
            string query = string.Format(helper.SqlUpdateEstado, ids, estado);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            dbProvider.ExecuteNonQuery(command);
        }

        public List<DpoBarraSplDTO> ListBarrasSPLByGrupo(string barras)
        {
            List<DpoBarraSplDTO> entitys = new List<DpoBarraSplDTO>();
            string query = string.Format(helper.SqlListBarrasSPLByGrupo, barras);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    DpoBarraSplDTO entity = new DpoBarraSplDTO();

                    int iBarsplcodi = dr.GetOrdinal(helper.Barsplcodi);
                    if (!dr.IsDBNull(iBarsplcodi)) entity.Barsplcodi = Convert.ToInt32(dr.GetValue(iBarsplcodi));

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    int iGrupoabrev = dr.GetOrdinal(helper.Grupoabrev);
                    if (!dr.IsDBNull(iGrupoabrev)) entity.Grupoabrev = dr.GetString(iGrupoabrev);

                    int iBarsplestado = dr.GetOrdinal(helper.Barsplestado);
                    if (!dr.IsDBNull(iBarsplestado)) entity.Barsplestado = dr.GetString(iBarsplestado);

                    int iBarsplusucreacion = dr.GetOrdinal(helper.Barsplusucreacion);
                    if (!dr.IsDBNull(iBarsplusucreacion)) entity.Barsplusucreacion = dr.GetString(iBarsplusucreacion);

                    int iBarsplfeccreacion = dr.GetOrdinal(helper.Barsplfeccreacion);
                    if (!dr.IsDBNull(iBarsplfeccreacion)) entity.Barsplfeccreacion = dr.GetDateTime(iBarsplfeccreacion);


                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
