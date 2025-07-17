using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Scada;
using COES.Dominio.Interfaces.Scada;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Scada;
using System.Globalization;
using System.Drawing;
using log4net;

namespace COES.Infraestructura.Datos.Respositorio.Scada
{
    /// <summary>
    /// Clase de acceso a datos de la tabla TR_CIRCULAR_SP7
    /// </summary>
    public class TrCircularSp7RepositorySp7 : RepositoryBase, ITrCircularSp7RepositorySp7

        
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ITrCircularSp7RepositorySp7));
        private static string NombreRepository = "ITrCircularSp7RepositorySp7";
        public TrCircularSp7RepositorySp7(string strConn) : base(strConn)
        {
        }

        TrCircularSp7Helper helper = new TrCircularSp7Helper();

        public void Save(TrCircularSp7DTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Canalcodi, DbType.Int32, entity.Canalcodi);
            dbProvider.AddInParameter(command, helper.Canalfhsist, DbType.DateTime, entity.Canalfhsist);
            dbProvider.AddInParameter(command, helper.Canalvalor, DbType.Decimal, entity.Canalvalor);
            dbProvider.AddInParameter(command, helper.Canalcalidad, DbType.Int32, entity.Canalcalidad);
            dbProvider.AddInParameter(command, helper.Canalfechahora, DbType.DateTime, entity.Canalfechahora);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(TrCircularSp7DTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Canalvalor, DbType.Decimal, entity.Canalvalor);
            dbProvider.AddInParameter(command, helper.Canalcalidad, DbType.Int32, entity.Canalcalidad);
            dbProvider.AddInParameter(command, helper.Canalfechahora, DbType.DateTime, entity.Canalfechahora);
            dbProvider.AddInParameter(command, helper.Canalcodi, DbType.Int32, entity.Canalcodi);
            dbProvider.AddInParameter(command, helper.Canalfhsist, DbType.DateTime, entity.Canalfhsist);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int canalcodi, DateTime canalfhsist)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Canalcodi, DbType.Int32, canalcodi);
            dbProvider.AddInParameter(command, helper.Canalfhsist, DbType.DateTime, canalfhsist);

            dbProvider.ExecuteNonQuery(command);
        }

        public TrCircularSp7DTO GetById(int canalcodi, DateTime canalfhsist)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Canalcodi, DbType.Int32, canalcodi);
            dbProvider.AddInParameter(command, helper.Canalfhsist, DbType.DateTime, canalfhsist);
            TrCircularSp7DTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<TrCircularSp7DTO> List()
        {
            List<TrCircularSp7DTO> entitys = new List<TrCircularSp7DTO>();
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

        public List<TrCircularSp7DTO> GetByCriteria()
        {
            List<TrCircularSp7DTO> entitys = new List<TrCircularSp7DTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        /// <summary>
        /// Graba los datos de la tabla TR_CIRCULAR_SP7
        /// </summary>
        public int SaveTrCircularSp7Id(TrCircularSp7DTO entity)
        {
            try
            {
                int id = 0;

                if (entity.Canalfhsist == null)
                    Save(entity);
                else
                {
                    Update(entity);
                }

                return id;

            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public List<TrCircularSp7DTO> BuscarOperaciones(string canalcodis, DateTime canalFhsistInicio, DateTime canalFhsistFin, int nroPage, int pageSize)
        {
            string fechaTabla = canalFhsistInicio.ToString(ConstantesBase.FormatoFecha).Replace("-", "");

            List<TrCircularSp7DTO> entitys = new List<TrCircularSp7DTO>();
            String sql = String.Format(this.helper.ObtenerListado, fechaTabla, canalcodis, canalFhsistInicio.ToString(ConstantesBase.FormatoFecha), canalFhsistFin.ToString(ConstantesBase.FormatoFecha), nroPage, pageSize);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    TrCircularSp7DTO entity = new TrCircularSp7DTO();

                    int iCanalcodi = dr.GetOrdinal(this.helper.Canalcodi);
                    if (!dr.IsDBNull(iCanalcodi)) entity.Canalcodi = Convert.ToInt32(dr.GetValue(iCanalcodi));

                    int iCanalfhsist = dr.GetOrdinal(this.helper.Canalfhsist);
                    if (!dr.IsDBNull(iCanalfhsist)) entity.Canalfhsist = dr.GetDateTime(iCanalfhsist);

                    int iCanalvalor = dr.GetOrdinal(this.helper.Canalvalor);
                    if (!dr.IsDBNull(iCanalvalor)) entity.Canalvalor = dr.GetDecimal(iCanalvalor);

                    int iCanalcalidad = dr.GetOrdinal(this.helper.Canalcalidad);
                    if (!dr.IsDBNull(iCanalcalidad)) entity.Canalcalidad = Convert.ToInt32(dr.GetValue(iCanalcalidad));

                    int iCanalfechahora = dr.GetOrdinal(this.helper.Canalfechahora);
                    if (!dr.IsDBNull(iCanalfechahora)) entity.Canalfechahora = dr.GetDateTime(iCanalfechahora);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<TrCircularSp7DTO> BuscarOperacionesRango(string canalcodis, DateTime canalFhsistInicio, DateTime canalFhsistFin, int nroPage, int pageSize)
        {
            //string fechaTabla = canalFhsistInicio.ToString(ConstantesBase.FormatoFecha).Replace("-", "");
            string fechaTabla = "";
            //canalFhsistFin = canalFhsistFin.AddDays(4);
            DateTime fechaConsulta;
            int numDias = (canalFhsistFin - canalFhsistInicio).Days + 1;
            List<TrCircularSp7DTO> entitys = new List<TrCircularSp7DTO>();
            try
            {
                for (int countDia = 0; countDia < numDias; countDia++)
                {
                    fechaConsulta = canalFhsistInicio.AddDays(countDia);
                    fechaTabla = fechaConsulta.ToString(ConstantesBase.FormatoFecha).Replace("-", "");
                    String sql = String.Format(this.helper.ObtenerListadoRango, fechaTabla, canalcodis, canalFhsistInicio.ToString(ConstantesBase.FormatoFechaExtendido), canalFhsistFin.ToString(ConstantesBase.FormatoFechaExtendido), nroPage, pageSize);
                    DbCommand command = dbProvider.GetSqlStringCommand(sql);
                    using (IDataReader dr = dbProvider.ExecuteReader(command))
                    {
                        while (dr.Read())
                        {
                            TrCircularSp7DTO entity = new TrCircularSp7DTO();

                            int iCanalcodi = dr.GetOrdinal(this.helper.Canalcodi);
                            if (!dr.IsDBNull(iCanalcodi)) entity.Canalcodi = Convert.ToInt32(dr.GetValue(iCanalcodi));

                            int iCanalfhsist = dr.GetOrdinal(this.helper.Canalfhsist);
                            if (!dr.IsDBNull(iCanalfhsist)) entity.Canalfhsist = dr.GetDateTime(iCanalfhsist);

                            int iCanalvalor = dr.GetOrdinal(this.helper.Canalvalor);
                            if (!dr.IsDBNull(iCanalvalor)) entity.Canalvalor = dr.GetDecimal(iCanalvalor);

                            int iCanalcalidad = dr.GetOrdinal(this.helper.Canalcalidad);
                            if (!dr.IsDBNull(iCanalcalidad)) entity.Canalcalidad = Convert.ToInt32(dr.GetValue(iCanalcalidad));

                            int iCanalfechahora = dr.GetOrdinal(this.helper.Canalfechahora);
                            if (!dr.IsDBNull(iCanalfechahora)) entity.Canalfechahora = dr.GetDateTime(iCanalfechahora);

                            try
                            {
                                entity.SetCanalcalidadDescripcion(dr.GetString(dr.GetOrdinal("CALIDADDESCRIPCION")));
                            }catch (Exception e) { }

                            entitys.Add(entity);
                        }
                    }
                }
                return entitys;

            }
            catch(Exception ex)
            {
                Logger.Error(NombreRepository + " - GrillaExcel", ex);
                return new List<TrCircularSp7DTO>();
            }   
        }


        public List<TrCircularSp7DTO> BuscarOperaciones(int canalcodi, DateTime canalFhsistInicio, DateTime canalFhsistFin, int nroPage, int pageSize, string analisis, int calidadNotRenew, int calidadHisNotCollected)
        {
            string fechaTabla = canalFhsistInicio.ToString(ConstantesBase.FormatoFecha).Replace("-", "");

            try
            {
                List<TrCircularSp7DTO> entitys = new List<TrCircularSp7DTO>();
                String sql = String.Format(this.helper.ObtenerListadoClasif, fechaTabla, canalcodi, canalFhsistInicio.ToString(ConstantesBase.FormatoFecha), canalFhsistFin.ToString(ConstantesBase.FormatoFecha), nroPage, pageSize, analisis, calidadNotRenew, calidadHisNotCollected);
                DbCommand command = dbProvider.GetSqlStringCommand(sql);
                using (IDataReader dr = dbProvider.ExecuteReader(command))
                {
                    while (dr.Read())
                    {
                        TrCircularSp7DTO entity = new TrCircularSp7DTO();

                        int iCanalcodi = dr.GetOrdinal(this.helper.Canalcodi);
                        if (!dr.IsDBNull(iCanalcodi)) entity.Canalcodi = Convert.ToInt32(dr.GetValue(iCanalcodi));

                        int iCanalfhsist = dr.GetOrdinal(this.helper.Canalfhsist);
                        if (!dr.IsDBNull(iCanalfhsist)) entity.Canalfhsist = dr.GetDateTime(iCanalfhsist);

                        int iCanalvalor = dr.GetOrdinal(this.helper.Canalvalor);
                        if (!dr.IsDBNull(iCanalvalor)) entity.Canalvalor = dr.GetDecimal(iCanalvalor);

                        int iCanalcalidad = dr.GetOrdinal(this.helper.Canalcalidad);
                        if (!dr.IsDBNull(iCanalcalidad)) entity.Canalcalidad = Convert.ToInt32(dr.GetValue(iCanalcalidad));

                        int iCanalfechahora = dr.GetOrdinal(this.helper.Canalfechahora);
                        if (!dr.IsDBNull(iCanalfechahora)) entity.Canalfechahora = dr.GetDateTime(iCanalfechahora);

                        int iCanalnomb = dr.GetOrdinal(this.helper.Canalnomb);
                        if (!dr.IsDBNull(iCanalnomb)) entity.Canalnomb = dr.GetString(iCanalnomb);

                        int iCalnomb = dr.GetOrdinal(this.helper.Calnomb);
                        if (!dr.IsDBNull(iCalnomb)) entity.Calnomb = dr.GetString(iCalnomb);


                        entitys.Add(entity);
                    }
                }

                return entitys;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public int ObtenerNroFilas(string canalcodis, DateTime canalFhsistInicio, DateTime canalFhsistFin, string analisis, int calidadNotRenew, int calidadHisNotCollected)
        {
            string fechaTabla = canalFhsistInicio.ToString(ConstantesBase.FormatoFecha).Replace("-", "");

            String sql = String.Format(this.helper.TotalRegistros, fechaTabla, canalcodis, canalFhsistInicio.ToString(ConstantesBase.FormatoFecha), canalFhsistFin.ToString(ConstantesBase.FormatoFecha), analisis, calidadNotRenew, calidadHisNotCollected);

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            object count = dbProvider.ExecuteScalar(command);

            if (count != null) return Convert.ToInt32(count);
            return 0;
        }



        public void CrearTabla(int i, string fecha)
        {

            String sql = "";

            switch (i)
            {
                case 0:
                    sql = String.Format(this.helper.CrearTablaCampo0, fecha);
                    break;
                case 1:
                    sql = String.Format(this.helper.CrearTablaCampo1, fecha);
                    break;
                case 2:
                    sql = String.Format(this.helper.CrearTablaCampo2, fecha);
                    break;
                case 3:
                    sql = String.Format(this.helper.CrearTablaCampo3, fecha);
                    break;
                case 4:
                    sql = String.Format(this.helper.CrearTablaCampo4, fecha);
                    break;
                case 5:
                    sql = String.Format(this.helper.CrearTablaCampo5, fecha);
                    break;
                case 6:
                    sql = String.Format(this.helper.CrearTablaCampo6, fecha);
                    break;
                case 7:
                    sql = String.Format(this.helper.CrearTablaCampo7, fecha);
                    break;
            }

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            dbProvider.ExecuteScalar(command);
        }

        public List<TrCircularSp7DTO> ObtenerConsultaFlujos(string table, string canales)
        {
            List<TrCircularSp7DTO> entitys = new List<TrCircularSp7DTO>();
            String sql = String.Format(this.helper.SqlObtenerConsultaFlujos, table, canales);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    TrCircularSp7DTO entity = new TrCircularSp7DTO();

                    int iCanalcodi = dr.GetOrdinal(this.helper.Canalcodi);
                    if (!dr.IsDBNull(iCanalcodi)) entity.Canalcodi = Convert.ToInt32(dr.GetValue(iCanalcodi));

                    int iCanalfhsist = dr.GetOrdinal(this.helper.Canalfhsist);
                    if (!dr.IsDBNull(iCanalfhsist)) entity.Canalfhsist = dr.GetDateTime(iCanalfhsist);

                    int iCanalvalor = dr.GetOrdinal(this.helper.Canalvalor);
                    if (!dr.IsDBNull(iCanalvalor)) entity.Canalvalor = dr.GetDecimal(iCanalvalor);

                    int iCanalcalidad = dr.GetOrdinal(this.helper.Canalcalidad);
                    if (!dr.IsDBNull(iCanalcalidad)) entity.Canalcalidad = Convert.ToInt32(dr.GetValue(iCanalcalidad));

                    int iCanalfechahora = dr.GetOrdinal(this.helper.Canalfechahora);
                    if (!dr.IsDBNull(iCanalfechahora)) entity.Canalfechahora = dr.GetDateTime(iCanalfechahora);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<TrCircularSp7DTO> ObtenerCircularesPorFecha(DateTime fecha)
        {
            string fechaTabla = fecha.ToString(ConstantesBase.FormatoFecha).Replace("-", "");
            List<TrCircularSp7DTO> entitys = new List<TrCircularSp7DTO>();
            String sql = String.Format(this.helper.SqlObtenerCircularesPorFecha, fechaTabla, fecha.ToString(ConstantesBase.FormatoFechaExtendido));
            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    TrCircularSp7DTO entity = new TrCircularSp7DTO();

                    int iCanalcodi = dr.GetOrdinal(this.helper.Canalcodi);
                    if (!dr.IsDBNull(iCanalcodi)) entity.Canalcodi = Convert.ToInt32(dr.GetValue(iCanalcodi));

                    int iCanalfhsist = dr.GetOrdinal(this.helper.Canalfhsist);
                    if (!dr.IsDBNull(iCanalfhsist)) entity.Canalfhsist = dr.GetDateTime(iCanalfhsist);

                    int iCanalvalor = dr.GetOrdinal(this.helper.Canalvalor);
                    if (!dr.IsDBNull(iCanalvalor)) entity.Canalvalor = dr.GetDecimal(iCanalvalor);

                    int iCanalcalidad = dr.GetOrdinal(this.helper.Canalcalidad);
                    if (!dr.IsDBNull(iCanalcalidad)) entity.Canalcalidad = Convert.ToInt32(dr.GetValue(iCanalcalidad));

                    int iCanalfechahora = dr.GetOrdinal(this.helper.Canalfechahora);
                    if (!dr.IsDBNull(iCanalfechahora)) entity.Canalfechahora = dr.GetDateTime(iCanalfechahora);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<int> ObtenerCodigosDeCanalesParaMuestraInstantanea()
        {
            List<int> lista = new List<int>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerCanalesParaMuestraInstantanea);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    lista.Add(Convert.ToInt32(dr.GetValue(0)));
                }
            }
            
            return lista;
        }

        public List<TrCircularSp7DTO> ObtenerCircularesPorIntervalosDeFechaMuestraInstantanea(int canalcodigo, DateTime fechaDesde, DateTime fechaHasta)
        {
            string fechaTabla = fechaHasta.ToString(ConstantesBase.FormatoFecha).Replace("-", "");
            List<TrCircularSp7DTO> entitys = new List<TrCircularSp7DTO>();
            String sql = String.Format(helper.SqlObtenerCircularesPorIntervalosDeFechaMuestraInstantanea, fechaTabla, canalcodigo, fechaDesde.ToString(ConstantesBase.FormatoFechaExtendido), fechaHasta.ToString(ConstantesBase.FormatoFechaExtendido));
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public TrCanalSp7DTO GetCanalById(int canalcodi)
        {
            TrCanalSp7Helper trCanalSp7Helper = new TrCanalSp7Helper();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetCanalById);

            dbProvider.AddInParameter(command, helper.Canalcodi, DbType.Int32, canalcodi);
            TrCanalSp7DTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = trCanalSp7Helper.CreateFromTrcoes(dr);
                }
            }

            return entity;
        }

        public void EliminarRegistroMuestraInstantanea(int canalcodi)
        {
            String sql = String.Format(helper.SqlEliminarRegistroMuestraInstantanea, canalcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            dbProvider.ExecuteNonQuery(command);
        }

        public void GenerarRegistroMuestraInstantanea(TrCanalSp7DTO canal, TrCircularSp7DTO circular, string usuario)
        {
            String estado = "-";
            if (canal.CanalPointType == "D")
            {
                if (circular.Canalvalor == 0)
                {
                    estado = "A";
                }
                else
                {
                    estado = "C";
                }
            }

            String sql = String.Format(helper.SqlGenerarRegistroMuestraInstantanea,
                canal.Canalcodi, circular.Canalcalidad, circular.Canalfechahora.ToString(ConstantesBase.FormatoFechaExtendidoConMilisegundo), circular.Canalfhsist.ToString(ConstantesBase.FormatoFechaExtendidoConMilisegundo),
                canal.Canalnomb, canal.Canaliccp, canal.Emprcodi, circular.Canalvalor, estado, usuario, usuario);

            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            dbProvider.ExecuteNonQuery(command);
        }

        public List<TrCalidadSp7DTO> GetCalidades()
        {
            List<TrCalidadSp7DTO> entitys = new List<TrCalidadSp7DTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.GetCalidadesSp7);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    TrCalidadSp7DTO trCalidadSp7DTO = new TrCalidadSp7DTO();
                    trCalidadSp7DTO.CalCodi = Convert.ToInt32(dr.GetString(0));
                    trCalidadSp7DTO.CalNomb = dr.GetString(1);
                    entitys.Add(trCalidadSp7DTO);
                }
            }

            return entitys;
        }
    }
}
