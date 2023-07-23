var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData2').DataTable({
        "ajax": {
            "url": "/Penjualan/GetAll"
        },
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

                <button href="/Penjualan/Upsert/${data}" class="btn btn-success form-control"> <i class="bi bi-pencil-square"></i>Edit</button>

                        <button onclick=Delete("/Penjualan/Delete/${data}") class="btn btn-danger form-control" style="cursor:pointer; margin-left:10px;">
                        <i class="bi bi-trash3"></i>
                        Delete</button>
                     </div>
                    `;
                }, "width": "20%"
            }
        ]
    });
}

function Delete(url) {
    swal({
        title: "Are you sure you want to Delete?",
        text: "You will not be able to restore the data !",
        icon: "warning",
        buttons: true,
        dangerMode: true,
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: "DELETE",
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}

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

function Search() {
    var id = document.getElementById("cbMotor").value;
    //var merk = $("txtMerk").val();
    var merk = document.getElementById("txtMerk").value;
    var harga = $("txtHarga").val();
   
    // build json object
    var penjualan = {
        merk: merk,
        harga: harga
    };

    $.ajax({
        type: "GET",
        url: "/Penjualan/GetDataMotor/" + id,
        data: JSON.stringify(penjualan),
        success: function (data)
        {
            var data2 = JSON.stringify(data);

            var penjualan = JSON.parse(data2);

            $("#txtMerk").val(penjualan['data']['merk']);
            $("#txtHarga").val(penjualan['data']['harga']);

            console.log("Hello world!", penjualan);

        },
        error: function (data)
        {
          console.log(data)
        }	
    });

    
}

