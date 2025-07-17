using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Scada;
using COES.Dominio.Interfaces.Scada;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Scada;
using System.Data.Odbc;

namespace COES.Infraestructura.Datos.Repositorio.Scada
{
    /// <summary>
    /// Clase de acceso a datos de la tabla SC_EMPRESA
    /// </summary>
    public class ScEmpresaRepository: RepositoryBase, IScEmpresaRepository
    {
        public ScEmpresaRepository(string strConn): base(strConn)
        {
        }

        ScEmpresaHelper helper = new ScEmpresaHelper();

        public int Save(ScEmpresaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Emprcodisic, DbType.Int32, entity.Emprcodisic);
            dbProvider.AddInParameter(command, helper.Emprenomb, DbType.String, entity.Emprenomb);
            dbProvider.AddInParameter(command, helper.Emprabrev, DbType.String, entity.Emprabrev);
            dbProvider.AddInParameter(command, helper.Empriccppri, DbType.String, entity.Empriccppri);
            dbProvider.AddInParameter(command, helper.Empriccpsec, DbType.String, entity.Empriccpsec);
            dbProvider.AddInParameter(command, helper.Empriccpconect, DbType.String, entity.Empriccpconect);
            dbProvider.AddInParameter(command, helper.Empriccplastdate, DbType.DateTime, entity.Empriccplastdate);
            dbProvider.AddInParameter(command, helper.Emprinvertrealq, DbType.String, entity.Emprinvertrealq);
            dbProvider.AddInParameter(command, helper.Emprinvertstateq, DbType.String, entity.Emprinvertstateq);
            dbProvider.AddInParameter(command, helper.Emprconec, DbType.String, entity.Emprconec);
            dbProvider.AddInParameter(command, helper.Linkcodi, DbType.Int32, entity.Linkcodi);
            dbProvider.AddInParameter(command, helper.Emprstateqgmt, DbType.String, entity.Emprstateqgmt);
            dbProvider.AddInParameter(command, helper.Emprrealqgmt, DbType.String, entity.Emprrealqgmt);
            dbProvider.AddInParameter(command, helper.Emprreenviar, DbType.String, entity.Emprreenviar);
            dbProvider.AddInParameter(command, helper.Emprlatencia, DbType.Int32, entity.Emprlatencia);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(ScEmpresaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Emprcodisic, DbType.Int32, entity.Emprcodisic);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Emprenomb, DbType.String, entity.Emprenomb);
            dbProvider.AddInParameter(command, helper.Emprabrev, DbType.String, entity.Emprabrev);
            dbProvider.AddInParameter(command, helper.Empriccppri, DbType.String, entity.Empriccppri);
            dbProvider.AddInParameter(command, helper.Empriccpsec, DbType.String, entity.Empriccpsec);
            dbProvider.AddInParameter(command, helper.Empriccpconect, DbType.String, entity.Empriccpconect);
            dbProvider.AddInParameter(command, helper.Empriccplastdate, DbType.DateTime, entity.Empriccplastdate);
            dbProvider.AddInParameter(command, helper.Emprinvertrealq, DbType.String, entity.Emprinvertrealq);
            dbProvider.AddInParameter(command, helper.Emprinvertstateq, DbType.String, entity.Emprinvertstateq);
            dbProvider.AddInParameter(command, helper.Emprconec, DbType.String, entity.Emprconec);
            dbProvider.AddInParameter(command, helper.Linkcodi, DbType.Int32, entity.Linkcodi);
            dbProvider.AddInParameter(command, helper.Emprstateqgmt, DbType.String, entity.Emprstateqgmt);
            dbProvider.AddInParameter(command, helper.Emprrealqgmt, DbType.String, entity.Emprrealqgmt);
            dbProvider.AddInParameter(command, helper.Emprreenviar, DbType.String, entity.Emprreenviar);
            dbProvider.AddInParameter(command, helper.Emprlatencia, DbType.Int32, entity.Emprlatencia);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int emprcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public ScEmpresaDTO GetById(int emprcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprcodi);
            ScEmpresaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<ScEmpresaDTO> List()
        {
            List<ScEmpresaDTO> entitys = new List<ScEmpresaDTO>();
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

        public List<ScEmpresaDTO> GetByCriteria()
        {
            List<ScEmpresaDTO> entitys = new List<ScEmpresaDTO>();
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


        public ScEmpresaDTO GetInfoScEmpresa(int emprcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetInfoScEmpresa);

            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprcodi);
            ScEmpresaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<ScEmpresaDTO> GetListaScEmpresa()
        {
            List<ScEmpresaDTO> entitys = new List<ScEmpresaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetListaScEmpresa);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        #region FIT - SEÑALES NO DISPONIBLES - ASOCIACION EMPRESAS

        public List<ScEmpresaDTO> ObtenerBusquedaAsociocionesEmpresa(string nombre)
        {

            List<ScEmpresaDTO> entitys = new List<ScEmpresaDTO>();
            string query = string.Format(helper.SqlObtenerBusquedaAsociocionesEmpresa, nombre);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    ScEmpresaDTO entity = helper.CreateAsociacion(dr);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        public int GrabarAsociacionEmpresa(int emprcodi, int emprcodisp7, string emprusumodificacion)
        {

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGuardarAsociocionesEmpresa);

            dbProvider.AddInParameter(command, helper.Emprcodisic, DbType.Int32, emprcodi);            
            dbProvider.AddInParameter(command, helper.Emprusumodificacion, DbType.String, emprusumodificacion);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprcodisp7);

            dbProvider.ExecuteNonQuery(command);

            return 1;
        }

        public void NuevoAsociacionEmpresa(ScEmpresaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlNuevoAsociacionEmpresa);

            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Emprcodisic, DbType.Int32, entity.Emprcodisic);
            dbProvider.AddInParameter(command, helper.Emprenomb, DbType.String, entity.Emprenomb);
            dbProvider.AddInParameter(command, helper.Emprusumodificacion, DbType.String, entity.Emprusumodificacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void EliminarAsociacionEmpresa(int emprcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlEliminarAsociocionesEmpresa);

            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        #endregion

    }
}
