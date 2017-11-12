$(document).ready(function () {
    var queueHubProxy = $.connection.queueHub;

    queueHubProxy.client.Redirect = function (id) {
        //$.post('/Game/StartQueueGame', {
        //    Id: id
        //}).done(function (response) {
        //    console.log('%c' + response, 'background: #222; color: #bada55');
        //    if (response == "Successful") {
        //        window.location = "/Game/Details/" + id;
        //    }
        //});
        window.location = "/Game/Details/" + id;
    }

    $.connection.hub.start().done(function () {
        console.log('Now connected');
    }).fail(function () {
        console.log('Did not work');
    })
});