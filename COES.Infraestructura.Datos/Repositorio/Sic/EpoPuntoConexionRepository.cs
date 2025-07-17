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
    public class EpoPuntoConexionRepository : RepositoryBase, IEpoPuntoConexionRepository
    {

        public EpoPuntoConexionRepository(string strConn) : base(strConn)
        {
        }

        EpoPuntoConexionHelper helper = new EpoPuntoConexionHelper();

        public List<EpoPuntoConexionDTO> List()
        {
            List<EpoPuntoConexionDTO> entitys = new List<EpoPuntoConexionDTO>();
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
        public EpoPuntoConexionDTO GetById(int estepocodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.PuntCodi, DbType.Int32, estepocodi);
            EpoPuntoConexionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iPuntCodi = dr.GetOrdinal(helper.PuntCodi);
                    if (!dr.IsDBNull(iPuntCodi)) entity.PuntCodi = Convert.ToInt32(dr.GetValue(iPuntCodi));

                    int iPuntDescrip = dr.GetOrdinal(helper.PuntDescripcion);
                    if (!dr.IsDBNull(iPuntDescrip)) entity.PuntDescripcion = dr.GetString(iPuntDescrip);

                    int iZonCodi = dr.GetOrdinal(helper.ZonCodi);
                    if (!dr.IsDBNull(iZonCodi)) entity.ZonCodi = Convert.ToInt32(dr.GetValue(iZonCodi));

                    int iPuntActivo = dr.GetOrdinal(helper.PuntActivo);
                    if (!dr.IsDBNull(iPuntActivo)) entity.PuntActivo = dr.GetString(iPuntActivo);

                }
            }
            return entity;
        }

        public List<EpoPuntoConexionDTO> GetByCriteria(EpoPuntoConexionDTO estudioepo)
        {
            string sql = string.Format(helper.SqlGetByCriteria, estudioepo.ZonCodi, estudioepo.PuntDescripcion, estudioepo.nroPagina, estudioepo.nroFilas);

            List<EpoPuntoConexionDTO> entitys = new List<EpoPuntoConexionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EpoPuntoConexionDTO entity = new EpoPuntoConexionDTO();

                    int iPuntCodi = dr.GetOrdinal(helper.PuntCodi);
                    if (!dr.IsDBNull(iPuntCodi)) entity.PuntCodi = Convert.ToInt32(dr.GetValue(iPuntCodi));

                    int iZona = dr.GetOrdinal(helper.ZonDescripcion);
                    if (!dr.IsDBNull(iZona)) entity.ZonDescripcion = dr.GetString(iZona);

                    int iPuntDescrip = dr.GetOrdinal(helper.PuntDescripcion);
                    if (!dr.IsDBNull(iPuntDescrip)) entity.PuntDescripcion = dr.GetString(iPuntDescrip);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int ObtenerNroRegistroBusqueda(EpoPuntoConexionDTO estudioepo)
        {
            string sql = string.Format(helper.SqlGetByCriteria, estudioepo.ZonCodi, estudioepo.PuntDescripcion, estudioepo.nroPagina, estudioepo.nroFilas);

            List<EpoPuntoConexionDTO> entitys = new List<EpoPuntoConexionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            object result = dbProvider.ExecuteScalar(command);

            if (result != null)
            {
                return Convert.ToInt32(result);
            }

            return 0;
        }


        public int Save(EpoPuntoConexionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);
            dbProvider.AddInParameter(command, helper.PuntCodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.PuntDescripcion, DbType.String, entity.PuntDescripcion.Trim());
            dbProvider.AddInParameter(command, helper.LastDate, DbType.DateTime, entity.LastDate);
            dbProvider.AddInParameter(command, helper.LastUser, DbType.String, entity.LastUser);
            dbProvider.AddInParameter(command, helper.ZonCodi, DbType.Int32, entity.ZonCodi);
            dbProvider.AddInParameter(command, helper.PuntActivo, DbType.String, entity.PuntActivo);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(EpoPuntoConexionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);
            dbProvider.AddInParameter(command, helper.PuntDescripcion, DbType.String, entity.PuntDescripcion.Trim());
            dbProvider.AddInParameter(command, helper.LastDate, DbType.DateTime, entity.LastDate);
            dbProvider.AddInParameter(command, helper.LastUser, DbType.String, entity.LastUser);
            dbProvider.AddInParameter(command, helper.ZonCodi, DbType.Int32, entity.ZonCodi);
            dbProvider.AddInParameter(command, helper.PuntActivo, DbType.String, entity.PuntActivo);
            dbProvider.AddInParameter(command, helper.PuntCodi, DbType.String, entity.PuntCodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int PuntCodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);
            dbProvider.AddInParameter(command, helper.PuntCodi, DbType.Int32, PuntCodi);
            dbProvider.ExecuteNonQuery(command);
        }


        public EpoPuntoConexionDTO GetByCodigo(string descripcion)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCodigo);

            dbProvider.AddInParameter(command, helper.PuntDescripcion, DbType.String, descripcion);
            EpoPuntoConexionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                    int iPuntDescrip = dr.GetOrdinal(helper.PuntDescripcion);
                    if (!dr.IsDBNull(iPuntDescrip)) entity.PuntDescripcion = dr.GetString(iPuntDescrip);
                }
            }
            return entity;
        }
    }
}
