using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Infraestructura.Datos.Helper.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase que contiene las operaciones con la base de datos
    /// </summary>
    public class VcePtomedModopeRepository : RepositoryBase, IVcePtomedModopeRepository
    {
        public VcePtomedModopeRepository(string strConn) : base(strConn)
        {
        }

        VcePtomedModopeHelper helper = new VcePtomedModopeHelper();

        public void Save(VcePtomedModopeDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Pmemopfecmodificacion, DbType.DateTime, entity.Pmemopfecmodificacion);
            dbProvider.AddInParameter(command, helper.Pmemopusumodificacion, DbType.String, entity.Pmemopusumodificacion);
            dbProvider.AddInParameter(command, helper.Pmemopfeccreacion, DbType.DateTime, entity.Pmemopfeccreacion);
            dbProvider.AddInParameter(command, helper.Pmemopusucreacion, DbType.String, entity.Pmemopusucreacion);
            dbProvider.AddInParameter(command, helper.Pmemopestregistro, DbType.String, entity.Pmemopestregistro);
            dbProvider.AddInParameter(command, helper.Pmemoporden, DbType.Int32, entity.Pmemoporden);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(VcePtomedModopeDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Pmemopfecmodificacion, DbType.DateTime, entity.Pmemopfecmodificacion);
            dbProvider.AddInParameter(command, helper.Pmemopusumodificacion, DbType.String, entity.Pmemopusumodificacion);
            dbProvider.AddInParameter(command, helper.Pmemopfeccreacion, DbType.DateTime, entity.Pmemopfeccreacion);
            dbProvider.AddInParameter(command, helper.Pmemopusucreacion, DbType.String, entity.Pmemopusucreacion);
            dbProvider.AddInParameter(command, helper.Pmemopestregistro, DbType.String, entity.Pmemopestregistro);
            dbProvider.AddInParameter(command, helper.Pmemoporden, DbType.Int32, entity.Pmemoporden);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int grupocodi, int ptomedicodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, grupocodi);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, ptomedicodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public VcePtomedModopeDTO GetById(int grupocodi, int ptomedicodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, grupocodi);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, ptomedicodi);
            VcePtomedModopeDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<VcePtomedModopeDTO> List()
        {
            List<VcePtomedModopeDTO> entitys = new List<VcePtomedModopeDTO>();
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

        public List<VcePtomedModopeDTO> GetByCriteria()
        {
            List<VcePtomedModopeDTO> entitys = new List<VcePtomedModopeDTO>();
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

        //NETC

        public List<VcePtomedModopeDTO> ListById(int pecacodi, int ptoMediCodi)
        {
            List<VcePtomedModopeDTO> entitys = new List<VcePtomedModopeDTO>();
            string queryString = string.Format(helper.SqlListById, pecacodi, ptoMediCodi);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VcePtomedModopeDTO entity = new VcePtomedModopeDTO();

                    int iGrupoCodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupoCodi)) entity.Grupocodi = dr.GetInt32(iGrupoCodi);


                    int iGrupoNomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGrupoNomb)) entity.Gruponomb = dr.GetString(iGrupoNomb);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public int SaveByEntity(VcePtomedModopeDTO entity)
        {
            string queryString = string.Format(helper.SqlGetMaxOrden, entity.Pecacodi, entity.Ptomedicodi);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            object result = dbProvider.ExecuteScalar(command);
            int orden = 1;
            if (result != null) 
            {
                orden = Convert.ToInt32(result);
            }


            command = dbProvider.GetSqlStringCommand(helper.SqlSaveByEntity);

            dbProvider.AddInParameter(command, helper.Pecacodi, DbType.Int32, entity.Pecacodi);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Pmemoporden, DbType.Int32, orden);
            dbProvider.AddInParameter(command, helper.Pmemopestregistro, DbType.String, entity.Pmemopestregistro);
            dbProvider.AddInParameter(command, helper.Pmemopusucreacion, DbType.String, entity.Pmemopusucreacion);
            dbProvider.ExecuteNonQuery(command);

            return orden;
        }

        public void DeleteByEntity(int pecacodi, int ptomedicodi, int grupocodi)
        {
            //DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdateByEntity);
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteByEntity);
            dbProvider.AddInParameter(command, helper.Pecacodi, DbType.Int32, pecacodi);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, ptomedicodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, grupocodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public int Validar(int pecacodi, int ptomedicodi, int grupocodi)
        {
            string queryString = string.Format(helper.SqlValidar, pecacodi, ptomedicodi, grupocodi);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            int val = 0;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    int iValidar = dr.GetOrdinal("VALOR");
                    if (!dr.IsDBNull(iValidar)) val = dr.GetInt32(iValidar);
                }
            }

            return val;
        }

        public void DeleteByVersion(int pecacodi)
        {
            try
            {
                string queryString = string.Format(helper.SqlDeleteByVersion, pecacodi);
                DbCommand command = dbProvider.GetSqlStringCommand(queryString);
                dbProvider.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SaveFromOtherVersion(int pecacodiDestino, int pecacodiOrigen, string usuCreacion)
        {
            string strFecha = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

            string queryString = string.Format(helper.SqlSaveFromOtherVersion, pecacodiDestino, pecacodiOrigen, usuCreacion, strFecha);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.ExecuteNonQuery(command);
        }


    }

}
