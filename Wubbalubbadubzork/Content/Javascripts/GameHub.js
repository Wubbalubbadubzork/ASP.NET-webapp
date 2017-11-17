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
        $('#button_dice').prop('disabled', true);
        gameHub.server.establishOptions($("#characterTurnConnection").html().trim(), $("#gameId").html().trim(), parseInt($("#characterTurnId").html().trim(), 10), parseInt($("#sceneId").html().trim(), 10));
    }

    gameHub.client.setSkills = function (id, i, name, targetid, type) {
        for (var j = i; j < 5; j++) {
            $("#skill" + i).html("Skill" + i);
            $("#skill" + i).prop('disabled', true);
            $("#skill" + i).click(null);
        }
        $("#skill" + i).html(name);
        $("#skill" + i).prop('disabled', false);
        $("#skill" + i).click(function () {
            gameHub.server.executeSkill($("#gameId").html().trim(), parseInt($("#sceneId").html().trim(), 10), parseInt($("#characterTurnId").html().trim(), 10), id, name, targetid, type, parseInt($("#dice").html().trim(), 10));
        })
    }

    gameHub.client.setOption = function (i, name, tag) {
        $("#option" + i).html(name);
        $("#option" + i).prop('disabled', false);
        if (name == "Si mismo") {
            $("#option" + i).click(function () {
                gameHub.server.establishSkills($("#gameId").html().trim(), parseInt($("#characterTurnId").html().trim(), 10), name,'selfOption');
            })
        }
        else if (name == "Todos enemigos") {
            $("#option" + i).click(function () {
                gameHub.server.establishSkills($("#gameId").html().trim(), parseInt($("#characterTurnId").html().trim(), 10), name,'enemiesOption');
            })
        }
        else if (name == "Todos aliados") {
            $("#option" + i).click(function () {
                gameHub.server.establishSkills($("#gameId").html().trim(), parseInt($("#characterTurnId").html().trim(), 10), name,'alliesOption');
            })
        }
        else {
            if (name == "Darius" || name == "Veigar" || name == "Ashe") {
                $("#option" + i).click(function () {
                    gameHub.server.establishSkills($("#gameId").html().trim(), parseInt($("#characterTurnId").html().trim(), 10), name, tag + "ally");
                })
            }
            else {
                $("#option" + i).click(function () {
                    gameHub.server.establishSkills($("#gameId").html().trim(), parseInt($("#characterTurnId").html().trim(), 10), name, tag);
                })
            }
        }       
    }

    gameHub.client.whoIsNext = function () {
        gameHub.server.NextTurn($("#gameId").html().trim(), parseInt($("#sceneId").html().trim(), 10), parseInt($("#characterTurnId").html().trim(), 10), $("#lastConnection").html().trim());
    }

    gameHub.client.printScene = function (username, message) {
        $('#mainServer').append('<li>' + username + ': ' + message + '</li>');
    }

    gameHub.client.finishGame = function () {
        $('#mainServer').append('<li>Narrador: Felicidades!! Derrotaron a Jimmy y terminaron esta partida. Reciban 100 puntos a su cuenta. Adiós!</li>');
    }

    gameHub.client.redirectIndex = function () {
        window.location = '/Home/Index';
    }

    gameHub.client.nextScene = function () {
        var scene_id = parseInt($("#sceneId").html().trim(), 10);
        scene_id++;
        $('#sceneId').html(scene_id + '');
        gameHub.server.sceneDisplay($("#lastConnection").html().trim(), parseInt($("#sceneId").html().trim(), 10), $("#gameId").html().trim());
        gameHub.server.whoseTurn($("#lastConnection").html().trim(), $("#gameId").html().trim());
    }

    gameHub.client.checkScene = function (connectionId ) {
        gameHub.server.enemiesLeft(connectionId, $("#gameId").html().trim(), parseInt($("#sceneId").html().trim(), 10));
    }

    gameHub.client.isTurn = function (connectionId, id, playable) {
        $("#characterTurnId").html(id);
        $("#playableOrNot").html(playable);
        if (playable == true) {
            $("#characterTurnConnection").html(connectionId);
        }
        else {
            $("#characterTurnConnection").html('');
        }        
        gameHub.server.turn($("#gameId").html().trim(), parseInt($("#characterTurnId").html().trim(), 10), $("#characterTurnConnection").html().trim());
    }

    gameHub.client.announceTurn = function () {
        alert("It is your turn boy. Please roll the dice.");
        $("#button_dice").prop('disabled', false);
        for (var i = 1; i <= 9; i++) {
            if (i <= 4) {
                $("#option" + i).prop('disabled', true);
                $("#skill" + i).prop('disabled', true);
            }
        }
    }

    gameHub.client.playerTurn = function () {
        gameHub.server.playerTurn($("#gameId").html().trim(), parseInt($("#characterTurnId").html().trim(), 10));
    }

    gameHub.client.enemyTurn = function () {
        gameHub.server.enemyTurn($("#gameId").html().trim(), parseInt($("#characterTurnId").html().trim(), 10));
    }

    gameHub.client.setLastConnection = function (connectionId) {
        $("#lastConnection").html(connectionId + "");
    }

    $.connection.hub.start().done(function () {
        console.log('Now connected');
        gameHub.server.establishCharacters($("#gameId").html().trim());
        gameHub.server.joinGame($("#gameId").html().trim());
        gameHub.server.setLast($("#gameId").html().trim());

        setTimeout(function () {
            gameHub.server.sceneDisplay($("#lastConnection").html().trim(), parseInt($("#sceneId").html().trim(), 10), $("#gameId").html().trim());
            gameHub.server.whoseTurn($("#lastConnection").html().trim(), $("#gameId").html().trim());
        }, 5000);

        $("#send").click(function () {
            if ($("#message").val() != '') {
                gameHub.server.sendMessage($('#username').html().trim(), $("#message").val(), $("#gameId").html().trim());
                $("#message").val('');
            }
        });

        $("#button_dice").click(function () {
            var result = dice.roll();
            gameHub.server.printDice(result, $("#gameId").html().trim());
        });
    }).fail(function () {
        console.log('Did not work');
    })
})