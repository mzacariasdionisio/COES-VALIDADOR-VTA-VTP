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
    public class CpaDocumentosDetalleRepository : RepositoryBase, ICpaDocumentosDetalleRepository
    {
        public CpaDocumentosDetalleRepository(string strConn)
            : base(strConn)
        {
        }

        CpaDocumentosDetalleHelper helper = new CpaDocumentosDetalleHelper();

        public int Save(CpaDocumentosDetalleDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Cpaddtcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Cpadoccodi, DbType.Int32, entity.Cpadoccodi);
            dbProvider.AddInParameter(command, helper.Cpaddtruta, DbType.String, entity.Cpaddtruta);
            dbProvider.AddInParameter(command, helper.Cpaddtnombre, DbType.String, entity.Cpaddtnombre);
            dbProvider.AddInParameter(command, helper.Cpaddttamano, DbType.String, entity.Cpaddttamano);
            dbProvider.AddInParameter(command, helper.Cpaddtusucreacion, DbType.String, entity.Cpaddtusucreacion);
            dbProvider.AddInParameter(command, helper.Cpaddtfeccreacion, DbType.DateTime, entity.Cpaddtfeccreacion);

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

        public List<CpaDocumentosDetalleDTO> GetDetalleByDocumento(int cpadoccodi)
        {
            CpaDocumentosDetalleDTO entity = new CpaDocumentosDetalleDTO();
            List<CpaDocumentosDetalleDTO> entitys = new List<CpaDocumentosDetalleDTO>();
            string query = string.Format(helper.SqlGetDetalleByDocumento, cpadoccodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new CpaDocumentosDetalleDTO();

                    int iCpaddtcodi = dr.GetOrdinal(helper.Cpaddtcodi);
                    if (!dr.IsDBNull(iCpaddtcodi)) entity.Cpaddtcodi = dr.GetInt32(iCpaddtcodi);

                    int iCpadoccodi = dr.GetOrdinal(helper.Cpadoccodi);
                    if (!dr.IsDBNull(iCpadoccodi)) entity.Cpadoccodi = dr.GetInt32(iCpadoccodi);

                    int iCpaddtruta = dr.GetOrdinal(helper.Cpaddtruta);
                    if (!dr.IsDBNull(iCpaddtruta)) entity.Cpaddtruta = dr.GetString(iCpaddtruta);

                    int iCpaddtnombre = dr.GetOrdinal(helper.Cpaddtnombre);
                    if (!dr.IsDBNull(iCpaddtnombre)) entity.Cpaddtnombre = dr.GetString(iCpaddtnombre);

                    int iCpaddttamano = dr.GetOrdinal(helper.Cpaddttamano);
                    if (!dr.IsDBNull(iCpaddttamano)) entity.Cpaddttamano = dr.GetString(iCpaddttamano);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
