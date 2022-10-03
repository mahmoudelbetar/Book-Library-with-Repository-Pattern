
﻿let datatables;


$(document).ready(function () {
    var url = window.location.search;
    var stat = url.split("=");
    stat = stat[1];
    switch (stat) {
        case "pending":
            loadDataTable("pending");
            break;
        case "inprocess":
            loadDataTable("inprocess");
            break;
        case "completed":
            loadDataTable("completed");
            break;
        default:
            loadDataTable("all");
    }
    
});

function loadDataTable(status) {
    datatables = $('#tblData').DataTable({
        "ajax": {
            "url": `/Admin/Order/GetAll?status=${status}`
        },
        "columns": [
            { "data": "id", "width": "15%" },
            { "data": "name", "width": "15%" },
            { "data": "phoneNumber", "width": "15%" },
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

