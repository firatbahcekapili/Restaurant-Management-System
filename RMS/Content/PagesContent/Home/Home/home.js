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
            secilenMasalar[j].style.display = "block";
        }
    } else {
        // Eğer "Tüm Konumlar" seçildiyse tüm masaları göster
        for (var k = 0; k < masalar.length; k++) {
            masalar[k].style.display = "block";
        }
    }
}


document.addEventListener('DOMContentLoaded', function () {
    var buttons = document.querySelectorAll('.card.btn');

    buttons.forEach(function (button) {
        button.addEventListener('click', function (event) {
            var cardId = event.currentTarget.id;

            window.location.href = '/Home/Masa/' + cardId;
        });


        var dropdown = button.querySelector('.form-control');
        dropdown.addEventListener('click', function (event) {
            event.stopPropagation();
        });
    });
});





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

            if (result != yeniDurum) {
                alert("Masada aktif siparişler olduğu için masa durumu değiştirilememektedir.");
            }


            Filtrele();
        },
        error: function (error) {
            console.error(error);
        }
    });
}