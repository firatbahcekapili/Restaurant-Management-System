$(document).ready(function () {
    $('#AdminGirisForm').submit(function (e) {
        e.preventDefault();
        var kullaniciAdi = $('#aKullaniciAdi').val();
        var sifre = $('#aSifre').val();


        if ($('#AdminGirisForm').valid()) {

            console.log("valid");
            $.ajax({
                url: '/Giris/AdminGiris/',
                type: 'POST',
                data: { aKullaniciAdi: kullaniciAdi, aSifre: sifre },

                success: function (result) {
                    var data = $(result);
                    if (result.redirectUrl) {
                        console.log("if");
                        window.location.href = result.redirectUrl;
                    } else {



                        $('#aAlert').empty();
                        $('#aAlert').append(data.find('#aAlert div'));







                    }

                },
                error: function (error) {
                    console.error(error);
                }





            });
        }



    });

});
$(document).ready(function () {
    $('#PersonelGirisForm').submit(function (e) {
        e.preventDefault();
        var pkullaniciAdi = $('#KullaniciAdi').val();
        var psifre = $('#Sifre').val();



        if ($('#PersonelGirisForm').valid()) {

            console.log("valid");
            $.ajax({
                url: '/Giris/PersonelGiris/',
                type: 'POST',
                data: { KullaniciAdi: pkullaniciAdi, Sifre: psifre },
                success: function (result) {
                    var data = $(result);


                    if (result.redirectUrl) {
                        console.log("if");
                        window.location.href = result.redirectUrl;
                    } else {

                        $('#pAlert').empty();

                        $('#pAlert').append(data.find('#pAlert div'));





                    }

                },
                error: function (error) {
                    console.error(error);
                }





            });
        }



    });

});
