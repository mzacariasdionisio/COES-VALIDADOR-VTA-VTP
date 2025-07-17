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
    /// Clase de acceso a datos de la tabla SEG_REGIONGRUPO
    /// </summary>
    public class SegRegiongrupoRepository: RepositoryBase, ISegRegiongrupoRepository
    {
        public SegRegiongrupoRepository(string strConn): base(strConn)
        {
        }

        SegRegiongrupoHelper helper = new SegRegiongrupoHelper();

        public int Save(SegRegiongrupoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Regcodi, DbType.Int32, entity.Regcodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Reggcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Segcotipo, DbType.Int32, entity.Segcotipo);
            dbProvider.AddInParameter(command, helper.Reggusucreacion, DbType.String, entity.Reggusucreacion);
            dbProvider.AddInParameter(command, helper.Reggfeccreacion, DbType.DateTime, entity.Reggfeccreacion);
            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(SegRegiongrupoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Regcodi, DbType.Int32, entity.Regcodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Reggcodi, DbType.Int32, entity.Reggcodi);
            dbProvider.AddInParameter(command, helper.Segcotipo, DbType.Int32, entity.Segcotipo);
            dbProvider.AddInParameter(command, helper.Reggusucreacion, DbType.String, entity.Reggusucreacion);
            dbProvider.AddInParameter(command, helper.Reggfeccreacion, DbType.DateTime, entity.Reggfeccreacion);
            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int regcodi, int grupocodi, int segcotipo)
        {
            string strdelete = string.Format(helper.SqlDelete, regcodi, grupocodi, segcotipo);
            DbCommand command = dbProvider.GetSqlStringCommand(strdelete);
            dbProvider.ExecuteNonQuery(command);
        }

        public SegRegiongrupoDTO GetById()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            SegRegiongrupoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SegRegiongrupoDTO> List()
        {
            List<SegRegiongrupoDTO> entitys = new List<SegRegiongrupoDTO>();
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

        public List<SegRegiongrupoDTO> GetByCriteria(int regsegcodi, int segcotipo)
        {
            List<SegRegiongrupoDTO> entitys = new List<SegRegiongrupoDTO>();
            string sqlQuery = string.Format(helper.SqlGetByCriteria, regsegcodi, segcotipo);
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SegRegiongrupoDTO entity = new SegRegiongrupoDTO();
                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));
                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);
                    int iTipoequipo = dr.GetOrdinal(helper.Tipoequipo);
                    if (!dr.IsDBNull(iTipoequipo)) entity.Tipoequipo = dr.GetString(iTipoequipo);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    
    }
}
