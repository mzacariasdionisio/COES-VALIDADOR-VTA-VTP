using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Dominio.DTO.Sic;
using System.Data;
using System.Data.Common;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    public class PrnConfiguracionRepository : RepositoryBase, IPrnConfiguracionRepository
    {
        public PrnConfiguracionRepository(string strConn) : base(strConn)
        {
        }

        PrnConfiguracionHelper helper = new PrnConfiguracionHelper();

        public void Save(PrnConfiguracionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.Prncfgfecha, DbType.DateTime, entity.Prncfgfecha);
            dbProvider.AddInParameter(command, helper.Prncfgporcerrormin, DbType.Decimal, entity.Prncfgporcerrormin);
            dbProvider.AddInParameter(command, helper.Prncfgporcerrormax, DbType.Decimal, entity.Prncfgporcerrormax);
            dbProvider.AddInParameter(command, helper.Prncfgmagcargamin, DbType.Decimal, entity.Prncfgmagcargamin);
            dbProvider.AddInParameter(command, helper.Prncfgmagcargamax, DbType.Decimal, entity.Prncfgmagcargamax);
            dbProvider.AddInParameter(command, helper.Prncfgporcdsvptrn, DbType.Decimal, entity.Prncfgporcdsvptrn);
            dbProvider.AddInParameter(command, helper.Prncfgporcmuestra, DbType.Decimal, entity.Prncfgporcmuestra);
            dbProvider.AddInParameter(command, helper.Prncfgporcdsvcnsc, DbType.Decimal, entity.Prncfgporcdsvcnsc);
            dbProvider.AddInParameter(command, helper.Prncfgnrocoincidn, DbType.Decimal, entity.Prncfgnrocoincidn);
            dbProvider.AddInParameter(command, helper.Prncfgflagveda, DbType.String, entity.Prncfgflagveda);
            dbProvider.AddInParameter(command, helper.Prncfgflagferiado, DbType.String, entity.Prncfgflagferiado);
            dbProvider.AddInParameter(command, helper.Prncfgflagatipico, DbType.String, entity.Prncfgflagatipico);
            dbProvider.AddInParameter(command, helper.Prncfgflagdepauto, DbType.String, entity.Prncfgflagdepauto);
            dbProvider.AddInParameter(command, helper.Prncfgtipopatron, DbType.String, entity.Prncfgtipopatron);
            dbProvider.AddInParameter(command, helper.Prncfgnumdiapatron, DbType.Int32, entity.Prncfgnumdiapatron);
            dbProvider.AddInParameter(command, helper.Prncfgflagdefecto, DbType.String, entity.Prncfgflagdefecto);

            dbProvider.AddInParameter(command, helper.Prncfgpse, DbType.Decimal, entity.Prncfgpse);
            dbProvider.AddInParameter(command, helper.Prncfgfactorf, DbType.Decimal, entity.Prncfgfactorf);

            dbProvider.AddInParameter(command, helper.Prncfgusucreacion, DbType.String, entity.Prncfgusucreacion);
            dbProvider.AddInParameter(command, helper.Prncfgfeccreacion, DbType.DateTime, entity.Prncfgfeccreacion);
            dbProvider.AddInParameter(command, helper.Prncfgusumodificacion, DbType.String, entity.Prncfgusumodificacion);
            dbProvider.AddInParameter(command, helper.Prncfgfecmodificacion, DbType.DateTime, entity.Prncfgfecmodificacion);

            dbProvider.ExecuteNonQuery(command);            
        }

        public void Update(PrnConfiguracionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);
            //set
            dbProvider.AddInParameter(command, helper.Prncfgporcerrormin, DbType.Decimal, entity.Prncfgporcerrormin);
            dbProvider.AddInParameter(command, helper.Prncfgporcerrormax, DbType.Decimal, entity.Prncfgporcerrormax);
            dbProvider.AddInParameter(command, helper.Prncfgmagcargamin, DbType.Decimal, entity.Prncfgmagcargamin);
            dbProvider.AddInParameter(command, helper.Prncfgmagcargamax, DbType.Decimal, entity.Prncfgmagcargamax);
            dbProvider.AddInParameter(command, helper.Prncfgporcdsvptrn, DbType.Decimal, entity.Prncfgporcdsvptrn);
            dbProvider.AddInParameter(command, helper.Prncfgporcmuestra, DbType.Decimal, entity.Prncfgporcmuestra);
            dbProvider.AddInParameter(command, helper.Prncfgporcdsvcnsc, DbType.Decimal, entity.Prncfgporcdsvcnsc);
            dbProvider.AddInParameter(command, helper.Prncfgnrocoincidn, DbType.Decimal, entity.Prncfgnrocoincidn);
            dbProvider.AddInParameter(command, helper.Prncfgflagveda, DbType.String, entity.Prncfgflagveda);
            dbProvider.AddInParameter(command, helper.Prncfgflagferiado, DbType.String, entity.Prncfgflagferiado);
            dbProvider.AddInParameter(command, helper.Prncfgflagatipico, DbType.String, entity.Prncfgflagatipico);
            dbProvider.AddInParameter(command, helper.Prncfgflagdepauto, DbType.String, entity.Prncfgflagdepauto);
            dbProvider.AddInParameter(command, helper.Prncfgtipopatron, DbType.String, entity.Prncfgtipopatron);
            dbProvider.AddInParameter(command, helper.Prncfgnumdiapatron, DbType.Int32, entity.Prncfgnumdiapatron);
            dbProvider.AddInParameter(command, helper.Prncfgflagdefecto, DbType.String, entity.Prncfgflagdefecto);

            dbProvider.AddInParameter(command, helper.Prncfgpse, DbType.Decimal, entity.Prncfgpse);
            dbProvider.AddInParameter(command, helper.Prncfgfactorf, DbType.Decimal, entity.Prncfgfactorf);

            dbProvider.AddInParameter(command, helper.Prncfgusumodificacion, DbType.String, entity.Prncfgusumodificacion);
            dbProvider.AddInParameter(command, helper.Prncfgfecmodificacion, DbType.DateTime, entity.Prncfgfecmodificacion);
            //where
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.Prncfgfecha, DbType.DateTime, entity.Prncfgfecha);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int ptomedicodi, DateTime prncnffecha)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, ptomedicodi);
            dbProvider.AddInParameter(command, helper.Prncfgfecha, DbType.DateTime, prncnffecha);

            dbProvider.ExecuteNonQuery(command);
        }

        public PrnConfiguracionDTO GetById(int ptomedicodi, DateTime prncnffecha)
        {
            PrnConfiguracionDTO entity = new PrnConfiguracionDTO();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, ptomedicodi);
            dbProvider.AddInParameter(command, helper.Prncfgfecha, DbType.DateTime, prncnffecha);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<PrnConfiguracionDTO> List()
        {
            List<PrnConfiguracionDTO> entitys = new List<PrnConfiguracionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public void BulkInsert(List<PrnConfiguracionDTO> entitys)
        {
            dbProvider.AddColumnMapping(helper.Ptomedicodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Prncfgfecha, DbType.DateTime);
            dbProvider.AddColumnMapping(helper.Prncfgporcerrormin, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Prncfgporcerrormax, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Prncfgmagcargamin, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Prncfgmagcargamax, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Prncfgporcdsvptrn, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Prncfgporcmuestra, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Prncfgporcdsvcnsc, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Prncfgnrocoincidn, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Prncfgflagveda, DbType.String);
            dbProvider.AddColumnMapping(helper.Prncfgflagferiado, DbType.String);
            dbProvider.AddColumnMapping(helper.Prncfgflagatipico, DbType.String);
            dbProvider.AddColumnMapping(helper.Prncfgflagdepauto, DbType.String);
            dbProvider.AddColumnMapping(helper.Prncfgtipopatron, DbType.String);
            dbProvider.AddColumnMapping(helper.Prncfgnumdiapatron, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Prncfgflagdefecto, DbType.String);
            dbProvider.AddColumnMapping(helper.Prncfgusucreacion, DbType.String);
            dbProvider.AddColumnMapping(helper.Prncfgfeccreacion, DbType.DateTime);
            dbProvider.AddColumnMapping(helper.Prncfgusumodificacion, DbType.String);
            dbProvider.AddColumnMapping(helper.Prncfgfecmodificacion, DbType.DateTime);

            dbProvider.BulkInsert<PrnConfiguracionDTO>(entitys, helper.TableName);
        }

        public List<PrnConfiguracionDTO> ParametrosList(string fecdesde, string fechasta, string lstpuntos)
        {
            List<PrnConfiguracionDTO> entitys = new List<PrnConfiguracionDTO>();
            string query = String.Format(helper.SqlParametrosList, lstpuntos, fecdesde, fechasta);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrnConfiguracionDTO entity = helper.Create(dr);

                    int iPtomedidesc = dr.GetOrdinal(this.helper.Ptomedidesc);
                    if (!dr.IsDBNull(iPtomedidesc)) entity.Ptomedidesc = dr.GetString(iPtomedidesc);

                    int iPrncfgfecha = dr.GetOrdinal(this.helper.Prncfgfecha);
                    if (!dr.IsDBNull(iPrncfgfecha)) entity.StrPrncfgfecha = dr.GetDateTime(iPrncfgfecha).ToString(this.helper.FormatoFecha);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public PrnConfiguracionDTO GetConfiguracion(int ptomedicodi, string fecha, int defid, string deffecha)
        {
            PrnConfiguracionDTO entity = new PrnConfiguracionDTO();
            string query = String.Format(helper.SqlGetConfiguracion, ptomedicodi, fecha, defid, deffecha);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);
                    int iPrncfgtiporeg = dr.GetOrdinal(this.helper.Prncfgtiporeg);
                    if (!dr.IsDBNull(iPrncfgtiporeg)) entity.Prncfgtiporeg = Convert.ToInt32(dr.GetValue(iPrncfgtiporeg));
                }
            }

            return entity;
        }

    }
}
