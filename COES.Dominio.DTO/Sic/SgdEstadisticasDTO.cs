using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    public class SgdEstadisticasDTO
    {
        public int Sgdecodi { get; set; } //LLave SGD_ESTADISTICAS. Autogenerada
        public DateTime? Sgdefecderdirresp; //Fecha de Derivacion a la Direccion Responsable . Se obtiene de fljfechainicio de tabla DOC_FLUJO_DET AREACODE = CODIGO AREA DE DIRECCION
        public DateTime? Sgdefecderarearesp; //Fecha de Derivacion al Area Responsable. Se obtiene de fljfechainicio de tabla DOC_FLUJO_DET AREACODE != CODIGO AREA DE DIRECCION
        //Calculo
        public int Sgdedirrespcodi { get; set; } //Codigo de la Direccion Responsable de Atencion. En función al área y tabla FW_AREA (compcode = 1, areapadre).
        public int Sgdearearespcodi { get; set; } //Codigo del Area Responsable
        public int Sgdediasatencion { get; set; } //Dias de Atencion del Documento
        public int Sgdediasdearea { get; set; } //Numero de Dias de Derivacion al Area Responsable
        public int Sgdediasdedir { get; set; } //Numero de Dias de Derivacion a la Direccion Responsable
        public String Sgdediadoc { get; set; } //Dia del Documento : L, M,W,J,V,S,D
        public DateTime Sgdefeclimatecion { get; set; } //Fecha limite de atencion del documento

        //Auditoria
        public string Sgdeusucreacion { get; set; }  //usuario de creacion
        public DateTime Sgdefeccreacion { get; set; } //fecha de creacion
        public string Sgdeusumodificacion { get; set; } //usuario de modificacion
        public DateTime Sgdefecmodificacion { get; set; } // fecha de modificacion
        //Complemento de tabla
        public String Sgdedirrespnomb { get; set; } //Nombre de la Direccion Responsable de Atencion
        public String Sgdearearespnomb { get; set; } //Nombre del Area Responsable de Atencion

        public String ListEmprcodi { get; set; } //Lista de Empresas Multiselect
        public String ListAreacodi { get; set; } //Lista de Areas Multiselect         //Agregado por sts 17 oct 2017

        //Matching SI_EMPRESA
        public int Emprcodi { get; set; } //Codigo de la Empresa asociada al Documento

        ///Matching con DOC_FLUJO
        public int Fljcodi { get; set; } //Numerodel Documento SGDOC
        public int Tipcodi { get; set; } //Tipo del Documento

        public int Corrnumproc { get; set; } //Tipo del Documento

        public String DescTipoDoc { get; set; } //Descripcion Tipo Documento
        public int Fljdetcodi { get; set; }
        public int Fljdetcodiref { get; set; }
        public string Fljnumext { get; set; } //Codigo del Documento recibido
        public string Fljnumextresp { get; set; } //Codigo del Documento respuesta
        public string Fljnombre { get; set; } //Asunto del documento
        public string Fljestado { get; set; } //Situacion del documento
        public DateTime? Fljfecharecep { get; set; } //Fecha recepcion del documento
        public DateTime? Fljfechaproce { get; set; } //Fecha registro en el SGDOC
        public DateTime? Fljfechaorig { get; set; } //Fecha de emision del documento
        public int Fljdiasmaxaten { get; set; } //Dias limite de atencion del documento
        public DateTime? Fljfechaterm { get; set; } //Fecha de respuesta del documento
        public int Tatcodi { get; set; } //Tipo de atencion
        public int Areacodedest { get; set; } //Area Responsable de atencion

        public String ListaTipoAtencion { get; set; } //Lista Tipo Atencion Documentos
        public String ListaAreasPadres { get; set; } //Lista Tipo Atencion Documentos

        //Begin Section Filter

        public int FilterTipoReporte;
        public DateTime? FilterFechaInicio;
        public DateTime? FilterFechaFin;
        public int? FilterTipoDoc;
        public int? FilterIncumplimiento;
        public String FilterTipoAgente;
        public String FilterSein;
        public String FilterEmprCoes;
        public String FilterTipoEmpr;
        public String FilterAmbito;
        public String FilterDomiciliada;
        public int? FilterRubro;
        public String FilterAgente;
        //End Section Filter

        //Complementos

        public String Emprnomb { get; set; } //Nombre de la Empresa asociada al Documento


    }

}