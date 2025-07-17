using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;


namespace COES.Dominio.Interfaces.Sic
{
    public interface ISiSolicitudAmpliacionRepository
    {
        int Save(string fecha, int msgcodi,int emprcodi, string fechaplazo, string usuario, string fechaAct, int formatcodi, int fdatcodi, int flag);

        SiSolicitudAmpliacionDTO GetByMsgCodi(int MsgCodi);
    }
}
