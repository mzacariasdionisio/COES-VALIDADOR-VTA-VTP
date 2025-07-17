using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Transferencias;
using COES.Infraestructura.Datos.Helper.Transferencias;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Repositorio.Transferencias
{
    // ASSETEC 2019-11
    /// <summary>
    /// Clase que contiene las operaciones con las tablas TRN_MODELO y TRN_MODELO_RETIRO
    /// </summary>
    public class TrnModeloRepository : RepositoryBase, ITrnModeloRepository
    {
        public TrnModeloRepository(string strConn) : base(strConn)
        {

        }

        TrnModeloHelper helper = new TrnModeloHelper();

        #region Trn_Modelo

        #region Metodos Basicos Trn_Modelo
        public int SaveTrnModelo(TrnModeloDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSaveTrnModelo);

            dbProvider.AddInParameter(command, helper.TrnModCodi, DbType.Int32, GetMaxIdTrnModelo());
            dbProvider.AddInParameter(command, helper.TrnModNombre, DbType.String, entity.TrnModNombre);           
            dbProvider.AddInParameter(command, helper.EmprCodi, DbType.Int32, entity.EmprCodi);           
            dbProvider.AddInParameter(command, helper.TrnModUseIns, DbType.String, entity.TrnModUseIns);
            dbProvider.AddInParameter(command, helper.TrnModFecIns, DbType.DateTime, entity.TrnModFecIns);

            return dbProvider.ExecuteNonQuery(command);
        }

        public int UpdateTrnModelo(TrnModeloDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdateTrnModelo);
            
            dbProvider.AddInParameter(command, helper.TrnModNombre, DbType.String, entity.TrnModNombre);
            dbProvider.AddInParameter(command, helper.EmprCodi, DbType.Int32, entity.EmprCodi);
            dbProvider.AddInParameter(command, helper.TrnModUseIns, DbType.String, entity.TrnModUseAct);
            dbProvider.AddInParameter(command, helper.TrnModFecIns, DbType.DateTime, entity.TrnModFecAct);
            dbProvider.AddInParameter(command, helper.TrnModCodi, DbType.Int32, entity.TrnModCodi);

            return dbProvider.ExecuteNonQuery(command);
        }

        public int DeleteTrnModelo(int id)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteTrnModelo);

            dbProvider.AddInParameter(command, helper.TrnModCodi, DbType.Int32, id);

            return dbProvider.ExecuteNonQuery(command);
        }

        public List<TrnModeloDTO> ListTrnModelo()
        {
            List<TrnModeloDTO> entitys = new List<TrnModeloDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListTrnModelo);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateListTrnModelo(dr));
                }
            }

            return entitys;
        }

        public List<TrnModeloDTO> ListTrnModeloByEmpresa(int emprcodi)
        {
            List<TrnModeloDTO> entitys = new List<TrnModeloDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListTrnModeloByEmpresa);
            dbProvider.AddInParameter(command, helper.EmprCodi, DbType.Int32, emprcodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateListTrnModelo(dr));
                }
            }

            return entitys;
        }
        #endregion

        #region Metodos Adicionales Trn_Modelo
        public List<TrnModeloDTO> ListarComboTrnModelo()
        {
            List<TrnModeloDTO> entitys = new List<TrnModeloDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListarComboTrnModelo);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateListarComboTrnModelo(dr));
                }
            }

            return entitys;
        }

        private int GetMaxIdTrnModelo()
        {
            int newId = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxIdTrnModelo);
            newId = Int32.Parse(dbProvider.ExecuteScalar(command).ToString());

            return newId;
        }
        #endregion

        #endregion

        #region Trn_Modelo_Retiro

        #region Metodos Basicos Trn_Modelo_Retiro
        public int SaveTrnModeloRetiro(TrnModeloRetiroDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSaveTrnModeloRetiro);

            dbProvider.AddInParameter(command, helper.TrnMreCodi, DbType.Int32, GetMaxIdTrnModeloRetiro());
            dbProvider.AddInParameter(command, helper.TrnModCodi, DbType.Int32, entity.TrnModCodi);
            dbProvider.AddInParameter(command, helper.BarrCodi, DbType.Int32, entity.BarrCodi);
            dbProvider.AddInParameter(command, helper.CoresoCodi, DbType.Int32, entity.CoresoCodi);
            dbProvider.AddInParameter(command, helper.CoresoCodigo, DbType.String, entity.CoresoCodigo);
            dbProvider.AddInParameter(command, helper.TrnModRetUseIns, DbType.String, entity.TrnModRetUseIns);
            dbProvider.AddInParameter(command, helper.TrnModRetFecIns, DbType.DateTime, entity.TrnModRetFecIns);

            return dbProvider.ExecuteNonQuery(command);
        }

        public int UpdateTrnModeloRetiro(TrnModeloRetiroDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdateTrnModeloRetiro);

            dbProvider.AddInParameter(command, helper.TrnModCodi, DbType.Int32, entity.TrnModCodi);
            dbProvider.AddInParameter(command, helper.BarrCodi, DbType.Int32, entity.BarrCodi);
            dbProvider.AddInParameter(command, helper.CoresoCodi, DbType.Int32, entity.CoresoCodi);
            dbProvider.AddInParameter(command, helper.CoresoCodigo, DbType.String, entity.CoresoCodigo);
            dbProvider.AddInParameter(command, helper.TrnModRetUseIns, DbType.String, entity.TrnModRetUseAct);
            dbProvider.AddInParameter(command, helper.TrnModRetFecIns, DbType.DateTime, entity.TrnModRetFecAct);
            dbProvider.AddInParameter(command, helper.TrnMreCodi, DbType.Int32, entity.TrnMreCodi);

            return dbProvider.ExecuteNonQuery(command);
        }

        public int DeleteTrnModeloRetiro(int id)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteTrnModeloRetiro);

            dbProvider.AddInParameter(command, helper.TrnMreCodi, DbType.Int32, id);

            return dbProvider.ExecuteNonQuery(command);
        }

        public List<TrnModeloRetiroDTO> ListTrnModeloRetiro(int idModelo)
        {
            List<TrnModeloRetiroDTO> entitys = new List<TrnModeloRetiroDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListTrnModeloRetiro);

            dbProvider.AddInParameter(command, helper.TrnModCodi, DbType.Int32, idModelo);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateListTrnModeloRetiro(dr));
                }
            }

            return entitys;
        }
        #endregion

        #region Metodos Adicionales Trn_Modelo_Retiro
        public List<BarraDTO> ListarComboBarras()
        {
            List<BarraDTO> entitys = new List<BarraDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListarComboBarras);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateListarComboBarras(dr));
                }
            }

            return entitys;
        }

        public List<CodigoRetiroDTO> ListComboCodigoSolicitudRetiro(int idBarra)
        {
            List<CodigoRetiroDTO> entitys = new List<CodigoRetiroDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListComboCodigoSolicitudRetiro);

            dbProvider.AddInParameter(command, helper.BarrCodi, DbType.Int32, idBarra);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateListComboCodigoSolicitudRetiro(dr));
                }
            }

            return entitys;
        }

        public bool TieneCodigosRetiroModelo(int idModelo)
        {
            bool rpta = false; // No existe el codigo de seguimiento

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlTieneCodigosRetiroModelo);

            dbProvider.AddInParameter(command, helper.TrnModCodi, DbType.Int32, idModelo);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    int nroReg = Convert.ToInt32(dr["nroregistros"].ToString().Trim());

                    if (nroReg > 0)
                    {
                        rpta = true; // Si tiene registros
                    }
                }
            }

            return rpta;
        }

        private int GetMaxIdTrnModeloRetiro()
        {
            int newId = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxIdTrnModeloRetiro);
            newId = Int32.Parse(dbProvider.ExecuteScalar(command).ToString());

            return newId;
        }
        #endregion

        #endregion
    }
}
