using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla IND_DETCUADRO
    /// </summary>
    public class IndDetcuadroDTO : EntityBase
    {
        public int Indcuadia { get; set; }
        public string Indcuatipoindisp { get; set; }
        public DateTime? Indcuahorainieje { get; set; }
        public DateTime? Indcuahorafineje { get; set; }
        public string Indcuausumodificacion { get; set; }
        public DateTime? Indcuafecmodificacion { get; set; }
        public decimal? Indcua23potefectunid { get; set; }
        public decimal? Indcua2potrestuniddia { get; set; }
        public decimal? Indcua2indparcdia { get; set; }
        public decimal? Indcua3factork { get; set; }
        public int Indcuacodi { get; set; }
        public DateTime? Indcuahorainiejemod { get; set; }
        public DateTime? Indcuahorafinejemod { get; set; }
        public decimal? Indcua3potasegunid { get; set; }
        public int? Indcua5valorxunid { get; set; }
        public decimal? Indcuaasegpotefectunid { get; set; }
        public decimal? Indcuaasegpotasegunid { get; set; }
        public int? Indcua2duracionminunid { get; set; }
        public decimal? Indcuapotasegunidprommensual { get; set; }
        public int Equicodi { get; set; }
        public string Indcuamodooperacionpa { get; set; }
        public int? Indcuaindparchoraspa { get; set; }
        public string Indcuaprimerarranquepa { get; set; }
        public string Indcuaexitosapa { get; set; }
        public string Indcuacompensarpa { get; set; }
        public int Percuacodi { get; set; }
        public int Emprcodi { get; set; }
        public int Tgenercodi { get; set; }
        public int? Indcua1234minu { get; set; }

        //Indisponibilidades
        public string Emprnomb { get; set; }
        public string Central { get; set; }
        public string Equinomb { get; set; }
        public string Gruponomb { get; set; }
        public string GrupAbrev { get; set; }
        public int Grupocodi { get; set; }
        public int Grupopadre { get; set; }
        public int? SumaFor { get; set; }
        public int? SumaProg { get; set; }

        public int Indcuaadieje { get; set; }
        public int Equipadre { get; set; }

        public decimal Rendimiento { get; set; }

        public List<IndDetcuadroDTO> ListaCentral = new List<IndDetcuadroDTO>();
        public List<IndDetcuadroDTO> ListaUnidades = new List<IndDetcuadroDTO>();

        public decimal? Potenciaefectiva { get; set; }
        public decimal? Potenciagarantizada { get; set; }
        public decimal? Potenciafirme { get; set; }
        public decimal? Factorindisponibilidad { get; set; }
        public decimal? Factorpresencia { get; set; }
        public int Tipoptomedicodi { get; set; }
    }
}
