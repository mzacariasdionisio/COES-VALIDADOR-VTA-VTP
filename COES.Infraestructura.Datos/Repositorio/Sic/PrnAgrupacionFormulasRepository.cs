using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Sic;
using System.Data;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    public class PrnAgrupacionFormulasRepository : RepositoryBase, IPrnAgrupacionFormulasRepository
    {
        public PrnAgrupacionFormulasRepository(string strConn) : base(strConn)
        {
        }

        PrnAgrupacionFormulasHelper helper = new PrnAgrupacionFormulasHelper();

        public void Save(PrnAgrupacionFormulasDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);
            dbProvider.AddInParameter(command, helper.Prnafmcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.Prrucodi, DbType.Int32, entity.Prrucodi);
            dbProvider.AddInParameter(command, helper.Prnafmprioridad, DbType.Int32, entity.Prnafmprioridad);
            dbProvider.AddInParameter(command, helper.Prnafmflagesmanual, DbType.String, entity.Prnafmflagesmanual);
            dbProvider.AddInParameter(command, helper.H1, DbType.Decimal, entity.H1);
            dbProvider.AddInParameter(command, helper.H2, DbType.Decimal, entity.H2);
            dbProvider.AddInParameter(command, helper.H3, DbType.Decimal, entity.H3);
            dbProvider.AddInParameter(command, helper.H4, DbType.Decimal, entity.H4);
            dbProvider.AddInParameter(command, helper.H5, DbType.Decimal, entity.H5);
            dbProvider.AddInParameter(command, helper.H6, DbType.Decimal, entity.H6);
            dbProvider.AddInParameter(command, helper.H7, DbType.Decimal, entity.H7);
            dbProvider.AddInParameter(command, helper.H8, DbType.Decimal, entity.H8);
            dbProvider.AddInParameter(command, helper.H9, DbType.Decimal, entity.H9);
            dbProvider.AddInParameter(command, helper.H10, DbType.Decimal, entity.H10);
            dbProvider.AddInParameter(command, helper.H11, DbType.Decimal, entity.H11);
            dbProvider.AddInParameter(command, helper.H12, DbType.Decimal, entity.H12);
            dbProvider.AddInParameter(command, helper.H13, DbType.Decimal, entity.H13);
            dbProvider.AddInParameter(command, helper.H14, DbType.Decimal, entity.H14);
            dbProvider.AddInParameter(command, helper.H15, DbType.Decimal, entity.H15);
            dbProvider.AddInParameter(command, helper.H16, DbType.Decimal, entity.H16);
            dbProvider.AddInParameter(command, helper.H17, DbType.Decimal, entity.H17);
            dbProvider.AddInParameter(command, helper.H18, DbType.Decimal, entity.H18);
            dbProvider.AddInParameter(command, helper.H19, DbType.Decimal, entity.H19);
            dbProvider.AddInParameter(command, helper.H20, DbType.Decimal, entity.H20);
            dbProvider.AddInParameter(command, helper.H21, DbType.Decimal, entity.H21);
            dbProvider.AddInParameter(command, helper.H22, DbType.Decimal, entity.H22);
            dbProvider.AddInParameter(command, helper.H23, DbType.Decimal, entity.H23);
            dbProvider.AddInParameter(command, helper.H24, DbType.Decimal, entity.H24);
            dbProvider.AddInParameter(command, helper.H25, DbType.Decimal, entity.H25);
            dbProvider.AddInParameter(command, helper.H26, DbType.Decimal, entity.H26);
            dbProvider.AddInParameter(command, helper.H27, DbType.Decimal, entity.H27);
            dbProvider.AddInParameter(command, helper.H28, DbType.Decimal, entity.H28);
            dbProvider.AddInParameter(command, helper.H29, DbType.Decimal, entity.H29);
            dbProvider.AddInParameter(command, helper.H30, DbType.Decimal, entity.H30);
            dbProvider.AddInParameter(command, helper.H31, DbType.Decimal, entity.H31);
            dbProvider.AddInParameter(command, helper.H32, DbType.Decimal, entity.H32);
            dbProvider.AddInParameter(command, helper.H33, DbType.Decimal, entity.H33);
            dbProvider.AddInParameter(command, helper.H34, DbType.Decimal, entity.H34);
            dbProvider.AddInParameter(command, helper.H35, DbType.Decimal, entity.H35);
            dbProvider.AddInParameter(command, helper.H36, DbType.Decimal, entity.H36);
            dbProvider.AddInParameter(command, helper.H37, DbType.Decimal, entity.H37);
            dbProvider.AddInParameter(command, helper.H38, DbType.Decimal, entity.H38);
            dbProvider.AddInParameter(command, helper.H39, DbType.Decimal, entity.H39);
            dbProvider.AddInParameter(command, helper.H40, DbType.Decimal, entity.H40);
            dbProvider.AddInParameter(command, helper.H41, DbType.Decimal, entity.H41);
            dbProvider.AddInParameter(command, helper.H42, DbType.Decimal, entity.H42);
            dbProvider.AddInParameter(command, helper.H43, DbType.Decimal, entity.H43);
            dbProvider.AddInParameter(command, helper.H44, DbType.Decimal, entity.H44);
            dbProvider.AddInParameter(command, helper.H45, DbType.Decimal, entity.H45);
            dbProvider.AddInParameter(command, helper.H46, DbType.Decimal, entity.H46);
            dbProvider.AddInParameter(command, helper.H47, DbType.Decimal, entity.H47);
            dbProvider.AddInParameter(command, helper.H48, DbType.Decimal, entity.H48);
            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int ptomedicodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, ptomedicodi);
            
            dbProvider.ExecuteNonQuery(command);
        }

        public List<PrnAgrupacionFormulasDTO> GetListAgrupacionFormulasByAgrupacion(int agrupacion)
        {
            PrnAgrupacionFormulasDTO entity = new PrnAgrupacionFormulasDTO();
            List<PrnAgrupacionFormulasDTO> entitys = new List<PrnAgrupacionFormulasDTO>();
            string query = string.Format(helper.SqlGetListAgrupacionFormulasByAgrupacion, agrupacion);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new PrnAgrupacionFormulasDTO();

                    int iPrnafmcodi = dr.GetOrdinal(helper.Prnafmcodi);
                    if (!dr.IsDBNull(iPrnafmcodi)) entity.Prnafmcodi = Convert.ToInt32(dr.GetValue(iPrnafmcodi));

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iPrrucodi = dr.GetOrdinal(helper.Prrucodi);
                    if (!dr.IsDBNull(iPrrucodi)) entity.Prrucodi = Convert.ToInt32(dr.GetValue(iPrrucodi));

                    int iPrruabrev = dr.GetOrdinal(helper.Prruabrev);
                    if (!dr.IsDBNull(iPrruabrev)) entity.Prruabrev = dr.GetString(iPrruabrev);

                    int iPrnafmprioridad = dr.GetOrdinal(helper.Prnafmprioridad);
                    if (!dr.IsDBNull(iPrnafmprioridad)) entity.Prnafmprioridad = Convert.ToInt32(dr.GetValue(iPrnafmprioridad));

                    int iPrnafmflagesmanual = dr.GetOrdinal(helper.Prnafmflagesmanual);
                    if (!dr.IsDBNull(iPrnafmflagesmanual)) entity.Prnafmflagesmanual = dr.GetString(iPrnafmflagesmanual);

                    int iH1 = dr.GetOrdinal(helper.H1);
                    if (!dr.IsDBNull(iH1)) entity.H1 = dr.GetDecimal(iH1);

                    int iH2 = dr.GetOrdinal(helper.H2);
                    if (!dr.IsDBNull(iH2)) entity.H2 = dr.GetDecimal(iH2);

                    int iH3 = dr.GetOrdinal(helper.H3);
                    if (!dr.IsDBNull(iH3)) entity.H3 = dr.GetDecimal(iH3);

                    int iH4 = dr.GetOrdinal(helper.H4);
                    if (!dr.IsDBNull(iH4)) entity.H4 = dr.GetDecimal(iH4);

                    int iH5 = dr.GetOrdinal(helper.H5);
                    if (!dr.IsDBNull(iH5)) entity.H5 = dr.GetDecimal(iH5);

                    int iH6 = dr.GetOrdinal(helper.H6);
                    if (!dr.IsDBNull(iH6)) entity.H6 = dr.GetDecimal(iH6);

                    int iH7 = dr.GetOrdinal(helper.H7);
                    if (!dr.IsDBNull(iH7)) entity.H7 = dr.GetDecimal(iH7);

                    int iH8 = dr.GetOrdinal(helper.H8);
                    if (!dr.IsDBNull(iH8)) entity.H8 = dr.GetDecimal(iH8);

                    int iH9 = dr.GetOrdinal(helper.H9);
                    if (!dr.IsDBNull(iH9)) entity.H9 = dr.GetDecimal(iH9);

                    int iH10 = dr.GetOrdinal(helper.H10);
                    if (!dr.IsDBNull(iH10)) entity.H10 = dr.GetDecimal(iH10);

                    int iH11 = dr.GetOrdinal(helper.H11);
                    if (!dr.IsDBNull(iH11)) entity.H11 = dr.GetDecimal(iH11);

                    int iH12 = dr.GetOrdinal(helper.H12);
                    if (!dr.IsDBNull(iH12)) entity.H12 = dr.GetDecimal(iH12);

                    int iH13 = dr.GetOrdinal(helper.H13);
                    if (!dr.IsDBNull(iH13)) entity.H13 = dr.GetDecimal(iH13);

                    int iH14 = dr.GetOrdinal(helper.H14);
                    if (!dr.IsDBNull(iH14)) entity.H14 = dr.GetDecimal(iH14);

                    int iH15 = dr.GetOrdinal(helper.H15);
                    if (!dr.IsDBNull(iH15)) entity.H15 = dr.GetDecimal(iH15);

                    int iH16 = dr.GetOrdinal(helper.H16);
                    if (!dr.IsDBNull(iH16)) entity.H16 = dr.GetDecimal(iH16);

                    int iH17 = dr.GetOrdinal(helper.H17);
                    if (!dr.IsDBNull(iH17)) entity.H17 = dr.GetDecimal(iH17);

                    int iH18 = dr.GetOrdinal(helper.H18);
                    if (!dr.IsDBNull(iH18)) entity.H18 = dr.GetDecimal(iH18);

                    int iH19 = dr.GetOrdinal(helper.H19);
                    if (!dr.IsDBNull(iH19)) entity.H19 = dr.GetDecimal(iH19);

                    int iH20 = dr.GetOrdinal(helper.H20);
                    if (!dr.IsDBNull(iH20)) entity.H20 = dr.GetDecimal(iH20);

                    int iH21 = dr.GetOrdinal(helper.H21);
                    if (!dr.IsDBNull(iH21)) entity.H21 = dr.GetDecimal(iH21);

                    int iH22 = dr.GetOrdinal(helper.H22);
                    if (!dr.IsDBNull(iH22)) entity.H22 = dr.GetDecimal(iH22);

                    int iH23 = dr.GetOrdinal(helper.H23);
                    if (!dr.IsDBNull(iH23)) entity.H23 = dr.GetDecimal(iH23);

                    int iH24 = dr.GetOrdinal(helper.H24);
                    if (!dr.IsDBNull(iH24)) entity.H24 = dr.GetDecimal(iH24);

                    int iH25 = dr.GetOrdinal(helper.H25);
                    if (!dr.IsDBNull(iH25)) entity.H25 = dr.GetDecimal(iH25);

                    int iH26 = dr.GetOrdinal(helper.H26);
                    if (!dr.IsDBNull(iH26)) entity.H26 = dr.GetDecimal(iH26);

                    int iH27 = dr.GetOrdinal(helper.H27);
                    if (!dr.IsDBNull(iH27)) entity.H27 = dr.GetDecimal(iH27);

                    int iH28 = dr.GetOrdinal(helper.H28);
                    if (!dr.IsDBNull(iH28)) entity.H28 = dr.GetDecimal(iH28);

                    int iH29 = dr.GetOrdinal(helper.H29);
                    if (!dr.IsDBNull(iH29)) entity.H29 = dr.GetDecimal(iH29);

                    int iH30 = dr.GetOrdinal(helper.H30);
                    if (!dr.IsDBNull(iH30)) entity.H30 = dr.GetDecimal(iH30);

                    int iH31 = dr.GetOrdinal(helper.H31);
                    if (!dr.IsDBNull(iH31)) entity.H31 = dr.GetDecimal(iH31);

                    int iH32 = dr.GetOrdinal(helper.H32);
                    if (!dr.IsDBNull(iH32)) entity.H32 = dr.GetDecimal(iH32);

                    int iH33 = dr.GetOrdinal(helper.H33);
                    if (!dr.IsDBNull(iH33)) entity.H33 = dr.GetDecimal(iH33);

                    int iH34 = dr.GetOrdinal(helper.H34);
                    if (!dr.IsDBNull(iH34)) entity.H34 = dr.GetDecimal(iH34);

                    int iH35 = dr.GetOrdinal(helper.H35);
                    if (!dr.IsDBNull(iH35)) entity.H35 = dr.GetDecimal(iH35);

                    int iH36 = dr.GetOrdinal(helper.H36);
                    if (!dr.IsDBNull(iH36)) entity.H36 = dr.GetDecimal(iH36);

                    int iH37 = dr.GetOrdinal(helper.H37);
                    if (!dr.IsDBNull(iH37)) entity.H37 = dr.GetDecimal(iH37);

                    int iH38 = dr.GetOrdinal(helper.H38);
                    if (!dr.IsDBNull(iH38)) entity.H38 = dr.GetDecimal(iH38);

                    int iH39 = dr.GetOrdinal(helper.H39);
                    if (!dr.IsDBNull(iH39)) entity.H39 = dr.GetDecimal(iH39);

                    int iH40 = dr.GetOrdinal(helper.H40);
                    if (!dr.IsDBNull(iH40)) entity.H40 = dr.GetDecimal(iH40);

                    int iH41 = dr.GetOrdinal(helper.H41);
                    if (!dr.IsDBNull(iH41)) entity.H41 = dr.GetDecimal(iH41);

                    int iH42 = dr.GetOrdinal(helper.H42);
                    if (!dr.IsDBNull(iH42)) entity.H42 = dr.GetDecimal(iH42);

                    int iH43 = dr.GetOrdinal(helper.H43);
                    if (!dr.IsDBNull(iH43)) entity.H43 = dr.GetDecimal(iH43);

                    int iH44 = dr.GetOrdinal(helper.H44);
                    if (!dr.IsDBNull(iH44)) entity.H44 = dr.GetDecimal(iH44);

                    int iH45 = dr.GetOrdinal(helper.H45);
                    if (!dr.IsDBNull(iH45)) entity.H45 = dr.GetDecimal(iH45);

                    int iH46 = dr.GetOrdinal(helper.H46);
                    if (!dr.IsDBNull(iH46)) entity.H46 = dr.GetDecimal(iH46);

                    int iH47 = dr.GetOrdinal(helper.H47);
                    if (!dr.IsDBNull(iH47)) entity.H47 = dr.GetDecimal(iH47);

                    int iH48 = dr.GetOrdinal(helper.H48);
                    if (!dr.IsDBNull(iH48)) entity.H48 = dr.GetDecimal(iH48);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<PrnAgrupacionFormulasDTO> List()
        {
            PrnAgrupacionFormulasDTO entity = new PrnAgrupacionFormulasDTO();
            List<PrnAgrupacionFormulasDTO> entitys = new List<PrnAgrupacionFormulasDTO>();
            string query = string.Format(helper.SqlList);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new PrnAgrupacionFormulasDTO();

                    int iPrnafmcodi = dr.GetOrdinal(helper.Prnafmcodi);
                    if (!dr.IsDBNull(iPrnafmcodi)) entity.Prnafmcodi = Convert.ToInt32(dr.GetValue(iPrnafmcodi));

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iPrrucodi = dr.GetOrdinal(helper.Prrucodi);
                    if (!dr.IsDBNull(iPrrucodi)) entity.Prrucodi = Convert.ToInt32(dr.GetValue(iPrrucodi));

                    int iPrruabrev = dr.GetOrdinal(helper.Prruabrev);
                    if (!dr.IsDBNull(iPrruabrev)) entity.Prruabrev = dr.GetString(iPrruabrev);

                    int iPrnafmprioridad = dr.GetOrdinal(helper.Prnafmprioridad);
                    if (!dr.IsDBNull(iPrnafmprioridad)) entity.Prnafmprioridad = Convert.ToInt32(dr.GetValue(iPrnafmprioridad));

                    int iPrnafmflagesmanual = dr.GetOrdinal(helper.Prnafmflagesmanual);
                    if (!dr.IsDBNull(iPrnafmflagesmanual)) entity.Prnafmflagesmanual = dr.GetString(iPrnafmflagesmanual);

                    int iH1 = dr.GetOrdinal(helper.H1);
                    if (!dr.IsDBNull(iH1)) entity.H1 = dr.GetDecimal(iH1);

                    int iH2 = dr.GetOrdinal(helper.H2);
                    if (!dr.IsDBNull(iH2)) entity.H2 = dr.GetDecimal(iH2);

                    int iH3 = dr.GetOrdinal(helper.H3);
                    if (!dr.IsDBNull(iH3)) entity.H3 = dr.GetDecimal(iH3);

                    int iH4 = dr.GetOrdinal(helper.H4);
                    if (!dr.IsDBNull(iH4)) entity.H4 = dr.GetDecimal(iH4);

                    int iH5 = dr.GetOrdinal(helper.H5);
                    if (!dr.IsDBNull(iH5)) entity.H5 = dr.GetDecimal(iH5);

                    int iH6 = dr.GetOrdinal(helper.H6);
                    if (!dr.IsDBNull(iH6)) entity.H6 = dr.GetDecimal(iH6);

                    int iH7 = dr.GetOrdinal(helper.H7);
                    if (!dr.IsDBNull(iH7)) entity.H7 = dr.GetDecimal(iH7);

                    int iH8 = dr.GetOrdinal(helper.H8);
                    if (!dr.IsDBNull(iH8)) entity.H8 = dr.GetDecimal(iH8);

                    int iH9 = dr.GetOrdinal(helper.H9);
                    if (!dr.IsDBNull(iH9)) entity.H9 = dr.GetDecimal(iH9);

                    int iH10 = dr.GetOrdinal(helper.H10);
                    if (!dr.IsDBNull(iH10)) entity.H10 = dr.GetDecimal(iH10);

                    int iH11 = dr.GetOrdinal(helper.H11);
                    if (!dr.IsDBNull(iH11)) entity.H11 = dr.GetDecimal(iH11);

                    int iH12 = dr.GetOrdinal(helper.H12);
                    if (!dr.IsDBNull(iH12)) entity.H12 = dr.GetDecimal(iH12);

                    int iH13 = dr.GetOrdinal(helper.H13);
                    if (!dr.IsDBNull(iH13)) entity.H13 = dr.GetDecimal(iH13);

                    int iH14 = dr.GetOrdinal(helper.H14);
                    if (!dr.IsDBNull(iH14)) entity.H14 = dr.GetDecimal(iH14);

                    int iH15 = dr.GetOrdinal(helper.H15);
                    if (!dr.IsDBNull(iH15)) entity.H15 = dr.GetDecimal(iH15);

                    int iH16 = dr.GetOrdinal(helper.H16);
                    if (!dr.IsDBNull(iH16)) entity.H16 = dr.GetDecimal(iH16);

                    int iH17 = dr.GetOrdinal(helper.H17);
                    if (!dr.IsDBNull(iH17)) entity.H17 = dr.GetDecimal(iH17);

                    int iH18 = dr.GetOrdinal(helper.H18);
                    if (!dr.IsDBNull(iH18)) entity.H18 = dr.GetDecimal(iH18);

                    int iH19 = dr.GetOrdinal(helper.H19);
                    if (!dr.IsDBNull(iH19)) entity.H19 = dr.GetDecimal(iH19);

                    int iH20 = dr.GetOrdinal(helper.H20);
                    if (!dr.IsDBNull(iH20)) entity.H20 = dr.GetDecimal(iH20);

                    int iH21 = dr.GetOrdinal(helper.H21);
                    if (!dr.IsDBNull(iH21)) entity.H21 = dr.GetDecimal(iH21);

                    int iH22 = dr.GetOrdinal(helper.H22);
                    if (!dr.IsDBNull(iH22)) entity.H22 = dr.GetDecimal(iH22);

                    int iH23 = dr.GetOrdinal(helper.H23);
                    if (!dr.IsDBNull(iH23)) entity.H23 = dr.GetDecimal(iH23);

                    int iH24 = dr.GetOrdinal(helper.H24);
                    if (!dr.IsDBNull(iH24)) entity.H24 = dr.GetDecimal(iH24);

                    int iH25 = dr.GetOrdinal(helper.H25);
                    if (!dr.IsDBNull(iH25)) entity.H25 = dr.GetDecimal(iH25);

                    int iH26 = dr.GetOrdinal(helper.H26);
                    if (!dr.IsDBNull(iH26)) entity.H26 = dr.GetDecimal(iH26);

                    int iH27 = dr.GetOrdinal(helper.H27);
                    if (!dr.IsDBNull(iH27)) entity.H27 = dr.GetDecimal(iH27);

                    int iH28 = dr.GetOrdinal(helper.H28);
                    if (!dr.IsDBNull(iH28)) entity.H28 = dr.GetDecimal(iH28);

                    int iH29 = dr.GetOrdinal(helper.H29);
                    if (!dr.IsDBNull(iH29)) entity.H29 = dr.GetDecimal(iH29);

                    int iH30 = dr.GetOrdinal(helper.H30);
                    if (!dr.IsDBNull(iH30)) entity.H30 = dr.GetDecimal(iH30);

                    int iH31 = dr.GetOrdinal(helper.H31);
                    if (!dr.IsDBNull(iH31)) entity.H31 = dr.GetDecimal(iH31);

                    int iH32 = dr.GetOrdinal(helper.H32);
                    if (!dr.IsDBNull(iH32)) entity.H32 = dr.GetDecimal(iH32);

                    int iH33 = dr.GetOrdinal(helper.H33);
                    if (!dr.IsDBNull(iH33)) entity.H33 = dr.GetDecimal(iH33);

                    int iH34 = dr.GetOrdinal(helper.H34);
                    if (!dr.IsDBNull(iH34)) entity.H34 = dr.GetDecimal(iH34);

                    int iH35 = dr.GetOrdinal(helper.H35);
                    if (!dr.IsDBNull(iH35)) entity.H35 = dr.GetDecimal(iH35);

                    int iH36 = dr.GetOrdinal(helper.H36);
                    if (!dr.IsDBNull(iH36)) entity.H36 = dr.GetDecimal(iH36);

                    int iH37 = dr.GetOrdinal(helper.H37);
                    if (!dr.IsDBNull(iH37)) entity.H37 = dr.GetDecimal(iH37);

                    int iH38 = dr.GetOrdinal(helper.H38);
                    if (!dr.IsDBNull(iH38)) entity.H38 = dr.GetDecimal(iH38);

                    int iH39 = dr.GetOrdinal(helper.H39);
                    if (!dr.IsDBNull(iH39)) entity.H39 = dr.GetDecimal(iH39);

                    int iH40 = dr.GetOrdinal(helper.H40);
                    if (!dr.IsDBNull(iH40)) entity.H40 = dr.GetDecimal(iH40);

                    int iH41 = dr.GetOrdinal(helper.H41);
                    if (!dr.IsDBNull(iH41)) entity.H41 = dr.GetDecimal(iH41);

                    int iH42 = dr.GetOrdinal(helper.H42);
                    if (!dr.IsDBNull(iH42)) entity.H42 = dr.GetDecimal(iH42);

                    int iH43 = dr.GetOrdinal(helper.H43);
                    if (!dr.IsDBNull(iH43)) entity.H43 = dr.GetDecimal(iH43);

                    int iH44 = dr.GetOrdinal(helper.H44);
                    if (!dr.IsDBNull(iH44)) entity.H44 = dr.GetDecimal(iH44);

                    int iH45 = dr.GetOrdinal(helper.H45);
                    if (!dr.IsDBNull(iH45)) entity.H45 = dr.GetDecimal(iH45);

                    int iH46 = dr.GetOrdinal(helper.H46);
                    if (!dr.IsDBNull(iH46)) entity.H46 = dr.GetDecimal(iH46);

                    int iH47 = dr.GetOrdinal(helper.H47);
                    if (!dr.IsDBNull(iH47)) entity.H47 = dr.GetDecimal(iH47);

                    int iH48 = dr.GetOrdinal(helper.H48);
                    if (!dr.IsDBNull(iH48)) entity.H48 = dr.GetDecimal(iH48);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
