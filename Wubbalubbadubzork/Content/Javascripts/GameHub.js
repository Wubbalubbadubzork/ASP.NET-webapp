$(document).ready(function () {
    var gameHub = $.connection.gameHub;

    var dice = {
        sides: 6,
        roll: function () {
            var randomNumber = Math.floor(Math.random() * this.sides) + 1;
            return randomNumber;
        }
    }

    gameHub.client.showMessage = function (username, message) {
        $('#liveChat').append('<li>' + username + ': ' + message + '</li>');
    }
    gameHub.client.rollDice = function (number) {
        $('#dice').html(number);
    }
    gameHub.client.printScene = function (username, message) {
        $('#mainServer').append('<li>' + username + ': ' + message + '</li>');
    }
    gameHub.client.nextScene = function () {
        $('#sceneId').html('' + parseInt($('#sceneId').html(), 10) + 1);
        $.post("/Server/LoadScene", {
            scene_id: $("#sceneId").html().trim()
        }).done(function (response) {
            gameHub.server.sceneDisplay(response, $("#gameId").html().trim());
        })
    }

    $.connection.hub.start().done(function () {
        console.log('Now connected');
        gameHub.server.establishCharacters($("#gameId").html().trim());
        gameHub.server.joinGame($("#gameId").html().trim(), $("#userId").html().trim());

        $.post("/Server/LoadScene", {
            scene_id: $("#sceneId").html().trim()
        }).done(function (response) {
            gameHub.server.sceneDisplay(response, $("#gameId").html().trim());
        })

        $("#send").click(function () {
            if ($("#message").val() != '') {
                gameHub.server.sendMessage($('#username').html().trim(), $("#message").val(), $("#gameId").html().trim());
                $("#message").val('');
            }
        })()

        $("#button_dice").click(function () {
            var result = dice.roll();
            gameHub.server.printDice(result, $("#gameId").html().trim());
        })
    }).fail(function () {
        console.log('Did not work');
    })
})