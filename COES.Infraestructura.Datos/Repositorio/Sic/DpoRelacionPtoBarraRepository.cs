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
    public class DpoRelacionPtoBarraRepository : RepositoryBase, IDpoRelacionPtoBarraRepository
    {
        public DpoRelacionPtoBarraRepository(string strConn) : base(strConn)
        {
        }

        DpoRelacionPtoBarraHelper helper = new DpoRelacionPtoBarraHelper();

        public void Save(DpoRelacionPtoBarraDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);
            dbProvider.AddInParameter(command, helper.Ptobarcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Splfrmcodi, DbType.Int32, entity.Splfrmcodi);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.Ptobarusucreacion, DbType.String, entity.Ptobarusucreacion);
            dbProvider.AddInParameter(command, helper.Ptobarfeccreacion, DbType.DateTime, entity.Ptobarfeccreacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(DpoRelacionPtoBarraDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.Ptobarusumodificacion, DbType.String, entity.Ptobarusumodificacion);
            dbProvider.AddInParameter(command, helper.Ptobarfecmodificacion, DbType.DateTime, entity.Ptobarfecmodificacion);
            dbProvider.AddInParameter(command, helper.Ptobarcodi, DbType.Int32, entity.Ptobarcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int codi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Ptobarcodi, DbType.Int32, codi);

            dbProvider.ExecuteNonQuery(command);
        }

        public List<DpoRelacionPtoBarraDTO> List()
        {
            List<DpoRelacionPtoBarraDTO> entitys = new List<DpoRelacionPtoBarraDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    DpoRelacionPtoBarraDTO entity = new DpoRelacionPtoBarraDTO();

                    int iPtobarcodi = dr.GetOrdinal(helper.Ptobarcodi);
                    if (!dr.IsDBNull(iPtobarcodi)) entity.Ptobarcodi = Convert.ToInt32(dr.GetValue(iPtobarcodi));

                    int iSplfrmcodi = dr.GetOrdinal(helper.Splfrmcodi);
                    if (!dr.IsDBNull(iSplfrmcodi)) entity.Splfrmcodi = Convert.ToInt32(dr.GetValue(iSplfrmcodi));

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iPtobarusucreacion = dr.GetOrdinal(helper.Ptobarusucreacion);
                    if (!dr.IsDBNull(iPtobarusucreacion)) entity.Ptobarusucreacion = dr.GetString(iPtobarusucreacion);

                    int iPtobarfeccreacion = dr.GetOrdinal(helper.Ptobarfeccreacion);
                    if (!dr.IsDBNull(iPtobarfeccreacion)) entity.Ptobarfeccreacion = dr.GetDateTime(iPtobarfeccreacion);

                    int iPtobarusumodificacion = dr.GetOrdinal(helper.Ptobarusumodificacion);
                    if (!dr.IsDBNull(iPtobarusumodificacion)) entity.Ptobarusumodificacion = dr.GetString(iPtobarusumodificacion);

                    int iPtobarfecmodificacion = dr.GetOrdinal(helper.Ptobarfecmodificacion);
                    if (!dr.IsDBNull(iPtobarfecmodificacion)) entity.Ptobarfecmodificacion = dr.GetDateTime(iPtobarfecmodificacion);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public DpoRelacionPtoBarraDTO GetById(int codi)
        {
            DpoRelacionPtoBarraDTO entity = new DpoRelacionPtoBarraDTO();

            string query = string.Format(helper.SqlGetById, codi);

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

        public DpoRelacionPtoBarraDTO GetPuntoById(int codi)
        {
            DpoRelacionPtoBarraDTO entity = new DpoRelacionPtoBarraDTO();

            string query = string.Format(helper.SqlGetPuntoById, codi);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<DpoRelacionPtoBarraDTO> ListBarraPuntoVersion(int version)
        {
            List<DpoRelacionPtoBarraDTO> entitys = new List<DpoRelacionPtoBarraDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListBarraPuntoVersion);
            dbProvider.AddInParameter(command, helper.Dposplcodi, DbType.Int32, version);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    DpoRelacionPtoBarraDTO entity = new DpoRelacionPtoBarraDTO();

                    int iPtobarcodi = dr.GetOrdinal(helper.Ptobarcodi);
                    if (!dr.IsDBNull(iPtobarcodi)) entity.Ptobarcodi = Convert.ToInt32(dr.GetValue(iPtobarcodi));

                    int iDposplcodi = dr.GetOrdinal(helper.Dposplcodi);
                    if (!dr.IsDBNull(iDposplcodi)) entity.Dposplcodi = Convert.ToInt32(dr.GetValue(iDposplcodi));

                    int iDposplnombre = dr.GetOrdinal(helper.Dposplnombre);
                    if (!dr.IsDBNull(iDposplnombre)) entity.Dposplnombre = dr.GetString(iDposplnombre);

                    int iSplfrmcodi = dr.GetOrdinal(helper.Splfrmcodi);
                    if (!dr.IsDBNull(iSplfrmcodi)) entity.Splfrmcodi = Convert.ToInt32(dr.GetValue(iSplfrmcodi));

                    int iBarsplcodi = dr.GetOrdinal(helper.Barsplcodi);
                    if (!dr.IsDBNull(iBarsplcodi)) entity.Barsplcodi = Convert.ToInt32(dr.GetValue(iBarsplcodi));

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    int iGrupoabrev = dr.GetOrdinal(helper.Grupoabrev);
                    if (!dr.IsDBNull(iGrupoabrev)) entity.Grupoabrev = dr.GetString(iGrupoabrev);

                    int iPtomedicodifveg = dr.GetOrdinal(helper.Ptomedicodifveg);
                    if (!dr.IsDBNull(iPtomedicodifveg)) entity.Ptomedicodifveg = Convert.ToInt32(dr.GetValue(iPtomedicodifveg));

                    int iNombvegetativa = dr.GetOrdinal(helper.Nombvegetativa);
                    if (!dr.IsDBNull(iNombvegetativa)) entity.Nombvegetativa = dr.GetString(iNombvegetativa);

                    int iPtomedicodiful = dr.GetOrdinal(helper.Ptomedicodiful);
                    if (!dr.IsDBNull(iPtomedicodiful)) entity.Ptomedicodiful = Convert.ToInt32(dr.GetValue(iPtomedicodiful));

                    int iNombindustrial = dr.GetOrdinal(helper.Nombindustrial);
                    if (!dr.IsDBNull(iNombindustrial)) entity.Nombindustrial = dr.GetString(iNombindustrial);

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iPtomedidesc = dr.GetOrdinal(helper.Ptomedidesc);
                    if (!dr.IsDBNull(iPtomedidesc)) entity.Ptomedidesc = dr.GetString(iPtomedidesc);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<DpoRelacionPtoBarraDTO> ListPuntoRelacionBarra(int codigo, string inicio, string fin)
        {
            List<DpoRelacionPtoBarraDTO> entitys = new List<DpoRelacionPtoBarraDTO>();
           
            string query = string.Format(helper.SqlListPuntoRelacionBarra, codigo, inicio, fin);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    DpoRelacionPtoBarraDTO entity = new DpoRelacionPtoBarraDTO();

                    int iMedifecha = dr.GetOrdinal(helper.Medifecha);
                    if (!dr.IsDBNull(iMedifecha)) entity.Medifecha = dr.GetDateTime(iMedifecha);

                    int ih1 = dr.GetOrdinal(helper.H1);
                    if (!dr.IsDBNull(ih1)) entity.H1 = dr.GetDecimal(ih1);

                    int ih2 = dr.GetOrdinal(helper.H2);
                    if (!dr.IsDBNull(ih2)) entity.H2 = dr.GetDecimal(ih2);

                    int ih3 = dr.GetOrdinal(helper.H3);
                    if (!dr.IsDBNull(ih3)) entity.H3 = dr.GetDecimal(ih3);

                    int ih4 = dr.GetOrdinal(helper.H4);
                    if (!dr.IsDBNull(ih4)) entity.H4 = dr.GetDecimal(ih4);

                    int ih5 = dr.GetOrdinal(helper.H5);
                    if (!dr.IsDBNull(ih5)) entity.H5 = dr.GetDecimal(ih5);

                    int ih6 = dr.GetOrdinal(helper.H6);
                    if (!dr.IsDBNull(ih6)) entity.H6 = dr.GetDecimal(ih6);

                    int ih7 = dr.GetOrdinal(helper.H7);
                    if (!dr.IsDBNull(ih7)) entity.H7 = dr.GetDecimal(ih7);

                    int ih8 = dr.GetOrdinal(helper.H8);
                    if (!dr.IsDBNull(ih8)) entity.H8 = dr.GetDecimal(ih8);

                    int ih9 = dr.GetOrdinal(helper.H9);
                    if (!dr.IsDBNull(ih9)) entity.H9 = dr.GetDecimal(ih9);

                    int ih10 = dr.GetOrdinal(helper.H10);
                    if (!dr.IsDBNull(ih10)) entity.H10 = dr.GetDecimal(ih10);

                    int ih11 = dr.GetOrdinal(helper.H11);
                    if (!dr.IsDBNull(ih11)) entity.H11 = dr.GetDecimal(ih11);

                    int ih12 = dr.GetOrdinal(helper.H12);
                    if (!dr.IsDBNull(ih12)) entity.H12 = dr.GetDecimal(ih12);

                    int ih13 = dr.GetOrdinal(helper.H13);
                    if (!dr.IsDBNull(ih13)) entity.H13 = dr.GetDecimal(ih13);

                    int ih14 = dr.GetOrdinal(helper.H14);
                    if (!dr.IsDBNull(ih14)) entity.H14 = dr.GetDecimal(ih14);

                    int ih15 = dr.GetOrdinal(helper.H15);
                    if (!dr.IsDBNull(ih15)) entity.H15 = dr.GetDecimal(ih15);

                    int ih16 = dr.GetOrdinal(helper.H16);
                    if (!dr.IsDBNull(ih16)) entity.H16 = dr.GetDecimal(ih16);

                    int ih17 = dr.GetOrdinal(helper.H17);
                    if (!dr.IsDBNull(ih17)) entity.H17 = dr.GetDecimal(ih17);

                    int ih18 = dr.GetOrdinal(helper.H18);
                    if (!dr.IsDBNull(ih18)) entity.H18 = dr.GetDecimal(ih18);

                    int ih19 = dr.GetOrdinal(helper.H19);
                    if (!dr.IsDBNull(ih19)) entity.H19 = dr.GetDecimal(ih19);

                    int ih20 = dr.GetOrdinal(helper.H20);
                    if (!dr.IsDBNull(ih20)) entity.H20 = dr.GetDecimal(ih20);

                    int ih21 = dr.GetOrdinal(helper.H21);
                    if (!dr.IsDBNull(ih21)) entity.H21 = dr.GetDecimal(ih21);

                    int ih22 = dr.GetOrdinal(helper.H22);
                    if (!dr.IsDBNull(ih22)) entity.H22 = dr.GetDecimal(ih22);

                    int ih23 = dr.GetOrdinal(helper.H23);
                    if (!dr.IsDBNull(ih23)) entity.H23 = dr.GetDecimal(ih23);

                    int ih24 = dr.GetOrdinal(helper.H24);
                    if (!dr.IsDBNull(ih24)) entity.H24 = dr.GetDecimal(ih24);

                    int ih25 = dr.GetOrdinal(helper.H25);
                    if (!dr.IsDBNull(ih25)) entity.H25 = dr.GetDecimal(ih25);

                    int ih26 = dr.GetOrdinal(helper.H26);
                    if (!dr.IsDBNull(ih26)) entity.H26 = dr.GetDecimal(ih26);

                    int ih27 = dr.GetOrdinal(helper.H27);
                    if (!dr.IsDBNull(ih27)) entity.H27 = dr.GetDecimal(ih27);

                    int ih28 = dr.GetOrdinal(helper.H28);
                    if (!dr.IsDBNull(ih28)) entity.H28 = dr.GetDecimal(ih28);

                    int ih29 = dr.GetOrdinal(helper.H29);
                    if (!dr.IsDBNull(ih29)) entity.H29 = dr.GetDecimal(ih29);

                    int ih30 = dr.GetOrdinal(helper.H30);
                    if (!dr.IsDBNull(ih30)) entity.H30 = dr.GetDecimal(ih30);

                    int ih31 = dr.GetOrdinal(helper.H31);
                    if (!dr.IsDBNull(ih31)) entity.H31 = dr.GetDecimal(ih31);

                    int ih32 = dr.GetOrdinal(helper.H32);
                    if (!dr.IsDBNull(ih32)) entity.H32 = dr.GetDecimal(ih32);

                    int ih33 = dr.GetOrdinal(helper.H33);
                    if (!dr.IsDBNull(ih33)) entity.H33 = dr.GetDecimal(ih33);

                    int ih34 = dr.GetOrdinal(helper.H34);
                    if (!dr.IsDBNull(ih34)) entity.H34 = dr.GetDecimal(ih34);

                    int ih35 = dr.GetOrdinal(helper.H35);
                    if (!dr.IsDBNull(ih35)) entity.H35 = dr.GetDecimal(ih35);

                    int ih36 = dr.GetOrdinal(helper.H36);
                    if (!dr.IsDBNull(ih36)) entity.H36 = dr.GetDecimal(ih36);

                    int ih37 = dr.GetOrdinal(helper.H37);
                    if (!dr.IsDBNull(ih37)) entity.H37 = dr.GetDecimal(ih37);

                    int ih38 = dr.GetOrdinal(helper.H38);
                    if (!dr.IsDBNull(ih38)) entity.H38 = dr.GetDecimal(ih38);

                    int ih39 = dr.GetOrdinal(helper.H39);
                    if (!dr.IsDBNull(ih39)) entity.H39 = dr.GetDecimal(ih39);

                    int ih40 = dr.GetOrdinal(helper.H40);
                    if (!dr.IsDBNull(ih40)) entity.H40 = dr.GetDecimal(ih40);

                    int ih41 = dr.GetOrdinal(helper.H41);
                    if (!dr.IsDBNull(ih41)) entity.H41 = dr.GetDecimal(ih41);

                    int ih42 = dr.GetOrdinal(helper.H42);
                    if (!dr.IsDBNull(ih42)) entity.H42 = dr.GetDecimal(ih42);

                    int ih43 = dr.GetOrdinal(helper.H43);
                    if (!dr.IsDBNull(ih43)) entity.H43 = dr.GetDecimal(ih43);

                    int ih44 = dr.GetOrdinal(helper.H44);
                    if (!dr.IsDBNull(ih44)) entity.H44 = dr.GetDecimal(ih44);

                    int ih45 = dr.GetOrdinal(helper.H45);
                    if (!dr.IsDBNull(ih45)) entity.H45 = dr.GetDecimal(ih45);

                    int ih46 = dr.GetOrdinal(helper.H46);
                    if (!dr.IsDBNull(ih46)) entity.H46 = dr.GetDecimal(ih46);

                    int ih47 = dr.GetOrdinal(helper.H47);
                    if (!dr.IsDBNull(ih47)) entity.H47 = dr.GetDecimal(ih47);

                    int ih48 = dr.GetOrdinal(helper.H48);
                    if (!dr.IsDBNull(ih48)) entity.H48 = dr.GetDecimal(ih48);

                    int ih49 = dr.GetOrdinal(helper.H49);
                    if (!dr.IsDBNull(ih49)) entity.H49 = dr.GetDecimal(ih49);

                    int ih50 = dr.GetOrdinal(helper.H50);
                    if (!dr.IsDBNull(ih50)) entity.H50 = dr.GetDecimal(ih50);

                    int ih51 = dr.GetOrdinal(helper.H51);
                    if (!dr.IsDBNull(ih51)) entity.H51 = dr.GetDecimal(ih51);

                    int ih52 = dr.GetOrdinal(helper.H52);
                    if (!dr.IsDBNull(ih50)) entity.H52 = dr.GetDecimal(ih52);

                    int ih53 = dr.GetOrdinal(helper.H53);
                    if (!dr.IsDBNull(ih53)) entity.H53 = dr.GetDecimal(ih53);

                    int ih54 = dr.GetOrdinal(helper.H54);
                    if (!dr.IsDBNull(ih54)) entity.H54 = dr.GetDecimal(ih54);

                    int ih55 = dr.GetOrdinal(helper.H55);
                    if (!dr.IsDBNull(ih55)) entity.H55 = dr.GetDecimal(ih55);

                    int ih56 = dr.GetOrdinal(helper.H56);
                    if (!dr.IsDBNull(ih56)) entity.H56 = dr.GetDecimal(ih56);

                    int ih57 = dr.GetOrdinal(helper.H57);
                    if (!dr.IsDBNull(ih57)) entity.H57 = dr.GetDecimal(ih57);

                    int ih58 = dr.GetOrdinal(helper.H58);
                    if (!dr.IsDBNull(ih58)) entity.H58 = dr.GetDecimal(ih58);

                    int ih59 = dr.GetOrdinal(helper.H59);
                    if (!dr.IsDBNull(ih59)) entity.H59 = dr.GetDecimal(ih59);

                    int ih60 = dr.GetOrdinal(helper.H60);
                    if (!dr.IsDBNull(ih60)) entity.H60 = dr.GetDecimal(ih60);

                    int ih61 = dr.GetOrdinal(helper.H61);
                    if (!dr.IsDBNull(ih61)) entity.H61 = dr.GetDecimal(ih61);

                    int ih62 = dr.GetOrdinal(helper.H62);
                    if (!dr.IsDBNull(ih62)) entity.H62 = dr.GetDecimal(ih62);

                    int ih63 = dr.GetOrdinal(helper.H63);
                    if (!dr.IsDBNull(ih63)) entity.H63 = dr.GetDecimal(ih63);

                    int ih64 = dr.GetOrdinal(helper.H64);
                    if (!dr.IsDBNull(ih64)) entity.H64 = dr.GetDecimal(ih64);

                    int ih65 = dr.GetOrdinal(helper.H65);
                    if (!dr.IsDBNull(ih65)) entity.H65 = dr.GetDecimal(ih65);

                    int ih66 = dr.GetOrdinal(helper.H66);
                    if (!dr.IsDBNull(ih66)) entity.H66 = dr.GetDecimal(ih66);

                    int ih67 = dr.GetOrdinal(helper.H67);
                    if (!dr.IsDBNull(ih67)) entity.H67 = dr.GetDecimal(ih67);

                    int ih68 = dr.GetOrdinal(helper.H68);
                    if (!dr.IsDBNull(ih68)) entity.H68 = dr.GetDecimal(ih68);

                    int ih69 = dr.GetOrdinal(helper.H69);
                    if (!dr.IsDBNull(ih69)) entity.H69 = dr.GetDecimal(ih69);

                    int ih70 = dr.GetOrdinal(helper.H70);
                    if (!dr.IsDBNull(ih70)) entity.H70 = dr.GetDecimal(ih70);

                    int ih71 = dr.GetOrdinal(helper.H71);
                    if (!dr.IsDBNull(ih71)) entity.H71 = dr.GetDecimal(ih71);

                    int ih72 = dr.GetOrdinal(helper.H72);
                    if (!dr.IsDBNull(ih72)) entity.H72 = dr.GetDecimal(ih72);

                    int ih73 = dr.GetOrdinal(helper.H73);
                    if (!dr.IsDBNull(ih73)) entity.H73 = dr.GetDecimal(ih73);

                    int ih74 = dr.GetOrdinal(helper.H74);
                    if (!dr.IsDBNull(ih74)) entity.H74 = dr.GetDecimal(ih74);

                    int ih75 = dr.GetOrdinal(helper.H75);
                    if (!dr.IsDBNull(ih75)) entity.H75 = dr.GetDecimal(ih75);

                    int ih76 = dr.GetOrdinal(helper.H76);
                    if (!dr.IsDBNull(ih76)) entity.H76 = dr.GetDecimal(ih76);

                    int ih77 = dr.GetOrdinal(helper.H77);
                    if (!dr.IsDBNull(ih77)) entity.H77 = dr.GetDecimal(ih77);

                    int ih78 = dr.GetOrdinal(helper.H78);
                    if (!dr.IsDBNull(ih78)) entity.H78 = dr.GetDecimal(ih78);

                    int ih79 = dr.GetOrdinal(helper.H79);
                    if (!dr.IsDBNull(ih79)) entity.H79 = dr.GetDecimal(ih79);

                    int ih80 = dr.GetOrdinal(helper.H80);
                    if (!dr.IsDBNull(ih80)) entity.H80 = dr.GetDecimal(ih80);

                    int ih81 = dr.GetOrdinal(helper.H81);
                    if (!dr.IsDBNull(ih81)) entity.H81 = dr.GetDecimal(ih81);

                    int ih82 = dr.GetOrdinal(helper.H82);
                    if (!dr.IsDBNull(ih82)) entity.H82 = dr.GetDecimal(ih82);

                    int ih83 = dr.GetOrdinal(helper.H83);
                    if (!dr.IsDBNull(ih83)) entity.H83 = dr.GetDecimal(ih83);

                    int ih84 = dr.GetOrdinal(helper.H84);
                    if (!dr.IsDBNull(ih84)) entity.H84 = dr.GetDecimal(ih84);

                    int ih85 = dr.GetOrdinal(helper.H85);
                    if (!dr.IsDBNull(ih85)) entity.H85 = dr.GetDecimal(ih85);

                    int ih86 = dr.GetOrdinal(helper.H86);
                    if (!dr.IsDBNull(ih86)) entity.H86 = dr.GetDecimal(ih86);

                    int ih87 = dr.GetOrdinal(helper.H87);
                    if (!dr.IsDBNull(ih87)) entity.H87 = dr.GetDecimal(ih87);

                    int ih88 = dr.GetOrdinal(helper.H88);
                    if (!dr.IsDBNull(ih88)) entity.H88 = dr.GetDecimal(ih88);

                    int ih89 = dr.GetOrdinal(helper.H89);
                    if (!dr.IsDBNull(ih89)) entity.H89 = dr.GetDecimal(ih89);

                    int ih90 = dr.GetOrdinal(helper.H90);
                    if (!dr.IsDBNull(ih90)) entity.H90 = dr.GetDecimal(ih90);

                    int ih91 = dr.GetOrdinal(helper.H91);
                    if (!dr.IsDBNull(ih91)) entity.H91 = dr.GetDecimal(ih91);

                    int ih92 = dr.GetOrdinal(helper.H92);
                    if (!dr.IsDBNull(ih92)) entity.H92 = dr.GetDecimal(ih92);

                    int ih93 = dr.GetOrdinal(helper.H93);
                    if (!dr.IsDBNull(ih93)) entity.H93 = dr.GetDecimal(ih93);

                    int ih94 = dr.GetOrdinal(helper.H94);
                    if (!dr.IsDBNull(ih94)) entity.H94 = dr.GetDecimal(ih94);

                    int ih95 = dr.GetOrdinal(helper.H95);
                    if (!dr.IsDBNull(ih95)) entity.H95 = dr.GetDecimal(ih95);

                    int ih96 = dr.GetOrdinal(helper.H96);
                    if (!dr.IsDBNull(ih96)) entity.H96 = dr.GetDecimal(ih96);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

    }
}
