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
        gameHub.server.establishOptions($("#gameId").html().trim(), parseInt($("#characterTurnId").html().trim(), 10), parseInt($("#sceneId").html().trim(), 10));
    }

    gameHub.client.setSkills = function (id, i, name, targetid, type) {
        for (var j = 1; j < i; j++) {
            $("#skill" + i).html(name);
            $("#skill" + i).prop('disabled', false);
            $("#skill" + i).click(function () {
                gameHub.server.executeSkill($("#gameId").html().trim(), parseInt($("#sceneId").html().trim(), 10), parseInt($("#characterTurnId").html().trim(), 10), id, name, targetid, type, parseInt($("#dice").html().trim()));
            })
        }
    }

    gameHub.client.setOption = function (i, name, tag) {
        $("#option" + i).html(name);
        if (name == "Si mismo") {
            $("#option" + i).addClass('selfOption');
            $("#option" + i).click(function () {
                gameHub.server.establishSkills($("#gameId").html().trim(), parseInt($("#characterTurnId").html().trim(), 10), name,'selfOption');
            })
        }
        else if (name == "Todos enemigos") {
            $("#option" + i).addClass('enemiesOption');
            $("#option" + i).click(function () {
                gameHub.server.establishSkills($("#gameId").html().trim(), parseInt($("#characterTurnId").html().trim(), 10), name,'enemiesOption');
            })
        }
        else if (name == "Todos aliados") {
            $("#option" + i).addClass('alliesOption');
            $("#option" + i).click(function () {
                gameHub.server.establishSkills($("#gameId").html().trim(), parseInt($("#characterTurnId").html().trim(), 10), name,'alliesOption');
            })
        }
        else {
            if (name == "Darius" || name == "Veigar" || name == "Ashe") {
                $("#option" + i).addClass(tag + "ally");
                $("#option" + i).click(function () {
                    gameHub.server.establishSkills($("#gameId").html().trim(), parseInt($("#characterTurnId").html().trim(), 10), name, tag + "ally");
                })
            }
            else {
                $("#option" + i).addClass(tag);
                $("#option" + i).click(function () {
                    gameHub.server.establishSkills($("#gameId").html().trim(), parseInt($("#characterTurnId").html().trim(), 10), name, tag);
                })
            }
        }
        $("#option" + i).prop('disabled', false);       
    }

    gameHub.client.whoIsNext = function () {
        gameHub.server.NextTurn($("#gameId").html().trim(), parseInt($("#sceneId").html().trim(), 10), parseInt($("#characterTurnId").html().trim(), 10));
    }

    gameHub.client.runTurn = function () {
        gameHub.server.Turn($("#gameId").html().trim(), parseInt($("#characterTurnId").html().trim(), 10));
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

    gameHub.client.isTurn = function (id, playable) {
        $("#characterTurnId").html(id);
        $("#playableOrNot").html(playable);
        gameHub.server.turn($("#gameId").html().trim(), parseInt($("#characterTurnId").html().trim(), 10));
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
        gameHub.server.sceneDisplay("Your turn. Please roll the dice", $("#gameId").html().trim());
    }

    gameHub.client.playerTurn = function () {
        gameHub.server.playerTurn($("#gameId").html().trim(), parseInt($("#characterTurnId").html().trim(), 10));
    }

    gameHub.client.enemyTurn = function () {
        gameHub.server.enemyTurn($("#gameId").html().trim(), parseInt($("#characterTurnId").html().trim(), 10));
    }

    $.connection.hub.start().done(function () {
        console.log('Now connected');
        gameHub.server.establishCharacters($("#gameId").html().trim());
        gameHub.server.joinGame($("#gameId").html().trim(), $("#userId").html().trim());

        $.post("/Server/LoadScene", {
            scene_id: $("#sceneId").html().trim()
        }).done(function (response) {
            gameHub.server.sceneDisplay(response, $("#gameId").html().trim());
        });

        gameHub.server.whoseTurn($("#gameId").html().trim());

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