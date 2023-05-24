$(document).ready(function () {
    var table = $('.data-table').DataTable({
        columnDefs: [{
            "searchable": false,
            "orderable": false,
            "targets": 0
        }],

        responsive: true,

        order: [[1, "asc"]],

        language: {
            info: "نمایش _START_ تا _END_ از تعداد _TOTAL_ داده",
            search: 'جست‌وجو:',
            processing: "در حال پردازش ...",
            loadingRecords: "در حال بارگزاری...",
            emptyTable: "داده‌ای برای نمایش وجود ندارد",
            lengthMenu: "نمایش _MENU_ داده در هر صفحه",
            infoEmpty: "بدون داده",
            infoFiltered: " - فیلتر شده از تعداد _MAX_ داده",
            zeroRecords: "داده‌ای یافت نشد.",
            paginate: {
                first: "ابتدا",
                previous: "قبل",
                next: "بعد",
                last: "انتها"
            },
            aria: {
                sortAscending: ": مرتب‌سازی صعودی",
                sortDescending: ": مرتب‌سازی نزولی"
            }
        },
    });

    table.on('order.dt search.dt', function () {
        table.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
            cell.innerHTML = i + 1;
        });
    }).draw();
});
