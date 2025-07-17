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
    /// Clase de acceso a datos de la tabla SI_EMPRESADAT
    /// </summary>
    public class SiEmpresadatRepository : RepositoryBase, ISiEmpresadatRepository
    {
        public SiEmpresadatRepository(string strConn)
            : base(strConn)
        {
        }

        SiEmpresadatHelper helper = new SiEmpresadatHelper();

        public void Save(SiEmpresadatDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Empdatfecha, DbType.DateTime, entity.Empdatfecha);
            dbProvider.AddInParameter(command, helper.Consiscodi, DbType.Int32, entity.Consiscodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Empdatusucreacion, DbType.String, entity.Empdatusucreacion);
            dbProvider.AddInParameter(command, helper.Empdatfeccreacion, DbType.DateTime, entity.Empdatfeccreacion);
            dbProvider.AddInParameter(command, helper.Empdatusumodificacion, DbType.String, entity.Empdatusumodificacion);
            dbProvider.AddInParameter(command, helper.Empdatfecmodificacion, DbType.DateTime, entity.Empdatfecmodificacion);
            dbProvider.AddInParameter(command, helper.Empdatdeleted, DbType.Int32, entity.Empdatdeleted);
            dbProvider.AddInParameter(command, helper.Empdatvalor, DbType.String, entity.Empdatvalor);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(SiEmpresadatDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Empdatfecha, DbType.DateTime, entity.Empdatfecha);
            dbProvider.AddInParameter(command, helper.Consiscodi, DbType.Int32, entity.Consiscodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Empdatusucreacion, DbType.String, entity.Empdatusucreacion);
            dbProvider.AddInParameter(command, helper.Empdatfeccreacion, DbType.DateTime, entity.Empdatfeccreacion);
            dbProvider.AddInParameter(command, helper.Empdatusumodificacion, DbType.String, entity.Empdatusumodificacion);
            dbProvider.AddInParameter(command, helper.Empdatfecmodificacion, DbType.DateTime, entity.Empdatfecmodificacion);
            dbProvider.AddInParameter(command, helper.Empdatdeleted, DbType.Int32, entity.Empdatdeleted);
            dbProvider.AddInParameter(command, helper.Empdatvalor, DbType.String, entity.Empdatvalor);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(DateTime empdatfecha, int consiscodi, int emprcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Empdatfecha, DbType.DateTime, empdatfecha);
            dbProvider.AddInParameter(command, helper.Consiscodi, DbType.Int32, consiscodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete_UpdateAuditoria(DateTime empdatfecha, int consiscodi, int emprcodi, string username)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete_UpdateAuditoria);

            dbProvider.AddInParameter(command, helper.Empdatusumodificacion, DbType.String, username);
            dbProvider.AddInParameter(command, helper.Empdatfecha, DbType.DateTime, empdatfecha);
            dbProvider.AddInParameter(command, helper.Consiscodi, DbType.Int32, consiscodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public SiEmpresadatDTO GetById(DateTime empdatfecha, int consiscodi, int emprcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Empdatfecha, DbType.DateTime, empdatfecha);
            dbProvider.AddInParameter(command, helper.Consiscodi, DbType.Int32, consiscodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprcodi);
            SiEmpresadatDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SiEmpresadatDTO> List()
        {
            List<SiEmpresadatDTO> entitys = new List<SiEmpresadatDTO>();
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

        public List<SiEmpresadatDTO> GetByCriteria()
        {
            List<SiEmpresadatDTO> entitys = new List<SiEmpresadatDTO>();
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

        public List<SiEmpresadatDTO> ListByEmpresaYConcepto(DateTime fechaInicio, DateTime fechaFin, string empresas, string conceptos)
        {
            List<SiEmpresadatDTO> entitys = new List<SiEmpresadatDTO>();

            string query = string.Format(helper.SqlListByEmpresaYConcepto, fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha), empresas, conceptos);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiEmpresadatDTO entity = helper.Create(dr);

                    int iEmprabrev = dr.GetOrdinal(helper.Emprabrev);
                    if (!dr.IsDBNull(iEmprabrev)) entity.Emprabrev = dr.GetString(iEmprabrev);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iTipoemprcodi = dr.GetOrdinal(helper.Tipoemprcodi);
                    if (!dr.IsDBNull(iTipoemprcodi)) entity.Tipoemprcodi = dr.GetInt32(iTipoemprcodi);

                    int iEmprestado = dr.GetOrdinal(helper.Emprestado);
                    if (!dr.IsDBNull(iEmprestado)) entity.Emprestado = dr.GetString(iEmprestado);

                    int iEmprestadoFecha = dr.GetOrdinal(helper.EmprestadoFecha);
                    if (!dr.IsDBNull(iEmprestadoFecha)) entity.EmprestadoFecha = dr.GetDateTime(iEmprestadoFecha);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
