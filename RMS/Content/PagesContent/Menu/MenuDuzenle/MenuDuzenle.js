
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




    $('#yemekSelect').change(function () {
        // Seçilen katalog ID'sini al

        if ($(this).val() == "Yemek Ekle") {
            $("#kt_modal_stacked_1").modal('show');
            document.getElementById("yemekSelect").selectedIndex = 0;
        } else {
            var selectedYemekId = $(this).val();
            console.log(selectedYemekId);
            // AJAX isteğiyle sunucuya seçilen katalog ID'si ile filtreleme yapılacak
            $.ajax({
                type: 'POST',
                url: '/Menu/Filtrele', // ControllerAdi, kendi Controller adınıza uygun olarak değiştirilmelidir
                data: { YemekId: selectedYemekId },
                success: function (data) {
                    // Gelen veriyi kullanarak sayfa içeriğini güncelle
                    $('#demo1').empty(); // Önceki içeriği temizle
                    console.log('Filtreleme başarılı');
                    for (var key in data) {
                        if (data.hasOwnProperty(key)) {
                            var KatalogAdi = key;
                            var KatalogId = data[key];
                            $('#demo1').append('<li class="list-group-item" data-item-id="' + KatalogId + '">' + KatalogAdi + '</li>');
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
    onAdd: function (evt) {
        console.log(evt.item);
        evt.item.querySelector(".custom-switch").remove();
        var child = evt.item.children[0];
        child.classList.add("p-0");
    },
    onEnd: function (evt) {
        var katalogid = evt.item.dataset.itemId; // Sürüklenen öğenin benzersiz kimliği
        var Yemekid = document.getElementById('yemekSelect').value;

        // Eğer öğe çöp kutusuna sürüklenip bırakıldıysa
        if (evt.to.id === 'trash') {
            console.log('Çöp kutusuna sürüklendi. Silme işlemi yapılacak.');
            $.ajax({
                type: 'POST',
                url: '/Menu/KatalogKaldir', // Silme işlemini gerçekleştirecek URL
                data: { Yemekid: Yemekid, KatalogId: katalogid },
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
        var KatalogId = evt.item.dataset.itemId; // Sürüklenen öğenin benzersiz kimliği
        var YemekId = document.getElementById('yemekSelect').value;


        // Eğer öğe demo1 e sürüklenip bırakıldıysa
        if (evt.to.id === 'demo1') {
            console.log('Ekleme işlemi yapılacak.');
            $.ajax({
                type: 'POST',
                url: '/Menu/KatalogEkle', // Ekleme işlemini gerçekleştirecek URL
                data: { YemekId: YemekId, KatalogId: KatalogId },
                success: function (result) {
                    console.log(result);
                    var deneme = document.getElementById("deneme");
                    deneme.innerHTML = result;
                    var a = deneme.querySelector('#viewbag');
                    console.log(a);



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
                    //setTimeout(function () {
                    //    $('.toast').toast('hide');
                    //}, 2000);

                },
                error: function () {
                    // Veritabanından ekleme sırasında bir hata oluştu
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





function SilKatalog(KatalogId) {


    var id = parseInt(KatalogId)
    $.ajax({
        url: '/Menu/KatalogDelete',
        data: { KatalogId: KatalogId },
        type: 'POST',
        success: function (result) {
            var $htmlContent = $(result);

            // Sadece istediğiniz öğeyi seçin
            var viewbagContent = $htmlContent.find('#kataloglar');


            $('#kataloglarParent').html(viewbagContent);






            var kataloglar = $htmlContent.find('#demo2 li');



            $('#demo2').html(kataloglar);

            $('#kt_modal_stacked_4').modal('hide');




        },
        error: function (error) {
            console.error(error);
        }
    });



}


function YemekEkle() {


    $('#kt_modal_stacked_1_01').modal('show');


}

function Sil(YemekId) {

    $.ajax({
        url: '/Menu/YemekSil',
        data: { YemekId: YemekId },
        type: 'POST',
        success: function (result) {
            var $htmlContent = $(result);





            var katalog = $htmlContent.find('#yemekSelect option');

            $('#yemekSelect').html(katalog);

            document.getElementById("yemekSelect").selectedIndex = 0;
            $('#demo1').empty();

            $('#kt_modal_stacked_2').modal('hide');


            var modal = $htmlContent.find('#Kategori option');
            $('#fKategori').html(modal);

            $('#Kategori').html(modal.clone());

            var yemekler = $htmlContent.find('#YemeklerParent');

            $('#nfkategori').val('');
            $('#new-food-name').val('');
            $('#new-food-price').val('');
            $('#fKategori').prop('selectedIndex', 0);
            $('#fKategori').trigger('change');

            $('#YemeklerParent').html(yemekler);



        },
        error: function (error) {
            console.error(error);
        }





    });
}

$(document).on('click', '.close-toast', function () {
    $(".toast").toast('hide');

});
$(document).on('click', '#modal-yemek-ekle-onay', function () {
    var YemekAd = $('#new-food-name').val();
    var YemekFiyat = $('#new-food-price').val();
    var YemekKategori = $('#new-food-form [name="YemekKategori"]').val();
    var file = $('#foodimgnew')[0].files[0];


    // FormData nesnesi oluştur
    var formData = new FormData();

    formData.append('YemekAd', YemekAd);
    formData.append('YemekFiyat', YemekFiyat);
    formData.append('YemekKategori', YemekKategori);
    formData.append('YemekResim', file);




    if (YemekAd == '') {
        console.log('Yemek adı boş');
    } else if (YemekFiyat == '') {
        console.log('Yemek fiyat boş');
    } else {
        $.ajax({
            url: '/Menu/YemekCreate',
            type: 'POST',
            data: formData,
            contentType: false,
            processData: false,

            success: function (result) {
                var $htmlContent = $(result);





                var katalog = $htmlContent.find('#yemekSelect option');

                $('#yemekSelect').html(katalog);

                document.getElementById("yemekSelect").selectedIndex = 0;
                $('#demo1').empty();

                $('#kt_modal_stacked_1_01').modal('hide');


                var option1 = $htmlContent.find('#Kategori option');
                var option2 = $htmlContent.find('#fKategori option');


                $('#fKategori').html(option2);

                $('#Kategori').html(option1);

                var yemekler = $htmlContent.find('#YemeklerParent');

                $('#nfkategori').val('');
                $('#new-food-name').val('');
                $('#new-food-price').val('');
                $('#fKategori').prop('selectedIndex', 0);
                $('#fKategori').trigger('change');

                $('#YemeklerParent').html(yemekler);

                var fileinputNew = '<input type="file" class="custom-file-input" id="foodimgnew" accept=".jpg,.jpeg,.png" aria-describedby="foodimg" onchange="dosyaOnizlemeNew(this);"><label class="custom-file-label" style = "text-align:start;" for= "inputGroupFile04" data-browse= "Yükle" > Resim Seç</label> ';
                $('.custom-file-new').html(fileinputNew);






            },
            error: function (error) {
                console.error(error);
            }





        });



    }


});



$(document).ready(function () {
    $('#YemekEkleForm').submit(function (e) {
        e.preventDefault();
        var YemekAd = $('#new-food-name').val();
        var YemekFiyat = $('#new-food-price').val();
        var YemekKategori = $('#new-food-form [name="YemekKategori"]').val();
        var YemekResim = $('#foodimgnew')[0].files[0];


        // FormData nesnesi oluştur
        var formData = new FormData();

        formData.append('YemekAd', YemekAd);
        formData.append('YemekFiyat', YemekFiyat);
        formData.append('YemekKategori', YemekKategori);
        formData.append('YemekResim', YemekResim);

        console.log(YemekAd + YemekFiyat + YemekKategori);


        if ($('#YemekEkleForm').valid()) {

            console.log("valid");
            $.ajax({
                url: '/Menu/YemekCreate/',
                type: 'POST',
                data: formData,
                contentType: false,
                processData: false,
                success: function (result) {
                    var $htmlContent = $(result);





                    var katalog = $htmlContent.find('#yemekSelect option');

                    $('#yemekSelect').html(katalog);

                    document.getElementById("yemekSelect").selectedIndex = 0;
                    $('#demo1').empty();

                    $('#kt_modal_stacked_1_01').modal('hide');


                    var option1 = $htmlContent.find('#Kategori option');
                    var option2 = $htmlContent.find('#fKategori option');


                    $('#fKategori').html(option2);

                    $('#Kategori').html(option1);

                    var yemekler = $htmlContent.find('#YemeklerParent');

                    $('#nfkategori').val('');
                    $('#new-food-name').val('');
                    $('#new-food-price').val('');
                    $('#fKategori').prop('selectedIndex', 0);
                    $('#fKategori').trigger('change');

                    $('#YemeklerParent').html(yemekler);

                    var fileinputNew = '<input type="file" class="custom-file-input" id="foodimgnew" accept=".jpg,.jpeg,.png" aria-describedby="foodimg" onchange="dosyaOnizlemeNew(this);"><label class="custom-file-label" style = "text-align:start;" for= "inputGroupFile04" data-browse= "Yükle" > Resim Seç</label> ';
                    $('.custom-file-new').html(fileinputNew);




                    $('.img-box div:last').remove();
                    $('.img-box div:last').remove();
                    $('.img-box-new div:last').remove();
                    $('.img-box-new').html('<div ><i class="col p-0 fa-solid fa-utensils"></i></div>');




                },
                error: function (error) {
                    console.error(error);
                }





            });
        }



    });

});




$(document).on('click', '#modal-yemek-duzenle-onay', function () {
    var YemekAd = $('#food-name').val();
    var YemekFiyat = $('#food-price').val();

    var YemekKategori = $('[name="YemekKategori"]').val();
    var YemekId = $('#modal-yemek-duzenle-onay').attr('yemekid');
    var file = $('#foodimg')[0].files[0];

    console.log('aasdasd');
    console.log(YemekAd + YemekFiyat + YemekKategori + YemekId);

    // FormData nesnesi oluştur
    var formData = new FormData();
    formData.append('YemekId', YemekId);
    formData.append('YemekAd', YemekAd);
    formData.append('YemekFiyat', YemekFiyat);
    formData.append('YemekKategori', YemekKategori);
    formData.append('YemekResim', file);




    if (YemekAd == '') {
        console.log('Yemek adı boş');
    } else if (YemekFiyat == '') {
        console.log('Yemek fiyat boş');
    } else {
        $.ajax({
            url: '/Menu/YemekDuzenle/',
            type: 'POST',
            data: formData,
            contentType: false,
            processData: false,
            success: function (result) {
                var $htmlContent = $(result);





                var katalog = $htmlContent.find('#yemekSelect option');

                $('#yemekSelect').html(katalog);

                document.getElementById("yemekSelect").selectedIndex = 0;
                $('#demo1').empty();

                $('#kt_modal_stacked_1_1').modal('hide');


                var modal = $htmlContent.find('#Kategori option');
                $('#Kategori').html(modal);
                var yemekler = $htmlContent.find('#YemeklerParent');

                $('#nkategori').val('');

                $('#YemeklerParent').html(yemekler);



            },
            error: function (error) {
                console.error(error);
            }





        });



    }


});


$(document).ready(function () {
    $('#MenuDuzenleForm').submit(function (e) {
        e.preventDefault();
        var YemekAd = $('#food-name').val();
        var YemekFiyat = $('#food-price').val();

        var YemekKategori = $('[name="YemekKategori"]').val();
        var YemekId = $('#MenuDuzenleForm').attr('yemekid');
        var file = $('#foodimg')[0].files[0];

        console.log('aasdasd');
        console.log(YemekAd + YemekFiyat + YemekKategori + YemekId);

        // FormData nesnesi oluştur
        var formData = new FormData();
        formData.append('YemekId', YemekId);
        formData.append('YemekAd', YemekAd);
        formData.append('YemekFiyat', YemekFiyat);
        formData.append('YemekKategori', YemekKategori);
        formData.append('YemekResim', file);

        if ($('#MenuDuzenleForm').valid()) {

            console.log("valid");
            $.ajax({
                url: '/Menu/YemekDuzenle/',
                type: 'POST',
                data: formData,
                contentType: false,
                processData: false,
                success: function (result) {
                    var $htmlContent = $(result);





                    var katalog = $htmlContent.find('#yemekSelect option');

                    $('#yemekSelect').html(katalog);

                    document.getElementById("yemekSelect").selectedIndex = 0;
                    $('#demo1').empty();

                    $('#kt_modal_stacked_1_1').modal('hide');


                    var modal = $htmlContent.find('#Kategori option');
                    $('#Kategori').html(modal);
                    var yemekler = $htmlContent.find('#YemeklerParent');

                    $('#nkategori').val('');

                    $('#YemeklerParent').html(yemekler);



                },
                error: function (error) {
                    console.error(error);
                }





            });
        }



    });

});




function ResimKaldır(YemekId) {



    $.ajax({
        url: '/Menu/ResimKaldır/',
        data: { YemekId: YemekId },
        type: 'POST',
        success: function (result) {
            var htmlContent = $(result);
            $(".img-box").html('<i class="col p-0 fa-solid fa-utensils"></i>');
            $(".custom-file").html('<input type="file" class="custom-file-input" id="foodimg" accept=".jpg,.jpeg,.png" aria-describedby="foodimg" onchange="dosyaOnizleme(this);"><label class="custom-file-label" style="text-align:start;" for="inputGroupFile04" data-browse="Yükle">Resim Seç</label>');



            var modalinside = htmlContent.find('#kt_modal_stacked_1_child');
            console.log(modalinside);
            $('#kt_modal_stacked_1').html(modalinside);





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
        url: '/Menu/OzellikCreate',
        data: { OzellikAd: OzellikAd.value },
        type: 'POST',
        success: function (result) {
            var $htmlContent = $(result);

            // Sadece istediğiniz öğeyi seçin
            var viewbagContent = $htmlContent.find('#özellikler');

            var deneme = document.getElementById("özellikler");

            $('#özelliklerParent').html(viewbagContent);
            KatalogAd.value = "";


            var katalog = $htmlContent.find('#özelliklerSelect option');

            $('#özelliklerSelect').html(katalog);


        },
        error: function (error) {
            console.error(error);
        }
    });


}


function KatalogEditStart(KatalogId) {

    var $katalog = $('#demo2 li[data-item-id="' + KatalogId + '"] span');
    $katalog.empty();
    $katalog.append('<div class="row w-100 justify-content-center mt-4" id="ozellik-parent-' + KatalogId + '"> <input class="form-control col-8 mr-2" type="text" id="input-' + KatalogId + '" value="" /> <a  class=" input-ok col-2 mr-2 d-flex align-items-center text-decoration-none" id="input-ok-' + KatalogId + '"><i class="fa-regular fa-paper-plane"></i></a> \n <div class="col-1 p-0 float-right d-flex flex-row-reverse align-items-center"> <a class="  input-exit  close " id="input-exit-' + KatalogId + '">&times;</a></div> </div>');



}



$(document).on('click', '.input-ok', function () {
    // btnok butonuna tıklandığında yapılacak işlemler buraya gelecek
    var KatalogId = $(this).attr('id').split("-")[2];
    console.log(KatalogId);
    var KatalogAd = $('#input-' + KatalogId).val();
    console.log(KatalogAd.toString());


    if (KatalogAd != "") {

        $.ajax({
            url: '/Menu/KatalogDuzenle/',
            data: { KatalogId: KatalogId, KatalogAd: KatalogAd },
            type: 'POST',
            success: function (result) {
                var $htmlContent = $(result);


                var viewbagContent = $htmlContent.find('#demo2 li');



                $('#demo2').html(viewbagContent);

                document.getElementById("yemekSelect").selectedIndex = 0;
                $('#demo1').empty();


                var ozelliklermodal = $htmlContent.find('#ozellikler');



                $('#ozelliklerParent').html(ozelliklermodal);

                var viewbagContent = $htmlContent.find('#kataloglar');



                $('#kataloglarParent').html(viewbagContent);
                KatalogAd.value = "";


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





function YyeniAd(KatalogId) {


    var KatalogAd = $('#input-yemekad-' + KatalogId).val();
    console.log(KatalogAd.toString());


    if (KatalogAd != "") {

        $.ajax({
            url: '/YemekKatalog/KatalogDuzenle/',
            data: { KatalogId: KatalogId, KatalogAd: KatalogAd },
            type: 'POST',
            success: function (result) {
                var $htmlContent = $(result);

                // Sadece istediğiniz öğeyi seçin
                var viewbagContent = $htmlContent.find('#kataloglar');

                var deneme = document.getElementById("kataloglar");

                $('#YemeklerParent').html(viewbagContent);
                KatalogAd.value = "";


                var katalog = $htmlContent.find('#yemekSelect option');

                $('#yemekSelect').html(katalog);

                document.getElementById("yemekSelect").selectedIndex = 0;
                $('#demo1').empty();


            },
            error: function (error) {
                console.error(error);
            }





        });

    }

}




function KyeniAd(KatalogId) {


    var KatalogAd = $('#input-katalogad-' + KatalogId).val();
    console.log(KatalogAd.toString());


    if (KatalogAd.trim() != "") {

        $.ajax({
            url: '/Menu/KatalogDuzenle/',
            data: { KatalogId: KatalogId, KatalogAd: KatalogAd },
            type: 'POST',
            success: function (result) {
                var $htmlContent = $(result);

                // Sadece istediğiniz öğeyi seçin
                var viewbagContent = $htmlContent.find('#kataloglar');

                var deneme = document.getElementById("kataloglar");

                $('#kataloglarParent').html(viewbagContent);
                KatalogAd.value = "";

                var kataloglar = $htmlContent.find('#demo2 li');

                $('#demo2').html(kataloglar);





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


function switchChanged(checkbox, KatalogId) {

    console.log();
    if (checkbox.checked) {
        // Checkbox işaretliyse yapılacak işlemi burada gerçekleştirin
        console.log("Checkbox işaretli");
    } else {
        // Checkbox işaretli değilse yapılacak işlemi burada gerçekleştirin
        console.log("Checkbox işaretli değil");
    }


    $.ajax({
        url: '/Menu/KatalogMultiChange/',
        data: { KatalogId: KatalogId, Multi: checkbox.checked },
        type: 'POST',
        success: function (result) {
            var $htmlContent = $(result);


            var viewbagContent = $htmlContent.find('#kataloglar');



            var kataloglar = $htmlContent.find('#demo2 li');

            $('#demo2').html(kataloglar);





        },
        error: function (error) {
            console.error(error);
        }





    });



}


$(document).on('click', 'a[data-toggle="collapse"]', function () {

    var currentTarget = $(this).attr('href');
    $('[id^=toggle-form-]').not(currentTarget).collapse('hide');
    $('[id^=toggle-form-]').not(currentTarget).removeClass('show');
});

$(document).ready(function () {
    $('#KatalogEkleForm').submit(function (e) {
        e.preventDefault();
        var KatalogAd = document.getElementById("KatalogAd");

        if ($('#KatalogEkleForm').valid()) {
            $.ajax({
                url: '/Menu/KatalogCreate',
                data: { KatalogAd: KatalogAd.value },
                type: 'POST',
                success: function (result) {

                    console.log("create");
                    var $htmlContent = $(result);
                    var dm2 = $htmlContent.find('#demo2 li');
                    $('#demo2').html(dm2);
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
