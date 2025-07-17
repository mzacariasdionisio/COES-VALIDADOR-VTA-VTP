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
    public class GmmValEnergiaRepository : RepositoryBase, IGmmValEnergiaRepository
    {
        public GmmValEnergiaRepository(string strConn)
            : base(strConn)
        {

        }

        GmmValEnergiaHelper helper = new GmmValEnergiaHelper();
        
        public int Save(GmmValEnergiaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            #region Mapeo de campos
            dbProvider.AddInParameter(command, helper.VALOCODI, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.PERICODI, DbType.Int32, entity.PERICODI);
            dbProvider.AddInParameter(command, helper.EMPRCODI, DbType.Int32, entity.EMPRCODI);
            dbProvider.AddInParameter(command, helper.EMPGCODI, DbType.Int32, entity.EMPGCODI);
            dbProvider.AddInParameter(command, helper.BARRCODI, DbType.Int32, entity.BARRCODI);
            dbProvider.AddInParameter(command, helper.CASDDBBARRA, DbType.String, entity.CASDDBBARRA);
            dbProvider.AddInParameter(command, helper.PTOMEDICODI, DbType.Int32, entity.PTOMEDICODI);
            dbProvider.AddInParameter(command, helper.TPTOMEDICODI, DbType.Int32, entity.TPTOMEDICODI);
            dbProvider.AddInParameter(command, helper.TIPOINFOCODI, DbType.Int32, entity.TIPOINFOCODI);
            dbProvider.AddInParameter(command, helper.MEDIFECHA, DbType.String, Convert.ToDateTime(entity.MEDIFECHA).ToString("yyyy-MM-dd"));
            dbProvider.AddInParameter(command, helper.LECTCODI, DbType.Int32, entity.LECTCODI);
            dbProvider.AddInParameter(command, helper.VALANIO, DbType.Int32, entity.VALANIO);
            dbProvider.AddInParameter(command, helper.VALMES, DbType.Int32, entity.VALMES);
            dbProvider.AddInParameter(command, helper.VALOVALOR1, DbType.Decimal, entity.VALOVALOR1);
            dbProvider.AddInParameter(command, helper.VALOVALOR2, DbType.Decimal, entity.VALOVALOR2);
            dbProvider.AddInParameter(command, helper.VALOVALOR3, DbType.Decimal, entity.VALOVALOR3);
            dbProvider.AddInParameter(command, helper.VALOVALOR4, DbType.Decimal, entity.VALOVALOR4);
            dbProvider.AddInParameter(command, helper.VALOVALOR5, DbType.Decimal, entity.VALOVALOR5);
            dbProvider.AddInParameter(command, helper.VALOVALOR6, DbType.Decimal, entity.VALOVALOR6);
            dbProvider.AddInParameter(command, helper.VALOVALOR7, DbType.Decimal, entity.VALOVALOR7);
            dbProvider.AddInParameter(command, helper.VALOVALOR8, DbType.Decimal, entity.VALOVALOR8);
            dbProvider.AddInParameter(command, helper.VALOVALOR9, DbType.Decimal, entity.VALOVALOR9);
            dbProvider.AddInParameter(command, helper.VALOVALOR10, DbType.Decimal, entity.VALOVALOR10);
            dbProvider.AddInParameter(command, helper.VALOVALOR11, DbType.Decimal, entity.VALOVALOR11);
            dbProvider.AddInParameter(command, helper.VALOVALOR12, DbType.Decimal, entity.VALOVALOR12);
            dbProvider.AddInParameter(command, helper.VALOVALOR13, DbType.Decimal, entity.VALOVALOR13);
            dbProvider.AddInParameter(command, helper.VALOVALOR14, DbType.Decimal, entity.VALOVALOR14);
            dbProvider.AddInParameter(command, helper.VALOVALOR15, DbType.Decimal, entity.VALOVALOR15);
            dbProvider.AddInParameter(command, helper.VALOVALOR16, DbType.Decimal, entity.VALOVALOR16);
            dbProvider.AddInParameter(command, helper.VALOVALOR17, DbType.Decimal, entity.VALOVALOR17);
            dbProvider.AddInParameter(command, helper.VALOVALOR18, DbType.Decimal, entity.VALOVALOR18);
            dbProvider.AddInParameter(command, helper.VALOVALOR19, DbType.Decimal, entity.VALOVALOR19);
            dbProvider.AddInParameter(command, helper.VALOVALOR20, DbType.Decimal, entity.VALOVALOR20);
            dbProvider.AddInParameter(command, helper.VALOVALOR21, DbType.Decimal, entity.VALOVALOR21);
            dbProvider.AddInParameter(command, helper.VALOVALOR22, DbType.Decimal, entity.VALOVALOR22);
            dbProvider.AddInParameter(command, helper.VALOVALOR23, DbType.Decimal, entity.VALOVALOR23);
            dbProvider.AddInParameter(command, helper.VALOVALOR24, DbType.Decimal, entity.VALOVALOR24);
            dbProvider.AddInParameter(command, helper.VALOVALOR25, DbType.Decimal, entity.VALOVALOR25);
            dbProvider.AddInParameter(command, helper.VALOVALOR26, DbType.Decimal, entity.VALOVALOR26);
            dbProvider.AddInParameter(command, helper.VALOVALOR27, DbType.Decimal, entity.VALOVALOR27);
            dbProvider.AddInParameter(command, helper.VALOVALOR28, DbType.Decimal, entity.VALOVALOR28);
            dbProvider.AddInParameter(command, helper.VALOVALOR29, DbType.Decimal, entity.VALOVALOR29);
            dbProvider.AddInParameter(command, helper.VALOVALOR30, DbType.Decimal, entity.VALOVALOR30);
            dbProvider.AddInParameter(command, helper.VALOVALOR31, DbType.Decimal, entity.VALOVALOR31);
            dbProvider.AddInParameter(command, helper.VALOVALOR32, DbType.Decimal, entity.VALOVALOR32);
            dbProvider.AddInParameter(command, helper.VALOVALOR33, DbType.Decimal, entity.VALOVALOR33);
            dbProvider.AddInParameter(command, helper.VALOVALOR34, DbType.Decimal, entity.VALOVALOR34);
            dbProvider.AddInParameter(command, helper.VALOVALOR35, DbType.Decimal, entity.VALOVALOR35);
            dbProvider.AddInParameter(command, helper.VALOVALOR36, DbType.Decimal, entity.VALOVALOR36);
            dbProvider.AddInParameter(command, helper.VALOVALOR37, DbType.Decimal, entity.VALOVALOR37);
            dbProvider.AddInParameter(command, helper.VALOVALOR38, DbType.Decimal, entity.VALOVALOR38);
            dbProvider.AddInParameter(command, helper.VALOVALOR39, DbType.Decimal, entity.VALOVALOR39);
            dbProvider.AddInParameter(command, helper.VALOVALOR40, DbType.Decimal, entity.VALOVALOR40);
            dbProvider.AddInParameter(command, helper.VALOVALOR41, DbType.Decimal, entity.VALOVALOR41);
            dbProvider.AddInParameter(command, helper.VALOVALOR42, DbType.Decimal, entity.VALOVALOR42);
            dbProvider.AddInParameter(command, helper.VALOVALOR43, DbType.Decimal, entity.VALOVALOR43);
            dbProvider.AddInParameter(command, helper.VALOVALOR44, DbType.Decimal, entity.VALOVALOR44);
            dbProvider.AddInParameter(command, helper.VALOVALOR45, DbType.Decimal, entity.VALOVALOR45);
            dbProvider.AddInParameter(command, helper.VALOVALOR46, DbType.Decimal, entity.VALOVALOR46);
            dbProvider.AddInParameter(command, helper.VALOVALOR47, DbType.Decimal, entity.VALOVALOR47);
            dbProvider.AddInParameter(command, helper.VALOVALOR48, DbType.Decimal, entity.VALOVALOR48);
            dbProvider.AddInParameter(command, helper.VALOVALOR49, DbType.Decimal, entity.VALOVALOR49);
            dbProvider.AddInParameter(command, helper.VALOVALOR50, DbType.Decimal, entity.VALOVALOR50);
            dbProvider.AddInParameter(command, helper.VALOVALOR51, DbType.Decimal, entity.VALOVALOR51);
            dbProvider.AddInParameter(command, helper.VALOVALOR52, DbType.Decimal, entity.VALOVALOR52);
            dbProvider.AddInParameter(command, helper.VALOVALOR53, DbType.Decimal, entity.VALOVALOR53);
            dbProvider.AddInParameter(command, helper.VALOVALOR54, DbType.Decimal, entity.VALOVALOR54);
            dbProvider.AddInParameter(command, helper.VALOVALOR55, DbType.Decimal, entity.VALOVALOR55);
            dbProvider.AddInParameter(command, helper.VALOVALOR56, DbType.Decimal, entity.VALOVALOR56);
            dbProvider.AddInParameter(command, helper.VALOVALOR57, DbType.Decimal, entity.VALOVALOR57);
            dbProvider.AddInParameter(command, helper.VALOVALOR58, DbType.Decimal, entity.VALOVALOR58);
            dbProvider.AddInParameter(command, helper.VALOVALOR59, DbType.Decimal, entity.VALOVALOR59);
            dbProvider.AddInParameter(command, helper.VALOVALOR60, DbType.Decimal, entity.VALOVALOR60);
            dbProvider.AddInParameter(command, helper.VALOVALOR61, DbType.Decimal, entity.VALOVALOR61);
            dbProvider.AddInParameter(command, helper.VALOVALOR62, DbType.Decimal, entity.VALOVALOR62);
            dbProvider.AddInParameter(command, helper.VALOVALOR63, DbType.Decimal, entity.VALOVALOR63);
            dbProvider.AddInParameter(command, helper.VALOVALOR64, DbType.Decimal, entity.VALOVALOR64);
            dbProvider.AddInParameter(command, helper.VALOVALOR65, DbType.Decimal, entity.VALOVALOR65);
            dbProvider.AddInParameter(command, helper.VALOVALOR66, DbType.Decimal, entity.VALOVALOR66);
            dbProvider.AddInParameter(command, helper.VALOVALOR67, DbType.Decimal, entity.VALOVALOR67);
            dbProvider.AddInParameter(command, helper.VALOVALOR68, DbType.Decimal, entity.VALOVALOR68);
            dbProvider.AddInParameter(command, helper.VALOVALOR69, DbType.Decimal, entity.VALOVALOR69);
            dbProvider.AddInParameter(command, helper.VALOVALOR70, DbType.Decimal, entity.VALOVALOR70);
            dbProvider.AddInParameter(command, helper.VALOVALOR71, DbType.Decimal, entity.VALOVALOR71);
            dbProvider.AddInParameter(command, helper.VALOVALOR72, DbType.Decimal, entity.VALOVALOR72);
            dbProvider.AddInParameter(command, helper.VALOVALOR73, DbType.Decimal, entity.VALOVALOR73);
            dbProvider.AddInParameter(command, helper.VALOVALOR74, DbType.Decimal, entity.VALOVALOR74);
            dbProvider.AddInParameter(command, helper.VALOVALOR75, DbType.Decimal, entity.VALOVALOR75);
            dbProvider.AddInParameter(command, helper.VALOVALOR76, DbType.Decimal, entity.VALOVALOR76);
            dbProvider.AddInParameter(command, helper.VALOVALOR77, DbType.Decimal, entity.VALOVALOR77);
            dbProvider.AddInParameter(command, helper.VALOVALOR78, DbType.Decimal, entity.VALOVALOR78);
            dbProvider.AddInParameter(command, helper.VALOVALOR79, DbType.Decimal, entity.VALOVALOR79);
            dbProvider.AddInParameter(command, helper.VALOVALOR80, DbType.Decimal, entity.VALOVALOR80);
            dbProvider.AddInParameter(command, helper.VALOVALOR81, DbType.Decimal, entity.VALOVALOR81);
            dbProvider.AddInParameter(command, helper.VALOVALOR82, DbType.Decimal, entity.VALOVALOR82);
            dbProvider.AddInParameter(command, helper.VALOVALOR83, DbType.Decimal, entity.VALOVALOR83);
            dbProvider.AddInParameter(command, helper.VALOVALOR84, DbType.Decimal, entity.VALOVALOR84);
            dbProvider.AddInParameter(command, helper.VALOVALOR85, DbType.Decimal, entity.VALOVALOR85);
            dbProvider.AddInParameter(command, helper.VALOVALOR86, DbType.Decimal, entity.VALOVALOR86);
            dbProvider.AddInParameter(command, helper.VALOVALOR87, DbType.Decimal, entity.VALOVALOR87);
            dbProvider.AddInParameter(command, helper.VALOVALOR88, DbType.Decimal, entity.VALOVALOR88);
            dbProvider.AddInParameter(command, helper.VALOVALOR89, DbType.Decimal, entity.VALOVALOR89);
            dbProvider.AddInParameter(command, helper.VALOVALOR90, DbType.Decimal, entity.VALOVALOR90);
            dbProvider.AddInParameter(command, helper.VALOVALOR91, DbType.Decimal, entity.VALOVALOR91);
            dbProvider.AddInParameter(command, helper.VALOVALOR92, DbType.Decimal, entity.VALOVALOR92);
            dbProvider.AddInParameter(command, helper.VALOVALOR93, DbType.Decimal, entity.VALOVALOR93);
            dbProvider.AddInParameter(command, helper.VALOVALOR94, DbType.Decimal, entity.VALOVALOR94);
            dbProvider.AddInParameter(command, helper.VALOVALOR95, DbType.Decimal, entity.VALOVALOR95);
            dbProvider.AddInParameter(command, helper.VALOVALOR96, DbType.Decimal, entity.VALOVALOR96);

            dbProvider.AddInParameter(command, helper.VALOVALORCM1, DbType.Decimal, entity.VALOVALORCM1);
            dbProvider.AddInParameter(command, helper.VALOVALORCM2, DbType.Decimal, entity.VALOVALORCM2);
            dbProvider.AddInParameter(command, helper.VALOVALORCM3, DbType.Decimal, entity.VALOVALORCM3);
            dbProvider.AddInParameter(command, helper.VALOVALORCM4, DbType.Decimal, entity.VALOVALORCM4);
            dbProvider.AddInParameter(command, helper.VALOVALORCM5, DbType.Decimal, entity.VALOVALORCM5);
            dbProvider.AddInParameter(command, helper.VALOVALORCM6, DbType.Decimal, entity.VALOVALORCM6);
            dbProvider.AddInParameter(command, helper.VALOVALORCM7, DbType.Decimal, entity.VALOVALORCM7);
            dbProvider.AddInParameter(command, helper.VALOVALORCM8, DbType.Decimal, entity.VALOVALORCM8);
            dbProvider.AddInParameter(command, helper.VALOVALORCM9, DbType.Decimal, entity.VALOVALORCM9);
            dbProvider.AddInParameter(command, helper.VALOVALORCM10, DbType.Decimal, entity.VALOVALORCM10);
            dbProvider.AddInParameter(command, helper.VALOVALORCM11, DbType.Decimal, entity.VALOVALORCM11);
            dbProvider.AddInParameter(command, helper.VALOVALORCM12, DbType.Decimal, entity.VALOVALORCM12);
            dbProvider.AddInParameter(command, helper.VALOVALORCM13, DbType.Decimal, entity.VALOVALORCM13);
            dbProvider.AddInParameter(command, helper.VALOVALORCM14, DbType.Decimal, entity.VALOVALORCM14);
            dbProvider.AddInParameter(command, helper.VALOVALORCM15, DbType.Decimal, entity.VALOVALORCM15);
            dbProvider.AddInParameter(command, helper.VALOVALORCM16, DbType.Decimal, entity.VALOVALORCM16);
            dbProvider.AddInParameter(command, helper.VALOVALORCM17, DbType.Decimal, entity.VALOVALORCM17);
            dbProvider.AddInParameter(command, helper.VALOVALORCM18, DbType.Decimal, entity.VALOVALORCM18);
            dbProvider.AddInParameter(command, helper.VALOVALORCM19, DbType.Decimal, entity.VALOVALORCM19);
            dbProvider.AddInParameter(command, helper.VALOVALORCM20, DbType.Decimal, entity.VALOVALORCM20);
            dbProvider.AddInParameter(command, helper.VALOVALORCM21, DbType.Decimal, entity.VALOVALORCM21);
            dbProvider.AddInParameter(command, helper.VALOVALORCM22, DbType.Decimal, entity.VALOVALORCM22);
            dbProvider.AddInParameter(command, helper.VALOVALORCM23, DbType.Decimal, entity.VALOVALORCM23);
            dbProvider.AddInParameter(command, helper.VALOVALORCM24, DbType.Decimal, entity.VALOVALORCM24);
            dbProvider.AddInParameter(command, helper.VALOVALORCM25, DbType.Decimal, entity.VALOVALORCM25);
            dbProvider.AddInParameter(command, helper.VALOVALORCM26, DbType.Decimal, entity.VALOVALORCM26);
            dbProvider.AddInParameter(command, helper.VALOVALORCM27, DbType.Decimal, entity.VALOVALORCM27);
            dbProvider.AddInParameter(command, helper.VALOVALORCM28, DbType.Decimal, entity.VALOVALORCM28);
            dbProvider.AddInParameter(command, helper.VALOVALORCM29, DbType.Decimal, entity.VALOVALORCM29);
            dbProvider.AddInParameter(command, helper.VALOVALORCM30, DbType.Decimal, entity.VALOVALORCM30);
            dbProvider.AddInParameter(command, helper.VALOVALORCM31, DbType.Decimal, entity.VALOVALORCM31);
            dbProvider.AddInParameter(command, helper.VALOVALORCM32, DbType.Decimal, entity.VALOVALORCM32);
            dbProvider.AddInParameter(command, helper.VALOVALORCM33, DbType.Decimal, entity.VALOVALORCM33);
            dbProvider.AddInParameter(command, helper.VALOVALORCM34, DbType.Decimal, entity.VALOVALORCM34);
            dbProvider.AddInParameter(command, helper.VALOVALORCM35, DbType.Decimal, entity.VALOVALORCM35);
            dbProvider.AddInParameter(command, helper.VALOVALORCM36, DbType.Decimal, entity.VALOVALORCM36);
            dbProvider.AddInParameter(command, helper.VALOVALORCM37, DbType.Decimal, entity.VALOVALORCM37);
            dbProvider.AddInParameter(command, helper.VALOVALORCM38, DbType.Decimal, entity.VALOVALORCM38);
            dbProvider.AddInParameter(command, helper.VALOVALORCM39, DbType.Decimal, entity.VALOVALORCM39);
            dbProvider.AddInParameter(command, helper.VALOVALORCM40, DbType.Decimal, entity.VALOVALORCM40);
            dbProvider.AddInParameter(command, helper.VALOVALORCM41, DbType.Decimal, entity.VALOVALORCM41);
            dbProvider.AddInParameter(command, helper.VALOVALORCM42, DbType.Decimal, entity.VALOVALORCM42);
            dbProvider.AddInParameter(command, helper.VALOVALORCM43, DbType.Decimal, entity.VALOVALORCM43);
            dbProvider.AddInParameter(command, helper.VALOVALORCM44, DbType.Decimal, entity.VALOVALORCM44);
            dbProvider.AddInParameter(command, helper.VALOVALORCM45, DbType.Decimal, entity.VALOVALORCM45);
            dbProvider.AddInParameter(command, helper.VALOVALORCM46, DbType.Decimal, entity.VALOVALORCM46);
            dbProvider.AddInParameter(command, helper.VALOVALORCM47, DbType.Decimal, entity.VALOVALORCM47);
            dbProvider.AddInParameter(command, helper.VALOVALORCM48, DbType.Decimal, entity.VALOVALORCM48);
            dbProvider.AddInParameter(command, helper.VALOVALORCM49, DbType.Decimal, entity.VALOVALORCM49);
            dbProvider.AddInParameter(command, helper.VALOVALORCM50, DbType.Decimal, entity.VALOVALORCM50);
            dbProvider.AddInParameter(command, helper.VALOVALORCM51, DbType.Decimal, entity.VALOVALORCM51);
            dbProvider.AddInParameter(command, helper.VALOVALORCM52, DbType.Decimal, entity.VALOVALORCM52);
            dbProvider.AddInParameter(command, helper.VALOVALORCM53, DbType.Decimal, entity.VALOVALORCM53);
            dbProvider.AddInParameter(command, helper.VALOVALORCM54, DbType.Decimal, entity.VALOVALORCM54);
            dbProvider.AddInParameter(command, helper.VALOVALORCM55, DbType.Decimal, entity.VALOVALORCM55);
            dbProvider.AddInParameter(command, helper.VALOVALORCM56, DbType.Decimal, entity.VALOVALORCM56);
            dbProvider.AddInParameter(command, helper.VALOVALORCM57, DbType.Decimal, entity.VALOVALORCM57);
            dbProvider.AddInParameter(command, helper.VALOVALORCM58, DbType.Decimal, entity.VALOVALORCM58);
            dbProvider.AddInParameter(command, helper.VALOVALORCM59, DbType.Decimal, entity.VALOVALORCM59);
            dbProvider.AddInParameter(command, helper.VALOVALORCM60, DbType.Decimal, entity.VALOVALORCM60);
            dbProvider.AddInParameter(command, helper.VALOVALORCM61, DbType.Decimal, entity.VALOVALORCM61);
            dbProvider.AddInParameter(command, helper.VALOVALORCM62, DbType.Decimal, entity.VALOVALORCM62);
            dbProvider.AddInParameter(command, helper.VALOVALORCM63, DbType.Decimal, entity.VALOVALORCM63);
            dbProvider.AddInParameter(command, helper.VALOVALORCM64, DbType.Decimal, entity.VALOVALORCM64);
            dbProvider.AddInParameter(command, helper.VALOVALORCM65, DbType.Decimal, entity.VALOVALORCM65);
            dbProvider.AddInParameter(command, helper.VALOVALORCM66, DbType.Decimal, entity.VALOVALORCM66);
            dbProvider.AddInParameter(command, helper.VALOVALORCM67, DbType.Decimal, entity.VALOVALORCM67);
            dbProvider.AddInParameter(command, helper.VALOVALORCM68, DbType.Decimal, entity.VALOVALORCM68);
            dbProvider.AddInParameter(command, helper.VALOVALORCM69, DbType.Decimal, entity.VALOVALORCM69);
            dbProvider.AddInParameter(command, helper.VALOVALORCM70, DbType.Decimal, entity.VALOVALORCM70);
            dbProvider.AddInParameter(command, helper.VALOVALORCM71, DbType.Decimal, entity.VALOVALORCM71);
            dbProvider.AddInParameter(command, helper.VALOVALORCM72, DbType.Decimal, entity.VALOVALORCM72);
            dbProvider.AddInParameter(command, helper.VALOVALORCM73, DbType.Decimal, entity.VALOVALORCM73);
            dbProvider.AddInParameter(command, helper.VALOVALORCM74, DbType.Decimal, entity.VALOVALORCM74);
            dbProvider.AddInParameter(command, helper.VALOVALORCM75, DbType.Decimal, entity.VALOVALORCM75);
            dbProvider.AddInParameter(command, helper.VALOVALORCM76, DbType.Decimal, entity.VALOVALORCM76);
            dbProvider.AddInParameter(command, helper.VALOVALORCM77, DbType.Decimal, entity.VALOVALORCM77);
            dbProvider.AddInParameter(command, helper.VALOVALORCM78, DbType.Decimal, entity.VALOVALORCM78);
            dbProvider.AddInParameter(command, helper.VALOVALORCM79, DbType.Decimal, entity.VALOVALORCM79);
            dbProvider.AddInParameter(command, helper.VALOVALORCM80, DbType.Decimal, entity.VALOVALORCM80);
            dbProvider.AddInParameter(command, helper.VALOVALORCM81, DbType.Decimal, entity.VALOVALORCM81);
            dbProvider.AddInParameter(command, helper.VALOVALORCM82, DbType.Decimal, entity.VALOVALORCM82);
            dbProvider.AddInParameter(command, helper.VALOVALORCM83, DbType.Decimal, entity.VALOVALORCM83);
            dbProvider.AddInParameter(command, helper.VALOVALORCM84, DbType.Decimal, entity.VALOVALORCM84);
            dbProvider.AddInParameter(command, helper.VALOVALORCM85, DbType.Decimal, entity.VALOVALORCM85);
            dbProvider.AddInParameter(command, helper.VALOVALORCM86, DbType.Decimal, entity.VALOVALORCM86);
            dbProvider.AddInParameter(command, helper.VALOVALORCM87, DbType.Decimal, entity.VALOVALORCM87);
            dbProvider.AddInParameter(command, helper.VALOVALORCM88, DbType.Decimal, entity.VALOVALORCM88);
            dbProvider.AddInParameter(command, helper.VALOVALORCM89, DbType.Decimal, entity.VALOVALORCM89);
            dbProvider.AddInParameter(command, helper.VALOVALORCM90, DbType.Decimal, entity.VALOVALORCM90);
            dbProvider.AddInParameter(command, helper.VALOVALORCM91, DbType.Decimal, entity.VALOVALORCM91);
            dbProvider.AddInParameter(command, helper.VALOVALORCM92, DbType.Decimal, entity.VALOVALORCM92);
            dbProvider.AddInParameter(command, helper.VALOVALORCM93, DbType.Decimal, entity.VALOVALORCM93);
            dbProvider.AddInParameter(command, helper.VALOVALORCM94, DbType.Decimal, entity.VALOVALORCM94);
            dbProvider.AddInParameter(command, helper.VALOVALORCM95, DbType.Decimal, entity.VALOVALORCM95);
            dbProvider.AddInParameter(command, helper.VALOVALORCM96, DbType.Decimal, entity.VALOVALORCM96);

            dbProvider.AddInParameter(command, helper.VALUSUCREACION, DbType.String, entity.VALUSUCREACION);
            #endregion

            var iRslt = dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public List<GmmValEnergiaDTO> ListarValores96Originales(GmmValEnergiaDTO valEnergiaDTO)
        {
            List<GmmValEnergiaDTO> entities = new List<GmmValEnergiaDTO>();
            int mes = valEnergiaDTO.VALMES; int anio = valEnergiaDTO.VALANIO;
            String per1 = anio.ToString() + (mes).ToString(); 
            String per2 = anio.ToString() + (mes + 1).ToString();
            String per3 = anio.ToString() + (mes + 2).ToString();
            if (mes >= 11)
                {
                    switch (mes)
                    {
                        case 11:
                            per3 = (anio + 1).ToString() + (1).ToString();
                            break;
                        case 12:
                            per2 = (anio + 1).ToString() + (1).ToString();
                            per3 = (anio + 1).ToString() + (2).ToString();
                            break;
                    }

             }

            int formato = valEnergiaDTO.formatoEnergiaTrimestral;
            if (valEnergiaDTO.EmpgPrimerMes != true)
            {
                //per3 = "0";
                formato = valEnergiaDTO.formatoEnergiaMensual;
            }
            
            string queryString = string.Format(helper.SqlListarValores96Originales,
               formato, valEnergiaDTO.EMPRCODI, valEnergiaDTO.PTOMEDICODI, per1, per2, per3);

            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entities.Add(helper.CreateListaValores(dr));
                }
            }

            return entities;
        }

        public List<GmmValEnergiaDTO> ListarValoresCostoMarginal(GmmValEnergiaDTO valEnergiaDTO, int anio, int mes)
        {
            List<GmmValEnergiaDTO> entities = new List<GmmValEnergiaDTO>();

            // Obtener primero la semana y codigo de envío más reciente
            int SEMANCODI = 0; int ENVIOCODI = 0;

            string queryString = string.Format(helper.SqlGetSemana,
               Convert.ToDateTime(valEnergiaDTO.MEDIFECHA).ToString("yyyy-MM-dd"));
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            IDataReader dr;
            using ( dr = dbProvider.ExecuteReader(command)) { 
                dr.Read();
                SEMANCODI = Convert.ToInt32(dr.GetValue(dr.GetOrdinal("semanacodi")));
                //ASSETEC 20220420 - El valor ENVIOCODI que se devuelve aqui no siempre es similar a la consulta siguiente (313 - toma mas de un minuto) por lo general devuelve el mismo valor.
                //Pero cuando el valor de ENVIOCODI en la siguiente consulta es distinto, ya no trae datos en la tercera sentencia.
                //La linea 310 se agrego y [312-318] se comento
                ENVIOCODI = Convert.ToInt32(dr.GetValue(dr.GetOrdinal("enviocodi")));
            }
            /*
            queryString = string.Format(helper.SqlGetEnvio, anio, mes);
            command = dbProvider.GetSqlStringCommand(queryString);
            using (dr = dbProvider.ExecuteReader(command)) {
                dr.Read();
                ENVIOCODI = Convert.ToInt32(dr.GetValue(dr.GetOrdinal("enviocodi")));
            }*/
            // Valores de Costo Marginal
            queryString = string.Format(helper.SqlListarValoresCostoMarginal, ENVIOCODI, ENVIOCODI, ENVIOCODI, ENVIOCODI,
               valEnergiaDTO.CASDDBBARRA, SEMANCODI);

            command = dbProvider.GetSqlStringCommand(queryString);
            using (dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entities.Add(helper.CreateListaValoresCostoMarginal(dr));
                }
            }
            return entities;
        }
    }
}
