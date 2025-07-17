using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Transferencias;
using COES.Infraestructura.Datos.Helper.Transferencias;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Repositorio.Transferencias
{
    public class CodigoRetiroRelacionEquivalenciasRepository : RepositoryBase, ICodigoRetiroRelacionEquivalenciasRepository
    {
        CodigoRetiroRelacionEquivalenciaHelper helper = new CodigoRetiroRelacionEquivalenciaHelper();
        private string strConexion;
        public CodigoRetiroRelacionEquivalenciasRepository(string strConn) : base(strConn)
        {
            strConexion = strConn;
        }

        public int Save(CodigoRetiroRelacionDTO entity)
        {
            int id = GetCodigoGenerado();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);
            dbProvider.AddInParameter(command, helper.RetRelCodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.RetrelVari, DbType.Int32, entity.Retrelvari);
            dbProvider.AddInParameter(command, helper.RetelEstado, DbType.String, entity.Retelestado);
            dbProvider.AddInParameter(command, helper.RetrelUsuCreacion, DbType.String, entity.Retrelusucreacion);
            dbProvider.AddInParameter(command, helper.RetrelFecCreacion, DbType.Date, DateTime.Now.Date);

            return (dbProvider.ExecuteNonQuery(command) > 0) ? id : 0;
        }

        public int Update(CodigoRetiroRelacionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);
            dbProvider.AddInParameter(command, helper.RetrelVari, DbType.Decimal, entity.Retrelvari);
            dbProvider.AddInParameter(command, helper.RetelEstado, DbType.String, entity.Retelestado);
            dbProvider.AddInParameter(command, helper.RetrelUsuModificacion, DbType.String, entity.Retrelusumodificacion);
            dbProvider.AddInParameter(command, helper.RetrelFecModificacion, DbType.Date, DateTime.Now.Date);
            dbProvider.AddInParameter(command, helper.RetRelCodi, DbType.Int32, entity.RetrelCodi);

            return dbProvider.ExecuteNonQuery(command);
        }

        public int GetCodigoGenerado()
        {
            int newId = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlCodigoGenerado);
            newId = Int32.Parse(dbProvider.ExecuteScalar(command).ToString());

            return newId;
        }


        public List<CodigoRetiroRelacionDetalleDTO> ListarRelacionCodigoRetiros(int nroPagina, int pageSize,
            int? genEmprCodi, int? cliEmprCodi, int? barrCodiTra, int? barrCodiSum, int? tipConCodi, int? tipUsuCodi, string estado, string codigo)
        {
            List<CodigoRetiroRelacionDetalleDTO> entitys = new List<CodigoRetiroRelacionDetalleDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListarRelacionCodigoRetiros);

            dbProvider.AddInParameter(command, helper.Genemprcodi, DbType.Int32, genEmprCodi);
            dbProvider.AddInParameter(command, helper.Genemprcodi, DbType.Int32, genEmprCodi);
            dbProvider.AddInParameter(command, helper.Genemprcodi, DbType.Int32, genEmprCodi);
            dbProvider.AddInParameter(command, helper.Cliemprcodi, DbType.Int32, cliEmprCodi);
            dbProvider.AddInParameter(command, helper.Cliemprcodi, DbType.Int32, cliEmprCodi);
            dbProvider.AddInParameter(command, helper.Cliemprcodi, DbType.Int32, cliEmprCodi);
            dbProvider.AddInParameter(command, helper.Barrcoditra, DbType.Int32, barrCodiTra);
            dbProvider.AddInParameter(command, helper.Barrcoditra, DbType.Int32, barrCodiTra);
            dbProvider.AddInParameter(command, helper.Barrcodisum, DbType.Int32, barrCodiSum);
            dbProvider.AddInParameter(command, helper.Barrcodisum, DbType.Int32, barrCodiSum);
            dbProvider.AddInParameter(command, helper.Tipconcodi, DbType.Int32, tipConCodi);
            dbProvider.AddInParameter(command, helper.Tipconcodi, DbType.Int32, tipConCodi);
            dbProvider.AddInParameter(command, helper.Tipconcodi, DbType.Int32, tipConCodi);
            dbProvider.AddInParameter(command, helper.Tipusucodi, DbType.Int32, tipUsuCodi);
            dbProvider.AddInParameter(command, helper.Tipusucodi, DbType.Int32, tipUsuCodi);
            dbProvider.AddInParameter(command, helper.Tipusucodi, DbType.Int32, tipUsuCodi);
            dbProvider.AddInParameter(command, helper.Codigo, DbType.String, string.IsNullOrEmpty(codigo) ? null : codigo);
            dbProvider.AddInParameter(command, helper.Codigo, DbType.String, string.IsNullOrEmpty(codigo) ? null : codigo);
            dbProvider.AddInParameter(command, helper.Codigo, DbType.String, string.IsNullOrEmpty(codigo) ? null : codigo);
            dbProvider.AddInParameter(command, helper.RetelEstado, DbType.String, string.IsNullOrEmpty(estado) ? "ACT" : estado);
            dbProvider.AddInParameter(command, helper.NroPagina, DbType.Int32, nroPagina);
            dbProvider.AddInParameter(command, helper.PageSize, DbType.Int32, pageSize);
            dbProvider.AddInParameter(command, helper.NroPagina, DbType.Int32, nroPagina);
            dbProvider.AddInParameter(command, helper.PageSize, DbType.Int32, pageSize);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                { 
                    
                    CodigoRetiroRelacionDetalleDTO entity = new CodigoRetiroRelacionDetalleDTO();
                    entity = helper.CreateDetalle(dr);

                    int iGenemprnombvtea = dr.GetOrdinal(helper.Genemprnombvtea);
                    if (!dr.IsDBNull(iGenemprnombvtea)) entity.Genemprnombvtea = dr.GetString(iGenemprnombvtea);

                    int iCliemprnombvtea = dr.GetOrdinal(helper.Cliemprnombvtea);
                    if (!dr.IsDBNull(iCliemprnombvtea)) entity.Cliemprnombvtea = dr.GetString(iCliemprnombvtea);

                    int iBarrnombvtea = dr.GetOrdinal(helper.Barrnombvtea);
                    if (!dr.IsDBNull(iBarrnombvtea)) entity.Barrnombvtea = dr.GetString(iBarrnombvtea);

                    int iGenemprnombvtp = dr.GetOrdinal(helper.Genemprnombvtp);
                    if (!dr.IsDBNull(iGenemprnombvtp)) entity.Genemprnombvtp = dr.GetString(iGenemprnombvtp);

                    int iCliemprnombvtp = dr.GetOrdinal(helper.Cliemprnombvtp);
                    if (!dr.IsDBNull(iCliemprnombvtp)) entity.Cliemprnombvtp = dr.GetString(iCliemprnombvtp);

                    int iBarrnombvtp = dr.GetOrdinal(helper.Barrnombvtp);
                    if (!dr.IsDBNull(iBarrnombvtp)) entity.Barrnombvtp = dr.GetString(iBarrnombvtp);

                    if (string.IsNullOrEmpty(entity.Codigovtp))
                    {
                        entity.Tipousuariovtp = string.Empty;
                        entity.Tipocontratovtp = string.Empty;
                    }

                    int iRetrelVari = dr.GetOrdinal(helper.RetrelVari);
                    if (!dr.IsDBNull(iRetrelVari)) entity.Retrelvari = dr.GetDecimal(iRetrelVari);

                    entitys.Add(entity);

                }
            }
            return entitys;
        }
        public List<CodigoRetiroRelacionDetalleDTO> ListarRelacionCodigoRetirosPorCodigo(int retrelcodi)
        {
            List<CodigoRetiroRelacionDetalleDTO> entitys = new List<CodigoRetiroRelacionDetalleDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListarRelacionCodigoRetirosPorCodigo);

            dbProvider.AddInParameter(command, helper.Genemprcodi, DbType.Int32, retrelcodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {

                    CodigoRetiroRelacionDetalleDTO entity = new CodigoRetiroRelacionDetalleDTO();
                    entity = helper.CreateDetalle(dr);

                    int iGenemprnombvtea = dr.GetOrdinal(helper.Genemprnombvtea);
                    if (!dr.IsDBNull(iGenemprnombvtea)) entity.Genemprnombvtea = dr.GetString(iGenemprnombvtea);

                    int iCliemprnombvtea = dr.GetOrdinal(helper.Cliemprnombvtea);
                    if (!dr.IsDBNull(iCliemprnombvtea)) entity.Cliemprnombvtea = dr.GetString(iCliemprnombvtea);

                    int iBarrnombvtea = dr.GetOrdinal(helper.Barrnombvtea);
                    if (!dr.IsDBNull(iBarrnombvtea)) entity.Barrnombvtea = dr.GetString(iBarrnombvtea);

                    int iGenemprnombvtp = dr.GetOrdinal(helper.Genemprnombvtp);
                    if (!dr.IsDBNull(iGenemprnombvtp)) entity.Genemprnombvtp = dr.GetString(iGenemprnombvtp);

                    int iCliemprnombvtp = dr.GetOrdinal(helper.Cliemprnombvtp);
                    if (!dr.IsDBNull(iCliemprnombvtp)) entity.Cliemprnombvtp = dr.GetString(iCliemprnombvtp);

                    int iBarrnombvtp = dr.GetOrdinal(helper.Barrnombvtp);
                    if (!dr.IsDBNull(iBarrnombvtp)) entity.Barrnombvtp = dr.GetString(iBarrnombvtp);

                    if (string.IsNullOrEmpty(entity.Codigovtp))
                    {
                        entity.Tipousuariovtp = string.Empty;
                        entity.Tipocontratovtp = string.Empty;
                    }

                    int iRetrelVari = dr.GetOrdinal(helper.RetrelVari);
                    if (!dr.IsDBNull(iRetrelVari)) entity.Retrelvari = dr.GetDecimal(iRetrelVari);

                    entitys.Add(entity);

                }
            }
            return entitys;
        }

        public decimal GetPoteCoincidenteByCodigoVtp(int pericodi, int recpotcodi, string codVTP, int emprcodi)
        {
            decimal poteCoincidente = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetPoteCoincidenteByCodigoVtp);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.String, pericodi);
            dbProvider.AddInParameter(command, helper.Recpotcodi, DbType.String, recpotcodi);
            dbProvider.AddInParameter(command, helper.Codigovtp, DbType.String, codVTP);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprcodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    poteCoincidente = Convert.ToDecimal(dr["pegrdpotecoincidente"].ToString().Trim());
                }
            }
            return poteCoincidente;
        }

        public int TotalRecordsRelacionCodigoRetiros(int? genEmprCodi, int? cliEmprCodi, int? barrCodiTra, int? barrCodiSum, int? tipConCodi, int? tipUsuCodi, string estado, string codigo)
        {
            int NroRegistros = 0;
            List<CodigoRetiroDTO> entitys = new List<CodigoRetiroDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlTotalRecordsRelacionCodigoRetiros);

            dbProvider.AddInParameter(command, helper.Genemprcodi, DbType.Int32, genEmprCodi);
            dbProvider.AddInParameter(command, helper.Genemprcodi, DbType.Int32, genEmprCodi);
            dbProvider.AddInParameter(command, helper.Genemprcodi, DbType.Int32, genEmprCodi);
            dbProvider.AddInParameter(command, helper.Cliemprcodi, DbType.Int32, cliEmprCodi);
            dbProvider.AddInParameter(command, helper.Cliemprcodi, DbType.Int32, cliEmprCodi);
            dbProvider.AddInParameter(command, helper.Cliemprcodi, DbType.Int32, cliEmprCodi);
            dbProvider.AddInParameter(command, helper.Barrcoditra, DbType.Int32, barrCodiTra);
            dbProvider.AddInParameter(command, helper.Barrcoditra, DbType.Int32, barrCodiTra);
            dbProvider.AddInParameter(command, helper.Barrcodisum, DbType.Int32, barrCodiSum);
            dbProvider.AddInParameter(command, helper.Barrcodisum, DbType.Int32, barrCodiSum);
            dbProvider.AddInParameter(command, helper.Tipconcodi, DbType.Int32, tipConCodi);
            dbProvider.AddInParameter(command, helper.Tipconcodi, DbType.Int32, tipConCodi);
            dbProvider.AddInParameter(command, helper.Tipconcodi, DbType.Int32, tipConCodi);
            dbProvider.AddInParameter(command, helper.Tipusucodi, DbType.Int32, tipUsuCodi);
            dbProvider.AddInParameter(command, helper.Tipusucodi, DbType.Int32, tipUsuCodi);
            dbProvider.AddInParameter(command, helper.Tipusucodi, DbType.Int32, tipUsuCodi);
            dbProvider.AddInParameter(command, helper.Codigo, DbType.String, string.IsNullOrEmpty(codigo) ? null : codigo);
            dbProvider.AddInParameter(command, helper.Codigo, DbType.String, string.IsNullOrEmpty(codigo) ? null : codigo);
            dbProvider.AddInParameter(command, helper.Codigo, DbType.String, string.IsNullOrEmpty(codigo) ? null : codigo);
            dbProvider.AddInParameter(command, helper.RetelEstado, DbType.String, string.IsNullOrEmpty(estado) ? "ACT" : estado);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    NroRegistros = Convert.ToInt32(dr["NroRegistros"].ToString().Trim());
                }
            }
            return NroRegistros;

        }

        public CodigoRetiroRelacionDTO GetById(int id)
        {
            CodigoRetiroRelacionDTO entity = new CodigoRetiroRelacionDTO();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.RetRelCodi, DbType.Int32, id);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    int iRetRelCodi = dr.GetOrdinal(helper.RetRelCodi);
                    if (!dr.IsDBNull(iRetRelCodi)) entity.RetrelCodi = dr.GetInt32(iRetRelCodi);

                    int iRetrelVari = dr.GetOrdinal(helper.RetrelVari);
                    if (!dr.IsDBNull(iRetrelVari)) entity.Retrelvari = dr.GetDecimal(iRetrelVari);

                    int iRetelEstado = dr.GetOrdinal(helper.RetelEstado);
                    if (!dr.IsDBNull(iRetelEstado)) entity.Retelestado = dr.GetString(iRetelEstado);
                }
            }
            return entity;
        }
    }
}
