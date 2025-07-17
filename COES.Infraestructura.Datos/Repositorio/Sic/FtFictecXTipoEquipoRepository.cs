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
    /// Clase de acceso a datos de la tabla FT_FICTECXTIPOEQUIPO
    /// </summary>
    public class FtFictecXTipoEquipoRepository : RepositoryBase, IFtFictecXTipoEquipoRepository
    {
        public FtFictecXTipoEquipoRepository(string strConn)
            : base(strConn)
        {
        }

        FtFictecXTipoEquipoHelper helper = new FtFictecXTipoEquipoHelper();

        public int Save(FtFictecXTipoEquipoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Fteqcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Fteqnombre, DbType.String, entity.Fteqnombre);
            dbProvider.AddInParameter(command, helper.Famcodi, DbType.Int32, entity.Famcodi);
            dbProvider.AddInParameter(command, helper.Catecodi, DbType.Int32, entity.Catecodi);
            dbProvider.AddInParameter(command, helper.Ftequsucreacion, DbType.String, entity.Ftequsucreacion);
            dbProvider.AddInParameter(command, helper.Ftequsumodificacion, DbType.String, entity.Ftequsumodificacion);
            dbProvider.AddInParameter(command, helper.Fteqfecmodificacion, DbType.DateTime, entity.Fteqfecmodificacion);
            dbProvider.AddInParameter(command, helper.Fteqfeccreacion, DbType.DateTime, entity.Fteqfeccreacion);
            dbProvider.AddInParameter(command, helper.Fteqpadre, DbType.Int32, entity.Fteqpadre);
            dbProvider.AddInParameter(command, helper.Fteqestado, DbType.String, entity.Fteqestado);
            dbProvider.AddInParameter(command, helper.Fteqflagext, DbType.Int32, entity.Fteqflagext);
            dbProvider.AddInParameter(command, helper.Fteqfecvigenciaext, DbType.DateTime, entity.Fteqfecvigenciaext);
            dbProvider.AddInParameter(command, helper.Fteqflagmostrarcoment, DbType.Int32, entity.Fteqflagmostrarcoment);
            dbProvider.AddInParameter(command, helper.Fteqflagmostrarsust, DbType.Int32, entity.Fteqflagmostrarsust);
            dbProvider.AddInParameter(command, helper.Fteqflagmostrarfech, DbType.Int32, entity.Fteqflagmostrarfech);
            dbProvider.AddInParameter(command, helper.Ftequsumodificacionasig, DbType.String, entity.Ftequsumodificacionasig);
            dbProvider.AddInParameter(command, helper.Fteqfecmodificacionasig, DbType.DateTime, entity.Fteqfecmodificacionasig);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(FtFictecXTipoEquipoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Fteqnombre, DbType.String, entity.Fteqnombre);
            dbProvider.AddInParameter(command, helper.Famcodi, DbType.Int32, entity.Famcodi);
            dbProvider.AddInParameter(command, helper.Catecodi, DbType.Int32, entity.Catecodi);
            dbProvider.AddInParameter(command, helper.Ftequsucreacion, DbType.String, entity.Ftequsucreacion);
            dbProvider.AddInParameter(command, helper.Ftequsumodificacion, DbType.String, entity.Ftequsumodificacion);
            dbProvider.AddInParameter(command, helper.Fteqfecmodificacion, DbType.DateTime, entity.Fteqfecmodificacion);
            dbProvider.AddInParameter(command, helper.Fteqfeccreacion, DbType.DateTime, entity.Fteqfeccreacion);
            dbProvider.AddInParameter(command, helper.Fteqpadre, DbType.Int32, entity.Fteqpadre);
            dbProvider.AddInParameter(command, helper.Fteqestado, DbType.String, entity.Fteqestado);
            dbProvider.AddInParameter(command, helper.Fteqflagext, DbType.Int32, entity.Fteqflagext);
            dbProvider.AddInParameter(command, helper.Fteqfecvigenciaext, DbType.DateTime, entity.Fteqfecvigenciaext);
            dbProvider.AddInParameter(command, helper.Fteqflagmostrarcoment, DbType.Int32, entity.Fteqflagmostrarcoment);
            dbProvider.AddInParameter(command, helper.Fteqflagmostrarsust, DbType.Int32, entity.Fteqflagmostrarsust);
            dbProvider.AddInParameter(command, helper.Fteqflagmostrarfech, DbType.Int32, entity.Fteqflagmostrarfech);
            dbProvider.AddInParameter(command, helper.Ftequsumodificacionasig, DbType.String, entity.Ftequsumodificacionasig);
            dbProvider.AddInParameter(command, helper.Fteqfecmodificacionasig, DbType.DateTime, entity.Fteqfecmodificacionasig);

            dbProvider.AddInParameter(command, helper.Fteqcodi, DbType.Int32, entity.Fteqcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(FtFictecXTipoEquipoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Ftequsumodificacion, DbType.String, entity.Ftequsumodificacion);
            dbProvider.AddInParameter(command, helper.Fteqfecmodificacion, DbType.DateTime, entity.Fteqfecmodificacion);

            dbProvider.AddInParameter(command, helper.Fteqcodi, DbType.Int32, entity.Fteqcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public FtFictecXTipoEquipoDTO GetById(int fteqcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Fteqcodi, DbType.Int32, fteqcodi);
            FtFictecXTipoEquipoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iFamnomb = dr.GetOrdinal(helper.Famnomb);
                    if (!dr.IsDBNull(iFamnomb)) entity.Famnomb = dr.GetString(iFamnomb);

                    int iCatenomb = dr.GetOrdinal(helper.Catenomb);
                    if (!dr.IsDBNull(iCatenomb)) entity.Catenomb = dr.GetString(iCatenomb);

                }
            }

            return entity;
        }

        public List<FtFictecXTipoEquipoDTO> List()
        {
            List<FtFictecXTipoEquipoDTO> entitys = new List<FtFictecXTipoEquipoDTO>();
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

        public List<FtFictecXTipoEquipoDTO> GetByCriteria(string estado)
        {
            List<FtFictecXTipoEquipoDTO> entitys = new List<FtFictecXTipoEquipoDTO>();

            string query = string.Format(helper.SqlGetByCriteria, estado);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iFamnomb = dr.GetOrdinal(helper.Famnomb);
                    if (!dr.IsDBNull(iFamnomb)) entity.Famnomb = dr.GetString(iFamnomb);

                    int iCatenomb = dr.GetOrdinal(helper.Catenomb);
                    if (!dr.IsDBNull(iCatenomb)) entity.Catenomb = dr.GetString(iCatenomb);

                    int iFteqnombrepadre = dr.GetOrdinal(this.helper.Fteqnombrepadre);
                    if (!dr.IsDBNull(iFteqnombrepadre)) entity.Fteqnombrepadre = dr.GetString(iFteqnombrepadre);

                    int iFamcodipadre = dr.GetOrdinal(this.helper.Famcodipadre);
                    if (!dr.IsDBNull(iFamcodipadre)) entity.Famcodipadre = Convert.ToInt32(dr.GetValue(iFamcodipadre));

                    int iFamnombpadre = dr.GetOrdinal(helper.Famnombpadre);
                    if (!dr.IsDBNull(iFamnombpadre)) entity.Famnombpadre = dr.GetString(iFamnombpadre);

                    int iCatecodipadre = dr.GetOrdinal(this.helper.Catecodipadre);
                    if (!dr.IsDBNull(iCatecodipadre)) entity.Catecodipadre = Convert.ToInt32(dr.GetValue(iCatecodipadre));

                    int iCatenombpadre = dr.GetOrdinal(helper.Catenombpadre);
                    if (!dr.IsDBNull(iCatenombpadre)) entity.Catenombpadre = dr.GetString(iCatenombpadre);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<FtFictecXTipoEquipoDTO> ListByFteccodi(int fteccodi)
        {
            List<FtFictecXTipoEquipoDTO> entitys = new List<FtFictecXTipoEquipoDTO>();

            string query = string.Format(helper.SqlListByFteccodi, fteccodi);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iFamnomb = dr.GetOrdinal(helper.Famnomb);
                    if (!dr.IsDBNull(iFamnomb)) entity.Famnomb = dr.GetString(iFamnomb);

                    int iCatenomb = dr.GetOrdinal(helper.Catenomb);
                    if (!dr.IsDBNull(iCatenomb)) entity.Catenomb = dr.GetString(iCatenomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<FtFictecXTipoEquipoDTO> ListAllByFteccodi(int fteccodi)
        {
            List<FtFictecXTipoEquipoDTO> entitys = new List<FtFictecXTipoEquipoDTO>();

            string query = string.Format(helper.SqlListAllByFteccodi, fteccodi);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iFamnomb = dr.GetOrdinal(helper.Famnomb);
                    if (!dr.IsDBNull(iFamnomb)) entity.Famnomb = dr.GetString(iFamnomb);

                    int iCatenomb = dr.GetOrdinal(helper.Catenomb);
                    if (!dr.IsDBNull(iCatenomb)) entity.Catenomb = dr.GetString(iCatenomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<FtFictecXTipoEquipoDTO> ListByFteqpadre(int fteqpadre)
        {
            List<FtFictecXTipoEquipoDTO> entitys = new List<FtFictecXTipoEquipoDTO>();

            string query = string.Format(helper.SqlListByFteqpadre, fteqpadre);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iFamnomb = dr.GetOrdinal(helper.Famnomb);
                    if (!dr.IsDBNull(iFamnomb)) entity.Famnomb = dr.GetString(iFamnomb);

                    int iCatenomb = dr.GetOrdinal(helper.Catenomb);
                    if (!dr.IsDBNull(iCatenomb)) entity.Catenomb = dr.GetString(iCatenomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
