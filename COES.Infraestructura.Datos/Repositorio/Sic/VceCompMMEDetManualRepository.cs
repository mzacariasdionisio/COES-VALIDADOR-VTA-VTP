using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Infraestructura.Datos.Helper.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase que contiene las operaciones con la base de datos
    /// </summary>
    public class VceCompMMEDetManualRepository : RepositoryBase, IVceCompMMEDetManualRepository
    {
        public VceCompMMEDetManualRepository(string strConn) : base(strConn)
        {
        }

        VceCompMMEDetManualHelper helper = new VceCompMMEDetManualHelper();
        

        public void DeleteCompensacionManual(int pecacodi, int grupocodi, DateTime cmmedmhora)
        {            
            string queryDelete = string.Format(helper.SqlDeleteManual, pecacodi, grupocodi, Convert.ToDateTime(cmmedmhora).ToString("dd/MM/yyyy HH:mm:ss"));
            DbCommand command = dbProvider.GetSqlStringCommand(queryDelete);           

            dbProvider.ExecuteNonQuery(command);

            queryDelete = string.Format(helper.SqlUpdateCompensacionRegular, pecacodi, grupocodi, Convert.ToDateTime(cmmedmhora).ToString("dd/MM/yyyy HH:mm:ss"));
            command = dbProvider.GetSqlStringCommand(queryDelete);

            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteCompensacionManualByVersion(int pecacodi)
        {
            string queryDelete = helper.SqlDeleteManualByVersion;
            DbCommand command = dbProvider.GetSqlStringCommand(queryDelete);            
            dbProvider.AddInParameter(command, helper.Pecacodi, DbType.Int32, pecacodi);

            dbProvider.ExecuteNonQuery(command);

            queryDelete = helper.SqlUpdateCompensacionRegularlByVersion;
            command = dbProvider.GetSqlStringCommand(queryDelete);
            dbProvider.AddInParameter(command, helper.Pecacodi, DbType.Int32, pecacodi);
            dbProvider.ExecuteNonQuery(command);
        }


        public void SaveEntity(VceCompMMEDetManualDetDTO entity)
        {           
            string query = helper.SqlSaveManual;
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            try
            {

                //dbProvider.AddInParameter(command, helper.Cmmedmhora, DbType.DateTime2, Convert.ToDateTime(entity.Cmmedmhora).ToString("dd-MM-yyyy HH:mm").ToUpper());
                //dbProvider.AddInParameter(command, helper.Cmmedmhora, DbType.DateTime, entity.Cmmedmhora);
                dbProvider.AddInParameter(command, helper.Subcausacodi, DbType.Int32, entity.Subcausacodi);
                dbProvider.AddInParameter(command, helper.Cmmedmenergia, DbType.Decimal, entity.Cmmedmenergia);

                dbProvider.AddInParameter(command, helper.Cmmedmpotencia, DbType.Decimal, entity.Cmmedmpotencia);
                dbProvider.AddInParameter(command, helper.Cmmedmconsumocomb, DbType.Decimal, entity.Cmmedmconsumocomb);
                dbProvider.AddInParameter(command, helper.Cmmedmprecioaplic, DbType.Decimal, entity.Cmmedmprecioaplic);
                dbProvider.AddInParameter(command, helper.Cmmedmcvc, DbType.Decimal, entity.Cmmedmcvc);
                dbProvider.AddInParameter(command, helper.Cmmedmcvnc, DbType.Decimal, entity.Cmmedmcvnc);

                dbProvider.AddInParameter(command, helper.Cmmedmcvt, DbType.Decimal, entity.Cmmedmcvt);
                dbProvider.AddInParameter(command, helper.Cmmedmcmg, DbType.Decimal, entity.Cmmedmcmg);
                dbProvider.AddInParameter(command, helper.Cmmedmcompensacion, DbType.Decimal, entity.Cmmedmcompensacion);
                dbProvider.AddInParameter(command, helper.Cmmedmtipocalc, DbType.String, entity.Cmmedmtipocalc);
                //campos clave
                dbProvider.AddInParameter(command, helper.Pecacodi, DbType.Int32, entity.PecaCodi);
                dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
                //dbProvider.AddInParameter(command, helper.Cmmedmhora, DbType.DateTime, entity.Cmmedmhora);
                dbProvider.AddInParameter(command, helper.Cmmedmhora, DbType.String, entity.Cmmedmhora.ToString("dd/MM/yyyy HH:mm:ss"));

                dbProvider.ExecuteNonQuery(command);

                
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
           
        }

        public void UpdateCompensacionDet(int pecacodi)
        {
            string query = string.Format(helper.SqUpdateCompensacionDet, pecacodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
           
            //dbProvider.AddInParameter(command, helper.Pecacodi, DbType.Int32, pecacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void SaveCompensacionDet(int pecacodi)
        {
            string query = string.Format(helper.SqlSaveCompensacionDet, pecacodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            
            //dbProvider.AddInParameter(command, helper.Pecacodi, DbType.Int32, pecacodi);

            dbProvider.ExecuteNonQuery(command);
        }

       

        public List<VceCompMMEDetManualDetDTO> ListCompensacionesManuales(int pecacodi, string empresa, string central, string grupo, string modo, string tipo, string fechaini, string fechafin, string tipocalculo)
        {
            string condicion = "";

            //DSH 30-06-2017 : Se actualizo por requerimiento
            //condicion = condicion + " AND pecacodi = " + pecacodi;

            if (!empresa.Equals("") && empresa != null)
            {
                condicion = condicion + " AND EMP.EMPRCODI = " + empresa;
            }

            if (!central.Equals("") && central != null)
            {
                condicion = condicion + " AND CG.EQUICODI = " + central;
            }

            if (!grupo.Equals("") && grupo != null)
            {
                condicion = condicion + " AND GG.GRUPOCODI = " + grupo;
            }

            if (!modo.Equals("") && modo != null)
            {
                condicion = condicion + " AND MO.GRUPOCODI = " + modo;
            }

            if (!tipo.Equals("") && tipo != null)
            {
                condicion = condicion + " AND BE.SUBCAUSACODI = " + tipo;
            }

            if (!fechaini.Equals("") && fechaini != null)
            {
                condicion = condicion + " AND TRUNC(BE.CMMEDMHORA) >= TO_DATE('" + fechaini + "','DD/MM/YYYY') ";
            }

            if (!fechafin.Equals("") && fechafin != null)
            {
                condicion = condicion + " AND TRUNC(BE.CMMEDMHORA) <= TO_DATE('" + fechafin + "','DD/MM/YYYY') ";
            }

            //if (!tipocalculo.Equals("") && tipocalculo != null)
            //{
            //    condicion = condicion + " AND BE.CRCBETIPOCALC = '" + tipocalculo + "'";
            //}
            List<VceCompMMEDetManualDetDTO> entitys = new List<VceCompMMEDetManualDetDTO>();
            string queryString = string.Format(helper.SqlListCompensacionManual, pecacodi, condicion);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VceCompMMEDetManualDetDTO entity = new VceCompMMEDetManualDetDTO();

                    entity = helper.Create(dr);

                    int iEmprNomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprNomb)) entity.Emprnomb = dr.GetString(iEmprNomb);

                    int iSubCausaDesc = dr.GetOrdinal(helper.Subcausadesc);
                    if (!dr.IsDBNull(iSubCausaDesc)) entity.Subcausadesc = dr.GetString(iSubCausaDesc);

                    int iGrupoNomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGrupoNomb)) entity.Gruponomb = dr.GetString(iGrupoNomb);

                    //int iCrcbeHorIni = dr.GetOrdinal(helper.Crcbehorini);
                    //if (!dr.IsDBNull(iCrcbeHorIni)) entity.Crcbehorini = dr.GetDateTime(iCrcbeHorIni);

                    //int iCrcbeHorFin = dr.GetOrdinal(helper.Crcbehorfin);
                    //if (!dr.IsDBNull(iCrcbeHorFin)) entity.Crcbehorfin = dr.GetDateTime(iCrcbeHorFin);

                    //int iCrcbePotencia = dr.GetOrdinal(helper.Crcbepotencia);
                    //if (!dr.IsDBNull(iCrcbePotencia)) entity.Crcbepotencia = dr.GetDecimal(iCrcbePotencia);

                    //int iCrcbeConsumo = dr.GetOrdinal(helper.Crcbeconsumo);
                    //if (!dr.IsDBNull(iCrcbeConsumo)) entity.Crcbeconsumo = dr.GetDecimal(iCrcbeConsumo);

                    //int iCrcbeCvc = dr.GetOrdinal(helper.Crcbecvc);
                    //if (!dr.IsDBNull(iCrcbeCvc)) entity.Crcbecvc = dr.GetDecimal(iCrcbeCvc);

                    //int iCrcbeCvnc = dr.GetOrdinal(helper.Crcbecvnc);
                    //if (!dr.IsDBNull(iCrcbeCvnc)) entity.Crcbecvnc = dr.GetDecimal(iCrcbeCvnc);

                    //int iCrcbeCvt = dr.GetOrdinal(helper.Crcbecvt);
                    //if (!dr.IsDBNull(iCrcbeCvt)) entity.Crcbecvt = dr.GetDecimal(iCrcbeCvt);

                    //int iCrcbeCompensacion = dr.GetOrdinal(helper.Crcbecompensacion);
                    //if (!dr.IsDBNull(iCrcbeCompensacion)) entity.Crcbecompensacion = dr.GetDecimal(iCrcbeCompensacion);

                    //int iGrupoCodi = dr.GetOrdinal(helper.Grupocodi);
                    //if (!dr.IsDBNull(iGrupoCodi)) entity.Grupocodi = dr.GetInt32(iGrupoCodi);

                    //int iSubcausaCodi = dr.GetOrdinal(helper.Subcausacodi);
                    //if (!dr.IsDBNull(iSubcausaCodi)) entity.Subcausacodi = dr.GetInt32(iSubcausaCodi);

                    //int iPecaCodi = dr.GetOrdinal(helper.Pecacodi);
                    //if (!dr.IsDBNull(iPecaCodi)) entity.PecaCodi = dr.GetInt32(iPecaCodi);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

    }

}
