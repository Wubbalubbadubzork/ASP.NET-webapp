$(document).ready(function () {
    var createHubProxy = $.connection.createHub;
    createHubProxy.client.UpdateStrings = function () {
        var j = 3;
        var i = parseInt($("#count").html().trim(), 10);
        var z = 1;

        while (z <= j) {
            if (z <= i) {
                $("#waiting_" + z).html("Ready!");
            }
            else {
                $("#waiting_" + z).html("Waiting...");
            }
            z++;
        }
    }

    createHubProxy.client.CurrCount = function (count) {
        $("#count").html("" + count)
    }

    $("#cancel").click(function () {
        createHubProxy.server.leaveGame($("#gameId").html().trim());
        window.location = "/Game/Index"
    })

    $.connection.hub.start().done(function () {
        console.log('Now connected');
        createHubProxy.server.joinGame($("#gameId").html().trim());
    
    }).fail(function () {
        console.log('Did not work');
        })


})