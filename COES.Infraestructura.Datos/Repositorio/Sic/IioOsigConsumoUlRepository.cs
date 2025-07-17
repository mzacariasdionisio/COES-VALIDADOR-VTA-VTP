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
    public class IioOsigConsumoUlRepository : RepositoryBase, IIioOsigConsumoUlRepository
    {
        private readonly IioOsigConsumoUlHelper helper = new IioOsigConsumoUlHelper();

        public IioOsigConsumoUlRepository(string strConn)
            : base(strConn)
        {

        }

        public void BulkInsert(List<IioOsigConsumoUlDTO> entitys)
        {
            dbProvider.AddColumnMapping(helper.Psiclicodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Ulconcodempresa, DbType.String);
            dbProvider.AddColumnMapping(helper.Ulconcodsuministro, DbType.String);
            dbProvider.AddColumnMapping(helper.Ulconfecha, DbType.DateTime);
            dbProvider.AddColumnMapping(helper.Ulconcodbarra, DbType.String);
            dbProvider.AddColumnMapping(helper.Ulconenergactv, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Ulconenergreac, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Ptomedicodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Ulconusucreacion, DbType.String);
            dbProvider.AddColumnMapping(helper.Ulconfeccreacion, DbType.DateTime);
            

            dbProvider.BulkInsert<IioOsigConsumoUlDTO>(entitys, helper.TableName);
        }

        public void SaveOsigConsumo(string usuario)
        {
            var query = string.Format(helper.SqlSaveOsigConsumo, usuario);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int psiclicodi, string empresa)
        {
            var query = string.Format(helper.SqlDelete, psiclicodi, empresa);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            dbProvider.ExecuteNonQuery(command);
        }
        
        public void UpdatePtoMediCodiOsigConsumo(int emprcodisuministrador, int psiclicodi, string empresa)
        {
            //Actualizamos la columna PTOMEDICODI 
            //string condicion = "";

            //if (!string.IsNullOrEmpty(empresa))
            //{
            //    condicion = " AND tmp.ULCONCODEMPRESA IN (" + empresa + ")";
            //}
            
            string queryString = string.Format(helper.SqlUpdateOsigConsumo, empresa);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            //dbProvider.AddInParameter(command, helper.Emprcodisuministrador, DbType.Int32, emprcodisuministrador);
            dbProvider.AddInParameter(command, helper.Psiclicodi, DbType.Int32, psiclicodi);
            //dbProvider.AddInParameter(command, helper.Ulconcodempresa, DbType.String, empresa);
            dbProvider.ExecuteNonQuery(command);
        }
        public string ValidarMigracionMeMedicion96(int psiclicodi, string empresa)
        {
            string rpta = "";
            string queryString = string.Empty;
            DbCommand command = null;

            //- HDT Fin

            //Verificamos que todos los puntos de medición esten registrados
            queryString = helper.SqlGetSumucodi;
            if (!string.IsNullOrEmpty(empresa))
            {
                queryString = string.Format(queryString, " AND ULCONCODEMPRESA = '" + empresa + "'");
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
                    IioOsigConsumoUlDTO entity = new IioOsigConsumoUlDTO();
                    int iSumucodi = dr.GetOrdinal(helper.Ulconcodsuministro);
                    if (!dr.IsDBNull(iSumucodi)) entity.Ulconcodsuministro = dr.GetString(iSumucodi);

                    if (rpta == "")
                    {
                        rpta = entity.Ulconcodsuministro;
                    }
                    else
                    {
                        rpta = rpta + ", " + entity.Ulconcodsuministro;
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

        public void MigrateMeMedicion96(int lectCodi, int tipoInfoCodi, string periodo, int psiclicodi, string empresa)
        {            
            string queryString = string.Empty;
            DbCommand command = null;

            var whereQuery = "";
            if (!string.IsNullOrEmpty(empresa))
            {
                whereQuery = " AND ULCONCODEMPRESA IN (" + empresa + ") ";
            }
            
            //Eliminamos los registros de la tabla ME_MEDICION96
            var listaParam = new object[] { lectCodi, tipoInfoCodi, periodo, psiclicodi, whereQuery };
            
            queryString = string.Format(helper.SqlDeleteMeMedicion96, lectCodi, tipoInfoCodi, periodo);
            command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.ExecuteNonQuery(command);

            //Insertamos en la tabla ME_MEDICION96
            
            queryString = string.Format(helper.SqlSaveMedicion96, listaParam);
            command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.ExecuteNonQuery(command);
           
        }

        public void GenerarOsigConsumoLogImportacionPtoMedicion(int psiclicodi, string periodo, string usuario, string tabla, string empresas)
        {

            var maxId = GetCorrelativoDisponibleLogImportacionSicli();

            var paramsQuery = new object[] { maxId, usuario, periodo, tabla, empresas };

            var query = string.Format(helper.SqlRegistrarLogimportacionPtoMedicion, paramsQuery);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            dbProvider.AddInParameter(command, helper.Psiclicodi, DbType.Int32, psiclicodi);
            dbProvider.ExecuteNonQuery(command);
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

    }
}