using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Sic
{
    public interface ISiEmpresaMensajeRepository
    {
        int GetMaxId();
        void SaveEmpresaMensaje(SiEmpresaMensajeDTO entity, IDbConnection conn, DbTransaction tran);

        // -------------------------------------------------------------------------------------------------------------------- 
        // Elimina el regsitro del SI_EMPRESAMENSAJE
        // -------------------------------------------------------------------------------------------------------------------- 
        void EliminarEmpresaMensajeXEnvDetCodi(int envdetcodi, IDbConnection conn, DbTransaction tran);
        // --------------------------------------------------------------------------------------------------------------------
        
        SiEmpresaMensajeDTO GetById(int msgcodi, string intercodis);
    }
}
