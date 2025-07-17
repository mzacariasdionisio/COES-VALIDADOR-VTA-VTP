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
    public class RcaDemandaUsuarioRepository : RepositoryBase, IRcaDemandaUsuarioRepository
    {
        public RcaDemandaUsuarioRepository(string strConn)
            : base(strConn)
        {
        }

        RcaDemandaUsuarioHelper helper = new RcaDemandaUsuarioHelper();

        public int GetMaxId()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }
        public void Save(RcaDemandaUsuarioDTO entity)
        {
            var stringSql = string.Format(helper.SqlSave, entity.Rcdeulfecmaxdem.ToString("dd/MM/yyyy"));
            DbCommand command = dbProvider.GetSqlStringCommand(stringSql);

           
            
            dbProvider.AddInParameter(command, helper.Rcdeulcodi, DbType.Int32, entity.Rcdeulcodi);
            dbProvider.AddInParameter(command, helper.Rcdeulperiodo, DbType.String, entity.Rcdeulperiodo);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);

            dbProvider.AddInParameter(command, helper.Rcdeuldemandahp, DbType.Decimal, entity.Rcdeuldemandahp);
            dbProvider.AddInParameter(command, helper.Rcdeuldemandahfp, DbType.Decimal, entity.Rcdeuldemandahfp);
            dbProvider.AddInParameter(command, helper.Rcdeulfuente, DbType.String, entity.Rcdeulfuente);
            //dbProvider.AddInParameter(command, helper.Rcdeulfecmaxdem, DbType.DateTime, entity.Rcdeulfecmaxdem);
            dbProvider.AddInParameter(command, helper.Osinergcodi, DbType.String, entity.Osinergcodi);

            dbProvider.AddInParameter(command, helper.Rcdeulusucreacion, DbType.String, entity.Rcdeulusucreacion);
            dbProvider.AddInParameter(command, helper.Rcdeulfeccreacion, DbType.DateTime, entity.Rcdeulfeccreacion);

            dbProvider.AddInParameter(command, helper.RCDEULH96, DbType.Decimal, entity.RCDEULH96);
            dbProvider.AddInParameter(command, helper.RCDEULH95, DbType.Decimal, entity.RCDEULH95);
            dbProvider.AddInParameter(command, helper.RCDEULH94, DbType.Decimal, entity.RCDEULH94);
            dbProvider.AddInParameter(command, helper.RCDEULH93, DbType.Decimal, entity.RCDEULH93);
            dbProvider.AddInParameter(command, helper.RCDEULH92, DbType.Decimal, entity.RCDEULH92);
            dbProvider.AddInParameter(command, helper.RCDEULH91, DbType.Decimal, entity.RCDEULH91);
            dbProvider.AddInParameter(command, helper.RCDEULH90, DbType.Decimal, entity.RCDEULH90);
            dbProvider.AddInParameter(command, helper.RCDEULH89, DbType.Decimal, entity.RCDEULH89);
            dbProvider.AddInParameter(command, helper.RCDEULH88, DbType.Decimal, entity.RCDEULH88);
            dbProvider.AddInParameter(command, helper.RCDEULH87, DbType.Decimal, entity.RCDEULH87);
            dbProvider.AddInParameter(command, helper.RCDEULH86, DbType.Decimal, entity.RCDEULH86);
            dbProvider.AddInParameter(command, helper.RCDEULH85, DbType.Decimal, entity.RCDEULH85);
            dbProvider.AddInParameter(command, helper.RCDEULH84, DbType.Decimal, entity.RCDEULH84);
            dbProvider.AddInParameter(command, helper.RCDEULH83, DbType.Decimal, entity.RCDEULH83);
            dbProvider.AddInParameter(command, helper.RCDEULH82, DbType.Decimal, entity.RCDEULH82);
            dbProvider.AddInParameter(command, helper.RCDEULH81, DbType.Decimal, entity.RCDEULH81);
            dbProvider.AddInParameter(command, helper.RCDEULH80, DbType.Decimal, entity.RCDEULH80);
            dbProvider.AddInParameter(command, helper.RCDEULH79, DbType.Decimal, entity.RCDEULH79);
            dbProvider.AddInParameter(command, helper.RCDEULH78, DbType.Decimal, entity.RCDEULH78);
            dbProvider.AddInParameter(command, helper.RCDEULH77, DbType.Decimal, entity.RCDEULH77);
            dbProvider.AddInParameter(command, helper.RCDEULH76, DbType.Decimal, entity.RCDEULH76);
            dbProvider.AddInParameter(command, helper.RCDEULH75, DbType.Decimal, entity.RCDEULH75);
            dbProvider.AddInParameter(command, helper.RCDEULH74, DbType.Decimal, entity.RCDEULH74);
            dbProvider.AddInParameter(command, helper.RCDEULH73, DbType.Decimal, entity.RCDEULH73);
            dbProvider.AddInParameter(command, helper.RCDEULH72, DbType.Decimal, entity.RCDEULH72);
            dbProvider.AddInParameter(command, helper.RCDEULH71, DbType.Decimal, entity.RCDEULH71);
            dbProvider.AddInParameter(command, helper.RCDEULH70, DbType.Decimal, entity.RCDEULH70);
            dbProvider.AddInParameter(command, helper.RCDEULH69, DbType.Decimal, entity.RCDEULH69);
            dbProvider.AddInParameter(command, helper.RCDEULH68, DbType.Decimal, entity.RCDEULH68);
            dbProvider.AddInParameter(command, helper.RCDEULH67, DbType.Decimal, entity.RCDEULH67);
            dbProvider.AddInParameter(command, helper.RCDEULH66, DbType.Decimal, entity.RCDEULH66);
            dbProvider.AddInParameter(command, helper.RCDEULH65, DbType.Decimal, entity.RCDEULH65);
            dbProvider.AddInParameter(command, helper.RCDEULH64, DbType.Decimal, entity.RCDEULH64);
            dbProvider.AddInParameter(command, helper.RCDEULH63, DbType.Decimal, entity.RCDEULH63);
            dbProvider.AddInParameter(command, helper.RCDEULH62, DbType.Decimal, entity.RCDEULH62);
            dbProvider.AddInParameter(command, helper.RCDEULH61, DbType.Decimal, entity.RCDEULH61);
            dbProvider.AddInParameter(command, helper.RCDEULH60, DbType.Decimal, entity.RCDEULH60);
            dbProvider.AddInParameter(command, helper.RCDEULH59, DbType.Decimal, entity.RCDEULH59);
            dbProvider.AddInParameter(command, helper.RCDEULH58, DbType.Decimal, entity.RCDEULH58);
            dbProvider.AddInParameter(command, helper.RCDEULH57, DbType.Decimal, entity.RCDEULH57);
            dbProvider.AddInParameter(command, helper.RCDEULH56, DbType.Decimal, entity.RCDEULH56);
            dbProvider.AddInParameter(command, helper.RCDEULH55, DbType.Decimal, entity.RCDEULH55);
            dbProvider.AddInParameter(command, helper.RCDEULH54, DbType.Decimal, entity.RCDEULH54);
            dbProvider.AddInParameter(command, helper.RCDEULH53, DbType.Decimal, entity.RCDEULH53);
            dbProvider.AddInParameter(command, helper.RCDEULH52, DbType.Decimal, entity.RCDEULH52);
            dbProvider.AddInParameter(command, helper.RCDEULH51, DbType.Decimal, entity.RCDEULH51);
            dbProvider.AddInParameter(command, helper.RCDEULH50, DbType.Decimal, entity.RCDEULH50);
            dbProvider.AddInParameter(command, helper.RCDEULH49, DbType.Decimal, entity.RCDEULH49);
            dbProvider.AddInParameter(command, helper.RCDEULH48, DbType.Decimal, entity.RCDEULH48);
            dbProvider.AddInParameter(command, helper.RCDEULH47, DbType.Decimal, entity.RCDEULH47);
            dbProvider.AddInParameter(command, helper.RCDEULH46, DbType.Decimal, entity.RCDEULH46);
            dbProvider.AddInParameter(command, helper.RCDEULH45, DbType.Decimal, entity.RCDEULH45);
            dbProvider.AddInParameter(command, helper.RCDEULH44, DbType.Decimal, entity.RCDEULH44);
            dbProvider.AddInParameter(command, helper.RCDEULH43, DbType.Decimal, entity.RCDEULH43);
            dbProvider.AddInParameter(command, helper.RCDEULH42, DbType.Decimal, entity.RCDEULH42);
            dbProvider.AddInParameter(command, helper.RCDEULH41, DbType.Decimal, entity.RCDEULH41);
            dbProvider.AddInParameter(command, helper.RCDEULH40, DbType.Decimal, entity.RCDEULH40);
            dbProvider.AddInParameter(command, helper.RCDEULH39, DbType.Decimal, entity.RCDEULH39);
            dbProvider.AddInParameter(command, helper.RCDEULH38, DbType.Decimal, entity.RCDEULH38);
            dbProvider.AddInParameter(command, helper.RCDEULH37, DbType.Decimal, entity.RCDEULH37);
            dbProvider.AddInParameter(command, helper.RCDEULH36, DbType.Decimal, entity.RCDEULH36);
            dbProvider.AddInParameter(command, helper.RCDEULH35, DbType.Decimal, entity.RCDEULH35);
            dbProvider.AddInParameter(command, helper.RCDEULH34, DbType.Decimal, entity.RCDEULH34);
            dbProvider.AddInParameter(command, helper.RCDEULH33, DbType.Decimal, entity.RCDEULH33);
            dbProvider.AddInParameter(command, helper.RCDEULH32, DbType.Decimal, entity.RCDEULH32);
            dbProvider.AddInParameter(command, helper.RCDEULH31, DbType.Decimal, entity.RCDEULH31);
            dbProvider.AddInParameter(command, helper.RCDEULH30, DbType.Decimal, entity.RCDEULH30);
            dbProvider.AddInParameter(command, helper.RCDEULH29, DbType.Decimal, entity.RCDEULH29);
            dbProvider.AddInParameter(command, helper.RCDEULH28, DbType.Decimal, entity.RCDEULH28);
            dbProvider.AddInParameter(command, helper.RCDEULH27, DbType.Decimal, entity.RCDEULH27);
            dbProvider.AddInParameter(command, helper.RCDEULH26, DbType.Decimal, entity.RCDEULH26);
            dbProvider.AddInParameter(command, helper.RCDEULH25, DbType.Decimal, entity.RCDEULH25);
            dbProvider.AddInParameter(command, helper.RCDEULH24, DbType.Decimal, entity.RCDEULH24);
            dbProvider.AddInParameter(command, helper.RCDEULH23, DbType.Decimal, entity.RCDEULH23);
            dbProvider.AddInParameter(command, helper.RCDEULH22, DbType.Decimal, entity.RCDEULH22);
            dbProvider.AddInParameter(command, helper.RCDEULH21, DbType.Decimal, entity.RCDEULH21);
            dbProvider.AddInParameter(command, helper.RCDEULH20, DbType.Decimal, entity.RCDEULH20);
            dbProvider.AddInParameter(command, helper.RCDEULH19, DbType.Decimal, entity.RCDEULH19);
            dbProvider.AddInParameter(command, helper.RCDEULH18, DbType.Decimal, entity.RCDEULH18);
            dbProvider.AddInParameter(command, helper.RCDEULH17, DbType.Decimal, entity.RCDEULH17);
            dbProvider.AddInParameter(command, helper.RCDEULH16, DbType.Decimal, entity.RCDEULH16);
            dbProvider.AddInParameter(command, helper.RCDEULH15, DbType.Decimal, entity.RCDEULH15);
            dbProvider.AddInParameter(command, helper.RCDEULH14, DbType.Decimal, entity.RCDEULH14);
            dbProvider.AddInParameter(command, helper.RCDEULH13, DbType.Decimal, entity.RCDEULH13);
            dbProvider.AddInParameter(command, helper.RCDEULH12, DbType.Decimal, entity.RCDEULH12);
            dbProvider.AddInParameter(command, helper.RCDEULH11, DbType.Decimal, entity.RCDEULH11);
            dbProvider.AddInParameter(command, helper.RCDEULH10, DbType.Decimal, entity.RCDEULH10);
            dbProvider.AddInParameter(command, helper.RCDEULH9, DbType.Decimal, entity.RCDEULH9);
            dbProvider.AddInParameter(command, helper.RCDEULH8, DbType.Decimal, entity.RCDEULH8);
            dbProvider.AddInParameter(command, helper.RCDEULH7, DbType.Decimal, entity.RCDEULH7);
            dbProvider.AddInParameter(command, helper.RCDEULH6, DbType.Decimal, entity.RCDEULH6);
            dbProvider.AddInParameter(command, helper.RCDEULH5, DbType.Decimal, entity.RCDEULH5);
            dbProvider.AddInParameter(command, helper.RCDEULH4, DbType.Decimal, entity.RCDEULH4);
            dbProvider.AddInParameter(command, helper.RCDEULH3, DbType.Decimal, entity.RCDEULH3);
            dbProvider.AddInParameter(command, helper.RCDEULH2, DbType.Decimal, entity.RCDEULH2);
            dbProvider.AddInParameter(command, helper.RCDEULH1, DbType.Decimal, entity.RCDEULH1);
            

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(string periodo)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Rcdeulperiodo, DbType.String, periodo);

            dbProvider.ExecuteNonQuery(command);
        }

        public List<RcaDemandaUsuarioDTO> ListDemandaUsuarioReporte(string periodo, string codigoZona, string codigoPuntoMedicion, string empresa, string suministrador, int regIni, int regFin)
        {
            string condicion = " WHERE CP.RCDEULPERIODO = '" + periodo + "'";

            if (!codigoZona.Equals("0"))
            {
                //condicion = condicion + " AND AR.AREACODI = " + codigoZona;
            }

            if (!string.IsNullOrEmpty(codigoPuntoMedicion))
            {
                condicion = condicion + " AND EQ.AREACODI IN (" + codigoPuntoMedicion + ") ";
            }
            if (!string.IsNullOrEmpty(suministrador))
            {
                condicion = condicion + " AND CP.EMPRCODI IN (" + suministrador + ") ";
            }
            if (!string.IsNullOrEmpty(empresa))
            {
                condicion = condicion + string.Format(" AND NVL(CL.EMPRRAZSOCIAL,CL.EMPRABREV) LIKE '%{0}%' ", empresa.ToUpper());
            }            

            string sqlQuery = string.Format(this.helper.SqlObtenerReporteDemandaUsuario, condicion, regFin, regIni);
            List<RcaDemandaUsuarioDTO> entitys = new List<RcaDemandaUsuarioDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RcaDemandaUsuarioDTO entity = new RcaDemandaUsuarioDTO();

                    int iItem = dr.GetOrdinal(helper.Item);
                    if (!dr.IsDBNull(iItem)) entity.Item = Convert.ToInt32(dr.GetValue(iItem));

                    int iRcdeulperiodo = dr.GetOrdinal(helper.Rcdeulperiodo);
                    if (!dr.IsDBNull(iRcdeulperiodo)) entity.Rcdeulperiodo = dr.GetString(iRcdeulperiodo);

                    //int iRcdeulfecmaxdem = dr.GetOrdinal(helper.Rcdeulfecmaxdem);
                    //if (!dr.IsDBNull(iRcdeulfecmaxdem)) entity.Rcdeulfecmaxdem = dr.GetDateTime(iRcdeulfecmaxdem);

                    //int iEmprCodi = dr.GetOrdinal(helper.Emprcodi);
                    //if (!dr.IsDBNull(iEmprCodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprCodi));

                    //int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    //if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iOsinergcodi = dr.GetOrdinal(helper.Osinergcodi);
                    if (!dr.IsDBNull(iOsinergcodi)) entity.Osinergcodi = dr.GetString(iOsinergcodi);

                    int iSuministrador = dr.GetOrdinal(helper.Suministrador);
                    if (!dr.IsDBNull(iSuministrador)) entity.Suministrador = dr.GetString(iSuministrador);

                    int iRuc = dr.GetOrdinal(helper.Ruc);
                    if (!dr.IsDBNull(iRuc)) entity.Ruc = dr.GetString(iRuc);

                    int iEmprrazsocial = dr.GetOrdinal(helper.Emprrazsocial);
                    if (!dr.IsDBNull(iEmprrazsocial)) entity.Emprrazsocial = dr.GetString(iEmprrazsocial);

                    int iSubestacion = dr.GetOrdinal(helper.Subestacion);
                    if (!dr.IsDBNull(iSubestacion)) entity.Subestacion = dr.GetString(iSubestacion);

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    //int iRcdeulfuente = dr.GetOrdinal(helper.Rcdeulfuente);
                    //if (!dr.IsDBNull(iRcdeulfuente)) entity.Rcdeulfuente = dr.GetString(iRcdeulfuente);

                    int iRcdeuldemandahp = dr.GetOrdinal(helper.Rcdeuldemandahp);
                    if (!dr.IsDBNull(iRcdeuldemandahp)) entity.Rcdeuldemandahp = dr.GetDecimal(iRcdeuldemandahp);

                    int iRcdeuldemandahfp = dr.GetOrdinal(helper.Rcdeuldemandahfp);
                    if (!dr.IsDBNull(iRcdeuldemandahfp)) entity.Rcdeuldemandahfp = dr.GetDecimal(iRcdeuldemandahfp);

                    int iRCDEULH1 = dr.GetOrdinal(helper.RCDEULH1);
                    if (!dr.IsDBNull(iRCDEULH1)) entity.RCDEULH1 = dr.GetDecimal(iRCDEULH1);

                    int iRCDEULH2 = dr.GetOrdinal(helper.RCDEULH2);
                    if (!dr.IsDBNull(iRCDEULH2)) entity.RCDEULH2 = dr.GetDecimal(iRCDEULH2);

                    int iRCDEULH3 = dr.GetOrdinal(helper.RCDEULH3);
                    if (!dr.IsDBNull(iRCDEULH3)) entity.RCDEULH3 = dr.GetDecimal(iRCDEULH3);

                    int iRCDEULH4 = dr.GetOrdinal(helper.RCDEULH4);
                    if (!dr.IsDBNull(iRCDEULH4)) entity.RCDEULH4 = dr.GetDecimal(iRCDEULH4);

                    int iRCDEULH5 = dr.GetOrdinal(helper.RCDEULH5);
                    if (!dr.IsDBNull(iRCDEULH5)) entity.RCDEULH5 = dr.GetDecimal(iRCDEULH5);

                    int iRCDEULH6 = dr.GetOrdinal(helper.RCDEULH6);
                    if (!dr.IsDBNull(iRCDEULH6)) entity.RCDEULH6 = dr.GetDecimal(iRCDEULH6);

                    int iRCDEULH7 = dr.GetOrdinal(helper.RCDEULH7);
                    if (!dr.IsDBNull(iRCDEULH7)) entity.RCDEULH7 = dr.GetDecimal(iRCDEULH7);

                    int iRCDEULH8 = dr.GetOrdinal(helper.RCDEULH8);
                    if (!dr.IsDBNull(iRCDEULH8)) entity.RCDEULH8 = dr.GetDecimal(iRCDEULH8);

                    int iRCDEULH9 = dr.GetOrdinal(helper.RCDEULH9);
                    if (!dr.IsDBNull(iRCDEULH9)) entity.RCDEULH9 = dr.GetDecimal(iRCDEULH9);

                    int iRCDEULH10 = dr.GetOrdinal(helper.RCDEULH10);
                    if (!dr.IsDBNull(iRCDEULH10)) entity.RCDEULH10 = dr.GetDecimal(iRCDEULH10);

                    int iRCDEULH11 = dr.GetOrdinal(helper.RCDEULH11);
                    if (!dr.IsDBNull(iRCDEULH11)) entity.RCDEULH11 = dr.GetDecimal(iRCDEULH11);

                    int iRCDEULH12 = dr.GetOrdinal(helper.RCDEULH12);
                    if (!dr.IsDBNull(iRCDEULH12)) entity.RCDEULH12 = dr.GetDecimal(iRCDEULH12);

                    int iRCDEULH13 = dr.GetOrdinal(helper.RCDEULH13);
                    if (!dr.IsDBNull(iRCDEULH13)) entity.RCDEULH13 = dr.GetDecimal(iRCDEULH13);

                    int iRCDEULH14 = dr.GetOrdinal(helper.RCDEULH14);
                    if (!dr.IsDBNull(iRCDEULH14)) entity.RCDEULH14 = dr.GetDecimal(iRCDEULH14);

                    int iRCDEULH15 = dr.GetOrdinal(helper.RCDEULH15);
                    if (!dr.IsDBNull(iRCDEULH15)) entity.RCDEULH15 = dr.GetDecimal(iRCDEULH15);

                    int iRCDEULH16 = dr.GetOrdinal(helper.RCDEULH16);
                    if (!dr.IsDBNull(iRCDEULH16)) entity.RCDEULH16 = dr.GetDecimal(iRCDEULH16);

                    int iRCDEULH17 = dr.GetOrdinal(helper.RCDEULH17);
                    if (!dr.IsDBNull(iRCDEULH17)) entity.RCDEULH17 = dr.GetDecimal(iRCDEULH17);

                    int iRCDEULH18 = dr.GetOrdinal(helper.RCDEULH18);
                    if (!dr.IsDBNull(iRCDEULH18)) entity.RCDEULH18 = dr.GetDecimal(iRCDEULH18);

                    int iRCDEULH19 = dr.GetOrdinal(helper.RCDEULH19);
                    if (!dr.IsDBNull(iRCDEULH19)) entity.RCDEULH19 = dr.GetDecimal(iRCDEULH19);

                    int iRCDEULH20 = dr.GetOrdinal(helper.RCDEULH20);
                    if (!dr.IsDBNull(iRCDEULH20)) entity.RCDEULH20 = dr.GetDecimal(iRCDEULH20);

                    int iRCDEULH21 = dr.GetOrdinal(helper.RCDEULH21);
                    if (!dr.IsDBNull(iRCDEULH21)) entity.RCDEULH21 = dr.GetDecimal(iRCDEULH21);

                    int iRCDEULH22 = dr.GetOrdinal(helper.RCDEULH22);
                    if (!dr.IsDBNull(iRCDEULH22)) entity.RCDEULH22 = dr.GetDecimal(iRCDEULH22);

                    int iRCDEULH23 = dr.GetOrdinal(helper.RCDEULH23);
                    if (!dr.IsDBNull(iRCDEULH23)) entity.RCDEULH23 = dr.GetDecimal(iRCDEULH23);

                    int iRCDEULH24 = dr.GetOrdinal(helper.RCDEULH24);
                    if (!dr.IsDBNull(iRCDEULH24)) entity.RCDEULH24 = dr.GetDecimal(iRCDEULH24);

                    int iRCDEULH25 = dr.GetOrdinal(helper.RCDEULH25);
                    if (!dr.IsDBNull(iRCDEULH25)) entity.RCDEULH25 = dr.GetDecimal(iRCDEULH25);

                    int iRCDEULH26 = dr.GetOrdinal(helper.RCDEULH26);
                    if (!dr.IsDBNull(iRCDEULH26)) entity.RCDEULH26 = dr.GetDecimal(iRCDEULH26);

                    int iRCDEULH27 = dr.GetOrdinal(helper.RCDEULH27);
                    if (!dr.IsDBNull(iRCDEULH27)) entity.RCDEULH27 = dr.GetDecimal(iRCDEULH27);

                    int iRCDEULH28 = dr.GetOrdinal(helper.RCDEULH28);
                    if (!dr.IsDBNull(iRCDEULH28)) entity.RCDEULH28 = dr.GetDecimal(iRCDEULH28);

                    int iRCDEULH29 = dr.GetOrdinal(helper.RCDEULH29);
                    if (!dr.IsDBNull(iRCDEULH29)) entity.RCDEULH29 = dr.GetDecimal(iRCDEULH29);

                    int iRCDEULH30 = dr.GetOrdinal(helper.RCDEULH30);
                    if (!dr.IsDBNull(iRCDEULH30)) entity.RCDEULH30 = dr.GetDecimal(iRCDEULH30);

                    int iRCDEULH31 = dr.GetOrdinal(helper.RCDEULH31);
                    if (!dr.IsDBNull(iRCDEULH31)) entity.RCDEULH31 = dr.GetDecimal(iRCDEULH31);

                    int iRCDEULH32 = dr.GetOrdinal(helper.RCDEULH32);
                    if (!dr.IsDBNull(iRCDEULH32)) entity.RCDEULH32 = dr.GetDecimal(iRCDEULH32);

                    int iRCDEULH33 = dr.GetOrdinal(helper.RCDEULH33);
                    if (!dr.IsDBNull(iRCDEULH33)) entity.RCDEULH33 = dr.GetDecimal(iRCDEULH33);

                    int iRCDEULH34 = dr.GetOrdinal(helper.RCDEULH34);
                    if (!dr.IsDBNull(iRCDEULH34)) entity.RCDEULH34 = dr.GetDecimal(iRCDEULH34);

                    int iRCDEULH35 = dr.GetOrdinal(helper.RCDEULH35);
                    if (!dr.IsDBNull(iRCDEULH35)) entity.RCDEULH35 = dr.GetDecimal(iRCDEULH35);

                    int iRCDEULH36 = dr.GetOrdinal(helper.RCDEULH36);
                    if (!dr.IsDBNull(iRCDEULH36)) entity.RCDEULH36 = dr.GetDecimal(iRCDEULH36);

                    int iRCDEULH37 = dr.GetOrdinal(helper.RCDEULH37);
                    if (!dr.IsDBNull(iRCDEULH37)) entity.RCDEULH37 = dr.GetDecimal(iRCDEULH37);

                    int iRCDEULH38 = dr.GetOrdinal(helper.RCDEULH38);
                    if (!dr.IsDBNull(iRCDEULH38)) entity.RCDEULH38 = dr.GetDecimal(iRCDEULH38);

                    int iRCDEULH39 = dr.GetOrdinal(helper.RCDEULH39);
                    if (!dr.IsDBNull(iRCDEULH39)) entity.RCDEULH39 = dr.GetDecimal(iRCDEULH39);

                    int iRCDEULH40 = dr.GetOrdinal(helper.RCDEULH40);
                    if (!dr.IsDBNull(iRCDEULH40)) entity.RCDEULH40 = dr.GetDecimal(iRCDEULH40);

                    int iRCDEULH41 = dr.GetOrdinal(helper.RCDEULH41);
                    if (!dr.IsDBNull(iRCDEULH41)) entity.RCDEULH41 = dr.GetDecimal(iRCDEULH41);

                    int iRCDEULH42 = dr.GetOrdinal(helper.RCDEULH42);
                    if (!dr.IsDBNull(iRCDEULH42)) entity.RCDEULH42 = dr.GetDecimal(iRCDEULH42);

                    int iRCDEULH43 = dr.GetOrdinal(helper.RCDEULH43);
                    if (!dr.IsDBNull(iRCDEULH43)) entity.RCDEULH43 = dr.GetDecimal(iRCDEULH43);

                    int iRCDEULH44 = dr.GetOrdinal(helper.RCDEULH44);
                    if (!dr.IsDBNull(iRCDEULH44)) entity.RCDEULH44 = dr.GetDecimal(iRCDEULH44);

                    int iRCDEULH45 = dr.GetOrdinal(helper.RCDEULH45);
                    if (!dr.IsDBNull(iRCDEULH45)) entity.RCDEULH45 = dr.GetDecimal(iRCDEULH45);

                    int iRCDEULH46 = dr.GetOrdinal(helper.RCDEULH46);
                    if (!dr.IsDBNull(iRCDEULH46)) entity.RCDEULH46 = dr.GetDecimal(iRCDEULH46);

                    int iRCDEULH47 = dr.GetOrdinal(helper.RCDEULH47);
                    if (!dr.IsDBNull(iRCDEULH47)) entity.RCDEULH47 = dr.GetDecimal(iRCDEULH47);

                    int iRCDEULH48 = dr.GetOrdinal(helper.RCDEULH48);
                    if (!dr.IsDBNull(iRCDEULH48)) entity.RCDEULH48 = dr.GetDecimal(iRCDEULH48);

                    int iRCDEULH49 = dr.GetOrdinal(helper.RCDEULH49);
                    if (!dr.IsDBNull(iRCDEULH49)) entity.RCDEULH49 = dr.GetDecimal(iRCDEULH49);

                    int iRCDEULH50 = dr.GetOrdinal(helper.RCDEULH50);
                    if (!dr.IsDBNull(iRCDEULH50)) entity.RCDEULH50 = dr.GetDecimal(iRCDEULH50);

                    int iRCDEULH51 = dr.GetOrdinal(helper.RCDEULH51);
                    if (!dr.IsDBNull(iRCDEULH51)) entity.RCDEULH51 = dr.GetDecimal(iRCDEULH51);

                    int iRCDEULH52 = dr.GetOrdinal(helper.RCDEULH52);
                    if (!dr.IsDBNull(iRCDEULH52)) entity.RCDEULH52 = dr.GetDecimal(iRCDEULH52);

                    int iRCDEULH53 = dr.GetOrdinal(helper.RCDEULH53);
                    if (!dr.IsDBNull(iRCDEULH53)) entity.RCDEULH53 = dr.GetDecimal(iRCDEULH53);

                    int iRCDEULH54 = dr.GetOrdinal(helper.RCDEULH54);
                    if (!dr.IsDBNull(iRCDEULH54)) entity.RCDEULH54 = dr.GetDecimal(iRCDEULH54);

                    int iRCDEULH55 = dr.GetOrdinal(helper.RCDEULH55);
                    if (!dr.IsDBNull(iRCDEULH55)) entity.RCDEULH55 = dr.GetDecimal(iRCDEULH55);

                    int iRCDEULH56 = dr.GetOrdinal(helper.RCDEULH56);
                    if (!dr.IsDBNull(iRCDEULH56)) entity.RCDEULH56 = dr.GetDecimal(iRCDEULH56);

                    int iRCDEULH57 = dr.GetOrdinal(helper.RCDEULH57);
                    if (!dr.IsDBNull(iRCDEULH57)) entity.RCDEULH57 = dr.GetDecimal(iRCDEULH57);

                    int iRCDEULH58 = dr.GetOrdinal(helper.RCDEULH58);
                    if (!dr.IsDBNull(iRCDEULH58)) entity.RCDEULH58 = dr.GetDecimal(iRCDEULH58);

                    int iRCDEULH59 = dr.GetOrdinal(helper.RCDEULH59);
                    if (!dr.IsDBNull(iRCDEULH59)) entity.RCDEULH59 = dr.GetDecimal(iRCDEULH59);

                    int iRCDEULH60 = dr.GetOrdinal(helper.RCDEULH60);
                    if (!dr.IsDBNull(iRCDEULH60)) entity.RCDEULH60 = dr.GetDecimal(iRCDEULH60);

                    int iRCDEULH61 = dr.GetOrdinal(helper.RCDEULH61);
                    if (!dr.IsDBNull(iRCDEULH61)) entity.RCDEULH61 = dr.GetDecimal(iRCDEULH61);

                    int iRCDEULH62 = dr.GetOrdinal(helper.RCDEULH62);
                    if (!dr.IsDBNull(iRCDEULH62)) entity.RCDEULH62 = dr.GetDecimal(iRCDEULH62);

                    int iRCDEULH63 = dr.GetOrdinal(helper.RCDEULH63);
                    if (!dr.IsDBNull(iRCDEULH63)) entity.RCDEULH63 = dr.GetDecimal(iRCDEULH63);

                    int iRCDEULH64 = dr.GetOrdinal(helper.RCDEULH64);
                    if (!dr.IsDBNull(iRCDEULH64)) entity.RCDEULH64 = dr.GetDecimal(iRCDEULH64);

                    int iRCDEULH65 = dr.GetOrdinal(helper.RCDEULH65);
                    if (!dr.IsDBNull(iRCDEULH65)) entity.RCDEULH65 = dr.GetDecimal(iRCDEULH65);

                    int iRCDEULH66 = dr.GetOrdinal(helper.RCDEULH66);
                    if (!dr.IsDBNull(iRCDEULH66)) entity.RCDEULH66 = dr.GetDecimal(iRCDEULH66);

                    int iRCDEULH67 = dr.GetOrdinal(helper.RCDEULH67);
                    if (!dr.IsDBNull(iRCDEULH67)) entity.RCDEULH67 = dr.GetDecimal(iRCDEULH67);

                    int iRCDEULH68 = dr.GetOrdinal(helper.RCDEULH68);
                    if (!dr.IsDBNull(iRCDEULH68)) entity.RCDEULH68 = dr.GetDecimal(iRCDEULH68);

                    int iRCDEULH69 = dr.GetOrdinal(helper.RCDEULH69);
                    if (!dr.IsDBNull(iRCDEULH69)) entity.RCDEULH69 = dr.GetDecimal(iRCDEULH69);

                    int iRCDEULH70 = dr.GetOrdinal(helper.RCDEULH70);
                    if (!dr.IsDBNull(iRCDEULH70)) entity.RCDEULH70 = dr.GetDecimal(iRCDEULH70);

                    int iRCDEULH71 = dr.GetOrdinal(helper.RCDEULH71);
                    if (!dr.IsDBNull(iRCDEULH71)) entity.RCDEULH71 = dr.GetDecimal(iRCDEULH71);

                    int iRCDEULH72 = dr.GetOrdinal(helper.RCDEULH72);
                    if (!dr.IsDBNull(iRCDEULH72)) entity.RCDEULH72 = dr.GetDecimal(iRCDEULH72);

                    int iRCDEULH73 = dr.GetOrdinal(helper.RCDEULH73);
                    if (!dr.IsDBNull(iRCDEULH73)) entity.RCDEULH73 = dr.GetDecimal(iRCDEULH73);

                    int iRCDEULH74 = dr.GetOrdinal(helper.RCDEULH74);
                    if (!dr.IsDBNull(iRCDEULH74)) entity.RCDEULH74 = dr.GetDecimal(iRCDEULH74);

                    int iRCDEULH75 = dr.GetOrdinal(helper.RCDEULH75);
                    if (!dr.IsDBNull(iRCDEULH75)) entity.RCDEULH75 = dr.GetDecimal(iRCDEULH75);

                    int iRCDEULH76 = dr.GetOrdinal(helper.RCDEULH76);
                    if (!dr.IsDBNull(iRCDEULH76)) entity.RCDEULH76 = dr.GetDecimal(iRCDEULH76);

                    int iRCDEULH77 = dr.GetOrdinal(helper.RCDEULH77);
                    if (!dr.IsDBNull(iRCDEULH77)) entity.RCDEULH77 = dr.GetDecimal(iRCDEULH77);

                    int iRCDEULH78 = dr.GetOrdinal(helper.RCDEULH78);
                    if (!dr.IsDBNull(iRCDEULH78)) entity.RCDEULH78 = dr.GetDecimal(iRCDEULH78);

                    int iRCDEULH79 = dr.GetOrdinal(helper.RCDEULH79);
                    if (!dr.IsDBNull(iRCDEULH79)) entity.RCDEULH79 = dr.GetDecimal(iRCDEULH79);

                    int iRCDEULH80 = dr.GetOrdinal(helper.RCDEULH80);
                    if (!dr.IsDBNull(iRCDEULH80)) entity.RCDEULH80 = dr.GetDecimal(iRCDEULH80);

                    int iRCDEULH81 = dr.GetOrdinal(helper.RCDEULH81);
                    if (!dr.IsDBNull(iRCDEULH81)) entity.RCDEULH81 = dr.GetDecimal(iRCDEULH81);

                    int iRCDEULH82 = dr.GetOrdinal(helper.RCDEULH82);
                    if (!dr.IsDBNull(iRCDEULH82)) entity.RCDEULH82 = dr.GetDecimal(iRCDEULH82);

                    int iRCDEULH83 = dr.GetOrdinal(helper.RCDEULH83);
                    if (!dr.IsDBNull(iRCDEULH83)) entity.RCDEULH83 = dr.GetDecimal(iRCDEULH83);

                    int iRCDEULH84 = dr.GetOrdinal(helper.RCDEULH84);
                    if (!dr.IsDBNull(iRCDEULH84)) entity.RCDEULH84 = dr.GetDecimal(iRCDEULH84);

                    int iRCDEULH85 = dr.GetOrdinal(helper.RCDEULH85);
                    if (!dr.IsDBNull(iRCDEULH85)) entity.RCDEULH85 = dr.GetDecimal(iRCDEULH85);

                    int iRCDEULH86 = dr.GetOrdinal(helper.RCDEULH86);
                    if (!dr.IsDBNull(iRCDEULH86)) entity.RCDEULH86 = dr.GetDecimal(iRCDEULH86);

                    int iRCDEULH87 = dr.GetOrdinal(helper.RCDEULH87);
                    if (!dr.IsDBNull(iRCDEULH87)) entity.RCDEULH87 = dr.GetDecimal(iRCDEULH87);

                    int iRCDEULH88 = dr.GetOrdinal(helper.RCDEULH88);
                    if (!dr.IsDBNull(iRCDEULH88)) entity.RCDEULH88 = dr.GetDecimal(iRCDEULH88);

                    int iRCDEULH89 = dr.GetOrdinal(helper.RCDEULH89);
                    if (!dr.IsDBNull(iRCDEULH89)) entity.RCDEULH89 = dr.GetDecimal(iRCDEULH89);

                    int iRCDEULH90 = dr.GetOrdinal(helper.RCDEULH90);
                    if (!dr.IsDBNull(iRCDEULH90)) entity.RCDEULH90 = dr.GetDecimal(iRCDEULH90);

                    int iRCDEULH91 = dr.GetOrdinal(helper.RCDEULH91);
                    if (!dr.IsDBNull(iRCDEULH91)) entity.RCDEULH91 = dr.GetDecimal(iRCDEULH91);

                    int iRCDEULH92 = dr.GetOrdinal(helper.RCDEULH92);
                    if (!dr.IsDBNull(iRCDEULH92)) entity.RCDEULH92 = dr.GetDecimal(iRCDEULH92);

                    int iRCDEULH93 = dr.GetOrdinal(helper.RCDEULH93);
                    if (!dr.IsDBNull(iRCDEULH93)) entity.RCDEULH93 = dr.GetDecimal(iRCDEULH93);

                    int iRCDEULH94 = dr.GetOrdinal(helper.RCDEULH94);
                    if (!dr.IsDBNull(iRCDEULH94)) entity.RCDEULH94 = dr.GetDecimal(iRCDEULH94);

                    int iRCDEULH95 = dr.GetOrdinal(helper.RCDEULH95);
                    if (!dr.IsDBNull(iRCDEULH95)) entity.RCDEULH95 = dr.GetDecimal(iRCDEULH95);

                    int iRCDEULH96 = dr.GetOrdinal(helper.RCDEULH96);
                    if (!dr.IsDBNull(iRCDEULH96)) entity.RCDEULH96 = dr.GetDecimal(iRCDEULH96);                    

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<MeEnvioDTO> ObtenerListaPeriodoReporte(string fecha)
        {
            string sqlQuery = string.Format(this.helper.SqlObtenerListaPeriodoReporte, fecha);
            List<MeEnvioDTO> entitys = new List<MeEnvioDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeEnvioDTO entity = new MeEnvioDTO();

                    int iIniRemision = dr.GetOrdinal(helper.IniRemision);
                    if (!dr.IsDBNull(iIniRemision)) entity.IniRemision = dr.GetDateTime(iIniRemision);

                    int iPeriodo = dr.GetOrdinal(helper.Periodo);
                    if (!dr.IsDBNull(iPeriodo)) entity.Periodo = dr.GetString(iPeriodo);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int ListDemandaUsuarioReporteCount(string periodo, string codigoZona, string codigoPuntoMedicion, string empresa, string suministrador)
        {            
            string condicion = " WHERE CP.RCDEULPERIODO = '" + periodo + "'";

            if (!string.IsNullOrEmpty(codigoPuntoMedicion))
            {
                condicion = condicion + " AND EQ.AREACODI IN (" + codigoPuntoMedicion + ") ";
            }
            if (!string.IsNullOrEmpty(suministrador))
            {
                condicion = condicion + " AND CP.EMPRCODI IN (" + suministrador + ") ";
            }
            if (!string.IsNullOrEmpty(empresa))
            {
                condicion = condicion + string.Format(" AND NVL(CL.EMPRRAZSOCIAL,CL.EMPRABREV) LIKE '%{0}%' ", empresa.ToUpper());
            } 
            
            string sqlQuery = string.Format(this.helper.SqlObtenerReporteDemandaUsuarioCount, condicion);
            
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

        public List<EqEquipoDTO> ObtenerEquipos()
        {
            string sqlQuery = this.helper.SqlObtenerEquipos;
            List<EqEquipoDTO> entitys = new List<EqEquipoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqEquipoDTO entity = new EqEquipoDTO();

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = dr.GetInt32(iEmprcodi);

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = dr.GetInt32(iEquicodi);

                    int iOsinergcodi = dr.GetOrdinal(helper.Osinergcodi);
                    if (!dr.IsDBNull(iOsinergcodi)) entity.Osinergcodi = dr.GetString(iOsinergcodi);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<RcaDemandaUsuarioDTO> ListDemandaUsuarioErroresPag(string periodo,  int regIni, int regFin)
        {
            //string condicion = " WHERE CP.RCDEULPERIODO = '" + periodo + "'";
                       
            string sqlQuery = string.Format(this.helper.SqObtenerDemandaUsuarioErroresPag, periodo, regFin, regIni);
            List<RcaDemandaUsuarioDTO> entitys = new List<RcaDemandaUsuarioDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RcaDemandaUsuarioDTO entity = new RcaDemandaUsuarioDTO();

                    int iItem = dr.GetOrdinal(helper.Item);
                    if (!dr.IsDBNull(iItem)) entity.Item = Convert.ToInt32(dr.GetValue(iItem));

                    int iRcdeulperiodo = dr.GetOrdinal(helper.Rcdeulperiodo);
                    if (!dr.IsDBNull(iRcdeulperiodo)) entity.Rcdeulperiodo = dr.GetString(iRcdeulperiodo);

                    //int iRcdeulfecmaxdem = dr.GetOrdinal(helper.Rcdeulfecmaxdem);
                    //if (!dr.IsDBNull(iRcdeulfecmaxdem)) entity.Rcdeulfecmaxdem = dr.GetDateTime(iRcdeulfecmaxdem);

                    //int iEmprCodi = dr.GetOrdinal(helper.Emprcodi);
                    //if (!dr.IsDBNull(iEmprCodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprCodi));

                    //int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    //if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iOsinergcodi = dr.GetOrdinal(helper.Osinergcodi);
                    if (!dr.IsDBNull(iOsinergcodi)) entity.Osinergcodi = dr.GetString(iOsinergcodi);

                    int iSuministrador = dr.GetOrdinal(helper.Suministrador);
                    if (!dr.IsDBNull(iSuministrador)) entity.Suministrador = dr.GetString(iSuministrador);

                    int iRuc = dr.GetOrdinal(helper.Ruc);
                    if (!dr.IsDBNull(iRuc)) entity.Ruc = dr.GetString(iRuc);

                    int iEmprrazsocial = dr.GetOrdinal(helper.Emprrazsocial);
                    if (!dr.IsDBNull(iEmprrazsocial)) entity.Emprrazsocial = dr.GetString(iEmprrazsocial);

                    int iSubestacion = dr.GetOrdinal(helper.Subestacion);
                    if (!dr.IsDBNull(iSubestacion)) entity.Subestacion = dr.GetString(iSubestacion);

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    //int iRcdeulfuente = dr.GetOrdinal(helper.Rcdeulfuente);
                    //if (!dr.IsDBNull(iRcdeulfuente)) entity.Rcdeulfuente = dr.GetString(iRcdeulfuente);

                    int iRcdeuldemandahp = dr.GetOrdinal(helper.Rcdeuldemandahp);
                    if (!dr.IsDBNull(iRcdeuldemandahp)) entity.Rcdeuldemandahp = dr.GetDecimal(iRcdeuldemandahp);

                    int iRcdeuldemandahfp = dr.GetOrdinal(helper.Rcdeuldemandahfp);
                    if (!dr.IsDBNull(iRcdeuldemandahfp)) entity.Rcdeuldemandahfp = dr.GetDecimal(iRcdeuldemandahfp);

                    int iRCDEULH1 = dr.GetOrdinal(helper.RCDEULH1);
                    if (!dr.IsDBNull(iRCDEULH1)) entity.RCDEULH1 = dr.GetDecimal(iRCDEULH1);

                    int iRCDEULH2 = dr.GetOrdinal(helper.RCDEULH2);
                    if (!dr.IsDBNull(iRCDEULH2)) entity.RCDEULH2 = dr.GetDecimal(iRCDEULH2);

                    int iRCDEULH3 = dr.GetOrdinal(helper.RCDEULH3);
                    if (!dr.IsDBNull(iRCDEULH3)) entity.RCDEULH3 = dr.GetDecimal(iRCDEULH3);

                    int iRCDEULH4 = dr.GetOrdinal(helper.RCDEULH4);
                    if (!dr.IsDBNull(iRCDEULH4)) entity.RCDEULH4 = dr.GetDecimal(iRCDEULH4);

                    int iRCDEULH5 = dr.GetOrdinal(helper.RCDEULH5);
                    if (!dr.IsDBNull(iRCDEULH5)) entity.RCDEULH5 = dr.GetDecimal(iRCDEULH5);

                    int iRCDEULH6 = dr.GetOrdinal(helper.RCDEULH6);
                    if (!dr.IsDBNull(iRCDEULH6)) entity.RCDEULH6 = dr.GetDecimal(iRCDEULH6);

                    int iRCDEULH7 = dr.GetOrdinal(helper.RCDEULH7);
                    if (!dr.IsDBNull(iRCDEULH7)) entity.RCDEULH7 = dr.GetDecimal(iRCDEULH7);

                    int iRCDEULH8 = dr.GetOrdinal(helper.RCDEULH8);
                    if (!dr.IsDBNull(iRCDEULH8)) entity.RCDEULH8 = dr.GetDecimal(iRCDEULH8);

                    int iRCDEULH9 = dr.GetOrdinal(helper.RCDEULH9);
                    if (!dr.IsDBNull(iRCDEULH9)) entity.RCDEULH9 = dr.GetDecimal(iRCDEULH9);

                    int iRCDEULH10 = dr.GetOrdinal(helper.RCDEULH10);
                    if (!dr.IsDBNull(iRCDEULH10)) entity.RCDEULH10 = dr.GetDecimal(iRCDEULH10);

                    int iRCDEULH11 = dr.GetOrdinal(helper.RCDEULH11);
                    if (!dr.IsDBNull(iRCDEULH11)) entity.RCDEULH11 = dr.GetDecimal(iRCDEULH11);

                    int iRCDEULH12 = dr.GetOrdinal(helper.RCDEULH12);
                    if (!dr.IsDBNull(iRCDEULH12)) entity.RCDEULH12 = dr.GetDecimal(iRCDEULH12);

                    int iRCDEULH13 = dr.GetOrdinal(helper.RCDEULH13);
                    if (!dr.IsDBNull(iRCDEULH13)) entity.RCDEULH13 = dr.GetDecimal(iRCDEULH13);

                    int iRCDEULH14 = dr.GetOrdinal(helper.RCDEULH14);
                    if (!dr.IsDBNull(iRCDEULH14)) entity.RCDEULH14 = dr.GetDecimal(iRCDEULH14);

                    int iRCDEULH15 = dr.GetOrdinal(helper.RCDEULH15);
                    if (!dr.IsDBNull(iRCDEULH15)) entity.RCDEULH15 = dr.GetDecimal(iRCDEULH15);

                    int iRCDEULH16 = dr.GetOrdinal(helper.RCDEULH16);
                    if (!dr.IsDBNull(iRCDEULH16)) entity.RCDEULH16 = dr.GetDecimal(iRCDEULH16);

                    int iRCDEULH17 = dr.GetOrdinal(helper.RCDEULH17);
                    if (!dr.IsDBNull(iRCDEULH17)) entity.RCDEULH17 = dr.GetDecimal(iRCDEULH17);

                    int iRCDEULH18 = dr.GetOrdinal(helper.RCDEULH18);
                    if (!dr.IsDBNull(iRCDEULH18)) entity.RCDEULH18 = dr.GetDecimal(iRCDEULH18);

                    int iRCDEULH19 = dr.GetOrdinal(helper.RCDEULH19);
                    if (!dr.IsDBNull(iRCDEULH19)) entity.RCDEULH19 = dr.GetDecimal(iRCDEULH19);

                    int iRCDEULH20 = dr.GetOrdinal(helper.RCDEULH20);
                    if (!dr.IsDBNull(iRCDEULH20)) entity.RCDEULH20 = dr.GetDecimal(iRCDEULH20);

                    int iRCDEULH21 = dr.GetOrdinal(helper.RCDEULH21);
                    if (!dr.IsDBNull(iRCDEULH21)) entity.RCDEULH21 = dr.GetDecimal(iRCDEULH21);

                    int iRCDEULH22 = dr.GetOrdinal(helper.RCDEULH22);
                    if (!dr.IsDBNull(iRCDEULH22)) entity.RCDEULH22 = dr.GetDecimal(iRCDEULH22);

                    int iRCDEULH23 = dr.GetOrdinal(helper.RCDEULH23);
                    if (!dr.IsDBNull(iRCDEULH23)) entity.RCDEULH23 = dr.GetDecimal(iRCDEULH23);

                    int iRCDEULH24 = dr.GetOrdinal(helper.RCDEULH24);
                    if (!dr.IsDBNull(iRCDEULH24)) entity.RCDEULH24 = dr.GetDecimal(iRCDEULH24);

                    int iRCDEULH25 = dr.GetOrdinal(helper.RCDEULH25);
                    if (!dr.IsDBNull(iRCDEULH25)) entity.RCDEULH25 = dr.GetDecimal(iRCDEULH25);

                    int iRCDEULH26 = dr.GetOrdinal(helper.RCDEULH26);
                    if (!dr.IsDBNull(iRCDEULH26)) entity.RCDEULH26 = dr.GetDecimal(iRCDEULH26);

                    int iRCDEULH27 = dr.GetOrdinal(helper.RCDEULH27);
                    if (!dr.IsDBNull(iRCDEULH27)) entity.RCDEULH27 = dr.GetDecimal(iRCDEULH27);

                    int iRCDEULH28 = dr.GetOrdinal(helper.RCDEULH28);
                    if (!dr.IsDBNull(iRCDEULH28)) entity.RCDEULH28 = dr.GetDecimal(iRCDEULH28);

                    int iRCDEULH29 = dr.GetOrdinal(helper.RCDEULH29);
                    if (!dr.IsDBNull(iRCDEULH29)) entity.RCDEULH29 = dr.GetDecimal(iRCDEULH29);

                    int iRCDEULH30 = dr.GetOrdinal(helper.RCDEULH30);
                    if (!dr.IsDBNull(iRCDEULH30)) entity.RCDEULH30 = dr.GetDecimal(iRCDEULH30);

                    int iRCDEULH31 = dr.GetOrdinal(helper.RCDEULH31);
                    if (!dr.IsDBNull(iRCDEULH31)) entity.RCDEULH31 = dr.GetDecimal(iRCDEULH31);

                    int iRCDEULH32 = dr.GetOrdinal(helper.RCDEULH32);
                    if (!dr.IsDBNull(iRCDEULH32)) entity.RCDEULH32 = dr.GetDecimal(iRCDEULH32);

                    int iRCDEULH33 = dr.GetOrdinal(helper.RCDEULH33);
                    if (!dr.IsDBNull(iRCDEULH33)) entity.RCDEULH33 = dr.GetDecimal(iRCDEULH33);

                    int iRCDEULH34 = dr.GetOrdinal(helper.RCDEULH34);
                    if (!dr.IsDBNull(iRCDEULH34)) entity.RCDEULH34 = dr.GetDecimal(iRCDEULH34);

                    int iRCDEULH35 = dr.GetOrdinal(helper.RCDEULH35);
                    if (!dr.IsDBNull(iRCDEULH35)) entity.RCDEULH35 = dr.GetDecimal(iRCDEULH35);

                    int iRCDEULH36 = dr.GetOrdinal(helper.RCDEULH36);
                    if (!dr.IsDBNull(iRCDEULH36)) entity.RCDEULH36 = dr.GetDecimal(iRCDEULH36);

                    int iRCDEULH37 = dr.GetOrdinal(helper.RCDEULH37);
                    if (!dr.IsDBNull(iRCDEULH37)) entity.RCDEULH37 = dr.GetDecimal(iRCDEULH37);

                    int iRCDEULH38 = dr.GetOrdinal(helper.RCDEULH38);
                    if (!dr.IsDBNull(iRCDEULH38)) entity.RCDEULH38 = dr.GetDecimal(iRCDEULH38);

                    int iRCDEULH39 = dr.GetOrdinal(helper.RCDEULH39);
                    if (!dr.IsDBNull(iRCDEULH39)) entity.RCDEULH39 = dr.GetDecimal(iRCDEULH39);

                    int iRCDEULH40 = dr.GetOrdinal(helper.RCDEULH40);
                    if (!dr.IsDBNull(iRCDEULH40)) entity.RCDEULH40 = dr.GetDecimal(iRCDEULH40);

                    int iRCDEULH41 = dr.GetOrdinal(helper.RCDEULH41);
                    if (!dr.IsDBNull(iRCDEULH41)) entity.RCDEULH41 = dr.GetDecimal(iRCDEULH41);

                    int iRCDEULH42 = dr.GetOrdinal(helper.RCDEULH42);
                    if (!dr.IsDBNull(iRCDEULH42)) entity.RCDEULH42 = dr.GetDecimal(iRCDEULH42);

                    int iRCDEULH43 = dr.GetOrdinal(helper.RCDEULH43);
                    if (!dr.IsDBNull(iRCDEULH43)) entity.RCDEULH43 = dr.GetDecimal(iRCDEULH43);

                    int iRCDEULH44 = dr.GetOrdinal(helper.RCDEULH44);
                    if (!dr.IsDBNull(iRCDEULH44)) entity.RCDEULH44 = dr.GetDecimal(iRCDEULH44);

                    int iRCDEULH45 = dr.GetOrdinal(helper.RCDEULH45);
                    if (!dr.IsDBNull(iRCDEULH45)) entity.RCDEULH45 = dr.GetDecimal(iRCDEULH45);

                    int iRCDEULH46 = dr.GetOrdinal(helper.RCDEULH46);
                    if (!dr.IsDBNull(iRCDEULH46)) entity.RCDEULH46 = dr.GetDecimal(iRCDEULH46);

                    int iRCDEULH47 = dr.GetOrdinal(helper.RCDEULH47);
                    if (!dr.IsDBNull(iRCDEULH47)) entity.RCDEULH47 = dr.GetDecimal(iRCDEULH47);

                    int iRCDEULH48 = dr.GetOrdinal(helper.RCDEULH48);
                    if (!dr.IsDBNull(iRCDEULH48)) entity.RCDEULH48 = dr.GetDecimal(iRCDEULH48);

                    int iRCDEULH49 = dr.GetOrdinal(helper.RCDEULH49);
                    if (!dr.IsDBNull(iRCDEULH49)) entity.RCDEULH49 = dr.GetDecimal(iRCDEULH49);

                    int iRCDEULH50 = dr.GetOrdinal(helper.RCDEULH50);
                    if (!dr.IsDBNull(iRCDEULH50)) entity.RCDEULH50 = dr.GetDecimal(iRCDEULH50);

                    int iRCDEULH51 = dr.GetOrdinal(helper.RCDEULH51);
                    if (!dr.IsDBNull(iRCDEULH51)) entity.RCDEULH51 = dr.GetDecimal(iRCDEULH51);

                    int iRCDEULH52 = dr.GetOrdinal(helper.RCDEULH52);
                    if (!dr.IsDBNull(iRCDEULH52)) entity.RCDEULH52 = dr.GetDecimal(iRCDEULH52);

                    int iRCDEULH53 = dr.GetOrdinal(helper.RCDEULH53);
                    if (!dr.IsDBNull(iRCDEULH53)) entity.RCDEULH53 = dr.GetDecimal(iRCDEULH53);

                    int iRCDEULH54 = dr.GetOrdinal(helper.RCDEULH54);
                    if (!dr.IsDBNull(iRCDEULH54)) entity.RCDEULH54 = dr.GetDecimal(iRCDEULH54);

                    int iRCDEULH55 = dr.GetOrdinal(helper.RCDEULH55);
                    if (!dr.IsDBNull(iRCDEULH55)) entity.RCDEULH55 = dr.GetDecimal(iRCDEULH55);

                    int iRCDEULH56 = dr.GetOrdinal(helper.RCDEULH56);
                    if (!dr.IsDBNull(iRCDEULH56)) entity.RCDEULH56 = dr.GetDecimal(iRCDEULH56);

                    int iRCDEULH57 = dr.GetOrdinal(helper.RCDEULH57);
                    if (!dr.IsDBNull(iRCDEULH57)) entity.RCDEULH57 = dr.GetDecimal(iRCDEULH57);

                    int iRCDEULH58 = dr.GetOrdinal(helper.RCDEULH58);
                    if (!dr.IsDBNull(iRCDEULH58)) entity.RCDEULH58 = dr.GetDecimal(iRCDEULH58);

                    int iRCDEULH59 = dr.GetOrdinal(helper.RCDEULH59);
                    if (!dr.IsDBNull(iRCDEULH59)) entity.RCDEULH59 = dr.GetDecimal(iRCDEULH59);

                    int iRCDEULH60 = dr.GetOrdinal(helper.RCDEULH60);
                    if (!dr.IsDBNull(iRCDEULH60)) entity.RCDEULH60 = dr.GetDecimal(iRCDEULH60);

                    int iRCDEULH61 = dr.GetOrdinal(helper.RCDEULH61);
                    if (!dr.IsDBNull(iRCDEULH61)) entity.RCDEULH61 = dr.GetDecimal(iRCDEULH61);

                    int iRCDEULH62 = dr.GetOrdinal(helper.RCDEULH62);
                    if (!dr.IsDBNull(iRCDEULH62)) entity.RCDEULH62 = dr.GetDecimal(iRCDEULH62);

                    int iRCDEULH63 = dr.GetOrdinal(helper.RCDEULH63);
                    if (!dr.IsDBNull(iRCDEULH63)) entity.RCDEULH63 = dr.GetDecimal(iRCDEULH63);

                    int iRCDEULH64 = dr.GetOrdinal(helper.RCDEULH64);
                    if (!dr.IsDBNull(iRCDEULH64)) entity.RCDEULH64 = dr.GetDecimal(iRCDEULH64);

                    int iRCDEULH65 = dr.GetOrdinal(helper.RCDEULH65);
                    if (!dr.IsDBNull(iRCDEULH65)) entity.RCDEULH65 = dr.GetDecimal(iRCDEULH65);

                    int iRCDEULH66 = dr.GetOrdinal(helper.RCDEULH66);
                    if (!dr.IsDBNull(iRCDEULH66)) entity.RCDEULH66 = dr.GetDecimal(iRCDEULH66);

                    int iRCDEULH67 = dr.GetOrdinal(helper.RCDEULH67);
                    if (!dr.IsDBNull(iRCDEULH67)) entity.RCDEULH67 = dr.GetDecimal(iRCDEULH67);

                    int iRCDEULH68 = dr.GetOrdinal(helper.RCDEULH68);
                    if (!dr.IsDBNull(iRCDEULH68)) entity.RCDEULH68 = dr.GetDecimal(iRCDEULH68);

                    int iRCDEULH69 = dr.GetOrdinal(helper.RCDEULH69);
                    if (!dr.IsDBNull(iRCDEULH69)) entity.RCDEULH69 = dr.GetDecimal(iRCDEULH69);

                    int iRCDEULH70 = dr.GetOrdinal(helper.RCDEULH70);
                    if (!dr.IsDBNull(iRCDEULH70)) entity.RCDEULH70 = dr.GetDecimal(iRCDEULH70);

                    int iRCDEULH71 = dr.GetOrdinal(helper.RCDEULH71);
                    if (!dr.IsDBNull(iRCDEULH71)) entity.RCDEULH71 = dr.GetDecimal(iRCDEULH71);

                    int iRCDEULH72 = dr.GetOrdinal(helper.RCDEULH72);
                    if (!dr.IsDBNull(iRCDEULH72)) entity.RCDEULH72 = dr.GetDecimal(iRCDEULH72);

                    int iRCDEULH73 = dr.GetOrdinal(helper.RCDEULH73);
                    if (!dr.IsDBNull(iRCDEULH73)) entity.RCDEULH73 = dr.GetDecimal(iRCDEULH73);

                    int iRCDEULH74 = dr.GetOrdinal(helper.RCDEULH74);
                    if (!dr.IsDBNull(iRCDEULH74)) entity.RCDEULH74 = dr.GetDecimal(iRCDEULH74);

                    int iRCDEULH75 = dr.GetOrdinal(helper.RCDEULH75);
                    if (!dr.IsDBNull(iRCDEULH75)) entity.RCDEULH75 = dr.GetDecimal(iRCDEULH75);

                    int iRCDEULH76 = dr.GetOrdinal(helper.RCDEULH76);
                    if (!dr.IsDBNull(iRCDEULH76)) entity.RCDEULH76 = dr.GetDecimal(iRCDEULH76);

                    int iRCDEULH77 = dr.GetOrdinal(helper.RCDEULH77);
                    if (!dr.IsDBNull(iRCDEULH77)) entity.RCDEULH77 = dr.GetDecimal(iRCDEULH77);

                    int iRCDEULH78 = dr.GetOrdinal(helper.RCDEULH78);
                    if (!dr.IsDBNull(iRCDEULH78)) entity.RCDEULH78 = dr.GetDecimal(iRCDEULH78);

                    int iRCDEULH79 = dr.GetOrdinal(helper.RCDEULH79);
                    if (!dr.IsDBNull(iRCDEULH79)) entity.RCDEULH79 = dr.GetDecimal(iRCDEULH79);

                    int iRCDEULH80 = dr.GetOrdinal(helper.RCDEULH80);
                    if (!dr.IsDBNull(iRCDEULH80)) entity.RCDEULH80 = dr.GetDecimal(iRCDEULH80);

                    int iRCDEULH81 = dr.GetOrdinal(helper.RCDEULH81);
                    if (!dr.IsDBNull(iRCDEULH81)) entity.RCDEULH81 = dr.GetDecimal(iRCDEULH81);

                    int iRCDEULH82 = dr.GetOrdinal(helper.RCDEULH82);
                    if (!dr.IsDBNull(iRCDEULH82)) entity.RCDEULH82 = dr.GetDecimal(iRCDEULH82);

                    int iRCDEULH83 = dr.GetOrdinal(helper.RCDEULH83);
                    if (!dr.IsDBNull(iRCDEULH83)) entity.RCDEULH83 = dr.GetDecimal(iRCDEULH83);

                    int iRCDEULH84 = dr.GetOrdinal(helper.RCDEULH84);
                    if (!dr.IsDBNull(iRCDEULH84)) entity.RCDEULH84 = dr.GetDecimal(iRCDEULH84);

                    int iRCDEULH85 = dr.GetOrdinal(helper.RCDEULH85);
                    if (!dr.IsDBNull(iRCDEULH85)) entity.RCDEULH85 = dr.GetDecimal(iRCDEULH85);

                    int iRCDEULH86 = dr.GetOrdinal(helper.RCDEULH86);
                    if (!dr.IsDBNull(iRCDEULH86)) entity.RCDEULH86 = dr.GetDecimal(iRCDEULH86);

                    int iRCDEULH87 = dr.GetOrdinal(helper.RCDEULH87);
                    if (!dr.IsDBNull(iRCDEULH87)) entity.RCDEULH87 = dr.GetDecimal(iRCDEULH87);

                    int iRCDEULH88 = dr.GetOrdinal(helper.RCDEULH88);
                    if (!dr.IsDBNull(iRCDEULH88)) entity.RCDEULH88 = dr.GetDecimal(iRCDEULH88);

                    int iRCDEULH89 = dr.GetOrdinal(helper.RCDEULH89);
                    if (!dr.IsDBNull(iRCDEULH89)) entity.RCDEULH89 = dr.GetDecimal(iRCDEULH89);

                    int iRCDEULH90 = dr.GetOrdinal(helper.RCDEULH90);
                    if (!dr.IsDBNull(iRCDEULH90)) entity.RCDEULH90 = dr.GetDecimal(iRCDEULH90);

                    int iRCDEULH91 = dr.GetOrdinal(helper.RCDEULH91);
                    if (!dr.IsDBNull(iRCDEULH91)) entity.RCDEULH91 = dr.GetDecimal(iRCDEULH91);

                    int iRCDEULH92 = dr.GetOrdinal(helper.RCDEULH92);
                    if (!dr.IsDBNull(iRCDEULH92)) entity.RCDEULH92 = dr.GetDecimal(iRCDEULH92);

                    int iRCDEULH93 = dr.GetOrdinal(helper.RCDEULH93);
                    if (!dr.IsDBNull(iRCDEULH93)) entity.RCDEULH93 = dr.GetDecimal(iRCDEULH93);

                    int iRCDEULH94 = dr.GetOrdinal(helper.RCDEULH94);
                    if (!dr.IsDBNull(iRCDEULH94)) entity.RCDEULH94 = dr.GetDecimal(iRCDEULH94);

                    int iRCDEULH95 = dr.GetOrdinal(helper.RCDEULH95);
                    if (!dr.IsDBNull(iRCDEULH95)) entity.RCDEULH95 = dr.GetDecimal(iRCDEULH95);

                    int iRCDEULH96 = dr.GetOrdinal(helper.RCDEULH96);
                    if (!dr.IsDBNull(iRCDEULH96)) entity.RCDEULH96 = dr.GetDecimal(iRCDEULH96);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<RcaDemandaUsuarioDTO> ListDemandaUsuarioErroresExcel(string periodo)
        {
            //string condicion = " WHERE CP.RCDEULPERIODO = '" + periodo + "'";

            string sqlQuery = string.Format(this.helper.SqObtenerDemandaUsuarioErroresExcel, periodo);
            List<RcaDemandaUsuarioDTO> entitys = new List<RcaDemandaUsuarioDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RcaDemandaUsuarioDTO entity = new RcaDemandaUsuarioDTO();

                    //int iItem = dr.GetOrdinal(helper.Item);
                    //if (!dr.IsDBNull(iItem)) entity.Item = Convert.ToInt32(dr.GetValue(iItem));

                    int iRcdeulperiodo = dr.GetOrdinal(helper.Rcdeulperiodo);
                    if (!dr.IsDBNull(iRcdeulperiodo)) entity.Rcdeulperiodo = dr.GetString(iRcdeulperiodo);

                    //int iRcdeulfecmaxdem = dr.GetOrdinal(helper.Rcdeulfecmaxdem);
                    //if (!dr.IsDBNull(iRcdeulfecmaxdem)) entity.Rcdeulfecmaxdem = dr.GetDateTime(iRcdeulfecmaxdem);

                    //int iEmprCodi = dr.GetOrdinal(helper.Emprcodi);
                    //if (!dr.IsDBNull(iEmprCodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprCodi));

                    //int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    //if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iOsinergcodi = dr.GetOrdinal(helper.Osinergcodi);
                    if (!dr.IsDBNull(iOsinergcodi)) entity.Osinergcodi = dr.GetString(iOsinergcodi);

                    int iSuministrador = dr.GetOrdinal(helper.Suministrador);
                    if (!dr.IsDBNull(iSuministrador)) entity.Suministrador = dr.GetString(iSuministrador);

                    int iRuc = dr.GetOrdinal(helper.Ruc);
                    if (!dr.IsDBNull(iRuc)) entity.Ruc = dr.GetString(iRuc);

                    int iEmprrazsocial = dr.GetOrdinal(helper.Emprrazsocial);
                    if (!dr.IsDBNull(iEmprrazsocial)) entity.Emprrazsocial = dr.GetString(iEmprrazsocial);

                    int iSubestacion = dr.GetOrdinal(helper.Subestacion);
                    if (!dr.IsDBNull(iSubestacion)) entity.Subestacion = dr.GetString(iSubestacion);

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    //int iRcdeulfuente = dr.GetOrdinal(helper.Rcdeulfuente);
                    //if (!dr.IsDBNull(iRcdeulfuente)) entity.Rcdeulfuente = dr.GetString(iRcdeulfuente);

                    int iRcdeuldemandahp = dr.GetOrdinal(helper.Rcdeuldemandahp);
                    if (!dr.IsDBNull(iRcdeuldemandahp)) entity.Rcdeuldemandahp = dr.GetDecimal(iRcdeuldemandahp);

                    int iRcdeuldemandahfp = dr.GetOrdinal(helper.Rcdeuldemandahfp);
                    if (!dr.IsDBNull(iRcdeuldemandahfp)) entity.Rcdeuldemandahfp = dr.GetDecimal(iRcdeuldemandahfp);

                    int iRCDEULH1 = dr.GetOrdinal(helper.RCDEULH1);
                    if (!dr.IsDBNull(iRCDEULH1)) entity.RCDEULH1 = dr.GetDecimal(iRCDEULH1);

                    int iRCDEULH2 = dr.GetOrdinal(helper.RCDEULH2);
                    if (!dr.IsDBNull(iRCDEULH2)) entity.RCDEULH2 = dr.GetDecimal(iRCDEULH2);

                    int iRCDEULH3 = dr.GetOrdinal(helper.RCDEULH3);
                    if (!dr.IsDBNull(iRCDEULH3)) entity.RCDEULH3 = dr.GetDecimal(iRCDEULH3);

                    int iRCDEULH4 = dr.GetOrdinal(helper.RCDEULH4);
                    if (!dr.IsDBNull(iRCDEULH4)) entity.RCDEULH4 = dr.GetDecimal(iRCDEULH4);

                    int iRCDEULH5 = dr.GetOrdinal(helper.RCDEULH5);
                    if (!dr.IsDBNull(iRCDEULH5)) entity.RCDEULH5 = dr.GetDecimal(iRCDEULH5);

                    int iRCDEULH6 = dr.GetOrdinal(helper.RCDEULH6);
                    if (!dr.IsDBNull(iRCDEULH6)) entity.RCDEULH6 = dr.GetDecimal(iRCDEULH6);

                    int iRCDEULH7 = dr.GetOrdinal(helper.RCDEULH7);
                    if (!dr.IsDBNull(iRCDEULH7)) entity.RCDEULH7 = dr.GetDecimal(iRCDEULH7);

                    int iRCDEULH8 = dr.GetOrdinal(helper.RCDEULH8);
                    if (!dr.IsDBNull(iRCDEULH8)) entity.RCDEULH8 = dr.GetDecimal(iRCDEULH8);

                    int iRCDEULH9 = dr.GetOrdinal(helper.RCDEULH9);
                    if (!dr.IsDBNull(iRCDEULH9)) entity.RCDEULH9 = dr.GetDecimal(iRCDEULH9);

                    int iRCDEULH10 = dr.GetOrdinal(helper.RCDEULH10);
                    if (!dr.IsDBNull(iRCDEULH10)) entity.RCDEULH10 = dr.GetDecimal(iRCDEULH10);

                    int iRCDEULH11 = dr.GetOrdinal(helper.RCDEULH11);
                    if (!dr.IsDBNull(iRCDEULH11)) entity.RCDEULH11 = dr.GetDecimal(iRCDEULH11);

                    int iRCDEULH12 = dr.GetOrdinal(helper.RCDEULH12);
                    if (!dr.IsDBNull(iRCDEULH12)) entity.RCDEULH12 = dr.GetDecimal(iRCDEULH12);

                    int iRCDEULH13 = dr.GetOrdinal(helper.RCDEULH13);
                    if (!dr.IsDBNull(iRCDEULH13)) entity.RCDEULH13 = dr.GetDecimal(iRCDEULH13);

                    int iRCDEULH14 = dr.GetOrdinal(helper.RCDEULH14);
                    if (!dr.IsDBNull(iRCDEULH14)) entity.RCDEULH14 = dr.GetDecimal(iRCDEULH14);

                    int iRCDEULH15 = dr.GetOrdinal(helper.RCDEULH15);
                    if (!dr.IsDBNull(iRCDEULH15)) entity.RCDEULH15 = dr.GetDecimal(iRCDEULH15);

                    int iRCDEULH16 = dr.GetOrdinal(helper.RCDEULH16);
                    if (!dr.IsDBNull(iRCDEULH16)) entity.RCDEULH16 = dr.GetDecimal(iRCDEULH16);

                    int iRCDEULH17 = dr.GetOrdinal(helper.RCDEULH17);
                    if (!dr.IsDBNull(iRCDEULH17)) entity.RCDEULH17 = dr.GetDecimal(iRCDEULH17);

                    int iRCDEULH18 = dr.GetOrdinal(helper.RCDEULH18);
                    if (!dr.IsDBNull(iRCDEULH18)) entity.RCDEULH18 = dr.GetDecimal(iRCDEULH18);

                    int iRCDEULH19 = dr.GetOrdinal(helper.RCDEULH19);
                    if (!dr.IsDBNull(iRCDEULH19)) entity.RCDEULH19 = dr.GetDecimal(iRCDEULH19);

                    int iRCDEULH20 = dr.GetOrdinal(helper.RCDEULH20);
                    if (!dr.IsDBNull(iRCDEULH20)) entity.RCDEULH20 = dr.GetDecimal(iRCDEULH20);

                    int iRCDEULH21 = dr.GetOrdinal(helper.RCDEULH21);
                    if (!dr.IsDBNull(iRCDEULH21)) entity.RCDEULH21 = dr.GetDecimal(iRCDEULH21);

                    int iRCDEULH22 = dr.GetOrdinal(helper.RCDEULH22);
                    if (!dr.IsDBNull(iRCDEULH22)) entity.RCDEULH22 = dr.GetDecimal(iRCDEULH22);

                    int iRCDEULH23 = dr.GetOrdinal(helper.RCDEULH23);
                    if (!dr.IsDBNull(iRCDEULH23)) entity.RCDEULH23 = dr.GetDecimal(iRCDEULH23);

                    int iRCDEULH24 = dr.GetOrdinal(helper.RCDEULH24);
                    if (!dr.IsDBNull(iRCDEULH24)) entity.RCDEULH24 = dr.GetDecimal(iRCDEULH24);

                    int iRCDEULH25 = dr.GetOrdinal(helper.RCDEULH25);
                    if (!dr.IsDBNull(iRCDEULH25)) entity.RCDEULH25 = dr.GetDecimal(iRCDEULH25);

                    int iRCDEULH26 = dr.GetOrdinal(helper.RCDEULH26);
                    if (!dr.IsDBNull(iRCDEULH26)) entity.RCDEULH26 = dr.GetDecimal(iRCDEULH26);

                    int iRCDEULH27 = dr.GetOrdinal(helper.RCDEULH27);
                    if (!dr.IsDBNull(iRCDEULH27)) entity.RCDEULH27 = dr.GetDecimal(iRCDEULH27);

                    int iRCDEULH28 = dr.GetOrdinal(helper.RCDEULH28);
                    if (!dr.IsDBNull(iRCDEULH28)) entity.RCDEULH28 = dr.GetDecimal(iRCDEULH28);

                    int iRCDEULH29 = dr.GetOrdinal(helper.RCDEULH29);
                    if (!dr.IsDBNull(iRCDEULH29)) entity.RCDEULH29 = dr.GetDecimal(iRCDEULH29);

                    int iRCDEULH30 = dr.GetOrdinal(helper.RCDEULH30);
                    if (!dr.IsDBNull(iRCDEULH30)) entity.RCDEULH30 = dr.GetDecimal(iRCDEULH30);

                    int iRCDEULH31 = dr.GetOrdinal(helper.RCDEULH31);
                    if (!dr.IsDBNull(iRCDEULH31)) entity.RCDEULH31 = dr.GetDecimal(iRCDEULH31);

                    int iRCDEULH32 = dr.GetOrdinal(helper.RCDEULH32);
                    if (!dr.IsDBNull(iRCDEULH32)) entity.RCDEULH32 = dr.GetDecimal(iRCDEULH32);

                    int iRCDEULH33 = dr.GetOrdinal(helper.RCDEULH33);
                    if (!dr.IsDBNull(iRCDEULH33)) entity.RCDEULH33 = dr.GetDecimal(iRCDEULH33);

                    int iRCDEULH34 = dr.GetOrdinal(helper.RCDEULH34);
                    if (!dr.IsDBNull(iRCDEULH34)) entity.RCDEULH34 = dr.GetDecimal(iRCDEULH34);

                    int iRCDEULH35 = dr.GetOrdinal(helper.RCDEULH35);
                    if (!dr.IsDBNull(iRCDEULH35)) entity.RCDEULH35 = dr.GetDecimal(iRCDEULH35);

                    int iRCDEULH36 = dr.GetOrdinal(helper.RCDEULH36);
                    if (!dr.IsDBNull(iRCDEULH36)) entity.RCDEULH36 = dr.GetDecimal(iRCDEULH36);

                    int iRCDEULH37 = dr.GetOrdinal(helper.RCDEULH37);
                    if (!dr.IsDBNull(iRCDEULH37)) entity.RCDEULH37 = dr.GetDecimal(iRCDEULH37);

                    int iRCDEULH38 = dr.GetOrdinal(helper.RCDEULH38);
                    if (!dr.IsDBNull(iRCDEULH38)) entity.RCDEULH38 = dr.GetDecimal(iRCDEULH38);

                    int iRCDEULH39 = dr.GetOrdinal(helper.RCDEULH39);
                    if (!dr.IsDBNull(iRCDEULH39)) entity.RCDEULH39 = dr.GetDecimal(iRCDEULH39);

                    int iRCDEULH40 = dr.GetOrdinal(helper.RCDEULH40);
                    if (!dr.IsDBNull(iRCDEULH40)) entity.RCDEULH40 = dr.GetDecimal(iRCDEULH40);

                    int iRCDEULH41 = dr.GetOrdinal(helper.RCDEULH41);
                    if (!dr.IsDBNull(iRCDEULH41)) entity.RCDEULH41 = dr.GetDecimal(iRCDEULH41);

                    int iRCDEULH42 = dr.GetOrdinal(helper.RCDEULH42);
                    if (!dr.IsDBNull(iRCDEULH42)) entity.RCDEULH42 = dr.GetDecimal(iRCDEULH42);

                    int iRCDEULH43 = dr.GetOrdinal(helper.RCDEULH43);
                    if (!dr.IsDBNull(iRCDEULH43)) entity.RCDEULH43 = dr.GetDecimal(iRCDEULH43);

                    int iRCDEULH44 = dr.GetOrdinal(helper.RCDEULH44);
                    if (!dr.IsDBNull(iRCDEULH44)) entity.RCDEULH44 = dr.GetDecimal(iRCDEULH44);

                    int iRCDEULH45 = dr.GetOrdinal(helper.RCDEULH45);
                    if (!dr.IsDBNull(iRCDEULH45)) entity.RCDEULH45 = dr.GetDecimal(iRCDEULH45);

                    int iRCDEULH46 = dr.GetOrdinal(helper.RCDEULH46);
                    if (!dr.IsDBNull(iRCDEULH46)) entity.RCDEULH46 = dr.GetDecimal(iRCDEULH46);

                    int iRCDEULH47 = dr.GetOrdinal(helper.RCDEULH47);
                    if (!dr.IsDBNull(iRCDEULH47)) entity.RCDEULH47 = dr.GetDecimal(iRCDEULH47);

                    int iRCDEULH48 = dr.GetOrdinal(helper.RCDEULH48);
                    if (!dr.IsDBNull(iRCDEULH48)) entity.RCDEULH48 = dr.GetDecimal(iRCDEULH48);

                    int iRCDEULH49 = dr.GetOrdinal(helper.RCDEULH49);
                    if (!dr.IsDBNull(iRCDEULH49)) entity.RCDEULH49 = dr.GetDecimal(iRCDEULH49);

                    int iRCDEULH50 = dr.GetOrdinal(helper.RCDEULH50);
                    if (!dr.IsDBNull(iRCDEULH50)) entity.RCDEULH50 = dr.GetDecimal(iRCDEULH50);

                    int iRCDEULH51 = dr.GetOrdinal(helper.RCDEULH51);
                    if (!dr.IsDBNull(iRCDEULH51)) entity.RCDEULH51 = dr.GetDecimal(iRCDEULH51);

                    int iRCDEULH52 = dr.GetOrdinal(helper.RCDEULH52);
                    if (!dr.IsDBNull(iRCDEULH52)) entity.RCDEULH52 = dr.GetDecimal(iRCDEULH52);

                    int iRCDEULH53 = dr.GetOrdinal(helper.RCDEULH53);
                    if (!dr.IsDBNull(iRCDEULH53)) entity.RCDEULH53 = dr.GetDecimal(iRCDEULH53);

                    int iRCDEULH54 = dr.GetOrdinal(helper.RCDEULH54);
                    if (!dr.IsDBNull(iRCDEULH54)) entity.RCDEULH54 = dr.GetDecimal(iRCDEULH54);

                    int iRCDEULH55 = dr.GetOrdinal(helper.RCDEULH55);
                    if (!dr.IsDBNull(iRCDEULH55)) entity.RCDEULH55 = dr.GetDecimal(iRCDEULH55);

                    int iRCDEULH56 = dr.GetOrdinal(helper.RCDEULH56);
                    if (!dr.IsDBNull(iRCDEULH56)) entity.RCDEULH56 = dr.GetDecimal(iRCDEULH56);

                    int iRCDEULH57 = dr.GetOrdinal(helper.RCDEULH57);
                    if (!dr.IsDBNull(iRCDEULH57)) entity.RCDEULH57 = dr.GetDecimal(iRCDEULH57);

                    int iRCDEULH58 = dr.GetOrdinal(helper.RCDEULH58);
                    if (!dr.IsDBNull(iRCDEULH58)) entity.RCDEULH58 = dr.GetDecimal(iRCDEULH58);

                    int iRCDEULH59 = dr.GetOrdinal(helper.RCDEULH59);
                    if (!dr.IsDBNull(iRCDEULH59)) entity.RCDEULH59 = dr.GetDecimal(iRCDEULH59);

                    int iRCDEULH60 = dr.GetOrdinal(helper.RCDEULH60);
                    if (!dr.IsDBNull(iRCDEULH60)) entity.RCDEULH60 = dr.GetDecimal(iRCDEULH60);

                    int iRCDEULH61 = dr.GetOrdinal(helper.RCDEULH61);
                    if (!dr.IsDBNull(iRCDEULH61)) entity.RCDEULH61 = dr.GetDecimal(iRCDEULH61);

                    int iRCDEULH62 = dr.GetOrdinal(helper.RCDEULH62);
                    if (!dr.IsDBNull(iRCDEULH62)) entity.RCDEULH62 = dr.GetDecimal(iRCDEULH62);

                    int iRCDEULH63 = dr.GetOrdinal(helper.RCDEULH63);
                    if (!dr.IsDBNull(iRCDEULH63)) entity.RCDEULH63 = dr.GetDecimal(iRCDEULH63);

                    int iRCDEULH64 = dr.GetOrdinal(helper.RCDEULH64);
                    if (!dr.IsDBNull(iRCDEULH64)) entity.RCDEULH64 = dr.GetDecimal(iRCDEULH64);

                    int iRCDEULH65 = dr.GetOrdinal(helper.RCDEULH65);
                    if (!dr.IsDBNull(iRCDEULH65)) entity.RCDEULH65 = dr.GetDecimal(iRCDEULH65);

                    int iRCDEULH66 = dr.GetOrdinal(helper.RCDEULH66);
                    if (!dr.IsDBNull(iRCDEULH66)) entity.RCDEULH66 = dr.GetDecimal(iRCDEULH66);

                    int iRCDEULH67 = dr.GetOrdinal(helper.RCDEULH67);
                    if (!dr.IsDBNull(iRCDEULH67)) entity.RCDEULH67 = dr.GetDecimal(iRCDEULH67);

                    int iRCDEULH68 = dr.GetOrdinal(helper.RCDEULH68);
                    if (!dr.IsDBNull(iRCDEULH68)) entity.RCDEULH68 = dr.GetDecimal(iRCDEULH68);

                    int iRCDEULH69 = dr.GetOrdinal(helper.RCDEULH69);
                    if (!dr.IsDBNull(iRCDEULH69)) entity.RCDEULH69 = dr.GetDecimal(iRCDEULH69);

                    int iRCDEULH70 = dr.GetOrdinal(helper.RCDEULH70);
                    if (!dr.IsDBNull(iRCDEULH70)) entity.RCDEULH70 = dr.GetDecimal(iRCDEULH70);

                    int iRCDEULH71 = dr.GetOrdinal(helper.RCDEULH71);
                    if (!dr.IsDBNull(iRCDEULH71)) entity.RCDEULH71 = dr.GetDecimal(iRCDEULH71);

                    int iRCDEULH72 = dr.GetOrdinal(helper.RCDEULH72);
                    if (!dr.IsDBNull(iRCDEULH72)) entity.RCDEULH72 = dr.GetDecimal(iRCDEULH72);

                    int iRCDEULH73 = dr.GetOrdinal(helper.RCDEULH73);
                    if (!dr.IsDBNull(iRCDEULH73)) entity.RCDEULH73 = dr.GetDecimal(iRCDEULH73);

                    int iRCDEULH74 = dr.GetOrdinal(helper.RCDEULH74);
                    if (!dr.IsDBNull(iRCDEULH74)) entity.RCDEULH74 = dr.GetDecimal(iRCDEULH74);

                    int iRCDEULH75 = dr.GetOrdinal(helper.RCDEULH75);
                    if (!dr.IsDBNull(iRCDEULH75)) entity.RCDEULH75 = dr.GetDecimal(iRCDEULH75);

                    int iRCDEULH76 = dr.GetOrdinal(helper.RCDEULH76);
                    if (!dr.IsDBNull(iRCDEULH76)) entity.RCDEULH76 = dr.GetDecimal(iRCDEULH76);

                    int iRCDEULH77 = dr.GetOrdinal(helper.RCDEULH77);
                    if (!dr.IsDBNull(iRCDEULH77)) entity.RCDEULH77 = dr.GetDecimal(iRCDEULH77);

                    int iRCDEULH78 = dr.GetOrdinal(helper.RCDEULH78);
                    if (!dr.IsDBNull(iRCDEULH78)) entity.RCDEULH78 = dr.GetDecimal(iRCDEULH78);

                    int iRCDEULH79 = dr.GetOrdinal(helper.RCDEULH79);
                    if (!dr.IsDBNull(iRCDEULH79)) entity.RCDEULH79 = dr.GetDecimal(iRCDEULH79);

                    int iRCDEULH80 = dr.GetOrdinal(helper.RCDEULH80);
                    if (!dr.IsDBNull(iRCDEULH80)) entity.RCDEULH80 = dr.GetDecimal(iRCDEULH80);

                    int iRCDEULH81 = dr.GetOrdinal(helper.RCDEULH81);
                    if (!dr.IsDBNull(iRCDEULH81)) entity.RCDEULH81 = dr.GetDecimal(iRCDEULH81);

                    int iRCDEULH82 = dr.GetOrdinal(helper.RCDEULH82);
                    if (!dr.IsDBNull(iRCDEULH82)) entity.RCDEULH82 = dr.GetDecimal(iRCDEULH82);

                    int iRCDEULH83 = dr.GetOrdinal(helper.RCDEULH83);
                    if (!dr.IsDBNull(iRCDEULH83)) entity.RCDEULH83 = dr.GetDecimal(iRCDEULH83);

                    int iRCDEULH84 = dr.GetOrdinal(helper.RCDEULH84);
                    if (!dr.IsDBNull(iRCDEULH84)) entity.RCDEULH84 = dr.GetDecimal(iRCDEULH84);

                    int iRCDEULH85 = dr.GetOrdinal(helper.RCDEULH85);
                    if (!dr.IsDBNull(iRCDEULH85)) entity.RCDEULH85 = dr.GetDecimal(iRCDEULH85);

                    int iRCDEULH86 = dr.GetOrdinal(helper.RCDEULH86);
                    if (!dr.IsDBNull(iRCDEULH86)) entity.RCDEULH86 = dr.GetDecimal(iRCDEULH86);

                    int iRCDEULH87 = dr.GetOrdinal(helper.RCDEULH87);
                    if (!dr.IsDBNull(iRCDEULH87)) entity.RCDEULH87 = dr.GetDecimal(iRCDEULH87);

                    int iRCDEULH88 = dr.GetOrdinal(helper.RCDEULH88);
                    if (!dr.IsDBNull(iRCDEULH88)) entity.RCDEULH88 = dr.GetDecimal(iRCDEULH88);

                    int iRCDEULH89 = dr.GetOrdinal(helper.RCDEULH89);
                    if (!dr.IsDBNull(iRCDEULH89)) entity.RCDEULH89 = dr.GetDecimal(iRCDEULH89);

                    int iRCDEULH90 = dr.GetOrdinal(helper.RCDEULH90);
                    if (!dr.IsDBNull(iRCDEULH90)) entity.RCDEULH90 = dr.GetDecimal(iRCDEULH90);

                    int iRCDEULH91 = dr.GetOrdinal(helper.RCDEULH91);
                    if (!dr.IsDBNull(iRCDEULH91)) entity.RCDEULH91 = dr.GetDecimal(iRCDEULH91);

                    int iRCDEULH92 = dr.GetOrdinal(helper.RCDEULH92);
                    if (!dr.IsDBNull(iRCDEULH92)) entity.RCDEULH92 = dr.GetDecimal(iRCDEULH92);

                    int iRCDEULH93 = dr.GetOrdinal(helper.RCDEULH93);
                    if (!dr.IsDBNull(iRCDEULH93)) entity.RCDEULH93 = dr.GetDecimal(iRCDEULH93);

                    int iRCDEULH94 = dr.GetOrdinal(helper.RCDEULH94);
                    if (!dr.IsDBNull(iRCDEULH94)) entity.RCDEULH94 = dr.GetDecimal(iRCDEULH94);

                    int iRCDEULH95 = dr.GetOrdinal(helper.RCDEULH95);
                    if (!dr.IsDBNull(iRCDEULH95)) entity.RCDEULH95 = dr.GetDecimal(iRCDEULH95);

                    int iRCDEULH96 = dr.GetOrdinal(helper.RCDEULH96);
                    if (!dr.IsDBNull(iRCDEULH96)) entity.RCDEULH96 = dr.GetDecimal(iRCDEULH96);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<RcaDemandaUsuarioDTO> ListDemandaUsuarioReporteExcel(string periodo, string codigoZona, string codigoPuntoMedicion, string empresa, string suministrador)
        {
            string condicion = " WHERE CP.RCDEULPERIODO = '" + periodo + "'";

            if (!string.IsNullOrEmpty(codigoPuntoMedicion))
            {
                condicion = condicion + " AND EQ.AREACODI IN (" + codigoPuntoMedicion + ") ";
            }
            if (!string.IsNullOrEmpty(suministrador))
            {
                condicion = condicion + " AND CP.EMPRCODI IN (" + suministrador + ") ";
            }
            if (!string.IsNullOrEmpty(empresa))
            {
                condicion = condicion + string.Format(" AND NVL(CL.EMPRRAZSOCIAL,CL.EMPRABREV) LIKE '%{0}%' ", empresa.ToUpper());
            }

            string sqlQuery = string.Format(this.helper.SqObtenerReporteDemandaUsuarioExcel, condicion);
            List<RcaDemandaUsuarioDTO> entitys = new List<RcaDemandaUsuarioDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RcaDemandaUsuarioDTO entity = new RcaDemandaUsuarioDTO();

                    //int iItem = dr.GetOrdinal(helper.Item);
                    //if (!dr.IsDBNull(iItem)) entity.Item = Convert.ToInt32(dr.GetValue(iItem));

                    int iRcdeulperiodo = dr.GetOrdinal(helper.Rcdeulperiodo);
                    if (!dr.IsDBNull(iRcdeulperiodo)) entity.Rcdeulperiodo = dr.GetString(iRcdeulperiodo);

                    //int iRcdeulfecmaxdem = dr.GetOrdinal(helper.Rcdeulfecmaxdem);
                    //if (!dr.IsDBNull(iRcdeulfecmaxdem)) entity.Rcdeulfecmaxdem = dr.GetDateTime(iRcdeulfecmaxdem);

                    //int iEmprCodi = dr.GetOrdinal(helper.Emprcodi);
                    //if (!dr.IsDBNull(iEmprCodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprCodi));

                    //int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    //if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iOsinergcodi = dr.GetOrdinal(helper.Osinergcodi);
                    if (!dr.IsDBNull(iOsinergcodi)) entity.Osinergcodi = dr.GetString(iOsinergcodi);

                    int iSuministrador = dr.GetOrdinal(helper.Suministrador);
                    if (!dr.IsDBNull(iSuministrador)) entity.Suministrador = dr.GetString(iSuministrador);

                    int iRuc = dr.GetOrdinal(helper.Ruc);
                    if (!dr.IsDBNull(iRuc)) entity.Ruc = dr.GetString(iRuc);

                    int iEmprrazsocial = dr.GetOrdinal(helper.Emprrazsocial);
                    if (!dr.IsDBNull(iEmprrazsocial)) entity.Emprrazsocial = dr.GetString(iEmprrazsocial);

                    int iSubestacion = dr.GetOrdinal(helper.Subestacion);
                    if (!dr.IsDBNull(iSubestacion)) entity.Subestacion = dr.GetString(iSubestacion);

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    //int iRcdeulfuente = dr.GetOrdinal(helper.Rcdeulfuente);
                    //if (!dr.IsDBNull(iRcdeulfuente)) entity.Rcdeulfuente = dr.GetString(iRcdeulfuente);

                    int iRcdeuldemandahp = dr.GetOrdinal(helper.Rcdeuldemandahp);
                    if (!dr.IsDBNull(iRcdeuldemandahp)) entity.Rcdeuldemandahp = dr.GetDecimal(iRcdeuldemandahp);

                    int iRcdeuldemandahfp = dr.GetOrdinal(helper.Rcdeuldemandahfp);
                    if (!dr.IsDBNull(iRcdeuldemandahfp)) entity.Rcdeuldemandahfp = dr.GetDecimal(iRcdeuldemandahfp);

                    int iRCDEULH1 = dr.GetOrdinal(helper.RCDEULH1);
                    if (!dr.IsDBNull(iRCDEULH1)) entity.RCDEULH1 = dr.GetDecimal(iRCDEULH1);

                    int iRCDEULH2 = dr.GetOrdinal(helper.RCDEULH2);
                    if (!dr.IsDBNull(iRCDEULH2)) entity.RCDEULH2 = dr.GetDecimal(iRCDEULH2);

                    int iRCDEULH3 = dr.GetOrdinal(helper.RCDEULH3);
                    if (!dr.IsDBNull(iRCDEULH3)) entity.RCDEULH3 = dr.GetDecimal(iRCDEULH3);

                    int iRCDEULH4 = dr.GetOrdinal(helper.RCDEULH4);
                    if (!dr.IsDBNull(iRCDEULH4)) entity.RCDEULH4 = dr.GetDecimal(iRCDEULH4);

                    int iRCDEULH5 = dr.GetOrdinal(helper.RCDEULH5);
                    if (!dr.IsDBNull(iRCDEULH5)) entity.RCDEULH5 = dr.GetDecimal(iRCDEULH5);

                    int iRCDEULH6 = dr.GetOrdinal(helper.RCDEULH6);
                    if (!dr.IsDBNull(iRCDEULH6)) entity.RCDEULH6 = dr.GetDecimal(iRCDEULH6);

                    int iRCDEULH7 = dr.GetOrdinal(helper.RCDEULH7);
                    if (!dr.IsDBNull(iRCDEULH7)) entity.RCDEULH7 = dr.GetDecimal(iRCDEULH7);

                    int iRCDEULH8 = dr.GetOrdinal(helper.RCDEULH8);
                    if (!dr.IsDBNull(iRCDEULH8)) entity.RCDEULH8 = dr.GetDecimal(iRCDEULH8);

                    int iRCDEULH9 = dr.GetOrdinal(helper.RCDEULH9);
                    if (!dr.IsDBNull(iRCDEULH9)) entity.RCDEULH9 = dr.GetDecimal(iRCDEULH9);

                    int iRCDEULH10 = dr.GetOrdinal(helper.RCDEULH10);
                    if (!dr.IsDBNull(iRCDEULH10)) entity.RCDEULH10 = dr.GetDecimal(iRCDEULH10);

                    int iRCDEULH11 = dr.GetOrdinal(helper.RCDEULH11);
                    if (!dr.IsDBNull(iRCDEULH11)) entity.RCDEULH11 = dr.GetDecimal(iRCDEULH11);

                    int iRCDEULH12 = dr.GetOrdinal(helper.RCDEULH12);
                    if (!dr.IsDBNull(iRCDEULH12)) entity.RCDEULH12 = dr.GetDecimal(iRCDEULH12);

                    int iRCDEULH13 = dr.GetOrdinal(helper.RCDEULH13);
                    if (!dr.IsDBNull(iRCDEULH13)) entity.RCDEULH13 = dr.GetDecimal(iRCDEULH13);

                    int iRCDEULH14 = dr.GetOrdinal(helper.RCDEULH14);
                    if (!dr.IsDBNull(iRCDEULH14)) entity.RCDEULH14 = dr.GetDecimal(iRCDEULH14);

                    int iRCDEULH15 = dr.GetOrdinal(helper.RCDEULH15);
                    if (!dr.IsDBNull(iRCDEULH15)) entity.RCDEULH15 = dr.GetDecimal(iRCDEULH15);

                    int iRCDEULH16 = dr.GetOrdinal(helper.RCDEULH16);
                    if (!dr.IsDBNull(iRCDEULH16)) entity.RCDEULH16 = dr.GetDecimal(iRCDEULH16);

                    int iRCDEULH17 = dr.GetOrdinal(helper.RCDEULH17);
                    if (!dr.IsDBNull(iRCDEULH17)) entity.RCDEULH17 = dr.GetDecimal(iRCDEULH17);

                    int iRCDEULH18 = dr.GetOrdinal(helper.RCDEULH18);
                    if (!dr.IsDBNull(iRCDEULH18)) entity.RCDEULH18 = dr.GetDecimal(iRCDEULH18);

                    int iRCDEULH19 = dr.GetOrdinal(helper.RCDEULH19);
                    if (!dr.IsDBNull(iRCDEULH19)) entity.RCDEULH19 = dr.GetDecimal(iRCDEULH19);

                    int iRCDEULH20 = dr.GetOrdinal(helper.RCDEULH20);
                    if (!dr.IsDBNull(iRCDEULH20)) entity.RCDEULH20 = dr.GetDecimal(iRCDEULH20);

                    int iRCDEULH21 = dr.GetOrdinal(helper.RCDEULH21);
                    if (!dr.IsDBNull(iRCDEULH21)) entity.RCDEULH21 = dr.GetDecimal(iRCDEULH21);

                    int iRCDEULH22 = dr.GetOrdinal(helper.RCDEULH22);
                    if (!dr.IsDBNull(iRCDEULH22)) entity.RCDEULH22 = dr.GetDecimal(iRCDEULH22);

                    int iRCDEULH23 = dr.GetOrdinal(helper.RCDEULH23);
                    if (!dr.IsDBNull(iRCDEULH23)) entity.RCDEULH23 = dr.GetDecimal(iRCDEULH23);

                    int iRCDEULH24 = dr.GetOrdinal(helper.RCDEULH24);
                    if (!dr.IsDBNull(iRCDEULH24)) entity.RCDEULH24 = dr.GetDecimal(iRCDEULH24);

                    int iRCDEULH25 = dr.GetOrdinal(helper.RCDEULH25);
                    if (!dr.IsDBNull(iRCDEULH25)) entity.RCDEULH25 = dr.GetDecimal(iRCDEULH25);

                    int iRCDEULH26 = dr.GetOrdinal(helper.RCDEULH26);
                    if (!dr.IsDBNull(iRCDEULH26)) entity.RCDEULH26 = dr.GetDecimal(iRCDEULH26);

                    int iRCDEULH27 = dr.GetOrdinal(helper.RCDEULH27);
                    if (!dr.IsDBNull(iRCDEULH27)) entity.RCDEULH27 = dr.GetDecimal(iRCDEULH27);

                    int iRCDEULH28 = dr.GetOrdinal(helper.RCDEULH28);
                    if (!dr.IsDBNull(iRCDEULH28)) entity.RCDEULH28 = dr.GetDecimal(iRCDEULH28);

                    int iRCDEULH29 = dr.GetOrdinal(helper.RCDEULH29);
                    if (!dr.IsDBNull(iRCDEULH29)) entity.RCDEULH29 = dr.GetDecimal(iRCDEULH29);

                    int iRCDEULH30 = dr.GetOrdinal(helper.RCDEULH30);
                    if (!dr.IsDBNull(iRCDEULH30)) entity.RCDEULH30 = dr.GetDecimal(iRCDEULH30);

                    int iRCDEULH31 = dr.GetOrdinal(helper.RCDEULH31);
                    if (!dr.IsDBNull(iRCDEULH31)) entity.RCDEULH31 = dr.GetDecimal(iRCDEULH31);

                    int iRCDEULH32 = dr.GetOrdinal(helper.RCDEULH32);
                    if (!dr.IsDBNull(iRCDEULH32)) entity.RCDEULH32 = dr.GetDecimal(iRCDEULH32);

                    int iRCDEULH33 = dr.GetOrdinal(helper.RCDEULH33);
                    if (!dr.IsDBNull(iRCDEULH33)) entity.RCDEULH33 = dr.GetDecimal(iRCDEULH33);

                    int iRCDEULH34 = dr.GetOrdinal(helper.RCDEULH34);
                    if (!dr.IsDBNull(iRCDEULH34)) entity.RCDEULH34 = dr.GetDecimal(iRCDEULH34);

                    int iRCDEULH35 = dr.GetOrdinal(helper.RCDEULH35);
                    if (!dr.IsDBNull(iRCDEULH35)) entity.RCDEULH35 = dr.GetDecimal(iRCDEULH35);

                    int iRCDEULH36 = dr.GetOrdinal(helper.RCDEULH36);
                    if (!dr.IsDBNull(iRCDEULH36)) entity.RCDEULH36 = dr.GetDecimal(iRCDEULH36);

                    int iRCDEULH37 = dr.GetOrdinal(helper.RCDEULH37);
                    if (!dr.IsDBNull(iRCDEULH37)) entity.RCDEULH37 = dr.GetDecimal(iRCDEULH37);

                    int iRCDEULH38 = dr.GetOrdinal(helper.RCDEULH38);
                    if (!dr.IsDBNull(iRCDEULH38)) entity.RCDEULH38 = dr.GetDecimal(iRCDEULH38);

                    int iRCDEULH39 = dr.GetOrdinal(helper.RCDEULH39);
                    if (!dr.IsDBNull(iRCDEULH39)) entity.RCDEULH39 = dr.GetDecimal(iRCDEULH39);

                    int iRCDEULH40 = dr.GetOrdinal(helper.RCDEULH40);
                    if (!dr.IsDBNull(iRCDEULH40)) entity.RCDEULH40 = dr.GetDecimal(iRCDEULH40);

                    int iRCDEULH41 = dr.GetOrdinal(helper.RCDEULH41);
                    if (!dr.IsDBNull(iRCDEULH41)) entity.RCDEULH41 = dr.GetDecimal(iRCDEULH41);

                    int iRCDEULH42 = dr.GetOrdinal(helper.RCDEULH42);
                    if (!dr.IsDBNull(iRCDEULH42)) entity.RCDEULH42 = dr.GetDecimal(iRCDEULH42);

                    int iRCDEULH43 = dr.GetOrdinal(helper.RCDEULH43);
                    if (!dr.IsDBNull(iRCDEULH43)) entity.RCDEULH43 = dr.GetDecimal(iRCDEULH43);

                    int iRCDEULH44 = dr.GetOrdinal(helper.RCDEULH44);
                    if (!dr.IsDBNull(iRCDEULH44)) entity.RCDEULH44 = dr.GetDecimal(iRCDEULH44);

                    int iRCDEULH45 = dr.GetOrdinal(helper.RCDEULH45);
                    if (!dr.IsDBNull(iRCDEULH45)) entity.RCDEULH45 = dr.GetDecimal(iRCDEULH45);

                    int iRCDEULH46 = dr.GetOrdinal(helper.RCDEULH46);
                    if (!dr.IsDBNull(iRCDEULH46)) entity.RCDEULH46 = dr.GetDecimal(iRCDEULH46);

                    int iRCDEULH47 = dr.GetOrdinal(helper.RCDEULH47);
                    if (!dr.IsDBNull(iRCDEULH47)) entity.RCDEULH47 = dr.GetDecimal(iRCDEULH47);

                    int iRCDEULH48 = dr.GetOrdinal(helper.RCDEULH48);
                    if (!dr.IsDBNull(iRCDEULH48)) entity.RCDEULH48 = dr.GetDecimal(iRCDEULH48);

                    int iRCDEULH49 = dr.GetOrdinal(helper.RCDEULH49);
                    if (!dr.IsDBNull(iRCDEULH49)) entity.RCDEULH49 = dr.GetDecimal(iRCDEULH49);

                    int iRCDEULH50 = dr.GetOrdinal(helper.RCDEULH50);
                    if (!dr.IsDBNull(iRCDEULH50)) entity.RCDEULH50 = dr.GetDecimal(iRCDEULH50);

                    int iRCDEULH51 = dr.GetOrdinal(helper.RCDEULH51);
                    if (!dr.IsDBNull(iRCDEULH51)) entity.RCDEULH51 = dr.GetDecimal(iRCDEULH51);

                    int iRCDEULH52 = dr.GetOrdinal(helper.RCDEULH52);
                    if (!dr.IsDBNull(iRCDEULH52)) entity.RCDEULH52 = dr.GetDecimal(iRCDEULH52);

                    int iRCDEULH53 = dr.GetOrdinal(helper.RCDEULH53);
                    if (!dr.IsDBNull(iRCDEULH53)) entity.RCDEULH53 = dr.GetDecimal(iRCDEULH53);

                    int iRCDEULH54 = dr.GetOrdinal(helper.RCDEULH54);
                    if (!dr.IsDBNull(iRCDEULH54)) entity.RCDEULH54 = dr.GetDecimal(iRCDEULH54);

                    int iRCDEULH55 = dr.GetOrdinal(helper.RCDEULH55);
                    if (!dr.IsDBNull(iRCDEULH55)) entity.RCDEULH55 = dr.GetDecimal(iRCDEULH55);

                    int iRCDEULH56 = dr.GetOrdinal(helper.RCDEULH56);
                    if (!dr.IsDBNull(iRCDEULH56)) entity.RCDEULH56 = dr.GetDecimal(iRCDEULH56);

                    int iRCDEULH57 = dr.GetOrdinal(helper.RCDEULH57);
                    if (!dr.IsDBNull(iRCDEULH57)) entity.RCDEULH57 = dr.GetDecimal(iRCDEULH57);

                    int iRCDEULH58 = dr.GetOrdinal(helper.RCDEULH58);
                    if (!dr.IsDBNull(iRCDEULH58)) entity.RCDEULH58 = dr.GetDecimal(iRCDEULH58);

                    int iRCDEULH59 = dr.GetOrdinal(helper.RCDEULH59);
                    if (!dr.IsDBNull(iRCDEULH59)) entity.RCDEULH59 = dr.GetDecimal(iRCDEULH59);

                    int iRCDEULH60 = dr.GetOrdinal(helper.RCDEULH60);
                    if (!dr.IsDBNull(iRCDEULH60)) entity.RCDEULH60 = dr.GetDecimal(iRCDEULH60);

                    int iRCDEULH61 = dr.GetOrdinal(helper.RCDEULH61);
                    if (!dr.IsDBNull(iRCDEULH61)) entity.RCDEULH61 = dr.GetDecimal(iRCDEULH61);

                    int iRCDEULH62 = dr.GetOrdinal(helper.RCDEULH62);
                    if (!dr.IsDBNull(iRCDEULH62)) entity.RCDEULH62 = dr.GetDecimal(iRCDEULH62);

                    int iRCDEULH63 = dr.GetOrdinal(helper.RCDEULH63);
                    if (!dr.IsDBNull(iRCDEULH63)) entity.RCDEULH63 = dr.GetDecimal(iRCDEULH63);

                    int iRCDEULH64 = dr.GetOrdinal(helper.RCDEULH64);
                    if (!dr.IsDBNull(iRCDEULH64)) entity.RCDEULH64 = dr.GetDecimal(iRCDEULH64);

                    int iRCDEULH65 = dr.GetOrdinal(helper.RCDEULH65);
                    if (!dr.IsDBNull(iRCDEULH65)) entity.RCDEULH65 = dr.GetDecimal(iRCDEULH65);

                    int iRCDEULH66 = dr.GetOrdinal(helper.RCDEULH66);
                    if (!dr.IsDBNull(iRCDEULH66)) entity.RCDEULH66 = dr.GetDecimal(iRCDEULH66);

                    int iRCDEULH67 = dr.GetOrdinal(helper.RCDEULH67);
                    if (!dr.IsDBNull(iRCDEULH67)) entity.RCDEULH67 = dr.GetDecimal(iRCDEULH67);

                    int iRCDEULH68 = dr.GetOrdinal(helper.RCDEULH68);
                    if (!dr.IsDBNull(iRCDEULH68)) entity.RCDEULH68 = dr.GetDecimal(iRCDEULH68);

                    int iRCDEULH69 = dr.GetOrdinal(helper.RCDEULH69);
                    if (!dr.IsDBNull(iRCDEULH69)) entity.RCDEULH69 = dr.GetDecimal(iRCDEULH69);

                    int iRCDEULH70 = dr.GetOrdinal(helper.RCDEULH70);
                    if (!dr.IsDBNull(iRCDEULH70)) entity.RCDEULH70 = dr.GetDecimal(iRCDEULH70);

                    int iRCDEULH71 = dr.GetOrdinal(helper.RCDEULH71);
                    if (!dr.IsDBNull(iRCDEULH71)) entity.RCDEULH71 = dr.GetDecimal(iRCDEULH71);

                    int iRCDEULH72 = dr.GetOrdinal(helper.RCDEULH72);
                    if (!dr.IsDBNull(iRCDEULH72)) entity.RCDEULH72 = dr.GetDecimal(iRCDEULH72);

                    int iRCDEULH73 = dr.GetOrdinal(helper.RCDEULH73);
                    if (!dr.IsDBNull(iRCDEULH73)) entity.RCDEULH73 = dr.GetDecimal(iRCDEULH73);

                    int iRCDEULH74 = dr.GetOrdinal(helper.RCDEULH74);
                    if (!dr.IsDBNull(iRCDEULH74)) entity.RCDEULH74 = dr.GetDecimal(iRCDEULH74);

                    int iRCDEULH75 = dr.GetOrdinal(helper.RCDEULH75);
                    if (!dr.IsDBNull(iRCDEULH75)) entity.RCDEULH75 = dr.GetDecimal(iRCDEULH75);

                    int iRCDEULH76 = dr.GetOrdinal(helper.RCDEULH76);
                    if (!dr.IsDBNull(iRCDEULH76)) entity.RCDEULH76 = dr.GetDecimal(iRCDEULH76);

                    int iRCDEULH77 = dr.GetOrdinal(helper.RCDEULH77);
                    if (!dr.IsDBNull(iRCDEULH77)) entity.RCDEULH77 = dr.GetDecimal(iRCDEULH77);

                    int iRCDEULH78 = dr.GetOrdinal(helper.RCDEULH78);
                    if (!dr.IsDBNull(iRCDEULH78)) entity.RCDEULH78 = dr.GetDecimal(iRCDEULH78);

                    int iRCDEULH79 = dr.GetOrdinal(helper.RCDEULH79);
                    if (!dr.IsDBNull(iRCDEULH79)) entity.RCDEULH79 = dr.GetDecimal(iRCDEULH79);

                    int iRCDEULH80 = dr.GetOrdinal(helper.RCDEULH80);
                    if (!dr.IsDBNull(iRCDEULH80)) entity.RCDEULH80 = dr.GetDecimal(iRCDEULH80);

                    int iRCDEULH81 = dr.GetOrdinal(helper.RCDEULH81);
                    if (!dr.IsDBNull(iRCDEULH81)) entity.RCDEULH81 = dr.GetDecimal(iRCDEULH81);

                    int iRCDEULH82 = dr.GetOrdinal(helper.RCDEULH82);
                    if (!dr.IsDBNull(iRCDEULH82)) entity.RCDEULH82 = dr.GetDecimal(iRCDEULH82);

                    int iRCDEULH83 = dr.GetOrdinal(helper.RCDEULH83);
                    if (!dr.IsDBNull(iRCDEULH83)) entity.RCDEULH83 = dr.GetDecimal(iRCDEULH83);

                    int iRCDEULH84 = dr.GetOrdinal(helper.RCDEULH84);
                    if (!dr.IsDBNull(iRCDEULH84)) entity.RCDEULH84 = dr.GetDecimal(iRCDEULH84);

                    int iRCDEULH85 = dr.GetOrdinal(helper.RCDEULH85);
                    if (!dr.IsDBNull(iRCDEULH85)) entity.RCDEULH85 = dr.GetDecimal(iRCDEULH85);

                    int iRCDEULH86 = dr.GetOrdinal(helper.RCDEULH86);
                    if (!dr.IsDBNull(iRCDEULH86)) entity.RCDEULH86 = dr.GetDecimal(iRCDEULH86);

                    int iRCDEULH87 = dr.GetOrdinal(helper.RCDEULH87);
                    if (!dr.IsDBNull(iRCDEULH87)) entity.RCDEULH87 = dr.GetDecimal(iRCDEULH87);

                    int iRCDEULH88 = dr.GetOrdinal(helper.RCDEULH88);
                    if (!dr.IsDBNull(iRCDEULH88)) entity.RCDEULH88 = dr.GetDecimal(iRCDEULH88);

                    int iRCDEULH89 = dr.GetOrdinal(helper.RCDEULH89);
                    if (!dr.IsDBNull(iRCDEULH89)) entity.RCDEULH89 = dr.GetDecimal(iRCDEULH89);

                    int iRCDEULH90 = dr.GetOrdinal(helper.RCDEULH90);
                    if (!dr.IsDBNull(iRCDEULH90)) entity.RCDEULH90 = dr.GetDecimal(iRCDEULH90);

                    int iRCDEULH91 = dr.GetOrdinal(helper.RCDEULH91);
                    if (!dr.IsDBNull(iRCDEULH91)) entity.RCDEULH91 = dr.GetDecimal(iRCDEULH91);

                    int iRCDEULH92 = dr.GetOrdinal(helper.RCDEULH92);
                    if (!dr.IsDBNull(iRCDEULH92)) entity.RCDEULH92 = dr.GetDecimal(iRCDEULH92);

                    int iRCDEULH93 = dr.GetOrdinal(helper.RCDEULH93);
                    if (!dr.IsDBNull(iRCDEULH93)) entity.RCDEULH93 = dr.GetDecimal(iRCDEULH93);

                    int iRCDEULH94 = dr.GetOrdinal(helper.RCDEULH94);
                    if (!dr.IsDBNull(iRCDEULH94)) entity.RCDEULH94 = dr.GetDecimal(iRCDEULH94);

                    int iRCDEULH95 = dr.GetOrdinal(helper.RCDEULH95);
                    if (!dr.IsDBNull(iRCDEULH95)) entity.RCDEULH95 = dr.GetDecimal(iRCDEULH95);

                    int iRCDEULH96 = dr.GetOrdinal(helper.RCDEULH96);
                    if (!dr.IsDBNull(iRCDEULH96)) entity.RCDEULH96 = dr.GetDecimal(iRCDEULH96);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }
    }
}
