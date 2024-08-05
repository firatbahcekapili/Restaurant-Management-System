function Filtrele() {
    // Seçilen konumu al
    var secilenKonum = document.getElementById("konumSecimi").value;

    // Tüm masaları gizle
    var masalar = document.getElementsByClassName("masa-konum");
    for (var i = 0; i < masalar.length; i++) {
        masalar[i].style.display = "none";
    }

    // Seçilen konuma ait masaları göster
    if (secilenKonum !== "TumKonumlar") {
        var secilenMasalar = document.getElementsByClassName(secilenKonum);
        for (var j = 0; j < secilenMasalar.length; j++) {
            secilenMasalar[j].style.display = "table";
        }
    } else {
        // Eğer "Tüm Konumlar" seçildiyse tüm masaları göster
        for (var k = 0; k < masalar.length; k++) {
            masalar[k].style.display = "table";
        }
    }
}

function DurumuDegistir(masaID, yeniDurum) {
    // AJAX kullanarak masa durumunu değiştirme
    $.ajax({
        url: '/Home/MasaDurumDegistir/' + masaID + '?durum=' + yeniDurum,
        type: 'POST',
        success: function (result) {

            var masaElement = document.getElementById(masaID);
            if (masaElement) {
                masaElement.classList.remove('border-left-danger', 'border-left-success');
                masaElement.classList.add('border-left-' + (result != 'Dolu' ? 'success' : 'danger'));
            }
            var durumselect = masaElement.querySelector('#durumDropdown');
            var durumText = masaElement.querySelector('#durumText');
            durumselect.value = result;
            durumText.innerText = result;

            Filtrele();
        },
        error: function (error) {
            console.error(error);
        }
    });
}





$(document).on('click', '.Adbtn', function () {


    var Masaid = $(this).closest('div').prevAll('input').attr('id').split('-')[1];
    var YeniAd = $(this).closest('div').prevAll('input').val();
    console.log(YeniAd)
    if (YeniAd != "") {

        $.ajax({
            url: '/Masalar/MasaAdDegis/',
            data: { Masaid: Masaid, YeniAd: YeniAd },
            type: 'POST',
            success: function (result) {
                var $htmlContent = $(result);

                $('#MasaAd-' + Masaid).html($htmlContent.find('#MasaAd-' + Masaid).text())
                $('.YeniAdDiv' + Masaid).remove();
                $('#YeniAd-' + Masaid).val('');
                $('#toggle-form-' + Masaid).collapse('hide');

            },
            error: function (error) {
                console.error(error);
            }





        });

    }

});

$(document).on('change', '.KapasiteDegis', function () {
    var label = $(this).prev('label');
    var yeniKapasite = $(this).val();
    var MasaId = $(this).attr('id').split('-')[1];
    console.log($(this).val());

    $.ajax({
        url: '/Masalar/MasaKapasiteDegis/',
        data: { Masaid: MasaId, Kapasite: yeniKapasite },
        type: 'POST',
        success: function (result) {
            var $htmlContent = $(result);
            label.text('Kapasite: ' + yeniKapasite);

            //var viewbagContent = $htmlContent.find('#demo2 li');



            //$('#demo2').html(viewbagContent);

            //document.getElementById("yemekSelect").selectedIndex = 0;
            //$('#demo1').empty();


            //var ozelliklermodal = $htmlContent.find('#ozellikler');



            //$('#ozelliklerParent').html(ozelliklermodal);

            //var viewbagContent = $htmlContent.find('#kataloglar');



            //$('#kataloglarParent').html(viewbagContent);
            //KatalogAd.value = "";


        },
        error: function (error) {
            console.error(error);
        }





    });
});




$(document).on('change', '#konumDegisim', function () {
    var Masaid = $(this).attr("masaid");




    if ($(this).val() == "Yeni Konum Ekle") {
        /*   $(this).prop('disabled', true);

         $(this).next('span').html('<div class="YeniKonumDiv' + Masaid + ' col-12 d-flex"><input type="text" name="name" value="" class="form-control " id="YeniKonum-' + Masaid + '" placeholder="Yeni Konum girin" /><button class="btn btn-primary ml-3 float-right Konumbtn">Değiştir</button><button onclick="CloseKonum(' + Masaid + ')" type="button" class="x" aria-hidden="true">❌</button></div> ');*/

        //$('#toggle-formkonum-' + Masaid).addClass('show');




    } else {
        YeniKonum = $(this).val();
        $.ajax({
            url: '/Masalar/MasaKonumDegis/',
            data: { Masaid: Masaid, Konum: YeniKonum },
            type: 'POST',
            success: function (result) {
                var $htmlContent = $(result);
                var eskiSelect = $('#konumSecimi').val();


                $('#masaAlani').empty();
                var yeni = $htmlContent.find('#masaparent');
                $('#masaAlani').html(yeni)
                var res = $htmlContent.find('#konumSecimi option').clone();
                $('#konumSecimi').empty();


                $('#konumSecimi').append(res);

                $('#konumSecimi').val(eskiSelect);
                $('#konumSecimi').change();


            },
            error: function (error) {
                console.error(error);
            }
        });
    }




});



$(document).on('click', '.Konumbtn', function () {


    var Masaid = $(this).closest('div').prevAll('input').attr('id').split('-')[1];
    var YeniKonum = $(this).closest('div').prevAll('input').val();
    if (YeniKonum != "") {

        $.ajax({
            url: '/Masalar/MasaKonumDegis/',
            data: { Masaid: Masaid, Konum: YeniKonum },
            type: 'POST',
            success: function (result) {
                var $htmlContent = $(result);

                var eskiSelect = $('#konumSecimi').val();

                $('#masaAlani').empty();
                var yeni = $htmlContent.find('#masaparent');
                $('#masaAlani').html(yeni)


                var res = $htmlContent.find('#konumSecimi option').clone();
                $('#konumSecimi').empty();


                $('#konumSecimi').append(res);


                $('#konumSecimi').val(eskiSelect);
                $('#konumSecimi').change();





            },
            error: function (error) {
                console.error(error);
            }





        });

    }

});

function MasaSil(Masaid) {

    $.ajax({
        url: '/Masalar/MasaSil/',
        data: { Masaid: Masaid },
        type: 'POST',
        success: function (result) {
            var $htmlContent = $(result);

            $('#masaAlani').empty();
            var yeni = $htmlContent.find('#masaparent');
            $('#masaAlani').html(yeni)

            //$('#silOnayModal').modal('hide');
            $('.x').click();
            //$('#silOnayModal').fadeOut();

            var res = $htmlContent.find('#konumSecimi option').clone();
            $('#konumSecimi').empty();
            $('#konumSecimi').append(res);

        },
        error: function (error) {
            console.error(error);
        }





    });
}


$(document).on('click', '.delete', function () {

    var Masaid = $(this).attr('id').split('-')[1];

    var Masaad = $(this).attr('id').split('-')[2];

    $('#masaadmodal').text(Masaad);
    $('#delete-footer').empty();
    $('#delete-footer').append('<button type="button" class="btn btn-secondary" data-dismiss="modal">İptal</button> <button type="button" onclick="MasaSil(' + Masaid + ')" class="btn btn-danger">Evet Sil!</button>')


});




$(document).on('click', '.MasaEkle', function () {

    $('#YeniMasaSelect').empty();

    $('#konumSecimi').find('option').clone().appendTo($('#YeniMasaSelect'));
    $('#YeniMasaSelect option[value="TumKonumlar"]').remove();

});
var clickCount = 0;
$(document).ready(function () {
    $('#Yeni-Masa-Btn').click(function () {
        // Form verilerini al
        var MasaAd = $('#YeniMasaAd').val();
        var Kapasite = $('#YeniKapasite').val();
        var Konum;
        if (!$('#YeniMasaSelect').prop('disabled')) {
            Konum = $('#YeniMasaSelect').val();

        } else {
            Konum = $('#YeniKonum').val();


        }



        // AJAX isteği gönder
        $.ajax({
            url: '/Masalar/MasaEkle', // Formun gönderileceği URL
            type: 'POST', // Gönderme metodu (GET veya POST)
            data: { MasaAd: MasaAd, Kapasite: Kapasite, Konum, Konum }, // Gönderilecek veriler
            success: function (result) {

                $htmlContent = $(result);
                var eskiSelect = $('#konumSecimi').val();

                $('#masaAlani').empty();
                var yeni = $htmlContent.find('#masaparent');
                $('#masaAlani').html(yeni)


                $('#Masa-Ekle-Modal').modal('hide');

                $('#YeniMasaSelect').prop('disabled', false);

                clickCount = 0;
                $('.YeniKonumDivModal').removeClass("show");
                $('#YeniKonum').val('');
                $('#YeniMasaAd').val('');
                $('#YeniKapasite').val('');
                clickCount = 0;


                var res = $htmlContent.find('#konumSecimi option').clone();

                $('#YeniMasaSelect').empty();
                $('#YeniMasaSelect').append(res);
                $('#YeniMasaSelect option[value="TumKonumlar"]').remove();
                $('#konumSecimi').empty();
                $('#konumSecimi').append(res);


                $('#konumSecimi').val(eskiSelect);
                $('#konumSecimi').change();

            },
            error: function (xhr, status, error) {

                alert('Masa eklenirken bir hata oluştu: ' + error);
            }
        });
    });
});



$(document).ready(function () {
    $('#MasaEkleForm').submit(function (e) {
        e.preventDefault(); // Formun normal submit işlemi engellendi
        // Ajax ile submit işlemi yapılıyor
        if ($(this).valid()) {

            var MasaAd = $('#YeniMasaAd').val();
            var Kapasite = $('#YeniKapasite').val();
            var Konum;
            if (!$('#YeniMasaSelect').prop('disabled')) {
                Konum = $('#YeniMasaSelect').val();

            } else {
                Konum = $('#YeniKonum').val();


            }

            console.log("yollanıyor");
            $.ajax({
                url: this.action,
                type: this.method,
                data: { MasaAd: MasaAd, Kapasite: Kapasite, Konum, Konum, Durum: "Boş" },
                success: function (result) {
                    // başarılı olursa handleSuccess fonksiyonu çalıştırılıyor
                    YeniMasaSucces(result);
                },
                error: function (xhr, status, error) {
                    // hata durumunda bir işlem yapılabilir
                    alert('Masa eklenirken bir hata oluştu: ' + error);
                }
            });

        } else {
            e.preventDefault();

        }

    });
});


function YeniMasaSucces(result) {
    console.log("YeniMasaSuccesYeniMasaSucces");
    $htmlContent = $(result);
    var eskiSelect = $('#konumSecimi').val();

    $('#masaAlani').empty();
    var yeni = $htmlContent.find('#masaparent');
    $('#masaAlani').html(yeni)


    $('#Masa-Ekle-Modal').modal('hide');

    $('#YeniMasaSelect').prop('disabled', false);

    clickCount = 0;
    $('.YeniKonumDivModal').removeClass("show");
    $('#YeniKonum').val('');
    $('#YeniMasaAd').val('');
    $('#YeniKapasite').val('');
    clickCount = 0;


    var res = $htmlContent.find('#konumSecimi option').clone();

    $('#YeniMasaSelect').empty();
    $('#YeniMasaSelect').append(res);
    $('#YeniMasaSelect option[value="TumKonumlar"]').remove();
    $('#konumSecimi').empty();
    $('#konumSecimi').append(res);


    $('#konumSecimi').val(eskiSelect);
    $('#konumSecimi').change();

}



$(document).on('click', '.yeniKonumbtn', function () {
    $('#YeniMasaSelect').prop('disabled', true);

    clickCount++;

    console.log(clickCount);
    if (clickCount === 2) {
        $('#YeniMasaSelect').prop('disabled', false);

        clickCount = 0;
    }





});







$(document).on('click', 'a[data-toggle="collapse"]', function () {

    var currentTarget = $(this).attr('href');
    $('[id^=toggle-form-]').not(currentTarget).collapse('hide');
    $('[id^=toggle-form-]').not(currentTarget).removeClass('show');
});
$(document).on('click', 'a[data-toggle="collapse"]', function () {

    var currentTarget = $(this).attr('href');
    $('[id^=toggle-formkonum-]').not(currentTarget).collapse('hide');
    $('[id^=toggle-formkonum-]').not(currentTarget).removeClass('show');
});
