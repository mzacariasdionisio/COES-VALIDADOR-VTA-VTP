using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Infraestructura.Datos.Helper.Sic;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla IND_STOCK_COMBUSTIBLE
    /// </summary>
    public class IndStockCombustibleRepository : RepositoryBase, IIndStockCombustibleRepository
    {
        private string strConexion;
        IndStockCombustibleHelper helper = new IndStockCombustibleHelper();

        public IndStockCombustibleRepository(string strConn) : base(strConn)
        {
            strConexion = strConn;
        }

        public IDbConnection BeginConnection()
        {
            Database db = DatabaseFactory.CreateDatabase(strConexion);
            IDbConnection conn = db.CreateConnection();
            conn.Open();
            return conn;
        }

        public DbTransaction StartTransaction(IDbConnection conn)
        {
            return (DbTransaction) conn.BeginTransaction();
        }

        public int GetMaxId()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            int id = 1;

            object result = dbProvider.ExecuteScalar(command);
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public IndStockCombustibleDTO GetById(int stkcmtcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Stkcmtcodi, DbType.Int32, stkcmtcodi);
            IndStockCombustibleDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.CreateGetById(dr);
                }
            }

            return entity;
        }

        public void Save(IndStockCombustibleDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand) conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection) conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Stkcmtcodi, DbType.Int32, entity.Stkcmtcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ipericodi, DbType.Int32, entity.Ipericodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Equicodicentral, DbType.Int32, entity.Equicodicentral));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Equicodiunidad, DbType.Int32, entity.Equicodiunidad));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Tipoinfocodi, DbType.Int32, entity.Tipoinfocodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Stkcmtusucreacion, DbType.String, entity.Stkcmtusucreacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Stkcmtfeccreacion, DbType.DateTime, entity.Stkcmtfeccreacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Stkcmtusumodificacion, DbType.String, entity.Stkcmtusumodificacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Stkcmtfecmodificacion, DbType.DateTime, entity.Stkcmtfecmodificacion));
            command.ExecuteNonQuery();
        }

        public List<IndStockCombustibleDTO> GetByCriteria(int ipericodi, int emprcodi, int equicodicentral, string equicodiunidad, int tipoinfocodi)
        {
            List<IndStockCombustibleDTO> entitys = new List<IndStockCombustibleDTO>();

            string query = string.Format(helper.SqlGetByCriteria, ipericodi, emprcodi, equicodicentral, equicodiunidad, tipoinfocodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    IndStockCombustibleDTO entity = helper.Create(dr);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<IndStockCombustibleDTO> ListStockByAnioMes(int anio, int mes)
        {
            IndStockCombustibleDTO entity = new IndStockCombustibleDTO();
            List<IndStockCombustibleDTO> entitys = new List<IndStockCombustibleDTO>();
            string query = string.Format(helper.SqlListStockByAnioMes, anio, mes);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new IndStockCombustibleDTO();

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEquicodicentral = dr.GetOrdinal(helper.Equicodicentral);
                    if (!dr.IsDBNull(iEquicodicentral)) entity.Equicodicentral = Convert.ToInt32(dr.GetValue(iEquicodicentral));

                    int iEquicodiunidad = dr.GetOrdinal(helper.Equicodiunidad);
                    if (!dr.IsDBNull(iEquicodiunidad)) entity.Equicodiunidad = Convert.ToInt32(dr.GetValue(iEquicodiunidad));

                    int iTipoinfocodi = dr.GetOrdinal(helper.Tipoinfocodi);
                    if (!dr.IsDBNull(iTipoinfocodi)) entity.Tipoinfocodi = Convert.ToInt32(dr.GetValue(iTipoinfocodi));

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iStkdetcodi = dr.GetOrdinal(helper.Stkdetcodi);
                    if (!dr.IsDBNull(iStkdetcodi)) entity.Stkdetcodi = Convert.ToInt32(dr.GetValue(iStkdetcodi));

                    int iStkcmtcodi= dr.GetOrdinal(helper.Stkcmtcodi);
                    if (!dr.IsDBNull(iStkcmtcodi)) entity.Stkcmtcodi = Convert.ToInt32(dr.GetValue(iStkcmtcodi));

                    int iStkdettipo = dr.GetOrdinal(helper.Stkdettipo);
                    if (!dr.IsDBNull(iStkdettipo)) entity.Stkdettipo = dr.GetString(iStkdettipo);

                    int iD1 = dr.GetOrdinal(helper.D1);
                    if (!dr.IsDBNull(iD1)) entity.D1 = dr.GetString(iD1);

                    int iD2 = dr.GetOrdinal(helper.D2);
                    if (!dr.IsDBNull(iD2)) entity.D2 = dr.GetString(iD2);

                    int iD3 = dr.GetOrdinal(helper.D3);
                    if (!dr.IsDBNull(iD3)) entity.D3 = dr.GetString(iD3);

                    int iD4 = dr.GetOrdinal(helper.D4);
                    if (!dr.IsDBNull(iD4)) entity.D4 = dr.GetString(iD4);

                    int iD5 = dr.GetOrdinal(helper.D5);
                    if (!dr.IsDBNull(iD5)) entity.D5 = dr.GetString(iD5);

                    int iD6 = dr.GetOrdinal(helper.D6);
                    if (!dr.IsDBNull(iD6)) entity.D6 = dr.GetString(iD6);

                    int iD7 = dr.GetOrdinal(helper.D7);
                    if (!dr.IsDBNull(iD7)) entity.D7 = dr.GetString(iD7);

                    int iD8 = dr.GetOrdinal(helper.D8);
                    if (!dr.IsDBNull(iD8)) entity.D8 = dr.GetString(iD8);

                    int iD9 = dr.GetOrdinal(helper.D9);
                    if (!dr.IsDBNull(iD9)) entity.D9 = dr.GetString(iD9);

                    int iD10 = dr.GetOrdinal(helper.D10);
                    if (!dr.IsDBNull(iD10)) entity.D10 = dr.GetString(iD10);

                    int iD11 = dr.GetOrdinal(helper.D11);
                    if (!dr.IsDBNull(iD11)) entity.D11 = dr.GetString(iD11);

                    int iD12 = dr.GetOrdinal(helper.D12);
                    if (!dr.IsDBNull(iD12)) entity.D12 = dr.GetString(iD12);

                    int iD13 = dr.GetOrdinal(helper.D13);
                    if (!dr.IsDBNull(iD13)) entity.D13 = dr.GetString(iD13);

                    int iD14 = dr.GetOrdinal(helper.D14);
                    if (!dr.IsDBNull(iD14)) entity.D14 = dr.GetString(iD14);

                    int iD15 = dr.GetOrdinal(helper.D15);
                    if (!dr.IsDBNull(iD15)) entity.D15 = dr.GetString(iD15);

                    int iD16 = dr.GetOrdinal(helper.D16);
                    if (!dr.IsDBNull(iD16)) entity.D16 = dr.GetString(iD16);

                    int iD17 = dr.GetOrdinal(helper.D17);
                    if (!dr.IsDBNull(iD17)) entity.D17 = dr.GetString(iD17);

                    int iD18 = dr.GetOrdinal(helper.D18);
                    if (!dr.IsDBNull(iD18)) entity.D18 = dr.GetString(iD18);

                    int iD19 = dr.GetOrdinal(helper.D19);
                    if (!dr.IsDBNull(iD19)) entity.D19 = dr.GetString(iD19);

                    int iD20 = dr.GetOrdinal(helper.D20);
                    if (!dr.IsDBNull(iD20)) entity.D20 = dr.GetString(iD20);

                    int iD21 = dr.GetOrdinal(helper.D21);
                    if (!dr.IsDBNull(iD21)) entity.D21 = dr.GetString(iD21);

                    int iD22 = dr.GetOrdinal(helper.D22);
                    if (!dr.IsDBNull(iD22)) entity.D22 = dr.GetString(iD22);

                    int iD23 = dr.GetOrdinal(helper.D23);
                    if (!dr.IsDBNull(iD23)) entity.D23 = dr.GetString(iD23);

                    int iD24 = dr.GetOrdinal(helper.D24);
                    if (!dr.IsDBNull(iD24)) entity.D24 = dr.GetString(iD24);

                    int iD25 = dr.GetOrdinal(helper.D25);
                    if (!dr.IsDBNull(iD25)) entity.D25 = dr.GetString(iD25);

                    int iD26 = dr.GetOrdinal(helper.D26);
                    if (!dr.IsDBNull(iD26)) entity.D26 = dr.GetString(iD26);

                    int iD27 = dr.GetOrdinal(helper.D27);
                    if (!dr.IsDBNull(iD27)) entity.D27 = dr.GetString(iD27);

                    int iD28 = dr.GetOrdinal(helper.D28);
                    if (!dr.IsDBNull(iD28)) entity.D28 = dr.GetString(iD28);

                    int iD29 = dr.GetOrdinal(helper.D29);
                    if (!dr.IsDBNull(iD29)) entity.D29 = dr.GetString(iD29);

                    int iD30 = dr.GetOrdinal(helper.D30);
                    if (!dr.IsDBNull(iD30)) entity.D30 = dr.GetString(iD30);

                    int iD31 = dr.GetOrdinal(helper.D31);
                    if (!dr.IsDBNull(iD31)) entity.D31 = dr.GetString(iD31);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
