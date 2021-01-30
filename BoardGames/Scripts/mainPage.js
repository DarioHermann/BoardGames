function gameButtonMouseOver(type) {
    $("#background-main-page").addClass(type.concat("Background"));
}

function gameButtonMouseLeave(type) {
    $("#background-main-page").removeClass(type.concat("Background"));
}
