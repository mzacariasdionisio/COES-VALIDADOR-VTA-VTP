using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla VTP_INGRESO_POTEFR
    /// </summary>
    public interface IVtpIngresoPotefrRepository
    {
        int Save(VtpIngresoPotefrDTO entity);
        void Update(VtpIngresoPotefrDTO entity);
        void Delete(int ipefrcodi);
        VtpIngresoPotefrDTO GetById(int ipefrcodi);
        List<VtpIngresoPotefrDTO> List();
        List<VtpIngresoPotefrDTO> GetByCriteria(int pericodi, int recpotcodi);
        string GetResultSave(int? ipefrdia, int pericodi, int recpotcodi);
        string GetResultUpdate(int ipefrcodi,int? ipefrdia, int pericodi, int recpotcodi);
        void DeleteByCriteria(int pericodi, int recpotcodi);
    }
}
