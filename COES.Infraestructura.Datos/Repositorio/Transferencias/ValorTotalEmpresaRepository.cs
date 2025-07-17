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
    public class ValorTotalEmpresaRepository : RepositoryBase, IValorTotalEmpresaRepository
    {
        public ValorTotalEmpresaRepository(string strConn)
            : base(strConn)
        {
        }
        
        ValorTotalEmpresaHelper helper = new ValorTotalEmpresaHelper();

        public int Save(ValorTotalEmpresaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.VALTOTAEMPCODI, DbType.Int32, GetCodigoGenerado());
            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, entity.PeriCodi);
            dbProvider.AddInParameter(command, helper.EMPCODI, DbType.Int32, entity.EmpCodi);
            dbProvider.AddInParameter(command, helper.VALTOTAEMPVERSION, DbType.Int32, entity.ValTotaEmpVersion);
            dbProvider.AddInParameter(command, helper.VALTOTAEMPTOTAL, DbType.Decimal, entity.ValTotaEmpTotal);
            dbProvider.AddInParameter(command, helper.VOTEMUSERNAME, DbType.String, entity.ValTotaEmpUserName);
            dbProvider.AddInParameter(command, helper.VALTOTAEMPFECINS, DbType.DateTime, DateTime.Now);   


            return dbProvider.ExecuteNonQuery(command);
        }

        public void Update(ValorTotalEmpresaDTO entity)
        {
          
        }

        public void Delete(int pericodi, int version)
        {

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.VALTOTAEMPVERSION, DbType.Int32, version);

            dbProvider.ExecuteNonQuery(command);
        }

        public ValorTotalEmpresaDTO GetById(System.Int32 id)
        {
            ValorTotalEmpresaDTO entity = null;          

            return entity;
        }

        public List<ValorTotalEmpresaDTO> List()
        {
            List<ValorTotalEmpresaDTO> entitys = new List<ValorTotalEmpresaDTO>();           

            return entitys;
        }

        public int GetCodigoGenerado()
        {
            int newId = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlCodigoGenerado);
            newId = Int32.Parse(dbProvider.ExecuteScalar(command).ToString());

            return newId;
        }

        public List<ValorTotalEmpresaDTO> GetEmpresaPositivaByCriteria(int pericodi, int version)
        {
            List<ValorTotalEmpresaDTO> entitys = new List<ValorTotalEmpresaDTO>();
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

        public List<ValorTotalEmpresaDTO> GetEmpresaNegativaByCriteria(int pericodi, int version)
        {
            List<ValorTotalEmpresaDTO> entitys = new List<ValorTotalEmpresaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetEmpresaNegativa);

            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.VALTOTAEMPVERSION, DbType.Int32, version);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    ValorTotalEmpresaDTO entity = new ValorTotalEmpresaDTO();

                    int iVALTOTAEMPCODI = dr.GetOrdinal(this.helper.VALTOTAEMPCODI);
                    if (!dr.IsDBNull(iVALTOTAEMPCODI)) entity.ValTotaEmpCodi = dr.GetInt32(iVALTOTAEMPCODI);

                    int iPERICODI = dr.GetOrdinal(this.helper.PERICODI);
                    if (!dr.IsDBNull(iPERICODI)) entity.PeriCodi = dr.GetInt32(iPERICODI);

                    int iEMPCODI = dr.GetOrdinal(this.helper.EMPCODI);
                    if (!dr.IsDBNull(iEMPCODI)) entity.EmpCodi = dr.GetInt32(iEMPCODI);

                    int iVALTOTAEMPVERSION = dr.GetOrdinal(this.helper.VALTOTAEMPVERSION);
                    if (!dr.IsDBNull(iVALTOTAEMPVERSION)) entity.ValTotaEmpVersion = dr.GetInt32(iVALTOTAEMPVERSION);

                    int iVALTOTAEMPTOTAL = dr.GetOrdinal(this.helper.VALTOTAEMPTOTAL);
                    if (!dr.IsDBNull(iVALTOTAEMPTOTAL)) entity.ValTotaEmpTotal = dr.GetDecimal(iVALTOTAEMPTOTAL);

                    int iVOTEMUSERNAME = dr.GetOrdinal(this.helper.VOTEMUSERNAME);
                    if (!dr.IsDBNull(iVOTEMUSERNAME)) entity.ValTotaEmpUserName = dr.GetString(iVOTEMUSERNAME);

                    int iVALTOTAEMPFECINS = dr.GetOrdinal(this.helper.VALTOTAEMPFECINS);
                    if (!dr.IsDBNull(iVALTOTAEMPFECINS)) entity.ValTotaEmpFecIns = dr.GetDateTime(iVALTOTAEMPFECINS);

                    int iTOTAL = dr.GetOrdinal(this.helper.TOTAL);
                    if (!dr.IsDBNull(iTOTAL)) entity.Total = dr.GetDecimal(iTOTAL);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<ValorTotalEmpresaDTO> ListarValorTotalEmpresa(int iPeriCodi, int iVTotEmVersion)
        {
            List<ValorTotalEmpresaDTO> entitys = new List<ValorTotalEmpresaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListarValorTotalEmpresa);
            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, iPeriCodi);
            dbProvider.AddInParameter(command, helper.VALTOTAEMPVERSION, DbType.Int32, iVTotEmVersion);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public ValorTotalEmpresaDTO GetByCriteria(int iPeriCodi, int iVTotEmVersion, int iEmprCodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);

            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, iPeriCodi);
            dbProvider.AddInParameter(command, helper.VALTOTAEMPVERSION, DbType.Int32, iVTotEmVersion);
            dbProvider.AddInParameter(command, helper.EMPCODI, DbType.Int32, iEmprCodi);
            dbProvider.AddInParameter(command, helper.EMPCODI, DbType.Int32, iEmprCodi);
            dbProvider.AddInParameter(command, helper.EMPCODI, DbType.Int32, iEmprCodi);
            ValorTotalEmpresaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }
    }
}
