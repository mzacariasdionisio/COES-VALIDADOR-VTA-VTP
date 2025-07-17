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
    public class PrnConfiguracionFormulaRepository : RepositoryBase, IPrnConfiguracionFormulaRepository
    {
        public PrnConfiguracionFormulaRepository(string strConn)
         : base(strConn)
        {
        }

        PrnConfiguracionFormulaHelper helper = new PrnConfiguracionFormulaHelper();

        public List<PrnConfiguracionFormulaDTO> List()
        {
            List<PrnConfiguracionFormulaDTO> entitys = new List<PrnConfiguracionFormulaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrnConfiguracionFormulaDTO entity = new PrnConfiguracionFormulaDTO();

                    int iCnffrmcodi = dr.GetOrdinal(helper.Cnffrmcodi);
                    if (!dr.IsDBNull(iCnffrmcodi)) entity.Cnffrmcodi = Convert.ToInt32(dr.GetValue(iCnffrmcodi));

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iCnffrmfecha = dr.GetOrdinal(helper.Cnffrmfecha);
                    if (!dr.IsDBNull(iCnffrmfecha)) entity.Cnffrmfecha = dr.GetDateTime(iCnffrmfecha);

                    int iCnffrmferiado = dr.GetOrdinal(helper.Cnffrmferiado);
                    if (!dr.IsDBNull(iCnffrmferiado)) entity.Cnffrmferiado = dr.GetString(iCnffrmferiado);

                    int iCnffrmatipico = dr.GetOrdinal(helper.Cnffrmatipico);
                    if (!dr.IsDBNull(iCnffrmatipico)) entity.Cnffrmatipico = dr.GetString(iCnffrmatipico);

                    int iCnffrmveda = dr.GetOrdinal(helper.Cnffrmveda);
                    if (!dr.IsDBNull(iCnffrmveda)) entity.Cnffrmveda = dr.GetString(iCnffrmveda);

                    int iCnffrmdepauto = dr.GetOrdinal(helper.Cnffrmdepauto);
                    if (!dr.IsDBNull(iCnffrmdepauto)) entity.Cnffrmdepauto = dr.GetString(iCnffrmdepauto);

                    int iCnffrmcargamax = dr.GetOrdinal(helper.Cnffrmcargamax);
                    if (!dr.IsDBNull(iCnffrmcargamax)) entity.Cnffrmcargamax = dr.GetDecimal(iCnffrmcargamax);

                    int iCnffrmcargamin = dr.GetOrdinal(helper.Cnffrmcargamin);
                    if (!dr.IsDBNull(iCnffrmcargamin)) entity.Cnffrmcargamin = dr.GetDecimal(iCnffrmcargamin);

                    int iCnffrmtolerancia = dr.GetOrdinal(helper.Cnffrmtolerancia);
                    if (!dr.IsDBNull(iCnffrmtolerancia)) entity.Cnffrmtolerancia = dr.GetDecimal(iCnffrmtolerancia);

                    int iCnffrmusucreacion = dr.GetOrdinal(helper.Cnffrmusucreacion);
                    if (!dr.IsDBNull(iCnffrmusucreacion)) entity.Cnffrmusucreacion = dr.GetString(iCnffrmusucreacion);

                    int iCnffrmfeccreacion = dr.GetOrdinal(helper.Cnffrmfeccreacion);
                    if (!dr.IsDBNull(iCnffrmfeccreacion)) entity.Cnffrmfeccreacion = dr.GetDateTime(iCnffrmfeccreacion);

                    int iCnffrmusumodificacion = dr.GetOrdinal(helper.Cnffrmusumodificacion);
                    if (!dr.IsDBNull(iCnffrmusumodificacion)) entity.Cnffrmusumodificacion = dr.GetString(iCnffrmusumodificacion);

                    int iCnffrmfecmodificacion = dr.GetOrdinal(helper.Cnffrmfecmodificacion);
                    if (!dr.IsDBNull(iCnffrmfecmodificacion)) entity.Cnffrmfecmodificacion = dr.GetDateTime(iCnffrmfecmodificacion);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public void Save(PrnConfiguracionFormulaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);
            dbProvider.AddInParameter(command, helper.Cnffrmcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Cnffrmformula, DbType.Int32, entity.Cnffrmformula);
            dbProvider.AddInParameter(command, helper.Cnffrmdiapatron, DbType.Int32, entity.Cnffrmdiapatron);
            dbProvider.AddInParameter(command, helper.Cnffrmfecha, DbType.DateTime, entity.Cnffrmfecha);
            dbProvider.AddInParameter(command, helper.Cnffrmferiado, DbType.String, entity.Cnffrmferiado);
            dbProvider.AddInParameter(command, helper.Cnffrmatipico, DbType.String, entity.Cnffrmatipico);
            dbProvider.AddInParameter(command, helper.Cnffrmveda, DbType.String, entity.Cnffrmveda);
            dbProvider.AddInParameter(command, helper.Cnffrmpatron, DbType.String, entity.Cnffrmpatron);
            dbProvider.AddInParameter(command, helper.Cnffrmdefecto, DbType.String, entity.Cnffrmdefecto);
            dbProvider.AddInParameter(command, helper.Cnffrmdepauto, DbType.String, entity.Cnffrmdepauto);
            dbProvider.AddInParameter(command, helper.Cnffrmcargamax, DbType.Decimal, entity.Cnffrmcargamax);
            dbProvider.AddInParameter(command, helper.Cnffrmcargamin, DbType.Decimal, entity.Cnffrmcargamin);
            dbProvider.AddInParameter(command, helper.Cnffrmtolerancia, DbType.Decimal, entity.Cnffrmtolerancia);
            dbProvider.AddInParameter(command, helper.Cnffrmusucreacion, DbType.String, entity.Cnffrmusucreacion);
            dbProvider.AddInParameter(command, helper.Cnffrmfeccreacion, DbType.DateTime, entity.Cnffrmfeccreacion);
            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(PrnConfiguracionFormulaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);
            dbProvider.AddInParameter(command, helper.Cnffrmformula, DbType.Int32, entity.Cnffrmformula);
            dbProvider.AddInParameter(command, helper.Cnffrmdiapatron, DbType.Int32, entity.Cnffrmdiapatron);
            dbProvider.AddInParameter(command, helper.Cnffrmferiado, DbType.String, entity.Cnffrmferiado);
            dbProvider.AddInParameter(command, helper.Cnffrmatipico, DbType.String, entity.Cnffrmatipico);
            dbProvider.AddInParameter(command, helper.Cnffrmveda, DbType.String, entity.Cnffrmveda);
            dbProvider.AddInParameter(command, helper.Cnffrmpatron, DbType.String, entity.Cnffrmpatron);
            dbProvider.AddInParameter(command, helper.Cnffrmdefecto, DbType.String, entity.Cnffrmdefecto);
            dbProvider.AddInParameter(command, helper.Cnffrmdepauto, DbType.String, entity.Cnffrmdepauto);
            dbProvider.AddInParameter(command, helper.Cnffrmcargamax, DbType.Decimal, entity.Cnffrmcargamax);
            dbProvider.AddInParameter(command, helper.Cnffrmcargamin, DbType.Decimal, entity.Cnffrmcargamin);
            dbProvider.AddInParameter(command, helper.Cnffrmtolerancia, DbType.Decimal, entity.Cnffrmtolerancia);
            dbProvider.AddInParameter(command, helper.Cnffrmusumodificacion, DbType.String, entity.Cnffrmusumodificacion);
            dbProvider.AddInParameter(command, helper.Cnffrmfecmodificacion, DbType.DateTime, entity.Cnffrmfecmodificacion);
            dbProvider.AddInParameter(command, helper.Cnffrmcodi, DbType.Int32, entity.Cnffrmcodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public PrnConfiguracionFormulaDTO GetById(int codigo)
        {
            PrnConfiguracionFormulaDTO entity = new PrnConfiguracionFormulaDTO();

            string query = string.Format(helper.SqlGetById, codigo);
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

        public void Delete(int codigo)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Cnffrmcodi, DbType.Int32, codigo);
            dbProvider.ExecuteNonQuery(command);
        }

        public List<PrnConfiguracionFormulaDTO> ListConfiguracionFormulaByFiltros(DateTime inicio, DateTime fin, string formula)
        {
            List<PrnConfiguracionFormulaDTO> entitys = new List<PrnConfiguracionFormulaDTO>();
            string query = string.Format(helper.SqlListConfiguracionFormulaByFiltros, inicio.ToString("dd/MM/yyyy"), fin.ToString("dd/MM/yyyy"), formula);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrnConfiguracionFormulaDTO entity = new PrnConfiguracionFormulaDTO();

                    int iCnffrmcodi = dr.GetOrdinal(helper.Cnffrmcodi);
                    if (!dr.IsDBNull(iCnffrmcodi)) entity.Cnffrmcodi = Convert.ToInt32(dr.GetValue(iCnffrmcodi));

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iPtomedidesc = dr.GetOrdinal(helper.Ptomedidesc);
                    if (!dr.IsDBNull(iPtomedidesc)) entity.Ptomedidesc = dr.GetString(iPtomedidesc);

                    int iCnffrmfecha = dr.GetOrdinal(helper.Cnffrmfecha);
                    if (!dr.IsDBNull(iCnffrmfecha)) entity.Cnffrmfecha = dr.GetDateTime(iCnffrmfecha);

                    int iCnffrmferiado = dr.GetOrdinal(helper.Cnffrmferiado);
                    if (!dr.IsDBNull(iCnffrmferiado)) entity.Cnffrmferiado = dr.GetString(iCnffrmferiado);

                    int iCnffrmatipico = dr.GetOrdinal(helper.Cnffrmatipico);
                    if (!dr.IsDBNull(iCnffrmatipico)) entity.Cnffrmatipico = dr.GetString(iCnffrmatipico);

                    int iCnffrmveda = dr.GetOrdinal(helper.Cnffrmveda);
                    if (!dr.IsDBNull(iCnffrmveda)) entity.Cnffrmveda = dr.GetString(iCnffrmveda);

                    int iCnffrmdepauto = dr.GetOrdinal(helper.Cnffrmdepauto);
                    if (!dr.IsDBNull(iCnffrmdepauto)) entity.Cnffrmdepauto = dr.GetString(iCnffrmdepauto);

                    int iCnffrmcargamax = dr.GetOrdinal(helper.Cnffrmcargamax);
                    if (!dr.IsDBNull(iCnffrmcargamax)) entity.Cnffrmcargamax = dr.GetDecimal(iCnffrmcargamax);

                    int iCnffrmcargamin = dr.GetOrdinal(helper.Cnffrmcargamin);
                    if (!dr.IsDBNull(iCnffrmcargamin)) entity.Cnffrmcargamin = dr.GetDecimal(iCnffrmcargamin);

                    int iCnffrmtolerancia = dr.GetOrdinal(helper.Cnffrmtolerancia);
                    if (!dr.IsDBNull(iCnffrmtolerancia)) entity.Cnffrmtolerancia = dr.GetDecimal(iCnffrmtolerancia);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public PrnConfiguracionFormulaDTO GetIdByCodigoFecha(int ptomedicodi, DateTime cnffrmfecha)
        {
            PrnConfiguracionFormulaDTO entity = new PrnConfiguracionFormulaDTO();
            string query = string.Format(helper.SqlGetIdByCodigoFecha, ptomedicodi, cnffrmfecha.ToString("dd/MM/yyyy"));
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    int iCnffrmcodi = dr.GetOrdinal(helper.Cnffrmcodi);
                    if (!dr.IsDBNull(iCnffrmcodi)) entity.Cnffrmcodi = Convert.ToInt32(dr.GetValue(iCnffrmcodi));

                    int iCnffrmFormula = dr.GetOrdinal(helper.Cnffrmformula);
                    if (!dr.IsDBNull(iCnffrmFormula)) entity.Cnffrmformula = Convert.ToInt32(dr.GetValue(iCnffrmFormula));

                }
            }

            return entity;
        }

        public PrnConfiguracionFormulaDTO GetParametrosByIdFecha(int ptomedicodi, DateTime prncnffecha)
        {
            PrnConfiguracionFormulaDTO entity = new PrnConfiguracionFormulaDTO();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetParametrosByIdFecha);

            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, ptomedicodi);
            dbProvider.AddInParameter(command, helper.Cnffrmfecha, DbType.DateTime, prncnffecha);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    int iCnffrmcodi = dr.GetOrdinal(helper.Cnffrmcodi);
                    if (!dr.IsDBNull(iCnffrmcodi)) entity.Cnffrmcodi = Convert.ToInt32(dr.GetValue(iCnffrmcodi));

                    int iCnffrmformula= dr.GetOrdinal(helper.Cnffrmformula);
                    if (!dr.IsDBNull(iCnffrmformula)) entity.Cnffrmformula = Convert.ToInt32(dr.GetValue(iCnffrmformula));

                    int iCnffrmdiapatron = dr.GetOrdinal(helper.Cnffrmdiapatron);
                    if (!dr.IsDBNull(iCnffrmdiapatron)) entity.Cnffrmdiapatron = Convert.ToInt32(dr.GetValue(iCnffrmdiapatron));

                    int iCnffrmfecha = dr.GetOrdinal(helper.Cnffrmfecha);
                    if (!dr.IsDBNull(iCnffrmfecha)) entity.Cnffrmfecha = dr.GetDateTime(iCnffrmfecha);

                    int iCnffrmferiado = dr.GetOrdinal(helper.Cnffrmferiado);
                    if (!dr.IsDBNull(iCnffrmferiado)) entity.Cnffrmferiado = dr.GetString(iCnffrmferiado);

                    int iCnffrmatipico = dr.GetOrdinal(helper.Cnffrmatipico);
                    if (!dr.IsDBNull(iCnffrmatipico)) entity.Cnffrmatipico = dr.GetString(iCnffrmatipico);

                    int iCnffrmveda = dr.GetOrdinal(helper.Cnffrmveda);
                    if (!dr.IsDBNull(iCnffrmveda)) entity.Cnffrmveda = dr.GetString(iCnffrmveda);

                    int iCnffrmpatron = dr.GetOrdinal(helper.Cnffrmpatron);
                    if (!dr.IsDBNull(iCnffrmpatron)) entity.Cnffrmpatron = dr.GetString(iCnffrmpatron);

                    int iCnffrmdefecto = dr.GetOrdinal(helper.Cnffrmdefecto);
                    if (!dr.IsDBNull(iCnffrmdefecto)) entity.Cnffrmdefecto = dr.GetString(iCnffrmdefecto);

                    int iCnffrmdepauto = dr.GetOrdinal(helper.Cnffrmdepauto);
                    if (!dr.IsDBNull(iCnffrmdepauto)) entity.Cnffrmdepauto = dr.GetString(iCnffrmdepauto);

                    int iCnffrmcargamax = dr.GetOrdinal(helper.Cnffrmcargamax);
                    if (!dr.IsDBNull(iCnffrmcargamax)) entity.Cnffrmcargamax = dr.GetDecimal(iCnffrmcargamax);

                    int iCnffrmcargamin = dr.GetOrdinal(helper.Cnffrmcargamin);
                    if (!dr.IsDBNull(iCnffrmcargamin)) entity.Cnffrmcargamin = dr.GetDecimal(iCnffrmcargamin);

                    int iCnffrmtolerancia = dr.GetOrdinal(helper.Cnffrmtolerancia);
                    if (!dr.IsDBNull(iCnffrmtolerancia)) entity.Cnffrmtolerancia = dr.GetDecimal(iCnffrmtolerancia);

                }
            }

            return entity;
        }

        public List<PrnConfiguracionFormulaDTO> ParametrosFormulasList(string fecdesde, string fechasta, string lstpuntos)
        {
            List<PrnConfiguracionFormulaDTO> entitys = new List<PrnConfiguracionFormulaDTO>();
            string query = String.Format(helper.SqlParametrosFormulasList, fecdesde, fechasta, lstpuntos);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrnConfiguracionFormulaDTO entity = new PrnConfiguracionFormulaDTO();

                    int iCnffrmcodi = dr.GetOrdinal(helper.Cnffrmcodi);
                    if (!dr.IsDBNull(iCnffrmcodi)) entity.Cnffrmcodi = Convert.ToInt32(dr.GetValue(iCnffrmcodi));

                    int iCnffrmformula = dr.GetOrdinal(helper.Cnffrmformula);
                    if (!dr.IsDBNull(iCnffrmformula)) entity.Cnffrmformula = Convert.ToInt32(dr.GetValue(iCnffrmformula));

                    int iCnffrmdiapatron = dr.GetOrdinal(helper.Cnffrmdiapatron);
                    if (!dr.IsDBNull(iCnffrmdiapatron)) entity.Cnffrmdiapatron = Convert.ToInt32(dr.GetValue(iCnffrmdiapatron));

                    int iPrruabrev = dr.GetOrdinal(helper.Prruabrev);
                    if (!dr.IsDBNull(iPrruabrev)) entity.Prruabrev = dr.GetString(iPrruabrev);

                    int iCnffrmfecha = dr.GetOrdinal(helper.Cnffrmfecha);
                    if (!dr.IsDBNull(iCnffrmfecha)) entity.Cnffrmfecha = dr.GetDateTime(iCnffrmfecha);

                    int iCnffrmferiado = dr.GetOrdinal(helper.Cnffrmferiado);
                    if (!dr.IsDBNull(iCnffrmferiado)) entity.Cnffrmferiado = dr.GetString(iCnffrmferiado);

                    int iCnffrmatipico = dr.GetOrdinal(helper.Cnffrmatipico);
                    if (!dr.IsDBNull(iCnffrmatipico)) entity.Cnffrmatipico = dr.GetString(iCnffrmatipico);

                    int iCnffrmveda = dr.GetOrdinal(helper.Cnffrmveda);
                    if (!dr.IsDBNull(iCnffrmveda)) entity.Cnffrmveda = dr.GetString(iCnffrmveda);

                    int iCnffrmpatron = dr.GetOrdinal(helper.Cnffrmpatron);
                    if (!dr.IsDBNull(iCnffrmpatron)) entity.Cnffrmpatron = dr.GetString(iCnffrmpatron);

                    int iCnffrmdefecto = dr.GetOrdinal(helper.Cnffrmdefecto);
                    if (!dr.IsDBNull(iCnffrmdefecto)) entity.Cnffrmdefecto = dr.GetString(iCnffrmdefecto);

                    int iCnffrmdepauto = dr.GetOrdinal(helper.Cnffrmdepauto);
                    if (!dr.IsDBNull(iCnffrmdepauto)) entity.Cnffrmdepauto = dr.GetString(iCnffrmdepauto);

                    int iCnffrmcargamax = dr.GetOrdinal(helper.Cnffrmcargamax);
                    if (!dr.IsDBNull(iCnffrmcargamax)) entity.Cnffrmcargamax = dr.GetDecimal(iCnffrmcargamax);

                    int iCnffrmcargamin = dr.GetOrdinal(helper.Cnffrmcargamin);
                    if (!dr.IsDBNull(iCnffrmcargamin)) entity.Cnffrmcargamin = dr.GetDecimal(iCnffrmcargamin);

                    int iCnffrmtolerancia = dr.GetOrdinal(helper.Cnffrmtolerancia);
                    if (!dr.IsDBNull(iCnffrmtolerancia)) entity.Cnffrmtolerancia = dr.GetDecimal(iCnffrmtolerancia);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
