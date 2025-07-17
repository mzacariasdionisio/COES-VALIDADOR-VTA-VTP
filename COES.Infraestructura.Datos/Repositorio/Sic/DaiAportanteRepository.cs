using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Sic;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla DAI_APORTANTE
    /// </summary>
    public class DaiAportanteRepository: RepositoryBase, IDaiAportanteRepository
    {
        public DaiAportanteRepository(string strConn): base(strConn)
        {
        }

        DaiAportanteHelper helper = new DaiAportanteHelper();

        public int Save(DaiAportanteDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Aporcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Prescodi, DbType.Int32, entity.Prescodi);
            dbProvider.AddInParameter(command, helper.Tabcdcodiestado, DbType.Int32, entity.Tabcdcodiestado);
            dbProvider.AddInParameter(command, helper.Aporporcentajeparticipacion, DbType.Decimal, entity.Aporporcentajeparticipacion);
            dbProvider.AddInParameter(command, helper.Apormontoparticipacion, DbType.Decimal, entity.Apormontoparticipacion);
            dbProvider.AddInParameter(command, helper.Aporaniosinaporte, DbType.Int32, entity.Aporaniosinaporte);
            dbProvider.AddInParameter(command, helper.Aporliquidado, DbType.String, entity.Aporliquidado);
            dbProvider.AddInParameter(command, helper.Aporusuliquidacion, DbType.String, entity.Aporusuliquidacion);
            dbProvider.AddInParameter(command, helper.Aporfecliquidacion, DbType.DateTime, entity.Aporfecliquidacion);
            dbProvider.AddInParameter(command, helper.Aporactivo, DbType.String, entity.Aporactivo);
            dbProvider.AddInParameter(command, helper.Aporusucreacion, DbType.String, entity.Aporusucreacion);
            dbProvider.AddInParameter(command, helper.Aprofeccreacion, DbType.DateTime, entity.Aprofeccreacion);
            dbProvider.AddInParameter(command, helper.Aporusumodificacion, DbType.String, entity.Aporusumodificacion);
            dbProvider.AddInParameter(command, helper.Aporfecmodificacion, DbType.DateTime, entity.Aporfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(DaiAportanteDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Prescodi, DbType.Int32, entity.Prescodi);
            dbProvider.AddInParameter(command, helper.Tabcdcodiestado, DbType.Int32, entity.Tabcdcodiestado);
            dbProvider.AddInParameter(command, helper.Aporaniosinaporte, DbType.Int32, entity.Aporaniosinaporte);
            dbProvider.AddInParameter(command, helper.Aporliquidado, DbType.String, entity.Aporliquidado);
            dbProvider.AddInParameter(command, helper.Aporusuliquidacion, DbType.String, entity.Aporusuliquidacion);
            dbProvider.AddInParameter(command, helper.Aporfecliquidacion, DbType.DateTime, entity.Aporfecliquidacion);
            dbProvider.AddInParameter(command, helper.Aporusumodificacion, DbType.String, entity.Aporusumodificacion);
            dbProvider.AddInParameter(command, helper.Aporfecmodificacion, DbType.DateTime, entity.Aporfecmodificacion);
            dbProvider.AddInParameter(command, helper.Aporcodi, DbType.Int32, entity.Aporcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int aporcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Aporcodi, DbType.Int32, aporcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteByPresupuesto(DaiAportanteDTO aportante)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteByPresupuesto);

            dbProvider.AddInParameter(command, helper.Aporusumodificacion, DbType.String, aportante.Aporusumodificacion);
            dbProvider.AddInParameter(command, helper.Aporfecmodificacion, DbType.DateTime, aportante.Aporfecmodificacion);
            dbProvider.AddInParameter(command, helper.Prescodi, DbType.Int32, aportante.Prescodi);

            dbProvider.ExecuteNonQuery(command);
        }
        

        public DaiAportanteDTO GetById(int aporcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Aporcodi, DbType.Int32, aporcodi);
            DaiAportanteDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEmprrazsocial = dr.GetOrdinal(helper.Emprrazsocial);
                    if (!dr.IsDBNull(iEmprrazsocial)) entity.Emprrazsocial = dr.GetString(iEmprrazsocial);

                    int iEmprruc = dr.GetOrdinal(helper.Emprruc);
                    if (!dr.IsDBNull(iEmprruc)) entity.Emprruc = dr.GetString(iEmprruc);
                }
            }

            return entity;
        }

        public List<DaiAportanteDTO> List()
        {
            List<DaiAportanteDTO> entitys = new List<DaiAportanteDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    DaiAportanteDTO aportante = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) aportante.Emprnomb = dr.GetString(iEmprnomb);

                    int iEmprrazsocial = dr.GetOrdinal(helper.Emprrazsocial);
                    if (!dr.IsDBNull(iEmprrazsocial)) aportante.Emprrazsocial = dr.GetString(iEmprrazsocial);

                    int iEmprruc = dr.GetOrdinal(helper.Emprruc);
                    if (!dr.IsDBNull(iEmprruc)) aportante.Emprruc = dr.GetString(iEmprruc);

                    int iTipoempresa = dr.GetOrdinal(helper.Tipoempresa);
                    if (!dr.IsDBNull(iTipoempresa)) aportante.Tipoempresa = dr.GetString(iTipoempresa);

                    int iEstadoaportante = dr.GetOrdinal(helper.Estadoaportante);
                    if (!dr.IsDBNull(iEstadoaportante)) aportante.Estadoaportante = dr.GetString(iEstadoaportante);

                    int iEstado = dr.GetOrdinal(helper.Estado);
                    if (!dr.IsDBNull(iEstado)) aportante.EstadoImportado = Convert.ToInt32(dr.GetValue(iEstado));

                    entitys.Add(aportante);
                }
            }

            return entitys;
        }

        public List<DaiAportanteDTO> ListCuadroDevolucion(decimal igv, int anio, int estado)
        {
            List<DaiAportanteDTO> entitys = new List<DaiAportanteDTO>();

            string sql = string.Format(helper.SqlListCuadroDevolucion, igv, anio, estado);

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    DaiAportanteDTO aportante = new DaiAportanteDTO();
                    
                        int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) aportante.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) aportante.Emprnomb = dr.GetString(iEmprnomb);

                    int iEmprrazsocial = dr.GetOrdinal(helper.Emprrazsocial);
                    if (!dr.IsDBNull(iEmprrazsocial)) aportante.Emprrazsocial = dr.GetString(iEmprrazsocial);

                    int iEmprruc = dr.GetOrdinal(helper.Emprruc);
                    if (!dr.IsDBNull(iEmprruc)) aportante.Emprruc = dr.GetString(iEmprruc);

                    int iTipoempresa = dr.GetOrdinal(helper.Tipoempresa);
                    if (!dr.IsDBNull(iTipoempresa)) aportante.Tipoempresa = dr.GetString(iTipoempresa);

                    int iCaleinteres = dr.GetOrdinal(helper.Caleinteres);
                    if (!dr.IsDBNull(iCaleinteres)) aportante.Caleinteres = Convert.ToDecimal(dr.GetValue(iCaleinteres));

                    int iCaleinteresigv = dr.GetOrdinal(helper.Caleinteresigv);
                    if (!dr.IsDBNull(iCaleinteresigv)) aportante.Caleinteresigv = Convert.ToDecimal(dr.GetValue(iCaleinteresigv));

                    int iTotalinteresesigv = dr.GetOrdinal(helper.Totalinteresesigv);
                    if (!dr.IsDBNull(iTotalinteresesigv)) aportante.Totalinteresesigv = Convert.ToDecimal(dr.GetValue(iTotalinteresesigv));

                    int iAmortizacion = dr.GetOrdinal(helper.Amortizacion);
                    if (!dr.IsDBNull(iAmortizacion)) aportante.Amortizacion = Convert.ToDecimal(dr.GetValue(iAmortizacion));

                    int iTotal = dr.GetOrdinal(helper.Total);
                    if (!dr.IsDBNull(iTotal)) aportante.Total = Convert.ToDecimal(dr.GetValue(iTotal));

                    entitys.Add(aportante);
                }
            }

            return entitys;
        }

        public List<DaiAportanteDTO> GetByCriteria(int prescodi, int tabcdcodiestado)
        {
            List<DaiAportanteDTO> entitys = new List<DaiAportanteDTO>();

            string sql = string.Format(helper.SqlGetByCriteria, prescodi, tabcdcodiestado > 0 ? " and tabcdcodiestado = " + tabcdcodiestado : "");

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    DaiAportanteDTO aportante = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) aportante.Emprnomb = dr.GetString(iEmprnomb);

                    int iEmprrazsocial = dr.GetOrdinal(helper.Emprrazsocial);
                    if (!dr.IsDBNull(iEmprrazsocial)) aportante.Emprrazsocial = dr.GetString(iEmprrazsocial);

                    int iEmprruc = dr.GetOrdinal(helper.Emprruc);
                    if (!dr.IsDBNull(iEmprruc)) aportante.Emprruc = dr.GetString(iEmprruc);

                    int iTipoempresa = dr.GetOrdinal(helper.Tipoempresa);
                    if (!dr.IsDBNull(iTipoempresa)) aportante.Tipoempresa = dr.GetString(iTipoempresa);

                    int iEstadoaportante = dr.GetOrdinal(helper.Estadoaportante);
                    if (!dr.IsDBNull(iEstadoaportante)) aportante.Estadoaportante = dr.GetString(iEstadoaportante);

                    int iEstado = dr.GetOrdinal(helper.Estado);
                    if (!dr.IsDBNull(iEstado)) aportante.EstadoImportado = Convert.ToInt32(dr.GetValue(iEstado));

                    entitys.Add(aportante);
                }
            }

            return entitys;
        }

        
        public List<DaiAportanteDTO> GetByCriteriaAportanteLiquidacion(int prescodi, int tabcdcodiestado)
        {
            List<DaiAportanteDTO> entitys = new List<DaiAportanteDTO>();

            string sql = string.Format(helper.SqlGetByCriteriaAportanteLiquidacion, prescodi, tabcdcodiestado > 0 ? " and cp.tabcdcodiestado = " + tabcdcodiestado : "");

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    DaiAportanteDTO aportante = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) aportante.Emprnomb = dr.GetString(iEmprnomb);

                    int iEmprrazsocial = dr.GetOrdinal(helper.Emprrazsocial);
                    if (!dr.IsDBNull(iEmprrazsocial)) aportante.Emprrazsocial = dr.GetString(iEmprrazsocial);

                    int iEmprruc = dr.GetOrdinal(helper.Emprruc);
                    if (!dr.IsDBNull(iEmprruc)) aportante.Emprruc = dr.GetString(iEmprruc);

                    int iTipoempresa = dr.GetOrdinal(helper.Tipoempresa);
                    if (!dr.IsDBNull(iTipoempresa)) aportante.Tipoempresa = dr.GetString(iTipoempresa);

                    int iEstadoaportante = dr.GetOrdinal(helper.Estadoaportante);
                    if (!dr.IsDBNull(iEstadoaportante)) aportante.Estadoaportante = dr.GetString(iEstadoaportante);

                    int iEstado = dr.GetOrdinal(helper.Estado);
                    if (!dr.IsDBNull(iEstado)) aportante.EstadoImportado = Convert.ToInt32(dr.GetValue(iEstado));

                    entitys.Add(aportante);
                }
            }

            return entitys;
        }

        public List<DaiAportanteDTO> GetByCriteriaAportanteCronograma(int anio, string tabcdcodiestado)
        {
            List<DaiAportanteDTO> entitys = new List<DaiAportanteDTO>();

            string sql = string.Format(helper.SqlGetByCriteriaAportanteCronograma, anio, tabcdcodiestado.Length > 0 ? " and cp.tabcdcodiestado in (" + tabcdcodiestado + ")" : "");

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    DaiAportanteDTO aportante = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) aportante.Emprnomb = dr.GetString(iEmprnomb);

                    int iEmprrazsocial = dr.GetOrdinal(helper.Emprrazsocial);
                    if (!dr.IsDBNull(iEmprrazsocial)) aportante.Emprrazsocial = dr.GetString(iEmprrazsocial);

                    int iEmprruc = dr.GetOrdinal(helper.Emprruc);
                    if (!dr.IsDBNull(iEmprruc)) aportante.Emprruc = dr.GetString(iEmprruc);

                    int iTipoempresa = dr.GetOrdinal(helper.Tipoempresa);
                    if (!dr.IsDBNull(iTipoempresa)) aportante.Tipoempresa = dr.GetString(iTipoempresa);

                    int iEstadoaportante = dr.GetOrdinal(helper.Estadoaportante);
                    if (!dr.IsDBNull(iEstadoaportante)) aportante.Estadoaportante = dr.GetString(iEstadoaportante);

                    int iEstado = dr.GetOrdinal(helper.Estado);
                    if (!dr.IsDBNull(iEstado)) aportante.EstadoImportado = Convert.ToInt32(dr.GetValue(iEstado));

                    entitys.Add(aportante);
                }
            }

            return entitys;
        }

        public List<DaiAportanteDTO> GetByCriteriaAniosCronograma(int prescodi, int estado)
        {
            List<DaiAportanteDTO> entitys = new List<DaiAportanteDTO>();

            string sql = string.Format(helper.SqlGetByCriteriaAniosCronograma, prescodi, estado > 0 ? " and cp.tabcdcodiestado in (" + estado + ")" : "");

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    int anio = 0;

                    int iCaleanio = dr.GetOrdinal(helper.Caleanio);
                    if (!dr.IsDBNull(iCaleanio)) anio = Convert.ToInt32(dr.GetValue(iCaleanio));

                    entitys.Add(new DaiAportanteDTO { Anio = anio });
                }
            }

            return entitys;
        }

        public List<DaiAportanteDTO> GetByCriteriaFinalizar(DaiAportanteDTO aportante)
        {
            List<DaiAportanteDTO> entitys = new List<DaiAportanteDTO>();

            string sql = string.Format(helper.SqlGetByCriteriaFinalizar, aportante.Tabcdcodiestado, aportante.Prescodi, aportante.Emprcodi);

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    int Aporcodi = 0;

                    int iAporcodi = dr.GetOrdinal(helper.Aporcodi);
                    if (!dr.IsDBNull(iAporcodi)) Aporcodi = Convert.ToInt32(dr.GetValue(iAporcodi));

                    entitys.Add(new DaiAportanteDTO { Aporcodi = Aporcodi });
                }
            }

            return entitys;
        }
        
    }
}
