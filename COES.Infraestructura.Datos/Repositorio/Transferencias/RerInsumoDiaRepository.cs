using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Transferencias;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Transferencias;

namespace COES.Infraestructura.Datos.Repositorio.Transferencias
{
    /// <summary>
    /// Clase de acceso a datos de la tabla RER_INSUMO_DIA
    /// </summary>
    public class RerInsumoDiaRepository : RepositoryBase, IRerInsumoDiaRepository
    {
        public RerInsumoDiaRepository(string strConn)
            : base(strConn)
        {
        }

        RerInsumoDiaHelper helper = new RerInsumoDiaHelper();

        public int Save(RerInsumoDiaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Rerinddiacodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Rerinmmescodi, DbType.Int32, entity.Rerinmmescodi);
            dbProvider.AddInParameter(command, helper.Rerpprcodi, DbType.Int32, entity.Rerpprcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.String, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Rerinddiafecdia, DbType.DateTime, entity.Rerinddiafecdia);
            dbProvider.AddInParameter(command, helper.Rerindtipresultado, DbType.String, entity.Rerindtipresultado);
            dbProvider.AddInParameter(command, helper.Rerindtipinformacion, DbType.String, entity.Rerindtipinformacion);

            dbProvider.AddInParameter(command, helper.Rerinddiah1, DbType.Decimal, entity.Rerinddiah1);
            dbProvider.AddInParameter(command, helper.Rerinddiah2, DbType.Decimal, entity.Rerinddiah2);
            dbProvider.AddInParameter(command, helper.Rerinddiah3, DbType.Decimal, entity.Rerinddiah3);
            dbProvider.AddInParameter(command, helper.Rerinddiah4, DbType.Decimal, entity.Rerinddiah4);
            dbProvider.AddInParameter(command, helper.Rerinddiah5, DbType.Decimal, entity.Rerinddiah5);
            dbProvider.AddInParameter(command, helper.Rerinddiah6, DbType.Decimal, entity.Rerinddiah6);
            dbProvider.AddInParameter(command, helper.Rerinddiah7, DbType.Decimal, entity.Rerinddiah7);
            dbProvider.AddInParameter(command, helper.Rerinddiah8, DbType.Decimal, entity.Rerinddiah8);
            dbProvider.AddInParameter(command, helper.Rerinddiah9, DbType.Decimal, entity.Rerinddiah9);
            dbProvider.AddInParameter(command, helper.Rerinddiah10, DbType.Decimal, entity.Rerinddiah10);
            dbProvider.AddInParameter(command, helper.Rerinddiah11, DbType.Decimal, entity.Rerinddiah11);
            dbProvider.AddInParameter(command, helper.Rerinddiah12, DbType.Decimal, entity.Rerinddiah12);
            dbProvider.AddInParameter(command, helper.Rerinddiah13, DbType.Decimal, entity.Rerinddiah13);
            dbProvider.AddInParameter(command, helper.Rerinddiah14, DbType.Decimal, entity.Rerinddiah14);
            dbProvider.AddInParameter(command, helper.Rerinddiah15, DbType.Decimal, entity.Rerinddiah15);
            dbProvider.AddInParameter(command, helper.Rerinddiah16, DbType.Decimal, entity.Rerinddiah16);
            dbProvider.AddInParameter(command, helper.Rerinddiah17, DbType.Decimal, entity.Rerinddiah17);
            dbProvider.AddInParameter(command, helper.Rerinddiah18, DbType.Decimal, entity.Rerinddiah18);
            dbProvider.AddInParameter(command, helper.Rerinddiah19, DbType.Decimal, entity.Rerinddiah19);
            dbProvider.AddInParameter(command, helper.Rerinddiah20, DbType.Decimal, entity.Rerinddiah20);
            dbProvider.AddInParameter(command, helper.Rerinddiah21, DbType.Decimal, entity.Rerinddiah21);
            dbProvider.AddInParameter(command, helper.Rerinddiah22, DbType.Decimal, entity.Rerinddiah22);
            dbProvider.AddInParameter(command, helper.Rerinddiah23, DbType.Decimal, entity.Rerinddiah23);
            dbProvider.AddInParameter(command, helper.Rerinddiah24, DbType.Decimal, entity.Rerinddiah24);
            dbProvider.AddInParameter(command, helper.Rerinddiah25, DbType.Decimal, entity.Rerinddiah25);
            dbProvider.AddInParameter(command, helper.Rerinddiah26, DbType.Decimal, entity.Rerinddiah26);
            dbProvider.AddInParameter(command, helper.Rerinddiah27, DbType.Decimal, entity.Rerinddiah27);
            dbProvider.AddInParameter(command, helper.Rerinddiah28, DbType.Decimal, entity.Rerinddiah28);
            dbProvider.AddInParameter(command, helper.Rerinddiah29, DbType.Decimal, entity.Rerinddiah29);
            dbProvider.AddInParameter(command, helper.Rerinddiah30, DbType.Decimal, entity.Rerinddiah30);
            dbProvider.AddInParameter(command, helper.Rerinddiah31, DbType.Decimal, entity.Rerinddiah31);
            dbProvider.AddInParameter(command, helper.Rerinddiah32, DbType.Decimal, entity.Rerinddiah32);
            dbProvider.AddInParameter(command, helper.Rerinddiah33, DbType.Decimal, entity.Rerinddiah33);
            dbProvider.AddInParameter(command, helper.Rerinddiah34, DbType.Decimal, entity.Rerinddiah34);
            dbProvider.AddInParameter(command, helper.Rerinddiah35, DbType.Decimal, entity.Rerinddiah35);
            dbProvider.AddInParameter(command, helper.Rerinddiah36, DbType.Decimal, entity.Rerinddiah36);
            dbProvider.AddInParameter(command, helper.Rerinddiah37, DbType.Decimal, entity.Rerinddiah37);
            dbProvider.AddInParameter(command, helper.Rerinddiah38, DbType.Decimal, entity.Rerinddiah38);
            dbProvider.AddInParameter(command, helper.Rerinddiah39, DbType.Decimal, entity.Rerinddiah39);
            dbProvider.AddInParameter(command, helper.Rerinddiah40, DbType.Decimal, entity.Rerinddiah40);
            dbProvider.AddInParameter(command, helper.Rerinddiah41, DbType.Decimal, entity.Rerinddiah41);
            dbProvider.AddInParameter(command, helper.Rerinddiah42, DbType.Decimal, entity.Rerinddiah42);
            dbProvider.AddInParameter(command, helper.Rerinddiah43, DbType.Decimal, entity.Rerinddiah43);
            dbProvider.AddInParameter(command, helper.Rerinddiah44, DbType.Decimal, entity.Rerinddiah44);
            dbProvider.AddInParameter(command, helper.Rerinddiah45, DbType.Decimal, entity.Rerinddiah45);
            dbProvider.AddInParameter(command, helper.Rerinddiah46, DbType.Decimal, entity.Rerinddiah46);
            dbProvider.AddInParameter(command, helper.Rerinddiah47, DbType.Decimal, entity.Rerinddiah47);
            dbProvider.AddInParameter(command, helper.Rerinddiah48, DbType.Decimal, entity.Rerinddiah48);
            dbProvider.AddInParameter(command, helper.Rerinddiah49, DbType.Decimal, entity.Rerinddiah49);
            dbProvider.AddInParameter(command, helper.Rerinddiah50, DbType.Decimal, entity.Rerinddiah50);
            dbProvider.AddInParameter(command, helper.Rerinddiah51, DbType.Decimal, entity.Rerinddiah51);
            dbProvider.AddInParameter(command, helper.Rerinddiah52, DbType.Decimal, entity.Rerinddiah52);
            dbProvider.AddInParameter(command, helper.Rerinddiah53, DbType.Decimal, entity.Rerinddiah53);
            dbProvider.AddInParameter(command, helper.Rerinddiah54, DbType.Decimal, entity.Rerinddiah54);
            dbProvider.AddInParameter(command, helper.Rerinddiah55, DbType.Decimal, entity.Rerinddiah55);
            dbProvider.AddInParameter(command, helper.Rerinddiah56, DbType.Decimal, entity.Rerinddiah56);
            dbProvider.AddInParameter(command, helper.Rerinddiah57, DbType.Decimal, entity.Rerinddiah57);
            dbProvider.AddInParameter(command, helper.Rerinddiah58, DbType.Decimal, entity.Rerinddiah58);
            dbProvider.AddInParameter(command, helper.Rerinddiah59, DbType.Decimal, entity.Rerinddiah59);
            dbProvider.AddInParameter(command, helper.Rerinddiah60, DbType.Decimal, entity.Rerinddiah60);
            dbProvider.AddInParameter(command, helper.Rerinddiah61, DbType.Decimal, entity.Rerinddiah61);
            dbProvider.AddInParameter(command, helper.Rerinddiah62, DbType.Decimal, entity.Rerinddiah62);
            dbProvider.AddInParameter(command, helper.Rerinddiah63, DbType.Decimal, entity.Rerinddiah63);
            dbProvider.AddInParameter(command, helper.Rerinddiah64, DbType.Decimal, entity.Rerinddiah64);
            dbProvider.AddInParameter(command, helper.Rerinddiah65, DbType.Decimal, entity.Rerinddiah65);
            dbProvider.AddInParameter(command, helper.Rerinddiah66, DbType.Decimal, entity.Rerinddiah66);
            dbProvider.AddInParameter(command, helper.Rerinddiah67, DbType.Decimal, entity.Rerinddiah67);
            dbProvider.AddInParameter(command, helper.Rerinddiah68, DbType.Decimal, entity.Rerinddiah68);
            dbProvider.AddInParameter(command, helper.Rerinddiah69, DbType.Decimal, entity.Rerinddiah69);
            dbProvider.AddInParameter(command, helper.Rerinddiah70, DbType.Decimal, entity.Rerinddiah70);
            dbProvider.AddInParameter(command, helper.Rerinddiah71, DbType.Decimal, entity.Rerinddiah71);
            dbProvider.AddInParameter(command, helper.Rerinddiah72, DbType.Decimal, entity.Rerinddiah72);
            dbProvider.AddInParameter(command, helper.Rerinddiah73, DbType.Decimal, entity.Rerinddiah73);
            dbProvider.AddInParameter(command, helper.Rerinddiah74, DbType.Decimal, entity.Rerinddiah74);
            dbProvider.AddInParameter(command, helper.Rerinddiah75, DbType.Decimal, entity.Rerinddiah75);
            dbProvider.AddInParameter(command, helper.Rerinddiah76, DbType.Decimal, entity.Rerinddiah76);
            dbProvider.AddInParameter(command, helper.Rerinddiah77, DbType.Decimal, entity.Rerinddiah77);
            dbProvider.AddInParameter(command, helper.Rerinddiah78, DbType.Decimal, entity.Rerinddiah78);
            dbProvider.AddInParameter(command, helper.Rerinddiah79, DbType.Decimal, entity.Rerinddiah79);
            dbProvider.AddInParameter(command, helper.Rerinddiah80, DbType.Decimal, entity.Rerinddiah80);
            dbProvider.AddInParameter(command, helper.Rerinddiah81, DbType.Decimal, entity.Rerinddiah81);
            dbProvider.AddInParameter(command, helper.Rerinddiah82, DbType.Decimal, entity.Rerinddiah82);
            dbProvider.AddInParameter(command, helper.Rerinddiah83, DbType.Decimal, entity.Rerinddiah83);
            dbProvider.AddInParameter(command, helper.Rerinddiah84, DbType.Decimal, entity.Rerinddiah84);
            dbProvider.AddInParameter(command, helper.Rerinddiah85, DbType.Decimal, entity.Rerinddiah85);
            dbProvider.AddInParameter(command, helper.Rerinddiah86, DbType.Decimal, entity.Rerinddiah86);
            dbProvider.AddInParameter(command, helper.Rerinddiah87, DbType.Decimal, entity.Rerinddiah87);
            dbProvider.AddInParameter(command, helper.Rerinddiah88, DbType.Decimal, entity.Rerinddiah88);
            dbProvider.AddInParameter(command, helper.Rerinddiah89, DbType.Decimal, entity.Rerinddiah89);
            dbProvider.AddInParameter(command, helper.Rerinddiah90, DbType.Decimal, entity.Rerinddiah90);
            dbProvider.AddInParameter(command, helper.Rerinddiah91, DbType.Decimal, entity.Rerinddiah91);
            dbProvider.AddInParameter(command, helper.Rerinddiah92, DbType.Decimal, entity.Rerinddiah92);
            dbProvider.AddInParameter(command, helper.Rerinddiah93, DbType.Decimal, entity.Rerinddiah93);
            dbProvider.AddInParameter(command, helper.Rerinddiah94, DbType.Decimal, entity.Rerinddiah94);
            dbProvider.AddInParameter(command, helper.Rerinddiah95, DbType.Decimal, entity.Rerinddiah95);
            dbProvider.AddInParameter(command, helper.Rerinddiah96, DbType.Decimal, entity.Rerinddiah96);

            dbProvider.AddInParameter(command, helper.Rerinddiatotal, DbType.Decimal, entity.Rerinddiatotal);

            dbProvider.AddInParameter(command, helper.Rerinddiausucreacion, DbType.String, entity.Rerinddiausucreacion);
            dbProvider.AddInParameter(command, helper.Rerinddiafeccreacion, DbType.DateTime, entity.Rerinddiafeccreacion);

            dbProvider.ExecuteNonQuery(command);

            return id;
        }

        public void Update(RerInsumoDiaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);
            dbProvider.AddInParameter(command, helper.Rerinddiacodi, DbType.Int32, entity.Rerinddiacodi);

            dbProvider.AddInParameter(command, helper.Rerinmmescodi, DbType.Int32, entity.Rerinmmescodi);
            dbProvider.AddInParameter(command, helper.Rerpprcodi, DbType.Int32, entity.Rerpprcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.String, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Rerinddiafecdia, DbType.DateTime, entity.Rerinddiafecdia);
            dbProvider.AddInParameter(command, helper.Rerindtipresultado, DbType.String, entity.Rerindtipresultado);
            dbProvider.AddInParameter(command, helper.Rerindtipinformacion, DbType.String, entity.Rerindtipinformacion);

            dbProvider.AddInParameter(command, helper.Rerinddiah1, DbType.Decimal, entity.Rerinddiah1);
            dbProvider.AddInParameter(command, helper.Rerinddiah2, DbType.Decimal, entity.Rerinddiah2);
            dbProvider.AddInParameter(command, helper.Rerinddiah3, DbType.Decimal, entity.Rerinddiah3);
            dbProvider.AddInParameter(command, helper.Rerinddiah4, DbType.Decimal, entity.Rerinddiah4);
            dbProvider.AddInParameter(command, helper.Rerinddiah5, DbType.Decimal, entity.Rerinddiah5);
            dbProvider.AddInParameter(command, helper.Rerinddiah6, DbType.Decimal, entity.Rerinddiah6);
            dbProvider.AddInParameter(command, helper.Rerinddiah7, DbType.Decimal, entity.Rerinddiah7);
            dbProvider.AddInParameter(command, helper.Rerinddiah8, DbType.Decimal, entity.Rerinddiah8);
            dbProvider.AddInParameter(command, helper.Rerinddiah9, DbType.Decimal, entity.Rerinddiah9);
            dbProvider.AddInParameter(command, helper.Rerinddiah10, DbType.Decimal, entity.Rerinddiah10);
            dbProvider.AddInParameter(command, helper.Rerinddiah11, DbType.Decimal, entity.Rerinddiah11);
            dbProvider.AddInParameter(command, helper.Rerinddiah12, DbType.Decimal, entity.Rerinddiah12);
            dbProvider.AddInParameter(command, helper.Rerinddiah13, DbType.Decimal, entity.Rerinddiah13);
            dbProvider.AddInParameter(command, helper.Rerinddiah14, DbType.Decimal, entity.Rerinddiah14);
            dbProvider.AddInParameter(command, helper.Rerinddiah15, DbType.Decimal, entity.Rerinddiah15);
            dbProvider.AddInParameter(command, helper.Rerinddiah16, DbType.Decimal, entity.Rerinddiah16);
            dbProvider.AddInParameter(command, helper.Rerinddiah17, DbType.Decimal, entity.Rerinddiah17);
            dbProvider.AddInParameter(command, helper.Rerinddiah18, DbType.Decimal, entity.Rerinddiah18);
            dbProvider.AddInParameter(command, helper.Rerinddiah19, DbType.Decimal, entity.Rerinddiah19);
            dbProvider.AddInParameter(command, helper.Rerinddiah20, DbType.Decimal, entity.Rerinddiah20);
            dbProvider.AddInParameter(command, helper.Rerinddiah21, DbType.Decimal, entity.Rerinddiah21);
            dbProvider.AddInParameter(command, helper.Rerinddiah22, DbType.Decimal, entity.Rerinddiah22);
            dbProvider.AddInParameter(command, helper.Rerinddiah23, DbType.Decimal, entity.Rerinddiah23);
            dbProvider.AddInParameter(command, helper.Rerinddiah24, DbType.Decimal, entity.Rerinddiah24);
            dbProvider.AddInParameter(command, helper.Rerinddiah25, DbType.Decimal, entity.Rerinddiah25);
            dbProvider.AddInParameter(command, helper.Rerinddiah26, DbType.Decimal, entity.Rerinddiah26);
            dbProvider.AddInParameter(command, helper.Rerinddiah27, DbType.Decimal, entity.Rerinddiah27);
            dbProvider.AddInParameter(command, helper.Rerinddiah28, DbType.Decimal, entity.Rerinddiah28);
            dbProvider.AddInParameter(command, helper.Rerinddiah29, DbType.Decimal, entity.Rerinddiah29);
            dbProvider.AddInParameter(command, helper.Rerinddiah30, DbType.Decimal, entity.Rerinddiah30);
            dbProvider.AddInParameter(command, helper.Rerinddiah31, DbType.Decimal, entity.Rerinddiah31);
            dbProvider.AddInParameter(command, helper.Rerinddiah32, DbType.Decimal, entity.Rerinddiah32);
            dbProvider.AddInParameter(command, helper.Rerinddiah33, DbType.Decimal, entity.Rerinddiah33);
            dbProvider.AddInParameter(command, helper.Rerinddiah34, DbType.Decimal, entity.Rerinddiah34);
            dbProvider.AddInParameter(command, helper.Rerinddiah35, DbType.Decimal, entity.Rerinddiah35);
            dbProvider.AddInParameter(command, helper.Rerinddiah36, DbType.Decimal, entity.Rerinddiah36);
            dbProvider.AddInParameter(command, helper.Rerinddiah37, DbType.Decimal, entity.Rerinddiah37);
            dbProvider.AddInParameter(command, helper.Rerinddiah38, DbType.Decimal, entity.Rerinddiah38);
            dbProvider.AddInParameter(command, helper.Rerinddiah39, DbType.Decimal, entity.Rerinddiah39);
            dbProvider.AddInParameter(command, helper.Rerinddiah40, DbType.Decimal, entity.Rerinddiah40);
            dbProvider.AddInParameter(command, helper.Rerinddiah41, DbType.Decimal, entity.Rerinddiah41);
            dbProvider.AddInParameter(command, helper.Rerinddiah42, DbType.Decimal, entity.Rerinddiah42);
            dbProvider.AddInParameter(command, helper.Rerinddiah43, DbType.Decimal, entity.Rerinddiah43);
            dbProvider.AddInParameter(command, helper.Rerinddiah44, DbType.Decimal, entity.Rerinddiah44);
            dbProvider.AddInParameter(command, helper.Rerinddiah45, DbType.Decimal, entity.Rerinddiah45);
            dbProvider.AddInParameter(command, helper.Rerinddiah46, DbType.Decimal, entity.Rerinddiah46);
            dbProvider.AddInParameter(command, helper.Rerinddiah47, DbType.Decimal, entity.Rerinddiah47);
            dbProvider.AddInParameter(command, helper.Rerinddiah48, DbType.Decimal, entity.Rerinddiah48);
            dbProvider.AddInParameter(command, helper.Rerinddiah49, DbType.Decimal, entity.Rerinddiah49);
            dbProvider.AddInParameter(command, helper.Rerinddiah50, DbType.Decimal, entity.Rerinddiah50);
            dbProvider.AddInParameter(command, helper.Rerinddiah51, DbType.Decimal, entity.Rerinddiah51);
            dbProvider.AddInParameter(command, helper.Rerinddiah52, DbType.Decimal, entity.Rerinddiah52);
            dbProvider.AddInParameter(command, helper.Rerinddiah53, DbType.Decimal, entity.Rerinddiah53);
            dbProvider.AddInParameter(command, helper.Rerinddiah54, DbType.Decimal, entity.Rerinddiah54);
            dbProvider.AddInParameter(command, helper.Rerinddiah55, DbType.Decimal, entity.Rerinddiah55);
            dbProvider.AddInParameter(command, helper.Rerinddiah56, DbType.Decimal, entity.Rerinddiah56);
            dbProvider.AddInParameter(command, helper.Rerinddiah57, DbType.Decimal, entity.Rerinddiah57);
            dbProvider.AddInParameter(command, helper.Rerinddiah58, DbType.Decimal, entity.Rerinddiah58);
            dbProvider.AddInParameter(command, helper.Rerinddiah59, DbType.Decimal, entity.Rerinddiah59);
            dbProvider.AddInParameter(command, helper.Rerinddiah60, DbType.Decimal, entity.Rerinddiah60);
            dbProvider.AddInParameter(command, helper.Rerinddiah61, DbType.Decimal, entity.Rerinddiah61);
            dbProvider.AddInParameter(command, helper.Rerinddiah62, DbType.Decimal, entity.Rerinddiah62);
            dbProvider.AddInParameter(command, helper.Rerinddiah63, DbType.Decimal, entity.Rerinddiah63);
            dbProvider.AddInParameter(command, helper.Rerinddiah64, DbType.Decimal, entity.Rerinddiah64);
            dbProvider.AddInParameter(command, helper.Rerinddiah65, DbType.Decimal, entity.Rerinddiah65);
            dbProvider.AddInParameter(command, helper.Rerinddiah66, DbType.Decimal, entity.Rerinddiah66);
            dbProvider.AddInParameter(command, helper.Rerinddiah67, DbType.Decimal, entity.Rerinddiah67);
            dbProvider.AddInParameter(command, helper.Rerinddiah68, DbType.Decimal, entity.Rerinddiah68);
            dbProvider.AddInParameter(command, helper.Rerinddiah69, DbType.Decimal, entity.Rerinddiah69);
            dbProvider.AddInParameter(command, helper.Rerinddiah70, DbType.Decimal, entity.Rerinddiah70);
            dbProvider.AddInParameter(command, helper.Rerinddiah71, DbType.Decimal, entity.Rerinddiah71);
            dbProvider.AddInParameter(command, helper.Rerinddiah72, DbType.Decimal, entity.Rerinddiah72);
            dbProvider.AddInParameter(command, helper.Rerinddiah73, DbType.Decimal, entity.Rerinddiah73);
            dbProvider.AddInParameter(command, helper.Rerinddiah74, DbType.Decimal, entity.Rerinddiah74);
            dbProvider.AddInParameter(command, helper.Rerinddiah75, DbType.Decimal, entity.Rerinddiah75);
            dbProvider.AddInParameter(command, helper.Rerinddiah76, DbType.Decimal, entity.Rerinddiah76);
            dbProvider.AddInParameter(command, helper.Rerinddiah77, DbType.Decimal, entity.Rerinddiah77);
            dbProvider.AddInParameter(command, helper.Rerinddiah78, DbType.Decimal, entity.Rerinddiah78);
            dbProvider.AddInParameter(command, helper.Rerinddiah79, DbType.Decimal, entity.Rerinddiah79);
            dbProvider.AddInParameter(command, helper.Rerinddiah80, DbType.Decimal, entity.Rerinddiah80);
            dbProvider.AddInParameter(command, helper.Rerinddiah81, DbType.Decimal, entity.Rerinddiah81);
            dbProvider.AddInParameter(command, helper.Rerinddiah82, DbType.Decimal, entity.Rerinddiah82);
            dbProvider.AddInParameter(command, helper.Rerinddiah83, DbType.Decimal, entity.Rerinddiah83);
            dbProvider.AddInParameter(command, helper.Rerinddiah84, DbType.Decimal, entity.Rerinddiah84);
            dbProvider.AddInParameter(command, helper.Rerinddiah85, DbType.Decimal, entity.Rerinddiah85);
            dbProvider.AddInParameter(command, helper.Rerinddiah86, DbType.Decimal, entity.Rerinddiah86);
            dbProvider.AddInParameter(command, helper.Rerinddiah87, DbType.Decimal, entity.Rerinddiah87);
            dbProvider.AddInParameter(command, helper.Rerinddiah88, DbType.Decimal, entity.Rerinddiah88);
            dbProvider.AddInParameter(command, helper.Rerinddiah89, DbType.Decimal, entity.Rerinddiah89);
            dbProvider.AddInParameter(command, helper.Rerinddiah90, DbType.Decimal, entity.Rerinddiah90);
            dbProvider.AddInParameter(command, helper.Rerinddiah91, DbType.Decimal, entity.Rerinddiah91);
            dbProvider.AddInParameter(command, helper.Rerinddiah92, DbType.Decimal, entity.Rerinddiah92);
            dbProvider.AddInParameter(command, helper.Rerinddiah93, DbType.Decimal, entity.Rerinddiah93);
            dbProvider.AddInParameter(command, helper.Rerinddiah94, DbType.Decimal, entity.Rerinddiah94);
            dbProvider.AddInParameter(command, helper.Rerinddiah95, DbType.Decimal, entity.Rerinddiah95);
            dbProvider.AddInParameter(command, helper.Rerinddiah96, DbType.Decimal, entity.Rerinddiah96);

            dbProvider.AddInParameter(command, helper.Rerinddiatotal, DbType.Decimal, entity.Rerinddiatotal);

            dbProvider.AddInParameter(command, helper.Rerinddiausucreacion, DbType.String, entity.Rerinddiausucreacion);
            dbProvider.AddInParameter(command, helper.Rerinddiafeccreacion, DbType.DateTime, entity.Rerinddiafeccreacion);


            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int Rerinddiacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Rerinddiacodi, DbType.Int32, Rerinddiacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public RerInsumoDiaDTO GetById(int Rerinddiacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Rerinddiacodi, DbType.Int32, Rerinddiacodi);
            RerInsumoDiaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<RerInsumoDiaDTO> List()
        {
            List<RerInsumoDiaDTO> entities = new List<RerInsumoDiaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entities.Add(helper.Create(dr));
                }
            }

            return entities;
        }

        #region CU21

        public RerInsumoDiaDTO GetByCriteria(int rerInmMesCodi, int emprcodi, int equicodi, string rerIndDiaFecha)
        {
            RerInsumoDiaDTO entity = null;
            string query = string.Format(helper.SqlGetByCriteria, rerInmMesCodi, emprcodi, equicodi, rerIndDiaFecha);
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
        public void DeleteByParametroPrimaAndTipo(int iRerpprcodi, int iRerpprmes, string sRerinmtipresultado)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteByParametroPrimaAndTipo);

            dbProvider.AddInParameter(command, helper.Rerpprcodi, DbType.Int32, iRerpprcodi);
            dbProvider.AddInParameter(command, helper.Rerpprmes, DbType.Int32, iRerpprmes);
            dbProvider.AddInParameter(command, helper.Rerinmtipresultado, DbType.String, sRerinmtipresultado);
            
            dbProvider.ExecuteNonQuery(command);
        }

        public List<RerInsumoDiaDTO> GetByTipoResultadoByPeriodo(string rerindtipresultado, int reravcodi, string rerpprmes)
        {
            string query = string.Format(helper.SqlGetByTipoResultadoByPeriodo, rerindtipresultado, reravcodi, rerpprmes);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            List<RerInsumoDiaDTO> entitys = new List<RerInsumoDiaDTO>();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateByTipoResultadoByPeriodo(dr));
                }
            }

            return entitys;
        }

        #endregion

        public List<RerInsumoDiaDTO> GetByMesByEmpresaByCentral(int rerinmmescodi, int emprcodi, int equicodi)
        {
            string query = string.Format(helper.SqlGetByMesByEmpresaByCentral, rerinmmescodi, emprcodi, equicodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            List<RerInsumoDiaDTO> entitys = new List<RerInsumoDiaDTO>();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;

        }


        public void TruncateTablaTemporal(string nombreTabla)
        {
            string query = string.Format(helper.SqlTruncateTablaTemporal, nombreTabla);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            dbProvider.ExecuteNonQuery(command);
        }

        public void SaveTablaTemporal(RerInsumoDiaTemporalDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSaveInsumoDiaTemporal);

            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.Ptomedidesc, DbType.String, entity.Ptomedidesc);
            dbProvider.AddInParameter(command, helper.Rerinddiafecdia, DbType.DateTime, entity.Rerinddiafecdia);
            dbProvider.AddInParameter(command, helper.Rerinddiah1, DbType.Decimal, entity.Rerinddiah1);
            dbProvider.AddInParameter(command, helper.Rerinddiah2, DbType.Decimal, entity.Rerinddiah2);
            dbProvider.AddInParameter(command, helper.Rerinddiah3, DbType.Decimal, entity.Rerinddiah3);
            dbProvider.AddInParameter(command, helper.Rerinddiah4, DbType.Decimal, entity.Rerinddiah4);
            dbProvider.AddInParameter(command, helper.Rerinddiah5, DbType.Decimal, entity.Rerinddiah5);
            dbProvider.AddInParameter(command, helper.Rerinddiah6, DbType.Decimal, entity.Rerinddiah6);
            dbProvider.AddInParameter(command, helper.Rerinddiah7, DbType.Decimal, entity.Rerinddiah7);
            dbProvider.AddInParameter(command, helper.Rerinddiah8, DbType.Decimal, entity.Rerinddiah8);
            dbProvider.AddInParameter(command, helper.Rerinddiah9, DbType.Decimal, entity.Rerinddiah9);
            dbProvider.AddInParameter(command, helper.Rerinddiah10, DbType.Decimal, entity.Rerinddiah10);
            dbProvider.AddInParameter(command, helper.Rerinddiah11, DbType.Decimal, entity.Rerinddiah11);
            dbProvider.AddInParameter(command, helper.Rerinddiah12, DbType.Decimal, entity.Rerinddiah12);
            dbProvider.AddInParameter(command, helper.Rerinddiah13, DbType.Decimal, entity.Rerinddiah13);
            dbProvider.AddInParameter(command, helper.Rerinddiah14, DbType.Decimal, entity.Rerinddiah14);
            dbProvider.AddInParameter(command, helper.Rerinddiah15, DbType.Decimal, entity.Rerinddiah15);
            dbProvider.AddInParameter(command, helper.Rerinddiah16, DbType.Decimal, entity.Rerinddiah16);
            dbProvider.AddInParameter(command, helper.Rerinddiah17, DbType.Decimal, entity.Rerinddiah17);
            dbProvider.AddInParameter(command, helper.Rerinddiah18, DbType.Decimal, entity.Rerinddiah18);
            dbProvider.AddInParameter(command, helper.Rerinddiah19, DbType.Decimal, entity.Rerinddiah19);
            dbProvider.AddInParameter(command, helper.Rerinddiah20, DbType.Decimal, entity.Rerinddiah20);
            dbProvider.AddInParameter(command, helper.Rerinddiah21, DbType.Decimal, entity.Rerinddiah21);
            dbProvider.AddInParameter(command, helper.Rerinddiah22, DbType.Decimal, entity.Rerinddiah22);
            dbProvider.AddInParameter(command, helper.Rerinddiah23, DbType.Decimal, entity.Rerinddiah23);
            dbProvider.AddInParameter(command, helper.Rerinddiah24, DbType.Decimal, entity.Rerinddiah24);
            dbProvider.AddInParameter(command, helper.Rerinddiah25, DbType.Decimal, entity.Rerinddiah25);
            dbProvider.AddInParameter(command, helper.Rerinddiah26, DbType.Decimal, entity.Rerinddiah26);
            dbProvider.AddInParameter(command, helper.Rerinddiah27, DbType.Decimal, entity.Rerinddiah27);
            dbProvider.AddInParameter(command, helper.Rerinddiah28, DbType.Decimal, entity.Rerinddiah28);
            dbProvider.AddInParameter(command, helper.Rerinddiah29, DbType.Decimal, entity.Rerinddiah29);
            dbProvider.AddInParameter(command, helper.Rerinddiah30, DbType.Decimal, entity.Rerinddiah30);
            dbProvider.AddInParameter(command, helper.Rerinddiah31, DbType.Decimal, entity.Rerinddiah31);
            dbProvider.AddInParameter(command, helper.Rerinddiah32, DbType.Decimal, entity.Rerinddiah32);
            dbProvider.AddInParameter(command, helper.Rerinddiah33, DbType.Decimal, entity.Rerinddiah33);
            dbProvider.AddInParameter(command, helper.Rerinddiah34, DbType.Decimal, entity.Rerinddiah34);
            dbProvider.AddInParameter(command, helper.Rerinddiah35, DbType.Decimal, entity.Rerinddiah35);
            dbProvider.AddInParameter(command, helper.Rerinddiah36, DbType.Decimal, entity.Rerinddiah36);
            dbProvider.AddInParameter(command, helper.Rerinddiah37, DbType.Decimal, entity.Rerinddiah37);
            dbProvider.AddInParameter(command, helper.Rerinddiah38, DbType.Decimal, entity.Rerinddiah38);
            dbProvider.AddInParameter(command, helper.Rerinddiah39, DbType.Decimal, entity.Rerinddiah39);
            dbProvider.AddInParameter(command, helper.Rerinddiah40, DbType.Decimal, entity.Rerinddiah40);
            dbProvider.AddInParameter(command, helper.Rerinddiah41, DbType.Decimal, entity.Rerinddiah41);
            dbProvider.AddInParameter(command, helper.Rerinddiah42, DbType.Decimal, entity.Rerinddiah42);
            dbProvider.AddInParameter(command, helper.Rerinddiah43, DbType.Decimal, entity.Rerinddiah43);
            dbProvider.AddInParameter(command, helper.Rerinddiah44, DbType.Decimal, entity.Rerinddiah44);
            dbProvider.AddInParameter(command, helper.Rerinddiah45, DbType.Decimal, entity.Rerinddiah45);
            dbProvider.AddInParameter(command, helper.Rerinddiah46, DbType.Decimal, entity.Rerinddiah46);
            dbProvider.AddInParameter(command, helper.Rerinddiah47, DbType.Decimal, entity.Rerinddiah47);
            dbProvider.AddInParameter(command, helper.Rerinddiah48, DbType.Decimal, entity.Rerinddiah48);
            dbProvider.AddInParameter(command, helper.Rerinddiah49, DbType.Decimal, entity.Rerinddiah49);
            dbProvider.AddInParameter(command, helper.Rerinddiah50, DbType.Decimal, entity.Rerinddiah50);
            dbProvider.AddInParameter(command, helper.Rerinddiah51, DbType.Decimal, entity.Rerinddiah51);
            dbProvider.AddInParameter(command, helper.Rerinddiah52, DbType.Decimal, entity.Rerinddiah52);
            dbProvider.AddInParameter(command, helper.Rerinddiah53, DbType.Decimal, entity.Rerinddiah53);
            dbProvider.AddInParameter(command, helper.Rerinddiah54, DbType.Decimal, entity.Rerinddiah54);
            dbProvider.AddInParameter(command, helper.Rerinddiah55, DbType.Decimal, entity.Rerinddiah55);
            dbProvider.AddInParameter(command, helper.Rerinddiah56, DbType.Decimal, entity.Rerinddiah56);
            dbProvider.AddInParameter(command, helper.Rerinddiah57, DbType.Decimal, entity.Rerinddiah57);
            dbProvider.AddInParameter(command, helper.Rerinddiah58, DbType.Decimal, entity.Rerinddiah58);
            dbProvider.AddInParameter(command, helper.Rerinddiah59, DbType.Decimal, entity.Rerinddiah59);
            dbProvider.AddInParameter(command, helper.Rerinddiah60, DbType.Decimal, entity.Rerinddiah60);
            dbProvider.AddInParameter(command, helper.Rerinddiah61, DbType.Decimal, entity.Rerinddiah61);
            dbProvider.AddInParameter(command, helper.Rerinddiah62, DbType.Decimal, entity.Rerinddiah62);
            dbProvider.AddInParameter(command, helper.Rerinddiah63, DbType.Decimal, entity.Rerinddiah63);
            dbProvider.AddInParameter(command, helper.Rerinddiah64, DbType.Decimal, entity.Rerinddiah64);
            dbProvider.AddInParameter(command, helper.Rerinddiah65, DbType.Decimal, entity.Rerinddiah65);
            dbProvider.AddInParameter(command, helper.Rerinddiah66, DbType.Decimal, entity.Rerinddiah66);
            dbProvider.AddInParameter(command, helper.Rerinddiah67, DbType.Decimal, entity.Rerinddiah67);
            dbProvider.AddInParameter(command, helper.Rerinddiah68, DbType.Decimal, entity.Rerinddiah68);
            dbProvider.AddInParameter(command, helper.Rerinddiah69, DbType.Decimal, entity.Rerinddiah69);
            dbProvider.AddInParameter(command, helper.Rerinddiah70, DbType.Decimal, entity.Rerinddiah70);
            dbProvider.AddInParameter(command, helper.Rerinddiah71, DbType.Decimal, entity.Rerinddiah71);
            dbProvider.AddInParameter(command, helper.Rerinddiah72, DbType.Decimal, entity.Rerinddiah72);
            dbProvider.AddInParameter(command, helper.Rerinddiah73, DbType.Decimal, entity.Rerinddiah73);
            dbProvider.AddInParameter(command, helper.Rerinddiah74, DbType.Decimal, entity.Rerinddiah74);
            dbProvider.AddInParameter(command, helper.Rerinddiah75, DbType.Decimal, entity.Rerinddiah75);
            dbProvider.AddInParameter(command, helper.Rerinddiah76, DbType.Decimal, entity.Rerinddiah76);
            dbProvider.AddInParameter(command, helper.Rerinddiah77, DbType.Decimal, entity.Rerinddiah77);
            dbProvider.AddInParameter(command, helper.Rerinddiah78, DbType.Decimal, entity.Rerinddiah78);
            dbProvider.AddInParameter(command, helper.Rerinddiah79, DbType.Decimal, entity.Rerinddiah79);
            dbProvider.AddInParameter(command, helper.Rerinddiah80, DbType.Decimal, entity.Rerinddiah80);
            dbProvider.AddInParameter(command, helper.Rerinddiah81, DbType.Decimal, entity.Rerinddiah81);
            dbProvider.AddInParameter(command, helper.Rerinddiah82, DbType.Decimal, entity.Rerinddiah82);
            dbProvider.AddInParameter(command, helper.Rerinddiah83, DbType.Decimal, entity.Rerinddiah83);
            dbProvider.AddInParameter(command, helper.Rerinddiah84, DbType.Decimal, entity.Rerinddiah84);
            dbProvider.AddInParameter(command, helper.Rerinddiah85, DbType.Decimal, entity.Rerinddiah85);
            dbProvider.AddInParameter(command, helper.Rerinddiah86, DbType.Decimal, entity.Rerinddiah86);
            dbProvider.AddInParameter(command, helper.Rerinddiah87, DbType.Decimal, entity.Rerinddiah87);
            dbProvider.AddInParameter(command, helper.Rerinddiah88, DbType.Decimal, entity.Rerinddiah88);
            dbProvider.AddInParameter(command, helper.Rerinddiah89, DbType.Decimal, entity.Rerinddiah89);
            dbProvider.AddInParameter(command, helper.Rerinddiah90, DbType.Decimal, entity.Rerinddiah90);
            dbProvider.AddInParameter(command, helper.Rerinddiah91, DbType.Decimal, entity.Rerinddiah91);
            dbProvider.AddInParameter(command, helper.Rerinddiah92, DbType.Decimal, entity.Rerinddiah92);
            dbProvider.AddInParameter(command, helper.Rerinddiah93, DbType.Decimal, entity.Rerinddiah93);
            dbProvider.AddInParameter(command, helper.Rerinddiah94, DbType.Decimal, entity.Rerinddiah94);
            dbProvider.AddInParameter(command, helper.Rerinddiah95, DbType.Decimal, entity.Rerinddiah95);
            dbProvider.AddInParameter(command, helper.Rerinddiah96, DbType.Decimal, entity.Rerinddiah96);
            dbProvider.AddInParameter(command, helper.Rerinddiatotal, DbType.Decimal, entity.Rerinddiatotal);

            dbProvider.ExecuteNonQuery(command);
        }

        public void BulkInsertRerInsumoDiaTemporal(List<RerInsumoDiaTemporalDTO> entitys, string nombreTabla)
        {
            dbProvider.AddColumnMapping(helper.Ptomedicodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Ptomedidesc, DbType.String);
            dbProvider.AddColumnMapping(helper.Rerinddiafecdia, DbType.DateTime);
            dbProvider.AddColumnMapping(helper.Rerinddiah1, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah2, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah3, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah4, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah5, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah6, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah7, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah8, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah9, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah10, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah11, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah12, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah13, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah14, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah15, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah16, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah17, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah18, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah19, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah20, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah21, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah22, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah23, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah24, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah25, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah26, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah27, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah28, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah29, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah30, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah31, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah32, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah33, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah34, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah35, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah36, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah37, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah38, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah39, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah40, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah41, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah42, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah43, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah44, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah45, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah46, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah47, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah48, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah49, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah50, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah51, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah52, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah53, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah54, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah55, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah56, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah57, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah58, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah59, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah60, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah61, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah62, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah63, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah64, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah65, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah66, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah67, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah68, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah69, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah70, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah71, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah72, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah73, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah74, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah75, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah76, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah77, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah78, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah79, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah80, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah81, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah82, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah83, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah84, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah85, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah86, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah87, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah88, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah89, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah90, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah91, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah92, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah93, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah94, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah95, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah96, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiatotal, DbType.Decimal);

            dbProvider.BulkInsert<RerInsumoDiaTemporalDTO>(entitys, nombreTabla);
        }

        public int GetMaxId()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            int id = 1;
            object result = dbProvider.ExecuteScalar(command);
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public void BulkInsertRerInsumoDia(List<RerInsumoDiaDTO> entitys)
        {
            string nombreTabla = "RER_INSUMO_DIA";

            dbProvider.AddColumnMapping(helper.Rerinddiacodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Rerinmmescodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Rerpprcodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Emprcodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Equicodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Rerinddiafecdia, DbType.DateTime);
            dbProvider.AddColumnMapping(helper.Rerindtipresultado, DbType.String);
            dbProvider.AddColumnMapping(helper.Rerindtipinformacion, DbType.String);
            dbProvider.AddColumnMapping(helper.Rerinddiah1, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah2, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah3, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah4, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah5, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah6, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah7, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah8, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah9, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah10, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah11, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah12, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah13, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah14, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah15, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah16, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah17, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah18, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah19, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah20, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah21, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah22, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah23, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah24, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah25, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah26, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah27, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah28, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah29, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah30, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah31, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah32, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah33, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah34, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah35, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah36, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah37, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah38, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah39, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah40, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah41, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah42, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah43, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah44, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah45, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah46, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah47, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah48, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah49, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah50, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah51, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah52, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah53, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah54, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah55, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah56, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah57, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah58, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah59, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah60, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah61, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah62, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah63, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah64, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah65, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah66, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah67, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah68, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah69, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah70, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah71, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah72, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah73, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah74, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah75, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah76, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah77, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah78, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah79, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah80, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah81, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah82, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah83, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah84, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah85, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah86, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah87, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah88, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah89, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah90, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah91, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah92, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah93, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah94, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah95, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiah96, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiatotal, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinddiausucreacion, DbType.String);
            dbProvider.AddColumnMapping(helper.Rerinddiafeccreacion, DbType.DateTime);

            dbProvider.BulkInsert<RerInsumoDiaDTO>(entitys, nombreTabla);
        }
    }
}

