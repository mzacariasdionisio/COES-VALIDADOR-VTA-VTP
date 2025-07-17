using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Infraestructura.Datos.Helper.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de las formulas
    /// </summary>
    public class EprCalculosRepository : RepositoryBase, IEprCalculosRepository
    {
        public EprCalculosRepository(string strConn) : base(strConn)
        {
        }

        EprCalculosHelper helper = new EprCalculosHelper();
                       

        public List<EprCalculosDTO> ListCalculosFormulasLinea(EprEquipoDTO equipo, int flgOrigen)
        {
            List<EprCalculosDTO> entitys = new List<EprCalculosDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.ListCalculoLinea);

            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, equipo.IdLinea);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, equipo.IdLinea);
            dbProvider.AddInParameter(command, helper.IdArea, DbType.String, flgOrigen > 0 ? (object)equipo.IdArea : null);
            dbProvider.AddInParameter(command, helper.CapacidadA, DbType.String, equipo.CapacidadA);
            dbProvider.AddInParameter(command, helper.CapacidadMva, DbType.String, equipo.CapacidadMva);
            dbProvider.AddInParameter(command, helper.IdCelda, DbType.String, flgOrigen > 0 ? (object)equipo.IdCelda : null);
            dbProvider.AddInParameter(command, helper.IdCelda2, DbType.String, flgOrigen > 0 ? (object)equipo.IdCelda2 : null);
            dbProvider.AddInParameter(command, helper.IdBancoCondensador, DbType.String, flgOrigen > 0 ? (object)equipo.IdBancoCondensador : null);
            dbProvider.AddInParameter(command, helper.CapacTransCond1Porcen, DbType.String, equipo.CapacTransCond1Porcen);
            dbProvider.AddInParameter(command, helper.CapacTransCond1Min, DbType.String, equipo.CapacTransCond1Min);

            dbProvider.AddInParameter(command, helper.CapacTransCorr1A, DbType.String, equipo.CapacTransCond1A);//*
            dbProvider.AddInParameter(command, helper.CapacTransCond2Porcen, DbType.String, equipo.CapacTransCond2Porcen);
            dbProvider.AddInParameter(command, helper.CapacTransCond2Min, DbType.String, equipo.CapacTransCond2Min);
            dbProvider.AddInParameter(command, helper.CapacTransCorr2A, DbType.String, equipo.CapacTransCond2A);//*

            dbProvider.AddInParameter(command, helper.CapacidadTransmisionA, DbType.String, equipo.CapacidadTransmisionA);
            dbProvider.AddInParameter(command, helper.CapacidadTransmisionMVA, DbType.String, equipo.CapacidadTransmisionMVA);
            dbProvider.AddInParameter(command, helper.LimiteSegCoes, DbType.String, equipo.LimiteSegCoes);
            dbProvider.AddInParameter(command, helper.FactorLimitanteCalc, DbType.String, equipo.FactorLimitanteCalc);
            dbProvider.AddInParameter(command, helper.FactorLimitanteFinal, DbType.String, equipo.FactorLimitanteFinal);

            dbProvider.AddInParameter(command, helper.Tension, DbType.String, equipo.Tension);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EprCalculosDTO entity = new EprCalculosDTO();

                    int iAreacodi = dr.GetOrdinal(helper.Propcodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Propcodi = Convert.ToInt32(dr.GetValue(iAreacodi));

                    int iAreanomb = dr.GetOrdinal(helper.Identificador);
                    if (!dr.IsDBNull(iAreanomb)) entity.Identificador = Convert.ToString(dr.GetValue(iAreanomb));

                    int iZona = dr.GetOrdinal(helper.Abreviatura);
                    if (!dr.IsDBNull(iZona)) entity.Parametro = Convert.ToString(dr.GetValue(iZona));

                    int iEpareafeccreacion = dr.GetOrdinal(helper.ValorIdentificador);
                    if (!dr.IsDBNull(iEpareafeccreacion)) entity.Valor = dr.GetValue(iEpareafeccreacion);

                    int iEpareafecmodificacion = dr.GetOrdinal(helper.Formula);
                    if (!dr.IsDBNull(iEpareafecmodificacion)) entity.Formula = Convert.ToString(dr.GetValue(iEpareafecmodificacion));

                    int iPropTipo = dr.GetOrdinal(helper.PropTipo);
                    if (!dr.IsDBNull(iPropTipo)) entity.TipoDato = Convert.ToString(dr.GetValue(iPropTipo));

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<EprCalculosDTO> ListCalculosFormulasReactor(EprEquipoDTO equipo, int flgOrigen)
        {
            List<EprCalculosDTO> entitys = new List<EprCalculosDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.ListCalculoReactor);

            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, equipo.IdReactor);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, equipo.IdReactor);          
          
            dbProvider.AddInParameter(command, helper.IdCelda, DbType.String, flgOrigen > 0 ? (object)equipo.IdCelda1 : null);
            dbProvider.AddInParameter(command, helper.IdCelda2, DbType.String, flgOrigen > 0 ? (object)equipo.IdCelda22 : null);
            dbProvider.AddInParameter(command, helper.CapacidadA, DbType.String, equipo.CapacidadA);
            dbProvider.AddInParameter(command, helper.CapacidadMvar, DbType.String, equipo.CapacidadMvar);
            dbProvider.AddInParameter(command, helper.CapacidadTransmisionA, DbType.String, equipo.CapacidadTransmisionA);
            dbProvider.AddInParameter(command, helper.CapacidadTransmisionMvar, DbType.String, equipo.CapacidadTransmisionMvar);                      
            dbProvider.AddInParameter(command, helper.FactorLimitanteCalc, DbType.String, equipo.FactorLimitanteCalc);
            dbProvider.AddInParameter(command, helper.FactorLimitanteFinal, DbType.String, equipo.FactorLimitanteFinal);

            dbProvider.AddInParameter(command, helper.NivelTension, DbType.String, equipo.NivelTension);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EprCalculosDTO entity = new EprCalculosDTO();

                    int iAreacodi = dr.GetOrdinal(helper.Propcodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Propcodi = Convert.ToInt32(dr.GetValue(iAreacodi));

                    int iAreanomb = dr.GetOrdinal(helper.Identificador);
                    if (!dr.IsDBNull(iAreanomb)) entity.Identificador = Convert.ToString(dr.GetValue(iAreanomb));

                    int iZona = dr.GetOrdinal(helper.Abreviatura);
                    if (!dr.IsDBNull(iZona)) entity.Parametro = Convert.ToString(dr.GetValue(iZona));

                    int iEpareafeccreacion = dr.GetOrdinal(helper.ValorIdentificador);
                    if (!dr.IsDBNull(iEpareafeccreacion)) entity.Valor = dr.GetValue(iEpareafeccreacion);

                    int iEpareafecmodificacion = dr.GetOrdinal(helper.Formula);
                    if (!dr.IsDBNull(iEpareafecmodificacion)) entity.Formula = Convert.ToString(dr.GetValue(iEpareafecmodificacion));

                    int iPropTipo = dr.GetOrdinal(helper.PropTipo);
                    if (!dr.IsDBNull(iPropTipo)) entity.TipoDato = Convert.ToString(dr.GetValue(iPropTipo));

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<EprCalculosDTO> ListCalculosFormulasCelda(EprEquipoDTO equipo, int flgOrigen)
        {
            List<EprCalculosDTO> entitys = new List<EprCalculosDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.ListCalculoCelda);

            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, equipo.IdCelda);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, equipo.IdCelda);
           
            dbProvider.AddInParameter(command, helper.IdInterruptor, DbType.String, flgOrigen > 0 ? (object)equipo.IdInterruptor : null);

            dbProvider.AddInParameter(command, helper.CapacidadTransmisionA, DbType.String, equipo.CapacidadTransmisionA);
            dbProvider.AddInParameter(command, helper.CapacidadTransmisionMvar, DbType.String, equipo.CapacidadTransmisionMvar);
            dbProvider.AddInParameter(command, helper.FactorLimitanteCalc, DbType.String, equipo.FactorLimitanteCalc);
            dbProvider.AddInParameter(command, helper.FactorLimitanteFinal, DbType.String, equipo.FactorLimitanteFinal);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EprCalculosDTO entity = new EprCalculosDTO();

                    int iAreacodi = dr.GetOrdinal(helper.Propcodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Propcodi = Convert.ToInt32(dr.GetValue(iAreacodi));

                    int iAreanomb = dr.GetOrdinal(helper.Identificador);
                    if (!dr.IsDBNull(iAreanomb)) entity.Identificador = Convert.ToString(dr.GetValue(iAreanomb));

                    int iZona = dr.GetOrdinal(helper.Abreviatura);
                    if (!dr.IsDBNull(iZona)) entity.Parametro = Convert.ToString(dr.GetValue(iZona));

                    int iEpareafeccreacion = dr.GetOrdinal(helper.ValorIdentificador);
                    if (!dr.IsDBNull(iEpareafeccreacion)) entity.Valor = dr.GetValue(iEpareafeccreacion);

                    int iEpareafecmodificacion = dr.GetOrdinal(helper.Formula);
                    if (!dr.IsDBNull(iEpareafecmodificacion)) entity.Formula = Convert.ToString(dr.GetValue(iEpareafecmodificacion));

                    int iPropTipo = dr.GetOrdinal(helper.PropTipo);
                    if (!dr.IsDBNull(iPropTipo)) entity.TipoDato = Convert.ToString(dr.GetValue(iPropTipo));

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<EprCalculosDTO> ListCalculosTransformadorDosDevanados(EprEquipoDTO equipo)
        {
            List<EprCalculosDTO> entitys = new List<EprCalculosDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.ListCalculoTransformadorDosDevanados);

            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, equipo.IdTransformador);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, equipo.IdTransformador);

            dbProvider.AddInParameter(command, helper.D1IdCelda, DbType.String, equipo.D1IdCelda);
            dbProvider.AddInParameter(command, helper.D2IdCelda, DbType.String, equipo.D2IdCelda);            

            dbProvider.AddInParameter(command, helper.D1CapacidadOnanMva, DbType.String, equipo.D1CapacidadOnanMva);
            dbProvider.AddInParameter(command, helper.D2CapacidadOnanMva, DbType.String, equipo.D2CapacidadOnanMva);            

            dbProvider.AddInParameter(command, helper.D1CapacidadOnafMva, DbType.String, equipo.D1CapacidadOnafMva);
            dbProvider.AddInParameter(command, helper.D2CapacidadOnafMva, DbType.String, equipo.D2CapacidadOnafMva);          

            dbProvider.AddInParameter(command, helper.D1CapacidadMva, DbType.String, equipo.D1CapacidadMva);
            dbProvider.AddInParameter(command, helper.D2CapacidadMva, DbType.String, equipo.D2CapacidadMva);         

            dbProvider.AddInParameter(command, helper.D1CapacidadA, DbType.String, equipo.D1CapacidadA);
            dbProvider.AddInParameter(command, helper.D2CapacidadA, DbType.String, equipo.D2CapacidadA);            

            dbProvider.AddInParameter(command, helper.D1CapacidadTransmisionA, DbType.String, equipo.D1CapacidadTransmisionA);
            dbProvider.AddInParameter(command, helper.D2CapacidadTransmisionA, DbType.String, equipo.D2CapacidadTransmisionA);           

            dbProvider.AddInParameter(command, helper.D1CapacidadTransmisionMva, DbType.String, equipo.D1CapacidadTransmisionMva);
            dbProvider.AddInParameter(command, helper.D2CapacidadTransmisionMva, DbType.String, equipo.D2CapacidadTransmisionMva);            

            dbProvider.AddInParameter(command, helper.D1FactorLimitanteCalc, DbType.String, equipo.D1FactorLimitanteCalc);
            dbProvider.AddInParameter(command, helper.D2FactorLimitanteCalc, DbType.String, equipo.D2FactorLimitanteCalc);            

            dbProvider.AddInParameter(command, helper.D1FactorLimitanteFinal, DbType.String, equipo.D1FactorLimitanteFinal);
            dbProvider.AddInParameter(command, helper.D2FactorLimitanteFinal, DbType.String, equipo.D2FactorLimitanteFinal);

            dbProvider.AddInParameter(command, helper.D1Tension, DbType.String, equipo.D1Tension);
            dbProvider.AddInParameter(command, helper.D2Tension, DbType.String, equipo.D2Tension);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EprCalculosDTO entity = new EprCalculosDTO();

                    int iAreacodi = dr.GetOrdinal(helper.Propcodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Propcodi = Convert.ToInt32(dr.GetValue(iAreacodi));

                    int iAreanomb = dr.GetOrdinal(helper.Identificador);
                    if (!dr.IsDBNull(iAreanomb)) entity.Identificador = Convert.ToString(dr.GetValue(iAreanomb));

                    int iZona = dr.GetOrdinal(helper.Abreviatura);
                    if (!dr.IsDBNull(iZona)) entity.Parametro = Convert.ToString(dr.GetValue(iZona));

                    int iEpareafeccreacion = dr.GetOrdinal(helper.ValorIdentificador);
                    if (!dr.IsDBNull(iEpareafeccreacion)) entity.Valor = dr.GetValue(iEpareafeccreacion);

                    int iEpareafecmodificacion = dr.GetOrdinal(helper.Formula);
                    if (!dr.IsDBNull(iEpareafecmodificacion)) entity.Formula = Convert.ToString(dr.GetValue(iEpareafecmodificacion));

                    int iPropTipo = dr.GetOrdinal(helper.PropTipo);
                    if (!dr.IsDBNull(iPropTipo)) entity.TipoDato = Convert.ToString(dr.GetValue(iPropTipo));

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<EprCalculosDTO> ListCalculosTransformadorTresDevanados(EprEquipoDTO equipo)
        {
            List<EprCalculosDTO> entitys = new List<EprCalculosDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.ListCalculoTransformadorTresDevanados);

            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, equipo.IdTransformador);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, equipo.IdTransformador);

            dbProvider.AddInParameter(command, helper.D1IdCelda, DbType.String, equipo.D1IdCelda);
            dbProvider.AddInParameter(command, helper.D2IdCelda, DbType.String, equipo.D2IdCelda);
            dbProvider.AddInParameter(command, helper.D3IdCelda, DbType.String, equipo.D3IdCelda);

            dbProvider.AddInParameter(command, helper.D1CapacidadOnanMva, DbType.String, equipo.D1CapacidadOnanMva);
            dbProvider.AddInParameter(command, helper.D2CapacidadOnanMva, DbType.String, equipo.D2CapacidadOnanMva);
            dbProvider.AddInParameter(command, helper.D3CapacidadOnanMva, DbType.String, equipo.D3CapacidadOnanMva);

            dbProvider.AddInParameter(command, helper.D1CapacidadOnafMva, DbType.String, equipo.D1CapacidadOnafMva);
            dbProvider.AddInParameter(command, helper.D2CapacidadOnafMva, DbType.String, equipo.D2CapacidadOnafMva);
            dbProvider.AddInParameter(command, helper.D3CapacidadOnafMva, DbType.String, equipo.D3CapacidadOnafMva);

            dbProvider.AddInParameter(command, helper.D1CapacidadMva, DbType.String, equipo.D1CapacidadMva);
            dbProvider.AddInParameter(command, helper.D2CapacidadMva, DbType.String, equipo.D2CapacidadMva);
            dbProvider.AddInParameter(command, helper.D3CapacidadMva, DbType.String, equipo.D3CapacidadMva);

            dbProvider.AddInParameter(command, helper.D1CapacidadA, DbType.String, equipo.D1CapacidadA);
            dbProvider.AddInParameter(command, helper.D2CapacidadA, DbType.String, equipo.D2CapacidadA);
            dbProvider.AddInParameter(command, helper.D3CapacidadA, DbType.String, equipo.D3CapacidadA);

            dbProvider.AddInParameter(command, helper.D1CapacidadTransmisionA, DbType.String, equipo.D1CapacidadTransmisionA);
            dbProvider.AddInParameter(command, helper.D2CapacidadTransmisionA, DbType.String, equipo.D2CapacidadTransmisionA);
            dbProvider.AddInParameter(command, helper.D3CapacidadTransmisionA, DbType.String, equipo.D3CapacidadTransmisionA);

            dbProvider.AddInParameter(command, helper.D1CapacidadTransmisionMva, DbType.String, equipo.D1CapacidadTransmisionMva);
            dbProvider.AddInParameter(command, helper.D2CapacidadTransmisionMva, DbType.String, equipo.D2CapacidadTransmisionMva);
            dbProvider.AddInParameter(command, helper.D3CapacidadTransmisionMva, DbType.String, equipo.D3CapacidadTransmisionMva);

            dbProvider.AddInParameter(command, helper.D1FactorLimitanteCalc, DbType.String, equipo.D1FactorLimitanteCalc);
            dbProvider.AddInParameter(command, helper.D2FactorLimitanteCalc, DbType.String, equipo.D2FactorLimitanteCalc);
            dbProvider.AddInParameter(command, helper.D3FactorLimitanteCalc, DbType.String, equipo.D3FactorLimitanteCalc);

            dbProvider.AddInParameter(command, helper.D1FactorLimitanteFinal, DbType.String, equipo.D1FactorLimitanteFinal);           
            dbProvider.AddInParameter(command, helper.D2FactorLimitanteFinal, DbType.String, equipo.D2FactorLimitanteFinal);                 
            dbProvider.AddInParameter(command, helper.D3FactorLimitanteFinal, DbType.String, equipo.D3FactorLimitanteFinal);

            dbProvider.AddInParameter(command, helper.D1Tension, DbType.String, equipo.D1Tension);
            dbProvider.AddInParameter(command, helper.D2Tension, DbType.String, equipo.D2Tension);
            dbProvider.AddInParameter(command, helper.D3Tension, DbType.String, equipo.D3Tension);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EprCalculosDTO entity = new EprCalculosDTO();

                    int iAreacodi = dr.GetOrdinal(helper.Propcodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Propcodi = Convert.ToInt32(dr.GetValue(iAreacodi));

                    int iAreanomb = dr.GetOrdinal(helper.Identificador);
                    if (!dr.IsDBNull(iAreanomb)) entity.Identificador = Convert.ToString(dr.GetValue(iAreanomb));

                    int iZona = dr.GetOrdinal(helper.Abreviatura);
                    if (!dr.IsDBNull(iZona)) entity.Parametro = Convert.ToString(dr.GetValue(iZona));

                    int iEpareafeccreacion = dr.GetOrdinal(helper.ValorIdentificador);
                    if (!dr.IsDBNull(iEpareafeccreacion)) entity.Valor = dr.GetValue(iEpareafeccreacion);

                    int iEpareafecmodificacion = dr.GetOrdinal(helper.Formula);
                    if (!dr.IsDBNull(iEpareafecmodificacion)) entity.Formula = Convert.ToString(dr.GetValue(iEpareafecmodificacion));

                    int iPropTipo = dr.GetOrdinal(helper.PropTipo);
                    if (!dr.IsDBNull(iPropTipo)) entity.TipoDato = Convert.ToString(dr.GetValue(iPropTipo));

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<EprCalculosDTO> ListCalculosTransformadorCuatroDevanados(EprEquipoDTO equipo)
        {
            List<EprCalculosDTO> entitys = new List<EprCalculosDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.ListCalculoTransformadorCuatroDevanados);

            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, equipo.IdTransformador);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, equipo.IdTransformador);

            dbProvider.AddInParameter(command, helper.D1IdCelda, DbType.String, equipo.D1IdCelda);
            dbProvider.AddInParameter(command, helper.D2IdCelda, DbType.String, equipo.D2IdCelda);
            dbProvider.AddInParameter(command, helper.D3IdCelda, DbType.String, equipo.D3IdCelda);
            dbProvider.AddInParameter(command, helper.D4IdCelda, DbType.String, equipo.D4IdCelda);

            dbProvider.AddInParameter(command, helper.D1CapacidadOnanMva, DbType.String, equipo.D1CapacidadOnanMva);
            dbProvider.AddInParameter(command, helper.D2CapacidadOnanMva, DbType.String, equipo.D2CapacidadOnanMva);
            dbProvider.AddInParameter(command, helper.D3CapacidadOnanMva, DbType.String, equipo.D3CapacidadOnanMva);
            dbProvider.AddInParameter(command, helper.D4CapacidadOnanMva, DbType.String, equipo.D4CapacidadOnanMva);

            dbProvider.AddInParameter(command, helper.D1CapacidadOnafMva, DbType.String, equipo.D1CapacidadOnafMva);
            dbProvider.AddInParameter(command, helper.D2CapacidadOnafMva, DbType.String, equipo.D2CapacidadOnafMva);
            dbProvider.AddInParameter(command, helper.D3CapacidadOnafMva, DbType.String, equipo.D3CapacidadOnafMva);
            dbProvider.AddInParameter(command, helper.D4CapacidadOnafMva, DbType.String, equipo.D4CapacidadOnafMva);

            dbProvider.AddInParameter(command, helper.D1CapacidadMva, DbType.String, equipo.D1CapacidadMva);
            dbProvider.AddInParameter(command, helper.D2CapacidadMva, DbType.String, equipo.D2CapacidadMva);
            dbProvider.AddInParameter(command, helper.D3CapacidadMva, DbType.String, equipo.D3CapacidadMva);
            dbProvider.AddInParameter(command, helper.D4CapacidadMva, DbType.String, equipo.D4CapacidadMva);

            dbProvider.AddInParameter(command, helper.D1CapacidadA, DbType.String, equipo.D1CapacidadA);
            dbProvider.AddInParameter(command, helper.D2CapacidadA, DbType.String, equipo.D2CapacidadA);
            dbProvider.AddInParameter(command, helper.D3CapacidadA, DbType.String, equipo.D3CapacidadA);
            dbProvider.AddInParameter(command, helper.D4CapacidadA, DbType.String, equipo.D4CapacidadA);

            dbProvider.AddInParameter(command, helper.D1CapacidadTransmisionA, DbType.String, equipo.D1CapacidadTransmisionA);
            dbProvider.AddInParameter(command, helper.D2CapacidadTransmisionA, DbType.String, equipo.D2CapacidadTransmisionA);
            dbProvider.AddInParameter(command, helper.D3CapacidadTransmisionA, DbType.String, equipo.D3CapacidadTransmisionA);
            dbProvider.AddInParameter(command, helper.D4CapacidadTransmisionA, DbType.String, equipo.D4CapacidadTransmisionA);

            dbProvider.AddInParameter(command, helper.D1CapacidadTransmisionMva, DbType.String, equipo.D1CapacidadTransmisionMva);
            dbProvider.AddInParameter(command, helper.D2CapacidadTransmisionMva, DbType.String, equipo.D2CapacidadTransmisionMva);
            dbProvider.AddInParameter(command, helper.D3CapacidadTransmisionMva, DbType.String, equipo.D3CapacidadTransmisionMva);
            dbProvider.AddInParameter(command, helper.D4CapacidadTransmisionMva, DbType.String, equipo.D4CapacidadTransmisionMva);

            dbProvider.AddInParameter(command, helper.D1FactorLimitanteCalc, DbType.String, equipo.D1FactorLimitanteCalc);
            dbProvider.AddInParameter(command, helper.D2FactorLimitanteCalc, DbType.String, equipo.D2FactorLimitanteCalc);
            dbProvider.AddInParameter(command, helper.D3FactorLimitanteCalc, DbType.String, equipo.D3FactorLimitanteCalc);
            dbProvider.AddInParameter(command, helper.D4FactorLimitanteCalc, DbType.String, equipo.D4FactorLimitanteCalc);

            dbProvider.AddInParameter(command, helper.D1FactorLimitanteFinal, DbType.String, equipo.D1FactorLimitanteFinal);
            dbProvider.AddInParameter(command, helper.D2FactorLimitanteFinal, DbType.String, equipo.D2FactorLimitanteFinal);
            dbProvider.AddInParameter(command, helper.D3FactorLimitanteFinal, DbType.String, equipo.D3FactorLimitanteFinal);
            dbProvider.AddInParameter(command, helper.D4FactorLimitanteFinal, DbType.String, equipo.D4FactorLimitanteFinal);

            dbProvider.AddInParameter(command, helper.D1Tension, DbType.String, equipo.D1Tension);
            dbProvider.AddInParameter(command, helper.D2Tension, DbType.String, equipo.D2Tension);
            dbProvider.AddInParameter(command, helper.D3Tension, DbType.String, equipo.D3Tension);
            dbProvider.AddInParameter(command, helper.D4Tension, DbType.String, equipo.D4Tension);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EprCalculosDTO entity = new EprCalculosDTO();

                    int iAreacodi = dr.GetOrdinal(helper.Propcodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Propcodi = Convert.ToInt32(dr.GetValue(iAreacodi));

                    int iAreanomb = dr.GetOrdinal(helper.Identificador);
                    if (!dr.IsDBNull(iAreanomb)) entity.Identificador = Convert.ToString(dr.GetValue(iAreanomb));

                    int iZona = dr.GetOrdinal(helper.Abreviatura);
                    if (!dr.IsDBNull(iZona)) entity.Parametro = Convert.ToString(dr.GetValue(iZona));

                    int iEpareafeccreacion = dr.GetOrdinal(helper.ValorIdentificador);
                    if (!dr.IsDBNull(iEpareafeccreacion)) entity.Valor = dr.GetValue(iEpareafeccreacion);

                    int iEpareafecmodificacion = dr.GetOrdinal(helper.Formula);
                    if (!dr.IsDBNull(iEpareafecmodificacion)) entity.Formula = Convert.ToString(dr.GetValue(iEpareafecmodificacion));

                    int iPropTipo = dr.GetOrdinal(helper.PropTipo);
                    if (!dr.IsDBNull(iPropTipo)) entity.TipoDato = Convert.ToString(dr.GetValue(iPropTipo));

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public double? EvaluarCeldaPosicionNucleo(int equicodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.EvaluarCeldaPosicionNucleo);

            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, equicodi);

            object result = dbProvider.ExecuteScalar(command);
            double? res = null;

            if (result != null && result != System.DBNull.Value) res = Convert.ToDouble(result);           
          
            return res;
        }

        public double? EvaluarCeldaPickUp(int equicodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.EvaluarCeldaPickUp);

            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, equicodi);

            object result = dbProvider.ExecuteScalar(command);
            double? res = null;

            if (result != null && result != System.DBNull.Value) res = Convert.ToDouble(result);

            return res;
        }

        public double? EvaluarPropiedadEquipo(int equicodi, string tipoPropiedad)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.EvaluarPropiedadEquipo);

            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, equicodi);
            dbProvider.AddInParameter(command, helper.TipoPropiedad, DbType.String, tipoPropiedad);

            object result = dbProvider.ExecuteScalar(command);
            double? res = null;

            if (result != null && result != System.DBNull.Value) res = Convert.ToDouble(result);

            return res;
        }

        public double? EvaluarTensionEquipo(int equicodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.EvaluarTensionEquipo);

            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, equicodi);

            object result = dbProvider.ExecuteScalar(command);
            double? res = null;

            if (result != null && result != System.DBNull.Value) res = Convert.ToDouble(result);

            return res;
        }

        public List<EprPropCatalogoDataDTO> ListFunciones()
        {
            List<EprPropCatalogoDataDTO> entitys = new List<EprPropCatalogoDataDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.ListFunciones);                    

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EprPropCatalogoDataDTO entity = new EprPropCatalogoDataDTO();                   

                    int iEqcatdabrev = dr.GetOrdinal("eqcatdabrev");
                    if (!dr.IsDBNull(iEqcatdabrev)) entity.Eqcatdabrev = Convert.ToString(dr.GetValue(iEqcatdabrev));

                    int iEqcatddescripcion = dr.GetOrdinal("eqcatddescripcion");
                    if (!dr.IsDBNull(iEqcatddescripcion)) entity.Eqcatddescripcion = Convert.ToString(dr.GetValue(iEqcatddescripcion));                   

                    int iEqcatdnota = dr.GetOrdinal("eqcatdnota");
                    if (!dr.IsDBNull(iEqcatdnota)) entity.Eqcatdnota = Convert.ToString(dr.GetValue(iEqcatdnota));

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<EqPropiedadDTO> ListPropiedades(int famcodi)
        {
            List<EqPropiedadDTO> entitys = new List<EqPropiedadDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.ListPropiedades);
            dbProvider.AddInParameter(command, helper.Famcodi, DbType.Int32, famcodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqPropiedadDTO entity = new EqPropiedadDTO();

                    int iPropabrev = dr.GetOrdinal("propabrev");
                    if (!dr.IsDBNull(iPropabrev)) entity.Propabrev = Convert.ToString(dr.GetValue(iPropabrev));

                    int iPropnomb = dr.GetOrdinal("propnomb");
                    if (!dr.IsDBNull(iPropnomb)) entity.Propnomb = Convert.ToString(dr.GetValue(iPropnomb));

                    int iProptipo = dr.GetOrdinal("proptipo");
                    if (!dr.IsDBNull(iProptipo)) entity.Proptipo = Convert.ToString(dr.GetValue(iProptipo));

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<EprCalculosDTO> ListValidarFormulas(int famcodi, string formula)
        {
            List<EprCalculosDTO> entitys = new List<EprCalculosDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.ListValidarFormula);

            dbProvider.AddInParameter(command, helper.Famcodi, DbType.Int32, famcodi);
            dbProvider.AddInParameter(command, "formula", DbType.String, formula);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EprCalculosDTO entity = new EprCalculosDTO();

                    int iAreacodi = dr.GetOrdinal(helper.Propcodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Propcodi = Convert.ToInt32(dr.GetValue(iAreacodi));

                    int iAreanomb = dr.GetOrdinal(helper.Identificador);
                    if (!dr.IsDBNull(iAreanomb)) entity.Identificador = Convert.ToString(dr.GetValue(iAreanomb));

                    int iZona = dr.GetOrdinal(helper.Abreviatura);
                    if (!dr.IsDBNull(iZona)) entity.Parametro = Convert.ToString(dr.GetValue(iZona));

                    int iEpareafeccreacion = dr.GetOrdinal(helper.ValorIdentificador);
                    if (!dr.IsDBNull(iEpareafeccreacion)) entity.Valor = dr.GetValue(iEpareafeccreacion);

                    int iEpareafecmodificacion = dr.GetOrdinal(helper.Formula);
                    if (!dr.IsDBNull(iEpareafecmodificacion)) entity.Formula = Convert.ToString(dr.GetValue(iEpareafecmodificacion));

                    int iPropTipo = dr.GetOrdinal(helper.PropTipo);
                    if (!dr.IsDBNull(iPropTipo)) entity.TipoDato = Convert.ToString(dr.GetValue(iPropTipo));

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<EprCalculosDTO> ListCalculosFormulasLineaMasivo(string listaCodigosEquipo, int famcodi)
        {
            List<EprCalculosDTO> entitys = new List<EprCalculosDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.ListCalculosLineaMasivo);

            dbProvider.AddInParameter(command, "lst_equicodi", DbType.String, listaCodigosEquipo);
            dbProvider.AddInParameter(command, helper.Famcodi, DbType.Int32, famcodi);         


            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EprCalculosDTO entity = new EprCalculosDTO();

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iAreacodi = dr.GetOrdinal(helper.Propcodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Propcodi = Convert.ToInt32(dr.GetValue(iAreacodi));

                    int iAreanomb = dr.GetOrdinal(helper.Identificador);
                    if (!dr.IsDBNull(iAreanomb)) entity.Identificador = Convert.ToString(dr.GetValue(iAreanomb));

                    int iZona = dr.GetOrdinal(helper.Abreviatura);
                    if (!dr.IsDBNull(iZona)) entity.Parametro = Convert.ToString(dr.GetValue(iZona));

                    int iEpareafeccreacion = dr.GetOrdinal(helper.ValorIdentificador);
                    if (!dr.IsDBNull(iEpareafeccreacion)) entity.Valor = dr.GetValue(iEpareafeccreacion);

                    int iEpareafecmodificacion = dr.GetOrdinal(helper.Formula);
                    if (!dr.IsDBNull(iEpareafecmodificacion)) entity.Formula = Convert.ToString(dr.GetValue(iEpareafecmodificacion));

                    int iPropTipo = dr.GetOrdinal(helper.PropTipo);
                    if (!dr.IsDBNull(iPropTipo)) entity.TipoDato = Convert.ToString(dr.GetValue(iPropTipo));

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<EprCalculosDTO> ListCalculosFormulasReactorMasivo(string listaCodigosEquipo, int famcodi)
        {
            List<EprCalculosDTO> entitys = new List<EprCalculosDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.ListCalculosReactorMasivo);

            dbProvider.AddInParameter(command, "lst_equicodi", DbType.String, listaCodigosEquipo);
            dbProvider.AddInParameter(command, helper.Famcodi, DbType.Int32, famcodi);


            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EprCalculosDTO entity = new EprCalculosDTO();

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iAreacodi = dr.GetOrdinal(helper.Propcodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Propcodi = Convert.ToInt32(dr.GetValue(iAreacodi));

                    int iAreanomb = dr.GetOrdinal(helper.Identificador);
                    if (!dr.IsDBNull(iAreanomb)) entity.Identificador = Convert.ToString(dr.GetValue(iAreanomb));

                    int iZona = dr.GetOrdinal(helper.Abreviatura);
                    if (!dr.IsDBNull(iZona)) entity.Parametro = Convert.ToString(dr.GetValue(iZona));

                    int iEpareafeccreacion = dr.GetOrdinal(helper.ValorIdentificador);
                    if (!dr.IsDBNull(iEpareafeccreacion)) entity.Valor = dr.GetValue(iEpareafeccreacion);

                    int iEpareafecmodificacion = dr.GetOrdinal(helper.Formula);
                    if (!dr.IsDBNull(iEpareafecmodificacion)) entity.Formula = Convert.ToString(dr.GetValue(iEpareafecmodificacion));

                    int iPropTipo = dr.GetOrdinal(helper.PropTipo);
                    if (!dr.IsDBNull(iPropTipo)) entity.TipoDato = Convert.ToString(dr.GetValue(iPropTipo));

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<EprCalculosDTO> ListCalculosFormulasCeldaMasivo(string listaCodigosEquipo, int famcodi)
        {
            List<EprCalculosDTO> entitys = new List<EprCalculosDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.ListCalculosCeldaMasivo);

            dbProvider.AddInParameter(command, "lst_equicodi", DbType.String, listaCodigosEquipo);
            dbProvider.AddInParameter(command, helper.Famcodi, DbType.Int32, famcodi);


            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EprCalculosDTO entity = new EprCalculosDTO();

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iAreacodi = dr.GetOrdinal(helper.Propcodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Propcodi = Convert.ToInt32(dr.GetValue(iAreacodi));

                    int iAreanomb = dr.GetOrdinal(helper.Identificador);
                    if (!dr.IsDBNull(iAreanomb)) entity.Identificador = Convert.ToString(dr.GetValue(iAreanomb));

                    int iZona = dr.GetOrdinal(helper.Abreviatura);
                    if (!dr.IsDBNull(iZona)) entity.Parametro = Convert.ToString(dr.GetValue(iZona));

                    int iEpareafeccreacion = dr.GetOrdinal(helper.ValorIdentificador);
                    if (!dr.IsDBNull(iEpareafeccreacion)) entity.Valor = dr.GetValue(iEpareafeccreacion);

                    int iEpareafecmodificacion = dr.GetOrdinal(helper.Formula);
                    if (!dr.IsDBNull(iEpareafecmodificacion)) entity.Formula = Convert.ToString(dr.GetValue(iEpareafecmodificacion));

                    int iPropTipo = dr.GetOrdinal(helper.PropTipo);
                    if (!dr.IsDBNull(iPropTipo)) entity.TipoDato = Convert.ToString(dr.GetValue(iPropTipo));

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<EprCalculosDTO> ListCalculosFormulasTransformadoDosDevanadosMasivo(string listaCodigosEquipo, int famcodi)
        {
            List<EprCalculosDTO> entitys = new List<EprCalculosDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.ListCalculosTransformadorDosDevanadosMasivo);

            dbProvider.AddInParameter(command, "lst_equicodi", DbType.String, listaCodigosEquipo);
            dbProvider.AddInParameter(command, helper.Famcodi, DbType.Int32, famcodi);


            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EprCalculosDTO entity = new EprCalculosDTO();

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iAreacodi = dr.GetOrdinal(helper.Propcodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Propcodi = Convert.ToInt32(dr.GetValue(iAreacodi));

                    int iAreanomb = dr.GetOrdinal(helper.Identificador);
                    if (!dr.IsDBNull(iAreanomb)) entity.Identificador = Convert.ToString(dr.GetValue(iAreanomb));

                    int iZona = dr.GetOrdinal(helper.Abreviatura);
                    if (!dr.IsDBNull(iZona)) entity.Parametro = Convert.ToString(dr.GetValue(iZona));

                    int iEpareafeccreacion = dr.GetOrdinal(helper.ValorIdentificador);
                    if (!dr.IsDBNull(iEpareafeccreacion)) entity.Valor = dr.GetValue(iEpareafeccreacion);

                    int iEpareafecmodificacion = dr.GetOrdinal(helper.Formula);
                    if (!dr.IsDBNull(iEpareafecmodificacion)) entity.Formula = Convert.ToString(dr.GetValue(iEpareafecmodificacion));

                    int iPropTipo = dr.GetOrdinal(helper.PropTipo);
                    if (!dr.IsDBNull(iPropTipo)) entity.TipoDato = Convert.ToString(dr.GetValue(iPropTipo));

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<EprCalculosDTO> ListCalculosFormulasTransformadoTresDevanadosMasivo(string listaCodigosEquipo, int famcodi)
        {
            List<EprCalculosDTO> entitys = new List<EprCalculosDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.ListCalculosTransformadorTresDevanadosMasivo);

            dbProvider.AddInParameter(command, "lst_equicodi", DbType.String, listaCodigosEquipo);
            dbProvider.AddInParameter(command, helper.Famcodi, DbType.Int32, famcodi);


            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EprCalculosDTO entity = new EprCalculosDTO();

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iAreacodi = dr.GetOrdinal(helper.Propcodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Propcodi = Convert.ToInt32(dr.GetValue(iAreacodi));

                    int iAreanomb = dr.GetOrdinal(helper.Identificador);
                    if (!dr.IsDBNull(iAreanomb)) entity.Identificador = Convert.ToString(dr.GetValue(iAreanomb));

                    int iZona = dr.GetOrdinal(helper.Abreviatura);
                    if (!dr.IsDBNull(iZona)) entity.Parametro = Convert.ToString(dr.GetValue(iZona));

                    int iEpareafeccreacion = dr.GetOrdinal(helper.ValorIdentificador);
                    if (!dr.IsDBNull(iEpareafeccreacion)) entity.Valor = dr.GetValue(iEpareafeccreacion);

                    int iEpareafecmodificacion = dr.GetOrdinal(helper.Formula);
                    if (!dr.IsDBNull(iEpareafecmodificacion)) entity.Formula = Convert.ToString(dr.GetValue(iEpareafecmodificacion));

                    int iPropTipo = dr.GetOrdinal(helper.PropTipo);
                    if (!dr.IsDBNull(iPropTipo)) entity.TipoDato = Convert.ToString(dr.GetValue(iPropTipo));

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<EprCalculosDTO> ListCalculosFormulasTransformadoCuatroDevanadosMasivo(string listaCodigosEquipo, int famcodi)
        {
            List<EprCalculosDTO> entitys = new List<EprCalculosDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.ListCalculosTransformadorCuatroDevanadosMasivo);

            dbProvider.AddInParameter(command, "lst_equicodi", DbType.String, listaCodigosEquipo);
            dbProvider.AddInParameter(command, helper.Famcodi, DbType.Int32, famcodi);


            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EprCalculosDTO entity = new EprCalculosDTO();

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iAreacodi = dr.GetOrdinal(helper.Propcodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Propcodi = Convert.ToInt32(dr.GetValue(iAreacodi));

                    int iAreanomb = dr.GetOrdinal(helper.Identificador);
                    if (!dr.IsDBNull(iAreanomb)) entity.Identificador = Convert.ToString(dr.GetValue(iAreanomb));

                    int iZona = dr.GetOrdinal(helper.Abreviatura);
                    if (!dr.IsDBNull(iZona)) entity.Parametro = Convert.ToString(dr.GetValue(iZona));

                    int iEpareafeccreacion = dr.GetOrdinal(helper.ValorIdentificador);
                    if (!dr.IsDBNull(iEpareafeccreacion)) entity.Valor = dr.GetValue(iEpareafeccreacion);

                    int iEpareafecmodificacion = dr.GetOrdinal(helper.Formula);
                    if (!dr.IsDBNull(iEpareafecmodificacion)) entity.Formula = Convert.ToString(dr.GetValue(iEpareafecmodificacion));

                    int iPropTipo = dr.GetOrdinal(helper.PropTipo);
                    if (!dr.IsDBNull(iPropTipo)) entity.TipoDato = Convert.ToString(dr.GetValue(iPropTipo));

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<EprCalculosDTO> ListCalculosFormulasInterruptor(EprEquipoDTO equipo)
        {
            List<EprCalculosDTO> entitys = new List<EprCalculosDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.ListCalculoInterruptor);

            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, equipo.IdInterruptor);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, equipo.IdInterruptor);
            

            dbProvider.AddInParameter(command, helper.CapacidadA, DbType.String, equipo.CapacidadA);
            dbProvider.AddInParameter(command, helper.CapacidadMva, DbType.String, equipo.CapacidadMvar);
         

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EprCalculosDTO entity = new EprCalculosDTO();

                    int iAreacodi = dr.GetOrdinal(helper.Propcodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Propcodi = Convert.ToInt32(dr.GetValue(iAreacodi));

                    int iAreanomb = dr.GetOrdinal(helper.Identificador);
                    if (!dr.IsDBNull(iAreanomb)) entity.Identificador = Convert.ToString(dr.GetValue(iAreanomb));

                    int iZona = dr.GetOrdinal(helper.Abreviatura);
                    if (!dr.IsDBNull(iZona)) entity.Parametro = Convert.ToString(dr.GetValue(iZona));

                    int iEpareafeccreacion = dr.GetOrdinal(helper.ValorIdentificador);
                    if (!dr.IsDBNull(iEpareafeccreacion)) entity.Valor = dr.GetValue(iEpareafeccreacion);

                    int iEpareafecmodificacion = dr.GetOrdinal(helper.Formula);
                    if (!dr.IsDBNull(iEpareafecmodificacion)) entity.Formula = Convert.ToString(dr.GetValue(iEpareafecmodificacion));

                    int iPropTipo = dr.GetOrdinal(helper.PropTipo);
                    if (!dr.IsDBNull(iPropTipo)) entity.TipoDato = Convert.ToString(dr.GetValue(iPropTipo));

                    entitys.Add(entity);
                }
            }
            return entitys;
        }
    }
}
