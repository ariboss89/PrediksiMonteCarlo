var dataTable;

//$(document).ready(function () {
//    loadDataTable();
//});

function loadDataTable(data) {
    dataTable = $('#tblData2').DataTable({
        
        "columns": [
            { "data": "namaMotor", "width": "20%" },
            { "data": "merk", "width": "20%" },
            { "data": "harga", "width": "10%" },
            { "data": "jumlah", "width": "10%" },
            { "data": "tanggal", "width": "20%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                <div class="w-75 btn-group" role="group">
                        <a href="/Penjualan/Upsert/${data}" class="btn-success text-white"> <i class="bi bi-pencil-square"></i>Edit</a>

                        <a onclick=Delete("/Penjualan/Delete/${data}") class="btn-danger text-white" style="cursor:pointer; margin-left:10px;">
                        <i class="bi bi-trash3"></i>
                        Delete</a>
                     </div>
                    `;
                }, "width": "20%"
            }
        ]
    });
}

//function Delete(url) {
//    swal({
//        title: "Are you sure you want to Delete?",
//        text: "You will not be able to restore the data !",
//        icon: "warning",
//        buttons: true,
//        dangerMode: true,
//    }).then((willDelete) => {
//        if (willDelete) {
//            $.ajax({
//                type: "DELETE",
//                url: url,
//                success: function (data) {
//                    if (data.success) {
//                        toastr.success(data.message);
//                        dataTable.ajax.reload();
//                    }
//                    else {
//                        toastr.error(data.message);
//                    }
//                }
//            });
//        }
//    });
//}

//$.ajax({
//    type: "POST",
//    url: "/Penjualan/GetDataMotor",
//    data: { 'id': id },
//    success: function (data) {
//        if (data.success) {
//            toastr.success(data.message);

//        }
//        else {
//            toastr.error(data.message);
//        }
//    }
//});

function StartPrediksi() {
    var id = document.getElementById("cbMotor").value;
    var tglAwal = document.getElementById("tglAwal").value;
    var tglAkhir = document.getElementById("tglAkhir").value;
    //var penjualan = {
    //    id: id,
    //    namaMotor: namaMotor,
    //    merk: merk,
    //    harga: harga,
    //    jumlah: jumlah
    //};
   
    $.ajax({
        type: "GET",
        url: "/Prediksi/Index?Id=" + id + "&tglAwal=" + tglAwal + "&tglAkhir=" + tglAkhir,
        //data: JSON.stringify(penjualan),
        success: function (data) {
            console.log(data),
                loadDataTable(data)
        },
        error: function (data) {
            console.log(url);
        }
    });
}


