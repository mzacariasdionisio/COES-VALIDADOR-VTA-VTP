using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Sic;

namespace COES.Dominio.Interfaces.Sic
{
    public interface IPrnClasificacionRepository
    {
        void Save(PrnClasificacionDTO entity);
        void Update(PrnClasificacionDTO entity);
        void Delete(int ptomedicodi, int lectcodi, DateTime prnclsfecha);
        List<PrnClasificacionDTO> List();
        PrnClasificacionDTO GetById(int ptomedicodi, int lectcodi, DateTime prnclsfecha);
        List<PrnClasificacionDTO> ListClasificacion48(DateTime medifecha, int prnmtipo, int lectcodi, int anivelcodi);//Eliminar?
        List<PrnClasificacionDTO> ListClasificacion96(DateTime medifecha, int prnmtipo, int lectcodi, int anivelcodi);//Eliminar?

        List<PrnClasificacionDTO> ListProdemPuntos(string areaoperativa);
        List<PrnClasificacionDTO> ListPuntosClasificados48(DateTime medifecha);
        List<PrnClasificacionDTO> ListPuntosClasificados96(DateTime medifecha);

        int CountMedicionesByRangoFechas(int ptomedicodi, int prnmtipo, DateTime fecini, DateTime fecfin);

        #region PRODEM 2 - Nuevas implementaciones 20191205

        List<PrnClasificacionDTO> ListDemandaClasificada(string ptomedicodi, string medifecha, string prnm48tipo, int lectcodi,
            int tipoinfocodi, int tipoemprcodi, string areacodi, string emprcodi, string prnclsperfil, string prnclsclasificacion, 
            string fechafin, string aocodi, string order);
        List<PrnClasificacionDTO> ListDemandaClasificadaBarrasCP(string ptomedicodi, string medifecha, string prnm48tipo, int lectcodi,
            int tipoinfocodi, int tipoemprcodi, string areacodi, string emprcodi, string prnclsperfil, string prnclsclasificacion,
            string fechafin, string aocodi, string order);

        List<PrnClasificacionDTO> ListDemandaNoClasificada(string ptomedicodi, string medifecha, string prnm48tipo, int lectcodi,
            int tipoinfocodi, int tipoemprcodi, string areacodi, string emprcodi);


        #endregion
    }
}
