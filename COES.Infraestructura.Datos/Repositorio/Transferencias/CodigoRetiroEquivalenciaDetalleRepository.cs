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
    public class CodigoRetiroEquivalenciaDetalleRepository : RepositoryBase, ICodigoRetiroEquivalenciaDetalleRepository
    {
        CodigoRetiroEquivalenciaDetalleHelper helper = new CodigoRetiroEquivalenciaDetalleHelper();

        public CodigoRetiroEquivalenciaDetalleRepository(string strConn) : base(strConn)
        {
        }

        public int Save(CodigoRetiroRelacionDetalleDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);
            dbProvider.AddInParameter(command, helper.Rerldtcodi, DbType.Int32, GetCodigoGenerado());
            dbProvider.AddInParameter(command, helper.Retrelcodi, DbType.Int32, entity.Retrelcodi);

            dbProvider.AddInParameter(command, helper.Genemprcodivtea, DbType.Int32, entity.Genemprcodivtea);
            dbProvider.AddInParameter(command, helper.Cliemprcodivtea, DbType.Int32, entity.Cliemprcodivtea);
            //dbProvider.AddInParameter(command, helper.Tipconcodivtea, DbType.Int32, entity.Tipconcodivtea);
            //dbProvider.AddInParameter(command, helper.Tipusuvtea, DbType.Int32, entity.Tipusuvtea);
            dbProvider.AddInParameter(command, helper.Barrcodivtea, DbType.Int32, entity.Barrcodivtea);
            dbProvider.AddInParameter(command, helper.Coresocodvtea, DbType.Int32, entity.Coresocodvtea);

            dbProvider.AddInParameter(command, helper.Genemprcodivtp, DbType.Int32, entity.Genemprcodivtp);
            dbProvider.AddInParameter(command, helper.Cliemprcodivtp, DbType.Int32, entity.Cliemprcodivtp);
            //dbProvider.AddInParameter(command, helper.Tipconcodivtp, DbType.Int32, entity.Tipconcodivtp);
            //dbProvider.AddInParameter(command, helper.Tipusuvtp, DbType.Int32, entity.Tipusuvtp);
            dbProvider.AddInParameter(command, helper.Barrcodivtp, DbType.Int32, entity.Barrcodivtp);
            dbProvider.AddInParameter(command, helper.Coresocodvtp, DbType.Int32, entity.Coresocodvtp);

            dbProvider.AddInParameter(command, helper.Rerldtestado, DbType.String, entity.Rerldtestado);
            dbProvider.AddInParameter(command, helper.Rerldtusucreacion, DbType.String, entity.Rerldtusucreacion);
            dbProvider.AddInParameter(command, helper.Rerldtfeccreacion, DbType.Date, DateTime.Now.Date);

            return dbProvider.ExecuteNonQuery(command);
        }

        public int Delete(int id)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);
            dbProvider.AddInParameter(command, helper.Retrelcodi, DbType.Int32, id);

            return dbProvider.ExecuteNonQuery(command);
        }

        public int GetCodigoGenerado()
        {
            int newId = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlCodigoGenerado);
            newId = Int32.Parse(dbProvider.ExecuteScalar(command).ToString());

            return newId;
        }

        public List<CodigoRetiroRelacionDetalleDTO> ListarRelacionCodigoRetiros(List<int> idArray)
        {
            string query = helper.SqlListarRelacionDetalleCodigoRetiros;
            List<CodigoRetiroRelacionDetalleDTO> entitys = new List<CodigoRetiroRelacionDetalleDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(query.Replace("@idArray", string.Join(",", idArray)));

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<CodigoRetiroRelacionDetalleDTO> ListarRelacionDetalle(List<int> idArray)
        {
            string query = helper.SqlListarRelacionDetalle;
            List<CodigoRetiroRelacionDetalleDTO> entitys = new List<CodigoRetiroRelacionDetalleDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(query.Replace("@idArray", string.Join(",", idArray)));

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CodigoRetiroRelacionDetalleDTO entity = new CodigoRetiroRelacionDetalleDTO();

                    entity = helper.Create(dr);

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

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<CodigoRetiroRelacionDetalleDTO> GetById(int id)
        {
            string query = helper.SqlListarRelacionDetalle;
            List<CodigoRetiroRelacionDetalleDTO> entitys = new List<CodigoRetiroRelacionDetalleDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);
            dbProvider.AddInParameter(command, helper.Retrelcodi, DbType.Int32, id);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CodigoRetiroRelacionDetalleDTO entity = new CodigoRetiroRelacionDetalleDTO();


                    int iRerldtcodi = dr.GetOrdinal(helper.Rerldtcodi);
                    if (!dr.IsDBNull(iRerldtcodi)) entity.Rerldtcodi = dr.GetInt32(iRerldtcodi);

                    int iGenemprcodivtea = dr.GetOrdinal(helper.Genemprcodivtea);
                    if (!dr.IsDBNull(iGenemprcodivtea)) entity.Genemprcodivtea = dr.GetInt32(iGenemprcodivtea);

                    int iCliemprcodivtea = dr.GetOrdinal(helper.Cliemprcodivtea);
                    if (!dr.IsDBNull(iCliemprcodivtea)) entity.Cliemprcodivtea = dr.GetInt32(iCliemprcodivtea);

                    int iBarrcodivtea = dr.GetOrdinal(helper.Barrcodivtea);
                    if (!dr.IsDBNull(iBarrcodivtea)) entity.Barrcodivtea = dr.GetInt32(iBarrcodivtea);

                    int iCodigovtea = dr.GetOrdinal(helper.Codigovtea);
                    if (!dr.IsDBNull(iCodigovtea)) entity.Codigovtea = dr.GetString(iCodigovtea);

                    int iCoresocodvtea = dr.GetOrdinal(helper.Coresocodvtea);
                    if (!dr.IsDBNull(iCoresocodvtea)) entity.Coresocodvtea = dr.GetInt32(iCoresocodvtea);

                    int iGenemprcodivtp = dr.GetOrdinal(helper.Genemprcodivtp);
                    if (!dr.IsDBNull(iGenemprcodivtp)) entity.Genemprcodivtp = dr.GetInt32(iGenemprcodivtp);

                    int iCliemprcodivtp = dr.GetOrdinal(helper.Cliemprcodivtp);
                    if (!dr.IsDBNull(iCliemprcodivtp)) entity.Cliemprcodivtp = dr.GetInt32(iCliemprcodivtp);

                    int iBarrcodivtp = dr.GetOrdinal(helper.Barrcodivtp);
                    if (!dr.IsDBNull(iBarrcodivtp)) entity.Barrcodivtp = dr.GetInt32(iBarrcodivtp);

                    int iCodigovtp = dr.GetOrdinal(helper.Codigovtp);
                    if (!dr.IsDBNull(iCodigovtp)) entity.Codigovtp = dr.GetString(iCodigovtp);

                    int iCoresocodvtp = dr.GetOrdinal(helper.Coresocodvtp);
                    if (!dr.IsDBNull(iCoresocodvtp)) entity.Coresocodvtp = dr.GetInt32(iCoresocodvtp);

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

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        public CodigoRetiroRelacionDetalleDTO GetRelacionDetallePorVTEA(int coresCodVTEA)
        {
            string query = helper.SqlListarRelacionDetalle;
            CodigoRetiroRelacionDetalleDTO entitys = null;
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetRelacionDetallePorVTEA);
            dbProvider.AddInParameter(command, helper.Coresocodvtea, DbType.Int32, coresCodVTEA);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    CodigoRetiroRelacionDetalleDTO entity = new CodigoRetiroRelacionDetalleDTO();

                    int iRetrelcodi = dr.GetOrdinal(helper.Retrelcodi);
                    if (!dr.IsDBNull(iRetrelcodi)) entity.Retrelcodi = dr.GetInt32(iRetrelcodi);

                    int irerldtcodi = dr.GetOrdinal(helper.Rerldtcodi);
                    if (!dr.IsDBNull(irerldtcodi)) entity.Rerldtcodi = dr.GetInt32(irerldtcodi);

                    entitys = entity;
                }
            }

            return entitys;
        }
        public bool ExisteVTEA(int id)
        {
            int count = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlExisteVTEA);
            dbProvider.AddInParameter(command, helper.Coresocodvtea, DbType.Int32, id);
            count = Int32.Parse(dbProvider.ExecuteScalar(command).ToString());

            return (count > 0);
        }

        public bool ExisteVTP(int id)
        {
            int count = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlExisteVTP);
            dbProvider.AddInParameter(command, helper.Coresocodvtp, DbType.Int32, id);
            count = Int32.Parse(dbProvider.ExecuteScalar(command).ToString());

            return (count > 0);
        }

    }
}
