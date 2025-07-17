using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using static COES.Dominio.DTO.Sic.EqEquipoDTO;
using static COES.Dominio.DTO.Sic.EqEquipoDTO.TipoPuntoMedicion;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla EQ_FAMILIA
    /// </summary>
    public interface IEqFamiliaRepository
    {
        int Save(EqFamiliaDTO entity);
        void Update(EqFamiliaDTO entity);
        void Delete(int famcodi);
        void Delete_UpdateAuditoria(int famcodi, string user);
        EqFamiliaDTO GetById(int famcodi);
        List<EqFamiliaDTO> List();
        List<TipoSerie> ListTipoSerie();
        List<TipoPuntoMedicion> ListTipoPuntoMedicion();
        List<MePtomedicionDTO> ListPuntoMedicionPorEmpresa(int CodEmpresa, int CodTipoSerie, int CodTipoPuntoMedicion);
        List<MePtomedicionDTO> ListPuntoMedicionPorCuenca(int cuenca, int tptomedicodi);

        List<MePtomedicionDTO> ListarPuntoMedicionPorCuencaNaturalEvaporado(int cuenca);
        List<MePtomedicionDTO> ListarPuntoMedicionPorCuencaNaturalEvaporadoPorTipoPuntoMedicion(int cuenca, int tipopuntomedicion);
        List<MePtomedicionDTO> ListarPtoMedicionCuenca(int cuenca);
        List<MePtomedicionDTO> ListarPtoMedicionCuencaTipoPuntoMedicion(int cuenca, int tipopuntomedicion);
        List<TablaVertical> ListaTablaVertical(string ptomedicodi, int tptomedicodi, int tiposeriecodi, int anioinicio, int aniofin);


        List<GraficoSeries> ObtenerGraficoAnual(int tiposeriecodi, int tptomedicodi, int ptomedicodi, int anioinicio, int aniofin);
        List<GraficoSeries> ObtenerCaudalPuntosCalculados(int tiposeriecodi, int tptomedicodi, int ptomedicodi, int anioinicio, int aniofin, string tipoReporte, string anios);

        List<GraficoSeries> ObtenerGraficoMensual(int tiposeriecodi, int tptomedicodi, int ptomedicodi, int anioinicio);
        List<GraficoSeries> ObtenerGraficoComparativaVolumen(int tiposeriecodi, int tptomedicodi, int ptomedicodi, string anios);
        List<GraficoSeries> ObtenerGraficoComparativaNaturalEvaporada(int tiposeriecodi, int ptomedicodi, string anioinicio);

        List<GraficoSeries> ObtenerGraficoComparativaLineaTendencia(int tiposeriecodi, int tptomedicodi, string ptomedicodi, int anioinicio, int aniofin);
        List<GraficoSeries> ObtenerGraficoTotal(int tiposeriecodi, int tptomedicodi, int ptomedicodi, int aniofin);
        List<GraficoSeries> ObtenerGraficoTotalNaturalEvaporada(int tiposeriecodi, int ptomedicodi, int aniofin);

        List<GraficoSeries> ObtenerGraficoTotalLineaTendencia(int tiposeriecodi, string ptomedicodi, int aniofin, int tptomedicodi);
        List<GraficoSeries> ObtenerGraficoEstadisticasAnuales(int tiposeriecodi, int tptomedicodi, int ptomedicodi, int anioinicio, int mesinicio, int aniofin, int mesfin);

        double standardDesviation(List<decimal?> valores);



        MePtomedicionDTO GetPtoMedicionById(int CodPuntoMedicion);
        List<EqFamiliaDTO> GetByCriteria(string strEstado);
        List<EqFamiliaDTO> ObtenerFamiliasProcManiobras();

        #region PR5
        List<EqFamiliaDTO> ListarFamiliaXEmp(int idEmpresa);
        #endregion

        #region INTERVENCIONES
        List<EqFamiliaDTO> ListarComboTipoEquiposXUbicaciones(int IdArea);
        List<EqFamiliaDTO> ListarByTareaIds(string idTareas);
        #endregion

        #region Mejoras Ieod

        List<EqFamiliaDTO> ListarFamiliaPorOrigenLecturaEquipo(int origlectcodi, int emprcodi);

        #endregion

        #region FICHA TÉCNICA
        List<EqFamiliaDTO> ListarFamiliasFT();

        List<EqFamiliaDTO> ListFamiliaEquipamientoCOES();

        #endregion
    }
}
