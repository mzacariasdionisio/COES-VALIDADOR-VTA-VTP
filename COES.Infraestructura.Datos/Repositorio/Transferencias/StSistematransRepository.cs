using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Transferencias;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Transferencias;

namespace COES.Infraestructura.Datos.Repositorio.Transferencias
{
    /// <summary>
    /// Clase de acceso a datos de la tabla ST_SISTEMATRANS
    /// </summary>
    public class StSistematransRepository : RepositoryBase, IStSistematransRepository
    {
        public StSistematransRepository(string strConn)
            : base(strConn)
        {
        }

        StSistematransHelper helper = new StSistematransHelper();

        public int Save(StSistematransDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Sistrncodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Strecacodi, DbType.Int32, entity.Strecacodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Sistrnnombre, DbType.String, entity.Sistrnnombre);
            dbProvider.AddInParameter(command, helper.Sistrnsucreacion, DbType.String, entity.Sistrnsucreacion);
            dbProvider.AddInParameter(command, helper.Sistrnfeccreacion, DbType.DateTime, entity.Sistrnfeccreacion);
            dbProvider.AddInParameter(command, helper.Sistrnsumodificacion, DbType.String, entity.Sistrnsumodificacion);
            dbProvider.AddInParameter(command, helper.Sistrnfecmodificacion, DbType.DateTime, entity.Sistrnfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(StSistematransDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Strecacodi, DbType.Int32, entity.Strecacodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Sistrnnombre, DbType.String, entity.Sistrnnombre);
            dbProvider.AddInParameter(command, helper.Sistrnsucreacion, DbType.String, entity.Sistrnsucreacion);
            dbProvider.AddInParameter(command, helper.Sistrnfeccreacion, DbType.DateTime, entity.Sistrnfeccreacion);
            dbProvider.AddInParameter(command, helper.Sistrnsumodificacion, DbType.String, entity.Sistrnsumodificacion);
            dbProvider.AddInParameter(command, helper.Sistrnfecmodificacion, DbType.DateTime, entity.Sistrnfecmodificacion);
            dbProvider.AddInParameter(command, helper.Sistrncodi, DbType.Int32, entity.Sistrncodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int sistrncodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Sistrncodi, DbType.Int32, sistrncodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteVersion(int strecacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteVersion);

            dbProvider.AddInParameter(command, helper.Strecacodi, DbType.Int32, strecacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public StSistematransDTO GetById(int sistrncodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Sistrncodi, DbType.Int32, sistrncodi);
            StSistematransDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<StSistematransDTO> List(int strecacodi)
        {
            List<StSistematransDTO> entitys = new List<StSistematransDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);
            dbProvider.AddInParameter(command, helper.Strecacodi, DbType.Int32, strecacodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<StSistematransDTO> GetByCriteria(int recacodi)
        {
            List<StSistematransDTO> entitys = new List<StSistematransDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);

            dbProvider.AddInParameter(command, helper.Strecacodi, DbType.Int32, recacodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    StSistematransDTO entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<StSistematransDTO> ListByStSistemaTransVersion(int strecacodi)
        {
            List<StSistematransDTO> entitys = new List<StSistematransDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListByStSistemaTransVersion);
            dbProvider.AddInParameter(command, helper.Strecacodi, DbType.String, strecacodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    StSistematransDTO entity = helper.Create(dr);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        //AGREGADO PARA CONSULTAS
        public StSistematransDTO GetBySisTransNomb(int strecacodi, string sSisTransnombre)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetBySisTransNombre);

            dbProvider.AddInParameter(command, helper.Strecacodi, DbType.Int32, strecacodi);
            dbProvider.AddInParameter(command, helper.Sistrnnombre, DbType.String, sSisTransnombre);
            StSistematransDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<StSistematransDTO> ListByStSistemaTransReporte(int strecacodi)
        {
            List<StSistematransDTO> entitys = new List<StSistematransDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListByStSistemaTransReporte);
            dbProvider.AddInParameter(command, helper.Strecacodi, DbType.String, strecacodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    StSistematransDTO entity = new StSistematransDTO();

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iSistrnnombre = dr.GetOrdinal(this.helper.Sistrnnombre);
                    if (!dr.IsDBNull(iSistrnnombre)) entity.Sistrnnombre = dr.GetString(iSistrnnombre);

                    int iStcompcodelemento = dr.GetOrdinal(this.helper.Stcompcodelemento);
                    if (!dr.IsDBNull(iStcompcodelemento)) entity.Stcompcodelemento = dr.GetString(iStcompcodelemento);

                    int iStcompnomelemento = dr.GetOrdinal(this.helper.Stcompnomelemento);
                    if (!dr.IsDBNull(iStcompnomelemento)) entity.Stcompnomelemento = dr.GetString(iStcompnomelemento);

                    int iStcompimpcompensacion = dr.GetOrdinal(this.helper.Stcompimpcompensacion);
                    if (!dr.IsDBNull(iStcompimpcompensacion)) entity.Stcompimpcompensacion = dr.GetDecimal(iStcompimpcompensacion);

                    int iBarrnombre1 = dr.GetOrdinal(this.helper.Barrnombre1);
                    if (!dr.IsDBNull(iBarrnombre1)) entity.Barrnombre1 = dr.GetString(iBarrnombre1);

                    int iBarrnombre2 = dr.GetOrdinal(this.helper.Barrnombre2);
                    if (!dr.IsDBNull(iBarrnombre2)) entity.Barrnombre2 = dr.GetString(iBarrnombre2);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
