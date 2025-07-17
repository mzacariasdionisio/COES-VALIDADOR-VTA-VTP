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
    /// Clase de acceso a datos de la tabla ME_TIPOPUNTOMEDICION
    /// </summary>
    public class MeTipopuntomedicionRepository: RepositoryBase, IMeTipopuntomedicionRepository
    {
        public MeTipopuntomedicionRepository(string strConn): base(strConn)
        {
        }

        MeTipopuntomedicionHelper helper = new MeTipopuntomedicionHelper();

        public int Save(MeTipopuntomedicionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Tipoptomedicodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Famcodi, DbType.Int32, entity.Famcodi);
            dbProvider.AddInParameter(command, helper.Tipoptomedinomb, DbType.String, entity.Tipoptomedinomb);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(MeTipopuntomedicionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Famcodi, DbType.Int32, entity.Famcodi);
            dbProvider.AddInParameter(command, helper.Tipoptomedinomb, DbType.String, entity.Tipoptomedinomb);
            dbProvider.AddInParameter(command, helper.Tipoptomedicodi, DbType.Int32, entity.Tipoptomedicodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int tipoptomedicodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Tipoptomedicodi, DbType.Int32, tipoptomedicodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public MeTipopuntomedicionDTO GetById(int tipoptomedicodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Tipoptomedicodi, DbType.Int32, tipoptomedicodi);
            MeTipopuntomedicionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<MeTipopuntomedicionDTO> List(string origlectcodi)
        {
            List<MeTipopuntomedicionDTO> entitys = new List<MeTipopuntomedicionDTO>();
            string query = string.Format(helper.SqlList, origlectcodi);
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

        public List<MeTipopuntomedicionDTO> GetByCriteria()
        {
            List<MeTipopuntomedicionDTO> entitys = new List<MeTipopuntomedicionDTO>();
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

        public List<MeTipopuntomedicionDTO> ListarMeTipoPuntoMedicion(string StrTptoMedicodi, string estadocodi)
        {
            List<MeTipopuntomedicionDTO> entitys = new List<MeTipopuntomedicionDTO>();
            string sql = String.Format(helper.SqlListarMeTipoPuntoMedicion, StrTptoMedicodi, estadocodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            MeTipopuntomedicionDTO entity;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);
                    int iFenergnomb = dr.GetOrdinal(helper.Fenergnomb);
                    if (!dr.IsDBNull(iFenergnomb)) entity.Fenergnomb = dr.GetString(iFenergnomb);
                    int iFenergcolor = dr.GetOrdinal(helper.Fenergcolor);
                    if (!dr.IsDBNull(iFenergcolor)) entity.Fenergcolor = dr.GetString(iFenergcolor);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        #region Modificación Tipo punto de medición
        public List<MeTipopuntomedicionDTO> ListarTiposPuntoMedicion(int famCodi, int tipoInfoCodi)
        {
            List<MeTipopuntomedicionDTO> entitys = new List<MeTipopuntomedicionDTO>();
            string sql = String.Format(helper.SqlListarTipoPtoMedicionXFamiliaxTipoInfo, famCodi, tipoInfoCodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            MeTipopuntomedicionDTO entity;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);                   
                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        #endregion
        #region Medidores de Generación PR15
        public List<MeTipopuntomedicionDTO> ListarTiposPuntoMedicionPorTipoInformacion(int tipoinfocodi)
        {
            List<MeTipopuntomedicionDTO> entitys = new List<MeTipopuntomedicionDTO>();
            string sql = String.Format(helper.SQLListarTipoPtoMedicionxTipoInfo, tipoinfocodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            MeTipopuntomedicionDTO entity;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        #endregion

        public List<MeTipopuntomedicionDTO> ListFromPtomedicion(string origlectcodi)
        {
            List<MeTipopuntomedicionDTO> entitys = new List<MeTipopuntomedicionDTO>();
            string query = string.Format(helper.SqlListFromPtomedicion, origlectcodi);
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
