

$(function () {
    //transaction index change color of transactiontype
    $('#transaction tbody tr td:nth-child(3)').each(function () {
        var data = $(this).text();
        if (data == 'Income') {
            $(this).find('span').addClass('income');
        }
        else {
            $(this).find('span').addClass('expense');
        }
    });
});


//delete functionality
displayAlert = (urlParam, isCatagory) => {
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this! " + isCatagory,
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                type: 'DELETE',
                url: urlParam,
                success: function (result) {
                    if (result.success == true) {
                        toastr.success(result.message, 'Alert')
                        window.setTimeout(function () {
                            location.reload();
                        }, 1000);
                    }
                    else {
                        toastr.error(result.message, 'Alert')
                    }
                }
            });
        }
    });
}

