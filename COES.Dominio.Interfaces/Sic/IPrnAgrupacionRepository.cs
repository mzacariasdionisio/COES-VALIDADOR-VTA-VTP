using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Sic;

namespace COES.Dominio.Interfaces.Sic
{
    public interface IPrnAgrupacionRepository
    {
        List<PrnAgrupacionDTO> List();
        void Save(PrnAgrupacionDTO entity);
        List<PrnAgrupacionDTO> ListById(int ptogrpcodi);
        List<MePtomedicionDTO> ListMeAgrupacion();
        
        int SavePuntoAgrupacion(PrnPuntoAgrupacionDTO entity);
        void CerrarPuntoAgrupacion(int ptogrpcodi, DateTime ptogrpfechafin);
        List<PrnPuntoAgrupacionDTO> ListByIdPuntoAgrupacion(int ptomedicodi);
        List<PrnAgrupacionDTO> ListPtosAgrupadosParaProdem();
        int ValidarNombreAgrupacion(string ptomedidesc);

        //20200113
        List<PrnAgrupacionDTO> ListDemandaAgrupada(int origlectcodi, int origlectcodi2, int lectcodi,
            int tipoinfocodi, string medifecha, string prnm48tipo, string areacodi, string emprcodi, 
            string ptomedicodi, int ptogrppronostico, int prnm48tipo2);

        //20200303
        List<MePtomedicionDTO> ListAgrupacionesActivas(string areacodi, string ptomedicodi, string emprcodi, int esPronostico);

        //20200309
        List<MePtomedicionDTO> ListPuntosPR03(string aonomb, string tipoemprcodi, string areacodi, string emprcodi, string ptomedicodi);
        List<EqAreaDTO> ListUbicacionesPR03(string aonomb);
        List<SiEmpresaDTO> ListEmpresasPR03(string tipoemprcodi, string emprcodi);
        List<PrnAgrupacionDTO> ListPuntosSeleccionados();
        List<PrnAgrupacionDTO> GetDetalleAgrupacion(int ptomedicodi);
        MePtomedicionDTO GetAgrupacion(int ptomedicodi);
    }
}
