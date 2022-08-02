﻿
let datatables;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    datatables = $('#tblData').DataTable({
        "ajax": {
            "url": "/Admin/Product/GetDataTable"
        },
        "columns": [
            { "data" : "title", "width" : "15%"},
            { "data": "isbn", "width": "15%"},
            { "data": "price", "width": "15%"},
            { "data": "author", "width": "15%" },
            { "data": "category.name", "width": "15%" },
            {
                "data": "id",
                "render": function (id) {
                    return `
                        <div class=" w-75 btn-group" role="group">
                            <a href="/Admin/Product/Upsert?id=${id}" class="btn btn-primary mx-2">
                                <i class="bi bi-pencil-square"></i> Edit
                            </a>
                            <a href="#" onClick=Delete('/Admin/Product/Delete/${id}') class="btn btn-danger mx-2">
                                <i class="bi bi-trash"></i> Delete
                            </a>
                        </div>
                    `;
                }
            }
        ]
    });
}

function Delete(url) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: "DELETE",
                success: function (data) {
                    if (data.success) {
                        datatables.ajax.reload();
                        toastr.success(data.message);
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            })
        }
    })
}