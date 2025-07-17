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
    /// Clase de acceso a datos de la tabla SMA_ACTIVACION_DATA
    /// </summary>
    public class SmaActivacionDataRepository: RepositoryBase, ISmaActivacionDataRepository
    {
        public SmaActivacionDataRepository(string strConn): base(strConn)
        {
        }

        SmaActivacionDataHelper helper = new SmaActivacionDataHelper();

        public int Save(SmaActivacionDataDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Smaacdcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Smapaccodi, DbType.Int32, entity.Smapaccodi);
            dbProvider.AddInParameter(command, helper.Smaacdtipodato, DbType.String, entity.Smaacdtipodato);
            dbProvider.AddInParameter(command, helper.Smaacdtiporeserva, DbType.String, entity.Smaacdtiporeserva);
            dbProvider.AddInParameter(command, helper.H1, DbType.Decimal, entity.H1);
            dbProvider.AddInParameter(command, helper.H2, DbType.Decimal, entity.H2);
            dbProvider.AddInParameter(command, helper.H3, DbType.Decimal, entity.H3);
            dbProvider.AddInParameter(command, helper.H4, DbType.Decimal, entity.H4);
            dbProvider.AddInParameter(command, helper.H5, DbType.Decimal, entity.H5);
            dbProvider.AddInParameter(command, helper.H6, DbType.Decimal, entity.H6);
            dbProvider.AddInParameter(command, helper.H7, DbType.Decimal, entity.H7);
            dbProvider.AddInParameter(command, helper.H8, DbType.Decimal, entity.H8);
            dbProvider.AddInParameter(command, helper.H9, DbType.Decimal, entity.H9);
            dbProvider.AddInParameter(command, helper.H10, DbType.Decimal, entity.H10);
            dbProvider.AddInParameter(command, helper.H11, DbType.Decimal, entity.H11);
            dbProvider.AddInParameter(command, helper.H12, DbType.Decimal, entity.H12);
            dbProvider.AddInParameter(command, helper.H13, DbType.Decimal, entity.H13);
            dbProvider.AddInParameter(command, helper.H14, DbType.Decimal, entity.H14);
            dbProvider.AddInParameter(command, helper.H15, DbType.Decimal, entity.H15);
            dbProvider.AddInParameter(command, helper.H16, DbType.Decimal, entity.H16);
            dbProvider.AddInParameter(command, helper.H17, DbType.Decimal, entity.H17);
            dbProvider.AddInParameter(command, helper.H18, DbType.Decimal, entity.H18);
            dbProvider.AddInParameter(command, helper.H19, DbType.Decimal, entity.H19);
            dbProvider.AddInParameter(command, helper.H20, DbType.Decimal, entity.H20);
            dbProvider.AddInParameter(command, helper.H21, DbType.Decimal, entity.H21);
            dbProvider.AddInParameter(command, helper.H22, DbType.Decimal, entity.H22);
            dbProvider.AddInParameter(command, helper.H23, DbType.Decimal, entity.H23);
            dbProvider.AddInParameter(command, helper.H24, DbType.Decimal, entity.H24);
            dbProvider.AddInParameter(command, helper.H25, DbType.Decimal, entity.H25);
            dbProvider.AddInParameter(command, helper.H26, DbType.Decimal, entity.H26);
            dbProvider.AddInParameter(command, helper.H27, DbType.Decimal, entity.H27);
            dbProvider.AddInParameter(command, helper.H28, DbType.Decimal, entity.H28);
            dbProvider.AddInParameter(command, helper.H29, DbType.Decimal, entity.H29);
            dbProvider.AddInParameter(command, helper.H30, DbType.Decimal, entity.H30);
            dbProvider.AddInParameter(command, helper.H31, DbType.Decimal, entity.H31);
            dbProvider.AddInParameter(command, helper.H32, DbType.Decimal, entity.H32);
            dbProvider.AddInParameter(command, helper.H33, DbType.Decimal, entity.H33);
            dbProvider.AddInParameter(command, helper.H34, DbType.Decimal, entity.H34);
            dbProvider.AddInParameter(command, helper.H35, DbType.Decimal, entity.H35);
            dbProvider.AddInParameter(command, helper.H36, DbType.Decimal, entity.H36);
            dbProvider.AddInParameter(command, helper.H37, DbType.Decimal, entity.H37);
            dbProvider.AddInParameter(command, helper.H38, DbType.Decimal, entity.H38);
            dbProvider.AddInParameter(command, helper.H39, DbType.Decimal, entity.H39);
            dbProvider.AddInParameter(command, helper.H40, DbType.Decimal, entity.H40);
            dbProvider.AddInParameter(command, helper.H41, DbType.Decimal, entity.H41);
            dbProvider.AddInParameter(command, helper.H42, DbType.Decimal, entity.H42);
            dbProvider.AddInParameter(command, helper.H43, DbType.Decimal, entity.H43);
            dbProvider.AddInParameter(command, helper.H44, DbType.Decimal, entity.H44);
            dbProvider.AddInParameter(command, helper.H45, DbType.Decimal, entity.H45);
            dbProvider.AddInParameter(command, helper.H46, DbType.Decimal, entity.H46);
            dbProvider.AddInParameter(command, helper.H47, DbType.Decimal, entity.H47);
            dbProvider.AddInParameter(command, helper.H48, DbType.Decimal, entity.H48);
            dbProvider.AddInParameter(command, helper.Smaacdusucreacion, DbType.String, entity.Smaacdusucreacion);
            dbProvider.AddInParameter(command, helper.Smaacdfeccreacion, DbType.DateTime, entity.Smaacdfeccreacion);
            dbProvider.AddInParameter(command, helper.Smaacdusumodificacion, DbType.String, entity.Smaacdusumodificacion);
            dbProvider.AddInParameter(command, helper.Smaacdfecmodificacion, DbType.DateTime, entity.Smaacdfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public int SaveTransaccional(SmaActivacionDataDTO entity, IDbConnection connection, DbTransaction transaction)
        {

            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = transaction;
                dbCommand.Connection = (DbConnection)connection;
                dbCommand.CommandText = helper.SqlGetMaxId;

                object result = dbCommand.ExecuteScalar();
                int id = result == null ? 1 : Convert.ToInt32(result);

                dbCommand.CommandText = helper.SqlSave;

                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Smaacdcodi, DbType.Int32, id));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Smapaccodi, DbType.Int32, entity.Smapaccodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Smaacdtipodato, DbType.String, entity.Smaacdtipodato));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Smaacdtiporeserva, DbType.String, entity.Smaacdtiporeserva));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.H1, DbType.Decimal, entity.H1));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.H2, DbType.Decimal, entity.H2));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.H3, DbType.Decimal, entity.H3));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.H4, DbType.Decimal, entity.H4));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.H5, DbType.Decimal, entity.H5));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.H6, DbType.Decimal, entity.H6));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.H7, DbType.Decimal, entity.H7));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.H8, DbType.Decimal, entity.H8));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.H9, DbType.Decimal, entity.H9));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.H10, DbType.Decimal, entity.H10));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.H11, DbType.Decimal, entity.H11));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.H12, DbType.Decimal, entity.H12));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.H13, DbType.Decimal, entity.H13));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.H14, DbType.Decimal, entity.H14));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.H15, DbType.Decimal, entity.H15));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.H16, DbType.Decimal, entity.H16));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.H17, DbType.Decimal, entity.H17));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.H18, DbType.Decimal, entity.H18));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.H19, DbType.Decimal, entity.H19));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.H20, DbType.Decimal, entity.H20));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.H21, DbType.Decimal, entity.H21));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.H22, DbType.Decimal, entity.H22));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.H23, DbType.Decimal, entity.H23));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.H24, DbType.Decimal, entity.H24));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.H25, DbType.Decimal, entity.H25));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.H26, DbType.Decimal, entity.H26));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.H27, DbType.Decimal, entity.H27));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.H28, DbType.Decimal, entity.H28));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.H29, DbType.Decimal, entity.H29));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.H30, DbType.Decimal, entity.H30));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.H31, DbType.Decimal, entity.H31));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.H32, DbType.Decimal, entity.H32));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.H33, DbType.Decimal, entity.H33));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.H34, DbType.Decimal, entity.H34));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.H35, DbType.Decimal, entity.H35));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.H36, DbType.Decimal, entity.H36));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.H37, DbType.Decimal, entity.H37));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.H38, DbType.Decimal, entity.H38));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.H39, DbType.Decimal, entity.H39));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.H40, DbType.Decimal, entity.H40));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.H41, DbType.Decimal, entity.H41));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.H42, DbType.Decimal, entity.H42));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.H43, DbType.Decimal, entity.H43));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.H44, DbType.Decimal, entity.H44));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.H45, DbType.Decimal, entity.H45));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.H46, DbType.Decimal, entity.H46));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.H47, DbType.Decimal, entity.H47));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.H48, DbType.Decimal, entity.H48));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Smaacdusucreacion, DbType.String, entity.Smaacdusucreacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Smaacdfeccreacion, DbType.DateTime, entity.Smaacdfeccreacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Smaacdusumodificacion, DbType.String, entity.Smaacdusumodificacion));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Smaacdfecmodificacion, DbType.DateTime, entity.Smaacdfecmodificacion));

                dbCommand.ExecuteNonQuery();
                return id;
            }
        }

        public void Update(SmaActivacionDataDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Smaacdcodi, DbType.Int32, entity.Smaacdcodi);
            dbProvider.AddInParameter(command, helper.Smapaccodi, DbType.Int32, entity.Smapaccodi);
            dbProvider.AddInParameter(command, helper.Smaacdtipodato, DbType.String, entity.Smaacdtipodato);
            dbProvider.AddInParameter(command, helper.Smaacdtiporeserva, DbType.String, entity.Smaacdtiporeserva);
            dbProvider.AddInParameter(command, helper.H1, DbType.Decimal, entity.H1);
            dbProvider.AddInParameter(command, helper.H2, DbType.Decimal, entity.H2);
            dbProvider.AddInParameter(command, helper.H3, DbType.Decimal, entity.H3);
            dbProvider.AddInParameter(command, helper.H4, DbType.Decimal, entity.H4);
            dbProvider.AddInParameter(command, helper.H5, DbType.Decimal, entity.H5);
            dbProvider.AddInParameter(command, helper.H6, DbType.Decimal, entity.H6);
            dbProvider.AddInParameter(command, helper.H7, DbType.Decimal, entity.H7);
            dbProvider.AddInParameter(command, helper.H8, DbType.Decimal, entity.H8);
            dbProvider.AddInParameter(command, helper.H9, DbType.Decimal, entity.H9);
            dbProvider.AddInParameter(command, helper.H10, DbType.Decimal, entity.H10);
            dbProvider.AddInParameter(command, helper.H11, DbType.Decimal, entity.H11);
            dbProvider.AddInParameter(command, helper.H12, DbType.Decimal, entity.H12);
            dbProvider.AddInParameter(command, helper.H13, DbType.Decimal, entity.H13);
            dbProvider.AddInParameter(command, helper.H14, DbType.Decimal, entity.H14);
            dbProvider.AddInParameter(command, helper.H15, DbType.Decimal, entity.H15);
            dbProvider.AddInParameter(command, helper.H16, DbType.Decimal, entity.H16);
            dbProvider.AddInParameter(command, helper.H17, DbType.Decimal, entity.H17);
            dbProvider.AddInParameter(command, helper.H18, DbType.Decimal, entity.H18);
            dbProvider.AddInParameter(command, helper.H19, DbType.Decimal, entity.H19);
            dbProvider.AddInParameter(command, helper.H20, DbType.Decimal, entity.H20);
            dbProvider.AddInParameter(command, helper.H21, DbType.Decimal, entity.H21);
            dbProvider.AddInParameter(command, helper.H22, DbType.Decimal, entity.H22);
            dbProvider.AddInParameter(command, helper.H23, DbType.Decimal, entity.H23);
            dbProvider.AddInParameter(command, helper.H24, DbType.Decimal, entity.H24);
            dbProvider.AddInParameter(command, helper.H25, DbType.Decimal, entity.H25);
            dbProvider.AddInParameter(command, helper.H26, DbType.Decimal, entity.H26);
            dbProvider.AddInParameter(command, helper.H27, DbType.Decimal, entity.H27);
            dbProvider.AddInParameter(command, helper.H28, DbType.Decimal, entity.H28);
            dbProvider.AddInParameter(command, helper.H29, DbType.Decimal, entity.H29);
            dbProvider.AddInParameter(command, helper.H30, DbType.Decimal, entity.H30);
            dbProvider.AddInParameter(command, helper.H31, DbType.Decimal, entity.H31);
            dbProvider.AddInParameter(command, helper.H32, DbType.Decimal, entity.H32);
            dbProvider.AddInParameter(command, helper.H33, DbType.Decimal, entity.H33);
            dbProvider.AddInParameter(command, helper.H34, DbType.Decimal, entity.H34);
            dbProvider.AddInParameter(command, helper.H35, DbType.Decimal, entity.H35);
            dbProvider.AddInParameter(command, helper.H36, DbType.Decimal, entity.H36);
            dbProvider.AddInParameter(command, helper.H37, DbType.Decimal, entity.H37);
            dbProvider.AddInParameter(command, helper.H38, DbType.Decimal, entity.H38);
            dbProvider.AddInParameter(command, helper.H39, DbType.Decimal, entity.H39);
            dbProvider.AddInParameter(command, helper.H40, DbType.Decimal, entity.H40);
            dbProvider.AddInParameter(command, helper.H41, DbType.Decimal, entity.H41);
            dbProvider.AddInParameter(command, helper.H42, DbType.Decimal, entity.H42);
            dbProvider.AddInParameter(command, helper.H43, DbType.Decimal, entity.H43);
            dbProvider.AddInParameter(command, helper.H44, DbType.Decimal, entity.H44);
            dbProvider.AddInParameter(command, helper.H45, DbType.Decimal, entity.H45);
            dbProvider.AddInParameter(command, helper.H46, DbType.Decimal, entity.H46);
            dbProvider.AddInParameter(command, helper.H47, DbType.Decimal, entity.H47);
            dbProvider.AddInParameter(command, helper.H48, DbType.Decimal, entity.H48);
            dbProvider.AddInParameter(command, helper.Smaacdusucreacion, DbType.String, entity.Smaacdusucreacion);
            dbProvider.AddInParameter(command, helper.Smaacdfeccreacion, DbType.DateTime, entity.Smaacdfeccreacion);
            dbProvider.AddInParameter(command, helper.Smaacdusumodificacion, DbType.String, entity.Smaacdusumodificacion);
            dbProvider.AddInParameter(command, helper.Smaacdfecmodificacion, DbType.DateTime, entity.Smaacdfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int smaacdcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Smaacdcodi, DbType.Int32, smaacdcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public SmaActivacionDataDTO GetById(int smaacdcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Smaacdcodi, DbType.Int32, smaacdcodi);
            SmaActivacionDataDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SmaActivacionDataDTO> List()
        {
            List<SmaActivacionDataDTO> entitys = new List<SmaActivacionDataDTO>();
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

        public List<SmaActivacionDataDTO> GetByCriteria()
        {
            List<SmaActivacionDataDTO> entitys = new List<SmaActivacionDataDTO>();
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
        public List<SmaActivacionDataDTO> ObtenerPorActivacionesOferta(string smapaccodis)
        {
            List<SmaActivacionDataDTO> entitys = new List<SmaActivacionDataDTO>();

            string query = string.Format(helper.SqlObtenerPorActivacionesOferta, smapaccodis);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SmaActivacionDataDTO entity = helper.Create(dr);

                    //int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    //if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);


                    entitys.Add(entity);
                }
            }

            return entitys;
        }
       

    }
}
