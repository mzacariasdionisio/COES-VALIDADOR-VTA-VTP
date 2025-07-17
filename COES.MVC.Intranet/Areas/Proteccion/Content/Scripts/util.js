
function inputSoloNumerosDecimales(inputs) {
    for (let input of inputs) {
      $('#' + input).on('input', function () {
          let valor = $(this).val();
          let regex = /^(?:\d{1,4}(\.\d{0,4})?|10000(\.0{0,4})?)$/;
          // Validar el valor actual
          if (!regex.test(valor)) {
              $(this).val(valor.slice(0, -1));
          }
      });
  }
};
