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

    $.connection.hub.start().done(function () {
        console.log('Now connected');
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