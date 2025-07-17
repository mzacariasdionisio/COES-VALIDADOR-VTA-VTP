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
    /// Clase de acceso a datos de la tabla IEE_BARRAZONA
    /// </summary>
    public class IeeBarrazonaRepository : RepositoryBase, IIeeBarrazonaRepository
    {
        public IeeBarrazonaRepository(string strConn) : base(strConn)
        {
        }

        IeeBarrazonaHelper helper = new IeeBarrazonaHelper();

        public int Save(IeeBarrazonaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Barrzcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Barrzarea, DbType.Int32, entity.Barrzarea);
            dbProvider.AddInParameter(command, helper.Barrcodi, DbType.Int32, entity.Barrcodi);
            dbProvider.AddInParameter(command, helper.Mrepcodi, DbType.Int32, entity.Mrepcodi);
            dbProvider.AddInParameter(command, helper.Barrzdesc, DbType.String, entity.Barrzdesc);
            dbProvider.AddInParameter(command, helper.Barrzusumodificacion, DbType.String, entity.Barrzusumodificacion);
            dbProvider.AddInParameter(command, helper.Barrzfecmodificacion, DbType.DateTime, entity.Barrzfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(IeeBarrazonaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Barrzcodi, DbType.Int32, entity.Barrzcodi);
            dbProvider.AddInParameter(command, helper.Barrzarea, DbType.Int32, entity.Barrzarea);
            dbProvider.AddInParameter(command, helper.Barrcodi, DbType.Int32, entity.Barrcodi);
            dbProvider.AddInParameter(command, helper.Mrepcodi, DbType.Int32, entity.Mrepcodi);
            dbProvider.AddInParameter(command, helper.Barrzdesc, DbType.String, entity.Barrzdesc);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int barrzcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Barrzcodi, DbType.Int32, barrzcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public IeeBarrazonaDTO GetById(int barrzcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Barrzcodi, DbType.Int32, barrzcodi);
            IeeBarrazonaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<IeeBarrazonaDTO> List()
        {
            List<IeeBarrazonaDTO> entitys = new List<IeeBarrazonaDTO>();
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

        public List<IeeBarrazonaDTO> GetByCriteria(int mrepcodi)
        {
            List<IeeBarrazonaDTO> entitys = new List<IeeBarrazonaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, helper.Mrepcodi, DbType.Int32, mrepcodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<IeeBarrazonaDTO> ObtenerBarrasPorAreas()
        {
            List<IeeBarrazonaDTO> entitys = new List<IeeBarrazonaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetBarrasPorAreas);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    IeeBarrazonaDTO entity = helper.Create(dr);

                    int iBarrnombre = dr.GetOrdinal(this.helper.Barrnombre);
                    if (!dr.IsDBNull(iBarrnombre)) entity.Barrnombre = dr.GetString(iBarrnombre);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<IeeBarrazonaDTO> ObtenerAgrupacionPorZona(int mrepcodi)
        {
            List<IeeBarrazonaDTO> entitys = new List<IeeBarrazonaDTO>();
            string sql = string.Format(helper.SqlObtenerAgrupacionPorZona, mrepcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    IeeBarrazonaDTO entity = new IeeBarrazonaDTO();

                    int iMrepcodi = dr.GetOrdinal(helper.Mrepcodi);
                    if (!dr.IsDBNull(iMrepcodi)) entity.Mrepcodi = Convert.ToInt32(dr.GetValue(iMrepcodi));

                    int iBarrzdesc = dr.GetOrdinal(helper.Barrzdesc);
                    if (!dr.IsDBNull(iBarrzdesc)) entity.Barrzdesc = dr.GetString(iBarrzdesc);

                    int iBarrzusumodificacion = dr.GetOrdinal(helper.Barrzusumodificacion);
                    if (!dr.IsDBNull(iBarrzusumodificacion)) entity.Barrzusumodificacion = dr.GetString(iBarrzusumodificacion);

                    int iBarrzfecmodificacion = dr.GetOrdinal(helper.Barrzfecmodificacion);
                    if (!dr.IsDBNull(iBarrzfecmodificacion)) entity.Barrzfecmodificacion = dr.GetDateTime(iBarrzfecmodificacion);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<IeeBarrazonaDTO> ObtenerBarrasPorAgrupacion(int mrepcodi, string agrupacion)
        {
            List<IeeBarrazonaDTO> entitys = new List<IeeBarrazonaDTO>();
            string sql = string.Format(helper.SqlObtenerBarrasPorAgrupacion, mrepcodi, agrupacion);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    IeeBarrazonaDTO entity = new IeeBarrazonaDTO();

                    int iBarrcodi = dr.GetOrdinal(this.helper.Barrcodi);
                    if (!dr.IsDBNull(iBarrcodi)) entity.Barrcodi = Convert.ToInt32(dr.GetValue(iBarrcodi));

                    int iBarrnombre = dr.GetOrdinal(this.helper.Barrnombre);
                    if (!dr.IsDBNull(iBarrnombre)) entity.Barrnombre = dr.GetString(iBarrnombre);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public void EliminarAgrupacion(int zona, string agrupacion)
        {
            string sql = string.Format(helper.SqlEliminarAgrupacion, zona, agrupacion);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);            
            dbProvider.ExecuteNonQuery(command);
        }

        public bool ValidarExistencia(int zona, string nombre)
        {
            bool flag = false;
            string sql = string.Format(helper.SqlValidarExistencia, zona, nombre);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            object result = dbProvider.ExecuteScalar(command);
            int id = 0;
            if (result != null)
            {
                id = Convert.ToInt32(result);
                if (id > 0) flag = true;
            }

            return flag;
            
        }

        public bool ValidarExistenciaEdicion(int zona, string nombre, string agrupacion)
        {
            bool flag = false;
            string sql = string.Format(helper.SqlValidarExistenciaEdicion, zona, nombre, agrupacion);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            object result = dbProvider.ExecuteScalar(command);
            int id = 0;
            if (result != null)
            {
                id = Convert.ToInt32(result);
                if (id > 0) flag = true;
            }
            
            return flag;
        }
    }
}
