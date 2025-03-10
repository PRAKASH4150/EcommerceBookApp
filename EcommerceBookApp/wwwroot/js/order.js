﻿$(document).ready(function () {
    var url = window.location.search;
    var status = "";
    if (url.includes("inprocess")) {
        status = "inprocess";
    }
    else if (url.includes("pending")) {
        status ="pending"
    }
    else if (url.includes("completed")) {
        status = "completed"
    }
    else if (url.includes("approved")) {
        status = "approved"
    }
    else if (url.includes("all")) {
        status = "all"
    }
    loadDataTable(status);
})
function loadDataTable(status) {
    dataTable = $('#tblData').DataTable({
        "ajax": '/admin/order/getall?status=' + status,
        "columns": [
            { data: 'id',"width":"10%" },
            { data: 'name', "width": "15%" },
            { data: 'phoneNumber', "width": "15%" },
            { data: 'applicationUser.email', "width": "20%" },
            { data: 'orderStatus', "width": "10%" },
            { data: 'orderTotal', "width": "10%" },
            {
                data: 'id',
                "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">
                       <a href="/admin/order/details?id=${data}" class="btn btn-primary mx-2">
									<i class="bi bi-pencil-square"></i>
					   </a>
                    </div>`
                },
                "width": "10%"
            },
        ]
    });
}
