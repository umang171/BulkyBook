console.log("call");
var dataTable;
$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $("#tblData").DataTable({
        "ajax": {
            "url": "/Admin/Product/GetAll"
        },
        "columns": [
            { "data": "title", "width": "15%" },
            { "data": "isbn", "width": "15%" },
            { "data": "price", "width": "15%" },
            { "data": "author", "width": "15%" },
            { "data": "category.name", "width": "15%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
         <a class="btn btn-primary" href="/Admin/Product/Upsert?id=${data}">
                            <i class="bi bi-pencil"></i> Edit
                        </a>
                        <a class="btn btn-danger" onClick=Delete('/Admin/Product/Delete?id=${data}')>
                            <i class="bi bi-trash-fill"></i>Delete
                        </a>
                        `
                },
                "width": "15%"
            },
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
                        console.log("gyu");
                        dataTable.ajax.reload();
                        Swal.fire(
                            'Deleted!',
                            data.message,
                            'success'
                        )
                    }
                }
            })

        }
    })
}