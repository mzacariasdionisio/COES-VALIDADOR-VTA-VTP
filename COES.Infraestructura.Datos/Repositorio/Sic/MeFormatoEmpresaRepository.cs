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
    /// Clase de acceso a datos de la tabla ME_FORMATO_EMPRESA
    /// </summary>
    public class MeFormatoEmpresaRepository: RepositoryBase, IMeFormatoEmpresaRepository
    {
        public MeFormatoEmpresaRepository(string strConn): base(strConn)
        {
        }

        MeFormatoEmpresaHelper helper = new MeFormatoEmpresaHelper();

        public void Save(MeFormatoEmpresaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Formatcodi, DbType.Int32, entity.Formatcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Foremdiatomamedicion, DbType.Int32, entity.Foremdiatomamedicion);
            dbProvider.AddInParameter(command, helper.Foremusucreacion, DbType.String, entity.Foremusucreacion);
            dbProvider.AddInParameter(command, helper.Foremfeccreacion, DbType.DateTime, entity.Foremfeccreacion);
            dbProvider.AddInParameter(command, helper.Foremusumodificacion, DbType.String, entity.Foremusumodificacion);
            dbProvider.AddInParameter(command, helper.Foremfecmodificacion, DbType.DateTime, entity.Foremfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(MeFormatoEmpresaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);
            
            dbProvider.AddInParameter(command, helper.Foremdiatomamedicion, DbType.Int32, entity.Foremdiatomamedicion);
            dbProvider.AddInParameter(command, helper.Foremusucreacion, DbType.String, entity.Foremusucreacion);
            dbProvider.AddInParameter(command, helper.Foremfeccreacion, DbType.DateTime, entity.Foremfeccreacion);
            dbProvider.AddInParameter(command, helper.Foremusumodificacion, DbType.String, entity.Foremusumodificacion);
            dbProvider.AddInParameter(command, helper.Foremfecmodificacion, DbType.DateTime, entity.Foremfecmodificacion);
			dbProvider.AddInParameter(command, helper.Formatcodi, DbType.Int32, entity.Formatcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int formatcodi, int emprcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Formatcodi, DbType.Int32, formatcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public MeFormatoEmpresaDTO GetById(int formatcodi, int emprcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Formatcodi, DbType.Int32, formatcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprcodi);
            MeFormatoEmpresaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<MeFormatoEmpresaDTO> List()
        {
            List<MeFormatoEmpresaDTO> entitys = new List<MeFormatoEmpresaDTO>();
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

        public List<MeFormatoEmpresaDTO> GetByCriteria()
        {
            List<MeFormatoEmpresaDTO> entitys = new List<MeFormatoEmpresaDTO>();
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

        public List<MeFormatoEmpresaDTO> ObtenerListaPeriodoEnvio(string fecha, int formato, int idEmpresa)
        {
            string sqlQuery = string.Format(helper.SqlObtenerListaPeriodoEnvio, fecha, formato, idEmpresa);
            List<MeFormatoEmpresaDTO> entitys = new List<MeFormatoEmpresaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeFormatoEmpresaDTO entity = new MeFormatoEmpresaDTO();

                    int iPeriodoFechaIni = dr.GetOrdinal("PRIMER_DIA");
                    if (!dr.IsDBNull(iPeriodoFechaIni)) entity.PeriodoFechaIni = dr.GetDateTime(iPeriodoFechaIni);

                    int iPeriodoFechaFin = dr.GetOrdinal("ULTIMO_DIA");
                    if (!dr.IsDBNull(iPeriodoFechaFin)) entity.PeriodoFechaFin = dr.GetDateTime(iPeriodoFechaFin);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
