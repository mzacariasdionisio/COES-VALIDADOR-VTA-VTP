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
    /// Clase de acceso a datos de la tabla INF_ARCHIVO_AGENTE
    /// </summary>
    public class InfArchivoAgenteRepository: RepositoryBase, IInfArchivoAgenteRepository
    {
        public InfArchivoAgenteRepository(string strConn): base(strConn)
        {
        }

        InfArchivoAgenteHelper helper = new InfArchivoAgenteHelper();

        public int Save(InfArchivoAgenteDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Archicodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Archinomb, DbType.String, entity.Archinomb);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Archiruta, DbType.String, entity.Archiruta);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            //dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(InfArchivoAgenteDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Archicodi, DbType.Int32, entity.Archicodi);
            dbProvider.AddInParameter(command, helper.Archinomb, DbType.String, entity.Archinomb);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Archiruta, DbType.String, entity.Archiruta);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int archicodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Archicodi, DbType.Int32, archicodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public InfArchivoAgenteDTO GetById(int archicodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Archicodi, DbType.Int32, archicodi);
            InfArchivoAgenteDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<InfArchivoAgenteDTO> List()
        {
            List<InfArchivoAgenteDTO> entitys = new List<InfArchivoAgenteDTO>();
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

        public List<InfArchivoAgenteDTO> GetByCriteria()
        {
            List<InfArchivoAgenteDTO> entitys = new List<InfArchivoAgenteDTO>();
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

        public List<InfArchivoAgenteDTO> ListarArchivosPorEmpresa(int iEmpresa, int nroPagina, int nroFilas)
        {
            var entitys = new List<InfArchivoAgenteDTO>();
            string strCommand = string.Format(helper.SqlListadoPorEmpresa, iEmpresa, nroPagina, nroFilas);
            DbCommand command = dbProvider.GetSqlStringCommand(strCommand);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var oEntidad = helper.Create(dr);
                    int iEmprNomb = dr.GetOrdinal("EMPRNOMB");
                    if (!dr.IsDBNull(iEmprNomb)) oEntidad.Emprnomb = Convert.ToString(dr.GetValue(iEmprNomb));
                    entitys.Add(oEntidad);
                }
            }

            return entitys;
        }

        public List<InfArchivoAgenteDTO> ListarArchivosPorFiltro(int iEmpresa, DateTime dtFechaInicio, DateTime dtFechaFin, int nroPagina, int nroFilas)
        {
            var entitys = new List<InfArchivoAgenteDTO>();
            string strCommand = string.Format(helper.SqlListadoPorEmpresaFechas, iEmpresa, dtFechaInicio.ToString("yyyy-MM-dd"), dtFechaFin.ToString("yyyy-MM-dd"),nroPagina, nroFilas);
            DbCommand command = dbProvider.GetSqlStringCommand(strCommand);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var oEntidad = helper.Create(dr);
                    int iEmprNomb = dr.GetOrdinal("EMPRNOMB");
                    if (!dr.IsDBNull(iEmprNomb)) oEntidad.Emprnomb = Convert.ToString(dr.GetValue(iEmprNomb));
                    entitys.Add(oEntidad);
                }
            }

            return entitys;
        }


        public int TotalListarArchivosPorEmpresa(int iEmpresa)
        {
            string sqlTotal = string.Format(helper.SqlCantidadListadoPorEmpresa, iEmpresa);
            DbCommand command = dbProvider.GetSqlStringCommand(sqlTotal);
            object result = dbProvider.ExecuteScalar(command);
            int total = 0;
            if (result != null) total = Convert.ToInt32(result);
            return total;
        }

        public int TotalListarArchivosPorFiltro(int iEmpresa, DateTime dtFechaInicio, DateTime dtFechaFin)
        {
            string sqlTotal = string.Format(helper.SqlCantidadListadoPorEmpresa, iEmpresa, dtFechaInicio.ToString("yyyy-MM-dd"), dtFechaFin.ToString("yyyy-MM-dd"));
            DbCommand command = dbProvider.GetSqlStringCommand(sqlTotal);
            object result = dbProvider.ExecuteScalar(command);
            int total = 0;
            if (result != null) total = Convert.ToInt32(result);
            return total;
        }


        public int CantidadArchivosPorNombre(string sNombreArchivo)
        {
            string sqlTotal = string.Format(helper.SqlCantidadArchivosNombre, sNombreArchivo);
            DbCommand command = dbProvider.GetSqlStringCommand(sqlTotal);
            object result = dbProvider.ExecuteScalar(command);
            int total = 0;
            if (result != null) total = Convert.ToInt32(result);
            return total;
        }
    }
}
