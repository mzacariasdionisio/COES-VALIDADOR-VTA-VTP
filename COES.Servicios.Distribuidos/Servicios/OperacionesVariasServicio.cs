using System;
using System.Collections.Generic;
using System.ServiceModel;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.General;
using COES.Servicios.Distribuidos.Contratos;
using COES.Servicios.Aplicacion.OperacionesVarias;
using System.Globalization;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using COES.Servicios.Distribuidos.Models;
using COES.Servicios.Aplicacion.Eventos.Helper;

namespace COES.Servicios.Distribuidos.Servicios
{
    /// <summary>
    /// Implementa los contratos de los servicios
    /// </summary>
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Single, InstanceContextMode = InstanceContextMode.PerSession)]
    public class OperacionesVariasServicio : IOperacionesVariasServicio
    {
        OperacionesVariasAppServicio servicio = new OperacionesVariasAppServicio();

        public List<EveIeodcuadro> ObtenerRegistros(int evenClase, int subCausacodi, string fechaIni, string fechaFin)
        {
            List<EveIeodcuadro> registros = new List<EveIeodcuadro>();
            DateTime fechaInicio = DateTime.Now;
            DateTime fechaFinal = DateTime.Now;
            string formatoFecha = "dd/MM/yyyy";
            int nroPage = 1;
            int PageSizeEvento = 999999999;

            try
            {
                fechaInicio = DateTime.ParseExact(fechaIni, formatoFecha, CultureInfo.InvariantCulture);
                fechaFinal = DateTime.ParseExact(fechaFin, formatoFecha, CultureInfo.InvariantCulture);
            }
            catch { }

            List<EveIeodcuadroDTO> ListaIeodcuadro = servicio.BuscarOperacionesDetallado(evenClase, subCausacodi, fechaInicio, fechaFinal, nroPage, PageSizeEvento);

            for (int i = 0; i < ListaIeodcuadro.Count; i++)
            {
                EveIeodcuadro obj = new EveIeodcuadro();
                obj.Iccodi = ListaIeodcuadro[i].Iccodi;
                obj.Emprnomb = ListaIeodcuadro[i].Emprnomb;
                obj.Areanomb = ListaIeodcuadro[i].Areanomb;
                obj.Famabrev = ListaIeodcuadro[i].Famabrev;
                obj.Equiabrev = ListaIeodcuadro[i].Equiabrev;
                obj.Ichorini = ListaIeodcuadro[i].Ichorini;
                obj.Ichorfin = ListaIeodcuadro[i].Ichorfin;
                obj.Subcausadesc = ListaIeodcuadro[i].Subcausadesc;
                obj.Lastuser = ListaIeodcuadro[i].Lastuser;
                obj.Lastdate = ListaIeodcuadro[i].Lastdate;
                obj.Subcausacodi = ListaIeodcuadro[i].Subcausacodi;
                obj.Icvalor1 = ListaIeodcuadro[i].Icvalor1;
                obj.Equicodi = ListaIeodcuadro[i].Equicodi;
                obj.Icdescrip1 = ListaIeodcuadro[i].Icdescrip1;
                obj.Icdescrip2 = ListaIeodcuadro[i].Icdescrip2;
                obj.Icdescrip3 = ListaIeodcuadro[i].Icdescrip3;
                obj.Iccheck1 = ListaIeodcuadro[i].Iccheck1;
                obj.Numtrsgsostn = ListaIeodcuadro[i].Numtrsgsostn;
                obj.Numtrsgsubit = ListaIeodcuadro[i].Numtrsgsubit;
                obj.Iccheck2 = ListaIeodcuadro[i].Iccheck2;
                obj.Evenclasecodi = ListaIeodcuadro[i].Evenclasecodi;
                obj.Ichor3 = ListaIeodcuadro[i].Ichor3;
                obj.Ichor4 = ListaIeodcuadro[i].Ichor4;
                obj.Iccheck3 = ListaIeodcuadro[i].Iccheck3;
                obj.Iccheck4 = ListaIeodcuadro[i].Iccheck4;
                obj.Icvalor2 = ListaIeodcuadro[i].Icvalor2;
                obj.Emprcodi = ListaIeodcuadro[i].Emprcodi;
                obj.ListadoEquipos = new List<EveIeodcuadroEquipos>();
                //obj.listadoEquipos;
                List<EveIeodcuadroDetDTO> listadoEquipos = servicio.GetByCriteria(ListaIeodcuadro[i].Iccodi);
                for(int iEqList = 0; iEqList < listadoEquipos.Count; iEqList++)
                {
                    EveIeodcuadroEquipos objEquipo = new EveIeodcuadroEquipos();
                    objEquipo.Iccodi = listadoEquipos[iEqList].Iccodi;
                    objEquipo.Equicodi = listadoEquipos[iEqList].Equicodi;
                    objEquipo.Icdetcheck1 = listadoEquipos[iEqList].Icdetcheck1;
                    objEquipo.Emprnomb = listadoEquipos[iEqList].Emprnomb;
                    objEquipo.Areanomb = listadoEquipos[iEqList].Areanomb;
                    objEquipo.Famabrev = listadoEquipos[iEqList].Famabrev;
                    objEquipo.Equiabrev = listadoEquipos[iEqList].Equiabrev;
                    obj.ListadoEquipos.Add(objEquipo);
                }

                registros.Add(obj);
            }

            return registros;
        }

        public List<EveEvenclaseDTO> ObtenerHorizontes()
        {
            return servicio.ListarEvenclase();
        }

        public List<EveSubcausaeventoDTO> ObtenerTiposOperacion()
        {
            return servicio.ListarSubcausaeventoByAreausuaria(ConstantesOperacionesVarias.EvenSubcausa, -1);
        }

    }

       
}

/*
 
        public int Iccodi { get; set; }
        public int Equicodi { get; set; }
        public string Icdetcheck1 { get; set; }

        public string Emprnomb { get; set; }
        public string Areanomb { get; set; }
        public string Famabrev { get; set; }
        public string Equiabrev { get; set; }



                    int iEquicodi = dr.GetOrdinal(this.helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = dr.GetInt32(iEquicodi);

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iAreanomb = dr.GetOrdinal(this.helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    int iFamabrev = dr.GetOrdinal(this.helper.Famabrev);
                    if (!dr.IsDBNull(iFamabrev)) entity.Famabrev = dr.GetString(iFamabrev);

                    int iEquibrev = dr.GetOrdinal(this.helper.Equiabrev);
                    if (!dr.IsDBNull(iEquibrev)) entity.Equiabrev = dr.GetString(iEquibrev);

                    int iIcdetcheck1 = dr.GetOrdinal(this.helper.Icdetcheck1);
                    if (!dr.IsDBNull(iIcdetcheck1)) entity.Icdetcheck1 = dr.GetString(iIcdetcheck1);

                    int iIccodi = dr.GetOrdinal(this.helper.Iccodi);
                    if (!dr.IsDBNull(iIcdetcheck1)) entity.Iccodi = dr.GetInt32(iIccodi);



        List<EveEvenclaseDTO> ObtenerHorizontes
        List<EveSubcausaeventoDTO> ObtenerTiposOperacion();
            model.ListaEvenclase = this.servicio.ListarEvenclase();
            model.ListaEvensubcausa = this.servicio.ListarSubcausaeventoByAreausuaria(ConstantesOperacionesVarias.EvenSubcausa, base.IdArea);*/