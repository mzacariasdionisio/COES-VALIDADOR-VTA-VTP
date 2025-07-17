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
    public class IioTmpConsumoRepository : RepositoryBase, IIioTmpConsumoRepository
    {
        private readonly IioTmpConsumoHelper helper = new IioTmpConsumoHelper();

        public IioTmpConsumoRepository(string strConn)
            : base(strConn)
        {

        }

        public void BulkInsert(List<IioTmpConsumoDTO> entitys)
        {
            dbProvider.AddColumnMapping(helper.Psiclicodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Uconempcodi, DbType.String);
            dbProvider.AddColumnMapping(helper.Sumucodi, DbType.String);
            dbProvider.AddColumnMapping(helper.Uconfecha, DbType.DateTime);
            dbProvider.AddColumnMapping(helper.Uconptosumincodi, DbType.String);
            dbProvider.AddColumnMapping(helper.Uconenergactv, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Uconenergreac, DbType.Decimal);

            dbProvider.BulkInsert<IioTmpConsumoDTO>(entitys, helper.TableName);
        }

        public void Delete()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);
            dbProvider.ExecuteNonQuery(command);
        }

        //- alpha.HDT - 10/04/2017: Cambio para atender el requerimiento. 
        public void UpdatePtoMediCodiTmpConsumo()
        {
            //Actualizamos la columna PTOMEDICODI 
            string queryString = string.Format(helper.SqlUpdateTmpConsumo);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            dbProvider.ExecuteNonQuery(command);
        }

        public string MigrateMeMedicion96(int lectCodi, int tipoInfoCodi, string periodo)
        {
            string rpta = "";

            //- alpha.HDT - Inicio 10/04/2017: Cambio para atender el requerimiento: este fragmento ahora se encuentra en el método UpdatePtoMediCodiTmpConsumo()
            ////Actualizamos la columna PTOMEDICODI 
            //string queryString = string.Format(helper.SqlUpdateTmpConsumo);
            //DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            //dbProvider.ExecuteNonQuery(command);

            string queryString = string.Empty;
            DbCommand command = null;

            //- HDT Fin

            //Verificamos que todos los puntos de medición esten registrados
            queryString = string.Format(helper.SqlGetSumucodi);
            command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.ExecuteNonQuery(command);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    IioTmpConsumoDTO entity = new IioTmpConsumoDTO();
                    int iSumucodi = dr.GetOrdinal("Sumucodi");
                    if (!dr.IsDBNull(iSumucodi)) entity.Sumucodi = dr.GetString(iSumucodi);

                    if (rpta == "")
                    {
                        rpta = entity.Sumucodi;
                    }
                    else
                    {
                        rpta = rpta + ", " + entity.Sumucodi;
                    }
                }
            }

            if (!rpta.Equals(""))
            {
                return rpta;
            }

            //Eliminamos los registros de la tabla ME_MEDICION96
            queryString = string.Format(helper.SqlDeleteMeMedicion96, lectCodi, tipoInfoCodi, periodo);
            command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.ExecuteNonQuery(command);

            //Insertamos en la tabla ME_MEDICION96
            queryString = string.Format(helper.SqlSaveMedicion96, lectCodi, tipoInfoCodi, periodo);
            command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.ExecuteNonQuery(command);

            //Actualizamos la columna H96
            queryString = string.Format(helper.SqlUpdateH96, lectCodi, tipoInfoCodi, periodo);
            command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.ExecuteNonQuery(command);

            rpta = "OK";

            return rpta;
        }
    }
}