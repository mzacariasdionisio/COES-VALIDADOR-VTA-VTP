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
    /// Clase de acceso a datos de la tabla CM_REGIONSEGURIDAD_DETALLE
    /// </summary>
    public class CmRegionseguridadDetalleRepository: RepositoryBase, ICmRegionseguridadDetalleRepository
    {
        public CmRegionseguridadDetalleRepository(string strConn): base(strConn)
        {
        }

        CmRegionseguridadDetalleHelper helper = new CmRegionseguridadDetalleHelper();

        public int Save(CmRegionseguridadDetalleDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Regdetcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Regsegcodi, DbType.Int32, entity.Regsegcodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Regsegusucreacion, DbType.String, entity.Regsegusucreacion);
            dbProvider.AddInParameter(command, helper.Regsegfeccreacion, DbType.DateTime, entity.Regsegfeccreacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CmRegionseguridadDetalleDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Regsegcodi, DbType.Int32, entity.Regsegcodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Regsegusucreacion, DbType.String, entity.Regsegusucreacion);
            dbProvider.AddInParameter(command, helper.Regsegfeccreacion, DbType.DateTime, entity.Regsegfeccreacion);
            dbProvider.AddInParameter(command, helper.Regdetcodi, DbType.Int32, entity.Regdetcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int idRegion, int idEquipo)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Regsegcodi, DbType.Int32, idRegion);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, idEquipo);

            dbProvider.ExecuteNonQuery(command);
        }

        public CmRegionseguridadDetalleDTO GetById(int regdetcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Regdetcodi, DbType.Int32, regdetcodi);
            CmRegionseguridadDetalleDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CmRegionseguridadDetalleDTO> List(int idRegion, int idEquipo)
        {
            List<CmRegionseguridadDetalleDTO> entitys = new List<CmRegionseguridadDetalleDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);
            dbProvider.AddInParameter(command, helper.Regsegcodi, DbType.Int32, idRegion);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, idEquipo);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<CmRegionseguridadDetalleDTO> GetByCriteria(int idRegion)
        {
            List<CmRegionseguridadDetalleDTO> entitys = new List<CmRegionseguridadDetalleDTO>();
            string sql = string.Format(helper.SqlGetByCriteria, idRegion);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CmRegionseguridadDetalleDTO entity = helper.Create(dr);

                    int iNombretna = dr.GetOrdinal(helper.Nombretna);
                    if (!dr.IsDBNull(iNombretna)) entity.Nombretna = dr.GetString(iNombretna);

                    int iTipoequipo = dr.GetOrdinal(helper.Tipoequipo);
                    if (!dr.IsDBNull(iTipoequipo)) entity.Tipoequipo = dr.GetString(iTipoequipo);

                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<CmRegionseguridadDetalleDTO> ObtenerEquipos(int tipo)
        {
            List<CmRegionseguridadDetalleDTO> entitys = new List<CmRegionseguridadDetalleDTO>();
            string sql = string.Format(helper.SqlObtenerEquipos, tipo);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CmRegionseguridadDetalleDTO entity = new CmRegionseguridadDetalleDTO();

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iNombretna = dr.GetOrdinal(helper.Nombretna);
                    if (!dr.IsDBNull(iNombretna)) entity.Nombretna = dr.GetString(iNombretna);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        //Mejoras Yupana
        public List<CmRegionseguridadDetalleDTO> ObtenerEquiposCentral()
        {
            List<CmRegionseguridadDetalleDTO> entitys = new List<CmRegionseguridadDetalleDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerEquiposCentral);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CmRegionseguridadDetalleDTO entity = new CmRegionseguridadDetalleDTO();

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iNombretna = dr.GetOrdinal(helper.Nombretna);
                    if (!dr.IsDBNull(iNombretna)) entity.Nombretna = dr.GetString(iNombretna);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<CmRegionseguridadDetalleDTO> ObtenerModoOperacion()
        {
            List<CmRegionseguridadDetalleDTO> entitys = new List<CmRegionseguridadDetalleDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerModoOperacion);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CmRegionseguridadDetalleDTO entity = new CmRegionseguridadDetalleDTO();

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iNombretna = dr.GetOrdinal(helper.Nombretna);
                    if (!dr.IsDBNull(iNombretna)) entity.Nombretna = dr.GetString(iNombretna);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<CmRegionseguridadDetalleDTO> ObtenerEquiposLinea(int tipo)
        {
            List<CmRegionseguridadDetalleDTO> entitys = new List<CmRegionseguridadDetalleDTO>();
            string sql = string.Format(helper.SqlObtenerEquiposLinea, tipo);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CmRegionseguridadDetalleDTO entity = new CmRegionseguridadDetalleDTO();

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iNombretna = dr.GetOrdinal(helper.Nombretna);
                    if (!dr.IsDBNull(iNombretna)) entity.Nombretna = dr.GetString(iNombretna);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
