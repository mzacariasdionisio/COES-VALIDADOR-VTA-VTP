using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Sic;
using COES.Base.Tools;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla CO_PERIODO
    /// </summary>
    public class CoPeriodoRepository: RepositoryBase, ICoPeriodoRepository
    {
        public CoPeriodoRepository(string strConn): base(strConn)
        {
        }

        CoPeriodoHelper helper = new CoPeriodoHelper();

        public int Save(CoPeriodoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Copercodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Coperanio, DbType.Int32, entity.Coperanio);
            dbProvider.AddInParameter(command, helper.Copermes, DbType.Int32, entity.Copermes);
            dbProvider.AddInParameter(command, helper.Copernomb, DbType.String, entity.Copernomb);
            dbProvider.AddInParameter(command, helper.Coperestado, DbType.String, entity.Coperestado);
            dbProvider.AddInParameter(command, helper.Coperusucreacion, DbType.String, entity.Coperusucreacion);
            dbProvider.AddInParameter(command, helper.Copperfeccreacion, DbType.DateTime, entity.Copperfeccreacion);
            dbProvider.AddInParameter(command, helper.Copperusumodificacion, DbType.String, entity.Copperusumodificacion);
            dbProvider.AddInParameter(command, helper.Copperfecmodificacion, DbType.DateTime, entity.Copperfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CoPeriodoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Coperanio, DbType.Int32, entity.Coperanio);
            dbProvider.AddInParameter(command, helper.Copermes, DbType.Int32, entity.Copermes);
            dbProvider.AddInParameter(command, helper.Copernomb, DbType.String, entity.Copernomb);
            dbProvider.AddInParameter(command, helper.Coperestado, DbType.String, entity.Coperestado);
            dbProvider.AddInParameter(command, helper.Coperusucreacion, DbType.String, entity.Coperusucreacion);
            dbProvider.AddInParameter(command, helper.Copperfeccreacion, DbType.DateTime, entity.Copperfeccreacion);
            dbProvider.AddInParameter(command, helper.Copperusumodificacion, DbType.String, entity.Copperusumodificacion);
            dbProvider.AddInParameter(command, helper.Copperfecmodificacion, DbType.DateTime, entity.Copperfecmodificacion);
            dbProvider.AddInParameter(command, helper.Copercodi, DbType.Int32, entity.Copercodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int copercodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Copercodi, DbType.Int32, copercodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CoPeriodoDTO GetById(int copercodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Copercodi, DbType.Int32, copercodi);
            CoPeriodoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CoPeriodoDTO> List()
        {
            List<CoPeriodoDTO> entitys = new List<CoPeriodoDTO>();
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

        public List<CoPeriodoDTO> GetByCriteria(int anio)
        {
            List<CoPeriodoDTO> entitys = new List<CoPeriodoDTO>();
            string sql = string.Format(helper.SqlGetByCriteria, anio);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);            

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CoPeriodoDTO entity = helper.Create(dr);

                    entity.Descmes = Util.ObtenerNombreMes((int)entity.Copermes);

                    int iUltimaversion = dr.GetOrdinal(helper.Ultimaversion);
                    if (!dr.IsDBNull(iUltimaversion)) entity.UltimaVersion = dr.GetString(iUltimaversion);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public bool ValidarExistencia(int anio, int mes)
        {
            bool flag = false;

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlValidarExistencia);
            dbProvider.AddInParameter(command, helper.Coperanio, DbType.Int32, anio);
            dbProvider.AddInParameter(command, helper.Copermes, DbType.Int32, mes);

            object result = dbProvider.ExecuteScalar(command);

            if (result != null)
            {
                if (Convert.ToInt32(result)>0)
                {
                    flag = true;
                }
            }

            return flag;

        }

        public CoPeriodoDTO GetMensualByFecha(string fecha)
        {
            List<CoFactorUtilizacionDTO> entitys = new List<CoFactorUtilizacionDTO>();

            string sql = string.Format(helper.SqlGetMensualByFecha, fecha);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            CoPeriodoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }
        
    }
}
