let datatables;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    datatables = $('#tblData').DataTable({
        "ajax": {
            "url": "/Admin/Order/GetAll"
        },
        "columns": [
            { "data" : "id", "width" : "15%"},
            { "data": "name", "width": "15%"},
            { "data": "phoneNumber", "width": "15%"},
            { "data": "applicationUser.email", "width": "15%" },
            { "data": "orderStatus", "width": "15%" },
            { "data": "orderTotal", "width": "15%" },
            {
                "data": "id",
                "render": function (id) {
                    return `
                        <div class=" w-75 btn-group" role="group">
                            <a href="/Admin/Order/Details?orderId=${id}" class="btn btn-primary mx-2">
                                <i class="bi bi-pencil-square"></i>
                            </a>
                            
                        </div>
                    `;
                }
            }
        ]
    });
}

