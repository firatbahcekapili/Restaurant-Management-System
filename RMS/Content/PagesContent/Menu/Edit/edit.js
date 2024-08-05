document.addEventListener("DOMContentLoaded", function () {

    var kId = "@Model.KatalogIdler";
    var kIdArray = kId.split(',');

    var chckbx = document.querySelectorAll(".katalogcheck");

    for (var i = 0; i < chckbx.length; i++) {
        var id = chckbx[i].id;

        if (kIdArray.includes(id)) {
            chckbx[i].checked = true;

        }
    }
});

$(document).ready(function () {
    $(".dropKategori").change(function () {
        if ($(this).val() == "Yeni Kategori Ekle") {
            $("#newKategoriInput").slideDown("fast");
            var dropdown = document.getElementById("Kategori");
            dropdown.setAttribute("name", "K");

            var newkategori = document.getElementById("nkategori");
            newkategori.setAttribute("name", "YemekKategori");
        } else {
            $("#newKategoriInput").slideUp("fast");
            var newkategori = document.getElementById("nkategori");
            newkategori.setAttribute("name", "nkategori");

            var dropdown = document.getElementById("Kategori");
            dropdown.setAttribute("name", "YemekKategori");
        }
    });
});

function Gonder() {
    // Seçilen checkbox'ların değerlerini al
    var selectedKataloglar = [];
    $(".katalogcheck:checked").each(function () {
        var id = $(this).attr("id");
        selectedKataloglar.push(id);
    });

    // Diğer gerekli verileri alabilirsiniz
    var YemekId = $("#YemekId").val();
    var yemekAd = $("#YemekAd").val(); // Örneğin YemekAd input alanından alındı
    var YemekKategori = $("#Kategori").val();
    var YemekFiyat = $("#YemekFiyat").val();
    var Kataloglar = selectedKataloglar.join(",");

    // Ajax isteğini oluştur
    $.ajax({
        url: "/Menu/Edit", // Controller ve Action isminizi uygun şekilde değiştirin
        type: "POST",
        data: { YemekId: parseInt(YemekId), YemekAd: yemekAd, YemekFiyat: parseInt(YemekFiyat), YemekKategori: YemekKategori, Kataloglar: Kataloglar },
        success: function (result) {


            window.location.href = "/admin/Yonetim";

            console.log(result);
        },
        error: function (error) {
            // Hata durumunda yapılacak işlemler
            console.log(error);
        }
    });
}
