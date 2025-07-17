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
    public class MeDemandaMercadoLibreRepository : RepositoryBase, IMeDemandaMercadoLibreRepository
    {
        public MeDemandaMercadoLibreRepository(string strConn)
            : base(strConn)
        {
        }

        MeDemandaMercadoLibreHelper helper = new MeDemandaMercadoLibreHelper();

        public int GetMaxId()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }
        public void Save(string periodo, string periodoSicli, int maxId, string usuario,string fechaDemandaMaxima, string fechaDemandaMaximaSicli)
        {
            var fechaIni = DateTime.ParseExact(fechaDemandaMaxima, "dd/MM/yyyy",null);

            var parametros = new object[]{
                maxId, periodo, fechaDemandaMaxima, fechaDemandaMaximaSicli, usuario, periodoSicli
            };
            //var stringSql = string.Format(helper.SqlSave, maxId, usuario, fechaDemandaMaxima);
            var stringSql = string.Format(helper.SqlSave, parametros);
            DbCommand command = dbProvider.GetSqlStringCommand(stringSql);

            var resultado = dbProvider.ExecuteNonQuery(command);
        }

        public void Update(string usuario, string periodo)
        {
            var stringSql = string.Format(helper.SqlUpdate, usuario, periodo);
            DbCommand command = dbProvider.GetSqlStringCommand(stringSql);

            dbProvider.ExecuteNonQuery(command);
        }
        public void Delete(string periodo)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Dmelibperiodo, DbType.String, periodo);

            dbProvider.ExecuteNonQuery(command);
        }

        public List<MeDemandaMLibreDTO> ListDemandaMercadoLibreReporte(string periodo, string suministrador, string empresa, int regIni, int regFin)
        {
            string condicion = " WHERE DMELIBPERIODO  = '" + periodo + "'";
            
            if (!string.IsNullOrEmpty(suministrador))
            {
                //condicion = condicion + " AND SU.EMPRCODISUMINISTRADOR IN (" + suministrador + ") ";
                condicion = condicion + " AND ME.EMPRCODI = " + suministrador;
            }
            if (!string.IsNullOrEmpty(empresa))
            {
                condicion = condicion + string.Format(" AND NVL(ECL.EMPRRAZSOCIAL,ECL.EMPRNOMB) LIKE '%{0}%' ", empresa.ToUpper());
            }

            string sqlQuery = string.Format(this.helper.SqlObtenerReporteDemandaMercadoLibre, condicion, regFin, regIni);
            List<MeDemandaMLibreDTO> entitys = new List<MeDemandaMLibreDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeDemandaMLibreDTO entity = new MeDemandaMLibreDTO();

                    int iItem = dr.GetOrdinal(helper.Item);
                    if (!dr.IsDBNull(iItem)) entity.Item = Convert.ToInt32(dr.GetValue(iItem));

                    int iDmelibperiodo = dr.GetOrdinal(helper.Dmelibperiodo);
                    if (!dr.IsDBNull(iDmelibperiodo)) entity.Dmelibperiodo = dr.GetString(iDmelibperiodo);

                    int iDmelibfuente = dr.GetOrdinal(helper.Dmelibfuente);
                    if (!dr.IsDBNull(iDmelibfuente)) entity.Dmelibfuente = dr.GetString(iDmelibfuente);

                    int iDmelibfecmaxdem = dr.GetOrdinal(helper.Dmelibfecmaxdem);
                    if (!dr.IsDBNull(iDmelibfecmaxdem)) entity.Dmelibfecmaxdem = dr.GetDateTime(iDmelibfecmaxdem);

                    //int iEmprCodi = dr.GetOrdinal(helper.Emprcodi);
                    //if (!dr.IsDBNull(iEmprCodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprCodi));

                    //int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    //if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iOsinergcodi = dr.GetOrdinal(helper.Osinergcodi);
                    if (!dr.IsDBNull(iOsinergcodi)) entity.Osinergcodi = dr.GetString(iOsinergcodi);

                    int iSuministrador = dr.GetOrdinal(helper.Suministrador);
                    if (!dr.IsDBNull(iSuministrador)) entity.Suministrador = dr.GetString(iSuministrador);

                    int iRuc = dr.GetOrdinal(helper.Emprruc);
                    if (!dr.IsDBNull(iRuc)) entity.Emprruc = dr.GetString(iRuc);

                    int iEmprrazsocial = dr.GetOrdinal(helper.Razonsocial);
                    if (!dr.IsDBNull(iEmprrazsocial)) entity.Razonsocial = dr.GetString(iEmprrazsocial);

                    int iNombresicli = dr.GetOrdinal(helper.Nombresicli);
                    if (!dr.IsDBNull(iNombresicli)) entity.Nombresicli = dr.GetString(iNombresicli);

                    int iAreanomb = dr.GetOrdinal(helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    int iEquitension = dr.GetOrdinal(helper.Equitension);
                    if (!dr.IsDBNull(iEquitension)) entity.Equitension = dr.GetDecimal(iEquitension);                  
                                       

                    int iDMELIBH1 = dr.GetOrdinal(helper.DMELIBH1);
                    if (!dr.IsDBNull(iDMELIBH1)) entity.Dmelibh1 = dr.GetDecimal(iDMELIBH1);

                    int iDMELIBH2 = dr.GetOrdinal(helper.DMELIBH2);
                    if (!dr.IsDBNull(iDMELIBH2)) entity.Dmelibh2 = dr.GetDecimal(iDMELIBH2);

                    int iDMELIBH3 = dr.GetOrdinal(helper.DMELIBH3);
                    if (!dr.IsDBNull(iDMELIBH3)) entity.Dmelibh3 = dr.GetDecimal(iDMELIBH3);

                    int iDMELIBH4 = dr.GetOrdinal(helper.DMELIBH4);
                    if (!dr.IsDBNull(iDMELIBH4)) entity.Dmelibh4 = dr.GetDecimal(iDMELIBH4);

                    int iDMELIBH5 = dr.GetOrdinal(helper.DMELIBH5);
                    if (!dr.IsDBNull(iDMELIBH5)) entity.Dmelibh5 = dr.GetDecimal(iDMELIBH5);

                    int iDMELIBH6 = dr.GetOrdinal(helper.DMELIBH6);
                    if (!dr.IsDBNull(iDMELIBH6)) entity.Dmelibh6 = dr.GetDecimal(iDMELIBH6);

                    int iDMELIBH7 = dr.GetOrdinal(helper.DMELIBH7);
                    if (!dr.IsDBNull(iDMELIBH7)) entity.Dmelibh7 = dr.GetDecimal(iDMELIBH7);

                    int iDMELIBH8 = dr.GetOrdinal(helper.DMELIBH8);
                    if (!dr.IsDBNull(iDMELIBH8)) entity.Dmelibh8 = dr.GetDecimal(iDMELIBH8);

                    int iDMELIBH9 = dr.GetOrdinal(helper.DMELIBH9);
                    if (!dr.IsDBNull(iDMELIBH9)) entity.Dmelibh9 = dr.GetDecimal(iDMELIBH9);

                    int iDMELIBH10 = dr.GetOrdinal(helper.DMELIBH10);
                    if (!dr.IsDBNull(iDMELIBH10)) entity.Dmelibh10 = dr.GetDecimal(iDMELIBH10);

                    int iDMELIBH11 = dr.GetOrdinal(helper.DMELIBH11);
                    if (!dr.IsDBNull(iDMELIBH11)) entity.Dmelibh11 = dr.GetDecimal(iDMELIBH11);

                    int iDMELIBH12 = dr.GetOrdinal(helper.DMELIBH12);
                    if (!dr.IsDBNull(iDMELIBH12)) entity.Dmelibh12 = dr.GetDecimal(iDMELIBH12);

                    int iDMELIBH13 = dr.GetOrdinal(helper.DMELIBH13);
                    if (!dr.IsDBNull(iDMELIBH13)) entity.Dmelibh13 = dr.GetDecimal(iDMELIBH13);

                    int iDMELIBH14 = dr.GetOrdinal(helper.DMELIBH14);
                    if (!dr.IsDBNull(iDMELIBH14)) entity.Dmelibh14 = dr.GetDecimal(iDMELIBH14);

                    int iDMELIBH15 = dr.GetOrdinal(helper.DMELIBH15);
                    if (!dr.IsDBNull(iDMELIBH15)) entity.Dmelibh15 = dr.GetDecimal(iDMELIBH15);

                    int iDMELIBH16 = dr.GetOrdinal(helper.DMELIBH16);
                    if (!dr.IsDBNull(iDMELIBH16)) entity.Dmelibh16 = dr.GetDecimal(iDMELIBH16);

                    int iDMELIBH17 = dr.GetOrdinal(helper.DMELIBH17);
                    if (!dr.IsDBNull(iDMELIBH17)) entity.Dmelibh17 = dr.GetDecimal(iDMELIBH17);

                    int iDMELIBH18 = dr.GetOrdinal(helper.DMELIBH18);
                    if (!dr.IsDBNull(iDMELIBH18)) entity.Dmelibh18 = dr.GetDecimal(iDMELIBH18);

                    int iDMELIBH19 = dr.GetOrdinal(helper.DMELIBH19);
                    if (!dr.IsDBNull(iDMELIBH19)) entity.Dmelibh19 = dr.GetDecimal(iDMELIBH19);

                    int iDMELIBH20 = dr.GetOrdinal(helper.DMELIBH20);
                    if (!dr.IsDBNull(iDMELIBH20)) entity.Dmelibh20 = dr.GetDecimal(iDMELIBH20);

                    int iDMELIBH21 = dr.GetOrdinal(helper.DMELIBH21);
                    if (!dr.IsDBNull(iDMELIBH21)) entity.Dmelibh21 = dr.GetDecimal(iDMELIBH21);

                    int iDMELIBH22 = dr.GetOrdinal(helper.DMELIBH22);
                    if (!dr.IsDBNull(iDMELIBH22)) entity.Dmelibh22 = dr.GetDecimal(iDMELIBH22);

                    int iDMELIBH23 = dr.GetOrdinal(helper.DMELIBH23);
                    if (!dr.IsDBNull(iDMELIBH23)) entity.Dmelibh23 = dr.GetDecimal(iDMELIBH23);

                    int iDMELIBH24 = dr.GetOrdinal(helper.DMELIBH24);
                    if (!dr.IsDBNull(iDMELIBH24)) entity.Dmelibh24 = dr.GetDecimal(iDMELIBH24);

                    int iDMELIBH25 = dr.GetOrdinal(helper.DMELIBH25);
                    if (!dr.IsDBNull(iDMELIBH25)) entity.Dmelibh25 = dr.GetDecimal(iDMELIBH25);

                    int iDMELIBH26 = dr.GetOrdinal(helper.DMELIBH26);
                    if (!dr.IsDBNull(iDMELIBH26)) entity.Dmelibh26 = dr.GetDecimal(iDMELIBH26);

                    int iDMELIBH27 = dr.GetOrdinal(helper.DMELIBH27);
                    if (!dr.IsDBNull(iDMELIBH27)) entity.Dmelibh27 = dr.GetDecimal(iDMELIBH27);

                    int iDMELIBH28 = dr.GetOrdinal(helper.DMELIBH28);
                    if (!dr.IsDBNull(iDMELIBH28)) entity.Dmelibh28 = dr.GetDecimal(iDMELIBH28);

                    int iDMELIBH29 = dr.GetOrdinal(helper.DMELIBH29);
                    if (!dr.IsDBNull(iDMELIBH29)) entity.Dmelibh29 = dr.GetDecimal(iDMELIBH29);

                    int iDMELIBH30 = dr.GetOrdinal(helper.DMELIBH30);
                    if (!dr.IsDBNull(iDMELIBH30)) entity.Dmelibh30 = dr.GetDecimal(iDMELIBH30);

                    int iDMELIBH31 = dr.GetOrdinal(helper.DMELIBH31);
                    if (!dr.IsDBNull(iDMELIBH31)) entity.Dmelibh31 = dr.GetDecimal(iDMELIBH31);

                    int iDMELIBH32 = dr.GetOrdinal(helper.DMELIBH32);
                    if (!dr.IsDBNull(iDMELIBH32)) entity.Dmelibh32 = dr.GetDecimal(iDMELIBH32);

                    int iDMELIBH33 = dr.GetOrdinal(helper.DMELIBH33);
                    if (!dr.IsDBNull(iDMELIBH33)) entity.Dmelibh33 = dr.GetDecimal(iDMELIBH33);

                    int iDMELIBH34 = dr.GetOrdinal(helper.DMELIBH34);
                    if (!dr.IsDBNull(iDMELIBH34)) entity.Dmelibh34 = dr.GetDecimal(iDMELIBH34);

                    int iDMELIBH35 = dr.GetOrdinal(helper.DMELIBH35);
                    if (!dr.IsDBNull(iDMELIBH35)) entity.Dmelibh35 = dr.GetDecimal(iDMELIBH35);

                    int iDMELIBH36 = dr.GetOrdinal(helper.DMELIBH36);
                    if (!dr.IsDBNull(iDMELIBH36)) entity.Dmelibh36 = dr.GetDecimal(iDMELIBH36);

                    int iDMELIBH37 = dr.GetOrdinal(helper.DMELIBH37);
                    if (!dr.IsDBNull(iDMELIBH37)) entity.Dmelibh37 = dr.GetDecimal(iDMELIBH37);

                    int iDMELIBH38 = dr.GetOrdinal(helper.DMELIBH38);
                    if (!dr.IsDBNull(iDMELIBH38)) entity.Dmelibh38 = dr.GetDecimal(iDMELIBH38);

                    int iDMELIBH39 = dr.GetOrdinal(helper.DMELIBH39);
                    if (!dr.IsDBNull(iDMELIBH39)) entity.Dmelibh39 = dr.GetDecimal(iDMELIBH39);

                    int iDMELIBH40 = dr.GetOrdinal(helper.DMELIBH40);
                    if (!dr.IsDBNull(iDMELIBH40)) entity.Dmelibh40 = dr.GetDecimal(iDMELIBH40);

                    int iDMELIBH41 = dr.GetOrdinal(helper.DMELIBH41);
                    if (!dr.IsDBNull(iDMELIBH41)) entity.Dmelibh41 = dr.GetDecimal(iDMELIBH41);

                    int iDMELIBH42 = dr.GetOrdinal(helper.DMELIBH42);
                    if (!dr.IsDBNull(iDMELIBH42)) entity.Dmelibh42 = dr.GetDecimal(iDMELIBH42);

                    int iDMELIBH43 = dr.GetOrdinal(helper.DMELIBH43);
                    if (!dr.IsDBNull(iDMELIBH43)) entity.Dmelibh43 = dr.GetDecimal(iDMELIBH43);

                    int iDMELIBH44 = dr.GetOrdinal(helper.DMELIBH44);
                    if (!dr.IsDBNull(iDMELIBH44)) entity.Dmelibh44 = dr.GetDecimal(iDMELIBH44);

                    int iDMELIBH45 = dr.GetOrdinal(helper.DMELIBH45);
                    if (!dr.IsDBNull(iDMELIBH45)) entity.Dmelibh45 = dr.GetDecimal(iDMELIBH45);

                    int iDMELIBH46 = dr.GetOrdinal(helper.DMELIBH46);
                    if (!dr.IsDBNull(iDMELIBH46)) entity.Dmelibh46 = dr.GetDecimal(iDMELIBH46);

                    int iDMELIBH47 = dr.GetOrdinal(helper.DMELIBH47);
                    if (!dr.IsDBNull(iDMELIBH47)) entity.Dmelibh47 = dr.GetDecimal(iDMELIBH47);

                    int iDMELIBH48 = dr.GetOrdinal(helper.DMELIBH48);
                    if (!dr.IsDBNull(iDMELIBH48)) entity.Dmelibh48 = dr.GetDecimal(iDMELIBH48);

                    int iDMELIBH49 = dr.GetOrdinal(helper.DMELIBH49);
                    if (!dr.IsDBNull(iDMELIBH49)) entity.Dmelibh49 = dr.GetDecimal(iDMELIBH49);

                    int iDMELIBH50 = dr.GetOrdinal(helper.DMELIBH50);
                    if (!dr.IsDBNull(iDMELIBH50)) entity.Dmelibh50 = dr.GetDecimal(iDMELIBH50);

                    int iDMELIBH51 = dr.GetOrdinal(helper.DMELIBH51);
                    if (!dr.IsDBNull(iDMELIBH51)) entity.Dmelibh51 = dr.GetDecimal(iDMELIBH51);

                    int iDMELIBH52 = dr.GetOrdinal(helper.DMELIBH52);
                    if (!dr.IsDBNull(iDMELIBH52)) entity.Dmelibh52 = dr.GetDecimal(iDMELIBH52);

                    int iDMELIBH53 = dr.GetOrdinal(helper.DMELIBH53);
                    if (!dr.IsDBNull(iDMELIBH53)) entity.Dmelibh53 = dr.GetDecimal(iDMELIBH53);

                    int iDMELIBH54 = dr.GetOrdinal(helper.DMELIBH54);
                    if (!dr.IsDBNull(iDMELIBH54)) entity.Dmelibh54 = dr.GetDecimal(iDMELIBH54);

                    int iDMELIBH55 = dr.GetOrdinal(helper.DMELIBH55);
                    if (!dr.IsDBNull(iDMELIBH55)) entity.Dmelibh55 = dr.GetDecimal(iDMELIBH55);

                    int iDMELIBH56 = dr.GetOrdinal(helper.DMELIBH56);
                    if (!dr.IsDBNull(iDMELIBH56)) entity.Dmelibh56 = dr.GetDecimal(iDMELIBH56);

                    int iDMELIBH57 = dr.GetOrdinal(helper.DMELIBH57);
                    if (!dr.IsDBNull(iDMELIBH57)) entity.Dmelibh57 = dr.GetDecimal(iDMELIBH57);

                    int iDMELIBH58 = dr.GetOrdinal(helper.DMELIBH58);
                    if (!dr.IsDBNull(iDMELIBH58)) entity.Dmelibh58 = dr.GetDecimal(iDMELIBH58);

                    int iDMELIBH59 = dr.GetOrdinal(helper.DMELIBH59);
                    if (!dr.IsDBNull(iDMELIBH59)) entity.Dmelibh59 = dr.GetDecimal(iDMELIBH59);

                    int iDMELIBH60 = dr.GetOrdinal(helper.DMELIBH60);
                    if (!dr.IsDBNull(iDMELIBH60)) entity.Dmelibh60 = dr.GetDecimal(iDMELIBH60);

                    int iDMELIBH61 = dr.GetOrdinal(helper.DMELIBH61);
                    if (!dr.IsDBNull(iDMELIBH61)) entity.Dmelibh61 = dr.GetDecimal(iDMELIBH61);

                    int iDMELIBH62 = dr.GetOrdinal(helper.DMELIBH62);
                    if (!dr.IsDBNull(iDMELIBH62)) entity.Dmelibh62 = dr.GetDecimal(iDMELIBH62);

                    int iDMELIBH63 = dr.GetOrdinal(helper.DMELIBH63);
                    if (!dr.IsDBNull(iDMELIBH63)) entity.Dmelibh63 = dr.GetDecimal(iDMELIBH63);

                    int iDMELIBH64 = dr.GetOrdinal(helper.DMELIBH64);
                    if (!dr.IsDBNull(iDMELIBH64)) entity.Dmelibh64 = dr.GetDecimal(iDMELIBH64);

                    int iDMELIBH65 = dr.GetOrdinal(helper.DMELIBH65);
                    if (!dr.IsDBNull(iDMELIBH65)) entity.Dmelibh65 = dr.GetDecimal(iDMELIBH65);

                    int iDMELIBH66 = dr.GetOrdinal(helper.DMELIBH66);
                    if (!dr.IsDBNull(iDMELIBH66)) entity.Dmelibh66 = dr.GetDecimal(iDMELIBH66);

                    int iDMELIBH67 = dr.GetOrdinal(helper.DMELIBH67);
                    if (!dr.IsDBNull(iDMELIBH67)) entity.Dmelibh67 = dr.GetDecimal(iDMELIBH67);

                    int iDMELIBH68 = dr.GetOrdinal(helper.DMELIBH68);
                    if (!dr.IsDBNull(iDMELIBH68)) entity.Dmelibh68 = dr.GetDecimal(iDMELIBH68);

                    int iDMELIBH69 = dr.GetOrdinal(helper.DMELIBH69);
                    if (!dr.IsDBNull(iDMELIBH69)) entity.Dmelibh69 = dr.GetDecimal(iDMELIBH69);

                    int iDMELIBH70 = dr.GetOrdinal(helper.DMELIBH70);
                    if (!dr.IsDBNull(iDMELIBH70)) entity.Dmelibh70 = dr.GetDecimal(iDMELIBH70);

                    int iDMELIBH71 = dr.GetOrdinal(helper.DMELIBH71);
                    if (!dr.IsDBNull(iDMELIBH71)) entity.Dmelibh71 = dr.GetDecimal(iDMELIBH71);

                    int iDMELIBH72 = dr.GetOrdinal(helper.DMELIBH72);
                    if (!dr.IsDBNull(iDMELIBH72)) entity.Dmelibh72 = dr.GetDecimal(iDMELIBH72);

                    int iDMELIBH73 = dr.GetOrdinal(helper.DMELIBH73);
                    if (!dr.IsDBNull(iDMELIBH73)) entity.Dmelibh73 = dr.GetDecimal(iDMELIBH73);

                    int iDMELIBH74 = dr.GetOrdinal(helper.DMELIBH74);
                    if (!dr.IsDBNull(iDMELIBH74)) entity.Dmelibh74 = dr.GetDecimal(iDMELIBH74);

                    int iDMELIBH75 = dr.GetOrdinal(helper.DMELIBH75);
                    if (!dr.IsDBNull(iDMELIBH75)) entity.Dmelibh75 = dr.GetDecimal(iDMELIBH75);

                    int iDMELIBH76 = dr.GetOrdinal(helper.DMELIBH76);
                    if (!dr.IsDBNull(iDMELIBH76)) entity.Dmelibh76 = dr.GetDecimal(iDMELIBH76);

                    int iDMELIBH77 = dr.GetOrdinal(helper.DMELIBH77);
                    if (!dr.IsDBNull(iDMELIBH77)) entity.Dmelibh77 = dr.GetDecimal(iDMELIBH77);

                    int iDMELIBH78 = dr.GetOrdinal(helper.DMELIBH78);
                    if (!dr.IsDBNull(iDMELIBH78)) entity.Dmelibh78 = dr.GetDecimal(iDMELIBH78);

                    int iDMELIBH79 = dr.GetOrdinal(helper.DMELIBH79);
                    if (!dr.IsDBNull(iDMELIBH79)) entity.Dmelibh79 = dr.GetDecimal(iDMELIBH79);

                    int iDMELIBH80 = dr.GetOrdinal(helper.DMELIBH80);
                    if (!dr.IsDBNull(iDMELIBH80)) entity.Dmelibh80 = dr.GetDecimal(iDMELIBH80);

                    int iDMELIBH81 = dr.GetOrdinal(helper.DMELIBH81);
                    if (!dr.IsDBNull(iDMELIBH81)) entity.Dmelibh81 = dr.GetDecimal(iDMELIBH81);

                    int iDMELIBH82 = dr.GetOrdinal(helper.DMELIBH82);
                    if (!dr.IsDBNull(iDMELIBH82)) entity.Dmelibh82 = dr.GetDecimal(iDMELIBH82);

                    int iDMELIBH83 = dr.GetOrdinal(helper.DMELIBH83);
                    if (!dr.IsDBNull(iDMELIBH83)) entity.Dmelibh83 = dr.GetDecimal(iDMELIBH83);

                    int iDMELIBH84 = dr.GetOrdinal(helper.DMELIBH84);
                    if (!dr.IsDBNull(iDMELIBH84)) entity.Dmelibh84 = dr.GetDecimal(iDMELIBH84);

                    int iDMELIBH85 = dr.GetOrdinal(helper.DMELIBH85);
                    if (!dr.IsDBNull(iDMELIBH85)) entity.Dmelibh85 = dr.GetDecimal(iDMELIBH85);

                    int iDMELIBH86 = dr.GetOrdinal(helper.DMELIBH86);
                    if (!dr.IsDBNull(iDMELIBH86)) entity.Dmelibh86 = dr.GetDecimal(iDMELIBH86);

                    int iDMELIBH87 = dr.GetOrdinal(helper.DMELIBH87);
                    if (!dr.IsDBNull(iDMELIBH87)) entity.Dmelibh87 = dr.GetDecimal(iDMELIBH87);

                    int iDMELIBH88 = dr.GetOrdinal(helper.DMELIBH88);
                    if (!dr.IsDBNull(iDMELIBH88)) entity.Dmelibh88 = dr.GetDecimal(iDMELIBH88);

                    int iDMELIBH89 = dr.GetOrdinal(helper.DMELIBH89);
                    if (!dr.IsDBNull(iDMELIBH89)) entity.Dmelibh89 = dr.GetDecimal(iDMELIBH89);

                    int iDMELIBH90 = dr.GetOrdinal(helper.DMELIBH90);
                    if (!dr.IsDBNull(iDMELIBH90)) entity.Dmelibh90 = dr.GetDecimal(iDMELIBH90);

                    int iDMELIBH91 = dr.GetOrdinal(helper.DMELIBH91);
                    if (!dr.IsDBNull(iDMELIBH91)) entity.Dmelibh91 = dr.GetDecimal(iDMELIBH91);

                    int iDMELIBH92 = dr.GetOrdinal(helper.DMELIBH92);
                    if (!dr.IsDBNull(iDMELIBH92)) entity.Dmelibh92 = dr.GetDecimal(iDMELIBH92);

                    int iDMELIBH93 = dr.GetOrdinal(helper.DMELIBH93);
                    if (!dr.IsDBNull(iDMELIBH93)) entity.Dmelibh93 = dr.GetDecimal(iDMELIBH93);

                    int iDMELIBH94 = dr.GetOrdinal(helper.DMELIBH94);
                    if (!dr.IsDBNull(iDMELIBH94)) entity.Dmelibh94 = dr.GetDecimal(iDMELIBH94);

                    int iDMELIBH95 = dr.GetOrdinal(helper.DMELIBH95);
                    if (!dr.IsDBNull(iDMELIBH95)) entity.Dmelibh95 = dr.GetDecimal(iDMELIBH95);

                    int iDMELIBH96 = dr.GetOrdinal(helper.DMELIBH96);
                    if (!dr.IsDBNull(iDMELIBH96)) entity.Dmelibh96 = dr.GetDecimal(iDMELIBH96);                    

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public int ListDemandaMercadoLibreReporteCount(string periodo, string suministrador, string empresa)
        {
            string condicion = " WHERE DMELIBPERIODO = '" + periodo + "'";

            if (!string.IsNullOrEmpty(suministrador))
            {
                //condicion = condicion + " AND SU.EMPRCODISUMINISTRADOR IN (" + suministrador + ") ";
                condicion = condicion + " AND ME.EMPRCODI = " + suministrador;
            }
            if (!string.IsNullOrEmpty(empresa))
            {
                condicion = condicion + string.Format(" AND NVL(ECL.EMPRRAZSOCIAL,ECL.EMPRNOMB) LIKE '%{0}%' ", empresa.ToUpper());
            } 
            
            string sqlQuery = string.Format(this.helper.SqlObtenerReporteDemandaMercadoLibreCount, condicion);
            
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            int cant = 0;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    int iQregistros = dr.GetOrdinal(helper.Qregistros);
                    cant = Convert.ToInt32(dr.GetValue(iQregistros));
                }
            }
            return cant;
        }

        public IDataReader ListDemandaMercadoLibreReporteExcel(string periodo, string periodoSICLI, string suministrador, string empresa)
        {
            string condicion = " WHERE DMELIBPERIODO  = '" + periodo + "'";

            if (!string.IsNullOrEmpty(suministrador))
            {
                //condicion = condicion + " AND SU.EMPRCODISUMINISTRADOR IN (" + suministrador + ") ";
                condicion = condicion + " AND ME.EMPRCODI = " + suministrador;
            }
            if (!string.IsNullOrEmpty(empresa))
            {
                condicion = condicion + string.Format(" AND NVL(ECL.EMPRRAZSOCIAL,ECL.EMPRNOMB) LIKE '%{0}%' ", empresa.ToUpper());
            }

            string sqlQuery = string.Format(this.helper.SqlObtenerReporteDemandaMercadoLibreExcel, condicion, periodoSICLI);
            List<MeDemandaMLibreDTO> entitys = new List<MeDemandaMLibreDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            IDataReader dr = dbProvider.ExecuteReader(command);

            return dr;
            //return entitys;
        }

        public List<IioPeriodoSicliDTO> ListPeriodoSicli(string permiso)
        {
            string sqlQuery = string.Format(this.helper.SqlObtenerPeriodoSicli, permiso);
            List<IioPeriodoSicliDTO> entitys = new List<IioPeriodoSicliDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    IioPeriodoSicliDTO entity = new IioPeriodoSicliDTO();

                    int iPsicliAnioMesPerrem = dr.GetOrdinal(IioPeriodoSicliHelper.PsicliAnioMesPerrem);
                    if (!dr.IsDBNull(iPsicliAnioMesPerrem)) entity.PsicliAnioMesPerrem = dr.GetString(iPsicliAnioMesPerrem);                   
                    

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public int  UpdatePeriodoDemandaSicli(string usuario, string periodo, string estadoPeriodo)
        {
            var stringSql = string.Format(helper.SqlUpdatePeriodoDemandaSicli, estadoPeriodo, usuario, periodo);
            DbCommand command = dbProvider.GetSqlStringCommand(stringSql);

            var res = dbProvider.ExecuteNonQuery(command);


            return res;
        }
              
    }
}
