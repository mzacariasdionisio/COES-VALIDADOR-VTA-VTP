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
    /// Clase de acceso a datos de la tabla ME_REPORTE
    /// </summary>
    public class MeReporteRepository : RepositoryBase, IMeReporteRepository
    {
        public MeReporteRepository(string strConn)
            : base(strConn)
        {
        }

        MeReporteHelper helper = new MeReporteHelper();

        public void Update(MeReporteDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);
            dbProvider.AddInParameter(command, helper.Repornombre, DbType.String, entity.Repornombre);
            dbProvider.AddInParameter(command, helper.Repordescrip, DbType.String, entity.Repordescrip);
            dbProvider.AddInParameter(command, helper.Lectcodi, DbType.Int32, entity.Lectcodi);
            dbProvider.AddInParameter(command, helper.Reporusucreacion, DbType.String, entity.Reporusucreacion);
            dbProvider.AddInParameter(command, helper.Reporfeccreacion, DbType.DateTime, entity.Reporfeccreacion);
            dbProvider.AddInParameter(command, helper.Reporusumodificacion, DbType.String, entity.Reporusumodificacion);
            dbProvider.AddInParameter(command, helper.Reporfecmodificacion, DbType.DateTime, entity.Reporfecmodificacion);
            dbProvider.AddInParameter(command, helper.Modcodi, DbType.Int32, entity.Modcodi);
            dbProvider.AddInParameter(command, helper.Cabcodi, DbType.Int32, entity.Cabcodi);
            dbProvider.AddInParameter(command, helper.Areacode, DbType.Int32, entity.Areacode);
            dbProvider.AddInParameter(command, helper.Reporcheckempresa, DbType.Int32, entity.Reporcheckempresa);
            dbProvider.AddInParameter(command, helper.Reporcheckequipo, DbType.Int32, entity.Reporcheckequipo);
            dbProvider.AddInParameter(command, helper.Reporchecktipoequipo, DbType.Int32, entity.Reporchecktipoequipo);
            dbProvider.AddInParameter(command, helper.Reporchecktipomedida, DbType.Int32, entity.Reporchecktipomedida);
            dbProvider.AddInParameter(command, helper.Mrepcodi, DbType.Int32, entity.Mrepcodi);
            dbProvider.AddInParameter(command, helper.Reporejey, DbType.String, entity.Reporejey); 
            dbProvider.AddInParameter(command, helper.Reporesgrafico, DbType.String, entity.Reporesgrafico);
            dbProvider.AddInParameter(command, helper.Reporcodi, DbType.Int32, entity.Reporcodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public int Save(MeReporteDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);
            dbProvider.AddInParameter(command, helper.Reporcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Repornombre, DbType.String, entity.Repornombre);
            dbProvider.AddInParameter(command, helper.Repordescrip, DbType.String, entity.Repordescrip);
            dbProvider.AddInParameter(command, helper.Lectcodi, DbType.Int32, entity.Lectcodi);
            dbProvider.AddInParameter(command, helper.Reporusucreacion, DbType.String, entity.Reporusucreacion);
            dbProvider.AddInParameter(command, helper.Reporfeccreacion, DbType.DateTime, entity.Reporfeccreacion);
            dbProvider.AddInParameter(command, helper.Reporusumodificacion, DbType.String, entity.Reporusumodificacion);
            dbProvider.AddInParameter(command, helper.Reporfecmodificacion, DbType.DateTime, entity.Reporfecmodificacion);
            dbProvider.AddInParameter(command, helper.Modcodi, DbType.Int32, entity.Modcodi);
            dbProvider.AddInParameter(command, helper.Cabcodi, DbType.Int32, entity.Cabcodi);
            dbProvider.AddInParameter(command, helper.Areacode, DbType.Int32, entity.Areacode);
            dbProvider.AddInParameter(command, helper.Reporcheckempresa, DbType.Int32, entity.Reporcheckempresa);
            dbProvider.AddInParameter(command, helper.Reporcheckequipo, DbType.Int32, entity.Reporcheckequipo);
            dbProvider.AddInParameter(command, helper.Reporchecktipoequipo, DbType.Int32, entity.Reporchecktipoequipo);
            dbProvider.AddInParameter(command, helper.Reporchecktipomedida, DbType.Int32, entity.Reporchecktipomedida);
            dbProvider.AddInParameter(command, helper.Mrepcodi, DbType.Int32, entity.Mrepcodi);
            dbProvider.AddInParameter(command, helper.Reporejey, DbType.String, entity.Reporejey);
            dbProvider.AddInParameter(command, helper.Reporesgrafico, DbType.String, entity.Reporesgrafico);
            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Delete()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);


            dbProvider.ExecuteNonQuery(command);
        }

        public MeReporteDTO GetById(int id)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);
            dbProvider.AddInParameter(command, helper.Reporcodi, DbType.Int32, id);
            MeReporteDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iReptiprepcodi = dr.GetOrdinal(this.helper.Reptiprepcodi);
                    if (!dr.IsDBNull(iReptiprepcodi)) entity.Reptiprepcodi = dr.GetInt32(iReptiprepcodi);
                }
            }

            return entity;
        }

        public List<MeReporteDTO> List()
        {
            List<MeReporteDTO> entitys = new List<MeReporteDTO>();
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

        public List<MeReporteDTO> ListarXModulo(int modcodi)
        {
            List<MeReporteDTO> entitys = new List<MeReporteDTO>();
            string sql = string.Format(helper.SqlListarXModulo, modcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<MeReporteDTO> ListarXArea(int areacode)
        {
            List<MeReporteDTO> entitys = new List<MeReporteDTO>();
            string sql = string.Format(helper.SqlListarXArea, areacode);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeReporteDTO entity = helper.Create(dr);

                    int iAreaname = dr.GetOrdinal(this.helper.Areaname);
                    if (!dr.IsDBNull(iAreaname)) entity.Areaname = dr.GetString(iAreaname);
                    int iModnombre = dr.GetOrdinal(this.helper.Modnombre);
                    if (!dr.IsDBNull(iModnombre)) entity.Modnombre = dr.GetString(iModnombre);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<MeReporteDTO> ListarXAreaYModulo(int idarea, int idmodulo)
        {
            List<MeReporteDTO> entitys = new List<MeReporteDTO>();
            string sql = string.Format(helper.SqlListarXAreaYModulo, idarea, idmodulo);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeReporteDTO entity = helper.Create(dr);

                    int iAreaname = dr.GetOrdinal(this.helper.Areaname);
                    if (!dr.IsDBNull(iAreaname)) entity.Areaname = dr.GetString(iAreaname);

                    int iAreaabrev = dr.GetOrdinal(helper.Areaabrev);
                    if (!dr.IsDBNull(iAreaabrev)) entity.Areaabrev = dr.GetString(iAreaabrev);

                    int iModnombre = dr.GetOrdinal(this.helper.Modnombre);
                    if (!dr.IsDBNull(iModnombre)) entity.Modnombre = dr.GetString(iModnombre);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
