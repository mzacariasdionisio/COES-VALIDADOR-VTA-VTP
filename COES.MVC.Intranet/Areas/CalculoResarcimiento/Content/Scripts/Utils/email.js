
(function ($) {

    $.fn.email_multiple = function (options) {

        let defaults = {
            reset: false,
            fill: false,
            data: null
        };

        let settings = $.extend(defaults, options);
        let email = "";

        return this.each(function () {
            $(this).after(
                "<div class=\"all-mail\"></div>\n" +
                "<input type=\"text\" name=\"email\" class=\"enter-mail-id\" placeholder=\"Ingrese correo ...\" />"
            );
            let $orig = $(this);
            let $element = $('.enter-mail-id');
            $element.keydown(function (e) {
                $element.css('border', '');
                if (e.keyCode === 13) { //Al presionar enter
                    let getValue = $element.val();
                    if (/^[a-z0-9._-]+@[a-z0-9._-]+\.[a-z]{2,}$/.test(getValue)) {
                        $('.all-mail').append('<span class="email-ids">' + getValue + '<span class="cancel-email" id="' + getValue + '">x</span></span>');//

                        $element.val('');

                        if (email.trim() == "") {
                            email += getValue;
                        } else {
                            email += ';' + getValue;
                        }

                    } else {
                        $element.css('border', '1px solid red !important')
                    }
                }
                
                $orig.val(email)
            });

            $(document).on('click', '.cancel-email', function () { //Borra un email ingresado
                
                $(this).parent().remove();
                var correos_ = document.getElementsByClassName("cancel-email");
                var nuevoVal = "";
                for (i = 0; i < correos_.length; i++) {
                    var id_ = correos_[i].id;
                    if (i == 0)
                        nuevoVal = id_;
                    else
                        nuevoVal = nuevoVal + ";" + id_;
                }
                $('#txtEmail').val(nuevoVal);
                email = nuevoVal;
            });

            if (settings.data) { // al abrir una empresa y lista los email existentes
                $.each(settings.data, function (x, y) {
                    
                    if (/^[a-z0-9._-]+@[a-z0-9._-]+\.[a-z]{2,}$/.test(y)) {
                        $('.all-mail').append('<span class="email-ids">' + y + '<span class="cancel-email" id="' + y + '">x</span></span>');
                        $element.val('');

                        email += y + ';'
                    } else {
                        $element.css('border', '1px solid red')
                    }
                })

                email = email.slice(0, -1);                
                $orig.val(email)
            }

            if (settings.reset) { //Elimina todos los correos
                $('.email-ids').remove()
            }

            return $orig.hide()
        });
    };

})(jQuery);
