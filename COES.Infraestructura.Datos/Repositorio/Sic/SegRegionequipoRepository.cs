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
    /// Clase de acceso a datos de la tabla SEG_REGIONEQUIPO
    /// </summary>
    public class SegRegionequipoRepository: RepositoryBase, ISegRegionequipoRepository
    {
        public SegRegionequipoRepository(string strConn): base(strConn)
        {
        }

        SegRegionequipoHelper helper = new SegRegionequipoHelper();

        public int Save(SegRegionequipoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Regcodi, DbType.Int32, entity.Regcodi);
            dbProvider.AddInParameter(command, helper.Regecodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Segcotipo, DbType.Int32, entity.Segcotipo);
            dbProvider.AddInParameter(command, helper.Regeusucreacion, DbType.String, entity.Regeusucreacion);
            dbProvider.AddInParameter(command, helper.Regefeccreacion, DbType.DateTime, entity.Regefeccreacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(SegRegionequipoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Regcodi, DbType.Int32, entity.Regcodi);
            dbProvider.AddInParameter(command, helper.Regecodi, DbType.Int32, entity.Regecodi);
            dbProvider.AddInParameter(command, helper.Segcotipo, DbType.Int32, entity.Segcotipo);
            dbProvider.AddInParameter(command, helper.Regeusucreacion, DbType.String, entity.Regeusucreacion);
            dbProvider.AddInParameter(command, helper.Regefeccreacion, DbType.DateTime, entity.Regefeccreacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int regcodi,int equicodi ,int segcotipo)
        {
            string strdelete = string.Format(helper.SqlDelete, regcodi, equicodi, segcotipo);
            DbCommand command = dbProvider.GetSqlStringCommand(strdelete);
            dbProvider.ExecuteNonQuery(command);
        }

        public SegRegionequipoDTO GetById()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            SegRegionequipoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SegRegionequipoDTO> List()
        {
            List<SegRegionequipoDTO> entitys = new List<SegRegionequipoDTO>();
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

        public List<SegRegionequipoDTO> GetByCriteria(int regsegcodi,int segcotipo)
        {
            List<SegRegionequipoDTO> entitys = new List<SegRegionequipoDTO>();
            string sqlQuery = string.Format(helper.SqlGetByCriteria, regsegcodi, segcotipo);
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SegRegionequipoDTO entity = new SegRegionequipoDTO();
                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));
                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);
                    int iTipoequipo = dr.GetOrdinal(helper.Tipoequipo);
                    if (!dr.IsDBNull(iTipoequipo)) entity.Tipoequipo = dr.GetString(iTipoequipo);
                    int iTipo = dr.GetOrdinal(helper.Tipo);
                    if (!dr.IsDBNull(iTipo)) entity.Tipo = Convert.ToInt32(dr.GetValue(iTipo));
                    entitys.Add(entity);

                }
            }

            return entitys;
        }
    }
}
