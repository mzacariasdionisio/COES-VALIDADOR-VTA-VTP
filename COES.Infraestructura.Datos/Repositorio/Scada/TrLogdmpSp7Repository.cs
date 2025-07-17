using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Scada;
using COES.Dominio.Interfaces.Scada;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Scada;

namespace COES.Infraestructura.Datos.Respositorio.Scada
{
    /// <summary>
    /// Clase de acceso a datos de la tabla TR_LOGDMP_SP7
    /// </summary>
    public class TrLogdmpSp7Repository: RepositoryBase, ITrLogdmpSp7Repository
    {
        public TrLogdmpSp7Repository(string strConn): base(strConn)
        {
        }

        TrLogdmpSp7Helper helper = new TrLogdmpSp7Helper();

        public int Save(TrLogdmpSp7DTO entity)
        {            
            DbCommand command;

            if (ConstantesBase.MedioActualizArchivo == entity.Ldmmedioimp)
            {
                command = dbProvider.GetSqlStringCommand(helper.SqlGetMinId);                
            }
            else
            {
                command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            }


            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Ldmcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Ldmfecha, DbType.DateTime, entity.Ldmfecha);
            dbProvider.AddInParameter(command, helper.Vercodi, DbType.Int32, entity.Vercodi);
            dbProvider.AddInParameter(command, helper.Ldmfechapub, DbType.DateTime, entity.Ldmfechapub);
            dbProvider.AddInParameter(command, helper.Ldmfechaimp, DbType.DateTime, entity.Ldmfechaimp);
            dbProvider.AddInParameter(command, helper.Ldmnomb, DbType.String, entity.Ldmnomb);
            dbProvider.AddInParameter(command, helper.Ldmtipo, DbType.String, entity.Ldmtipo);
            dbProvider.AddInParameter(command, helper.Ldmestadoserv, DbType.String, entity.Ldmestadoserv);
            dbProvider.AddInParameter(command, helper.Ldmestadocli, DbType.String, entity.Ldmestadocli);
            dbProvider.AddInParameter(command, helper.Ldmnotaexp, DbType.String, entity.Ldmnotaexp);
            dbProvider.AddInParameter(command, helper.Ldmnotaimp, DbType.String, entity.Ldmnotaimp);
            dbProvider.AddInParameter(command, helper.Ldmmedioimp, DbType.String, entity.Ldmmedioimp);
            dbProvider.AddInParameter(command, helper.Ldmcomandoexp, DbType.String, entity.Ldmcomandoexp);
            dbProvider.AddInParameter(command, helper.Ldmcomandoimp, DbType.String, entity.Ldmcomandoimp);
            dbProvider.AddInParameter(command, helper.Ldmenlace, DbType.String, entity.Ldmenlace);
            dbProvider.AddInParameter(command, helper.Ldmusucreacion, DbType.String, entity.Ldmusucreacion);
            dbProvider.AddInParameter(command, helper.Ldmfeccreacion, DbType.DateTime, entity.Ldmfeccreacion);
            dbProvider.AddInParameter(command, helper.Ldmusumodificacion, DbType.String, entity.Ldmusumodificacion);
            dbProvider.AddInParameter(command, helper.Ldmfecmodificacion, DbType.DateTime, entity.Ldmfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(TrLogdmpSp7DTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Ldmfecha, DbType.DateTime, entity.Ldmfecha);
            dbProvider.AddInParameter(command, helper.Vercodi, DbType.Int32, entity.Vercodi);
            dbProvider.AddInParameter(command, helper.Ldmfechapub, DbType.DateTime, entity.Ldmfechapub);
            dbProvider.AddInParameter(command, helper.Ldmfechaimp, DbType.DateTime, entity.Ldmfechaimp);
            dbProvider.AddInParameter(command, helper.Ldmnomb, DbType.String, entity.Ldmnomb);
            dbProvider.AddInParameter(command, helper.Ldmtipo, DbType.String, entity.Ldmtipo);
            dbProvider.AddInParameter(command, helper.Ldmestadoserv, DbType.String, entity.Ldmestadoserv);
            dbProvider.AddInParameter(command, helper.Ldmestadocli, DbType.String, entity.Ldmestadocli);
            dbProvider.AddInParameter(command, helper.Ldmnotaexp, DbType.String, entity.Ldmnotaexp);
            dbProvider.AddInParameter(command, helper.Ldmnotaimp, DbType.String, entity.Ldmnotaimp);
            dbProvider.AddInParameter(command, helper.Ldmmedioimp, DbType.String, entity.Ldmmedioimp);
            dbProvider.AddInParameter(command, helper.Ldmcomandoexp, DbType.String, entity.Ldmcomandoexp);
            dbProvider.AddInParameter(command, helper.Ldmcomandoimp, DbType.String, entity.Ldmcomandoimp);
            dbProvider.AddInParameter(command, helper.Ldmenlace, DbType.String, entity.Ldmenlace);
            dbProvider.AddInParameter(command, helper.Ldmusucreacion, DbType.String, entity.Ldmusucreacion);
            dbProvider.AddInParameter(command, helper.Ldmfeccreacion, DbType.DateTime, entity.Ldmfeccreacion);
            dbProvider.AddInParameter(command, helper.Ldmusumodificacion, DbType.String, entity.Ldmusumodificacion);
            dbProvider.AddInParameter(command, helper.Ldmfecmodificacion, DbType.DateTime, entity.Ldmfecmodificacion);
            dbProvider.AddInParameter(command, helper.Ldmcodi, DbType.Int32, entity.Ldmcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int ldmcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Ldmcodi, DbType.Int32, ldmcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public TrLogdmpSp7DTO GetById(int ldmcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Ldmcodi, DbType.Int32, ldmcodi);
            TrLogdmpSp7DTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<TrLogdmpSp7DTO> List()
        {
            List<TrLogdmpSp7DTO> entitys = new List<TrLogdmpSp7DTO>();
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

        public List<TrLogdmpSp7DTO> ListExportacion(string estado)
        {
            List<TrLogdmpSp7DTO> entitys = new List<TrLogdmpSp7DTO>();
            String sql = String.Format(this.helper.SqlListExportacion, estado);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<TrLogdmpSp7DTO> ListImportacion(string estado)
        {
            List<TrLogdmpSp7DTO> entitys = new List<TrLogdmpSp7DTO>();
            String sql = String.Format(this.helper.SqlListImportacion, estado);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<TrLogdmpSp7DTO> GetByCriteria()
        {
            List<TrLogdmpSp7DTO> entitys = new List<TrLogdmpSp7DTO>();
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

        /// <summary>
        /// Graba los datos de la tabla TR_LOGDMP_SP7
        /// </summary>
        public int SaveTrLogdmpSp7Id(TrLogdmpSp7DTO entity)
        {
            try
            {
                int id = 0;

                if (entity.Ldmcodi==0)
                    id = Save(entity);
                else
                { 
                    Update(entity);
                    id = entity.Ldmcodi;
                }

                return id;

            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public List<TrLogdmpSp7DTO> BuscarOperaciones(DateTime fechaIni, DateTime fechaFin, string tipo, int nroPage, int pageSize)
        {
            List<TrLogdmpSp7DTO> entitys = new List<TrLogdmpSp7DTO>();
            String sql = String.Format(this.helper.ObtenerListado, fechaIni.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha),tipo, nroPage, pageSize);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    TrLogdmpSp7DTO entity = new TrLogdmpSp7DTO();

                    int iLdmcodi = dr.GetOrdinal(this.helper.Ldmcodi);
                    if (!dr.IsDBNull(iLdmcodi)) entity.Ldmcodi = Convert.ToInt32(dr.GetValue(iLdmcodi));

                    int iLdmfecha = dr.GetOrdinal(this.helper.Ldmfecha);
                    if (!dr.IsDBNull(iLdmfecha)) entity.Ldmfecha = dr.GetDateTime(iLdmfecha);

                    int iVercodi = dr.GetOrdinal(this.helper.Vercodi);
                    if (!dr.IsDBNull(iVercodi)) entity.Vercodi = Convert.ToInt32(dr.GetValue(iVercodi));

                    int iLdmfechapub = dr.GetOrdinal(this.helper.Ldmfechapub);
                    if (!dr.IsDBNull(iLdmfechapub)) entity.Ldmfechapub = dr.GetDateTime(iLdmfechapub);

                    int iLdmfechaimp = dr.GetOrdinal(this.helper.Ldmfechaimp);
                    if (!dr.IsDBNull(iLdmfechaimp)) entity.Ldmfechaimp = dr.GetDateTime(iLdmfechaimp);

                    int iLdmnomb = dr.GetOrdinal(this.helper.Ldmnomb);
                    if (!dr.IsDBNull(iLdmnomb)) entity.Ldmnomb = dr.GetString(iLdmnomb);

                    int iLdmtipo = dr.GetOrdinal(this.helper.Ldmtipo);
                    if (!dr.IsDBNull(iLdmtipo)) entity.Ldmtipo = dr.GetString(iLdmtipo);

                    int iLdmestadoserv = dr.GetOrdinal(this.helper.Ldmestadoserv);
                    if (!dr.IsDBNull(iLdmestadoserv)) entity.Ldmestadoserv = dr.GetString(iLdmestadoserv);

                    int iLdmestadocli = dr.GetOrdinal(this.helper.Ldmestadocli);
                    if (!dr.IsDBNull(iLdmestadocli)) entity.Ldmestadocli = dr.GetString(iLdmestadocli);

                    int iLdmnotaexp = dr.GetOrdinal(this.helper.Ldmnotaexp);
                    if (!dr.IsDBNull(iLdmnotaexp)) entity.Ldmnotaexp = dr.GetString(iLdmnotaexp);

                    int iLdmnotaimp = dr.GetOrdinal(this.helper.Ldmnotaimp);
                    if (!dr.IsDBNull(iLdmnotaimp)) entity.Ldmnotaimp = dr.GetString(iLdmnotaimp);

                    int iLdmmedioimp = dr.GetOrdinal(this.helper.Ldmmedioimp);
                    if (!dr.IsDBNull(iLdmmedioimp)) entity.Ldmmedioimp = dr.GetString(iLdmmedioimp);

                    int iLdmcomandoexp = dr.GetOrdinal(this.helper.Ldmcomandoexp);
                    if (!dr.IsDBNull(iLdmcomandoexp)) entity.Ldmcomandoexp = dr.GetString(iLdmcomandoexp);

                    int iLdmcomandoimp = dr.GetOrdinal(this.helper.Ldmcomandoimp);
                    if (!dr.IsDBNull(iLdmcomandoimp)) entity.Ldmcomandoimp = dr.GetString(iLdmcomandoimp);

                    int iLdmenlace = dr.GetOrdinal(this.helper.Ldmenlace);
                    if (!dr.IsDBNull(iLdmenlace)) entity.Ldmenlace = dr.GetString(iLdmenlace);

                    int iLdmusucreacion = dr.GetOrdinal(this.helper.Ldmusucreacion);
                    if (!dr.IsDBNull(iLdmusucreacion)) entity.Ldmusucreacion = dr.GetString(iLdmusucreacion);

                    int iLdmfeccreacion = dr.GetOrdinal(this.helper.Ldmfeccreacion);
                    if (!dr.IsDBNull(iLdmfeccreacion)) entity.Ldmfeccreacion = dr.GetDateTime(iLdmfeccreacion);

                    int iLdmusumodificacion = dr.GetOrdinal(this.helper.Ldmusumodificacion);
                    if (!dr.IsDBNull(iLdmusumodificacion)) entity.Ldmusumodificacion = dr.GetString(iLdmusumodificacion);

                    int iLdmfecmodificacion = dr.GetOrdinal(this.helper.Ldmfecmodificacion);
                    if (!dr.IsDBNull(iLdmfecmodificacion)) entity.Ldmfecmodificacion = dr.GetDateTime(iLdmfecmodificacion);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int ObtenerNroFilas(DateTime fechaIni, DateTime fechaFin)
        {
            String sql = String.Format(this.helper.TotalRegistros, fechaIni.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha));

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            object count = dbProvider.ExecuteScalar(command);

            if (count != null) return Convert.ToInt32(count);
            return 0;
        }
    }
}
