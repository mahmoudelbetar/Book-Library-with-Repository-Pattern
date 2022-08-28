var datatables;

$(document).ready(function () {
    loadDatatables();
});

function loadDatatables() {
    $("#table").DataTable({
        "ajax": {
            "url": "/Admin/Company/GetAllCompanies"
        },
        "columns": [
            {"data":"name"},
            {"data":"city"},
            {"data":"phoneNumber"},
            {
                "data": "image",
                "render": function (image) {
                    return `<img src="data:image/jpg;base64,${image}" width="100px">`;
                }
            },
            {
                "data": "id",
                "render": function (id) {
                    return `
                        <div class=" w-75 btn-group" role="group">
                            <a href="/Admin/Company/Edit/${id}" class="btn btn-primary mx-2">
                                <i class="bi bi-pencil-square"></i> Edit
                            </a>
                            <a href="#" onClick=Remove("/Admin/Company/Delete/${id}") class="btn btn-danger mx-2">
                                <i class="bi bi-trash"></i> Delete
                            </a>
                        </div>
                            `;
                }
            }
        ]
    });
}

function Remove(url) {
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
                    if (data != "") {
                        window.location.reload();
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