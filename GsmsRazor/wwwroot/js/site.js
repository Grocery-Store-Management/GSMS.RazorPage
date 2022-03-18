$(() => {

    var connection = new signalR.HubConnectionBuilder().withUrl("/SignalRHub").build();
    connection.start();
    LoadNotesData();

    connection.on("reloadNotes", function () {
        LoadNotesData();
    })

    connection.on("reloadPage", function () {
        LoadIndex();
    })


    function LoadNotesData() {
        $('#btnShow').prop('disabled', true);
        $("#btnPost").prop('disabled', true);
        var li = '';
        $.ajax({
            url: `/Dashboard?handler=LoadNotes`,
            method: "GET",
            success: (result) => {
                $.each(result, (k, v) => {
                    li += `<li class="list-group-item col-col-md-6">
                    <form class="row" onsubmit="return deleteNote(event)">
                        <input type="hidden" value=${v.id} />
                        <div class="col col-md-8">
                            ${v.senderName} says: ${v.content}
                        </div>
                        <input type="submit" value="Cancel" class="btn btn-danger col col-md-3 ml-4" />
                    </form>
                </li>`
                })
                $("#dashboard").html(li);
                $('#btnShow').prop('disabled', false);
                $("#btnPost").prop('disabled', false);
            }, error: (error) => {
                console.log(error)
            }
        });
    }

    function LoadIndex() {
        toastr.options.showEasing = 'swing';
        toastr.success('Points updated!', "Success");

        setTimeout(() => {
            $("#btnClosePoints").click();
        }, 1500);
    }

})






