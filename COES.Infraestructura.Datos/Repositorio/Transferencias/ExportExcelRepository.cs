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
    public class ExportExcelRepository : RepositoryBase, IEnvioInformacionRepository
    {        
        public ExportExcelRepository(string strConn) : base(strConn)
        {
        }  

        ExportExelHelper helper = new ExportExelHelper();

        public List<ExportExcelDTO> ListVistaTodo(int iPeriCodi, int iEmprCodi, int iBarrCodi)
        {
            List<ExportExcelDTO> entitys = new List<ExportExcelDTO>();
            //DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteriaListaVistaTodo);
            //dbProvider.AddInParameter(command, helper.EMPRCODI, DbType.Int32, iEmprCodi);
            //dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, iPeriCodi);
            //dbProvider.AddInParameter(command, helper.BARRCODI, DbType.Int32, iBarrCodi);
            //dbProvider.AddInParameter(command, helper.BARRCODI, DbType.Int32, iBarrCodi);
            //dbProvider.AddInParameter(command, helper.BARRCODI, DbType.Int32, iBarrCodi);

            String sql = String.Format(this.helper.SqlGetByCriteriaListaVistaTodo, iEmprCodi, iPeriCodi, iBarrCodi);
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


        public List<ExportExcelDTO> GetByCriteria(int iPeriCodi, int iEmprCodi)
        {
            List<ExportExcelDTO> entitys = new List<ExportExcelDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);
            dbProvider.AddInParameter(command, helper.EMPRCODI, DbType.Int32, iEmprCodi);
            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, iPeriCodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<ExportExcelDTO> GetByListCodigoInfoBase(int iPeriCodi, int iEmprCodi)
        {
            List<ExportExcelDTO> entitys = new List<ExportExcelDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByListCodigoInfoBase);
            dbProvider.AddInParameter(command, helper.EMPRCODI, DbType.Int32, iEmprCodi);
            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, iPeriCodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    ExportExcelDTO entity = new ExportExcelDTO();
                    //Para Informacion Base
                    int iBarrCodi = dr.GetOrdinal(helper.COINFBCODI);
                    if (!dr.IsDBNull(iBarrCodi)) entity.CoInfbCodi = dr.GetInt32(iBarrCodi);

                    int iEmprNomb = dr.GetOrdinal(helper.EMPRNOMB);
                    if (!dr.IsDBNull(iEmprNomb)) entity.EmprNomb = dr.GetString(iEmprNomb);

                    int iBARRNOMBBARRTRAN = dr.GetOrdinal(helper.BARRNOMBBARRTRAN);
                    if (!dr.IsDBNull(iBARRNOMBBARRTRAN)) entity.BarrNombBarrTran = dr.GetString(iBARRNOMBBARRTRAN);

                    int iCOINFBCODIGO = dr.GetOrdinal(helper.COINFBCODIGO);
                    if (!dr.IsDBNull(iCOINFBCODIGO)) entity.CoInfbCodigo = dr.GetString(iCOINFBCODIGO);

                    int iTipo = dr.GetOrdinal(helper.Tipo);
                    if (!dr.IsDBNull(iTipo)) entity.Tipo = dr.GetString(iTipo);

                    int iCENTGENENOMBRE = dr.GetOrdinal(helper.CENTGENENOMBRE);
                    if (!dr.IsDBNull(iCENTGENENOMBRE)) entity.CentGeneCliNombre = dr.GetString(iCENTGENENOMBRE);


                    int iEMPRCODI = dr.GetOrdinal(helper.EMPRCODI);
                    if (!dr.IsDBNull(iEMPRCODI)) entity.EmprCodi = dr.GetInt32(iEMPRCODI);

                    entitys.Add(entity);

                }
            }

            return entitys;





        }

        /* ASSETEC 202001  */ 
        public List<ExportExcelDTO> GetByListCodigoInfoBaseByEnvio(int trnenvcodi)
        {
            List<ExportExcelDTO> entitys = new List<ExportExcelDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByListCodigoInfoBaseByEnvio);
            dbProvider.AddInParameter(command, helper.TRNENVCODI, DbType.Int32, trnenvcodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    ExportExcelDTO entity = new ExportExcelDTO();
                    //Para Informacion Base
                    int iBarrCodi = dr.GetOrdinal(helper.COINFBCODI);
                    if (!dr.IsDBNull(iBarrCodi)) entity.CoInfbCodi = dr.GetInt32(iBarrCodi);

                    int iEmprNomb = dr.GetOrdinal(helper.EMPRNOMB);
                    if (!dr.IsDBNull(iEmprNomb)) entity.EmprNomb = dr.GetString(iEmprNomb);

                    int iBARRNOMBBARRTRAN = dr.GetOrdinal(helper.BARRNOMBBARRTRAN);
                    if (!dr.IsDBNull(iBARRNOMBBARRTRAN)) entity.BarrNombBarrTran = dr.GetString(iBARRNOMBBARRTRAN);

                    int iCOINFBCODIGO = dr.GetOrdinal(helper.COINFBCODIGO);
                    if (!dr.IsDBNull(iCOINFBCODIGO)) entity.CoInfbCodigo = dr.GetString(iCOINFBCODIGO);

                    int iTipo = dr.GetOrdinal(helper.Tipo);
                    if (!dr.IsDBNull(iTipo)) entity.Tipo = dr.GetString(iTipo);

                    int iCENTGENENOMBRE = dr.GetOrdinal(helper.CENTGENENOMBRE);
                    if (!dr.IsDBNull(iCENTGENENOMBRE)) entity.CentGeneCliNombre = dr.GetString(iCENTGENENOMBRE);


                    int iEMPRCODI = dr.GetOrdinal(helper.EMPRCODI);
                    if (!dr.IsDBNull(iEMPRCODI)) entity.EmprCodi = dr.GetInt32(iEMPRCODI);

                    entitys.Add(entity);

                }
            }

            return entitys;
        }

        public List<ExportExcelDTO> GetByListCodigoModelo(int pericodi, int emprcodi, int trnmodcodi)
        {
            List<ExportExcelDTO> entitys = new List<ExportExcelDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByListCodigoModelo);
            dbProvider.AddInParameter(command, helper.TRNMODCODI, DbType.Int32, trnmodcodi);
            dbProvider.AddInParameter(command, helper.EMPRCODI, DbType.Int32, emprcodi);
            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, pericodi);
            //dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, pericodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    ExportExcelDTO entity = helper.Create(dr);
                    //El generador, a quien pertenece el código
                    int iEMPRCODI = dr.GetOrdinal(helper.EMPRCODI);
                    if (!dr.IsDBNull(iEMPRCODI)) entity.EmprCodi = dr.GetInt32(iEMPRCODI);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<ExportExcelDTO> GetByListCodigoModeloVTA(int pericodi, int emprcodi, int trnmodcodi)
        {
            List<ExportExcelDTO> entitys = new List<ExportExcelDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByListCodigoModeloVTA);
            dbProvider.AddInParameter(command, helper.TRNMODCODI, DbType.Int32, trnmodcodi);
            dbProvider.AddInParameter(command, helper.EMPRCODI, DbType.Int32, emprcodi);            
            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, pericodi);
            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, pericodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    ExportExcelDTO entity = helper.Create(dr);
                    //El generador, a quien pertenece el código
                    int iEMPRCODI = dr.GetOrdinal(helper.EMPRCODI);
                    if (!dr.IsDBNull(iEMPRCODI)) entity.EmprCodi = dr.GetInt32(iEMPRCODI);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }
    }
}
