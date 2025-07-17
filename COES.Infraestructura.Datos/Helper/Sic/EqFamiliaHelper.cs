using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using static COES.Dominio.DTO.Sic.EqEquipoDTO;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla EQ_FAMILIA
    /// </summary>
    public class EqFamiliaHelper : HelperBase
    {
        public EqFamiliaHelper(): base(Consultas.EqFamiliaSql)
        {
        }

        public EqFamiliaDTO Create(IDataReader dr)
        {
            EqFamiliaDTO entity = new EqFamiliaDTO();

            int iFamcodi = dr.GetOrdinal(this.Famcodi);
            if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

            int iFamabrev = dr.GetOrdinal(this.Famabrev);
            if (!dr.IsDBNull(iFamabrev)) entity.Famabrev = dr.GetString(iFamabrev);

            int iTipoecodi = dr.GetOrdinal(this.Tipoecodi);
            if (!dr.IsDBNull(iTipoecodi)) entity.Tipoecodi = Convert.ToInt32(dr.GetValue(iTipoecodi));

            int iTareacodi = dr.GetOrdinal(this.Tareacodi);
            if (!dr.IsDBNull(iTareacodi)) entity.Tareacodi = Convert.ToInt32(dr.GetValue(iTareacodi));

            int iFamnomb = dr.GetOrdinal(this.Famnomb);
            if (!dr.IsDBNull(iFamnomb)) entity.Famnomb = dr.GetString(iFamnomb);

            int iFamnumconec = dr.GetOrdinal(this.Famnumconec);
            if (!dr.IsDBNull(iFamnumconec)) entity.Famnumconec = Convert.ToInt32(dr.GetValue(iFamnumconec));

            int iFamnombgraf = dr.GetOrdinal(this.Famnombgraf);
            if (!dr.IsDBNull(iFamnombgraf)) entity.Famnombgraf = dr.GetString(iFamnombgraf);

            int iFamestado = dr.GetOrdinal(this.Famestado);
            if (!dr.IsDBNull(iFamestado)) entity.Famestado = dr.GetString(iFamestado);

            int iUsuarioCreacion = dr.GetOrdinal(this.UsuarioCreacion);
            if (!dr.IsDBNull(iUsuarioCreacion)) entity.UsuarioCreacion = dr.GetString(iUsuarioCreacion);

            int iFechaCreacion = dr.GetOrdinal(this.FechaCreacion);
            if (!dr.IsDBNull(iFechaCreacion)) entity.FechaCreacion = Convert.ToDateTime(dr.GetValue(iFechaCreacion));

            int iUsuarioUpdate = dr.GetOrdinal(this.UsuarioUpdate);
            if (!dr.IsDBNull(iUsuarioUpdate)) entity.UsuarioUpdate = dr.GetString(iUsuarioUpdate);

            int iFechaUpdate = dr.GetOrdinal(this.FechaUpdate);
            if (!dr.IsDBNull(iFechaUpdate)) entity.FechaUpdate = Convert.ToDateTime(dr.GetValue(iFechaUpdate));

            return entity;
        }
        public TipoSerie CreateListaSerie(IDataReader dr)
        {
            TipoSerie entity = new TipoSerie();

            int iTipoSerieCodi = dr.GetOrdinal(this.TipoSerieCodi);
            if (!dr.IsDBNull(iTipoSerieCodi)) entity.Tiposeriecodi = Convert.ToInt32(dr.GetValue(iTipoSerieCodi));

            int iTipoSerieNomb = dr.GetOrdinal(this.TipoSerieNomb);
            if (!dr.IsDBNull(iTipoSerieNomb)) entity.Tiposerienomb = dr.GetString(iTipoSerieNomb);
            return entity;

        }

        public TipoPuntoMedicion CreateListaTipoPuntoMedicion(IDataReader dr)
        {
            TipoPuntoMedicion entity = new TipoPuntoMedicion();

            int iTipoPtoMediCodi = dr.GetOrdinal(this.TipoPtoMediCodi);
            if (!dr.IsDBNull(iTipoPtoMediCodi)) entity.TipoPtoMediCodi = Convert.ToInt32(dr.GetValue(iTipoPtoMediCodi));

            int iTipoPtoMediNomb = dr.GetOrdinal(this.TipoPtoMediNomb);
            if (!dr.IsDBNull(iTipoPtoMediNomb)) entity.TipoPtoMediNomb = dr.GetString(iTipoPtoMediNomb);

            int iTipoInfoDesc = dr.GetOrdinal(this.TipoInfoDesc);
            if (!dr.IsDBNull(iTipoInfoDesc)) entity.TipoInfoDesc = dr.GetString(iTipoInfoDesc);

            return entity;

        }

        public MePtomedicionDTO CreateListaPuntoMedicion(IDataReader dr)
        {
            MePtomedicionDTO entity = new MePtomedicionDTO();

            int iPtoMediCodi = dr.GetOrdinal(this.PtoMediCodi);
            if (!dr.IsDBNull(iPtoMediCodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtoMediCodi));

            int iPtoMediNomb = dr.GetOrdinal(this.PtoMediNomb);
            if (!dr.IsDBNull(iPtoMediNomb)) entity.Ptomedielenomb = dr.GetString(iPtoMediNomb);

            int iPtoMediDesc = dr.GetOrdinal(this.PtoMediDesc);
            if (!dr.IsDBNull(iPtoMediDesc)) entity.Ptomedidesc = dr.GetString(iPtoMediDesc);

            int iPtoMediBarraNomb = dr.GetOrdinal(this.PtoMediBarraNomb);
            if (!dr.IsDBNull(iPtoMediBarraNomb)) entity.Ptomedibarranomb = dr.GetString(iPtoMediBarraNomb);

            return entity;

        }
        public MePtomedicionDTO CreateListaPuntoMedicionCuenca(IDataReader dr)
        {
            MePtomedicionDTO entity = new MePtomedicionDTO();

            int iPtoMediCodi = dr.GetOrdinal(this.PtoMediCodi);
            if (!dr.IsDBNull(iPtoMediCodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtoMediCodi));

            int iEquiNomb = dr.GetOrdinal(this.EquiNomb);
            if (!dr.IsDBNull(iEquiNomb)) entity.Equinomb = dr.GetString(iEquiNomb);

            int iEmprNomb = dr.GetOrdinal(this.EmprNomb);
            if (!dr.IsDBNull(iEmprNomb)) entity.Emprnomb = dr.GetString(iEmprNomb);

            int iPtoMediNomb = dr.GetOrdinal(this.PtoMediNomb);
            if (!dr.IsDBNull(iPtoMediNomb)) entity.Ptomedielenomb = dr.GetString(iPtoMediNomb);

            return entity;

        }
        public MePtomedicionDTO CreatePuntoMedicion(IDataReader dr)
        {
            MePtomedicionDTO entity = new MePtomedicionDTO();

            int iPtoMediCodi = dr.GetOrdinal(this.PtoMediCodi);
            if (!dr.IsDBNull(iPtoMediCodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtoMediCodi));

            int iPtoMediNomb = dr.GetOrdinal(this.PtoMediNomb);
            if (!dr.IsDBNull(iPtoMediNomb)) entity.Ptomedielenomb = dr.GetString(iPtoMediNomb);

            int iPtoMediDesc = dr.GetOrdinal(this.PtoMediDesc);
            if (!dr.IsDBNull(iPtoMediDesc)) entity.Ptomedidesc = dr.GetString(iPtoMediDesc);

            int iPtoMediBarraNomb = dr.GetOrdinal(this.PtoMediBarraNomb);
            if (!dr.IsDBNull(iPtoMediBarraNomb)) entity.Ptomedibarranomb = dr.GetString(iPtoMediBarraNomb);

            int iTipoPtoMedi = dr.GetOrdinal(this.TipoPtoMedi);
            if (!dr.IsDBNull(iTipoPtoMedi)) entity.Tipoptomedinomb = dr.GetString(iTipoPtoMedi);

            int iEquiNomb = dr.GetOrdinal(this.EquiNomb);
            if (!dr.IsDBNull(iEquiNomb)) entity.Equinomb = dr.GetString(iEquiNomb);

            int iEquiPadre = dr.GetOrdinal(this.EquiPadre);
            if (!dr.IsDBNull(iEquiPadre)) entity.EquiPadrenomb = dr.GetString(iEquiPadre);

            int iEmprNomb = dr.GetOrdinal(this.EmprNomb);
            if (!dr.IsDBNull(iEmprNomb)) entity.Emprnomb  = dr.GetString(iEmprNomb);

            int iCoorX = dr.GetOrdinal(this.CoorX);
            if (!dr.IsDBNull(iCoorX)) entity.CoordenadaX = dr.GetString(iCoorX);

            int iCoorY = dr.GetOrdinal(this.CoorY);
            if (!dr.IsDBNull(iCoorY)) entity.CoordenadaY = dr.GetString(iCoorY);

            int iAltitud = dr.GetOrdinal(this.Altitud);
            if (!dr.IsDBNull(iAltitud)) entity.Altitud = dr.GetString(iAltitud);

            int iCapacidad = dr.GetOrdinal(this.Capacidad);
            if (!dr.IsDBNull(iCapacidad)) entity.Capacidad = dr.GetString(iCapacidad);

            int iPtoMediCalculado = dr.GetOrdinal(this.PtoMediCalculado);
            if (!dr.IsDBNull(iPtoMediCalculado)) entity.PtomediCalculado = dr.GetString(iPtoMediCalculado);

            return entity;

        }
        public GraficoSeries CreateListaGraficoTendencia(IDataReader dr)
        {
            GraficoSeries entity = new GraficoSeries();

            int iPtoMediCodi = dr.GetOrdinal(this.PtoMediCodi);
            if (!dr.IsDBNull(iPtoMediCodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtoMediCodi));

            int iPtoMediNomb = dr.GetOrdinal(this.PtoMediNomb);
            entity.Ptomedielenomb = dr.IsDBNull(iPtoMediNomb) ? null : dr.GetString(iPtoMediNomb); // Permitir nulos aquí


            int iAnio = dr.GetOrdinal("ANIO");
            if (!dr.IsDBNull(iAnio)) entity.Anio = Convert.ToInt32(dr.GetValue(iAnio));

            int iM1 = dr.GetOrdinal("M1");
            if (!dr.IsDBNull(iM1)) entity.M1 = Convert.ToDecimal(dr.GetValue(iM1));

            int iM2 = dr.GetOrdinal("M2");
            if (!dr.IsDBNull(iM2)) entity.M2 = Convert.ToDecimal(dr.GetValue(iM2));

            int iM3 = dr.GetOrdinal("M3");
            if (!dr.IsDBNull(iM3)) entity.M3 = Convert.ToDecimal(dr.GetValue(iM3));

            int iM4 = dr.GetOrdinal("M4");
            if (!dr.IsDBNull(iM4)) entity.M4 = Convert.ToDecimal(dr.GetValue(iM4));

            int iM5 = dr.GetOrdinal("M5");
            if (!dr.IsDBNull(iM5)) entity.M5 = Convert.ToDecimal(dr.GetValue(iM5));

            int iM6 = dr.GetOrdinal("M6");
            if (!dr.IsDBNull(iM6)) entity.M6 = Convert.ToDecimal(dr.GetValue(iM6));

            int iM7 = dr.GetOrdinal("M7");
            if (!dr.IsDBNull(iM7)) entity.M7 = Convert.ToDecimal(dr.GetValue(iM7));

            int iM8 = dr.GetOrdinal("M8");
            if (!dr.IsDBNull(iM8)) entity.M8 = Convert.ToDecimal(dr.GetValue(iM8));

            int iM9 = dr.GetOrdinal("M9");
            if (!dr.IsDBNull(iM9)) entity.M9 = Convert.ToDecimal(dr.GetValue(iM9));

            int iM10 = dr.GetOrdinal("M10");
            if (!dr.IsDBNull(iM10)) entity.M10 = Convert.ToDecimal(dr.GetValue(iM10));

            int iM11 = dr.GetOrdinal("M11");
            if (!dr.IsDBNull(iM11)) entity.M11 = Convert.ToDecimal(dr.GetValue(iM11));

            int iM12 = dr.GetOrdinal("M12");
            if (!dr.IsDBNull(iM12)) entity.M12 = Convert.ToDecimal(dr.GetValue(iM12));

            return entity;

        }

        
        public GraficoSeries CreateListaGrafico(IDataReader dr)
        {
            GraficoSeries entity = new GraficoSeries();

            int iPtoMediCodi = dr.GetOrdinal(this.PtoMediCodi);
            if (!dr.IsDBNull(iPtoMediCodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtoMediCodi));

            int iAnio = dr.GetOrdinal("ANIO");
            if (!dr.IsDBNull(iAnio)) entity.Anio = Convert.ToInt32(dr.GetValue(iAnio));

            int iM1 = dr.GetOrdinal("M1");
            if (!dr.IsDBNull(iM1)) entity.M1 = Convert.ToDecimal(dr.GetValue(iM1));

            int iM2 = dr.GetOrdinal("M2");
            if (!dr.IsDBNull(iM2)) entity.M2 = Convert.ToDecimal(dr.GetValue(iM2));

            int iM3 = dr.GetOrdinal("M3");
            if (!dr.IsDBNull(iM3)) entity.M3 = Convert.ToDecimal(dr.GetValue(iM3));

            int iM4 = dr.GetOrdinal("M4");
            if (!dr.IsDBNull(iM4)) entity.M4 = Convert.ToDecimal(dr.GetValue(iM4));

            int iM5 = dr.GetOrdinal("M5");
            if (!dr.IsDBNull(iM5)) entity.M5 = Convert.ToDecimal(dr.GetValue(iM5));

            int iM6 = dr.GetOrdinal("M6");
            if (!dr.IsDBNull(iM6)) entity.M6 = Convert.ToDecimal(dr.GetValue(iM6));

            int iM7 = dr.GetOrdinal("M7");
            if (!dr.IsDBNull(iM7)) entity.M7 = Convert.ToDecimal(dr.GetValue(iM7));

            int iM8 = dr.GetOrdinal("M8");
            if (!dr.IsDBNull(iM8)) entity.M8 = Convert.ToDecimal(dr.GetValue(iM8));

            int iM9 = dr.GetOrdinal("M9");
            if (!dr.IsDBNull(iM9)) entity.M9 = Convert.ToDecimal(dr.GetValue(iM9));

            int iM10 = dr.GetOrdinal("M10");
            if (!dr.IsDBNull(iM10)) entity.M10 = Convert.ToDecimal(dr.GetValue(iM10));

            int iM11 = dr.GetOrdinal("M11");
            if (!dr.IsDBNull(iM11)) entity.M11 = Convert.ToDecimal(dr.GetValue(iM11));

            int iM12 = dr.GetOrdinal("M12");
            if (!dr.IsDBNull(iM12)) entity.M12 = Convert.ToDecimal(dr.GetValue(iM12));

            return entity;

        }

        public GraficoSeries CreateListaGraficoCaudal(IDataReader dr)
        {
            GraficoSeries entity = new GraficoSeries();

            int iPtoMediCodi = dr.GetOrdinal(this.PtoMediCodi);
            if (!dr.IsDBNull(iPtoMediCodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtoMediCodi));

            int iAnio = dr.GetOrdinal("ANIO");
            if (!dr.IsDBNull(iAnio)) entity.Anio = Convert.ToInt32(dr.GetValue(iAnio));

            int iM1 = dr.GetOrdinal("M1");
            if (!dr.IsDBNull(iM1)) entity.M1 = Convert.ToDecimal(dr.GetValue(iM1));

            int iM2 = dr.GetOrdinal("M2");
            if (!dr.IsDBNull(iM2)) entity.M2 = Convert.ToDecimal(dr.GetValue(iM2));

            int iM3 = dr.GetOrdinal("M3");
            if (!dr.IsDBNull(iM3)) entity.M3 = Convert.ToDecimal(dr.GetValue(iM3));

            int iM4 = dr.GetOrdinal("M4");
            if (!dr.IsDBNull(iM4)) entity.M4 = Convert.ToDecimal(dr.GetValue(iM4));

            int iM5 = dr.GetOrdinal("M5");
            if (!dr.IsDBNull(iM5)) entity.M5 = Convert.ToDecimal(dr.GetValue(iM5));

            int iM6 = dr.GetOrdinal("M6");
            if (!dr.IsDBNull(iM6)) entity.M6 = Convert.ToDecimal(dr.GetValue(iM6));

            int iM7 = dr.GetOrdinal("M7");
            if (!dr.IsDBNull(iM7)) entity.M7 = Convert.ToDecimal(dr.GetValue(iM7));

            int iM8 = dr.GetOrdinal("M8");
            if (!dr.IsDBNull(iM8)) entity.M8 = Convert.ToDecimal(dr.GetValue(iM8));

            int iM9 = dr.GetOrdinal("M9");
            if (!dr.IsDBNull(iM9)) entity.M9 = Convert.ToDecimal(dr.GetValue(iM9));

            int iM10 = dr.GetOrdinal("M10");
            if (!dr.IsDBNull(iM10)) entity.M10 = Convert.ToDecimal(dr.GetValue(iM10));

            int iM11 = dr.GetOrdinal("M11");
            if (!dr.IsDBNull(iM11)) entity.M11 = Convert.ToDecimal(dr.GetValue(iM11));

            int iM12 = dr.GetOrdinal("M12");
            if (!dr.IsDBNull(iM12)) entity.M12 = Convert.ToDecimal(dr.GetValue(iM12));

            int iCaudal = dr.GetOrdinal("CAUDAL");
            if (!dr.IsDBNull(iCaudal)) entity.Caudal = dr.IsDBNull(iCaudal) ? null : dr.GetString(iCaudal);

            int iEMPRNOMB = dr.GetOrdinal("EMPRNOMB");
            if (!dr.IsDBNull(iEMPRNOMB)) entity.Emprnomb = dr.IsDBNull(iEMPRNOMB) ? null : dr.GetString(iEMPRNOMB);

            int iptomedielenomb = dr.GetOrdinal("ptomedielenomb");
            if (!dr.IsDBNull(iptomedielenomb)) entity.Ptomedielenomb = dr.IsDBNull(iptomedielenomb) ? null : dr.GetString(iptomedielenomb);

            return entity;

        }
        public TablaVertical CreateListaTablaVertical(IDataReader dr)
        {
            TablaVertical entity = new TablaVertical();

            int iPtoMediCodi = dr.GetOrdinal(this.PtoMediCodi);
            if (!dr.IsDBNull(iPtoMediCodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtoMediCodi));

            int iEquiNomb = dr.GetOrdinal(this.EquiNomb);
            entity.Equinomb = dr.IsDBNull(iEquiNomb) ? null : dr.GetString(iEquiNomb); // Permitir nulos aquí

            int iEmprNomb = dr.GetOrdinal(this.EmprNomb);
            entity.Emprnomb = dr.IsDBNull(iEmprNomb) ? null : dr.GetString(iEmprNomb); // Permitir nulos aquí

            int iPtoMediNomb = dr.GetOrdinal(this.PtoMediNomb);
            entity.Ptomedielenomb = dr.IsDBNull(iPtoMediNomb) ? null : dr.GetString(iPtoMediNomb); // Permitir nulos aquí

            int iAnio = dr.GetOrdinal("ANIO");
            if (!dr.IsDBNull(iAnio)) entity.Anio = Convert.ToInt32(dr.GetValue(iAnio));

            int iM1 = dr.GetOrdinal("M1");
            if (!dr.IsDBNull(iM1)) entity.M1 = Convert.ToDecimal(dr.GetValue(iM1));

            int iM2 = dr.GetOrdinal("M2");
            if (!dr.IsDBNull(iM2)) entity.M2 = Convert.ToDecimal(dr.GetValue(iM2));

            int iM3 = dr.GetOrdinal("M3");
            if (!dr.IsDBNull(iM3)) entity.M3 = Convert.ToDecimal(dr.GetValue(iM3));

            int iM4 = dr.GetOrdinal("M4");
            if (!dr.IsDBNull(iM4)) entity.M4 = Convert.ToDecimal(dr.GetValue(iM4));

            int iM5 = dr.GetOrdinal("M5");
            if (!dr.IsDBNull(iM5)) entity.M5 = Convert.ToDecimal(dr.GetValue(iM5));

            int iM6 = dr.GetOrdinal("M6");
            if (!dr.IsDBNull(iM6)) entity.M6 = Convert.ToDecimal(dr.GetValue(iM6));

            int iM7 = dr.GetOrdinal("M7");
            if (!dr.IsDBNull(iM7)) entity.M7 = Convert.ToDecimal(dr.GetValue(iM7));

            int iM8 = dr.GetOrdinal("M8");
            if (!dr.IsDBNull(iM8)) entity.M8 = Convert.ToDecimal(dr.GetValue(iM8));

            int iM9 = dr.GetOrdinal("M9");
            if (!dr.IsDBNull(iM9)) entity.M9 = Convert.ToDecimal(dr.GetValue(iM9));

            int iM10 = dr.GetOrdinal("M10");
            if (!dr.IsDBNull(iM10)) entity.M10 = Convert.ToDecimal(dr.GetValue(iM10));

            int iM11 = dr.GetOrdinal("M11");
            if (!dr.IsDBNull(iM11)) entity.M11 = Convert.ToDecimal(dr.GetValue(iM11));

            int iM12 = dr.GetOrdinal("M12");
            if (!dr.IsDBNull(iM12)) entity.M12 = Convert.ToDecimal(dr.GetValue(iM12));

            int iCaudal = dr.GetOrdinal("CAUDAL");
            if (!dr.IsDBNull(iCaudal)) entity.Caudal = dr.IsDBNull(iCaudal) ? null : dr.GetString(iCaudal);

            return entity;

        }
        public GraficoSeries CreateListaGraficoNombrePtoMedicion(IDataReader dr)
        {
            GraficoSeries entity = new GraficoSeries();

            int iPtoMediCodi = dr.GetOrdinal(this.PtoMediCodi);
            if (!dr.IsDBNull(iPtoMediCodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtoMediCodi));

            int iAnio = dr.GetOrdinal("ANIO");
            if (!dr.IsDBNull(iAnio)) entity.Anio = Convert.ToInt32(dr.GetValue(iAnio));

            int iM1 = dr.GetOrdinal("M1");
            if (!dr.IsDBNull(iM1)) entity.M1 = Convert.ToDecimal(dr.GetValue(iM1));

            int iM2 = dr.GetOrdinal("M2");
            if (!dr.IsDBNull(iM2)) entity.M2 = Convert.ToDecimal(dr.GetValue(iM2));

            int iM3 = dr.GetOrdinal("M3");
            if (!dr.IsDBNull(iM3)) entity.M3 = Convert.ToDecimal(dr.GetValue(iM3));

            int iM4 = dr.GetOrdinal("M4");
            if (!dr.IsDBNull(iM4)) entity.M4 = Convert.ToDecimal(dr.GetValue(iM4));

            int iM5 = dr.GetOrdinal("M5");
            if (!dr.IsDBNull(iM5)) entity.M5 = Convert.ToDecimal(dr.GetValue(iM5));

            int iM6 = dr.GetOrdinal("M6");
            if (!dr.IsDBNull(iM6)) entity.M6 = Convert.ToDecimal(dr.GetValue(iM6));

            int iM7 = dr.GetOrdinal("M7");
            if (!dr.IsDBNull(iM7)) entity.M7 = Convert.ToDecimal(dr.GetValue(iM7));

            int iM8 = dr.GetOrdinal("M8");
            if (!dr.IsDBNull(iM8)) entity.M8 = Convert.ToDecimal(dr.GetValue(iM8));

            int iM9 = dr.GetOrdinal("M9");
            if (!dr.IsDBNull(iM9)) entity.M9 = Convert.ToDecimal(dr.GetValue(iM9));

            int iM10 = dr.GetOrdinal("M10");
            if (!dr.IsDBNull(iM10)) entity.M10 = Convert.ToDecimal(dr.GetValue(iM10));

            int iM11 = dr.GetOrdinal("M11");
            if (!dr.IsDBNull(iM11)) entity.M11 = Convert.ToDecimal(dr.GetValue(iM11));

            int iM12 = dr.GetOrdinal("M12");
            if (!dr.IsDBNull(iM12)) entity.M12 = Convert.ToDecimal(dr.GetValue(iM12));

            int iPtoMediEleNomb = dr.GetOrdinal("PTOMEDIELENOMB");
            if (!dr.IsDBNull(iPtoMediEleNomb)) entity.Ptomedielenomb = dr.GetValue(iPtoMediEleNomb).ToString();

            return entity;

        }

        public MeRelacionptoDTO CreateRelacionPto(IDataReader dr)
        {
            MeRelacionptoDTO entity = new MeRelacionptoDTO();

            int iRelPtoCodi = dr.GetOrdinal(this.RelPtoCodi);
            if (!dr.IsDBNull(iRelPtoCodi)) entity.Relptocodi = Convert.ToInt32(dr.GetValue(iRelPtoCodi));

            int iPtoMediCodi1 = dr.GetOrdinal(this.PtoMediCodi1);
            if (!dr.IsDBNull(iPtoMediCodi1)) entity.Ptomedicodi1 = Convert.ToInt32(dr.GetValue(iPtoMediCodi1));

            int iPtoMediCodi2 = dr.GetOrdinal(this.PtoMediCodi2);
            if (!dr.IsDBNull(iPtoMediCodi2)) entity.Ptomedicodi2 = Convert.ToInt32(dr.GetValue(iPtoMediCodi2));

            int iTrPtoCodi = dr.GetOrdinal(this.TrPtoCodi);
            if (!dr.IsDBNull(iTrPtoCodi)) entity.Trptocodi = Convert.ToInt32(dr.GetValue(iTrPtoCodi));

            int iRelPtoFactor = dr.GetOrdinal(this.RelPtoFactor);
            if (!dr.IsDBNull(iRelPtoFactor)) entity.Relptofactor = Convert.ToDecimal(dr.GetValue(iRelPtoFactor));

            int iTipoInfoCodi = dr.GetOrdinal(this.TipoInfoCodi);
            if (!dr.IsDBNull(iTipoInfoCodi)) entity.Tipoinfocodi = Convert.ToInt32(dr.GetValue(iTipoInfoCodi));

            int iTPtoMediCodi = dr.GetOrdinal(this.TPtoMediCodi);
            if (!dr.IsDBNull(iTPtoMediCodi)) entity.Tptomedicodi = Convert.ToInt32(dr.GetValue(iTPtoMediCodi));

            int iRelPtoTabMed = dr.GetOrdinal(this.RelPtoTabMed);
            if (!dr.IsDBNull(iRelPtoTabMed)) entity.Relptotabmed = Convert.ToInt32(dr.GetValue(iRelPtoTabMed));

            int iRelPtoPotencia = dr.GetOrdinal(this.RelPtoPotencia);
            if (!dr.IsDBNull(iRelPtoPotencia)) entity.Relptopotencia = Convert.ToDecimal(dr.GetValue(iRelPtoPotencia));

            return entity;
        }



        #region Mapeo de Campos

        public string Famcodi = "FAMCODI";
        public string Famabrev = "FAMABREV";
        public string Tipoecodi = "TIPOECODI";
        public string Tareacodi = "TAREACODI";
        public string Famnomb = "FAMNOMB";
        public string Famnumconec = "FAMNUMCONEC";
        public string Famnombgraf = "FAMNOMBGRAF";
        public string Famestado = "FAMESTADO";
        public string UsuarioCreacion = "USUARIOCREACION";
        public string FechaCreacion = "FECHACREACION";
        public string UsuarioUpdate = "USUARIOUPDATE";
        public string FechaUpdate = "FECHAUPDATE";
        private const string Prefijo = "FAM";
        public static string Nomb = Prefijo + "NOMB";
        public string Areacodi = "AREACODI";

        public string Tareaabrev = "TAREAABREV";

        public string TipoSerieCodi = "TipoSerieCodi";
        public string TipoSerieNomb = "TipoSerieNomb";

        public string TipoPtoMediCodi = "tptomedicodi";
        public string TipoPtoMediNomb = "tptomedinomb";
        public string TipoInfoDesc = "tipoinfodesc";

        public string PtoMediCodi = "ptomedicodi";
        public string PtoMediNomb = "ptomedielenomb";
        public string PtoMediDesc = "ptomedidesc";
        public string PtoMediBarraNomb = "ptomedibarranomb";
        public string TipoPtoMedi = "tptomedinomb";
        public string EquiNomb = "EquiNomb";
        public string EquiPadre = "EquiPadre";
        public string EmprNomb = "EmprNomb";
        public string CoorX = "CoorX";
        public string CoorY = "CoorY";
        public string Altitud = "Altitud";
        public string Capacidad = "Cap";
        public string PtoMediCalculado = "ptomedicalculado";

        public string RelPtoCodi = "relptocodi";
        public string PtoMediCodi1 = "ptomedicodi1";
        public string PtoMediCodi2 = "ptomedicodi2";
        public string TrPtoCodi = "trptocodi";
        public string RelPtoFactor = "relptofactor";
        public string TipoInfoCodi = "tipoinfocodi";
        public string TPtoMediCodi = "tptomedicodi";
        public string RelPtoTabMed = "relptotabmed";
        public string RelPtoPotencia = "relptopotencia";

        public string EMPRCODI = "EMPRCODI";
        public string EQUICODI = "EQUICODI";
        public string TIPOPUNTOMEDICION = "TIPOPUNTOMEDICION";
        public string TIPOSERIECODI = "TIPOSERIECODI";
        public string PTOMEDICODI = "PTOMEDICODI";
        public string AnioInicio = "AnioInicio";
        public string AnioFin = "AnioFin";


        #endregion

        #region PR5
        public string SqlObtenerFamiliasXEmp
        {
            get { return base.GetSqlXml("ObtenerFamiliasXEmp"); }
        }
        #endregion

        #region INTERVENCIONES
        public string SqlListarComboTipoEquiposXUbicaciones
        {
            get { return base.GetSqlXml("ListarComboTipoEquiposXUbicaciones"); }
        }

        public string SqlObtenerFamiliasProcManiobras
        {
            get { return base.GetSqlXml("ObtenerFamiliasProcManiobras"); }
        }

        public string SqlListarByTareaIds
        {
            get { return base.GetSqlXml("ListarByTareaIds"); }
        }
        #endregion

        #region Mejoras Ieod

        public string SqlListarFamiliaPorOrigenLecturaEquipo
        {
            get { return base.GetSqlXml("ListarFamiliaPorOrigenLecturaEquipo"); }
        }
        #endregion

        #region FICHA TÉCNICA
        public string SqlListarFamiliasFT
        {
            get { return base.GetSqlXml("ListarFamiliasFT"); }
        }

        #endregion

    }
}
