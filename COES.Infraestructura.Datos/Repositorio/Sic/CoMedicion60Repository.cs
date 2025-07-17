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
    /// Clase de acceso a datos de la tabla CO_MEDICION60
    /// </summary>
    public class CoMedicion60Repository: RepositoryBase, ICoMedicion60Repository
    {
        public CoMedicion60Repository(string strConn): base(strConn)
        {
        }

        CoMedicion60Helper helper = new CoMedicion60Helper();

        public int Save(CoMedicion60DTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Comedicodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.H56, DbType.Decimal, entity.H56);
            dbProvider.AddInParameter(command, helper.H57, DbType.Decimal, entity.H57);
            dbProvider.AddInParameter(command, helper.H58, DbType.Decimal, entity.H58);
            dbProvider.AddInParameter(command, helper.H59, DbType.Decimal, entity.H59);
            dbProvider.AddInParameter(command, helper.H60, DbType.Decimal, entity.H60);
            dbProvider.AddInParameter(command, helper.H55, DbType.Decimal, entity.H55);
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
            dbProvider.AddInParameter(command, helper.H49, DbType.Decimal, entity.H49);
            dbProvider.AddInParameter(command, helper.H50, DbType.Decimal, entity.H50);
            dbProvider.AddInParameter(command, helper.H51, DbType.Decimal, entity.H51);
            dbProvider.AddInParameter(command, helper.H52, DbType.Decimal, entity.H52);
            dbProvider.AddInParameter(command, helper.H53, DbType.Decimal, entity.H53);
            dbProvider.AddInParameter(command, helper.H54, DbType.Decimal, entity.H54);
            dbProvider.AddInParameter(command, helper.Prodiacodi, DbType.Int32, entity.Prodiacodi);
            dbProvider.AddInParameter(command, helper.Cotidacodi, DbType.Int32, entity.Cotidacodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Canalcodi, DbType.Int32, entity.Canalcodi);
            dbProvider.AddInParameter(command, helper.Comedihora, DbType.Int32, entity.Comedihora);
            dbProvider.AddInParameter(command, helper.Comediminuto, DbType.Int32, entity.Comediminuto);
            dbProvider.AddInParameter(command, helper.Comeditipo, DbType.Int32, entity.Comeditipo);
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

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public int Save(CoMedicion60DTO entity, IDbConnection connection, IDbTransaction transaction)
        {
            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = (DbTransaction)transaction;
                dbCommand.Connection = (DbConnection)connection;
                dbCommand.CommandText = helper.SqlGetMaxId;

                object result = dbCommand.ExecuteScalar();
                int id = result == null ? 1 : Convert.ToInt32(result);

                dbCommand.CommandText = helper.SqlSave;

                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Comedicodi, DbType.Int32, id));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Prodiacodi, DbType.Int32, entity.Prodiacodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Cotidacodi, DbType.Int32, entity.Cotidacodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Grupocodi, DbType.Int32, entity.Grupocodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Equicodi, DbType.Int32, entity.Equicodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Canalcodi, DbType.Int32, entity.Canalcodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Comedihora, DbType.Int32, entity.Comedihora));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Comediminuto, DbType.Int32, entity.Comediminuto));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Comeditipo, DbType.Int32, entity.Comeditipo));
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
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.H49, DbType.Decimal, entity.H49));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.H50, DbType.Decimal, entity.H50));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.H51, DbType.Decimal, entity.H51));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.H52, DbType.Decimal, entity.H52));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.H53, DbType.Decimal, entity.H53));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.H54, DbType.Decimal, entity.H54));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.H55, DbType.Decimal, entity.H55));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.H56, DbType.Decimal, entity.H56));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.H57, DbType.Decimal, entity.H57));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.H58, DbType.Decimal, entity.H58));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.H59, DbType.Decimal, entity.H59));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.H60, DbType.Decimal, entity.H60));
                
                
                


                dbCommand.ExecuteNonQuery();
                return id;
            }
        }

        public void GrabarDatosXBloquesMed60(List<CoMedicion60DTO> entitys)
        {
            dbProvider.AddColumnMapping(helper.Comedicodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Prodiacodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Cotidacodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Grupocodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Equicodi, DbType.Int32);
            

            dbProvider.AddColumnMapping(helper.H1, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H2, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H3, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H4, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H5, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H6, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H7, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H8, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H9, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H10, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H11, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H12, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H13, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H14, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H15, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H16, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H17, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H18, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H19, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H20, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H21, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H22, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H23, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H24, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H25, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H26, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H27, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H28, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H29, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H30, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H31, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H32, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H33, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H34, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H35, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H36, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H37, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H38, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H39, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H40, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H41, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H42, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H43, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H44, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H45, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H46, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H47, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H48, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H49, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H50, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H51, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H52, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H53, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H54, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H55, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H56, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H57, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H58, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H59, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H60, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Canalcodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Comedihora, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Comediminuto, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Comeditipo, DbType.Int32);

            dbProvider.BulkInsertRSF<CoMedicion60DTO>(entitys, helper.TableName);
        }

       
        public void GrabarDataXBloquesMed60(List<CoMedicion60DTO> entitys, string nombTabla)
        {
            //dbProvider.AddColumnMapping(helper.Comedicodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Prodiacodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Cotidacodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Grupocodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Equicodi, DbType.Int32);


            dbProvider.AddColumnMapping(helper.H1, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H2, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H3, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H4, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H5, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H6, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H7, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H8, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H9, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H10, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H11, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H12, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H13, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H14, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H15, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H16, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H17, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H18, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H19, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H20, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H21, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H22, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H23, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H24, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H25, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H26, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H27, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H28, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H29, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H30, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H31, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H32, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H33, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H34, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H35, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H36, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H37, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H38, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H39, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H40, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H41, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H42, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H43, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H44, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H45, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H46, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H47, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H48, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H49, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H50, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H51, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H52, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H53, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H54, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H55, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H56, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H57, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H58, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H59, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.H60, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Canalcodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Comedihora, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Comediminuto, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Comeditipo, DbType.Int32);

            dbProvider.BulkInsertRSF<CoMedicion60DTO>(entitys, nombTabla);
        }

        
        public void Update(CoMedicion60DTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.H56, DbType.Decimal, entity.H56);
            dbProvider.AddInParameter(command, helper.H57, DbType.Decimal, entity.H57);
            dbProvider.AddInParameter(command, helper.H58, DbType.Decimal, entity.H58);
            dbProvider.AddInParameter(command, helper.H59, DbType.Decimal, entity.H59);
            dbProvider.AddInParameter(command, helper.H60, DbType.Decimal, entity.H60);
            dbProvider.AddInParameter(command, helper.H55, DbType.Decimal, entity.H55);
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
            dbProvider.AddInParameter(command, helper.H49, DbType.Decimal, entity.H49);
            dbProvider.AddInParameter(command, helper.H50, DbType.Decimal, entity.H50);
            dbProvider.AddInParameter(command, helper.H51, DbType.Decimal, entity.H51);
            dbProvider.AddInParameter(command, helper.H52, DbType.Decimal, entity.H52);
            dbProvider.AddInParameter(command, helper.H53, DbType.Decimal, entity.H53);
            dbProvider.AddInParameter(command, helper.H54, DbType.Decimal, entity.H54);
            dbProvider.AddInParameter(command, helper.Comedicodi, DbType.Int32, entity.Comedicodi);
            dbProvider.AddInParameter(command, helper.Prodiacodi, DbType.Int32, entity.Prodiacodi);
            dbProvider.AddInParameter(command, helper.Cotidacodi, DbType.Int32, entity.Cotidacodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Canalcodi, DbType.Int32, entity.Canalcodi);
            dbProvider.AddInParameter(command, helper.Comedihora, DbType.Int32, entity.Comedihora);
            dbProvider.AddInParameter(command, helper.Comediminuto, DbType.Int32, entity.Comediminuto);
            dbProvider.AddInParameter(command, helper.Comeditipo, DbType.Int32, entity.Comeditipo);
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

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int comedicodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Comedicodi, DbType.Int32, comedicodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CoMedicion60DTO GetById(int comedicodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Comedicodi, DbType.Int32, comedicodi);
            CoMedicion60DTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CoMedicion60DTO> List()
        {
            List<CoMedicion60DTO> entitys = new List<CoMedicion60DTO>();
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

        public List<CoMedicion60DTO> GetByCriteria()
        {
            List<CoMedicion60DTO> entitys = new List<CoMedicion60DTO>();
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

        public List<CoMedicion60DTO> ListarUltimoMinutoPorRango(string rangoDias, string lstStrCanalcodis)
        {
            List<CoMedicion60DTO> entitys = new List<CoMedicion60DTO>();
            string sql = string.Format(helper.SqlListarUltimoMinutoPorRango, rangoDias, lstStrCanalcodis);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            CoMedicion60DTO entity = new CoMedicion60DTO();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iFecha = dr.GetOrdinal(helper.Fecha);
                    if (!dr.IsDBNull(iFecha)) entity.Fecha = dr.GetDateTime(iFecha);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<CoMedicion60DTO> ListarUltimoMinutoDiaAnteriorMuchasTablas(string query)
        {
            List<CoMedicion60DTO> entitys = new List<CoMedicion60DTO>();
            string sql = string.Format(helper.SqlListarUltimoMinutoDiaAnteriorMuchasTablas, query);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            CoMedicion60DTO entity = new CoMedicion60DTO();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iFecha = dr.GetOrdinal(helper.Fecha);
                    if (!dr.IsDBNull(iFecha)) entity.Fecha = dr.GetDateTime(iFecha);                    

                    int iCovercodi = dr.GetOrdinal(helper.Covercodi);
                    if (!dr.IsDBNull(iCovercodi)) entity.Covercodi = Convert.ToInt32(dr.GetValue(iCovercodi));

                    

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int GetMaximoID()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }
        
        public void EliminarMedicion60XTabla(string nombTabla, string listaProdiacodis)
        {
            string query = string.Format(helper.SqlEliminarMedicion60XTabla, nombTabla, listaProdiacodis);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            dbProvider.ExecuteNonQuery(command);

        }
        
        public List<CoMedicion60DTO> ObtenerDataReporte(int prodiacodi, string canalcodis, string tiposdatosids, string nombreTabla)
        {
            List<CoMedicion60DTO> entitys = new List<CoMedicion60DTO>();
            string sql = string.Format(helper.SqlObtenerDataReporte, prodiacodi, canalcodis, tiposdatosids, nombreTabla);
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

        public List<CoMedicion60DTO> ObtenerDataReporteFP(int prodiacodi, int idUrs, int cotidacodi, string nombreTabla)
        {
            List<CoMedicion60DTO> entitys = new List<CoMedicion60DTO>();
            string sql = string.Format(helper.SqlObtenerDataReporteFP, prodiacodi, idUrs, cotidacodi, nombreTabla);
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

        public List<CoMedicion60DTO> ListarTablas(string tablas)
        {
            List<CoMedicion60DTO> entitys = new List<CoMedicion60DTO>();
            String sql = String.Format(helper.SqlListarTablas, tablas);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CoMedicion60DTO entity = new CoMedicion60DTO();

                    int iTablanomb = dr.GetOrdinal(this.helper.Tablanomb);
                    if (!dr.IsDBNull(iTablanomb)) entity.Tablanomb = dr.GetString(iTablanomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<CoMedicion60DTO> ObtenerDataReporteFU(int prodiacodi, string nombreTabla, int tipo)
        {

            List<CoMedicion60DTO> entitys = new List<CoMedicion60DTO>();
            string sql = string.Format(helper.SqlObtenerDataReporteFU, prodiacodi, nombreTabla, tipo);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CoMedicion60DTO entity = new CoMedicion60DTO();

                    int iComedihora = dr.GetOrdinal(helper.Comedihora);
                    if (!dr.IsDBNull(iComedihora)) entity.Comedihora = Convert.ToInt32(dr.GetValue(iComedihora));

                    int iComediminuto = dr.GetOrdinal(helper.Comediminuto);
                    if (!dr.IsDBNull(iComediminuto)) entity.Comediminuto = Convert.ToInt32(dr.GetValue(iComediminuto));

                    for (int i = 1; i <= 60; i++)
                    {
                        decimal valorSuma = 0;
                        int iSuma = dr.GetOrdinal("H" + i);
                        if (!dr.IsDBNull(iSuma)) valorSuma = dr.GetDecimal(iSuma);
                        entity.GetType().GetProperty("H" + i).SetValue(entity, valorSuma);                       
                    }

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        #region Creacion de Tablas Por Periodos o Versiones


        public void CrearTabla(int i, string nombre)
        {

            string sql = "";

            switch (i)
            {
                case 0:
                    sql = string.Format(this.helper.SqlCrearTabla, nombre);
                    break;
                case 1:
                    sql = string.Format(this.helper.SqlCrearIndex, nombre);
                    break;
                case 2:
                    sql = string.Format(this.helper.SqlCrearPKMedicion60, nombre);
                    break;
                case 3:
                    sql = string.Format(this.helper.SqlCrearFKProcesoDiario, nombre);
                    break;
                case 4:
                    sql = string.Format(this.helper.SqlCrearFKGrupo, nombre);
                    break;
                case 5:
                    sql = string.Format(this.helper.SqlCrearFKEquipo, nombre);
                    break;
                case 6:
                    sql = string.Format(this.helper.SqlCrearFKTipoDato, nombre);
                    break;
                case 7:
                    sql = string.Format(this.helper.SqlCrearFKCanalSP7, nombre);
                    break;
                case 8:
                    sql = string.Format(this.helper.SqlCrearIndiceBusueda, nombre);
                    break;
            }

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            dbProvider.ExecuteScalar(command);
        }

        #endregion


        public void EliminarDataTabla(string nombreTabla)
        {
            string query = string.Format(helper.SqlEliminarTodaDataMed60, nombreTabla);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            dbProvider.ExecuteNonQuery(command);

        }

        public int ObtenerIdInicial(string nombTabla)
        {
            String sql = String.Format(helper.SqlObtenerIdInicial, nombTabla);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }


        public int VerificarExistenciaTabla(string nombre)
        {
            string sql = string.Format(helper.SqlVerificarExisttenciaTabla, nombre);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            object result = dbProvider.ExecuteScalar(command);

            if (result != null)
            {
                return Convert.ToInt32(result);
            }
            return 0;
        }

        public List<CoMedicion60DTO> GetInformacionAGC(DateTime fechaConsulta, string strfecha, string gruposcodi, string equicodis, string cotidacodis)
        {
            string nombTabla = fechaConsulta.Year.ToString() + fechaConsulta.Month.ToString("00");

            List<CoMedicion60DTO> entitys = new List<CoMedicion60DTO>();
            string sql = string.Format(helper.SqlGetInformacionAGC, nombTabla, strfecha, gruposcodi, equicodis, cotidacodis);
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

        public  void ProcesarTabla(string tableName, string prodiasCodi)
        {
            string query = string.Format(helper.SqlProcesarTabla, tableName, prodiasCodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            dbProvider.ExecuteNonQuery(command);
        }
    }
}
