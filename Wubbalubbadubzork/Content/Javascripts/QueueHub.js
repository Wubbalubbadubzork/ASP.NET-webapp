$(document).ready(function () {
    var queueHubProxy = $.connection.queueHub;

    queueHubProxy.client.Redirect = function (id) {
        window.location = "/Game/Details/" + id;
    }

    $.connection.hub.start().done(function () {
        console.log('Now connected');
    }).fail(function () {
        console.log('Did not work');
    })
});