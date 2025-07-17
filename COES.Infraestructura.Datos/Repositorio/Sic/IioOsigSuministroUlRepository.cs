using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Infraestructura.Datos.Helper.Sic;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla IIO_TMP_CONSUMO
    /// </summary>
    public class IioOsigSuministroUlRepository : RepositoryBase, IIioOsigSuministroUlRepository
    {
        private readonly IioOsigSuministroUlHelper helper = new IioOsigSuministroUlHelper();

        public IioOsigSuministroUlRepository(string strConn)
            : base(strConn)
        {

        }

        public void BulkInsert(List<IioOsigSuministroUlDTO> entitys)
        {
            dbProvider.AddColumnMapping(helper.Psiclicodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Ulsumicodempresa, DbType.String);
            dbProvider.AddColumnMapping(helper.Ulsumicodusuariolibre, DbType.String);
            dbProvider.AddColumnMapping(helper.Ulsumicodsuministro, DbType.String);
            dbProvider.AddColumnMapping(helper.Ulsuminombreusuariolibre, DbType.String);
            dbProvider.AddColumnMapping(helper.Ulsumidireccionsuministro, DbType.String);
            dbProvider.AddColumnMapping(helper.Ulsuminrosuministroemp, DbType.String);
            dbProvider.AddColumnMapping(helper.Ulsumiubigeo, DbType.String);            
            dbProvider.AddColumnMapping(helper.Ulsumicodciiu, DbType.String);

            dbProvider.AddColumnMapping(helper.Equicodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Ulsumiusucreacion, DbType.String);
            dbProvider.AddColumnMapping(helper.Ulsumifeccreacion, DbType.DateTime);
            

            dbProvider.BulkInsert<IioOsigSuministroUlDTO>(entitys, helper.TableName);
        }

        public void Delete(int psiclicodi, string empresa)
        {
            var query = string.Format(helper.SqlDelete, psiclicodi, empresa);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            dbProvider.ExecuteNonQuery(command);
        }
        public void UpdateOsigSuministro(int psiclicodi, string empresa)
        {
            var query = string.Format(helper.SqlUpdateOsigSuministro, empresa);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            dbProvider.AddInParameter(command, helper.Psiclicodi, DbType.Int32, psiclicodi);
            //dbProvider.AddInParameter(command, helper.Ulsumicodempresa, DbType.String, empresa);

            dbProvider.ExecuteNonQuery(command);
        }
        public string ValidarOsigSuministroEquipos(int psiclicodi, string empresa)
        {
            string rpta = "";


            string queryString = string.Empty;
            DbCommand command = null;
            
            //Verificamos que todos los suministros tienen un equipo asignado
            queryString = helper.SqlValidarEquipos;
            if (!string.IsNullOrEmpty(empresa))
            {
                queryString = string.Format(queryString, " AND ULSUMICODEMPRESA = '" + empresa + "'");
            }
            else
            {
                queryString = string.Format(queryString, "");
            }
            command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.AddInParameter(command, helper.Psiclicodi, DbType.Int32, psiclicodi);

            dbProvider.ExecuteNonQuery(command);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    IioOsigSuministroUlDTO entity = new IioOsigSuministroUlDTO();
                    int iUlsumicodsuministro = dr.GetOrdinal(helper.Ulsumicodsuministro);
                    if (!dr.IsDBNull(iUlsumicodsuministro)) entity.Ulsumicodsuministro = dr.GetString(iUlsumicodsuministro);

                    if (rpta == "")
                    {
                        rpta = entity.Ulsumicodsuministro;
                    }
                    else
                    {
                        rpta = rpta + ", " + entity.Ulsumicodsuministro;
                    }
                }
            }

            if (!rpta.Equals(""))
            {
                return rpta;
            }

            rpta = "OK";

            return rpta;
        }

        public int GetCorrelativoDisponibleLogImportacionSicli()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxIdIioLogImportacion);
            int correlativo = 1;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    int iCorrelativo = dr.GetOrdinal(helper.Correlativo);
                    if (!dr.IsDBNull(iCorrelativo)) correlativo = dr.GetInt32(iCorrelativo);
                }
            }

            return correlativo;
        }

        public void GenerarOsigSuministroLogImportacionEquipo(int psiclicodi, string periodo, string usuario, string tabla, string empresas)
        {

            var maxId = GetCorrelativoDisponibleLogImportacionSicli();

            var paramsQuery = new object[] { maxId, usuario, periodo, tabla, empresas };
            var query = string.Format(helper.SqlRegistrarLogimportacionEquipo, paramsQuery);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            dbProvider.AddInParameter(command, helper.Psiclicodi, DbType.Int32, psiclicodi);
            dbProvider.ExecuteNonQuery(command);
        }
    }
}