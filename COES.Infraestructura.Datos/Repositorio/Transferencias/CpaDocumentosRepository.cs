using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Transferencias;
using COES.Infraestructura.Datos.Helper.Transferencias;

namespace COES.Infraestructura.Datos.Repositorio.Transferencias
{
    public class CpaDocumentosRepository : RepositoryBase, ICpaDocumentosRepository
    {
        public CpaDocumentosRepository(string strConn)
            : base(strConn)
        {
        }

        CpaDocumentosHelper helper = new CpaDocumentosHelper();

        public int Save(CpaDocumentosDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Cpadoccodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Cparcodi, DbType.Int32, entity.Cparcodi);
            dbProvider.AddInParameter(command, helper.Cpadoccodenvio, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Cpadocusucreacion, DbType.String, entity.Cpadocusucreacion);
            dbProvider.AddInParameter(command, helper.Cpadocfeccreacion, DbType.DateTime, entity.Cpadocfeccreacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        //public void Update(CpaEmpresaDTO entity)
        //{
        //    DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

        //    dbProvider.AddInParameter(command, helper.Cpaempcodi, DbType.Int32, entity.Cpaempcodi);
        //    dbProvider.AddInParameter(command, helper.Cparcodi, DbType.Int32, entity.Cparcodi);
        //    dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
        //    dbProvider.AddInParameter(command, helper.Cpaemptipo, DbType.String, entity.Cpaemptipo);
        //    dbProvider.AddInParameter(command, helper.Cpaempestado, DbType.String, entity.Cpaempestado);
        //    dbProvider.AddInParameter(command, helper.Cpaempusucreacion, DbType.String, entity.Cpaempusucreacion);
        //    dbProvider.AddInParameter(command, helper.Cpaempfeccreacion, DbType.DateTime, entity.Cpaempfeccreacion);
        //    dbProvider.AddInParameter(command, helper.Cpaempusumodificacion, DbType.String, entity.Cpaempusumodificacion);
        //    dbProvider.AddInParameter(command, helper.Cpaempfecmodificacion, DbType.DateTime, entity.Cpaempfecmodificacion);

        //    dbProvider.ExecuteNonQuery(command);
        //}

        //public void Delete(int cpaEmpcodi)
        //{
        //    DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

        //    dbProvider.AddInParameter(command, helper.Cpaempcodi, DbType.Int32, cpaEmpcodi);

        //    dbProvider.ExecuteNonQuery(command);
        //}

        //public CpaEmpresaDTO GetById(int cpaEmpcodi)
        //{
        //    DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

        //    dbProvider.AddInParameter(command, helper.Cpaempcodi, DbType.Int32, cpaEmpcodi);
        //    CpaEmpresaDTO entity = null;

        //    using (IDataReader dr = dbProvider.ExecuteReader(command))
        //    {
        //        if (dr.Read())
        //        {
        //            entity = helper.Create(dr);
        //        }
        //    }

        //    return entity;
        //}

        //public List<CpaEmpresaDTO> List()
        //{
        //    List<CpaEmpresaDTO> entities = new List<CpaEmpresaDTO>();
        //    DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

        //    using (IDataReader dr = dbProvider.ExecuteReader(command))
        //    {
        //        while (dr.Read())
        //        {
        //            entities.Add(helper.Create(dr));
        //        }
        //    }

        //    return entities;
        //}

        //public List<CpaDocumentosDTO> GetDocumentosByFilters(int cparcodi, string user, string inicio, string fin, int emprcodi)
        public List<CpaDocumentosDTO> GetDocumentosByFilters(int cparcodi, string user, int emprcodi)
        {
            CpaDocumentosDTO entity = new CpaDocumentosDTO();
            List<CpaDocumentosDTO> entitys = new List<CpaDocumentosDTO>();
            string query = string.Format(helper.SqlGetDocumentosByFilters, cparcodi, user, emprcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new CpaDocumentosDTO();

                    int iCpadococdi= dr.GetOrdinal(helper.Cpadoccodi);
                    if (!dr.IsDBNull(iCpadococdi)) entity.Cpadoccodi = dr.GetInt32(iCpadococdi);

                    int iCpadocfeccreacion = dr.GetOrdinal(helper.Cpadocfeccreacion);
                    if (!dr.IsDBNull(iCpadocfeccreacion)) entity.Cpadocfeccreacion = dr.GetDateTime(iCpadocfeccreacion);

                    int iCpadocusucreacion = dr.GetOrdinal(helper.Cpadocusucreacion);
                    if (!dr.IsDBNull(iCpadocusucreacion)) entity.Cpadocusucreacion = dr.GetString(iCpadocusucreacion);

                    int iCpaapanio = dr.GetOrdinal(helper.Cpaapanio);
                    if (!dr.IsDBNull(iCpaapanio)) entity.Cpaapanio = dr.GetInt32(iCpaapanio);

                    int iCpaapajuste = dr.GetOrdinal(helper.Cpaapajuste);
                    if (!dr.IsDBNull(iCpaapajuste)) entity.Cpaapajuste = dr.GetString(iCpaapajuste);

                    int iCparcodi = dr.GetOrdinal(helper.Cparcodi);
                    if (!dr.IsDBNull(iCparcodi)) entity.Cparcodi = dr.GetInt32(iCparcodi);

                    int iCparrevision = dr.GetOrdinal(helper.Cparrevision);
                    if (!dr.IsDBNull(iCparrevision)) entity.Cparrevision = dr.GetString(iCparrevision);

                    int iCpadoccodenvio = dr.GetOrdinal(helper.Cpadoccodenvio);
                    if (!dr.IsDBNull(iCpadoccodenvio)) entity.Cpadoccodenvio = dr.GetInt32(iCpadoccodenvio);

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = dr.GetInt32(iEmprcodi);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
