var datatable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    datatable = $("#dtload").DataTable({
        "ajax": {
            "url": "/books/getall",
            "type": "GET",
            "datatype": "json",
        },
        "columns": [
            { "data": "name", "width": "20%" },
            { "data": "author", "width": "20%" },
            { "data": "isbn", "width": "20%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="text-center">
                            <a href="/books/upsert?id=${data}" class="btn btn-success text-white" style="cursor:pointer;width: 70px;">Edit</a>
                            &nbsp;
                            <a onclick=Delete('/books/delete?id='+${data}) class="btn btn-danger text-white" style="cursor:pointer;width: 70px;">Delete</a>
                        <div>
                    `;
                }, "width": "40%"
            }
        ],
        "language": {
            "emptyTable": "no data found"
        },
        "width": "100%"
    });
}

function Delete(url) {
    swal({
        title: "Are you sure?",
        text: "Once deleted, you will not be able to recover it.",
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
                        datatable.ajax.reload()
                    } else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    })
}