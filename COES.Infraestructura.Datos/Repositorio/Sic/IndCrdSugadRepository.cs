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
    public class IndCrdSugadRepository : RepositoryBase, IIndCrdSugadRepository
    {
        public IndCrdSugadRepository(string strConn) : base(strConn)
        {
        }

        IndCrdSugadHelper helper = new IndCrdSugadHelper();

        public int Save(IndCrdSugadDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Crdsgdcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Indcbrcodi, DbType.Int32, entity.Indcbrcodi);
            dbProvider.AddInParameter(command, helper.Equicodicentral, DbType.Int32, entity.Equicodicentral);
            dbProvider.AddInParameter(command, helper.Equicodiunidad, DbType.Int32, entity.Equicodiunidad);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Famcodi, DbType.Int32, entity.Famcodi);
            dbProvider.AddInParameter(command, helper.Crdsgdtipo, DbType.Int32, entity.Crdsgdtipo);
            dbProvider.AddInParameter(command, helper.D1, DbType.Decimal, entity.D1);
            dbProvider.AddInParameter(command, helper.D2, DbType.Decimal, entity.D2);
            dbProvider.AddInParameter(command, helper.D3, DbType.Decimal, entity.D3);
            dbProvider.AddInParameter(command, helper.D4, DbType.Decimal, entity.D4);
            dbProvider.AddInParameter(command, helper.D5, DbType.Decimal, entity.D5);
            dbProvider.AddInParameter(command, helper.D6, DbType.Decimal, entity.D6);
            dbProvider.AddInParameter(command, helper.D7, DbType.Decimal, entity.D7);
            dbProvider.AddInParameter(command, helper.D8, DbType.Decimal, entity.D8);
            dbProvider.AddInParameter(command, helper.D9, DbType.Decimal, entity.D9);
            dbProvider.AddInParameter(command, helper.D10, DbType.Decimal, entity.D10);
            dbProvider.AddInParameter(command, helper.D11, DbType.Decimal, entity.D11);
            dbProvider.AddInParameter(command, helper.D12, DbType.Decimal, entity.D12);
            dbProvider.AddInParameter(command, helper.D13, DbType.Decimal, entity.D13);
            dbProvider.AddInParameter(command, helper.D14, DbType.Decimal, entity.D14);
            dbProvider.AddInParameter(command, helper.D15, DbType.Decimal, entity.D15);
            dbProvider.AddInParameter(command, helper.D16, DbType.Decimal, entity.D16);
            dbProvider.AddInParameter(command, helper.D17, DbType.Decimal, entity.D17);
            dbProvider.AddInParameter(command, helper.D18, DbType.Decimal, entity.D18);
            dbProvider.AddInParameter(command, helper.D19, DbType.Decimal, entity.D19);
            dbProvider.AddInParameter(command, helper.D20, DbType.Decimal, entity.D20);
            dbProvider.AddInParameter(command, helper.D21, DbType.Decimal, entity.D21);
            dbProvider.AddInParameter(command, helper.D22, DbType.Decimal, entity.D22);
            dbProvider.AddInParameter(command, helper.D23, DbType.Decimal, entity.D23);
            dbProvider.AddInParameter(command, helper.D24, DbType.Decimal, entity.D24);
            dbProvider.AddInParameter(command, helper.D25, DbType.Decimal, entity.D25);
            dbProvider.AddInParameter(command, helper.D26, DbType.Decimal, entity.D26);
            dbProvider.AddInParameter(command, helper.D27, DbType.Decimal, entity.D27);
            dbProvider.AddInParameter(command, helper.D28, DbType.Decimal, entity.D28);
            dbProvider.AddInParameter(command, helper.D29, DbType.Decimal, entity.D29);
            dbProvider.AddInParameter(command, helper.D30, DbType.Decimal, entity.D30);
            dbProvider.AddInParameter(command, helper.D31, DbType.Decimal, entity.D31);
            dbProvider.AddInParameter(command, helper.E1, DbType.String, entity.E1);
            dbProvider.AddInParameter(command, helper.E2, DbType.String, entity.E2);
            dbProvider.AddInParameter(command, helper.E3, DbType.String, entity.E3);
            dbProvider.AddInParameter(command, helper.E4, DbType.String, entity.E4);
            dbProvider.AddInParameter(command, helper.E5, DbType.String, entity.E5);
            dbProvider.AddInParameter(command, helper.E6, DbType.String, entity.E6);
            dbProvider.AddInParameter(command, helper.E7, DbType.String, entity.E7);
            dbProvider.AddInParameter(command, helper.E8, DbType.String, entity.E8);
            dbProvider.AddInParameter(command, helper.E9, DbType.String, entity.E9);
            dbProvider.AddInParameter(command, helper.E10, DbType.String, entity.E10);
            dbProvider.AddInParameter(command, helper.E11, DbType.String, entity.E11);
            dbProvider.AddInParameter(command, helper.E12, DbType.String, entity.E12);
            dbProvider.AddInParameter(command, helper.E13, DbType.String, entity.E13);
            dbProvider.AddInParameter(command, helper.E14, DbType.String, entity.E14);
            dbProvider.AddInParameter(command, helper.E15, DbType.String, entity.E15);
            dbProvider.AddInParameter(command, helper.E16, DbType.String, entity.E16);
            dbProvider.AddInParameter(command, helper.E17, DbType.String, entity.E17);
            dbProvider.AddInParameter(command, helper.E18, DbType.String, entity.E18);
            dbProvider.AddInParameter(command, helper.E19, DbType.String, entity.E19);
            dbProvider.AddInParameter(command, helper.E20, DbType.String, entity.E20);
            dbProvider.AddInParameter(command, helper.E21, DbType.String, entity.E21);
            dbProvider.AddInParameter(command, helper.E22, DbType.String, entity.E22);
            dbProvider.AddInParameter(command, helper.E23, DbType.String, entity.E23);
            dbProvider.AddInParameter(command, helper.E24, DbType.String, entity.E24);
            dbProvider.AddInParameter(command, helper.E25, DbType.String, entity.E25);
            dbProvider.AddInParameter(command, helper.E26, DbType.String, entity.E26);
            dbProvider.AddInParameter(command, helper.E27, DbType.String, entity.E27);
            dbProvider.AddInParameter(command, helper.E28, DbType.String, entity.E28);
            dbProvider.AddInParameter(command, helper.E29, DbType.String, entity.E29);
            dbProvider.AddInParameter(command, helper.E30, DbType.String, entity.E30);
            dbProvider.AddInParameter(command, helper.E31, DbType.String, entity.E31);
            dbProvider.AddInParameter(command, helper.Crdsgdusucreacion, DbType.String, entity.Crdsgdusucreacion);
            dbProvider.AddInParameter(command, helper.Crdsgdfeccreacion, DbType.DateTime, entity.Crdsgdfeccreacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void UpdateIndCrdSugad(IndCrdSugadDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdateIndCrdSugad);

            dbProvider.AddInParameter(command, helper.D1, DbType.Decimal, entity.D1);
            dbProvider.AddInParameter(command, helper.D2, DbType.Decimal, entity.D2);
            dbProvider.AddInParameter(command, helper.D3, DbType.Decimal, entity.D3);
            dbProvider.AddInParameter(command, helper.D4, DbType.Decimal, entity.D4);
            dbProvider.AddInParameter(command, helper.D5, DbType.Decimal, entity.D5);
            dbProvider.AddInParameter(command, helper.D6, DbType.Decimal, entity.D6);
            dbProvider.AddInParameter(command, helper.D7, DbType.Decimal, entity.D7);
            dbProvider.AddInParameter(command, helper.D8, DbType.Decimal, entity.D8);
            dbProvider.AddInParameter(command, helper.D9, DbType.Decimal, entity.D9);
            dbProvider.AddInParameter(command, helper.D10, DbType.Decimal, entity.D10);
            dbProvider.AddInParameter(command, helper.D11, DbType.Decimal, entity.D11);
            dbProvider.AddInParameter(command, helper.D12, DbType.Decimal, entity.D12);
            dbProvider.AddInParameter(command, helper.D13, DbType.Decimal, entity.D13);
            dbProvider.AddInParameter(command, helper.D14, DbType.Decimal, entity.D14);
            dbProvider.AddInParameter(command, helper.D15, DbType.Decimal, entity.D15);
            dbProvider.AddInParameter(command, helper.D16, DbType.Decimal, entity.D16);
            dbProvider.AddInParameter(command, helper.D17, DbType.Decimal, entity.D17);
            dbProvider.AddInParameter(command, helper.D18, DbType.Decimal, entity.D18);
            dbProvider.AddInParameter(command, helper.D19, DbType.Decimal, entity.D19);
            dbProvider.AddInParameter(command, helper.D20, DbType.Decimal, entity.D20);
            dbProvider.AddInParameter(command, helper.D21, DbType.Decimal, entity.D21);
            dbProvider.AddInParameter(command, helper.D22, DbType.Decimal, entity.D22);
            dbProvider.AddInParameter(command, helper.D23, DbType.Decimal, entity.D23);
            dbProvider.AddInParameter(command, helper.D24, DbType.Decimal, entity.D24);
            dbProvider.AddInParameter(command, helper.D25, DbType.Decimal, entity.D25);
            dbProvider.AddInParameter(command, helper.D26, DbType.Decimal, entity.D26);
            dbProvider.AddInParameter(command, helper.D27, DbType.Decimal, entity.D27);
            dbProvider.AddInParameter(command, helper.D28, DbType.Decimal, entity.D28);
            dbProvider.AddInParameter(command, helper.D29, DbType.Decimal, entity.D29);
            dbProvider.AddInParameter(command, helper.D30, DbType.Decimal, entity.D30);
            dbProvider.AddInParameter(command, helper.D31, DbType.Decimal, entity.D31);
            dbProvider.AddInParameter(command, helper.E1, DbType.String, entity.E1);
            dbProvider.AddInParameter(command, helper.E2, DbType.String, entity.E2);
            dbProvider.AddInParameter(command, helper.E3, DbType.String, entity.E3);
            dbProvider.AddInParameter(command, helper.E4, DbType.String, entity.E4);
            dbProvider.AddInParameter(command, helper.E5, DbType.String, entity.E5);
            dbProvider.AddInParameter(command, helper.E6, DbType.String, entity.E6);
            dbProvider.AddInParameter(command, helper.E7, DbType.String, entity.E7);
            dbProvider.AddInParameter(command, helper.E8, DbType.String, entity.E8);
            dbProvider.AddInParameter(command, helper.E9, DbType.String, entity.E9);
            dbProvider.AddInParameter(command, helper.E10, DbType.String, entity.E10);
            dbProvider.AddInParameter(command, helper.E11, DbType.String, entity.E11);
            dbProvider.AddInParameter(command, helper.E12, DbType.String, entity.E12);
            dbProvider.AddInParameter(command, helper.E13, DbType.String, entity.E13);
            dbProvider.AddInParameter(command, helper.E14, DbType.String, entity.E14);
            dbProvider.AddInParameter(command, helper.E15, DbType.String, entity.E15);
            dbProvider.AddInParameter(command, helper.E16, DbType.String, entity.E16);
            dbProvider.AddInParameter(command, helper.E17, DbType.String, entity.E17);
            dbProvider.AddInParameter(command, helper.E18, DbType.String, entity.E18);
            dbProvider.AddInParameter(command, helper.E19, DbType.String, entity.E19);
            dbProvider.AddInParameter(command, helper.E20, DbType.String, entity.E20);
            dbProvider.AddInParameter(command, helper.E21, DbType.String, entity.E21);
            dbProvider.AddInParameter(command, helper.E22, DbType.String, entity.E22);
            dbProvider.AddInParameter(command, helper.E23, DbType.String, entity.E23);
            dbProvider.AddInParameter(command, helper.E24, DbType.String, entity.E24);
            dbProvider.AddInParameter(command, helper.E25, DbType.String, entity.E25);
            dbProvider.AddInParameter(command, helper.E26, DbType.String, entity.E26);
            dbProvider.AddInParameter(command, helper.E27, DbType.String, entity.E27);
            dbProvider.AddInParameter(command, helper.E28, DbType.String, entity.E28);
            dbProvider.AddInParameter(command, helper.E29, DbType.String, entity.E29);
            dbProvider.AddInParameter(command, helper.E30, DbType.String, entity.E30);
            dbProvider.AddInParameter(command, helper.E31, DbType.String, entity.E31);
            dbProvider.AddInParameter(command, helper.Crdsgdcodi, DbType.Int32, entity.Crdsgdcodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public void UpdateIndCrdEstado(IndCrdSugadDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdateIndCrdEstado);

            dbProvider.AddInParameter(command, helper.E1, DbType.String, entity.E1);
            dbProvider.AddInParameter(command, helper.E2, DbType.String, entity.E2);
            dbProvider.AddInParameter(command, helper.E3, DbType.String, entity.E3);
            dbProvider.AddInParameter(command, helper.E4, DbType.String, entity.E4);
            dbProvider.AddInParameter(command, helper.E5, DbType.String, entity.E5);
            dbProvider.AddInParameter(command, helper.E6, DbType.String, entity.E6);
            dbProvider.AddInParameter(command, helper.E7, DbType.String, entity.E7);
            dbProvider.AddInParameter(command, helper.E8, DbType.String, entity.E8);
            dbProvider.AddInParameter(command, helper.E9, DbType.String, entity.E9);
            dbProvider.AddInParameter(command, helper.E10, DbType.String, entity.E10);
            dbProvider.AddInParameter(command, helper.E11, DbType.String, entity.E11);
            dbProvider.AddInParameter(command, helper.E12, DbType.String, entity.E12);
            dbProvider.AddInParameter(command, helper.E13, DbType.String, entity.E13);
            dbProvider.AddInParameter(command, helper.E14, DbType.String, entity.E14);
            dbProvider.AddInParameter(command, helper.E15, DbType.String, entity.E15);
            dbProvider.AddInParameter(command, helper.E16, DbType.String, entity.E16);
            dbProvider.AddInParameter(command, helper.E17, DbType.String, entity.E17);
            dbProvider.AddInParameter(command, helper.E18, DbType.String, entity.E18);
            dbProvider.AddInParameter(command, helper.E19, DbType.String, entity.E19);
            dbProvider.AddInParameter(command, helper.E20, DbType.String, entity.E20);
            dbProvider.AddInParameter(command, helper.E21, DbType.String, entity.E21);
            dbProvider.AddInParameter(command, helper.E22, DbType.String, entity.E22);
            dbProvider.AddInParameter(command, helper.E23, DbType.String, entity.E23);
            dbProvider.AddInParameter(command, helper.E24, DbType.String, entity.E24);
            dbProvider.AddInParameter(command, helper.E25, DbType.String, entity.E25);
            dbProvider.AddInParameter(command, helper.E26, DbType.String, entity.E26);
            dbProvider.AddInParameter(command, helper.E27, DbType.String, entity.E27);
            dbProvider.AddInParameter(command, helper.E28, DbType.String, entity.E28);
            dbProvider.AddInParameter(command, helper.E29, DbType.String, entity.E29);
            dbProvider.AddInParameter(command, helper.E30, DbType.String, entity.E30);
            dbProvider.AddInParameter(command, helper.E31, DbType.String, entity.E31);
            dbProvider.AddInParameter(command, helper.Crdsgdcodi, DbType.Int32, entity.Crdsgdcodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public IndCrdSugadDTO GetById(int crdsgdcodi)
        {
            IndCrdSugadDTO entity = new IndCrdSugadDTO();
            string query = string.Format(helper.SqlGetById, crdsgdcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    int iCrdsgdcodi = dr.GetOrdinal(helper.Crdsgdcodi);
                    if (!dr.IsDBNull(iCrdsgdcodi)) entity.Crdsgdcodi = Convert.ToInt32(dr.GetValue(iCrdsgdcodi));

                    int iEquicodicentral = dr.GetOrdinal(helper.Equicodicentral);
                    if (!dr.IsDBNull(iEquicodicentral)) entity.Equicodicentral = Convert.ToInt32(dr.GetValue(iEquicodicentral));

                    int iEquicodiunidad = dr.GetOrdinal(helper.Equicodiunidad);
                    if (!dr.IsDBNull(iEquicodiunidad)) entity.Equicodiunidad = Convert.ToInt32(dr.GetValue(iEquicodiunidad));

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

                    int iCrdsgdtipo = dr.GetOrdinal(helper.Crdsgdtipo);
                    if (!dr.IsDBNull(iCrdsgdtipo)) entity.Crdsgdtipo = Convert.ToInt32(dr.GetValue(iCrdsgdtipo));

                    int iD1 = dr.GetOrdinal(helper.D1);
                    if (!dr.IsDBNull(iD1)) entity.D1 = dr.GetDecimal(iD1);

                    int iD2 = dr.GetOrdinal(helper.D2);
                    if (!dr.IsDBNull(iD2)) entity.D2 = dr.GetDecimal(iD2);

                    int iD3 = dr.GetOrdinal(helper.D3);
                    if (!dr.IsDBNull(iD3)) entity.D3 = dr.GetDecimal(iD3);

                    int iD4 = dr.GetOrdinal(helper.D4);
                    if (!dr.IsDBNull(iD4)) entity.D4 = dr.GetDecimal(iD4);

                    int iD5 = dr.GetOrdinal(helper.D5);
                    if (!dr.IsDBNull(iD5)) entity.D5 = dr.GetDecimal(iD5);

                    int iD6 = dr.GetOrdinal(helper.D6);
                    if (!dr.IsDBNull(iD6)) entity.D6 = dr.GetDecimal(iD6);

                    int iD7 = dr.GetOrdinal(helper.D7);
                    if (!dr.IsDBNull(iD7)) entity.D7 = dr.GetDecimal(iD7);

                    int iD8 = dr.GetOrdinal(helper.D8);
                    if (!dr.IsDBNull(iD8)) entity.D8 = dr.GetDecimal(iD8);

                    int iD9 = dr.GetOrdinal(helper.D9);
                    if (!dr.IsDBNull(iD9)) entity.D9 = dr.GetDecimal(iD9);

                    int iD10 = dr.GetOrdinal(helper.D10);
                    if (!dr.IsDBNull(iD10)) entity.D10 = dr.GetDecimal(iD10);

                    int iD11 = dr.GetOrdinal(helper.D11);
                    if (!dr.IsDBNull(iD11)) entity.D11 = dr.GetDecimal(iD11);

                    int iD12 = dr.GetOrdinal(helper.D12);
                    if (!dr.IsDBNull(iD12)) entity.D12 = dr.GetDecimal(iD12);

                    int iD13 = dr.GetOrdinal(helper.D13);
                    if (!dr.IsDBNull(iD13)) entity.D13 = dr.GetDecimal(iD13);

                    int iD14 = dr.GetOrdinal(helper.D14);
                    if (!dr.IsDBNull(iD14)) entity.D14 = dr.GetDecimal(iD14);

                    int iD15 = dr.GetOrdinal(helper.D15);
                    if (!dr.IsDBNull(iD15)) entity.D15 = dr.GetDecimal(iD15);

                    int iD16 = dr.GetOrdinal(helper.D16);
                    if (!dr.IsDBNull(iD16)) entity.D16 = dr.GetDecimal(iD16);

                    int iD17 = dr.GetOrdinal(helper.D17);
                    if (!dr.IsDBNull(iD17)) entity.D17 = dr.GetDecimal(iD17);

                    int iD18 = dr.GetOrdinal(helper.D18);
                    if (!dr.IsDBNull(iD18)) entity.D18 = dr.GetDecimal(iD18);

                    int iD19 = dr.GetOrdinal(helper.D19);
                    if (!dr.IsDBNull(iD19)) entity.D19 = dr.GetDecimal(iD19);

                    int iD20 = dr.GetOrdinal(helper.D20);
                    if (!dr.IsDBNull(iD20)) entity.D20 = dr.GetDecimal(iD20);

                    int iD21 = dr.GetOrdinal(helper.D21);
                    if (!dr.IsDBNull(iD21)) entity.D21 = dr.GetDecimal(iD21);

                    int iD22 = dr.GetOrdinal(helper.D22);
                    if (!dr.IsDBNull(iD22)) entity.D22 = dr.GetDecimal(iD22);

                    int iD23 = dr.GetOrdinal(helper.D23);
                    if (!dr.IsDBNull(iD23)) entity.D23 = dr.GetDecimal(iD23);

                    int iD24 = dr.GetOrdinal(helper.D24);
                    if (!dr.IsDBNull(iD24)) entity.D24 = dr.GetDecimal(iD24);

                    int iD25 = dr.GetOrdinal(helper.D25);
                    if (!dr.IsDBNull(iD25)) entity.D25 = dr.GetDecimal(iD25);

                    int iD26 = dr.GetOrdinal(helper.D26);
                    if (!dr.IsDBNull(iD26)) entity.D26 = dr.GetDecimal(iD26);

                    int iD27 = dr.GetOrdinal(helper.D27);
                    if (!dr.IsDBNull(iD27)) entity.D27 = dr.GetDecimal(iD27);

                    int iD28 = dr.GetOrdinal(helper.D28);
                    if (!dr.IsDBNull(iD28)) entity.D28 = dr.GetDecimal(iD28);

                    int iD29 = dr.GetOrdinal(helper.D29);
                    if (!dr.IsDBNull(iD29)) entity.D29 = dr.GetDecimal(iD29);

                    int iD30 = dr.GetOrdinal(helper.D30);
                    if (!dr.IsDBNull(iD30)) entity.D30 = dr.GetDecimal(iD30);

                    int iD31 = dr.GetOrdinal(helper.D31);
                    if (!dr.IsDBNull(iD31)) entity.D31 = dr.GetDecimal(iD31);

                    int iE1 = dr.GetOrdinal(helper.E1);
                    if (!dr.IsDBNull(iE1)) entity.E1 = dr.GetString(iE1);

                    int iE2 = dr.GetOrdinal(helper.E2);
                    if (!dr.IsDBNull(iE2)) entity.E2 = dr.GetString(iE2);

                    int iE3 = dr.GetOrdinal(helper.E3);
                    if (!dr.IsDBNull(iE3)) entity.E3 = dr.GetString(iE3);

                    int iE4 = dr.GetOrdinal(helper.E4);
                    if (!dr.IsDBNull(iE4)) entity.E4 = dr.GetString(iE4);

                    int iE5 = dr.GetOrdinal(helper.E5);
                    if (!dr.IsDBNull(iE5)) entity.E5 = dr.GetString(iE5);

                    int iE6 = dr.GetOrdinal(helper.E6);
                    if (!dr.IsDBNull(iE6)) entity.E6 = dr.GetString(iE6);

                    int iE7 = dr.GetOrdinal(helper.E7);
                    if (!dr.IsDBNull(iE7)) entity.E7 = dr.GetString(iE7);

                    int iE8 = dr.GetOrdinal(helper.E8);
                    if (!dr.IsDBNull(iE8)) entity.E8 = dr.GetString(iE8);

                    int iE9 = dr.GetOrdinal(helper.E9);
                    if (!dr.IsDBNull(iE9)) entity.E9 = dr.GetString(iE9);

                    int iE10 = dr.GetOrdinal(helper.E10);
                    if (!dr.IsDBNull(iE10)) entity.E10 = dr.GetString(iE10);

                    int iE11 = dr.GetOrdinal(helper.E11);
                    if (!dr.IsDBNull(iE11)) entity.E11 = dr.GetString(iE11);

                    int iE12 = dr.GetOrdinal(helper.E12);
                    if (!dr.IsDBNull(iE12)) entity.E12 = dr.GetString(iE12);

                    int iE13 = dr.GetOrdinal(helper.E13);
                    if (!dr.IsDBNull(iE13)) entity.E13 = dr.GetString(iE13);

                    int iE14 = dr.GetOrdinal(helper.E14);
                    if (!dr.IsDBNull(iE14)) entity.E14 = dr.GetString(iE14);

                    int iE15 = dr.GetOrdinal(helper.E15);
                    if (!dr.IsDBNull(iE15)) entity.E15 = dr.GetString(iE15);

                    int iE16 = dr.GetOrdinal(helper.E16);
                    if (!dr.IsDBNull(iE16)) entity.E16 = dr.GetString(iE16);

                    int iE17 = dr.GetOrdinal(helper.E17);
                    if (!dr.IsDBNull(iE17)) entity.E17 = dr.GetString(iE17);

                    int iE18 = dr.GetOrdinal(helper.E18);
                    if (!dr.IsDBNull(iE18)) entity.E18 = dr.GetString(iE18);

                    int iE19 = dr.GetOrdinal(helper.E19);
                    if (!dr.IsDBNull(iE19)) entity.E19 = dr.GetString(iE19);

                    int iE20 = dr.GetOrdinal(helper.E20);
                    if (!dr.IsDBNull(iE20)) entity.E20 = dr.GetString(iE20);

                    int iE21 = dr.GetOrdinal(helper.E21);
                    if (!dr.IsDBNull(iE21)) entity.E21 = dr.GetString(iE21);

                    int iE22 = dr.GetOrdinal(helper.E22);
                    if (!dr.IsDBNull(iE22)) entity.E22 = dr.GetString(iE22);

                    int iE23 = dr.GetOrdinal(helper.E23);
                    if (!dr.IsDBNull(iE23)) entity.E23 = dr.GetString(iE23);

                    int iE24 = dr.GetOrdinal(helper.E24);
                    if (!dr.IsDBNull(iE24)) entity.E24 = dr.GetString(iE24);

                    int iE25 = dr.GetOrdinal(helper.E25);
                    if (!dr.IsDBNull(iE25)) entity.E25 = dr.GetString(iE25);

                    int iE26 = dr.GetOrdinal(helper.E26);
                    if (!dr.IsDBNull(iE26)) entity.E26 = dr.GetString(iE26);

                    int iE27 = dr.GetOrdinal(helper.E27);
                    if (!dr.IsDBNull(iE27)) entity.E27 = dr.GetString(iE27);

                    int iE28 = dr.GetOrdinal(helper.E28);
                    if (!dr.IsDBNull(iE28)) entity.E28 = dr.GetString(iE28);

                    int iE29 = dr.GetOrdinal(helper.E29);
                    if (!dr.IsDBNull(iE29)) entity.E29 = dr.GetString(iE29);

                    int iE30 = dr.GetOrdinal(helper.E30);
                    if (!dr.IsDBNull(iE30)) entity.E30 = dr.GetString(iE30);

                    int iE31 = dr.GetOrdinal(helper.E31);
                    if (!dr.IsDBNull(iE31)) entity.E31 = dr.GetString(iE31);
                }
            }

            return entity;
        }

        public List<IndCrdSugadDTO> ListCrdSugadByCabecera(int indcbrcodi)
        {
            IndCrdSugadDTO entity = new IndCrdSugadDTO();
            List<IndCrdSugadDTO> entitys = new List<IndCrdSugadDTO>();
            string query = string.Format(helper.SqlListCrdSugadByCabecera, indcbrcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new IndCrdSugadDTO();

                    int iCrdsgdcodi = dr.GetOrdinal(helper.Crdsgdcodi);
                    if (!dr.IsDBNull(iCrdsgdcodi)) entity.Crdsgdcodi = Convert.ToInt32(dr.GetValue(iCrdsgdcodi));

                    int iIndcbrcodi = dr.GetOrdinal(helper.Indcbrcodi);
                    if (!dr.IsDBNull(iIndcbrcodi)) entity.Indcbrcodi = Convert.ToInt32(dr.GetValue(iIndcbrcodi));

                    int iEquicodicentral = dr.GetOrdinal(helper.Equicodicentral);
                    if (!dr.IsDBNull(iEquicodicentral)) entity.Equicodicentral = Convert.ToInt32(dr.GetValue(iEquicodicentral));

                    int iEquicodiunidad = dr.GetOrdinal(helper.Equicodiunidad);
                    if (!dr.IsDBNull(iEquicodiunidad)) entity.Equicodiunidad = Convert.ToInt32(dr.GetValue(iEquicodiunidad));
                    
                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

                    int iCrdsgdtipo = dr.GetOrdinal(helper.Crdsgdtipo);
                    if (!dr.IsDBNull(iCrdsgdtipo)) entity.Crdsgdtipo = Convert.ToInt32(dr.GetValue(iCrdsgdtipo));

                    int iD1 = dr.GetOrdinal(helper.D1);
                    if (!dr.IsDBNull(iD1)) entity.D1 = dr.GetDecimal(iD1);

                    int iD2 = dr.GetOrdinal(helper.D2);
                    if (!dr.IsDBNull(iD2)) entity.D2 = dr.GetDecimal(iD2);

                    int iD3 = dr.GetOrdinal(helper.D3);
                    if (!dr.IsDBNull(iD3)) entity.D3 = dr.GetDecimal(iD3);

                    int iD4 = dr.GetOrdinal(helper.D4);
                    if (!dr.IsDBNull(iD4)) entity.D4 = dr.GetDecimal(iD4);

                    int iD5 = dr.GetOrdinal(helper.D5);
                    if (!dr.IsDBNull(iD5)) entity.D5 = dr.GetDecimal(iD5);

                    int iD6 = dr.GetOrdinal(helper.D6);
                    if (!dr.IsDBNull(iD6)) entity.D6 = dr.GetDecimal(iD6);

                    int iD7 = dr.GetOrdinal(helper.D7);
                    if (!dr.IsDBNull(iD7)) entity.D7 = dr.GetDecimal(iD7);

                    int iD8 = dr.GetOrdinal(helper.D8);
                    if (!dr.IsDBNull(iD8)) entity.D8 = dr.GetDecimal(iD8);

                    int iD9 = dr.GetOrdinal(helper.D9);
                    if (!dr.IsDBNull(iD9)) entity.D9 = dr.GetDecimal(iD9);

                    int iD10 = dr.GetOrdinal(helper.D10);
                    if (!dr.IsDBNull(iD10)) entity.D10 = dr.GetDecimal(iD10);

                    int iD11 = dr.GetOrdinal(helper.D11);
                    if (!dr.IsDBNull(iD11)) entity.D11 = dr.GetDecimal(iD11);

                    int iD12 = dr.GetOrdinal(helper.D12);
                    if (!dr.IsDBNull(iD12)) entity.D12 = dr.GetDecimal(iD12);

                    int iD13 = dr.GetOrdinal(helper.D13);
                    if (!dr.IsDBNull(iD13)) entity.D13 = dr.GetDecimal(iD13);

                    int iD14 = dr.GetOrdinal(helper.D14);
                    if (!dr.IsDBNull(iD14)) entity.D14 = dr.GetDecimal(iD14);

                    int iD15 = dr.GetOrdinal(helper.D15);
                    if (!dr.IsDBNull(iD15)) entity.D15 = dr.GetDecimal(iD15);

                    int iD16 = dr.GetOrdinal(helper.D16);
                    if (!dr.IsDBNull(iD16)) entity.D16 = dr.GetDecimal(iD16);

                    int iD17 = dr.GetOrdinal(helper.D17);
                    if (!dr.IsDBNull(iD17)) entity.D17 = dr.GetDecimal(iD17);

                    int iD18 = dr.GetOrdinal(helper.D18);
                    if (!dr.IsDBNull(iD18)) entity.D18 = dr.GetDecimal(iD18);

                    int iD19 = dr.GetOrdinal(helper.D19);
                    if (!dr.IsDBNull(iD19)) entity.D19 = dr.GetDecimal(iD19);

                    int iD20 = dr.GetOrdinal(helper.D20);
                    if (!dr.IsDBNull(iD20)) entity.D20 = dr.GetDecimal(iD20);

                    int iD21 = dr.GetOrdinal(helper.D21);
                    if (!dr.IsDBNull(iD21)) entity.D21 = dr.GetDecimal(iD21);

                    int iD22 = dr.GetOrdinal(helper.D22);
                    if (!dr.IsDBNull(iD22)) entity.D22 = dr.GetDecimal(iD22);

                    int iD23 = dr.GetOrdinal(helper.D23);
                    if (!dr.IsDBNull(iD23)) entity.D23 = dr.GetDecimal(iD23);

                    int iD24 = dr.GetOrdinal(helper.D24);
                    if (!dr.IsDBNull(iD24)) entity.D24 = dr.GetDecimal(iD24);

                    int iD25 = dr.GetOrdinal(helper.D25);
                    if (!dr.IsDBNull(iD25)) entity.D25 = dr.GetDecimal(iD25);

                    int iD26 = dr.GetOrdinal(helper.D26);
                    if (!dr.IsDBNull(iD26)) entity.D26 = dr.GetDecimal(iD26);

                    int iD27 = dr.GetOrdinal(helper.D27);
                    if (!dr.IsDBNull(iD27)) entity.D27 = dr.GetDecimal(iD27);

                    int iD28 = dr.GetOrdinal(helper.D28);
                    if (!dr.IsDBNull(iD28)) entity.D28 = dr.GetDecimal(iD28);

                    int iD29 = dr.GetOrdinal(helper.D29);
                    if (!dr.IsDBNull(iD29)) entity.D29 = dr.GetDecimal(iD29);

                    int iD30 = dr.GetOrdinal(helper.D30);
                    if (!dr.IsDBNull(iD30)) entity.D30 = dr.GetDecimal(iD30);

                    int iD31 = dr.GetOrdinal(helper.D31);
                    if (!dr.IsDBNull(iD31)) entity.D31 = dr.GetDecimal(iD31);

                    int iE1 = dr.GetOrdinal(helper.E1);
                    if (!dr.IsDBNull(iE1)) entity.E1 = dr.GetString(iE1);

                    int iE2 = dr.GetOrdinal(helper.E2);
                    if (!dr.IsDBNull(iE2)) entity.E2 = dr.GetString(iE2);

                    int iE3 = dr.GetOrdinal(helper.E3);
                    if (!dr.IsDBNull(iE3)) entity.E3 = dr.GetString(iE3);

                    int iE4 = dr.GetOrdinal(helper.E4);
                    if (!dr.IsDBNull(iE4)) entity.E4 = dr.GetString(iE4);

                    int iE5 = dr.GetOrdinal(helper.E5);
                    if (!dr.IsDBNull(iE5)) entity.E5 = dr.GetString(iE5);

                    int iE6 = dr.GetOrdinal(helper.E6);
                    if (!dr.IsDBNull(iE6)) entity.E6 = dr.GetString(iE6);

                    int iE7 = dr.GetOrdinal(helper.E7);
                    if (!dr.IsDBNull(iE7)) entity.E7 = dr.GetString(iE7);

                    int iE8 = dr.GetOrdinal(helper.E8);
                    if (!dr.IsDBNull(iE8)) entity.E8 = dr.GetString(iE8);

                    int iE9 = dr.GetOrdinal(helper.E9);
                    if (!dr.IsDBNull(iE9)) entity.E9 = dr.GetString(iE9);

                    int iE10 = dr.GetOrdinal(helper.E10);
                    if (!dr.IsDBNull(iE10)) entity.E10 = dr.GetString(iE10);

                    int iE11 = dr.GetOrdinal(helper.E11);
                    if (!dr.IsDBNull(iE11)) entity.E11 = dr.GetString(iE11);

                    int iE12 = dr.GetOrdinal(helper.E12);
                    if (!dr.IsDBNull(iE12)) entity.E12 = dr.GetString(iE12);

                    int iE13 = dr.GetOrdinal(helper.E13);
                    if (!dr.IsDBNull(iE13)) entity.E13 = dr.GetString(iE13);

                    int iE14 = dr.GetOrdinal(helper.E14);
                    if (!dr.IsDBNull(iE14)) entity.E14 = dr.GetString(iE14);

                    int iE15 = dr.GetOrdinal(helper.E15);
                    if (!dr.IsDBNull(iE15)) entity.E15 = dr.GetString(iE15);

                    int iE16 = dr.GetOrdinal(helper.E16);
                    if (!dr.IsDBNull(iE16)) entity.E16 = dr.GetString(iE16);

                    int iE17 = dr.GetOrdinal(helper.E17);
                    if (!dr.IsDBNull(iE17)) entity.E17 = dr.GetString(iE17);

                    int iE18 = dr.GetOrdinal(helper.E18);
                    if (!dr.IsDBNull(iE18)) entity.E18 = dr.GetString(iE18);

                    int iE19 = dr.GetOrdinal(helper.E19);
                    if (!dr.IsDBNull(iE19)) entity.E19 = dr.GetString(iE19);

                    int iE20 = dr.GetOrdinal(helper.E20);
                    if (!dr.IsDBNull(iE20)) entity.E20 = dr.GetString(iE20);

                    int iE21 = dr.GetOrdinal(helper.E21);
                    if (!dr.IsDBNull(iE21)) entity.E21 = dr.GetString(iE21);

                    int iE22 = dr.GetOrdinal(helper.E22);
                    if (!dr.IsDBNull(iE22)) entity.E22 = dr.GetString(iE22);

                    int iE23 = dr.GetOrdinal(helper.E23);
                    if (!dr.IsDBNull(iE23)) entity.E23 = dr.GetString(iE23);

                    int iE24 = dr.GetOrdinal(helper.E24);
                    if (!dr.IsDBNull(iE24)) entity.E24 = dr.GetString(iE24);

                    int iE25 = dr.GetOrdinal(helper.E25);
                    if (!dr.IsDBNull(iE25)) entity.E25 = dr.GetString(iE25);

                    int iE26 = dr.GetOrdinal(helper.E26);
                    if (!dr.IsDBNull(iE26)) entity.E26 = dr.GetString(iE26);

                    int iE27 = dr.GetOrdinal(helper.E27);
                    if (!dr.IsDBNull(iE27)) entity.E27 = dr.GetString(iE27);

                    int iE28 = dr.GetOrdinal(helper.E28);
                    if (!dr.IsDBNull(iE28)) entity.E28 = dr.GetString(iE28);

                    int iE29 = dr.GetOrdinal(helper.E29);
                    if (!dr.IsDBNull(iE29)) entity.E29 = dr.GetString(iE29);

                    int iE30 = dr.GetOrdinal(helper.E30);
                    if (!dr.IsDBNull(iE30)) entity.E30 = dr.GetString(iE30);

                    int iE31 = dr.GetOrdinal(helper.E31);
                    if (!dr.IsDBNull(iE31)) entity.E31 = dr.GetString(iE31);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<IndCrdSugadDTO> ListByCriteria(int ipericodi, string emprcodi, string indcbrtipo, string equicodicentral, string equicodiunidad, string grupocodi, string famcodi, string crdsgdtipo)
        {
            List<IndCrdSugadDTO> entitys = new List<IndCrdSugadDTO>();
            string query = string.Format(helper.SqlListByCriteria, ipericodi, emprcodi, indcbrtipo, equicodicentral, equicodiunidad, grupocodi, famcodi, crdsgdtipo);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    IndCrdSugadDTO entity = helper.CreateListByCriteria(dr);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }


        public List<IndCrdSugadDTO> ListCrdSugadJoinCabecera(int emprcodi, int ipericodi, int indcbrtipo)
        {
            IndCrdSugadDTO entity = new IndCrdSugadDTO();
            List<IndCrdSugadDTO> entitys = new List<IndCrdSugadDTO>();
            string query = string.Format(helper.SqlListCrdSugadJoinCabecera, emprcodi, ipericodi, indcbrtipo);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new IndCrdSugadDTO();

                    int iCrdsgdcodi = dr.GetOrdinal(helper.Crdsgdcodi);
                    if (!dr.IsDBNull(iCrdsgdcodi)) entity.Crdsgdcodi = Convert.ToInt32(dr.GetValue(iCrdsgdcodi));

                    int iIndcbrcodi = dr.GetOrdinal(helper.Indcbrcodi);
                    if (!dr.IsDBNull(iIndcbrcodi)) entity.Indcbrcodi = Convert.ToInt32(dr.GetValue(iIndcbrcodi));

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iIpericodi = dr.GetOrdinal(helper.Ipericodi);
                    if (!dr.IsDBNull(iIpericodi)) entity.Ipericodi = Convert.ToInt32(dr.GetValue(iIpericodi));

                    int iIndcbrtipo = dr.GetOrdinal(helper.Indcbrtipo);
                    if (!dr.IsDBNull(iIndcbrtipo)) entity.Indcbrtipo = Convert.ToInt32(dr.GetValue(iIndcbrtipo));

                    int iEquicodicentral = dr.GetOrdinal(helper.Equicodicentral);
                    if (!dr.IsDBNull(iEquicodicentral)) entity.Equicodicentral = Convert.ToInt32(dr.GetValue(iEquicodicentral));

                    int iEquicodiunidad = dr.GetOrdinal(helper.Equicodiunidad);
                    if (!dr.IsDBNull(iEquicodiunidad)) entity.Equicodiunidad = Convert.ToInt32(dr.GetValue(iEquicodiunidad));

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

                    int iCrdsgdtipo = dr.GetOrdinal(helper.Crdsgdtipo);
                    if (!dr.IsDBNull(iCrdsgdtipo)) entity.Crdsgdtipo = Convert.ToInt32(dr.GetValue(iCrdsgdtipo));

                    int iD1 = dr.GetOrdinal(helper.D1);
                    if (!dr.IsDBNull(iD1)) entity.D1 = dr.GetDecimal(iD1);

                    int iD2 = dr.GetOrdinal(helper.D2);
                    if (!dr.IsDBNull(iD2)) entity.D2 = dr.GetDecimal(iD2);

                    int iD3 = dr.GetOrdinal(helper.D3);
                    if (!dr.IsDBNull(iD3)) entity.D3 = dr.GetDecimal(iD3);

                    int iD4 = dr.GetOrdinal(helper.D4);
                    if (!dr.IsDBNull(iD4)) entity.D4 = dr.GetDecimal(iD4);

                    int iD5 = dr.GetOrdinal(helper.D5);
                    if (!dr.IsDBNull(iD5)) entity.D5 = dr.GetDecimal(iD5);

                    int iD6 = dr.GetOrdinal(helper.D6);
                    if (!dr.IsDBNull(iD6)) entity.D6 = dr.GetDecimal(iD6);

                    int iD7 = dr.GetOrdinal(helper.D7);
                    if (!dr.IsDBNull(iD7)) entity.D7 = dr.GetDecimal(iD7);

                    int iD8 = dr.GetOrdinal(helper.D8);
                    if (!dr.IsDBNull(iD8)) entity.D8 = dr.GetDecimal(iD8);

                    int iD9 = dr.GetOrdinal(helper.D9);
                    if (!dr.IsDBNull(iD9)) entity.D9 = dr.GetDecimal(iD9);

                    int iD10 = dr.GetOrdinal(helper.D10);
                    if (!dr.IsDBNull(iD10)) entity.D10 = dr.GetDecimal(iD10);

                    int iD11 = dr.GetOrdinal(helper.D11);
                    if (!dr.IsDBNull(iD11)) entity.D11 = dr.GetDecimal(iD11);

                    int iD12 = dr.GetOrdinal(helper.D12);
                    if (!dr.IsDBNull(iD12)) entity.D12 = dr.GetDecimal(iD12);

                    int iD13 = dr.GetOrdinal(helper.D13);
                    if (!dr.IsDBNull(iD13)) entity.D13 = dr.GetDecimal(iD13);

                    int iD14 = dr.GetOrdinal(helper.D14);
                    if (!dr.IsDBNull(iD14)) entity.D14 = dr.GetDecimal(iD14);

                    int iD15 = dr.GetOrdinal(helper.D15);
                    if (!dr.IsDBNull(iD15)) entity.D15 = dr.GetDecimal(iD15);

                    int iD16 = dr.GetOrdinal(helper.D16);
                    if (!dr.IsDBNull(iD16)) entity.D16 = dr.GetDecimal(iD16);

                    int iD17 = dr.GetOrdinal(helper.D17);
                    if (!dr.IsDBNull(iD17)) entity.D17 = dr.GetDecimal(iD17);

                    int iD18 = dr.GetOrdinal(helper.D18);
                    if (!dr.IsDBNull(iD18)) entity.D18 = dr.GetDecimal(iD18);

                    int iD19 = dr.GetOrdinal(helper.D19);
                    if (!dr.IsDBNull(iD19)) entity.D19 = dr.GetDecimal(iD19);

                    int iD20 = dr.GetOrdinal(helper.D20);
                    if (!dr.IsDBNull(iD20)) entity.D20 = dr.GetDecimal(iD20);

                    int iD21 = dr.GetOrdinal(helper.D21);
                    if (!dr.IsDBNull(iD21)) entity.D21 = dr.GetDecimal(iD21);

                    int iD22 = dr.GetOrdinal(helper.D22);
                    if (!dr.IsDBNull(iD22)) entity.D22 = dr.GetDecimal(iD22);

                    int iD23 = dr.GetOrdinal(helper.D23);
                    if (!dr.IsDBNull(iD23)) entity.D23 = dr.GetDecimal(iD23);

                    int iD24 = dr.GetOrdinal(helper.D24);
                    if (!dr.IsDBNull(iD24)) entity.D24 = dr.GetDecimal(iD24);

                    int iD25 = dr.GetOrdinal(helper.D25);
                    if (!dr.IsDBNull(iD25)) entity.D25 = dr.GetDecimal(iD25);

                    int iD26 = dr.GetOrdinal(helper.D26);
                    if (!dr.IsDBNull(iD26)) entity.D26 = dr.GetDecimal(iD26);

                    int iD27 = dr.GetOrdinal(helper.D27);
                    if (!dr.IsDBNull(iD27)) entity.D27 = dr.GetDecimal(iD27);

                    int iD28 = dr.GetOrdinal(helper.D28);
                    if (!dr.IsDBNull(iD28)) entity.D28 = dr.GetDecimal(iD28);

                    int iD29 = dr.GetOrdinal(helper.D29);
                    if (!dr.IsDBNull(iD29)) entity.D29 = dr.GetDecimal(iD29);

                    int iD30 = dr.GetOrdinal(helper.D30);
                    if (!dr.IsDBNull(iD30)) entity.D30 = dr.GetDecimal(iD30);

                    int iD31 = dr.GetOrdinal(helper.D31);
                    if (!dr.IsDBNull(iD31)) entity.D31 = dr.GetDecimal(iD31);

                    int iE1 = dr.GetOrdinal(helper.E1);
                    if (!dr.IsDBNull(iE1)) entity.E1 = dr.GetString(iE1);

                    int iE2 = dr.GetOrdinal(helper.E2);
                    if (!dr.IsDBNull(iE2)) entity.E2 = dr.GetString(iE2);

                    int iE3 = dr.GetOrdinal(helper.E3);
                    if (!dr.IsDBNull(iE3)) entity.E3 = dr.GetString(iE3);

                    int iE4 = dr.GetOrdinal(helper.E4);
                    if (!dr.IsDBNull(iE4)) entity.E4 = dr.GetString(iE4);

                    int iE5 = dr.GetOrdinal(helper.E5);
                    if (!dr.IsDBNull(iE5)) entity.E5 = dr.GetString(iE5);

                    int iE6 = dr.GetOrdinal(helper.E6);
                    if (!dr.IsDBNull(iE6)) entity.E6 = dr.GetString(iE6);

                    int iE7 = dr.GetOrdinal(helper.E7);
                    if (!dr.IsDBNull(iE7)) entity.E7 = dr.GetString(iE7);

                    int iE8 = dr.GetOrdinal(helper.E8);
                    if (!dr.IsDBNull(iE8)) entity.E8 = dr.GetString(iE8);

                    int iE9 = dr.GetOrdinal(helper.E9);
                    if (!dr.IsDBNull(iE9)) entity.E9 = dr.GetString(iE9);

                    int iE10 = dr.GetOrdinal(helper.E10);
                    if (!dr.IsDBNull(iE10)) entity.E10 = dr.GetString(iE10);

                    int iE11 = dr.GetOrdinal(helper.E11);
                    if (!dr.IsDBNull(iE11)) entity.E11 = dr.GetString(iE11);

                    int iE12 = dr.GetOrdinal(helper.E12);
                    if (!dr.IsDBNull(iE12)) entity.E12 = dr.GetString(iE12);

                    int iE13 = dr.GetOrdinal(helper.E13);
                    if (!dr.IsDBNull(iE13)) entity.E13 = dr.GetString(iE13);

                    int iE14 = dr.GetOrdinal(helper.E14);
                    if (!dr.IsDBNull(iE14)) entity.E14 = dr.GetString(iE14);

                    int iE15 = dr.GetOrdinal(helper.E15);
                    if (!dr.IsDBNull(iE15)) entity.E15 = dr.GetString(iE15);

                    int iE16 = dr.GetOrdinal(helper.E16);
                    if (!dr.IsDBNull(iE16)) entity.E16 = dr.GetString(iE16);

                    int iE17 = dr.GetOrdinal(helper.E17);
                    if (!dr.IsDBNull(iE17)) entity.E17 = dr.GetString(iE17);

                    int iE18 = dr.GetOrdinal(helper.E18);
                    if (!dr.IsDBNull(iE18)) entity.E18 = dr.GetString(iE18);

                    int iE19 = dr.GetOrdinal(helper.E19);
                    if (!dr.IsDBNull(iE19)) entity.E19 = dr.GetString(iE19);

                    int iE20 = dr.GetOrdinal(helper.E20);
                    if (!dr.IsDBNull(iE20)) entity.E20 = dr.GetString(iE20);

                    int iE21 = dr.GetOrdinal(helper.E21);
                    if (!dr.IsDBNull(iE21)) entity.E21 = dr.GetString(iE21);

                    int iE22 = dr.GetOrdinal(helper.E22);
                    if (!dr.IsDBNull(iE22)) entity.E22 = dr.GetString(iE22);

                    int iE23 = dr.GetOrdinal(helper.E23);
                    if (!dr.IsDBNull(iE23)) entity.E23 = dr.GetString(iE23);

                    int iE24 = dr.GetOrdinal(helper.E24);
                    if (!dr.IsDBNull(iE24)) entity.E24 = dr.GetString(iE24);

                    int iE25 = dr.GetOrdinal(helper.E25);
                    if (!dr.IsDBNull(iE25)) entity.E25 = dr.GetString(iE25);

                    int iE26 = dr.GetOrdinal(helper.E26);
                    if (!dr.IsDBNull(iE26)) entity.E26 = dr.GetString(iE26);

                    int iE27 = dr.GetOrdinal(helper.E27);
                    if (!dr.IsDBNull(iE27)) entity.E27 = dr.GetString(iE27);

                    int iE28 = dr.GetOrdinal(helper.E28);
                    if (!dr.IsDBNull(iE28)) entity.E28 = dr.GetString(iE28);

                    int iE29 = dr.GetOrdinal(helper.E29);
                    if (!dr.IsDBNull(iE29)) entity.E29 = dr.GetString(iE29);

                    int iE30 = dr.GetOrdinal(helper.E30);
                    if (!dr.IsDBNull(iE30)) entity.E30 = dr.GetString(iE30);

                    int iE31 = dr.GetOrdinal(helper.E31);
                    if (!dr.IsDBNull(iE31)) entity.E31 = dr.GetString(iE31);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<IndCrdSugadDTO> ListCumplimientoDiario(int emprcodi, int ipericodi, int indcbrtipo)
        {
            IndCrdSugadDTO entity = new IndCrdSugadDTO();
            List<IndCrdSugadDTO> entitys = new List<IndCrdSugadDTO>();
            string query = string.Format(helper.SqlListCumplimientoDiario, emprcodi, ipericodi, indcbrtipo);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new IndCrdSugadDTO();

                    int iCrdsgdcodi = dr.GetOrdinal(helper.Crdsgdcodi);
                    if (!dr.IsDBNull(iCrdsgdcodi)) entity.Crdsgdcodi = Convert.ToInt32(dr.GetValue(iCrdsgdcodi));

                    int iIndcbrfeccreacion = dr.GetOrdinal(helper.Indcbrfeccreacion);
                    if (!dr.IsDBNull(iIndcbrfeccreacion)) entity.Indcbrfeccreacion = dr.GetDateTime(iIndcbrfeccreacion);

                    int iIndcbrfecha = dr.GetOrdinal(helper.Indcbrfecha);
                    if (!dr.IsDBNull(iIndcbrfecha)) entity.Indcbrfecha = dr.GetString(iIndcbrfecha);

                    int iIndcbrusucreacion = dr.GetOrdinal(helper.Indcbrusucreacion);
                    if (!dr.IsDBNull(iIndcbrusucreacion)) entity.Indcbrusucreacion = dr.GetString(iIndcbrusucreacion);

                    int iIndcbrcodi = dr.GetOrdinal(helper.Indcbrcodi);
                    if (!dr.IsDBNull(iIndcbrcodi)) entity.Indcbrcodi = Convert.ToInt32(dr.GetValue(iIndcbrcodi));

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iIpericodi = dr.GetOrdinal(helper.Ipericodi);
                    if (!dr.IsDBNull(iIpericodi)) entity.Ipericodi = Convert.ToInt32(dr.GetValue(iIpericodi));

                    int iIndcbrtipo = dr.GetOrdinal(helper.Indcbrtipo);
                    if (!dr.IsDBNull(iIndcbrtipo)) entity.Indcbrtipo = Convert.ToInt32(dr.GetValue(iIndcbrtipo));

                    int iEquicodicentral = dr.GetOrdinal(helper.Equicodicentral);
                    if (!dr.IsDBNull(iEquicodicentral)) entity.Equicodicentral = Convert.ToInt32(dr.GetValue(iEquicodicentral));

                    int iEquinombcentral = dr.GetOrdinal(helper.Equinombcentral);
                    if (!dr.IsDBNull(iEquinombcentral)) entity.Equinombcentral = dr.GetString(iEquinombcentral);

                    int iEquicodiunidad = dr.GetOrdinal(helper.Equicodiunidad);
                    if (!dr.IsDBNull(iEquicodiunidad)) entity.Equicodiunidad = Convert.ToInt32(dr.GetValue(iEquicodiunidad));

                    int iEquinombunidad = dr.GetOrdinal(helper.Equinombunidad);
                    if (!dr.IsDBNull(iEquinombunidad)) entity.Equinombunidad = dr.GetString(iEquinombunidad);

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

                    int iCrdsgdtipo = dr.GetOrdinal(helper.Crdsgdtipo);
                    if (!dr.IsDBNull(iCrdsgdtipo)) entity.Crdsgdtipo = Convert.ToInt32(dr.GetValue(iCrdsgdtipo));

                    int iE1 = dr.GetOrdinal(helper.E1);
                    if (!dr.IsDBNull(iE1)) entity.E1 = dr.GetString(iE1);

                    int iE2 = dr.GetOrdinal(helper.E2);
                    if (!dr.IsDBNull(iE2)) entity.E2 = dr.GetString(iE2);

                    int iE3 = dr.GetOrdinal(helper.E3);
                    if (!dr.IsDBNull(iE3)) entity.E3 = dr.GetString(iE3);

                    int iE4 = dr.GetOrdinal(helper.E4);
                    if (!dr.IsDBNull(iE4)) entity.E4 = dr.GetString(iE4);

                    int iE5 = dr.GetOrdinal(helper.E5);
                    if (!dr.IsDBNull(iE5)) entity.E5 = dr.GetString(iE5);

                    int iE6 = dr.GetOrdinal(helper.E6);
                    if (!dr.IsDBNull(iE6)) entity.E6 = dr.GetString(iE6);

                    int iE7 = dr.GetOrdinal(helper.E7);
                    if (!dr.IsDBNull(iE7)) entity.E7 = dr.GetString(iE7);

                    int iE8 = dr.GetOrdinal(helper.E8);
                    if (!dr.IsDBNull(iE8)) entity.E8 = dr.GetString(iE8);

                    int iE9 = dr.GetOrdinal(helper.E9);
                    if (!dr.IsDBNull(iE9)) entity.E9 = dr.GetString(iE9);

                    int iE10 = dr.GetOrdinal(helper.E10);
                    if (!dr.IsDBNull(iE10)) entity.E10 = dr.GetString(iE10);

                    int iE11 = dr.GetOrdinal(helper.E11);
                    if (!dr.IsDBNull(iE11)) entity.E11 = dr.GetString(iE11);

                    int iE12 = dr.GetOrdinal(helper.E12);
                    if (!dr.IsDBNull(iE12)) entity.E12 = dr.GetString(iE12);

                    int iE13 = dr.GetOrdinal(helper.E13);
                    if (!dr.IsDBNull(iE13)) entity.E13 = dr.GetString(iE13);

                    int iE14 = dr.GetOrdinal(helper.E14);
                    if (!dr.IsDBNull(iE14)) entity.E14 = dr.GetString(iE14);

                    int iE15 = dr.GetOrdinal(helper.E15);
                    if (!dr.IsDBNull(iE15)) entity.E15 = dr.GetString(iE15);

                    int iE16 = dr.GetOrdinal(helper.E16);
                    if (!dr.IsDBNull(iE16)) entity.E16 = dr.GetString(iE16);

                    int iE17 = dr.GetOrdinal(helper.E17);
                    if (!dr.IsDBNull(iE17)) entity.E17 = dr.GetString(iE17);

                    int iE18 = dr.GetOrdinal(helper.E18);
                    if (!dr.IsDBNull(iE18)) entity.E18 = dr.GetString(iE18);

                    int iE19 = dr.GetOrdinal(helper.E19);
                    if (!dr.IsDBNull(iE19)) entity.E19 = dr.GetString(iE19);

                    int iE20 = dr.GetOrdinal(helper.E20);
                    if (!dr.IsDBNull(iE20)) entity.E20 = dr.GetString(iE20);

                    int iE21 = dr.GetOrdinal(helper.E21);
                    if (!dr.IsDBNull(iE21)) entity.E21 = dr.GetString(iE21);

                    int iE22 = dr.GetOrdinal(helper.E22);
                    if (!dr.IsDBNull(iE22)) entity.E22 = dr.GetString(iE22);

                    int iE23 = dr.GetOrdinal(helper.E23);
                    if (!dr.IsDBNull(iE23)) entity.E23 = dr.GetString(iE23);

                    int iE24 = dr.GetOrdinal(helper.E24);
                    if (!dr.IsDBNull(iE24)) entity.E24 = dr.GetString(iE24);

                    int iE25 = dr.GetOrdinal(helper.E25);
                    if (!dr.IsDBNull(iE25)) entity.E25 = dr.GetString(iE25);

                    int iE26 = dr.GetOrdinal(helper.E26);
                    if (!dr.IsDBNull(iE26)) entity.E26 = dr.GetString(iE26);

                    int iE27 = dr.GetOrdinal(helper.E27);
                    if (!dr.IsDBNull(iE27)) entity.E27 = dr.GetString(iE27);

                    int iE28 = dr.GetOrdinal(helper.E28);
                    if (!dr.IsDBNull(iE28)) entity.E28 = dr.GetString(iE28);

                    int iE29 = dr.GetOrdinal(helper.E29);
                    if (!dr.IsDBNull(iE29)) entity.E29 = dr.GetString(iE29);

                    int iE30 = dr.GetOrdinal(helper.E30);
                    if (!dr.IsDBNull(iE30)) entity.E30 = dr.GetString(iE30);

                    int iE31 = dr.GetOrdinal(helper.E31);
                    if (!dr.IsDBNull(iE31)) entity.E31 = dr.GetString(iE31);

                    int iCumplimiento = dr.GetOrdinal(helper.Cumplimiento);
                    if (!dr.IsDBNull(iCumplimiento)) entity.Cumplimiento = dr.GetString(iCumplimiento);

                    int iPorcentaje = dr.GetOrdinal(helper.Porcentaje);
                    if (!dr.IsDBNull(iPorcentaje)) entity.Porcentaje = dr.GetDecimal(iPorcentaje);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
