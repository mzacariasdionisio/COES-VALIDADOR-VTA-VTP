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
    public class DpoDemandaScoRepository : RepositoryBase, IDpoDemandaScoRepository
    {
        public DpoDemandaScoRepository(string strConn) : base(strConn)
        {
        }

        DpoDemandaScoHelper helper = new DpoDemandaScoHelper();

        public void Save(DpoDemandaScoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.Medifecha, DbType.DateTime, entity.Medifecha);
            dbProvider.AddInParameter(command, helper.Prnvarcodi, DbType.Int32, entity.Prnvarcodi);
            dbProvider.AddInParameter(command, helper.Meditotal, DbType.Decimal, entity.Meditotal);            
            dbProvider.AddInParameter(command, helper.h1, DbType.Decimal, entity.H1);
            dbProvider.AddInParameter(command, helper.h2, DbType.Decimal, entity.H2);
            dbProvider.AddInParameter(command, helper.h3, DbType.Decimal, entity.H3);
            dbProvider.AddInParameter(command, helper.h4, DbType.Decimal, entity.H4);
            dbProvider.AddInParameter(command, helper.h5, DbType.Decimal, entity.H5);
            dbProvider.AddInParameter(command, helper.h6, DbType.Decimal, entity.H6);
            dbProvider.AddInParameter(command, helper.h7, DbType.Decimal, entity.H7);
            dbProvider.AddInParameter(command, helper.h8, DbType.Decimal, entity.H8);
            dbProvider.AddInParameter(command, helper.h9, DbType.Decimal, entity.H9);
            dbProvider.AddInParameter(command, helper.h10, DbType.Decimal, entity.H10);
            dbProvider.AddInParameter(command, helper.h11, DbType.Decimal, entity.H11);
            dbProvider.AddInParameter(command, helper.h12, DbType.Decimal, entity.H12);
            dbProvider.AddInParameter(command, helper.h13, DbType.Decimal, entity.H13);
            dbProvider.AddInParameter(command, helper.h14, DbType.Decimal, entity.H14);
            dbProvider.AddInParameter(command, helper.h15, DbType.Decimal, entity.H15);
            dbProvider.AddInParameter(command, helper.h16, DbType.Decimal, entity.H16);
            dbProvider.AddInParameter(command, helper.h17, DbType.Decimal, entity.H17);
            dbProvider.AddInParameter(command, helper.h18, DbType.Decimal, entity.H18);
            dbProvider.AddInParameter(command, helper.h19, DbType.Decimal, entity.H19);
            dbProvider.AddInParameter(command, helper.h20, DbType.Decimal, entity.H20);
            dbProvider.AddInParameter(command, helper.h21, DbType.Decimal, entity.H21);
            dbProvider.AddInParameter(command, helper.h22, DbType.Decimal, entity.H22);
            dbProvider.AddInParameter(command, helper.h23, DbType.Decimal, entity.H23);
            dbProvider.AddInParameter(command, helper.h24, DbType.Decimal, entity.H24);
            dbProvider.AddInParameter(command, helper.h25, DbType.Decimal, entity.H25);
            dbProvider.AddInParameter(command, helper.h26, DbType.Decimal, entity.H26);
            dbProvider.AddInParameter(command, helper.h27, DbType.Decimal, entity.H27);
            dbProvider.AddInParameter(command, helper.h28, DbType.Decimal, entity.H28);
            dbProvider.AddInParameter(command, helper.h29, DbType.Decimal, entity.H29);
            dbProvider.AddInParameter(command, helper.h30, DbType.Decimal, entity.H30);
            dbProvider.AddInParameter(command, helper.h31, DbType.Decimal, entity.H31);
            dbProvider.AddInParameter(command, helper.h32, DbType.Decimal, entity.H32);
            dbProvider.AddInParameter(command, helper.h33, DbType.Decimal, entity.H33);
            dbProvider.AddInParameter(command, helper.h34, DbType.Decimal, entity.H34);
            dbProvider.AddInParameter(command, helper.h35, DbType.Decimal, entity.H35);
            dbProvider.AddInParameter(command, helper.h36, DbType.Decimal, entity.H36);
            dbProvider.AddInParameter(command, helper.h37, DbType.Decimal, entity.H37);
            dbProvider.AddInParameter(command, helper.h38, DbType.Decimal, entity.H38);
            dbProvider.AddInParameter(command, helper.h39, DbType.Decimal, entity.H39);
            dbProvider.AddInParameter(command, helper.h40, DbType.Decimal, entity.H40);
            dbProvider.AddInParameter(command, helper.h41, DbType.Decimal, entity.H41);
            dbProvider.AddInParameter(command, helper.h42, DbType.Decimal, entity.H42);
            dbProvider.AddInParameter(command, helper.h43, DbType.Decimal, entity.H43);
            dbProvider.AddInParameter(command, helper.h44, DbType.Decimal, entity.H44);
            dbProvider.AddInParameter(command, helper.h45, DbType.Decimal, entity.H45);
            dbProvider.AddInParameter(command, helper.h46, DbType.Decimal, entity.H46);
            dbProvider.AddInParameter(command, helper.h47, DbType.Decimal, entity.H47);
            dbProvider.AddInParameter(command, helper.h48, DbType.Decimal, entity.H48);
            dbProvider.AddInParameter(command, helper.h49, DbType.Decimal, entity.H49);
            dbProvider.AddInParameter(command, helper.h50, DbType.Decimal, entity.H50);
            dbProvider.AddInParameter(command, helper.h51, DbType.Decimal, entity.H51);
            dbProvider.AddInParameter(command, helper.h52, DbType.Decimal, entity.H52);
            dbProvider.AddInParameter(command, helper.h53, DbType.Decimal, entity.H53);
            dbProvider.AddInParameter(command, helper.h54, DbType.Decimal, entity.H54);
            dbProvider.AddInParameter(command, helper.h55, DbType.Decimal, entity.H55);
            dbProvider.AddInParameter(command, helper.h56, DbType.Decimal, entity.H56);
            dbProvider.AddInParameter(command, helper.h57, DbType.Decimal, entity.H57);
            dbProvider.AddInParameter(command, helper.h58, DbType.Decimal, entity.H58);
            dbProvider.AddInParameter(command, helper.h59, DbType.Decimal, entity.H59);
            dbProvider.AddInParameter(command, helper.h60, DbType.Decimal, entity.H60);
            dbProvider.AddInParameter(command, helper.h61, DbType.Decimal, entity.H61);
            dbProvider.AddInParameter(command, helper.h62, DbType.Decimal, entity.H62);
            dbProvider.AddInParameter(command, helper.h63, DbType.Decimal, entity.H63);
            dbProvider.AddInParameter(command, helper.h64, DbType.Decimal, entity.H64);
            dbProvider.AddInParameter(command, helper.h65, DbType.Decimal, entity.H65);
            dbProvider.AddInParameter(command, helper.h66, DbType.Decimal, entity.H66);
            dbProvider.AddInParameter(command, helper.h67, DbType.Decimal, entity.H67);
            dbProvider.AddInParameter(command, helper.h68, DbType.Decimal, entity.H68);
            dbProvider.AddInParameter(command, helper.h69, DbType.Decimal, entity.H69);
            dbProvider.AddInParameter(command, helper.h70, DbType.Decimal, entity.H70);
            dbProvider.AddInParameter(command, helper.h71, DbType.Decimal, entity.H71);
            dbProvider.AddInParameter(command, helper.h72, DbType.Decimal, entity.H72);
            dbProvider.AddInParameter(command, helper.h73, DbType.Decimal, entity.H73);
            dbProvider.AddInParameter(command, helper.h74, DbType.Decimal, entity.H74);
            dbProvider.AddInParameter(command, helper.h75, DbType.Decimal, entity.H75);
            dbProvider.AddInParameter(command, helper.h76, DbType.Decimal, entity.H76);
            dbProvider.AddInParameter(command, helper.h77, DbType.Decimal, entity.H77);
            dbProvider.AddInParameter(command, helper.h78, DbType.Decimal, entity.H78);
            dbProvider.AddInParameter(command, helper.h79, DbType.Decimal, entity.H79);
            dbProvider.AddInParameter(command, helper.h80, DbType.Decimal, entity.H80);
            dbProvider.AddInParameter(command, helper.h81, DbType.Decimal, entity.H81);
            dbProvider.AddInParameter(command, helper.h82, DbType.Decimal, entity.H82);
            dbProvider.AddInParameter(command, helper.h83, DbType.Decimal, entity.H83);
            dbProvider.AddInParameter(command, helper.h84, DbType.Decimal, entity.H84);
            dbProvider.AddInParameter(command, helper.h85, DbType.Decimal, entity.H85);
            dbProvider.AddInParameter(command, helper.h86, DbType.Decimal, entity.H86);
            dbProvider.AddInParameter(command, helper.h87, DbType.Decimal, entity.H87);
            dbProvider.AddInParameter(command, helper.h88, DbType.Decimal, entity.H88);
            dbProvider.AddInParameter(command, helper.h89, DbType.Decimal, entity.H89);
            dbProvider.AddInParameter(command, helper.h90, DbType.Decimal, entity.H90);
            dbProvider.AddInParameter(command, helper.h91, DbType.Decimal, entity.H91);
            dbProvider.AddInParameter(command, helper.h92, DbType.Decimal, entity.H92);
            dbProvider.AddInParameter(command, helper.h93, DbType.Decimal, entity.H93);
            dbProvider.AddInParameter(command, helper.h94, DbType.Decimal, entity.H94);
            dbProvider.AddInParameter(command, helper.h95, DbType.Decimal, entity.H95);
            dbProvider.AddInParameter(command, helper.h96, DbType.Decimal, entity.H96);            
            dbProvider.AddInParameter(command, helper.Demscofeccreacion, DbType.DateTime, entity.Demscofeccreacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(DpoDemandaScoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Meditotal, DbType.Decimal, entity.Meditotal);
            dbProvider.AddInParameter(command, helper.h1, DbType.Decimal, entity.H1);
            dbProvider.AddInParameter(command, helper.h2, DbType.Decimal, entity.H2);
            dbProvider.AddInParameter(command, helper.h3, DbType.Decimal, entity.H3);
            dbProvider.AddInParameter(command, helper.h4, DbType.Decimal, entity.H4);
            dbProvider.AddInParameter(command, helper.h5, DbType.Decimal, entity.H5);
            dbProvider.AddInParameter(command, helper.h6, DbType.Decimal, entity.H6);
            dbProvider.AddInParameter(command, helper.h7, DbType.Decimal, entity.H7);
            dbProvider.AddInParameter(command, helper.h8, DbType.Decimal, entity.H8);
            dbProvider.AddInParameter(command, helper.h9, DbType.Decimal, entity.H9);
            dbProvider.AddInParameter(command, helper.h10, DbType.Decimal, entity.H10);
            dbProvider.AddInParameter(command, helper.h11, DbType.Decimal, entity.H11);
            dbProvider.AddInParameter(command, helper.h12, DbType.Decimal, entity.H12);
            dbProvider.AddInParameter(command, helper.h13, DbType.Decimal, entity.H13);
            dbProvider.AddInParameter(command, helper.h14, DbType.Decimal, entity.H14);
            dbProvider.AddInParameter(command, helper.h15, DbType.Decimal, entity.H15);
            dbProvider.AddInParameter(command, helper.h16, DbType.Decimal, entity.H16);
            dbProvider.AddInParameter(command, helper.h17, DbType.Decimal, entity.H17);
            dbProvider.AddInParameter(command, helper.h18, DbType.Decimal, entity.H18);
            dbProvider.AddInParameter(command, helper.h19, DbType.Decimal, entity.H19);
            dbProvider.AddInParameter(command, helper.h20, DbType.Decimal, entity.H20);
            dbProvider.AddInParameter(command, helper.h21, DbType.Decimal, entity.H21);
            dbProvider.AddInParameter(command, helper.h22, DbType.Decimal, entity.H22);
            dbProvider.AddInParameter(command, helper.h23, DbType.Decimal, entity.H23);
            dbProvider.AddInParameter(command, helper.h24, DbType.Decimal, entity.H24);
            dbProvider.AddInParameter(command, helper.h25, DbType.Decimal, entity.H25);
            dbProvider.AddInParameter(command, helper.h26, DbType.Decimal, entity.H26);
            dbProvider.AddInParameter(command, helper.h27, DbType.Decimal, entity.H27);
            dbProvider.AddInParameter(command, helper.h28, DbType.Decimal, entity.H28);
            dbProvider.AddInParameter(command, helper.h29, DbType.Decimal, entity.H29);
            dbProvider.AddInParameter(command, helper.h30, DbType.Decimal, entity.H30);
            dbProvider.AddInParameter(command, helper.h31, DbType.Decimal, entity.H31);
            dbProvider.AddInParameter(command, helper.h32, DbType.Decimal, entity.H32);
            dbProvider.AddInParameter(command, helper.h33, DbType.Decimal, entity.H33);
            dbProvider.AddInParameter(command, helper.h34, DbType.Decimal, entity.H34);
            dbProvider.AddInParameter(command, helper.h35, DbType.Decimal, entity.H35);
            dbProvider.AddInParameter(command, helper.h36, DbType.Decimal, entity.H36);
            dbProvider.AddInParameter(command, helper.h37, DbType.Decimal, entity.H37);
            dbProvider.AddInParameter(command, helper.h38, DbType.Decimal, entity.H38);
            dbProvider.AddInParameter(command, helper.h39, DbType.Decimal, entity.H39);
            dbProvider.AddInParameter(command, helper.h40, DbType.Decimal, entity.H40);
            dbProvider.AddInParameter(command, helper.h41, DbType.Decimal, entity.H41);
            dbProvider.AddInParameter(command, helper.h42, DbType.Decimal, entity.H42);
            dbProvider.AddInParameter(command, helper.h43, DbType.Decimal, entity.H43);
            dbProvider.AddInParameter(command, helper.h44, DbType.Decimal, entity.H44);
            dbProvider.AddInParameter(command, helper.h45, DbType.Decimal, entity.H45);
            dbProvider.AddInParameter(command, helper.h46, DbType.Decimal, entity.H46);
            dbProvider.AddInParameter(command, helper.h47, DbType.Decimal, entity.H47);
            dbProvider.AddInParameter(command, helper.h48, DbType.Decimal, entity.H48);
            dbProvider.AddInParameter(command, helper.h49, DbType.Decimal, entity.H49);
            dbProvider.AddInParameter(command, helper.h50, DbType.Decimal, entity.H50);
            dbProvider.AddInParameter(command, helper.h51, DbType.Decimal, entity.H51);
            dbProvider.AddInParameter(command, helper.h52, DbType.Decimal, entity.H52);
            dbProvider.AddInParameter(command, helper.h53, DbType.Decimal, entity.H53);
            dbProvider.AddInParameter(command, helper.h54, DbType.Decimal, entity.H54);
            dbProvider.AddInParameter(command, helper.h55, DbType.Decimal, entity.H55);
            dbProvider.AddInParameter(command, helper.h56, DbType.Decimal, entity.H56);
            dbProvider.AddInParameter(command, helper.h57, DbType.Decimal, entity.H57);
            dbProvider.AddInParameter(command, helper.h58, DbType.Decimal, entity.H58);
            dbProvider.AddInParameter(command, helper.h59, DbType.Decimal, entity.H59);
            dbProvider.AddInParameter(command, helper.h60, DbType.Decimal, entity.H60);
            dbProvider.AddInParameter(command, helper.h61, DbType.Decimal, entity.H61);
            dbProvider.AddInParameter(command, helper.h62, DbType.Decimal, entity.H62);
            dbProvider.AddInParameter(command, helper.h63, DbType.Decimal, entity.H63);
            dbProvider.AddInParameter(command, helper.h64, DbType.Decimal, entity.H64);
            dbProvider.AddInParameter(command, helper.h65, DbType.Decimal, entity.H65);
            dbProvider.AddInParameter(command, helper.h66, DbType.Decimal, entity.H66);
            dbProvider.AddInParameter(command, helper.h67, DbType.Decimal, entity.H67);
            dbProvider.AddInParameter(command, helper.h68, DbType.Decimal, entity.H68);
            dbProvider.AddInParameter(command, helper.h69, DbType.Decimal, entity.H69);
            dbProvider.AddInParameter(command, helper.h70, DbType.Decimal, entity.H70);
            dbProvider.AddInParameter(command, helper.h71, DbType.Decimal, entity.H71);
            dbProvider.AddInParameter(command, helper.h72, DbType.Decimal, entity.H72);
            dbProvider.AddInParameter(command, helper.h73, DbType.Decimal, entity.H73);
            dbProvider.AddInParameter(command, helper.h74, DbType.Decimal, entity.H74);
            dbProvider.AddInParameter(command, helper.h75, DbType.Decimal, entity.H75);
            dbProvider.AddInParameter(command, helper.h76, DbType.Decimal, entity.H76);
            dbProvider.AddInParameter(command, helper.h77, DbType.Decimal, entity.H77);
            dbProvider.AddInParameter(command, helper.h78, DbType.Decimal, entity.H78);
            dbProvider.AddInParameter(command, helper.h79, DbType.Decimal, entity.H79);
            dbProvider.AddInParameter(command, helper.h80, DbType.Decimal, entity.H80);
            dbProvider.AddInParameter(command, helper.h81, DbType.Decimal, entity.H81);
            dbProvider.AddInParameter(command, helper.h82, DbType.Decimal, entity.H82);
            dbProvider.AddInParameter(command, helper.h83, DbType.Decimal, entity.H83);
            dbProvider.AddInParameter(command, helper.h84, DbType.Decimal, entity.H84);
            dbProvider.AddInParameter(command, helper.h85, DbType.Decimal, entity.H85);
            dbProvider.AddInParameter(command, helper.h86, DbType.Decimal, entity.H86);
            dbProvider.AddInParameter(command, helper.h87, DbType.Decimal, entity.H87);
            dbProvider.AddInParameter(command, helper.h88, DbType.Decimal, entity.H88);
            dbProvider.AddInParameter(command, helper.h89, DbType.Decimal, entity.H89);
            dbProvider.AddInParameter(command, helper.h90, DbType.Decimal, entity.H90);
            dbProvider.AddInParameter(command, helper.h91, DbType.Decimal, entity.H91);
            dbProvider.AddInParameter(command, helper.h92, DbType.Decimal, entity.H92);
            dbProvider.AddInParameter(command, helper.h93, DbType.Decimal, entity.H93);
            dbProvider.AddInParameter(command, helper.h94, DbType.Decimal, entity.H94);
            dbProvider.AddInParameter(command, helper.h95, DbType.Decimal, entity.H95);
            dbProvider.AddInParameter(command, helper.h96, DbType.Decimal, entity.H96);            
            dbProvider.AddInParameter(command, helper.Demscofeccreacion, DbType.DateTime, entity.Demscofeccreacion);
            
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.Medifecha, DbType.DateTime, entity.Medifecha);
            dbProvider.AddInParameter(command, helper.Prnvarcodi, DbType.Int32, entity.Prnvarcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int ptomedicodi, DateTime medifecha, int prnvarcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, ptomedicodi);
            dbProvider.AddInParameter(command, helper.Medifecha, DbType.DateTime, medifecha);
            dbProvider.AddInParameter(command, helper.Prnvarcodi, DbType.Int32, prnvarcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public List<DpoDemandaScoDTO> List()
        {
            List<DpoDemandaScoDTO> entitys = new List<DpoDemandaScoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    DpoDemandaScoDTO entity = new DpoDemandaScoDTO();

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iMedifecha = dr.GetOrdinal(helper.Medifecha);
                    if (!dr.IsDBNull(iMedifecha)) entity.Medifecha = dr.GetDateTime(iMedifecha);

                    int iPrnvarcodi = dr.GetOrdinal(helper.Prnvarcodi);
                    if (!dr.IsDBNull(iPrnvarcodi)) entity.Prnvarcodi = Convert.ToInt32(dr.GetValue(iPrnvarcodi));

                    int iMeditotal = dr.GetOrdinal(helper.Meditotal);
                    if (!dr.IsDBNull(iMeditotal)) entity.Meditotal = dr.GetDecimal(iMeditotal);

                    int ih1 = dr.GetOrdinal(helper.h1);
                    if (!dr.IsDBNull(ih1)) entity.H1 = dr.GetDecimal(ih1);

                    int ih2 = dr.GetOrdinal(helper.h2);
                    if (!dr.IsDBNull(ih2)) entity.H2 = dr.GetDecimal(ih2);

                    int ih3 = dr.GetOrdinal(helper.h3);
                    if (!dr.IsDBNull(ih3)) entity.H3 = dr.GetDecimal(ih3);

                    int ih4 = dr.GetOrdinal(helper.h4);
                    if (!dr.IsDBNull(ih4)) entity.H4 = dr.GetDecimal(ih4);

                    int ih5 = dr.GetOrdinal(helper.h5);
                    if (!dr.IsDBNull(ih5)) entity.H5 = dr.GetDecimal(ih5);

                    int ih6 = dr.GetOrdinal(helper.h6);
                    if (!dr.IsDBNull(ih6)) entity.H6 = dr.GetDecimal(ih6);

                    int ih7 = dr.GetOrdinal(helper.h7);
                    if (!dr.IsDBNull(ih7)) entity.H7 = dr.GetDecimal(ih7);

                    int ih8 = dr.GetOrdinal(helper.h8);
                    if (!dr.IsDBNull(ih8)) entity.H8 = dr.GetDecimal(ih8);

                    int ih9 = dr.GetOrdinal(helper.h9);
                    if (!dr.IsDBNull(ih9)) entity.H9 = dr.GetDecimal(ih9);

                    int ih10 = dr.GetOrdinal(helper.h10);
                    if (!dr.IsDBNull(ih10)) entity.H10 = dr.GetDecimal(ih10);

                    int ih11 = dr.GetOrdinal(helper.h11);
                    if (!dr.IsDBNull(ih11)) entity.H11 = dr.GetDecimal(ih11);

                    int ih12 = dr.GetOrdinal(helper.h12);
                    if (!dr.IsDBNull(ih12)) entity.H12 = dr.GetDecimal(ih12);

                    int ih13 = dr.GetOrdinal(helper.h13);
                    if (!dr.IsDBNull(ih13)) entity.H13 = dr.GetDecimal(ih13);

                    int ih14 = dr.GetOrdinal(helper.h14);
                    if (!dr.IsDBNull(ih14)) entity.H14 = dr.GetDecimal(ih14);

                    int ih15 = dr.GetOrdinal(helper.h15);
                    if (!dr.IsDBNull(ih15)) entity.H15 = dr.GetDecimal(ih15);

                    int ih16 = dr.GetOrdinal(helper.h16);
                    if (!dr.IsDBNull(ih16)) entity.H16 = dr.GetDecimal(ih16);

                    int ih17 = dr.GetOrdinal(helper.h17);
                    if (!dr.IsDBNull(ih17)) entity.H17 = dr.GetDecimal(ih17);

                    int ih18 = dr.GetOrdinal(helper.h18);
                    if (!dr.IsDBNull(ih18)) entity.H18 = dr.GetDecimal(ih18);

                    int ih19 = dr.GetOrdinal(helper.h19);
                    if (!dr.IsDBNull(ih19)) entity.H19 = dr.GetDecimal(ih19);

                    int ih20 = dr.GetOrdinal(helper.h20);
                    if (!dr.IsDBNull(ih20)) entity.H20 = dr.GetDecimal(ih20);

                    int ih21 = dr.GetOrdinal(helper.h21);
                    if (!dr.IsDBNull(ih21)) entity.H21 = dr.GetDecimal(ih21);

                    int ih22 = dr.GetOrdinal(helper.h22);
                    if (!dr.IsDBNull(ih22)) entity.H22 = dr.GetDecimal(ih22);

                    int ih23 = dr.GetOrdinal(helper.h23);
                    if (!dr.IsDBNull(ih23)) entity.H23 = dr.GetDecimal(ih23);

                    int ih24 = dr.GetOrdinal(helper.h24);
                    if (!dr.IsDBNull(ih24)) entity.H24 = dr.GetDecimal(ih24);

                    int ih25 = dr.GetOrdinal(helper.h25);
                    if (!dr.IsDBNull(ih25)) entity.H25 = dr.GetDecimal(ih25);

                    int ih26 = dr.GetOrdinal(helper.h26);
                    if (!dr.IsDBNull(ih26)) entity.H26 = dr.GetDecimal(ih26);

                    int ih27 = dr.GetOrdinal(helper.h27);
                    if (!dr.IsDBNull(ih27)) entity.H27 = dr.GetDecimal(ih27);

                    int ih28 = dr.GetOrdinal(helper.h28);
                    if (!dr.IsDBNull(ih28)) entity.H28 = dr.GetDecimal(ih28);

                    int ih29 = dr.GetOrdinal(helper.h29);
                    if (!dr.IsDBNull(ih29)) entity.H29 = dr.GetDecimal(ih29);

                    int ih30 = dr.GetOrdinal(helper.h30);
                    if (!dr.IsDBNull(ih30)) entity.H30 = dr.GetDecimal(ih30);

                    int ih31 = dr.GetOrdinal(helper.h31);
                    if (!dr.IsDBNull(ih31)) entity.H31 = dr.GetDecimal(ih31);

                    int ih32 = dr.GetOrdinal(helper.h32);
                    if (!dr.IsDBNull(ih32)) entity.H32 = dr.GetDecimal(ih32);

                    int ih33 = dr.GetOrdinal(helper.h33);
                    if (!dr.IsDBNull(ih33)) entity.H33 = dr.GetDecimal(ih33);

                    int ih34 = dr.GetOrdinal(helper.h34);
                    if (!dr.IsDBNull(ih34)) entity.H34 = dr.GetDecimal(ih34);

                    int ih35 = dr.GetOrdinal(helper.h35);
                    if (!dr.IsDBNull(ih35)) entity.H35 = dr.GetDecimal(ih35);

                    int ih36 = dr.GetOrdinal(helper.h36);
                    if (!dr.IsDBNull(ih36)) entity.H36 = dr.GetDecimal(ih36);

                    int ih37 = dr.GetOrdinal(helper.h37);
                    if (!dr.IsDBNull(ih37)) entity.H37 = dr.GetDecimal(ih37);

                    int ih38 = dr.GetOrdinal(helper.h38);
                    if (!dr.IsDBNull(ih38)) entity.H38 = dr.GetDecimal(ih38);

                    int ih39 = dr.GetOrdinal(helper.h39);
                    if (!dr.IsDBNull(ih39)) entity.H39 = dr.GetDecimal(ih39);

                    int ih40 = dr.GetOrdinal(helper.h40);
                    if (!dr.IsDBNull(ih40)) entity.H40 = dr.GetDecimal(ih40);

                    int ih41 = dr.GetOrdinal(helper.h41);
                    if (!dr.IsDBNull(ih41)) entity.H41 = dr.GetDecimal(ih41);

                    int ih42 = dr.GetOrdinal(helper.h42);
                    if (!dr.IsDBNull(ih42)) entity.H42 = dr.GetDecimal(ih42);

                    int ih43 = dr.GetOrdinal(helper.h43);
                    if (!dr.IsDBNull(ih43)) entity.H43 = dr.GetDecimal(ih43);

                    int ih44 = dr.GetOrdinal(helper.h44);
                    if (!dr.IsDBNull(ih44)) entity.H44 = dr.GetDecimal(ih44);

                    int ih45 = dr.GetOrdinal(helper.h45);
                    if (!dr.IsDBNull(ih45)) entity.H45 = dr.GetDecimal(ih45);

                    int ih46 = dr.GetOrdinal(helper.h46);
                    if (!dr.IsDBNull(ih46)) entity.H46 = dr.GetDecimal(ih46);

                    int ih47 = dr.GetOrdinal(helper.h47);
                    if (!dr.IsDBNull(ih47)) entity.H47 = dr.GetDecimal(ih47);

                    int ih48 = dr.GetOrdinal(helper.h48);
                    if (!dr.IsDBNull(ih48)) entity.H48 = dr.GetDecimal(ih48);

                    int ih49 = dr.GetOrdinal(helper.h49);
                    if (!dr.IsDBNull(ih49)) entity.H49 = dr.GetDecimal(ih49);

                    int ih50 = dr.GetOrdinal(helper.h50);
                    if (!dr.IsDBNull(ih50)) entity.H50 = dr.GetDecimal(ih50);

                    int ih51 = dr.GetOrdinal(helper.h51);
                    if (!dr.IsDBNull(ih51)) entity.H51 = dr.GetDecimal(ih51);

                    int ih52 = dr.GetOrdinal(helper.h52);
                    if (!dr.IsDBNull(ih50)) entity.H52 = dr.GetDecimal(ih52);

                    int ih53 = dr.GetOrdinal(helper.h53);
                    if (!dr.IsDBNull(ih53)) entity.H53 = dr.GetDecimal(ih53);

                    int ih54 = dr.GetOrdinal(helper.h54);
                    if (!dr.IsDBNull(ih54)) entity.H54 = dr.GetDecimal(ih54);

                    int ih55 = dr.GetOrdinal(helper.h55);
                    if (!dr.IsDBNull(ih55)) entity.H55 = dr.GetDecimal(ih55);

                    int ih56 = dr.GetOrdinal(helper.h56);
                    if (!dr.IsDBNull(ih56)) entity.H56 = dr.GetDecimal(ih56);

                    int ih57 = dr.GetOrdinal(helper.h57);
                    if (!dr.IsDBNull(ih57)) entity.H57 = dr.GetDecimal(ih57);

                    int ih58 = dr.GetOrdinal(helper.h58);
                    if (!dr.IsDBNull(ih58)) entity.H58 = dr.GetDecimal(ih58);

                    int ih59 = dr.GetOrdinal(helper.h59);
                    if (!dr.IsDBNull(ih59)) entity.H59 = dr.GetDecimal(ih59);

                    int ih60 = dr.GetOrdinal(helper.h60);
                    if (!dr.IsDBNull(ih60)) entity.H60 = dr.GetDecimal(ih60);

                    int ih61 = dr.GetOrdinal(helper.h61);
                    if (!dr.IsDBNull(ih61)) entity.H61 = dr.GetDecimal(ih61);

                    int ih62 = dr.GetOrdinal(helper.h62);
                    if (!dr.IsDBNull(ih62)) entity.H62 = dr.GetDecimal(ih62);

                    int ih63 = dr.GetOrdinal(helper.h63);
                    if (!dr.IsDBNull(ih63)) entity.H63 = dr.GetDecimal(ih63);

                    int ih64 = dr.GetOrdinal(helper.h64);
                    if (!dr.IsDBNull(ih64)) entity.H64 = dr.GetDecimal(ih64);

                    int ih65 = dr.GetOrdinal(helper.h65);
                    if (!dr.IsDBNull(ih65)) entity.H65 = dr.GetDecimal(ih65);

                    int ih66 = dr.GetOrdinal(helper.h66);
                    if (!dr.IsDBNull(ih66)) entity.H66 = dr.GetDecimal(ih66);

                    int ih67 = dr.GetOrdinal(helper.h67);
                    if (!dr.IsDBNull(ih67)) entity.H67 = dr.GetDecimal(ih67);

                    int ih68 = dr.GetOrdinal(helper.h68);
                    if (!dr.IsDBNull(ih68)) entity.H68 = dr.GetDecimal(ih68);

                    int ih69 = dr.GetOrdinal(helper.h69);
                    if (!dr.IsDBNull(ih69)) entity.H69 = dr.GetDecimal(ih69);

                    int ih70 = dr.GetOrdinal(helper.h70);
                    if (!dr.IsDBNull(ih70)) entity.H70 = dr.GetDecimal(ih70);

                    int ih71 = dr.GetOrdinal(helper.h71);
                    if (!dr.IsDBNull(ih71)) entity.H71 = dr.GetDecimal(ih71);

                    int ih72 = dr.GetOrdinal(helper.h72);
                    if (!dr.IsDBNull(ih72)) entity.H72 = dr.GetDecimal(ih72);

                    int ih73 = dr.GetOrdinal(helper.h73);
                    if (!dr.IsDBNull(ih73)) entity.H73 = dr.GetDecimal(ih73);

                    int ih74 = dr.GetOrdinal(helper.h74);
                    if (!dr.IsDBNull(ih74)) entity.H74 = dr.GetDecimal(ih74);

                    int ih75 = dr.GetOrdinal(helper.h75);
                    if (!dr.IsDBNull(ih75)) entity.H75 = dr.GetDecimal(ih75);

                    int ih76 = dr.GetOrdinal(helper.h76);
                    if (!dr.IsDBNull(ih76)) entity.H76 = dr.GetDecimal(ih76);

                    int ih77 = dr.GetOrdinal(helper.h77);
                    if (!dr.IsDBNull(ih77)) entity.H77 = dr.GetDecimal(ih77);

                    int ih78 = dr.GetOrdinal(helper.h78);
                    if (!dr.IsDBNull(ih78)) entity.H78 = dr.GetDecimal(ih78);

                    int ih79 = dr.GetOrdinal(helper.h79);
                    if (!dr.IsDBNull(ih79)) entity.H79 = dr.GetDecimal(ih79);

                    int ih80 = dr.GetOrdinal(helper.h80);
                    if (!dr.IsDBNull(ih80)) entity.H80 = dr.GetDecimal(ih80);

                    int ih81 = dr.GetOrdinal(helper.h81);
                    if (!dr.IsDBNull(ih81)) entity.H81 = dr.GetDecimal(ih81);

                    int ih82 = dr.GetOrdinal(helper.h82);
                    if (!dr.IsDBNull(ih82)) entity.H82 = dr.GetDecimal(ih82);

                    int ih83 = dr.GetOrdinal(helper.h83);
                    if (!dr.IsDBNull(ih83)) entity.H83 = dr.GetDecimal(ih83);

                    int ih84 = dr.GetOrdinal(helper.h84);
                    if (!dr.IsDBNull(ih84)) entity.H84 = dr.GetDecimal(ih84);

                    int ih85 = dr.GetOrdinal(helper.h85);
                    if (!dr.IsDBNull(ih85)) entity.H85 = dr.GetDecimal(ih85);

                    int ih86 = dr.GetOrdinal(helper.h86);
                    if (!dr.IsDBNull(ih86)) entity.H86 = dr.GetDecimal(ih86);

                    int ih87 = dr.GetOrdinal(helper.h87);
                    if (!dr.IsDBNull(ih87)) entity.H87 = dr.GetDecimal(ih87);

                    int ih88 = dr.GetOrdinal(helper.h88);
                    if (!dr.IsDBNull(ih88)) entity.H88 = dr.GetDecimal(ih88);

                    int ih89 = dr.GetOrdinal(helper.h89);
                    if (!dr.IsDBNull(ih89)) entity.H89 = dr.GetDecimal(ih89);

                    int ih90 = dr.GetOrdinal(helper.h90);
                    if (!dr.IsDBNull(ih90)) entity.H90 = dr.GetDecimal(ih90);

                    int ih91 = dr.GetOrdinal(helper.h91);
                    if (!dr.IsDBNull(ih91)) entity.H91 = dr.GetDecimal(ih91);

                    int ih92 = dr.GetOrdinal(helper.h92);
                    if (!dr.IsDBNull(ih92)) entity.H92 = dr.GetDecimal(ih92);

                    int ih93 = dr.GetOrdinal(helper.h93);
                    if (!dr.IsDBNull(ih93)) entity.H93 = dr.GetDecimal(ih93);

                    int ih94 = dr.GetOrdinal(helper.h94);
                    if (!dr.IsDBNull(ih94)) entity.H94 = dr.GetDecimal(ih94);

                    int ih95 = dr.GetOrdinal(helper.h95);
                    if (!dr.IsDBNull(ih95)) entity.H95 = dr.GetDecimal(ih95);

                    int ih96 = dr.GetOrdinal(helper.h96);
                    if (!dr.IsDBNull(ih96)) entity.H96 = dr.GetDecimal(ih96);

                    int iDemscofeccreacion = dr.GetOrdinal(helper.Demscofeccreacion);
                    if (!dr.IsDBNull(iDemscofeccreacion)) entity.Demscofeccreacion = dr.GetDateTime(iDemscofeccreacion);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<DpoDemandaScoDTO> ListGroupByMonthYear(string anio, string mes, string cargas, string tipo)
        {
            List<DpoDemandaScoDTO> entitys = new List<DpoDemandaScoDTO>();
            
            string query = string.Format(helper.SqlListGroupByMonthYear, anio, mes, cargas, tipo);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    DpoDemandaScoDTO entity = new DpoDemandaScoDTO();

                    int iMedifecha = dr.GetOrdinal(helper.Medifecha);
                    if (!dr.IsDBNull(iMedifecha)) entity.Medifecha = dr.GetDateTime(iMedifecha);

                    int ih1 = dr.GetOrdinal(helper.h1);
                    if (!dr.IsDBNull(ih1)) entity.H1 = dr.GetDecimal(ih1);

                    int ih2 = dr.GetOrdinal(helper.h2);
                    if (!dr.IsDBNull(ih2)) entity.H2 = dr.GetDecimal(ih2);

                    int ih3 = dr.GetOrdinal(helper.h3);
                    if (!dr.IsDBNull(ih3)) entity.H3 = dr.GetDecimal(ih3);

                    int ih4 = dr.GetOrdinal(helper.h4);
                    if (!dr.IsDBNull(ih4)) entity.H4 = dr.GetDecimal(ih4);

                    int ih5 = dr.GetOrdinal(helper.h5);
                    if (!dr.IsDBNull(ih5)) entity.H5 = dr.GetDecimal(ih5);

                    int ih6 = dr.GetOrdinal(helper.h6);
                    if (!dr.IsDBNull(ih6)) entity.H6 = dr.GetDecimal(ih6);

                    int ih7 = dr.GetOrdinal(helper.h7);
                    if (!dr.IsDBNull(ih7)) entity.H7 = dr.GetDecimal(ih7);

                    int ih8 = dr.GetOrdinal(helper.h8);
                    if (!dr.IsDBNull(ih8)) entity.H8 = dr.GetDecimal(ih8);

                    int ih9 = dr.GetOrdinal(helper.h9);
                    if (!dr.IsDBNull(ih9)) entity.H9 = dr.GetDecimal(ih9);

                    int ih10 = dr.GetOrdinal(helper.h10);
                    if (!dr.IsDBNull(ih10)) entity.H10 = dr.GetDecimal(ih10);

                    int ih11 = dr.GetOrdinal(helper.h11);
                    if (!dr.IsDBNull(ih11)) entity.H11 = dr.GetDecimal(ih11);

                    int ih12 = dr.GetOrdinal(helper.h12);
                    if (!dr.IsDBNull(ih12)) entity.H12 = dr.GetDecimal(ih12);

                    int ih13 = dr.GetOrdinal(helper.h13);
                    if (!dr.IsDBNull(ih13)) entity.H13 = dr.GetDecimal(ih13);

                    int ih14 = dr.GetOrdinal(helper.h14);
                    if (!dr.IsDBNull(ih14)) entity.H14 = dr.GetDecimal(ih14);

                    int ih15 = dr.GetOrdinal(helper.h15);
                    if (!dr.IsDBNull(ih15)) entity.H15 = dr.GetDecimal(ih15);

                    int ih16 = dr.GetOrdinal(helper.h16);
                    if (!dr.IsDBNull(ih16)) entity.H16 = dr.GetDecimal(ih16);

                    int ih17 = dr.GetOrdinal(helper.h17);
                    if (!dr.IsDBNull(ih17)) entity.H17 = dr.GetDecimal(ih17);

                    int ih18 = dr.GetOrdinal(helper.h18);
                    if (!dr.IsDBNull(ih18)) entity.H18 = dr.GetDecimal(ih18);

                    int ih19 = dr.GetOrdinal(helper.h19);
                    if (!dr.IsDBNull(ih19)) entity.H19 = dr.GetDecimal(ih19);

                    int ih20 = dr.GetOrdinal(helper.h20);
                    if (!dr.IsDBNull(ih20)) entity.H20 = dr.GetDecimal(ih20);

                    int ih21 = dr.GetOrdinal(helper.h21);
                    if (!dr.IsDBNull(ih21)) entity.H21 = dr.GetDecimal(ih21);

                    int ih22 = dr.GetOrdinal(helper.h22);
                    if (!dr.IsDBNull(ih22)) entity.H22 = dr.GetDecimal(ih22);

                    int ih23 = dr.GetOrdinal(helper.h23);
                    if (!dr.IsDBNull(ih23)) entity.H23 = dr.GetDecimal(ih23);

                    int ih24 = dr.GetOrdinal(helper.h24);
                    if (!dr.IsDBNull(ih24)) entity.H24 = dr.GetDecimal(ih24);

                    int ih25 = dr.GetOrdinal(helper.h25);
                    if (!dr.IsDBNull(ih25)) entity.H25 = dr.GetDecimal(ih25);

                    int ih26 = dr.GetOrdinal(helper.h26);
                    if (!dr.IsDBNull(ih26)) entity.H26 = dr.GetDecimal(ih26);

                    int ih27 = dr.GetOrdinal(helper.h27);
                    if (!dr.IsDBNull(ih27)) entity.H27 = dr.GetDecimal(ih27);

                    int ih28 = dr.GetOrdinal(helper.h28);
                    if (!dr.IsDBNull(ih28)) entity.H28 = dr.GetDecimal(ih28);

                    int ih29 = dr.GetOrdinal(helper.h29);
                    if (!dr.IsDBNull(ih29)) entity.H29 = dr.GetDecimal(ih29);

                    int ih30 = dr.GetOrdinal(helper.h30);
                    if (!dr.IsDBNull(ih30)) entity.H30 = dr.GetDecimal(ih30);

                    int ih31 = dr.GetOrdinal(helper.h31);
                    if (!dr.IsDBNull(ih31)) entity.H31 = dr.GetDecimal(ih31);

                    int ih32 = dr.GetOrdinal(helper.h32);
                    if (!dr.IsDBNull(ih32)) entity.H32 = dr.GetDecimal(ih32);

                    int ih33 = dr.GetOrdinal(helper.h33);
                    if (!dr.IsDBNull(ih33)) entity.H33 = dr.GetDecimal(ih33);

                    int ih34 = dr.GetOrdinal(helper.h34);
                    if (!dr.IsDBNull(ih34)) entity.H34 = dr.GetDecimal(ih34);

                    int ih35 = dr.GetOrdinal(helper.h35);
                    if (!dr.IsDBNull(ih35)) entity.H35 = dr.GetDecimal(ih35);

                    int ih36 = dr.GetOrdinal(helper.h36);
                    if (!dr.IsDBNull(ih36)) entity.H36 = dr.GetDecimal(ih36);

                    int ih37 = dr.GetOrdinal(helper.h37);
                    if (!dr.IsDBNull(ih37)) entity.H37 = dr.GetDecimal(ih37);

                    int ih38 = dr.GetOrdinal(helper.h38);
                    if (!dr.IsDBNull(ih38)) entity.H38 = dr.GetDecimal(ih38);

                    int ih39 = dr.GetOrdinal(helper.h39);
                    if (!dr.IsDBNull(ih39)) entity.H39 = dr.GetDecimal(ih39);

                    int ih40 = dr.GetOrdinal(helper.h40);
                    if (!dr.IsDBNull(ih40)) entity.H40 = dr.GetDecimal(ih40);

                    int ih41 = dr.GetOrdinal(helper.h41);
                    if (!dr.IsDBNull(ih41)) entity.H41 = dr.GetDecimal(ih41);

                    int ih42 = dr.GetOrdinal(helper.h42);
                    if (!dr.IsDBNull(ih42)) entity.H42 = dr.GetDecimal(ih42);

                    int ih43 = dr.GetOrdinal(helper.h43);
                    if (!dr.IsDBNull(ih43)) entity.H43 = dr.GetDecimal(ih43);

                    int ih44 = dr.GetOrdinal(helper.h44);
                    if (!dr.IsDBNull(ih44)) entity.H44 = dr.GetDecimal(ih44);

                    int ih45 = dr.GetOrdinal(helper.h45);
                    if (!dr.IsDBNull(ih45)) entity.H45 = dr.GetDecimal(ih45);

                    int ih46 = dr.GetOrdinal(helper.h46);
                    if (!dr.IsDBNull(ih46)) entity.H46 = dr.GetDecimal(ih46);

                    int ih47 = dr.GetOrdinal(helper.h47);
                    if (!dr.IsDBNull(ih47)) entity.H47 = dr.GetDecimal(ih47);

                    int ih48 = dr.GetOrdinal(helper.h48);
                    if (!dr.IsDBNull(ih48)) entity.H48 = dr.GetDecimal(ih48);

                    int ih49 = dr.GetOrdinal(helper.h49);
                    if (!dr.IsDBNull(ih49)) entity.H49 = dr.GetDecimal(ih49);

                    int ih50 = dr.GetOrdinal(helper.h50);
                    if (!dr.IsDBNull(ih50)) entity.H50 = dr.GetDecimal(ih50);

                    int ih51 = dr.GetOrdinal(helper.h51);
                    if (!dr.IsDBNull(ih51)) entity.H51 = dr.GetDecimal(ih51);

                    int ih52 = dr.GetOrdinal(helper.h52);
                    if (!dr.IsDBNull(ih50)) entity.H52 = dr.GetDecimal(ih52);

                    int ih53 = dr.GetOrdinal(helper.h53);
                    if (!dr.IsDBNull(ih53)) entity.H53 = dr.GetDecimal(ih53);

                    int ih54 = dr.GetOrdinal(helper.h54);
                    if (!dr.IsDBNull(ih54)) entity.H54 = dr.GetDecimal(ih54);

                    int ih55 = dr.GetOrdinal(helper.h55);
                    if (!dr.IsDBNull(ih55)) entity.H55 = dr.GetDecimal(ih55);

                    int ih56 = dr.GetOrdinal(helper.h56);
                    if (!dr.IsDBNull(ih56)) entity.H56 = dr.GetDecimal(ih56);

                    int ih57 = dr.GetOrdinal(helper.h57);
                    if (!dr.IsDBNull(ih57)) entity.H57 = dr.GetDecimal(ih57);

                    int ih58 = dr.GetOrdinal(helper.h58);
                    if (!dr.IsDBNull(ih58)) entity.H58 = dr.GetDecimal(ih58);

                    int ih59 = dr.GetOrdinal(helper.h59);
                    if (!dr.IsDBNull(ih59)) entity.H59 = dr.GetDecimal(ih59);

                    int ih60 = dr.GetOrdinal(helper.h60);
                    if (!dr.IsDBNull(ih60)) entity.H60 = dr.GetDecimal(ih60);

                    int ih61 = dr.GetOrdinal(helper.h61);
                    if (!dr.IsDBNull(ih61)) entity.H61 = dr.GetDecimal(ih61);

                    int ih62 = dr.GetOrdinal(helper.h62);
                    if (!dr.IsDBNull(ih62)) entity.H62 = dr.GetDecimal(ih62);

                    int ih63 = dr.GetOrdinal(helper.h63);
                    if (!dr.IsDBNull(ih63)) entity.H63 = dr.GetDecimal(ih63);

                    int ih64 = dr.GetOrdinal(helper.h64);
                    if (!dr.IsDBNull(ih64)) entity.H64 = dr.GetDecimal(ih64);

                    int ih65 = dr.GetOrdinal(helper.h65);
                    if (!dr.IsDBNull(ih65)) entity.H65 = dr.GetDecimal(ih65);

                    int ih66 = dr.GetOrdinal(helper.h66);
                    if (!dr.IsDBNull(ih66)) entity.H66 = dr.GetDecimal(ih66);

                    int ih67 = dr.GetOrdinal(helper.h67);
                    if (!dr.IsDBNull(ih67)) entity.H67 = dr.GetDecimal(ih67);

                    int ih68 = dr.GetOrdinal(helper.h68);
                    if (!dr.IsDBNull(ih68)) entity.H68 = dr.GetDecimal(ih68);

                    int ih69 = dr.GetOrdinal(helper.h69);
                    if (!dr.IsDBNull(ih69)) entity.H69 = dr.GetDecimal(ih69);

                    int ih70 = dr.GetOrdinal(helper.h70);
                    if (!dr.IsDBNull(ih70)) entity.H70 = dr.GetDecimal(ih70);

                    int ih71 = dr.GetOrdinal(helper.h71);
                    if (!dr.IsDBNull(ih71)) entity.H71 = dr.GetDecimal(ih71);

                    int ih72 = dr.GetOrdinal(helper.h72);
                    if (!dr.IsDBNull(ih72)) entity.H72 = dr.GetDecimal(ih72);

                    int ih73 = dr.GetOrdinal(helper.h73);
                    if (!dr.IsDBNull(ih73)) entity.H73 = dr.GetDecimal(ih73);

                    int ih74 = dr.GetOrdinal(helper.h74);
                    if (!dr.IsDBNull(ih74)) entity.H74 = dr.GetDecimal(ih74);

                    int ih75 = dr.GetOrdinal(helper.h75);
                    if (!dr.IsDBNull(ih75)) entity.H75 = dr.GetDecimal(ih75);

                    int ih76 = dr.GetOrdinal(helper.h76);
                    if (!dr.IsDBNull(ih76)) entity.H76 = dr.GetDecimal(ih76);

                    int ih77 = dr.GetOrdinal(helper.h77);
                    if (!dr.IsDBNull(ih77)) entity.H77 = dr.GetDecimal(ih77);

                    int ih78 = dr.GetOrdinal(helper.h78);
                    if (!dr.IsDBNull(ih78)) entity.H78 = dr.GetDecimal(ih78);

                    int ih79 = dr.GetOrdinal(helper.h79);
                    if (!dr.IsDBNull(ih79)) entity.H79 = dr.GetDecimal(ih79);

                    int ih80 = dr.GetOrdinal(helper.h80);
                    if (!dr.IsDBNull(ih80)) entity.H80 = dr.GetDecimal(ih80);

                    int ih81 = dr.GetOrdinal(helper.h81);
                    if (!dr.IsDBNull(ih81)) entity.H81 = dr.GetDecimal(ih81);

                    int ih82 = dr.GetOrdinal(helper.h82);
                    if (!dr.IsDBNull(ih82)) entity.H82 = dr.GetDecimal(ih82);

                    int ih83 = dr.GetOrdinal(helper.h83);
                    if (!dr.IsDBNull(ih83)) entity.H83 = dr.GetDecimal(ih83);

                    int ih84 = dr.GetOrdinal(helper.h84);
                    if (!dr.IsDBNull(ih84)) entity.H84 = dr.GetDecimal(ih84);

                    int ih85 = dr.GetOrdinal(helper.h85);
                    if (!dr.IsDBNull(ih85)) entity.H85 = dr.GetDecimal(ih85);

                    int ih86 = dr.GetOrdinal(helper.h86);
                    if (!dr.IsDBNull(ih86)) entity.H86 = dr.GetDecimal(ih86);

                    int ih87 = dr.GetOrdinal(helper.h87);
                    if (!dr.IsDBNull(ih87)) entity.H87 = dr.GetDecimal(ih87);

                    int ih88 = dr.GetOrdinal(helper.h88);
                    if (!dr.IsDBNull(ih88)) entity.H88 = dr.GetDecimal(ih88);

                    int ih89 = dr.GetOrdinal(helper.h89);
                    if (!dr.IsDBNull(ih89)) entity.H89 = dr.GetDecimal(ih89);

                    int ih90 = dr.GetOrdinal(helper.h90);
                    if (!dr.IsDBNull(ih90)) entity.H90 = dr.GetDecimal(ih90);

                    int ih91 = dr.GetOrdinal(helper.h91);
                    if (!dr.IsDBNull(ih91)) entity.H91 = dr.GetDecimal(ih91);

                    int ih92 = dr.GetOrdinal(helper.h92);
                    if (!dr.IsDBNull(ih92)) entity.H92 = dr.GetDecimal(ih92);

                    int ih93 = dr.GetOrdinal(helper.h93);
                    if (!dr.IsDBNull(ih93)) entity.H93 = dr.GetDecimal(ih93);

                    int ih94 = dr.GetOrdinal(helper.h94);
                    if (!dr.IsDBNull(ih94)) entity.H94 = dr.GetDecimal(ih94);

                    int ih95 = dr.GetOrdinal(helper.h95);
                    if (!dr.IsDBNull(ih95)) entity.H95 = dr.GetDecimal(ih95);

                    int ih96 = dr.GetOrdinal(helper.h96);
                    if (!dr.IsDBNull(ih96)) entity.H96 = dr.GetDecimal(ih96);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<DpoDemandaScoDTO> ListDatosTNA(int anio, string mes, string cargas, string tipo)
        {
            List<DpoDemandaScoDTO> entitys = new List<DpoDemandaScoDTO>();

            string query = string.Format(helper.SqlListDatosTNA, anio, mes, cargas, tipo);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    DpoDemandaScoDTO entity = new DpoDemandaScoDTO();

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iMedifecha = dr.GetOrdinal(helper.Medifecha);
                    if (!dr.IsDBNull(iMedifecha)) entity.Medifecha = dr.GetDateTime(iMedifecha);

                    int ih1 = dr.GetOrdinal(helper.h1);
                    if (!dr.IsDBNull(ih1)) entity.H1 = dr.GetDecimal(ih1);

                    int ih2 = dr.GetOrdinal(helper.h2);
                    if (!dr.IsDBNull(ih2)) entity.H2 = dr.GetDecimal(ih2);

                    int ih3 = dr.GetOrdinal(helper.h3);
                    if (!dr.IsDBNull(ih3)) entity.H3 = dr.GetDecimal(ih3);

                    int ih4 = dr.GetOrdinal(helper.h4);
                    if (!dr.IsDBNull(ih4)) entity.H4 = dr.GetDecimal(ih4);

                    int ih5 = dr.GetOrdinal(helper.h5);
                    if (!dr.IsDBNull(ih5)) entity.H5 = dr.GetDecimal(ih5);

                    int ih6 = dr.GetOrdinal(helper.h6);
                    if (!dr.IsDBNull(ih6)) entity.H6 = dr.GetDecimal(ih6);

                    int ih7 = dr.GetOrdinal(helper.h7);
                    if (!dr.IsDBNull(ih7)) entity.H7 = dr.GetDecimal(ih7);

                    int ih8 = dr.GetOrdinal(helper.h8);
                    if (!dr.IsDBNull(ih8)) entity.H8 = dr.GetDecimal(ih8);

                    int ih9 = dr.GetOrdinal(helper.h9);
                    if (!dr.IsDBNull(ih9)) entity.H9 = dr.GetDecimal(ih9);

                    int ih10 = dr.GetOrdinal(helper.h10);
                    if (!dr.IsDBNull(ih10)) entity.H10 = dr.GetDecimal(ih10);

                    int ih11 = dr.GetOrdinal(helper.h11);
                    if (!dr.IsDBNull(ih11)) entity.H11 = dr.GetDecimal(ih11);

                    int ih12 = dr.GetOrdinal(helper.h12);
                    if (!dr.IsDBNull(ih12)) entity.H12 = dr.GetDecimal(ih12);

                    int ih13 = dr.GetOrdinal(helper.h13);
                    if (!dr.IsDBNull(ih13)) entity.H13 = dr.GetDecimal(ih13);

                    int ih14 = dr.GetOrdinal(helper.h14);
                    if (!dr.IsDBNull(ih14)) entity.H14 = dr.GetDecimal(ih14);

                    int ih15 = dr.GetOrdinal(helper.h15);
                    if (!dr.IsDBNull(ih15)) entity.H15 = dr.GetDecimal(ih15);

                    int ih16 = dr.GetOrdinal(helper.h16);
                    if (!dr.IsDBNull(ih16)) entity.H16 = dr.GetDecimal(ih16);

                    int ih17 = dr.GetOrdinal(helper.h17);
                    if (!dr.IsDBNull(ih17)) entity.H17 = dr.GetDecimal(ih17);

                    int ih18 = dr.GetOrdinal(helper.h18);
                    if (!dr.IsDBNull(ih18)) entity.H18 = dr.GetDecimal(ih18);

                    int ih19 = dr.GetOrdinal(helper.h19);
                    if (!dr.IsDBNull(ih19)) entity.H19 = dr.GetDecimal(ih19);

                    int ih20 = dr.GetOrdinal(helper.h20);
                    if (!dr.IsDBNull(ih20)) entity.H20 = dr.GetDecimal(ih20);

                    int ih21 = dr.GetOrdinal(helper.h21);
                    if (!dr.IsDBNull(ih21)) entity.H21 = dr.GetDecimal(ih21);

                    int ih22 = dr.GetOrdinal(helper.h22);
                    if (!dr.IsDBNull(ih22)) entity.H22 = dr.GetDecimal(ih22);

                    int ih23 = dr.GetOrdinal(helper.h23);
                    if (!dr.IsDBNull(ih23)) entity.H23 = dr.GetDecimal(ih23);

                    int ih24 = dr.GetOrdinal(helper.h24);
                    if (!dr.IsDBNull(ih24)) entity.H24 = dr.GetDecimal(ih24);

                    int ih25 = dr.GetOrdinal(helper.h25);
                    if (!dr.IsDBNull(ih25)) entity.H25 = dr.GetDecimal(ih25);

                    int ih26 = dr.GetOrdinal(helper.h26);
                    if (!dr.IsDBNull(ih26)) entity.H26 = dr.GetDecimal(ih26);

                    int ih27 = dr.GetOrdinal(helper.h27);
                    if (!dr.IsDBNull(ih27)) entity.H27 = dr.GetDecimal(ih27);

                    int ih28 = dr.GetOrdinal(helper.h28);
                    if (!dr.IsDBNull(ih28)) entity.H28 = dr.GetDecimal(ih28);

                    int ih29 = dr.GetOrdinal(helper.h29);
                    if (!dr.IsDBNull(ih29)) entity.H29 = dr.GetDecimal(ih29);

                    int ih30 = dr.GetOrdinal(helper.h30);
                    if (!dr.IsDBNull(ih30)) entity.H30 = dr.GetDecimal(ih30);

                    int ih31 = dr.GetOrdinal(helper.h31);
                    if (!dr.IsDBNull(ih31)) entity.H31 = dr.GetDecimal(ih31);

                    int ih32 = dr.GetOrdinal(helper.h32);
                    if (!dr.IsDBNull(ih32)) entity.H32 = dr.GetDecimal(ih32);

                    int ih33 = dr.GetOrdinal(helper.h33);
                    if (!dr.IsDBNull(ih33)) entity.H33 = dr.GetDecimal(ih33);

                    int ih34 = dr.GetOrdinal(helper.h34);
                    if (!dr.IsDBNull(ih34)) entity.H34 = dr.GetDecimal(ih34);

                    int ih35 = dr.GetOrdinal(helper.h35);
                    if (!dr.IsDBNull(ih35)) entity.H35 = dr.GetDecimal(ih35);

                    int ih36 = dr.GetOrdinal(helper.h36);
                    if (!dr.IsDBNull(ih36)) entity.H36 = dr.GetDecimal(ih36);

                    int ih37 = dr.GetOrdinal(helper.h37);
                    if (!dr.IsDBNull(ih37)) entity.H37 = dr.GetDecimal(ih37);

                    int ih38 = dr.GetOrdinal(helper.h38);
                    if (!dr.IsDBNull(ih38)) entity.H38 = dr.GetDecimal(ih38);

                    int ih39 = dr.GetOrdinal(helper.h39);
                    if (!dr.IsDBNull(ih39)) entity.H39 = dr.GetDecimal(ih39);

                    int ih40 = dr.GetOrdinal(helper.h40);
                    if (!dr.IsDBNull(ih40)) entity.H40 = dr.GetDecimal(ih40);

                    int ih41 = dr.GetOrdinal(helper.h41);
                    if (!dr.IsDBNull(ih41)) entity.H41 = dr.GetDecimal(ih41);

                    int ih42 = dr.GetOrdinal(helper.h42);
                    if (!dr.IsDBNull(ih42)) entity.H42 = dr.GetDecimal(ih42);

                    int ih43 = dr.GetOrdinal(helper.h43);
                    if (!dr.IsDBNull(ih43)) entity.H43 = dr.GetDecimal(ih43);

                    int ih44 = dr.GetOrdinal(helper.h44);
                    if (!dr.IsDBNull(ih44)) entity.H44 = dr.GetDecimal(ih44);

                    int ih45 = dr.GetOrdinal(helper.h45);
                    if (!dr.IsDBNull(ih45)) entity.H45 = dr.GetDecimal(ih45);

                    int ih46 = dr.GetOrdinal(helper.h46);
                    if (!dr.IsDBNull(ih46)) entity.H46 = dr.GetDecimal(ih46);

                    int ih47 = dr.GetOrdinal(helper.h47);
                    if (!dr.IsDBNull(ih47)) entity.H47 = dr.GetDecimal(ih47);

                    int ih48 = dr.GetOrdinal(helper.h48);
                    if (!dr.IsDBNull(ih48)) entity.H48 = dr.GetDecimal(ih48);

                    int ih49 = dr.GetOrdinal(helper.h49);
                    if (!dr.IsDBNull(ih49)) entity.H49 = dr.GetDecimal(ih49);

                    int ih50 = dr.GetOrdinal(helper.h50);
                    if (!dr.IsDBNull(ih50)) entity.H50 = dr.GetDecimal(ih50);

                    int ih51 = dr.GetOrdinal(helper.h51);
                    if (!dr.IsDBNull(ih51)) entity.H51 = dr.GetDecimal(ih51);

                    int ih52 = dr.GetOrdinal(helper.h52);
                    if (!dr.IsDBNull(ih50)) entity.H52 = dr.GetDecimal(ih52);

                    int ih53 = dr.GetOrdinal(helper.h53);
                    if (!dr.IsDBNull(ih53)) entity.H53 = dr.GetDecimal(ih53);

                    int ih54 = dr.GetOrdinal(helper.h54);
                    if (!dr.IsDBNull(ih54)) entity.H54 = dr.GetDecimal(ih54);

                    int ih55 = dr.GetOrdinal(helper.h55);
                    if (!dr.IsDBNull(ih55)) entity.H55 = dr.GetDecimal(ih55);

                    int ih56 = dr.GetOrdinal(helper.h56);
                    if (!dr.IsDBNull(ih56)) entity.H56 = dr.GetDecimal(ih56);

                    int ih57 = dr.GetOrdinal(helper.h57);
                    if (!dr.IsDBNull(ih57)) entity.H57 = dr.GetDecimal(ih57);

                    int ih58 = dr.GetOrdinal(helper.h58);
                    if (!dr.IsDBNull(ih58)) entity.H58 = dr.GetDecimal(ih58);

                    int ih59 = dr.GetOrdinal(helper.h59);
                    if (!dr.IsDBNull(ih59)) entity.H59 = dr.GetDecimal(ih59);

                    int ih60 = dr.GetOrdinal(helper.h60);
                    if (!dr.IsDBNull(ih60)) entity.H60 = dr.GetDecimal(ih60);

                    int ih61 = dr.GetOrdinal(helper.h61);
                    if (!dr.IsDBNull(ih61)) entity.H61 = dr.GetDecimal(ih61);

                    int ih62 = dr.GetOrdinal(helper.h62);
                    if (!dr.IsDBNull(ih62)) entity.H62 = dr.GetDecimal(ih62);

                    int ih63 = dr.GetOrdinal(helper.h63);
                    if (!dr.IsDBNull(ih63)) entity.H63 = dr.GetDecimal(ih63);

                    int ih64 = dr.GetOrdinal(helper.h64);
                    if (!dr.IsDBNull(ih64)) entity.H64 = dr.GetDecimal(ih64);

                    int ih65 = dr.GetOrdinal(helper.h65);
                    if (!dr.IsDBNull(ih65)) entity.H65 = dr.GetDecimal(ih65);

                    int ih66 = dr.GetOrdinal(helper.h66);
                    if (!dr.IsDBNull(ih66)) entity.H66 = dr.GetDecimal(ih66);

                    int ih67 = dr.GetOrdinal(helper.h67);
                    if (!dr.IsDBNull(ih67)) entity.H67 = dr.GetDecimal(ih67);

                    int ih68 = dr.GetOrdinal(helper.h68);
                    if (!dr.IsDBNull(ih68)) entity.H68 = dr.GetDecimal(ih68);

                    int ih69 = dr.GetOrdinal(helper.h69);
                    if (!dr.IsDBNull(ih69)) entity.H69 = dr.GetDecimal(ih69);

                    int ih70 = dr.GetOrdinal(helper.h70);
                    if (!dr.IsDBNull(ih70)) entity.H70 = dr.GetDecimal(ih70);

                    int ih71 = dr.GetOrdinal(helper.h71);
                    if (!dr.IsDBNull(ih71)) entity.H71 = dr.GetDecimal(ih71);

                    int ih72 = dr.GetOrdinal(helper.h72);
                    if (!dr.IsDBNull(ih72)) entity.H72 = dr.GetDecimal(ih72);

                    int ih73 = dr.GetOrdinal(helper.h73);
                    if (!dr.IsDBNull(ih73)) entity.H73 = dr.GetDecimal(ih73);

                    int ih74 = dr.GetOrdinal(helper.h74);
                    if (!dr.IsDBNull(ih74)) entity.H74 = dr.GetDecimal(ih74);

                    int ih75 = dr.GetOrdinal(helper.h75);
                    if (!dr.IsDBNull(ih75)) entity.H75 = dr.GetDecimal(ih75);

                    int ih76 = dr.GetOrdinal(helper.h76);
                    if (!dr.IsDBNull(ih76)) entity.H76 = dr.GetDecimal(ih76);

                    int ih77 = dr.GetOrdinal(helper.h77);
                    if (!dr.IsDBNull(ih77)) entity.H77 = dr.GetDecimal(ih77);

                    int ih78 = dr.GetOrdinal(helper.h78);
                    if (!dr.IsDBNull(ih78)) entity.H78 = dr.GetDecimal(ih78);

                    int ih79 = dr.GetOrdinal(helper.h79);
                    if (!dr.IsDBNull(ih79)) entity.H79 = dr.GetDecimal(ih79);

                    int ih80 = dr.GetOrdinal(helper.h80);
                    if (!dr.IsDBNull(ih80)) entity.H80 = dr.GetDecimal(ih80);

                    int ih81 = dr.GetOrdinal(helper.h81);
                    if (!dr.IsDBNull(ih81)) entity.H81 = dr.GetDecimal(ih81);

                    int ih82 = dr.GetOrdinal(helper.h82);
                    if (!dr.IsDBNull(ih82)) entity.H82 = dr.GetDecimal(ih82);

                    int ih83 = dr.GetOrdinal(helper.h83);
                    if (!dr.IsDBNull(ih83)) entity.H83 = dr.GetDecimal(ih83);

                    int ih84 = dr.GetOrdinal(helper.h84);
                    if (!dr.IsDBNull(ih84)) entity.H84 = dr.GetDecimal(ih84);

                    int ih85 = dr.GetOrdinal(helper.h85);
                    if (!dr.IsDBNull(ih85)) entity.H85 = dr.GetDecimal(ih85);

                    int ih86 = dr.GetOrdinal(helper.h86);
                    if (!dr.IsDBNull(ih86)) entity.H86 = dr.GetDecimal(ih86);

                    int ih87 = dr.GetOrdinal(helper.h87);
                    if (!dr.IsDBNull(ih87)) entity.H87 = dr.GetDecimal(ih87);

                    int ih88 = dr.GetOrdinal(helper.h88);
                    if (!dr.IsDBNull(ih88)) entity.H88 = dr.GetDecimal(ih88);

                    int ih89 = dr.GetOrdinal(helper.h89);
                    if (!dr.IsDBNull(ih89)) entity.H89 = dr.GetDecimal(ih89);

                    int ih90 = dr.GetOrdinal(helper.h90);
                    if (!dr.IsDBNull(ih90)) entity.H90 = dr.GetDecimal(ih90);

                    int ih91 = dr.GetOrdinal(helper.h91);
                    if (!dr.IsDBNull(ih91)) entity.H91 = dr.GetDecimal(ih91);

                    int ih92 = dr.GetOrdinal(helper.h92);
                    if (!dr.IsDBNull(ih92)) entity.H92 = dr.GetDecimal(ih92);

                    int ih93 = dr.GetOrdinal(helper.h93);
                    if (!dr.IsDBNull(ih93)) entity.H93 = dr.GetDecimal(ih93);

                    int ih94 = dr.GetOrdinal(helper.h94);
                    if (!dr.IsDBNull(ih94)) entity.H94 = dr.GetDecimal(ih94);

                    int ih95 = dr.GetOrdinal(helper.h95);
                    if (!dr.IsDBNull(ih95)) entity.H95 = dr.GetDecimal(ih95);

                    int ih96 = dr.GetOrdinal(helper.h96);
                    if (!dr.IsDBNull(ih96)) entity.H96 = dr.GetDecimal(ih96);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        //ListMedidorDemandaTna
        public List<DpoDemandaScoDTO> ListMedidorDemandaTna(string punto, string inicio, string fin, string tipo)
        {
            List<DpoDemandaScoDTO> entitys = new List<DpoDemandaScoDTO>();

            string query = string.Format(helper.SqlListMedidorDemandaTna, punto, inicio, fin, tipo);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    DpoDemandaScoDTO entity = new DpoDemandaScoDTO();

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iPtomedielenomb = dr.GetOrdinal(helper.Ptomedielenomb);
                    if (!dr.IsDBNull(iPtomedielenomb)) entity.Ptomedielenomb = dr.GetString(iPtomedielenomb);

                    int iPtomedidesc = dr.GetOrdinal(helper.Ptomedidesc);
                    if (!dr.IsDBNull(iPtomedidesc)) entity.Ptomedidesc = dr.GetString(iPtomedidesc);

                    int iMedifecha = dr.GetOrdinal(helper.Medifecha);
                    if (!dr.IsDBNull(iMedifecha)) entity.Medifecha = dr.GetDateTime(iMedifecha);

                    int iMeditotal = dr.GetOrdinal(helper.Meditotal);
                    if (!dr.IsDBNull(iMeditotal)) entity.Meditotal = dr.GetDecimal(iMeditotal);

                    int ih1 = dr.GetOrdinal(helper.h1);
                    if (!dr.IsDBNull(ih1)) entity.H1 = dr.GetDecimal(ih1);

                    int ih2 = dr.GetOrdinal(helper.h2);
                    if (!dr.IsDBNull(ih2)) entity.H2 = dr.GetDecimal(ih2);

                    int ih3 = dr.GetOrdinal(helper.h3);
                    if (!dr.IsDBNull(ih3)) entity.H3 = dr.GetDecimal(ih3);

                    int ih4 = dr.GetOrdinal(helper.h4);
                    if (!dr.IsDBNull(ih4)) entity.H4 = dr.GetDecimal(ih4);

                    int ih5 = dr.GetOrdinal(helper.h5);
                    if (!dr.IsDBNull(ih5)) entity.H5 = dr.GetDecimal(ih5);

                    int ih6 = dr.GetOrdinal(helper.h6);
                    if (!dr.IsDBNull(ih6)) entity.H6 = dr.GetDecimal(ih6);

                    int ih7 = dr.GetOrdinal(helper.h7);
                    if (!dr.IsDBNull(ih7)) entity.H7 = dr.GetDecimal(ih7);

                    int ih8 = dr.GetOrdinal(helper.h8);
                    if (!dr.IsDBNull(ih8)) entity.H8 = dr.GetDecimal(ih8);

                    int ih9 = dr.GetOrdinal(helper.h9);
                    if (!dr.IsDBNull(ih9)) entity.H9 = dr.GetDecimal(ih9);

                    int ih10 = dr.GetOrdinal(helper.h10);
                    if (!dr.IsDBNull(ih10)) entity.H10 = dr.GetDecimal(ih10);

                    int ih11 = dr.GetOrdinal(helper.h11);
                    if (!dr.IsDBNull(ih11)) entity.H11 = dr.GetDecimal(ih11);

                    int ih12 = dr.GetOrdinal(helper.h12);
                    if (!dr.IsDBNull(ih12)) entity.H12 = dr.GetDecimal(ih12);

                    int ih13 = dr.GetOrdinal(helper.h13);
                    if (!dr.IsDBNull(ih13)) entity.H13 = dr.GetDecimal(ih13);

                    int ih14 = dr.GetOrdinal(helper.h14);
                    if (!dr.IsDBNull(ih14)) entity.H14 = dr.GetDecimal(ih14);

                    int ih15 = dr.GetOrdinal(helper.h15);
                    if (!dr.IsDBNull(ih15)) entity.H15 = dr.GetDecimal(ih15);

                    int ih16 = dr.GetOrdinal(helper.h16);
                    if (!dr.IsDBNull(ih16)) entity.H16 = dr.GetDecimal(ih16);

                    int ih17 = dr.GetOrdinal(helper.h17);
                    if (!dr.IsDBNull(ih17)) entity.H17 = dr.GetDecimal(ih17);

                    int ih18 = dr.GetOrdinal(helper.h18);
                    if (!dr.IsDBNull(ih18)) entity.H18 = dr.GetDecimal(ih18);

                    int ih19 = dr.GetOrdinal(helper.h19);
                    if (!dr.IsDBNull(ih19)) entity.H19 = dr.GetDecimal(ih19);

                    int ih20 = dr.GetOrdinal(helper.h20);
                    if (!dr.IsDBNull(ih20)) entity.H20 = dr.GetDecimal(ih20);

                    int ih21 = dr.GetOrdinal(helper.h21);
                    if (!dr.IsDBNull(ih21)) entity.H21 = dr.GetDecimal(ih21);

                    int ih22 = dr.GetOrdinal(helper.h22);
                    if (!dr.IsDBNull(ih22)) entity.H22 = dr.GetDecimal(ih22);

                    int ih23 = dr.GetOrdinal(helper.h23);
                    if (!dr.IsDBNull(ih23)) entity.H23 = dr.GetDecimal(ih23);

                    int ih24 = dr.GetOrdinal(helper.h24);
                    if (!dr.IsDBNull(ih24)) entity.H24 = dr.GetDecimal(ih24);

                    int ih25 = dr.GetOrdinal(helper.h25);
                    if (!dr.IsDBNull(ih25)) entity.H25 = dr.GetDecimal(ih25);

                    int ih26 = dr.GetOrdinal(helper.h26);
                    if (!dr.IsDBNull(ih26)) entity.H26 = dr.GetDecimal(ih26);

                    int ih27 = dr.GetOrdinal(helper.h27);
                    if (!dr.IsDBNull(ih27)) entity.H27 = dr.GetDecimal(ih27);

                    int ih28 = dr.GetOrdinal(helper.h28);
                    if (!dr.IsDBNull(ih28)) entity.H28 = dr.GetDecimal(ih28);

                    int ih29 = dr.GetOrdinal(helper.h29);
                    if (!dr.IsDBNull(ih29)) entity.H29 = dr.GetDecimal(ih29);

                    int ih30 = dr.GetOrdinal(helper.h30);
                    if (!dr.IsDBNull(ih30)) entity.H30 = dr.GetDecimal(ih30);

                    int ih31 = dr.GetOrdinal(helper.h31);
                    if (!dr.IsDBNull(ih31)) entity.H31 = dr.GetDecimal(ih31);

                    int ih32 = dr.GetOrdinal(helper.h32);
                    if (!dr.IsDBNull(ih32)) entity.H32 = dr.GetDecimal(ih32);

                    int ih33 = dr.GetOrdinal(helper.h33);
                    if (!dr.IsDBNull(ih33)) entity.H33 = dr.GetDecimal(ih33);

                    int ih34 = dr.GetOrdinal(helper.h34);
                    if (!dr.IsDBNull(ih34)) entity.H34 = dr.GetDecimal(ih34);

                    int ih35 = dr.GetOrdinal(helper.h35);
                    if (!dr.IsDBNull(ih35)) entity.H35 = dr.GetDecimal(ih35);

                    int ih36 = dr.GetOrdinal(helper.h36);
                    if (!dr.IsDBNull(ih36)) entity.H36 = dr.GetDecimal(ih36);

                    int ih37 = dr.GetOrdinal(helper.h37);
                    if (!dr.IsDBNull(ih37)) entity.H37 = dr.GetDecimal(ih37);

                    int ih38 = dr.GetOrdinal(helper.h38);
                    if (!dr.IsDBNull(ih38)) entity.H38 = dr.GetDecimal(ih38);

                    int ih39 = dr.GetOrdinal(helper.h39);
                    if (!dr.IsDBNull(ih39)) entity.H39 = dr.GetDecimal(ih39);

                    int ih40 = dr.GetOrdinal(helper.h40);
                    if (!dr.IsDBNull(ih40)) entity.H40 = dr.GetDecimal(ih40);

                    int ih41 = dr.GetOrdinal(helper.h41);
                    if (!dr.IsDBNull(ih41)) entity.H41 = dr.GetDecimal(ih41);

                    int ih42 = dr.GetOrdinal(helper.h42);
                    if (!dr.IsDBNull(ih42)) entity.H42 = dr.GetDecimal(ih42);

                    int ih43 = dr.GetOrdinal(helper.h43);
                    if (!dr.IsDBNull(ih43)) entity.H43 = dr.GetDecimal(ih43);

                    int ih44 = dr.GetOrdinal(helper.h44);
                    if (!dr.IsDBNull(ih44)) entity.H44 = dr.GetDecimal(ih44);

                    int ih45 = dr.GetOrdinal(helper.h45);
                    if (!dr.IsDBNull(ih45)) entity.H45 = dr.GetDecimal(ih45);

                    int ih46 = dr.GetOrdinal(helper.h46);
                    if (!dr.IsDBNull(ih46)) entity.H46 = dr.GetDecimal(ih46);

                    int ih47 = dr.GetOrdinal(helper.h47);
                    if (!dr.IsDBNull(ih47)) entity.H47 = dr.GetDecimal(ih47);

                    int ih48 = dr.GetOrdinal(helper.h48);
                    if (!dr.IsDBNull(ih48)) entity.H48 = dr.GetDecimal(ih48);

                    int ih49 = dr.GetOrdinal(helper.h49);
                    if (!dr.IsDBNull(ih49)) entity.H49 = dr.GetDecimal(ih49);

                    int ih50 = dr.GetOrdinal(helper.h50);
                    if (!dr.IsDBNull(ih50)) entity.H50 = dr.GetDecimal(ih50);

                    int ih51 = dr.GetOrdinal(helper.h51);
                    if (!dr.IsDBNull(ih51)) entity.H51 = dr.GetDecimal(ih51);

                    int ih52 = dr.GetOrdinal(helper.h52);
                    if (!dr.IsDBNull(ih50)) entity.H52 = dr.GetDecimal(ih52);

                    int ih53 = dr.GetOrdinal(helper.h53);
                    if (!dr.IsDBNull(ih53)) entity.H53 = dr.GetDecimal(ih53);

                    int ih54 = dr.GetOrdinal(helper.h54);
                    if (!dr.IsDBNull(ih54)) entity.H54 = dr.GetDecimal(ih54);

                    int ih55 = dr.GetOrdinal(helper.h55);
                    if (!dr.IsDBNull(ih55)) entity.H55 = dr.GetDecimal(ih55);

                    int ih56 = dr.GetOrdinal(helper.h56);
                    if (!dr.IsDBNull(ih56)) entity.H56 = dr.GetDecimal(ih56);

                    int ih57 = dr.GetOrdinal(helper.h57);
                    if (!dr.IsDBNull(ih57)) entity.H57 = dr.GetDecimal(ih57);

                    int ih58 = dr.GetOrdinal(helper.h58);
                    if (!dr.IsDBNull(ih58)) entity.H58 = dr.GetDecimal(ih58);

                    int ih59 = dr.GetOrdinal(helper.h59);
                    if (!dr.IsDBNull(ih59)) entity.H59 = dr.GetDecimal(ih59);

                    int ih60 = dr.GetOrdinal(helper.h60);
                    if (!dr.IsDBNull(ih60)) entity.H60 = dr.GetDecimal(ih60);

                    int ih61 = dr.GetOrdinal(helper.h61);
                    if (!dr.IsDBNull(ih61)) entity.H61 = dr.GetDecimal(ih61);

                    int ih62 = dr.GetOrdinal(helper.h62);
                    if (!dr.IsDBNull(ih62)) entity.H62 = dr.GetDecimal(ih62);

                    int ih63 = dr.GetOrdinal(helper.h63);
                    if (!dr.IsDBNull(ih63)) entity.H63 = dr.GetDecimal(ih63);

                    int ih64 = dr.GetOrdinal(helper.h64);
                    if (!dr.IsDBNull(ih64)) entity.H64 = dr.GetDecimal(ih64);

                    int ih65 = dr.GetOrdinal(helper.h65);
                    if (!dr.IsDBNull(ih65)) entity.H65 = dr.GetDecimal(ih65);

                    int ih66 = dr.GetOrdinal(helper.h66);
                    if (!dr.IsDBNull(ih66)) entity.H66 = dr.GetDecimal(ih66);

                    int ih67 = dr.GetOrdinal(helper.h67);
                    if (!dr.IsDBNull(ih67)) entity.H67 = dr.GetDecimal(ih67);

                    int ih68 = dr.GetOrdinal(helper.h68);
                    if (!dr.IsDBNull(ih68)) entity.H68 = dr.GetDecimal(ih68);

                    int ih69 = dr.GetOrdinal(helper.h69);
                    if (!dr.IsDBNull(ih69)) entity.H69 = dr.GetDecimal(ih69);

                    int ih70 = dr.GetOrdinal(helper.h70);
                    if (!dr.IsDBNull(ih70)) entity.H70 = dr.GetDecimal(ih70);

                    int ih71 = dr.GetOrdinal(helper.h71);
                    if (!dr.IsDBNull(ih71)) entity.H71 = dr.GetDecimal(ih71);

                    int ih72 = dr.GetOrdinal(helper.h72);
                    if (!dr.IsDBNull(ih72)) entity.H72 = dr.GetDecimal(ih72);

                    int ih73 = dr.GetOrdinal(helper.h73);
                    if (!dr.IsDBNull(ih73)) entity.H73 = dr.GetDecimal(ih73);

                    int ih74 = dr.GetOrdinal(helper.h74);
                    if (!dr.IsDBNull(ih74)) entity.H74 = dr.GetDecimal(ih74);

                    int ih75 = dr.GetOrdinal(helper.h75);
                    if (!dr.IsDBNull(ih75)) entity.H75 = dr.GetDecimal(ih75);

                    int ih76 = dr.GetOrdinal(helper.h76);
                    if (!dr.IsDBNull(ih76)) entity.H76 = dr.GetDecimal(ih76);

                    int ih77 = dr.GetOrdinal(helper.h77);
                    if (!dr.IsDBNull(ih77)) entity.H77 = dr.GetDecimal(ih77);

                    int ih78 = dr.GetOrdinal(helper.h78);
                    if (!dr.IsDBNull(ih78)) entity.H78 = dr.GetDecimal(ih78);

                    int ih79 = dr.GetOrdinal(helper.h79);
                    if (!dr.IsDBNull(ih79)) entity.H79 = dr.GetDecimal(ih79);

                    int ih80 = dr.GetOrdinal(helper.h80);
                    if (!dr.IsDBNull(ih80)) entity.H80 = dr.GetDecimal(ih80);

                    int ih81 = dr.GetOrdinal(helper.h81);
                    if (!dr.IsDBNull(ih81)) entity.H81 = dr.GetDecimal(ih81);

                    int ih82 = dr.GetOrdinal(helper.h82);
                    if (!dr.IsDBNull(ih82)) entity.H82 = dr.GetDecimal(ih82);

                    int ih83 = dr.GetOrdinal(helper.h83);
                    if (!dr.IsDBNull(ih83)) entity.H83 = dr.GetDecimal(ih83);

                    int ih84 = dr.GetOrdinal(helper.h84);
                    if (!dr.IsDBNull(ih84)) entity.H84 = dr.GetDecimal(ih84);

                    int ih85 = dr.GetOrdinal(helper.h85);
                    if (!dr.IsDBNull(ih85)) entity.H85 = dr.GetDecimal(ih85);

                    int ih86 = dr.GetOrdinal(helper.h86);
                    if (!dr.IsDBNull(ih86)) entity.H86 = dr.GetDecimal(ih86);

                    int ih87 = dr.GetOrdinal(helper.h87);
                    if (!dr.IsDBNull(ih87)) entity.H87 = dr.GetDecimal(ih87);

                    int ih88 = dr.GetOrdinal(helper.h88);
                    if (!dr.IsDBNull(ih88)) entity.H88 = dr.GetDecimal(ih88);

                    int ih89 = dr.GetOrdinal(helper.h89);
                    if (!dr.IsDBNull(ih89)) entity.H89 = dr.GetDecimal(ih89);

                    int ih90 = dr.GetOrdinal(helper.h90);
                    if (!dr.IsDBNull(ih90)) entity.H90 = dr.GetDecimal(ih90);

                    int ih91 = dr.GetOrdinal(helper.h91);
                    if (!dr.IsDBNull(ih91)) entity.H91 = dr.GetDecimal(ih91);

                    int ih92 = dr.GetOrdinal(helper.h92);
                    if (!dr.IsDBNull(ih92)) entity.H92 = dr.GetDecimal(ih92);

                    int ih93 = dr.GetOrdinal(helper.h93);
                    if (!dr.IsDBNull(ih93)) entity.H93 = dr.GetDecimal(ih93);

                    int ih94 = dr.GetOrdinal(helper.h94);
                    if (!dr.IsDBNull(ih94)) entity.H94 = dr.GetDecimal(ih94);

                    int ih95 = dr.GetOrdinal(helper.h95);
                    if (!dr.IsDBNull(ih95)) entity.H95 = dr.GetDecimal(ih95);

                    int ih96 = dr.GetOrdinal(helper.h96);
                    if (!dr.IsDBNull(ih96)) entity.H96 = dr.GetDecimal(ih96);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public DpoDemandaScoDTO GetById(int ptomedicodi, DateTime medifecha, int prnvarcodi)
        {
            DpoDemandaScoDTO entity = new DpoDemandaScoDTO();

            string fecha = medifecha.ToString("dd/MM/yyyy");
            string query = string.Format(helper.SqlGetById, 
                ptomedicodi, fecha, prnvarcodi);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public void BulkInsert(List<DpoDemandaScoDTO> entitys, string nombreTabla)
        {
            dbProvider.AddColumnMapping(helper.Ptomedicodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Medifecha, DbType.DateTime);
            dbProvider.AddColumnMapping(helper.Prnvarcodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Meditotal, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.h1, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.h2, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.h3, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.h4, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.h5, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.h6, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.h7, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.h8, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.h9, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.h10, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.h11, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.h12, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.h13, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.h14, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.h15, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.h16, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.h17, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.h18, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.h19, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.h20, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.h21, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.h22, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.h23, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.h24, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.h25, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.h26, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.h27, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.h28, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.h29, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.h30, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.h31, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.h32, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.h33, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.h34, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.h35, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.h36, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.h37, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.h38, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.h39, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.h40, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.h41, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.h42, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.h43, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.h44, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.h45, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.h46, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.h47, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.h48, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.h49, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.h50, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.h51, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.h52, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.h53, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.h54, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.h55, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.h56, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.h57, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.h58, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.h59, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.h60, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.h61, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.h62, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.h63, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.h64, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.h65, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.h66, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.h67, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.h68, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.h69, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.h70, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.h71, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.h72, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.h73, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.h74, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.h75, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.h76, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.h77, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.h78, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.h79, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.h80, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.h81, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.h82, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.h83, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.h84, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.h85, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.h86, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.h87, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.h88, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.h89, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.h90, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.h91, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.h92, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.h93, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.h94, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.h95, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.h96, DbType.Decimal);            
            dbProvider.AddColumnMapping(helper.Demscofeccreacion, DbType.DateTime);

            dbProvider.BulkInsert<DpoDemandaScoDTO>(entitys, nombreTabla);
        }

        public void TruncateTablaTemporal(string nombreTabla)
        {
            string query = string.Format(helper.SqlTruncateTablaTemporal, nombreTabla);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            dbProvider.ExecuteNonQuery(command);
        }

        public List<DateTime> ReporteEstadoProceso(
            int dporawfuente, string fecIni, 
            string fecFin)
        {
            List<DateTime> entities = new List<DateTime>();
            
            string query = string.Format(helper.SqlReporteEstadoProceso, 
                fecIni, fecFin);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    int iMedifecha = dr.GetOrdinal(this.helper.Medifecha);
                    if (!dr.IsDBNull(iMedifecha)) entities.Add(dr.GetDateTime(iMedifecha));
                }
            }

            return entities;
        }

        public void DeleteRangoFecha(int punto, string fecIni, string fecFin)
        {
            string query = string.Format(helper.SqlDeleteRangoFecha,
                punto, fecIni, fecFin);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            dbProvider.ExecuteNonQuery(command);
        }

        public List<DpoDemandaScoDTO> ObtenerDemandaSco(string ptomedicodi, 
            string prnvarcodi, string medifecha)
        {
            List<DpoDemandaScoDTO> entitys = new List<DpoDemandaScoDTO>();
            string query = string.Format(helper.SqlObtenerDemandaSco,
                ptomedicodi, prnvarcodi, medifecha);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    DpoDemandaScoDTO entity = new DpoDemandaScoDTO();

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iMedifecha = dr.GetOrdinal(helper.Medifecha);
                    if (!dr.IsDBNull(iMedifecha)) entity.Medifecha = dr.GetDateTime(iMedifecha);

                    int ih1 = dr.GetOrdinal(helper.h1);
                    if (!dr.IsDBNull(ih1)) entity.H1 = dr.GetDecimal(ih1);

                    int ih2 = dr.GetOrdinal(helper.h2);
                    if (!dr.IsDBNull(ih2)) entity.H2 = dr.GetDecimal(ih2);

                    int ih3 = dr.GetOrdinal(helper.h3);
                    if (!dr.IsDBNull(ih3)) entity.H3 = dr.GetDecimal(ih3);

                    int ih4 = dr.GetOrdinal(helper.h4);
                    if (!dr.IsDBNull(ih4)) entity.H4 = dr.GetDecimal(ih4);

                    int ih5 = dr.GetOrdinal(helper.h5);
                    if (!dr.IsDBNull(ih5)) entity.H5 = dr.GetDecimal(ih5);

                    int ih6 = dr.GetOrdinal(helper.h6);
                    if (!dr.IsDBNull(ih6)) entity.H6 = dr.GetDecimal(ih6);

                    int ih7 = dr.GetOrdinal(helper.h7);
                    if (!dr.IsDBNull(ih7)) entity.H7 = dr.GetDecimal(ih7);

                    int ih8 = dr.GetOrdinal(helper.h8);
                    if (!dr.IsDBNull(ih8)) entity.H8 = dr.GetDecimal(ih8);

                    int ih9 = dr.GetOrdinal(helper.h9);
                    if (!dr.IsDBNull(ih9)) entity.H9 = dr.GetDecimal(ih9);

                    int ih10 = dr.GetOrdinal(helper.h10);
                    if (!dr.IsDBNull(ih10)) entity.H10 = dr.GetDecimal(ih10);

                    int ih11 = dr.GetOrdinal(helper.h11);
                    if (!dr.IsDBNull(ih11)) entity.H11 = dr.GetDecimal(ih11);

                    int ih12 = dr.GetOrdinal(helper.h12);
                    if (!dr.IsDBNull(ih12)) entity.H12 = dr.GetDecimal(ih12);

                    int ih13 = dr.GetOrdinal(helper.h13);
                    if (!dr.IsDBNull(ih13)) entity.H13 = dr.GetDecimal(ih13);

                    int ih14 = dr.GetOrdinal(helper.h14);
                    if (!dr.IsDBNull(ih14)) entity.H14 = dr.GetDecimal(ih14);

                    int ih15 = dr.GetOrdinal(helper.h15);
                    if (!dr.IsDBNull(ih15)) entity.H15 = dr.GetDecimal(ih15);

                    int ih16 = dr.GetOrdinal(helper.h16);
                    if (!dr.IsDBNull(ih16)) entity.H16 = dr.GetDecimal(ih16);

                    int ih17 = dr.GetOrdinal(helper.h17);
                    if (!dr.IsDBNull(ih17)) entity.H17 = dr.GetDecimal(ih17);

                    int ih18 = dr.GetOrdinal(helper.h18);
                    if (!dr.IsDBNull(ih18)) entity.H18 = dr.GetDecimal(ih18);

                    int ih19 = dr.GetOrdinal(helper.h19);
                    if (!dr.IsDBNull(ih19)) entity.H19 = dr.GetDecimal(ih19);

                    int ih20 = dr.GetOrdinal(helper.h20);
                    if (!dr.IsDBNull(ih20)) entity.H20 = dr.GetDecimal(ih20);

                    int ih21 = dr.GetOrdinal(helper.h21);
                    if (!dr.IsDBNull(ih21)) entity.H21 = dr.GetDecimal(ih21);

                    int ih22 = dr.GetOrdinal(helper.h22);
                    if (!dr.IsDBNull(ih22)) entity.H22 = dr.GetDecimal(ih22);

                    int ih23 = dr.GetOrdinal(helper.h23);
                    if (!dr.IsDBNull(ih23)) entity.H23 = dr.GetDecimal(ih23);

                    int ih24 = dr.GetOrdinal(helper.h24);
                    if (!dr.IsDBNull(ih24)) entity.H24 = dr.GetDecimal(ih24);

                    int ih25 = dr.GetOrdinal(helper.h25);
                    if (!dr.IsDBNull(ih25)) entity.H25 = dr.GetDecimal(ih25);

                    int ih26 = dr.GetOrdinal(helper.h26);
                    if (!dr.IsDBNull(ih26)) entity.H26 = dr.GetDecimal(ih26);

                    int ih27 = dr.GetOrdinal(helper.h27);
                    if (!dr.IsDBNull(ih27)) entity.H27 = dr.GetDecimal(ih27);

                    int ih28 = dr.GetOrdinal(helper.h28);
                    if (!dr.IsDBNull(ih28)) entity.H28 = dr.GetDecimal(ih28);

                    int ih29 = dr.GetOrdinal(helper.h29);
                    if (!dr.IsDBNull(ih29)) entity.H29 = dr.GetDecimal(ih29);

                    int ih30 = dr.GetOrdinal(helper.h30);
                    if (!dr.IsDBNull(ih30)) entity.H30 = dr.GetDecimal(ih30);

                    int ih31 = dr.GetOrdinal(helper.h31);
                    if (!dr.IsDBNull(ih31)) entity.H31 = dr.GetDecimal(ih31);

                    int ih32 = dr.GetOrdinal(helper.h32);
                    if (!dr.IsDBNull(ih32)) entity.H32 = dr.GetDecimal(ih32);

                    int ih33 = dr.GetOrdinal(helper.h33);
                    if (!dr.IsDBNull(ih33)) entity.H33 = dr.GetDecimal(ih33);

                    int ih34 = dr.GetOrdinal(helper.h34);
                    if (!dr.IsDBNull(ih34)) entity.H34 = dr.GetDecimal(ih34);

                    int ih35 = dr.GetOrdinal(helper.h35);
                    if (!dr.IsDBNull(ih35)) entity.H35 = dr.GetDecimal(ih35);

                    int ih36 = dr.GetOrdinal(helper.h36);
                    if (!dr.IsDBNull(ih36)) entity.H36 = dr.GetDecimal(ih36);

                    int ih37 = dr.GetOrdinal(helper.h37);
                    if (!dr.IsDBNull(ih37)) entity.H37 = dr.GetDecimal(ih37);

                    int ih38 = dr.GetOrdinal(helper.h38);
                    if (!dr.IsDBNull(ih38)) entity.H38 = dr.GetDecimal(ih38);

                    int ih39 = dr.GetOrdinal(helper.h39);
                    if (!dr.IsDBNull(ih39)) entity.H39 = dr.GetDecimal(ih39);

                    int ih40 = dr.GetOrdinal(helper.h40);
                    if (!dr.IsDBNull(ih40)) entity.H40 = dr.GetDecimal(ih40);

                    int ih41 = dr.GetOrdinal(helper.h41);
                    if (!dr.IsDBNull(ih41)) entity.H41 = dr.GetDecimal(ih41);

                    int ih42 = dr.GetOrdinal(helper.h42);
                    if (!dr.IsDBNull(ih42)) entity.H42 = dr.GetDecimal(ih42);

                    int ih43 = dr.GetOrdinal(helper.h43);
                    if (!dr.IsDBNull(ih43)) entity.H43 = dr.GetDecimal(ih43);

                    int ih44 = dr.GetOrdinal(helper.h44);
                    if (!dr.IsDBNull(ih44)) entity.H44 = dr.GetDecimal(ih44);

                    int ih45 = dr.GetOrdinal(helper.h45);
                    if (!dr.IsDBNull(ih45)) entity.H45 = dr.GetDecimal(ih45);

                    int ih46 = dr.GetOrdinal(helper.h46);
                    if (!dr.IsDBNull(ih46)) entity.H46 = dr.GetDecimal(ih46);

                    int ih47 = dr.GetOrdinal(helper.h47);
                    if (!dr.IsDBNull(ih47)) entity.H47 = dr.GetDecimal(ih47);

                    int ih48 = dr.GetOrdinal(helper.h48);
                    if (!dr.IsDBNull(ih48)) entity.H48 = dr.GetDecimal(ih48);

                    int ih49 = dr.GetOrdinal(helper.h49);
                    if (!dr.IsDBNull(ih49)) entity.H49 = dr.GetDecimal(ih49);

                    int ih50 = dr.GetOrdinal(helper.h50);
                    if (!dr.IsDBNull(ih50)) entity.H50 = dr.GetDecimal(ih50);

                    int ih51 = dr.GetOrdinal(helper.h51);
                    if (!dr.IsDBNull(ih51)) entity.H51 = dr.GetDecimal(ih51);

                    int ih52 = dr.GetOrdinal(helper.h52);
                    if (!dr.IsDBNull(ih50)) entity.H52 = dr.GetDecimal(ih52);

                    int ih53 = dr.GetOrdinal(helper.h53);
                    if (!dr.IsDBNull(ih53)) entity.H53 = dr.GetDecimal(ih53);

                    int ih54 = dr.GetOrdinal(helper.h54);
                    if (!dr.IsDBNull(ih54)) entity.H54 = dr.GetDecimal(ih54);

                    int ih55 = dr.GetOrdinal(helper.h55);
                    if (!dr.IsDBNull(ih55)) entity.H55 = dr.GetDecimal(ih55);

                    int ih56 = dr.GetOrdinal(helper.h56);
                    if (!dr.IsDBNull(ih56)) entity.H56 = dr.GetDecimal(ih56);

                    int ih57 = dr.GetOrdinal(helper.h57);
                    if (!dr.IsDBNull(ih57)) entity.H57 = dr.GetDecimal(ih57);

                    int ih58 = dr.GetOrdinal(helper.h58);
                    if (!dr.IsDBNull(ih58)) entity.H58 = dr.GetDecimal(ih58);

                    int ih59 = dr.GetOrdinal(helper.h59);
                    if (!dr.IsDBNull(ih59)) entity.H59 = dr.GetDecimal(ih59);

                    int ih60 = dr.GetOrdinal(helper.h60);
                    if (!dr.IsDBNull(ih60)) entity.H60 = dr.GetDecimal(ih60);

                    int ih61 = dr.GetOrdinal(helper.h61);
                    if (!dr.IsDBNull(ih61)) entity.H61 = dr.GetDecimal(ih61);

                    int ih62 = dr.GetOrdinal(helper.h62);
                    if (!dr.IsDBNull(ih62)) entity.H62 = dr.GetDecimal(ih62);

                    int ih63 = dr.GetOrdinal(helper.h63);
                    if (!dr.IsDBNull(ih63)) entity.H63 = dr.GetDecimal(ih63);

                    int ih64 = dr.GetOrdinal(helper.h64);
                    if (!dr.IsDBNull(ih64)) entity.H64 = dr.GetDecimal(ih64);

                    int ih65 = dr.GetOrdinal(helper.h65);
                    if (!dr.IsDBNull(ih65)) entity.H65 = dr.GetDecimal(ih65);

                    int ih66 = dr.GetOrdinal(helper.h66);
                    if (!dr.IsDBNull(ih66)) entity.H66 = dr.GetDecimal(ih66);

                    int ih67 = dr.GetOrdinal(helper.h67);
                    if (!dr.IsDBNull(ih67)) entity.H67 = dr.GetDecimal(ih67);

                    int ih68 = dr.GetOrdinal(helper.h68);
                    if (!dr.IsDBNull(ih68)) entity.H68 = dr.GetDecimal(ih68);

                    int ih69 = dr.GetOrdinal(helper.h69);
                    if (!dr.IsDBNull(ih69)) entity.H69 = dr.GetDecimal(ih69);

                    int ih70 = dr.GetOrdinal(helper.h70);
                    if (!dr.IsDBNull(ih70)) entity.H70 = dr.GetDecimal(ih70);

                    int ih71 = dr.GetOrdinal(helper.h71);
                    if (!dr.IsDBNull(ih71)) entity.H71 = dr.GetDecimal(ih71);

                    int ih72 = dr.GetOrdinal(helper.h72);
                    if (!dr.IsDBNull(ih72)) entity.H72 = dr.GetDecimal(ih72);

                    int ih73 = dr.GetOrdinal(helper.h73);
                    if (!dr.IsDBNull(ih73)) entity.H73 = dr.GetDecimal(ih73);

                    int ih74 = dr.GetOrdinal(helper.h74);
                    if (!dr.IsDBNull(ih74)) entity.H74 = dr.GetDecimal(ih74);

                    int ih75 = dr.GetOrdinal(helper.h75);
                    if (!dr.IsDBNull(ih75)) entity.H75 = dr.GetDecimal(ih75);

                    int ih76 = dr.GetOrdinal(helper.h76);
                    if (!dr.IsDBNull(ih76)) entity.H76 = dr.GetDecimal(ih76);

                    int ih77 = dr.GetOrdinal(helper.h77);
                    if (!dr.IsDBNull(ih77)) entity.H77 = dr.GetDecimal(ih77);

                    int ih78 = dr.GetOrdinal(helper.h78);
                    if (!dr.IsDBNull(ih78)) entity.H78 = dr.GetDecimal(ih78);

                    int ih79 = dr.GetOrdinal(helper.h79);
                    if (!dr.IsDBNull(ih79)) entity.H79 = dr.GetDecimal(ih79);

                    int ih80 = dr.GetOrdinal(helper.h80);
                    if (!dr.IsDBNull(ih80)) entity.H80 = dr.GetDecimal(ih80);

                    int ih81 = dr.GetOrdinal(helper.h81);
                    if (!dr.IsDBNull(ih81)) entity.H81 = dr.GetDecimal(ih81);

                    int ih82 = dr.GetOrdinal(helper.h82);
                    if (!dr.IsDBNull(ih82)) entity.H82 = dr.GetDecimal(ih82);

                    int ih83 = dr.GetOrdinal(helper.h83);
                    if (!dr.IsDBNull(ih83)) entity.H83 = dr.GetDecimal(ih83);

                    int ih84 = dr.GetOrdinal(helper.h84);
                    if (!dr.IsDBNull(ih84)) entity.H84 = dr.GetDecimal(ih84);

                    int ih85 = dr.GetOrdinal(helper.h85);
                    if (!dr.IsDBNull(ih85)) entity.H85 = dr.GetDecimal(ih85);

                    int ih86 = dr.GetOrdinal(helper.h86);
                    if (!dr.IsDBNull(ih86)) entity.H86 = dr.GetDecimal(ih86);

                    int ih87 = dr.GetOrdinal(helper.h87);
                    if (!dr.IsDBNull(ih87)) entity.H87 = dr.GetDecimal(ih87);

                    int ih88 = dr.GetOrdinal(helper.h88);
                    if (!dr.IsDBNull(ih88)) entity.H88 = dr.GetDecimal(ih88);

                    int ih89 = dr.GetOrdinal(helper.h89);
                    if (!dr.IsDBNull(ih89)) entity.H89 = dr.GetDecimal(ih89);

                    int ih90 = dr.GetOrdinal(helper.h90);
                    if (!dr.IsDBNull(ih90)) entity.H90 = dr.GetDecimal(ih90);

                    int ih91 = dr.GetOrdinal(helper.h91);
                    if (!dr.IsDBNull(ih91)) entity.H91 = dr.GetDecimal(ih91);

                    int ih92 = dr.GetOrdinal(helper.h92);
                    if (!dr.IsDBNull(ih92)) entity.H92 = dr.GetDecimal(ih92);

                    int ih93 = dr.GetOrdinal(helper.h93);
                    if (!dr.IsDBNull(ih93)) entity.H93 = dr.GetDecimal(ih93);

                    int ih94 = dr.GetOrdinal(helper.h94);
                    if (!dr.IsDBNull(ih94)) entity.H94 = dr.GetDecimal(ih94);

                    int ih95 = dr.GetOrdinal(helper.h95);
                    if (!dr.IsDBNull(ih95)) entity.H95 = dr.GetDecimal(ih95);

                    int ih96 = dr.GetOrdinal(helper.h96);
                    if (!dr.IsDBNull(ih96)) entity.H96 = dr.GetDecimal(ih96);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
