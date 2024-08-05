

document.querySelectorAll('.collapsebtn').forEach(collapseBtn => {
    collapseBtn.addEventListener('click', function () {
        console.log("asdada");
        var target = document.querySelector(collapseBtn.getAttribute('href'));
        console.log(target);
        // Tıklanan collapse haricindeki tüm açık collapse'leri kapat
        document.querySelectorAll('.collapse.show').forEach(shownCollapse => {
            if (shownCollapse !== target) {
                shownCollapse.classList.remove('show');
            } else {
                if (target.classList.contains('show')) {
                    setTimeout(function () {
                        target.classList.remove('show');
                    }, 100);
                }
            }

        });


    });
});




$(document).ready(function () {
    $('#personel').DataTable({
        order: [[2, 'desc']],
        responsive: true,

    });
});


// Collapse dışında bir yere tıklanınca collapse'ları kapat
document.addEventListener('click', function (event) {
    if (!event.target.closest('.collapsebtn') && !event.target.closest('.collapse')) {
        document.querySelectorAll('.collapse.show').forEach(shownCollapse => {
            shownCollapse.classList.remove('show');
        });
    }
});


const ctx = document.getElementById('myChart');
var data1 = document.getElementById('polarChartData-1');
var data2 = document.getElementById('polarChartData-2');
var data3 = document.getElementById('polarChartData-3');
var data4 = document.getElementById('polarChartData-4');
var data5 = document.getElementById('polarChartData-5');

const data = {
    labels: [
        data1.getAttribute('text'),
        data2.getAttribute('text'),
        data3.getAttribute('text'),
        data4.getAttribute('text'),
        data5.getAttribute('text')
    ],

    datasets: [{
        label: 'Satış Adeti',
        data: [data1.getAttribute('value'), data2.getAttribute('value'), data3.getAttribute('value'), data4.getAttribute('value'), data5.getAttribute('value')],
        backgroundColor: [
            'rgb(255, 99, 132)',
            'rgb(75, 192, 192)',
            'rgb(255, 205, 86)',
            'rgb(201, 203, 207)',
            'rgb(54, 162, 235)'
        ]
    }]
};
new Chart(ctx, {
    type: 'polarArea',
    data: data,
    options: {
        maintainAspectRatio: false,
        scales: {

        }
    }
});

//$(document).ready(function () {
//    $.fn.dataTable.moment('DD/MM/YY HH.mm.ss');
//    var table = $('#siparisTable').DataTable({
//        responsive: true,
//        autoFill: true,
//        dom: 'Qlfrtip',

//        columnDefs: [{
//            targets: 4,
//            /* 			render: $.fn.dataTable.render.moment( 'DD/MM/YY' ) */
//            render: function (data) {
//                return moment(data, 'DD.MM.YYYY HH:mm:ss').format('DD/MM/YY HH.mm.ss');
//            }
//        }],

//        language: {
//            url: 'https://cdn.datatables.net/plug-ins/1.10.24/i18n/tr.json',
//        },

//    });
//});


//$(document).ready(function () {
//    $.fn.dataTable.moment('DD/MM/YY HH.mm.ss');
//    var table = $('#siparisTable').DataTable({
//        responsive: true,
//        autoFill: true,
//        dom: 'Qlfrtip',

//        columnDefs: [{
//            targets: 4,
//            /* 			render: $.fn.dataTable.render.moment( 'DD/MM/YY' ) */
//            render: function (data) {
//                return moment(data, 'DD.MM.YYYY HH:mm:ss').format('DD/MM/YY HH.mm.ss');
//            }
//        }],

//        language: {
//            url: 'https://cdn.datatables.net/plug-ins/1.10.24/i18n/tr.json',
//        },

//    });
//});



$(document).ready(function () {

    $.fn.dataTable.moment('DD/MM/YY HH.mm');
    var table = $('#siparisTable').DataTable({
        responsive: true,
        autoFill: true,
        dom: 'Qlfrtip',


        columnDefs: [{
            targets: 4,

            render: function (data) {
                return moment(data, 'DD.MM.YYYY HH:mm:ss').format('DD/MM/YY HH.mm');
            }
        }],
        order: [[1, 'desc']],

        rowGroup: {
            dataSrc: 1

        },
        language: {
            url: '//cdn.datatables.net/plug-ins/1.13.7/i18n/tr.json',
        },


    });



});





//$(document).ready(function () {
//    var collapsedGroups = {};

//    $.fn.dataTable.moment('DD/MM/YY HH.mm');

//    var table = $('#siparisTable').DataTable({
//        layout: {
//            topStart: {
//                buttons: ['searchBuilder']
//            }
//        },
//        responsive: true,
//        autoFill: true,
//        dom: 'Qlfrtip',
//        order: [[4, 'desc']],
//        columnDefs: [{
//            targets: 4,
//            render: function (data) {
//                return moment(data, 'DD.MM.YYYY HH:mm:ss').format('DD/MM/YY HH.mm');
//            }
//        }],
//        rowGroup: {
//            // Uses the 'row group' plugin
//            dataSrc: 1,
//            startRender: function (rows, group) {
//                var collapsed = !!collapsedGroups[group];

//                rows.nodes().each(function (r) {
//                    r.style.display = collapsed ? 'none' : '';
//                });

//                // Add category name to the <tr>. NOTE: Hardcoded colspan
//                return $('<tr/>')
//                    .append('<td colspan="8">' + group + ' (' + rows.count() + ')</td>')
//                    .attr('data-name', group)
//                    .toggleClass('collapsed', collapsed);
//            }
//        },
//        language: {
//            url: '//cdn.datatables.net/plug-ins/1.13.7/i18n/tr.json',
//        },
//        searchBuilder: {
//            dateTime: true
//        }
//    });

//    // Hata yakalama
//    try {
//        $.fn.dataTable.moment('DD/MM/YY HH.mm');
//    } catch (error) {
//        console.error("moment.js hatası:", error);
//    }




//    // Tüm grupları kapalı olarak ayarla
//    $('#siparisTable tbody tr.group-start').each(function () {
//        var name = $(this).data('name');
//        collapsedGroups[name] = true; // Tüm gruplar kapalı olsun
//        // Başlangıçta grupları gizle
//        $(this).nextUntil('tr.group-start').hide();
//    });

//    table.draw(false); // Tabloyu yeniden çiz

//    // Grup başlıklarına tıklandığında grupların açılması ve kapanması
//    $('#siparisTable tbody').on('click', 'tr.group-start', function () {
//        var name = $(this).data('name');

//        // Tıklanan grup açıksa, kapat; kapalıysa, aç
//        collapsedGroups[name] = !collapsedGroups[name];

//        // Tüm grupları yeniden çiz
//        table.draw(false);
//    });



//});




//$(document).ready(function () {
//    var collapsedGroups = {};
//    $.fn.dataTable.moment('DD/MM/YY HH.mm');
//    var table = $('#siparisTable').DataTable({
//        responsive: true,
//        autoFill: true,
//        dom: 'Qlfrtip',


//        columnDefs: [{
//            targets: 4,

//            render: function (data) {
//                return moment(data, 'DD.MM.YYYY HH:mm:ss').format('DD/MM/YY HH.mm');
//            }
//        }],
//        order: [[1, 'asc']],
//        rowGroup: {
//            dataSrc: 1,
//            startRender: function (rows, group) {
//                var collapsed = !!collapsedGroups[group];

//                rows.nodes().each(function (r) {
//                    r.style.display = 'none';
//                    if (collapsed) {
//                        r.style.display = '';
//                    }
//                });

//                // Add category name to the <tr>. NOTE: Hardcoded colspan
//                return $('<tr/>')
//                    .append('<td colspan="8">' + group + ' (' + rows.count() + ')</td>')
//                    .attr('data-name', group)
//                    .toggleClass('collapsed', collapsed);
//            }

//        },
//        language: {
//            url: '//cdn.datatables.net/plug-ins/1.13.7/i18n/tr.json',
//        },

//    });

//    $('#siparisTable tbody').on('click', 'tr.group-start', function () {
//        var name = $(this).data('name');
//        collapsedGroups[name] = !collapsedGroups[name];
//        table.draw(false);
//    });

//});







//$(document).ready(function () {
//    $.fn.dataTable.moment('DD.MM.YY HH:MM:SS');
//    $('#siparisTable').DataTable({
//        responsive: true,

//        columns: [
//            null, // Otomatik tip algıla
//            null,
//            null,// Otomatik tip algıla
//            null,
//            { type: 'date' }, // Tarih tipi
//            null, // Otomatik tip algıla
//        ],
//        buttons: [{
//            extend: 'searchBuilder'
//        }],

//        language: {
//            url: '//cdn.datatables.net/plug-ins/1.13.7/i18n/tr.json',


//        },
//        dom: 'Qlfrtip',
//        columnDefs: [{
//            targets: 4, render: function (data) {
//                return moment(data).format('DD.MM.YY HH:MM:SS');
//            }
//        }],
//    });


//});
