$(function () {
    disableInput();
    $("#username").removeAttr("disabled");

    var playerId;

    var gameHub = $.connection.gameHubCheckers;


    //CLIENT METHODS
    gameHub.client.playerJoined = function (player) {
        playerId = player.Id;
        $("#usernameGroup").removeClass("was-validated");
        disableInput();
    };

    // The username is already taken
    gameHub.client.usernameTaken = function () {
        $("#status").html("The username is already taken.");
        $("#usernameGroup").addClass("was-validated");
    };

    // The opponent left so game is over and allow player to find a new game
    gameHub.client.opponentLeft = function () {
        $("#status").html("Opponent has left. Game over.");
        endGame();
    };

    // Notify Player that they are in a waiting pool for another opponent
    gameHub.client.waitingList = function () {
        $("#status").html("Waiting for an opponent.");
    };

    // Starts a new game by displaying the board and showing whose turn it is
    gameHub.client.start = function (game) {
        buildBoard(game.Board);
        var opponent = getOpponent(game);
        displayTurn(game.WhoseTurn, opponent);
    };

    // Handles the case where a user tried to place a piece not on their turn
    gameHub.client.notPlayersTurn = function () {
        $("#status").html("Please wait your turn.");
    };

    // Handles the case where the player tried an invalid move
    gameHub.client.notValidMove = function () {
        $("#status").html("Please choose another location.");
    };

    // Handles the case where the player tried to use and invalid piece
    gameHub.client.notValidPiece = function () {
        $("#status").html("Please choose another Piece, that piece is not yours.");
    };

    gameHub.client.selectPiece = function(row, col, validMoves) {
        $("#pos-" + row + "-" + col).addClass("checkersPieceSelected");

        
        for (var i = 0; i < validMoves.length; i++) {
            $("#pos-" + validMoves[i][0] + "-" + validMoves[i][1]).addClass("validMove");
        }
    }

    gameHub.client.deselectPiece = function (row, col, validMoves) {
        $("#pos-" + row + "-" + col).removeClass("checkersPieceSelected");

        for (var i = 0; i < validMoves.length; i++) {
            $("#pos-" + validMoves[i][0] + "-" + validMoves[i][1]).removeClass("validMove");
        }
    }

    gameHub.client.eatPiece = function(row, col, pieceEatenRow, pieceEatenCol, endRow, endCol, piece) {
        $("#pos-" + row + "-" + col).html("");
        $("#pos-" + pieceEatenRow + "-" + pieceEatenCol).html("");
        $("#pos-" + endRow + "-" + endCol).html(piece);
    }

    // A piece has been moved on the board
    gameHub.client.movePiece = function (row, col, endRow, endCol, piece) {
        $("#pos-" + row + "-" + col).html("");
        $("#pos-" + endRow + "-" + endCol).html(piece);
    };

    // updates the board
    gameHub.client.updateTurn = function (game) {
        var opponent = getOpponent(game);
        displayTurn(game.WhoseTurn, opponent);
    };

    // Handle the game in case of win
    gameHub.client.winner = function (playerName) {
        $("#status").html("Winner is " + playerName);
        endGame();
    };


    // CLIENT BEHAVIOURS
    // Call server to find a game if button is clicked
    $("#findGame").click(function () {
        var chosenUsername = $("#username").val();
        gameHub.server.findGame(chosenUsername);
    });

    // Pressing "Enter" will automatically click Find Game button
    $("#username").keypress(function (e) {
        if ((e.which && e.which == 13) || (e.keyCode && e.keyCode == 13)) {
            $("#findGame").click();
            return false;
        }
        return true;
    });


    function enableInput() {
        $("#username").removeAttr("disabled");
        $("#findGame").removeAttr("disabled");
        $("#username").focus();
    };

    function disableInput() {
        $("#username").attr("disabled", "disabled");
        $("#findGame").attr("disabled", "disabled");
    };

    // Game over business logic should disable board button handlers and allow player to join a new game
    function endGame() {
        // Removes click handlers from board positions
        $("td[id^=pos-]").off("click");
        enableInput();
    };

    // Display whose turn it is
    function displayTurn(playersTurn, opponent) {
        var turnMessage = "You are playing against " + opponent.Name + "<br />";
        if (playerId == playersTurn.Id) {
            turnMessage = turnMessage + "Your turn";
        } else {
            turnMessage = turnMessage + playersTurn.Name + "\'s turn";
        }

        $("#status").html(turnMessage);
    };

    // Build and display the board
    function buildBoard(board) {
        var template = Handlebars.compile($("#board-template").html());
        $("#board").html(template(board));

        $("td[id^=pos-]").click(function (e) {
            e.preventDefault();
            var id = this.id; // "pos-0-0"
            var parts = id.split("-"); // [pos, 0, 0]
            var row = parts[1];
            var col = parts[2];
            gameHub.server.selectPiece(row, col);
        });
    };

    // Retrieves the opponent player from the game
    function getOpponent(game) {
        if (playerId == game.Player1.Id) {
            return game.Player2;
        } else {
            return game.Player1;
        }
    };

    // Open a connection to the server hub
    $.connection.hub.logging = true;
    $.connection.hub.start().done(function () {
        enableInput();
    });
});