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
                        <div class="col col-md-6">
                            ${v.senderName} says: ${v.content}
                        </div>
                        <input type="submit" value="Cancel" class="btn btn-danger col col-md-3 ml-3" />
                    </form>
                </li>`
                })
                $("#dashboard").html(li);
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
            window.location.replace("/")
        }, 1500);
    }

})






