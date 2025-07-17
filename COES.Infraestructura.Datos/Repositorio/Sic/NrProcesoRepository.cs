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
    /// Clase de acceso a datos de la tabla NR_PROCESO
    /// </summary>
    public class NrProcesoRepository: RepositoryBase, INrProcesoRepository
    {
        public NrProcesoRepository(string strConn): base(strConn)
        {
        }

        NrProcesoHelper helper = new NrProcesoHelper();

        public int Save(NrProcesoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Nrprccodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Nrpercodi, DbType.Int32, entity.Nrpercodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Nrcptcodi, DbType.Int32, entity.Nrcptcodi);
            dbProvider.AddInParameter(command, helper.Nrprcfechainicio, DbType.DateTime, entity.Nrprcfechainicio);
            dbProvider.AddInParameter(command, helper.Nrprcfechafin, DbType.DateTime, entity.Nrprcfechafin);
            dbProvider.AddInParameter(command, helper.Nrprchoraunidad, DbType.Decimal, entity.Nrprchoraunidad);
            dbProvider.AddInParameter(command, helper.Nrprchoracentral, DbType.Decimal, entity.Nrprchoracentral);
            dbProvider.AddInParameter(command, helper.Nrprcpotencialimite, DbType.Decimal, entity.Nrprcpotencialimite);
            dbProvider.AddInParameter(command, helper.Nrprcpotenciarestringida, DbType.Decimal, entity.Nrprcpotenciarestringida);
            dbProvider.AddInParameter(command, helper.Nrprcpotenciaadjudicada, DbType.Decimal, entity.Nrprcpotenciaadjudicada);
            dbProvider.AddInParameter(command, helper.Nrprcpotenciaefectiva, DbType.Decimal, entity.Nrprcpotenciaefectiva);
            dbProvider.AddInParameter(command, helper.Nrprcpotenciaprommedidor, DbType.Decimal, entity.Nrprcpotenciaprommedidor);
            dbProvider.AddInParameter(command, helper.Nrprcprctjrestringefect, DbType.Decimal, entity.Nrprcprctjrestringefect);
            dbProvider.AddInParameter(command, helper.Nrprcvolumencombustible, DbType.Decimal, entity.Nrprcvolumencombustible);
            dbProvider.AddInParameter(command, helper.Nrprcrendimientounidad, DbType.Decimal, entity.Nrprcrendimientounidad);
            dbProvider.AddInParameter(command, helper.Nrprcede, DbType.Decimal, entity.Nrprcede);
            dbProvider.AddInParameter(command, helper.Nrprcpadre, DbType.Int32, entity.Nrprcpadre);
            dbProvider.AddInParameter(command, helper.Nrprcexceptuacoes, DbType.String, entity.Nrprcexceptuacoes);
            dbProvider.AddInParameter(command, helper.Nrprcexceptuaosinergmin, DbType.String, entity.Nrprcexceptuaosinergmin);
            dbProvider.AddInParameter(command, helper.Nrprctipoingreso, DbType.String, entity.Nrprctipoingreso);
            dbProvider.AddInParameter(command, helper.Nrprchorafalla, DbType.String, entity.Nrprchorafalla);
            dbProvider.AddInParameter(command, helper.Nrprcsobrecosto, DbType.Decimal, entity.Nrprcsobrecosto);
            dbProvider.AddInParameter(command, helper.Nrprcobservacion, DbType.String, entity.Nrprcobservacion);
            dbProvider.AddInParameter(command, helper.Nrprcnota, DbType.String, entity.Nrprcnota);
            dbProvider.AddInParameter(command, helper.Nrprcnotaautomatica, DbType.String, entity.Nrprcnotaautomatica);
            dbProvider.AddInParameter(command, helper.Nrprcfiltrado, DbType.String, entity.Nrprcfiltrado);
            dbProvider.AddInParameter(command, helper.Nrprcrpf, DbType.Decimal, entity.Nrprcrpf);
            dbProvider.AddInParameter(command, helper.Nrprctolerancia, DbType.Decimal, entity.Nrprctolerancia);
            dbProvider.AddInParameter(command, helper.Nrprcusucreacion, DbType.String, entity.Nrprcusucreacion);
            dbProvider.AddInParameter(command, helper.Nrprcfeccreacion, DbType.DateTime, entity.Nrprcfeccreacion);
            dbProvider.AddInParameter(command, helper.Nrprcusumodificacion, DbType.String, entity.Nrprcusumodificacion);
            dbProvider.AddInParameter(command, helper.Nrprcfecmodificacion, DbType.DateTime, entity.Nrprcfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(NrProcesoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Nrpercodi, DbType.Int32, entity.Nrpercodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Nrcptcodi, DbType.Int32, entity.Nrcptcodi);
            dbProvider.AddInParameter(command, helper.Nrprcfechainicio, DbType.DateTime, entity.Nrprcfechainicio);
            dbProvider.AddInParameter(command, helper.Nrprcfechafin, DbType.DateTime, entity.Nrprcfechafin);
            dbProvider.AddInParameter(command, helper.Nrprchoraunidad, DbType.Decimal, entity.Nrprchoraunidad);
            dbProvider.AddInParameter(command, helper.Nrprchoracentral, DbType.Decimal, entity.Nrprchoracentral);
            dbProvider.AddInParameter(command, helper.Nrprcpotencialimite, DbType.Decimal, entity.Nrprcpotencialimite);
            dbProvider.AddInParameter(command, helper.Nrprcpotenciarestringida, DbType.Decimal, entity.Nrprcpotenciarestringida);
            dbProvider.AddInParameter(command, helper.Nrprcpotenciaadjudicada, DbType.Decimal, entity.Nrprcpotenciaadjudicada);
            dbProvider.AddInParameter(command, helper.Nrprcpotenciaefectiva, DbType.Decimal, entity.Nrprcpotenciaefectiva);
            dbProvider.AddInParameter(command, helper.Nrprcpotenciaprommedidor, DbType.Decimal, entity.Nrprcpotenciaprommedidor);
            dbProvider.AddInParameter(command, helper.Nrprcprctjrestringefect, DbType.Decimal, entity.Nrprcprctjrestringefect);
            dbProvider.AddInParameter(command, helper.Nrprcvolumencombustible, DbType.Decimal, entity.Nrprcvolumencombustible);
            dbProvider.AddInParameter(command, helper.Nrprcrendimientounidad, DbType.Decimal, entity.Nrprcrendimientounidad);
            dbProvider.AddInParameter(command, helper.Nrprcede, DbType.Decimal, entity.Nrprcede);
            dbProvider.AddInParameter(command, helper.Nrprcpadre, DbType.Int32, entity.Nrprcpadre);
            dbProvider.AddInParameter(command, helper.Nrprcexceptuacoes, DbType.String, entity.Nrprcexceptuacoes);
            dbProvider.AddInParameter(command, helper.Nrprcexceptuaosinergmin, DbType.String, entity.Nrprcexceptuaosinergmin);
            dbProvider.AddInParameter(command, helper.Nrprctipoingreso, DbType.String, entity.Nrprctipoingreso);
            dbProvider.AddInParameter(command, helper.Nrprchorafalla, DbType.String, entity.Nrprchorafalla);
            dbProvider.AddInParameter(command, helper.Nrprcsobrecosto, DbType.Decimal, entity.Nrprcsobrecosto);
            dbProvider.AddInParameter(command, helper.Nrprcobservacion, DbType.String, entity.Nrprcobservacion);
            dbProvider.AddInParameter(command, helper.Nrprcnota, DbType.String, entity.Nrprcnota);
            dbProvider.AddInParameter(command, helper.Nrprcnotaautomatica, DbType.String, entity.Nrprcnotaautomatica);
            dbProvider.AddInParameter(command, helper.Nrprcfiltrado, DbType.String, entity.Nrprcfiltrado);
            dbProvider.AddInParameter(command, helper.Nrprcrpf, DbType.Decimal, entity.Nrprcrpf);
            dbProvider.AddInParameter(command, helper.Nrprctolerancia, DbType.Decimal, entity.Nrprctolerancia);
            dbProvider.AddInParameter(command, helper.Nrprcusucreacion, DbType.String, entity.Nrprcusucreacion);
            dbProvider.AddInParameter(command, helper.Nrprcfeccreacion, DbType.DateTime, entity.Nrprcfeccreacion);
            dbProvider.AddInParameter(command, helper.Nrprcusumodificacion, DbType.String, entity.Nrprcusumodificacion);
            dbProvider.AddInParameter(command, helper.Nrprcfecmodificacion, DbType.DateTime, entity.Nrprcfecmodificacion);
            dbProvider.AddInParameter(command, helper.Nrprccodi, DbType.Int32, entity.Nrprccodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int nrprccodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Nrprccodi, DbType.Int32, nrprccodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int nrpercodi, int nrcptcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeletePeriodoConcepto);

            dbProvider.AddInParameter(command, helper.Nrpercodi, DbType.Int32, nrpercodi);
            dbProvider.AddInParameter(command, helper.Nrcptcodi, DbType.Int32, nrcptcodi);

            dbProvider.ExecuteNonQuery(command);
        }


        public NrProcesoDTO GetById(int nrprccodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Nrprccodi, DbType.Int32, nrprccodi);
            NrProcesoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }



        public List<NrProcesoDTO> List()
        {
            List<NrProcesoDTO> entitys = new List<NrProcesoDTO>();
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

        public string ListObservaciones(int nrperCodi)
        {
            string resultado = "";

            NrProcesoDTO entity = new NrProcesoDTO();

            String sql = String.Format(this.helper.SqlListObservacion, nrperCodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    int iNrprcnotaautomatica = dr.GetOrdinal(this.helper.Nrprcnotaautomatica);
                    if (!dr.IsDBNull(iNrprcnotaautomatica)) entity.Nrprcnotaautomatica = dr.GetString(iNrprcnotaautomatica);

                    resultado += entity.Nrprcnotaautomatica + "\r\n";

                }
            }

            return resultado;
        }

        public List<NrProcesoDTO> GetByCriteria()
        {
            List<NrProcesoDTO> entitys = new List<NrProcesoDTO>();
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
        /// Graba los datos de la tabla NR_PROCESO
        /// </summary>
        public int SaveNrProcesoId(NrProcesoDTO entity)
        {
            try
            {
                int id = 0;

                if (entity.Nrprccodi==0)
                    id = Save(entity);
                else
                { 
                    Update(entity);
                    id = entity.Nrprccodi;
                }

                return id;

            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public List<NrProcesoDTO> BuscarOperaciones(string estado, int nrperCodi, int grupoCodi, int nrcptCodi, DateTime nrprcFechaInicio, DateTime nrprcFechaFin, int nroPage, int pageSize)
        {
            List<NrProcesoDTO> entitys = new List<NrProcesoDTO>();
            String sql = String.Format(this.helper.ObtenerListado, estado, nrperCodi,grupoCodi,nrcptCodi,nrprcFechaInicio.ToString(ConstantesBase.FormatoFecha),nrprcFechaFin.ToString(ConstantesBase.FormatoFecha), nroPage, pageSize);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    NrProcesoDTO entity = new NrProcesoDTO();

                    int iNrprccodi = dr.GetOrdinal(this.helper.Nrprccodi);
                    if (!dr.IsDBNull(iNrprccodi)) entity.Nrprccodi = Convert.ToInt32(dr.GetValue(iNrprccodi));

                    int iNrpercodi = dr.GetOrdinal(this.helper.Nrpercodi);
                    if (!dr.IsDBNull(iNrpercodi)) entity.Nrpercodi = Convert.ToInt32(dr.GetValue(iNrpercodi));

                    int iGrupocodi = dr.GetOrdinal(this.helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iNrcptcodi = dr.GetOrdinal(this.helper.Nrcptcodi);
                    if (!dr.IsDBNull(iNrcptcodi)) entity.Nrcptcodi = Convert.ToInt32(dr.GetValue(iNrcptcodi));

                    int iNrprcfechainicio = dr.GetOrdinal(this.helper.Nrprcfechainicio);
                    if (!dr.IsDBNull(iNrprcfechainicio)) entity.Nrprcfechainicio = dr.GetDateTime(iNrprcfechainicio);

                    int iNrprcfechafin = dr.GetOrdinal(this.helper.Nrprcfechafin);
                    if (!dr.IsDBNull(iNrprcfechafin)) entity.Nrprcfechafin = dr.GetDateTime(iNrprcfechafin);

                    int iNrprchoraunidad = dr.GetOrdinal(this.helper.Nrprchoraunidad);
                    if (!dr.IsDBNull(iNrprchoraunidad)) entity.Nrprchoraunidad = dr.GetDecimal(iNrprchoraunidad);

                    int iNrprchoracentral = dr.GetOrdinal(this.helper.Nrprchoracentral);
                    if (!dr.IsDBNull(iNrprchoracentral)) entity.Nrprchoracentral = dr.GetDecimal(iNrprchoracentral);

                    int iNrprcpotencialimite = dr.GetOrdinal(this.helper.Nrprcpotencialimite);
                    if (!dr.IsDBNull(iNrprcpotencialimite)) entity.Nrprcpotencialimite = dr.GetDecimal(iNrprcpotencialimite);

                    int iNrprcpotenciarestringida = dr.GetOrdinal(this.helper.Nrprcpotenciarestringida);
                    if (!dr.IsDBNull(iNrprcpotenciarestringida)) entity.Nrprcpotenciarestringida = dr.GetDecimal(iNrprcpotenciarestringida);

                    int iNrprcpotenciaadjudicada = dr.GetOrdinal(this.helper.Nrprcpotenciaadjudicada);
                    if (!dr.IsDBNull(iNrprcpotenciaadjudicada)) entity.Nrprcpotenciaadjudicada = dr.GetDecimal(iNrprcpotenciaadjudicada);

                    int iNrprcpotenciaefectiva = dr.GetOrdinal(this.helper.Nrprcpotenciaefectiva);
                    if (!dr.IsDBNull(iNrprcpotenciaefectiva)) entity.Nrprcpotenciaefectiva = dr.GetDecimal(iNrprcpotenciaefectiva);

                    int iNrprcpotenciaprommedidor = dr.GetOrdinal(this.helper.Nrprcpotenciaprommedidor);
                    if (!dr.IsDBNull(iNrprcpotenciaprommedidor)) entity.Nrprcpotenciaprommedidor = dr.GetDecimal(iNrprcpotenciaprommedidor);

                    int iNrprcprctjrestringefect = dr.GetOrdinal(this.helper.Nrprcprctjrestringefect);
                    if (!dr.IsDBNull(iNrprcprctjrestringefect)) entity.Nrprcprctjrestringefect = dr.GetDecimal(iNrprcprctjrestringefect);

                    int iNrprcvolumencombustible = dr.GetOrdinal(this.helper.Nrprcvolumencombustible);
                    if (!dr.IsDBNull(iNrprcvolumencombustible)) entity.Nrprcvolumencombustible = dr.GetDecimal(iNrprcvolumencombustible);

                    int iNrprcrendimientounidad = dr.GetOrdinal(this.helper.Nrprcrendimientounidad);
                    if (!dr.IsDBNull(iNrprcrendimientounidad)) entity.Nrprcrendimientounidad = dr.GetDecimal(iNrprcrendimientounidad);

                    int iNrprcede = dr.GetOrdinal(this.helper.Nrprcede);
                    if (!dr.IsDBNull(iNrprcede)) entity.Nrprcede = dr.GetDecimal(iNrprcede);

                    int iNrprcpadre = dr.GetOrdinal(this.helper.Nrprcpadre);
                    if (!dr.IsDBNull(iNrprcpadre)) entity.Nrprcpadre = Convert.ToInt32(dr.GetValue(iNrprcpadre));

                    int iNrprcexceptuacoes = dr.GetOrdinal(this.helper.Nrprcexceptuacoes);
                    if (!dr.IsDBNull(iNrprcexceptuacoes)) entity.Nrprcexceptuacoes = dr.GetString(iNrprcexceptuacoes);

                    int iNrprcexceptuaosinergmin = dr.GetOrdinal(this.helper.Nrprcexceptuaosinergmin);
                    if (!dr.IsDBNull(iNrprcexceptuaosinergmin)) entity.Nrprcexceptuaosinergmin = dr.GetString(iNrprcexceptuaosinergmin);

                    int iNrprctipoingreso = dr.GetOrdinal(this.helper.Nrprctipoingreso);
                    if (!dr.IsDBNull(iNrprctipoingreso)) entity.Nrprctipoingreso = dr.GetString(iNrprctipoingreso);

                    int iNrprchorafalla = dr.GetOrdinal(this.helper.Nrprchorafalla);
                    if (!dr.IsDBNull(iNrprchorafalla)) entity.Nrprchorafalla = dr.GetString(iNrprchorafalla);

                    int iNrprcsobrecosto = dr.GetOrdinal(this.helper.Nrprcsobrecosto);
                    if (!dr.IsDBNull(iNrprcsobrecosto)) entity.Nrprcsobrecosto = dr.GetDecimal(iNrprcsobrecosto);

                    int iNrprcobservacion = dr.GetOrdinal(this.helper.Nrprcobservacion);
                    if (!dr.IsDBNull(iNrprcobservacion)) entity.Nrprcobservacion = dr.GetString(iNrprcobservacion);

                    int iNrprcnota = dr.GetOrdinal(this.helper.Nrprcnota);
                    if (!dr.IsDBNull(iNrprcnota)) entity.Nrprcnota = dr.GetString(iNrprcnota);

                    int iNrprcnotaautomatica = dr.GetOrdinal(this.helper.Nrprcnotaautomatica);
                    if (!dr.IsDBNull(iNrprcnotaautomatica)) entity.Nrprcnotaautomatica = dr.GetString(iNrprcnotaautomatica);

                    int iNrprcfiltrado = dr.GetOrdinal(this.helper.Nrprcfiltrado);
                    if (!dr.IsDBNull(iNrprcfiltrado)) entity.Nrprcfiltrado = dr.GetString(iNrprcfiltrado);

                    int iNrprcrpf = dr.GetOrdinal(this.helper.Nrprcrpf);
                    if (!dr.IsDBNull(iNrprcrpf)) entity.Nrprcrpf = dr.GetDecimal(iNrprcrpf);

                    int iNrprctolerancia = dr.GetOrdinal(this.helper.Nrprctolerancia);
                    if (!dr.IsDBNull(iNrprctolerancia)) entity.Nrprctolerancia = dr.GetDecimal(iNrprctolerancia);

                    int iNrprcusucreacion = dr.GetOrdinal(this.helper.Nrprcusucreacion);
                    if (!dr.IsDBNull(iNrprcusucreacion)) entity.Nrprcusucreacion = dr.GetString(iNrprcusucreacion);

                    int iNrprcfeccreacion = dr.GetOrdinal(this.helper.Nrprcfeccreacion);
                    if (!dr.IsDBNull(iNrprcfeccreacion)) entity.Nrprcfeccreacion = dr.GetDateTime(iNrprcfeccreacion);

                    int iNrprcusumodificacion = dr.GetOrdinal(this.helper.Nrprcusumodificacion);
                    if (!dr.IsDBNull(iNrprcusumodificacion)) entity.Nrprcusumodificacion = dr.GetString(iNrprcusumodificacion);

                    int iNrprcfecmodificacion = dr.GetOrdinal(this.helper.Nrprcfecmodificacion);
                    if (!dr.IsDBNull(iNrprcfecmodificacion)) entity.Nrprcfecmodificacion = dr.GetDateTime(iNrprcfecmodificacion);

                    int iNrpermes = dr.GetOrdinal(this.helper.Nrpermes);
                    if (!dr.IsDBNull(iNrpermes)) entity.Nrpermes = dr.GetDateTime(iNrpermes).ToString("yyyy-MM");//dr.GetString(iNrpermes);
                    
                    int iGruponomb = dr.GetOrdinal(this.helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    int iNrcptabrev = dr.GetOrdinal(this.helper.Nrcptabrev);
                    if (!dr.IsDBNull(iNrcptabrev)) entity.Nrcptabrev = dr.GetString(iNrcptabrev);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int ObtenerNroFilas(string estado, int nrperCodi,int grupoCodi,int nrcptCodi,DateTime nrprcFechaInicio,DateTime nrprcFechaFin)
        {
            String sql = String.Format(this.helper.TotalRegistros, estado, nrperCodi,grupoCodi,nrcptCodi,nrprcFechaInicio.ToString(ConstantesBase.FormatoFecha),nrprcFechaFin.ToString(ConstantesBase.FormatoFecha));

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            object count = dbProvider.ExecuteScalar(command);

            if (count != null) return Convert.ToInt32(count);
            return 0;
        }

        public List<ReservaDTO> ObtenerReservaDiariaEjecutada(DateTime dtFecha)
        {
            String query = string.Format(helper.SqlReservaDiariaRSF, dtFecha.ToString("yyyy-MM-dd"));
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            var lista = new List<ReservaDTO>();
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = new ReservaDTO();
                    entity.URS = Convert.ToString(dr.GetValue(dr.GetOrdinal("URS")));
                    entity.Central = Convert.ToString(dr.GetValue(dr.GetOrdinal("AREANOMB")));
                    entity.Empresa = Convert.ToString(dr.GetValue(dr.GetOrdinal("EMPRNOMB")));
                    entity.Equipo = Convert.ToString(dr.GetValue(dr.GetOrdinal("EQUINOMB")));
                    entity.FechaHoraFin = Convert.ToDateTime(dr.GetValue(dr.GetOrdinal("RSFHORFIN")));
                    entity.FechaHoraInicio = Convert.ToDateTime(dr.GetValue(dr.GetOrdinal("RSFHORINICIO")));
                    entity.Hora = Convert.ToString(dr.GetValue(dr.GetOrdinal("HORA")));
                    entity.Tipo = Convert.ToString(dr.GetValue(dr.GetOrdinal("CAUSA")));
                    entity.TipoEquipo = Convert.ToString(dr.GetValue(dr.GetOrdinal("FAMABREV")));
                    entity.ValorReserva = Convert.ToDecimal(dr.GetValue(dr.GetOrdinal("rsfdetvalaut")));
                    lista.Add(entity);
                }
            }
            return lista;
        }
    }
}
