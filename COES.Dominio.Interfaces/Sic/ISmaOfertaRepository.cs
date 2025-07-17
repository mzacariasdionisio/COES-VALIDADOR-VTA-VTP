using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SMA_OFERTA
    /// </summary>
    public interface ISmaOfertaRepository
    {
        IDbConnection BeginConnection();

        DbTransaction StartTransaction(IDbConnection conn);

        int Save(SmaOfertaDTO entity, string urscodisEnvio, IDbConnection conn, DbTransaction tran);//
        void Update(SmaOfertaDTO entity);//
        SmaOfertaDTO GetById(int ofercodi);//

        List<SmaOfertaDTO> List(int ofertipo, DateTime oferfechaenvio, int usercode, int ofercodi, string oferestado, string oferfuente);
        List<SmaOfertaDTO> ListInterna(int ofertipo, DateTime oferfechaInicio, DateTime oferfechaFin, int usercode, string ofercodi, string oferestado, int emprcodi, string urscodi, string oferfuente);

        List<SmaOfertaDTO> ListOfertasxDia(int ofertipo, DateTime oferfechaInicio, DateTime oferfechaFin, int usercode, int emprcodi, string urscodi, string oferfuente);
        List<SmaOfertaSimetricaHorarioDTO> ListSmaOfertaSimetricaHorario();
        void CrearSmaOfertaSimetricaHorario(string horarioInicio, string horarioFin);
        void RevertirEstadoSmaOfertaSimetricaHorario(string id, int estado);
        bool ExisteVigenteSmaOfertaSimetricaHorario();

        void ResetearOfertaDefecto(DateTime fechaIniMes, DateTime fechaFinMes);
    }
}