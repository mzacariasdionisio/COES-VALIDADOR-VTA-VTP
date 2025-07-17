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
    public class GmmValEnergiaEntregaRepository : RepositoryBase, IGmmValEnergiaEntregaRepository
    {
        public GmmValEnergiaEntregaRepository(string strConn)
            : base(strConn)
        {

        }

        GmmValEnergiaEntregaHelper helper = new GmmValEnergiaEntregaHelper();
        
        public int Save(GmmValEnergiaEntregaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            #region Mapeo de campos
            dbProvider.AddInParameter(command, helper.VALENECODI, DbType.Int32, id);
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
            dbProvider.AddInParameter(command, helper.VALENEVALENEVAL1, DbType.Decimal, entity.VALENEVALENEVAL1);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVAL2, DbType.Decimal, entity.VALENEVALENEVAL2);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVAL3, DbType.Decimal, entity.VALENEVALENEVAL3);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVAL4, DbType.Decimal, entity.VALENEVALENEVAL4);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVAL5, DbType.Decimal, entity.VALENEVALENEVAL5);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVAL6, DbType.Decimal, entity.VALENEVALENEVAL6);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVAL7, DbType.Decimal, entity.VALENEVALENEVAL7);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVAL8, DbType.Decimal, entity.VALENEVALENEVAL8);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVAL9, DbType.Decimal, entity.VALENEVALENEVAL9);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVAL10, DbType.Decimal, entity.VALENEVALENEVAL10);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVAL11, DbType.Decimal, entity.VALENEVALENEVAL11);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVAL12, DbType.Decimal, entity.VALENEVALENEVAL12);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVAL13, DbType.Decimal, entity.VALENEVALENEVAL13);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVAL14, DbType.Decimal, entity.VALENEVALENEVAL14);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVAL15, DbType.Decimal, entity.VALENEVALENEVAL15);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVAL16, DbType.Decimal, entity.VALENEVALENEVAL16);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVAL17, DbType.Decimal, entity.VALENEVALENEVAL17);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVAL18, DbType.Decimal, entity.VALENEVALENEVAL18);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVAL19, DbType.Decimal, entity.VALENEVALENEVAL19);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVAL20, DbType.Decimal, entity.VALENEVALENEVAL20);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVAL21, DbType.Decimal, entity.VALENEVALENEVAL21);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVAL22, DbType.Decimal, entity.VALENEVALENEVAL22);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVAL23, DbType.Decimal, entity.VALENEVALENEVAL23);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVAL24, DbType.Decimal, entity.VALENEVALENEVAL24);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVAL25, DbType.Decimal, entity.VALENEVALENEVAL25);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVAL26, DbType.Decimal, entity.VALENEVALENEVAL26);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVAL27, DbType.Decimal, entity.VALENEVALENEVAL27);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVAL28, DbType.Decimal, entity.VALENEVALENEVAL28);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVAL29, DbType.Decimal, entity.VALENEVALENEVAL29);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVAL30, DbType.Decimal, entity.VALENEVALENEVAL30);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVAL31, DbType.Decimal, entity.VALENEVALENEVAL31);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVAL32, DbType.Decimal, entity.VALENEVALENEVAL32);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVAL33, DbType.Decimal, entity.VALENEVALENEVAL33);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVAL34, DbType.Decimal, entity.VALENEVALENEVAL34);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVAL35, DbType.Decimal, entity.VALENEVALENEVAL35);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVAL36, DbType.Decimal, entity.VALENEVALENEVAL36);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVAL37, DbType.Decimal, entity.VALENEVALENEVAL37);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVAL38, DbType.Decimal, entity.VALENEVALENEVAL38);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVAL39, DbType.Decimal, entity.VALENEVALENEVAL39);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVAL40, DbType.Decimal, entity.VALENEVALENEVAL40);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVAL41, DbType.Decimal, entity.VALENEVALENEVAL41);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVAL42, DbType.Decimal, entity.VALENEVALENEVAL42);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVAL43, DbType.Decimal, entity.VALENEVALENEVAL43);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVAL44, DbType.Decimal, entity.VALENEVALENEVAL44);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVAL45, DbType.Decimal, entity.VALENEVALENEVAL45);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVAL46, DbType.Decimal, entity.VALENEVALENEVAL46);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVAL47, DbType.Decimal, entity.VALENEVALENEVAL47);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVAL48, DbType.Decimal, entity.VALENEVALENEVAL48);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVAL49, DbType.Decimal, entity.VALENEVALENEVAL49);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVAL50, DbType.Decimal, entity.VALENEVALENEVAL50);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVAL51, DbType.Decimal, entity.VALENEVALENEVAL51);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVAL52, DbType.Decimal, entity.VALENEVALENEVAL52);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVAL53, DbType.Decimal, entity.VALENEVALENEVAL53);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVAL54, DbType.Decimal, entity.VALENEVALENEVAL54);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVAL55, DbType.Decimal, entity.VALENEVALENEVAL55);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVAL56, DbType.Decimal, entity.VALENEVALENEVAL56);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVAL57, DbType.Decimal, entity.VALENEVALENEVAL57);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVAL58, DbType.Decimal, entity.VALENEVALENEVAL58);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVAL59, DbType.Decimal, entity.VALENEVALENEVAL59);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVAL60, DbType.Decimal, entity.VALENEVALENEVAL60);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVAL61, DbType.Decimal, entity.VALENEVALENEVAL61);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVAL62, DbType.Decimal, entity.VALENEVALENEVAL62);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVAL63, DbType.Decimal, entity.VALENEVALENEVAL63);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVAL64, DbType.Decimal, entity.VALENEVALENEVAL64);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVAL65, DbType.Decimal, entity.VALENEVALENEVAL65);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVAL66, DbType.Decimal, entity.VALENEVALENEVAL66);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVAL67, DbType.Decimal, entity.VALENEVALENEVAL67);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVAL68, DbType.Decimal, entity.VALENEVALENEVAL68);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVAL69, DbType.Decimal, entity.VALENEVALENEVAL69);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVAL70, DbType.Decimal, entity.VALENEVALENEVAL70);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVAL71, DbType.Decimal, entity.VALENEVALENEVAL71);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVAL72, DbType.Decimal, entity.VALENEVALENEVAL72);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVAL73, DbType.Decimal, entity.VALENEVALENEVAL73);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVAL74, DbType.Decimal, entity.VALENEVALENEVAL74);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVAL75, DbType.Decimal, entity.VALENEVALENEVAL75);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVAL76, DbType.Decimal, entity.VALENEVALENEVAL76);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVAL77, DbType.Decimal, entity.VALENEVALENEVAL77);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVAL78, DbType.Decimal, entity.VALENEVALENEVAL78);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVAL79, DbType.Decimal, entity.VALENEVALENEVAL79);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVAL80, DbType.Decimal, entity.VALENEVALENEVAL80);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVAL81, DbType.Decimal, entity.VALENEVALENEVAL81);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVAL82, DbType.Decimal, entity.VALENEVALENEVAL82);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVAL83, DbType.Decimal, entity.VALENEVALENEVAL83);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVAL84, DbType.Decimal, entity.VALENEVALENEVAL84);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVAL85, DbType.Decimal, entity.VALENEVALENEVAL85);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVAL86, DbType.Decimal, entity.VALENEVALENEVAL86);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVAL87, DbType.Decimal, entity.VALENEVALENEVAL87);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVAL88, DbType.Decimal, entity.VALENEVALENEVAL88);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVAL89, DbType.Decimal, entity.VALENEVALENEVAL89);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVAL90, DbType.Decimal, entity.VALENEVALENEVAL90);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVAL91, DbType.Decimal, entity.VALENEVALENEVAL91);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVAL92, DbType.Decimal, entity.VALENEVALENEVAL92);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVAL93, DbType.Decimal, entity.VALENEVALENEVAL93);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVAL94, DbType.Decimal, entity.VALENEVALENEVAL94);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVAL95, DbType.Decimal, entity.VALENEVALENEVAL95);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVAL96, DbType.Decimal, entity.VALENEVALENEVAL96);

            dbProvider.AddInParameter(command, helper.VALENEVALENEVALCM1, DbType.Decimal, entity.VALENEVALENEVALCM1);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVALCM2, DbType.Decimal, entity.VALENEVALENEVALCM2);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVALCM3, DbType.Decimal, entity.VALENEVALENEVALCM3);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVALCM4, DbType.Decimal, entity.VALENEVALENEVALCM4);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVALCM5, DbType.Decimal, entity.VALENEVALENEVALCM5);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVALCM6, DbType.Decimal, entity.VALENEVALENEVALCM6);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVALCM7, DbType.Decimal, entity.VALENEVALENEVALCM7);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVALCM8, DbType.Decimal, entity.VALENEVALENEVALCM8);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVALCM9, DbType.Decimal, entity.VALENEVALENEVALCM9);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVALCM10, DbType.Decimal, entity.VALENEVALENEVALCM10);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVALCM11, DbType.Decimal, entity.VALENEVALENEVALCM11);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVALCM12, DbType.Decimal, entity.VALENEVALENEVALCM12);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVALCM13, DbType.Decimal, entity.VALENEVALENEVALCM13);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVALCM14, DbType.Decimal, entity.VALENEVALENEVALCM14);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVALCM15, DbType.Decimal, entity.VALENEVALENEVALCM15);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVALCM16, DbType.Decimal, entity.VALENEVALENEVALCM16);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVALCM17, DbType.Decimal, entity.VALENEVALENEVALCM17);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVALCM18, DbType.Decimal, entity.VALENEVALENEVALCM18);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVALCM19, DbType.Decimal, entity.VALENEVALENEVALCM19);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVALCM20, DbType.Decimal, entity.VALENEVALENEVALCM20);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVALCM21, DbType.Decimal, entity.VALENEVALENEVALCM21);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVALCM22, DbType.Decimal, entity.VALENEVALENEVALCM22);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVALCM23, DbType.Decimal, entity.VALENEVALENEVALCM23);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVALCM24, DbType.Decimal, entity.VALENEVALENEVALCM24);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVALCM25, DbType.Decimal, entity.VALENEVALENEVALCM25);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVALCM26, DbType.Decimal, entity.VALENEVALENEVALCM26);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVALCM27, DbType.Decimal, entity.VALENEVALENEVALCM27);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVALCM28, DbType.Decimal, entity.VALENEVALENEVALCM28);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVALCM29, DbType.Decimal, entity.VALENEVALENEVALCM29);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVALCM30, DbType.Decimal, entity.VALENEVALENEVALCM30);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVALCM31, DbType.Decimal, entity.VALENEVALENEVALCM31);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVALCM32, DbType.Decimal, entity.VALENEVALENEVALCM32);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVALCM33, DbType.Decimal, entity.VALENEVALENEVALCM33);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVALCM34, DbType.Decimal, entity.VALENEVALENEVALCM34);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVALCM35, DbType.Decimal, entity.VALENEVALENEVALCM35);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVALCM36, DbType.Decimal, entity.VALENEVALENEVALCM36);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVALCM37, DbType.Decimal, entity.VALENEVALENEVALCM37);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVALCM38, DbType.Decimal, entity.VALENEVALENEVALCM38);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVALCM39, DbType.Decimal, entity.VALENEVALENEVALCM39);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVALCM40, DbType.Decimal, entity.VALENEVALENEVALCM40);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVALCM41, DbType.Decimal, entity.VALENEVALENEVALCM41);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVALCM42, DbType.Decimal, entity.VALENEVALENEVALCM42);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVALCM43, DbType.Decimal, entity.VALENEVALENEVALCM43);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVALCM44, DbType.Decimal, entity.VALENEVALENEVALCM44);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVALCM45, DbType.Decimal, entity.VALENEVALENEVALCM45);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVALCM46, DbType.Decimal, entity.VALENEVALENEVALCM46);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVALCM47, DbType.Decimal, entity.VALENEVALENEVALCM47);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVALCM48, DbType.Decimal, entity.VALENEVALENEVALCM48);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVALCM49, DbType.Decimal, entity.VALENEVALENEVALCM49);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVALCM50, DbType.Decimal, entity.VALENEVALENEVALCM50);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVALCM51, DbType.Decimal, entity.VALENEVALENEVALCM51);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVALCM52, DbType.Decimal, entity.VALENEVALENEVALCM52);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVALCM53, DbType.Decimal, entity.VALENEVALENEVALCM53);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVALCM54, DbType.Decimal, entity.VALENEVALENEVALCM54);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVALCM55, DbType.Decimal, entity.VALENEVALENEVALCM55);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVALCM56, DbType.Decimal, entity.VALENEVALENEVALCM56);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVALCM57, DbType.Decimal, entity.VALENEVALENEVALCM57);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVALCM58, DbType.Decimal, entity.VALENEVALENEVALCM58);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVALCM59, DbType.Decimal, entity.VALENEVALENEVALCM59);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVALCM60, DbType.Decimal, entity.VALENEVALENEVALCM60);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVALCM61, DbType.Decimal, entity.VALENEVALENEVALCM61);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVALCM62, DbType.Decimal, entity.VALENEVALENEVALCM62);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVALCM63, DbType.Decimal, entity.VALENEVALENEVALCM63);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVALCM64, DbType.Decimal, entity.VALENEVALENEVALCM64);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVALCM65, DbType.Decimal, entity.VALENEVALENEVALCM65);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVALCM66, DbType.Decimal, entity.VALENEVALENEVALCM66);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVALCM67, DbType.Decimal, entity.VALENEVALENEVALCM67);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVALCM68, DbType.Decimal, entity.VALENEVALENEVALCM68);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVALCM69, DbType.Decimal, entity.VALENEVALENEVALCM69);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVALCM70, DbType.Decimal, entity.VALENEVALENEVALCM70);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVALCM71, DbType.Decimal, entity.VALENEVALENEVALCM71);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVALCM72, DbType.Decimal, entity.VALENEVALENEVALCM72);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVALCM73, DbType.Decimal, entity.VALENEVALENEVALCM73);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVALCM74, DbType.Decimal, entity.VALENEVALENEVALCM74);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVALCM75, DbType.Decimal, entity.VALENEVALENEVALCM75);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVALCM76, DbType.Decimal, entity.VALENEVALENEVALCM76);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVALCM77, DbType.Decimal, entity.VALENEVALENEVALCM77);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVALCM78, DbType.Decimal, entity.VALENEVALENEVALCM78);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVALCM79, DbType.Decimal, entity.VALENEVALENEVALCM79);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVALCM80, DbType.Decimal, entity.VALENEVALENEVALCM80);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVALCM81, DbType.Decimal, entity.VALENEVALENEVALCM81);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVALCM82, DbType.Decimal, entity.VALENEVALENEVALCM82);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVALCM83, DbType.Decimal, entity.VALENEVALENEVALCM83);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVALCM84, DbType.Decimal, entity.VALENEVALENEVALCM84);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVALCM85, DbType.Decimal, entity.VALENEVALENEVALCM85);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVALCM86, DbType.Decimal, entity.VALENEVALENEVALCM86);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVALCM87, DbType.Decimal, entity.VALENEVALENEVALCM87);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVALCM88, DbType.Decimal, entity.VALENEVALENEVALCM88);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVALCM89, DbType.Decimal, entity.VALENEVALENEVALCM89);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVALCM90, DbType.Decimal, entity.VALENEVALENEVALCM90);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVALCM91, DbType.Decimal, entity.VALENEVALENEVALCM91);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVALCM92, DbType.Decimal, entity.VALENEVALENEVALCM92);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVALCM93, DbType.Decimal, entity.VALENEVALENEVALCM93);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVALCM94, DbType.Decimal, entity.VALENEVALENEVALCM94);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVALCM95, DbType.Decimal, entity.VALENEVALENEVALCM95);
            dbProvider.AddInParameter(command, helper.VALENEVALENEVALCM96, DbType.Decimal, entity.VALENEVALENEVALCM96);

            dbProvider.AddInParameter(command, helper.VALUSUCREACION, DbType.String, entity.VALUSUCREACION);
            #endregion

            var iRslt = dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public List<GmmValEnergiaEntregaDTO> ListarValores96Originales(GmmValEnergiaEntregaDTO valEnergiaDTO)
        {
            List<GmmValEnergiaEntregaDTO> entities = new List<GmmValEnergiaEntregaDTO>();
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

        public List<GmmValEnergiaEntregaDTO> ListarValoresCostoMarginal(GmmValEnergiaEntregaDTO valEnergiaDTO, int anio, int mes)
        {
            List<GmmValEnergiaEntregaDTO> entities = new List<GmmValEnergiaEntregaDTO>();

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
