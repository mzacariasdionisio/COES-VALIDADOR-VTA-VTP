function validateAnioTarifarioAndVersion(idCbAnioTarifario, idCbVersion) {

    let data = {
        'AnioTarifario': parseInt($(idCbAnioTarifario).val()),
        'NumeroVersion': parseInt($(idCbVersion).val()),
        'Error': false,
        'ErrorMessage': ''
    }

    if (isNaN(data.AnioTarifario) || data.AnioTarifario < 0) {
        data.Error = true;
        data.ErrorMessage = 'Debe seleccionar un año tarifario';
        return data;
    }

    if (isNaN(data.NumeroVersion) || data.NumeroVersion < 0) {
        data.Error = true;
        data.ErrorMessage = 'Debe seleccionar una versión';
        return data;
    }

    return data;
}