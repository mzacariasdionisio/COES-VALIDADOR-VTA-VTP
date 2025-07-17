using System;
using System.Collections.Generic;
using System.ServiceModel;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.General;
using COES.Servicios.Distribuidos.Contratos;
using System.Globalization;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using COES.Servicios.Distribuidos.Models;
using COES.Servicios.Aplicacion.Eventos.Helper;
using COES.Servicios.Aplicacion.Hidrologia;
using System.Linq;

namespace COES.Servicios.Distribuidos.Servicios
{
    /// <summary>
    /// Implementa los contratos de los servicios
    /// </summary>
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Single, InstanceContextMode = InstanceContextMode.PerSession)]
    public class HidrologiaServicio : IHidrologiaServicio
    {
        private GeneralAppServicio logicGeneral = new GeneralAppServicio();
        HidrologiaAppServicio logic = new HidrologiaAppServicio();
        public const int IdOrigenLectura = 16;
        public const string FormatoFecha = "dd/MM/yyyy";
        public List<MeMedicion24> ObtenerData(string idsEmpresa, string fecha, string fechaFinal, string idsTipoPtoMed)
        {
            List<MeMedicion24> listaM24 = new List<MeMedicion24>();
            HidrologiaModel model = new HidrologiaModel();
            DateTime fechaIni = DateTime.MinValue;
            DateTime fechaFin = DateTime.MinValue;
            int tipoReporte = 1;
            fechaIni = DateTime.ParseExact(fecha, FormatoFecha, CultureInfo.InvariantCulture);
            fechaFin = DateTime.ParseExact(fechaFinal, FormatoFecha, CultureInfo.InvariantCulture);
            List<MeMedicion24DTO> data = this.logic.ListaMed24HidrologiaTiempoReal(tipoReporte, IdOrigenLectura, idsEmpresa, fechaIni, fechaFin, idsTipoPtoMed);


            for (int i = 0; i < data.Count; i++)
            {

                MeMedicion24 obj = new MeMedicion24();

                obj.Lectcodi = data[i].Lectcodi;
                obj.Medifecha = data[i].Medifecha;
                obj.Tipoinfocodi = data[i].Tipoinfocodi;
                obj.Ptomedicodi = data[i].Ptomedicodi;
                obj.Meditotal = data[i].Meditotal;
                obj.Mediestado = data[i].Mediestado;
                obj.H1 = data[i].H1;
                obj.H2 = data[i].H2;
                obj.H3 = data[i].H3;
                obj.H4 = data[i].H4;
                obj.H5 = data[i].H5;
                obj.H6 = data[i].H6;
                obj.H7 = data[i].H7;
                obj.H8 = data[i].H8;
                obj.H9 = data[i].H9;
                obj.H10 = data[i].H10;
                obj.H11 = data[i].H11;
                obj.H12 = data[i].H12;
                obj.H13 = data[i].H13;
                obj.H14 = data[i].H14; 
                obj.H15 = data[i].H15;
                obj.H16 = data[i].H16;
                obj.H17 = data[i].H17;
                obj.H18 = data[i].H18;
                obj.H19 = data[i].H19;
                obj.H20 = data[i].H20;
                obj.H21 = data[i].H21;
                obj.H22 = data[i].H22;
                obj.H23 = data[i].H23;
                obj.H24 = data[i].H24;
                obj.Lastuser = data[i].Lastuser;
                obj.Lastdate = data[i].Lastdate;
                obj.Equicodi = data[i].Equicodi;
                obj.Equinomb = data[i].Equinomb;
                obj.Emprcodi = data[i].Emprcodi;
                obj.Emprnomb = data[i].Emprnomb;
                obj.Tipoinfoabrev = data[i].Tipoinfoabrev;
                obj.Tipoptomedicodi = data[i].Tipoptomedicodi;
                obj.Tipoptomedinomb = data[i].Tipoptomedinomb;
                obj.Ptomedibarranomb = data[i].Ptomedibarranomb;
                obj.Famcodi = data[i].Famcodi;
                obj.Famabrev = data[i].Famabrev;
                obj.IdCuenca = data[i].IdCuenca;
                obj.Cuenca = data[i].Cuenca;
                obj.Gruponomb = data[i].Gruponomb;
                obj.Ptomedielenomb = data[i].Ptomedielenomb;
                obj.Grupocodi = data[i].Grupocodi;
                obj.ReporteOrden = data[i].ReporteOrden;
                obj.Osinergcodi = data[i].Osinergcodi;

                //MigracionSGOCOES-GrupoB
                obj.Grupoabrev = data[i].Grupoabrev;
                obj.Grupotipo = data[i].Grupotipo;
                obj.Equiabrev = data[i].Equiabrev;
                obj.Minimo = data[i].Minimo;
                obj.PotenciaEfectiva = data[i].PotenciaEfectiva;
                obj.Digsilent = data[i].Digsilent;
                obj.Areanomb = data[i].Areanomb;
                obj.FechapropequiMin = data[i].FechapropequiMin;
                obj.FechapropequiPotefec = data[i].FechapropequiPotefec;
                obj.FechapropequiMinDesc = data[i].FechapropequiMinDesc;
                obj.FechapropequiPotefecDesc = data[i].FechapropequiPotefecDesc;
                obj.FactorUnidadFicticia = data[i].FactorUnidadFicticia;
                obj.NumUnidadesXGrupo = data[i].NumUnidadesXGrupo;
                obj.MVAxUnidad = data[i].MVAxUnidad;
                obj.PotenciaTotalMVA = data[i].PotenciaTotalMVA;

                listaM24.Add(obj);
            }
                
            return listaM24;
        }

        public List<SiEmpresaDTO> listadoEmpresas()
        {
            return this.logicGeneral.ObtenerEmpresasHidro();
        }

        public List<SiTipoinformacionDTO> listadoUnidades()
        {
            int[] tipoInfocodis = { 11, 14, 40, 1, 3 };
            List <SiTipoinformacionDTO> data =  this.logic.ListSiTipoinformacions().Where(x => tipoInfocodis.Contains(x.Tipoinfocodi)).ToList();
            foreach (var item in data)
            {
                switch (item.Tipoinfocodi)
                {
                    case 11:
                        item.Tipoinfodesc = "Caudal";
                        break;
                    case 14:
                        item.Tipoinfodesc = "Volumen";
                        break;
                    case 40:
                        item.Tipoinfodesc = "Nivel";
                        break;
                    case 1:
                        item.Tipoinfodesc = "Potencia promedio";
                        break;
                    case 3:
                        item.Tipoinfodesc = "Energía restante";
                        break;
                }            
            }
            return data;
        }

    }


}
