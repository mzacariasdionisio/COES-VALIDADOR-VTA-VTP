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
    /// <summary>
    /// Clase que contiene las operaciones con la base de datos
    /// </summary>
    public class EmpresaPagoRepository : RepositoryBase, IEmpresaPagoRepository
    {
        public EmpresaPagoRepository(string strConn)
            : base(strConn)
        {
        }

        EmpresaPagoHelper helper = new EmpresaPagoHelper();    

        public int Save(EmpresaPagoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.EMPPAGOCODI, DbType.Int32, GetCodigoGenerado());
            dbProvider.AddInParameter(command, helper.VALTOTAEMPCODI, DbType.Int32, entity.ValTotaEmpCodi);
            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, entity.PeriCodi);
            dbProvider.AddInParameter(command, helper.EMPCODI, DbType.Int32, entity.EmpCodi);
            dbProvider.AddInParameter(command, helper.VALTOTAEMPVERSION, DbType.Int32, entity.ValTotaEmpVersion);           
            dbProvider.AddInParameter(command, helper.EMPPAGOCODEMPPAGO, DbType.Int32, entity.EmpPagoCodEmpPago);
            dbProvider.AddInParameter(command, helper.EMPPAGOMONTO, DbType.Decimal, entity.EmpPagoMonto);
            dbProvider.AddInParameter(command, helper.EMPPAGUSERNAME, DbType.String, entity.EmpPagpUserName);
            dbProvider.AddInParameter(command, helper.EMPPAGOFECINS, DbType.DateTime, DateTime.Now);

            return dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int PeriCodi, int Version)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, PeriCodi);
            dbProvider.AddInParameter(command, helper.VALTOTAEMPVERSION, DbType.Int32, Version);

            dbProvider.ExecuteNonQuery(command);
        }

        public List<EmpresaPagoDTO> GetByCriteria(int pericodi, int version)
        {
            List<EmpresaPagoDTO> entitys = new List<EmpresaPagoDTO>();
            //DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            //dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, pericodi);
            //dbProvider.AddInParameter(command, helper.VALTOTAEMPVERSION, DbType.Int32, version);

            //using (IDataReader dr = dbProvider.ExecuteReader(command))
            //{
            //    while (dr.Read())
            //    {
            //        entitys.Add(helper.Create(dr));
            //    }
            //}

            return entitys;
        }

      

        public int GetCodigoGenerado()
        {
            int newId = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlCodigoGenerado);
            newId = Int32.Parse(dbProvider.ExecuteScalar(command).ToString());

            return newId;
        }


        public List<EmpresaPagoDTO> GetEmpresaPositivaByCriteria(int pericodi, int version)
        {
           List<EmpresaPagoDTO> entitys = new List<EmpresaPagoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetEmpresaPositiva);
            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.VALTOTAEMPVERSION, DbType.Int32, version);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<EmpresaPagoDTO> GetEmpresaNegativaByCriteria(int pericodi, int version)
        {
            List<EmpresaPagoDTO> entitys = new List<EmpresaPagoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetEmpresaNegativa);
            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.VALTOTAEMPVERSION, DbType.Int32, version);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<EmpresaPagoDTO> ObtenerListaEmpresaPago(int pericodi, int emppagversion, int? emprcodi)
        {
            List<EmpresaPagoDTO> entitys = new List<EmpresaPagoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerListaEmpresaPago);
            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.VALTOTAEMPVERSION, DbType.Int32, emppagversion);
            dbProvider.AddInParameter(command, helper.EMPCODI, DbType.Int32, emprcodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = new EmpresaPagoDTO();

                    int iEMPPAGOCODI = dr.GetOrdinal(helper.EMPPAGOCODI);
                    if (!dr.IsDBNull(iEMPPAGOCODI)) entity.EmpPagoCodi = dr.GetInt32(iEMPPAGOCODI);

                    int iPERICODI = dr.GetOrdinal(helper.PERICODI);
                    if (!dr.IsDBNull(iPERICODI)) entity.PeriCodi = dr.GetInt32(iPERICODI);

                    int iEMPCODI = dr.GetOrdinal(helper.EMPCODI);
                    if (!dr.IsDBNull(iEMPCODI)) entity.EmpCodi = dr.GetInt32(iEMPCODI);

                    int iVALTOTAEMPVERSION = dr.GetOrdinal(helper.VALTOTAEMPVERSION);
                    if (!dr.IsDBNull(iVALTOTAEMPVERSION)) entity.ValTotaEmpVersion = dr.GetInt32(iVALTOTAEMPVERSION);

                    int iEMPPAGOCODEMPPAGO = dr.GetOrdinal(helper.EMPPAGOCODEMPPAGO);
                    if (!dr.IsDBNull(iEMPPAGOCODEMPPAGO)) entity.EmpPagoCodEmpPago = dr.GetInt32(iEMPPAGOCODEMPPAGO);

                    int iEMPPAGOMONTO = dr.GetOrdinal(helper.EMPPAGOMONTO);
                    if (!dr.IsDBNull(iEMPPAGOMONTO)) entity.EmpPagoMonto = dr.GetDecimal(iEMPPAGOMONTO);

                    int iEMPPAGUSERNAME = dr.GetOrdinal(helper.EMPPAGUSERNAME);
                    if (!dr.IsDBNull(iEMPPAGOMONTO)) entity.EmpPagpUserName = dr.GetString(iEMPPAGUSERNAME);

                    int iEmppagfecins = dr.GetOrdinal(helper.Emppagfecins);
                    if (!dr.IsDBNull(iEmppagfecins)) entity.EmpPagoFecIns = dr.GetDateTime(iEmppagfecins);

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.EmprNomb = Convert.ToString(dr.GetValue(iEmprnomb));

                    int iEmprcodosinergmin = dr.GetOrdinal(this.helper.Emprcodosinergmin);
                    if (!dr.IsDBNull(iEmprcodosinergmin)) entity.Emprcodosinergmin = dr.GetString(iEmprcodosinergmin);

                    int iEmprnombcobro = dr.GetOrdinal(this.helper.Emprnombpago);
                    if (!dr.IsDBNull(iEmprnombcobro)) entity.EmprNombPago = Convert.ToString(dr.GetValue(iEmprnombcobro));

                    int iEmprcodosinergminpago = dr.GetOrdinal(this.helper.Emprcodosinergminpago);
                    if (!dr.IsDBNull(iEmprcodosinergminpago)) entity.Emprcodosinergminpago = dr.GetString(iEmprcodosinergminpago);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
