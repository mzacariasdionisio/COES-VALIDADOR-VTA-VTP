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
    /// Clase de acceso a datos de la tabla RER_INSUMO_VTEA
    /// </summary>
    public class RerInsumoVteaRepository : RepositoryBase, IRerInsumoVteaRepository
    {
        public RerInsumoVteaRepository(string strConn)
            : base(strConn)
        {
        }

        RerInsumoVteaHelper helper = new RerInsumoVteaHelper();

        public int Save(RerInsumoVteaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Rerinecodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Rerinscodi, DbType.Int32, entity.Rerinscodi);
            dbProvider.AddInParameter(command, helper.Rerpprcodi, DbType.Int32, entity.Rerpprcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Rerinefecdia, DbType.DateTime, entity.Rerinefecdia);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, entity.Pericodi);
            dbProvider.AddInParameter(command, helper.Recacodi, DbType.Int32, entity.Recacodi);
            dbProvider.AddInParameter(command, helper.Rerinediah1, DbType.Decimal, entity.Rerinediah1);
            dbProvider.AddInParameter(command, helper.Rerinediah2, DbType.Decimal, entity.Rerinediah2);
            dbProvider.AddInParameter(command, helper.Rerinediah3, DbType.Decimal, entity.Rerinediah3);
            dbProvider.AddInParameter(command, helper.Rerinediah4, DbType.Decimal, entity.Rerinediah4);
            dbProvider.AddInParameter(command, helper.Rerinediah5, DbType.Decimal, entity.Rerinediah5);
            dbProvider.AddInParameter(command, helper.Rerinediah6, DbType.Decimal, entity.Rerinediah6);
            dbProvider.AddInParameter(command, helper.Rerinediah7, DbType.Decimal, entity.Rerinediah7);
            dbProvider.AddInParameter(command, helper.Rerinediah8, DbType.Decimal, entity.Rerinediah8);
            dbProvider.AddInParameter(command, helper.Rerinediah9, DbType.Decimal, entity.Rerinediah9);
            dbProvider.AddInParameter(command, helper.Rerinediah10, DbType.Decimal, entity.Rerinediah10);
            dbProvider.AddInParameter(command, helper.Rerinediah11, DbType.Decimal, entity.Rerinediah11);
            dbProvider.AddInParameter(command, helper.Rerinediah12, DbType.Decimal, entity.Rerinediah12);
            dbProvider.AddInParameter(command, helper.Rerinediah13, DbType.Decimal, entity.Rerinediah13);
            dbProvider.AddInParameter(command, helper.Rerinediah14, DbType.Decimal, entity.Rerinediah14);
            dbProvider.AddInParameter(command, helper.Rerinediah15, DbType.Decimal, entity.Rerinediah15);
            dbProvider.AddInParameter(command, helper.Rerinediah16, DbType.Decimal, entity.Rerinediah16);
            dbProvider.AddInParameter(command, helper.Rerinediah17, DbType.Decimal, entity.Rerinediah17);
            dbProvider.AddInParameter(command, helper.Rerinediah18, DbType.Decimal, entity.Rerinediah18);
            dbProvider.AddInParameter(command, helper.Rerinediah19, DbType.Decimal, entity.Rerinediah19);
            dbProvider.AddInParameter(command, helper.Rerinediah20, DbType.Decimal, entity.Rerinediah20);
            dbProvider.AddInParameter(command, helper.Rerinediah21, DbType.Decimal, entity.Rerinediah21);
            dbProvider.AddInParameter(command, helper.Rerinediah22, DbType.Decimal, entity.Rerinediah22);
            dbProvider.AddInParameter(command, helper.Rerinediah23, DbType.Decimal, entity.Rerinediah23);
            dbProvider.AddInParameter(command, helper.Rerinediah24, DbType.Decimal, entity.Rerinediah24);
            dbProvider.AddInParameter(command, helper.Rerinediah25, DbType.Decimal, entity.Rerinediah25);
            dbProvider.AddInParameter(command, helper.Rerinediah26, DbType.Decimal, entity.Rerinediah26);
            dbProvider.AddInParameter(command, helper.Rerinediah27, DbType.Decimal, entity.Rerinediah27);
            dbProvider.AddInParameter(command, helper.Rerinediah28, DbType.Decimal, entity.Rerinediah28);
            dbProvider.AddInParameter(command, helper.Rerinediah29, DbType.Decimal, entity.Rerinediah29);
            dbProvider.AddInParameter(command, helper.Rerinediah30, DbType.Decimal, entity.Rerinediah30);
            dbProvider.AddInParameter(command, helper.Rerinediah31, DbType.Decimal, entity.Rerinediah31);
            dbProvider.AddInParameter(command, helper.Rerinediah32, DbType.Decimal, entity.Rerinediah32);
            dbProvider.AddInParameter(command, helper.Rerinediah33, DbType.Decimal, entity.Rerinediah33);
            dbProvider.AddInParameter(command, helper.Rerinediah34, DbType.Decimal, entity.Rerinediah34);
            dbProvider.AddInParameter(command, helper.Rerinediah35, DbType.Decimal, entity.Rerinediah35);
            dbProvider.AddInParameter(command, helper.Rerinediah36, DbType.Decimal, entity.Rerinediah36);
            dbProvider.AddInParameter(command, helper.Rerinediah37, DbType.Decimal, entity.Rerinediah37);
            dbProvider.AddInParameter(command, helper.Rerinediah38, DbType.Decimal, entity.Rerinediah38);
            dbProvider.AddInParameter(command, helper.Rerinediah39, DbType.Decimal, entity.Rerinediah39);
            dbProvider.AddInParameter(command, helper.Rerinediah40, DbType.Decimal, entity.Rerinediah40);
            dbProvider.AddInParameter(command, helper.Rerinediah41, DbType.Decimal, entity.Rerinediah41);
            dbProvider.AddInParameter(command, helper.Rerinediah42, DbType.Decimal, entity.Rerinediah42);
            dbProvider.AddInParameter(command, helper.Rerinediah43, DbType.Decimal, entity.Rerinediah43);
            dbProvider.AddInParameter(command, helper.Rerinediah44, DbType.Decimal, entity.Rerinediah44);
            dbProvider.AddInParameter(command, helper.Rerinediah45, DbType.Decimal, entity.Rerinediah45);
            dbProvider.AddInParameter(command, helper.Rerinediah46, DbType.Decimal, entity.Rerinediah46);
            dbProvider.AddInParameter(command, helper.Rerinediah47, DbType.Decimal, entity.Rerinediah47);
            dbProvider.AddInParameter(command, helper.Rerinediah48, DbType.Decimal, entity.Rerinediah48);
            dbProvider.AddInParameter(command, helper.Rerinediah49, DbType.Decimal, entity.Rerinediah49);
            dbProvider.AddInParameter(command, helper.Rerinediah50, DbType.Decimal, entity.Rerinediah50);
            dbProvider.AddInParameter(command, helper.Rerinediah51, DbType.Decimal, entity.Rerinediah51);
            dbProvider.AddInParameter(command, helper.Rerinediah52, DbType.Decimal, entity.Rerinediah52);
            dbProvider.AddInParameter(command, helper.Rerinediah53, DbType.Decimal, entity.Rerinediah53);
            dbProvider.AddInParameter(command, helper.Rerinediah54, DbType.Decimal, entity.Rerinediah54);
            dbProvider.AddInParameter(command, helper.Rerinediah55, DbType.Decimal, entity.Rerinediah55);
            dbProvider.AddInParameter(command, helper.Rerinediah56, DbType.Decimal, entity.Rerinediah56);
            dbProvider.AddInParameter(command, helper.Rerinediah57, DbType.Decimal, entity.Rerinediah57);
            dbProvider.AddInParameter(command, helper.Rerinediah58, DbType.Decimal, entity.Rerinediah58);
            dbProvider.AddInParameter(command, helper.Rerinediah59, DbType.Decimal, entity.Rerinediah59);
            dbProvider.AddInParameter(command, helper.Rerinediah60, DbType.Decimal, entity.Rerinediah60);
            dbProvider.AddInParameter(command, helper.Rerinediah61, DbType.Decimal, entity.Rerinediah61);
            dbProvider.AddInParameter(command, helper.Rerinediah62, DbType.Decimal, entity.Rerinediah62);
            dbProvider.AddInParameter(command, helper.Rerinediah63, DbType.Decimal, entity.Rerinediah63);
            dbProvider.AddInParameter(command, helper.Rerinediah64, DbType.Decimal, entity.Rerinediah64);
            dbProvider.AddInParameter(command, helper.Rerinediah65, DbType.Decimal, entity.Rerinediah65);
            dbProvider.AddInParameter(command, helper.Rerinediah66, DbType.Decimal, entity.Rerinediah66);
            dbProvider.AddInParameter(command, helper.Rerinediah67, DbType.Decimal, entity.Rerinediah67);
            dbProvider.AddInParameter(command, helper.Rerinediah68, DbType.Decimal, entity.Rerinediah68);
            dbProvider.AddInParameter(command, helper.Rerinediah69, DbType.Decimal, entity.Rerinediah69);
            dbProvider.AddInParameter(command, helper.Rerinediah70, DbType.Decimal, entity.Rerinediah70);
            dbProvider.AddInParameter(command, helper.Rerinediah71, DbType.Decimal, entity.Rerinediah71);
            dbProvider.AddInParameter(command, helper.Rerinediah72, DbType.Decimal, entity.Rerinediah72);
            dbProvider.AddInParameter(command, helper.Rerinediah73, DbType.Decimal, entity.Rerinediah73);
            dbProvider.AddInParameter(command, helper.Rerinediah74, DbType.Decimal, entity.Rerinediah74);
            dbProvider.AddInParameter(command, helper.Rerinediah75, DbType.Decimal, entity.Rerinediah75);
            dbProvider.AddInParameter(command, helper.Rerinediah76, DbType.Decimal, entity.Rerinediah76);
            dbProvider.AddInParameter(command, helper.Rerinediah77, DbType.Decimal, entity.Rerinediah77);
            dbProvider.AddInParameter(command, helper.Rerinediah78, DbType.Decimal, entity.Rerinediah78);
            dbProvider.AddInParameter(command, helper.Rerinediah79, DbType.Decimal, entity.Rerinediah79);
            dbProvider.AddInParameter(command, helper.Rerinediah80, DbType.Decimal, entity.Rerinediah80);
            dbProvider.AddInParameter(command, helper.Rerinediah81, DbType.Decimal, entity.Rerinediah81);
            dbProvider.AddInParameter(command, helper.Rerinediah82, DbType.Decimal, entity.Rerinediah82);
            dbProvider.AddInParameter(command, helper.Rerinediah83, DbType.Decimal, entity.Rerinediah83);
            dbProvider.AddInParameter(command, helper.Rerinediah84, DbType.Decimal, entity.Rerinediah84);
            dbProvider.AddInParameter(command, helper.Rerinediah85, DbType.Decimal, entity.Rerinediah85);
            dbProvider.AddInParameter(command, helper.Rerinediah86, DbType.Decimal, entity.Rerinediah86);
            dbProvider.AddInParameter(command, helper.Rerinediah87, DbType.Decimal, entity.Rerinediah87);
            dbProvider.AddInParameter(command, helper.Rerinediah88, DbType.Decimal, entity.Rerinediah88);
            dbProvider.AddInParameter(command, helper.Rerinediah89, DbType.Decimal, entity.Rerinediah89);
            dbProvider.AddInParameter(command, helper.Rerinediah90, DbType.Decimal, entity.Rerinediah90);
            dbProvider.AddInParameter(command, helper.Rerinediah91, DbType.Decimal, entity.Rerinediah91);
            dbProvider.AddInParameter(command, helper.Rerinediah92, DbType.Decimal, entity.Rerinediah92);
            dbProvider.AddInParameter(command, helper.Rerinediah93, DbType.Decimal, entity.Rerinediah93);
            dbProvider.AddInParameter(command, helper.Rerinediah94, DbType.Decimal, entity.Rerinediah94);
            dbProvider.AddInParameter(command, helper.Rerinediah95, DbType.Decimal, entity.Rerinediah95);
            dbProvider.AddInParameter(command, helper.Rerinediah96, DbType.Decimal, entity.Rerinediah96);
            dbProvider.AddInParameter(command, helper.Rerinediatotal, DbType.Decimal, entity.Rerinediatotal);
            dbProvider.AddInParameter(command, helper.Rerinediausucreacion, DbType.String, entity.Rerinediausucreacion);
            dbProvider.AddInParameter(command, helper.Rerinediafeccreacion, DbType.DateTime, entity.Rerinediafeccreacion);
            
            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(RerInsumoVteaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Rerinscodi, DbType.Int32, entity.Rerinscodi);
            dbProvider.AddInParameter(command, helper.Rerpprcodi, DbType.Int32, entity.Rerpprcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, entity.Pericodi);
            dbProvider.AddInParameter(command, helper.Recacodi, DbType.Int32, entity.Recacodi);
            dbProvider.AddInParameter(command, helper.Rerinediah1, DbType.Decimal, entity.Rerinediah1);
            dbProvider.AddInParameter(command, helper.Rerinediah2, DbType.Decimal, entity.Rerinediah2);
            dbProvider.AddInParameter(command, helper.Rerinediah3, DbType.Decimal, entity.Rerinediah3);
            dbProvider.AddInParameter(command, helper.Rerinediah4, DbType.Decimal, entity.Rerinediah4);
            dbProvider.AddInParameter(command, helper.Rerinediah5, DbType.Decimal, entity.Rerinediah5);
            dbProvider.AddInParameter(command, helper.Rerinediah6, DbType.Decimal, entity.Rerinediah6);
            dbProvider.AddInParameter(command, helper.Rerinediah7, DbType.Decimal, entity.Rerinediah7);
            dbProvider.AddInParameter(command, helper.Rerinediah8, DbType.Decimal, entity.Rerinediah8);
            dbProvider.AddInParameter(command, helper.Rerinediah9, DbType.Decimal, entity.Rerinediah9);
            dbProvider.AddInParameter(command, helper.Rerinediah10, DbType.Decimal, entity.Rerinediah10);
            dbProvider.AddInParameter(command, helper.Rerinediah11, DbType.Decimal, entity.Rerinediah11);
            dbProvider.AddInParameter(command, helper.Rerinediah12, DbType.Decimal, entity.Rerinediah12);
            dbProvider.AddInParameter(command, helper.Rerinediah13, DbType.Decimal, entity.Rerinediah13);
            dbProvider.AddInParameter(command, helper.Rerinediah14, DbType.Decimal, entity.Rerinediah14);
            dbProvider.AddInParameter(command, helper.Rerinediah15, DbType.Decimal, entity.Rerinediah15);
            dbProvider.AddInParameter(command, helper.Rerinediah16, DbType.Decimal, entity.Rerinediah16);
            dbProvider.AddInParameter(command, helper.Rerinediah17, DbType.Decimal, entity.Rerinediah17);
            dbProvider.AddInParameter(command, helper.Rerinediah18, DbType.Decimal, entity.Rerinediah18);
            dbProvider.AddInParameter(command, helper.Rerinediah19, DbType.Decimal, entity.Rerinediah19);
            dbProvider.AddInParameter(command, helper.Rerinediah20, DbType.Decimal, entity.Rerinediah20);
            dbProvider.AddInParameter(command, helper.Rerinediah21, DbType.Decimal, entity.Rerinediah21);
            dbProvider.AddInParameter(command, helper.Rerinediah22, DbType.Decimal, entity.Rerinediah22);
            dbProvider.AddInParameter(command, helper.Rerinediah23, DbType.Decimal, entity.Rerinediah23);
            dbProvider.AddInParameter(command, helper.Rerinediah24, DbType.Decimal, entity.Rerinediah24);
            dbProvider.AddInParameter(command, helper.Rerinediah25, DbType.Decimal, entity.Rerinediah25);
            dbProvider.AddInParameter(command, helper.Rerinediah26, DbType.Decimal, entity.Rerinediah26);
            dbProvider.AddInParameter(command, helper.Rerinediah27, DbType.Decimal, entity.Rerinediah27);
            dbProvider.AddInParameter(command, helper.Rerinediah28, DbType.Decimal, entity.Rerinediah28);
            dbProvider.AddInParameter(command, helper.Rerinediah29, DbType.Decimal, entity.Rerinediah29);
            dbProvider.AddInParameter(command, helper.Rerinediah30, DbType.Decimal, entity.Rerinediah30);
            dbProvider.AddInParameter(command, helper.Rerinediah31, DbType.Decimal, entity.Rerinediah31);
            dbProvider.AddInParameter(command, helper.Rerinediah32, DbType.Decimal, entity.Rerinediah32);
            dbProvider.AddInParameter(command, helper.Rerinediah33, DbType.Decimal, entity.Rerinediah33);
            dbProvider.AddInParameter(command, helper.Rerinediah34, DbType.Decimal, entity.Rerinediah34);
            dbProvider.AddInParameter(command, helper.Rerinediah35, DbType.Decimal, entity.Rerinediah35);
            dbProvider.AddInParameter(command, helper.Rerinediah36, DbType.Decimal, entity.Rerinediah36);
            dbProvider.AddInParameter(command, helper.Rerinediah37, DbType.Decimal, entity.Rerinediah37);
            dbProvider.AddInParameter(command, helper.Rerinediah38, DbType.Decimal, entity.Rerinediah38);
            dbProvider.AddInParameter(command, helper.Rerinediah39, DbType.Decimal, entity.Rerinediah39);
            dbProvider.AddInParameter(command, helper.Rerinediah40, DbType.Decimal, entity.Rerinediah40);
            dbProvider.AddInParameter(command, helper.Rerinediah41, DbType.Decimal, entity.Rerinediah41);
            dbProvider.AddInParameter(command, helper.Rerinediah42, DbType.Decimal, entity.Rerinediah42);
            dbProvider.AddInParameter(command, helper.Rerinediah43, DbType.Decimal, entity.Rerinediah43);
            dbProvider.AddInParameter(command, helper.Rerinediah44, DbType.Decimal, entity.Rerinediah44);
            dbProvider.AddInParameter(command, helper.Rerinediah45, DbType.Decimal, entity.Rerinediah45);
            dbProvider.AddInParameter(command, helper.Rerinediah46, DbType.Decimal, entity.Rerinediah46);
            dbProvider.AddInParameter(command, helper.Rerinediah47, DbType.Decimal, entity.Rerinediah47);
            dbProvider.AddInParameter(command, helper.Rerinediah48, DbType.Decimal, entity.Rerinediah48);
            dbProvider.AddInParameter(command, helper.Rerinediah49, DbType.Decimal, entity.Rerinediah49);
            dbProvider.AddInParameter(command, helper.Rerinediah50, DbType.Decimal, entity.Rerinediah50);
            dbProvider.AddInParameter(command, helper.Rerinediah51, DbType.Decimal, entity.Rerinediah51);
            dbProvider.AddInParameter(command, helper.Rerinediah52, DbType.Decimal, entity.Rerinediah52);
            dbProvider.AddInParameter(command, helper.Rerinediah53, DbType.Decimal, entity.Rerinediah53);
            dbProvider.AddInParameter(command, helper.Rerinediah54, DbType.Decimal, entity.Rerinediah54);
            dbProvider.AddInParameter(command, helper.Rerinediah55, DbType.Decimal, entity.Rerinediah55);
            dbProvider.AddInParameter(command, helper.Rerinediah56, DbType.Decimal, entity.Rerinediah56);
            dbProvider.AddInParameter(command, helper.Rerinediah57, DbType.Decimal, entity.Rerinediah57);
            dbProvider.AddInParameter(command, helper.Rerinediah58, DbType.Decimal, entity.Rerinediah58);
            dbProvider.AddInParameter(command, helper.Rerinediah59, DbType.Decimal, entity.Rerinediah59);
            dbProvider.AddInParameter(command, helper.Rerinediah60, DbType.Decimal, entity.Rerinediah60);
            dbProvider.AddInParameter(command, helper.Rerinediah61, DbType.Decimal, entity.Rerinediah61);
            dbProvider.AddInParameter(command, helper.Rerinediah62, DbType.Decimal, entity.Rerinediah62);
            dbProvider.AddInParameter(command, helper.Rerinediah63, DbType.Decimal, entity.Rerinediah63);
            dbProvider.AddInParameter(command, helper.Rerinediah64, DbType.Decimal, entity.Rerinediah64);
            dbProvider.AddInParameter(command, helper.Rerinediah65, DbType.Decimal, entity.Rerinediah65);
            dbProvider.AddInParameter(command, helper.Rerinediah66, DbType.Decimal, entity.Rerinediah66);
            dbProvider.AddInParameter(command, helper.Rerinediah67, DbType.Decimal, entity.Rerinediah67);
            dbProvider.AddInParameter(command, helper.Rerinediah68, DbType.Decimal, entity.Rerinediah68);
            dbProvider.AddInParameter(command, helper.Rerinediah69, DbType.Decimal, entity.Rerinediah69);
            dbProvider.AddInParameter(command, helper.Rerinediah70, DbType.Decimal, entity.Rerinediah70);
            dbProvider.AddInParameter(command, helper.Rerinediah71, DbType.Decimal, entity.Rerinediah71);
            dbProvider.AddInParameter(command, helper.Rerinediah72, DbType.Decimal, entity.Rerinediah72);
            dbProvider.AddInParameter(command, helper.Rerinediah73, DbType.Decimal, entity.Rerinediah73);
            dbProvider.AddInParameter(command, helper.Rerinediah74, DbType.Decimal, entity.Rerinediah74);
            dbProvider.AddInParameter(command, helper.Rerinediah75, DbType.Decimal, entity.Rerinediah75);
            dbProvider.AddInParameter(command, helper.Rerinediah76, DbType.Decimal, entity.Rerinediah76);
            dbProvider.AddInParameter(command, helper.Rerinediah77, DbType.Decimal, entity.Rerinediah77);
            dbProvider.AddInParameter(command, helper.Rerinediah78, DbType.Decimal, entity.Rerinediah78);
            dbProvider.AddInParameter(command, helper.Rerinediah79, DbType.Decimal, entity.Rerinediah79);
            dbProvider.AddInParameter(command, helper.Rerinediah80, DbType.Decimal, entity.Rerinediah80);
            dbProvider.AddInParameter(command, helper.Rerinediah81, DbType.Decimal, entity.Rerinediah81);
            dbProvider.AddInParameter(command, helper.Rerinediah82, DbType.Decimal, entity.Rerinediah82);
            dbProvider.AddInParameter(command, helper.Rerinediah83, DbType.Decimal, entity.Rerinediah83);
            dbProvider.AddInParameter(command, helper.Rerinediah84, DbType.Decimal, entity.Rerinediah84);
            dbProvider.AddInParameter(command, helper.Rerinediah85, DbType.Decimal, entity.Rerinediah85);
            dbProvider.AddInParameter(command, helper.Rerinediah86, DbType.Decimal, entity.Rerinediah86);
            dbProvider.AddInParameter(command, helper.Rerinediah87, DbType.Decimal, entity.Rerinediah87);
            dbProvider.AddInParameter(command, helper.Rerinediah88, DbType.Decimal, entity.Rerinediah88);
            dbProvider.AddInParameter(command, helper.Rerinediah89, DbType.Decimal, entity.Rerinediah89);
            dbProvider.AddInParameter(command, helper.Rerinediah90, DbType.Decimal, entity.Rerinediah90);
            dbProvider.AddInParameter(command, helper.Rerinediah91, DbType.Decimal, entity.Rerinediah91);
            dbProvider.AddInParameter(command, helper.Rerinediah92, DbType.Decimal, entity.Rerinediah92);
            dbProvider.AddInParameter(command, helper.Rerinediah93, DbType.Decimal, entity.Rerinediah93);
            dbProvider.AddInParameter(command, helper.Rerinediah94, DbType.Decimal, entity.Rerinediah94);
            dbProvider.AddInParameter(command, helper.Rerinediah95, DbType.Decimal, entity.Rerinediah95);
            dbProvider.AddInParameter(command, helper.Rerinediah96, DbType.Decimal, entity.Rerinediah96);
            dbProvider.AddInParameter(command, helper.Rerinediatotal, DbType.Decimal, entity.Rerinediatotal);
            dbProvider.AddInParameter(command, helper.Rerinediausucreacion, DbType.String, entity.Rerinediausucreacion);
            dbProvider.AddInParameter(command, helper.Rerinediafeccreacion, DbType.DateTime, entity.Rerinediafeccreacion);
            dbProvider.AddInParameter(command, helper.Rerinecodi, DbType.Int32, entity.Rerinecodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int rerinecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Rerinecodi, DbType.Int32, rerinecodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public RerInsumoVteaDTO GetById(int rerinecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Rerinecodi, DbType.Int32, rerinecodi);
            RerInsumoVteaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<RerInsumoVteaDTO> List()
        {
            List<RerInsumoVteaDTO> entitys = new List<RerInsumoVteaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RerInsumoVteaDTO entity = helper.Create(dr);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<RerInsumoVteaDTO> GetByCriteria()
        {
            List<RerInsumoVteaDTO> entitys = new List<RerInsumoVteaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RerInsumoVteaDTO entity = helper.Create(dr);
                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public void DeleteByParametroPrimaAndMes(int iRerpprcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteByParametroPrimaAndMes);

            dbProvider.AddInParameter(command, helper.Rerpprcodi, DbType.Int32, iRerpprcodi);
            
            dbProvider.ExecuteNonQuery(command);
        }

        public List<RerInsumoVteaDTO> GetByPeriodo(int reravcodi, string rerpprmes)
        {
            string query = string.Format(helper.SqlGetByPeriodo, reravcodi, rerpprmes);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            List<RerInsumoVteaDTO> entitys = new List<RerInsumoVteaDTO>();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateByPeriodo(dr));
                }
            }

            return entitys;
        }

        public int GetMaxId()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            int id = 1;
            object result = dbProvider.ExecuteScalar(command);
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public void BulkInsertRerInsumoVtea(List<RerInsumoVteaDTO> entitys)
        {
            string nombreTabla = "RER_INSUMO_VTEA";

            dbProvider.AddColumnMapping(helper.Rerinecodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Rerinscodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Rerpprcodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Emprcodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Equicodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Rerinefecdia, DbType.DateTime);
            dbProvider.AddColumnMapping(helper.Pericodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Recacodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Rerinediah1, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinediah2, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinediah3, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinediah4, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinediah5, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinediah6, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinediah7, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinediah8, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinediah9, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinediah10, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinediah11, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinediah12, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinediah13, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinediah14, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinediah15, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinediah16, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinediah17, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinediah18, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinediah19, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinediah20, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinediah21, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinediah22, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinediah23, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinediah24, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinediah25, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinediah26, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinediah27, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinediah28, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinediah29, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinediah30, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinediah31, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinediah32, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinediah33, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinediah34, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinediah35, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinediah36, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinediah37, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinediah38, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinediah39, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinediah40, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinediah41, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinediah42, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinediah43, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinediah44, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinediah45, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinediah46, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinediah47, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinediah48, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinediah49, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinediah50, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinediah51, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinediah52, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinediah53, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinediah54, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinediah55, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinediah56, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinediah57, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinediah58, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinediah59, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinediah60, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinediah61, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinediah62, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinediah63, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinediah64, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinediah65, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinediah66, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinediah67, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinediah68, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinediah69, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinediah70, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinediah71, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinediah72, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinediah73, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinediah74, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinediah75, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinediah76, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinediah77, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinediah78, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinediah79, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinediah80, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinediah81, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinediah82, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinediah83, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinediah84, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinediah85, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinediah86, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinediah87, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinediah88, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinediah89, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinediah90, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinediah91, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinediah92, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinediah93, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinediah94, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinediah95, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinediah96, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinediatotal, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Rerinediausucreacion, DbType.String);
            dbProvider.AddColumnMapping(helper.Rerinediafeccreacion, DbType.DateTime);

            dbProvider.BulkInsert<RerInsumoVteaDTO>(entitys, nombreTabla);
        }

        public decimal ObtenerSaldoVteaByInsumoVTEA(int iRerpprcodi, int iEmprcodi, int iEquicodi)
        {
            decimal dResultado = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerSaldoVteaByInsumoVTEA);
            dbProvider.AddInParameter(command, helper.Rerpprcodi, DbType.Int32, iRerpprcodi);
            dbProvider.AddInParameter(command, helper.Rerpprcodi, DbType.Int32, iEmprcodi);
            dbProvider.AddInParameter(command, helper.Rerpprcodi, DbType.Int32, iEquicodi);
            object result = dbProvider.ExecuteScalar(command);
            if (result != null) dResultado = Convert.ToDecimal(result);

            return dResultado;
        }
    }
}

