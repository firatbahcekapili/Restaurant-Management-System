$(document).on('click', '.close-toast', function () {
    $(".toast").toast('hide');

});
new Sortable(trash, {
    group: {
        name: 'nonshared',

        put: ["shared"] // Bu liste içine öğe eklemeye izin ver
    },

    handle: '.list-group-item',

    sort: false
});

$(document).ready(function () {
    // Sayfa yüklendiğinde çalışacak fonksiyon

    $('#demo1').empty();


});

$(document).ready(function () {



    $('#katalogSelect').change(function () {
        // Seçilen katalog ID'sini al

        if ($(this).val() == "Katalog Ekle") {
            $("#kt_modal_stacked_1").modal('show');
            document.getElementById("katalogSelect").selectedIndex = 0;
        } else {
            var selectedKatalogId = $(this).val();

            // AJAX isteğiyle sunucuya seçilen katalog ID'si ile filtreleme yapılacak
            $.ajax({
                type: 'POST',
                url: '/YemekKatalog/Filtrele', // ControllerAdi, kendi Controller adınıza uygun olarak değiştirilmelidir
                data: { katalogId: selectedKatalogId },
                success: function (data) {
                    // Gelen veriyi kullanarak sayfa içeriğini güncelle
                    $('#demo1').empty(); // Önceki içeriği temizle
                    console.log('Filtreleme başarılı');
                    for (var key in data) {
                        if (data.hasOwnProperty(key)) {
                            var ozellikAdi = key;
                            var ozellikId = data[key];
                            $('#demo1').append('<li class="list-group-item" data-item-id="' + ozellikId + '">' + ozellikAdi + '</li>');
                        }
                    }

                },
                error: function () {
                    console.error('Filtreleme sırasında bir hata oluştu');
                }
            });

        }




    });


    //// Demo 1 için Sortable
    //new Sortable(document.getElementById('demo1'), {
    //    group: {
    //        name: 'shared',
    //    },
    //    animation: 150,
    //    sort: true // Sıralamayı devre dışı bırakmak için: sort'u false yapın
    //    , onEnd: function (evt) {
    //        var itemId = evt.item.dataset.itemId; // Sürüklenen öğenin benzersiz kimliği
    //        var katalog = document.getElementById('katalogSelect').value;
    //       katalog
    //        console.log(itemId);
    //        $.ajax({
    //            type: 'POST',
    //            url: '/YemekKatalog/OzellikKaldir', // Silme işlemini gerçekleştirecek URL
    //            data: { OzellikId: itemId ,KatalogId: katalog},
    //            success: function (result) {
    //                // Veritabanından silme başarılı
    //                console.log('Veritabanından silme başarılı');
    //                // Silinen öğeyi DOM'dan kaldır
    //                 evt.item.parentNode.removeChild(evt.item);
    //               /* $('#demo1').empty();*/
    //                //document.getElementById("partial").innerHTML = result;
    //            },
    //            error: function () {
    //                // Veritabanından silme sırasında bir hata oluştu
    //                console.error('Veritabanından silme sırasında bir hata oluştu');
    //            }
    //        });
    //    }
    //});

    // Demo 1 için Sortable
    new Sortable(document.getElementById('demo1'), {
        group: {
            name: 'shared',
            put: ["all"],
        },
        animation: 150,
        sort: true, // Sıralamayı devre dışı bırakmak için: sort'u false yapın
        onEnd: function (evt) {
            var itemId = evt.item.dataset.itemId; // Sürüklenen öğenin benzersiz kimliği
            var katalog = document.getElementById('katalogSelect').value;

            // Eğer öğe çöp kutusuna sürüklenip bırakıldıysa
            if (evt.to.id === 'trash') {
                console.log('Çöp kutusuna sürüklendi. Silme işlemi yapılacak.');
                $.ajax({
                    type: 'POST',
                    url: '/YemekKatalog/OzellikKaldir', // Silme işlemini gerçekleştirecek URL
                    data: { OzellikId: itemId, KatalogId: katalog },
                    success: function (result) {
                        // Veritabanından silme başarılı
                        console.log('Veritabanından silme başarılı');
                        $('#trash').empty();
                        document.getElementById("deneme").innerHTML = "Silindi";
                        $('.toast').toast('show');
                    },
                    error: function () {
                        // Veritabanından silme sırasında bir hata oluştu
                        console.error('Veritabanından silme sırasında bir hata oluştu');
                        document.getElementById("deneme").innerHTML = 'Veritabanından silme sırasında bir hata oluştu';
                        $('.toast').toast('show');
                    }
                });

            }
        }
    });



    // Demo 2 için Sortable
    new Sortable(document.getElementById('demo2'), {
        //multiDrag: true, // Enable multi-drag
        //selectedClass: 'selected', // The class applied to the selected items
        fallbackTolerance: 3, // So that we can select items on mobile
        group: {
            name: 'all',
            pull: 'clone',
            put: false // Bu liste içine öğe eklemeye izin verme
        },
        handle: '.list-group-item',
        pull: 'clone',
        sort: true,
        animation: 150,
        onEnd: function (evt) {
            var itemId = evt.item.dataset.itemId; // Sürüklenen öğenin benzersiz kimliği
            var katalog = document.getElementById('katalogSelect').value;

            // Eğer öğe demo1 e sürüklenip bırakıldıysa
            if (evt.to.id === 'demo1') {
                console.log('Ekleme işlemi yapılacak.');
                $.ajax({
                    type: 'POST',
                    url: '/YemekKatalog/OzellikEkle', // Ekleme işlemini gerçekleştirecek URL
                    data: { OzellikId: itemId, KatalogId: katalog },
                    success: function (result) {

                        var deneme = document.getElementById("deneme");
                        deneme.innerHTML = result;
                        var a = deneme.querySelector('#viewbag');



                        $('#deneme').html(a);


                        if (a.innerHTML === "Başarılı") {
                            // Veritabanına ekleme başarılı
                            console.log('Veritabanına ekleme başarılı');

                            /*$('#trash').empty();*/
                        } else {


                            $(evt.item).remove();



                            console.log('Zaten böyle bir özellik mevcut')

                        }
                        $('.toast').toast('show');



                    },
                    error: function () {
                        console.error('Veritabanına ekleme sırasında bir hata oluştu');
                        document.getElementById("deneme").innerHTML = 'Veritabanına ekleme sırasında bir hata oluştu';
                        $(evt.item).remove();
                        $('.toast').toast('show');
                    }
                });

            }

        }
    });



});


function Sil(KatalogId) {

    var id = parseInt(KatalogId)
    $.ajax({
        url: '/YemekKatalog/KatalogDelete',
        data: { KatalogId: KatalogId },
        type: 'POST',
        success: function (result) {
            var $htmlContent = $(result);

            // Sadece istediğiniz öğeyi seçin
            var viewbagContent = $htmlContent.find('#kataloglar');


            var deneme = document.getElementById("kataloglar");

            $('#kataloglarParent').html(viewbagContent);

            var katalog = $htmlContent.find('#katalogSelect option');

            $('#katalogSelect').html(katalog);

            $('#kt_modal_stacked_2').modal('hide');


            document.getElementById("katalogSelect").selectedIndex = 0;
            $('#katalogSelect').trigger('change');
        },
        error: function (error) {
            console.error(error);
        }
    });


}

function Ekle() {
    var KatalogAd = document.getElementById("KatalogAd");
    console.log(KatalogAd);

    $.ajax({
        url: '/YemekKatalog/KatalogCreate',
        data: { KatalogAd: KatalogAd.value },
        type: 'POST',
        success: function (result) {
            var $htmlContent = $(result);

            // Sadece istediğiniz öğeyi seçin
            var viewbagContent = $htmlContent.find('#kataloglar');

            var deneme = document.getElementById("kataloglar");

            $('#kataloglarParent').html(viewbagContent);
            KatalogAd.value = "";


            var katalog = $htmlContent.find('#katalogSelect option');

            $('#katalogSelect').html(katalog);
            document.getElementById("katalogSelect").selectedIndex = 0;
            $('#katalogSelect').trigger('change');


        },
        error: function (error) {
            console.error(error);
        }
    });


}

$(document).ready(function () {
    $('#KatalogEkleForm').submit(function (e) {
        e.preventDefault();
        var KatalogAd = document.getElementById("KatalogAd");

        if ($('#KatalogEkleForm').valid()) {
            $.ajax({
                url: '/YemekKatalog/KatalogCreate',
                data: { KatalogAd: KatalogAd.value },
                type: 'POST',
                success: function (result) {
                    var $htmlContent = $(result);

                    // Sadece istediğiniz öğeyi seçin
                    var viewbagContent = $htmlContent.find('#kataloglar');

                    var deneme = document.getElementById("kataloglar");

                    $('#kataloglarParent').html(viewbagContent);
                    KatalogAd.value = "";


                    var katalog = $htmlContent.find('#katalogSelect option');

                    $('#katalogSelect').html(katalog);
                    document.getElementById("katalogSelect").selectedIndex = 0;
                    $('#katalogSelect').trigger('change');


                },
                error: function (error) {
                    console.error(error);
                }
            });
        }



    });

});

function SilOzellik(OzellikId) {

    var id = parseInt(OzellikId)
    $.ajax({
        url: '/YemekKatalog/OzellikDelete',
        data: { OzellikId: OzellikId },
        type: 'POST',
        success: function (result) {
            var $htmlContent = $(result);

            // Sadece istediğiniz öğeyi seçin
            var viewbagContent = $htmlContent.find('#ozellikler');


            var deneme = document.getElementById("ozellikler");

            $('#ozelliklerParent').html(viewbagContent);

            var ozellikler = $htmlContent.find('#demo2 li');

            $('#demo2').html(ozellikler);
            var OzellikAd = document.getElementById("OzellikAd");
            OzellikAd.value = "";

            $('#kt_modal_stacked_4').modal('hide');


        },
        error: function (error) {
            console.error(error);
        }
    });


}

function EkleOzellik() {
    var OzellikAd = document.getElementById("OzellikAd");
    console.log(OzellikAd);

    $.ajax({
        url: '/YemekKatalog/OzellikCreate',
        data: { OzellikAd: OzellikAd.value },
        type: 'POST',
        success: function (result) {
            var $htmlContent = $(result);

            // Sadece istediğiniz öğeyi seçin
            var viewbagContent = $htmlContent.find('#ozellikler');

            var deneme = document.getElementById("ozellikler");

            $('#ozelliklerParent').html(viewbagContent);
            OzellikAd.value = "";


            var ozellikler = $htmlContent.find('#demo2 li');

            $('#demo2').html(ozellikler);


        },
        error: function (error) {
            console.error(error);
        }
    });


}
$(document).ready(function () {
    $('#OzellikEkleForm').submit(function (e) {
        e.preventDefault();
        var OzellikAd = document.getElementById("OzellikAd");

        if ($('#OzellikEkleForm').valid()) {


            $.ajax({
                url: '/YemekKatalog/OzellikCreate',
                data: { OzellikAd: OzellikAd.value },
                type: 'POST',
                success: function (result) {
                    var $htmlContent = $(result);

                    // Sadece istediğiniz öğeyi seçin
                    var viewbagContent = $htmlContent.find('#ozellikler');

                    var deneme = document.getElementById("ozellikler");

                    $('#ozelliklerParent').html(viewbagContent);
                    OzellikAd.value = "";


                    var ozellikler = $htmlContent.find('#demo2 li');

                    $('#demo2').html(ozellikler);


                },
                error: function (error) {
                    console.error(error);
                }
            });

        }



    });

});

function OzellikEditStart(OzellikId) {

    var $ozellik = $('#demo2 li[data-item-id="' + OzellikId + '"] span');
    $ozellik.empty();
    $ozellik.append('<div class="row w-100 justify-content-center mt-4" id="ozellik-parent-' + OzellikId + '"> <input class="form-control col-8 mr-2" type="text" id="input-' + OzellikId + '" value="" /> <a  class=" input-ok col-2 mr-2 d-flex align-items-center text-decoration-none" id="input-ok-' + OzellikId + '"><i class="fa-regular fa-paper-plane"></i></a> \n <div class="col-1 p-0 float-right d-flex flex-row-reverse align-items-center"> <a class="  input-exit  close " id="input-exit-' + OzellikId + '">&times;</a></div> </div>');



}
$(document).on('click', '.input-ok', function () {
    // btnok butonuna tıklandığında yapılacak işlemler buraya gelecek
    var OzellikId = $(this).attr('id').split("-")[2];
    console.log(OzellikId);
    var OzellikAd = $('#input-' + OzellikId).val();
    console.log(OzellikAd.toString());


    if (OzellikAd != "") {

        $.ajax({
            url: '/YemekKatalog/OzellikDuzenle/',
            data: { OzellikId: OzellikId, OzellikAd: OzellikAd },
            type: 'POST',
            success: function (result) {
                var $htmlContent = $(result);


                var viewbagContent = $htmlContent.find('#demo2 li');



                $('#demo2').html(viewbagContent);

                document.getElementById("katalogSelect").selectedIndex = 0;
                $('#demo1').empty();


                var ozelliklermodal = $htmlContent.find('#ozellikler');



                $('#ozelliklerParent').html(ozelliklermodal);





            },
            error: function (error) {
                console.error(error);
            }





        });

    }



});



$(document).on('click', '.modal3', function () {
    // 2 saniye sonra içeriği kontrol etmek için setTimeout kullan
    setTimeout(function () {
        // Tüm marquee-container elementlerini seç
        $('.marquee-container').each(function () {
            // container içindeki marquee-content elementini seç
            var content = $(this).find('.content');

            console.log(content.width());
            // Eğer content'in genişliği container'ın genişliğinden büyükse marquee efektini başlat
            if (content.width() > $(this).width()) {
                content.addClass("marquee-content");
            }
        });
    }, 1000); // 1 saniye beklet
});


$(document).on('click', '.input-ok-modal', function () {
    // btnok butonuna tıklandığında yapılacak işlemler buraya gelecek
    var OzellikId = $(this).attr('id').split("-")[2];
    console.log(OzellikId);
    var OzellikAd = $('#input-ozellikad-' + OzellikId).val();
    console.log(OzellikAd.toString());


    if (OzellikAd != "") {

        $.ajax({
            url: '/YemekKatalog/OzellikDuzenle/',
            data: { OzellikId: OzellikId, OzellikAd: OzellikAd },
            type: 'POST',
            success: function (result) {
                var $htmlContent = $(result);


                var viewbagContent = $htmlContent.find('#ozellikler');

                var deneme = document.getElementById("ozellikler");

                $('#ozelliklerParent').html(viewbagContent);
                OzellikAd.value = "";


                var ozellikler = $htmlContent.find('#demo2 li');

                $('#demo2').html(ozellikler);




            },
            error: function (error) {
                console.error(error);
            }





        });

    }



});

$(document).on('click', '.input-exit', function () {

    var elementId = $(this).attr('id');
    var OzellikId = elementId.split("-")[2];
    $('#ozellik-parent-' + OzellikId).remove();

});





function KyeniAd(KatalogId) {


    var KatalogAd = $('#input-katalogad-' + KatalogId).val();
    console.log(KatalogAd.toString());


    if (KatalogAd.trim() != "") {
        $.ajax({
            url: '/YemekKatalog/KatalogDuzenle/',
            data: { KatalogId: KatalogId, KatalogAd: KatalogAd },
            type: 'POST',
            success: function (result) {
                var $htmlContent = $(result);

                // Sadece istediğiniz öğeyi seçin
                var viewbagContent = $htmlContent.find('#kataloglar');

                var deneme = document.getElementById("kataloglar");

                $('#kataloglarParent').html(viewbagContent);
                KatalogAd.value = "";


                var katalog = $htmlContent.find('#katalogSelect option');

                $('#katalogSelect').html(katalog);

                document.getElementById("katalogSelect").selectedIndex = 0;
                $('#demo1').empty();

                document.getElementById("katalogSelect").selectedIndex = 0;
                $('#katalogSelect').trigger('change');


            },
            error: function (error) {
                console.error(error);
            }





        });

    } else {
        document.getElementById("deneme").innerHTML = "Katalog adı boş olamaz";
        $('.toast').toast('show');
    }

}



function OyeniAd(OzellikId) {


    var OzellikAd = $('#input-ozellikad-' + OzellikId).val();
    console.log(OzellikAd.toString());


    if (OzellikAd.trim() != "") {
        $.ajax({
            url: '/YemekKatalog/OzellikDuzenle/',
            data: { OzellikId: OzellikId, OzellikAd: OzellikAd },
            type: 'POST',
            success: function (result) {
                var $htmlContent = $(result);

             

                var viewbagContent = $htmlContent.find('#ozellikler');

                var deneme = document.getElementById("ozellikler");

                $('#ozelliklerParent').html(viewbagContent);
                OzellikAd.value = "";


                var ozellikler = $htmlContent.find('#demo2 li');

                $('#demo2').html(ozellikler);

      



             



            },
            error: function (error) {
                console.error(error);
            }





        });

    } else {
        document.getElementById("deneme").innerHTML = "Ozellik adı boş olamaz";
        $('.toast').toast('show');
    }

}
$(document).on('click', 'a[data-toggle="collapse"]', function () {

    var currentTarget = $(this).attr('href');
    $('[id^=toggle-form-]').not(currentTarget).collapse('hide');
    $('[id^=toggle-form-]').not(currentTarget).removeClass('show');
});

