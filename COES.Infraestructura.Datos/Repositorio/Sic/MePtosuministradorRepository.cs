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
    /// Clase de acceso a datos de la tabla ME_PTOSUMINISTRADOR
    /// </summary>
    public class MePtosuministradorRepository: RepositoryBase, IMePtosuministradorRepository
    {
        public MePtosuministradorRepository(string strConn): base(strConn)
        {
        }

        MePtosuministradorHelper helper = new MePtosuministradorHelper();

        public int Save(MePtosuministradorDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Ptosucodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Ptosufechainicio, DbType.DateTime, entity.Ptosufechainicio);
            dbProvider.AddInParameter(command, helper.Ptosufechafin, DbType.DateTime, entity.Ptosufechafin);
            dbProvider.AddInParameter(command, helper.Ptosuusucreacion, DbType.String, entity.Ptosuusucreacion);
            dbProvider.AddInParameter(command, helper.Ptosufeccreacion, DbType.DateTime, entity.Ptosufeccreacion);
            dbProvider.AddInParameter(command, helper.Ptosuusumodificacion, DbType.String, entity.Ptosuusumodificacion);
            dbProvider.AddInParameter(command, helper.Ptosufecmodificacion, DbType.DateTime, entity.Ptosufecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(MePtosuministradorDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);
            
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Ptosufechainicio, DbType.DateTime, entity.Ptosufechainicio);
            dbProvider.AddInParameter(command, helper.Ptosufechafin, DbType.DateTime, entity.Ptosufechafin);
            dbProvider.AddInParameter(command, helper.Ptosuusucreacion, DbType.String, entity.Ptosuusucreacion);
            dbProvider.AddInParameter(command, helper.Ptosufeccreacion, DbType.DateTime, entity.Ptosufeccreacion);
            dbProvider.AddInParameter(command, helper.Ptosuusumodificacion, DbType.String, entity.Ptosuusumodificacion);
            dbProvider.AddInParameter(command, helper.Ptosufecmodificacion, DbType.DateTime, entity.Ptosufecmodificacion);
			dbProvider.AddInParameter(command, helper.Ptosucodi, DbType.Int32, entity.Ptosucodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int ptosucodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Ptosucodi, DbType.Int32, ptosucodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public MePtosuministradorDTO GetById(int ptosucodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Ptosucodi, DbType.Int32, ptosucodi);
            MePtosuministradorDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<MePtosuministradorDTO> List()
        {
            List<MePtosuministradorDTO> entitys = new List<MePtosuministradorDTO>();
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

        public List<MePtosuministradorDTO> GetByCriteria()
        {
            List<MePtosuministradorDTO> entitys = new List<MePtosuministradorDTO>();
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

        //- pr16.JDEL - Inicio 21/10/2016: Cambio para atender el requerimiento.
        public List<MePtosuministradorDTO> ListaEditorPtoSuministro(string periodo, int empresa, int formato)
        {
            string sqlQuery = string.Format(this.helper.SqlListaEditorPtoSuministro, periodo, empresa, formato);
            List<MePtosuministradorDTO> entitys = new List<MePtosuministradorDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MePtosuministradorDTO entity = new MePtosuministradorDTO();

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iPtomedidesc = dr.GetOrdinal(helper.Ptomedidesc);
                    if (!dr.IsDBNull(iPtomedidesc)) entity.Ptomedidesc = dr.GetString(iPtomedidesc);

                    int iPtosucodi = dr.GetOrdinal(helper.Ptosucodi);
                    if (!dr.IsDBNull(iPtosucodi)) entity.Ptosucodi = Convert.ToInt32(dr.GetValue(iPtosucodi));

                    int iEmprrazsocial = dr.GetOrdinal(helper.Emprrazsocial);
                    if (!dr.IsDBNull(iEmprrazsocial)) entity.Emprrazsocial = dr.GetString(iEmprrazsocial);

                    int iVigEmprcodi = dr.GetOrdinal(helper.VigEmprcodi);
                    if (!dr.IsDBNull(iVigEmprcodi)) entity.VigEmprcodi = Convert.ToInt32(dr.GetValue(iVigEmprcodi));

                    int iPerPtosucodi = dr.GetOrdinal(helper.PerPtosucodi);
                    if (!dr.IsDBNull(iPerPtosucodi)) entity.PerPtosucodi = Convert.ToInt32(dr.GetValue(iPerPtosucodi));

                    int iPerEmprcodi = dr.GetOrdinal(helper.PerEmprcodi);
                    if (!dr.IsDBNull(iPerEmprcodi)) entity.PerEmprcodi = Convert.ToInt32(dr.GetValue(iPerEmprcodi));

                    int iSelEmprcodi = dr.GetOrdinal(helper.SelEmprcodi);
                    if (!dr.IsDBNull(iSelEmprcodi)) entity.SelEmprcodi = Convert.ToInt32(dr.GetValue(iSelEmprcodi));


                    entitys.Add(entity);
                }
            }
            return entitys;

        }

        public MePtosuministradorDTO GetByPtoPeriodo(int ptomedicodi, string fecha)
        {
            string sqlQuery = string.Format(this.helper.SqlGetByPtoPeriodo, ptomedicodi, fecha);
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            MePtosuministradorDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        //- JDEL Fin
        public MePtosuministradorDTO ObtenerSuministradorVigente(int ptomedicodi)
        {
            string sqlQuery = string.Format(this.helper.SqlObtenerSuministradorVigente, ptomedicodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            MePtosuministradorDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        #region Rechazo Carga
        /// <summary>
        /// Metodo para modulo Rechazo Carga
        /// </summary>
        /// <returns></returns>
        public List<RcaSuministradorDTO> ListaSuministradoresRechazoCarga()
        {
            string sqlQuery = string.Format(this.helper.SqlListaSuministradoresRechazoCarga);
            List<RcaSuministradorDTO> entitys = new List<RcaSuministradorDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RcaSuministradorDTO entity = new RcaSuministradorDTO();

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprrazsocial = dr.GetOrdinal(helper.Emprrazsocial);
                    if (!dr.IsDBNull(iEmprrazsocial)) entity.Emprrazsocial = dr.GetString(iEmprrazsocial);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        #endregion
    }
}
