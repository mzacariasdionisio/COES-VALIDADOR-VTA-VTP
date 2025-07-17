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
    /// Clase de acceso a datos de la tabla FT_EXT_PROYECTO
    /// </summary>
    public class FtExtProyectoRepository : RepositoryBase, IFtExtProyectoRepository
    {
        public FtExtProyectoRepository(string strConn) : base(strConn)
        {
        }

        FtExtProyectoHelper helper = new FtExtProyectoHelper();

        public int Save(FtExtProyectoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Ftprycodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Ftprynombre, DbType.String, entity.Ftprynombre);
            dbProvider.AddInParameter(command, helper.Ftpryeonombre, DbType.String, entity.Ftpryeonombre);
            dbProvider.AddInParameter(command, helper.Ftpryeocodigo, DbType.String, entity.Ftpryeocodigo);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Esteocodi, DbType.Int32, entity.Esteocodi);
            dbProvider.AddInParameter(command, helper.Ftpryestado, DbType.String, entity.Ftpryestado);
            dbProvider.AddInParameter(command, helper.Ftpryusucreacion, DbType.String, entity.Ftpryusucreacion);
            dbProvider.AddInParameter(command, helper.Ftpryfeccreacion, DbType.DateTime, entity.Ftpryfeccreacion);
            dbProvider.AddInParameter(command, helper.Ftpryusumodificacion, DbType.String, entity.Ftpryusumodificacion);
            dbProvider.AddInParameter(command, helper.Ftpryfecmodificacion, DbType.DateTime, entity.Ftpryfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(FtExtProyectoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);


            dbProvider.AddInParameter(command, helper.Ftprynombre, DbType.String, entity.Ftprynombre);
            dbProvider.AddInParameter(command, helper.Ftpryeonombre, DbType.String, entity.Ftpryeonombre);
            dbProvider.AddInParameter(command, helper.Ftpryeocodigo, DbType.String, entity.Ftpryeocodigo);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Esteocodi, DbType.Int32, entity.Esteocodi);
            dbProvider.AddInParameter(command, helper.Ftpryestado, DbType.String, entity.Ftpryestado);
            dbProvider.AddInParameter(command, helper.Ftpryusucreacion, DbType.String, entity.Ftpryusucreacion);
            dbProvider.AddInParameter(command, helper.Ftpryfeccreacion, DbType.DateTime, entity.Ftpryfeccreacion);
            dbProvider.AddInParameter(command, helper.Ftpryusumodificacion, DbType.String, entity.Ftpryusumodificacion);
            dbProvider.AddInParameter(command, helper.Ftpryfecmodificacion, DbType.DateTime, entity.Ftpryfecmodificacion);
            dbProvider.AddInParameter(command, helper.Ftprycodi, DbType.Int32, entity.Ftprycodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int ftprycodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Ftprycodi, DbType.Int32, ftprycodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public FtExtProyectoDTO GetById(int ftprycodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Ftprycodi, DbType.Int32, ftprycodi);
            FtExtProyectoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);
                }
            }

            return entity;
        }

        public List<FtExtProyectoDTO> List()
        {
            List<FtExtProyectoDTO> entitys = new List<FtExtProyectoDTO>();
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

        public List<FtExtProyectoDTO> GetByCriteria()
        {
            List<FtExtProyectoDTO> entitys = new List<FtExtProyectoDTO>();
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

        public List<FtExtProyectoDTO> ListarProyectosPorRangoYEmpresa(string empresa, DateTime fechaIni, DateTime fechaFin)
        {
            List<FtExtProyectoDTO> entitys = new List<FtExtProyectoDTO>();

            string query = string.Format(helper.SqlListarPorRangoYEmpresa, empresa, fechaIni.ToString(ConstantesBase.FormatoFechaPE), fechaFin.ToString(ConstantesBase.FormatoFechaPE));
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    FtExtProyectoDTO entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);


                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<FtExtProyectoDTO> ListarProyectosSinCodigoEOPorAnio(int anio)
        {
            List<FtExtProyectoDTO> entitys = new List<FtExtProyectoDTO>();

            string query = string.Format(helper.SqlListarProyectosSinCodigoEOPorAnio, anio);
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

        public List<FtExtProyectoDTO> ListarPorEstado(string estado)
        {
            List<FtExtProyectoDTO> entitys = new List<FtExtProyectoDTO>();

            string query = string.Format(helper.SqlListarPorEstado, estado);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    FtExtProyectoDTO entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<FtExtProyectoDTO> ListarGrupo(string ftprycodis)
        {
            List<FtExtProyectoDTO> entitys = new List<FtExtProyectoDTO>();

            string query = string.Format(helper.SqlListarGrupo, ftprycodis);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    FtExtProyectoDTO entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<FtExtProyectoDTO> ListarPorEmpresaYEtapa(int emprcodi, int ftetcodi, string feepryestado)
        {
            List<FtExtProyectoDTO> entitys = new List<FtExtProyectoDTO>();

            string query = string.Format(helper.SqlListarPorEmpresaYEtapa, emprcodi, ftetcodi, feepryestado);
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
