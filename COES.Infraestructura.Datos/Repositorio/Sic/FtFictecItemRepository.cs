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
    /// Clase de acceso a datos de la tabla FT_FICTECITEM
    /// </summary>
    public class FtFictecItemRepository : RepositoryBase, IFtFictecItemRepository
    {
        public FtFictecItemRepository(string strConn)
            : base(strConn)
        {
        }

        FtFictecItemHelper helper = new FtFictecItemHelper();

        public int Save(FtFictecItemDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Ftitcodi, DbType.Int32, id);

            dbProvider.AddInParameter(command, helper.Propcodi, DbType.Int32, entity.Propcodi);
            dbProvider.AddInParameter(command, helper.Ftitorden, DbType.Int32, entity.Ftitorden);
            dbProvider.AddInParameter(command, helper.Ftitusucreacion, DbType.String, entity.Ftitusucreacion);
            dbProvider.AddInParameter(command, helper.Ftitusumodificacion, DbType.String, entity.Ftitusumodificacion);
            dbProvider.AddInParameter(command, helper.Ftitfecmodificacion, DbType.DateTime, entity.Ftitfecmodificacion);
            dbProvider.AddInParameter(command, helper.Ftitactivo, DbType.Int32, entity.Ftitactivo);
            dbProvider.AddInParameter(command, helper.Concepcodi, DbType.Int32, entity.Concepcodi);
            dbProvider.AddInParameter(command, helper.Fteqcodi, DbType.Int32, entity.Fteqcodi);
            dbProvider.AddInParameter(command, helper.Ftitnombre, DbType.String, entity.Ftitnombre);
            dbProvider.AddInParameter(command, helper.Ftitdet, DbType.Int32, entity.Ftitdet);
            dbProvider.AddInParameter(command, helper.Ftitfeccreacion, DbType.DateTime, entity.Ftitfeccreacion);
            dbProvider.AddInParameter(command, helper.Ftitpadre, DbType.Int32, entity.Ftitpadre);
            dbProvider.AddInParameter(command, helper.Ftitorientacion, DbType.String, entity.Ftitorientacion);
            dbProvider.AddInParameter(command, helper.Ftittipoitem, DbType.Int32, entity.Ftittipoitem);
            dbProvider.AddInParameter(command, helper.Ftittipoprop, DbType.Int32, entity.Ftittipoprop);
            dbProvider.AddInParameter(command, helper.Ftpropcodi, DbType.Int32, entity.Ftpropcodi);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(FtFictecItemDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Propcodi, DbType.Int32, entity.Propcodi);
            dbProvider.AddInParameter(command, helper.Ftitorden, DbType.Int32, entity.Ftitorden);
            dbProvider.AddInParameter(command, helper.Ftitusucreacion, DbType.String, entity.Ftitusucreacion);
            dbProvider.AddInParameter(command, helper.Ftitusumodificacion, DbType.String, entity.Ftitusumodificacion);
            dbProvider.AddInParameter(command, helper.Ftitfecmodificacion, DbType.DateTime, entity.Ftitfecmodificacion);
            dbProvider.AddInParameter(command, helper.Ftitactivo, DbType.Int32, entity.Ftitactivo);
            dbProvider.AddInParameter(command, helper.Concepcodi, DbType.Int32, entity.Concepcodi);
            dbProvider.AddInParameter(command, helper.Fteqcodi, DbType.Int32, entity.Fteqcodi);
            dbProvider.AddInParameter(command, helper.Ftitnombre, DbType.String, entity.Ftitnombre);
            dbProvider.AddInParameter(command, helper.Ftitdet, DbType.Int32, entity.Ftitdet);
            dbProvider.AddInParameter(command, helper.Ftitfeccreacion, DbType.DateTime, entity.Ftitfeccreacion);
            dbProvider.AddInParameter(command, helper.Ftitpadre, DbType.Int32, entity.Ftitpadre);
            dbProvider.AddInParameter(command, helper.Ftitorientacion, DbType.String, entity.Ftitorientacion);
            dbProvider.AddInParameter(command, helper.Ftittipoitem, DbType.Int32, entity.Ftittipoitem);
            dbProvider.AddInParameter(command, helper.Ftittipoprop, DbType.Int32, entity.Ftittipoprop);
            dbProvider.AddInParameter(command, helper.Ftpropcodi, DbType.Int32, entity.Ftpropcodi);

            dbProvider.AddInParameter(command, helper.Ftitcodi, DbType.Int32, entity.Ftitcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(FtFictecItemDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Ftitusumodificacion, DbType.String, entity.Ftitusumodificacion);
            dbProvider.AddInParameter(command, helper.Ftitfecmodificacion, DbType.DateTime, entity.Ftitfecmodificacion);

            dbProvider.AddInParameter(command, helper.Ftitcodi, DbType.Int32, entity.Ftitcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public FtFictecItemDTO GetById(int ftitcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Ftitcodi, DbType.Int32, ftitcodi);
            FtFictecItemDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<FtFictecItemDTO> List()
        {
            List<FtFictecItemDTO> entitys = new List<FtFictecItemDTO>();
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

        public List<FtFictecItemDTO> GetByCriteria()
        {
            List<FtFictecItemDTO> entitys = new List<FtFictecItemDTO>();
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

        public List<FtFictecItemDTO> ListarItemsByFichaTecnica(int fteqcodi)
        {
            List<FtFictecItemDTO> entitys = new List<FtFictecItemDTO>();

            string query = string.Format(helper.SqlListarItemsByFichaTecnica, fteqcodi);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    FtFictecItemDTO entity = helper.Create(dr);

                    int iPropnomb = dr.GetOrdinal(this.helper.Propnomb);
                    if (!dr.IsDBNull(iPropnomb)) entity.Propnomb = dr.GetString(iPropnomb);

                    int iPropunidad = dr.GetOrdinal(this.helper.Propunidad);
                    if (!dr.IsDBNull(iPropunidad)) entity.Propunidad = dr.GetString(iPropunidad);

                    int iProptipo = dr.GetOrdinal(this.helper.Proptipo);
                    if (!dr.IsDBNull(iProptipo)) entity.Proptipo = dr.GetString(iProptipo);

                    int iPropfile = dr.GetOrdinal(this.helper.Propfile);
                    if (!dr.IsDBNull(iPropfile)) entity.Propfile = dr.GetString(iPropfile);

                    int iConcepdesc = dr.GetOrdinal(this.helper.Concepdesc);
                    if (!dr.IsDBNull(iConcepdesc)) entity.Concepdesc = dr.GetString(iConcepdesc);

                    int iConcepunid = dr.GetOrdinal(this.helper.Concepunid);
                    if (!dr.IsDBNull(iConcepunid)) entity.Concepunid = dr.GetString(iConcepunid);

                    int iConceptipo = dr.GetOrdinal(this.helper.Conceptipo);
                    if (!dr.IsDBNull(iConceptipo)) entity.Conceptipo = dr.GetString(iConceptipo);

                    int iFtpropnomb = dr.GetOrdinal(this.helper.Ftpropnomb);
                    if (!dr.IsDBNull(iFtpropnomb)) entity.Ftpropnomb = dr.GetString(iFtpropnomb);

                    int iFtproptipo = dr.GetOrdinal(this.helper.Ftproptipo);
                    if (!dr.IsDBNull(iFtproptipo)) entity.Ftproptipo = dr.GetString(iFtproptipo);

                    int iFtpropunidad = dr.GetOrdinal(this.helper.Ftpropunidad);
                    if (!dr.IsDBNull(iFtpropunidad)) entity.Ftpropunidad = dr.GetString(iFtpropunidad);

                    int iConcepflagcolor = dr.GetOrdinal(this.helper.Concepflagcolor);
                    if (!dr.IsDBNull(iConcepflagcolor)) entity.Concepflagcolor = Convert.ToInt32(dr.GetValue(iConcepflagcolor));

                    int iPropflagcolor = dr.GetOrdinal(this.helper.Propflagcolor);
                    if (!dr.IsDBNull(iPropflagcolor)) entity.Propflagcolor = Convert.ToInt32(dr.GetValue(iPropflagcolor));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<FtFictecItemDTO> ListarPorIds(string ftitcodis)
        {
            List<FtFictecItemDTO> entitys = new List<FtFictecItemDTO>();
            string query = string.Format(helper.SqlListarPorIds, ftitcodis);
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
